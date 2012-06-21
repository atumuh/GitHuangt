using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HydraulicsService
{
    /// <summary>
    /// Summary description for InletService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class InletService : GutterService
    {
        internal const double InchPerFt = 12;
        internal const double g = 32.16;
        internal const double DesignD = 0.3;
        internal const double Cw = 2.3;
        internal const double CWw = 3.0;
        internal const double SIFeet = 1;
        internal const double CurbK = 0.6;

        private double[] K1 = { 0, 0.03026, 1.7607, 2.2186, 0.735, 0.2814, 0.505, 0.9883 };
        private double[] K2 = { 0, 4.8506, 3.1172, 4.0314, 2.4366, 2.2776, 2.344, 2.6245 };
        private double[] K3 = { 0, -1.3132, -0.4506, -0.6245, -0.2649, -0.1789, -0.2, -0.3589 };
        private double[] K4 = { 0, 0.1522, 0.0333, 0.0556, 0.0178, 0.01, 0.0144, 0.0289 };

        [WebMethod]
        public double ComputeSplashOverVelocity(double GrateLength)
        {
            // Vo = 2.218 + 4.031*L – 0.649*L^2 + 0.056*L^3
            return K1[3] + K2[3] * GrateLength + K3[3] * Math.Pow(GrateLength, 2) + K4[3] * Math.Pow(GrateLength, 3);
        }

        [WebMethod]
        public InletResult InletCalculation(double ManningRoughness, InletData inletData, double DesignFlow)
        {
            try
            {
                InletResult inletResult = new InletResult();
                inletResult.GutterIn = GutterCalculation_DesignFlow(ManningRoughness, inletData, DesignFlow);
                inletResult.GutterOut = new GutterResult();
                inletResult.Efficiency = 1;
                if (DesignFlow == 0)
                {
                    inletResult.InterceptedFlow = 0;
                    inletResult.BypassFlow = 0;
                    inletResult.ApproachVelocity = 0;
                    inletResult.SplashoverVelocity = 0;
                    inletResult.InletDepth = 0;
                    inletResult.InletSpread = 0;
                    inletResult.InletWeirControl = false;
                    inletResult.EoEx = 0;
                    return inletResult;
                }

                double DepSlope, Dep;
                double DEPTH, Di;
                double DEPTH2, QTEST;
                double QTESTW, QTESTO;
                double LT, p;
                double DSww, DSxx, Perim;
                double Se, L;
                double KIncr = 0.01, H;
                double InLength, Sx, DepH, Rs, Kc, TOTQInLength;
                double QCInLength, QBYInLength;

                //Now compute Q captured,QC(), && Q bypassed, QBY()
                //First Compute Inlet cross slope as DepSlope!
                //Dep! is the amount of depression
                if (inletData.LocalDepression != 0)
                {
                    DepSlope = inletData.CrossSlopeW + ((inletData.LocalDepression / InchPerFt) / inletData.GutterWidth);
                    Dep = inletData.LocalDepression / InchPerFt + GetGutterDepression(inletData.CrossSlopeW, inletData.CrossSlopeX, inletData.GutterWidth);
                }
                else
                {
                    DepSlope = inletData.CrossSlopeW;
                    Dep = GetGutterDepression(inletData.CrossSlopeW, inletData.CrossSlopeX, inletData.GutterWidth);
                }

                if (inletData.InletType == EnumInletType.Drop_Curb || inletData.InletType == EnumInletType.Drop_Grate || inletData.InletType == EnumInletType.Slotted)
                {
                    DepSlope = inletData.CrossSlopeX;
                    Dep = 0;
                }
                switch (inletData.InletType)
                {
                    #region Slotted
                    case EnumInletType.Slotted:
                        {
                            switch (inletData.InletLocation)
                            {
                                #region Sag
                                case EnumInletLocation.In_Sag: //Slotted Inlet in sag. Pg 4-51 HEC-22
                                    {
                                        inletData.LongitudinalSlope = 0;
                                        if (inletData.GrateLength > 0)
                                        {
                                            //Weir Flow, 2.48 is english. 1.4 Metric
                                            DEPTH = Math.Pow(DesignFlow / (2.48 * inletData.GrateLength), 8.0 / 3.0);
                                            //Orifice
                                            DEPTH2 = Math.Pow(DesignFlow / (0.8 * inletData.GrateLength * inletData.GrateWidth), 2.0);
                                            DEPTH2 = DEPTH2 / (2 * g);
                                            if (DEPTH > DEPTH2) //Weir flow
                                            {
                                                inletResult.InletDepth = DEPTH;
                                                inletResult.GutterIn.GutterDepth = DEPTH;
                                                inletResult.InletWeirControl = true;
                                            }
                                            else //Orifice flow
                                            {
                                                inletResult.InletDepth = DEPTH2;
                                                inletResult.GutterIn.GutterDepth = DEPTH2;
                                            }
                                        }
                                        else //Design Length
                                        {
                                            //DesignD
                                            if (DesignD <= 0.4) //Design in weir control
                                            {
                                                inletData.GrateLength = DesignFlow / (2.48 * Math.Pow(DesignD, 1.5));
                                            }
                                            else //Orifice
                                            {
                                                //inletData.GrateLength = DesignFlow / (0.8 * inletData.GrateWidth * Math.Pow((2 * g * DesignD), 0.5));
                                            }
                                            DEPTH = DesignD;
                                            inletResult.InletDepth = DEPTH;
                                            inletResult.GutterIn.GutterDepth = DEPTH;
                                        }
                                        //L = 0
                                        //Compute Ponding Spread width && depth
                                        //inletResult.InletSpread  = DEPTH / DepSlope
                                        inletResult.InletSpread = inletResult.InletDepth / DepSlope;
                                        inletResult.GutterIn.GutterSpread = inletResult.InletSpread;
                                        inletResult.GutterIn.GutterVelocity = 0;

                                        inletResult.InterceptedFlow = DesignFlow;
                                        inletResult.BypassFlow = 0;
                                        inletResult.GutterOut.GutterDepth = 0;
                                        inletResult.GutterOut.GutterSpread = 0;
                                    }
                                    break;
                                #endregion
                                #region Grade
                                case EnumInletLocation.On_grade: //On Grade
                                    {
                                        //Same as Curb Inlet on Grade per HEC-22 but this uses
                                        //WinStorm 3.05 methodology
                                        //Note, limits are:
                                        //Q <= 5.5 cfs
                                        //INGS()<=.09 ft/ft
                                        //n between .011 && .017

                                        LT = CurbK * Math.Pow(DesignFlow, 0.42) * Math.Pow(inletData.LongitudinalSlope, 0.3);
                                        LT = LT * Math.Pow((1 / (ManningRoughness * inletData.CrossSlopeX)), 0.6);
                                        inletResult.Efficiency = 1 - Math.Pow((1 - inletData.GrateLength / LT), 1.8);
                                        inletResult.InterceptedFlow = DesignFlow * inletResult.Efficiency;
                                        inletResult.BypassFlow = DesignFlow - inletResult.InterceptedFlow;
                                        if (inletResult.BypassFlow < 0)
                                        {
                                            inletResult.BypassFlow = 0;
                                        }

                                        inletResult.InletSpread = inletResult.GutterIn.GutterSpread;
                                        inletResult.InletDepth = inletResult.GutterIn.GutterDepth;
                                        //Compute Bypass Spread
                                        if (inletResult.BypassFlow > 0)
                                        {
                                            inletResult.GutterOut = GutterCalculation_DesignFlow(ManningRoughness, inletData, inletResult.BypassFlow);
                                        }
                                        else
                                        {
                                            inletResult.GutterOut.GutterDepth = 0;
                                            inletResult.GutterOut.GutterSpread = 0;
                                        }

                                    }
                                    break;
                                #endregion
                            }
                        }
                        break;
                    #endregion
                    #region Curb, Drop_Curb
                    case EnumInletType.Curb:
                    case EnumInletType.Drop_Curb: //==============================
                        {
                            switch (inletData.InletLocation)
                            {
                                #region Sag
                                case EnumInletLocation.In_Sag: //curb Inlet in sag. Pg 4-49 HEC-22
                                    {
                                        Dep = Dep - (inletData.CrossSlopeW - inletData.CrossSlopeX) * inletData.GutterWidth;
                                        inletResult.InterceptedFlow = DesignFlow;
                                        inletResult.BypassFlow = 0;
                                        inletResult.GutterOut.GutterDepth = 0;
                                        inletResult.GutterOut.GutterSpread = 0;
                                        inletData.LongitudinalSlope = 0;
                                        if (inletData.InletLength > 0)   // Compute pond width
                                        {
                                            //Weir flow?
                                            if (inletData.LocalDepression / InchPerFt > 0)
                                            {
                                                //Depressed gutter
                                                DEPTH = Math.Pow((DesignFlow / (Cw * (inletData.InletLength + 1.8 * inletData.GutterWidth))), 2.0 / 3.0); //Eq (4-28)
                                                //Check limits
                                                if ((DEPTH > (inletData.CurbOpeningHigh / InchPerFt) + Dep + Dep) || inletData.InletLength > 12 * SIFeet)   //Exceeded
                                                {
                                                    DEPTH = Math.Pow((DesignFlow / (CWw * inletData.InletLength)), 2.0 / 3.0); //Eq (4-30)
                                                }
                                            }
                                            else //Non depressed
                                            {
                                                DEPTH = Math.Pow((DesignFlow / (CWw * inletData.InletLength)), 2.0 / 3.0); //Eq (4-30)
                                            }
                                            //Check for orifice flow
                                            Di = DEPTH;
                                            if (Di * InchPerFt <= inletData.CurbOpeningHigh)
                                            {
                                                //OK, weir flow
                                                DepSlope = inletData.CrossSlopeW;
                                                inletResult.InletWeirControl = true;
                                            }
                                            else
                                            {
                                                //Orifice flow, must use different eq.
                                                DEPTH2 = Math.Pow((DesignFlow / (0.67 * inletData.InletLength * ((inletData.CurbOpeningHigh / InchPerFt) + Dep))), 2);
                                                DEPTH2 = DEPTH2 / (2 * g);
                                                DEPTH2 = DEPTH2 + (inletData.CurbOpeningHigh / InchPerFt + Dep) * 0.5;
                                                if (DEPTH2 > 1.4 * inletData.CurbOpeningHigh / InchPerFt + Dep)
                                                {
                                                    //Acting as orifice
                                                    Di = DEPTH2;
                                                }
                                                else
                                                {
                                                    //Transition flow. Take highest depth
                                                    if (DEPTH2 > Di)
                                                    {
                                                        Di = DEPTH2;
                                                    }
                                                }
                                            }
                                        }
                                        else //Design inlet for 100% capture
                                        {
                                            if (inletData.LocalDepression / InchPerFt > 0)
                                            {
                                                //Depressed gutter
                                                inletData.InletLength = (DesignFlow / Math.Pow(Cw * (inletData.CurbOpeningHigh / InchPerFt - Dep), 1.5)) - 1.8 * inletData.GutterWidth;
                                                Di = (inletData.CurbOpeningHigh / InchPerFt);
                                            }
                                            else
                                            {
                                                //Non depressed
                                                inletData.InletLength = DesignFlow / Math.Pow(CWw * (inletData.CurbOpeningHigh / InchPerFt), 1.5);// Design
                                                Di = inletData.CurbOpeningHigh / InchPerFt;
                                            }
                                            if (inletData.InletLength <= 1 * SIFeet)
                                            {
                                                inletData.InletLength = 1 * SIFeet;
                                                //GoTo 100 ;//To recompute depth/spread
                                            }
                                        }

                                        //Compute Ponding Spread width && depth
                                        DSww = DepSlope * inletData.GutterWidth;
                                        if (Di <= DSww)
                                        {
                                            inletResult.InletSpread = Di / DepSlope;
                                        }
                                        else
                                        {
                                            DSxx = Di - DSww;
                                            inletResult.InletSpread = inletData.GutterWidth + DSxx / inletData.CrossSlopeX;
                                        }
                                        //Inlet Depth = Depth on gutter + Local Depression.
                                        inletResult.InletDepth = Di + inletData.LocalDepression / InchPerFt;
                                        inletResult.GutterIn.GutterDepth = Di;
                                        inletResult.GutterIn.GutterSpread = inletResult.InletSpread;
                                    }
                                    break;
                                #endregion
                                #region Grade
                                case EnumInletLocation.On_grade: //Curb Inlet on grade, Pg 4-38/9 HEC-22
                                    {
                                        inletData.GrateWidth = 0; // Curb Opening, the Grate width && length are unnecessary.
                                        inletData.GrateLength = 0; // Set to zero for avoiding affecting the calculation.
                                        //Find equivalent Cross Slope, Se
                                        //First find inletResult.GutterIn.Eo, inletResult.InletSpread, Etc.
                                        //Has nothing to do with throat ht. Go figure.
                                        GutterCalculation_SPREADCALC(ManningRoughness, inletData, DesignFlow, true);
                                        Se = inletData.CrossSlopeX + inletResult.GutterIn.Eo * (DepSlope - inletData.CrossSlopeX);

                                        LT = CurbK * Math.Pow(DesignFlow, 0.42) * Math.Pow(inletData.LongitudinalSlope, 0.3);
                                        LT = LT * Math.Pow((1 / (ManningRoughness * Se)), 0.6);
                                        if (inletData.InletLength == 0) inletData.InletLength = LT; //Design for 100% capture
                                        if (inletData.InletLength < 1 * SIFeet) inletData.InletLength = 1 * SIFeet; // 1 ft min size
                                        if (inletData.InletLength > LT) LT = inletData.InletLength;
                                        inletResult.Efficiency = 1 - Math.Pow((1 - inletData.InletLength / LT), 1.8);
                                        if (inletData.CurbOpeningHigh == 0 && inletData.LocalDepression == 0)
                                        {
                                            inletResult.InterceptedFlow = 0;
                                        }
                                        else
                                        {
                                            inletResult.InterceptedFlow = DesignFlow * inletResult.Efficiency;
                                        }
                                        inletResult.BypassFlow = DesignFlow - inletResult.InterceptedFlow;
                                        if (inletResult.BypassFlow < 0)
                                        {
                                            inletResult.BypassFlow = 0;
                                        }

                                        if (inletResult.BypassFlow > 0)
                                        {
                                            inletResult.GutterOut = GutterCalculation_DesignFlow(ManningRoughness, inletData, inletResult.BypassFlow);
                                        }
                                        else
                                        {
                                            inletResult.GutterOut.GutterDepth = 0;
                                            inletResult.GutterOut.GutterSpread = 0;
                                        }

                                        inletResult.ApproachVelocity = inletResult.GutterIn.GutterVelocity;
                                        inletResult.InletDepth = inletResult.GutterIn.GutterDepth;
                                        inletResult.InletSpread = inletResult.GutterIn.GutterSpread;
                                    }
                                    break;
                                #endregion
                            }
                        }
                        break;
                    #endregion
                    #region Grate
                    case EnumInletType.Grate: //Grate Inlet ==================
                        {
                            switch (inletData.InletLocation)
                            {
                                #region Sag
                                case EnumInletLocation.In_Sag: //Grate in sag, Pg 69 HEC-12
                                    {
                                        inletData.LongitudinalSlope = 0;

                                        if (inletData.GrateLength == 0)
                                        {
                                            //Is Grate Length = 0?
                                            ////  design at Ho = 1.0 ft.
                                            //QTEST = 0.67 * (GTL   * GTW  ) * (2 * g * 1 * SIFeet) ^ 0.5 //Orifice
                                            ////Neglect bars & side against curb
                                            //QTEST = (CWw * (1 * SIFeet) ^ 1.5) * (GTL   + 2 * GTW  ) //Weir
                                            ////Note: CWw = 3 (1.66 metric)
                                            ////Note: 1 ft is when the transition from weir to orifice flow occurs
                                            ////Note: Make Orifice Flow equal to Weir Flow, &&   solving the equation to get GTL 
                                            //GTL   = 2 * GTW   / ((0.67 * GTW   * (2 * g * 1 * SIFeet) ^ 0.5) / (CWw * (1 * SIFeet) ^ 1.5) - 1)
                                            inletData.GrateLength = ((0.67 * inletData.GrateWidth * Math.Pow((2 * g * 1 * SIFeet), 0.5)) / (CWw * Math.Pow((1 * SIFeet), 1.5)) - 1);
                                            if (inletData.GrateLength > 0) inletData.GrateLength = 2 * inletData.GrateWidth / inletData.GrateLength;
                                            if (inletData.GrateLength <= 0) inletData.GrateLength = 1 * SIFeet;
                                        }

                                        double TmpArea = inletData.GrateLength * inletData.GrateWidth;
                                        if (inletData.CurbOpeningArea <= 0 || inletData.CurbOpeningArea > TmpArea)
                                        {
                                            inletData.CurbOpeningArea = TmpArea;
                                        }

                                        //Compare grate flows using orifice & weir eqs.
                                        //Largest head controls
                                        //Orifice
                                        DEPTH = Math.Pow((DesignFlow / (0.67 * inletData.CurbOpeningArea)), 2);
                                        DEPTH = DEPTH / (2 * g);

                                        //Weir
                                        DEPTH2 = Math.Pow((DesignFlow / (CWw * (2 * inletData.GrateWidth + inletData.GrateLength))), 2.0 / 3.0);

                                        if (DEPTH2 >= DEPTH)//Weir control
                                        {
                                            DEPTH = DEPTH2;
                                            DepSlope = inletData.CrossSlopeW;
                                            Dep = (inletData.CrossSlopeW - inletData.CrossSlopeX) * inletData.GutterWidth;
                                            inletResult.InletWeirControl = true;
                                        }

                                        //Design depth is average depth across the grate, in HEC22 equation 4-26 && 4-27
                                        DSww = inletData.GrateWidth * DepSlope;
                                        if (DEPTH < DSww * 0.5)
                                        {
                                            DEPTH = DEPTH * 2;
                                        }
                                        else
                                        {
                                            //So that the depth at the curb should add inletData.GrateWidth*SlopeW*0.5
                                            DEPTH = DEPTH + DSww * 0.5;
                                        }

                                        //Compute Ponding Spread width && depth
                                        DSww = DepSlope * inletData.GutterWidth;
                                        if (DEPTH <= DSww)
                                        {
                                            inletResult.InletSpread = DEPTH / DepSlope;
                                        }
                                        else
                                        {
                                            DSxx = DEPTH - DSww;
                                            inletResult.InletSpread = inletData.GutterWidth + DSxx / inletData.CrossSlopeX;
                                        }
                                        inletResult.InletDepth = DEPTH;
                                        inletResult.GutterIn.GutterDepth = DEPTH - Dep + ((inletData.CrossSlopeW - inletData.CrossSlopeX) * inletData.GutterWidth);
                                        Dep = inletData.LocalDepression / InchPerFt;
                                        inletResult.InletDepth = inletResult.GutterIn.GutterDepth + Dep;

                                        inletResult.GutterIn.GutterSpread = inletResult.InletSpread;

                                        inletResult.InterceptedFlow = DesignFlow;
                                        inletResult.BypassFlow = 0;
                                        inletResult.GutterOut.GutterDepth = 0;
                                        inletResult.GutterOut.GutterSpread = 0;
                                    }
                                    break;
                                #endregion
                                #region Grade

                                case EnumInletLocation.On_grade: //Grate on Grade, HEC-22
                                    {
                                        //computes EoEx
                                        inletResult.EoEx = GetInletGrateEoEx_DesignFlow(inletResult.GutterIn, inletData);

                                        if (inletData.GrateLength == 0)   // Use weir eq.
                                        {
                                            //Note: Design will not capture 100%
                                            Perim = DesignFlow / (CWw * Math.Pow(inletResult.InletDepth, 1.5));
                                            //Neglect side against curb
                                            L = Perim - 2 * inletData.GrateWidth;
                                            if (L <= 0) L = 1 * SIFeet;
                                            //Design
                                            inletData.GrateLength = L;
                                        }

                                        inletResult.SplashoverVelocity = ComputeSplashOverVelocity(inletData.GrateLength); // Splash-over velocity

                                        double Rf = 1.0;
                                        if (inletResult.GutterIn.GutterVelocity > inletResult.SplashoverVelocity)
                                        {
                                            Rf = 1.0 - 0.09 * (inletResult.GutterIn.GutterVelocity - inletResult.SplashoverVelocity);
                                        }

                                        //Now compute inletResult.Efficiency factors
                                        //Frontal flow, Rf
                                        //Since HEC-22 uses unique grates, this
                                        //program assumes Rf = 1.
                                        //Compute side flow, Rs
                                        Kc = 0.15;

                                        Sx = inletData.CrossSlopeX;
                                        Rs = Sx * Math.Pow(inletData.GrateLength, 2.3);
                                        Rs = 1 + (Kc * Math.Pow(inletResult.GutterIn.GutterVelocity, 1.8) / Rs);
                                        Rs = 1 / Rs;
                                        inletResult.Efficiency = inletResult.EoEx * Rf + Rs * (1 - inletResult.EoEx);
                                        inletResult.InterceptedFlow = DesignFlow * inletResult.Efficiency;
                                        if (inletResult.InterceptedFlow > DesignFlow) inletResult.InterceptedFlow = DesignFlow;
                                        inletResult.BypassFlow = DesignFlow - inletResult.InterceptedFlow;

                                        if (inletResult.BypassFlow > 0)
                                        {
                                            inletResult.GutterOut = GutterCalculation_DesignFlow(ManningRoughness, inletData, inletResult.BypassFlow);
                                        }
                                        else
                                        {
                                            inletResult.GutterOut.GutterDepth = 0;
                                            inletResult.GutterOut.GutterSpread = 0;
                                        }

                                        inletResult.ApproachVelocity = inletResult.GutterIn.GutterVelocity;
                                        inletResult.InletDepth = inletResult.GutterIn.GutterDepth;
                                        inletResult.InletSpread = inletResult.GutterIn.GutterSpread;
                                    }
                                    break;
                                #endregion
                            }
                        }
                        break;
                    #endregion
                    #region Combination
                    case EnumInletType.Combination: //Combination ================================
                        {
                            switch (inletData.InletLocation)
                            {
                                #region Sag
                                case EnumInletLocation.In_Sag: //Combination in sag
                                    {
                                        inletData.LongitudinalSlope = 0;
                                        //Interception capacity is equal to grate only
                                        //if in weir flow. In orifice flow Qc = capacity of
                                        //curb + grate

                                        if (inletData.GrateLength == 0)
                                        {
                                            //Is Grate Length = 0?
                                            ////  design at Ho = 1.0 ft.
                                            //QTEST = 0.67 * (GTL   * GTW  ) * (2 * g * 1 * SIFeet) ^ 0.5 //Orifice
                                            ////Neglect bars & side against curb
                                            //QTEST = (CWw * (1 * SIFeet) ^ 1.5) * (GTL   + 2 * GTW  ) //Weir
                                            ////Note: CWw = 3 (1.66 metric)
                                            ////Note: 1 ft is when the transition from weir to orifice flow occurs
                                            ////Note: Make Orifice Flow equal to Weir Flow, &&   solving the equation to get GTL 
                                            //GTL   = 2 * GTW   / ((0.67 * GTW   * (2 * g * 1 * SIFeet) ^ 0.5) / (CWw * (1 * SIFeet) ^ 1.5) - 1)
                                            inletData.GrateLength = ((0.67 * inletData.GrateWidth * Math.Pow((2 * g * 1 * SIFeet), 0.5)) / (CWw * Math.Pow((1 * SIFeet), 1.5)) - 1);
                                            if (inletData.GrateLength > 0) inletData.GrateLength = 2 * inletData.GrateWidth / inletData.GrateLength;
                                            if (inletData.GrateLength <= 0) inletData.GrateLength = 1 * SIFeet;
                                        }

                                        double TmpArea = inletData.GrateLength * inletData.GrateWidth;
                                        if (inletData.CurbOpeningArea <= 0 || inletData.CurbOpeningArea > TmpArea)
                                        {
                                            inletData.CurbOpeningArea = TmpArea;
                                        }

                                        //As NO longitudinal slope of road, the sweeper combo can not be handled.
                                        //if INL   > GTL   && GTL   > 0   //Sweeper
                                        //    //Curb Inlet on slope, Pg 59 HEC-12
                                        //    //Find equivalent Cross Slope, Se
                                        //    //First find inletResult.GutterIn.Eo, inletResult.InletSpread, Etc.
                                        //      SPREADCALC()
                                        //    //Composite Gutter Sections, Pg 4-12 HEC22 Equation 4-4
                                        //    inletResult.GutterIn.Eo = 1 / (1 + ((INSW  / INSX ) / ((1 + (INSW  / INSX ) / (inletResult.GutterIn.GutterSpread  / INGW   - 1)) ^ 2.67 - 1)))
                                        //    Se = INSX  + inletResult.GutterIn.Eo * (DepSlope - INSX )
                                        //    InLength = INL   - GTL  
                                        //    TOTQInLength = TOTQ
                                        //    LT = CurbK * (TOTQInLength) ^ 0.42 * INGS  ^ 0.3
                                        //    LT = LT * (1 / (ManningRoughness * Se)) ^ 0.6
                                        //    if InLength > LT   LT = InLength
                                        //    EF = 1 - (1 - InLength / LT) ^ 1.8
                                        //    QCInLength = TOTQInLength * EF
                                        //    QBYInLength = TOTQInLength - QCInLength
                                        //    if QBYInLength < 0   QBYInLength = 0
                                        //    TOTQ = QBYInLength //What the curb did//nt get
                                        //}

                                        //Find interception
                                        //Solve for depth, H, by trial & error
                                        for (H = KIncr; H <= 5; H += KIncr)
                                        {
                                            //Grate
                                            if (H >= inletData.GrateWidth * inletData.CrossSlopeW * 0.5) //Perimeter, wetted perimeter of combination inlet doesn't include the Length of Grate which nearby Curb.
                                            {
                                                p = 2 * inletData.GrateWidth + inletData.InletLength;
                                            }
                                            else
                                            {
                                                p = 2 * inletData.GrateWidth * (2 * H / (inletData.GrateWidth * inletData.CrossSlopeW)) + inletData.InletLength - inletData.GrateLength;
                                            }
                                            QTESTW = CWw * p * Math.Pow(H, 1.5); //HEC-12, Pg 69
                                            //QTESTW = CWw * (2 * GTW   + GTL  ) * H ^ 1.5 //HEC-12, Pg 69
                                            QTESTO = 0.67 * inletData.CurbOpeningArea * Math.Pow((2 * g * H), 0.5); //HEC-12, Pg 69
                                            if (QTESTW < QTESTO)
                                            {
                                                QTEST = QTESTW; //Grate in weir flow
                                                //Neglect curb opening
                                                DepSlope = inletData.CrossSlopeW;
                                                Dep = (inletData.CrossSlopeW - inletData.CrossSlopeX) * inletData.GutterWidth;
                                                inletResult.InletWeirControl = true;
                                            }
                                            else
                                            {
                                                QTEST = QTESTO; //Grate in orifice flow
                                                QTESTW = 0;
                                                inletResult.InletWeirControl = false;
                                            }

                                            //Compute curb portion
                                            if (H > Dep && QTESTW == 0)   //Remember, d is measured from projection of Sx
                                            {
                                                DepH = H - Dep;
                                                if (H <= (inletData.CurbOpeningHigh * 1.4) / InchPerFt)
                                                {
                                                    //Curb inlet as a weir
                                                    //Which weir equation?
                                                    if (inletData.LocalDepression != 0 || (inletData.CrossSlopeW > inletData.CrossSlopeX))
                                                    {
                                                        //Depressed gutter
                                                        if (inletData.InletLength <= 12 * SIFeet)
                                                        {
                                                            QTEST = QTEST + Cw * (inletData.InletLength + 1.8 * inletData.GutterWidth) * Math.Pow(DepH, 1.5); //HEC-22, Pg 4-49
                                                        }
                                                        else
                                                        {
                                                            QTEST = QTEST + CWw * inletData.InletLength * Math.Pow(DepH, 1.5); //HEC-22, Pg 4-49
                                                        }
                                                    }
                                                    else
                                                    {
                                                        QTEST = QTEST + CWw * inletData.InletLength * Math.Pow(DepH, 1.5); //HEC-22, Pg 4-49
                                                    }
                                                }
                                                else
                                                {
                                                    //Curb inlet as an orifice
                                                    QTEST = QTEST + 0.67 * (inletData.CurbOpeningHigh / InchPerFt) * inletData.InletLength * Math.Pow((2 * g * (H - 0.5 * inletData.CurbOpeningHigh / InchPerFt)), 0.5);
                                                }
                                            }
                                            if (QTEST >= DesignFlow) break;
                                        }
                                        //Design depth is average depth across the grate, in HEC22 equation 4-26 && 4-27
                                        DSww = inletData.GrateWidth * DepSlope;
                                        if (H < DSww * 0.5)
                                        {
                                            H = H * 2;
                                        }
                                        else
                                        {
                                            //So that the depth at the curb should add inletData.GrateWidth*SlopeW*0.5
                                            H = H + DSww * 0.5;
                                        }

                                        //Compute Inlet Spread width && depth
                                        DSww = DepSlope * inletData.GutterWidth;
                                        if (H <= DSww)
                                        {
                                            inletResult.InletSpread = H / DepSlope;
                                        }
                                        else
                                        {
                                            DSxx = H - DSww;
                                            inletResult.InletSpread = inletData.GutterWidth + DSxx / inletData.CrossSlopeX;
                                        }
                                        inletResult.InletDepth = H;
                                        inletResult.GutterIn.GutterDepth = H - Dep + ((inletData.CrossSlopeW - inletData.CrossSlopeX) * inletData.GutterWidth);
                                        Dep = inletData.LocalDepression / InchPerFt;
                                        inletResult.InletDepth = inletResult.GutterIn.GutterDepth + Dep;
                                        //-------------
                                        inletResult.GutterIn.GutterSpread = inletResult.InletSpread;
                                        inletResult.InterceptedFlow = DesignFlow;
                                        inletResult.BypassFlow = 0;
                                        inletResult.GutterOut.GutterDepth = 0;
                                        inletResult.GutterOut.GutterSpread = 0;
                                    }
                                    break;
                                #endregion
                                #region Grade
                                case EnumInletLocation.On_grade: //Combination on grade
                                    {
                                        if (inletData.InletLength > inletData.GrateLength && inletData.GrateLength > 0)   //Sweeper
                                        {
                                            //Sweeper. No design options
                                            //Capacity = curb portion>grate length + the grate. Ref. HEC-22

                                            //Curb Inlet on slope, Pg 59 HEC-12
                                            //Find equivalent Cross Slope, Se
                                            //computes EoEx
                                            inletResult.EoEx = GetInletGrateEoEx_DesignFlow(inletResult.GutterIn, inletData);

                                            TOTQInLength = DesignFlow;
                                            inletResult.GutterIn.Eo = (1 + (inletData.CrossSlopeW / inletData.CrossSlopeX) / (inletResult.GutterIn.GutterSpread / inletData.GutterWidth - 1));
                                            if (inletResult.GutterIn.Eo > 0)
                                            {
                                                //Composite Gutter Sections, Pg 4-12 HEC22 Equation 4-4
                                                inletResult.GutterIn.Eo = 1 / (1 + ((inletData.CrossSlopeW / inletData.CrossSlopeX) / (Math.Pow(inletResult.GutterIn.Eo, 2.67) - 1)));
                                                Se = inletData.CrossSlopeX + inletResult.GutterIn.Eo * (DepSlope - inletData.CrossSlopeX);
                                                InLength = inletData.InletLength - inletData.GrateLength;
                                                LT = CurbK * Math.Pow(TOTQInLength, 0.42) * Math.Pow(inletData.LongitudinalSlope, 0.3);
                                                LT = LT * Math.Pow((1 / (ManningRoughness * Se)), 0.6);
                                                if (InLength > LT) LT = InLength;
                                                inletResult.Efficiency = 1 - Math.Pow((1 - InLength / LT), 1.8);
                                                QCInLength = TOTQInLength * inletResult.Efficiency;
                                            }
                                            else
                                            {
                                                QCInLength = TOTQInLength;
                                            }
                                            QBYInLength = TOTQInLength - QCInLength;
                                            if (QBYInLength < 0) QBYInLength = 0;
                                            DesignFlow = QBYInLength; //What the curb did//nt get

                                            //Next, compute captured by grate
                                            //Get Depth, spread based on new TOTQ
                                            if (DesignFlow > 0)
                                            {
                                                GutterResult GutterIn = GutterCalculation_DesignFlow(ManningRoughness, inletData, DesignFlow);
                                                double EoEx = GetInletGrateEoEx_DesignFlow(GutterIn, inletData);

                                                if (inletData.GrateLength == 0)   // Use weir eq.
                                                {
                                                    //Note: Design will not capture 100%
                                                    Perim = DesignFlow / (CWw * Math.Pow(inletResult.InletDepth, 1.5));
                                                    //Neglect side against curb
                                                    L = Perim - 2 * inletData.GrateWidth;
                                                    if (L <= 0) L = 1 * SIFeet;
                                                    //Design
                                                    inletData.GrateLength = L;
                                                }
                                                if (inletData.InletLength == 0) inletData.InletLength = inletData.GrateLength;

                                                inletResult.SplashoverVelocity = ComputeSplashOverVelocity(inletData.GrateLength); // Splash-over velocity

                                                double Rf = 1.0;
                                                if (inletResult.GutterIn.GutterVelocity > inletResult.SplashoverVelocity)
                                                {
                                                    Rf = 1.0 - 0.09 * (inletResult.GutterIn.GutterVelocity - inletResult.SplashoverVelocity);
                                                }

                                                //Now compute inletResult.Efficiency factors
                                                //Frontal flow, Rf
                                                //Since HEC-22 uses unique grates, this
                                                //program assumes Rf = 1.
                                                //Compute side flow, Rs
                                                Kc = 0.15;

                                                Sx = inletData.CrossSlopeX;
                                                Rs = Sx * Math.Pow(inletData.GrateLength, 2.3);
                                                Rs = 1 + (Kc * Math.Pow(inletResult.GutterIn.GutterVelocity, 1.8) / Rs);
                                                Rs = 1 / Rs;
                                                double Efficiency = EoEx * Rf + Rs * (1 - EoEx);
                                                double InterceptedFlow = DesignFlow * Efficiency;
                                                if (InterceptedFlow > DesignFlow) InterceptedFlow = DesignFlow;
                                                //Add the curb portion back in
                                                inletResult.InterceptedFlow = InterceptedFlow + QCInLength;
                                                if (inletResult.InterceptedFlow > TOTQInLength) inletResult.InterceptedFlow = TOTQInLength;
                                                inletResult.BypassFlow = TOTQInLength - inletResult.InterceptedFlow;
                                                inletResult.Efficiency = inletResult.InterceptedFlow / TOTQInLength;
                                            }
                                            DesignFlow = TOTQInLength;//Restore Q
                                        }
                                        else //Not a sweeper
                                        {
                                            //Capacity = the grate only. Ref. HEC-22
                                            //computes EoEx
                                            inletResult.EoEx = GetInletGrateEoEx_DesignFlow(inletResult.GutterIn, inletData);

                                            if (inletData.GrateLength == 0)   // Use weir eq.
                                            {
                                                //Note: Design will not capture 100%
                                                Perim = DesignFlow / (CWw * Math.Pow(inletResult.InletDepth, 1.5));
                                                //Neglect side against curb
                                                L = Perim - 2 * inletData.GrateWidth;
                                                if (L <= 0) L = 1 * SIFeet;
                                                //Design
                                                inletData.GrateLength = L;
                                            }
                                            if (inletData.InletLength == 0) inletData.InletLength = inletData.GrateLength;

                                            inletResult.SplashoverVelocity = ComputeSplashOverVelocity(inletData.GrateLength); // Splash-over velocity

                                            double Rf = 1.0;
                                            if (inletResult.GutterIn.GutterVelocity > inletResult.SplashoverVelocity)
                                            {
                                                Rf = 1.0 - 0.09 * (inletResult.GutterIn.GutterVelocity - inletResult.SplashoverVelocity);
                                            }

                                            //Now compute inletResult.Efficiency factors
                                            //Frontal flow, Rf
                                            //Since HEC-22 uses unique grates, this
                                            //program assumes Rf = 1.
                                            //Compute side flow, Rs
                                            Kc = 0.15;

                                            Sx = inletData.CrossSlopeX;
                                            Rs = Sx * Math.Pow(inletData.GrateLength, 2.3);
                                            Rs = 1 + (Kc * Math.Pow(inletResult.GutterIn.GutterVelocity, 1.8) / Rs);
                                            Rs = 1 / Rs;
                                            inletResult.Efficiency = inletResult.EoEx * Rf + Rs * (1 - inletResult.EoEx);
                                            inletResult.InterceptedFlow = DesignFlow * inletResult.Efficiency;
                                            if (inletResult.InterceptedFlow > DesignFlow) inletResult.InterceptedFlow = DesignFlow;
                                            inletResult.BypassFlow = DesignFlow - inletResult.InterceptedFlow;
                                        } //Sweeper inlet?

                                        if (inletResult.BypassFlow > 0)
                                        {
                                            inletResult.GutterOut = GutterCalculation_DesignFlow(ManningRoughness, inletData, inletResult.BypassFlow);
                                        }
                                        else
                                        {
                                            inletResult.GutterOut.GutterDepth = 0;
                                            inletResult.GutterOut.GutterSpread = 0;
                                        }

                                        inletResult.ApproachVelocity = inletResult.GutterIn.GutterVelocity;
                                        inletResult.InletDepth = inletResult.GutterIn.GutterDepth;
                                        inletResult.InletSpread = inletResult.GutterIn.GutterSpread;
                                    }
                                    break;
                                #endregion
                            }
                        }
                        break;
                    #endregion
                    #region Drop_Grate
                    case EnumInletType.Drop_Grate: //Drop Grate Inlet ==================
                        {
                            switch (inletData.InletLocation)
                            {
                                #region Sag
                                case EnumInletLocation.In_Sag: //Grate in sag, Pg 69 HEC-12
                                    {
                                        inletData.LongitudinalSlope = 0;

                                        if (inletData.GrateLength == 0)
                                        {
                                            //Is Grate Length = 0?
                                            ////  design at Ho = 1.0 ft.
                                            //QTEST = 0.67 * (GTL   * GTW  ) * (2 * g * 1 * SIFeet) ^ 0.5 //Orifice
                                            ////Neglect bars & side against curb
                                            //QTEST = (CWw * (1 * SIFeet) ^ 1.5) * (2 * GTL   + 2 * GTW  ) //Weir
                                            ////Note: CWw = 3 (1.66 metric)
                                            ////Note: 1 ft is when the transition from weir to orifice flow occurs
                                            ////Note: Make Orifice Flow equal to Weir Flow, &&   solving the equation to get GTL 
                                            //GTL   = 2 * GTW   / ((0.67 * GTW   * (2 * g * 1 * SIFeet) ^ 0.5) / (CWw * (1 * SIFeet) ^ 1.5) - 2)
                                            inletData.GrateLength = ((0.67 * inletData.GrateWidth * Math.Pow((2 * g * 1 * SIFeet), 0.5)) / (CWw * Math.Pow((1 * SIFeet), 1.5)) - 2);
                                            if (inletData.GrateLength > 0) inletData.GrateLength = 2 * inletData.GrateWidth / inletData.GrateLength;
                                            if (inletData.GrateLength <= 0) inletData.GrateLength = 1 * SIFeet;
                                        }

                                        double TmpArea = inletData.GrateLength * inletData.GrateWidth;
                                        if (inletData.CurbOpeningArea <= 0 || inletData.CurbOpeningArea > TmpArea)
                                        {
                                            inletData.CurbOpeningArea = TmpArea;
                                        }

                                        //Compare grate flows using orifice & weir eqs.
                                        //Largest head controls

                                        //Orifice
                                        DEPTH = Math.Pow((DesignFlow / (0.67 * inletData.CurbOpeningArea)), 2);
                                        DEPTH = DEPTH / (2 * g);

                                        //Weir
                                        //4 Sides
                                        DEPTH2 = Math.Pow((DesignFlow / (CWw * (2 * inletData.GrateWidth + 2 * inletData.GrateLength))), 2.0 / 3.0);
                                        if (DEPTH2 >= DEPTH)   //Weir control
                                        {
                                            DEPTH = DEPTH2;
                                            inletResult.InletWeirControl = true;
                                        }

                                        //Compute Ponding Spread width && depth
                                        inletResult.InletSpread = DEPTH / DepSlope;
                                        //Add the gutter || grate width & other side width
                                        if (inletData.GutterWidth > inletData.GrateWidth)
                                        {
                                            inletResult.InletSpread = inletResult.InletSpread * 2 + inletData.GutterWidth;
                                        }
                                        else
                                        {
                                            inletResult.InletSpread = inletResult.InletSpread * 2 + inletData.GrateWidth;
                                        }
                                        inletResult.InletDepth = DEPTH;
                                        inletResult.GutterIn.GutterDepth = DEPTH;
                                        inletResult.GutterIn.GutterSpread = inletResult.InletSpread;
                                        inletResult.InterceptedFlow = DesignFlow;
                                        inletResult.BypassFlow = 0;
                                        inletResult.GutterOut.GutterDepth = 0;
                                        inletResult.GutterOut.GutterSpread = 0;
                                    }
                                    break;
                                #endregion
                                #region Grade
                                case EnumInletLocation.On_grade: //Drop grate on grade. Use HEC-22 grate on grade
                                    {
                                        //Also computes inletResult.GutterIn.Eo & GutterVel
                                        if (inletData.GrateLength == 0)   // Use weir eq.
                                        {
                                            //Note: Design will not capture 100%
                                            Perim = DesignFlow / (CWw * Math.Pow(inletResult.InletDepth, 1.5));
                                            //Do not neglect side against curb
                                            L = (Perim - 2 * inletData.GrateWidth) * 0.5;
                                            if (L <= 0) L = 1 * SIFeet;
                                            //Design
                                            inletData.GrateLength = L;
                                        }

                                        //Now compute inletResult.Efficiency factors
                                        //Frontal flow, Rf
                                        //Since HEC-22 uses unique grates, this
                                        //program assumes Rf = 1.

                                        //Compute side flow, Rs
                                        Kc = 0.15;
                                        //Per HEC-22, Pg 4-67, Eq. 4-19, Chart 6
                                        if (inletData.GutterWidth > inletData.GrateWidth)
                                        {
                                            Sx = 0.01;
                                        }
                                        else
                                        {
                                            Sx = inletData.CrossSlopeW;
                                        }

                                        Rs = Sx * Math.Pow(inletData.GrateLength, 2.3);
                                        Rs = 1 + (Kc * Math.Pow(inletResult.GutterIn.GutterVelocity, 1.8) / Rs);
                                        Rs = 1 / Rs;
                                        inletResult.Efficiency = inletResult.GutterIn.Eo * 1 + Rs * (1 - inletResult.GutterIn.Eo);
                                        inletResult.InterceptedFlow = DesignFlow * inletResult.Efficiency;
                                        if (inletData.GrateWidth == 0) inletResult.InterceptedFlow = 0;
                                        if (inletResult.InterceptedFlow > DesignFlow) inletResult.InterceptedFlow = DesignFlow;
                                        inletResult.BypassFlow = DesignFlow - inletResult.InterceptedFlow;

                                        inletResult.GutterIn.GutterSpread = inletResult.InletSpread;
                                        inletResult.GutterIn.GutterDepth = inletResult.InletDepth;
                                        if (inletResult.BypassFlow > 0)
                                        {
                                            inletResult.GutterOut = GutterCalculation_DesignFlow(ManningRoughness, inletData, inletResult.BypassFlow);
                                        }
                                        else
                                        {
                                            inletResult.GutterOut.GutterDepth = 0;
                                            inletResult.GutterOut.GutterSpread = 0;
                                        }
                                    }
                                    break;
                                #endregion

                            }
                        }
                        break; //Grade || sag
                    #endregion
                }
                return inletResult;
            }
            catch
            {
                return new InletResult();
            }
        }

        [WebMethod]
        public GutterResult GutterCalculation_SPREADCALC(double ManningRoughness, InletData gd, double DesignFlow, bool bIgnoreLocalDepression = false)
        {
            //double TempSlope = DepSlope;
            //if (bIgnoreLocalDepression == true)
            //{
            //    DepSlope = INSW; // DepSlope as used in SPREADCALC should be INSW;  See HEC22 Sect 4.4.4
            //}
            GutterResult gr = GutterCalculation_DesignFlow(ManningRoughness, gd, DesignFlow);
            //if (bIgnoreLocalDepression == true)
            //{
            //    InletDepth = InletDepth + DEPINL / InchPerFt; // Add back ignored local depression
            //    DepSlope = TempSlope; // DepSlope needs to be re-set for local depression
            //}
            return gr;
        }

        [WebMethod]
        public double GetInletGrateEoEx_DesignFlow(GutterResult gutterResult, InletData inletData)
        {
            double EoEx = gutterResult.Eo;
            if (inletData.GutterWidth > inletData.GrateWidth && gutterResult.GutterSpread > inletData.GrateWidth && inletData.GrateWidth > 0)
            {
                double area1 = 0.5 * inletData.GrateWidth * (gutterResult.GutterDepth * 2.0 - inletData.CrossSlopeW * inletData.GrateWidth);
                double width = (inletData.GutterWidth < gutterResult.GutterSpread) ? inletData.GutterWidth : gutterResult.GutterSpread;
                double area2 = 0.5 * width * (gutterResult.GutterDepth * 2.0 - inletData.CrossSlopeW * width);
                EoEx *= area1 / area2;
            }
            return EoEx;
        }
    }

    [Serializable]
    public enum EnumInletType
    {
        NONE = 0,
        Drop_Curb,
        Drop_Grate,
        Slotted,
        Curb,
        Grate,
        Combination,
    }

    [Serializable]
    public enum EnumInletLocation
    {
        NONE = 0,
        In_Sag,
        On_grade,
    }

    [Serializable]
    public enum EnumGrateType
    {
        NONE = 0,
        CURVED_VANE,
        PARALLEL_BAR_P_1_1_BY_8,
        PARALLEL_BAR_P_1_7_BY_8,
        PARALLEL_BAR_P_1_7_BY_8_4,
        RECTICULIN,
        TILT_BAR_30_DEG,
        TILE_BAR_45_DEG,
    }

    [Serializable]
    public class InletData : GutterData
    {
        public EnumInletLocation InletLocation { get; set; }
        public EnumInletType InletType { get; set; }
        public EnumGrateType GrateType { get; set; }
        public double GrateWidth { get; set; }
        public double GrateLength { get; set; }
        public double InletLength { get; set; }
        public double CurbOpeningHigh { get; set; }
        public double CurbOpeningArea { get; set; }
        public double LocalDepression { get; set; }
    }

    [Serializable]
    public class InletResult
    {
        public GutterResult GutterIn;
        public GutterResult GutterOut;
        public double EoEx;
        public double InterceptedFlow;
        public double BypassFlow;
        public double ApproachVelocity;
        public double SplashoverVelocity;
        public double Efficiency;
        public double InletDepth;
        public double InletSpread;
        public bool InletWeirControl;
    }

    [Serializable]
    public enum EnumInlMethod
    {
        NONE = 0,
        Q_vs_Depth,
        Known_Q,
    }
}
