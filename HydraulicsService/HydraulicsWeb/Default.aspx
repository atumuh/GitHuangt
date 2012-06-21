<%@ Page Title="Gutter Calculation" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="HydraulicsWeb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Hydraulics Web Site!
    </h2>
<%--    <p>
        To learn more about ASP.NET visit <a href="http://www.asp.net" title="ASP.NET Website">www.asp.net</a>.
    </p>
    <p>
        You can also find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
            title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDN</a>.
    </p>--%>
    <asp:Label ID="Label1" runat="server" Text="Input the data :"></asp:Label>
    <asp:Panel ID="Panel1" runat="server" Height="293px" Width="428px">
        <p>
            <label>Longitudinal slope of road :</label>
            <asp:TextBox ID="LongitudinalSlope" runat="server" Text="0.02" Width="200px"></asp:TextBox>
        </p>
        <p>
            <label>Cross-slope of pavement：</label>
            <asp:TextBox ID="CrossSlopeX" runat="server" Text="0.02" Width="200px"></asp:TextBox>
        </p>
        <p>
            <label>Define cross-slope of gutter：</label>
            <asp:TextBox ID="CrossSlopeW" runat="server" Text="0.05" Width="200px"></asp:TextBox>
        </p>
        <p>
            <label>Gutter Width：</label>
            <asp:TextBox ID="GutterWidth" runat="server" Text="4" Width="200px"></asp:TextBox>
        </p>
        <p>
            <label>Manning's roughness：</label>
            <asp:TextBox ID="ManningRoughness" runat="server" Text="0.016" Width="200px"></asp:TextBox>
        </p>
        <p>
            <label>Design Flow：</label>
            <asp:TextBox ID="DesignFlow" runat="server" Text="3" Width="200px"></asp:TextBox>
        </p>
    </asp:Panel>
    <p>
        <asp:Button ID="Submit" runat="server" onclick="Button1_Click" Text="Submit" />
    </p>
    <label>Output Hydraulics Result：
    </label>
    <asp:Panel ID="Panel2" runat="server" 
        Width="427px" Height="219px">
        <p>
            <label>Width of spread：</label>
            <asp:TextBox ID="GutterSpread" runat="server" Width="200px" ReadOnly=True></asp:TextBox>
        </p>
        <p>
            <label>Depth at Curb：</label>
            <asp:TextBox ID="GutterDepth" runat="server" Width="200px" ReadOnly=True></asp:TextBox>
        </p>
        <p>
            <label>Gutter Velocity：</label>
            <asp:TextBox ID="GutterVelocity" runat="server" Width="200px" ReadOnly=True></asp:TextBox>
        </p>
        <p>
            <label>Eo：</label>
            <asp:TextBox ID="Eo" runat="server" Width="200px" ReadOnly=True></asp:TextBox>
        </p>
        <canvas width="400" height="400">
        </canvas>
    </asp:Panel>
</asp:Content>
