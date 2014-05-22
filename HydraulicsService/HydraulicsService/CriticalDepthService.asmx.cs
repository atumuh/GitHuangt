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
        internal const double PI = 3.14159265358979323846;
        internal const double g = 32.16;
        internal const double TOLERANCE = 1e-8;
        internal const double k_AngleStart = 0.05;
        internal const double k_Tolerance = 0.001;
        internal const int k_IterationTimes = 10000;

        private double GetDepthByAngle(double angleT, double diameter)
        {
            return ((diameter * 0.5) * (1 - Math.Cos(angleT * 0.5)));
        }

        private double GetAreaByAngle(double angleT, double diameter)
        {
            return ((diameter * diameter) * 0.125) * (angleT - Math.Sin(angleT));
        }

        private double GetTopWidthByDepth(double depth, double diameter)
        {
            return (diameter * Math.Sin(Math.Acos((((depth * 2) - diameter) / diameter))));
        }

        private double ComputeFroudeNumber(double designFlow, double topWidth, double area)
        {
            return Math.Sqrt((Math.Pow(designFlow, 2.0) * topWidth) / (Math.Pow(area, 3.0) * g));
        }

        private double CalculateFroudeNumber(double angleT, double designFlow, double diameter)
        {
            double depth = GetDepthByAngle(angleT, diameter);
            double area = GetAreaByAngle(angleT, diameter);
            double topWidth = GetTopWidthByDepth(depth, diameter);
            double froudeNumber = ComputeFroudeNumber(designFlow, topWidth, area);
            return froudeNumber;
        }

        private bool FuzzyGreaterThanZero(double value)
        {
            return value > TOLERANCE;
        }

        [WebMethod]
        public double CalculateCriticalDepth_Cricular(double designFlow, double diameter)
        {
            if (!FuzzyGreaterThanZero(designFlow) ||
                !FuzzyGreaterThanZero(diameter))
            {
                return 0;
            }

            double froudeNumber = 1.0;
            double angleT1 = k_AngleStart;
            double dist1 = Math.Pow(CalculateFroudeNumber(angleT1, designFlow, diameter), 2.0) - froudeNumber;
            double angleT2 = PI * 2;
            double dist2 = Math.Pow(CalculateFroudeNumber(angleT2, designFlow, diameter), 2.0) - froudeNumber;
            if (FuzzyGreaterThanZero(dist1 * dist2))
            {
                return diameter;
            }

            double angleT = (angleT1 + angleT2) / 2.0;
            double dist = Math.Pow(CalculateFroudeNumber(angleT, designFlow, diameter), 2.0) - froudeNumber;
            int iTimes = 0;
            // Critical Depth is when Froude's Number = 1.0 +/- 0.001
            while (FuzzyGreaterThanZero(Math.Abs(dist) - k_Tolerance))
            {
                if (FuzzyGreaterThanZero(dist * dist1))
                {
                    angleT1 = angleT;
                }
                else
                {
                    angleT2 = angleT;
                }
                angleT = (angleT1 + angleT2) / 2.0;
                dist = Math.Pow(CalculateFroudeNumber(angleT, designFlow, diameter), 2.0) - froudeNumber;
                iTimes++;
                if (iTimes > k_IterationTimes)
                {
                    return diameter;
                }
            }

            return GetDepthByAngle(angleT, diameter);
        }
    }
}
