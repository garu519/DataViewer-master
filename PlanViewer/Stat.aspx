<%@ Page Title="Статистика" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stat.aspx.cs" Inherits="PlanViewer._Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TeamProjectDBConnectionString1 %>"   
   >
        </asp:SqlDataSource>
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2>Отображение статистики по разнице плановых и фактических затрат на материалы</h2>
            </hgroup>
            <asp:Chart ID="Chart1" runat="server">
                <series>
                    <asp:Series Name="Series1" >
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </chartareas>
            </asp:Chart>
        
        
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <section style ="vertical-align: middle" >

    </section>
</asp:Content>
