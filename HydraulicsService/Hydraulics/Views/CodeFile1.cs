using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

using HydraulicsService;
//using System.Web.UI.WebControls;

public class RGB
{
    private RGB() { }
    public RGB(int R, int G, int B) { }
}

public class Point
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}

public class Canvas
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double CurrentX { get; set; }
    public double CurrentY { get; set; }
    public double lineWidth { get; set; }
    public double ScaleLeft { get; set; }
    public double ScaleWidth { get; set; }
    public double ScaleTop { get; set; }
    public double ScaleHeight { get; set; }
    public RGB FillColor { get; set; }
    public RGB ForeColor { get; set; }
    public double FillStyle { get; set; }
    public double DrawStyle { get; set; }
    public double DrawMode { get; set; }
    public double TextHeight(string str) { return 1; }
    public double TextWidth(string str) { return 1; }
    public void Print(string str) { }
    public void Cls() { }
    public void Scale() { }
    public void Scale(double SX1, double SY1, double SX2, double SY2) { }
    public void Circle(double x, double y, double rad, RGB c) { }
    public void PSet(double x1, double y1) { }
    public void lineTo(double x1, double y1) { }
    public void lineTo(double x1, double y1, RGB c) { }
    public void lineTo(double x1, double y1, double x2, double y2, RGB c) { }
    public void lineTo(bool b1, double x1, double y1, bool b2, double x2, double y2, RGB c) { }
    public void CreateHydroScreenGraphics() { }
    public void EndDraw() { }
}

public class TempClass
{
    public void SetScroll() { }
    public void ConvertToPixels() { }
    public void InPCurvePlot() { }
    public void BypSpreadDrop() { }
    public void PlotInlet(Canvas context, EnumInletType InlType, EnumInletLocation INLSAG, bool PCBut, bool ThreeDBut, bool SPINSCALEUP, bool SPINSCALEDOWN, bool SPINXLEFT,
        bool SPINXRIGHT, bool REDO, bool EGLBUT, bool OutlineBut, bool InCurrentCalc, bool DiagBut, bool ZOOMOUTBUT, bool ZOOMBUT, bool REDRAW,
        double ScaleXX, double ScaleYY, double ISpreadWidth, double IGutterWidth, double INGW, double XRed, double INSW, double InlISpread,
        double INTH, double INSX, double InlGVel, double InlSplashOverVel, double SIFeet, double INL, bool ImZoomed, bool ZoomOn, double DEPINL,
        double GTL, double GTW, double DInletLength, double InlGDepth, double InlIDepth, double InlBypDepth, double InlBypSpread, double InlGSpread,
        double InlQBY, double YScaleFactor, double XScaleFactor, double SX1, double SY1, double SX2, double SY2, double PFactorX, double PFactorY,
        double Dep, double ZDepthAdd, double DepSlope, double WedgeX)
    {
        double InchPerFt = 12, DesignD = 1;
        int Z, Fuck = 0, i;
        double Yp, Chec, TopInletWidth, TopCurbWidth, ZDepth, Y1L, X1L, LabelX, Radi;
        double Xu, Yu, Zu, ZMin, ZMax, Xt, Yt, Zt, H, K;
        List<Point> Gut = new List<Point>();
        List<Point> Inlet = new List<Point>();
        List<Point> Water = new List<Point>();
        List<Point> Depr = new List<Point>();
        List<Point> Grate = new List<Point>();
        List<RGB> QBColor = new List<RGB>();
        string Text = string.Empty, DimM = string.Empty;
        //int PictureIndex;

        #region Start
        if (PCBut == true)
        {
            ThreeDBut = false;
            SPINSCALEUP = false;
            SPINSCALEDOWN = false;
            SPINXLEFT = false;
            SPINXRIGHT = false;
            REDO = false;
        }
        else
        {
            ThreeDBut = true;
            SPINSCALEUP = true;
            SPINSCALEDOWN = true;
            SPINXLEFT = true;
            SPINXRIGHT = true;
            REDO = true;
        }

        EGLBUT = false;
        OutlineBut = false;

        if (InCurrentCalc == false || DiagBut == true)
        { //Blank screen
            //SCROLLX  = ImZoomed;
            //SCROLLY  = ImZoomed;
            //ScrollPanel  = ImZoomed;
            ZOOMOUTBUT = false;
            ZOOMBUT = false;
            REDRAW = false;
            //Pic1.Width = Panel.Width;
            //Pic1.Height = VB6.TwipsToPixelsY(Pic1Ht);
            context.Cls();
            context.Scale();
            //if (PCBut == true)
            //{
            //    PictureIndex = 8; //Same as Channel
            //}
            //else
            //{
            //    switch (InlType)
            //    {
            //        case EnumInletType.Curb:
            //            PictureIndex = 9;
            //            break;
            //        case EnumInletType.Grate:
            //            PictureIndex = 10;
            //            break;
            //        case EnumInletType.Combination:
            //            PictureIndex = 11;
            //            break;
            //        case EnumInletType.Drop_Curb:
            //            PictureIndex = 12;
            //            break;
            //        case EnumInletType.Drop_Grate:
            //            PictureIndex = 13;
            //            break;
            //        case EnumInletType.Slotted:
            //            PictureIndex = 14;
            //            break;
            //    }
            //}
            //Pic2(PictureIndex).Left = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Pic1.Width) - VB6.PixelsToTwipsX(Pic2(PictureIndex).Width)) * 0.5);
            //Pic2(PictureIndex).Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(Pic1.Height) - VB6.PixelsToTwipsY(Pic2(PictureIndex).Height)) * 0.5);
            //Pic2(PictureIndex)  = true;
            return;
        }
        else
        {
            context.Cls();
            REDRAW = true;
            ZOOMOUTBUT = true;
            ZOOMBUT = true;
        }

        if (PCBut == true)
        {
            InPCurvePlot();
            return;
        }

        //Plot ---------------------------------------------------

        //Plot using
        //InlQ  
        //InlQC  
        //InlQBY  
        //InlISpread 
        //InlIDepth  
        //InlGSpread  
        //InlGDepth 
        //InlBypDepth  
        //InlBypSpread  

        if (YScaleFactor <= 0) { YScaleFactor = 1; }
        if (XScaleFactor <= 0) { XScaleFactor = 1; }

        //PFactorX = 0
        //PFactorY = 0


        TopInletWidth = 0.333 * SIFeet; //4-inches
        TopCurbWidth = 1;

        ThreeDBut = true;
        SPINSCALEUP = true;
        SPINSCALEDOWN = true;
        SPINXLEFT = true;
        SPINXRIGHT = true;
        REDO = true;
        EGLBUT = true;
        ZOOMBUT = true;
        ZOOMOUTBUT = true;
        REDRAW = true;

        //Draw
        context.CreateHydroScreenGraphics();
        context.DrawStyle = 0;
        context.FillStyle = 1;
        context.lineWidth = 2;
        //Pic1.FillColor = new RGB(0, 0, 0); //Black

        //Compute scale
        context.Cls();

        context.Scale();
        //Get scales for different inlet types
        //Use InlNPoints for max scale
        switch (InlType)
        {
            case EnumInletType.Curb ^ EnumInletType.Grate ^ EnumInletType.Combination ^ EnumInletType.Slotted:
                {
                    Chec = 0;
                    switch (InlType)
                    {
                        case EnumInletType.Curb ^ EnumInletType.Grate ^ EnumInletType.Combination ^ EnumInletType.Slotted:
                            if (InlISpread > Chec)
                            {
                                Chec = InlISpread;
                            }
                            break;
                    }
                    if (Chec < 5 * SIFeet) { Chec = 5 * SIFeet; }
                    ScaleXX = Chec * 1.25; //1.5 in Storm Sewers

                    Chec = 1 * SIFeet;
                    switch (InlType)
                    {
                        case EnumInletType.Curb ^ EnumInletType.Grate ^ EnumInletType.Combination ^ EnumInletType.Slotted:
                            if (InlIDepth > Chec)
                            {
                                Chec = InlIDepth;
                            }
                            break;
                    }
                    if (Chec < DesignD) { Chec = DesignD; } //See Codes
                    ScaleYY = Chec * 2.75; //3
                }
                break;

            case EnumInletType.Drop_Curb ^ EnumInletType.Drop_Grate:
                {
                    Chec = 0;
                    switch (InlType)
                    {
                        case EnumInletType.Drop_Curb ^ EnumInletType.Drop_Grate:
                            if (InlType == EnumInletType.Drop_Curb)
                            {
                                Chec = InlISpread * 2 + INL * 0.25;
                            }
                            else
                            {
                                Chec = InlISpread;
                            }
                            break;
                    }
                    if (Chec > 30 * SIFeet) { Chec = 30 * SIFeet; }
                    ScaleXX = Chec * 1;

                    Chec = 0;
                    if (InlType == EnumInletType.Drop_Curb || InlType == EnumInletType.Drop_Grate)
                    {
                        Chec = InlIDepth;
                    }
                    if (Chec < DesignD) { Chec = DesignD; } //See Codes
                    if (INLSAG == EnumInletLocation.On_grade)
                    {
                        ScaleYY = Chec * 4;
                    }
                    else
                    {
                        ScaleYY = Chec * 6;
                    }
                }
                break;
        }

        context.Scale();
        //if (ImZoomed == false){
        //        Pic1.Width = Panel.Width;
        //        Pic1.Height = VB6.TwipsToPixelsY(Pic1Ht);
        //}
        //    else
        //    {
        //        Pic1.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(Grid.Left) - VB6.PixelsToTwipsX(Grid.Width) * 1.05 - 1.5 * VB6.PixelsToTwipsX(SCROLLY.Width));
        //        Pic1.Height = VB6.TwipsToPixelsY(Pic1Ht - VB6.PixelsToTwipsY(SCROLLX.Height));
        //        SCROLLY.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Pic1.Left) + VB6.PixelsToTwipsX(Pic1.Width));
        //        SCROLLY.Top = Pic1.Top;
        //        SCROLLY.Height = Pic1.Height;
        //        SCROLLX.Left = Pic1.Left;
        //        SCROLLX.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Pic1.Top) + VB6.PixelsToTwipsY(Pic1.Height));
        //        SCROLLX.Width = Pic1.Width;
        //}

        //SCROLLX  = ImZoomed;
        //SCROLLY  = ImZoomed;
        //ScrollPanel.Top = SCROLLX.Top;
        //ScrollPanel.Left = SCROLLY.Left;
        //ScrollPanel.Width = SCROLLY.Width;
        //ScrollPanel.Height = SCROLLX.Height;
        //ScrollPanel  = ImZoomed;


        ScaleYY = ScaleYY * YScaleFactor;
        ScaleXX = ScaleXX * XScaleFactor;

        //if ( ZoomOn = false && ImZoomed = false && AlreadySet = false; {
        if (ZoomOn == false && ImZoomed == false)
        {
            SX1 = -0.5 * ScaleXX;
            SX2 = ScaleXX * 1.5;
            SY1 = ScaleYY;
            SY2 = -0.25 * ScaleYY;
            SetScroll();
            //AlreadySet = true;
        }

        context.Scale(SX1, SY1, SX2, SY2);

        if (ImZoomed == false && ThreeDBut == true)
        {
            //Pic1.Font = VB6.FontChangeSize(Pic1.Font, 10);
            //Pic1.Font = VB6.FontChangeBold(Pic1.Font, true);
            //Pic1.Font = VB6.FontChangeUnderline(Pic1.Font, true);
            //Pic1.ForeColor = QBColor ;
            //Text = My.Resources.MainMenu_Inlet_Section;
            context.CurrentX = context.ScaleLeft + context.ScaleWidth * 0.5 - 0.5 * context.TextWidth(Text);
            context.CurrentY = context.ScaleTop - System.Math.Abs(context.ScaleHeight * 0.95) - 1 * context.TextHeight(Text);
            context.Print(Text);
            if (INLSAG == EnumInletLocation.On_grade)
            {
                //Pic1.Font = VB6.FontChangeSize(Pic1.Font, 8);
                //Pic1.Font = VB6.FontChangeBold(Pic1.Font, false);
                //Pic1.Font = VB6.FontChangeUnderline(Pic1.Font, false);
                //Pic1.ForeColor = QBColor ;
                //Text = My.Resources.MainMenu_Looking_downstream;
                context.CurrentX = context.ScaleLeft + context.ScaleWidth * 0.5 - 0.5 * context.TextWidth(Text);
                context.CurrentY = context.ScaleTop - System.Math.Abs(context.ScaleHeight * 0.95); //- 2.5 * Pic1.TextHeight(Text$)
                context.Print(Text);
            }
        }

        //if ( ThreeDBut == false) {
        //    ToolTip1.SetToolTip(SPINSCALEUP, My.Resources.MainMenu_Adjust_Y_Projection);
        //    ToolTip1.SetToolTip(SPINSCALEDOWN, My.Resources.MainMenu_Adjust_Y_Projection);
        //    ToolTip1.SetToolTip(SPINXLEFT, My.Resources.MainMenu_Adjust_X_Projection);
        //    ToolTip1.SetToolTip(SPINXRIGHT, My.Resources.MainMenu_Adjust_X_Projection);
        //}else{ //2D
        //    ToolTip1.SetToolTip(SPINSCALEUP, My.Resources.MainMenu_Adjust_Y_Scale);
        //    ToolTip1.SetToolTip(SPINSCALEDOWN, My.Resources.MainMenu_Adjust_Y_Scale);
        //    ToolTip1.SetToolTip(SPINXLEFT, My.Resources.MainMenu_Adjust_X_Scale);
        //    ToolTip1.SetToolTip(SPINXRIGHT, My.Resources.MainMenu_Adjust_X_Scale);
        //}

        context.lineWidth = 2;
        //Pic1.Font = VB6.FontChangeSize(Pic1.Font, 8);
        //Pic1.Font = VB6.FontChangeBold(Pic1.Font, false);
        //Pic1.Font = VB6.FontChangeUnderline(Pic1.Font, false);

        //Draw Structures ===============================

        X1L = ScaleXX * 0.25;
        Y1L = ScaleYY * 0.25; //Gutter invert

        //Fill/Outline Color assign
        RGB Concrete = new RGB(244, 244, 244); //Lt Gray
        RGB GutterColor = new RGB(100, 100, 100); //Dk Gray
        RGB WaterFill = new RGB(146, 219, 255); //Lt blu
        RGB WaterColor = new RGB(49, 106, 197);
        RGB InletColor = new RGB(50, 50, 50); //Outline almost black

        switch (InlType)
        {
            case EnumInletType.Curb ^ EnumInletType.Grate ^ EnumInletType.Combination ^ EnumInletType.Slotted:
                {
                    if (PFactorX == 0) { PFactorX = 0.55; } //See also REDO_Click
                    if (PFactorY == 0) { PFactorY = 0.65; }
                }
                break;
            case EnumInletType.Drop_Curb:
                {
                    if (PFactorX == 0) { PFactorX = 0.7; } //See also REDO_Click
                    if (PFactorY == 0) { PFactorY = 0.85; }
                }
                break;
            case EnumInletType.Drop_Grate: //Drops need centered
                {
                    if (PFactorX == 0)
                    {
                        if (INLSAG == EnumInletLocation.On_grade)
                        {
                            PFactorX = 0.5;
                        }
                        else
                        {
                            PFactorX = 0.7;
                        }
                    }
                    if (PFactorY == 0)
                    {
                        if (INLSAG == EnumInletLocation.On_grade)
                        {
                            PFactorY = 0.65; //0.5
                        }
                        else
                        {
                            PFactorY = 0.85;
                        }
                    }
                }
                break;
        }

        Xu = ScaleXX * PFactorX;
        Yu = ScaleYY * PFactorY;
        switch (InlType)
        {
            case EnumInletType.Drop_Curb:
                {
                    Zu = -25;
                    if (System.Math.Abs(Zu) < InlISpread) { Zu = InlISpread * -1; }
                }
                break;
            case EnumInletType.Drop_Grate:
                {
                    if (INLSAG == EnumInletLocation.In_Sag)
                    {
                        Zu = -18;
                        if (System.Math.Abs(Zu) < InlISpread) { Zu = InlISpread * -1; }
                    }
                    else
                    {
                        Zu = -6;
                    }
                }
                break;
            default:
                Zu = -6;
                break;
        }

        ZMin = 0.9 * Zu; //Towards viewer //Was .95
        ZMax = -8 * Zu; //Away from viewer

        //Dep! is the amount of depression
        switch (InlType)
        {
            case EnumInletType.Curb ^ EnumInletType.Grate ^ EnumInletType.Combination: // ^ EnumInletType.Slotted
                {
                    if (DEPINL != 0 && INGW != 0)
                    {
                        DepSlope = INSW + ((DEPINL / InchPerFt) / INGW);
                        Dep = DEPINL / InchPerFt;
                    }
                    else
                    {
                        DepSlope = INSW;
                        Dep = 0;
                    }
                }
                break;
        }

        ////Ignore the Local inlet depression when weir flow
        //if ( InlWeirControl  = true && INLSAG == EnumInletLocation.In_Sag && (InlType == EnumInletType.Combination || InlType == EnumInletType.Grate) {
        //    DepSlope = INSW 
        //    Dep = 0
        //}

        switch (InlType)
        {
            case EnumInletType.Curb ^ EnumInletType.Combination:
                Yp = Dep + INTH / InchPerFt + TopInletWidth;
                break;
            default:
                Yp = InlIDepth;
                break;
        }
        if (Yp < 0.5 * SIFeet) { Yp = 0.5 * SIFeet; }

        XRed = InlISpread * 1.05 / ScaleXX; //; i <= keep line from going off the screen
        if (XRed < 0.75) { XRed = 0.75; }//Keeps drawing big enough
        ZDepth = 0;
        #endregion

        switch (InlType)
        {
            case EnumInletType.Curb ^ EnumInletType.Grate ^ EnumInletType.Combination ^ EnumInletType.Slotted:
                #region Curb, Grate, Combination, Slotted
                {
                    switch (InlType)
                    {
                        case EnumInletType.Curb ^ EnumInletType.Combination:
                            ZDepth = INL;
                            ZDepthAdd = 1 * SIFeet;
                            break;
                        case EnumInletType.Grate ^ EnumInletType.Slotted:
                            ZDepth = GTL;
                            ZDepthAdd = 1 * SIFeet;//0
                            break;
                    }
                    if (ZDepth == 0) { ZDepth = DInletLength; }

                    //Gutter coords
                    Gut[1].X = X1L - 3 * TopCurbWidth; //Back curb
                    Gut[1].Y = Y1L + Yp;
                    Gut[1].Z = ZMin;
                    Gut[2].X = X1L - 3 * TopCurbWidth;
                    Gut[2].Y = Y1L + Yp;
                    Gut[2].Z = ZMax;
                    Gut[3].X = X1L; //Top of curb
                    Gut[3].Y = Y1L + Yp;
                    Gut[3].Z = ZMax;
                    Gut[4].X = X1L;
                    Gut[4].Y = Y1L + Yp;
                    Gut[4].Z = ZMin;
                    Gut[5].X = X1L; //Gutter flowline
                    Gut[5].Y = Y1L;
                    Gut[5].Z = ZMin;
                    Gut[6].X = X1L;
                    Gut[6].Y = Y1L;
                    Gut[6].Z = 0 - ZDepthAdd;
                    Gut[7].X = X1L; //Sw meets Sx
                    Gut[7].Y = Y1L + INGW * (INSW - DepSlope);
                    Gut[7].Z = 0;
                    Gut[8].X = X1L;
                    Gut[8].Y = Y1L + INGW * (INSW - DepSlope);
                    Gut[8].Z = ZDepth;
                    Gut[9].X = X1L;
                    Gut[9].Y = Y1L;
                    Gut[9].Z = ZDepth + ZDepthAdd;
                    Gut[10].X = X1L;
                    Gut[10].Y = Y1L;
                    Gut[10].Z = ZMax;
                    Gut[11].X = X1L + INGW;
                    Gut[11].Y = Y1L + INGW * INSW;
                    Gut[11].Z = ZMax;
                    Gut[12].X = X1L + INGW;
                    Gut[12].Y = Y1L + INGW * INSW;
                    Gut[12].Z = ZMin;
                    Gut[13].X = X1L + INGW + ScaleXX * XRed;
                    Gut[13].Y = Gut[12].Y + ScaleXX * XRed * INSX;
                    Gut[13].Z = ZMin;
                    Gut[14].X = X1L + INGW + ScaleXX * XRed;
                    Gut[14].Y = Gut[13].Y;
                    Gut[14].Z = ZMax;
                    //Add depression lines
                    if (DEPINL > 0)
                    {
                        Gut[15].X = X1L + INGW;
                        Gut[15].Y = Y1L + INGW * INSW;
                        Gut[15].Z = 0;
                        Gut[16].X = X1L + INGW;
                        Gut[16].Y = Y1L + INGW * INSW;
                        Gut[16].Z = ZDepth;
                    }
                    else
                    {
                        Gut[15].X = Gut[14].X;
                        Gut[15].Y = Gut[14].Y;
                        Gut[15].Z = Gut[14].Z;
                        Gut[16].X = Gut[14].X;
                        Gut[16].Y = Gut[14].Y;
                        Gut[16].Z = Gut[14].Z;
                    }
                    //Fill Location
                    Gut[17].X = Gut[5].X + (Gut[13].X - Gut[5].X);
                    Gut[17].Y = Gut[5].Y;
                    Gut[17].X = 0;

                    if (ThreeDBut == false)
                    {
                        //Transform
                        for (i = 1; i <= 17; i++)
                        {

                            Xt = Gut[i].X;
                            Yt = Gut[i].Y;
                            Zt = Gut[i].Z;
                            if (Zt < ZMin)
                            {
                                Zt = ZMin;
                            }
                            Gut[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                            Gut[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                        }
                    }
                    //Draw gutter
                    if (ThreeDBut == false)
                    {
                        //Do perimeter first. { fill it.
                        context.lineTo(Gut[1].X, Gut[1].Y, Gut[2].X, Gut[2].Y, GutterColor);
                        context.lineTo(Gut[3].X, Gut[3].Y, GutterColor);
                        context.lineTo(Gut[10].X, Gut[10].Y, GutterColor);
                        context.lineTo(Gut[11].X, Gut[11].Y, GutterColor);
                        context.lineTo(Gut[14].X, Gut[14].Y, GutterColor);
                        context.lineTo(Gut[13].X, Gut[13].Y, GutterColor);
                        context.lineTo(Gut[12].X, Gut[12].Y, GutterColor);
                        context.lineTo(Gut[5].X, Gut[5].Y, GutterColor);
                        context.lineTo(Gut[4].X, Gut[4].Y, GutterColor);
                        context.lineTo(Gut[1].X, Gut[1].Y, GutterColor);
                        context.DrawMode = 13;
                        context.FillColor = Concrete;
                        context.FillStyle = 0;
                        H = Gut[17].X;
                        K = Gut[17].Y;
                        ConvertToPixels();
                        //Fx = FloodFill(Pic1.hdc, H, K, GutterColor); ; 
                        context.FillColor = QBColor[15];
                    }

                    context.lineTo(Gut[1].X, Gut[1].Y, Gut[2].X, Gut[2].Y, GutterColor);
                    context.lineTo(Gut[4].X, Gut[4].Y, Gut[3].X, Gut[3].Y, GutterColor);
                    context.lineTo(Gut[5].X, Gut[5].Y, Gut[6].X, Gut[6].Y, GutterColor);
                    context.lineTo(Gut[7].X, Gut[7].Y, GutterColor);
                    context.lineTo(Gut[8].X, Gut[8].Y, GutterColor);
                    context.lineTo(Gut[9].X, Gut[9].Y, GutterColor);
                    context.lineTo(Gut[10].X, Gut[10].Y, GutterColor);
                    context.lineTo(Gut[12].X, Gut[12].Y, Gut[11].X, Gut[11].Y, GutterColor);
                    context.lineTo(Gut[13].X, Gut[13].Y, Gut[14].X, Gut[14].Y, GutterColor);

                    context.lineTo(Gut[2].X, Gut[2].Y, Gut[3].X, Gut[3].Y, GutterColor);
                    context.lineTo(Gut[10].X, Gut[10].Y, GutterColor);
                    context.lineTo(Gut[11].X, Gut[11].Y, GutterColor);
                    context.lineTo(Gut[14].X, Gut[14].Y, GutterColor);

                    context.lineTo(Gut[1].X, Gut[1].Y, Gut[4].X, Gut[4].Y, GutterColor);
                    context.lineTo(Gut[5].X, Gut[5].Y, GutterColor);
                    context.lineTo(Gut[12].X, Gut[12].Y, GutterColor);
                    context.lineTo(Gut[13].X, Gut[13].Y, GutterColor);

                    //Depression lines
                    if (DEPINL > 0)
                    {
                        context.lineWidth = 1;
                        context.lineTo(Gut[6].X, Gut[6].Y, Gut[15].X, Gut[15].Y, GutterColor);
                        context.lineTo(Gut[7].X, Gut[7].Y, GutterColor);
                        context.lineTo(Gut[8].X, Gut[8].Y, Gut[16].X, Gut[16].Y, GutterColor);
                        context.lineTo(Gut[9].X, Gut[9].Y, GutterColor);
                        context.lineWidth = 2;
                    }

                    //Inlet
                    Inlet[1].X = X1L - 3 * TopCurbWidth;
                    Inlet[1].Y = Y1L + Yp;
                    Inlet[1].Z = 0 - ZDepthAdd;
                    Inlet[2].X = X1L;
                    Inlet[2].Y = Y1L + Yp;
                    Inlet[2].Z = Inlet[1].Z;
                    Inlet[3].X = X1L;
                    Inlet[3].Y = Y1L;
                    Inlet[3].Z = Inlet[1].Z;
                    Inlet[4].X = X1L;
                    Inlet[4].Y = Y1L + INGW * (INSW - DepSlope);
                    Inlet[4].Z = 0;
                    switch (InlType)
                    {
                        case EnumInletType.Curb ^ EnumInletType.Combination:
                            Inlet[5].X = X1L;
                            Inlet[5].Y = Y1L + Yp - TopInletWidth; //Throat ht
                            Inlet[5].Z = 0;
                            Inlet[6].X = X1L;
                            Inlet[6].Y = Inlet[5].Y;
                            Inlet[6].Z = ZDepth;
                            //Fill location
                            Inlet[11].X = Inlet[5].X;
                            Inlet[11].Y = Inlet[4].Y + (Inlet[5].Y - Inlet[4].Y) * 0.5;
                            Inlet[11].Z = ZDepth * 0.5;
                            break;
                        default:
                            Inlet[5].X = Inlet[4].X;
                            Inlet[5].Y = Inlet[4].Y;
                            Inlet[5].Z = Inlet[4].Z;
                            Inlet[6].X = Inlet[4].X;
                            Inlet[6].Y = Inlet[4].Y;
                            Inlet[6].Z = Inlet[4].Z;
                            //Fill location
                            Inlet[11].X = Inlet[4].X;
                            Inlet[11].Y = Inlet[4].Y + (Inlet[2].Y - Inlet[4].Y) * 0.5;
                            Inlet[11].Z = ZDepth * 0.5;
                            break;
                    }
                    Inlet[7].X = X1L;
                    Inlet[7].Y = Inlet[4].Y;
                    Inlet[7].Z = ZDepth;
                    Inlet[8].X = X1L;
                    Inlet[8].Y = Y1L;
                    Inlet[8].Z = ZDepth + ZDepthAdd;
                    Inlet[9].X = X1L;
                    Inlet[9].Y = Y1L + Yp;
                    Inlet[9].Z = ZDepth + ZDepthAdd;
                    Inlet[10].X = X1L - 3 * TopCurbWidth;
                    Inlet[10].Y = Y1L + Yp;
                    Inlet[10].Z = ZDepth + ZDepthAdd;
                    for (i = 12; i <= 22; i++)
                    {
                        Inlet[i].X = 0;
                        Inlet[i].Y = 0;
                        Inlet[i].Z = 0;
                    }
                    //Transform
                    if (ThreeDBut == false)
                    {
                        for (i = 1; i <= 11; i++)
                        {
                            Xt = Inlet[i].X;
                            Yt = Inlet[i].Y;
                            Zt = Inlet[i].Z;
                            Inlet[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                            Inlet[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                        }
                    }
                    for (i = 1; i <= 9; i++)
                    {
                        context.lineTo(Inlet[i].X, Inlet[i].Y, Inlet[i + 1].X, Inlet[i + 1].Y, InletColor);
                    }
                    context.lineTo(Inlet[10].X, Inlet[10].Y, Inlet[1].X, Inlet[1].Y, InletColor);
                    context.lineTo(Inlet[4].X, Inlet[4].Y, Inlet[7].X, Inlet[7].Y, InletColor);
                    //Fill
                    if (ThreeDBut == false)
                    {
                        context.DrawMode = 13;
                        context.FillColor = new RGB(254, 254, 254);//Lter Gray
                        context.FillStyle = 0;
                        H = Inlet[11].X;
                        K = Inlet[11].Y;
                        ConvertToPixels();
                        //Fx = FloodFill(Pic1.Hdc, H, K, InletColor) ; 

                        context.FillColor = QBColor[15];
                    }
                    context.lineTo(Inlet[9].X, Inlet[9].Y, Inlet[2].X, Inlet[2].Y, InletColor);

                    if (ThreeDBut = true && (InlType == EnumInletType.Curb || InlType == EnumInletType.Combination))
                    { //Draw throat ht
                        context.lineWidth = 1;
                        context.DrawStyle = 2;//Dotted
                        context.lineTo(Inlet[5].X, Inlet[5].Y, Inlet[1].X, Inlet[5].Y, InletColor);
                        context.DrawStyle = 0;
                        context.lineWidth = 2;
                    }

                    //Draw dotted line showing depressed curb slope
                    if (DEPINL != 0 && ThreeDBut == true)
                    {
                        Depr[1].X = X1L;
                        Depr[1].Y = Y1L + INGW * (INSW - DepSlope) + Dep;
                        Depr[1].Z = 0;
                        Depr[2].X = X1L + INGW;
                        Depr[2].Y = Depr[1].Y + INGW * INSX;
                        Depr[2].Z = 0;
                        Depr[3].X = X1L;
                        Depr[3].Y = Depr[1].Y;
                        Depr[3].Z = ZDepth;
                        Depr[4].X = X1L + INGW;
                        Depr[4].Y = Depr[2].Y;
                        Depr[4].Z = ZDepth;
                        //Transform
                        if (ThreeDBut == false)
                        {
                            for (i = 1; i <= 4; i++)
                            {
                                Xt = Depr[i].X;
                                Yt = Depr[i].Y;
                                Zt = Depr[i].Z;
                                Depr[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                                Depr[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                            }
                        }
                        context.lineWidth = 1;
                        context.DrawStyle = 2;
                        context.lineTo(Depr[1].X, Depr[1].Y, Depr[2].X, Depr[2].Y, GutterColor);
                        context.lineTo(Depr[3].X, Depr[3].Y, Depr[4].X, Depr[4].Y, GutterColor);
                        context.DrawStyle = 0;
                    }

                    if (InCurrentCalc == true)
                    {
                        //Water Spread
                        Water[1].X = X1L;
                        Water[1].Y = Y1L + InlGDepth;//Upstream
                        Water[1].Z = ZMin;
                        Water[2].X = X1L;
                        Water[2].Y = Y1L + INGW * (INSW - DepSlope) + InlIDepth;//Upstream at Inlet
                        Water[2].Z = 0;
                        Water[3].X = X1L;
                        Water[3].Y = Y1L + InlBypDepth;
                        Water[3].Z = ZDepth;
                        Water[4].X = X1L;
                        Water[4].Y = Y1L + InlBypDepth;
                        Water[4].Z = ZMax;
                        Water[5].X = X1L + InlBypSpread;
                        Water[5].Y = Y1L + InlBypDepth;
                        Water[5].Z = ZMax;
                        Water[6].X = X1L + InlBypSpread;
                        Water[6].Y = Y1L + InlBypDepth;
                        Water[6].Z = ZDepth;
                        Water[7].X = X1L + InlISpread;
                        Water[7].Y = Water[2].Y;
                        Water[7].Z = 0;
                        if (INLSAG == EnumInletLocation.In_Sag)
                        {
                            Water[8].X = Water[7].X;
                            Water[8].Y = Water[7].Y;
                            Water[8].Z = ZMin;
                        }
                        else
                        {
                            Water[8].X = X1L + InlGSpread;
                            Water[8].Y = Y1L + InlGDepth;
                            Water[8].Z = ZMin;
                        }
                        if (INLSAG == EnumInletLocation.In_Sag)
                        {
                            Water[3].Y = Water[2].Y;
                            Water[4].X = Water[2].X;
                            Water[4].Y = Water[2].Y;
                            Water[5].X = Water[7].X;
                            Water[5].Y = Water[7].Y;
                            Water[6].X = Water[7].X;
                            Water[6].Y = Water[7].Y;
                        }
                        //Locate flood-fill point
                        Water[9].X = Water[2].X + (Water[7].X - Water[2].X) * 0.5;
                        Water[9].Y = Water[2].Y;
                        Water[9].Z = 0;

                        //Transform
                        context.lineWidth = 2;
                        if (ThreeDBut == false)
                        {
                            for (i = 1; i <= 9; i++)
                            {
                                Xt = Water[i].X;
                                Yt = Water[i].Y;
                                Zt = Water[i].Z;
                                Water[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                                Water[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                            }
                            //Draw
                            context.lineTo(Water[1].X, Water[1].Y, Water[2].X, Water[2].Y, new RGB(49, 106, 197));
                            context.lineTo(Water[3].X, Water[3].Y, new RGB(49, 106, 197));
                            if (InlQBY > 0 || INLSAG == EnumInletLocation.In_Sag)
                            {
                                context.lineTo(Water[4].X, Water[4].Y, new RGB(49, 106, 197));
                                context.lineTo(Water[5].X, Water[5].Y, new RGB(49, 106, 197));
                            }
                            context.lineTo(Water[6].X, Water[6].Y, new RGB(49, 106, 197));
                            context.lineTo(Water[7].X, Water[7].Y, new RGB(49, 106, 197));
                            context.lineTo(Water[8].X, Water[8].Y, new RGB(49, 106, 197));
                            context.lineTo(Water[1].X, Water[1].Y, new RGB(49, 106, 197));
                            //Fill water
                            context.FillColor = WaterFill;
                            context.DrawMode = 10;
                            H = Water[9].X;
                            K = Water[9].Y;
                            ConvertToPixels();
                            //Fx = FloodFill(Pic1.Hdc, H, K, WaterColor);  

                            context.FillColor = QBColor[15];
                            context.DrawMode = 13;

                            //Dotted line
                            context.lineWidth = 1;
                            context.DrawStyle = 1;
                            context.lineTo(Water[2].X, Water[2].Y, Water[7].X, Water[7].Y, new RGB(49, 106, 197));
                            context.DrawStyle = 0;
                        }
                        else
                        { //2D
                            context.DrawStyle = 0;
                            context.lineWidth = 2;
                            context.lineTo(Water[2].X, Water[2].Y, Water[7].X, Water[7].Y, new RGB(49, 106, 197));
                        }
                    } //Current calc

                    //Grate? Slotted? ---------------------
                    if (InlType == EnumInletType.Grate || InlType == EnumInletType.Combination || InlType == EnumInletType.Slotted)
                    { //Grate Inlet
                        i = 4 + 2 * ((int)(GTL / (0.5 * SIFeet)));
                        Grate[1].X = X1L;
                        Grate[1].Y = Y1L + INGW * (INSW - DepSlope);
                        Grate[1].Z = ZDepth;
                        Grate[2].X = Grate[1].X;
                        Grate[2].Y = Grate[1].Y;
                        Grate[2].Z = Grate[1].Z - GTL;
                        Grate[3].X = Grate[1].X + GTW;
                        Grate[3].Y = Grate[2].Y + GTW * DepSlope;
                        Grate[3].Z = Grate[2].Z;
                        Grate[4].X = Grate[3].X;
                        Grate[4].Y = Grate[3].Y;
                        Grate[4].Z = Grate[1].Z;
                        for (Z = 5; Z <= i - 1; Z += 2)
                        {
                            Fuck = Fuck + 1;
                            Grate[Z].X = Grate[1].X;
                            Grate[Z].Y = Grate[1].Y;
                            Grate[Z].Z = Grate[1].Z - 0.5 * SIFeet * Fuck;
                            Grate[Z + 1].X = Grate[3].X;
                            Grate[Z + 1].Y = Grate[3].Y;
                            Grate[Z + 1].Z = Grate[Z].Z;
                        }
                        Z = i;
                        //Transform
                        if (ThreeDBut == false)
                        {
                            for (i = 1; i <= Z; i++)
                            {
                                Xt = Grate[i].X;
                                Yt = Grate[i].Y;
                                Zt = Grate[i].Z;
                                Grate[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                                Grate[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                            }
                        }
                        context.lineWidth = 2;
                        context.lineTo(Grate[1].X, Grate[1].Y, Grate[2].X, Grate[2].Y, QBColor[0]);
                        context.lineTo(Grate[3].X, Grate[3].Y, QBColor[0]);
                        context.lineTo(Grate[4].X, Grate[4].Y, QBColor[0]);
                        context.lineTo(Grate[1].X, Grate[1].Y, QBColor[0]);
                        for (i = 5; i <= Z - 1; i += 2)
                        {
                            context.lineTo(Grate[i].X, Grate[i].Y, Grate[i + 1].X, Grate[i + 1].Y, QBColor[0]);
                        }
                    }
                }
                #endregion
                break;

            case EnumInletType.Drop_Curb:
                #region Drop_Curb
                {
                    //Gutter Bottom
                    //How Wide?
                    IGutterWidth = INL * 0.25;
                    ISpreadWidth = InlISpread * 2 + IGutterWidth;
                    ZDepth = INL * 0.25;
                    ZDepthAdd = 0;

                    //Gutter Coords
                    H = 1.25 * InlISpread;
                    //H = INTH(Sc%) / InchPerFt + TopInletWidth!
                    if (H > System.Math.Abs(ZMin)) { H = System.Math.Abs(ZMin); }
                    Gut[1].X = 0.5 * ScaleXX - 0.5 * IGutterWidth - H;
                    Gut[1].Y = Y1L + H * INSX;
                    Gut[1].Z = -H;
                    Gut[2].X = Gut[1].X;
                    Gut[2].Y = Gut[1].Y;
                    Gut[2].Z = ZDepth + H;
                    Gut[3].X = 0.5 * ScaleXX - 0.5 * IGutterWidth;
                    Gut[3].Y = Y1L;
                    Gut[3].Z = 0;
                    Gut[4].X = 0.5 * ScaleXX - 0.5 * IGutterWidth;
                    Gut[4].Y = Y1L;
                    Gut[4].Z = ZDepth;
                    Gut[5].X = 0.5 * ScaleXX + 0.5 * IGutterWidth + H;
                    Gut[5].Y = Gut[1].Y;
                    Gut[5].Z = Gut[1].Z;
                    Gut[6].X = 0.5 * ScaleXX + 0.5 * IGutterWidth;
                    Gut[6].Y = Y1L;
                    Gut[6].Z = 0;
                    Gut[7].X = 0.5 * ScaleXX + 0.5 * IGutterWidth;
                    Gut[7].Y = Y1L;
                    Gut[7].Z = ZDepth;

                    Gut[8].X = 0.5 * ScaleXX + 0.5 * IGutterWidth + H;
                    Gut[8].Y = Gut[5].Y;
                    Gut[8].Z = Gut[2].Z;
                    for (i = 9; i <= 17; i++)
                    {
                        Gut[i].X = 0;
                        Gut[i].Y = 0;
                        Gut[i].Z = 0;
                    }
                    //for Flood-fill locations
                    Gut[9].X = Gut[1].X + (Gut[3].X - Gut[1].X) * 0.5;
                    Gut[9].Y = Gut[3].Y + (Gut[1].Y - Gut[3].Y) * 0.5;
                    Gut[9].Z = Gut[1].Y + (Gut[2].Y - Gut[1].Y) * 0.5;
                    Gut[10].X = Gut[4].X + (Gut[7].X - Gut[4].X) * 0.5;
                    Gut[10].Y = Gut[4].Y + (Gut[2].Y - Gut[4].Y) * 0.5;
                    Gut[10].Z = ZDepth + H * 0.5;
                    Gut[11].X = Gut[6].X + (Gut[5].X - Gut[6].X) * 0.5;
                    Gut[11].Y = Gut[9].Y;
                    Gut[11].Z = Gut[9].Z;
                    Gut[12].X = Gut[10].X;
                    Gut[12].Y = Gut[10].Y;
                    Gut[12].Z = 0 - H * 0.5;

                    if (ThreeDBut == false)
                    {
                        //Transform
                        for (i = 1; i <= 12; i++)
                        {
                            Xt = Gut[i].X;
                            Yt = Gut[i].Y;
                            Zt = Gut[i].Z;
                            if (Zt < ZMin)
                            {
                                Zt = ZMin;
                            }
                            Gut[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                            Gut[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                        }
                    }
                    //Draw gutter
                    context.lineWidth = 2;
                    context.lineTo(Gut[1].X, Gut[1].Y, Gut[2].X, Gut[2].Y, GutterColor);
                    context.lineTo(Gut[4].X, Gut[4].Y, GutterColor);
                    context.lineTo(Gut[3].X, Gut[3].Y, GutterColor);
                    context.lineTo(Gut[1].X, Gut[1].Y, GutterColor);

                    context.lineTo(Gut[5].X, Gut[5].Y, Gut[8].X, Gut[8].Y, GutterColor);
                    context.lineTo(Gut[7].X, Gut[7].Y, GutterColor);
                    context.lineTo(Gut[6].X, Gut[6].Y, GutterColor);
                    context.lineTo(Gut[5].X, Gut[5].Y, GutterColor);

                    context.lineTo(Gut[3].X, Gut[3].Y, Gut[6].X, Gut[6].Y, GutterColor);
                    context.lineTo(Gut[4].X, Gut[4].Y, Gut[7].X, Gut[7].Y, GutterColor);

                    if (ThreeDBut == false)
                    { //Back horiz. line
                        context.lineTo(Gut[2].X, Gut[2].Y, Gut[8].X, Gut[8].Y, GutterColor);
                        //Fills
                        context.FillColor = Concrete;
                        context.FillStyle = 0;
                        for (i = 9; i <= 11; i++)
                        {
                            H = Gut[i].X;
                            K = Gut[i].Y;
                            ConvertToPixels();
                            //Fx = FloodFill(Pic1.Hdc, H, K, GutterColor); 

                        }

                        if (Gut[3].Y > Gut[1].Y)
                        {
                            //Draw front & Fill it
                            context.lineTo(Gut[1].X, Gut[1].Y, Gut[5].X, Gut[5].Y, GutterColor);
                            H = Gut[12].X;
                            K = Gut[12].Y;
                            ConvertToPixels();
                            //Fx = FloodFill(Pic1.Hdc, H, K, GutterColor);

                        }

                        //Inlet invert Fill
                        context.FillColor = new RGB(49, 106, 197);
                        H = Gut[3].X + (Gut[6].X - Gut[3].X) * 0.5;
                        K = Gut[6].Y + (Gut[4].Y - Gut[6].Y) * 0.5;
                        ConvertToPixels();
                        //Fx = FloodFill(Pic1.Hdc, H, K, GutterColor); 


                        context.FillColor = QBColor[15];
                    }

                    if (InCurrentCalc == true)
                    {
                        //Water Spread
                        WedgeX = 0.5 * ScaleXX - 0.5 * IGutterWidth - InlISpread;
                        WedgeX = WedgeX + 0.6 * InlISpread;
                        H = InlIDepth / INSX; //Horiz. Dist.
                        if (H > System.Math.Abs(ZMin)) { H = System.Math.Abs(ZMin); }
                        Water[1].X = 0.5 * ScaleXX - 0.5 * ISpreadWidth;
                        Water[1].Y = Y1L + InlIDepth; //Upstream
                        Water[1].Z = -H;
                        Water[2].X = Water[1].X;
                        Water[2].Y = Water[1].Y;
                        Water[2].Z = 0;
                        Water[3].X = Water[1].X;
                        Water[3].Y = Water[1].Y;
                        Water[3].Z = ZDepth;
                        Water[4].X = Water[1].X;
                        Water[4].Y = Water[1].Y;
                        Water[4].Z = ZDepth + H;
                        Water[5].X = 0.5 * ScaleXX + 0.5 * ISpreadWidth;
                        Water[5].Y = Water[1].Y;
                        Water[5].Z = Water[4].Z;
                        Water[6].X = Water[5].X;
                        Water[6].Y = Water[1].Y;
                        Water[6].Z = ZDepth;
                        Water[7].X = Water[5].X;
                        Water[7].Y = Water[1].Y;
                        Water[7].Z = 0;
                        Water[8].X = Water[5].X;
                        Water[8].Y = Water[1].Y;//Upstream
                        Water[8].Z = Water[1].Z;

                        //Locate flood-fill point
                        Water[9].X = Water[1].X + (Water[8].X - Water[1].X) * 0.5;
                        Water[9].Y = Water[1].Y;
                        Water[9].Z = 0;

                        //Transform
                        context.lineWidth = 2;
                        if (ThreeDBut == false)
                        {
                            for (i = 1; i <= 9; i++)
                            {
                                Xt = Water[i].X;
                                Yt = Water[i].Y;
                                Zt = Water[i].Z;
                                Water[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                                Water[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                            }
                            context.lineTo(Water[1].X, Water[1].Y, Water[2].X, Water[2].Y, WaterColor); //new RGB(49, 106, 197)
                            context.lineTo(Water[3].X, Water[3].Y, WaterColor);
                            if (InlQBY > 0 || INLSAG == EnumInletLocation.In_Sag)
                            {
                                context.lineTo(Water[4].X, Water[4].Y, WaterColor);
                                context.lineTo(Water[5].X, Water[5].Y, WaterColor);
                            }
                            context.lineTo(Water[6].X, Water[6].Y, WaterColor);
                            context.lineTo(Water[7].X, Water[7].Y, WaterColor);
                            context.lineTo(Water[8].X, Water[8].Y, WaterColor);
                            context.lineTo(Water[1].X, Water[1].Y, WaterColor);

                            //Fill water
                            context.FillColor = WaterFill;
                            context.DrawMode = 10;//15
                            H = Water[9].X;
                            K = Water[9].Y;
                            ConvertToPixels();
                            //Fx = FloodFill(Pic1.Hdc, H, K, WaterColor);  

                            context.FillColor = QBColor[15];
                            context.DrawMode = 13;

                        }
                        else
                        { //2D
                            context.DrawStyle = 0;
                            context.lineWidth = 2;
                            context.lineTo(Water[2].X, Water[2].Y, Water[7].X, Water[7].Y, new RGB(49, 106, 197));
                        }
                    } //InCurrentCalc(Sc%)

                    //Inlet
                    Inlet[1].X = 0.5 * ScaleXX - 0.5 * IGutterWidth;
                    Inlet[1].Y = Y1L + INTH / InchPerFt + TopInletWidth;
                    Inlet[1].Z = 0;
                    Inlet[2].X = Inlet[1].X;
                    Inlet[2].Y = Inlet[1].Y;
                    Inlet[2].Z = ZDepth;
                    Inlet[3].X = 0.5 * ScaleXX + 0.5 * IGutterWidth;
                    Inlet[3].Y = Inlet[1].Y;
                    Inlet[3].Z = ZDepth;
                    Inlet[4].X = Inlet[3].X;
                    Inlet[4].Y = Inlet[1].Y;
                    Inlet[4].Z = 0;
                    Inlet[5].X = Inlet[1].X;
                    Inlet[5].Y = Y1L + INTH / InchPerFt;
                    Inlet[5].Z = 0;
                    Inlet[6].X = Inlet[4].X;
                    Inlet[6].Y = Inlet[5].Y;
                    Inlet[6].Z = 0;
                    Inlet[7].X = Inlet[6].X;
                    Inlet[7].Y = Inlet[6].Y;
                    Inlet[7].Z = ZDepth;
                    Inlet[8].X = Inlet[5].X;
                    Inlet[8].Y = Inlet[5].Y;
                    Inlet[8].Z = Inlet[2].Z;

                    //Lft post
                    Inlet[9].X = Inlet[1].X;
                    Inlet[9].Y = Y1L;
                    Inlet[9].Z = 0;
                    //Rt Front post
                    Inlet[10].X = Inlet[4].X;
                    Inlet[10].Y = Y1L;
                    Inlet[10].Z = 0;
                    //Rt back post
                    Inlet[11].X = Inlet[7].X;
                    Inlet[11].Y = Y1L;
                    Inlet[11].Z = ZDepth;
                    Inlet[12].X = Inlet[1].X;
                    Inlet[12].Y = Y1L;
                    Inlet[12].Z = ZDepth;
                    for (i = 13; i <= 22; i++)
                    {
                        Inlet[i].X = 0;
                        Inlet[i].Y = 0;
                        Inlet[i].Z = 0;
                    }

                    //Floodfill locations for water
                    if (InlIDepth >= INTH / InchPerFt)
                    {
                        Inlet[13].X = Inlet[1].X;
                        if (InlIDepth > INTH / InchPerFt + TopInletWidth)
                        {
                            Inlet[13].Y = Inlet[1].Y;
                        }
                        else
                        {
                            Inlet[13].Y = Y1L + InlIDepth;
                        }
                        Inlet[13].Z = Inlet[1].Z;
                        Inlet[14].X = Inlet[1].X;
                        Inlet[14].Y = Inlet[13].Y;
                        Inlet[14].Z = Inlet[2].Z;
                        Inlet[15].X = Inlet[3].X;
                        Inlet[15].Y = Inlet[13].Y;
                        Inlet[15].Z = Inlet[3].Z;
                        Inlet[16].X = Inlet[4].X;
                        Inlet[16].Y = Inlet[13].Y;
                        Inlet[16].Z = Inlet[4].Z;
                    }

                    //Transform
                    if (ThreeDBut == false)
                    {
                        for (i = 1; i <= 22; i++)
                        {
                            Xt = Inlet[i].X;
                            Yt = Inlet[i].Y;
                            Zt = Inlet[i].Z;
                            Inlet[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                            Inlet[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                        }
                    }

                    //Draw Inlet
                    context.FillStyle = 0;
                    context.FillColor = Concrete;

                    //4 Posts
                    context.lineWidth = 3;
                    context.lineTo(Inlet[5].X, Inlet[5].Y, Inlet[9].X, Inlet[9].Y, InletColor);
                    context.lineTo(Inlet[6].X, Inlet[6].Y, Inlet[10].X, Inlet[10].Y, InletColor);
                    context.lineTo(Inlet[7].X, Inlet[7].Y, Inlet[11].X, Inlet[11].Y, InletColor);
                    context.lineTo(Inlet[8].X, Inlet[8].Y, Inlet[12].X, Inlet[12].Y, InletColor);

                    //Back side
                    context.lineTo(Inlet[2].X, Inlet[2].Y, Inlet[3].X, Inlet[3].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[7].X, Inlet[7].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[8].X, Inlet[8].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[2].X, Inlet[2].Y, new RGB(20, 20, 20));
                    //Fill
                    H = Inlet[2].X + (Inlet[3].X - Inlet[2].X) * 0.5;
                    K = Inlet[8].Y + (Inlet[2].Y - Inlet[8].Y) * 0.5;
                    ConvertToPixels();
                    //Fx = FloodFill(Pic1.Hdc, H, K, new RGB(20, 20, 20));  

                    //Redraw in Blk
                    context.lineTo(Inlet[2].X, Inlet[2].Y, Inlet[3].X, Inlet[3].Y, InletColor);
                    context.lineTo(Inlet[7].X, Inlet[7].Y, InletColor);
                    context.lineTo(Inlet[8].X, Inlet[8].Y, InletColor);
                    context.lineTo(Inlet[2].X, Inlet[2].Y, InletColor);
                    //No need ; i <= fill the back

                    //Lft Side------------------------------
                    context.lineTo(Inlet[1].X, Inlet[1].Y, Inlet[2].X, Inlet[2].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[8].X, Inlet[8].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[5].X, Inlet[5].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[1].X, Inlet[1].Y, new RGB(20, 20, 20));
                    //Fill
                    H = Inlet[1].X + (Inlet[8].X - Inlet[1].X) * 0.5;
                    K = Inlet[5].Y + (Inlet[2].Y - Inlet[5].Y) * 0.5;
                    ConvertToPixels();
                    //Fx = FloodFill(Pic1.Hdc, H, K, new RGB(20, 20, 20))  ;

                    //Redraw in Blk
                    context.lineTo(Inlet[1].X, Inlet[1].Y, Inlet[2].X, Inlet[2].Y, InletColor);
                    context.lineTo(Inlet[8].X, Inlet[8].Y, InletColor);
                    context.lineTo(Inlet[5].X, Inlet[5].Y, InletColor);
                    context.lineTo(Inlet[1].X, Inlet[1].Y, InletColor);

                    if (ThreeDBut == false && InCurrentCalc == true)
                    {
                        //Floodfill side w/ water
                        if (InlIDepth >= INTH / InchPerFt)
                        {
                            //Draw in blk
                            context.lineWidth = 1;
                            context.lineTo(Inlet[13].X, Inlet[13].Y, Inlet[14].X, Inlet[14].Y, InletColor);
                            //Fill
                            context.FillColor = WaterFill;
                            context.DrawMode = 10;
                            H = Inlet[13].X + (Inlet[14].X - Inlet[13].X) * 0.5;
                            K = Inlet[5].Y + (Inlet[14].Y - Inlet[5].Y) * 0.5;
                            ConvertToPixels();
                            //Fx = FloodFill(Pic1.Hdc, H, K, InletColor)  ;


                            //ReDraw in WaterColor
                            context.DrawMode = 13;
                            context.lineTo(Inlet[13].X, Inlet[13].Y, Inlet[14].X, Inlet[14].Y, WaterColor);
                            context.FillColor = QBColor[15]; //White
                            context.DrawMode = 13;
                            context.lineWidth = 2;
                        }
                        context.FillColor = Concrete;
                    }

                    //Rt Side
                    context.lineTo(Inlet[4].X, Inlet[4].Y, Inlet[3].X, Inlet[3].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[7].X, Inlet[7].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[6].X, Inlet[6].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[4].X, Inlet[4].Y, new RGB(20, 20, 20));
                    //Fill
                    H = Inlet[4].X + (Inlet[7].X - Inlet[4].X) * 0.5;
                    K = Inlet[6].Y + (Inlet[3].Y - Inlet[6].Y) * 0.5;
                    ConvertToPixels();
                    //Fx = FloodFill(Pic1.Hdc, H, K, new RGB(20, 20, 20))  ;


                    //Redraw in Blk
                    context.lineTo(Inlet[4].X, Inlet[4].Y, Inlet[3].X, Inlet[3].Y, InletColor);
                    context.lineTo(Inlet[7].X, Inlet[7].Y, InletColor);
                    context.lineTo(Inlet[6].X, Inlet[6].Y, InletColor);
                    context.lineTo(Inlet[4].X, Inlet[4].Y, InletColor);

                    if (ThreeDBut = false && InCurrentCalc == true)
                    {
                        //Floodfill side w/ water
                        if (InlIDepth >= INTH / InchPerFt)
                        {
                            //Draw in blk
                            context.lineWidth = 1;
                            context.lineTo(Inlet[16].X, Inlet[16].Y, Inlet[15].X, Inlet[15].Y, InletColor);
                            //Fill
                            context.FillColor = WaterFill;
                            context.DrawMode = 10;
                            H = Inlet[16].X + (Inlet[15].X - Inlet[16].X) * 0.5;
                            K = Inlet[6].Y + (Inlet[15].Y - Inlet[6].Y) * 0.5;
                            ConvertToPixels();
                            //Fx = FloodFill(Pic1.Hdc, H, K, InletColor)  ;


                            //ReDraw in WaterColor
                            context.DrawMode = 13;
                            context.lineTo(Inlet[16].X, Inlet[16].Y, Inlet[15].X, Inlet[15].Y, WaterColor);
                            context.FillColor = QBColor[15]; //White
                            context.DrawMode = 13;
                            context.lineWidth = 2;
                        }
                        context.FillColor = Concrete;
                    }

                    //Top
                    context.lineTo(Inlet[1].X, Inlet[1].Y, Inlet[2].X, Inlet[2].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[3].X, Inlet[3].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[4].X, Inlet[4].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[1].X, Inlet[1].Y, new RGB(20, 20, 20));
                    //Fill
                    if (InCurrentCalc == true && ThreeDBut == false && InlIDepth > INTH / InchPerFt + TopInletWidth)
                    {
                        //Floodfill top w/ water
                        context.FillColor = WaterFill;
                    }
                    else
                    {
                        context.FillColor = Concrete;
                    }
                    H = Inlet[1].X + (Inlet[3].X - Inlet[1].X) * 0.5;
                    K = Inlet[1].Y + (Inlet[2].Y - Inlet[1].Y) * 0.5;
                    ConvertToPixels();
                    //Fx = FloodFill(Pic1.Hdc, H, K, new RGB(20, 20, 20)) ; 


                    context.FillColor = Concrete;
                    //Redraw Top
                    context.lineTo(Inlet[1].X, Inlet[1].Y, Inlet[2].X, Inlet[2].Y, InletColor);
                    context.lineTo(Inlet[3].X, Inlet[3].Y, InletColor);
                    context.lineTo(Inlet[4].X, Inlet[4].Y, InletColor);
                    context.lineTo(Inlet[1].X, Inlet[1].Y, InletColor);

                    //Front--------------------------
                    context.lineTo(Inlet[1].X, Inlet[1].Y, Inlet[4].X, Inlet[4].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[6].X, Inlet[6].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[5].X, Inlet[5].Y, new RGB(20, 20, 20));
                    context.lineTo(Inlet[1].X, Inlet[1].Y, new RGB(20, 20, 20));
                    //Fill
                    H = Inlet[1].X + (Inlet[4].X - Inlet[1].X) * 0.5;
                    K = Inlet[5].Y + (Inlet[4].Y - Inlet[5].Y) * 0.5;
                    ConvertToPixels();
                    //Fx = FloodFill(Pic1.Hdc, H, K, new RGB(20, 20, 20))  ;


                    //Redraw in blk
                    context.lineTo(Inlet[1].X, Inlet[1].Y, Inlet[4].X, Inlet[4].Y, InletColor);
                    context.lineTo(Inlet[6].X, Inlet[6].Y, InletColor);
                    context.lineTo(Inlet[5].X, Inlet[5].Y, InletColor);
                    context.lineTo(Inlet[1].X, Inlet[1].Y, InletColor);

                    if (ThreeDBut == false && InCurrentCalc == true)
                    {
                        //Floodfill locations for water
                        if (InlIDepth >= INTH / InchPerFt)
                        {
                            //Draw in blk
                            context.lineWidth = 1;
                            context.lineTo(Inlet[13].X, Inlet[13].Y, Inlet[16].X, Inlet[16].Y, InletColor);
                            //Fill
                            context.FillColor = WaterFill;
                            context.DrawMode = 10;
                            H = Inlet[13].X + (Inlet[16].X - Inlet[13].X) * 0.5;
                            K = Inlet[5].Y + (Inlet[13].Y - Inlet[5].Y) * 0.5;
                            ConvertToPixels();
                            //Fx = FloodFill(Pic1.Hdc, H, K, InletColor) ; 


                            //ReDraw in WaterColor
                            context.DrawMode = 13;
                            context.lineTo(Inlet[13].X, Inlet[13].Y, Inlet[16].X, Inlet[16].Y, WaterColor);
                            context.FillColor = QBColor[15]; //White
                            context.DrawMode = 13;
                        }
                        context.FillColor = QBColor[15]; //White
                    }

                    //Dotted line for water
                    if (ThreeDBut == false && InCurrentCalc == true)
                    {
                        context.lineWidth = 1;
                        context.DrawStyle = 1;
                        context.lineTo(Water[2].X, Water[2].Y, Water[7].X, Water[7].Y, new RGB(49, 106, 197));
                        context.DrawStyle = 0;
                        //Across front of inlet
                        context.lineWidth = 2;
                        context.lineTo(Water[1].X, Water[1].Y, Water[8].X, Water[8].Y, new RGB(49, 106, 197));
                    }
                }
                #endregion
                break;
            case EnumInletType.Drop_Grate: // ===============================
                #region Drop_Grate
                {
                    //Gutter Bottom
                    //How Wide?
                    IGutterWidth = INGW;
                    ISpreadWidth = InlISpread;
                    ZDepth = GTL;
                    ZDepthAdd = 0;

                    //Gutter Coords
                    if (INLSAG == EnumInletLocation.On_grade)
                    { //On grade
                        Gut[1].X = 0.5 * ScaleXX - 0.5 * IGutterWidth - 0.5 * InlISpread; //ISpreadWidth
                        //Gut[1].Y = Y1L + 0.5 * ISpreadWidth * INSX(Sc%]
                        Gut[1].Y = Y1L + 0.5 * InlISpread * INSX;
                        Gut[1].Z = ZMin;
                        Gut[2].X = Gut[1].X;
                        Gut[2].Y = Gut[1].Y;
                        Gut[2].Z = ZMax;
                        Gut[3].X = 0.5 * ScaleXX - 0.5 * IGutterWidth;
                        Gut[3].Y = Y1L;
                        Gut[3].Z = ZMin;
                        Gut[4].X = 0.5 * ScaleXX - 0.5 * IGutterWidth;
                        Gut[4].Y = Y1L;
                        Gut[4].Z = ZMax;
                        Gut[6].X = 0.5 * ScaleXX + 0.5 * IGutterWidth;
                        Gut[6].Y = Y1L;
                        Gut[6].Z = ZMax;
                        Gut[5].X = 0.5 * ScaleXX + 0.5 * IGutterWidth;
                        Gut[5].Y = Y1L;
                        Gut[5].Z = ZMin;
                        Gut[7].X = 0.5 * ScaleXX + 0.5 * IGutterWidth + 0.5 * InlISpread; //ISpreadWidth
                        //Gut[7].Y = Y1L + 0.5 * ISpreadWidth * INSX(Sc%]
                        Gut[7].Y = Y1L + 0.5 * InlISpread * INSX;
                        Gut[7].Z = ZMin;
                        Gut[8].X = Gut[7].X;
                        Gut[8].Y = Gut[7].Y;
                        Gut[8].Z = ZMax;
                        for (i = 9; i <= 17; i++)
                        {
                            Gut[i].X = 0;
                            Gut[i].Y = 0;
                            Gut[i].Z = 0;
                        }

                        //for Flood-fill locations
                        Gut[9].X = Gut[1].X + (Gut[3].X - Gut[1].X) * 0.5;
                        Gut[9].Y = Gut[3].Y + (Gut[1].Y - Gut[3].Y) * 0.5;
                        Gut[9].Z = Gut[1].Y + (Gut[2].Y - Gut[1].Y) * 0.5;
                        Gut[10].X = Gut[3].X + (Gut[5].X - Gut[3].X) * 0.5;
                        Gut[10].Y = Gut[3].Y + (Gut[4].Y - Gut[3].Y) * 0.5;
                        Gut[10].Z = 0;
                        Gut[11].X = Gut[5].X + (Gut[7].X - Gut[5].X) * 0.5;
                        Gut[11].Y = Gut[9].Y;
                        Gut[11].Z = Gut[9].Z;

                    }
                    else
                    { //In Sag
                        H = 0.5 * InlISpread; //ISpreadWidth
                        if (H > System.Math.Abs(ZMin)) { H = System.Math.Abs(ZMin); }
                        Gut[1].X = 0.5 * ScaleXX - 0.5 * IGutterWidth - H;
                        Gut[1].Y = Y1L + H * INSX;
                        Gut[1].Z = -H;
                        Gut[2].X = Gut[1].X;
                        Gut[2].Y = Gut[1].Y;
                        Gut[2].Z = ZDepth + H;
                        Gut[3].X = 0.5 * ScaleXX - 0.5 * IGutterWidth;
                        Gut[3].Y = Y1L;
                        Gut[3].Z = 0;
                        Gut[4].X = 0.5 * ScaleXX - 0.5 * IGutterWidth;
                        Gut[4].Y = Y1L;
                        Gut[4].Z = ZDepth;
                        Gut[5].X = 0.5 * ScaleXX + 0.5 * IGutterWidth + H;
                        Gut[5].Y = Gut[1].Y;
                        Gut[5].Z = Gut[1].Z;
                        Gut[6].X = 0.5 * ScaleXX + 0.5 * IGutterWidth;
                        Gut[6].Y = Y1L;
                        Gut[6].Z = 0;
                        Gut[7].X = 0.5 * ScaleXX + 0.5 * IGutterWidth;
                        Gut[7].Y = Y1L;
                        Gut[7].Z = ZDepth;

                        Gut[8].X = 0.5 * ScaleXX + 0.5 * IGutterWidth + H;
                        Gut[8].Y = Gut[5].Y;
                        Gut[8].Z = Gut[2].Z;
                        for (i = 9; i <= 17; i++)
                        {
                            Gut[i].X = 0;
                            Gut[i].Y = 0;
                            Gut[i].Z = 0;
                        }
                        //for Flood-fill locations
                        Gut[9].X = Gut[1].X + (Gut[3].X - Gut[1].X) * 0.5;
                        Gut[9].Y = Gut[3].Y + (Gut[1].Y - Gut[3].Y) * 0.5;
                        Gut[9].Z = Gut[1].Y + (Gut[2].Y - Gut[1].Y) * 0.5;
                        Gut[10].X = Gut[4].X + (Gut[7].X - Gut[4].X) * 0.5;
                        Gut[10].Y = Gut[4].Y + (Gut[2].Y - Gut[4].Y) * 0.5;
                        Gut[10].Z = ZDepth + H * 0.5;
                        Gut[11].X = Gut[6].X + (Gut[5].X - Gut[6].X) * 0.5;
                        Gut[11].Y = Gut[9].Y;
                        Gut[11].Z = Gut[9].Z;
                        Gut[12].X = Gut[10].X;
                        Gut[12].Y = Gut[10].Y;
                        Gut[12].Z = 0 - H * 0.5;
                    } //Sag || on grade gutter

                    if (ThreeDBut == false)
                    {
                        //Transform
                        for (i = 1; i <= 12; i++)
                        {
                            Xt = Gut[i].X;
                            Yt = Gut[i].Y;
                            Zt = Gut[i].Z;
                            if (Zt < ZMin)
                            {
                                Zt = ZMin;
                            }
                            Gut[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                            Gut[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                        }
                    }

                    //Draw gutter
                    context.lineWidth = 2;
                    context.DrawMode = 13;
                    if (INLSAG == EnumInletLocation.On_grade)
                    { //On grade
                        context.lineTo(Gut[1].X, Gut[1].Y, Gut[2].X, Gut[2].Y, GutterColor);
                        context.lineTo(Gut[3].X, Gut[3].Y, Gut[4].X, Gut[4].Y, GutterColor);
                        context.lineTo(Gut[5].X, Gut[5].Y, Gut[6].X, Gut[6].Y, GutterColor);
                        context.lineTo(Gut[7].X, Gut[7].Y, Gut[8].X, Gut[8].Y, GutterColor);

                        context.lineTo(Gut[1].X, Gut[1].Y, Gut[3].X, Gut[3].Y, GutterColor);
                        context.lineTo(Gut[5].X, Gut[5].Y, GutterColor);
                        context.lineTo(Gut[7].X, Gut[7].Y, GutterColor);

                        context.lineTo(Gut[2].X, Gut[2].Y, Gut[4].X, Gut[4].Y, GutterColor);
                        context.lineTo(Gut[6].X, Gut[6].Y, GutterColor);
                        context.lineTo(Gut[8].X, Gut[8].Y, GutterColor);

                        if (ThreeDBut == false)
                        {
                            //Fills
                            context.FillColor = Concrete;
                            context.FillStyle = 0;
                            for (i = 9; i <= 11; i++)
                            {
                                H = Gut[i].X;
                                K = Gut[i].Y;
                                ConvertToPixels();
                                //Fx = FloodFill(Pic1.Hdc, H, K, GutterColor);  ;

                            }
                        }
                    }
                    else
                    { //Sag
                        context.lineTo(Gut[1].X, Gut[1].Y, Gut[2].X, Gut[2].Y, GutterColor);
                        context.lineTo(Gut[4].X, Gut[4].Y, GutterColor);
                        context.lineTo(Gut[3].X, Gut[3].Y, GutterColor);
                        context.lineTo(Gut[1].X, Gut[1].Y, GutterColor);

                        context.lineTo(Gut[5].X, Gut[5].Y, Gut[8].X, Gut[8].Y, GutterColor);
                        context.lineTo(Gut[7].X, Gut[7].Y, GutterColor);
                        context.lineTo(Gut[6].X, Gut[6].Y, GutterColor);
                        context.lineTo(Gut[5].X, Gut[5].Y, GutterColor);

                        context.lineTo(Gut[3].X, Gut[3].Y, Gut[6].X, Gut[6].Y, GutterColor);
                        context.lineTo(Gut[4].X, Gut[4].Y, Gut[7].X, Gut[7].Y, GutterColor);

                        if (ThreeDBut == false)
                        { //Back horiz. line
                            context.lineTo(Gut[2].X, Gut[2].Y, Gut[8].X, Gut[8].Y, GutterColor);
                            //Fills
                            context.FillColor = Concrete;
                            context.FillStyle = 0;
                            for (i = 9; i <= 11; i++)
                            {
                                H = Gut[i].X;
                                K = Gut[i].Y;
                                ConvertToPixels();
                                //Fx = FloodFill(Pic1.Hdc, H, K, GutterColor);  ;

                            }
                            if (Gut[3].Y > Gut[1].Y)
                            {
                                //Draw front & Fill it
                                context.lineTo(Gut[1].X, Gut[1].Y, Gut[5].X, Gut[5].Y, GutterColor);
                                H = Gut[12].X;
                                K = Gut[12].Y;
                                ConvertToPixels();
                                //Fx = FloodFill(Pic1.Hdc, H, K, GutterColor);  

                            }

                            //Inlet invert Fill
                            context.FillColor = Concrete;
                            H = Gut[3].X + (Gut[6].X - Gut[3].X) * 0.5;
                            K = Gut[6].Y + (Gut[4].Y - Gut[6].Y) * 0.5;
                            ConvertToPixels();
                            //Fx = FloodFill(Pic1.Hdc, H, K, GutterColor);  

                            context.FillColor = QBColor[15];
                        }

                    }

                    if (InCurrentCalc == true)
                    {
                        //Water Spread
                        WedgeX = 0.5 * ScaleXX - 0.5 * InlISpread;
                        WedgeX = WedgeX + 0.5 * ISpreadWidth;
                        //GDepth = 0
                        //GSpread = 0

                        if (INLSAG == EnumInletLocation.On_grade)
                        { //On grade
                            Water[1].X = 0.5 * ScaleXX - 0.5 * InlGSpread;
                            Water[1].Y = Y1L + InlGDepth; //Upstream
                            Water[1].Z = ZMin;
                            Water[2].X = 0.5 * ScaleXX - 0.5 * InlISpread;
                            Water[2].Y = Y1L + InlIDepth; //At inlet
                            Water[2].Z = 0;
                            if (InlQBY > 0) { BypSpreadDrop(); }
                            Water[3].X = 0.5 * ScaleXX - 0.5 * InlBypSpread;
                            Water[3].Y = Y1L + InlBypDepth;
                            Water[3].Z = ZDepth; //Either grate length || Inlet length
                            Water[4].X = Water[3].X;
                            Water[4].Y = Y1L + InlBypDepth;
                            Water[4].Z = ZMax;
                            Water[5].X = 0.5 * ScaleXX + 0.5 * InlBypSpread;
                            Water[5].Y = Y1L + InlBypDepth;
                            Water[5].Z = ZMax;
                            Water[6].X = 0.5 * ScaleXX + 0.5 * InlBypSpread;
                            Water[6].Y = Y1L + InlBypDepth;
                            Water[6].Z = ZDepth;
                            Water[7].X = 0.5 * ScaleXX + 0.5 * InlISpread;
                            Water[7].Y = Y1L + InlIDepth; //At inlet
                            Water[7].Z = 0;
                            Water[8].X = 0.5 * ScaleXX + 0.5 * InlGSpread;
                            Water[8].Y = Y1L + InlGDepth; //Upstream
                            Water[8].Z = Water[1].Z;
                            if (INLSAG == EnumInletLocation.In_Sag)
                            {
                                Water[3].Y = Water[2].Y;
                                Water[6].X = Water[7].X;
                                Water[6].Y = Water[7].Y;
                            }

                        }
                        else
                        { //Sag
                            H = InlIDepth / INSX; //Horiz. Dist.
                            if (H > System.Math.Abs(ZMin)) { H = System.Math.Abs(ZMin); }
                            //GDepth = InlGDepth 
                            //GSpread = InlISpread 
                            Water[1].X = 0.5 * ScaleXX - 0.5 * ISpreadWidth;
                            Water[1].Y = Y1L + InlIDepth; //Upstream
                            Water[1].Z = -H;
                            Water[2].X = Water[1].X;
                            Water[2].Y = Water[1].Y;
                            Water[2].Z = 0;
                            Water[3].X = Water[1].X;
                            Water[3].Y = Water[1].Y;
                            Water[3].Z = ZDepth;
                            Water[4].X = Water[1].X;
                            Water[4].Y = Water[1].Y;
                            Water[4].Z = ZDepth + H;
                            Water[5].X = 0.5 * ScaleXX + 0.5 * ISpreadWidth;
                            Water[5].Y = Water[1].Y;
                            Water[5].Z = Water[4].Z;
                            Water[6].X = Water[5].X;
                            Water[6].Y = Water[1].Y;
                            Water[6].Z = ZDepth;
                            Water[7].X = Water[5].X;
                            Water[7].Y = Water[1].Y;
                            Water[7].Z = 0;
                            Water[8].X = Water[5].X;
                            Water[8].Y = Water[1].Y;//Upstream
                            Water[8].Z = Water[1].Z;
                        } //On grade || sag

                        //Locate flood-fill point
                        Water[9].X = Water[1].X + (Water[8].X - Water[1].X) * 0.5;
                        Water[9].Y = Water[1].Y;
                        Water[9].Z = 0;

                        //Transform
                        context.lineWidth = 2;
                        if (ThreeDBut == false)
                        {
                            for (i = 1; i <= 9; i++)
                            {
                                Xt = Water[i].X;
                                Yt = Water[i].Y;
                                Zt = Water[i].Z;
                                Water[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                                Water[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                            }
                            context.lineTo(Water[1].X, Water[1].Y, Water[2].X, Water[2].Y, WaterColor); ;//new RGB(49, 106, 197)
                            context.lineTo(Water[3].X, Water[3].Y, WaterColor);
                            if (InlQBY > 0 || INLSAG == EnumInletLocation.In_Sag)
                            {
                                context.lineTo(Water[4].X, Water[4].Y, WaterColor);
                                context.lineTo(Water[5].X, Water[5].Y, WaterColor);
                            }
                            context.lineTo(Water[6].X, Water[6].Y, WaterColor);
                            context.lineTo(Water[7].X, Water[7].Y, WaterColor);
                            context.lineTo(Water[8].X, Water[8].Y, WaterColor);
                            context.lineTo(Water[1].X, Water[1].Y, WaterColor);

                            //Fill water
                            context.FillColor = WaterFill;
                            context.DrawMode = 10;
                            H = Water[9].X;
                            K = Water[9].Y;
                            ConvertToPixels();
                            //Fx = FloodFill(Pic1.Hdc, H, K, WaterColor);  

                            context.FillColor = QBColor[15];
                            context.DrawMode = 13;

                        }
                        else
                        { //2D
                            context.DrawStyle = 0;
                            context.lineWidth = 2;
                            context.lineTo(Water[2].X, Water[2].Y, Water[7].X, Water[7].Y, new RGB(49, 106, 197));
                        }
                    } //InCurrentCalc(Sc%)

                    //Grate
                    i = 4 + 2 * ((int)(GTL / (0.5 * SIFeet)));
                    Grate[1].X = 0.5 * ScaleXX - 0.5 * GTW;
                    Grate[1].Y = Y1L;
                    Grate[1].Z = 0;
                    Grate[2].X = Grate[1].X;
                    Grate[2].Y = Y1L;
                    Grate[2].Z = GTL;
                    Grate[3].X = Grate[1].X + GTW;
                    Grate[3].Y = Y1L;
                    Grate[3].Z = Grate[2].Z;
                    Grate[4].X = Grate[3].X;
                    Grate[4].Y = Y1L;
                    Grate[4].Z = 0;
                    Fuck = 0;
                    Z = 5;
                    for (Z = 5; Z <= i - 1; Z += 2)
                    {
                        Fuck = Fuck + 1;
                        Grate[Z].X = Grate[1].X;
                        Grate[Z].Y = Y1L;
                        Grate[Z].Z = 0 + 0.5 * SIFeet * Fuck;
                        Grate[Z + 1].X = Grate[1].X + GTW;
                        Grate[Z + 1].Y = Y1L;
                        Grate[Z + 1].Z = Grate[Z].Z;
                    }
                    //Transform
                    Z = i;
                    if (ThreeDBut == false)
                    {
                        for (i = 1; i <= Z; i++)
                        {
                            Xt = Grate[i].X;
                            Yt = Grate[i].Y;
                            Zt = Grate[i].Z;
                            Grate[i].X = (Xu * Zt - Xt * Zu) / (Zt - Zu);
                            Grate[i].Y = (Yu * Zt - Yt * Zu) / (Zt - Zu);
                        }
                    }
                    if (ThreeDBut == true)
                    { //2D
                        context.lineWidth = 5;
                        context.lineTo(Grate[1].X, Grate[1].Y, Grate[4].X, Grate[4].Y, QBColor[0]);
                        context.lineWidth = 2;
                    }
                    else
                    {
                        context.lineTo(Grate[1].X, Grate[1].Y, Grate[2].X, Grate[2].Y, QBColor[0]);
                        context.lineTo(Grate[3].X, Grate[3].Y, QBColor[0]);
                        context.lineTo(Grate[4].X, Grate[4].Y, QBColor[0]);
                        context.lineTo(Grate[1].X, Grate[1].Y, QBColor[0]);
                        for (i = 5; i <= Z - 1; i += 2)
                        {
                            context.lineTo(Grate[i].X, Grate[i].Y, Grate[i + 1].X, Grate[i + 1].Y, QBColor[0]);
                        }
                    }

                    //Dotted line
                    if (ThreeDBut == false && InCurrentCalc == true)
                    {
                        context.lineWidth = 1;
                        context.DrawStyle = 1;
                        context.lineTo(Water[2].X, Water[2].Y, Water[7].X, Water[7].Y, new RGB(49, 106, 197));
                        context.DrawStyle = 0;
                    }
                }
                #endregion
                break;
        }

        #region End

        if (InCurrentCalc == true && ThreeDBut == true && InlISpread >= 0.1 * ScaleXX)
        {
            //DRAW WEDGE
            context.lineWidth = 1;
            context.FillStyle = 0;
            context.FillColor = new RGB(0, 0, 255);
            if (InlType == EnumInletType.Drop_Curb || InlType == EnumInletType.Drop_Grate)
            {
                context.CurrentX = WedgeX;
                context.CurrentY = Y1L + InlIDepth + 0.015 * ScaleYY;
            }
            else
            {
                context.CurrentX = X1L + InlISpread * 0.35;
                context.CurrentY = Y1L + INGW * (INSW - DepSlope) + InlIDepth + 0.015 * ScaleYY;
            }
            context.lineTo(true, 0.0, 0.0, true, -0.02 * ScaleXX, 0.06 * ScaleYY, new RGB(49, 106, 197));
            context.lineTo(true, 0.0, 0.0, true, 2.0 * 0.02 * ScaleXX, 0.0, new RGB(49, 106, 197));
            context.lineTo(true, 0.0, 0.0, true, -0.02 * ScaleXX, -0.06 * ScaleYY, new RGB(49, 106, 197));

            if (InlType == EnumInletType.Drop_Curb || InlType == EnumInletType.Drop_Grate)
            {
                context.CurrentX = WedgeX - 0.05 * InlISpread;
                context.CurrentY = Y1L + InlIDepth - 0.02 * ScaleYY;
            }
            else
            {
                context.CurrentX = X1L + InlISpread * 0.35 - 0.05 * InlISpread;
                context.CurrentY = Y1L + INGW * (INSW - DepSlope) + InlIDepth - 0.02 * ScaleYY;
            }

            context.lineTo(true, 0.0, 0.0, true, 0.1 * InlISpread, 0.0, new RGB(49, 106, 197));

        }

        //Dimension lines
        //if ( GoMetric = true; {
        //    DimM = My.Resources.MainMenu_All_dimensions_in_meters
        //}else{
        //    DimM = My.Resources.MainMenu_All_dimensions_in_feet
        //}
        //DimM = IIf(UnitConvertersManager.IsSI, My.Resources.MainMenu_All_dimensions_in_meters, My.Resources.MainMenu_All_dimensions_in_feet);
        context.lineWidth = 1;
        context.ForeColor = QBColor[0];
        context.CurrentX = context.ScaleLeft + 0.5 * context.TextWidth("A");
        context.CurrentY = context.ScaleTop;
        context.Print(DimM);

        if (InlGVel > InlSplashOverVel && InlSplashOverVel > 0)
        {
            //Pic1.Font = VB6.FontChangeBold(Pic1.Font, true);
            //Text = My.Resources.ExpressApp_Warning;
            context.ForeColor = QBColor[4];
            context.CurrentX = context.ScaleLeft + 0.5 * context.TextWidth("A");
            context.CurrentY = context.ScaleTop + context.ScaleHeight - context.TextHeight("A");
            context.Print(Text);
            //Text = String.Format(My.Resources.MainMenu_The_gutter_velocity__0___2___is_greater_than_the_splash_over_velocity__1___2____Please_consider_a_longer_grate_, VB6.Format(InlGVel, UnitFormat), VB6.Format(InlSplashOverVel, UnitFormat), VelFts2Ms.Unit);
            context.lineWidth = 1;
            context.ForeColor = QBColor[0];
            context.CurrentX = context.ScaleLeft + 0.5 * context.TextWidth("A");
            context.CurrentY = context.ScaleTop + context.ScaleHeight;
            context.Print(Text);
            //Pic1.Font = VB6.FontChangeBold(Pic1.Font, false);
        }

        context.FillColor = new RGB(255, 0, 0);
        context.FillStyle = 0;

        Radi = 0.008 * ScaleXX;
        switch (InlType)
        {
            case EnumInletType.Curb ^ EnumInletType.Grate ^ EnumInletType.Combination ^ EnumInletType.Slotted:
                if (INGW >= 0)
                {
                    context.lineTo(X1L, Y1L + INGW * (INSW - DepSlope) - 0.1 * Y1L, X1L, Y1L + INGW * (INSW - DepSlope) - 0.75 * Y1L, new RGB(255, 0, 0));
                    context.lineTo(X1L + INGW, Y1L + INGW * DepSlope + INGW * (INSW - DepSlope) - 0.1 * Y1L, X1L + INGW, Y1L + INGW * (INSW - DepSlope) - 0.75 * Y1L, new RGB(255, 0, 0));
                    if (InCurrentCalc = true && InlISpread >= INGW)
                    {
                        context.lineTo(X1L + InlISpread, Y1L + INGW * (INSW - DepSlope) + InlIDepth - 0.1 * Y1L, X1L + InlISpread, Y1L + INGW * (INSW - DepSlope) - 0.75 * Y1L, new RGB(255, 0, 0));
                        context.lineTo(X1L - 0.2 * X1L, Y1L + INGW * (INSW - DepSlope) - 0.5 * Y1L, X1L + InlISpread + 0.2 * X1L, Y1L + INGW * (INSW - DepSlope) - 0.5 * Y1L, new RGB(255, 0, 0));
                    }
                    else
                    {
                        context.lineTo(X1L - 0.2 * X1L, Y1L + INGW * (INSW - DepSlope) - 0.5 * Y1L, X1L + INGW + 0.2 * X1L, Y1L + INGW * (INSW - DepSlope) - 0.5 * Y1L, new RGB(255, 0, 0));
                    }

                    context.FillColor = new RGB(255, 0, 0);
                    context.FillStyle = 0;
                    //Radi! = 0.008 * ScaleXX!
                    context.Circle(X1L, Y1L + INGW * (INSW - DepSlope) - 0.5 * Y1L, Radi, new RGB(255, 0, 0));
                    context.Circle(X1L + INGW, Y1L + INGW * (INSW - DepSlope) - 0.5 * Y1L, Radi, new RGB(255, 0, 0));
                    if (InCurrentCalc = true && InlISpread >= INGW)
                    {
                        context.Circle(X1L + InlISpread, Y1L + INGW * (INSW - DepSlope) - 0.5 * Y1L, Radi, new RGB(255, 0, 0));
                    }
                    //Gutter Width
                    if (INGW > 0)
                    {
                        LabelX = X1L + 0.5 * INGW;
                        //Text = VB6.Format(INGW, "0.0#");
                        context.CurrentX = LabelX - context.TextWidth(Text) * 0.5;
                        context.CurrentY = Y1L + INGW * (INSW - DepSlope) - 0.5 * Y1L - 1.1 * context.TextHeight(Text);
                        context.Print(Text);
                    }
                }
                if (InCurrentCalc == true && InlISpread >= INGW)
                {
                    LabelX = X1L + InlISpread - 0.5 * (InlISpread - INGW);
                    //Text = VB6.Format((InlISpread - INGW) * LengthFt2M, "0.0#");
                    context.CurrentX = LabelX - context.TextWidth(Text) * 0.5;
                    context.CurrentY = Y1L + INGW * (INSW - DepSlope) - 0.5 * Y1L - 1.1 * context.TextHeight(Text);
                    context.Print(Text);
                }
                if (InCurrentCalc == true)
                {
                    //Inlet depth dimension
                    context.lineTo(X1L - 0.1 * X1L, Y1L + INGW * (INSW - DepSlope), X1L - 0.4 * X1L, Y1L + INGW * (INSW - DepSlope), new RGB(255, 0, 0));
                    context.lineTo(X1L - 0.1 * X1L, Y1L + INGW * (INSW - DepSlope) + InlIDepth, X1L - 0.4 * X1L, Y1L + INGW * (INSW - DepSlope) + InlIDepth, new RGB(255, 0, 0));
                    context.lineTo(X1L - 0.3 * X1L, Y1L + INGW * (INSW - DepSlope) - 0.2 * Y1L, X1L - 0.3 * X1L, Y1L + INGW * (INSW - DepSlope) + 0.2 * Y1L + InlIDepth, new RGB(255, 0, 0));
                    context.Circle(X1L - 0.3 * X1L, Y1L + INGW * (INSW - DepSlope) + InlIDepth, Radi, new RGB(255, 0, 0));
                    context.Circle(X1L - 0.3 * X1L, Y1L + INGW * (INSW - DepSlope), Radi, new RGB(255, 0, 0));
                    context.FillColor = new RGB(0, 0, 0);

                    LabelX = X1L - 0.4 * X1L;
                    //Text = VB6.Format(InlIDepth, "0.0#");
                    context.CurrentX = LabelX - context.TextWidth(Text);
                    context.CurrentY = Y1L + INGW * (INSW - DepSlope) + 0.5 * InlIDepth - 0.5 * context.TextHeight(Text);
                    context.Print(Text);
                }
                else if (InlType != EnumInletType.Grate && InlType != EnumInletType.Slotted)
                {
                    //Show depr & Throat Ht.
                    context.lineTo(X1L - 0.1 * X1L, Y1L + INGW * (INSW - DepSlope), X1L - 0.4 * X1L, Y1L + INGW * (INSW - DepSlope), new RGB(255, 0, 0));
                    context.lineTo(X1L - 0.1 * X1L, Y1L + Yp - TopInletWidth, X1L - 0.4 * X1L, Y1L + Yp - TopInletWidth, new RGB(255, 0, 0));
                    context.lineTo(X1L - 0.3 * X1L, Y1L + INGW * (INSW - DepSlope) - 0.2 * Y1L, X1L - 0.3 * X1L, Y1L + Yp - TopInletWidth + 0.2 * Y1L, new RGB(255, 0, 0));
                    context.Circle(X1L - 0.3 * X1L, Y1L + Yp - TopInletWidth, Radi, new RGB(255, 0, 0));
                    context.Circle(X1L - 0.3 * X1L, Y1L + INGW * (INSW - DepSlope), Radi, new RGB(255, 0, 0));
                    context.FillColor = new RGB(0, 0, 0);
                    LabelX = X1L - 0.4 * X1L;
                    //Text = VB6.Format((Dep + INTH / InchPerFt) * LengthFt2M, "0.0#");
                    context.CurrentX = LabelX - context.TextWidth(Text);
                    context.CurrentY = Y1L + 0.5 * (Yp - TopInletWidth);
                    context.Print(Text);
                }
                if (ThreeDBut == true)
                {
                    //Label Sx
                    //Text = My.Resources.MainMenu_Sx;
                    context.CurrentX = X1L + INGW + (ScaleXX * XRed) * 0.5;
                    context.CurrentY = Y1L + INGW * INSW + ScaleXX * XRed * INSX * 0.5 + 0.25 * context.TextHeight(Text);
                    context.Print(Text);
                }
                break;
            case EnumInletType.Drop_Curb ^ EnumInletType.Drop_Grate:
                {
                    //Bottom Dimension
                    context.FillColor = new RGB(255, 0, 0);
                    context.FillStyle = 0;
                    if (InCurrentCalc == true)
                    {
                        //Horiz
                        //Pic1.PSet(0.5 * ScaleXX - 0.55 * ISpreadWidth, 0.04 * ScaleYY);
                        context.lineTo(true, 0.0, 0.0, true, ISpreadWidth * 1.1, 0.0, new RGB(255, 0, 0));
                        //Vert left
                        context.lineTo(0.5 * ScaleXX - 0.5 * ISpreadWidth, 0, 0.5 * ScaleXX - 0.5 * ISpreadWidth, Y1L + 0.8 * InlIDepth, new RGB(255, 0, 0));
                        context.Circle(0.5 * ScaleXX - 0.5 * ISpreadWidth, 0.04 * ScaleYY, Radi, new RGB(255, 0, 0));
                        LabelX = 0.5 * ScaleXX - 0.5 * IGutterWidth - 0.5 * InlIDepth / INSX;
                        //Text = VB6.Format((ISpreadWidth * 0.5 - 0.5 * IGutterWidth) * LengthFt2M, "0.00");
                        context.CurrentX = LabelX - 0.5 * context.TextWidth(Text);
                        context.CurrentY = 0 - 0.5 * context.TextHeight(Text);
                        context.Print(Text);
                    }
                    //Vert Left/bottom
                    context.lineTo(0.5 * ScaleXX - 0.5 * IGutterWidth, 0, 0.5 * ScaleXX - 0.5 * IGutterWidth, 0.9 * Y1L, new RGB(255, 0, 0));
                    context.Circle(0.5 * ScaleXX - 0.5 * IGutterWidth, 0.04 * ScaleYY, Radi, new RGB(255, 0, 0));
                    LabelX = 0.5 * ScaleXX;
                    //Text = VB6.Format(IGutterWidth * LengthFt2M, "0.00");
                    context.CurrentX = LabelX - 0.5 * context.TextWidth(Text);
                    context.CurrentY = 0 - 0.5 * context.TextHeight(Text);
                    context.Print(Text);

                    //Vert Rt/bottom
                    context.lineTo(0.5 * ScaleXX + 0.5 * IGutterWidth, 0, 0.5 * ScaleXX + 0.5 * IGutterWidth, 0.9 * Y1L, new RGB(255, 0, 0));
                    context.Circle(0.5 * ScaleXX + 0.5 * IGutterWidth, 0.04 * ScaleYY, Radi, new RGB(255, 0, 0));

                    if (InCurrentCalc == true)
                    {
                        //Vert Rt
                        context.lineTo(0.5 * ScaleXX + 0.5 * ISpreadWidth, 0, 0.5 * ScaleXX + 0.5 * ISpreadWidth, Y1L + 0.8 * InlIDepth, new RGB(255, 0, 0));
                        context.Circle(0.5 * ScaleXX + 0.5 * ISpreadWidth, 0.04 * ScaleYY, Radi, new RGB(255, 0, 0));
                        LabelX = 0.5 * ScaleXX + 0.5 * IGutterWidth + 0.5 * InlIDepth / INSX;
                        //Text = VB6.Format((ISpreadWidth * 0.5 - 0.5 * IGutterWidth) * LengthFt2M, "0.00");
                        context.CurrentX = LabelX - 0.5 * context.TextWidth(Text);
                        context.CurrentY = 0 - 0.5 * context.TextHeight(Text);
                        context.Print(Text);
                    }

                    if (InCurrentCalc == true)
                    {
                        //Inlet depth dimension
                        context.FillColor = new RGB(255, 0, 0);
                        context.FillStyle = 0;
                        //Horiz
                        context.lineTo(X1L + X1L - 0.75 * IGutterWidth, Y1L, 0.5 * ScaleXX - 0.5 * IGutterWidth - 0.6 * ISpreadWidth - 0.025 * ScaleXX, Y1L, new RGB(255, 0, 0));
                        context.lineTo(0.5 * ScaleXX - 0.6 * ISpreadWidth, Y1L + InlIDepth, 0.5 * ScaleXX - 0.5 * IGutterWidth - 0.6 * ISpreadWidth - 0.025 * ScaleXX, Y1L + InlIDepth, new RGB(255, 0, 0));
                        //Vert
                        context.lineTo(0.5 * ScaleXX - 0.5 * IGutterWidth - 0.6 * ISpreadWidth, Y1L - 0.2 * Y1L, 0.5 * ScaleXX - 0.5 * IGutterWidth - 0.6 * ISpreadWidth, Y1L + 0.2 * Y1L + InlIDepth, new RGB(255, 0, 0));
                        context.Circle(0.5 * ScaleXX - 0.5 * IGutterWidth - 0.6 * ISpreadWidth, Y1L + InlIDepth, Radi, new RGB(255, 0, 0));
                        context.Circle(0.5 * ScaleXX - 0.5 * IGutterWidth - 0.6 * ISpreadWidth, Y1L, Radi, new RGB(255, 0, 0));
                        context.FillColor = new RGB(0, 0, 0);

                        LabelX = 0.5 * ScaleXX - 0.5 * IGutterWidth - 0.64 * ISpreadWidth;
                        //Text = VB6.Format(InlIDepth, "0.0#");
                        context.CurrentX = LabelX - context.TextWidth(Text);
                        context.CurrentY = Y1L + 0.5 * InlIDepth - 0.5 * context.TextHeight(Text);
                        context.Print(Text);
                    }
                }
                break;
        }
        #endregion

        context.EndDraw();
    }

    public void PlotGutter(Canvas context, double GutterWidth, double CrossSlopeW, double CrossSlopeX, double GutterDepth, double SCX)
    {
        context.Cls();
        List<Point> StaEl = new List<Point>();

        double XMAXV = 0;
        double YMAXV = -10000;
        double YMINV = 10000;
        YMAXV = YMINV + 0.5;
        double Gx1 = GutterWidth;
        double Gy1 = CrossSlopeW * Gx1;
        double Gx2 = 0;
        double Gy2 = GutterDepth - Gy1;
        if (Gy2 < 0)
        {
            Gy2 = 0;
            XMAXV = Gx1;
        }
        else
        {
            Gx2 = Gy2 / CrossSlopeX;
            XMAXV = GutterWidth + Gx2;
        }
        //if (Fix(YMAXV + 1) - YMAXV > 1)
        //{
        //    YMAXV = Fix(YMAXV);
        //}
        //else
        //{
        //    YMAXV = Fix(YMAXV + 1);
        //}
        //DATUM = YMINV;
        //YMINV = Fix(YMINV);
        //HT = YMAXV;
        //double HLeft = 0;
        //double HRight = 0;
        SetScreen(context);
        double H = 0;
        double K = 0;
        context.ForeColor = new RGB(0, 0, 0);
        context.lineWidth = 1;
        H = StaEl[1].X; //Sta
        K = StaEl[1].Y; //Elev
        context.PSet(H, K);
        context.lineWidth = 3;
        for (int x = 1; x <= 50; x++)
        {
            H = StaEl[x].X; //Sta
            K = StaEl[x].Y; //Elev
            if (H > 0 || K > 0)
            {
                context.lineTo(H + SCX, K);
            }
        }
        context.lineTo(context.CurrentX + SCX, context.CurrentY);
        context.EndDraw();
    }

    public void SetScreen(Canvas context)
    {

    }
}
