using Autodesk.Revit.UI;
using NijhofAddIn.Resources;
using NijhofAddIn.Revit.Core;
using ricaun.Revit.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Media;

namespace NijhofAddIn.Revit

{
    [AppLoader]
    public class App : IExternalApplication
    {
        const string RIBBON_TAB = "Nijhof Tools";
        const string RIBBON_PANEL1 = "Prefab";
        const string RIBBON_PANEL2 = "GPS Punten";
        const string RIBBON_PANEL3 = "Sparingen";
        const string RIBBON_PANEL4 = "Wijzigen";
        const string RIBBON_PANEL5 = "Export";
        const string RIBBON_PANEL6 = "Elektra";
        const string RIBBON_PANEL7 = "Overig";
        const string RIBBON_PANEL8 = "Content";
        const string RIBBON_PANEL9 = "Berekenen";

        public Result OnStartup(UIControlledApplication app)
        {
            #region Tabblad
            /// Maakt Ribbon Tabblad aan
            try
            {
                app.CreateRibbonTab(RIBBON_TAB);
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                //tab already exists
            }
            #endregion

            #region Panel (Content)
            /// Maakt Panel (Content) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel8 = null;
            List<RibbonPanel> panels8 = app.GetRibbonPanels(RIBBON_TAB);
            foreach (RibbonPanel pnl in panels8)
            {
                if (pnl.Name == RIBBON_PANEL8)
                {
                    panel8 = pnl;
                    break;
                }
            }
            /// Bestaat panel8 niet? Maak panel1 aan
            if (panel8 == null)
            {
                panel8 = app.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL8);
            }
            #endregion

            #region Panel (Prefab)
            /// Maakt Panel (Prefab) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel1 = null;
            List<RibbonPanel> panels1 = app.GetRibbonPanels(RIBBON_TAB);
            foreach (RibbonPanel pnl in panels1)
            {
                if (pnl.Name == RIBBON_PANEL1)
                {
                    panel1 = pnl;
                    break;
                }
            }
            /// Bestaat panel1 niet? Maak panel1 aan
            if (panel1 == null)
            {
                panel1 = app.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL1);
            }
            #endregion

            #region Panel (Berekenen)
            /// Maakt Panel (Berekenen) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel9 = null;
            List<RibbonPanel> panels9 = app.GetRibbonPanels(RIBBON_TAB);
            foreach (RibbonPanel pnl in panels9)
            {
                if (pnl.Name == RIBBON_PANEL9)
                {
                    panel9 = pnl;
                    break;
                }
            }
            /// Bestaat panel9 niet? Maak panel9 aan
            if (panel9 == null)
            {
                panel9 = app.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL9);
            }
            #endregion

            #region Panel (GPS Punten)
            /// Maakt Panel (GPS Punten) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel2 = null;
            List<RibbonPanel> panels2 = app.GetRibbonPanels(RIBBON_TAB);
            foreach (RibbonPanel pnl in panels2)
            {
                if (pnl.Name == RIBBON_PANEL2)
                {
                    panel2 = pnl;
                    break;
                }
            }
            /// Bestaat panel2 niet? Maak panel2 aan
            if (panel2 == null)
            {
                panel2 = app.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL2);
            }
            #endregion

            #region Panel (Sparingen)
            /// Maakt Panel (Sparingen) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel3 = null;
            List<RibbonPanel> panels3 = app.GetRibbonPanels(RIBBON_TAB);
            foreach (RibbonPanel pnl in panels3)
            {
                if (pnl.Name == RIBBON_PANEL3)
                {
                    panel3 = pnl;
                    break;
                }
            }
            /// Bestaat panel3 niet? Maak panel3 aan
            if (panel3 == null)
            {
                panel3 = app.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL3);
            }
            #endregion

            #region Panel (Wijzigen)
            /// Maakt Panel (Wijzigen) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel4 = null;
            List<RibbonPanel> panels4 = app.GetRibbonPanels(RIBBON_TAB);
            foreach (RibbonPanel pnl in panels4)
            {
                if (pnl.Name == RIBBON_PANEL4)
                {
                    panel4 = pnl;
                    break;
                }
            }
            /// Bestaat panel4 niet? Maak panel4 aan
            if (panel4 == null)
            {
                panel4 = app.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL4);
            }
            #endregion

            #region Panel (Export)
            /// Maakt Panel (Export) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel5 = null;
            List<RibbonPanel> panels5 = app.GetRibbonPanels(RIBBON_TAB);
            foreach (RibbonPanel pnl in panels5)
            {
                if (pnl.Name == RIBBON_PANEL5)
                {
                    panel5 = pnl;
                    break;
                }
            }
            /// Bestaat panel5 niet? Maak panel4 aan
            if (panel5 == null)
            {
                panel5 = app.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL5);
            }
            #endregion

            #region Panel (Elektra)
            /// Maakt Panel (Elektra) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel6 = null;
            List<RibbonPanel> panels6 = app.GetRibbonPanels(RIBBON_TAB);
            foreach (RibbonPanel pnl in panels6)
            {
                if (pnl.Name == RIBBON_PANEL6)
                {
                    panel6 = pnl;
                    break;
                }
            }
            /// Bestaat panel6 niet? Maak panel4 aan
            if (panel6 == null)
            {
                panel6 = app.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL6);
            }
            #endregion

            #region Panel (Overig)
            /// Maakt Panel (Overig) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel7 = null;
            List<RibbonPanel> panels7 = app.GetRibbonPanels(RIBBON_TAB);
            foreach (RibbonPanel pnl in panels7)
            {
                if (pnl.Name == RIBBON_PANEL7)
                {
                    panel7 = pnl;
                    break;
                }
            }
            /// Bestaat panel7 niet? Maak panel4 aan
            if (panel7 == null)
            {
                panel7 = app.CreateRibbonPanel(RIBBON_TAB, RIBBON_PANEL7);
            }
            #endregion

            #region Icons
            AppIcons appBase = new AppIcons();

            Image PlaceHolder16 = Icons.error_16;
            ImageSource PlaceHolder16Scr = appBase.GetImageSource(PlaceHolder16);

            Image PlaceHolder32 = Icons.error_32;
            ImageSource PlaceHolder32Scr = appBase.GetImageSource(PlaceHolder32);

            Image Library16 = Icons.library_16;
            ImageSource Library16Scr = appBase.GetImageSource(Library16);

            Image Library32 = Icons.library_32;
            ImageSource Library32Scr = appBase.GetImageSource(Library32);

            Image Tag16 = Icons.tag_16;
            ImageSource Tag16Scr = appBase.GetImageSource(Tag16);

            Image Tag32 = Icons.tag_32;
            ImageSource Tag32Scr = appBase.GetImageSource(Tag32);

            Image _3D16 = Icons._3d_16;
            ImageSource _3D16Scr = appBase.GetImageSource(_3D16);

            Image _3D32 = Icons._3d_32;
            ImageSource _3D32Scr = appBase.GetImageSource(_3D32);

            Image Delete16 = Icons.delete_16;
            ImageSource Delete16Scr = appBase.GetImageSource(Delete16);

            Image Delete32 = Icons.delete_32;
            ImageSource Delete32Scr = appBase.GetImageSource(Delete32);

            Image GPS16 = Icons.gps_16;
            ImageSource GPS16Scr = appBase.GetImageSource(GPS16);

            Image GPS32 = Icons.gps_32;
            ImageSource GPS32Scr = appBase.GetImageSource(GPS32);

            Image GPSimport16 = Icons.gpsimport_16;
            ImageSource GPSimport16Scr = appBase.GetImageSource(GPSimport16);

            Image GPSimport32 = Icons.gpsimport_32;
            ImageSource GPSimport32Scr = appBase.GetImageSource(GPSimport32);

            Image GPSdel16 = Icons.gpsdel_16;
            ImageSource GPSdel16Scr = appBase.GetImageSource(GPSdel16);

            Image GPSdel32 = Icons.gpsdel_32;
            ImageSource GPSdel32Scr = appBase.GetImageSource(GPSdel32);

            Image MuurSparing16 = Icons.wall_16;
            ImageSource MuurSparing16Scr = appBase.GetImageSource(MuurSparing16);

            Image MuurSparing32 = Icons.wall_32;
            ImageSource MuurSparing32Scr = appBase.GetImageSource(MuurSparing32);

            Image BalkSparing16 = Icons.concrete_16;
            ImageSource BalkSparing16Scr = appBase.GetImageSource(BalkSparing16);

            Image BalkSparing32 = Icons.concrete_32;
            ImageSource BalkSparing32Scr = appBase.GetImageSource(BalkSparing32);

            Image Ontstop16 = Icons.manhole_16;
            ImageSource Ontstop16Scr = appBase.GetImageSource(Ontstop16);

            Image Ontstop32 = Icons.manhole_32;
            ImageSource Ontstop32Scr = appBase.GetImageSource(Ontstop32);

            Image UpdateArtikel16 = Icons.article_16;
            ImageSource UpdateArtikel16Scr = appBase.GetImageSource(UpdateArtikel16);

            Image UpdateArtikel32 = Icons.article_32;
            ImageSource UpdateArtikel32Scr = appBase.GetImageSource(UpdateArtikel32);

            Image Regen16 = Icons.rain16;
            ImageSource Regen16Scr = appBase.GetImageSource(Regen16);

            Image Regen32 = Icons.rain32;
            ImageSource Regen32Scr = appBase.GetImageSource(Regen32);

            Image Lengte16 = Icons.length16;
            ImageSource Lengte16Scr = appBase.GetImageSource(Lengte16);

            Image Lengte32 = Icons.length32;
            ImageSource Lengte32Scr = appBase.GetImageSource(Lengte32);

            //Image Export16 = Properties.Resources.export_excel_16;
            //ImageSource Export16Scr = appBase.GetImageSource(Export16);

            //Image Export32 = Properties.Resources.export_excel_32;
            //ImageSource Export32Scr = appBase.GetImageSource(Export32);

            Image Materiaal16 = Icons.basket16;
            ImageSource Materiaal16Scr = appBase.GetImageSource(Materiaal16);

            Image Materiaal32 = Icons.basket32;
            ImageSource Materiaal32Scr = appBase.GetImageSource(Materiaal32);

            Image Zaag16 = Icons.saw16;
            ImageSource Zaag16Scr = appBase.GetImageSource(Zaag16);

            Image Zaag32 = Icons.saw32;
            ImageSource Zaag32Scr = appBase.GetImageSource(Zaag32);

            //Image Reset16 = Icons.reset_16;
            //ImageSource Reset16Scr = appBase.GetImageSource(Reset16);

            //Image Reset32 = Icons.reset_32;
            //ImageSource Reset32Scr = appBase.GetImageSource(Reset32);

            //Image Parameter16 = Icons.properties_16;
            //ImageSource Parameter16Scr = appBase.GetImageSource(Parameter16);

            //Image Parameter32 = Icons.properties_32;
            //ImageSource Parameter32Scr = appBase.GetImageSource(Parameter32);

            Image SwitchCode16 = Icons.list_16;
            ImageSource SwitchCode16Scr = appBase.GetImageSource(SwitchCode16);

            Image SwitchCode32 = Icons.list_32;
            ImageSource SwitchCode32Scr = appBase.GetImageSource(SwitchCode32);

            //Image GroepTag16 = Icons.electrical_16;
            //ImageSource GroepTag16Scr = appBase.GetImageSource(GroepTag16);

            Image GroepTag32 = Icons.electrical_32;
            ImageSource GroepTag32Scr = appBase.GetImageSource(GroepTag32);

            //Image SwitchTag16 = Icons.switch_16;
            //ImageSource SwitchTag16Scr = appBase.GetImageSource(SwitchTag16);

            Image SwitchTag32 = Icons.switch_32;
            ImageSource SwitchTag32Scr = appBase.GetImageSource(SwitchTag32);

            Image Gift16 = Icons.gift16;
            ImageSource Gift16Scr = appBase.GetImageSource(Gift16);

            Image Gift32 = Icons.gift32;
            ImageSource Gift32Scr = appBase.GetImageSource(Gift32);

            Image Vraag16 = Icons.question_16;
            ImageSource Vraag16Scr = appBase.GetImageSource(Vraag16);

            Image Vraag32 = Icons.question_32;
            ImageSource Vraag32Scr = appBase.GetImageSource(Vraag32);

            Image Info16 = Icons.info_16;
            ImageSource Info16Scr = appBase.GetImageSource(Info16);

            Image Info32 = Icons.info_32;
            ImageSource Info32Scr = appBase.GetImageSource(Info32);



            /// Kopieer en pas aan om nieuwe afbeelding toe te voegen, voeg hierboven toe
            ///Image HWA_Article_Img = Properties.Resources.HWA_articlenr;
            ///ImageSource HWA_Article_ImgScr = appBase.GetImageSource(HWA_Article_Img);

            #endregion

            #region Buttons Panel (Content)
            #region pushButton (Library)
            /// Knopgegevens instellen /// Herhaalbaar voor elke extra knop die je wilt
            PushButtonData btndataLibrary = new PushButtonData
                (
                "Library",
                "Library",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Core.Foutmelding" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Opent de 'Nijhof Bibliotheek'",
                LongDescription = "Een bibliotheek waar alle vaak gebruikte modellen, tags en groups in gezet kunnen worden zodat die makkelijk te vinden- en in te laden zijn",
                Image = Library16Scr,
                LargeImage = Library32Scr,
            };

            /// Voeg de knop toe aan het Ribbon Panel
            PushButton buttonLibrary = (PushButton)panel8.AddItem(btndataLibrary);
            buttonLibrary.Enabled = false;
            ContextualHelp contextHelpLibrary = new ContextualHelp(ContextualHelpType.Url,
                    "http://www.autodesk.com");
            buttonLibrary.SetContextualHelp(contextHelpLibrary);

            //panel8.AddSeparator(); //Voegt een verticale lijn toe
            #endregion
            #endregion

            #region Buttons Panel (Prefab)
            #region pushButton (Instant 3D Creator)
            /// Knopgegevens instellen /// Herhaalbaar voor elke extra knop die je wilt
            PushButtonData Creator3D = new PushButtonData
                (
                "Prefab 3D Creator",
                "Prefab\n3D creator",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.Prefab3DCreator" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Maakt van een viewport in een sheet een 3D view van het rioleringstelsel en zet deze op sheet",
                LongDescription = "Bij het maken van bijvoorbeeld Prefab hoef je alleen op de viewport te klikken en er wordt een 3D view" +
                "gemaakt met de view template: \\\"05_Plot_Prefab_Riool+Lucht_3D\\\". De 3D wordt gelijk op de sheet geplaatst. De sub-diciplines van de template is de" +
                "plek waar deze te vinden is in de project brouwser.",
                Image = _3D16Scr,
                LargeImage = _3D32Scr,
            };

            /// Voeg de knop toe aan het Ribbon Panel
            PushButton buttonC3D = (PushButton)panel1.AddItem(Creator3D);
            buttonC3D.Enabled = true;
            ContextualHelp contextHelpC3D = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#prefab-3d-creator");
            buttonC3D.SetContextualHelp(contextHelpC3D);

            panel1.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region pushButtondata (VWA Prefab Taggen)
            /// Knopgegevens instellen voor de eerste knop vwa
            PushButtonData btndataVWAtag25 = new PushButtonData(
                "VWA 2.5mm", /// De naam van de standaardactie
                "VWA Tag 2.5mm", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag25" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de tweede knop vwa
            PushButtonData btndataVWAtag35 = new PushButtonData(
                "VWA 3.5mm",
                "VWA Tag 3.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag35" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de derde knop vwa
            PushButtonData btndataVWAtag50 = new PushButtonData(
                "VWA 5.0mm",
                "VWA Tag 5.0mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag50" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de vierde knop vwa
            PushButtonData btndataVWAtag75 = new PushButtonData(
                "VWA 7.5mm",
                "VWA Tag 7.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag75" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de vijfde knop vwa
            PushButtonData btndataVWAtag100 = new PushButtonData(
                "VWA 10mm",
                "VWA Tag 10mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag100" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            #endregion

            #region pushButtondata (HWA Prefab Taggen)
            /// Knopgegevens instellen voor de eerste knop hwa
            PushButtonData btndataHWAtag25 = new PushButtonData(
                "HWA 2.5mm", /// De naam van de standaardactie
                "HWA Tag 2.5mm", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag25" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de tweede knop hwa
            PushButtonData btndataHWAtag35 = new PushButtonData(
                "HWA 3.5mm",
                "HWA Tag 3.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag35" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de derde knop hwa
            PushButtonData btndataHWAtag50 = new PushButtonData(
                "HWA 5.0mm",
                "HWA Tag 5.0mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag50" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de vierde knop hwa
            PushButtonData btndataHWAtag75 = new PushButtonData(
                "HWA 7.5mm",
                "HWA Tag 7.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag75" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de vijfde knop hwa
            PushButtonData btndataHWAtag100 = new PushButtonData(
                "HWA 10mm",
                "HWA Tag 10mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.VWAtag100" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            #endregion

            #region pushButtondata (Lucht Prefab Taggen)
            /// Knopgegevens instellen voor de eerste knop mv
            PushButtonData btndataMVtag25 = new PushButtonData(
                "MV 2.5mm", /// De naam van de standaardactie
                "MV Tag 2.5mm", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.MVtag25" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de tweede knop mv
            PushButtonData btndataMVtag35 = new PushButtonData(
                "MV 3.5mm",
                "MV Tag 3.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.MVtag35" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de derde knop mv
            PushButtonData btndataMVtag50 = new PushButtonData(
                "MV 5.0mm",
                "MV Tag 5.0mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.MVtag50" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de vierde knop mv
            PushButtonData btndataMVtag75 = new PushButtonData(
                "MV 7.5mm",
                "MV Tag 7.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.MVtag75" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            /// Knopgegevens instellen voor de vijfde knop mv
            PushButtonData btndataMVtag100 = new PushButtonData(
                "MV 10mm",
                "MV Tag 10mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Prefab.MVtag100" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = Tag16Scr,
                LargeImage = Tag32Scr,
            };

            #endregion

            #region Stacked splitButton
            /// 2. Maak SplitButtonData
            SplitButtonData SB_A = new SplitButtonData("SplitGroupVWA", "Split Group VWA");
            SplitButtonData SB_B = new SplitButtonData("SplitGroupHWA", "Split Group HWA");
            SplitButtonData SB_C = new SplitButtonData("SplitgroupMV", "Split Group MV");

            /// 3. Maak een "Stacked" indeling
            IList<RibbonItem> stackedItems = panel1.AddStackedItems(SB_A, SB_B, SB_C);

            /// 4. Maakt de knoppen "SplitButtons"
            SplitButton SBvwa_Button = stackedItems[0] as SplitButton;
            SplitButton SBhwa_Button = stackedItems[1] as SplitButton;
            SplitButton SBmv_Button = stackedItems[2] as SplitButton;
            #endregion

            #region pushButton VWA stack
            /// voeg de eerste knop toe aan de vwa stack
            PushButton buttonVWAtag25 = SBvwa_Button.AddPushButton(btndataVWAtag25);
            buttonVWAtag25.Enabled = true;
            ContextualHelp contexthelpvwatag25 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#vwa-tag");
            buttonVWAtag25.SetContextualHelp(contexthelpvwatag25);

            /// Voeg de tweede knop toe aan de vwa stack
            PushButton buttonVWAtag35 = SBvwa_Button.AddPushButton(btndataVWAtag35);
            buttonVWAtag35.Enabled = true;
            ContextualHelp contextHelpVWAtag35 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#vwa-tag");
            buttonVWAtag35.SetContextualHelp(contextHelpVWAtag35);

            /// Voeg de derde knop toe aan de vwa stack
            PushButton buttonVWAtag50 = SBvwa_Button.AddPushButton(btndataVWAtag50);
            buttonVWAtag50.Enabled = true;
            ContextualHelp contextHelpVWAtag50 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#vwa-tag");
            buttonVWAtag50.SetContextualHelp(contextHelpVWAtag50);

            /// Voeg de vierde knop toe aan de vwa stack
            PushButton buttonVWAtag75 = SBvwa_Button.AddPushButton(btndataVWAtag75);
            buttonVWAtag75.Enabled = true;
            ContextualHelp contextHelpVWAtag75 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#vwa-tag");
            buttonVWAtag75.SetContextualHelp(contextHelpVWAtag75);

            /// Voeg de vijfde knop toe aan de vwa stack
            PushButton buttonVWAtag100 = SBvwa_Button.AddPushButton(btndataVWAtag100);
            buttonVWAtag100.Enabled = true;
            ContextualHelp contextHelpVWAtag100 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#vwa-tag");
            buttonVWAtag100.SetContextualHelp(contextHelpVWAtag100);
            #endregion

            #region pushButton HWA stack
            /// Voeg de eerste knop toe aan hwa stack
            PushButton buttonHWAtag25 = SBhwa_Button.AddPushButton(btndataHWAtag25);
            buttonHWAtag25.Enabled = true;
            ContextualHelp contextHelpHWAtag25 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#hwa-tag");
            buttonHWAtag25.SetContextualHelp(contextHelpHWAtag25);

            /// Voeg de tweede knop toe aan hwa stack
            PushButton buttonHWAtag35 = SBhwa_Button.AddPushButton(btndataHWAtag35);
            buttonHWAtag35.Enabled = true;
            ContextualHelp contextHelpHWAtag35 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#hwa-tag");
            buttonHWAtag35.SetContextualHelp(contextHelpHWAtag35);

            /// Voeg de derde knop toe aan hwa stack
            PushButton buttonHWAtag50 = SBhwa_Button.AddPushButton(btndataHWAtag50);
            buttonHWAtag50.Enabled = true;
            ContextualHelp contextHelpHWAtag50 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#hwa-tag");
            buttonHWAtag50.SetContextualHelp(contextHelpHWAtag50);

            /// Voeg de vierde knop toe aan hwa stack
            PushButton buttonHWAtag75 = SBhwa_Button.AddPushButton(btndataHWAtag75);
            buttonHWAtag75.Enabled = true;
            ContextualHelp contextHelpHWAtag75 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#hwa-tag");
            buttonHWAtag75.SetContextualHelp(contextHelpHWAtag75);

            /// Voeg de vijfde knop toe aan hwa stack
            PushButton buttonHWAtag100 = SBhwa_Button.AddPushButton(btndataHWAtag100);
            buttonHWAtag100.Enabled = true;
            ContextualHelp contextHelpHWAtag100 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#hwa-tag");
            buttonHWAtag100.SetContextualHelp(contextHelpHWAtag100);
            #endregion

            #region pushButton MV stack
            /// Voeg de eerst knop toe aan mv stack
            PushButton buttonMVtag25 = SBmv_Button.AddPushButton(btndataMVtag25);
            buttonMVtag25.Enabled = true;
            ContextualHelp contextHelpMVtag25 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#mv-tag");
            buttonMVtag25.SetContextualHelp(contextHelpMVtag25);

            /// Voeg de tweede knop toe aan mv stack
            PushButton buttonMVtag35 = SBmv_Button.AddPushButton(btndataMVtag35);
            buttonMVtag35.Enabled = true;
            ContextualHelp contextHelpMVtag35 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#mv-tag");
            buttonMVtag35.SetContextualHelp(contextHelpMVtag35);

            /// Voeg de derde knop toe aan mv stack
            PushButton buttonMVtag50 = SBmv_Button.AddPushButton(btndataMVtag50);
            buttonMVtag50.Enabled = true;
            ContextualHelp contextHelpMVtag50 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#mv-tag");
            buttonMVtag50.SetContextualHelp(contextHelpMVtag50);

            /// Voeg de vierde knop toe aan mv stack
            PushButton buttonMVtag75 = SBmv_Button.AddPushButton(btndataMVtag75);
            buttonMVtag75.Enabled = true;
            ContextualHelp contextHelpMVtag75 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#mv-tag");
            buttonMVtag75.SetContextualHelp(contextHelpMVtag75);

            /// Voeg de vijfde knop toe aan mv stack
            PushButton buttonMVtag100 = SBmv_Button.AddPushButton(btndataMVtag100);
            buttonMVtag100.Enabled = true;
            ContextualHelp contextHelpMVtag100 = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#mv-tag");
            buttonMVtag100.SetContextualHelp(contextHelpMVtag100);
            #endregion
            #endregion

            #region Buttons Panel (Berekenen)
            #region pushButton (Rekenpunten Verwijderen)
            /// Knopgegevens instellen /// Herhaalbaar voor elke extra knop die je wilt
            PushButtonData btndataRV = new PushButtonData
                (
                "Rekenpunten verwijderen",
                "Rekenpunten\nverwijderen",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Core.Foutmelding" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verwijdert Riool Rekenpunten",
                LongDescription = "Zoekt naar de 'Stabicad' rekenpunten en verwijdert die.",
                Image = Delete16Scr,
                LargeImage = Delete32Scr,
            };

            /// Voeg de knop toe aan het Ribbon Panel
            PushButton buttonRV = (PushButton)panel9.AddItem(btndataRV);
            buttonRV.Enabled = true;
            ContextualHelp contextHelpRV = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Berekenen#rekenpunten-verwijderen");
            buttonRV.SetContextualHelp(contextHelpRV);

            //panel9.AddSeparator(); //Voegt een verticale lijn toe
            #endregion
            #endregion

            #region Buttons Panel (GPS Punten)
            #region splitButton (GPS Inladen)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataGPSload = new SplitButtonData(
                "GPS Inladen",
                "Inladen")
            {
                Image = GPSimport16Scr,
                LargeImage = GPSimport32Scr,
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonGPSload = panel2.AddItem(splitButtonDataGPSload) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonGPSload.IsSynchronizedWithCurrentItem = false;

            /// Stel hoofdactie van de splitButton in
            PushButtonData GPSloadButtonData = new PushButtonData(
            "GPS Inladen", /// De naam van de standaardactie
            "GPS\nInladen", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
            "NijhofAddIn.Revit.Commands.GPS.Inladen" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Laad alle GPS punten",
                LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                Image = GPSimport16Scr,
                LargeImage = GPSimport32Scr,
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton GPSloadButton = splitButtonGPSload.AddPushButton(GPSloadButtonData);
            GPSloadButton.Enabled = true;
            ContextualHelp contextHelpGPSload = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-inladen");
            GPSloadButton.SetContextualHelp(contextHelpGPSload);

            /// Voegt een horizontale lijn toe onder de eerste knop
            splitButtonGPSload.AddSeparator();

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataRload = new PushButtonData(
                "Plaats Riool",
                "Plaats Riool",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.InladenRiool" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Riool GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                Image = GPSimport16Scr,
                LargeImage = GPSimport32Scr,
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonRload = splitButtonGPSload.AddPushButton(btndataRload);
            buttonRload.Enabled = true;
            ContextualHelp contextHelpRload = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#plaats-");
            buttonRload.SetContextualHelp(contextHelpRload);

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataMVload = new PushButtonData(
                "Plaats Lucht",
                "Plaats Lucht",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.InladenLucht" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Lucht GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                Image = GPSimport16Scr,
                LargeImage = GPSimport32Scr,
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonMVload = splitButtonGPSload.AddPushButton(btndataMVload);
            buttonMVload.Enabled = true;
            ContextualHelp contextHelpMVload = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#plaats-");
            buttonMVload.SetContextualHelp(contextHelpMVload);

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataKWload = new PushButtonData(
                "Plaats Koud Water",
                "Plaats Koud Water",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.InladenKW" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Koud Water GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                Image = GPSimport16Scr,
                LargeImage = GPSimport32Scr,
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonKWload = splitButtonGPSload.AddPushButton(btndataKWload);
            buttonKWload.Enabled = true;
            ContextualHelp contextHelpKWload = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#plaats-");
            buttonKWload.SetContextualHelp(contextHelpKWload);

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndataWWload = new PushButtonData(
                "Plaats Warm Water",
                "Plaats Warm Water",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.InladenWW" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Warm Water GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                Image = GPSimport16Scr,
                LargeImage = GPSimport32Scr,
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton buttonWWload = splitButtonGPSload.AddPushButton(btndataWWload);
            buttonWWload.Enabled = true;
            ContextualHelp contextHelpWWload = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#plaats-");
            buttonWWload.SetContextualHelp(contextHelpWWload);

            /// Knopgegevens instellen voor de zesde knop onder de dropdown
            PushButtonData btndataEleload = new PushButtonData(
                "Plaats Elektra",
                "Plaats Elektra",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.InladenElektra" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Elektra GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                Image = GPSimport16Scr,
                LargeImage = GPSimport32Scr,
            };

            /// Voeg de zesde knop toe aan de dropdown
            PushButton buttonEleload = splitButtonGPSload.AddPushButton(btndataEleload);
            buttonEleload.Enabled = true;
            ContextualHelp contextHelpEleload = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#plaats-");
            buttonEleload.SetContextualHelp(contextHelpEleload);

            /// Knopgegevens instellen voor de zevende knop onder de dropdown
            PushButtonData btndataMKload = new PushButtonData(
                "Plaats Meterkast",
                "Plaats Meterkast",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.InladenMeterkast" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Meterkast GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                Image = GPSimport16Scr,
                LargeImage = GPSimport32Scr,
            };

            /// Voeg de zevende knop toe aan de dropdown
            PushButton buttonMKload = splitButtonGPSload.AddPushButton(btndataMKload);
            buttonMKload.Enabled = true;
            ContextualHelp contextHelpMKload = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#plaats-");
            buttonMKload.SetContextualHelp(contextHelpMKload);

            /// Knopgegevens instellen voor de achtste knop onder de dropdown
            PushButtonData btndataTIload = new PushButtonData(
                "Plaats Tag/ Intercom",
                "Plaats Tag/ Intercom",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.InladenTI" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Tag/ Intercom GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                Image = GPSimport16Scr,
                LargeImage = GPSimport32Scr,
            };

            /// Voeg de achtste knop toe aan de dropdown
            PushButton buttonTIload = splitButtonGPSload.AddPushButton(btndataTIload);
            buttonTIload.Enabled = true;
            ContextualHelp contextHelpTIload = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#plaats-");
            buttonTIload.SetContextualHelp(contextHelpTIload);

            //panel2.AddSeparator(); //Voegt een verticale lijn toe

            #endregion

            #region splitButton (GPS Punten Toevoegen)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataGPS = new SplitButtonData(
                "GPS Toevoegen",
                "Toevoegen")
            {
                Image = GPS16Scr,
                LargeImage = GPS32Scr,
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonGPS = panel2.AddItem(splitButtonDataGPS) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonGPS.IsSynchronizedWithCurrentItem = false;

            /// Stel hoofdactie van de splitButton in
            PushButtonData GPSButtonData = new PushButtonData(
                "GPS Toevoegen", /// De naam van de standaardactie
                "GPS\nToevoegen", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.AddAlles" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Voegt GPS Punten toe",
                LongDescription = "Voegt Riool, Lucht, Warmwater en Koudwater GPS punten toe op de door de code bepaalde locaties.",
                Image = GPS16Scr,
                LargeImage = GPS32Scr,
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton GPSButton = splitButtonGPS.AddPushButton(GPSButtonData);
            GPSButton.Enabled = true;
            ContextualHelp contextHelpGPSButton = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-toevoegen");
            GPSButton.SetContextualHelp(contextHelpGPSButton);

            /// Voegt een horizontale lijn toe onder de eerste knop
            splitButtonGPS.AddSeparator();

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataR = new PushButtonData(
                "GPS: Riool",
                "GPS: Riool",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.AddRiool" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt Riool GPS Punten toe",
                LongDescription = "Voegt alleen Riool GPS punten toe op alle speciedeksels in het model.",
                Image = GPS16Scr,
                LargeImage = GPS32Scr,
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonR = splitButtonGPS.AddPushButton(btndataR);
            buttonR.Enabled = true;
            ContextualHelp contextHelpR = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-toevoegen");
            buttonR.SetContextualHelp(contextHelpR);

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataMV = new PushButtonData(
                "GPS: Lucht",
                "GPS: Lucht",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.AddLucht" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt Lucht GPS Punten toe",
                LongDescription = "Voegt alleen Lucht GPS punten toe op alle ventielen in het model",
                Image = GPS16Scr,
                LargeImage = GPS32Scr,
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonMV = splitButtonGPS.AddPushButton(btndataMV);
            buttonMV.Enabled = true;
            ContextualHelp contextHelpMV = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-toevoegen");
            buttonMV.SetContextualHelp(contextHelpMV);

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataKW = new PushButtonData(
                "GPS: Koud Water",
                "GPS: Koud Water",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.AddKoudWater" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt Koud Water GPS Punten toe",
                LongDescription = "Voegt alleen Koud Water GPS punten toe op alle open einden van opgaande waterleidingen.",
                Image = GPS16Scr,
                LargeImage = GPS32Scr,
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonKW = splitButtonGPS.AddPushButton(btndataKW);
            buttonKW.Enabled = true;
            ContextualHelp contextHelpKW = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-toevoegen");
            buttonKW.SetContextualHelp(contextHelpKW);

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndataWW = new PushButtonData(
                "GPS: Warm Water",
                "GPS: Warm Water",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.AddWarmWater" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt Warm Water GPS Punten toe",
                LongDescription = "Voegt alleen Warm Water GPS punten toe op alle open einden van opgaande waterleidingen.",
                Image = GPS16Scr,
                LargeImage = GPS32Scr,
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton buttonWW = splitButtonGPS.AddPushButton(btndataWW);
            buttonWW.Enabled = true;
            ContextualHelp contextHelpWW = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-toevoegen");
            buttonWW.SetContextualHelp(contextHelpWW);

            #endregion

            #region splitButton (GPS Punten Verwijderen)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataGPSdel = new SplitButtonData(
                "GPS Verwijderen",
                "Verwijderen")
            {
                Image = GPSdel16Scr,
                LargeImage = GPSdel32Scr,
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonGPSdel = panel2.AddItem(splitButtonDataGPSdel) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonGPSdel.IsSynchronizedWithCurrentItem = false;

            /// Stel hoofdactie van de SplitButton in
            PushButtonData delButtonData = new PushButtonData(
                "GPS Verwijderen", /// De naam van de standaardactie
                "GPS\nVerwijderen", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.DelAlles" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Verwijdert alle GPS Punten",
                LongDescription = "Verwijdert alle GPS punten in het model",
                Image = GPSdel16Scr,
                LargeImage = GPSdel32Scr,
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton delButton = splitButtonGPSdel.AddPushButton(delButtonData);
            delButton.Enabled = true;
            ContextualHelp contextHelpdelButton = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-verwijderen");
            delButton.SetContextualHelp(contextHelpdelButton);

            /// Voegt een horizontale lijn toe onder de eerste knop
            splitButtonGPSdel.AddSeparator();

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataRdel = new PushButtonData(
                "GPS: Riool",
                "GPS: Riool",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.DelRiool" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verwijdert alle Riool GPS Punten",
                LongDescription = "Verwijdert alleen de Riool GPS punten in het model.",
                Image = GPSdel16Scr,
                LargeImage = GPSdel32Scr,
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonRdel = splitButtonGPSdel.AddPushButton(btndataRdel);
            buttonRdel.Enabled = true;
            ContextualHelp contextHelpRdel = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-verwijderen");
            buttonRdel.SetContextualHelp(contextHelpRdel);

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataMVdel = new PushButtonData(
                "GPS: Lucht",
                "GPS: Lucht",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.DelLucht" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verwijdert alle Lucht GPS Punten",
                LongDescription = "Verwijdert alleen de Lucht GPS punten in het model.",
                Image = GPSdel16Scr,
                LargeImage = GPSdel32Scr,
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonMVdel = splitButtonGPSdel.AddPushButton(btndataMVdel);
            buttonMVdel.Enabled = true;
            ContextualHelp contextHelpMVdel = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-verwijderen");
            buttonMVdel.SetContextualHelp(contextHelpMVdel);

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataKWdel = new PushButtonData(
                "GPS: Koud Water",
                "GPS: Koud Water",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.DelKoudWater" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verwijdert alle Koud Water GPS Punten",
                LongDescription = "Verwijdert alleen de Koud Water GPS punten in het model.",
                Image = GPSdel16Scr,
                LargeImage = GPSdel32Scr,
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonKWdel = splitButtonGPSdel.AddPushButton(btndataKWdel);
            buttonKWdel.Enabled = true;
            ContextualHelp contextHelpKWdel = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-verwijderen");
            buttonKWdel.SetContextualHelp(contextHelpKWdel);

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndataWWdel = new PushButtonData(
                "GPS: Warm Water",
                "GPS: Warm Water",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.GPS.DelWarmWater" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verwijdert alle Warm Water GPS Punten",
                LongDescription = "Verwijdert alleen de Warm Water GPS punten in het model.",
                Image = GPSdel16Scr,
                LargeImage = GPSdel32Scr,
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton buttonWWdel = splitButtonGPSdel.AddPushButton(btndataWWdel);
            buttonWWdel.Enabled = true;
            ContextualHelp contextHelpWWdel = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-verwijderen");
            buttonWWdel.SetContextualHelp(contextHelpWWdel);

            #endregion
            #endregion

            #region Buttons Panel (Sparingen)
            #region pushButton (Muur Sparingen)
            /// Knopgegevens instellen /// Herhaalbaar voor elke extra knop die je wilt
            PushButtonData btndataMS = new PushButtonData
                (
                "Muursparingen Toevoegen",
                "Muur-\nsparingen",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Sparingen.AddMuur" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt sparingen in de muren toe",
                LongDescription = "Zoekt naar waar VWA of HWA clasht met een vloer en zet daar een sparing neer.",
                Image = MuurSparing16Scr,
                LargeImage = MuurSparing32Scr,
            };

            /// Voeg de knop toe aan het Ribbon Panel
            PushButton buttonMS = (PushButton)panel3.AddItem(btndataMS);
            buttonMS.Enabled = false;
            ContextualHelp contextHelpMS = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Sparingen#muur-sparingen");
            buttonMS.SetContextualHelp(contextHelpMS);

            //panel2.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region pushButton (Balk Sparingen)
            /// Knopgegevens instellen /// Herhaalbaar voor elke extra knop die je wilt
            PushButtonData btndataBS = new PushButtonData
                (
                "Balksparingen Toevoegen",
                "Balk-\nsparingen",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Sparingen.AddBalk" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt sparingen in de balken toe",
                LongDescription = "Zoekt naar waar een VWA of HWA pipe clasht met een funderingsbalk en zet daar een sparing neer.",
                Image = BalkSparing16Scr,
                LargeImage = BalkSparing32Scr,
            };

            /// Voeg de knop toe aan het Ribbon Panel
            PushButton buttonBS = (PushButton)panel3.AddItem(btndataBS);
            buttonBS.Enabled = false;
            ContextualHelp contextHelpBS = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Sparingen#balk-sparingen");
            buttonBS.SetContextualHelp(contextHelpBS);

            panel3.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region pushButton (TEST)
            /// Knopgegevens instellen /// Herhaalbaar voor elke extra knop die je wilt
            PushButtonData btndataTEST = new PushButtonData
                (
                "TEST",
                "TEST",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Core.Foutmelding" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Een knop waar ik functies mee kan testen",
                LongDescription = "Hier voeg ik soms bepaalde functies aan toe die ik moet testen zodat ik geen andere knoppen sloop of nutteloze dingen toe voeg",
                Image = PlaceHolder16Scr,
                LargeImage = PlaceHolder32Scr,
            };

            /// Voeg de knop toe aan het Ribbon Panel
            PushButton buttonTEST = (PushButton)panel3.AddItem(btndataTEST);
            buttonTEST.Enabled = true;
            ContextualHelp contextHelpTEST = new ContextualHelp(ContextualHelpType.Url,
                    "http://www.autodesk.com");
            buttonTEST.SetContextualHelp(contextHelpTEST);

            //panel2.AddSeparator(); //Voegt een verticale lijn toe
            #endregion
            #endregion

            #region Buttons Panel (Wijzigen)
            #region pushButton (Ontstoppingsstuk Omzetten)
            /// Knopgegevens instellen /// Herhaalbaar voor elke extra knop die je wilt
            PushButtonData btndataOntstop = new PushButtonData
                (
                "Onstoppingsstuk aanpassen",
                "Ontstoppings-\nstuk",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Wijzigen.OntstoppingsstukOmzetten" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Veranderd de 'Family Type' van alle Ontstoppingsstukken",
                LongDescription = "Deze functie zoekt in het hele model naar Manchet Ontstoppingsstukken van category Pipe Accessories. Deze zet hij om naar Pipe fittings" +
                                  "en laad deze in het model. Ontstoppingsstukken zonder manchet worden niet omgezet.",
                Image = Ontstop16Scr,
                LargeImage = Ontstop32Scr,
            };

            PushButton buttonOntstop = (PushButton)panel4.AddItem(btndataOntstop);
            buttonOntstop.Enabled = true;
            ContextualHelp contextHelpOntstop = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Wijzigen#ontstoppingsstuk");
            buttonOntstop.SetContextualHelp(contextHelpOntstop);
            #endregion

            #region pushButton (Update HWA Artikelnummer)
            PushButtonData btndataArtikeln = new PushButtonData
                (
                "HWA Artikelnummer Updaten",
                "HWA\n Artikelnr.",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Wijzigen.UpdateHWAArtikelnummer" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Past de artikelnummers van HWA ø80 aan naar die we bij Nijhof gebruiken",
                LongDescription = "Deze tool past van alle ø80 HWA met type name: HWA 6m, HWA 5,55m en PVC 5,55m het artikelnummer aan naar 20033890.",
                Image = UpdateArtikel16Scr,
                LargeImage = UpdateArtikel32Scr,
            };

            PushButton buttonArtikeln = (PushButton)panel4.AddItem(btndataArtikeln);
            buttonArtikeln.Enabled = true;
            ContextualHelp contextHelpArtikeln = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Wijzigen#hwa-artikelnr");
            buttonArtikeln.SetContextualHelp(contextHelpArtikeln);
            #endregion

            #region pushButtondata (HWA Lengte Updater)
            PushButtonData btndataUpdater = new PushButtonData
                (
                "HWA Lengte Updaten",
                "HWA\nLengte",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Wijzigen.UpdateHWALengte" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Past de opgaande lengte van HWA aan",
                LongDescription = "Deze tool zoekt naar alle pipes van het type: HWA 6m, HWA 5,55m en PVC 5,55m en of deze ø80 of ø100 heeft en veranderd de lengte naar 800mm.",
                Image = Regen16Scr,
                LargeImage = Regen32Scr,
            };

            PushButton buttonUpdater = (PushButton)panel4.AddItem(btndataUpdater);
            buttonUpdater.Enabled = true;
            ContextualHelp contextHelpUpdater = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Wijzigen#hwa-lengte");
            buttonUpdater.SetContextualHelp(contextHelpUpdater);
            #endregion

            #region pushButtondata (Standleiding Lengte Aanpassen)
            PushButtonData btndataStllengte = new PushButtonData
                (
                "Standleiding Lengte Aanpassen",
                "Standleiding\nLengte",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Wijzigen.StandleidingLengte" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Past schuine gedeelte van de standleiding aan naar 250mm",
                LongDescription = "Selecteer de bochten van de standleiding, deze functie verplaatst het onderste hulpstuk richting de standleiding zodat de lengte van het schuine gedeelte 250 mm is.",
                Image = Lengte16Scr,
                LargeImage = Lengte32Scr,
            };

            PushButton buttonStllengte = (PushButton)panel4.AddItem(btndataStllengte);
            buttonStllengte.Enabled = true;
            ContextualHelp contextHelpStllengte = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Wijzigen#standleiding-lengte");
            buttonStllengte.SetContextualHelp(contextHelpStllengte);
            #endregion
            #endregion

            #region Buttons Panel (Export)
            #region splitButton (Export Options)
            ///// Instellen van de split-knopgegevens
            //SplitButtonData splitButtonDataExport = new SplitButtonData(
            //    "Schedule Export",
            //    "Schedule\nExport")
            //{
            //    Image = Export16Scr,
            //    LargeImage = Export32Scr,
            //};

            ///// Voeg de SplitButton toe aan het panel (vervang 'panel' door de juiste panel variabele)
            //SplitButton splitButtonExport = panel5.AddItem(splitButtonDataExport) as SplitButton;

            ///// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            //splitButtonExport.IsSynchronizedWithCurrentItem = false;

            ///// Knopgegevens instellen voor "Export Schedules"
            //PushButtonData btndataExport = new PushButtonData(
            //    "Schedule Export",
            //    "Schedule\nExport",
            //    Assembly.GetExecutingAssembly().Location,
            //    "Damians_Tools.Export.ExportSchedule"
            //)
            //{
            //    ToolTip = "Export schedule naar Excel",
            //    LongDescription = "Exporteert de schedules waarvan de variabele 'Schedule exporteren' is aangevinkt naar excel naar een gespecificeerde locatie.",
            //    Image = Export16Scr,
            //    LargeImage = Export32Scr,
            //};

            ///// Voeg hoofdactie toe aan de splitbutton
            //PushButton buttonExport = splitButtonExport.AddPushButton(btndataExport);
            //buttonExport.Enabled = true;
            //ContextualHelp contextHelpExport = new ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com");
            //buttonExport.SetContextualHelp(contextHelpExport);

            ///// Knopgegevens instellen voor "Reset Schedule Export"
            //PushButtonData btndataExportReset = new PushButtonData(
            //    "Reset Schedule Export",
            //    "Reset Schedule\nExport",
            //    Assembly.GetExecutingAssembly().Location,
            //    "Damians_Tools.Export.ExportScheduleReset"
            //)
            //{
            //    ToolTip = "Reset 'Export Schedule' vinkjes",
            //    LongDescription = "Reset de variabele 'Schedule exporteren' van alle schedules. Deze wordt op false gezet (uitgevinkt).",
            //    Image = Reset16Scr,
            //    LargeImage = Reset32Scr,
            //};

            ///// Voeg de tweede knop toe aan de splitbutton
            //PushButton buttonExportReset = splitButtonExport.AddPushButton(btndataExportReset);
            //buttonExportReset.Enabled = true;
            //ContextualHelp contextHelpExportReset = new ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com");
            //buttonExportReset.SetContextualHelp(contextHelpExportReset);

            ///// Knopgegevens instellen voor "Parameters Toevoegen"
            //PushButtonData btndataParameter = new PushButtonData(
            //    "Paramaters Toevoegen",
            //    "Parameters\nToevoegen",
            //    Assembly.GetExecutingAssembly().Location,
            //    "Damians_Tools.Core.Foutmelding"
            //)
            //{
            //    ToolTip = "Parameter toevoegen voor Export Schedules",
            //    LongDescription = "Deze funtie voegt de 'Schedule exporteren' parameter toe. Als die vervolgens is aangevinkt kan je met de knop 'Export Schedule' deze exporteren.",
            //    Image = Parameter16Scr,
            //    LargeImage = Parameter32Scr,
            //};

            ///// Voeg de derde knop toe aan de splitbutton
            //PushButton buttonParameter = splitButtonExport.AddPushButton(btndataParameter);
            //buttonParameter.Enabled = false;
            //ContextualHelp contextHelpParameter = new ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com");
            //buttonParameter.SetContextualHelp(contextHelpParameter);
            #endregion

            #region pushButton (Materiaal Export)
            /// Knopgegevens instellen voor "Materiaal Export"
            PushButtonData btndataMateriaalExport = new PushButtonData
                (
                "Materiaallijst Exporteren",
                "Materiaal\nExport",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Export.ExportMateriaallijst" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Exporteert materiaal gegevens",
                LongDescription = "Exporteert de materiaalgegevens naar een gespecificeerde locatie.",
                Image = Materiaal16Scr,
                LargeImage = Materiaal32Scr,
            };

            /// Voeg de knop toe aan het Ribbon Panel
            PushButton buttonMateriaalExport = (PushButton)panel5.AddItem(btndataMateriaalExport);
            buttonMateriaalExport.Enabled = true;
            ContextualHelp contextHelpMateriaalExport = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Export#materiaal-export");
            buttonMateriaalExport.SetContextualHelp(contextHelpMateriaalExport);

            // panel5.AddSeparator(); // Voegt een verticale lijn toe indien nodig
            #endregion

            #region pushButton (Zaaglijst Export)
            /// Knopgegevens instellen voor "Zaaglijst Export"
            PushButtonData btndataZaaglijstExport = new PushButtonData
                (
                "Zaaglijst Export",
                "Zaaglijst\nExport",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Export.ExportZaaglijst" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Exporteert zaaglijst gegevens",
                LongDescription = "Exporteert de zaaglijstgegevens naar een gespecificeerde locatie.",
                Image = Zaag16Scr,
                LargeImage = Zaag32Scr,
            };

            /// Voeg de knop toe aan het Ribbon Panel
            PushButton buttonZaaglijstExport = (PushButton)panel5.AddItem(btndataZaaglijstExport);
            buttonZaaglijstExport.Enabled = true;
            ContextualHelp contextHelpZaaglijstExport = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/Export#zaaglijst-export");
            buttonZaaglijstExport.SetContextualHelp(contextHelpZaaglijstExport);

            // panel5.AddSeparator(); // Voegt een verticale lijn toe indien nodig
            #endregion
            #endregion

            #region Buttons Panel (Elektra)
            #region pushButton (SwitchCodes)
            PushButtonData btndataSwitchCode = new PushButtonData
                (
                "Switch Codes",
                "Switch\nCodes",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SwitchcodeList" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Laat alle Switchcodes zien die in het project gebruikt zijn",
                LongDescription = "Deze funtie kijkt naar de ingevulde parameters 'Switch code', 'Switch Code', 'Switch code 1' en 'Switch code 2' en verwijderd dubbelen. Deze worden in een lijst gezet en getoond in een pop-up.",
                Image = SwitchCode16Scr,
                LargeImage = SwitchCode32Scr,
            };

            PushButton buttonSwitchCode = (PushButton)panel6.AddItem(btndataSwitchCode);
            buttonSwitchCode.Enabled = true;
            ContextualHelp contextHelpSwitchCode = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Elektra#switchcodes");
            buttonSwitchCode.SetContextualHelp(contextHelpSwitchCode);
            #endregion

            #region pushButton (tagGroepNummer)
            PushButtonData btndataTagGN = new PushButtonData
                (
                "Tag Groepnummer",
                "Tag Groep-\nnummer",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.GroepTag" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tagged alle Elektra elementen met een ingevulde Groepnummer",
                LongDescription = "Deze funtie tagged alle 'Electrical Fixtures', 'Lighting Devices' etc. in de huidige view waarbij de parameter 'Groep- (nummer)' is ingevuld. Als de 'Groep- (nummer)' niet is ingevuld tagged die deze dus niet.",
                Image = PlaceHolder16Scr,
                LargeImage = GroepTag32Scr,
            };

            PushButton buttonTagGN = (PushButton)panel6.AddItem(btndataTagGN);
            buttonTagGN.Enabled = true;
            ContextualHelp contextHelpTagGN = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Elektra#tag-groepnummer");
            buttonTagGN.SetContextualHelp(contextHelpTagGN);
            #endregion

            #region pushButton (tagSwitchCode)
            PushButtonData btndataTagSC = new PushButtonData
                (
                "Tag Switchcodes",
                "Tag Switch-\ncodes",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SwitchcodeTag" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tagged alle Elektra elementen met een ingevulde Switchcode",
                LongDescription = "Deze funtie tagged alle 'Electrical Fixtures', 'Lighting Devices' etc. in de huidige view waarbij de parameter 'Switchcode' is ingevuld. Als de 'Switchcode' niet is ingevuld tagged die deze dus niet.",
                Image = PlaceHolder16Scr,
                LargeImage = SwitchTag32Scr,
            };

            PushButton buttonTagSC = (PushButton)panel6.AddItem(btndataTagSC);
            buttonTagSC.Enabled = true;
            ContextualHelp contextHelpTagSC = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Elektra#tag-switchcodes");
            buttonTagSC.SetContextualHelp(contextHelpTagSC);
            #endregion
            #endregion

            #region Buttons Panel (Overig)
            #region pushButton (Klik op mij)
            PushButtonData btndataKlik = new PushButtonData
                (
                "Klik op mij",
                "Klik op\nmij",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Overig.Klik" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verrassing!",
                LongDescription = "Ja ik ga hier echt niet vertellen wat deze knop doet hoor :)",
                Image = Gift16Scr,
                LargeImage = Gift32Scr,
            };

            PushButton buttonKlik = (PushButton)panel7.AddItem(btndataKlik);
            buttonKlik.Enabled = true;
            #endregion

            #region pushButton (Help)
            PushButtonData btndataHelp = new PushButtonData
                (
                "Help",
                "Help",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Overig.Help" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Een 'Help' knop",
                LongDescription = "Wat meer wil je weten?",
                Image = Vraag16Scr,
                LargeImage = Vraag32Scr,
            };

            PushButton buttonHelp = (PushButton)panel7.AddItem(btndataHelp);
            buttonHelp.Enabled = true;
            #endregion

            #region pushButton (Info)
            PushButtonData btndataInfo = new PushButtonData
                (
                "Info",
                "Info",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Overig.Info" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Geeft info weer",
                LongDescription = "Deze functie geeft de versie weer, dus wanneer deze tool voor het laatst is aangepast.",
                Image = Info16Scr,
                LargeImage = Info32Scr,
            };

            PushButton buttonInfo = (PushButton)panel7.AddItem(btndataInfo);
            buttonInfo.Enabled = true;
            #endregion
            #endregion

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }

}