<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventario.aspx.cs" Inherits="Uotep.Inventario" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        /* 1. Sgancia tutti i contenitori padri di Bootstrap compresa la MasterPage */
        html, body, form, .container, .body-content, main, [class*="container"] {
            max-width: 100% !important;
            width: 100% !important;
            padding-left: 10px !important;
            padding-right: 10px !important;
            margin-left: 0 !important;
            margin-right: 0 !important;
        }

        /* 2. Forza le schede bianche interne ad allargarsi completamente */
        .section-box, #DivGrid, .jumbotron {
            width: 100% !important;
            max-width: 100% !important;
            box-sizing: border-box !important;
        }

        /* 3. Estende la tabella a tutto lo spazio disponibile */
        .table-responsive {
            width: 100% !important;
            max-width: 100% !important;
            overflow-x: auto !important;
        }

        #gvDett, .gridview-full {
            width: 100% !important;
            max-width: 100% !important;
            table-layout: auto !important; /* Espande le colonne su tutta la larghezza */
        }

        /* 4. Filtri sulle intestazioni */
        .larghezzaText {
            width: 100% !important;
            box-sizing: border-box;
            font-size: 0.8rem !important;
            padding: 2px 4px !important;
        }
    </style>

    <script>
        function ShowErrorMessage(message) {
            $('#errorMessage').text(message);
            $('#errorModal').modal('show');
        }
        function HideErrorMessage() {
            $('#errorModal').modal('hide');
        }
        function ShowProtocollo(message) {
            $('#txtProtocollo').text(message);
            $('#ModalProtocollo').modal('show');
        }
        function VisibleDiv() {
            var div = document.getElementById('<%= DivEstraiInventario.ClientID %>');
    if (div.style.display === "none" || div.style.display === "") {
        div.style.display = "block"; // Mostra il div
    } else {
        div.style.display = "none";  // Nasconde il div
    }
}
    </script>

    <!-- Usato container-fluid full-width per occupare il 100% effettivo della pagina -->
    <div class="container-fluid full-width-container">

        <div class="dashboard-header mt-3">
            <h1><span class="fa-solid fa-gear fa-spin"></span>INVENTARIO</h1>
        </div>



        <div class="section-box w-100" id="divInserimento" runat="server">
            <div class="row ">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="DdlCodep">Codice Stanza</label>
                        <asp:DropDownList ID="DdlCodep" runat="server" CssClass="form-control" OnSelectedIndexChanged="DdlCodep_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="" />
                            <asp:ListItem Text="A00" />
                            <asp:ListItem Text="A01" />
                            <asp:ListItem Text="A02" />
                            <asp:ListItem Text="A03" />
                            <asp:ListItem Text="A04" />
                            <asp:ListItem Text="A05" />
                            <asp:ListItem Text="A06" />
                            <asp:ListItem Text="A06 bis" />
                            <asp:ListItem Text="A07" />
                            <asp:ListItem Text="A08" />
                            <asp:ListItem Text="A09" />
                            <asp:ListItem Text="B01" />
                            <asp:ListItem Text="B02" />
                            <asp:ListItem Text="B03" />
                            <asp:ListItem Text="B04" />
                            <asp:ListItem Text="B05" />
                            <asp:ListItem Text="B06" />
                            <asp:ListItem Text="C01" />
                            <asp:ListItem Text="C02" />
                            <asp:ListItem Text="C03" />
                            <asp:ListItem Text="C04" />
                            <asp:ListItem Text="C05" />
                            <asp:ListItem Text="C06" />

                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="txtProgrBene">Progressivo bene</label>
                        <asp:TextBox ID="txtProgrBene" runat="server" CssClass="form-control" Enabled="false" />

                    </div>

                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="DdlUfficio">Ufficio</label>
                        <asp:DropDownList ID="DdlUfficio" runat="server" CssClass="form-control">
                            <asp:ListItem Text="" Value="" />
                            <asp:ListItem Text="Archivio" />
                            <asp:ListItem Text="CDR" />
                            <asp:ListItem Text="Corridoio p1" />
                            <asp:ListItem Text="Corridoio p2" />
                            <asp:ListItem Text="Coordinamento Decretazione Atti" />
                            <asp:ListItem Text="Coordinamento e Statistiche" />
                            <asp:ListItem Text="Fureria" />
                            <asp:ListItem Text="Macroarea Ed 1" />
                            <asp:ListItem Text="Macroarea Ed 2" />
                            <asp:ListItem Text="Macroarea Ed 3" />
                            <asp:ListItem Text="Macroarea Tp 1" />
                            <asp:ListItem Text="Macroarea Tp 2" />
                            <asp:ListItem Text="Macroarea Tp 3" />
                            <asp:ListItem Text="Napoli Servizi" />
                            <asp:ListItem Text="Trasmissione atti" />
                            <asp:ListItem Text="Sgomberi & URP" />
                            <asp:ListItem Text="Supporto Macro Aree" />

                        </asp:DropDownList>

                    </div>
                    <div class="form-group">
                        <label for="TxtData">Data Inserimento</label>
                        <asp:TextBox ID="TxtData" runat="server" CssClass="form-control data-auto" Enabled="false" />
                    </div>
                </div>

                <div class="col-md-4">

                    <div class="form-group">
                        <label for="txtDescrizione">Descrizione</label>
                        <asp:TextBox ID="txtDescrizione" runat="server" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <label for="txtEtichetta">Etichetta</label>
                        <asp:TextBox ID="txtEtichetta" runat="server" CssClass="form-control" />

                    </div>
                </div>
            </div>

            <div class="row ">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="txtCategoria">Categoria</label>
                        <asp:TextBox ID="txtCategoria" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label>Tipologia</label>
                        <asp:TextBox ID="txtTipologia" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Seriale</label>
                        <asp:TextBox ID="txtSeriale" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Note</label>
                        <asp:TextBox ID="txtNote" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                </div>
                <div class="form-check">
                    <asp:CheckBox ID="CkDismesso" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label ms-2" for="CkDismesso">Dismesso</label>

                </div>
            </div>

            <%-- Estrai inventario solo delle voci selezionate--%>
            <div id="DivEstraiInventario" runat="server" visible="false" style="margin-left: 250px!important">

                <div class="row justify-content-start" style="margin-left: -100px!important">
                    <!-- Aggiunto justify-content-start per sicurezza -->
                    <div class="form-check" style="display: flex; flex-direction: row; align-items: baseline;">
                        <!-- Stili per allineamento e spacing -->
                        <div style="margin-right: 55px;">
                            <%--                        <!- Spacing per il primo gruppo ->--%>
                            <asp:CheckBox ID="CkAppendino" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkAppendino">Appendino</label>
                        </div>
                        <div style="margin-right: 88px;">
                            <%--                        <!- Spacing per il secondo gruppo ->--%>
                            <asp:CheckBox ID="CkArmadio" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkArmadio">Armadio</label>
                        </div>
                        <div style="margin-right: 13px;">
                            <%--                        <!- Spacing per il terzo gruppo ->--%>
                            <asp:CheckBox ID="CkAsciugamani" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkAsciugamani">Asciugamani elettrico</label>
                        </div>
                        <div style="margin-right: 60px;">
                            <%--                        <!- Spacing per il quarto gruppo ->--%>
                            <asp:CheckBox ID="CkBacheca" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkBacheca">Bacheca</label>
                        </div>
                        <div style="margin-right: 55px">
                            <%--                        <!- Nessun margin-right per l'ultimo gruppo ->--%>
                            <asp:CheckBox ID="CkBandiera" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkBandiera">Bandiera</label>
                        </div>
                        <div style="margin-right: 55px">
                            
                            <asp:CheckBox ID="CkCassaforte" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkCassaforte">Cassaforte</label>
                        </div>
                    </div>
                </div>

                <div class="row justify-content-start" style="margin-left: -100px!important">
                    <!-- Aggiunto justify-content-start per sicurezza -->
                    <div class="form-check" style="display: flex; flex-direction: row; align-items: baseline;">
                        <!-- Stili per allineamento e spacing -->
                        <div style="margin-right: 10px;">
                            <%--                        <!- Spacing per il primo gruppo ->--%>
                            <asp:CheckBox ID="CkCassettiera" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkCassettiera">Cassettiera</label>
                        </div>

                        <div style="margin-right: 10px;">
                            <%--                        <!- Spacing per il secondo gruppo ->--%>
                            <asp:CheckBox ID="CkClassificatore" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkClassificatore">Classificatore</label>
                        </div>

                        <div style="margin-right: 10px;">
                            <%--                        <!- Spacing per il terzo gruppo ->--%>
                            <asp:CheckBox ID="CkComodino" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkComodino">Comodino</label>
                        </div>


                        <div style="margin-right: 10px;">
                            <%--                        <!- Nessun margin-right per l'ultimo gruppo ->--%>
                            <asp:CheckBox ID="CkMacchinaDaScrivere" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkMacchinaDaScrivere">Macchina da scriv.</label>
                        </div>
                        <div style="margin-right: 10px;">
                            <%--                        <!- Nessun margin-right per l'ultimo gruppo ->--%>
                            <asp:CheckBox ID="CkMacchinaFotogr" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label ms-2" for="CkMacchinaFotogr">Macchina fotografica</label>
                        </div>

                    </div>
                </div>
                <div class="row">
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button ID="btEstrai" Text="Estrai" runat="server" OnClick="btEstrai_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
            </div>


            <asp:Button ID="btnSalva" runat="server" Text="Salva" CommandName="Save" CssClass="btn btn-primary btn-sm" OnClick="Salva_Click" />
            <asp:Button ID="btnStampa" runat="server" Text="Stampa" CssClass="btn btn-primary btn-sm" OnClick="btStampa_Click" />
            <asp:Button ID="btnFiltra" runat="server" Text="Filtri" CssClass="btn btn-primary btn-sm" OnClientClick="VisibleDiv()" />
        </div>

        <!-- Sezione GridView estesa a 100% -->
        <div id="DivGrid" runat="server" class="section-box w-100">
            <div class="d-flex justify-content-between align-items-center mb-2 px-1">
                <div class="small text-muted">
                    <asp:Label ID="lblInfoPagine" runat="server" Text="Pagina"></asp:Label>
                    <strong>
                        <asp:Label ID="lblNumRighe" runat="server" Text=""></asp:Label>
                    </strong>
                </div>
            </div>

            <div class="table-responsive w-100">
                <asp:GridView ID="gvDett" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered gridview-autofit gridview-full"
                    DataKeyNames="id_inventario" Width="100%"
                    OnRowDataBound="gvDett_RowDataBound"
                    OnRowCommand="gvDett_RowCommand"
                    OnDataBound="gvDett_DataBound"
                    AllowPaging="true" PageSize="10"
                    OnPageIndexChanging="gvDett_PageIndexChanging"
                    RowStyle-CssClass="GridViewRow"
                    AlternatingRowStyle-CssClass="GridViewAlternatingRow"
                    PagerSettings-Position="Top"
                    PagerSettings-Mode="NextPreviousFirstLast"
                    PagerSettings-FirstPageText="&laquo; Prima"
                    PagerSettings-LastPageText="Ultima &raquo;"
                    PagerSettings-NextPageText="Succ. &rsaquo;"
                    PagerSettings-PreviousPageText="&lsaquo; Prec.">

                    <Columns>
                        <asp:BoundField DataField="id_inventario" HeaderText="ID" Visible="false" />

                        <asp:TemplateField HeaderText="CodStanza">
                            <HeaderTemplate>
                                Codice Stanza<br />
                                <asp:TextBox ID="txtFilterCodep" runat="server" OnTextChanged="txtFilterCodep_TextChanged" AutoPostBack="True" ToolTip="Filtra..." Width="50px" CssClass="form-control form-control-sm" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("codep") %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Ufficio">
                            <HeaderTemplate>
                                Ufficio<br />
                                <asp:TextBox ID="txtFilterUfficio" runat="server" OnTextChanged="txtFilterUfficio_TextChanged" AutoPostBack="True" ToolTip="Filtra..." Width="50px" CssClass="form-control form-control-sm" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("ufficio") %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Etichetta">
                            <HeaderTemplate>
                                Etichetta<br />
                                <asp:TextBox ID="txtFilterEtichetta" runat="server" OnTextChanged="txtFilterEtichetta_TextChanged" AutoPostBack="True" ToolTip="Filtra..." Width="50px" CssClass="form-control form-control-sm" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("Etichetta") %></ItemTemplate>

                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Descrizione">
                            <HeaderTemplate>
                                Descrizione<br />
                                <asp:TextBox ID="txtFilterDescrizione" runat="server" OnTextChanged="txtFilterDescrizione_TextChanged" AutoPostBack="True" ToolTip="Filtra..." Width="50px" CssClass="form-control form-control-sm" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("Descrizione") %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Categoria">
                            <HeaderTemplate>
                                Categoria<br />
                                <asp:TextBox ID="txtFilterCategoria" runat="server" OnTextChanged="txtFilterCategoria_TextChanged" AutoPostBack="True" ToolTip="Filtra..." Width="50px" CssClass="form-control form-control-sm" placeholder="Filtra..." />

                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("Categoria") %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tipologia">
                            <HeaderTemplate>
                                Tipologia<br />
                                <asp:TextBox ID="txtFilterTipologia" runat="server" OnTextChanged="txtFilterTipologia_TextChanged" AutoPostBack="True" ToolTip="Filtra..." Width="50px" CssClass="form-control form-control-sm" placeholder="Filtra..." />

                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("Tipologia") %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Seriale">
                            <HeaderTemplate>
                                Seriale<br />
                                <asp:TextBox ID="txtFilterSeriale" runat="server" OnTextChanged="txtFilterSeriale_TextChanged" AutoPostBack="True" ToolTip="Filtra..." Width="50px" CssClass="form-control form-control-sm" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("Seriale") %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Dismesso" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span>
                                    <%# Eval("Dismesso") != DBNull.Value && Convert.ToBoolean(Eval("Dismesso")) ? "SÌ" : "NO" %>
                                </span>
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Note">
                            <HeaderTemplate>
                                Note<br />
                                <asp:TextBox ID="txtFilterNote" runat="server" OnTextChanged="txtFilterNote_TextChanged" AutoPostBack="True" ToolTip="Filtra..." Width="50px" CssClass="form-control form-control-sm" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("Note") %></ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnSelect" runat="server" Text="Modifica"
                                    CommandName="Select"
                                    CommandArgument='<%# Eval("id_inventario") %>'
                                    CssClass="btn btn-success btn-sm" />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnSelect" runat="server" Text="Visualizza"
                                    CommandName="View"
                                    CommandArgument='<%# Eval("id_inventario") %>'
                                    CssClass="btn btn-success btn-sm" />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Button ID="btnModifica" runat="server" Text="Mod." CommandName="Edit" CssClass="btn btn-warning btn-sm" CommandArgument='<%# Eval("id_inventario")%>' />
                            </ItemTemplate>
                            
                        </asp:TemplateField>--%>
                    </Columns>

                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
            </div>

            <div class="modal-footer bg-light">
                <asp:Button ID="btBack" runat="server" class="btn btn-secondary" Text="Azzera Filtri" OnClick="btBack_Click" ToolTip="Torna alla lista completa" Visible="false" />
            </div>
        </div>

        <asp:HiddenField ID="HfFiltroTipologia" runat="server" />
        <asp:HiddenField ID="HfFiltroCategoria" runat="server" />
        <asp:HiddenField ID="HfFiltroDescrizione" runat="server" />
        <asp:HiddenField ID="HfFiltroEtichetta" runat="server" />
        <asp:HiddenField ID="Hfid" runat="server" />
        <asp:HiddenField ID="HfFiltroCodep" runat="server" />
        <asp:HiddenField ID="HfFiltroNote" runat="server" />
        <asp:HiddenField ID="HfFiltroSeriale" runat="server" />
        <asp:HiddenField ID="HfFiltroUfficio" runat="server" />
        <asp:HiddenField ID="hfMod" runat="server" />
    </div>

    <!-- Modali -->
    <div class="modal fade" id="errorModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">ATTENZIONE</h5>
                </div>
                <div class="modal-body text-center">
                    <p id="errorMessage" style="color: red; font-size: 1.2rem; font-weight: bold;"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="HideErrorMessage()">Chiudi</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalProtocollo" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-warning">
                    <h5 class="modal-title">AVVISO</h5>
                </div>
                <div class="modal-body text-center">
                    <label for="txtProtocollo">Protocollo Uscita</label>
                    <asp:TextBox ID="txtProtocollo" runat="server" CssClass="form-group"></asp:TextBox>
                    <label for="txtDataProt">Del</label>
                    <asp:TextBox ID="txtDataProt" runat="server" CssClass="form-group data-auto"></asp:TextBox>
                    <label for="txtProtocolloRiscontro">Protocollo Riscontro</label>
                    <asp:TextBox ID="txtProtocolloRiscontro" runat="server" CssClass="form-group"></asp:TextBox>
                    <label for="txtDataProtRiscontro">Del</label>
                    <asp:TextBox ID="txtDataProtRiscontro" runat="server" CssClass="form-group data-auto"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btChiudiAvvertenze" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="btChiudiAvvertenze_Click" />
                    <asp:Button ID="btContinua" runat="server" class="btn btn-secondary" Text="Continua" OnClick="btContinua_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
