<%@ Page Title="Создание плана" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreatePlan.aspx.cs" Inherits="PlanViewer.createPlan" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <p>
                Пожалуйста, заполните все необходимые поля и нажмите кнопку "Отправить на согласование"
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:SqlDataSource ID="PlansDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:TeamProjectDBConnectionString1 %>" OnSelecting="PlansDataSource_Selecting" SelectCommand="SELECT 'Plan #' + CAST([ID] AS varchar(5)) +' On object'+ [Object] as res FROM [Plan]"></asp:SqlDataSource>
    <asp:TextBox ID="PlanName" runat="server">Название плана</asp:TextBox>
    <br />
    <asp:GridView ID="GridView1" runat="server" DataSourceID="PlansDataSource"  OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="925px">
        <Columns>
        <asp:BoundField DataField="ID" 
            InsertVisible="False" ReadOnly="True" SortExpression="ID" />
        <asp:BoundField DataField="WorkObj" HeaderText="Объект работ" 
            SortExpression="WorkObj" />
        <asp:BoundField DataField="TypeOfWork" HeaderText="Тип работ" 
            SortExpression="TypeOfWork" />
        <asp:BoundField DataField="CostName" HeaderText="Наименование единичной расценки" 
            SortExpression="CostName" />
        <asp:BoundField DataField="UnitName" HeaderText="Единица изменерия" 
            SortExpression="UnitName" />
        <asp:BoundField DataField="Labor" HeaderText="Трудозатраты" 
            SortExpression="Labor" />
        <asp:BoundField DataField="Materials" HeaderText="Материалы" 
            SortExpression="Materials" />
        <asp:BoundField DataField="Mechanisms" HeaderText="Механизмы" 
            SortExpression="Mechanisms" />
        <asp:BoundField DataField="Status" HeaderText="Текущий статус" 
            SortExpression="Status" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="AddRecord" runat="server" onclick="ButtonSend_Click" Text="Добавить строку" />
    <br>
    <asp:Button ID="ButtonSend" runat="server" onclick="ButtonSend_Click" Text="Отправить на согласование" />
</asp:Content>
