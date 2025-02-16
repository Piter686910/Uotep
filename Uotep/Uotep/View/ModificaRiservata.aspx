<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModificaRiservata.aspx.cs" Inherits="Uotep.ModificaRiservata" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        function openPopup() {
            document.getElementById("popupModal").style.display = "block";
            document.getElementById("overlay").style.display = "block";
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
            <p class="text-center lead">Modifica Riservata</p>
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

                    <!-- Pulsante Login -->
                    <div class="form-group text-center mt-4" >
                        <asp:Button Text="Login" runat="server" OnClick="trova_Click" ToolTip="Login" CssClass="btn btn-primary px-4" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <!-- Contenitore per centrare -->
        <div id="DivRicerca" runat="server" class="d-flex flex-column justify-content-center align-items-center" style="height: 300px;">
            <!-- Righe di input con Bootstrap -->
            <div class="d-flex gap-3 w-50">
                <label for="txtProtoloccolRicerca">Nr Protocollo</label>
                <asp:TextBox ID="txtProtoloccolRicerca" runat="server" CssClass="form-control" placeholder="Nr Protocollo" />
                <label for="txtAnnoRicerca">Anno</label>
                <asp:TextBox ID="txtAnnoRicerca" runat="server" CssClass="form-control" placeholder="Anno" />
                <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Nuova Ricerca" CssClass="btn btn-primary mt-3" />

            </div>
        </div>


        <asp:Label ID="lblmessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
        <%-- campi nascosti --%>
        <asp:HiddenField ID="Holdmat" runat="server" />
        <asp:HiddenField ID="HolDate" runat="server" />
         <asp:HiddenField ID="HoldProtocollo" runat="server" />
        <%-- campi nascosti --%>
        <div class="container" id="DivDettagli" runat="server">
            <div id="DivGrid" runat="server" visible="false" class="row">
                <div class="form-group">
                    <!-- GridView nel popup -->
                    <asp:GridView ID="gvPopupProtocolli" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                        OnRowDataBound="gvPopupProtocolli_RowDataBound" OnRowCommand="gvPopupProtocolli_RowCommand">
                        <Columns>

                            <asp:BoundField DataField="Nr_Protocollo" HeaderText="Protocollo" />
                            <asp:BoundField DataField="Sigla" HeaderText="Sigla" />
                            <asp:BoundField DataField="Nominativo" HeaderText="Nominativo" />
                            <asp:BoundField DataField="Accertatori" HeaderText="Accertatori" />
                            <asp:BoundField DataField="Indirizzo" HeaderText="Indirizzo" />
                            <asp:BoundField DataField="Matricola" HeaderText="Matricola" />
                            <asp:BoundField DataField="DataInserimento" HeaderText="DataInserimento" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnSelect" runat="server" Text="Seleziona"
                                        CommandName="Select"
                                        CommandArgument='<%# Eval("Nr_Protocollo") + "|" + Eval("Matricola") + "|" + Eval("DataInserimento") + "|" + Eval("Sigla")%>'
                                        CssClass="btn btn-success btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>
            </div>

            <label for="txtProt">Nr Protocollo</label>
            <div class="row">
                <!-- Colonna Sinistra -->

                <div class="col-md-6">
                    <div class="form-group mb-3">

                        <asp:TextBox ID="txtProt" runat="server" CssClass="form-control1" ForeColor="Red" Font-Bold="true" />
                        <label for="Ddltipo">/</label>
                        <asp:DropDownList ID="DdlSigla" runat="server" CssClass="form-control1">
                            <asp:ListItem Text="ED"> </asp:ListItem>
                            <asp:ListItem Text="PG"> </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtDataArrivo">Data Arrivo</label>
                        <asp:TextBox ID="txtDataArrivo" runat="server" CssClass="form-control" Font-Bold="true" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="DdlGiudice">Giudice</label>
                        <asp:DropDownList ID="DdlGiudice" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="DdlTipoProvvAg">Tipo Provvedimento AG</label>
                        <asp:DropDownList ID="DdlTipoProvvAg" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtAccertatori">Accertatori</label>
                        <asp:TextBox ID="txtAccertatori" runat="server" CssClass="form-control mb-3" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="DdlQuartiere">Quartiere</label>
                        <asp:DropDownList ID="DdlQuartiere" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="DdlProvenienza">Provenienza</label>
                        <asp:DropDownList ID="DdlProvenienza" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rqProvenienza" runat="server" ControlToValidate="DdlProvenienza" ErrorMessage="inserire la provenienza">

                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group mb-3" style="margin-top: -20px!important">
                        <label for="txtScaturito">Scaturito</label>

                        <!-- DropDownList occupa metà spazio -->
                        <div class="form-group mb-3">
                            <asp:DropDownList ID="DdlScaturito" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>

                <!-- Colonna Destra -->
                <div class="col-md-6">
                    <div class="form-group mb-3" style="margin-top: -25px!important">
                        <label for="txtRifProtGen">Riferimento Prot. Gen.</label>
                        <asp:TextBox ID="txtRifProtGen" runat="server" CssClass="form-control" />

                    </div>

                    <!-- Indirizzo e TextBox sulla stessa riga -->
                    <div class="form-group mb-3">
                        <label for="DdlIndirizzo">Indirizzo</label>
                        <div class="row">
                            <!-- DropDownList occupa metà spazio -->
                            <div class="col-md-6">
                                <asp:DropDownList ID="DdlIndirizzo" runat="server" CssClass="form-control" />
                            </div>
                            <!-- TextBox occupa metà spazio -->
                            <div class="col-md-6">
                                <asp:TextBox ID="txtVia" runat="server" CssClass="form-control" placeholder="specifica l'indirizzo" />
                            </div>
                        </div>
                    </div>

                    <div class="form-group mb-3">
                        <label for="txtProdPenNr">Procedimento Penale nr</label>
                        <asp:TextBox ID="txtProdPenNr" runat="server" CssClass="form-control" />
                    </div>

                    <div class="form-group mb-3">
                        <label for="txtNominativo">Nominativo</label>
                        <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtDataCarico">Data Carico</label>
                        <asp:TextBox ID="txtDataCarico" runat="server" CssClass="form-control mb-3" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="txPratica">Pratica</label>
                        <asp:TextBox ID="txPratica" runat="server" CssClass="form-control" />
                    </div>

                    <div class="form-group mb-3">
                        <label for="DdlTipoAtto">Tipologia Atto</label>
                        <asp:DropDownList ID="DdlTipoAtto" runat="server" CssClass="form-control" />
                    </div>


                </div>

            </div>

            <div class="row align-items-center mb-3">
                <div class="col-md-3 d-flex align-items-center">
                    <div class="form-check">
                        <asp:CheckBox ID="CkEvasa" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkEvasa">Evasa</label>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtDataDataEvasa" class="form-label">In data</label>
                        <asp:TextBox ID="txtDataDataEvasa" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtinviata" class="form-label">Inviata</label>
                        <asp:TextBox ID="txtinviata" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtDataInvio" class="form-label">Il</label>
                        <asp:TextBox ID="txtDataInvio" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <div class="form-group mb-3">
                        <label for="txtNote">Eventuali Note</label>
                        <asp:TextBox ID="txtNote" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 text-center">
                    <asp:Button Text="Nuova Ricerca" runat="server" OnClick="NuovaRicerca_Click" ToolTip="Nuova Ricerca" CssClass="btn btn-primary mt-3" />
                    <asp:Button Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary mt-3" />
                    <asp:Button Text="Cerca Quartiere" runat="server" OnClick="apripopup_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" />

                </div>
            </div>
        </div>
    </div>
    <!-- Modale Bootstrap -->

    <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">Popup di Ricerca</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">
                        <label for="txtIndirizzo">Indirizzo:</label>
                        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" placeholder="Campo obbligatorio" />

                    </div>
                    <%--<div class="form-group">
                                <label for="txtSpecie">Toponimo:</label>
                                <asp:TextBox ID="txtSpecie" runat="server" CssClass="form-control" />
                            </div>--%>
                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="gvPopup" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopup_RowDataBound" OnRowCommand="gvPopup_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="Toponimo" HeaderText="Toponimo" />
                                <asp:BoundField DataField="Quartiere" HeaderText="Quartiere" />
                                <asp:BoundField DataField="Specie" HeaderText="Specie" />
                                <asp:BoundField DataField="Nota" HeaderText="Nota" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("Quartiere") %>' CssClass="btn btn-success btn-sm" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="RicercaQuartiere_Click" />
                    <asp:Button ID="btnchiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                </div>
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
    <%-- <div id="popupModal" class="modal" style="display: none; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 4px 8px rgba(0,0,0,0.2); z-index: 1000; width: 400px;">
        <h4>Inserisci Indirizzo</h4>
        <asp:TextBox ID="txtSpecie" runat="server" CssClass="form-control" placeholder="specie" />
        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" placeholder="Inserisci indirizzo" />
        <asp:Button ID="btnCercaQuartiere" runat="server" Text="Cerca Quartiere" CssClass="btn btn-success mt-3" OnClick="RicercaQuartiere_Click" />
        <asp:Label ID="lblQuartiere" runat="server" CssClass="mt-3 d-block" />
        <button onclick="closePopup()" class="btn btn-secondary mt-3">Chiudi</button>
    </div>
    <div id="overlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0,0,0,0.5); z-index: 999;"></div>--%>
</asp:Content>
