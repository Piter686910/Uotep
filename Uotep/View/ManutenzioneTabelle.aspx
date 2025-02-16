<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManutenzioneTabelle.aspx.cs" Inherits="Uotep.ManutenzioneTabelle" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }

    </script>
    <div class="jumbotron">
        <h1>CRUSCOTTO GESTIONE TABELLE</h1>
        <p class="lead">In questa pagina è possibile inserire nuovi elementi nelle seguenti tabelle</p>
    </div>
    <%-- --%>

    <asp:Panel ID="pnlButton" runat="server" CssClass="text-center" Visible="true">
        <div class="d-flex justify-content-center mt-4">

            <p>
                <!-- Pulsanti -->
                <asp:Button ID="btGiudice" runat="server" OnClick="btGiudice_Click" Text="Tabella dei Giudici" CssClass="btn btn-primary mx-2" ToolTip="Inserisci nuovo giudice" />
                <asp:Button ID="btScaturito" runat="server" OnClick="btScaturito_Click" Text="Tabella Scaturito" ToolTip="Inserisci nuovo elemento" CssClass="btn btn-primary mx-2" />
                <asp:Button ID="btProvenienza" runat="server" OnClick="btProvenienza_Click" Text=" Tabella Provenienza" ToolTip="Inserisci nuovo elemento" CssClass="btn btn-primary mx-2" />
                <asp:Button ID="btTipologia" runat="server" OnClick="btTipologia_Click" Text="Tabella Tipologia" ToolTip="Inserisci nuovo elemento" CssClass="btn btn-primary mx-2" />

                <asp:Button ID="btTipologiaNotaAg" runat="server" OnClick="btTipologiaNotaAg_Click" Text="Tabella Tipologia Ag" ToolTip="Inserisci nuovo elemento" CssClass="btn btn-primary mx-2" />
                <asp:Button ID="btInviati" runat="server" OnClick="btInviati_Click" Text="Tabella Inviata" ToolTip="Inserisci nuovo elemento" CssClass="btn btn-primary mx-2" />

            </p>
        </div>

    </asp:Panel>


    <asp:Panel ID="pnlGestTabelle" runat="server" CssClass="text-center">
        <div id="DivTabelle" runat="server" class="row d-flex justify-content-center align-items-center vh-100" style="height: 300px; margin-left: 400px!important">
            <!-- Righe di input  -->
            <div class="col-md-4 custom-border">
                <%-- DIV Giudice --%>
                <div id="DivGiudice" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label8" runat="server" Text="Inserisci nuovo elemento" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtGiudice" runat="server" CssClass="form-control" placeholder="Giudice" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button ID="btInserisci" Text="Inserisci" runat="server" OnClick="btInserisci_Click" ToolTip="Inserisci" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                   <%-- <div id="DivGrid" runat="server" visible="false" class="row">
                        <div class="form-group">
                            <!-- GridView nel popup -->
                            <asp:GridView ID="gvPopupGiudice" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                                OnRowDataBound="gvPopupGiudice_RowDataBound" OnRowCommand="gvPopupGiudice_RowCommand">
                                <Columns>

                                    <asp:BoundField DataField="Giudice" HeaderText="Giudice" />

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" Text="Elimina"
                                                CommandName="Select"
                                                CommandArgument='<%# Eval("Giudice") + "|" + Eval("Id_giudice") %>'
                                                CssClass="btn btn-success btn-sm" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>--%>
                </div>
                <%-- DIV Scaturito --%>
                <div id="DivScaturito" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label10" runat="server" Text="Inserisci nuovo elemento" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtScaturito" runat="server" CssClass="form-control" placeholder="Scaturito" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Inserisci" OnClick="btInserisci_Click" runat="server" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>

                </div>

                <%-- DIV Provenienza --%>
                <div id="DivProvenienza" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label13" runat="server" Text="Inserisci nuovo elemento" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProvenienza" runat="server" CssClass="form-control" placeholder="Provenienza" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Inserisci" OnClick="btInserisci_Click" runat="server" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>

                </div>
                <%-- DIV  Tipologia --%>
                <div id="DivTipologia" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label14" runat="server" Text="Inserisci nuovo elemento" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtTipologia" runat="server" CssClass="form-control" placeholder="Tipologia" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Inserisci" OnClick="btInserisci_Click" runat="server" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>

                <%-- DIV  Inviati --%>
                <div id="DivInviati" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label18" runat="server" Text="Inserisci nuovo elemento" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtInviati" runat="server" CssClass="form-control" placeholder="Inviata" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Inserisci" OnClick="btInserisci_Click" runat="server" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>

                <%-- DIV Tipologia nota ag --%>
                <div id="DivTipologiaNotaAg" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label20" runat="server" Text="Inserisci nuovo elemento" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtTipologiaNotaAg" runat="server" CssClass="form-control" placeholder="Tipologia Nota Ag" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Inserisci" OnClick="btInserisci_Click" runat="server" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
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
