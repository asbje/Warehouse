using System.IO;
using System.Xml.Linq;

namespace Warehouse.DataLake.Modules.DaluxFM.Refine
{
    public class LotsRefine : BaseRefine
    {
        public XDocument Data { get; set; }

        public LotsRefine(string moduleName, Stream xmlStream) : base(moduleName, "lots")
        {
            xmlStream.Position = 0;
            Data = XDocument.Load(xmlStream);
            Refine();
        }

        public override void Refine()
        {
            var r = 0;
            var genericHelper = new GenericHelper();
            foreach (var item in Data.Root.Descendants("Lot"))
            {
                genericHelper.AddAttributes(r, item, CsvSet, new string[] { "GisPolygon" });
                genericHelper.AddLayerDatas(r, item, CsvSet);
                r++;
            }
        }
    }
}
