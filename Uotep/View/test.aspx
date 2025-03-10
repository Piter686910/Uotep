<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Uote.test" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <style>
        #suggestionsList {
            position: absolute;
            background-color: white;
            border: 1px solid #ccc;
            width: 200px;
            display: none;
            z-index: 1000;
        }

            #suggestionsList ul {
                list-style-type: none;
                padding: 0;
                margin: 0;
            }

            #suggestionsList li {
                padding: 5px;
                cursor: pointer;
            }

                #suggestionsList li:hover {
                    background-color: #f0f0f0;
                }
    </style>


    <div>
        <label for="txtInput">Digita un nome:</label>
<asp:TextBox ID="txtInput" runat="server" CssClass="form-control" AutoPostBack="false" onkeyup="filterDropdown()" style="width: 200px;" ClientIDMode="Static"></asp:TextBox>
<div id="suggestionsList" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;"> <!-- Stili base per la lista suggerimenti -->
</div>
    <div class="form-group mb-3">
    <label for="DdlGiudice">Giudice</label>
    <asp:DropDownList ID="DdlGiudice" runat="server" CssClass="form-control" />

</div>
<script type="text/javascript">
    function filterDropdown() {
        var input, filter, dropdown, options, i, txtValue;
        input = document.getElementById("txtInput");
        filter = input.value.toUpperCase();
        dropdown = document.getElementById("MainContent_DdlGiudice");
        options = dropdown.getElementsByTagName("option");
        var suggestionsListDiv = document.getElementById("MainContent_suggestionsList");

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

                //suggestionElement.onclick = function () { // Al click, inserisci il testo nel textbox

                //    console.log("Suggerimento selezionato:", this.textContent); // DEBUG: Verifica il suggerimento cliccato
                //    console.log("Elemento input:", input); // DEBUG: Verifica l'elemento input

                //    input.value = this.textContent; // Imposta il valore nel textbox

                //    console.log("Valore textbox dopo impostazione:", input.value); // DEBUG: Verifica il valore impostato

                //    suggestionsListDiv.style.display = "none"; // Nascondi la lista suggerimenti

                //    // Importante: Previeni l'Autopostback immediato (se è questo il problema)
                //    return false; // Aggiungi per prevenire l'autopostback se interferisce
                //};

                suggestionElement.addEventListener('click', function () {
                    console.log("Funzione addEventListener CLICK ESEGUITA per:", this.textContent); // DEBUG: Verifica addEventListener
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

</asp:Content>



