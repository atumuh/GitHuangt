using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Hydraulics.Models;
using HydraulicsService;

namespace Hydraulics.Controllers
{
    public class InletController : Controller
    {
        //
        // GET: /Inlet/

        public ActionResult Index()
        {
            InletDataEx model = new InletDataEx()
            {

                Name = "Test_Inlet",
                ManningRoughness = 0.016,
                DesignFlow = 4,

                inletData = new InletData()
                {
                    LongitudinalSlope = 0.02,
                    CrossSlopeW = 0.05,
                    CrossSlopeX = 0.02,
                    GutterWidth = 4,

                    InletLocation = EnumInletLocation.On_grade,
                    InletType = EnumInletType.Grate,
                    GrateType = EnumGrateType.PARALLEL_BAR_P_1_7_BY_8,
                    GrateWidth = 2,
                    GrateLength = 4,
                    InletLength = 4,
                    CurbOpeningHigh = 6,
                    LocalDepression = 3,
                },

                inletResult = new InletResult()
                {
                    GutterIn = new GutterResult(),
                    GutterOut = new GutterResult(),
                },
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(InletDataEx model)
        {
            InletService service = new InletService();
            model.inletResult = service.InletCalculation(model.ManningRoughness, model.inletData, model.DesignFlow);
            return View(model);
        }

    }
}
