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
    <style>
        /* Stile per centrare orizzontalmente un elemento a blocco come una tabella */
.center-table {
    margin-left: auto;
    margin-right: auto;
    /* In breve: margin: 0 auto; */
    /* Opzionale: potresti voler collassare i bordi se li usi */
    /* border-collapse: collapse; */
    /* Puoi usare border-spacing se vuoi spazio tra le celle (sia riga che colonna) */
    /* border-spacing: 0 15px; /* 0 orizzontale, 15px verticale */
}

/* Stile per aggiungere spazio interno alle celle (padding), crea distanza tra i bottoni */
/* Aggiunge padding a tutte le celle della tabella con classe center-table */
.center-table td {
    padding-bottom: 15px; /* Aggiunge 15px di spazio SOTTO il contenuto della cella */
    padding-top: 5px;    /* Opzionale: Aggiunge un po' di spazio SOPRA */
    /* Puoi anche aggiungere padding orizzontale se necessario, ma mx-2 sul bottone già lo fa */
    /* padding-left: 5px; */
    /* padding-right: 5px; */
}

/* Stile per rendere i bottoni stessa altezza e larghezza */
.uniform-button {
    width: 180px !important; /* Esempio: Larghezza fissa per i bottoni */
    height: 45px !important;  /* Esempio: Altezza fissa per i bottoni */
    display: flex !important;
    justify-content: center !important;
    align-items: center !important;
    text-align: center !important;
    white-space: normal !important;
    word-break: break-word !important;
    margin-left: 10px !important;
}

    </style>
    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">RICERCA UNA PRATICA</p>
            <!-- Contenitore per centrare -->

            <asp:Panel ID="pnlButton" runat="server" CssClass="text-center" Visible="true">
                <%--
        Il div sottostante con d-flex e justify-content-center sta cercando di centrare
        il contenuto interno usando Flexbox. Può essere utile per il mt-4 (margin top).
        Aggiungendo margin: 0 auto; alla tabella, centriamo la tabella stessa come blocco.
                --%>
                <div class="d-flex justify-content-center mt-4">
                    <%-- Inizio Tabella per i Pulsanti --%>
                    <table class="center-table">
                        <%-- <--- AGGIUNGI class="center-table" QUI --%>
                        <tr>
                            <%-- Prima riga: 4 pulsanti --%>
                            <td>
                                <asp:Button ID="btNominativo" runat="server" OnClick="btNominativo_Click" Text="Responsabile" ToolTip="Ricerca Responsabile" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btIndirizzo" runat="server" OnClick="btIndirizzo_Click" Text="Indirizzo" ToolTip="Ricerca Per Indirizzo" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btDatiCatastali" runat="server" OnClick="btDatiCatastali_Click" Text="Dati Catastali" ToolTip="Dati Catastali" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btNpraticaStorico" runat="server" CommandArgument="StoricoPratica" OnClick="btNpratica_Click" Text="Storico Pratica" ToolTip="Storico Della Pratica" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                        </tr>
                        <tr>
                            <%-- Seconda riga: altri 4 pulsanti --%>
                            <td>
                                <asp:Button ID="btNota" runat="server" OnClick="btNota_Click" Text="Ricerca Nota" ToolTip="Ricerca Nota" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btAnnoMese" runat="server" OnClick="btAnnoMese_Click" Text="Ricerca Anno/Mese" ToolTip="Ricerca Anno/Mese" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btEstraiParziale" runat="server" OnClick="btEstraiParziale_Click" Text="Estrazione Parziale" ToolTip="Estrazione Parziale" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btEstraiTotale" runat="server" OnClick="btEstraiTotale_Click" Text="Estrai DB" ToolTip="Estrazione Totale" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btNpratica" runat="server" OnClick="btNpratica_Click" CommandArgument="Pratica"  Text="Nr. Pratica" ToolTip="Ricerca Pratica" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                        </tr>
                    </table>
                    <%-- Fine Tabella Pulsanti --%>
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

                    <asp:Label ID="Label9" runat="server" Text="Responsabile" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtResponsabile" runat="server" CssClass="form-control" placeholder="Responsabile" />

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
                <%-- DIV RICERCA Note --%>
                <div id="DivNote" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label4" runat="server" Text="Nota" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicNota" runat="server" CssClass="form-control" placeholder="Nota" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Anno Mese --%>
                <div id="DivAnnoMese" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label5" runat="server" Text="Anno" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control" placeholder="Anno" />
                    <asp:Label ID="Label8" runat="server" Text="Mese" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtMese" runat="server" CssClass="form-control" placeholder="Numero del Mese" />
                    <asp:RegularExpressionValidator
                        ID="RegularExpressionValidator2"
                        runat="server"
                        ControlToValidate="txtMese"
                        ValidationExpression="^([1-9]|1[0-2])$"
                        ErrorMessage="Inserisci un numero da 1 a 12"
                        ForeColor="Red"
                        ValidationGroup="bt"
                        Display="Static">
                    </asp:RegularExpressionValidator>

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>

            </div>

        </div>


        <%-- DIV Estrai db --%>
        <div id="DivEstraiDb" runat="server" visible="false" style="margin-left: 250px!important">

            <div class="row justify-content-start">
                <!-- Aggiunto justify-content-start per sicurezza -->
                <div class="form-check" style="display: flex; flex-direction: row; align-items: baseline;">
                    <!-- Stili per allineamento e spacing -->
                    <div style="margin-right: 50px;">
                        <%--                        <!- Spacing per il primo gruppo ->--%>
                        <asp:CheckBox ID="CkEvasa" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkEvasa">Evasa</label>
                    </div>
                    <div style="margin-right: 50px;">
                        <%--                        <!- Spacing per il secondo gruppo ->--%>
                        <asp:CheckBox ID="Ck1089" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="Ck1089">1089</label>
                    </div>
                    <div style="margin-right: 50px;">
                        <%--                        <!- Spacing per il terzo gruppo ->--%>
                        <asp:CheckBox ID="CkSuoloPubblico" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkSuoloPubblico">Suolo Pubblico</label>
                    </div>
                    <div style="margin-right: 50px;">
                        <%--                        <!- Spacing per il quarto gruppo ->--%>
                        <asp:CheckBox ID="CkVincoli" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkVincoli">Vincoli</label>
                    </div>
                    <div>
                        <%--                        <!- Nessun margin-right per l'ultimo gruppo ->--%>
                        <asp:CheckBox ID="CkDemolita" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkDemolita">Demolita</label>
                    </div>
                </div>
            </div>

            <div class="row justify-content-start">
                <!-- Aggiunto justify-content-start per sicurezza -->
                <div class="form-check" style="display: flex; flex-direction: row; align-items: baseline;">
                    <!-- Stili per allineamento e spacing -->
                    <div style="margin-right: 50px;">
                        <%--                        <!- Spacing per il primo gruppo ->--%>
                        <asp:CheckBox ID="CkPropPriv" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkPropPriv">Prop. Privata</label>
                    </div>

                    <div style="margin-right: 50px;">
                        <%--                        <!- Spacing per il secondo gruppo ->--%>
                        <asp:CheckBox ID="CkPropComunale" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkPropComunale">Prop. Comunale</label>
                    </div>

                    <div style="margin-right: 50px;">
                        <%--                        <!- Spacing per il terzo gruppo ->--%>
                        <asp:CheckBox ID="CkPropBeniCult" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkPropBeniCult">Prop. Beni Cult.</label>
                    </div>

                    <div>
                        <%--                        <!- Nessun margin-right per l'ultimo gruppo ->--%>
                        <asp:CheckBox ID="CkPropAltri" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkPropAltri">Prop. Altri Enti</label>
                    </div>
                </div>
            </div>
            <div class="row">
                <div style="margin-left: 1px!important; margin-top: 30px!important">
                    <asp:Button Text="Estrai" runat="server" OnClick="Estrai_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                </div>
            </div>
        </div>


    </div>
    <asp:HiddenField ID="HfPratica" runat="server" Value="" />



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
