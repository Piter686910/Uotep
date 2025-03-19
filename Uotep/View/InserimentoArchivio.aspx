<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InserimentoArchivio.aspx.cs" Inherits="Uotep.InserimentoArchivio" %>


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



    </script>
    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">INSERISCI DATI</p>
        </div>
        <div class="container">
            <div class="row align-items-start">
                <!-- Colonna Sinistra -->
                <div class="col-md-6">
                    <div class="form-group mb-3">
                        <label for="txtPratica">Nr Pratica</label>
                        <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control1" ForeColor="Red" Enabled="false" Font-Bold="true" />


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
                                <div id="suggestionsListIndirizzo" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                </div>

                                <asp:DropDownList ID="DdlIndirizzo" runat="server" CssClass="form-control" Style="display: none" />
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
                        <div id="suggestionsListQuartiere" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                        </div>
                        <asp:DropDownList ID="DdlQuartiere" runat="server" CssClass="form-control" Style="display: none" />
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
                        <asp:TextBox ID="txtDataUltimoIntervento" runat="server" CssClass="form-control" />
                    </div>

                    <div class="form-group mb-3">
                        <label for="txtNominativo">Nominativo</label>
                        <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" />
                    </div>

                    <div class="form-group mb-3">
                        <label for="txtResponsabile">Responsabile</label>
                        <asp:TextBox ID="txtResponsabile" runat="server" CssClass="form-control" />

                    </div>
                    <div class="form-group mb-3">
                        <label for="txtGiudice">Giudice</label>
                        <asp:TextBox ID="txtGiudice" runat="server" AutoPostBack="false" onkeyup="filterDropdownGiudice()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsList" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                        </div>

                        <asp:DropDownList ID="DdlGiudice" runat="server" Style="display: none;" CssClass="form-control" />
                    </div>




                    <div class="form-group mb-3">

                        <label for="txtInCarico" class="form-label">In Carico</label>
                        <asp:TextBox ID="txtInCarico" runat="server" AutoPostBack="false" onkeyup="filterDropdownInviata()" Style="width: 250px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListInviata" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                        </div>
                        <asp:DropDownList ID="DdlInviati" runat="server" CssClass="form-control" Style="display: none" />

                    </div>



                    <div class="form-group mb-3">
                        <label for="txtTipoAtto">Tipologia Atto</label>
                        <asp:TextBox ID="txtTipoAtto" runat="server" AutoPostBack="false" onkeyup="filterDropdownTipoAtto()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        <div id="suggestionsListTipoAtto" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            <asp:HiddenField ID="HfTipoAtto" runat="server" />
                        </div>
                        <asp:DropDownList ID="DdlTipoAtto" runat="server" CssClass="form-control" Style="display: none" />
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

            <div class="row">
                <div class="col-12 text-center">
                    <asp:Button Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary mt-3" />
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
