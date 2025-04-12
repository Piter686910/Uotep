<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatistichePg.aspx.cs" Inherits="Uotep.StatistichePg" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }

    </script>

    <div class="panel panel-default">
        <div class="form-group mb-3"></div>

        <div class="panel-body" id="divStat" runat="server">
            <div class="jumbotron">
                <div style="margin-top: -90px!important">
                    <%--<asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>--%>
                    <p class="text-center lead">INSERISCI STATISTICHE</p>
                </div>

                <div class="container">
                    <div class="row align-items-end">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtMM">Mese</label>
                                <asp:TextBox ID="txtMM" runat="server" CssClass="form-control" Width="110px" />
                                <asp:RequiredFieldValidator ID="rqMM" runat="server" ControlToValidate="txtMM" ErrorMessage="inserire il mese" ValidationGroup="bt" ForeColor="Red">
                                </asp:RequiredFieldValidator>

                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtYYYY">Anno</label>
                                <asp:TextBox ID="txtYYYY" runat="server" CssClass="form-control" Width="110px" />
                            </div>
                            <asp:RequiredFieldValidator ID="rqAnno" runat="server" ControlToValidate="txtYYYY" ErrorMessage="inserire l'anno" ValidationGroup="bt" ForeColor="Red">
                            </asp:RequiredFieldValidator>

                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtInterrogatorio">Interrogatori</label>
                                <asp:TextBox ID="txtInterrogatorio" runat="server" CssClass="form-control" Width="50px" />
                                <asp:RequiredFieldValidator ID="rqInterrogatorio" runat="server" ControlToValidate="txtInterrogatorio" ErrorMessage="inserire un numero" ValidationGroup="bt" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Bottone Salva -->
                <div class="col-md-4 ">
                    <div class="form-group">

                        <asp:Button ID="btSalva" runat="server" ValidationGroup="bt" Text="Inserisci" CssClass="btn btn-primary" OnClick="btInserisci_Click" />
                    </div>
                </div>
            </div>
        </div>

    </div>



    <%-- il seguente style serve per i bordi azzurri --%>
    <style>
        .custom-border {
            border: 2px solid #007bff; /* Cornice blu */
            border-radius: 8px; /* Angoli arrotondati */
            padding: 15px; /* Spazio interno */
            margin: 5px 0; /* Spazio esterno */
            margin-left: -10px;
        }

        .larghezzaText {
            width: 110px;
        }
    </style>


</asp:Content>
