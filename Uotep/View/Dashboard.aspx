<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Uotep._Dashboard" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }

    </script>
    <div class="jumbotron">
        <h1>DASHBOARD AMMINISTRATORE</h1>
        <p class="lead"></p>
    </div>
    <%-- LOGIN --%>
    <asp:Panel ID="pnlGestUtenti" runat="server" CssClass="text-center">
        <div class="row d-flex justify-content-center align-items-center vh-100">
            <div class="container">
                <div class="row">
                    <div id="divNewUtente" runat="server" class="col-md-6" visible="false">
                        <div class="form-group mb-3">
                            <!-- Etichetta Matricola e Campo -->
                            <div class="form-group text-center" style="text-align: left !important">
                                <asp:Label ID="lblm" runat="server" Text="Matricola" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:TextBox ID="TxtMatricola" runat="server" ToolTip="matricola" TabIndex="1" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <!-- Etichetta Profilo e Campo -->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label1" runat="server" Text="Profilo" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:TextBox ID="TxtProfilo" runat="server" ToolTip="Profilo" TabIndex="2" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Etichetta area -->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label2" runat="server" Text="Area" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:TextBox ID="txtArea" runat="server" ToolTip="area appartenenza" TabIndex="3" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <!-- Etichetta macroarea -->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label4" runat="server" Text="Macro Area" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:TextBox ID="txtMacroArea" runat="server" ToolTip="Macro Area" TabIndex="4" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <!-- Etichetta nominativo -->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label7" runat="server" Text="Nominativo" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:TextBox ID="txtNominativo" runat="server" ToolTip="nominativo" TabIndex="5" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <!-- Colonna Destra -->
                    <div id="divDestra" runat="server" visible="false" class="col-md-6">
                        <!-- Etichetta nota -->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label5" runat="server" Text="Nota" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:TextBox ID="TxtNota" runat="server" ToolTip="Nota" TextMode="MultiLine" Rows="4" TabIndex="6" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <!-- Etichetta ruolo -->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label6" runat="server" Text="Ruolo" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:DropDownList ID="DdlRuolo" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="admin"> </asp:ListItem>
                                    <asp:ListItem Text="accertatori"> </asp:ListItem>
                                    <asp:ListItem Text="archivio"> </asp:ListItem>
                                    <asp:ListItem Text="coordinamento ag"> </asp:ListItem>
                                    <asp:ListItem Text="coordinamento pg"> </asp:ListItem>
                                    <asp:ListItem Text="MasterAG"> </asp:ListItem>
                                    <asp:ListItem Text="segreteria"> </asp:ListItem>
                                    <asp:ListItem Text="superAdmin"> </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-6" style="margin-top: 20px!important">
                            <div class="form-group mb-3">
                                <asp:Button Text="OK" runat="server" OnClick="InsOpetratore_Click" ToolTip="Inserisci" CssClass="btn btn-primary px-4" OnLoginError="Login1_LoginError" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divReset" runat="server" class="col-md-4" visible="false">
                <!-- Etichetta Matricola e Campo -->
                <div class="col-md-6">
                    <div class="form-group mb-3" style="text-align: left !important">
                        <asp:Label ID="Label3" runat="server" Text="Matricola" CssClass="form-label d-block mb-2"></asp:Label>
                        <asp:TextBox ID="txtResetMatricola" runat="server" ToolTip="matricola" TabIndex="6" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6" style="margin-top: 20px!important">
                    <div class="form-group mb-3">
                        <asp:Button Text="OK" runat="server" OnClick="ModificaP_Click" ToolTip="Reset" CssClass="btn btn-primary px-4" OnLoginError="Login1_LoginError" />
                    </div>
                </div>
            </div>
            <!-- Pulsante Login -->
            <div class="form-group text-center mt-4">
                <asp:Button Text="Reset Password" runat="server" OnClick="Reset_Click" ToolTip="Reset Password" CssClass="btn btn-primary px-4" OnLoginError="Login1_LoginError" />
                <asp:Button Text="Nuovo Utente" runat="server" OnClick="NuovoUt_Click" ToolTip="Nuovo Utente" CssClass="btn btn-primary px-4" OnLoginError="Login1_LoginError" />
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
                    <asp:Button ID="Button2" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
