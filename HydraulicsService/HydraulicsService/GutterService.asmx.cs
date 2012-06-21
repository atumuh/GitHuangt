using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HydraulicsService
{
    /// <summary>
    /// Summary description for GutterService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GutterService : System.Web.Services.WebService
    {
        internal const double GutterKc = 0.56;

        internal double ComputeFlowQ(double LongitudinalSlope, double ManningRoughness, double CrossSlope, double GutterDepth, double DepthUp = 0)
        {
            double flowQ = 0;
            flowQ = GutterKc * (Math.Pow(GutterDepth, 8.0 / 3.0) - Math.Pow(DepthUp, 8.0 / 3.0)) * Math.Sqrt(LongitudinalSlope);
            flowQ = flowQ / (ManningRoughness * CrossSlope);
            return flowQ;
        }

        [WebMethod]
        public static double GetGutterDepression(double CrossSlopeW, double CrossSlopeX, double GutterWidth)
        {
            double result = 0;
            if (CrossSlopeW > CrossSlopeX && GutterWidth > 0)
            {
                result = (CrossSlopeW - CrossSlopeX) * GutterWidth;
            }
            return result;
        }

        [WebMethod]
        public GutterResult GutterCalculation_DesignFlow(double ManningRoughness, GutterData gutterData, double DesignFlow)
        {
            try
            {
                GutterResult gutterResult = new GutterResult();
                gutterResult.DesignFlow = DesignFlow;
                if (gutterResult.DesignFlow == 0)
                {
                    gutterResult.GutterSpread = 0;
                    gutterResult.GutterDepth = 0;
                    gutterResult.GutterVelocity = 0;
                    gutterResult.Eo = 0;
                    return gutterResult;
                }

                double delta = 1E-4;
                double depth1 = 0, q1 = 0, Qx = 0;
                double depth2 = gutterData.CrossSlopeW * gutterData.GutterWidth;
                double Qw = ComputeFlowQ(gutterData.LongitudinalSlope, ManningRoughness, gutterData.CrossSlopeW, depth2);
                double q2 = Qx + Qw;
                while (q2 < gutterResult.DesignFlow)
                {
                    depth1 = depth2;
                    q1 = q2;
                    depth2 *= 2;
                    Qx = ComputeFlowQ(gutterData.LongitudinalSlope, ManningRoughness, gutterData.CrossSlopeX, depth2 - gutterData.CrossSlopeW * gutterData.GutterWidth);
                    Qw = ComputeFlowQ(gutterData.LongitudinalSlope, ManningRoughness, gutterData.CrossSlopeW, depth2, depth2 - gutterData.CrossSlopeW * gutterData.GutterWidth);
                    q2 = Qx + Qw;
                }
                double qTmp = q2;
                double depthTmp = depth2;
                if (depth1 == 0)
                {
                    Qx = 0;
                    while (Math.Abs(qTmp - gutterResult.DesignFlow) > delta)
                    {
                        depthTmp = (depth1 + depth2) / 2.0;
                        Qw = ComputeFlowQ(gutterData.LongitudinalSlope, ManningRoughness, gutterData.CrossSlopeW, depthTmp);
                        qTmp = Qw + Qx;
                        if (qTmp < gutterResult.DesignFlow)
                        {
                            depth1 = depthTmp;
                            q1 = qTmp;
                        }
                        else
                        {
                            depth2 = depthTmp;
                            q2 = qTmp;
                        }
                    }
                }
                else
                {
                    while (Math.Abs(qTmp - gutterResult.DesignFlow) > delta)
                    {
                        depthTmp = (depth1 + depth2) / 2.0;
                        Qx = ComputeFlowQ(gutterData.LongitudinalSlope, ManningRoughness, gutterData.CrossSlopeX, depthTmp - gutterData.CrossSlopeW * gutterData.GutterWidth);
                        Qw = ComputeFlowQ(gutterData.LongitudinalSlope, ManningRoughness, gutterData.CrossSlopeW, depthTmp, depthTmp - gutterData.CrossSlopeW * gutterData.GutterWidth);
                        qTmp = Qw + Qx;
                        if (qTmp < gutterResult.DesignFlow)
                        {
                            depth1 = depthTmp;
                            q1 = qTmp;
                        }
                        else
                        {
                            depth2 = depthTmp;
                            q2 = qTmp;
                        }
                    }
                }
                gutterResult.GutterDepth = depthTmp;

                gutterResult.Eo = Qw / gutterResult.DesignFlow;

                if ((gutterResult.GutterDepth - gutterData.CrossSlopeW * gutterData.GutterWidth) > 0)
                {
                    gutterResult.GutterSpread = (gutterResult.GutterDepth - gutterData.CrossSlopeW * gutterData.GutterWidth) / gutterData.CrossSlopeX + gutterData.GutterWidth;
                    gutterResult.GutterArea = ((gutterResult.GutterDepth * 2 - gutterData.CrossSlopeW * gutterData.GutterWidth) * gutterData.GutterWidth) * 0.5;
                    gutterResult.GutterArea = gutterResult.GutterArea + ((gutterResult.GutterDepth - gutterData.CrossSlopeW * gutterData.GutterWidth) * (gutterResult.GutterSpread - gutterData.GutterWidth)) * 0.5;
                }
                else
                {
                    gutterResult.GutterSpread = gutterResult.GutterDepth / gutterData.CrossSlopeW;
                    gutterResult.GutterArea = (gutterResult.GutterDepth * gutterResult.GutterSpread) * 0.5;
                }
                gutterResult.GutterVelocity = gutterResult.DesignFlow / gutterResult.GutterArea;
                return gutterResult;
            }
            catch
            {
                return new GutterResult();
            }
        }

        [WebMethod]
        public GutterResult GutterCalculation_GutterSpread(double ManningRoughness, GutterData gutterData, double GutterSpread)
        {
            try
            {
                GutterResult gutterResult = new GutterResult();
                gutterResult.GutterSpread = GutterSpread;
                if (GutterSpread == 0)
                {
                    gutterResult.DesignFlow = 0;
                    gutterResult.GutterDepth = 0;
                    gutterResult.GutterVelocity = 0;
                    gutterResult.Eo = 0;
                    return gutterResult;
                }

                if (gutterResult.GutterSpread <= gutterData.GutterWidth)
                {
                    gutterResult.GutterDepth = gutterResult.GutterSpread * gutterData.CrossSlopeW;
                    gutterResult.GutterArea = (gutterResult.GutterDepth * gutterResult.GutterSpread) * 0.5;
                    gutterResult.DesignFlow = ComputeFlowQ(gutterData.LongitudinalSlope, ManningRoughness, gutterData.CrossSlopeW, gutterResult.GutterDepth);
                    gutterResult.Eo = 1;
                }
                else
                {
                    gutterResult.GutterDepth = (gutterResult.GutterSpread - gutterData.GutterWidth) * gutterData.CrossSlopeX + gutterData.CrossSlopeW * gutterData.GutterWidth;
                    gutterResult.GutterArea = ((gutterResult.GutterDepth * 2 - gutterData.CrossSlopeW * gutterData.GutterWidth) * gutterData.GutterWidth) * 0.5;
                    gutterResult.GutterArea = gutterResult.GutterArea + ((gutterResult.GutterDepth - gutterData.CrossSlopeW * gutterData.GutterWidth) * (gutterResult.GutterSpread - gutterData.GutterWidth)) * 0.5;
                    double Qx = ComputeFlowQ(gutterData.LongitudinalSlope, ManningRoughness, gutterData.CrossSlopeX, gutterResult.GutterDepth - gutterData.CrossSlopeW * gutterData.GutterWidth);
                    double Qw = ComputeFlowQ(gutterData.LongitudinalSlope, ManningRoughness, gutterData.CrossSlopeW, gutterResult.GutterDepth, gutterResult.GutterDepth - gutterData.CrossSlopeW * gutterData.GutterWidth);
                    gutterResult.DesignFlow = Qx + Qw;
                    gutterResult.Eo = Qw / gutterResult.DesignFlow;
                }
                gutterResult.GutterVelocity = gutterResult.DesignFlow / gutterResult.GutterArea;

                return gutterResult;
            }
            catch
            {
                return new GutterResult();
            }
        }
    }

    [Serializable]
    public class GutterData
    {
        public double LongitudinalSlope { get; set; }
        public double CrossSlopeX { get; set; }
        public double CrossSlopeW { get; set; }
        public double GutterWidth { get; set; }
        public double GutterDepression
        {
            get
            {
                return GutterService.GetGutterDepression(CrossSlopeW, CrossSlopeX, GutterWidth);
            }
        }
    }

    [Serializable]
    public class GutterResult
    {
        public double DesignFlow { get; set; }
        public double GutterSpread { get; set; }
        public double GutterDepth { get; set; }
        public double GutterVelocity { get; set; }
        public double GutterArea { get; set; }
        public double Eo { get; set; }
    }
}
