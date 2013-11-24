<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewPlan.aspx.cs" Inherits="PlanViewer.ViewPlan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<!-- jTable style file -->
<link href="/Scripts/jtable/themes/standard/blue/jtable_blue.css" rel="stylesheet" type="text/css" />
 
<!-- A helper library for JSON serialization -->
<script type="text/javascript" src="/Scripts/jtable/external/json2.js"></script>
<!-- Core jTable script file -->
<script type="text/javascript" src="/Scripts/jtable/jquery.jtable.js"></script>
<!-- ASP.NET Web Forms extension for jTable -->
<script type="text/javascript" src="/Scripts/jtable/extensions/jquery.jtable.aspnetpagemethods.js"></script>
    <div id="PlanTableContainer"></div>
    <script type="text/javascript">

        $(document).ready(function () {

            //Prepare jtable plugin
            $('#PlanTableContainer').jtable({
                title: 'Просмотр плана',
                paging: true, //Enables paging
                pageSize: 10, //Actually this is not needed since default value is 10.
                sorting: true, //Enables sorting
                defaultSorting: 'Name ASC', //Optional. Default sorting on first load.
                actions: {
                     listAction: '/PagingAndSorting.aspx/PlanList',
                 //   createAction: '/PagingAndSorting.aspx/CreatePlan',
                 //  updateAction: '/PagingAndSorting.aspx/UpdatePlan',
                 //   deleteAction: '/PagingAndSorting.aspx/DeletePlan'
                },
                fields: {
                    ID: {
                        title: 'ID',
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    Customer: {
                        title: 'Заказчик',
                        //width: '23%'
                    },
                    Contractor: {
                        title: 'Подрядчик',
                       // width: '23%'
                    },
                    WorkObj: {
                        title: 'Объект работ',
                        list: false
                    },
                    TypeOfWork: {
                        title: 'Вид работ',
                        list: false
                    },
                    CostName: {
                        title: 'Наименование единичной расценки',
                        list: false
                    },
                    UnitName: {
                        title: 'Единица изменерия',
                    },
                    Labor: {
                        title: 'Трудозатраты',
                        list: false
                    },
                    Materials: {
                        title: 'Материалы',
                        list: false
                    },
                    Mechanisms: {
                        title: 'Механизмы',
                        list: false
                    },
                    Status: {
                        title: 'Текущий статус',
                       // width: '13%',
                        options: { 'T': 'Подтверджен', 'F': 'Отклонен','D':'Ожидает подтверждения' }
                    },
                    
                }
            });

            //Load student list from server
            $('#PlanTableContainer').jtable('load');
        });

</script>

    <asp:SqlDataSource ID="PlansDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:TeamProjectDBConnectionString1 %>" OnSelecting="PlansDataSource_Selecting" SelectCommand="SELECT 'Plan #' + CAST([ID] AS varchar(5)) +' On object'+ [Object] as res FROM [Plan]"></asp:SqlDataSource>
   
    
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Одобрить" />
    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Отклонить"/>

    <asp:TextBox ID="commentTextBox" runat="server" style="text-align:right;" />
    <asp:Button ID="commentButton" runat="server" style="text-align:right;" OnClick="sendComment" Text="Отправить комментарий подрядчику"/>
    
</asp:Content>
