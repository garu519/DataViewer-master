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
            
            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="Name" DataValueField="PlanID" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true" OnDataBound="DropDownList1_DataBound" >
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TeamProjectDBConnectionString1 %>" SelectCommand="SELECT Customer.Name, [Plan].PlanID FROM Contractor INNER JOIN [Plan] ON Contractor.ID = [Plan].Contractor INNER JOIN Customer ON [Plan].Customer = Customer.ID">
            </asp:SqlDataSource>
            
        </div>
    </asp:Panel>
    <div>
        <asp:Panel ID="Panel2" runat ="server">
            <asp:Table ID="Table1" runat="server" Caption ="План">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Text="Номер плана"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Объект работ"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Вид работ"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Наименование единичной расценки"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Единица измерения"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Трудозатраты"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Материалы"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Механизмы"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Статус"></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </asp:Panel>
    </div>
        
        <asp:Panel runat="server" ID="Panel3">
            <asp:GridView GridLines="Both" Width="" Caption="Факт" ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="ID" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
        <Columns>
            <asp:TemplateField HeaderText="#plana" HeaderStyle-HorizontalAlign="Center">
                <EditItemTemplate>
                    <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Объект работ">
                <EditItemTemplate>
                    <asp:TextBox ID="FactObject" runat="server" Text='<%# Bind("FactObject") %>' ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="FactObjectlab" runat="server" Text='<%# Bind("FactObject") %>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
      <%--      <asp:BoundField DataField="FactObject" HeaderText="Объект работ" ApplyFormatInEditMode="false"/>
            <asp:BoundField DataField="WorkType" HeaderText="Вид работ" />
            <asp:BoundField DataField="CostName" HeaderText="Наименование единичной расценки" />
            <asp:BoundField DataField="UnitName" HeaderText="Единица измерения" />
            <asp:BoundField DataField="Labor" HeaderText="Трудозатраты" />
            <asp:BoundField DataField="Materials" HeaderText="Материалы" />
            <asp:BoundField DataField="Mechanisms" HeaderText="Механизмы" /> --%>       
            <asp:CommandField ShowEditButton="true" />
            <asp:CommandField ShowDeleteButton="true" />                          
        </Columns>
                <EditRowStyle CssClass="GridViewEditRow" />
                
        </asp:GridView>
        </asp:Panel>
               
        

</asp:Content>
