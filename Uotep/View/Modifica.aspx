﻿<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Modifica.aspx.cs" Inherits="Uotep.Modifica" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function showModal() {
            $('#ModalDataEvasa').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalDataEvasa').modal('hide');

        }
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        //function openPopup() {
        //    document.getElementById("popupModal").style.display = "block";
        //    document.getElementById("overlay").style.display = "block";
        //}
        // Mostra il popup
        function showModal() {
            $('#ModalQuartiere').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalQuartiere').modal('hide');
        }
        // Mostra il popup ricerca
        function showModal() {
            $('#ModalRicerca').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalRicerca').modal('hide');
        }
        // Mostra il popup ricerca
        function showModal() {
            $('#ModalAvvertenze').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalAvvertenze').modal('hide');
        }

        // Mostra il popup Decretazione
        function showModalDecretazione() {
            $('#ModalDecretazione').modal('show');
        }

        // Nasconde il popup
        function hideModalDecretazione() {
            $('#ModalDecretazione').modal('hide');
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
        //Inviata
        <%--function filterDropdownInviata() {
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
        }--%>

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
        //ESITO
        function filterDropdownEsito() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtEsito");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlEsito.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListEsito.ClientID %>');
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
        //document.addEventListener('DOMContentLoaded', function () {
        //    var textBox = document.getElementById('txtDataInvio');


        //    if (textBox) {
        //        textBox.addEventListener('input', function (event) {
        //            // Salva la posizione attuale del cursore
        //            var cursorPos = this.selectionStart;
        //            var originalLength = this.value.length;

        //            // Sostituisci tutti gli spazi con trattini
        //            this.value = this.value.replace(/ /g, '/');

        //            // Se la lunghezza è cambiata (cioè uno spazio è stato sostituito),
        //            // e se l'ultimo carattere digitato era uno spazio (ora un trattino),
        //            // riposiziona il cursore.
        //            // Questa logica semplice funziona bene per sostituzioni 1 a 1.
        //            if (this.value.length === originalLength) {
        //                this.setSelectionRange(cursorPos, cursorPos);
        //            } else {
        //                // Se più spazi sono stati sostituiti o incollati,
        //                // il cursore potrebbe andare alla fine.
        //                // Per la semplice digitazione di uno spazio,
        //                // cursorPos dovrebbe essere corretto.
        //                this.setSelectionRange(cursorPos, cursorPos);
        //            }
        //        });
        //    } else {
        //        console.error("Textbox con ID 'txtDataInvio' non trovata.");
        //    }
        //});
        //document.addEventListener('DOMContentLoaded', function () {
        //    var textBox = document.getElementById('txtDataDataEvasa');


        //    if (textBox) {
        //        textBox.addEventListener('input', function (event) {
        //            // Salva la posizione attuale del cursore
        //            var cursorPos = this.selectionStart;
        //            var originalLength = this.value.length;

        //            // Sostituisci tutti gli spazi con trattini
        //            this.value = this.value.replace(/ /g, '/');

        //            // Se la lunghezza è cambiata (cioè uno spazio è stato sostituito),
        //            // e se l'ultimo carattere digitato era uno spazio (ora un trattino),
        //            // riposiziona il cursore.
        //            // Questa logica semplice funziona bene per sostituzioni 1 a 1.
        //            if (this.value.length === originalLength) {
        //                this.setSelectionRange(cursorPos, cursorPos);
        //            } else {
        //                // Se più spazi sono stati sostituiti o incollati,
        //                // il cursore potrebbe andare alla fine.
        //                // Per la semplice digitazione di uno spazio,
        //                // cursorPos dovrebbe essere corretto.
        //                this.setSelectionRange(cursorPos, cursorPos);
        //            }
        //        });
        //    } else {
        //        console.error("Textbox con ID 'txtDataDataEvasa' non trovata.");
        //    }
        //});

        //rende visibile un textbox su onclientclick
       <%-- function TextBoxVisibility1(buttonReference) {
        // Otteniamo un riferimento alla TextBox usando il suo ClientID generato da ASP.NET
            // `<%= div1.ClientID %>` inserisce l'ID reale della TextBox nell'HTML
            var textBox = document.getElementById('<%= div1.ClientID %>');

            if (textBox) {
                if (textBox.style.visibility === 'hidden') {
                    // Se la TextBox è nascosta, la rendiamo visibile
                    textBox.style.visibility = 'visible'; // O 'inline', 'inline-block' a seconda del contesto
                    //buttonReference.value = 'Nascondi Textbox'; // Cambia il testo del bottone
                } else {
                    // Se la TextBox è visibile, la nascondiamo
                    textBox.style.display = 'none';
                    //buttonReference.value = 'Mostra Textbox'; // Cambia il testo del bottone
                }
            }

            // Restituire 'false' impedisce il postback al server.
            // Se non restituisci 'false', il click attiverà ANCHE l'evento OnClick lato server.
            return false;
        }--%>
        //tipo atto
        function filterDropdownTipoAtto() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtTipoAtto");
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

        .gridview-autofit .colonna-stretta {
            white-space: nowrap;
            width: 1%;
            /* Opzionale: aggiungi un po' di padding per la leggibilità */
            padding: 8px 10px;
        }

        .gridview-autofit .colonna-descrizione {
            min-width: 250px; /* Non sarà mai più stretta di 100px */
            max-width: 500px;
            /* Se il testo è più lungo, andrà a capo */
            /* Non usiamo nowrap qui, vogliamo che vada a capo se necessario */
        }
    </style>
    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">MODIFICA CARICO</p>


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
                        <asp:Button ID="btDataArrivo" runat="server" OnClick="btDataArrivo_Click" Text="Data Inserimento" ToolTip="Ricerca Data Inserimento" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btAccertatori" runat="server" OnClick="btAccertatori_Click" Text="Accertatori" ToolTip="Ricerca Accertatori" CssClass="btn btn-primary mx-2" />
                        <asp:Button ID="btIndirizzo" runat="server" OnClick="btIndirizzo_Click" Text="Indirizzo" ToolTip="Ricerca Per Indirizzo" CssClass="btn btn-primary mx-2" />
                    </p>
                </div>

            </asp:Panel>
        </div>
        <%-- < SEZIONE PANNELLI RICERCA --%>
        <div id="DivRicerca" runat="server" class="row d-flex justify-content-center align-items-center vh-100" style="height: 300px; margin-left: 400px!important">
            <!-- Righe di input  -->
            <div class="col-md-4 custom-border">
                <%-- DIV RICERCA PROTOCOLLO --%>
                <div id="DivProtocollo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

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
                <div id="DivProcPenale" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label2" runat="server" Text="Nr Procediemnto Penale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProcPenale" runat="server" CssClass="form-control" placeholder="Nr Procedimento Penale" />



                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA evasa ag --%>
                <div id="DivEvasaAg" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label3" runat="server" Text="Data Inizio Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataDa" runat="server" CssClass="form-control" placeholder="Data Inizio" />

                    <asp:Label ID="Label4" runat="server" Text="Data Fine Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataA" runat="server" CssClass="form-control" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA rif protocollo generale --%>
                <div id="DivProtGen" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label5" runat="server" Text="Rif. Protocollo Generale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProtGen" runat="server" CssClass="form-control" placeholder="Rif. Prot. Gen." />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA pratica --%>
                <div id="DivPratica" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label6" runat="server" Text="Nr. Pratica" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" placeholder="Nr. Pratica" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA giudice --%>
                <div id="DivGiudice" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label7" runat="server" Text="Giudice" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicGiudice" runat="server" CssClass="form-control" placeholder="Giudice" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA provenienza --%>
                <div id="DivProvenienza" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label8" runat="server" Text="Provenienza" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicProvenienza" runat="server" CssClass="form-control" placeholder="Provenienza" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA nominativo --%>
                <div id="DivNominativo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label9" runat="server" Text="Nominativo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicNominativo" runat="server" CssClass="form-control" placeholder="Nominativo" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Accertatori --%>
                <div id="DivAccertatori" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label10" runat="server" Text="Accertatori" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicAccertatori" runat="server" CssClass="form-control" placeholder="Accertatori" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Indirizzo --%>
                <div id="DivIndirizzo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label11" runat="server" Text="Indirizzo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicIndirizzo" runat="server" CssClass="form-control" placeholder="Indirizzo" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA data carico --%>
                <div id="DivDataArrivo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label12" runat="server" Text="Data Inserimento Da" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatArrivoDa" runat="server" CssClass="form-control" placeholder="Data Inizio" />

                    <asp:Label ID="Label13" runat="server" Text="Data Inserimento A" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatArrivoA" runat="server" CssClass="form-control" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
            </div>
        </div>
        <%--  --%>
        <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
        <div class="container" id="DivDettagli" runat="server">

            <div class="tab-content">
                <p style="font-weight: bold; font-size: medium">Dati Generali</p>
                <div class="row custom-border">
                    <div class="col-md-4 ">
                        <div class="form-group mb-3" style="margin-left: -25px; margin-top: 30px">
                            <label for="txtProt">Nr Carico</label>
                            <asp:TextBox ID="txtProt" runat="server" CssClass="form-control1" ForeColor="Red" Enabled="false" Font-Bold="true" />

                            <%--<asp:TextBox ID="txtSigla" runat="server" CssClass="form-control mb-3" Enabled="false" />--%>
                            <label for="Ddltipo">/</label>
                            <asp:DropDownList ID="DdlSigla" runat="server" CssClass="form-control1" OnSelectedIndexChanged="DdlSigla_SelectedIndexChanged" AutoPostBack="true">
                                <%--<asp:ListItem Text="ED"> </asp:ListItem>
                                <asp:ListItem Text="TP"> </asp:ListItem>
                                <asp:ListItem Text="AG"> </asp:ListItem>--%>
                            </asp:DropDownList>
                           
                        </div>
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtTipoAtto">Tipologia Atto</label>
                            <asp:TextBox ID="txtTipoAtto" runat="server" AutoPostBack="false" onkeyup="filterDropdownTipoAtto()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <div id="suggestionsListTipoAtto" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                <asp:HiddenField ID="HfTipoAtto" runat="server" />
                            </div>
                            <%--<asp:Button ID="btSalvaTipoAtto" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaTipoAtto_Click" Visible="false" />--%>
                            <asp:DropDownList ID="DdlTipoAtto" runat="server" CssClass="form-control" Style="display: none" />
                        </div>
                    </div>
                    <%-- seconda colonna --%>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtDataInsCarico">Data Inserimento</label>
                            <asp:TextBox ID="txtDataInsCarico" runat="server" CssClass="form-control" Font-Bold="true" />


                        </div>
                        <div class="form-group mb-3">
                            <label for="txtProvenienza">Provenienza</label>
                            <asp:TextBox ID="txtProvenienza" runat="server" AutoPostBack="false" onkeyup="filterDropdownProvenienza()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <div id="suggestionsListProvenienza" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                <asp:HiddenField ID="HfProvenienza" runat="server" />
                            </div>
                            <asp:DropDownList ID="DdlProvenienza" runat="server" CssClass="form-control" Style="display: none" />

                        </div>

                    </div>
                    <%-- terza colonna --%>
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtRifProtGen">Protocollo Generale</label>
                            <asp:TextBox ID="txtRifProtGen" runat="server" CssClass="form-control mb-3" />

                        </div>
                    </div>

                </div>
                <p style="font-weight: bold; font-size: medium">Dati Relativi Alla Pratica</p>
                <div class="row custom-border">
                    <div class="col-md-4">
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtIndirizzo">Indirizzo</label>
                            <asp:TextBox ID="txtIndirizzo" runat="server" AutoPostBack="false" onkeyup="filterDropdownIndirizzo()" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <div id="suggestionsListIndirizzo" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                <asp:HiddenField ID="HfIndirizzo" runat="server" />
                            </div>
                            <asp:DropDownList ID="DdlIndirizzo" runat="server" CssClass="form-control" Style="display: none" />

                        </div>

                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txPratica">Pratica</label>
                            <asp:TextBox ID="txPratica" runat="server" CssClass="form-control mb-3" />

                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtQuartiere">Quartiere</label>
                            <asp:TextBox ID="txtQuartiere" runat="server" AutoPostBack="false" onkeyup="filterDropdownQuartiere()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <div id="suggestionsListQuartiere" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                <asp:HiddenField ID="HfQuartiere" runat="server" />
                            </div>
                            <asp:DropDownList ID="DdlQuartiere" runat="server" CssClass="form-control" Style="display: none" />

                        </div>
                        <div class="form-group mb-3">
                            <label for="txtAreaCompetenza">Area Competenza</label>
                            <asp:TextBox ID="txtAreaCompetenza" runat="server" CssClass="form-control mb-3" />


                            <%--<div id="bt1" runat="server" class="col-md-3" style="margin-top: 25px!important; margin-left: -15px!important">--%>

                            <%--<asp:Button ID="Add" Text="+" runat="server" ToolTip="Aggiungi accertatore 2" CssClass="btn btn-primary mt-3" OnClick="Add_Click" />--%>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtNominativo">Nominativo</label>
                            <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control mb-3" />


                        </div>
                        <div class="form-group mb-3">
                            <label for="txtDataCarico">Data Carico</label>
                            <asp:TextBox ID="txtDataCarico" runat="server" CssClass="form-control mb-3" ClientIDMode="Static" />
                        </div>

                    </div>


                </div>

                <p style="font-weight: bold; font-size: medium">Esito Accertamento</p>
                <div class="row custom-border">
                    <div class="col-md-4">
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtDataEsito">Data Esito</label>
                            <asp:TextBox ID="TxtDataEsito" runat="server" CssClass="form-control mb-3" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtEsito">Esito</label>

                            <div class="form-group mb-3">
                                <asp:TextBox ID="txtEsito" runat="server" AutoPostBack="false" onkeyup="filterDropdownEsito()" Style="width: 250px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                <div id="suggestionsListEsito" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                    <asp:HiddenField ID="HfEsito" runat="server" />
                                </div>
                                <asp:DropDownList ID="DdlEsito" runat="server" CssClass="form-control" Style="display: none" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtAccertatori">Accertatori</label>
                            <asp:TextBox ID="txtAccertatori" runat="server" CssClass="form-control mb-3" TextMode="MultiLine" Rows="2" Style="width: 100%; max-width: 600px;" />

                        </div>
                    </div>

                </div>
                <div id="divAg" runat="server" visible="false">

                    <p style="font-weight: bold; font-size: medium">Dati AG</p>
                    <div class="row custom-border">
                        <div class="col-md-4">
                            <div class="form-group mb-3" style="margin-left: -25px">
                                <label for="txtGiudice">Giudice</label>
                                <asp:TextBox ID="txtGiudice" runat="server" CssClass="form-control mb-3" />

                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group mb-3">
                                <label for="TxtTipoProvvAg">Tipo Provvedimento AG</label>
                                <asp:TextBox ID="TxtTipoProvvAg" runat="server" CssClass="form-control mb-3" AutoPostBack="false" onkeyup="filterDropdownTipoProv()" Style="width: 300px;" ClientIDMode="Static" Visible="false"></asp:TextBox>

                                <div id="suggestionsListTipoProv" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                    <asp:HiddenField ID="HfTipoProv" runat="server" />

                                </div>
                                <%--                                <asp:Button ID="btSalvaTipoProvv" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaTipoProvv_Click" Visible="false" />--%>
                                <asp:DropDownList ID="DdlTipoProvvAg" runat="server" CssClass="form-control" OnSelectedIndexChanged="DdlTipoProvvAg_SelectedIndexChanged" AutoPostBack="true" />

                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-3">
                                <label for="txtProdPenNr">Procedimento Penale nr</label>
                                <asp:TextBox ID="txtProdPenNr" runat="server" CssClass="form-control mb-3" />

                            </div>
                        </div>
                        <%--                        <div class="col-md-3" style="margin-top: -25px">
                            <div class="form-check">
                                <asp:CheckBox ID="CkEvasa" runat="server" CssClass="form-check-input" Enabled="false" />
                                <label class="form-check-label ms-2" for="CkEvasa">Evasa</label>
                            </div>
                        </div>--%>
                    </div>
                </div>


            </div>


            <div class="row">
                <div class="col-12 text-center">
                    <%-- <asp:Button Text="Nuova Ricerca" runat="server" OnClick="NuovaRicerca_Click" ToolTip="Nuova Ricerca" CssClass="btn btn-primary mt-3" />--%>
                    <asp:Button ID="btSalva" Text="Salva" runat="server" OnClick="Salva_Click" ToolTip="salva" CssClass="btn btn-primary mt-3" />
                    <asp:Button ID="btCercaQuartiere" Text="Cerca Quartiere" runat="server" OnClick="apripopup_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" />
                    <asp:Button Text="Decretazione" runat="server" OnClick="Decretazione_Click" ToolTip="Decrertazione" CssClass="btn btn-primary mt-3" />

                </div>
            </div>

        </div>
    </div>



    <!-- Modale Bootstrap quartiere -->

    <div class="modal fade" id="ModalQuartiere" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel1">Ricerca quartiere</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">
                        <label for="txtIndirizzoQuartiere">Indirizzo:</label>
                        <asp:TextBox ID="txtIndirizzoQuartiere" runat="server" CssClass="form-control" placeholder="Campo obbligatorio" />

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
                                        <asp:Button ID="Button1" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("Quartiere") %>' CssClass="btn btn-success btn-sm" />
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

    <%-- Modale ricerca fascicolo --%>
    <div class="modal fade" id="ModalRicerca" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 100%">
            <%--<div class="container" id="DivDettagli" runat="server">--%>
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel2">Ricerca Fascicolo</h5>

                </div>
                <div id="DivGrid" runat="server" visible="false" class="row" style="padding-left: 10px">
                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="gvPopupD" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopupD_RowDataBound" OnRowCommand="gvPopupD_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvPopupD_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="Nr_Protocollo" HeaderText="Nr. Carico" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="Sigla" HeaderText="Sigla" ItemStyle-Width="20px" Visible="false" />
                                <asp:BoundField DataField="nr_Pratica" HeaderText="N. Pratica" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />

                                <%--                                <asp:BoundField DataField="Nominativo" HeaderText="Nominativo" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>--%>

                                <asp:TemplateField HeaderText="Nominativo" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                    <HeaderTemplate>
                                        Nominativo
                                          <br />
                                        <asp:TextBox ID="txtFilterNominativo" runat="server" OnTextChanged="txtFilterNominativo_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        Filtro
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Nominativo") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:BoundField DataField="Indirizzo" HeaderText="Indirizzo" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>--%>

                                <asp:TemplateField HeaderText="Indirizzo" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                    <HeaderTemplate>
                                        Indirizzo
                                          <br />
                                        <asp:TextBox ID="txtFilterIndirizzo" runat="server" OnTextChanged="txtFilterIndirizzo_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        Filtro
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("indirizzo") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="ProcedimentoPen" HeaderText="Proc. Penale" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Tipologia_atto" HeaderText="Tipologia Atto" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Accertatori" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                    <HeaderTemplate>
                                        Accertatori
                                          <br />
                                        <asp:TextBox ID="txtFilterAccertatori" runat="server" OnTextChanged="txtFilterAccertatori_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        Filtro
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Accertatori") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- <asp:BoundField DataField="Accertatori" HeaderText="Accertatori" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>--%>
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
                <%-- campi nascosti --%>
                <asp:HiddenField ID="Holdmat" runat="server" />
                <asp:HiddenField ID="HolDate" runat="server" />
                <asp:HiddenField ID="Hid" runat="server" />
                <asp:HiddenField ID="HfFiltroNote" runat="server" />
                <asp:HiddenField ID="HfFiltroIndirizzo" runat="server" />
                <asp:HiddenField ID="HfFiltroNominativo" runat="server" />
                <asp:HiddenField ID="HfFiltroAccertatori" runat="server" />

                <%-- campi nascosti --%>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                </div>
            </div>
        </div>
    </div>

    <%-- popup avvertenze --%>
<div class="modal fade" id="ModalAvvertenze" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-hidden="true">
    <div class="modal-dialog"
        role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modalLabel6">ATTENZIONE</h5>

            </div>
            <div class="modal-body">
                <!-- Campi di input per la ricerca -->
                <div class="form-group">

                    <p id="errorAvvertenze" style="color: red"></p>

                </div>
            </div>
            <div class="modal-footer">
                <!-- Bottone per avviare la ricerca -->
                <asp:Button ID="btChiudiAvvertenze" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="btChiudiAvvertenze_Click" />
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
    <%-- Modale ModalDecretazione --%>
    <div class="modal fade" id="ModalDecretazione" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 100%">
            <%--<div class="container" id="DivDettagli" runat="server">--%>
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel3">Decretazione</h5>

                </div>
                <div id="DivDecretazione" runat="server">
                    <div class="row custom-border" style="margin-left: 0px!important">
                        <div class="col-md-3 " style="margin-left: 20px!important">
                            <div class="form-group mb-3">
                                <label for="txtPraticaDecr">Pratica</label>
                                <asp:TextBox ID="txtPraticaDecr" runat="server" Enabled="false" CssClass="form-control mb-3" Width="120px"></asp:TextBox>
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDecretante">Decretante</label>
                                <asp:TextBox ID="txtDecretante" runat="server" CssClass="form-control mb-3"></asp:TextBox>
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDecretato">Decretato</label>
                                <asp:TextBox ID="txtDecretato" runat="server" CssClass="form-control mb-3"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RfDecretato" runat="server" ControlToValidate="txtDecretato" ValidationGroup="btDecretazione" ErrorMessage="Inserire decretato" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDataDecretazione">Data</label>
                                <asp:TextBox ID="txtDataDecretazione" runat="server" CssClass="form-control mb-3"></asp:TextBox>

                            </div>
                            <div class="form-group mb-3">
                                <label for="txtNotaDecretazione">Nota</label>
                                <asp:TextBox ID="txtNotaDecretazione" runat="server" CssClass="form-control mb-3" TextMode="MultiLine" Rows="12" Style="width: 100%; max-width: 600px;"></asp:TextBox>
                            </div>

                            <div class="form-group mb-3">
                                <asp:Button ID="btAggiungiDecretazione" runat="server" CssClass="btn btn-primary mt-3" Text="Aggiungi" OnClick="btAggiungiDecretazione_Click" ValidationGroup="btDecretazione" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <div class="form-group">
                                    <!-- GridView nel popup -->
                                    <asp:GridView ID="GVDecretazione" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered gridview-autofit"
                                        OnRowDataBound="GVDecretazione_RowDataBound" OnRowCommand="GVDecretazione_RowCommand" AllowPaging="true" PageSize="5" OnPageIndexChanging="GVDecretazione_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="decr_id" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="decr_idPratica" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="decr_pratica" HeaderText="Pratica" Visible="false" />
                                            <asp:BoundField DataField="decr_decretante" HeaderText="Decretante">
                                                <HeaderStyle CssClass="colonna-descrizione" />
                                                <ItemStyle CssClass="uppercase-text" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="decr_data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />

                                            <asp:BoundField DataField="decr_decretato" HeaderText="Decretato">
                                                <HeaderStyle CssClass="colonna-descrizione" />
                                                <ItemStyle CssClass="uppercase-text" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="decr_nota" HeaderText="Nota">
                                                <HeaderStyle CssClass="colonna-descrizione" />
                                                <ItemStyle CssClass="uppercase-text colonna-descrizione" />
                                            </asp:BoundField>
                                            <%--<asp:BoundField DataField="decr_dataChiusura" HeaderText="Data Chiusura" DataFormatString="{0:dd/MM/yyyy}" />--%>

                                            <asp:TemplateField HeaderText="Data Chiusura">
                                                <ItemTemplate>
                                                    <%# FormatMyDate(Eval("decr_dataChiusura")) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="Button1" runat="server" Text="Modifica" CommandName="Select" CommandArgument='<%# Eval("decr_id") + "|" + Eval("decr_decretato") + "|" + Eval("decr_data") + "|" + Eval("decr_nota")%>' CssClass="btn btn-success btn-sm" />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="Button4" runat="server" Text="Salva" CommandName="Save" CommandArgument='<%# Eval("decr_id") + "|" + Eval("decr_decretato") + "|" + Eval("decr_data") + "|" + Eval("decr_nota")%>' CssClass="btn btn-success btn-sm" />

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
                                    <asp:Button ID="btChiudiDecretazione" runat="server" Text="Chiudi Decretazione" OnClick="btChiudiDecretazione_Click" CommandName="Select" CommandArgument='<%# Eval("decr_pratica") + "|" + Eval("decr_idPratica") %>' CssClass="btn btn-success btn-sm" />

                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="Button3" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopupDecretazione_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- Popup Modale inseriemnto data evasa -->
    <div class="modal fade" id="ModalDataEvasa" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 20%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel4">Inserisci La Data Evasa</h5>

                </div>
                <div id="Div3" runat="server" class="row" style="margin-left: 30px!important">
                    <div class="form-group mb-3">
                        <label for="txtdataEvasaPopup">Data Evasa</label>
                        <asp:TextBox ID="txtdataEvasaPopup" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>

                <div class="modal-footer">
                    <!-- Bottone per avviare chiousura decretazione -->
                    <asp:Button ID="ModalChiudiDecretazione" runat="server" class="btn btn-secondary" Text="Salva" OnClick="ModalChiudiDecretazione_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
