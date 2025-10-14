<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InserimentoArchivioUotp.aspx.cs" Inherits="Uotep.InserimentoArchivioUotp" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        // Attendere che il DOM sia completamente caricato
        document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('TxtSub');
        // Se non usi ClientIDMode="Static", dovresti usare:
            // var textBox = document.getElementById('<%= TxtSub.ClientID %>');

            if (textBox) {
                textBox.addEventListener('input', function (event) {
                    // Salva la posizione attuale del cursore
                    var cursorPos = this.selectionStart;
                    var originalLength = this.value.length;

                    // Sostituisci tutti gli spazi con trattini
                    this.value = this.value.replace(/ /g, '-');

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
                console.error("Textbox con ID 'txtsub' non trovata.");
            }
        });







        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        function HideErrorMessage(message) {
            $('#errorModal').modal('hide');
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
        //popup per la conferma inserimento
        $(document).ready(function () {
            $('#btnModalOk').click(function () {
                document.getElementById('<%=hdnConfermaUtente.ClientID%>').value = 'true';
                __doPostBack('<%=btSalva.UniqueID%>', ''); // Forza PostBack
                $('#confermaModal').modal('hide'); // Chiudi il modale dopo aver gestito l'OK
            });
        });



        //giudice
        function filterDropdownGiudice() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtGiudice");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlGiudiceI.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListG.ClientID %>');
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
        //quartiere
        function filterDropdownQuartiere() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtQuartiere");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlQuartiereI.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListQ.ClientID %>');
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
       <%-- function filterDropdownTipoAtto() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtTipoAtto");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlTipoAttoI.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListTA.ClientID %>');
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
        //Inviata
        function filterDropdownInviata() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtInCarico");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlInviatiI.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListInCarico.ClientID %>');
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
            dropdown = document.getElementById('<%= DdlIndirizzoI.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListI.ClientID %>');
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
        .uppercase-text {
            text-transform: uppercase;
        }

        .custom-border {
            border: 2px solid #007bff; /* Cornice blu */
            border-radius: 8px; /* Angoli arrotondati */
            padding: 15px; /* Spazio interno */
            margin: 5px 0; /* Spazio esterno */
            margin-left: -30px;
        }
    </style>

    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">INSERISCI UNA NUOVA PRATICA</p>
        </div>






        <div class="panel panel-default">
            <div class="panel-body">
                <div class="container">

                    <div class="tab-content">
                        <p style="font-weight: bold;">Informazioni Generali</p>

                        <div class="row custom-border">
                            <div class="col-md-6 ">
                                <div class="form-check mb-2">
                                    <label for="txtPratN">Prot.N.</label>
                                    <asp:TextBox ID="txtPratN" runat="server" CssClass="form-control1" ForeColor="Red" Font-Bold="true" />
                                    <label for="txtSiglaTp">Sigla</label>
                                    <asp:TextBox ID="txtSiglaTp" runat="server" CssClass="form-control1" Font-Bold="true" />

                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtDataInserimentoTp">Data Inserimento</label>
                                    <asp:TextBox ID="txtDataInserimentoTp" runat="server" CssClass="form-control" Enabled="false" Font-Bold="true" />
                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtProGenTp">Prot. Gen.</label>
                                    <asp:TextBox ID="txtProGenTp" runat="server" CssClass="form-control"></asp:TextBox>

                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtDataProtGen">In Data</label>
                                    <asp:TextBox ID="txtDataProtGen" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator2"
                                        runat="server"
                                        ControlToValidate="txtDataProtGen"
                                        ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$"
                                        ErrorMessage="la data deve essere dd/mm/aaaa"
                                        ForeColor="Red"
                                        ValidationGroup="bt"
                                        Display="Static">
                                    </asp:RegularExpressionValidator>

                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtProProcTp">Prot. Proc.</label>
                                    <asp:TextBox ID="txtProProcTp" runat="server" CssClass="form-control"></asp:TextBox>

                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtDataProtProc">In Data</label>
                                    <asp:TextBox ID="txtDataProtProc" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator3"
                                        runat="server"
                                        ControlToValidate="txtDataProtProc"
                                        ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$"
                                        ErrorMessage="la data deve essere dd/mm/aaaa"
                                        ForeColor="Red"
                                        ValidationGroup="bt"
                                        Display="Static">
                                    </asp:RegularExpressionValidator>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-content">
                        <p style="font-weight: bold;">Dettagli</p>
                        <div class="row custom-border">

                            <div class="col-md-6">
                                <div class="form-check mb-2">
                                    <label for="txtOggettoTp">Oggetto</label>
                                    <asp:TextBox ID="txtOggettoTp" runat="server" CssClass="form-control" />

                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtDestinatarioTp">Destinatario</label>
                                    <asp:TextBox ID="txtDestinatarioTp" runat="server" CssClass="form-control" />
                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtQuartiereTp">Quartiere</label>
                                    <asp:TextBox ID="txtQuartiereTp" runat="server" CssClass="form-control" />
                                </div>

                                <div class="form-check mb-2">
                                    <label for="txtNotaTp">Nota</label>
                                    <asp:TextBox ID="txtNotaTp" runat="server" CssClass="form-control" />
                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtCartellinaTp">Cartellina</label>
                                    <asp:TextBox ID="txtCartellinaTp" runat="server" CssClass="form-control" />
                                </div>

                            </div>
                        </div>
                        <
                    </div>
                </div>
            </div>
        </div>



        <asp:HiddenField ID="hdnConfermaUtente" runat="server" Value="false" />
        <div class="row">
            <asp:Label ID="lblRisultatoAzione" runat="server" Text="" Visible="false" ForeColor="Green"></asp:Label>
            <div class="col-12 text-center">
                <asp:Button Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary mt-3" ID="btSalva" ValidationGroup="bt" />
                <asp:Button ID="btCercaQuartiere" Text="Cerca Quartiere" runat="server" OnClick="apripopup_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" />

            </div>
        </div>
    </div>

    <!-- Modale Bootstrap quartiere -->
    <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel2">Ricerca Quartiere</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">
                        <label for="txtIndirizzoQuartiere">Indirizzo:</label>
                        <asp:TextBox ID="txtIndirizzoQuartiere" runat="server" CssClass="form-control" placeholder="Campo obbligatorio" />

                    </div>

                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="gvPopup" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopup_RowDataBound" OnRowCommand="gvPopup_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID_quartiere" HeaderText="ID" Visible="false" />
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
                        <asp:GridView ID="GVRicercaPratica" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopup_RowDataBoundP" OnRowCommand="gvPopup_RowCommandP" AllowPaging="true" PageSize="10" OnPageIndexChanging="GVRicercaPratica_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="id_Archivio" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="arch_numPratica" HeaderText="Numero Pratica" />
                                <asp:BoundField DataField="arch_doppione" HeaderText="Doppione" />

                                <asp:TemplateField HeaderText="Responsabile" ItemStyle-CssClass="uppercase-text">
                                    <HeaderTemplate>
                                        Responsabile
                                       <br />
                                        <asp:TextBox ID="txtFilterResponsabile" runat="server" OnTextChanged="txtFilterResponsabile_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        Filtro
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("arch_responsabile") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="arch_responsabile" HeaderText="Responsabile" />--%>
                                <%--<asp:BoundField DataField="arch_indirizzo" HeaderText="Indirizzo" />--%>



                                <asp:TemplateField HeaderText="Indirizzo" ItemStyle-CssClass="uppercase-text">
                                    <HeaderTemplate>
                                        Indirizzo
                                        <br />
                                        <asp:TextBox ID="txtFilterIndirizzo" runat="server" OnTextChanged="txtFilterIndirizzo_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        Filtro
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("arch_indirizzo") %>
                                    </ItemTemplate>
                                </asp:TemplateField>




                                <asp:BoundField DataField="arch_dataIns" HeaderText="Anno/Mese" DataFormatString="{0:yyyy/MM}" HtmlEncode="false" />
                                <%--<asp:BoundField DataField="arch_note" HeaderText="Nota" />--%>
                                <asp:TemplateField HeaderText="Note" ItemStyle-CssClass="uppercase-text">
                                    <HeaderTemplate>
                                        Note
                                        <br />
                                        <asp:TextBox ID="txtFilterNote" runat="server" OnTextChanged="txtFilterNote_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        Filtro
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("arch_note") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="arch_datault_intervento" HeaderText="Ultima Modifica" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
                                <asp:BoundField DataField="arch_matricola" HeaderText="Matricola" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("id_Archivio") + ";" + Eval("arch_numPratica")   %>' CssClass="btn btn-success btn-sm" />
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
                <asp:HiddenField ID="HfStato" runat="server" />
                <asp:HiddenField ID="HfFiltroNote" runat="server" />
                <asp:HiddenField ID="HfFiltroIndirizzo" runat="server" />
                <asp:HiddenField ID="HfFiltroResponsabile" runat="server" />
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                </div>
            </div>
        </div>
    </div>
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

                        <p id="errorMessage" runat="server" style="color: red"></p>

                    </div>

                </div>
                <div class="modal-footer">

                    <asp:Button ID="Button2" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopupErrore_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modale per conferma inserimento -->
    <div class="modal fade" id="confermaModal" tabindex="-1" role="dialog" aria-labelledby="confermaModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="confermaModalLabel">Conferma Azione</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Chiudi">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body" id="modalMessaggioBody" style="color: red;">
                    <!-- Il messaggio verrà inserito qui tramite JavaScript -->
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="btnModalOk">OK</button>

                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Annulla</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
