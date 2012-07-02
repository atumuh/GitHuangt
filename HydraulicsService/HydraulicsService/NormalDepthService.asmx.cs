using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HydraulicsService
{
    /// <summary>
    /// Summary description for NormalDepthService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NormalDepthService : System.Web.Services.WebService
    {
        internal const double ManEq = 1.486;
        [WebMethod]
        public double CalculateNormalDepth_Cricular(double ManningRoughness, double DesignFlow, double Diameter, double ElevationUp, double ElevationDown, double Length)
        {
            double NDepth = 0;
            double SLINV = (ElevationUp - ElevationDown) / Length;
            if (SLINV <= 0)
            {
                NDepth = Diameter;
                return NDepth;
            }
            double CHECK = (DesignFlow * ManningRoughness) / (ManEq * Math.Pow(SLINV, 0.5));
            double T = 0.05;
            for (; T < Math.PI * 2; T += 0.01)
            {
                double H = (Diameter / 2) * (1 - Math.Cos(T / 2));
                double ARDN = (Math.Pow(Diameter, 2) / 8) * (T - Math.Sin(T));
                double HRDN = (Diameter / 4) * (1 - (Math.Sin(T) / T));
                if (ARDN * Math.Pow(HRDN, 0.667) >= CHECK)
                {
                    NDepth = H;
                    return NDepth;
                }
            }

            NDepth = Diameter;
            return NDepth;
        }
    }
}
