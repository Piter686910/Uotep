<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatisticheAtti.aspx.cs" Inherits="Uotep.StatisticheAtti" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        function CloseErrorMessage(message) {
            $('#errorModal').modal('hide');
        }
    </script>



    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <%--<asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>--%>
            <%--<p class="text-center lead">INSERISCI STATISTICHE</p>--%>
            <div class="dashboard-header">
                <h1><span class="fa-solid fa-gear fa-spin"></span> INSERISCI STATISTICHE</h1>
            </div>
        </div>

        <div class="container">
            <div class="row align-items-end">
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtMM">Mese</label>
                        <asp:TextBox ID="txtMM" runat="server" CssClass="form-control" Width="110px" autofocus="" />
                        <asp:RequiredFieldValidator ID="rqMM" runat="server" ControlToValidate="txtMM" ErrorMessage="inserire il mese" ValidationGroup="bt" ForeColor="Red">
                        </asp:RequiredFieldValidator>

                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtYYYY">Anno</label>
                        <asp:TextBox ID="txtYYYY" runat="server" CssClass="form-control" Width="110px" />
                    </div>
                    <asp:RequiredFieldValidator ID="rqAnno" runat="server" ControlToValidate="txtYYYY" ErrorMessage="inserire l'anno" ValidationGroup="bt" ForeColor="Red">
                    </asp:RequiredFieldValidator>

                </div>

                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtEspostiEvasi">Esposti Ricevuti</label>
                        <asp:TextBox ID="txtEspostiRicevuti" runat="server" CssClass="form-control" Width="50px" Enabled="false" />

                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtDenunceUff">Denunce Uff.</label>
                        <asp:TextBox ID="txtDenunceUff" runat="server" CssClass="form-control" Width="50px" />

                    </div>
                </div>

            </div>
        </div>

        <!-- Bottone Salva -->
        <div class="col-md-4 ">
            <div class="form-group">

                <asp:Button ID="btSalva" runat="server" Text="Inserisci" CssClass="btn btn-primary" OnClick="btInserisci_Click" />
            </div>
        </div>
    </div>




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
                    <asp:Button ID="Button2" runat="server" class="btn btn-secondary" Text="Chiudi" OnClientClick="CloseErrorMessage()" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
