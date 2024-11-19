using Autodesk.Revit.UI;
using NijhofAddIn.Resources;
using NijhofAddIn.Revit.Commands.Tools.Tag;
using NijhofAddIn.Revit.Core;
using ricaun.Revit.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace NijhofAddIn.Revit

{
    [AppLoader]
    public class App : IExternalApplication
    {
        // Nijhof Tools
        const string RIBBON_TAB1 = "Nijhof Tools";
        const string RIBBON_PANEL1 = "Content";
        const string RIBBON_PANEL2 = "Tools";
        const string RIBBON_PANEL3 = "Sparingen";
        const string RIBBON_PANEL4 = "GPS Punten";
        const string RIBBON_PANEL5 = "Prefab";
        const string RIBBON_PANEL6 = "View";
        const string RIBBON_PANEL7 = "Tag";
        const string RIBBON_PANEL8 = "Schedules";
        const string RIBBON_PANEL9 = "Export";
        const string RIBBON_PANEL10 = "Overig";

        // Nijhof Elektra
        const string RIBBON_TAB2 = "Nijhof Elektra";
        //const string RIBBON_PANEL11 = "Content";
        const string RIBBON_PANEL12 = "Toevoegen";
        const string RIBBON_PANEL13 = "Tag";
        const string RIBBON_PANEL14 = "Overig";

        

        public Result OnStartup(UIControlledApplication app)
        {
            // Nijhof Tools

            #region Tabblad 1 Nijhof Tools
            /// Maakt Ribbon Tabblad aan
            try
            {
                app.CreateRibbonTab(RIBBON_TAB1);
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                // Tabblad bestaat al
            }
            #endregion

            #region Panelen Nijhof Tools
            /// Methode om een Ribbon Panel te verkrijgen of aan te maken
            RibbonPanel GetOrCreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
            {
                var panels = app.GetRibbonPanels(tabName);
                foreach (var pnl in panels)
                {
                    if (pnl.Name == panelName)
                    {
                        return pnl;
                    }
                }
                return app.CreateRibbonPanel(tabName, panelName);
            }

            // Panel 1 (Content)
            RibbonPanel panel1 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL1);

            // Panel 2 (Tools)
            RibbonPanel panel2 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL2);

            // Panel 3 (Sparingen)
            RibbonPanel panel3 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL3);

            // Panel 4 (GPS Punten)
            RibbonPanel panel4 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL4);

            // Panel 5 (Prefab)
            RibbonPanel panel5 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL5);

            // Panel 6 (View)
            RibbonPanel panel6 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL6);

            // Panel 7 (Tag)
            RibbonPanel panel7 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL7);

            // Panel 8 (Schedules)
            //RibbonPanel panel8 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL8);

            // Panel 9 (Export)
            RibbonPanel panel9 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL9);

            // Panel 10 (Overig)
            RibbonPanel panel10 = GetOrCreateRibbonPanel(app, RIBBON_TAB1, RIBBON_PANEL10);
            #endregion



            // Nijhof Elektra

            #region Tabblad 2 Nijhof Elektra
            /// Maakt Ribbon Tabblad aan
            try
            {
                app.CreateRibbonTab(RIBBON_TAB2);
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                //tab already exists
            }
            #endregion

            #region Panelen Nijhof Elektra
            /// Methode voor het aanmaken van een Ribbon Panel
            RibbonPanel CreateRibbonPanelIfNotExists(UIControlledApplication app, string tabName, string panelName)
            {
                List<RibbonPanel> panels = app.GetRibbonPanels(tabName);
                foreach (RibbonPanel pnl in panels)
                {
                    if (pnl.Name == panelName)
                    {
                        return pnl; // Panel bestaat al, return bestaand panel
                    }
                }
                // Bestaat panel niet? Maak panel aan
                return app.CreateRibbonPanel(tabName, panelName);
            }

            // Panel 11 (Content)
            RibbonPanel panel11 = CreateRibbonPanelIfNotExists(app, RIBBON_TAB2, RIBBON_PANEL1);

            // Panel 12 (Toevoegen)
            RibbonPanel panel12 = CreateRibbonPanelIfNotExists(app, RIBBON_TAB2, RIBBON_PANEL12);

            // Panel 13 (Tag)
            RibbonPanel panel13 = CreateRibbonPanelIfNotExists(app, RIBBON_TAB2, RIBBON_PANEL13);

            // Panel 14 (Overig)
            RibbonPanel panel14 = CreateRibbonPanelIfNotExists(app, RIBBON_TAB2, RIBBON_PANEL14);
            #endregion



            // Nijhof Tools Icons

            #region Icons
            AppIcons appBase = new AppIcons();

            var iconSources = new Dictionary<string, ImageSource>
            {
                { "PlaceHolder16", appBase.GetImageSource(Icons.error_16) },
                { "PlaceHolder32", appBase.GetImageSource(Icons.error_32) },
                { "Library32", appBase.GetImageSource(Icons.library32) },
                { "bruinTag16", appBase.GetImageSource(Icons.bruintag16) },
                { "groenTag16", appBase.GetImageSource(Icons.groentag16) },
                { "blauwTag16", appBase.GetImageSource(Icons.blauwtag16) },
                { "_3D32", appBase.GetImageSource(Icons._3d32) },
                { "MuurSparing32", appBase.GetImageSource(Icons.wall32) },
                { "VloerSparing32", appBase.GetImageSource(Icons.floor32) },
                { "BalkSparing32", appBase.GetImageSource(Icons.concrete32) },
                { "Ontstop16", appBase.GetImageSource(Icons.manhole16) },
                { "UpdateArtikel16", appBase.GetImageSource(Icons.article16) },
                { "Regen16", appBase.GetImageSource(Icons.rain16) },
                { "Lengte32", appBase.GetImageSource(Icons.length32) },
                { "Materiaal32", appBase.GetImageSource(Icons.basket32) },
                { "Zaag32", appBase.GetImageSource(Icons.saw32) },
                { "Vraag32", appBase.GetImageSource(Icons.question32) },
                { "Info32", appBase.GetImageSource(Icons.info32) },
                { "Manage32", appBase.GetImageSource(Icons.prototype32) },
                { "Prefab32", appBase.GetImageSource(Icons.pipelines32) },
                { "newPrefab32", appBase.GetImageSource(Icons.newpipelines32) },
                { "addPrefab32", appBase.GetImageSource(Icons.addpipelines32) },
                { "delPrefab32", appBase.GetImageSource(Icons.deletepipelines32) },
                { "Refresh32", appBase.GetImageSource(Icons.refresh32) },
                { "Connect32", appBase.GetImageSource(Icons.connect32) },
            };
            #endregion

            #region GPS Icons
            var gpsIconSources = new Dictionary<string, ImageSource>
            {
                { "OranjeGPS32", appBase.GetImageSource(Icons.oranjegps32) },
                { "BlauwGPS32", appBase.GetImageSource(Icons.blauwgps32) },
                { "RoodGPS32", appBase.GetImageSource(Icons.roodgps32) },
                { "GroenGPS32", appBase.GetImageSource(Icons.groengps32) },
                { "GeelGPS32", appBase.GetImageSource(Icons.geelgps32) },
                { "MagentaGPS32", appBase.GetImageSource(Icons.magentagps32) },
                { "laadZwartGPS32", appBase.GetImageSource(Icons.laadzwartgps32) },
                { "addZwartGPS32", appBase.GetImageSource(Icons.addzwartgps32) },
                { "addOranjeGPS32", appBase.GetImageSource(Icons.addoranjegps32) },
                { "addBlauwGPS32", appBase.GetImageSource(Icons.addblauwgps32) },
                { "addRoodGPS32", appBase.GetImageSource(Icons.addroodgps32) },
                { "addGroenGPS32", appBase.GetImageSource(Icons.addgroengps32) },
                { "addGeelGPS32", appBase.GetImageSource(Icons.addgeelgps32) },
                { "addMagentaGPS32", appBase.GetImageSource(Icons.addzwartgps32) },
                { "delZwartGPS32", appBase.GetImageSource(Icons.delzwartgps32) },
                { "delOranjeGPS32", appBase.GetImageSource(Icons.deloranjegps32) },
                { "delBlauwGPS32", appBase.GetImageSource(Icons.delblauwgps32) },
                { "delRoodGPS32", appBase.GetImageSource(Icons.delroodgps32) },
                { "delGroenGPS32", appBase.GetImageSource(Icons.delgroengps32) },
                { "delGeelGPS32", appBase.GetImageSource(Icons.delgeelgps32) },
                { "delMagentaGPS32", appBase.GetImageSource(Icons.delzwartgps32) },
            };
            #endregion



            // Nijhof Elektra Icons
            
            #region Icons
            var EleiconSources = new Dictionary<string, ImageSource>
            {
                // Stopcontacten
                { "WCDEnkel32", appBase.GetImageSource(Icons.WCDEnkel_32) },
                { "WCDDubbel32", appBase.GetImageSource(Icons.WCDDubbel_32) },
                { "WCDOpbouw32", appBase.GetImageSource(Icons.WCDOpbouw_32) },
                { "WCDEnkelWater32", appBase.GetImageSource(Icons.WCDWater1v_32) },
                { "WCDDubbelWater32", appBase.GetImageSource(Icons.WCDWater2v_32) },
                { "WCDPerilex32", appBase.GetImageSource(Icons.WCDPerilex_32) },
                { "WCDKracht32", appBase.GetImageSource(Icons.WCDKracht_32) },
                { "WCDVloer32", appBase.GetImageSource(Icons.WCDVloer_32) },

                // Aansluitpunten
                { "Bedraad32", appBase.GetImageSource(Icons.Bedraad_32) },
                { "Onbedraad32", appBase.GetImageSource(Icons.Onbedraad_32) },
                { "Enkel230v32", appBase.GetImageSource(Icons.Enkel230v_32) },
                { "Dubbel230v32", appBase.GetImageSource(Icons.Dubbel230v_32) },
                { "Enkel400v32", appBase.GetImageSource(Icons.Enkel400v_32) },
                { "CAP32", appBase.GetImageSource(Icons.CAP_32) },

                // Data
                { "EnkelData32", appBase.GetImageSource(Icons.EnkelData_32) },
                { "DubbelData32", appBase.GetImageSource(Icons.DubbelData_32) },

                // Schakelaars
                { "Enkelpolig32", appBase.GetImageSource(Icons.enkelpolig_32) },
                { "Dubbelpolig32", appBase.GetImageSource(Icons.dubbelpolig_32) },
                { "Wissel32", appBase.GetImageSource(Icons.wissel_32) },
                { "DubbelWissel32", appBase.GetImageSource(Icons.dubbelwissel_32) },
                { "Wissel2x32", appBase.GetImageSource(Icons.wissel2x_32) },
                { "Serie32", appBase.GetImageSource(Icons.serie_32) },
                { "Kruis32", appBase.GetImageSource(Icons.kruis_32) },
                { "Dimmer32", appBase.GetImageSource(Icons.dimmer_32) },
                { "WisselDimmer32", appBase.GetImageSource(Icons.wisseldimmer_32) },
                { "Jaloezie32", appBase.GetImageSource(Icons.jaloezie_32) },

                // Verlichting
                { "Centraaldoos32", appBase.GetImageSource(Icons.centraaldoos_32) },
                { "LichtPlafond32", appBase.GetImageSource(Icons.plafondlicht_32) },
                { "Inbouwspot32", appBase.GetImageSource(Icons.inbouwspot_32) },
                { "LichtWand32", appBase.GetImageSource(Icons.wandlicht_32) },

                // Overig
                { "Bediening32", appBase.GetImageSource(Icons.bbediening_32) },
                { "Rookmelder32", appBase.GetImageSource(Icons.rookmelder_32) },
                { "Deurbel32", appBase.GetImageSource(Icons.deurbel_32) },
                { "Dingdong32", appBase.GetImageSource(Icons.dingdong_32) },
                { "Intercom32", appBase.GetImageSource(Icons.intercom_32) },
                { "Grondkabel32", appBase.GetImageSource(Icons.grondkabel_32) },

                // Diversen
                { "SwitchCode32", appBase.GetImageSource(Icons.list_32) },
                { "GroepTag32", appBase.GetImageSource(Icons.electrical_32) },
                { "SwitchTag32", appBase.GetImageSource(Icons.switch_32) },
            };
            #endregion



            // Nijhof Tools Buttons

            #region CreatePushButton
            /// <summary>
            /// Creëert een PushButton en voegt deze toe aan het opgegeven RibbonPanel.
            /// </summary>
            /// <param name="panel">Het panel waarin de knop moet worden toegevoegd.</param>
            /// <param name="name">De naam van de knop.</param>
            /// <param name="text">De tekst die op de knop staat.</param>
            /// <param name="toolTip">De tooltip van de knop.</param>
            /// <param name="longDescription">De lange beschrijving van de knop.</param>
            /// <param name="commandNamespace">De namespace van het command dat uitgevoerd moet worden.</param>
            /// <param name="smallImage">De kleine afbeelding die aan de knop moet worden toegevoegd.</param>
            /// <param name="largeImage">De grote afbeelding die aan de knop moet worden toegevoegd.</param>
            /// <param name="helpUrl">De URL voor contextuele hulp.</param>
            /// <returns>De gemaakte PushButton.</returns>

            PushButton CreatePushButton(
                RibbonPanel panel,
                string name,
                string text,
                string toolTip,
                string longDescription,
                string commandNamespace,
                ImageSource smallImage = null,
                ImageSource largeImage = null,
                string helpUrl = null)
            {
                var pushButtonData = new PushButtonData(
                    name,
                    text,
                    Assembly.GetExecutingAssembly().Location,
                    commandNamespace)
                {
                    ToolTip = toolTip,
                    LongDescription = longDescription,
                    Image = smallImage,
                    LargeImage = largeImage

                };

                if (smallImage != null)
                {
                    pushButtonData.Image = smallImage;
                }

                if (largeImage != null)
                {
                    pushButtonData.LargeImage = largeImage;
                }

                var pushButton = (PushButton)panel.AddItem(pushButtonData);

                if (!string.IsNullOrEmpty(helpUrl))
                {
                    pushButton.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, helpUrl));
                }

                return pushButton;
            }
            #endregion

            #region Buttons Panel 1 (Content)
            #region pushButton (Library)
            PushButton buttonLibrary = CreatePushButton(
                panel1,
                "Library",
                "Library",
                "Opent de 'Nijhof Bibliotheek'",
                "Een bibliotheek waar alle vaak gebruikte modellen, tags en groepen in gezet kunnen worden zodat die makkelijk te vinden en in te laden zijn",
                "NijhofAddIn.Revit.Commands.Tools.Content.FamilyLoader",
                null,
                iconSources["Library32"],
                "http://www.jouwinternehulplink.com"
            );
            buttonLibrary.Enabled = true;
            #endregion
            #endregion

            #region Buttons Panel 2 (Tools)
            #region pushButton (Aansluiten Element)
            PushButton buttonAansluitenElement = CreatePushButton(
                panel2,
                "Aansluiten Element",
                "Aansluiten\nElement",
                "Lorem Ipsum",
                "Lorem Ipsum Lorem Ipsum",
                "NijhofAddIn.Revit.Commands.Tools.Tools.AansluitenElement",
                null,
                iconSources["Connect32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Wijzigen#standleiding-lengte"
            );
            buttonAansluitenElement.Enabled = true;
            #endregion

            #region pushButton (Standleiding Lengte Aanpassen)
            PushButton buttonStllengte = CreatePushButton(
                panel2,
                "Standleiding Lengte Aanpassen",
                "Standleiding\nLengte",
                "Past schuine gedeelte van de standleiding aan naar 250mm",
                "Selecteer de bochten van de standleiding, deze functie verplaatst het onderste hulpstuk richting de standleiding zodat de lengte van het schuine gedeelte 250 mm is.",
                "NijhofAddIn.Revit.Commands.Tools.Tools.StandleidingLengte",
                null,
                iconSources["Lengte32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Wijzigen#standleiding-lengte"
            );
            buttonStllengte.Enabled = true;
            panel2.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region pushButtondata (Ontstoppingsstuk Omzetten)
            PushButtonData btndataOntstop = new PushButtonData
                (
                "Onstoppingsstuk aanpassen",
                "Ontstoppingsstuk",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tools.OntstoppingsstukOmzetten"
                )
            {
                ToolTip = "Verandert de 'Family Type' van alle Ontstoppingsstukken",
                LongDescription = "Deze functie zoekt in het hele model naar Manchet Ontstoppingsstukken van category Pipe Accessories. Deze zet hij om naar Pipe fittings" +
                                  "en laad deze in het model. Ontstoppingsstukken zonder manchet worden niet omgezet.",
                Image = iconSources["Ontstop16"]
            };
            #endregion

            #region pushButtondata (Update HWA Artikelnummer)
            PushButtonData btndataArtikeln = new PushButtonData
                (
                "HWA Artikelnummer Updaten",
                "HWA Artikelnr.",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tools.UpdateHWAArtikelnummer"
                )
            {
                ToolTip = "Past de artikelnummers van HWA ø80 aan naar die we bij Nijhof gebruiken",
                LongDescription = "Deze tool past van alle ø80 HWA met type name: HWA 6m, HWA 5,55m en PVC 5,55m het artikelnummer aan naar 20033890.",
                Image = iconSources["UpdateArtikel16"]
            };
            #endregion

            #region pushButtondata (HWA Lengte Updater)
            PushButtonData btndataUpdater = new PushButtonData
                (
                "HWA Lengte Updaten",
                "HWA Lengte",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tools.UpdateHWALengte"
                )
            {
                ToolTip = "Past de opgaande lengte van HWA aan",
                LongDescription = "Deze tool zoekt naar alle opgaande HWA pipes en veranderd die naar een lengte van 800mm.",
                Image = iconSources["Regen16"]
            };
            #endregion

            #region Stacked pushButton (Ontstoppingsstuk, HWA Artikelnr, HWA Lengte)
            IList<RibbonItem> stackedButtons = panel2.AddStackedItems(btndataOntstop, btndataArtikeln, btndataUpdater);

            PushButton buttonOntstop = (PushButton)stackedButtons[0];
            PushButton buttonArtikeln = (PushButton)stackedButtons[1];
            PushButton buttonUpdater = (PushButton)stackedButtons[2];
            
            ContextualHelp contextHelpOntstop = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Wijzigen#ontstoppingsstuk");
            buttonOntstop.SetContextualHelp(contextHelpOntstop);

            ContextualHelp contextHelpArtikeln = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Wijzigen#hwa-artikelnr");
            buttonArtikeln.SetContextualHelp(contextHelpArtikeln);

            ContextualHelp contextHelpUpdater = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Wijzigen#hwa-lengte");
            buttonUpdater.SetContextualHelp(contextHelpUpdater);
            #endregion
            #endregion

            #region Buttons Panel 3 (Sparingen)
            #region pushButton (Muur Sparingen)
            PushButton buttonMS = CreatePushButton(
                panel3,
                "Muursparingen Toevoegen",
                "Muur",
                "Voegt sparingen in de muren toe",
                "LET OP! Werkt alleen met Revit modellen/ links. Niet met IFC. Zoekt naar waar VWA of HWA clasht met een muur en zet daar een sparing neer. Gebruiker moet nog controleren!",
                "NijhofAddIn.Revit.Core.Foutmelding",
                null,
                iconSources["MuurSparing32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Sparingen#muur-sparingen"
            );

            buttonMS.Enabled = true;
            #endregion

            #region pushButton (Vloer Sparingen)
            PushButton buttonVS = CreatePushButton(
                panel3,
                "Vloersparingen Toevoegen",
                "Vloer",
                "Voegt sparingen in de vloeren toe",
                "LET OP! Werkt alleen met Revit modellen/ links. Niet met IFC. Zoekt naar waar een VWA of HWA pipe clasht met een vloer en zet daar een sparing neer. Gebruiker moet nog controleren!",
                "NijhofAddIn.Revit.Core.Foutmelding",
                null,
                iconSources["VloerSparing32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Sparingen#balk-sparingen"
            );

            buttonVS.Enabled = true;
            #endregion

            #region pushButton (Balk Sparingen)
            PushButton buttonBS = CreatePushButton(
                panel3,
                "Balksparingen Toevoegen",
                "Balk",
                "Voegt sparingen in de balken toe",
                "LET OP! Werkt alleen met Revit modellen/ links. Niet met IFC. Zoekt naar waar een VWA of HWA pipe clasht met een funderingsbalk en zet daar een sparing neer. Gebruiker moet nog controleren!",
                "NijhofAddIn.Revit.Core.Foutmelding",
                null,
                iconSources["BalkSparing32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Sparingen#balk-sparingen"
            );

            buttonBS.Enabled = true;
            #endregion
            #endregion

            #region Buttons Panel 4 (GPS Punten)
            #region splitButton (GPS Inladen)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataGPSload = new SplitButtonData(
                "GPS Inladen",
                "Inladen")
            {
                LargeImage = gpsIconSources["laadZwartGPS32"]
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonGPSload = panel4.AddItem(splitButtonDataGPSload) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonGPSload.IsSynchronizedWithCurrentItem = false;

            /// Stel hoofdactie van de splitButton in
            PushButtonData GPSloadButtonData = new PushButtonData(
            "GPS Inladen", /// De naam van de standaardactie
            "Inladen", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
            "NijhofAddIn.Revit.Commands.Tools.GPS.Inladen" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Laad alle GPS punten",
                LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                LargeImage = gpsIconSources["laadZwartGPS32"]
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
                "GPS: Riool",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.GPS.InladenRiool" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Riool GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = gpsIconSources["OranjeGPS32"]
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
                "GPS: Lucht",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.GPS.InladenLucht" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Lucht GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = gpsIconSources["OranjeGPS32"]
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
                "GPS: Koud Water",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.GPS.InladenKW" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Koud Water GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = gpsIconSources["BlauwGPS32"]
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
                "GPS: Warm Water",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.GPS.InladenWW" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Warm Water GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = gpsIconSources["RoodGPS32"]
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
                "GPS: Elektra",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.GPS.InladenElektra" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Elektra GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = gpsIconSources["GroenGPS32"]
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
                "GPS: Meterkast",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.GPS.InladenMeterkast" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Meterkast GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = gpsIconSources["GeelGPS32"]
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
                "GPS: Tag/ Intercom",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.GPS.InladenTI" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats Tag/ Intercom GPS Punt",
                LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = gpsIconSources["MagentaGPS32"]
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
                LargeImage = gpsIconSources["addZwartGPS32"]
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonGPS = panel4.AddItem(splitButtonDataGPS) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonGPS.IsSynchronizedWithCurrentItem = false;

            /// Stel hoofdactie van de splitButton in
            PushButtonData GPSButtonData = new PushButtonData(
                "GPS Toevoegen", /// De naam van de standaardactie
                "Toevoegen", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.GPS.AddAlles" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Voegt GPS Punten toe",
                LongDescription = "Voegt Riool, Lucht, Warmwater en Koudwater GPS punten toe op de door de code bepaalde locaties. Gebruiker moet nog controleren!",
                LargeImage = gpsIconSources["addZwartGPS32"]
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
                "NijhofAddIn.Revit.Commands.Tools.GPS.AddRiool" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt Riool GPS Punten toe",
                LongDescription = "Voegt alleen Riool GPS punten toe op alle speciedeksels in het model. Gebruiker moet nog controleren!",
                LargeImage = gpsIconSources["addOranjeGPS32"]
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
                "NijhofAddIn.Revit.Commands.Tools.GPS.AddLucht" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt Lucht GPS Punten toe",
                LongDescription = "Voegt alleen Lucht GPS punten toe op alle ventielen in het model. Gebruiker moet nog controleren!",
                LargeImage = gpsIconSources["addOranjeGPS32"]
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
                "NijhofAddIn.Revit.Commands.Tools.GPS.AddKoudWater" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt Koud Water GPS Punten toe",
                LongDescription = "Voegt alleen Koud Water GPS punten toe op alle open einden van opgaande waterleidingen. Gebruiker moet nog controleren!",
                LargeImage = gpsIconSources["addBlauwGPS32"]
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
                "NijhofAddIn.Revit.Commands.Tools.GPS.AddWarmWater" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Voegt Warm Water GPS Punten toe",
                LongDescription = "Voegt alleen Warm Water GPS punten toe op alle open einden van opgaande waterleidingen. Gebruiker moet nog controleren!",
                LargeImage = gpsIconSources["addRoodGPS32"]
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
                LargeImage = gpsIconSources["delZwartGPS32"]
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonGPSdel = panel4.AddItem(splitButtonDataGPSdel) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonGPSdel.IsSynchronizedWithCurrentItem = false;

            /// Stel hoofdactie van de SplitButton in
            PushButtonData delButtonData = new PushButtonData(
                "GPS Verwijderen", /// De naam van de standaardactie
                "Verwijderen", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.GPS.DelAlles" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Verwijdert alle GPS Punten",
                LongDescription = "Verwijdert alle GPS punten in het model",
                LargeImage = gpsIconSources["delZwartGPS32"]
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
                "NijhofAddIn.Revit.Commands.Tools.GPS.DelRiool" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verwijdert alle Riool GPS Punten",
                LongDescription = "Verwijdert alleen de Riool GPS punten in het model.",
                LargeImage = gpsIconSources["delOranjeGPS32"]
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
                "NijhofAddIn.Revit.Commands.Tools.GPS.DelLucht" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verwijdert alle Lucht GPS Punten",
                LongDescription = "Verwijdert alleen de Lucht GPS punten in het model.",
                LargeImage = gpsIconSources["delOranjeGPS32"]
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
                "NijhofAddIn.Revit.Commands.Tools.GPS.DelKoudWater" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verwijdert alle Koud Water GPS Punten",
                LongDescription = "Verwijdert alleen de Koud Water GPS punten in het model.",
                LargeImage = gpsIconSources["delBlauwGPS32"]
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
                "NijhofAddIn.Revit.Commands.Tools.GPS.DelWarmWater" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Verwijdert alle Warm Water GPS Punten",
                LongDescription = "Verwijdert alleen de Warm Water GPS punten in het model.",
                LargeImage = gpsIconSources["delRoodGPS32"]
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton buttonWWdel = splitButtonGPSdel.AddPushButton(btndataWWdel);
            buttonWWdel.Enabled = true;
            ContextualHelp contextHelpWWdel = new ContextualHelp(ContextualHelpType.Url,
                "https://github.com/Damianmts/NijhofAddIn/wiki/GPS-Punten#gps-verwijderen");
            buttonWWdel.SetContextualHelp(contextHelpWWdel);

            #endregion

            #region pushButton (GPS Punten Synchroniseren)
            PushButton buttonSync = CreatePushButton(
                panel4,
                "GPS Update",
                "Update",
                "Update de GPS punten",
                "Alle GPS punten die met 'GPS Toevoegen' zijn geplaatst worden geüpdate zodat ze weer op de juiste plek staan",
                "NijhofAddIn.Revit.Commands.Tools.GPS.Synchroniseren",
                null,
                iconSources["Refresh32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/"
            );
            buttonSync.Enabled = true;
            #endregion
            #endregion

            #region Buttons Panel 5 (Prefab)
            #region pushButton (Beheer sets)
            PushButton buttonManagePrefab = CreatePushButton(
                panel5,
                "Beheer Sets",
                "Beheer\nSets",
                "Lorem Ipsum",
                "Lorem Ipsum",
                "NijhofAddIn.Revit.Commands.Tools.Prefab.PrefabManager",
                null,
                iconSources["Manage32"],
                "http://www.autodesk.com"
            );
            buttonManagePrefab.Enabled = true;
            panel5.AddSeparator();
            #endregion

            #region pushButton (Nieuwe set)
            PushButton buttonNewPrefab = CreatePushButton(
                panel5,
                "Nieuwe Set",
                "Nieuwe\nSet",
                "Lorem Ipsum",
                "Lorem Ipsum",
                "NijhofAddIn.Revit.Commands.Tools.Prefab.PrefabCreator",
                null,
                iconSources["newPrefab32"],
                "http://www.autodesk.com"
            );
            buttonNewPrefab.Enabled = true;
            //panel5.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region pushButton (Toevoegen aan set)
            PushButton buttonAddPrefab = CreatePushButton(
                panel5,
                "Toevoegen",
                "Toevoegen\naan Set",
                "Lorem Ipsum",
                "Lorem Ipsum",
                "NijhofAddIn.Revit.Commands.Tools.Prefab.PrefabAdd",
                null,
                iconSources["addPrefab32"],
                "http://www.autodesk.com"
            );
            buttonAddPrefab.Enabled = true;
            //panel5.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region pushButton (Verwijderen uit set)
            PushButton buttonRemovePrefab = CreatePushButton(
                panel5,
                "Verwijderen",
                "Verwijderen\nuit Set",
                "Lorem Ipsum",
                "Lorem Ipsum",
                "NijhofAddIn.Revit.Commands.Tools.Prefab.PrefabRemove",
                null,
                iconSources["delPrefab32"],
                "http://www.autodesk.com"
            );
            buttonRemovePrefab.Enabled = true;
            //panel5.AddSeparator(); //Voegt een verticale lijn toe
            #endregion
            #endregion

            #region Buttons Panel 6 (View)
            #region pushButton (Refresh)
            PushButton buttonRefreshView = CreatePushButton(
                panel6,
                "Refresh",
                "Refresh",
                "Lorem Ipsum",
                "Lorem Ipsum",
                "NijhofAddIn.Revit.Commands.Tools.Views.RefreshView",
                null,
                iconSources["Refresh32"],
                "http://www.autodesk.com"
            );
            buttonRefreshView.Enabled = true;
            panel6.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region pushButton (Instant 3D Creator)
            PushButton buttonC3D = CreatePushButton(
                panel6,
                "Prefab 3D Creator",
                "Prefab\n3D creator",
                "Maakt van een viewport in een sheet een 3D view van het rioleringstelsel en zet deze op sheet",
                "Bij het maken van bijvoorbeeld Prefab hoef je alleen op de viewport te klikken en er wordt een 3D view" +
                "gemaakt met de view template: \"05_Plot_Prefab_Riool+Lucht_3D\". De 3D wordt gelijk op de sheet geplaatst. De sub-diciplines van de template is de" +
                "plek waar deze te vinden is in de project brouwser.",
                "NijhofAddIn.Revit.Commands.Tools.Views.Prefab3DCreator",
                null,
                iconSources["_3D32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Prefab#prefab-3d-creator"
            );
            buttonC3D.Enabled = true;
            //panel6.AddSeparator(); //Voegt een verticale lijn toe
            #endregion
            #endregion

            #region Buttons Panel 7 (Tag)
            #region pushButtondata (VWA Lengte)
            /// Knopgegevens instellen voor de eerste knop vwa
            PushButtonData btndataVWAtag25 = new PushButtonData(
                "VWA 2.5mm", /// De naam van de standaardactie
                "VWA Lengte 2.5mm", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag25" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["bruinTag16"]
            };

            btndataVWAtag25.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de tweede knop vwa
            PushButtonData btndataVWAtag35 = new PushButtonData(
                "VWA 3.5mm",
                "VWA Lengte 3.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag35" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["bruinTag16"]
            };

            btndataVWAtag35.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de derde knop vwa
            PushButtonData btndataVWAtag50 = new PushButtonData(
                "VWA 5.0mm",
                "VWA Lengte 5.0mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag50" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["bruinTag16"]
            };

            btndataVWAtag50.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de vierde knop vwa
            PushButtonData btndataVWAtag75 = new PushButtonData(
                "VWA 7.5mm",
                "VWA Lengte 7.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag75" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["bruinTag16"]
            };

            btndataVWAtag75.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de vijfde knop vwa
            PushButtonData btndataVWAtag100 = new PushButtonData(
                "VWA 10mm",
                "VWA Lengte 10mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag100" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["bruinTag16"]
            };

            btndataVWAtag100.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;
            #endregion

            #region pushButtondata (HWA Lengte)
            /// Knopgegevens instellen voor de eerste knop hwa
            PushButtonData btndataHWAtag25 = new PushButtonData(
                "HWA 2.5mm", /// De naam van de standaardactie
                "HWA Lengte 2.5mm", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag25" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["groenTag16"]
            };

            btndataHWAtag25.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de tweede knop hwa
            PushButtonData btndataHWAtag35 = new PushButtonData(
                "HWA 3.5mm",
                "HWA Lengte 3.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag35" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["groenTag16"]
            };

            btndataHWAtag35.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de derde knop hwa
            PushButtonData btndataHWAtag50 = new PushButtonData(
                "HWA 5.0mm",
                "HWA Lengte 5.0mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag50" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["groenTag16"]
            };

            btndataHWAtag50.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de vierde knop hwa
            PushButtonData btndataHWAtag75 = new PushButtonData(
                "HWA 7.5mm",
                "HWA Lengte 7.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag75" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["groenTag16"]
            };

            btndataHWAtag75.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de vijfde knop hwa
            PushButtonData btndataHWAtag100 = new PushButtonData(
                "HWA 10mm",
                "HWA Lengte 10mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.VWAtag100" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["groenTag16"]
            };

            btndataHWAtag100.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            #endregion

            #region pushButtondata (Lucht Lengte)
            /// Knopgegevens instellen voor de eerste knop mv
            PushButtonData btndataMVtag25 = new PushButtonData(
                "MV 2.5mm", /// De naam van de standaardactie
                "MV   Lengte 2.5mm", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.MVtag25" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["blauwTag16"]
            };

            btndataMVtag25.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de tweede knop mv
            PushButtonData btndataMVtag35 = new PushButtonData(
                "MV 3.5mm",
                "MV   Lengte 3.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.MVtag35" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["blauwTag16"]
            };

            btndataMVtag35.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de derde knop mv
            PushButtonData btndataMVtag50 = new PushButtonData(
                "MV 5.0mm",
                "MV   Lengte 5.0mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.MVtag50" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["blauwTag16"]
            };

            btndataMVtag50.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de vierde knop mv
            PushButtonData btndataMVtag75 = new PushButtonData(
                "MV 7.5mm",
                "MV   Lengte 7.5mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.MVtag75" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["blauwTag16"]
            };

            btndataMVtag75.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            /// Knopgegevens instellen voor de vijfde knop mv
            PushButtonData btndataMVtag100 = new PushButtonData(
                "MV 10mm",
                "MV   Lengte 10mm",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Tools.Tag.MVtag100" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Tag elementen in de viewport",
                LongDescription = "Klik op de knop, selecteer vervolgens de viewport die je getagd wil hebben en tadaa. Je zal waarschijnlijk de tags nog wel moeten verplaatsen.",
                Image = iconSources["blauwTag16"]
            };

            btndataMVtag100.AvailabilityClassName = typeof(SheetViewOnlyAvailability).FullName;

            #endregion

            #region Stacked splitButton Lengte
            /// 2. Maak SplitButtonData
            SplitButtonData SB_A = new SplitButtonData("SplitGroupVWA", "Split Group VWA");
            SplitButtonData SB_B = new SplitButtonData("SplitGroupHWA", "Split Group HWA");
            SplitButtonData SB_C = new SplitButtonData("SplitgroupMV", "Split Group MV");

            /// 3. Maak een "Stacked" indeling
            IList<RibbonItem> stackedItems = panel7.AddStackedItems(SB_A, SB_B, SB_C);

            /// 4. Maakt de knoppen "SplitButtons"
            SplitButton SBvwa_Button = stackedItems[0] as SplitButton;
            SplitButton SBhwa_Button = stackedItems[1] as SplitButton;
            SplitButton SBmv_Button = stackedItems[2] as SplitButton;
            #endregion

            #region pushButton VWA stack Lengte
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

            #region pushButton HWA stack Lengte
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

            #region pushButton MV stack Lengte
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

            //panel17.AddSeparator(); //Voegt een verticale lijn toe
            #endregion
            #endregion

            #region Buttons Panel 8 (Schedules)

            #endregion

            #region Buttons Panel 9 (Export)
            #region pushButton (Materiaal Export)
            PushButton buttonMateriaalExport = CreatePushButton(
                panel9,
                "Materiaallijst Exporteren",
                "Materiaal-\nlijst",
                "Exporteert materiaal gegevens",
                "Exporteert de materiaalgegevens naar een gespecificeerde locatie.",
                "NijhofAddIn.Revit.Commands.Tools.Export.ExportMateriaallijst",
                null,
                iconSources["Materiaal32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Export#materiaal-export"
            );
            buttonMateriaalExport.Enabled = true;
            // panel9.AddSeparator(); // Voegt een verticale lijn toe indien nodig
            #endregion

            #region pushButton (Zaaglijst Export)
            PushButton buttonZaaglijstExport2 = CreatePushButton(
                panel9,
                "Zaaglijst Export",
                "Zaaglijst",
                "Exporteert zaaglijst gegevens",
                "Exporteert de zaaglijstgegevens naar een gespecificeerde locatie.",
                "NijhofAddIn.Revit.Commands.Tools.Export.ExportZaaglijst",
                null,
                iconSources["Zaag32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Export#zaaglijst-export"
            );
            buttonZaaglijstExport2.Enabled = true;
            //panel9.AddSeparator(); // Voegt een verticale lijn toe indien nodig
            #endregion
            #endregion

            #region Buttons Panel 10 (Overig)
            #region pushButton (Help)
            PushButton buttonHelp = CreatePushButton(
                panel10,
                "Help",
                "Help",
                "Een 'Help' knop",
                "Wat meer wil je weten?",
                "NijhofAddIn.Revit.Commands.Tools.Overig.Help",
                null,
                iconSources["Vraag32"]
            );
            buttonHelp.Enabled = true;
            #endregion

            #region pushButton (Info)
            PushButton buttonInfo = CreatePushButton(
                panel10,
                "Info",
                "Info",
                "Geeft info weer",
                "Deze functie geeft de versie weer, dus wanneer deze tool voor het laatst is aangepast.",
                "NijhofAddIn.Revit.Commands.Tools.Overig.Info",
                null,
                iconSources["Info32"]
            );
            buttonInfo.Enabled = true;
            #endregion
            #endregion



            // Nijhof Elektra Buttons

            #region Buttons Panel 11 (Content)
            #region pushButton (Library)
            PushButton buttonLibrary2 = CreatePushButton(
                panel11,
                "Library",
                "Library",
                "Opent de 'Nijhof Bibliotheek'",
                "Een bibliotheek waar alle vaak gebruikte modellen, tags en groepen in gezet kunnen worden zodat die makkelijk te vinden en in te laden zijn",
                "NijhofAddIn.Revit.Commands.Tools.Content.FamilyLoader",
                null,
                iconSources["Library32"],
                "http://www.jouwinternehulplink.com"
            );
            buttonLibrary2.Enabled = true;
            //panel11.AddSeparator(); //Voegt een verticale lijn toe
            #endregion
            #endregion

            #region Buttons Panel 12 (Toevoegen)
            #region splitButton (WCD)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataWCD = new SplitButtonData(
                "Stopcontacten Plaatsen",
                "WCD")
            {
                LargeImage = iconSources["PlaceHolder32"]
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonWCD = panel12.AddItem(splitButtonDataWCD) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonWCD.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData WCDEnkelButtonData = new PushButtonData(
            "1v Plaatsen", /// De naam van de standaardactie
            "WCD:\n1 Voudig", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
            "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Stopcontact1v" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Plaats 1-voudig stopcontact",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                LargeImage = EleiconSources["WCDEnkel32"]
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton WCDEnkelButton = splitButtonWCD.AddPushButton(WCDEnkelButtonData);
            WCDEnkelButton.Enabled = true;

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataWCDDubbel = new PushButtonData(
                "2v Plaatsen",
                "WCD:\n2 Voudig",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Stopcontact2v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats 2-voudig stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["WCDDubbel32"]
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonWCDDubbel = splitButtonWCD.AddPushButton(btndataWCDDubbel);
            buttonWCDDubbel.Enabled = true;

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataWCDOpbouw = new PushButtonData(
                "2v Opbouw Plaatsen",
                "WCD:\n2v Opbouw",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Opbouw" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats 2-voudig opbouw stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["WCDOpbouw32"]
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonWCDOpbouw = splitButtonWCD.AddPushButton(btndataWCDOpbouw);
            buttonWCDOpbouw.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonWCD.AddSeparator();

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataWCDWater1v = new PushButtonData(
                "1v Spatwaterdicht Plaatsen",
                "WCD:\n1v Waterd.",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Spatwaterdicht1v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats 1v spatwaterdicht stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["WCDEnkelWater32"]
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonWCDWater1v = splitButtonWCD.AddPushButton(btndataWCDWater1v);
            buttonWCDWater1v.Enabled = true;

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndataWCDWater2v = new PushButtonData(
                "2v Spatwaterdicht Plaatsen",
                "WCD:\n2v Waterd.",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Spatwaterdicht2v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats 2v spatwaterdicht stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["WCDDubbelWater32"]
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton buttonWCDWater2v = splitButtonWCD.AddPushButton(btndataWCDWater2v);
            buttonWCDWater2v.Enabled = true;

            /// Knopgegevens instellen voor de zesde knop onder de dropdown
            PushButtonData btndataWCDPerilex = new PushButtonData(
                "Perilex Plaatsen",
                "WCD:\nPerilex",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Perilex" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats perilex stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["WCDPerilex32"]
            };

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonWCD.AddSeparator();

            /// Voeg de zesde knop toe aan de dropdown
            PushButton buttonWCDPerilex = splitButtonWCD.AddPushButton(btndataWCDPerilex);
            buttonWCDPerilex.Enabled = true;

            /// Knopgegevens instellen voor de zevende knop onder de dropdown
            PushButtonData btndataWCDKracht = new PushButtonData(
                "Krachtstroom Plaatsen",
                "WCD:\nKracht",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Krachtstroom" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats krachtstroom stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["WCDKracht32"]
            };

            /// Voeg de zevende knop toe aan de dropdown
            PushButton buttonWCDKracht = splitButtonWCD.AddPushButton(btndataWCDKracht);
            buttonWCDKracht.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonWCD.AddSeparator();

            /// Knopgegevens instellen voor de achtste knop onder de dropdown
            PushButtonData btndataWCDVloer = new PushButtonData(
                "Vloerstopcontact Plaatsen",
                "WCD:\nVloer",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Vloerstopcontact" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats vloerstopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["WCDVloer32"]
            };

            /// Voeg de achtste knop toe aan de dropdown
            PushButton buttonWCDVloer = splitButtonWCD.AddPushButton(btndataWCDVloer);
            buttonWCDVloer.Enabled = true;

            //panel2.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region splitButton (Aansluitpunt)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataAansluitpunt = new SplitButtonData(
                "Aansluitpunten Plaatsen",
                "Aansluitpunt")
            {
                LargeImage = EleiconSources["Enkel230v32"]
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonAansluitpunt = panel12.AddItem(splitButtonDataAansluitpunt) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonAansluitpunt.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData ASPBedraadButtonData = new PushButtonData(
            "Aansluitpunt Bedraad Plaatsen", /// De naam van de standaardactie
            "Aansluitpunt:\nBedraad", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
            "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.AansluitpuntBedraad" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Plaats een bedraad aansluitpunt",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                LargeImage = EleiconSources["Bedraad32"]
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton ASPBedraadButton = splitButtonAansluitpunt.AddPushButton(ASPBedraadButtonData);
            ASPBedraadButton.Enabled = true;

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataASPOnbedraad = new PushButtonData(
                "Aansluitpunt Onbedraad Plaatsen",
                "Aansluitpunt:\nOnbedraad",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.AansluitpuntOnbedraad" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een onbedraad aansluitpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Onbedraad32"]
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonASPOnbedraad = splitButtonAansluitpunt.AddPushButton(btndataASPOnbedraad);
            buttonASPOnbedraad.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonAansluitpunt.AddSeparator();

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndata230v = new PushButtonData(
                "Aansluitpunt 230v plaatsen",
                "Aansluitpunt:\n230v",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Enkel230v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een 230v aansluitpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Enkel230v32"]
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton button230v = splitButtonAansluitpunt.AddPushButton(btndata230v);
            button230v.Enabled = true;

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndata2x230v = new PushButtonData(
                "Aansluitpunt 2x 230v Plaatsen",
                "Aansluitpunt:\n2x 230v",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Dubbel230v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een 2x 230v aansluitpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Dubbel230v32"]
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton button2x230v = splitButtonAansluitpunt.AddPushButton(btndata2x230v);
            button2x230v.Enabled = true;

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndata400v = new PushButtonData(
                "400v Plaatsen",
                "Aansluitpunt:\n400v",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Enkel400v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een 400v aansluitpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Enkel400v32"]
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton button400v = splitButtonAansluitpunt.AddPushButton(btndata400v);
            button400v.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonAansluitpunt.AddSeparator();

            /// Knopgegevens instellen voor de zesde knop onder de dropdown
            PushButtonData btndataAardepunt = new PushButtonData(
                "Centraal Aardepunt Plaatsen",
                "Aansluitpunt:\nCAP",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.CAP" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een Centraal Aardepunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["CAP32"]
            };

            /// Voeg de zesde knop toe aan de dropdown
            PushButton buttonAardepunt = splitButtonAansluitpunt.AddPushButton(btndataAardepunt);
            buttonAardepunt.Enabled = true;

            //panel2.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region splitButton (Data)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataData = new SplitButtonData(
                "Data Plaatsen",
                "Data")
            {
                LargeImage = EleiconSources["EnkelData32"]
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonData = panel12.AddItem(splitButtonDataData) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonData.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData btndataEnkelData = new PushButtonData(
                "Enkel Data Plaatsen",
                "Data:\nEnkel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.EnkelData" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een eenvoudig data punt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["EnkelData32"]
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton EnkelDataButton = splitButtonData.AddPushButton(btndataEnkelData);
            EnkelDataButton.Enabled = true;

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataDubbelData = new PushButtonData(
                "Dubbele Data Plaatsen",
                "Data:\nDubbel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.DubbelData" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een dubbel data punt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["DubbelData32"]
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonDubbelData = splitButtonData.AddPushButton(btndataDubbelData);
            buttonDubbelData.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonData.AddSeparator();

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData DataBekabeldButtonData = new PushButtonData(
                "Data Bekabeld Plaatsen", /// De naam van de standaardactie
                "Data:\nBekabeld", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.DataBekabeld" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Plaats een bekabeld data punt",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                LargeImage = EleiconSources["Bedraad32"]
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonDataBekabeld = splitButtonData.AddPushButton(DataBekabeldButtonData);
            buttonDataBekabeld.Enabled = true;

            panel12.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region splitButton (Schakelaar)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataSchakelaar = new SplitButtonData(
                "Schakelaar Plaatsen",
                "Schakelaar")
            {
                LargeImage = EleiconSources["Enkelpolig32"]
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonSchakelaar = panel12.AddItem(splitButtonDataSchakelaar) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonSchakelaar.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData btndataSchakelaarEnkelpolig = new PushButtonData(
                "Enkelpolige Schakelaar", /// De naam van de standaardactie
                "Schakelaar:\nEnkelpolig", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarEnkelpolig" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Plaats een enkelpolige schakelaar",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                LargeImage = EleiconSources["Enkelpolig32"]
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton buttonSchakelaarEnkelpolig = splitButtonSchakelaar.AddPushButton(btndataSchakelaarEnkelpolig);
            buttonSchakelaarEnkelpolig.Enabled = true;

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataSchakelaarDubbelpolig = new PushButtonData(
                "Dubbelpolige Schakelaar",
                "Schakelaar:\nDubbelpolig",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarDubbelpolig" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een dubbelpolige schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Dubbelpolig32"]
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonSchakelaarDubbelpolig = splitButtonSchakelaar.AddPushButton(btndataSchakelaarDubbelpolig);
            buttonSchakelaarDubbelpolig.Enabled = true;

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataSchakelaarVierpolig = new PushButtonData(
                "Vierpolige Schakelaar",
                "Schakelaar:\nVierpolig",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarVierpolig" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een vierpolige schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = iconSources["PlaceHolder32"]
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonSchakelaarVierpolig = splitButtonSchakelaar.AddPushButton(btndataSchakelaarVierpolig);
            buttonSchakelaarVierpolig.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonSchakelaar.AddSeparator();

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataSchakelaarWissel = new PushButtonData(
                "Wissel Schakelaar",
                "Schakelaar:\nWissel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarWissel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een wissel schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Wissel32"]
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonSchakelaarWissel = splitButtonSchakelaar.AddPushButton(btndataSchakelaarWissel);
            buttonSchakelaarWissel.Enabled = true;

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndataSchakelaarDubbelWissel = new PushButtonData(
                "Dubbelpolige Wisselschakelaar",
                "Schakelaar:\nWissel\nDubbelp.",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarDubbelWissel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een dubbelpolige wisselschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["DubbelWissel32"]
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton buttonSchakelaarDubbelWissel = splitButtonSchakelaar.AddPushButton(btndataSchakelaarDubbelWissel);
            buttonSchakelaarDubbelWissel.Enabled = true;

            /// Knopgegevens instellen voor de zesde knop onder de dropdown
            PushButtonData btndataSchakelaar2xWissel = new PushButtonData(
                "2x Wisselschakelaar",
                "Schakelaar:\n2x Wissel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Schakelaar2xWissel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een 2x wisselschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Wissel2x32"]
            };

            /// Voeg de zesde knop toe aan de dropdown
            PushButton buttonSchakelaar2xWissel = splitButtonSchakelaar.AddPushButton(btndataSchakelaar2xWissel);
            buttonSchakelaar2xWissel.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonSchakelaar.AddSeparator();

            /// Knopgegevens instellen voor de zevende knop onder de dropdown
            PushButtonData btndataSchakelaarSerie = new PushButtonData(
                "Seriechakelaar",
                "Schakelaar:\nSerie",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarSerie" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een serieschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Serie32"]
            };

            /// Voeg de zevende knop toe aan de dropdown
            PushButton buttonSchakelaarSerie = splitButtonSchakelaar.AddPushButton(btndataSchakelaarSerie);
            buttonSchakelaarSerie.Enabled = true;

            /// Knopgegevens instellen voor de achtste knop onder de dropdown
            PushButtonData btndataSchakelaarKruis = new PushButtonData(
                "Kruisschakelaar",
                "Schakelaar:\nKruis",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarKruis" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een kruisschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Kruis32"]
            };

            /// Voeg de achtste knop toe aan de dropdown
            PushButton buttonSchakelaarKruis = splitButtonSchakelaar.AddPushButton(btndataSchakelaarKruis);
            buttonSchakelaarKruis.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonSchakelaar.AddSeparator();

            /// Knopgegevens instellen voor de negende knop onder de dropdown
            PushButtonData btndataSchakelaarDimmer = new PushButtonData(
                "Leddimmer Schakelaar",
                "Schakelaar:\nDimmer",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarDimmer" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een leddimmer schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Dimmer32"]
            };

            /// Voeg de negende knop toe aan de dropdown
            PushButton buttonSchakelaarDimmer = splitButtonSchakelaar.AddPushButton(btndataSchakelaarDimmer);
            buttonSchakelaarDimmer.Enabled = true;

            /// Knopgegevens instellen voor de tiende knop onder de dropdown
            PushButtonData btndataSchakelaarDimmerWissel = new PushButtonData(
                "Dimmer Wisselschakelaar",
                "Schakelaar:\nDimmer\nWissel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarDimmerWissel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een dimmer wisselschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["WisselDimmer32"]
            };

            /// Voeg de tiende knop toe aan de dropdown
            PushButton buttonSchakelaarDimmerWissel = splitButtonSchakelaar.AddPushButton(btndataSchakelaarDimmerWissel);
            buttonSchakelaarDimmerWissel.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonSchakelaar.AddSeparator();

            /// Knopgegevens instellen voor de elfde knop onder de dropdown
            PushButtonData btndataSchakelaarJaloezie = new PushButtonData(
                "Jaloezie Schakelaar",
                "Schakelaar:\nJaloezie",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarJaloezie" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een jaloezie schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Jaloezie32"]
            };

            /// Voeg de elfde knop toe aan de dropdown
            PushButton buttonSchakelaarJaloezie = splitButtonSchakelaar.AddPushButton(btndataSchakelaarJaloezie);
            buttonSchakelaarJaloezie.Enabled = true;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonSchakelaar.AddSeparator();

            /// Knopgegevens instellen voor de twaalfde knop onder de dropdown
            PushButtonData btndataSchakelaarBewegingWand = new PushButtonData(
                "Wand Bewegingsmelder Schakelaar",
                "Schakelaar:\nBeweging\nWand",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarBewegingWand" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een wand bewegingsmelder",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = iconSources["PlaceHolder32"],
            };

            /// Voeg de twaalfde knop toe aan de dropdown
            PushButton buttonSchakelaarBewegingWand = splitButtonSchakelaar.AddPushButton(btndataSchakelaarBewegingWand);
            buttonSchakelaarBewegingWand.Enabled = false;

            /// Knopgegevens instellen voor de dertiende knop onder de dropdown
            PushButtonData btndataSchakelaarBewegingPlafond = new PushButtonData(
                "Plafond Beweging Schakelaar",
                "Schakelaar:\nBeweging\nPlafond",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarBewegingPlafond" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een plafond bewegingsmelder",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = iconSources["PlaceHolder32"],
            };

            /// Voeg de dertiende knop toe aan de dropdown
            PushButton buttonSchakelaarBewegingPlafond = splitButtonSchakelaar.AddPushButton(btndataSchakelaarBewegingPlafond);
            buttonSchakelaarBewegingPlafond.Enabled = false;

            /// Knopgegevens instellen voor de veertiende knop onder de dropdown
            PushButtonData btndataSchakelaarSchemer = new PushButtonData(
                "Schemerschakelaar",
                "Schakelaar:\nSchemer",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.SchakelaarSchemer" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een schemerschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = iconSources["PlaceHolder32"],
            };

            /// Voeg de veertiende knop toe aan de dropdown
            PushButton buttonSchakelaarSchemer = splitButtonSchakelaar.AddPushButton(btndataSchakelaarSchemer);
            buttonSchakelaarSchemer.Enabled = false;

            ////panel2.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region splitButton (Verlichting)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataVerlichting = new SplitButtonData(
                "Verlichting Plaatsen",
                "Verlichting")
            {
                LargeImage = EleiconSources["Centraaldoos32"]
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonVerlichting = panel12.AddItem(splitButtonDataVerlichting) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonVerlichting.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData btndataCentraaldoos = new PushButtonData(
            "Centraaldoos Plaatsen", /// De naam van de standaardactie
            "Verlichting:\nCentraaldoos", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
            "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Centraaldoos" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Plaats een centraaldoos",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                LargeImage = EleiconSources["Centraaldoos32"]
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton buttonCentraaldoos = splitButtonVerlichting.AddPushButton(btndataCentraaldoos);
            buttonCentraaldoos.Enabled = true;

            /// Voegt een horizontale lijn toe onder de eerste knop
            //splitButtonGPSload.AddSeparator();

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataLichtPlafond = new PushButtonData(
                "Plafond Lichtpunt Plaatsen",
                "Verlichting:\nPlafond",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.LichtPlafond" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een plafond lichtpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["LichtPlafond32"]
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonLichtPlafond = splitButtonVerlichting.AddPushButton(btndataLichtPlafond);
            buttonLichtPlafond.Enabled = true;

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataInbouwspot = new PushButtonData(
                "Inbouwspot Plaatsen",
                "Verlichting:\nSpot",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Inbouwspot" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een inbouwspot",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Inbouwspot32"]
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonInbouwspot = splitButtonVerlichting.AddPushButton(btndataInbouwspot);
            buttonInbouwspot.Enabled = true;

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataLichtWand = new PushButtonData(
                "Wand Lichtpunt Plaatsen",
                "Verlichting:\nWand",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.LichtWand" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een wandlichtpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["LichtWand32"]
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonLichtWand = splitButtonVerlichting.AddPushButton(btndataLichtWand);
            buttonLichtWand.Enabled = true;

            panel12.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region splitButton (Overig)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataOverig = new SplitButtonData(
                "Overig Plaatsen",
                "Overig")
            {
                LargeImage = EleiconSources["Rookmelder32"]
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonOverig = panel12.AddItem(splitButtonDataOverig) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonOverig.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData btndataRookmelder = new PushButtonData(
                "Rookmelder Plaatsen",
                "Overig:\nRookmelder",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Rookmelder" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een rookmelder",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Rookmelder32"]
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton buttonRookmelder = splitButtonOverig.AddPushButton(btndataRookmelder);
            buttonRookmelder.Enabled = true;

                /// Knopgegevens instellen voor de tweede knop onder de dropdown
                PushButtonData btndataBedieningLos = new PushButtonData(
                "Bediening Los Plaatsen", /// De naam van de standaardactie
                "Overig:\nBediening", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.BedieningLos" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Plaats bediening los",
                    //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                    LargeImage = EleiconSources["Bediening32"]
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonBedieningLos = splitButtonOverig.AddPushButton(btndataBedieningLos);
            buttonBedieningLos.Enabled = true;

            /// Voegt een horizontale lijn toe onder de eerste knop
            splitButtonOverig.AddSeparator();

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataDrukknopbel = new PushButtonData(
                "Drukknop Bel Plaatsen",
                "Overig:\nBel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Drukknopbel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een drukknop voor de bel",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Deurbel32"]
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonDrukknopbel = splitButtonOverig.AddPushButton(btndataDrukknopbel);
            buttonDrukknopbel.Enabled = true;

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataSchel = new PushButtonData(
                "Schel Plaatsen",
                "Overig:\nSchel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Schel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een schel",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Dingdong32"]
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonSchel = splitButtonOverig.AddPushButton(btndataSchel);
            buttonSchel.Enabled = true;

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndataIntercom = new PushButtonData(
                "Intercom Plaatsen",
                "Overig:\nIntercom",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Intercom" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een intercom",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Intercom32"]
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton buttonIntercom = splitButtonOverig.AddPushButton(btndataIntercom);
            buttonIntercom.Enabled = true;

            /// Voegt een horizontale lijn toe onder de eerste knop
            splitButtonOverig.AddSeparator();

            /// Knopgegevens instellen voor de zesde knop onder de dropdown
            PushButtonData btndataGrondkabel = new PushButtonData(
                "Grondkabel Plaatsen",
                "Overig:\nGrondkabel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Toevoegen.Grondkabel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een grondkabel",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                LargeImage = EleiconSources["Grondkabel32"]
            };

            /// Voeg de zesde knop toe aan de dropdown
            PushButton buttonGrondkabel = splitButtonOverig.AddPushButton(btndataGrondkabel);
            buttonGrondkabel.Enabled = true;

            //panel2.AddSeparator(); //Voegt een verticale lijn toe
            #endregion
            #endregion

            #region Buttons Panel 13 (Tag)
            #region pushButton (tagGroepNummer)
            PushButton buttonTagGN = CreatePushButton(
                panel13,
                "Tag Groepnummer",
                "Groep-\nnummer",
                "Tagged alle Elektra elementen met een ingevulde Groepnummer",
                "Deze funtie tagged alle 'Electrical Fixtures', 'Lighting Devices' etc. in de huidige view waarbij de parameter 'Groep- (nummer)' is ingevuld. Als de 'Groep- (nummer)' niet is ingevuld tagged die deze dus niet.",
                "NijhofAddIn.Revit.Commands.Elektra.Tag.GroepTag",
                null,
                EleiconSources["GroepTag32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Elektra#tag-groepnummer"
            );

            buttonTagGN.Enabled = true;
            #endregion

            #region pushButton (tagSwitchCode)
            PushButton buttonTagSC = CreatePushButton(
                panel13,
                "Tag Switchcodes",
                "Switch-\ncodes",
                "Tagged alle Elektra elementen met een ingevulde Switchcode",
                "Deze funtie tagged alle 'Electrical Fixtures', 'Lighting Devices' etc. in de huidige view waarbij de parameter 'Switchcode' is ingevuld. Als de 'Switchcode' niet is ingevuld tagged die deze dus niet.",
                "NijhofAddIn.Revit.Commands.Elektra.Tag.SwitchcodeTag",
                null,
                EleiconSources["SwitchTag32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Elektra#tag-switchcodes"
            );

            buttonTagSC.Enabled = true;
            #endregion
            #endregion

            #region Buttons Panel 14 (Overig)
            #region pushButton (SwitchCodes)
            PushButton buttonSwitchCode = CreatePushButton(
                panel14,
                "Switch Codes",
                "Code\nLijst",
                "Laat alle Switchcodes zien die in het project gebruikt zijn",
                "Deze funtie kijkt naar de ingevulde parameters 'Switch code', 'Switch Code', 'Switch code 1' en 'Switch code 2' en verwijderd dubbelen. Deze worden in een lijst gezet en getoond in een pop-up.",
                "NijhofAddIn.Revit.Commands.Elektra.Overig.SwitchcodeList",
                null,
                EleiconSources["SwitchCode32"],
                "https://github.com/Damianmts/NijhofAddIn/wiki/Elektra#switchcodes"
            );

            buttonSwitchCode.Enabled = true;
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