using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

using HydraulicsService;

namespace Hydraulics.Models
{
    public class DepthData_Critical
    {
        public double  DesignFlow { get; set; }
        public double  Diameter { get; set; }
        public double  Depth { get; set; }
    }

    public class DepthData_Normal
    {
        public double  ManningRoughness { get; set; }
        public double  DesignFlow { get; set; }
        public double  Diameter { get; set; }
        public double  ElevationUp { get; set; }
        public double  ElevationDown { get; set; }
        public double  Length { get; set; }
        public double  Depth { get; set; }
    }
}
