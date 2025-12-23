<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Turnazione.aspx.cs" Inherits="Uotep.Turnazione" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        // Mostra il popup 
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        // Nasconde il popup
        function HideErrorMessage() {
            $('#errorModal').modal('hide');
        }

        function ricalcolaRiga(inputElement) {
            // 1. Trova la riga (TR) a cui appartiene l'input modificato
            var row = inputElement.closest('tr');

            // 2. Colleziona tutti gli input di quella riga
            var inputs = row.querySelectorAll('.shift-input');

            var count1 = 0;
            var count2 = 0;

            // 3. Conta le occorrenze
            inputs.forEach(function (inp) {
                var val = inp.value.trim().toUpperCase();
                if (val === '1') count1++;
                else if (val === '2') count2++;
            });

            // 4. Calcolo Matematico
            var totale = count1 + count2;
            var cellaPerc = row.querySelector('.col-stats'); // La cella percentuale

            if (totale > 0) {
                var perc = (count1 / totale) * 100;
                var percFixed = perc.toFixed(0); // Arrotonda (es 60)

                cellaPerc.innerText = percFixed + '%';

                // 5. Cambio Colore Dinamico
                if (percFixed < 50 || percFixed > 60) {
                    cellaPerc.style.color = 'red'; // Avviso fuori range
                } else {
                    cellaPerc.style.color = 'green'; // OK
                }
            } else {
                cellaPerc.innerText = 'N/A';
                cellaPerc.style.color = 'black';
            }

            // (Opzionale) Cambia colore della cella stessa in base al valore inserito
            var cella = inputElement.parentElement;
            cella.className = ''; // Reset classi
            if (inputElement.value.toUpperCase() === '1') cella.classList.add('t-1');
            else if (inputElement.value.toUpperCase() === '2') cella.classList.add('t-2');
            else if (inputElement.value.toUpperCase() === 'Q') cella.classList.add('t-q');
            else if (inputElement.value.toUpperCase() === 'RF') cella.classList.add('t-rf');
            // Re-aggiunge eventuale bordo se necessario, ma base colore funziona
        }
    </script>
    <style>
        /* Stile per l'intestazione della colonna (il giorno) */
        .giorno-festivo-header {
            background-color: #f7d5d7 !important; /* Rosso chiaro */
            color: #b70014; /* Testo Rosso */
            font-weight: bold;
            border-bottom: 2px solid #b70014;
        }

        /* Stile per le celle della griglia che corrispondono al giorno festivo/weekend */
        .giorno-festivo-cella {
            background-color: #ffeaea; /* Rosso chiarissimo */
        }

            /* Assicurati che le TextBox nelle celle modificate abbiano lo stesso sfondo festivo */
            .giorno-festivo-cella input[type="text"] {
                background-color: #ffeaea;
            }

        /* Stile aggiuntivo per mantenere il testo dell'header centrato */
        /*.text-center {
            text-align: center !important;
        }*/

        .gruppo-ufficio-header {
            background-color: #e9ecef; /* Un grigio chiaro, simile a thead */
            font-weight: bold;
            color: #495057;
            font-size: 1.1em;
            text-align: left; /* O 'center' se preferisci */
            padding-left: 10px;
        }

        .group-separator td {
            border-top: 2px solid #333 !important; /* Bordo superiore scuro e ben visibile */
        }

        .ufficio-separator-row td {
            background-color: #343a40; /* Un grigio più scuro e deciso */
            color: white;
            font-weight: bold;
            padding: 8px 12px; /* Un po' più di padding */
            text-transform: uppercase; /* Rende il titolo più evidente */
            font-size: 1em;
        }
        /*****/
        /* Reset e Font base */
        /*body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 13px;
            color: #333;
        }*/



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
                    width: 140px; /* Larghezza fissa ristretta */
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
                    white-space: nowrap; /* Tutto su una riga */
                    overflow: hidden; /* Nascondi eccesso */
                    text-overflow: ellipsis; /* Aggiungi "..." se tagliato */

                    font-weight: 600;
                    font-size: 12px; /* Font un po' più piccolo */
                    color: #2c3e50;
                    border-right: 2px solid #555; /* Separatore marcato */
                }

        .badge-q {
            font-size: 10px;
            background: #eee;
            border: 1px solid #ccc;
            color: #333;
            padding: 0 3px;
            border-radius: 3px;
            float: right;
            margin-right: 2px;
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

        .weekend-h {
            background-color: #c0392b !important;
        }
        /* Rosso header weekend */

        /* --- CELLE TURNI --- */
        /* Colori Sfondo */
        .t-1 {
            background-color: #e3f2fd;
            color: #0277bd;
            font-weight: bold;
        }

        .t-2 {
            background-color: #fff8e1;
            color: #f57f17;
            font-weight: bold;
        }

        /* LA Q: Deve risaltare nel giorno specifico */
        .t-q {
            background-color: #ffcdd2;
            color: #b71c1c;
            font-weight: 900;
            border: 2px solid #d32f2f !important; /* Riquadro rosso marcato interno */
        }

        /* Colonna weekend verticale (grigio chiaro) */
        .weekend-col {
            background-color: #f2f2f2;
        }

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
            color: #2e7d32; /* Verde foresta scuro */
            font-weight: bold;
            border: 2px solid #a5d6a7 !important; /* Bordo verde */
        }


        .t-q {
            background-color: #ffcdd2;
            color: #b71c1c;
            font-weight: 900;
            border: 2px solid #d32f2f !important;
        }

        .t-1 {
            background-color: #e3f2fd;
            color: #0277bd;
            font-weight: bold;
        }

        .t-2 {
            background-color: #fff8e1;
            color: #f57f17;
            font-weight: bold;
        }
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
        /* Stile per gli input modificabili dentro la cella */
        .shift-input {
            width: 100%;
            height: 100%;
            border: none;
            background: transparent;
            text-align: center;
            font-weight: bold;
            font-family: inherit;
            font-size: inherit;
            color: inherit; /* Prende il colore dalla classe CSS del genitore (es rosso per Q) */
            text-transform: uppercase; /* Forza maiuscolo */
            cursor: pointer;
        }

            /* Evidenzia la cella quando la modifichi */
            .shift-input:focus {
                background-color: #ffffcc;
                outline: 2px solid #007bff;
            }

        /* La colonna percentuale che aggiorneremo via JS */
        .col-stats {
            font-weight: bold;
            transition: color 0.3s;
        }

        .btn-load {
            background-color: #17a2b8;
            color: white;
            border: none;
            padding: 5px 15px;
            cursor: pointer;
            margin-left: 10px;
        }

            .btn-load:hover {
                background-color: #138496;
            }

        .btn-excel {
            background-color: #217346;
            color: white;
            border: none;
            padding: 5px 15px;
            cursor: pointer;
            margin-left: 10px;
        }

            .btn-excel:hover {
                background-color: #1e6b41;
            }

        .btn-pdf {
            background-color: #dc3545;
            color: white;
            border: none;
            padding: 5px 15px;
            cursor: pointer;
            margin-left: 10px;
        }

            .btn-pdf:hover {
                background-color: #c82333;
            }

      
            /***************/
    </style>

    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">TURNAZIONE PER IL MESE DI</p>
        </div>

        <%-- MODIFICA: Utilizza container-fluid per occupare l'intera larghezza disponibile e rimuovi il margine negativo --%>
        <div class="container-fluid">
            <asp:Literal ID="ltlDebug" runat="server" EnableViewState="false"></asp:Literal>
            <!-- GridView  -->
            <asp:UpdatePanel ID="updPanelGrid" runat="server">

                <ContentTemplate>
                    <%-- Aggiunto un div class="row" per un corretto allineamento dei controlli --%>
                    <div class="row">
                        <div class="col-md-4" style="margin-bottom: 10px; margin-top: 20px; padding-left: 2em">
                            <asp:Label ID="lblMese" runat="server" Text="Seleziona Mese/Anno: "></asp:Label>
                            <asp:DropDownList ID="ddlMese" runat="server" CssClass="form-control">
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
                        </div>
                        <div class="col-md-3" style="margin-bottom: 10px; margin-top: 40px; padding-left: 2em">
                            <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control"></asp:TextBox>

                        </div>
                        <div class="col-md-3" style="margin-bottom: 10px; margin-top: 40px; padding-left: 2em">
                            <asp:Button ID="btnCarica" runat="server" Text="Calcola Turni" CssClass="btn btn-primary" OnClick="btnCarica_Click" />
                        </div>
                        <div class="col-md-3" style="margin-bottom: 10px; margin-top: 40px; padding-left: 2em">
                            <asp:Button ID="btnsalva" runat="server" Text="💾 Salva Turni su DB" CssClass="btn btn-primary" OnClick="btnsalva_Click1" Enabled="false" />
                        </div>
                        <div class="col-md-3" style="margin-bottom: 10px; margin-top: 40px; padding-left: 2em">
                            <asp:Button ID="btGetTurnoMensile" runat="server" Text="📂 Ricerca da DB" CssClass="btn btn-primary" OnClick="btGetTurnoMensile_Click" />
                        </div>
                        <div class="col-md-3" style="margin-bottom: 10px; margin-top: 40px; padding-left: 2em">
                            <asp:Button ID="btnExportExcel" runat="server" Text="📊 Esporta Excel"
                                OnClick="btnExportExcel_Click" CssClass="btn-excel" />
                        </div>
                        <div class="col-md-3" style="margin-bottom: 10px; margin-top: 40px; padding-left: 2em">
                            <asp:Button ID="btnExportPdf" runat="server" Text="📄 Stampa PDF"
                                OnClick="btnExportPdf_Click" CssClass="btn-pdf" />
                        </div>




                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <asp:Literal ID="ltlTabella" runat="server"></asp:Literal>
                </ContentTemplate>
                <Triggers>
                    <%-- AGGIUNGI QUESTA RIGA: Forza il pulsante a ricaricare l'intera pagina --%>
                    <asp:PostBackTrigger ControlID="btnExportPdf" />
                    <asp:PostBackTrigger ControlID="btnExportExcel" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <%-- popup errori --%>
    <div class="modal fade" id="errorModal" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">ATTENZIONE</h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <p id="errorMessage" runat="server" style="color: red"></p>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btClose" runat="server" class="btn btn-secondary" Text="Chiudi" OnClientClick="HideErrorMessage()" />
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        //dice alla masterpage di trasformare il container in container-fluid
        $(document).ready(function () {
            // Cerca il container genitore (solitamente ha la classe .container e .body-content)
            // e sostituisce la classe 'container' con 'container-fluid'
            var mainContainer = $('#<%= updPanelGrid.ClientID %>').closest('.container');
            if (mainContainer.length > 0) {
                mainContainer.removeClass('container').addClass('container-fluid');
            }
        });
    </script>
</asp:Content>
