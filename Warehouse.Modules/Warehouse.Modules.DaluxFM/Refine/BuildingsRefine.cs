using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Warehouse.DataLake.CsvTools;

namespace Warehouse.Modules.DaluxFM.Refine
{
    public class BuildingsRefine : RefineBase
    {
        public XDocument Data { get; set; }

        public BuildingsRefine(IExporter exporter, Stream xmlStream) : base(exporter, "buildings")
        {
            xmlStream.Position = 0;
            Data = XDocument.Load(xmlStream);
            Refine();
        }

        public override void Refine()
        {
            var r = 0;
            CsvSet.AddHeader("latitude", out int buildLatCol);
            CsvSet.AddHeader("longitude", out int buildLonCol);
            var genericHelper = new GenericHelper();
            var buildingElements = Data.Root.Descendants("Building");
            foreach (var item in buildingElements)
            {
                genericHelper.AddAttributes(r, item, CsvSet, new string[] { "GisPolygon" });
                genericHelper.AddLayerDatas(r, item, CsvSet);
                AddBuildingGps(ref buildLatCol, ref buildLonCol, r, item);
                r++;
            }
        }

        private void AddBuildingGps(ref int latCol, ref int lonCol, int r, XElement element)
        {
            var coords = new List<(float Lat, float Lon)>();
            foreach (var coord in element.Element("GIS").Element("OuterPolygon").Elements("Coordinate"))
                coords.Add(((float)coord.Attribute("x"), (float)coord.Attribute("y")));

            if (GenericHelper.GetGravityPoint(coords, out (float Lat, float Lon) gps))
            {
                CsvSet.AddRecord(latCol, r, gps.Lat);
                CsvSet.AddRecord(lonCol, r, gps.Lon);
            }
        }
    }
}
