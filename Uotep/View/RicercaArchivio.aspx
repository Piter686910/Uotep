<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RicercaArchivio.aspx.cs" Inherits="Uotep.RicercaArchivio" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }

        // Mostra il popup
        function showModal() {
            $('#myModal').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#myModal').modal('hide');
        }
       

    </script>
    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">RICERCA UNA PRATICA</p>
            <!-- Contenitore per centrare -->

            <asp:Panel ID="pnlButton" runat="server" CssClass="text-center" Visible="true">
                <div class="d-flex justify-content-center mt-4">

                    <p>
                        <!-- Pulsanti -->
                        <asp:Button ID="btNominativo" runat="server" OnClick="btNominativo_Click" Text="Nominativo" ToolTip="Ricerca Nominativo" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btIndirizzo" runat="server" OnClick="btIndirizzo_Click" Text="Indirizzo" ToolTip="Ricerca Per Indirizzo" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btDatiCatastali" runat="server" OnClick="btDatiCatastali_Click" Text="Dati Catastali" ToolTip="Dati Catastali" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btNpratica" runat="server" OnClick="btNpratica_Click" Text="Nr. Pratica" ToolTip="Ricerca Pratica" CssClass="btn btn-primary mx-2" />

                    </p>

                </div>

            </asp:Panel>
        </div>
        
        <%--Sezione di ricerca  --%>
        <div id="DivRicerca" runat="server" class="row d-flex justify-content-center align-items-center vh-100" style="height: 300px; margin-left: 400px!important">
            <!-- Righe di input  -->
            <div class="col-md-4 custom-border">

                <%-- DIV RICERCA pratica --%>
                <div id="DivPratica" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label6" runat="server" Text="Nr. Pratica" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" placeholder="Nr. Pratica" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Indirizzo --%>
                <div id="DivIndirizzo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label11" runat="server" Text="Indirizzo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" placeholder="Indirizzo" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA nominativo --%>
                <div id="DivNominativo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label9" runat="server" Text="Nominativo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" placeholder="Nominativo" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Dati Catastali --%>
                <div id="DivDatiCatastali" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label7" runat="server" Text="Sezione" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtSez" runat="server" CssClass="form-control" placeholder="Sezione" />
                    <asp:Label ID="Label1" runat="server" Text="Foglio" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtFoglio" runat="server" CssClass="form-control" placeholder="Foglio" />
                    <asp:Label ID="Label2" runat="server" Text="P.lla" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtParticella" runat="server" CssClass="form-control" placeholder="Particella" />
                    <asp:Label ID="Label3" runat="server" Text="Sub" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtSub" runat="server" CssClass="form-control" placeholder="Sub" />



                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
            </div>
        </div>



    </div>



    <%-- popup errori --%>
    <div class="modal fade" id="errorModal" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <%--role="document">--%>
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">ATTENZIONE</h5>

                </div>
                <div class="modal-body">

                    <div class="form-group">

                        <p id="errorMessage" style="color: red"></p>

                    </div>

                </div>
                <div class="modal-footer">

                    <asp:Button ID="Button2" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopupErrore_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
