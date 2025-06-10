<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RicercaScheda.aspx.cs" Inherits="Uotep.RicercaScheda" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }

        // Mostra il popup ricerca
        function showModal() {
            $('#ModalRicerca').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalRicerca').modal('hide');
        }
        // Funzione per aggiungere testo a un TextBox
        function appendToTextBox(TextPattugliaCompleta, DdlPattuglia) {
            // Ottieni il TextBox tramite il suo ID
            const textBox = document.getElementById(TextPattugliaCompleta);
            const dropDown = document.getElementById(DdlPattuglia);
            // Aggiungi il valore al contenuto corrente
            if (textBox && dropDown) {
                // Ottieni il valore selezionato nella DropDownList
                const selectedValue = dropDown.value;

                // Aggiungi il valore selezionato al contenuto del TextBox
                textBox.value += selectedValue;
            }
        }


    </script>

    <div class="panel panel-default">
        <div class="form-group mb-3"></div>
        <div class="panel-heading">
            <h3 class="panel-title" style="font-weight: bold;">Intervento</h3>
        </div>
        <!-- Colonna sinistra -->
        <div class="col-md-3 form-check">
            <asp:CheckBox ID="CkAttivita" runat="server" Enabled="false" />
            <label class="form-check-label ms-3" for="CkAttivita">Attività Interna</label>
        </div>

        <!-- Colonna destra -->
        <div class="col-md-3 form-check">
            <label class="form-check-label ms-3 mt-3 text-left" for="RadioButton">Area Appartenenza</label>
            <asp:RadioButton ID="rdUote" runat="server" GroupName="AreaGroup" Text="UOTE" Enabled="false" />
            <asp:RadioButton ID="rdUotp" runat="server" GroupName="AreaGroup" Text="UOTP" Enabled="false" />
        </div>


        <div class="panel-body" id="divTesta" runat="server">
            <div class="jumbotron">
                <div style="margin-top: -50px!important">
                    <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
                    <p class="text-center lead">COMPILAZIONE SCHEDA INTERVENTO</p>
                </div>

                <div class="container">
                    <div class="row">
                        <!-- Colonna 1 -->
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <label for="txtPratica">Nr Pratica</label>
                                <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" Font-Bold="true" ForeColor="Red" />
                            </div>

                            <div class="form-group mb-3">
                                <label for="txtIndirizzo">Indirizzo</label>
                                <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="ddlCapopattuglia">Capo Pattuglia</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlCapopattuglia" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtNote">Note</label>
                                <asp:TextBox ID="txtNote" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                            </div>
                        </div>

                        <!-- Colonna 2 -->
                        <div class="col-md-3 d-flex flex-column justify-content-center">
                            <div class="form-group mb-3">
                                <label for="TxtDataIntervento">Data Intervento</label>
                                <asp:TextBox ID="TxtDataIntervento" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtDataIntervento" ValidationGroup="bt" ErrorMessage="Inserire data intervento" ForeColor="Red">
                                </asp:RequiredFieldValidator>

                            </div>

                            <div class="form-group mb-3" style="margin-top: -20px!important">
                                <label for="txtDataConsegna">Data Consegna</label>
                                <asp:TextBox ID="txtDataConsegna" runat="server" CssClass="form-control" />
                            </div>

                            <div class="form-group mb-3">
                                <label for="DdlPattuglia">Pattuglia</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="DdlPattuglia" runat="server" CssClass="form-control" />
                                </div>
                            </div>

                            <!-- Colonna 3 -->
                        </div>
                        <div class="col-md-1" style="margin-top: 180px!important">
                            <div class="form-group mb-3">
                                <asp:Button ID="btAggiungi" runat="server" Text=">>" CssClass="btn btn-primary me-3" OnClick="Aggiungi_Click" ToolTip="Aggiungi" />
                                <asp:Button ID="btElimina" runat="server" Text="<<" CssClass="btn btn-primary me-3" OnClick="btElimina_Click" ToolTip="Elimina" />
                            </div>
                        </div>
                        <!-- Colonna 4 -->
                        <div class="col-md-3">



                            <div class="form-group mb-3">
                                <label for="txtNominativo">Nominativo</label>
                                <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" />
                            </div>

                            <div class="form-group mb-3" style="margin-top: 120px!important">
                                <asp:ListBox ID="LPattugliaCompleta" runat="server" CssClass="form-control"></asp:ListBox>
                            </div>

                        </div>
                    </div>

                </div>

                <!-- Bottone Salva -->
                <div class="row">
                    <div class="col-12">
                        <asp:Button ID="btSalva" runat="server" ValidationGroup="bt" Text="Salva Scheda" CssClass="btn btn-primary me-3" OnClick="btSalva_Click" Enabled="false" />

                    </div>
                </div>
            </div>
        </div>

        <asp:Button ID="btStampa" runat="server" Text="Stampa" CssClass="btn btn-primary me-3" OnClick="btStampa_Click" />

    </div>

    <!-- Bottone Salva -->
    <div cssclass="text-center" style="margin-bottom: 15px!important">
        <div class="col-12">
            <asp:Button ID="btModificaScheda" runat="server" ValidationGroup="bottoni" Text="Modifica Scheda" CssClass="btn btn-primary me-3" OnClick="btModificaScheda_Click" />
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
    </style>
    <%-- panel dei dettagli --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title" style="font-weight: bold;">Dettagli Aggiuntivi</h3>
        </div>
        <div class="panel-body" id="divDettagli" runat="server">
            <div class="container">

                <div class="tab-content">
                    <%--<div class="tab-pane fade show active" id="scheda1" role="tabpanel">--%>
                    <p style="font-weight: bold;">Fonte intervento</p>
                    <div class="row custom-border">
                        <div class="col-md-6 ">
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckDelega" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckDelega">Delega Ag.</label>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckSegnalazione" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckSegnalazione">Segnalazione</label>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckNotifica" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckNotifica">Notifica</label>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckCdr" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckCdr">CDR</label>
                            </div>
                        </div>


                        <div class="col-md-6">
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckResa" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckResa">R.E.S.A.</label>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckEsposto" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckEsposto">Esposto n°</label>
                                <asp:TextBox ID="txt_numEspostiSegn" runat="server" CssClass="form-control" MaxLength="10" />
                                <%--<asp:RegularExpressionValidator ID="REx" runat="server" ControlToValidate="txt_numEspostiSegn" ValidationGroup="bottoni" ErrorMessage="Solo valori numerici" ForeColor="Red" ValidationExpression="\d{5}"></asp:RegularExpressionValidator>--%>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckIniziativa" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckIniziativa">Iniziativa</label>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckCoordinatore" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ChecckCoordinatorekBox8">Coordinatore di turno</label>
                            </div>
                        </div>
                    </div>

                    <div class="tab-content">
                        <%--<div class="tab-pane fade show active" id="scheda1" role="tabpanel">--%>
                        <p style="font-weight: bold;">Atti Redatti</p>

                        <div class="row custom-border">

                            <div class="col-md-6">
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckRelazione" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckRelazione">Relazione</label>
                                </div>
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckAnnotazionePG" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckAnnotazionePG">Annotazione PG</label>
                                </div>
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckEsitoDelega" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckEsitoDelega">Esito Delega</label>
                                </div>

                            </div>
                            <div class="col-md-6">
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckCnr" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckCnr">C.N.R.</label>
                                </div>
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckVerbaleSeq" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckVerbaleSeq">Verbale di Sequestro</label>
                                </div>
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckContestazioneAmm" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckContestazioneAmm">Contestazione amministrativa (ex. art 7 L. 241/90)</label>
                                </div>

                            </div>
                        </div>

                    </div>
                    <div class="tab-content">
                        <%--<div class="tab-pane fade show active" id="scheda1" role="tabpanel">--%>
                        <p style="font-weight: bold;">Provvedimenti adottati e attività svolte</p>

                        <div class="row custom-border">
                            <div class="col-md-6">
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckConvalida" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckConvalida">Convalida</label>
                                </div>
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckViolazioneSigilli" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckViolazioneSigilli">Violazione Sigilli</label>
                                </div>
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckViolazioneBeniCult" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckViolazioneBeniCult">Violazione Codici dei Beni Culturali(D.Lgs. n. 42/04 artt. 169/181)</label>
                                </div>

                            </div>
                            <div class="col-md-6">
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckDisseqDefinitivo" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckDisseqDefinitivo">Dissequestro Definitivo</label>
                                </div>

                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckDisseqTemp" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckDisseqTemp">Dissequestro Temporaneo [</label>
                                    <asp:RadioButton ID="rdRimozione" runat="server" GroupName="DissequestroGroup" Text="Rimozione" />
                                    <asp:RadioButton ID="rdRiapposizione" runat="server" GroupName="DissequestroGroup" Text="Riapposizione" />
                                    <label class="form-check-label">]</label>
                                </div>
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckAccertAvvenutoRipr" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckAccertAvvenutoRipr">Accertamento avvenuto ripristino [</label>
                                    <asp:RadioButton ID="rdTotale" runat="server" GroupName="AccertamentoGroup" Text="Totale" />
                                    <asp:RadioButton ID="rdParziale" runat="server" GroupName="AccertamentoGroup" Text="Parziale" />
                                    <asp:RadioButton ID="rdNonAvvenuto" runat="server" GroupName="AccertamentoGroup" Text="Non Avvenuto" />

                                    <label class="form-check-label">]</label>
                                </div>
                                <div class="form-check mb-2">
                                    <asp:CheckBox ID="ckControlliSCIA" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="ckControlliSCIA">Controlli SCIA</label>
                                </div>

                            </div>
                        </div>

                    </div>
                    <div class="tab-content">
                        <%--<div class="tab-pane fade show active" id="scheda1" role="tabpanel">--%>
                        <p style="font-weight: bold;">Qualificazione dell'intervento</p>

                        <div class="row custom-border">

                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckContrSuoloPubblico" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckContrSuoloPubblico">Controllo aree cantiere su suolo pubblico (impalcature)</label>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckControlliCant" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckControlliCant">Controllo Cantiere (rientrano i controlli dei cantieri sottoposti a sequestro)</label>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckControlloDaEsposti" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckControlloDaEsposti">Controllo nato da esposti</label>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckControlliDaSegnalazioni" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckControlliDaSegnalazioni">Controllo nato da segnalazioni (provenienti da cittadini, Servizi comunali ed altre Istituzioni)</label>
                            </div>
                            <div class="form-check mb-2">
                                <asp:CheckBox ID="ckControlliLavoriEdiliSenzaProt" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckControlliLavoriEdiliSenzaProt">Controlli lavori edili con/senza protezione (d.p.i.) [</label>
                                <asp:RadioButton ID="rdCon" runat="server" GroupName="ProtezioniGroup" Text="Con" />
                                <asp:RadioButton ID="rdSenza" runat="server" GroupName="ProtezioniGroup" Text="Senza" />
                                <label class="form-check-label">]</label>
                            </div>


                        </div>

                    </div>

                    <%--</div>--%>
                </div>
            </div>
        </div>
    </div>



    <%-- Modale ricerca scheda --%>
    <div class="modal fade" id="ModalRicerca" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel2">Ricerca Schede</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">
                        <label for="txtModPratica">Nr. Pratica:</label>
                        <asp:TextBox ID="txtModPratica" runat="server" CssClass="form-control" placeholder="Numero Pratica" />

                    </div>
                    <div class="form-group">
                        <label for="txtModPratica">Pattuglia:</label>
                        <asp:TextBox ID="txtModPattuglia" runat="server" CssClass="form-control" placeholder="Pattuglia" />

                    </div>

                    <div class="form-group">
                        <label for="txtModDataIntervento">Data Intervento:</label>
                        <asp:TextBox ID="txtModDataIntervento" runat="server" CssClass="form-control" placeholder="gg/mm/aaaa" />
                    </div>
                    <div class="form-group">
                        <label for="txtModAttivitàInterna">Attività Interna:</label>
                        <asp:CheckBox ID="ckModAttivitòInterna" runat="server" />
                    </div>
                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="GVRicecaScheda" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopup_RowDataBound" OnRowCommand="gvPopup_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="GVRicecaScheda_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="id_rapp_scheda" HeaderText="ID"  Visible="false"/>
                                <asp:BoundField DataField="rapp_numero_pratica" HeaderText="Numero Pratica" />
                                <asp:BoundField DataField="rapp_nominativo" HeaderText="Nominativo" />
                                <asp:BoundField DataField="rapp_pattuglia" HeaderText="Pattuglia" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("rapp_numero_pratica") + ";" + Eval("rapp_nominativo") + ";" + Eval("rapp_pattuglia") + ";" + Eval("id_rapp_scheda")   %>' CssClass="btn btn-success btn-sm" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" Position="Top" />
                            <PagerStyle HorizontalAlign="Center" />
                            <PagerTemplate>
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
                <asp:HiddenField ID="HfIdScheda" runat="server" />
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />
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
