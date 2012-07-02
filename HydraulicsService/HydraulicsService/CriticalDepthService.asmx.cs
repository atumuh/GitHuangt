using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HydraulicsService
{
    /// <summary>
    /// Summary description for CriticalDepthService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CriticalDepthService : System.Web.Services.WebService
    {
        internal const double g = 32.16;

        [WebMethod]
        public double CalculateCriticalDepth_Cricular(double DesignFlow, double Diameter)
        {
            double T = 0.5;
            //Compute cir. critical depth by traditional method
            for (; T < Math.PI * 2; T += 0.001)
            {
                double H = (Diameter * 0.5) * (1 - Math.Cos(T * 0.5)); // Hydraulic Depth (HGL) as a function of angle T
                double Area = ((Diameter * Diameter) * 0.125) * (T - Math.Sin(T)); // Cross-sectional Area as a function of T and radius
                double TCR = T; // Capture TCR
                // Critical Depth is when Froude//s Number = 1.0 +/- 0.001
                //// TW = (2 * (H * (Diameter   - H))) ^ 0.5
                double TW = Diameter * Math.Sin(Math.Acos((((2 * H) - Diameter) / Diameter)));
                double F = ((Math.Pow(DesignFlow, 2) * TW) / (Math.Pow(Area, 3) * g));
                if (Math.Abs(1.0 - F) < 0.005)  // Critical Depth is when Froude//s Number = 1.0 +/- 0.001
                {
                    return H;
                }
            }

            return Diameter;
        }
    }
}
