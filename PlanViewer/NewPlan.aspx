<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewPlan.aspx.cs" Inherits="PlanViewer.NewPlan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
        .GridViewEditRow input[type=text] {width:50px;} /* size textboxes */
        .GridViewEditRow select { width:50px;}        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="Name" DataValueField="ID">
    </asp:DropDownList>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TeamProjectDBConnectionString1 %>" SelectCommand="SELECT [Name], [ID] FROM [Customer]" UpdateCommand="UPDATE [Plan] SET  Object=@Object, WorkType=@WorkType, CostName=@CostName, UnitName=@UnitName, Labor=@Labor, Materials=@Materials, Mechanisms=@Mechanisms    WHERE ID=@ID"  
    DeleteCommand="DELETE FROM [Plan] WHERE ID=@ID" InsertCommand="INSERT INTO [Plan] (Object,WorkType,CostName,UnitName,Labor,Materials,Mechanisms,PlanID) VALUES ('','','','','','','',@id)" >
        <insertparameters>
            <asp:formparameter name="Object"  />
            <asp:formparameter name="WorkType"   />
            <asp:formparameter name="CostName"   />
            <asp:formparameter name="UnitName"   />
            <asp:formparameter name="Labor"   />
            <asp:formparameter name="Materials"   />
            <asp:formparameter name="Mechanisms"   />
        </insertparameters>
    </asp:SqlDataSource>
    <asp:Panel runat="server" ID="Panel3">
        <asp:TextBox runat="server" ID="PlanName" BorderWidth="2px" Text="Введите название плана" Width="317px" OnTextChanged="PlanName_TextChanged"></asp:TextBox>
            <asp:GridView BackColor="LightBlue" GridLines="Both" BorderWidth="1px" Width="" ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="ID" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" ShowFooter="true" OnRowCommand="GridView1_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="№ ">
                    <HeaderStyle Width="80px" />
                    <ItemTemplate>    <%# Convert.ToString( Container.DataItemIndex + 1 ) %>  </ItemTemplate>
                </asp:TemplateField>
            <asp:TemplateField HeaderText="№  " HeaderStyle-HorizontalAlign="Center" Visible ="false">
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
                    <asp:TextBox ID="FactObject" runat="server" Text='<%# Bind("Object") %>' Width="95%"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="FactObjectlab" runat="server" Text='<%# Bind("Object") %>' Width="10%" ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="FactObject" runat="server" Text='<%# Bind("Object") %>' Width="95%"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Вид работ">
                <EditItemTemplate>
                    <asp:TextBox ID="WorkType" runat="server" Text='<%# Bind("WorkType") %>' Width="95%"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="WorkTypelab" runat="server" Text='<%# Bind("WorkType") %>' Width="10%"></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="WorkType" runat="server" Text='<%# Bind("WorkType") %>' Width="95%"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Наименование единичной расценки">
                <EditItemTemplate>
                    <asp:TextBox ID="CostName" runat="server" Text='<%# Bind("CostName") %>' Width="95%"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="CostNamelab" runat="server" Text='<%# Bind("CostName") %>' Width="20%" ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="CostName" runat="server" Text='<%# Bind("CostName") %>' Width="95%"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Единицы измерения">
                <EditItemTemplate>
                    <asp:TextBox ID="UnitName" runat="server" Text='<%# Bind("UnitName") %>' Width="95%" ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="UnitNamelab" runat="server" Text='<%# Bind("UnitName") %>' Width="15%" ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="UnitName" runat="server" Text='<%# Bind("UnitName") %>' Width="95%"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Трудозатраты">
                <EditItemTemplate>
                    <asp:TextBox ID="Labor" runat="server" Text='<%# Bind("Labor") %>' Width="95%"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Laborlab" runat="server" Text='<%# Bind("Labor") %>' Width="15%" ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="Labor" runat="server" Text='<%# Bind("Labor") %>' Width="95%"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Материалы">
                <EditItemTemplate>
                    <asp:TextBox ID="Materials" runat="server" Text='<%# Bind("Materials") %>' Width="95%" ></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Materialslab" runat="server" Text='<%# Bind("Materials") %>' Width="15%" ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="Materials" runat="server" Text='<%# Bind("Materials") %>' Width="95%" ></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Механизмы">
                <EditItemTemplate>
                    <asp:TextBox ID="Mechanisms" runat="server" Text='<%# Bind("Mechanisms") %>' Width="95%"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Mechanismslab" runat="server" Text='<%# Bind("Mechanisms") %>' Width="15%" ></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="Mechanisms" runat="server" Text='<%# Bind("Mechanisms") %>' Width="95%"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>        
            <%--<asp:CommandField ShowEditButton="true" />--%>
                                                
            <asp:TemplateField HeaderText="" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                <EditItemTemplate> 
                    <asp:LinkButton ID="lbkUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Изменить"></asp:LinkButton> 
                    <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Отмена"></asp:LinkButton> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="False" CommandName="Insert" Text="Вставить"></asp:LinkButton> 
                </FooterTemplate> 
                <ItemTemplate> 
                    <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Править"></asp:LinkButton> 
                     <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Удалить"></asp:LinkButton> 
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:CommandField/>  
        </Columns>
                <EditRowStyle CssClass="GridViewEditRow" />
                <FooterStyle CssClass="GridViewEditRow" />
                
        </asp:GridView>
        </asp:Panel>
        <asp:Button ID="Finish" runat="server" OnClick="Finish_Click" Text="Отправить план" Width="429px" />
</asp:Content>
