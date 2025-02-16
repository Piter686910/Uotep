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
                            <asp:ListItem Text="PG"> </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group mb-3">
                        <label for="txtDataArrivo">Data Arrivo</label>
                        <asp:TextBox ID="txtDataArrivo" runat="server" CssClass="form-control" Enabled="false" Font-Bold="true" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="DdlGiudice">Giudice</label>
                        <asp:DropDownList ID="DdlGiudice" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="DdlTipoProvvAg">Tipo Provvedimento AG</label>
                        <asp:DropDownList ID="DdlTipoProvvAg" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="DdlQuartiere">Quartiere</label>
                        <asp:DropDownList ID="DdlQuartiere" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group mb-3">
                         <asp:Button ID="btinsProv" runat="server" Text="+" CssClass="btn btn-primary mt-3" OnClick="apripopupProvenienza_Click"/>
                        <label for="DdlProvenienza">Provenienza</label>
                        
                        <asp:DropDownList ID="DdlProvenienza" runat="server" CssClass="form-control" />
                        
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
                                <asp:DropDownList ID="DdlIndirizzo" runat="server" CssClass="form-control" />
                            </div>
                            <!-- TextBox occupa metà spazio -->
                            <div class="col-md-6">
                                <asp:TextBox ID="txtVia" runat="server" CssClass="form-control" placeholder="specifica l'indirizzo" />
                            </div>
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
                        <label for="DdlTipoAtto">Tipologia Atto</label>
                        <asp:DropDownList ID="DdlTipoAtto" runat="server" CssClass="form-control" />
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
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtinviata" class="form-label">Inviata</label>
                        <%--<asp:TextBox ID="txtinviata" runat="server" CssClass="form-control" />--%>
                        <asp:DropDownList ID="DdlInviati" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="txtDataInvio" class="form-label">Il</label>
                        <asp:TextBox ID="txtDataInvio" runat="server" CssClass="form-control" />
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
                        <label for="txtIndirizzo">Indirizzo:</label>
                        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" placeholder="Campo obbligatorio" />

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
    <!-- Modale Bootstrap provenienza -->
     <div class="modal fade" id="myModalProvenienza" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
     <div class="modal-dialog">
         <div class="modal-content">
             <div class="modal-header">
                 <h5 class="modal-title" id="modalLabel1">Inserisci testo per la provenienza</h5>

             </div>
             <div class="modal-body">
                 <!-- Campi di input per la ricerca -->
                 <div class="form-group">
                     <label for="txtIndirizzo">Testo:</label>
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
