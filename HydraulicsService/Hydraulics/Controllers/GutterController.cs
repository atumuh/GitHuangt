using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Hydraulics.Models;
using HydraulicsService;

namespace Hydraulics.Controllers
{
    public class GutterController : Controller
    {
        //
        // GET: /Gutter/

        public ActionResult Index()
        {
            GutterDataEx model = new GutterDataEx()
            {

                Name = "TestGutter",
                ManningRoughness = 0.016,
                DesignFlow = 4,

                gutterData = new GutterData()
                {
                    LongitudinalSlope = 0.02,
                    CrossSlopeW = 0.05,
                    CrossSlopeX = 0.02,
                    GutterWidth = 4,
                },

                gutterResult = new GutterResult(),
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(GutterDataEx model)
        {
            GutterService service = new GutterService();
            model.gutterResult = service.GutterCalculation_DesignFlow(model.ManningRoughness, model.gutterData, model.DesignFlow);
            return View(model);
        }

    }
}
