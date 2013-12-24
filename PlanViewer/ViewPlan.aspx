<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewPlan.aspx.cs" Inherits="PlanViewer.ViewPlan" %>
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
            <asp:Label runat="server" ID="PlanLabel" Text="Выберите план:" Width="400" Visible="true"></asp:Label>
            <br />
            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="res" DataValueField="PlanID" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true" OnDataBound="DropDownList1_DataBound" >
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TeamProjectDBConnectionString1 %>">
            </asp:SqlDataSource>
            
        </div>
    </asp:Panel>
    <div>
        <asp:Panel ID="Panel2" runat ="server">
            <asp:Table ID="Table1" runat="server" Caption ="План/Факт" BackColor="#79EA48" BorderWidth="1px" BorderStyle="Solid" GridLines="Both" BorderColor="Black" ForeColor="Black">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Text="№  " BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" Visible="false"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Объект работ" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Вид работ" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Наименование единичной расценки" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Единица измерения" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Трудозатраты (план)" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Трудозатраты (факт)" BackColor="PowderBlue" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Материалы (план)" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Материалы (факт)" BackColor="PowderBlue" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Механизмы (план)" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Механизмы (факт)" BackColor="PowderBlue" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Статус" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" Visible="false"></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </asp:Panel>
    </div>
        
        
    <asp:Panel runat="server" ID="Panel4">
        <br />
        <asp:Button runat="server" ID="approve" Text="Одобрить" OnClick="approve_Click" Visible="false"/>
        <asp:Button runat="server" ID="reject" Text="Отклонить" OnClick="reject_Click" Visible="false"/>
        <asp:Button runat="server" ID="download" Text="Скачать отчет" OnClick="download_Click1" />        
    </asp:Panel>
    <asp:Panel runat="server" ID="Panel5" HorizontalAlign="Left">
        <br />
        <br />
        <asp:Label ID ="Label3" runat="server" Text = "Отправить письмо заказчику: " Font-Bold="true"/>
        <br />
        <br />
        <asp:Label ID ="Label1" runat="server" Text = "Тема письма: "/>
        <br />
        <asp:TextBox ID="Subject" runat="server" TextMode="SingleLine" ToolTip="Тема письма" ></asp:TextBox>
        <br />
        <asp:Label ID ="Label2" runat="server" Text = "Содержание письма: "/>
        <br />
        <asp:TextBox ID="MessageText" runat="server" TextMode="MultiLine" ToolTip="Текст запроса подрядчику" ></asp:TextBox>
        <br />
        <asp:Button runat="server" ID="sendRequest" Text="Отправить письмо" OnClick="sendRequest_Click" />
    </asp:Panel>
</asp:Content>
