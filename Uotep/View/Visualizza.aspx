<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Visualizza.aspx.cs" Inherits="Uotep.Visualizza" %>


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
        // Mostra il popup ricerca
        function showModal() {
            $('#ModalRicerca').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalRicerca').modal('hide');
        }
    </script>
    <style>
        .custom-border {
            border: 2px solid #007bff; /* Cornice blu */
            border-radius: 8px; /* Angoli arrotondati */
            padding: 15px; /* Spazio interno */
            margin: 5px 0; /* Spazio esterno */
            margin-left: -30px;
        }

        .uppercase-text {
            text-transform: uppercase;
        }
    </style>
    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">Ricerca Atti</p>

            <!-- Contenitore per centrare -->

            <asp:Panel ID="pnlButton" runat="server" CssClass="text-center" Visible="true">
                <div class="d-flex justify-content-center mt-4">

                    <p>
                        <!-- Pulsanti -->
                        <asp:Button ID="btNProtocollo" runat="server" OnClick="btNProtocollo_Click" Text="Nr. Carico" CssClass="btn btn-primary mx-2" ToolTip="Ricerca per numero carico" />
                        <asp:Button ID="btProcPenale" runat="server" OnClick="btProcPenale_Click" Text="Proc. Penale" ToolTip="Ricerca Procedimento Penale" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btProtGen" runat="server" OnClick="btProtGen_Click" Text="Rif. Prot. Gen." ToolTip="Ricerca Protocollo Generale" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btEvaseAg" runat="server" OnClick="btEvaseAg_Click" Text="Evase Ag." ToolTip="Ricerca Evase AG" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btNpratica" runat="server" OnClick="btNpratica_Click" Text="Nr. Pratica" ToolTip="Ricerca Pratica" CssClass="btn btn-primary mx-2" />

                    </p>
                    <p>
                        <!-- Pulsanti -->
                        <asp:Button ID="btGiudice" runat="server" OnClick="btGiudice_Click" Text="Giudice" ToolTip="Ricerca Giudice" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btProvenienza" runat="server" OnClick="btProvenienza_Click" Text="Provenienza" ToolTip="Ricerca Per ProvenienzaG" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btNominativo" runat="server" OnClick="btNominativo_Click" Text="Nominativo" ToolTip="Ricerca Nominativo" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btDataCarico" runat="server" OnClick="btDataCarico_Click" Text="Data Arrivo" ToolTip="Ricerca Data Arrivo" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btAccertatori" runat="server" OnClick="btAccertatori_Click" Text="Accertatori" ToolTip="Ricerca Accertatori" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btIndirizzo" runat="server" OnClick="btIndirizzo_Click" Text="Indirizzo" ToolTip="Ricerca Per Indirizzo" CssClass="btn btn-primary mx-2" />
                    </p>
                </div>

            </asp:Panel>

        </div>

        <div id="DivRicerca" runat="server" visible="false" class="row d-flex justify-content-center align-items-center vh-100" style="height: 300px; margin-left: 400px!important">
            <!-- Righe di input  -->
            <div class="col-md-4 custom-border">
                <%-- DIV RICERCA PROTOCOLLO --%>
                <div id="DivProtocollo" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="lblm" runat="server" Text="Nr Protocollo" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNProtocollo" ErrorMessage="Inserire numero pratica" ForeColor="Red" ValidationGroup="bt">

                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtNProtocollo" runat="server" CssClass="form-control" placeholder="Nr Protocollo" />


                    <asp:Label ID="Label1" runat="server" Text="Anno" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:RequiredFieldValidator ID="rqanno" runat="server" ControlToValidate="txtAnnoRicerca" ErrorMessage="Inserire l'anno per la ricerca" ForeColor="Red" ValidationGroup="bt">

                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtAnnoRicerca" runat="server" CssClass="form-control" placeholder="Anno" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA PROocedimento penale --%>
                <div id="DivProcPenale" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label2" runat="server" Text="Nr Procediemnto Penale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProcPenale" runat="server" CssClass="form-control" placeholder="Nr Procedimento Penale" />



                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA evasa ag --%>
                <div id="DivEvasaAg" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label3" runat="server" Text="Data Inizio Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataDa" runat="server" CssClass="form-control" placeholder="Data Inizio" />

                    <asp:Label ID="Label4" runat="server" Text="Data Fine Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataA" runat="server" CssClass="form-control" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA rif protocollo generale --%>
                <div id="DivProtGen" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label5" runat="server" Text="Rif. Protocollo Generale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProtGen" runat="server" CssClass="form-control" placeholder="Rif. Prot. Gen." />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA pratica --%>
                <div id="DivPratica" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label6" runat="server" Text="Nr. Pratica" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" placeholder="Nr. Pratica" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA giudice --%>
                <div id="DivGiudice" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label7" runat="server" Text="Giudice" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicGiudice" runat="server" CssClass="form-control" placeholder="Giudice" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA provenienza --%>
                <div id="DivProvenienza" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label8" runat="server" Text="Provenienza" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicProvenienza" runat="server" CssClass="form-control" placeholder="Provenienza" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA nominativo --%>
                <div id="DivNominativo" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label9" runat="server" Text="Nominativo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicNominativo" runat="server" CssClass="form-control" placeholder="Nominativo" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Accertatori --%>
                <div id="DivAccertatori" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label10" runat="server" Text="Accertatori" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicAccertatori" runat="server" CssClass="form-control" placeholder="Accertatori" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Indirizzo --%>
                <div id="DivIndirizzo" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label11" runat="server" Text="Indirizzo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicIndirizzo" runat="server" CssClass="form-control" placeholder="Indirizzo" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA data carico --%>
                <div id="DivDataArrivo" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label12" runat="server" Text="Data Arrivo Da" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatArrivoDa" runat="server" CssClass="form-control" placeholder="Data Inizio" />

                    <asp:Label ID="Label13" runat="server" Text="Data Arrivo A" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatArrivoA" runat="server" CssClass="form-control" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
            </div>
        </div>
        <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
        <div class="container" id="DivDettagli" runat="server">


            <!-- Riga principale con 4 colonne -->
            <div class="row">
                <!-- Colonna 2 -->
                <div class="col-md-3">
                    <label for="txtProt">Nr Protocollo</label>
                    <asp:TextBox ID="txtProt" runat="server" CssClass="form-control mb-3" ForeColor="Red" Enabled="false" Font-Bold="true" />
                    <asp:TextBox ID="txtSigla" runat="server" CssClass="form-control mb-3" placeholder="Sigla" Enabled="false" />
                    <label for="txtDataArrivo">Data Arrivo</label>
                    <asp:TextBox ID="txtDataArrivo" runat="server" CssClass="form-control mb-3" Enabled="false" />
                    <%--<asp:Label ID="lblGiorno" runat="server" CssClass="form-control mb-3" > </asp:Label>--%>
                </div>

                <!-- Colonna 3 -->
                <div class="col-md-3">
                    <label for="txtGiudice">Giudice</label>
                    <asp:TextBox ID="txtGiudice" runat="server" CssClass="form-control mb-3" Enabled="false" />
                    <label for="TxtTipoProvvAg">Tipo Provvedimento AG</label>
                    <asp:TextBox ID="TxtTipoProvvAg" runat="server" CssClass="form-control mb-3" Enabled="false" />
                    <label for="TxtQuartiere">Quartiere</label>
                    <asp:TextBox ID="TxtQuartiere" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>

                <!-- Colonna 4 -->
                <div class="col-md-3">
                    <label for="txtProvenienza">Provenienza</label>
                    <asp:TextBox ID="txtProvenienza" runat="server" CssClass="form-control mb-3" Enabled="false" />
                    <label for="txtIndirizzo">Indirizzo</label>
                    <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control mb-3" Enabled="false" />
                    <label for="txtProdPenNr">Procedimento Penale nr</label>
                    <asp:TextBox ID="txtProdPenNr" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
            </div>

            <!-- Altra riga con 4 colonne -->
            <div class="row mt-4">
                <div class="col-md-3">
                    <label for="txtNominativo">Nominativo</label>
                    <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label for="txtPraticaOut">Pratica</label>
                    <asp:TextBox ID="txtPraticaOut" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label for="txtTipoAtto">Tipologia Atto</label>
                    <asp:TextBox ID="txtTipoAtto" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label for="txtRifProtGen">Riferimento Prot. Gen.</label>
                    <asp:TextBox ID="txtRifProtGen" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
            </div>

            <!-- Note -->
            <div class="row mt-4">
                <div class="col-md-3">
                    <label for="txtNote">Eventuali Note</label>
                    <asp:TextBox ID="txtNote" runat="server" CssClass="form-control mb-3" TextMode="MultiLine" Rows="4" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label for="txtAccertatori">Accertatori</label>
                    <asp:TextBox ID="txtAccertatori" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label for="txtDataCarico">Data Carico</label>
                    <asp:TextBox ID="txtDataCarico" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label for="txtScaturito">Scaturito</label>
                    <asp:TextBox ID="txtScaturito" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
            </div>

            <!-- Pulsanti finali -->
            <div class="row mt-4">
                <div class="col-md-3">
                    <div class="form-check">
                        <asp:CheckBox ID="CkEvasa" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkEvasa">Evasa</label>
                    </div>
                </div>
                <div class="col-md-3">
                    <label for="txtDataDataEvasa" class="form-label">In data</label>
                    <asp:TextBox ID="txtDataDataEvasa" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label for="txtinviata" class="form-label">Inviata</label>
                    <asp:TextBox ID="txtinviata" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
                <div class="col-md-3">
                    <label for="txtDataInvio" class="form-label">Il</label>
                    <asp:TextBox ID="txtDataInvio" runat="server" CssClass="form-control mb-3" Enabled="false" />
                </div>
            </div>

            <div class="row">
                <div class="col-12 text-center">
                    <asp:Button Text="Nuova Ricerca" runat="server" OnClick="NuovaRicerca_Click" ToolTip="Nuova Ricerca" CssClass="btn btn-primary mt-3" />
                </div>
            </div>
        </div>
    </div>
    <%-- Modale ricerca fascicolo --%>
    <div class="modal fade" id="ModalRicerca" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 100%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel2">Ricerca Fascicolo</h5>

                </div>
                <div id="DivGrid" runat="server" visible="false" class="row">
                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="gvPopup" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopup_RowDataBound" OnRowCommand="gvPopup_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvPopup_PageIndexChanging">

                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="Nr_Protocollo" HeaderText="Nr. Carico" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="Sigla" HeaderText="Sigla" ItemStyle-Width="20px" Visible="false" />
                                <asp:BoundField DataField="nr_Pratica" HeaderText="N. Pratica" ItemStyle-Width="50px" />

                                <asp:BoundField DataField="Nominativo" HeaderText="Nominativo" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Indirizzo" HeaderText="Indirizzo" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProcedimentoPen" HeaderText="Proc. Penale" ItemStyle-Width="30px" />
                                <asp:BoundField DataField="TipoProvvedimentoAG" HeaderText="Tipo Prov. AG" ItemStyle-Width="30px" />

                                <asp:BoundField DataField="Tipologia_atto" HeaderText="Tipologia Atto" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Accertatori" HeaderText="Accertatori" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Rif_Prot_Gen" HeaderText="Prot. Generale" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Evasa" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Eval("evasa").ToString() == "True" ? "Si" : "No" %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Matricola" HeaderText="Matricola" Visible="false" />
                                <asp:BoundField DataField="DataInserimento" HeaderText="Data Inserimento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="20px" Visible="false" />
                                <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Text="Seleziona"
                                            CommandName="Select"
                                            CommandArgument='<%# Eval("Nr_Protocollo") + "|" + Eval("Matricola") + "|" + Eval("DataInserimento") + "|" + Eval("Sigla") + "|" + Eval("ID")  %>'
                                            CssClass="btn btn-success btn-sm" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" Position="Top" />
                            <PagerStyle HorizontalAlign="Center" />
                            <PagerTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 50%; text-align: left;">
                                            <asp:Label ID="lblPageInfo" runat="server" />
                                        </td>

                                    </tr>
                                </table>

                                <div style="padding: 5px;">
                                    <asp:Button ID="btnFirst" runat="server" CommandName="Page" CommandArgument="First" Text="<< Prima" CssClass="pager-button" />
                                    <asp:Button ID="btnPrev" runat="server" CommandName="Page" CommandArgument="Prev" Text="< Precedente" CssClass="pager-button" />

                                    <span style="margin: 0 10px;">Pagina:
               
                                    </span>

                                    <%-- Contenitore per i link numerici delle pagine --%>
                                    <asp:PlaceHolder ID="phPagerNumbers" runat="server" />

                                    <asp:Button ID="btnNext" runat="server" CommandName="Page" CommandArgument="Next" Text="Successiva >" CssClass="pager-button" />
                                    <asp:Button ID="btnLast" runat="server" CommandName="Page" CommandArgument="Last" Text="Ultima >>" CssClass="pager-button" />
                                </div>
                            </PagerTemplate>

                        </asp:GridView>

                    </div>
                </div>
                <asp:HiddenField ID="HidPratica" runat="server" />
                <asp:HiddenField ID="HfIdScheda" runat="server" />
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
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
