<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SchedaDipendente.aspx.cs" Inherits="Uotep.SchedaDipendente" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
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
        //nel textbox data sostituisce lo spazio con lo / 
        document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('TxtDataAssunzione');
        // Se non usi ClientIDMode="Static", dovresti usare:
     // var textBox = document.getElementById('<%= TxtDataAssunzione.ClientID %>');

            if (textBox) {
                textBox.addEventListener('input', function (event) {
                    // Salva la posizione attuale del cursore
                    var cursorPos = this.selectionStart;
                    var originalLength = this.value.length;

                    // Sostituisci tutti gli spazi con trattini
                    this.value = this.value.replace(/ /g, '/');

                    // Se la lunghezza è cambiata (cioè uno spazio è stato sostituito),
                    // e se l'ultimo carattere digitato era uno spazio (ora un trattino),
                    // riposiziona il cursore.
                    // Questa logica semplice funziona bene per sostituzioni 1 a 1.
                    if (this.value.length === originalLength) {
                        this.setSelectionRange(cursorPos, cursorPos);
                    } else {
                        // Se più spazi sono stati sostituiti o incollati,
                        // il cursore potrebbe andare alla fine.
                        // Per la semplice digitazione di uno spazio,
                        // cursorPos dovrebbe essere corretto.
                        this.setSelectionRange(cursorPos, cursorPos);
                    }
                });
            } else {
                console.error("Textbox con ID 'TxtDataAssunzione' non trovata.");
            }
        });



    </script>

    <div class="panel panel-default">
        <div class="form-group mb-3"></div>
        <div class="panel-heading">
            <h3 class="panel-title" style="font-weight: bold;">Anagrafica</h3>
        </div>
        <div class="row">



            <div class="col-md-3 form-check">
                <label class="form-check-label ms-3 mt-3 text-left" for="RadioButton1">Area Appartenenza</label>
                <asp:RadioButton ID="rdUote" runat="server" GroupName="AreaGroup" Text="UOTE" onclick="gestisciVisibilita();" />
                <asp:RadioButton ID="rdUotp" runat="server" GroupName="AreaGroup" Text="UOTP" onclick="gestisciVisibilita();" />
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
                    <p class="text-center lead">GESTIONE SCHEDA PERSONALE UOTEP</p>
                </div>

                <div class="container">
                    <div class="row">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMatricola" ValidationGroup="bt" ErrorMessage="Inserire matricola" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNominativo" ValidationGroup="bt" ErrorMessage="Inserire Nominativo" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="row">
                        <!-- Colonna 1 -->
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <label for="txtMatricola">Matricola CED</label>
                                <asp:TextBox ID="txtMatricola" runat="server" CssClass="form-control" />
                            </div>

                            <div class="form-group mb-3">
                                <label for="txtGrado">Grado</label>
                                <asp:TextBox ID="txtGrado" runat="server" CssClass="form-control" />
                            </div>


                        </div>

                        <!-- Colonna 2 -->
                        <div class="col-md-3 d-flex flex-column justify-content-center">

                            <div class="form-group mb-3">
                                <label for="txtNominativo">Nominativo</label>
                                <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" />
                            </div>

                            <!-- Colonna 3 -->
                            <div class="form-group mb-3">
                                <label for="txtUfficio">Ufficio</label>
                                <asp:TextBox ID="txtUfficio" runat="server" CssClass="form-control" />
                            </div>

                        </div>

                        <!-- Colonna 4 -->
                        <div class="col-md-3">

                            <div class="form-group mb-3">
                                <label for="TxtDataAssunzione">Data Assunzione</label>
                                <asp:TextBox ID="TxtDataAssunzione" runat="server" CssClass="form-control data-auto" ClientIDMode="Static" />

                            </div>

                            <div class="form-group mb-3">
                                <label for="txtMacroArea">Macro Area</label>
                                <asp:TextBox ID="txtMacroArea" runat="server" CssClass="form-control" MaxLength="1" />
                            </div>
                        </div>

                        <!-- Colonna 5 -->
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <label for="TxtCategoriaEconomica">Categoria Econ.</label>
                                <asp:TextBox ID="TxtCategoriaEconomica" runat="server" CssClass="form-control" MaxLength="2" />

                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDataProssimaSorveglianza">Prox. Sorveglianza Sanitaria</label>
                                <asp:TextBox ID="txtDataProssimaSorveglianza" runat="server" CssClass="form-control data-auto" />
                            </div>

                        </div>
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
            margin-left: -2px;
        }

        .larghezzaText {
            width: 50px;
        }
    </style>
    <%-- panel dei dettagli --%>
    <div class="panel panel-default" id="divDettagli" runat="server">
        <div class="panel-heading">
            <h3 class="panel-title" style="font-weight: bold;">Dettagli Aggiuntivi</h3>
        </div>
        <div class="panel-body ">
            <div class="container-fluid ">

                <div class="row custom-border">
                    <div class="col-md-3">
                        <div class="form-group mb-2" style="display: flex; align-items: center; gap: 5px;">
                            <label for="txtGruppoRep" style="margin-bottom: 0; white-space: nowrap;">Gruppo Rep.</label>
                            <asp:TextBox ID="txtGruppoRep" runat="server" CssClass="form-control larghezzaText" />
                        </div>
                    </div>


                    <div class="col-md-3">
                        <!-- Allargato leggermente a col-md-4 per evitare a capo -->
                        <!-- HEADER BOX 3: Altezza forzata identica alla colonna 2 -->
                        <div class="form-group mb-2">
                            <%--<asp:CheckBox ID="ckQuartina" runat="server" CssClass="form-check-input" />--%>
                            <label class="form-check-label" style="margin-right: 5px;" for="ckQuartina">Quartina [</label>
                            <span style="white-space: nowrap;">
                                <asp:RadioButton ID="rdQ1" runat="server" GroupName="AccertamentoGroup" Text="I" />
                                <asp:RadioButton ID="rdQ2" runat="server" GroupName="AccertamentoGroup" Text="II" />
                                <asp:RadioButton ID="rdQ3" runat="server" GroupName="AccertamentoGroup" Text="III" />
                                <asp:RadioButton ID="rdQ4" runat="server" GroupName="AccertamentoGroup" Text="IV" />
                            </span>
                            <label class="form-check-label">]</label>
                        </div>


                    </div>
                    <div class="col-md-3">
                        <div class="form-check mb-2" style="min-height: 30px;">
                            <!-- TEXTBOX 3 -->
                            <div class="form-group mb-2" style="display: flex; align-items: center; gap: 5px;">
                                <label for="txtGruppoQ" style="margin-bottom: 0; white-space: nowrap; text-align: right;">Gruppo Q.</label>
                                <asp:TextBox ID="txtGruppoQ" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-check mb-2" style="min-height: 30px;">
                            <!-- TEXTBOX 3 -->
                            <div class="form-group mb-2" style="display: flex; align-items: center; gap: 5px;">
                                <label for="txtTurnoPref" style="margin-bottom: 0; white-space: nowrap; text-align: right;">Turno Pref.</label>
                                <asp:TextBox ID="txtTurnoPref" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
                <%-- row 2 --%>
                <div class="row custom-border">
                    <div class="col-md-2">
                        <div class="form-check mb-2">
                            <asp:CheckBox ID="ckArmato" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="ckArmato">Armato</label>
                        </div>
                    </div>

                    <div class="col-md-2">
                        <div class="form-check mb-2" style="display: block;">
                            <asp:CheckBox ID="ckAutista" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="ckAutista">Autista</label>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-check mb-2">
                            <asp:CheckBox ID="ckLimitazioni" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="ckLimitazioni">Limitazioni</label>
                        </div>
                    </div>

                    <div class="col-md-2">
                        <div class="form-check mb-2">
                            <asp:CheckBox ID="ckArt53" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="ckArt53">L.53 Art 4 co.1</label>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-check mb-2">
                            <asp:CheckBox ID="ckL104" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="ckL104">L.104</label>
                        </div>
                    </div>
                </div>





                <%--<div class="col-md-3">
                        <div class="form-check mb-2" style="display: block;">
                            <asp:CheckBox ID="ckPermStudio" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="ckPermStudio">Permessi Studio</label>
                        </div>
                    </div>--%>
            </div>
        </div>
    </div>
    <!-- Bottone Salva -->
    <div class="row">
        <div class="col-6">
            <asp:Button ID="btSalva" runat="server" ValidationGroup="bt" Text="💾 Salva Scheda" CssClass="btn btn-primary me-3" OnClick="Salva_Click" />
            <asp:Button ID="btCerca" runat="server" Text="📂 Ricerca da DB" CssClass="btn btn-primary" OnClick="btCerca_Click" />
        </div>
        <%--<div class="col-6">
                        <asp:Button ID="btStampa" runat="server" ValidationGroup="bt" Text="Stampa" CssClass="btn btn-primary me-3" OnClick="btStampa_Click" />
                    </div>--%>
    </div>


    <asp:HiddenField ID="HfIdScheda" runat="server" />

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

                        <p id="errorMessage" runat="server" style="color: red"></p>

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
