<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EstraiStatistiche.aspx.cs" Inherits="Uotep.EstraiStatistiche" %>


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

        <div class="panel-body" id="divTesta" runat="server">
            <div class="jumbotron">
                <div style="margin-top: -90px!important">
                    <%--<asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>--%>
                    <p class="text-center lead">ESTRAZIONE STATISTICHE</p>
                </div>

                <div class="container">
                    <div class="row align-items-end">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtMese">Mese</label>
                                <asp:TextBox ID="txtMese" runat="server" CssClass="form-control" Width="110px" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtAnno">Anno</label>
                                <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control" Width="110px" />
                            </div>
                        </div>


                    </div>
                </div>

                <!-- Bottone Salva -->
                <div class="col-md-4 ">
                    <div class="form-group">

                        <asp:Button ID="btEsegui" runat="server" ValidationGroup="bt" Text="Esegui" CssClass="btn btn-primary" OnClick="btEsegui_Click" />
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
            width:110px;
        }
    </style>
    <%-- panel dei dettagli --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title" style="font-weight: bold;">Dettagli</h3>
        </div>
        <div class="panel-body" id="divDettagli" runat="server">
            <div class="container">

                <div class="tab-content">

                    <div class="row custom-border">
                        <div class="col-md-6 ">
                            <div class="form-group mb-3">
                                <label for="txtRelazioni">Relazioni</label>
                                <asp:TextBox ID="txtRelazioni" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="TxtPonteggi">Ponteggi</label>
                                <asp:TextBox ID="TxtPonteggi" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDPI">DPI</label>
                                <asp:TextBox ID="txtDPI" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtEspostiRicevuti">Esposti Ricevuti</label>
                                <asp:TextBox ID="txtEspostiRicevuti" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtCNR">CNR</label>
                                <asp:TextBox ID="txtCNR" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtAnnotazioni">Annotazioni</label>
                                <asp:TextBox ID="txtAnnotazioni" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDelegheEsitate">Deleghe Esitate</label>
                                <asp:TextBox ID="txtDelegheEsitate" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtCnrAnnotazioni">CNR + Annotazioni</label>
                                <asp:TextBox ID="txtCnrAnnotazioni" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtInterrogatori">Interrogatori</label>
                                <asp:TextBox ID="txtInterrogatori" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtConvalide">Convalide</label>
                                <asp:TextBox ID="txtConvalide" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtViolazioneSigilli">Violazione Sigilli</label>
                                <asp:TextBox ID="txtViolazioneSigilli" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDissequestriTemp">Dissequestri Temp.</label>
                                <asp:TextBox ID="txtDissequestriTemp" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtRimozioneSigilli">Rimozione Sigilli</label>
                                <asp:TextBox ID="txtRimozioneSigilli" runat="server" CssClass="form-control larghezzaText" />
                            </div>




                        </div>


                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label for="txtEspostiEvasi">Esposti Evasi</label>
                                <asp:TextBox ID="txtEspostiEvasi" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtRipristino">Ripr. Tot. Parz.</label>
                                <asp:TextBox ID="txtRipristino" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtControlliScia">Controlli Scia</label>
                                <asp:TextBox ID="txtControlliScia" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtControlliCant">Controlli C/Ri Giornalieri</label>
                                <asp:TextBox ID="txtControlliCant" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtNotifiche">Notifiche</label>
                                <asp:TextBox ID="txtNotifiche" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtSequestri">Sequestri</label>
                                <asp:TextBox ID="txtSequestri" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtRiapposizioneSigilli">Riapposizione Sigilli</label>
                                <asp:TextBox ID="txtRiapposizioneSigilli" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDelegheRicevute">Deleghe Ricevute</label>
                                <asp:TextBox ID="txtDelegheRicevute" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDenunceUff">Denunce Uff</label>
                                <asp:TextBox ID="txtDenunceUff" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDemolizioni">Demolizioni</label>
                                <asp:TextBox ID="txtDemolizioni" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDissequestri">Dissequestri</label>
                                <asp:TextBox ID="txtDissequestri" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtControlliDLGS">Controlli D.LGS 42/04</label>
                                <asp:TextBox ID="txtControlliDLGS" runat="server" CssClass="form-control larghezzaText  " />
                            </div>


                        </div>
                    </div>


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
