<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModificaRiservata.aspx.cs" Inherits="Uotep.ModificaRiservata" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
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
        function showModal() {
            $('#ModalPratica').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalPratica').modal('hide');
        }
        //nel textbox data sostituisce lo spazio con lo / 
        document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('txtDataCarico');
        // Se non usi ClientIDMode="Static", dovresti usare:
             // var textBox = document.getElementById('<%= txtDataCarico.ClientID %>');

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
                console.error("Textbox con ID 'txtDataCarico' non trovata.");
            }
        });
        document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('txtDataInvio');
        // Se non usi ClientIDMode="Static", dovresti usare:
              // var textBox = document.getElementById('<%= txtDataInvio.ClientID %>');

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
                console.error("Textbox con ID 'txtDataInvio' non trovata.");
            }
        });
        document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('txtDataDataEvasa');
        // Se non usi ClientIDMode="Static", dovresti usare:
                // var textBox = document.getElementById('<%= txtDataDataEvasa.ClientID %>');

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
                    console.error("Textbox con ID 'txtDataDataEvasa' non trovata.");
                }
            });
        //giudice
        function filterDropdownGiudice() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtGiudice");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlGiudice.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsList.ClientID %>');
            suggestionsListDiv.innerHTML = ""; // Pulisci la lista dei suggerimenti precedenti

            var suggestionsFound = false;

            if (filter.length > 0) { // Esegui il filtro solo se c'è testo nell'input
                for (i = 0; i < options.length; i++) {
                    txtValue = options[i].textContent || options[i].innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        suggestionsFound = true;
                        var suggestionElement = document.createElement("div");
                        suggestionElement.textContent = txtValue;
                        suggestionElement.classList.add("suggestion-item"); // Aggiungi una classe CSS per lo stile
                        suggestionElement.addEventListener('mouseover', function () { this.classList.add('suggestion-hover'); }); // Effetto hover con classe CSS
                        suggestionElement.addEventListener('mouseout', function () { this.classList.remove('suggestion-hover'); });
                        suggestionElement.addEventListener('click', function () {
                            input.value = this.textContent;
                            suggestionsListDiv.style.display = "none";
                        });
                        suggestionsListDiv.appendChild(suggestionElement);
                    }
                }
            }

            if (suggestionsFound) {
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }

        //tipo provvedimento
        function filterDropdownTipoProv() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtTipoProv");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlTipoProvvAg.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            // var suggestionsListDiv = document.getElementById("MainContent_suggestionsList");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListTipoProv.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //quartiere
        function filterDropdownQuartiere() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtQuartiere");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlQuartiere.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListQuartiere.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //provenienza
        function filterDropdownProvenienza() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtProvenienza");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlProvenienza.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListProvenienza.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //tipo atto
        function filterDropdownTipoAtto() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtTipoAttoR");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlTipoAtto.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListTipoAtto.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //Inviata
        function filterDropdownInviata() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtInviata");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlInviati.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListInviata.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //Scaturito
        function filterDropdownScaturito() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtScaturito");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlScaturito.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListScaturito.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //Indirizzo
        function filterDropdownIndirizzo() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtIndirizzo");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlIndirizzo.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListIndirizzo.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
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
                    <div class="form-group text-center mt-4">
                        <asp:Button Text="Login" runat="server" OnClick="trova_Click" ToolTip="Login" CssClass="btn btn-primary px-4" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <!-- Contenitore per centrare -->
        <div id="DivRicerca" runat="server" class="d-flex flex-column justify-content-center align-items-center" style="height: 100px;">
            <!-- Righe di input con Bootstrap -->
            <%--<div class="d-flex gap-3 w-50">
                <label for="txtProtoloccolRicerca">Nr Protocollo</label>
                <asp:TextBox ID="txtProtoloccolRicerca" runat="server" CssClass="form-control" placeholder="Nr Protocollo" />
                <label for="txtAnnoRicerca">Anno</label>
                <asp:TextBox ID="txtAnnoRicerca" runat="server" CssClass="form-control" placeholder="Anno" />
                <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Nuova Ricerca" CssClass="btn btn-primary mt-3" />

            </div>--%>
            <asp:Panel ID="pnlButton" runat="server" CssClass="text-center" Visible="true">
                <div class="d-flex justify-content-center mt-4">

                    <p>
                        <!-- Pulsanti -->
                        <asp:Button ID="btNProtocollo" runat="server" OnClick="btNProtocollo_Click" Text="Nr. Protocollo" CssClass="btn btn-primary mx-2" ToolTip="Ricerca per numero protocollo" />
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
                        <asp:Button ID="btDataCarico" runat="server" OnClick="btDataCarico_Click" Text="Data Carico" ToolTip="Ricerca Data Carico" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btAccertatori" runat="server" OnClick="btAccertatori_Click" Text="Accertatori" ToolTip="Ricerca Accertatori" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btIndirizzo" runat="server" OnClick="btIndirizzo_Click" Text="Indirizzo" ToolTip="Ricerca Per Indirizzo" CssClass="btn btn-primary mx-2" />
                    </p>
                </div>

            </asp:Panel>
        </div>
        <%--  --%>
        <div id="DivRicercaButton" runat="server" class="row d-flex justify-content-center align-items-center vh-100" style="height: 300px; margin-left: 400px!important">
            <!-- Righe di input ricerche  -->
            <div class="col-md-4 custom-border">
                <%-- DIV RICERCA PROTOCOLLO --%>
                <div id="DivProtocollo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label2" runat="server" Text="Nr Protocollo" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNProtocollo" ErrorMessage="Inserire numero pratica" ForeColor="Red" ValidationGroup="bt">

                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtNProtocollo" runat="server" CssClass="form-control" placeholder="Nr Protocollo" />


                    <asp:Label ID="Label3" runat="server" Text="Anno" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:RequiredFieldValidator ID="rqanno" runat="server" ControlToValidate="txtAnnoRicerca" ErrorMessage="Inserire l'anno per la ricerca" ForeColor="Red" ValidationGroup="bt">

                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtAnnoRicerca" runat="server" CssClass="form-control" placeholder="Anno" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA PROocedimento penale --%>
                <div id="DivProcPenale" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label4" runat="server" Text="Nr Procediemnto Penale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProcPenale" runat="server" CssClass="form-control" placeholder="Nr Procedimento Penale" />



                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA evasa ag --%>
                <div id="DivEvasaAg" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label5" runat="server" Text="Data Inizio Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataDa" runat="server" CssClass="form-control" placeholder="Data Inizio" />

                    <asp:Label ID="Label6" runat="server" Text="Data Fine Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataA" runat="server" CssClass="form-control" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA rif protocollo generale --%>
                <div id="DivProtGen" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label7" runat="server" Text="Rif. Protocollo Generale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProtGen" runat="server" CssClass="form-control" placeholder="Rif. Prot. Gen." />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA pratica --%>
                <div id="DivPratica" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label8" runat="server" Text="Nr. Pratica" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtPraticaR" runat="server" CssClass="form-control" placeholder="Nr. Pratica" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA giudice --%>
                <div id="DivGiudice" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label9" runat="server" Text="Giudice" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicGiudice" runat="server" CssClass="form-control" placeholder="Giudice" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA provenienza --%>
                <div id="DivProvenienza" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label10" runat="server" Text="Provenienza" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicProvenienza" runat="server" CssClass="form-control" placeholder="Provenienza" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA nominativo --%>
                <div id="DivNominativo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label11" runat="server" Text="Nominativo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicNominativo" runat="server" CssClass="form-control" placeholder="Nominativo" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Accertatori --%>
                <div id="DivAccertatori" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label12" runat="server" Text="Accertatori" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicAccertatori" runat="server" CssClass="form-control" placeholder="Accertatori" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Indirizzo --%>
                <div id="DivIndirizzo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label13" runat="server" Text="Indirizzo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicIndirizzo" runat="server" CssClass="form-control" placeholder="Indirizzo" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA data carico --%>
                <div id="DivDataCarico" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label14" runat="server" Text="Data Carico Da" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatCaricoDa" runat="server" CssClass="form-control" placeholder="Data Inizio" />

                    <asp:Label ID="Label15" runat="server" Text="Data Carico A" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatCaricoA" runat="server" CssClass="form-control" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
            </div>
        </div>
        <%--  --%>

        <asp:Label ID="lblmessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
        <%-- campi nascosti --%>
        <asp:HiddenField ID="Holdmat" runat="server" />
        <asp:HiddenField ID="HolDate" runat="server" />
        <asp:HiddenField ID="HoldProtocollo" runat="server" />
        <%-- campi nascosti --%>
        <div class="container" id="DivDettagli" runat="server">


            <label for="txtProt">Nr Protocollo</label>
            <div class="row">
                <!-- Colonna Sinistra -->

                <div class="col-md-6">
                    <div class="form-group mb-3">

                        <asp:TextBox ID="txtProt" runat="server" CssClass="form-control1" ForeColor="Red" Font-Bold="true" />
                        <label for="Ddltipo">/</label>
                        <asp:DropDownList ID="DdlSigla" runat="server" CssClass="form-control1">
                            <asp:ListItem Text="ED"> </asp:ListItem>
                            <asp:ListItem Text="TP"> </asp:ListItem>
                            <asp:ListItem Text="PG"> </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtDataArrivo">Data Arrivo</label>
                        <asp:TextBox ID="txtDataArrivo" runat="server" CssClass="form-control" Font-Bold="true" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtGiudice">Giudice</label>
                        <asp:TextBox ID="txtGiudice" runat="server" AutoPostBack="false" onkeyup="filterDropdownGiudice()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsList" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            <asp:HiddenField ID="HfGiudice" runat="server" />
                        </div>
                        <asp:Button ID="btSalvaGiudice" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaGiudice_Click" Visible="false" />
                        <asp:DropDownList ID="DdlGiudice" runat="server" CssClass="form-control" Style="display: none" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="DdlTipoProvvAg">Tipo Provvedimento AG</label>
                        <asp:TextBox ID="txtTipoProv" runat="server" AutoPostBack="false" onkeyup="filterDropdownTipoProv()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListTipoProv" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            <asp:HiddenField ID="HfTipoProv" runat="server" />

                        </div>
                        <asp:Button ID="btSalvaTipoProvv" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaTipoProvv_Click" Visible="false" />

                        <asp:DropDownList ID="DdlTipoProvvAg" runat="server" CssClass="form-control" Style="display: none" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtAccertatori">Accertatori</label>
                        <asp:TextBox ID="txtAccertatori" runat="server" CssClass="form-control mb-3" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtQuartiere">Quartiere</label>
                        <asp:TextBox ID="txtQuartiere" runat="server" AutoPostBack="false" onkeyup="filterDropdownQuartiere()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListQuartiere" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                        </div>
                        <asp:DropDownList ID="DdlQuartiere" runat="server" CssClass="form-control" Style="display: none" />
                    </div>
                    <div class="form-group mb-3">
                        <%--<asp:Button ID="btinsProv" runat="server" Text="+" CssClass="btn btn-primary mt-3" OnClick="apripopupProvenienza_Click" />--%>
                        <label for="txtProvenienza">Provenienza</label>
                        <asp:TextBox ID="txtProvenienza" runat="server" AutoPostBack="false" onkeyup="filterDropdownProvenienza()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListProvenienza" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            <asp:HiddenField ID="HfProvenienza" runat="server" />
                        </div>
                        <asp:Button ID="btSalvaProvenienza" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaProvenienza_Click" Visible="false" />

                        <asp:DropDownList ID="DdlProvenienza" runat="server" CssClass="form-control" Style="display: none" />
                        <asp:RequiredFieldValidator ID="rqProvenienza" runat="server" ControlToValidate="DdlProvenienza" ErrorMessage="inserire la provenienza">

                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group mb-3" style="margin-top: -20px!important">
                        <label for="txtScaturito">Scaturito</label>

                        <!-- DropDownList occupa metà spazio -->
                        <div class="form-group mb-3">
                            <asp:TextBox ID="txtScaturito" runat="server" AutoPostBack="false" onkeyup="filterDropdownScaturito()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <div id="suggestionsListScaturito" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                <asp:HiddenField ID="HfScaturito" runat="server" />
                            </div>
                            <asp:Button ID="BtSalvaScaturito" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaScaturito_Click" Visible="false" />

                            <asp:DropDownList ID="DdlScaturito" runat="server" CssClass="form-control" Style="display: none" />
                        </div>
                    </div>
                </div>

                <!-- Colonna Destra -->
                <div class="form-group mb-3">
                    <div class="form-group mb-3" style="margin-top: 25px!important">
                        <label for="txtRifProtGen">Riferimento Prot. Gen.</label>
                        <asp:TextBox ID="txtRifProtGen" runat="server" CssClass="form-control" />

                    </div>

                    <!-- Indirizzo e TextBox sulla stessa riga -->
                    <div class="form-group mb-3">
                        <label for="txtIndirizzo">Indirizzo</label>
                        <asp:TextBox ID="txtIndirizzo" runat="server" AutoPostBack="false" onkeyup="filterDropdownIndirizzo()" Style="width: 400px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListIndirizzo" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            <asp:HiddenField ID="HfIndirizzo" runat="server" />
                        </div>
                        <asp:DropDownList ID="DdlIndirizzo" runat="server" CssClass="form-control" Style="display: none" />
                        <!-- TextBox occupa metà spazio -->
                        <%--<div class="col-md-6">
                                <asp:TextBox ID="txtVia" runat="server" CssClass="form-control" placeholder="specifica l'indirizzo" />
                            </div>--%>
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
                    <asp:TextBox ID="txtDataCarico" runat="server" CssClass="form-control mb-3" ClientIDMode="Static" />
                </div>
                <div class="form-group mb-3">
                    <label for="txtPratica">Pratica</label>
                    <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group mb-3">
                    <label for="txtTipoAttoR">Tipologia Atto</label>
                    <asp:TextBox ID="txtTipoAttoR" runat="server" AutoPostBack="false" onkeyup="filterDropdownTipoAtto()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    <div id="suggestionsListTipoAtto" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                        <asp:HiddenField ID="HfTipoAtto" runat="server" />
                    </div>
                    <asp:Button ID="btSalvaTipoAtto" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaTipoAtto_Click" Visible="false" />

                    <asp:DropDownList ID="DdlTipoAtto" runat="server" CssClass="form-control" Style="display: none" />
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
                        <asp:TextBox ID="txtDataDataEvasa" runat="server" CssClass="form-control" ClientIDMode="Static" />
                        <asp:RegularExpressionValidator
                            ID="RegularExpressionValidator2"
                            runat="server"
                            ControlToValidate="txtDataDataEvasa"
                            ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$"
                            ErrorMessage="la data deve essere dd/mm/aaaa"
                            ForeColor="Red"
                            ValidationGroup="bt"
                            Display="Static">
                        </asp:RegularExpressionValidator>

                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtInviata" class="form-label">Inviata</label>
                        <asp:TextBox ID="txtInviata" runat="server" AutoPostBack="false" onkeyup="filterDropdownInviata()" Style="width: 250px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListInviata" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            <asp:HiddenField ID="HfInviata" runat="server" />
                        </div>
                        <asp:Button ID="btSalvaInviata" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaInviata_Click" Visible="false" />

                        <asp:DropDownList ID="DdlInviati" runat="server" CssClass="form-control" Style="display: none" />

                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtDataInvio" class="form-label">Il</label>
                        <asp:TextBox ID="txtDataInvio" runat="server" CssClass="form-control" ClientIDMode="Static" />
                        <asp:RegularExpressionValidator
                            ID="RegularExpressionValidator1"
                            runat="server"
                            ControlToValidate="txtDataInvio"
                            ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$"
                            ErrorMessage="la data deve essere dd/mm/aaaa"
                            ForeColor="Red"
                            ValidationGroup="bt"
                            Display="Static">
                        </asp:RegularExpressionValidator>

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



    <%-- Modale ricerca pratica --%>
    <div class="modal fade" id="ModalPratica" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 100%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel10">Ricerca Pratica</h5>

                </div>
                <div class="modal-body">

                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <%--<div id="DivGrid" runat="server" visible="false" class="row">--%>

                        <!-- GridView nel popup -->
                        <asp:GridView ID="gvPopupProtocolli" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopupProtocolli_RowDataBound" OnRowCommand="gvPopupProtocolli_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvPopupProtocolli_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="Nr_Protocollo" HeaderText="Protocollo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="Sigla" HeaderText="Sigla" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Nominativo" HeaderText="Nominativo" ItemStyle-Wrap="true" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="Accertatori" HeaderText="Accertatori" ItemStyle-Wrap="true" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="Indirizzo" HeaderText="Indirizzo" ItemStyle-Wrap="true" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="Matricola" HeaderText="Matricola" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="DataInserimento" HeaderText="Data Inserimento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center" />
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


                        <%--</div>--%>
                    </div>
                </div>
                <asp:HiddenField ID="HidPratica" runat="server" />
                <asp:HiddenField ID="HfStato" runat="server" />
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopupPratica_Click" />
                </div>
            </div>
        </div>
    </div>




    <!-- Modale Bootstrap quartiere-->
    <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">Popup di Ricerca</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">
                        <label for="txtIndirizzoRic">Indirizzo:</label>
                        <asp:TextBox ID="txtIndirizzoRic" runat="server" CssClass="form-control" placeholder="Campo obbligatorio" />

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
                                <asp:BoundField DataField="ID_quartiere" HeaderText="ID" />
                                <asp:BoundField DataField="Toponimo" HeaderText="Toponimo" />
                                <asp:BoundField DataField="Quartiere" HeaderText="Quartiere" />
                                <asp:BoundField DataField="Specie" HeaderText="Specie" />
                                <asp:BoundField DataField="Nota" HeaderText="Nota" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
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

    <!-- Modale Bootstrap provenienza NON PIU' NECESSARIA-->
    <div class="modal fade" id="myModalProvenienza" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel1">Inserisci testo per la provenienza</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">
                        <label for="txtTestoProvenienza">Testo:</label>
                        <asp:TextBox ID="txtTestoProvenienza" runat="server" CssClass="form-control" placeholder="Inserisci una provenienza" />

                    </div>


                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btInsProvenienza" runat="server" CssClass="btn btn-primary" Text="Inserisci" OnClick="btInsProvenienza_Click" />
                    <asp:Button ID="Button3" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopupProvenienza_Click" />
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
