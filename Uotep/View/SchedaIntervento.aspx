<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SchedaIntervento.aspx.cs" Inherits="Uotep.SchedaIntervento" %>


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
        <div class="row">
            <!-- Colonna sinistra -->
            <div class="col-md-3 form-check">
                <asp:CheckBox ID="CkAttivita" runat="server" AutoPostBack="true" OnCheckedChanged="CkAttivita_CheckedChanged1" />
                <label class="form-check-label ms-3" for="CkAttivita">Attività Interna</label>
            </div>

            <!-- Colonna destra -->
            <div class="col-md-3 form-check">
                <label class="form-check-label ms-3 mt-3 text-left" for="RadioButton1">Area Appartenenza</label>
                <asp:RadioButton ID="rdUote" runat="server" GroupName="AreaGroup" Text="UOTE" />
                <asp:RadioButton ID="rdUotp" runat="server" GroupName="AreaGroup" Text="UOTP" />
            </div>
        </div>


        <%-- <asp:RegularExpressionValidator
            Font-Bold="true"
            ID="revData"
            ForeColor="Red"
            runat="server"
            ControlToValidate="TxtDataIntervento"
            ErrorMessage="Controllare il formato delle date, deve essere gg/mm/aaaa."
            ValidationExpression="^([0-2][0-9]|(3)[0-1])/(0[1-9]|1[0-2])/((19|20)\d\d)$"></asp:RegularExpressionValidator>
        <asp:RegularExpressionValidator
            Font-Bold="true"
            ID="RegularExpressionValidator1"
            ForeColor="Red"
            runat="server"
            ControlToValidate="TxtDataConsegna"
            ErrorMessage="Controllare il formato delle date, deve essere gg/mm/aaaa."
            ValidationExpression="^([0-2][0-9]|(3)[0-1])/(0[1-9]|1[0-2])/((19|20)\d\d)$"></asp:RegularExpressionValidator>--%>

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
                    <div class="col-6">
                        <asp:Button ID="btSalva" runat="server" ValidationGroup="bt" Text="Salva Scheda" CssClass="btn btn-primary me-3" OnClick="Salva_Click" />
                    </div>
                    <div class="col-6">
                        <asp:Button ID="btStampa" runat="server" ValidationGroup="bt" Text="Stampa" CssClass="btn btn-primary me-3" OnClick="btStampa_Click" />
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
            margin-left: -30px;
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
                                <asp:TextBox ID="txt_numEspostiSegn" runat="server" CssClass="form-control"  />
                                <%--<asp:RegularExpressionValidator ID="REx" runat="server" ControlToValidate="txt_numEspostiSegn" ErrorMessage="Solo valori numerici" ForeColor="Red" ValidationExpression="\d{5}"></asp:RegularExpressionValidator>--%>
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
                                <label class="form-check-label" for="ckControlliLavoriEdiliSenzaProt">Controlli lavori edili senza protezione (d.p.i.) [</label>
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




    <!-- Modale Bootstrap quartiere -->

    <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">Ricerca Quartiere</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <%-- <div class="form-group">
                        <label for="txtIndirizzo">Indirizzo:</label>
                        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" placeholder="Campo obbligatorio" />

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
