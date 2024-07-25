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
        //Nijhof Tools
        const string RIBBON_TAB1 = "Nijhof Tools";
        const string RIBBON_PANEL1 = "Prefab";
        const string RIBBON_PANEL2 = "GPS Punten";
        const string RIBBON_PANEL3 = "Sparingen";
        const string RIBBON_PANEL4 = "Wijzigen";
        const string RIBBON_PANEL5 = "Export";
        const string RIBBON_PANEL7 = "Overig";
        const string RIBBON_PANEL8 = "Content";
        const string RIBBON_PANEL9 = "Berekenen";

        // Nijhof Elektra
        const string RIBBON_TAB2 = "Nijhof Elektra";
        const string RIBBON_PANEL10 = "Toevoegen";
        const string RIBBON_PANEL6 = "Tag";
        const string RIBBON_PANEL11 = "Overig";

        public Result OnStartup(UIControlledApplication app)
        {
            //Nijhof Tools

            #region Tabblad Tools
            /// Maakt Ribbon Tabblad aan
            try
            {
                app.CreateRibbonTab(RIBBON_TAB1);
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                //tab already exists
            }
            #endregion

            #region Panel (Content)
            /// Maakt Panel (Content) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel8 = null;
            List<RibbonPanel> panels8 = app.GetRibbonPanels(RIBBON_TAB1);
            foreach (RibbonPanel pnl in panels8)
            {
                if (pnl.Name == RIBBON_PANEL8)
                {
                    panel8 = pnl;
                    break;
                }
            }
            /// Bestaat panel niet? Maak panel aan
            if (panel8 == null)
            {
                panel8 = app.CreateRibbonPanel(RIBBON_TAB1, RIBBON_PANEL8);
            }
            #endregion

            #region Panel (Prefab)
            /// Maakt Panel (Prefab) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel1 = null;
            List<RibbonPanel> panels1 = app.GetRibbonPanels(RIBBON_TAB1);
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
                panel1 = app.CreateRibbonPanel(RIBBON_TAB1, RIBBON_PANEL1);
            }
            #endregion

            #region Panel (Berekenen)
            /// Maakt Panel (Berekenen) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel9 = null;
            List<RibbonPanel> panels9 = app.GetRibbonPanels(RIBBON_TAB1);
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
                panel9 = app.CreateRibbonPanel(RIBBON_TAB1, RIBBON_PANEL9);
            }
            #endregion

            #region Panel (GPS Punten)
            /// Maakt Panel (GPS Punten) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel2 = null;
            List<RibbonPanel> panels2 = app.GetRibbonPanels(RIBBON_TAB1);
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
                panel2 = app.CreateRibbonPanel(RIBBON_TAB1, RIBBON_PANEL2);
            }
            #endregion

            #region Panel (Sparingen)
            /// Maakt Panel (Sparingen) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel3 = null;
            List<RibbonPanel> panels3 = app.GetRibbonPanels(RIBBON_TAB1);
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
                panel3 = app.CreateRibbonPanel(RIBBON_TAB1, RIBBON_PANEL3);
            }
            #endregion

            #region Panel (Wijzigen)
            /// Maakt Panel (Wijzigen) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel4 = null;
            List<RibbonPanel> panels4 = app.GetRibbonPanels(RIBBON_TAB1);
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
                panel4 = app.CreateRibbonPanel(RIBBON_TAB1, RIBBON_PANEL4);
            }
            #endregion

            #region Panel (Export)
            /// Maakt Panel (Export) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel5 = null;
            List<RibbonPanel> panels5 = app.GetRibbonPanels(RIBBON_TAB1);
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
                panel5 = app.CreateRibbonPanel(RIBBON_TAB1, RIBBON_PANEL5);
            }
            #endregion

            #region Panel (Overig)
            /// Maakt Panel (Overig) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel7 = null;
            List<RibbonPanel> panels7 = app.GetRibbonPanels(RIBBON_TAB1);
            foreach (RibbonPanel pnl in panels7)
            {
                if (pnl.Name == RIBBON_PANEL7)
                {
                    panel7 = pnl;
                    break;
                }
            }
            /// Bestaat panel7 niet? Maak panel7 aan
            if (panel7 == null)
            {
                panel7 = app.CreateRibbonPanel(RIBBON_TAB1, RIBBON_PANEL7);
            }
            #endregion

            //Nijhof Elektra

            #region Tabblad Elektra
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

            #region Panel (Toevoegen)
            /// Maakt Panel (Content) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel10 = null;
            List<RibbonPanel> panels10 = app.GetRibbonPanels(RIBBON_TAB2);
            foreach (RibbonPanel pnl in panels8)
            {
                if (pnl.Name == RIBBON_PANEL10)
                {
                    panel10 = pnl;
                    break;
                }
            }
            /// Bestaat panel niet? Maak panel aan
            if (panel10 == null)
            {
                panel10 = app.CreateRibbonPanel(RIBBON_TAB2, RIBBON_PANEL10);
            }
            #endregion

            #region Panel (Tag)
            /// Maakt Panel (Elektra) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel6 = null;
            List<RibbonPanel> panels6 = app.GetRibbonPanels(RIBBON_TAB2);
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
                panel6 = app.CreateRibbonPanel(RIBBON_TAB2, RIBBON_PANEL6);
            }
            #endregion

            #region Panel (Overig)
            /// Maakt Panel (Overig) aan /// Herhaalbaar voor elke nieuwe panel
            RibbonPanel panel11 = null;
            List<RibbonPanel> panels11 = app.GetRibbonPanels(RIBBON_TAB2);
            foreach (RibbonPanel pnl in panels11)
            {
                if (pnl.Name == RIBBON_PANEL11)
                {
                    panel11 = pnl;
                    break;
                }
            }
            /// Bestaat panel niet? Maak panel aan
            if (panel11 == null)
            {
                panel11 = app.CreateRibbonPanel(RIBBON_TAB2, RIBBON_PANEL11);
            }
            #endregion

            //Nijhof Tools

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

            Image GPSRiool32 = Icons.gpsrioollucht_32;
            ImageSource GPSRiool32Scr = appBase.GetImageSource(GPSRiool32);

            Image GPSKoudwater32 = Icons.gpskoudwater_32;
            ImageSource GPSKoudwater32Scr = appBase.GetImageSource(GPSKoudwater32);

            Image GPSWarmwater32 = Icons.gpswarmwater_32;
            ImageSource GPSWarmwater32Scr = appBase.GetImageSource(GPSWarmwater32);

            Image GPSElektra32 = Icons.gpselektra_32;
            ImageSource GPSElektra32Scr = appBase.GetImageSource(GPSElektra32);

            Image GPSMeterkast32 = Icons.gpsmeterkast_32;
            ImageSource GPSMeterkast32Scr = appBase.GetImageSource(GPSMeterkast32);

            Image GPSIntercom32 = Icons.gpsintercom_32;
            ImageSource GPSIntercom32Scr = appBase.GetImageSource(GPSIntercom32);

            Image GPSimport16 = Icons.gpsimport_16;
            ImageSource GPSimport16Scr = appBase.GetImageSource(GPSimport16);

            Image GPSimport32 = Icons.gpsinladen_32;
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

            //Nijhof Elektra

            #region Icons
            //Stopcontacten
            Image WCDEnkel32 = Icons.WCDEnkel_32;
            ImageSource WCDEnkel32Scr = appBase.GetImageSource(WCDEnkel32);

            Image WCDDubbel32 = Icons.WCDDubbel_32;
            ImageSource WCDDubbel32Scr = appBase.GetImageSource(WCDDubbel32);

            Image WCDOpbouw32 = Icons.WCDOpbouw_32;
            ImageSource WCDOpbouw32Scr = appBase.GetImageSource(WCDOpbouw32);

            Image WCDEnkelWater32 = Icons.WCDWater1v_32;
            ImageSource WCDEnkelWater32Scr = appBase.GetImageSource(WCDEnkelWater32);

            Image WCDDubbelWater32 = Icons.WCDWater2v_32;
            ImageSource WCDDubbelWater32Scr = appBase.GetImageSource(WCDDubbelWater32);

            Image WCDPerilex32 = Icons.WCDPerilex_32;
            ImageSource WCDPerilex32Scr = appBase.GetImageSource(WCDPerilex32);

            Image WCDKracht32 = Icons.WCDKracht_32;
            ImageSource WCDKracht32Scr = appBase.GetImageSource(WCDKracht32);

            Image WCDVloer32 = Icons.WCDVloer_32;
            ImageSource WCDVloer32Scr = appBase.GetImageSource(WCDVloer32);

            //Aansluitpunten
            Image Bedraad32 = Icons.Bedraad_32;
            ImageSource Bedraad32Scr = appBase.GetImageSource(Bedraad32);

            Image Onbedraad32 = Icons.Onbedraad_32;
            ImageSource Onbedraad32Scr = appBase.GetImageSource(Onbedraad32);

            Image Enkel230v32 = Icons.Enkel230v_32;
            ImageSource Enkel230v32Scr = appBase.GetImageSource(Enkel230v32);

            Image Dubbel230v32 = Icons.Dubbel230v_32;
            ImageSource Dubbel230v32Scr = appBase.GetImageSource(Dubbel230v32);

            Image Enkel400v32 = Icons.Enkel400v_32;
            ImageSource Enkel400v32Scr = appBase.GetImageSource(Enkel400v32);

            Image CAP32 = Icons.CAP_32;
            ImageSource CAP32Scr = appBase.GetImageSource(CAP32);

            //Data
            Image EnkelData32 = Icons.EnkelData_32;
            ImageSource EnkelData32Scr = appBase.GetImageSource(EnkelData32);

            Image DubbelData32 = Icons.DubbelData_32;
            ImageSource DubbelData32Scr = appBase.GetImageSource(DubbelData32);

            //Schakelaars
            Image Enkelpolig32 = Icons.enkelpolig_32;
            ImageSource Enkelpolig32Scr = appBase.GetImageSource(Enkelpolig32);

            Image Dubbelpolig32 = Icons.dubbelpolig_32;
            ImageSource Dubbelpolig32Scr = appBase.GetImageSource(Dubbelpolig32);

            //Image Vierpolig32 = Icons.EnkelData_32;
            //ImageSource Vierpolig32Scr = appBase.GetImageSource(Vierpolig32);

            Image Wissel32 = Icons.wissel_32;
            ImageSource Wissel32Scr = appBase.GetImageSource(Wissel32);

            Image DubbelWissel32 = Icons.dubbelwissel_32;
            ImageSource DubbelWissel32Scr = appBase.GetImageSource(DubbelWissel32);

            Image Wissel2x32 = Icons.wissel2x_32;
            ImageSource Wissel2x32Scr = appBase.GetImageSource(Wissel2x32);

            Image Serie32 = Icons.serie_32;
            ImageSource Serie32Scr = appBase.GetImageSource(Serie32);

            Image Kruis32 = Icons.kruis_32;
            ImageSource Kruis32Scr = appBase.GetImageSource(Kruis32);

            Image Dimmer32 = Icons.dimmer_32;
            ImageSource Dimmer32Scr = appBase.GetImageSource(Dimmer32);

            Image WisselDimmer32 = Icons.wisseldimmer_32;
            ImageSource WisselDimmer32Scr = appBase.GetImageSource(WisselDimmer32);

            Image Jaloezie32 = Icons.jaloezie_32;
            ImageSource Jaloezie32Scr = appBase.GetImageSource(Jaloezie32);

            //Image BewegingWand32 = Icons.EnkelData_32;
            //ImageSource Vierpolig32Scr = appBase.GetImageSource(Vierpolig32);

            //Image BewegingPlafond32 = Icons.EnkelData_32;
            //ImageSource Vierpolig32Scr = appBase.GetImageSource(Vierpolig32);

            //Image Schemer32 = Icons.EnkelData_32;
            //ImageSource Vierpolig32Scr = appBase.GetImageSource(Vierpolig32);

            //Verlichting
            Image Centraaldoos32 = Icons.centraaldoos_32;
            ImageSource Centraaldoos32Scr = appBase.GetImageSource(Centraaldoos32);

            Image LichtPlafond32 = Icons.plafondlicht_32;
            ImageSource LichtPlafond32Scr = appBase.GetImageSource(LichtPlafond32);

            Image Inbouwspot32 = Icons.inbouwspot_32;
            ImageSource Inbouwspot32Scr = appBase.GetImageSource(Inbouwspot32);

            Image LichtWand32 = Icons.wandlicht_32;
            ImageSource LichtWand32Scr = appBase.GetImageSource(LichtWand32);

            //Overig
            Image Bediening32 = Icons.bbediening_32;
            ImageSource Bediening32Scr = appBase.GetImageSource(Bediening32);

            Image Rookmelder32 = Icons.rookmelder_32;
            ImageSource Rookmelder32Scr = appBase.GetImageSource(Rookmelder32);

            Image Deurbel32 = Icons.deurbel_32;
            ImageSource Deurbel32Scr = appBase.GetImageSource(Deurbel32);

            Image Dingdong32 = Icons.dingdong_32;
            ImageSource Dingdong32Scr = appBase.GetImageSource(Dingdong32);

            Image Intercom32 = Icons.intercom_32;
            ImageSource Intercom32Scr = appBase.GetImageSource(Intercom32);

            Image Grondkabel32 = Icons.grondkabel_32;
            ImageSource Grondkabel32Scr = appBase.GetImageSource(Grondkabel32);

            #endregion

            //Nijhof Tools

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
            "Inladen", /// Tooltip voor de standaardactie
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
                LargeImage = GPSRiool32Scr,
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
                LargeImage = GPSRiool32Scr,
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
                LargeImage = GPSKoudwater32Scr,
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
                LargeImage = GPSWarmwater32Scr,
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
                LargeImage = GPSElektra32Scr,
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
                LargeImage = GPSMeterkast32Scr,
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
                LargeImage = GPSIntercom32Scr,
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
                "Toevoegen", /// Tooltip voor de standaardactie
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
                LargeImage = GPSRiool32Scr,
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
                LargeImage = GPSRiool32Scr,
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
                LargeImage = GPSKoudwater32Scr,
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
                LargeImage = GPSWarmwater32Scr,
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
                "Verwijderen", /// Tooltip voor de standaardactie
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
            #region pushButton (Materiaal Export)
            /// Knopgegevens instellen voor "Materiaal Export"
            PushButtonData btndataMateriaalExport = new PushButtonData
                (
                "Materiaallijst Exporteren",
                "Materiaal-\nlijst",
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
                "Zaaglijst",
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

            //Nijhof Elektra

            #region Buttons Panel (Toevoegen)
            #region splitButton (WCD)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataWCD = new SplitButtonData(
                "Stopcontacten Plaatsen",
                "WCD")
            {
                Image = PlaceHolder16Scr,
                LargeImage = PlaceHolder32Scr,
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonWCD = panel10.AddItem(splitButtonDataWCD) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonWCD.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData WCDEnkelButtonData = new PushButtonData(
            "1v Plaatsen", /// De naam van de standaardactie
            "WCD:\n1 Voudig", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
            "NijhofAddIn.Revit.Commands.Elektra.Stopcontact1v" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Plaats 1-voudig stopcontact",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                //Image = PlaceHolder16Scr,
                LargeImage = WCDEnkel32Scr,
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton WCDEnkelButton = splitButtonWCD.AddPushButton(WCDEnkelButtonData);
            WCDEnkelButton.Enabled = true;

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataWCDDubbel = new PushButtonData(
                "2v Plaatsen",
                "WCD:\n2 Voudig",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Stopcontact2v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats 2-voudig stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = WCDDubbel32Scr,
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonWCDDubbel = splitButtonWCD.AddPushButton(btndataWCDDubbel);
            buttonWCDDubbel.Enabled = true;

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataWCDOpbouw = new PushButtonData(
                "2v Opbouw Plaatsen",
                "WCD:\n2v Opbouw",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Opbouw" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats 2-voudig opbouw stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = WCDOpbouw32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.Spatwaterdicht1v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats 1v spatwaterdicht stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = WCDEnkelWater32Scr,
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonWCDWater1v = splitButtonWCD.AddPushButton(btndataWCDWater1v);
            buttonWCDWater1v.Enabled = true;

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndataWCDWater2v = new PushButtonData(
                "2v Spatwaterdicht Plaatsen",
                "WCD:\n2v Waterd.",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Spatwaterdicht2v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats 2v spatwaterdicht stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = WCDDubbelWater32Scr,
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton buttonWCDWater2v = splitButtonWCD.AddPushButton(btndataWCDWater2v);
            buttonWCDWater2v.Enabled = true;

            /// Knopgegevens instellen voor de zesde knop onder de dropdown
            PushButtonData btndataWCDPerilex = new PushButtonData(
                "Perilex Plaatsen",
                "WCD:\nPerilex",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Perilex" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats perilex stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = WCDPerilex32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.Krachtstroom" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats krachtstroom stopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = WCDKracht32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.Vloerstopcontact" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats vloerstopcontact",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = WCDVloer32Scr,
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
                //Image = PlaceHolder16Scr,
                LargeImage = Enkel230v32Scr,
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonAansluitpunt = panel10.AddItem(splitButtonDataAansluitpunt) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonAansluitpunt.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData ASPBedraadButtonData = new PushButtonData(
            "Aansluitpunt Bedraad Plaatsen", /// De naam van de standaardactie
            "Aansluitpunt:\nBedraad", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
            "NijhofAddIn.Revit.Commands.Elektra.AansluitpuntBedraad" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Plaats een bedraad aansluitpunt",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                //Image = PlaceHolder16Scr,
                LargeImage = Bedraad32Scr,
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton ASPBedraadButton = splitButtonAansluitpunt.AddPushButton(ASPBedraadButtonData);
            ASPBedraadButton.Enabled = true;

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataASPOnbedraad = new PushButtonData(
                "Aansluitpunt Onbedraad Plaatsen",
                "Aansluitpunt:\nOnbedraad",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.AansluitpuntOnbedraad" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een onbedraad aansluitpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Onbedraad32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.Enkel230v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een 230v aansluitpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Enkel230v32Scr,
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton button230v = splitButtonAansluitpunt.AddPushButton(btndata230v);
            button230v.Enabled = true;

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndata2x230v = new PushButtonData(
                "Aansluitpunt 2x 230v Plaatsen",
                "Aansluitpunt:\n2x 230v",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Dubbel230v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een 2x 230v aansluitpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Dubbel230v32Scr,
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton button2x230v = splitButtonAansluitpunt.AddPushButton(btndata2x230v);
            button2x230v.Enabled = true;

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndata400v = new PushButtonData(
                "400v Plaatsen",
                "Aansluitpunt:\n400v",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Enkel400v" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een 400v aansluitpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Enkel400v32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.CAP" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een Centraal Aardepunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = CAP32Scr,
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
                //Image = PlaceHolder16Scr,
                LargeImage = EnkelData32Scr,
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonData = panel10.AddItem(splitButtonDataData) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonData.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData btndataEnkelData = new PushButtonData(
                "Enkel Data Plaatsen",
                "Data:\nEnkel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.EnkelData" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een eenvoudig data punt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = EnkelData32Scr,
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton EnkelDataButton = splitButtonData.AddPushButton(btndataEnkelData);
            EnkelDataButton.Enabled = true;

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataDubbelData = new PushButtonData(
                "Dubbele Data Plaatsen",
                "Data:\nDubbel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.DubbelData" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een dubbel data punt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = DubbelData32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.DataBekabeld" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Plaats een bekabeld data punt",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                //Image = PlaceHolder16Scr,
                LargeImage = Bedraad32Scr,
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonDataBekabeld = splitButtonData.AddPushButton(DataBekabeldButtonData);
            buttonDataBekabeld.Enabled = true;

            panel10.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region splitButton (Schakelaar)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataSchakelaar = new SplitButtonData(
                "Schakelaar Plaatsen",
                "Schakelaar")
            {
                //Image = PlaceHolder16Scr,
                LargeImage = Enkelpolig32Scr,
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonSchakelaar = panel10.AddItem(splitButtonDataSchakelaar) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonSchakelaar.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData btndataSchakelaarEnkelpolig = new PushButtonData(
                "Enkelpolige Schakelaar", /// De naam van de standaardactie
                "Schakelaar:\nEnkelpolig", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarEnkelpolig" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Plaats een enkelpolige schakelaar",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                //Image = PlaceHolder16Scr,
                LargeImage = Enkelpolig32Scr,
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton buttonSchakelaarEnkelpolig = splitButtonSchakelaar.AddPushButton(btndataSchakelaarEnkelpolig);
            buttonSchakelaarEnkelpolig.Enabled = true;

            /// Knopgegevens instellen voor de tweede knop onder de dropdown
            PushButtonData btndataSchakelaarDubbelpolig = new PushButtonData(
                "Dubbelpolige Schakelaar",
                "Schakelaar:\nDubbelpolig",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarDubbelpolig" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een dubbelpolige schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Dubbelpolig32Scr,
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonSchakelaarDubbelpolig = splitButtonSchakelaar.AddPushButton(btndataSchakelaarDubbelpolig);
            buttonSchakelaarDubbelpolig.Enabled = true;

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataSchakelaarVierpolig = new PushButtonData(
                "Vierpolige Schakelaar",
                "Schakelaar:\nVierpolig",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarVierpolig" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een vierpolige schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = PlaceHolder32Scr,
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonSchakelaarVierpolig = splitButtonSchakelaar.AddPushButton(btndataSchakelaarVierpolig);
            buttonSchakelaarVierpolig.Enabled = false;

            /// Voegt een horizontale lijn toe onder de knop
            splitButtonSchakelaar.AddSeparator();

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataSchakelaarWissel = new PushButtonData(
                "Wissel Schakelaar",
                "Schakelaar:\nWissel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarWissel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een wissel schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Wissel32Scr,
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonSchakelaarWissel = splitButtonSchakelaar.AddPushButton(btndataSchakelaarWissel);
            buttonSchakelaarWissel.Enabled = true;

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndataSchakelaarDubbelWissel = new PushButtonData(
                "Dubbelpolige Wisselschakelaar",
                "Schakelaar:\nWissel\nDubbelp.",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarDubbelWissel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een dubbelpolige wisselschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = DubbelWissel32Scr,
            };

            /// Voeg de vijfde knop toe aan de dropdown
            PushButton buttonSchakelaarDubbelWissel = splitButtonSchakelaar.AddPushButton(btndataSchakelaarDubbelWissel);
            buttonSchakelaarDubbelWissel.Enabled = true;

            /// Knopgegevens instellen voor de zesde knop onder de dropdown
            PushButtonData btndataSchakelaar2xWissel = new PushButtonData(
                "2x Wisselschakelaar",
                "Schakelaar:\n2x Wissel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Schakelaar2xWissel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een 2x wisselschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Wissel2x32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarSerie" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een serieschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Serie32Scr,
            };

            /// Voeg de zevende knop toe aan de dropdown
            PushButton buttonSchakelaarSerie = splitButtonSchakelaar.AddPushButton(btndataSchakelaarSerie);
            buttonSchakelaarSerie.Enabled = true;

            /// Knopgegevens instellen voor de achtste knop onder de dropdown
            PushButtonData btndataSchakelaarKruis = new PushButtonData(
                "Kruisschakelaar",
                "Schakelaar:\nKruis",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarKruis" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een kruisschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Kruis32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarDimmer" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een leddimmer schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Dimmer32Scr,
            };

            /// Voeg de negende knop toe aan de dropdown
            PushButton buttonSchakelaarDimmer = splitButtonSchakelaar.AddPushButton(btndataSchakelaarDimmer);
            buttonSchakelaarDimmer.Enabled = true;

            /// Knopgegevens instellen voor de tiende knop onder de dropdown
            PushButtonData btndataSchakelaarDimmerWissel = new PushButtonData(
                "Dimmer Wisselschakelaar",
                "Schakelaar:\nDimmer\nWissel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarDimmerWissel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een dimmer wisselschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = WisselDimmer32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarJaloezie" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een jaloezie schakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Jaloezie32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarBewegingWand" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een wand bewegingsmelder",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = PlaceHolder32Scr,
            };

            /// Voeg de twaalfde knop toe aan de dropdown
            PushButton buttonSchakelaarBewegingWand = splitButtonSchakelaar.AddPushButton(btndataSchakelaarBewegingWand);
            buttonSchakelaarBewegingWand.Enabled = false;

            /// Knopgegevens instellen voor de dertiende knop onder de dropdown
            PushButtonData btndataSchakelaarBewegingPlafond = new PushButtonData(
                "Plafond Beweging Schakelaar",
                "Schakelaar:\nBeweging\nPlafond",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarBewegingPlafond" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een plafond bewegingsmelder",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = PlaceHolder32Scr,
            };

            /// Voeg de dertiende knop toe aan de dropdown
            PushButton buttonSchakelaarBewegingPlafond = splitButtonSchakelaar.AddPushButton(btndataSchakelaarBewegingPlafond);
            buttonSchakelaarBewegingPlafond.Enabled = false;

            /// Knopgegevens instellen voor de veertiende knop onder de dropdown
            PushButtonData btndataSchakelaarSchemer = new PushButtonData(
                "Schemerschakelaar",
                "Schakelaar:\nSchemer",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.SchakelaarSchemer" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een schemerschakelaar",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = PlaceHolder32Scr,
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
                //Image = PlaceHolder16Scr,
                LargeImage = Centraaldoos32Scr,
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonVerlichting = panel10.AddItem(splitButtonDataVerlichting) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonVerlichting.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData btndataCentraaldoos = new PushButtonData(
            "Centraaldoos Plaatsen", /// De naam van de standaardactie
            "Verlichting:\nCentraaldoos", /// Tooltip voor de standaardactie
            Assembly.GetExecutingAssembly().Location,
            "NijhofAddIn.Revit.Commands.Elektra.Centraaldoos" /// Vervang door de relevante namespace en klasse
            )
            {
                ToolTip = "Plaats een centraaldoos",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                //Image = PlaceHolder16Scr,
                LargeImage = Centraaldoos32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.LichtPlafond" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een plafond lichtpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = LichtPlafond32Scr,
            };

            /// Voeg de tweede knop toe aan de dropdown
            PushButton buttonLichtPlafond = splitButtonVerlichting.AddPushButton(btndataLichtPlafond);
            buttonLichtPlafond.Enabled = true;

            /// Knopgegevens instellen voor de derde knop onder de dropdown
            PushButtonData btndataInbouwspot = new PushButtonData(
                "Inbouwspot Plaatsen",
                "Verlichting:\nSpot",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Inbouwspot" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een inbouwspot",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Inbouwspot32Scr,
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonInbouwspot = splitButtonVerlichting.AddPushButton(btndataInbouwspot);
            buttonInbouwspot.Enabled = true;

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataLichtWand = new PushButtonData(
                "Wand Lichtpunt Plaatsen",
                "Verlichting:\nWand",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.LichtWand" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een wandlichtpunt",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = LichtWand32Scr,
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonLichtWand = splitButtonVerlichting.AddPushButton(btndataLichtWand);
            buttonLichtWand.Enabled = true;

            panel10.AddSeparator(); //Voegt een verticale lijn toe
            #endregion

            #region splitButton (Overig)
            /// Instellen van de split-knopgegevens
            SplitButtonData splitButtonDataOverig = new SplitButtonData(
                "Overig Plaatsen",
                "Overig")
            {
                //Image = PlaceHolder16Scr,
                LargeImage = Rookmelder32Scr,
            };

            /// Voeg de SplitButton toe aan de panel
            SplitButton splitButtonOverig = panel10.AddItem(splitButtonDataOverig) as SplitButton;

            /// Zet IsSynchronizedWithCurrentItem op false om synchronisatie te voorkomen
            splitButtonOverig.IsSynchronizedWithCurrentItem = true;

            /// Stel hoofdactie van de splitButton in
            PushButtonData btndataRookmelder = new PushButtonData(
                "Rookmelder Plaatsen",
                "Overig:\nRookmelder",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Rookmelder" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een rookmelder",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Rookmelder32Scr,
            };

            /// Voeg hoofdactie toe aan de splitbutton
            PushButton buttonRookmelder = splitButtonOverig.AddPushButton(btndataRookmelder);
            buttonRookmelder.Enabled = true;

                /// Knopgegevens instellen voor de tweede knop onder de dropdown
                PushButtonData btndataBedieningLos = new PushButtonData(
                "Bediening Los Plaatsen", /// De naam van de standaardactie
                "Overig:\nBediening", /// Tooltip voor de standaardactie
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.BedieningLos" /// Vervang door de relevante namespace en klasse
                )
            {
                ToolTip = "Plaats bediening los",
                //LongDescription = "Laad alle GPS punten in het project vanuit een opgegeven locatie",
                //Image = PlaceHolder16Scr,
                LargeImage = Bediening32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.Drukknopbel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een drukknop voor de bel",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Deurbel32Scr,
            };

            /// Voeg de derde knop toe aan de dropdown
            PushButton buttonDrukknopbel = splitButtonOverig.AddPushButton(btndataDrukknopbel);
            buttonDrukknopbel.Enabled = true;

            /// Knopgegevens instellen voor de vierde knop onder de dropdown
            PushButtonData btndataSchel = new PushButtonData(
                "Schel Plaatsen",
                "Overig:\nSchel",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Schel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een schel",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Dingdong32Scr,
            };

            /// Voeg de vierde knop toe aan de dropdown
            PushButton buttonSchel = splitButtonOverig.AddPushButton(btndataSchel);
            buttonSchel.Enabled = true;

            /// Knopgegevens instellen voor de vijfde knop onder de dropdown
            PushButtonData btndataIntercom = new PushButtonData(
                "Intercom Plaatsen",
                "Overig:\nIntercom",
                Assembly.GetExecutingAssembly().Location,
                "NijhofAddIn.Revit.Commands.Elektra.Intercom" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een intercom",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Intercom32Scr,
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
                "NijhofAddIn.Revit.Commands.Elektra.Grondkabel" /// Dit is de file namespace van de command die uitgevoerd moet worden bij deze knop
                )
            {
                ToolTip = "Plaats een grondkabel",
                //LongDescription = "Klik op de plek waar je het GPS punt wilt plaatsen. Als je niks ziet moet je de 'View Properties' aanpassen",
                //Image = PlaceHolder16Scr,
                LargeImage = Grondkabel32Scr,
            };

            /// Voeg de zesde knop toe aan de dropdown
            PushButton buttonGrondkabel = splitButtonOverig.AddPushButton(btndataGrondkabel);
            buttonGrondkabel.Enabled = true;

            //panel2.AddSeparator(); //Voegt een verticale lijn toe
            #endregion
            #endregion

            #region Buttons Panel (Tag)
            #region pushButton (tagGroepNummer)
            PushButtonData btndataTagGN = new PushButtonData
                (
                "Tag Groepnummer",
                "Groep-\nnummer",
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
                "Switch-\ncodes",
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

            PushButton buttonSwitchCode = (PushButton)panel11.AddItem(btndataSwitchCode);
            buttonSwitchCode.Enabled = true;
            ContextualHelp contextHelpSwitchCode = new ContextualHelp(ContextualHelpType.Url,
                    "https://github.com/Damianmts/NijhofAddIn/wiki/Elektra#switchcodes");
            buttonSwitchCode.SetContextualHelp(contextHelpSwitchCode);
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