﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLake.Modules.Eloverblik.Refine;
using Warehouse.DataLake.ModulesTest.Helpers;

namespace Warehouse.DataLake.ModulesTest.Eloverblik.Refine
{
    [TestClass]
    public class MeteringReadingsRefineTest : GenericTest
    {

        [TestMethod]
        public void MeteringReadingsRefine()
        {
            var meteringReadings = ExporterTest.GetData("RedingsPerYear_2020.json");

            var meteringReadingsRefine = new MeteringReadingsRefine("Eloverblik", "readingsPerYear", meteringReadings);
            
            Assert.IsFalse(meteringReadingsRefine.HasErrors);

            meteringReadingsRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "Eloverblik", "Out", "readingsPerYear.csv"));
        }
    }
}