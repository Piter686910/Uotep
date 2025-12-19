<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Uote.test" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


<style>
    /* Reset e Font base */
    body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 13px; color: #333; }
    
    /* Contenitore con scroll orizzontale se necessario */
    .container { 
        width: 98%; margin: 10px auto; 
        overflow-x: auto; /* Permette lo scroll se i giorni sono tanti */
        background: #fff; padding: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); 
    }

    /* --- TABELLA --- */
    table.tabella-turni {
        border-collapse: separate; 
        border-spacing: 0;
        border: 1px solid #999;
        min-width: 100%;
    }

    /* Tutte le celle: bordi netti per effetto "Riquadro" */
    table.tabella-turni th, 
    table.tabella-turni td {
        border-right: 1px solid #ccc;
        border-bottom: 1px solid #ccc;
        padding: 4px 2px; /* Padding ridotto per compattare */
        text-align: center;
        vertical-align: middle;
    }

    /* --- COLONNA DIPENDENTE (Sticky a sinistra) --- */
    /* Header Dipendente */
    table.tabella-turni th.col-dip-header {
        position: sticky;
        left: 0;
        z-index: 20; /* Sopra tutto */
        background-color: #34495e;
        color: white;
        width: 140px;      /* Larghezza fissa ristretta */
        min-width: 140px;
        max-width: 140px;
        border-right: 2px solid #555; /* Separatore marcato */
    }

    /* Celle Nome Dipendente */
    table.tabella-turni td.col-dipendente {
        position: sticky;
        left: 0;
        z-index: 10; /* Sopra le celle normali */
        background-color: #fff; /* Necessario per coprire le celle che scorrono sotto */
        text-align: left !important;
        padding-left: 8px !important;
        
        /* Gestione testo lungo */
        width: 140px;
        min-width: 140px;
        max-width: 140px;
        white-space: nowrap;      /* Tutto su una riga */
        overflow: hidden;         /* Nascondi eccesso */
        text-overflow: ellipsis;  /* Aggiungi "..." se tagliato */
        
        font-weight: 600;
        font-size: 12px;          /* Font un po' più piccolo */
        color: #2c3e50;
        border-right: 2px solid #555; /* Separatore marcato */
    }

    .badge-q { 
        font-size: 10px; background: #eee; border: 1px solid #ccc; 
        color: #333; padding: 0 3px; border-radius: 3px; float: right; margin-right: 2px; 
    }

    /* --- GIORNI (Intestazioni) --- */
    table.tabella-turni th.giorno-header {
        position: sticky;
        top: 0;
        z-index: 5;
        background-color: #34495e; /* Blu scuro */
        color: white;
        height: 40px;
        width: 28px; /* Larghezza fissa per ogni giorno: quadrato */
        min-width: 28px;
    }
    .weekend-h { background-color: #c0392b !important; } /* Rosso header weekend */

    /* --- CELLE TURNI --- */
    /* Colori Sfondo */
    .t-1 { background-color: #e3f2fd; color: #0277bd; font-weight: bold; } 
    .t-2 { background-color: #fff8e1; color: #f57f17; font-weight: bold; } 
    
    /* LA Q: Deve risaltare nel giorno specifico */
    .t-q { 
        background-color: #ffcdd2; 
        color: #b71c1c; 
        font-weight: 900; 
        border: 2px solid #d32f2f !important; /* Riquadro rosso marcato interno */
    }

    /* Colonna weekend verticale (grigio chiaro) */
    .weekend-col { background-color: #f2f2f2; }

    /* Riga Ufficio */
    .tr-ufficio td {
        background-color: #636e72;
        color: white;
        text-align: left;
        padding: 5px 10px;
        font-size: 12px;
        font-weight: bold;
        letter-spacing: 1px;
    }
    .t-rf { 
    background-color: #c8e6c9; /* Verde pastello chiaro */
    color: #2e7d32;            /* Verde foresta scuro */
    font-weight: bold; 
    border: 2px solid #a5d6a7 !important; /* Bordo verde */
}

/* ... assicurati che le altre classi esistano ancora ... */
.t-q { background-color: #ffcdd2; color: #b71c1c; font-weight: 900; border: 2px solid #d32f2f !important; }
.t-1 { background-color: #e3f2fd; color: #0277bd; font-weight: bold; } 
.t-2 { background-color: #fff8e1; color: #f57f17; font-weight: bold; }
/* Intestazione della colonna percentuale */
th.col-stats-header {
    background-color: #444; /* Un grigio diverso per staccare */
    color: #fff;
    width: 50px;
    min-width: 50px;
    border-right: 2px solid #777;
    position: sticky;
    left: 140px; /* Deve essere uguale alla width della colonna dipendente */
    z-index: 20;
}

/* Cella del valore percentuale */
td.col-stats {
    background-color: #f9f9f9;
    font-weight: bold;
    color: #333;
    font-size: 11px;
    border-right: 2px solid #777;
    position: sticky;
    left: 140px; /* Si incolla dopo il nome */
    z-index: 10;
}
</style>


    <div>
        <label for="txtInput">Digita un nome:</label>
        <asp:TextBox ID="txtInput" runat="server" CssClass="form-control" AutoPostBack="false" onkeyup="filterDropdown()" Style="width: 200px;" ClientIDMode="Static"></asp:TextBox>
        <div id="suggestionsList" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
            <!-- Stili base per la lista suggerimenti -->
        </div>
       <div class="container">
            <h2>Gestione Turni Mensili</h2>
            
            <div class="controls">
                <asp:Label ID="lblAnno" runat="server" Text="Anno: "></asp:Label>
                <asp:TextBox ID="txtAnno" runat="server" Text="2024" Width="60"></asp:TextBox>
                
                <asp:Label ID="lblMese" runat="server" Text="Mese: "></asp:Label>
                <asp:DropDownList ID="ddlMese" runat="server">
                    <asp:ListItem Value="1">Gennaio</asp:ListItem>
                    <asp:ListItem Value="2">Febbraio</asp:ListItem>
                    <asp:ListItem Value="3">Marzo</asp:ListItem>
                    <asp:ListItem Value="4">Aprile</asp:ListItem>
                    <asp:ListItem Value="5">Maggio</asp:ListItem>
                    <asp:ListItem Value="6">Giugno</asp:ListItem>
                    <asp:ListItem Value="7">Luglio</asp:ListItem>
                    <asp:ListItem Value="8">Agosto</asp:ListItem>
                    <asp:ListItem Value="9">Settembre</asp:ListItem>
                    <asp:ListItem Value="10">Ottobre</asp:ListItem>
                    <asp:ListItem Value="11">Novembre</asp:ListItem>
                    <asp:ListItem Value="12">Dicembre</asp:ListItem>
                </asp:DropDownList>
                
                <asp:Button ID="btnCalcola" runat="server" Text="Elabora Turni" OnClick="btnCalcola_Click" />
                <asp:Button ID="btnSalva" runat="server" Text="💾 Salva Turni su DB" 
            OnClick="btnSalva_Click" CssClass="btn-save" />
                <asp:Label ID="lblError" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
            </div>

            <!-- Qui verrà iniettata la tabella HTML -->
            <asp:Literal ID="ltlTabella" runat="server"></asp:Literal>
        </div>
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



