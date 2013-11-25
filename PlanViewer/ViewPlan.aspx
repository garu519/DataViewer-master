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
            
            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="Name" DataValueField="PlanID" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true"  >
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TeamProjectDBConnectionString1 %>">
            </asp:SqlDataSource>
            
        </div>
    </asp:Panel>
    <div>
        <asp:Panel ID="Panel2" runat ="server">
            <asp:Table ID="Table1" runat="server" Caption ="План" BackColor="LightGreen" BorderWidth="1px">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Text="№  "></asp:TableHeaderCell>
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
            <asp:GridView BackColor="LightBlue" GridLines="Both" BorderWidth="1px" Width="" Caption="Факт" ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="ID"  ShowFooter="false" >
        <Columns>
            <asp:TemplateField HeaderText="№  " HeaderStyle-HorizontalAlign="Center">
                <EditItemTemplate>
                    <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Объект работ">
                <EditItemTemplate>
                    <asp:TextBox ID="FactObject" runat="server" Text='<%# Bind("FactObject") %>' ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="FactObjectlab" runat="server" Text='<%# Bind("FactObject") %>' ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="FactObject" runat="server" Text='<%# Bind("FactObject") %>' ></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Вид работ">
                <EditItemTemplate>
                    <asp:TextBox ID="WorkType" runat="server" Text='<%# Bind("WorkType") %>' ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="WorkTypelab" runat="server" Text='<%# Bind("WorkType") %>' ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="WorkType" runat="server" Text='<%# Bind("WorkType") %>' ></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Наименование единичной расценки">
                <EditItemTemplate>
                    <asp:TextBox ID="CostName" runat="server" Text='<%# Bind("CostName") %>' ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="CostNamelab" runat="server" Text='<%# Bind("CostName") %>' ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="CostName" runat="server" Text='<%# Bind("CostName") %>' ></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Единицы измерения">
                <EditItemTemplate>
                    <asp:TextBox ID="UnitName" runat="server" Text='<%# Bind("UnitName") %>' ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="UnitNamelab" runat="server" Text='<%# Bind("UnitName") %>' ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="UnitName" runat="server" Text='<%# Bind("UnitName") %>' ></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Трудозатраты">
                <EditItemTemplate>
                    <asp:TextBox ID="Labor" runat="server" Text='<%# Bind("Labor") %>' ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Laborlab" runat="server" Text='<%# Bind("Labor") %>' ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="Labor" runat="server" Text='<%# Bind("Labor") %>' ></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Материалы">
                <EditItemTemplate>
                    <asp:TextBox ID="Materials" runat="server" Text='<%# Bind("Materials") %>' ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Materialslab" runat="server" Text='<%# Bind("Materials") %>' ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="Materials" runat="server" Text='<%# Bind("Materials") %>' ></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Механизмы">
                <EditItemTemplate>
                    <asp:TextBox ID="Mechanisms" runat="server" Text='<%# Bind("Mechanisms") %>' ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Mechanismslab" runat="server" Text='<%# Bind("Mechanisms") %>' ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="Mechanisms" runat="server" Text='<%# Bind("Mechanisms") %>' ></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>        
            <%--<asp:CommandField ShowEditButton="true" />
            <asp:CommandField ShowDeleteButton="true" />  --%>                                    
            <asp:TemplateField HeaderText="Edit" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                <EditItemTemplate> 
                    <asp:LinkButton ID="lbkUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton> 
                    <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="False" CommandName="Insert" Text="Insert"></asp:LinkButton> 
                </FooterTemplate> 
                <ItemTemplate> 
                    <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
                </ItemTemplate> 
            </asp:TemplateField>
        </Columns>
                <EditRowStyle CssClass="GridViewEditRow" />
                <FooterStyle CssClass="GridViewEditRow" />
                
        </asp:GridView>
        </asp:Panel>
    <asp:Panel runat="server" ID="Panel4">
        <asp:Button runat="server" ID="approve" Text="Одобрить" OnClick="approve_Click"/>
        <asp:Button runat="server" ID="Cancel" Text="Отклонить" OnClick="Cancel_Click"/>
    </asp:Panel>
</asp:Content>
