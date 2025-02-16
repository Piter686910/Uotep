<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Uotep._Default" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        
    </script>
    <div class="jumbotron">
        <h1>ARCHIVIO PRATICHE U.O.T.E.P</h1>
        <p class="lead"></p>
    </div>
    <%-- LOGIN --%>
    <asp:Panel ID="pnlLogin" runat="server" CssClass="text-center">
        <div class="row d-flex justify-content-center align-items-center vh-100">
            <div class="col-md-4">
                <!-- Etichetta Matricola e Campo -->
                <div class="form-group text-center" style="text-align: left !important">
                    <asp:Label ID="lblm" runat="server" Text="Matricola" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:TextBox ID="TxtMatricola" runat="server" ToolTip="matricola" TabIndex="1" CssClass="form-control"></asp:TextBox>
                    <asp:HiddenField ID="Hmatricola" runat="server" />
                </div>

                <!-- Etichetta Password e Campo -->
                <div class="form-group text-center mt-4" style="text-align: left !important">
                    <asp:Label ID="Label1" runat="server" Text="Password" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:TextBox ID="TxtPassw" runat="server" ToolTip="password" MaxLength="8" TextMode="Password" TabIndex="2" CssClass="form-control"></asp:TextBox>
                </div>
                <!-- Etichetta nuova password -->
                <div id="DivNewPassw" runat="server" class="form-group text-center mt-4" visible="false" style="text-align: left !important">
                    <asp:Label ID="Label2" runat="server" Text="Nuova Password" CssClass="form-label d-block mb-2" ForeColor="red"></asp:Label>
                    <asp:TextBox ID="txtNewPassw" runat="server" ToolTip="password" MaxLength="8" TextMode="Password" TabIndex="2" CssClass="form-control"></asp:TextBox>
                
                </div>
                <!-- Pulsante Login -->
                <div class="form-group text-center mt-4">
                    <asp:Button ID="btLogin" Text="Login" runat="server" OnClick="trova_Click" ToolTip="Login" CssClass="btn btn-primary px-4" OnLoginError="Login1_LoginError" />
                    <asp:Button ID="btsave" Text="Save" runat="server" OnClick="SalvaPassw_Click" ToolTip="Salva password" CssClass="btn btn-primary px-4" Visible="false"  />

                </div>
            </div>
        </div>
    </asp:Panel>
  
    <%-- popup errori --%>
    <div class="modal fade" id="errorModal" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-hidden="true">
        <div class="modal-dialog"
            role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">ERRORE</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">

                        <p id="errorMessage" style="color:red"></p>

                    </div>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="Button2" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
