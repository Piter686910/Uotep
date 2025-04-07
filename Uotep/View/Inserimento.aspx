<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inserimento.aspx.cs" Inherits="Uotep.Inserimento" %>


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
            <p class="text-center lead">INSERISCI DATI</p>
        </div>
        <div class="container">
            <div class="row">
                <!-- Colonna Sinistra -->
                <div class="col-md-6">
                    <div class="form-group mb-3">
                        <label for="txtProt">Nr Protocollo</label>
                        <asp:TextBox ID="txtProt" runat="server" CssClass="form-control1" ForeColor="Red" Enabled="false" Font-Bold="true" />
                        <label for="Ddltipo">/</label>
                        <asp:DropDownList ID="DdlSigla" runat="server" CssClass="form-control1">
                            <asp:ListItem Text="ED"> </asp:ListItem>
                            <asp:ListItem Text="TP"> </asp:ListItem>
                            <asp:ListItem Text="PG"> </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtDataArrivo">Data Arrivo</label>
                        <asp:TextBox ID="txtDataArrivo" runat="server" CssClass="form-control" Enabled="false" Font-Bold="true" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtGiudice">Giudice</label>
                        <asp:TextBox ID="txtGiudice" runat="server" AutoPostBack="false" onkeyup="filterDropdownGiudice()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsList" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            <asp:HiddenField ID="HfGiudice" runat="server" />
                        </div>
                        <asp:Button ID="btSalvaGiudice" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaGiudice_Click" Visible="false" />

                        <asp:DropDownList ID="DdlGiudice" runat="server" Style="display: none;" CssClass="form-control" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="DdlTipoProvvAg">Tipo Provvedimento AG</label>

                        <asp:TextBox ID="txtTipoProv" runat="server" AutoPostBack="false" onkeyup="filterDropdownTipoProv()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListTipoProv" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            <asp:HiddenField ID="HfTipoProv" runat="server" />

                        </div>
                        <asp:Button ID="btSalvaTipoProvv" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaTipoProvv_Click" Visible="false" />
                        <asp:DropDownList ID="DdlTipoProvvAg" runat="server" CssClass="form-control" Style="display: none;" />
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
                </div>

                <!-- Colonna Destra -->
                <div class="col-md-6">

                    <div class="form-group mb-3">
                        <label for="txtRifProtGen">Riferimento Prot. Gen.</label>
                        <asp:TextBox ID="txtRifProtGen" runat="server" CssClass="form-control" />
                    </div>

                    <!-- Indirizzo e TextBox sulla stessa riga -->
                    <div class="form-group mb-3">
                        <label for="DdlIndirizzo">Indirizzo</label>
                        <div class="row">
                            <!-- DropDownList occupa metà spazio -->
                            <div class="col-md-6">
                                <asp:TextBox ID="txtIndirizzo" runat="server" AutoPostBack="false" onkeyup="filterDropdownIndirizzo()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                <div id="suggestionsListIndirizzo" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                    <asp:HiddenField ID="HfIndirizzo" runat="server" />
                                </div>

                                <asp:DropDownList ID="DdlIndirizzo" runat="server" CssClass="form-control" Style="display: none" />
                            </div>
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

                        <label for="txPratica">Pratica</label>
                        <asp:TextBox ID="txPratica" runat="server" CssClass="form-control" />
                    </div>

                    <div class="form-group mb-3">
                        <label for="txtTipoAtto">Tipologia Atto</label>
                        <asp:TextBox ID="txtTipoAtto" runat="server" AutoPostBack="false" onkeyup="filterDropdownTipoAtto()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListTipoAtto" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            <asp:HiddenField ID="HfTipoAtto" runat="server" />
                        </div>
                        <asp:Button ID="btSalvaTipoAtto" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaTipoAtto_Click" Visible="false" />
                        <asp:DropDownList ID="DdlTipoAtto" runat="server" CssClass="form-control" Style="display: none" />
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
                        <asp:TextBox ID="txtDataDataEvasa" runat="server" CssClass="form-control" />
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
                        <asp:TextBox ID="txtDataInvio" runat="server" CssClass="form-control" />
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
                <div class="col-12 text-center">
                    <asp:Button Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
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
