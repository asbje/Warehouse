using Warehouse.DataLake.CsvTools;

namespace Warehouse.Modules.OutlookBookings.Refine
{
    public class LocationsRefine : RefineBase
    {

        public LocationsRefine(IExporter exporter) : base(exporter, "locations")
        {
            Refine();
        }

        public override void Refine()
        {
            CsvSet = new CsvSet("Mail,Capacity,Location,Level,Building,Name,Type,ShortName");
            CsvSet.AddRecords(new[] {
                "ByogMiljo-Stor_Projektor@hillerod.dk,,Ukendt,Ukendt,Ukendt,By og Miljø – Stor Projektor,Asset,DHH_By og Miljø – Stor Projektor",
                "PPR_WISC_IV_taske1@hillerod.dk,,Ukendt,Ukendt,Ukendt,PPR WISC-IV_taske1,Asset,DHH_PPR WISC-IV_taske1",
                "PPR_WISC_IV_taske2@hillerod.dk,,Ukendt,Ukendt,Ukendt,PPR WISC-IV_taske2,Asset,DHH_PPR WISC-IV_taske2",
                "PPR_WISC_IV_taske3@hillerod.dk,,Ukendt,Ukendt,Ukendt,PPR WISC-IV_taske3,Asset,DHH_PPR WISC-IV_taske3",
                "PPR_WISC_IV_taske4@hillerod.dk,,Ukendt,Ukendt,Ukendt,PPR WISC-IV_taske4,Asset,DHH_PPR WISC-IV_taske4",
                "PPR_WISC_IV_taske5@hillerod.dk,,Ukendt,Ukendt,Ukendt,PPR WISC-IV_taske5,Asset,DHH_PPR WISC-IV_taske5",
                "PPR_WISC_IV_taske6@hillerod.dk,,Ukendt,Ukendt,Ukendt,PPR WISC-IV_taske6,Asset,DHH_PPR WISC-IV_taske6",
                "PPR_WPPSI_IV_taske1@hillerod.dk,,Ukendt,Ukendt,Ukendt,PPR WPPSI-IV_taske1,Asset,DHH_PPR WPPSI-IV_taske1",
                "PPR_WPPSI_IV_taske2@hillerod.dk,,Ukendt,Ukendt,Ukendt,PPR WPPSI-IV_taske2,Asset,DHH_PPR WPPSI-IV_taske2",
                "rh-byraadssalen@hillerod.dk,,Ukendt,Ukendt,Ukendt,RH-Byrådssalen,Asset,DHH_RH-Byrådssalen",
                "RH-HUB-01@hillerod.dk,,Ukendt,Ukendt,Ukendt,RH-HUB-01,Asset,DHH_RH-HUB-01",
                "RH-HUB-02@hillerod.dk,,Ukendt,Ukendt,Ukendt,RH-HUB-02,Asset,DHH_RH-HUB-02",
                "AutismeprofilCUSFaellesKalender@hillerod.dk,,Ukendt,Ukendt,Ukendt,Autismeprofil - CUS fælleskalender,Calendar,DHH_Autismeprofil - CUS fælleskalender",
                "Bocentrets_faelleskalender@hillerod.dk,,Ukendt,Ukendt,Ukendt,Bocenter-fælleskalender,Calendar,DHH_Bocenter-fælleskalender",
                "CSUegedammen-faelles@hillerod.dk,,Ukendt,Ukendt,Ukendt,CSUegedammen-fælleskalender,Calendar,DHH_CSUegedammen-fælleskalender",
                "Familiehuset_faelleskalender@hillerod.dk,,Ukendt,Ukendt,Ukendt,Familiehuset-fælleskalender,Calendar,DHH_Familiehuset-fælleskalender",
                "KC-HIL-BOOK-KURSUS-IPADS-Kalender@hillerod.dk,,Ukendt,Ukendt,Ukendt,KC-HIL-BOOK-KURSUS-IPADS Kalender,Calendar,DHH_KC-HIL-BOOK-KURSUS-IPADS Kalender",
                "Kessers_Hus_Kalender@hillerod.dk,,Ukendt,Ukendt,Ukendt,Kessers Hus Kalender,Calendar,DHH_Kessers Hus Kalender",
                "Skoleafdelingensaarskalender@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skoleafdelingens årskalender,Calendar,DHH_Skoleafdelingens årskalender",
                "Skovhusetsaktivitetskalender@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skovhusets aktivitets kalender,Calendar,DHH_Skovhusets aktivitets kalender",
                "tandplejensfaelleskalender@hillerod.dk,,Ukendt,Ukendt,Ukendt,Tandplejens fælleskalender,Calendar,DHH_Tandplejens fælleskalender",
                "VisitationFamilierogSundhedskalender@hillerod.dk,,Ukendt,Ukendt,Ukendt,Visitation Familier og Sundheds kalender,Calendar,DHH_Visitation Familier og Sundheds kalender",
                "aarshjul-hillerod-bibliotek@hillerod.dk,,Ukendt,Ukendt,Ukendt,Årshjul Hillerød Bibliotek,Calendar,DHH_Årshjul Hillerød Bibliotek",
                "Aarskalender_Gadevang_Asyl@hillerod.dk,,Ukendt,Ukendt,Ukendt,Årskalender Gadevang Asyl,Calendar,DHH_Årskalender Gadevang Asyl",
                "Det-Hvide-Hus-Belle-de-Boskoop@hillerod.dk,6,Trollesmindealle 27,1. sal,Det hvide hus,Belle de Boskob,Room,DHH1_Belle de Boskob",
                "Det-Hvide-Hus-Cox-Orange@hillerod.dk,4,Trollesmindealle 27,1. sal,Det hvide hus,Cox Orange,Room,DHH1_Cox Orange",
                "Det-Hvide-Hus-Graasten@hillerod.dk,8,Trollesmindealle 27,1. sal,Det hvide hus,Gråsten,Room,DHH1_Gråsten",
                "Det-Hvide-Hus-Ingrid-Marie@hillerod.dk,8,Trollesmindealle 27,1. sal,Det hvide hus,Ingrid Marie,Room,DHH1_Ingrid Marie",
                "Det-Hvide-Hus-Pederstrup@hillerod.dk,4,Trollesmindealle 27,1. sal,Det hvide hus,Pederstrup,Room,DHH1_Pederstrup",
                "Det-Hvide-Hus-Per-Smed@hillerod.dk,8,Trollesmindealle 27,1. sal,Det hvide hus,Per Smed,Room,DHH1_Per Smed",
                "andegaarden-1.sal@hillerod.dk,6,Trollesmindealle 27,1. sal,Rådhus,Andergården,Room,RÅD1_Andergården",
                "hoenseriet-1.sal@hillerod.dk,6,Trollesmindealle 27,1. sal,Rådhus,Hønseriet,Room,RÅD1_Hønseriet",
                "hundegaarden-1.sal@hillerod.dk,6,Trollesmindealle 27,1. sal,Rådhus,Hundegården,Room,RÅD1_Hundegården",
                "kaningaarden-1.sal@hillerod.dk,6,Trollesmindealle 27,1. sal,Rådhus,Kaningården,Room,RÅD1_Kaningården",
                "raevegaarden-1.sal@hillerod.dk,6,Trollesmindealle 27,1. sal,Rådhus,Rævegården,Room,RÅD1_Rævegården",
                "fasanburet-2.sal@hillerod.dk,6,Trollesmindealle 27,2. sal,Rådhus,Fasanburet,Room,RÅD2_Fasanburet",
                "hestestalden-2.sal@hillerod.dk,6,Trollesmindealle 27,2. sal,Rådhus,Hestestalden,Room,RÅD2_Hestestalden",
                "kostalden-2.sal@hillerod.dk,6,Trollesmindealle 27,2. sal,Rådhus,Kostalden,Room,RÅD2_Kostalden",
                "plagestalden-2.sal@hillerod.dk,6,Trollesmindealle 27,2. sal,Rådhus,Plagestalden,Room,RÅD2_Plagestalden",
                "rugekassen-2.sal@hillerod.dk,6,Trollesmindealle 27,2. sal,Rådhus,Rugekassen,Room,RÅD2_Rugekassen",
                "byraadssalen-moedecenter@hillerod.dk,30,Trollesmindealle 27,Stuen,Rådhus,Byrådssalen,Room,RÅD0_Byrådssalen",
                "harven-moedecenter@hillerod.dk,4,Trollesmindealle 27,Stuen,Rådhus,Harven,Room,RÅD0_Harven",
                "kantinen-moedecenter@hillerod.dk,100,Trollesmindealle 27,Stuen,Rådhus,Kantinen,Room,RÅD0_Kantinen",
                "leen-moedecenter@hillerod.dk,12,Trollesmindealle 27,Stuen,Rådhus,Leen,Room,RÅD0_Leen",
                "mejeriet-stueetagen@hillerod.dk,8,Trollesmindealle 27,Stuen,Rådhus,Mejeriet,Room,RÅD0_Mejeriet",
                "ploven-moedecenter@hillerod.dk,8,Trollesmindealle 27,Stuen,Rådhus,Ploven,Room,RÅD0_Ploven",
                "seglet-moedecenter@hillerod.dk,10,Trollesmindealle 27,Stuen,Rådhus,Seglet,Room,RÅD0_Seglet",
                "tromlen-moedecenter@hillerod.dk,4,Trollesmindealle 27,Stuen,Rådhus,Tromlen,Room,RÅD0_Tromlen",
                "Bocenter-Modelokale1@hillerod.dk,7,Ukendt,Ukendt,Ukendt,Bocenter - Mødelokale 1,Room,NaN_Bocenter - Mødelokale 1",
                "Bocenter-Modelokale2@hillerod.dk,3,Ukendt,Ukendt,Ukendt,Bocenter - Mødelokale 2,Room,NaN_Bocenter - Mødelokale 2",
                "BocenterMoedelokale3@hillerod.dk,3,Ukendt,Ukendt,Ukendt,Bocenter - Mødelokale 3,Room,NaN_Bocenter - Mødelokale 3",
                "CBB-Modelokale@hillerod.dk,,Ukendt,Ukendt,Ukendt,CBB-Mødelokale (lille),Room,NaN_CBB-Mødelokale (lille)",
                "CBB-Personalerum@hillerod.dk,,Ukendt,Ukendt,Ukendt,CBB-Personalerum,Room,NaN_CBB-Personalerum",
                "Clausensvej_3_Baand_Broer_lokalet@hillerod.dk,,Ukendt,Ukendt,Ukendt,Clausensvej 3 - Bånd&Broer lokalet,Room,NaN_Clausensvej 3 - Bånd&Broer lokalet",
                "Clausensvej_3_Familie_lokalet@hillerod.dk,,Ukendt,Ukendt,Ukendt,Clausensvej 3 - Familie lokalet,Room,NaN_Clausensvej 3 - Familie lokalet",
                "Clausensvej_3_Model_hjem_lokalet@hillerod.dk,,Ukendt,Ukendt,Ukendt,Clausensvej 3 - Model hjem lokalet,Room,NaN_Clausensvej 3 - Model hjem lokalet",
                "Clausensvej_3_P_moede_lokalet@hillerod.dk,,Ukendt,Ukendt,Ukendt,Clausensvej 3 - P møde lokalet,Room,NaN_Clausensvej 3 - P møde lokalet",
                "Clausensvej_3_SKP_lokalet@hillerod.dk,,Ukendt,Ukendt,Ukendt,Clausensvej 3 - SKP lokalet,Room,NaN_Clausensvej 3 - SKP lokalet",
                "CSU_Egedammen_lille_modelokale_blok_B@hillerod.dk,,Ukendt,Ukendt,Ukendt,CSU Egedammen lille mødelokale blok B,Room,NaN_CSU Egedammen lille mødelokale blok B",
                "CUS_CBB-gymnastik@hillerod.dk,,Ukendt,Ukendt,Ukendt,CUS-CBB - gymnastiksal,Room,NaN_CUS-CBB - gymnastiksal",
                "CUSBibliotekTrollesbro@hillerod.dk,,Ukendt,Ukendt,Ukendt,CUS - Bibliotek; Trollesbro,Room,NaN_CUS - Bibliotek; Trollesbro",
                "CUS-flexrum@hillerod.dk,,Ukendt,Ukendt,Ukendt,CBB-CUS flexrum1,Room,NaN_CBB-CUS flexrum1",
                "CUS-moderummedkok@hillerod.dk,,Ukendt,Ukendt,Ukendt,CUS - møderum med køkken,Room,NaN_CUS - møderum med køkken",
                "engen-samtalerum@hillerod.dk,,Ukendt,Ukendt,Ukendt,engen-lokale samtalerum,Room,NaN_engen-lokale samtalerum",
                "Familiehuset-lokale@hillerod.dk,,Ukendt,Ukendt,Ukendt,Familiehuset-lokale,Room,NaN_Familiehuset-lokale",
                "Familiehuset-mim@hillerod.dk,,Ukendt,Ukendt,Ukendt,Familiehuset-mim,Room,NaN_Familiehuset-mim",
                "ForsamlingshusetbagerstSkanselyet@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skanselyet - Forsamlingshuset bagerst,Room,NaN_Skanselyet - Forsamlingshuset bagerst",
                "ForsamlingshusetforrestSkanselyet@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skanselyet - Forsamlingshuset forrest,Room,NaN_Skanselyet - Forsamlingshuset forrest",
                "Frb_Centret_Lokale_8@hillerod.dk,,Ukendt,Ukendt,Ukendt,Frb. Centret Lokale 8,Room,NaN_Frb. Centret Lokale 8",
                "Frb_Centret_Lokale6a@hillerod.dk,8,Ukendt,Ukendt,Ukendt,Frb. Centret lokale 6 A,Room,NaN_Frb. Centret lokale 6 A",
                "Frb_Centret_Lokale6b@hillerod.dk,8,Ukendt,Ukendt,Ukendt,Frb. Centret lokale 6 B,Room,NaN_Frb. Centret lokale 6 B",
                "Frb_Centret_Lokale6c@hillerod.dk,8,Ukendt,Ukendt,Ukendt,Frb. Centret lokale 6 C,Room,NaN_Frb. Centret lokale 6 C",
                "Frb_Centret_Lokale7@hillerod.dk,50,Ukendt,Ukendt,Ukendt,Frb. Centret lokale 7,Room,NaN_Frb. Centret lokale 7",
                "Frb_Centret_moedelokale_1@hillerod.dk,30,Ukendt,Ukendt,Ukendt,Frb. Centret mødelokale 1,Room,NaN_Frb. Centret mødelokale 1",
                "HerrevaerelsetSkanse@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skanselyet - Herreværelset,Room,NaN_Skanselyet - Herreværelset",
                "HUV-modelokale1@hillerod.dk,10,Ukendt,Ukendt,Ukendt,UU - Nordre Jernbanevej 6 - Mødelokale 1,Room,NaN_UU - Nordre Jernbanevej 6 - Mødelokale 1",
                "HUV-modelokale2@hillerod.dk,10,Ukendt,Ukendt,Ukendt,UU - Nordre Jernbanevej 6 - Mødelokale 2,Room,NaN_UU - Nordre Jernbanevej 6 - Mødelokale 2",
                "Kulturrummet-Hilbib@hillerod.dk,,Ukendt,Ukendt,Ukendt,Kulturrummet Hilbib,Room,NaN_Kulturrummet Hilbib",
                "Lokale_120_KC@hillerod.dk,,Ukendt,Ukendt,Ukendt,Lokale 120 - KC,Room,NaN_Lokale 120 - KC",
                "Lokale156-KC@hillerod.dk,,Ukendt,Ukendt,Ukendt,Lokale 156 - KC,Room,NaN_Lokale 156 - KC",
                "Lokale-223-KC@hillerod.dk,,Ukendt,Ukendt,Ukendt,Lokale 325 - KC,Room,NaN_Lokale 325 - KC",
                "Lokale-306-KC@hillerod.dk,,Ukendt,Ukendt,Ukendt,Lokale 306 - KC,Room,NaN_Lokale 306 - KC",
                "Lokale-308-KC@hillerod.dk,,Ukendt,Ukendt,Ukendt,Lokale 308 - KC,Room,NaN_Lokale 308 - KC",
                "Lokale-310-KC@hillerod.dk,,Ukendt,Ukendt,Ukendt,Lokale 310 - KC,Room,NaN_Lokale 310 - KC",
                "Lokale331-KC@hillerod.dk,,Ukendt,Ukendt,Ukendt,Lokale 331 - KC,Room,NaN_Lokale 331 - KC",
                "marken-samtalerum@hillerod.dk,,Ukendt,Ukendt,Ukendt,marken-lokale samtalerum,Room,NaN_marken-lokale samtalerum",
                "Midtbyens_science_hus_Lindevej_2@hillerod.dk,,Ukendt,Ukendt,Ukendt,Midtbyens science hus - Lindevej 2,Room,NaN_Midtbyens science hus - Lindevej 2",
                "mitkontor@hillerod.dk,,Ukendt,Ukendt,Ukendt,Mit Kontor,Room,NaN_Mit Kontor",
                "Mode_Bakkehus_Plejecenter_Skovhuset@hillerod.dk,,Ukendt,Ukendt,Ukendt,Møde Bakkehus_Plejecenter_Skovhuset,Room,NaN_Møde Bakkehus_Plejecenter_Skovhuset",
                "Mode_Koglen_Plejecenter_Skovhuset@hillerod.dk,20,Ukendt,Ukendt,Ukendt,Møde Koglen_Plejecenter_Skovhuset,Room,NaN_Møde Koglen_Plejecenter_Skovhuset",
                "Mode_Kvisten_Plejecenter_Skovhuset@hillerod.dk,6,Ukendt,Ukendt,Ukendt,Møde Kvisten_Plejecenter_Skovhuset,Room,NaN_Møde Kvisten_Plejecenter_Skovhuset",
                "Mode_Stubben_Plejecenter_Skovhuset@hillerod.dk,6,Ukendt,Ukendt,Ukendt,Møde Stubben_Plejecenter_Skovhuset,Room,NaN_Møde Stubben_Plejecenter_Skovhuset",
                "Modelokale_lille_Lions_Park@hillerod.dk,8,Ukendt,Ukendt,Ukendt,Mødelokale lille Lions Park,Room,NaN_Mødelokale lille Lions Park",
                "Modelokale_stort_Lions_Park@hillerod.dk,50,Ukendt,Ukendt,Ukendt,Mødelokale stort Lions Park,Room,NaN_Mødelokale stort Lions Park",
                "Modelokale1-Biblioteket@hillerod.dk,,Ukendt,Ukendt,Ukendt,Mødelokale1-lokale Biblioteket,Room,NaN_Mødelokale1-lokale Biblioteket",
                "Modelokale2-Biblioteket@hillerod.dk,,Ukendt,Ukendt,Ukendt,Mødelokale2-lokale Biblioteket,Room,NaN_Mødelokale2-lokale Biblioteket",
                "MoedelokaletvedbroenSkanselyet@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skanselyet - Mødelokalet ved broen,Room,NaN_Skanselyet - Mødelokalet ved broen",
                "PPR_lokale_Sophienborgskolen@hillerod.dk,,Ukendt,Ukendt,Ukendt,PPR lokale; Sophienborgskolen,Room,NaN_PPR lokale; Sophienborgskolen",
                "Psykologkontorfrbysk@hillerod.dk,,Ukendt,Ukendt,Ukendt,Psykologkontor Fr.byskole,Room,NaN_Psykologkontor Fr.byskole",
                "Psykologkontor-Jespervej@hillerod.dk,8,Ukendt,Ukendt,Ukendt,Psykologkontor Jespervej,Room,NaN_Psykologkontor Jespervej",
                "SamtalerumvedkontoretSkanselyet@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skanselyet - Samtalerum ved kontoret,Room,NaN_Skanselyet - Samtalerum ved kontoret",
                "Skanselyet_drivhus@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skanselyet - drivhus,Room,NaN_Skanselyet - drivhus",
                "skoven-samtalerum@hillerod.dk,,Ukendt,Ukendt,Ukendt,skoven-lokale samtalerum,Room,NaN_skoven-lokale samtalerum",
                "Skovkilden_Bagerste@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skovkilden - Bagerste Del,Room,NaN_Skovkilden - Bagerste Del",
                "Skovkilden_cafe_Plejecenter_Skovhuset@hillerod.dk,50,Ukendt,Ukendt,Ukendt,Skovkilden_cafe_Plejecenter_Skovhuset,Room,NaN_Skovkilden_cafe_Plejecenter_Skovhuset",
                "Skovkilden_Cafe2@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skovkilden - Cafe Området,Room,NaN_Skovkilden - Cafe Området",
                "soeen-samtalerum@hillerod.dk,,Ukendt,Ukendt,Ukendt,søen-lokale samtalerum,Room,NaN_søen-lokale samtalerum",
                "Stemningen-lokale-Biblioteket@hillerod.dk,,Ukendt,Ukendt,Ukendt,Stemningen-lokale Biblioteket,Room,NaN_Stemningen-lokale Biblioteket",
                "StilleKontorBiblioteket@hillerod.dk,,Ukendt,Ukendt,Ukendt,Stille Kontor Biblioteket,Room,NaN_Stille Kontor Biblioteket",
                "SundhedsCentretModelokale1A@hillerod.dk,,Ukendt,Ukendt,Ukendt,SundhedsCentret Mødelokale 1A,Room,NaN_SundhedsCentret Mødelokale 1A",
                "SundhedsCentretModelokale2A@hillerod.dk,,Ukendt,Ukendt,Ukendt,SundhedsCentret Mødelokale 2A,Room,NaN_SundhedsCentret Mødelokale 2A",
                "Undervisningslokalet-aalholmhjemmet@hillerod.dk,8,Ukendt,Ukendt,Ukendt,Undervisningslokale 1 - Ålholmhjemmet,Room,NaN_Undervisningslokale 1 - Ålholmhjemmet",
                "UU_Nordre_Jernbanevej_6_lille_moderum@hillerod.dk,4,Ukendt,Ukendt,Ukendt,UU Nordre Jernbanevej 6 lille møderum,Room,NaN_UU Nordre Jernbanevej 6 lille møderum",
                "UU_Nordre_Jernbanevej_6_lokale_2_sal@hillerod.dk,3,Ukendt,Ukendt,Ukendt,UU Nordre Jernbanevej 6 lokale 2. sal,Room,NaN_UU Nordre Jernbanevej 6 lokale 2. sal",
                "Vandrehallen_Biblioteket@hillerod.dk,,Ukendt,Ukendt,Ukendt,Vandrehallen - Biblioteket,Room,NaN_Vandrehallen - Biblioteket",
                "walkandtalk@hillerod.dk,,Ukendt,Ukendt,Ukendt,Walk & talk lokale,Room,NaN_Walk & talk lokale",
                "Bil1-KC@hillerod.dk,,Ukendt,Ukendt,Ukendt,Bil1 - KC,Vehicle,NaN_Bil1 - KC",
                "Bil2-KC@hillerod.dk,,Ukendt,Ukendt,Ukendt,Bil2 - KC,Vehicle,NaN_Bil2 - KC",
                "BilABA97100@hillerod.dk,,Ukendt,Ukendt,Ukendt,Ældre og sundhed - Bil BA 97100,Vehicle,NaN_Ældre og sundhed - Bil BA 97100",
                "BilAM80253@hillerod.dk,,Ukendt,Ukendt,Ukendt,Ældre og sundhed - Bil CH 40093,Vehicle,NaN_Ældre og sundhed - Bil CH 40093",
                "BilAW75448@hillerod.dk,,Ukendt,Ukendt,Ukendt,Ældre og sundhed - Bil AW 75448,Vehicle,NaN_Ældre og sundhed - Bil AW 75448",
                "BillBC35424@hillerod.dk,,Ukendt,Ukendt,Ukendt,Ældre og sundhed - Bil BC 35424,Vehicle,NaN_Ældre og sundhed - Bil BC 35424",
                "BilAAW75449@hillerod.dk,,Ukendt,Ukendt,Ukendt,Ældre og sundhed - Bil  AW 75449,Vehicle,NaN_Ældre og sundhed - Bil  AW 75449",
                "BSS-Bil-CH16136@hillerod.dk,,Ukendt,Ukendt,Ukendt,BSS Bil CH16136,Vehicle,NaN_BSS Bil CH16136",
                "ByogMiljo-Bil3@hillerod.dk,,Ukendt,Ukendt,Ukendt,By og Miljø - Bil 3,Vehicle,NaN_By og Miljø - Bil 3",
                "byogmiljobil5@hillerod.dk,,Ukendt,Ukendt,Ukendt,By og Miljø - Bil 5,Vehicle,NaN_By og Miljø - Bil 5",
                "byogmiljobil6@hillerod.dk,,Ukendt,Ukendt,Ukendt,By og Miljø - Bil 6,Vehicle,NaN_By og Miljø - Bil 6",
                "CBB_Minibus_BV18685@hillerod.dk,,Ukendt,Ukendt,Ukendt,CBB_Minibus_BV18685,Vehicle,NaN_CBB_Minibus_BV18685",
                "CBBAM97134@hillerod.dk,,Ukendt,Ukendt,Ukendt,CBB_VW_Trans_AM97134,Vehicle,NaN_CBB_VW_Trans_AM97134",
                "CBBDW17693@hillerod.dk,,Ukendt,Ukendt,Ukendt,CBB_VW_Trans_DW17693,Vehicle,NaN_CBB_VW_Trans_DW17693",
                "CUS-Bil-CH16137@hillerod.dk,,Ukendt,Ukendt,Ukendt,CUS-Bil CH16137,Vehicle,NaN_CUS-Bil CH16137",
                "CUS-Bil-CJ-78031@hillerod.dk,,Ukendt,Ukendt,Ukendt,CUS-Bil CJ 78031,Vehicle,NaN_CUS-Bil CJ 78031",
                "CUS-bil-CJ84298@hillerod.dk,,Ukendt,Ukendt,Ukendt,CUS-bil CR 19913,Vehicle,NaN_CUS-bil CR 19913",
                "CUS-bil-CJ84299@hillerod.dk,,Ukendt,Ukendt,Ukendt,CUS-bil CR 19914,Vehicle,NaN_CUS-bil CR 19914",
                "CUS-bil-CJ84300@hillerod.dk,,Ukendt,Ukendt,Ukendt,CUS-bil CR 66671,Vehicle,NaN_CUS-bil CR 66671",
                "DIGIT-Bil-CG62554@hillerod.dk,,Ukendt,Ukendt,Ukendt,DIGIT-Bil CG62554,Vehicle,NaN_DIGIT-Bil CG62554",
                "Skovhuset-Bakkehus-bussen@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skovhuset - Bakkehus-bussen,Vehicle,NaN_Skovhuset - Bakkehus-bussen",
                "Skovhuset-Bussen@hillerod.dk,,Ukendt,Ukendt,Ukendt,Skovhuset-Bussen,Vehicle,NaN_Skovhuset-Bussen",
            });
        }
    }
}
