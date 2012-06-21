using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Hydraulics.Models;
using HydraulicsService;

namespace Hydraulics.Controllers
{
    public class CriticalDepthController : Controller
    {
        //
        // GET: /CriticalDepth/

        public ActionResult Index()
        {
            DepthData_Critical model = new DepthData_Critical()
            {
                Diameter = 2,
                DesignFlow = 9.4,
                Depth = 0,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DepthData_Critical model)
        {
            CriticalDepthService service = new CriticalDepthService();
            model.Depth = service.CalculateCriticalDepth_Cricular(model.DesignFlow, model.Diameter);
            return View(model);
        }

    }
}
