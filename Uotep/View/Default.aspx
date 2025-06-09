<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Uotep._Default" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        // Nasconde il popup
        function HideErrorMessage() {
            $('#errorModal').modal('hide');
        }
    </script>
    <style>
        .centered-panel {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 100%;
            max-width: 400px; /* Limita la larghezza del form */
        }
    </style>
    <%-- <div class="jumbotron">
        <h1>ARCHIVIO PRATICHE U.O.T.E.P</h1>

        <p class="lead"></p>
    </div>--%>
    <%-- LOGIN --%>
    <asp:Panel ID="pnlLogin" runat="server" CssClass="centered-panel">
        <div class="card shadow p-4">
            <div cssclass="form-label text-start d-block mb-2">
                <h2>Accesso</h2>
            </div>

            <!-- Etichetta Matricola e Campo -->
            <div class="form-group">
                <asp:Label ID="lblm" runat="server" Text="Matricola" CssClass="form-label text-start d-block mb-2"></asp:Label>
                <asp:TextBox ID="TxtMatricola" runat="server" ToolTip="matricola" TabIndex="1" CssClass="form-control"></asp:TextBox>
                <asp:HiddenField ID="Hmatricola" runat="server" />
            </div>

            <!-- Etichetta Password e Campo -->
            <div class="form-group mt-3">
                <asp:Label ID="Label1" runat="server" Text="Password" CssClass="form-label text-start d-block mb-2"></asp:Label>
                <asp:TextBox ID="TxtPassw" runat="server" ToolTip="password" MaxLength="8" TextMode="Password" TabIndex="2" CssClass="form-control"></asp:TextBox>
            </div>

            <!-- Etichetta nuova password -->
            <div id="DivNewPassw" runat="server" class="form-group mt-3" visible="false">
                <asp:Label ID="Label2" runat="server" Text="Nuova Password" CssClass="form-label text-start d-block mb-2 text-danger"></asp:Label>
                <asp:TextBox ID="txtNewPassw" runat="server" ToolTip="password" MaxLength="8" TextMode="Password" TabIndex="2" CssClass="form-control"></asp:TextBox>
            </div>

            <!-- Pulsanti -->
            <div class="form-group text-center mt-4">
                <asp:Button ID="btLogin" Text="Entra" runat="server" OnClick="trova_Click" ToolTip="Login" CssClass="btn btn-primary px-4" OnLoginError="Login1_LoginError" />
                <asp:Button ID="btsave" Text="Save" runat="server" OnClick="SalvaPassw_Click" ToolTip="Salva password" CssClass="btn btn-primary px-4" Visible="false" />
                <%--<asp:Button ID="btResetPassw" Text="Reset Password" runat="server" OnClick="btResetPassw_Click" ToolTip="Reset Password" CssClass="btn btn-primary px-4" OnLoginError="Login1_LoginError" />--%>
                <asp:LinkButton id="lkreset" OnClick="lkreset_Click" Text="Reset Password" runat="server"> </asp:LinkButton>
            </div>
        </div>
    </asp:Panel>




    <%-- popup errori --%>
    <div class="modal fade" id="errorModal" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-hidden="true">
        <div class="modal-dialog"
            role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">ATTENZIONE</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">

                        <p id="errorMessage" style="color: red"></p>

                    </div>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btChiudiPop" runat="server" class="btn btn-secondary" Text="Chiudi"  OnClick="btChiudiPop_Click"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
