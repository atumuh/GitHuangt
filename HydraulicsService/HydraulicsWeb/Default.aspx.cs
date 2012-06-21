using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HydraulicsService;

namespace HydraulicsWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GutterData gd = new GutterData();
            gd.LongitudinalSlope = double.Parse(this.LongitudinalSlope.Text);
            gd.CrossSlopeX = double.Parse(this.CrossSlopeX.Text);
            gd.CrossSlopeW = double.Parse(this.CrossSlopeW.Text);
            gd.GutterWidth = double.Parse(this.GutterWidth.Text);
            double DesignFlow = double.Parse(this.DesignFlow.Text);
            double ManningRoughness = double.Parse(this.ManningRoughness.Text);
            GutterService service = new GutterService();
            GutterResult gr = service.GutterCalculation_DesignFlow(ManningRoughness, gd, DesignFlow);
            this.GutterSpread.Text = Math.Round(gr.GutterSpread, 3).ToString();
            this.GutterDepth.Text = Math.Round(gr.GutterDepth, 3).ToString();
            this.GutterVelocity.Text = Math.Round(gr.GutterVelocity, 3).ToString();
            this.Eo.Text = Math.Round(gr.Eo, 3).ToString();
        }
    }
}
