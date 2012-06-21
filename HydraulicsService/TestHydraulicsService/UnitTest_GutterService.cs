using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HydraulicsService;

namespace TestHydraulicsService
{
    [TestClass]
    public class UnitTest_GutterService
    {
        [TestMethod]
        public void Test_GutterCalculation()
        {
            GutterData gd = new GutterData();
            gd.LongitudinalSlope = 0.02;
            gd.CrossSlopeX = 0.02;
            gd.CrossSlopeW = 0.05;
            gd.GutterWidth = 4.0;
            double GutterKc = 0.56, ManningRoughness = 0.016, DesignFlow = 3.0, GrateWidth = 2.0;
            GutterService service = new GutterService();
            GutterResult gr = service.GutterCalculation(GutterKc, ManningRoughness, gd, DesignFlow, GrateWidth);
            Assert.AreEqual(7.293, gr.GutterSpread, 0.003);
            Assert.AreEqual(0.266, gr.GutterDepth, 0.001);
            Assert.AreEqual(3.885, gr.GutterVelocity, 0.001);
            Assert.AreEqual(0.942, gr.Eo, 0.001);
            Assert.AreEqual(0.662, gr.EoEx, 0.001);
        }
    }
}
