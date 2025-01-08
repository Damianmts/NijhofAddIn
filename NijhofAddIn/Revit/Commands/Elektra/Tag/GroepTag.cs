using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NijhofAddIn.Revit.Core.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;

namespace NijhofAddIn.Revit.Commands.Elektra.Tag
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class GroepTag : IExternalCommand, IDisposable
    {
        private readonly Dictionary<BuiltInCategory, string> _categories;
        private ProgressWindowWPF _progressWindow;
        private ExternalEvent _externalEvent;
        private TagEventHandler _handler;
        private readonly ILogger _logger;

        public GroepTag() : this(null) { }

        public GroepTag(ILogger logger = null)
        {
            _logger = logger ?? new DefaultLogger();
            _categories = new Dictionary<BuiltInCategory, string>
            {
                { BuiltInCategory.OST_FireAlarmDevices, "Brandmelders" },
                { BuiltInCategory.OST_LightingDevices, "Schakelaars" },
                { BuiltInCategory.OST_LightingFixtures, "Verlichting" },
                { BuiltInCategory.OST_ElectricalFixtures, "Elektra" }
            };
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                InitializeComponents();
                ProcessCategories(commandData.Application.ActiveUIDocument.Document);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = $"Er is een fout opgetreden: {ex.Message}";
                _logger.LogError(ex);
                return Result.Failed;
            }
        }

        private void InitializeComponents()
        {
            _progressWindow = new ProgressWindowWPF();
            _handler = new TagEventHandler();
            _externalEvent = ExternalEvent.Create(_handler);
            _progressWindow.Show();
        }

        private void ProcessCategories(Document doc)
        {
            Task.Run(async () =>
            {
                try
                {
                    foreach (var category in _categories)
                    {
                        await ProcessCategory(doc, category);
                    }
                }
                finally
                {
                    _progressWindow.Dispatcher.Invoke(() => _progressWindow.Close());
                }
            });
        }

        private async Task ProcessCategory(Document doc, KeyValuePair<BuiltInCategory, string> category)
        {
            UpdateProgressWindowStatus(category.Value);

            var elements = GetFilteredElements(doc, category.Key);
            if (!elements.Any()) return;

            var batchSize = CalculateBatchSize(elements.Count);
            var batches = CreateBatches(elements, batchSize);

            await ProcessBatches(doc, batches, elements.Count);
        }

        private void UpdateProgressWindowStatus(string categoryName)
        {
            _progressWindow.Dispatcher.Invoke(() =>
            {
                _progressWindow.UpdateStatusText(categoryName);
                _progressWindow.ResetProgress();
            });
        }

        private IList<Element> GetFilteredElements(Document doc, BuiltInCategory category)
        {
            // Haal eerst alle elementen op van de categorie met een Groep parameter
            var elementsWithGroup = new FilteredElementCollector(doc)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .Where(el =>
                {
                    var groepParam = el.LookupParameter("Groep");
                    return groepParam != null && !string.IsNullOrWhiteSpace(groepParam.AsString());
                })
                .ToList(); // Zorg ervoor dat dit een List<Element> is

            // Haal alle bestaande tags op voor deze view die de 'Groep' parameter tonen
            var existingGroupTags = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .OfClass(typeof(IndependentTag))
                .Cast<IndependentTag>()
                .Where(tag =>
                {
                    // Controleer of de tag de 'Groep' parameter weergeeft
                    Parameter parameterToCheck = tag.LookupParameter("Parameter to display");
                    return parameterToCheck != null && parameterToCheck.AsString() == "Groep";
                })
                .ToList(); // Zorg ervoor dat dit een List<IndependentTag> is

            // Filter elementen die nog geen Groep tag hebben
            var untaggedElements = elementsWithGroup
                .Where(element => !existingGroupTags.Any(tag => tag.GetTaggedLocalElementIds().Contains(element.Id)))
                .ToList(); // Dit zet het resultaat om in een List<Element>

            return untaggedElements;
        }

        private int CalculateBatchSize(int totalElements) =>
            totalElements switch
            {
                <= 100 => 5,
                <= 250 => 10,
                <= 750 => 50,
                _ => 100
            };

        private IEnumerable<List<Element>> CreateBatches(IList<Element> elements, int batchSize)
        {
            for (int i = 0; i < elements.Count; i += batchSize)
            {
                yield return elements.Skip(i).Take(batchSize).ToList();
            }
        }

        private async Task ProcessBatches(Document doc, IEnumerable<List<Element>> batches, int totalElements)
        {
            int processedElements = 0;
            int updateFrequency = Math.Max(1, totalElements / 50);

            foreach (var batch in batches)
            {
                await ExecuteBatchAsync(doc, batch);
                processedElements += batch.Count;

                if (processedElements % updateFrequency == 0 || processedElements == totalElements)
                {
                    UpdateProgress(processedElements, totalElements);
                }
            }
        }

        private void UpdateProgress(int processedElements, int totalElements)
        {
            _progressWindow.Dispatcher.Invoke(() =>
            {
                int progress = (processedElements * 100) / totalElements;
                _progressWindow.UpdateProgress(progress);
            });
        }

        private async Task ExecuteBatchAsync(Document doc, List<Element> batch)
        {
            var tcs = new TaskCompletionSource<bool>();

            _handler.Operation = _ =>
            {
                using (Transaction tx = new Transaction(doc, "Tag Elements"))
                {
                    tx.Start();
                    try
                    {
                        foreach (var element in batch)
                        {
                            try
                            {
                                CreateTagForElement(doc, element);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Fout bij het taggen van element {element.Id}: {ex.Message}");
                            }
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Transactie fout: {ex.Message}");
                        tx.RollBack();
                        throw;
                    }
                }
                tcs.SetResult(true);
            };

            _externalEvent.Raise();
            await tcs.Task;
        }

        private void CreateTagForElement(Document doc, Element element)
        {
            LocationPoint locationPoint = element.Location as LocationPoint;
            if (locationPoint == null)
            {
                throw new InvalidOperationException($"Element {element.Id} heeft geen LocationPoint");
            }

            var tag = IndependentTag.Create(
                doc,
                doc.ActiveView.Id,
                new Reference(element),
                false,
                TagMode.TM_ADDBY_MULTICATEGORY,
                TagOrientation.Horizontal,
                locationPoint.Point
            );

            // Zorg ervoor dat de nieuwe tag de Groep parameter weergeeft
            Parameter parameterToDisplay = tag.LookupParameter("Parameter to display");
            if (parameterToDisplay != null)
            {
                parameterToDisplay.Set("Groep");
            }
        }

        public void Dispose()
        {
            _externalEvent?.Dispose();
            _progressWindow?.Close();
        }
    }

    public interface ILogger
    {
        void LogError(string message);
        void LogError(Exception ex);
    }

    public class DefaultLogger : ILogger
    {
        public void LogError(string message)
        {
            TaskDialog.Show("Error", message);
        }

        public void LogError(Exception ex)
        {
            LogError(ex.Message);
        }
    }
}