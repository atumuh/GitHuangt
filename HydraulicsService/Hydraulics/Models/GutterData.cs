using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

using HydraulicsService;

namespace Hydraulics.Models
{
    public class GutterDataEx
    {
        public string Name { get; set; }
        public double ManningRoughness { get; set; }
        public double DesignFlow { get; set; }
        public GutterData gutterData { get; set; }
        public GutterResult gutterResult { get; set; }
    }

    public class InletDataEx
    {
        public string Name { get; set; }
        public double ManningRoughness { get; set; }
        public double DesignFlow { get; set; }
        public InletData inletData { get; set; }
        public InletResult inletResult { get; set; }
    }
}
