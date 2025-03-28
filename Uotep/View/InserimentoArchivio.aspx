<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InserimentoArchivio.aspx.cs" Inherits="Uotep.InserimentoArchivio" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
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
        function filterDropdownTipoAtto() {
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
        }
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
    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">INSERISCI UNA NUOVA PRATICA</p>
        </div>

        <div class="container">
            <div class="row align-items-start">
                <!-- Colonna Sinistra -->
                <div class="col-md-6">
                    <div class="form-group mb-3">
                        <label for="txtPratica">Nr Pratica</label>
                        <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control1" ForeColor="Red" Font-Bold="true" />


                    </div>
                    <div class="form-group mb-3">
                        <label for="txtDataInserimento">Data Inserimento</label>
                        <asp:TextBox ID="txtDataInserimento" runat="server" CssClass="form-control" Enabled="false" Font-Bold="true" />
                    </div>

                    <!-- Indirizzo e TextBox sulla stessa riga -->
                    <div class="form-group mb-3">
                        <label for="DdlIndirizzo">Indirizzo</label>
                        <div class="row">
                            <!-- DropDownList occupa metà spazio -->
                            <div class="col-md-6">
                                <asp:TextBox ID="txtIndirizzo" runat="server" AutoPostBack="false" onkeyup="filterDropdownIndirizzo()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                <div id="suggestionsListI" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                </div>

                                <asp:DropDownList ID="DdlIndirizzoI" runat="server" CssClass="form-control" Style="display: none" />
                            </div>

                        </div>

                    </div>

                    <div class="form-group mb-3">
                        <label for="txtDataNascita">Data Nascita</label>

                        <asp:TextBox ID="txtDataNascita" runat="server" AutoPostBack="false" onkeyup="filterDropdownTipoProv()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>

                    </div>
                    <div class="form-group mb-3">
                        <label for="txtQuartiere">Quartiere</label>
                        <asp:TextBox ID="txtQuartiere" runat="server" AutoPostBack="false" onkeyup="filterDropdownQuartiere()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListQ" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                        </div>
                        <asp:DropDownList ID="DdlQuartiereI" runat="server" CssClass="form-control" Style="display: none" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtSezione">Dati Catastali</label>
                        <div class="form-group mb-3">
                            <asp:TextBox ID="txtSezione" runat="server" MaxLength="3" Style="width: 60px; display: inline-block !important;" CssClass="form-control" placeholder="Sezione"></asp:TextBox>
                            <asp:TextBox ID="TxtFoglio" runat="server" MaxLength="4" Style="width: 60px; display: inline-block !important; margin-left: 5px;" CssClass="form-control" placeholder="Foglio"></asp:TextBox>
                            <asp:TextBox ID="TxtParticella" runat="server" MaxLength="5" Style="width: 80px; display: inline-block !important; margin-left: 5px;" CssClass="form-control" placeholder="Part.lla"></asp:TextBox>
                            <asp:TextBox ID="TxtSub" runat="server" MaxLength="4" Style="width: 70px; display: inline-block !important; margin-left: 5px;" CssClass="form-control" placeholder="Sub"></asp:TextBox>

                        </div>
                    </div>
                    <div class="form-group mb-3">

                        <label for="txtNote">Note</label>
                        <asp:TextBox ID="txtNote" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />

                    </div>

                </div>

                <!-- Colonna Destra -->
                <div class="col-md-6">

                    <div class="form-group mb-3">
                        <label for="txtDataUltimoIntervento">Data Ultimo Intervento</label>
                        <asp:TextBox ID="txtDataUltimoIntervento" runat="server" CssClass="form-control" Enabled="false" />
                    </div>

                    <div class="form-group mb-3">
                        <label for="txtNominativo">Nominativo</label>
                        <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rqNominativo" runat="server" ControlToValidate="txtNominativo" ErrorMessage="inserire un nominativo" ForeColor="Red" ValidationGroup="bt">
                        </asp:RequiredFieldValidator>

                    </div>

                    <div class="form-group mb-3">
                        <label for="txtResponsabile">Responsabile</label>
                        <asp:TextBox ID="txtResponsabile" runat="server" CssClass="form-control" />

                    </div>
                    <div class="form-group mb-3">
                        <label for="txtGiudice">Giudice</label>
                        <asp:TextBox ID="txtGiudice" runat="server" AutoPostBack="false" onkeyup="filterDropdownGiudice()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListG" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                        </div>

                        <asp:DropDownList ID="DdlGiudiceI" runat="server" Style="display: none;" CssClass="form-control" />
                    </div>




                    <div class="form-group mb-3">

                        <label for="txtInCarico" class="form-label">In Carico</label>
                        <asp:TextBox ID="txtInCarico" runat="server" AutoPostBack="false" onkeyup="filterDropdownInviata()" Style="width: 250px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListInCarico" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                        </div>
                        <asp:DropDownList ID="DdlInviatiI" runat="server" CssClass="form-control" Style="display: none" />

                    </div>



                    <div class="form-group mb-3">
                        <label for="txtTipoAtto">Tipologia Atto</label>
                        <asp:TextBox ID="txtTipoAtto" runat="server" AutoPostBack="false" onkeyup="filterDropdownTipoAtto()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListTA" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                        </div>
                        <asp:DropDownList ID="DdlTipoAttoI" runat="server" CssClass="form-control" Style="display: none" />
                    </div>
                </div>

            </div>


            <div class="row ">
                <div class="col-md-2 d-flex align-items-center">
                    <div class="form-check">
                        <asp:CheckBox ID="CkEvasa" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkEvasa">Evasa</label>
                    </div>
                </div>
                <div class="col-md-2 d-flex align-items-center">
                    <div class="form-group">
                        <asp:CheckBox ID="Ck1089" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="Ck1089">1089</label>
                    </div>
                </div>

                <div class="col-md-2 d-flex align-items-center">
                    <div class="form-group">
                        <asp:CheckBox ID="CkSuoloPubblico" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkSuoloPubblico">Suolo Pubblico</label>
                    </div>
                </div>
                <div class="col-md-2 d-flex align-items-center">
                    <div class="form-group">
                        <asp:CheckBox ID="CkVincoli" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkVincoli">Vincoli</label>
                    </div>
                </div>
                <div class="col-md-2 d-flex align-items-center">
                    <div class="form-group">
                        <asp:CheckBox ID="CkDemolita" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="CkDemolita">Demolita</label>
                    </div>
                </div>

            </div>
            <asp:HiddenField ID="hdnConfermaUtente" runat="server" Value="false" />
            <div class="row">
                <asp:Label ID="lblRisultatoAzione" runat="server" Text="" Visible="false" ForeColor="Green"></asp:Label>
                <div class="col-12 text-center">
                    <asp:Button Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary mt-3" ID="btSalva"  ValidationGroup="bt" />
                    <%--<asp:Button Text="Modifica" runat="server" OnClick="Modifica_Click" CssClass="btn btn-primary mt-3" />--%>
                    <asp:Button Text="Cerca Quartiere" runat="server" OnClick="apripopup_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" />

                </div>
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
                                <asp:BoundField DataField="ID_quartiere" HeaderText="ID" />
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
    <%-- Modale ricerca pratica --%>
    <div class="modal fade" id="ModalPratica" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel10">Ricerca Pratica</h5>

                </div>
                <div class="modal-body">

                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="GVRicercaPratica" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopup_RowDataBoundP" OnRowCommand="gvPopup_RowCommandP">
                            <Columns>
                                <asp:BoundField DataField="id_Archivio" HeaderText="ID" />
                                <asp:BoundField DataField="arch_numPratica" HeaderText="Numero Pratica" />
                                <asp:BoundField DataField="arch_nominativo" HeaderText="Nominativo" />
                                <asp:BoundField DataField="arch_indirizzo" HeaderText="Indirizzo" />
                                <asp:BoundField DataField="arch_datault_intervento" HeaderText="Ultima Modifica" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
                                <asp:BoundField DataField="arch_matricola" HeaderText="Matricola" />

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("id_Archivio") + ";" + Eval("arch_numPratica")   %>' CssClass="btn btn-success btn-sm" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
                <asp:HiddenField ID="HfStato" runat="server" />
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

</asp:Content>
