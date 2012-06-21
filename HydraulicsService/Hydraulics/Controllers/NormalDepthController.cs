using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Hydraulics.Models;
using HydraulicsService;

namespace Hydraulics.Controllers
{
    public class NormalDepthController : Controller
    {
        //
        // GET: /NormalDepth/

        public ActionResult Index()
        {
            DepthData_Normal model = new DepthData_Normal()
            {
                ManningRoughness = 0.016,
                Diameter = 2,
                DesignFlow = 13.2,
                ElevationUp = 103,
                ElevationDown = 101,
                Length = 100.5,
                Depth = 0,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DepthData_Normal model)
        {
            NormalDepthService service = new NormalDepthService();
            model.Depth = service.CalculateNormalDepth_Cricular(model.ManningRoughness, model.DesignFlow, model.Diameter, model.ElevationUp, model.ElevationDown, model.Length);
            return View(model);
        }

    }
}
