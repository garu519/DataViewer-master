<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EnterFact.aspx.cs" Inherits="PlanViewer.CreatePlan1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .GridViewEditRow input[type=text] {width:50px;} /* size textboxes */
        .GridViewEditRow select { width:50px;}        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        <div>
            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="res" DataValueField="PlanID" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true" OnDataBound="DropDownList1_DataBound" >
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TeamProjectDBConnectionString1 %>">
            </asp:SqlDataSource>
            
        </div>
    </asp:Panel>
    <div>
        <asp:Panel ID="Panel5" runat ="server">
            <asp:Label runat="server" ID="NoPlan" Text="На данный момент у Вас нет одобренных планов" Width="400" Visible="false"></asp:Label>
            <asp:Table ID="Table" runat="server" Caption ="План" BackColor="LightGreen" BorderWidth="1px">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Text="№  " BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" Visible="false"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Объект работ" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Вид работ" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Наименование единичной расценки" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Единица измерения" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Трудозатраты (План)" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Трудозатраты (Факт)" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" BackColor="LightBlue"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Материалы (План)" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Материалы (Факт)" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" BackColor="LightBlue"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Механизмы (План)" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Механизмы (Факт)" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" BackColor="LightBlue"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Статус" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" Visible="false"></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </asp:Panel>
        <asp:Button runat="server" ID="sendFact" Text="Отправить факт" OnClick="sendFact_Click"/>
    </div>   
</asp:Content>
