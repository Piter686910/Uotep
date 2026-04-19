<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Urp.aspx.cs" Inherits="Uotep.Urp" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function showModal() {
            $('#ModalRicercaScadenziario').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalRicercaScadenziario').modal('hide');

        }
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        // Nasconde il popup
        function HideErrorMessage() {
            $('#errorModal').modal('hide');
        }
        function ShowMsgMessage(message) {
            $('#MsgModal').modal('show');
        }
        // Nasconde il popup
        function HideMsgMessage() {
            $('#MsgModal').modal('hide');
        }
        function showModal() {
            $('#ModalDataScadenza').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalDataScadenza').modal('hide');

        }

       <%-- //Esito
        function filterDropdownEsito() {
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
        }--%>

    </script>


    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <%--  <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">GESTIONE PRATICHE URP</p>--%>
            <div class="dashboard-header">
                <h1><span class="fa-solid fa-gear fa-spin"></span> GESTIONE PRATICHE URP</h1>
            </div>
        </div>
        <div class="container">

            <div class="tab-content">
                <!-- Titolo -->
                <p style="font-weight: bold; font-size: medium">Dati Scadenziario</p>

                <div class="row custom-border">

                    <!-- ### RIGA IN ALTO (Intera larghezza) ### -->
                    <div class="col-md-12" style="margin-bottom: 15px; border-bottom: 1px solid #eee; padding-bottom: 10px;">
                        <label style="font-weight: bold; margin-right: 15px;">Tipologia Richiesta:</label>

                        <span style="margin-right: 20px;">
                            <asp:RadioButton ID="rd241_90" runat="server" GroupName="AreaGroup" Text=" L.241/90" />
                        </span>
                        <span>
                            <asp:RadioButton ID="rd33_2013" runat="server" GroupName="AreaGroup" Text=" Dgls. 33/13" />
                        </span>
                        <label style="font-weight: bold; margin-right: 15px; margin-left: 150px">Oggetto:</label>
                        <span style="margin-right: 20px;">
                            <asp:RadioButton ID="rdCopiaVisione" runat="server" GroupName="AreaGroup1" Text=" Copia/Visione " />
                        </span>
                        <span>
                            <asp:RadioButton ID="rdRicCopia" runat="server" GroupName="AreaGroup1" Text=" Rich. Copia " />
                        </span>
                        <span>
                            <asp:RadioButton ID="rdRicVisione" runat="server" GroupName="AreaGroup1" Text=" Rich. Visione " />
                        </span>

                    </div>
                    <!-- ### FINE RIGA IN ALTO ### -->


                    <!-- COLONNA 1 -->
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtCarico">Nr Carico</label>
                            <asp:TextBox ID="txtCarico" runat="server" CssClass="form-control focus larghezzaText" ForeColor="Red" Font-Bold="true" autofocus="" />
                        </div>

                        <div class="form-group mb-3">
                            <label for="txtProtGen">Protocollo Generale</label>
                            <asp:TextBox ID="txtProtGen" runat="server" CssClass="form-control " />
                        </div>

                        <div class="form-group mb-3">
                            <label for="txtProtUscita">Protocollo Uscita</label>
                            <asp:TextBox ID="txtProtUscita" runat="server" CssClass="form-control " />
                        </div>

                    </div>

                    <!-- COLONNA 2 -->
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtAnno">Anno</label>
                            <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control larghezzaText70" />
                        </div>


                        <div class="form-group mb-3">
                            <label for="txtDataArrivo">Data Arrivo</label>
                            <asp:TextBox ID="txtDataArrivo" runat="server" CssClass="form-control data-auto" placeholder="gg/mm/yyyy" onchange="aggiungi30Giorni(this, 'txtDataScadenza')" />
                        </div>
                        <div class="form-group mb-3">
                            <label for="txtDataUscita">Data Uscita</label>
                            <asp:TextBox ID="txtDataUscita" runat="server" CssClass="form-control data-auto" placeholder="gg/mm/yyyy" />
                        </div>
                    </div>

                    <!-- COLONNA 3 -->
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtPratica">Pratica</label>
                            <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control larghezzaText" />
                        </div>

                        <div class="form-group mb-3">
                            <label for="txtDataScadenza">Data Scadenza</label>
                            <asp:TextBox ID="txtDataScadenza" runat="server" CssClass="form-control data-auto" placeholder="gg/mm/yyyy" ClientIDMode="Static" />
                        </div>


                        <div class="form-group mb-3">
                            <label for="DdlEsito">Esito</label>
                            <asp:DropDownList ID="DdlEsito" runat="server" CssClass="form-control " AutoPostBack="true" OnSelectedIndexChanged="DdlEsito_SelectedIndexChanged" />
                        </div>


                    </div>

                    <!-- COLONNA 4 -->
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtRichiedente">Richiedente</label>
                            <asp:TextBox ID="txtRichiedente" runat="server" CssClass="form-control " />
                        </div>


                    </div>
                    <!-- "Contro Interessati" rimane qui allineato ai campi successivi -->
                    <div class="form-group mb-3">
                        <label style="display: block; margin-bottom: 5px;">Contro Interessati</label>

                        <!-- Metti lo stesso GroupName a entrambi -->
                        <asp:RadioButton ID="rbControInteressatiSi" runat="server" GroupName="GruppoInteressati" Text=" SI" />
                        <span style="margin-left: 10px;"></span>
                        <!-- Spazietto -->
                        <asp:RadioButton ID="rbControInteressatiNo" runat="server" GroupName="GruppoInteressati" Text=" NO" />
                    </div>

                    <div class="col-md-3" style="margin-top: 7px">
                        <label for="txtMotivazione">Motivazione</label>
                        <asp:TextBox ID="txtMotivazione" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <!-- Fine Row Custom Border -->

            </div>

        </div>
        <div class="row">
            <div class="col-12 text-center">
                <asp:Button ID="btSalva" Text="💾 Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                <asp:Button ID="btNewIns" Text="➕ Nuovo" runat="server" OnClick="btNewIns_Click" CssClass="btn btn-primary mt-3" Visible="false" />
                <asp:Button ID="btRicerca" Text="📂 Ricerca" runat="server" OnClick="btRicerca_Click" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                <asp:Button ID="btRegistro" Text="📂 Registro" runat="server" OnClick="btRegistro_Click" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />

            </div>
        </div>
    </div>
    <asp:HiddenField ID="HfDataArrivo" runat="server" />
    <asp:HiddenField ID="HfDataScadenza" runat="server" />
    <asp:HiddenField ID="HfDataUscita" runat="server" />
    <asp:HiddenField ID="HfFiltroEsito" runat="server" />
    <asp:HiddenField ID="HfNewDataScadenza" runat="server" />
    <asp:HiddenField ID="HfRegistro" runat="server" />
    <asp:HiddenField ID="HfId" runat="server" />

    <%-- Modal Scadenziario --%>
    <div class="modal fade" id="ModalRicercaScadenziario" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 100%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel2">Scadenziario</h5>

                </div>

                <!-- GridView Scadenziario -->
                <div id="DivGrid" runat="server" class="form-group" style="padding-left: -50px">

                    <asp:GridView ID="gvScadenziario" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                        OnRowDataBound="gvScadenziario_RowDataBound" OnRowCommand="gvScadenziario_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvScadenziario_PageIndexChanging" RowStyle-CssClass="GridViewRow"
                        AlternatingRowStyle-CssClass="GridViewAlternatingRow">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id_scadenziario" Visible="false" />
                            <asp:BoundField DataField="nr_carico" HeaderText="Nr. Carico" ItemStyle-Width="40px" />
                            <asp:BoundField DataField="anno" HeaderText="Anno" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="nr_pratica" HeaderText="Nr. Pratica" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="richiedente" HeaderText="Richiedente" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="protGen" HeaderText="Prot. Gen." ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />

                            <asp:TemplateField HeaderText="dataArrivo" ItemStyle-Width="50px">
                                <HeaderTemplate>
                                    data Arrivo
                       <br />
                                    <asp:TextBox ID="txtFilterDataArrivo" runat="server" OnTextChanged="txtFilterDataArrivo_TextChanged" AutoPostBack="True" CssClass="larghezzaText data-auto"></asp:TextBox>
                                    Filtro
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("dataArrivo", "{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="dataScadenza" ItemStyle-Width="50px">
                                <HeaderTemplate>
                                    data Scadenza
                       <br />
                                    <asp:TextBox ID="txtFilterDataScadenza" runat="server" OnTextChanged="txtFilterDataScadenza_TextChanged" AutoPostBack="True" CssClass="larghezzaText data-auto"></asp:TextBox>
                                    Filtro
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("dataScadenza", "{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="C. Interessati" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("controInteressati").ToString() == "True" ? "Si" : "No" %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <%--<asp:BoundField DataField="esito" HeaderText="Esito" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />--%>

                            <asp:TemplateField ItemStyle-Width="80px">
                                <HeaderTemplate>
                                    Esito
                                       <br />
                                    <asp:DropDownList ID="DdlEsitoFiltro" runat="server" OnSelectedIndexChanged="DdlEsitoFiltro_SelectedIndexChanged" AutoPostBack="True" CssClass="larghezzaText">
                                    </asp:DropDownList>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("esito") %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:BoundField DataField="motivazione" HeaderText="Motivazione" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="protUscita" HeaderText="Prot. Uscita" ItemStyle-Width="50px" />


                            <asp:TemplateField HeaderText="Data Uscita" ItemStyle-Width="50px">
                                <HeaderTemplate>
                                    data Uscita
                       <br />
                                    <asp:TextBox ID="txtFilterDataUscita" runat="server" OnTextChanged="txtFilterDataUscita_TextChanged" AutoPostBack="True" CssClass="larghezzaText data-auto"></asp:TextBox>
                                    Filtro
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("dataUscita", "{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Ric. 241/90" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("ric24190").ToString() == "True" ? "Si" : "No" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ric. 33/13" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("ric3313").ToString() == "True" ? "Si" : "No" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnSelect" runat="server" Text="Sel."
                                        CommandName="Select"
                                        CommandArgument='<%# Eval("id_Scadenziario")  + "|" + Eval("nr_carico") + "|" + Eval("anno")%>'
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
                    <div class="modal-footer">
                        <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClientClick="hideModal()" />
                        <asp:Button ID="btBack" runat="server" class="btn btn-secondary" Text="Back" OnClick="btBack_Click" />
                    </div>
                </div>
            </div>

        </div>
    </div>
    <%-- Modal Registro --%>
    <div class="modal fade" id="ModalRicercaRegistro" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 100%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel5">Registro</h5>

                </div>

                <!-- GridView Registro -->
                <div id="Div1" runat="server" class="form-group" style="padding-left: -50px">

                    <asp:GridView ID="gvRegistro" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered"
                        AllowPaging="true"
                        PageSize="10"
                        DataKeyNames="id_registro"
                        OnRowDataBound="gvRegistro_RowDataBound"
                        OnPageIndexChanging="gvRegistro_PageIndexChanging"
                        OnRowEditing="gvRegistro_RowEditing"
                        OnRowCancelingEdit="gvRegistro_RowCancelingEdit"
                        OnRowDeleting="gvRegistro_RowDeleting"
                        OnRowUpdating="gvRegistro_RowUpdating"
                        RowStyle-CssClass="GridViewRow"
                        AlternatingRowStyle-CssClass="GridViewAlternatingRow">

                        <Columns>
                            <asp:BoundField DataField="id_registro" HeaderText="id" Visible="false" ReadOnly="true" />

                            <%-- ESEMPIO CAMPO EDITABILE: OGGETTO --%>
                            <asp:TemplateField HeaderText="Oggetto" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <%# Eval("oggetto") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtOggetto" runat="server" Text='<%# Bind("oggetto") %>' CssClass="form-control input-sm"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- ESEMPIO CAMPO DATA EDITABILE --%>
                            <asp:TemplateField HeaderText="Data Presentazione" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%# FormatMyDate(Eval("dataPresentRichiesta", "{0:dd/MM/yyyy}") )%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDataPres" runat="server" Text='<%# Bind("dataPresentRichiesta", "{0:dd/MM/yyyy}") %>' CssClass="form-control input-sm" placeholder="gg/mm/yyyy"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="nrPgTrasmissioneRichiesto" HeaderText="Nr PG Trasm." ItemStyle-Width="40px" ReadOnly="true" />--%>
                            <asp:TemplateField HeaderText="Nr PG Trasm.">
                                <ItemTemplate><%# Eval("nrPgTrasmissioneRichiesto") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPgTrasmissioneRichiesto" runat="server" Text='<%# Bind("nrPgTrasmissioneRichiesto") %>' CssClass="form-control input-sm" Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Ufficio Detentore">
                                <ItemTemplate><%# Eval("uffDetentore") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUffDetentore" runat="server" Text='<%# Bind("uffDetentore") %>' CssClass="form-control input-sm"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- ESEMPIO CHECKBOX PER SI/NO --%>
                            <asp:TemplateField HeaderText="C. Interessati" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("controInteressati").ToString() == "True" ? "Si" : "No" %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkControInteressati" runat="server" Checked='<%# Bind("controInteressati") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Esito">
                                <ItemTemplate><%# Eval("esito") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEsito" runat="server" Text='<%# Bind("esito") %>' CssClass="form-control input-sm"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- COLONNA COMANDI (MODIFICA / SALVA / ANNULLA) --%>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <!-- Il pulsante per entrare in modifica deve avere CommandName="Edit" -->
                                    <asp:Button ID="btnModifica" runat="server" Text="Mod." CommandName="Edit" CssClass="btn btn-warning btn-sm" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <!-- Pulsanti visibili quando sei in modifica -->
                                    <asp:Button ID="btnSalva" runat="server" Text="Salva" CommandName="Update" CssClass="btn btn-primary btn-sm" ValidationGroup="EditVG" />
                                    <asp:Button ID="btnAnnulla" runat="server" Text="X" CommandName="Cancel" CssClass="btn btn-secondary btn-sm" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <asp:Button ID="btnElimina" runat="server" Text="Del." CommandName="Delete" CommandArgument='<%# Eval("id_registro") %>' CssClass="btn btn-danger btn-sm" />
                                    <%-- OnClientClick="return confirm('Sei sicuro di voler eliminare questa riga?');" --%>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                        <%-- ... (TUA PAGER SETTINGS RIMANE UGUALE) ... --%>
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

                    <div class="modal-footer">
                        <asp:Button ID="Button1" runat="server" class="btn btn-secondary" Text="📊 Esporta Excel" OnClick="btnExportExcel_Click" />
                        <asp:Button ID="Button3" runat="server" class="btn btn-secondary" Text="Chiudi" OnClientClick="hideModal()" />
                    </div>
                </div>


            </div>
        </div>
    </div>

    <!-- Popup Modale inserimento data scadenza -->
    <div class="modal fade" id="ModalDataScadenza" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 20%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel4">Inserisci La Nuova Data Scadenza</h5>

                </div>
                <div id="Div3" runat="server" class="row" style="margin-left: 30px!important">
                    <div class="form-group mb-3">
                        <label for="txtdataScadenzaPopup">Data Scadenza</label>
                        <asp:TextBox ID="txtdataScadenzaPopup" runat="server" CssClass="form-control data-auto"></asp:TextBox>
                    </div>
                </div>

                <div class="modal-footer">
                    <!-- Bottone per avviare chiousura decretazione -->
                    <asp:Button ID="ModalChiudiDataScadenza" runat="server" class="btn btn-secondary" Text="Salva" OnClick="ModalChiudiDataScadenza_Click" />
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

                    <asp:Button ID="btClose" runat="server" class="btn btn-secondary" Text="Chiudi" OnClientClick="HideErrorMessage()" />
                </div>
            </div>
        </div>
    </div>
    <%-- popup messaggi --%>
    <div class="modal fade" id="MsgModal" tabindex="-1" role="dialog" aria-labelledby="MsgModalLabel" aria-hidden="true">
        <div class="modal-dialog"
            role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel11">ATTENZIONE</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">

                        <p id="TextMessage" runat="server" style="color: red"></p>

                    </div>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btChiudiMsgModal" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="btChiudiMsgModal_Click" />
                    <asp:Button ID="btOKCan" runat="server" class="btn btn-secondary" Text="OK" OnClick="btOKCan_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
