<%@ Page Title="Вход в систему" Language="C#" EnableEventValidation="false" MasterPageFile="~/LoginRegister.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PlanViewer.Login" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>
<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Выберите вариант входа в систему</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
    <link rel="stylesheet" type="text/css" href="style.css" />
    <link rel="stylesheet" type="text/css" href="css/buttons.css">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js"></script>
    <script type="text/javascript">
        (function ($) {
            $.fn.extend({
                leanModal: function (options) {
                    var defaults = { top: 100, overlay: 0.5, closeButton: null }; var overlay = $("<div id='lean_overlay'></div>"); $("body").append(overlay); options = $.extend(defaults, options); return this.each(function () {
                        var o = options; $(this).click(function (e) {
                            var modal_id = $(this).attr("href"); $("#lean_overlay").click(function () { close_modal(modal_id) }); $(o.closeButton).click(function () { close_modal(modal_id) }); var modal_height = $(modal_id).outerHeight(); var modal_width = $(modal_id).outerWidth();
                            $("#lean_overlay").css({ "display": "block", opacity: 0 }); $("#lean_overlay").fadeTo(200, o.overlay); $(modal_id).css({ "display": "block", "position": "fixed", "opacity": 0, "z-index": 11000, "left": 50 + "%", "margin-left": -(modal_width / 2) + "px", "top": o.top + "px" }); $(modal_id).fadeTo(200, 1); e.preventDefault()
                        })
                    }); function close_modal(modal_id) { $("#lean_overlay").fadeOut(200); $(modal_id).css({ "display": "none" }) }
                }
            })
        })(jQuery);
    </script>
    <script type="text/javascript">
        $(function () {
            $('a[rel*=leanModal]').leanModal({ top: 200, overlay: 0.4, closeButton: ".modal_close" });
        });
    </script>
    <div style="float: left">
        <asp:Login ID="Customer" runat="server"
            RememberMeSet="false"
            DisplayRememberMe="false"
            LoginButtonText="Войти как заказчик"
            UserNameLabelText="email"
            FailureText="Неверный логин или пароль"
            TitleText="Вход для заказчика"
            DestinationPageUrl="~/Default.aspx" OnAuthenticate="Customer_Authenticate">
        </asp:Login>
        <div>
            <a class="green goodbutton" id="A1" rel="leanModal" href="#customerLostPass" >Восстановить пароль</a>
            <br />
        </div>
    </div>
    <div style="float: right">
        <asp:Login ID="Contractor" runat="server"
            RememberMeSet="false"
            DisplayRememberMe="false"
            LoginButtonText="Войти как подрядчик"
            UserNameLabelText="email"
            FailureText="Неверный логин или пароль"
            TitleText="Вход для подрядчика"
            DestinationPageUrl="~/Default.aspx" OnAuthenticate="Contractor_Authenticate">
        </asp:Login>
        <a class="right" style="text-align: right; float: right" id="A2" rel="leanModal" href="#contractorLostPass" >Восстановить пароль</a>
        <br />
    </div>
    <br>
    <div style="margin-left: auto; margin-right: auto; text-align: center; ">
        <a class="green goodbutton" id="customerRegister" rel="leanModal" href="#signupCustomer">Зарегистрировать заказчика</a>
        <br>
        <a class="green goodbutton" id="contractorRegister" rel="leanModal" href="#signupContractor">Зарегистрировать подрядчика</a>
        <div id="signupCustomer" class="signup" style="display: none; position: fixed; opacity: 1; z-index: 11000; left: 50%; margin-left: -202px; top: 50px;">
            <div class="signup-ct">
                <div class="signup-header">
                    <h2>Регистрация заказчика</h2>
                </div>

                <asp:Panel ID="Panel1" runat="server">

                    <div class="txt-fld">
                        <label for="">ФИО</label>
                        <asp:TextBox runat="server" ID="CustomerName" TextMode="SingleLine" />

                    </div>
                    <div class="txt-fld">
                        <label for="">email</label>
                        <asp:TextBox runat="server" ID="CustomerEmail" TextMode="Email" />
                    </div>
                    <div class="txt-fld">
                        <label for="">Пароль</label>
                        <asp:TextBox runat="server" ID="CustomerPassword" name="" TextMode="Password" />
                    </div>

                    <div class="txt-fld">
                        <label for="">Пароль еще раз</label>
                        <asp:TextBox runat="server" ID="CustomerPassword2" name="" TextMode="Password" />
                    </div>

                    <div class="btn-fld">
                        <asp:Button runat="server" ID="buttonCustomer" type="submit" OnClick="registerCustomer" Text="Зарегистрироваться"></asp:Button>
                    </div>
                </asp:Panel>
            </div>
        </div>

        <div id="signupContractor" class="signup" style="display: none; position: fixed; opacity: 1; z-index: 11000; left: 50%; margin-left: -202px; top: 50px;">
            <div class="signup-ct">
                <div class="signup-header">
                    <h2>Регистрация подрядчика</h2>
                </div>

                <!-- <form method="post" > -->
                <asp:Panel ID="Panel2" runat="server">

                    <div class="txt-fld">
                        <label for="">ФИО</label>
                        <asp:TextBox runat="server" ID="ContracorName" TextMode="SingleLine" />

                    </div>
                    <div class="txt-fld">
                        <label for="">email</label>
                        <asp:TextBox runat="server" ID="ContracorEmail" TextMode="Email" />
                    </div>
                    <div class="txt-fld">
                        <label for="">Пароль</label>
                        <asp:TextBox runat="server" ID="ContracorPassword" TextMode="Password" onchange="validate" />
                    </div>

                    <div class="txt-fld">
                        <label for="">Пароль еще раз</label>
                        <asp:TextBox runat="server" ID="ContracorPassword2" TextMode="Password" />
                    </div>
                    <script>
                        function validate(control) {
                            alert("smth happened");
                        }
                    </script>
                    <div class="btn-fld">
                        <asp:Button runat="server" ID="buttonContracor" OnClick="registerContractor" Text="Зарегистрироваться"></asp:Button>

                    </div>
                </asp:Panel>
            </div>
        </div>
        <div id="customerLostPass" class="signup" style="display: none; position: fixed; opacity: 1; z-index: 11000; left: 50%; margin-left: -202px; top: 200px;">
            <div class="signup-ct">
                <div class="signup-header">
                    <h2>Восстановление пароля</h2>
                </div>

                <asp:Panel ID="Panel3" runat="server">

                    <div class="txt-fld">
                        <label for="">Введите ваш email</label>
                        <br />
                        <asp:TextBox runat="server" ID="femailcs" TextMode="Email" />
                    </div>                    

                    <div class="btn-fld">
                        <asp:Button runat="server" ID="custRPass" type="submit" OnClick="custRPass_Click" Text="Отправить пароль"></asp:Button>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div id="contractorLostPass" class="signup" style="display: none; position: fixed; opacity: 1; z-index: 11000; left: 50%; margin-left: -202px; top: 200px;">
            <div class="signup-ct">
                <div class="signup-header">
                    <h2>Восстановление пароля</h2>
                </div>

                <asp:Panel ID="Panel4" runat="server">

                    <div class="txt-fld">
                        <label for="">Введите ваш email</label>
                        <br />
                        <asp:TextBox runat="server" ID="femailco" TextMode="Email" />
                    </div>                    

                    <div class="btn-fld">
                        <asp:Button runat="server" ID="contRPass" type="submit" OnClick="contRPass_Click" Text="Отправить пароль"></asp:Button>
                    </div>
                </asp:Panel>
            </div>
        </div>

    </div>


</asp:Content>
