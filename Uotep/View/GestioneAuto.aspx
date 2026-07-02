<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestioneAuto.aspx.cs" Inherits="Uotep.GestioneAuto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .larghezzaText {
            width: 100%;
            font-size: 0.9rem !important;
            padding: 5px !important;
            color: #333;
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
        function ShowAvvertenze(message) {
            $('#txtAvvertenze').text(message);
            $('#ModalAvvertenze').modal('show');
        }
    </script>

    <div class="container-fluid mt-4">
        <div class="dashboard-header">
            <h1><span class="fa-solid fa-gear fa-spin"></span> GESTIONE CARTE CARBURANTE</h1>
        </div>

        <div class="section-box">
            <div class="row d-flex align-items-end">
                <div class="col-md-2">
                    <div class="form-group mb-0">
                        <label>Mese</label>
                        <asp:TextBox ID="txtMese" runat="server" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="col-md-2">

                    <div class="form-group mb-0">
                        <label>Anno</label>
                        <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="col-md-8 text-right">
                    <asp:Button ID="btSalva" Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary ml-2" ValidationGroup="bt" />
                    <asp:Button ID="btCerca" Text="Cerca" runat="server" OnClick="btCerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary ml-2" />
                    <asp:Button ID="btStampa" Text="Stampa" runat="server" OnClick="btStampa_Click" ToolTip="Stampa" CssClass="btn btn-primary ml-2" />
                </div>
            </div>

        </div>

        <div class="section-box" id="divInserimento" runat="server">
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="DdlSigla">Sigla</label>
                        <asp:DropDownList ID="DdlSigla" runat="server" CssClass="form-control" OnSelectedIndexChanged="DdlSigla_SelectedIndexChanged1" AutoPostBack="true" />
                    </div>
                    <div class="form-group">
                        <label for="txtAutista">Autista</label>
                        <asp:TextBox ID="txtAutista" runat="server" CssClass="form-control" />
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="txtTarga">Targa</label>
                        <asp:TextBox ID="txtTarga" runat="server" CssClass="form-control" Font-Bold="true" />
                    </div>
                    <div class="form-group">
                        <label for="TxtData">Data</label>
                        <asp:TextBox ID="TxtData" runat="server" CssClass="form-control data-auto" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtData" ValidationGroup="bt" ErrorMessage="Inserire data" ForeColor="Red" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtData" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$" ErrorMessage="Usa dd/mm/aaaa" ForeColor="Red" ValidationGroup="bt" Display="Dynamic" />
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="DdlCarburante">Tipo Carburante</label>
                        <asp:DropDownList ID="DdlCarburante" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Benzina" />
                            <asp:ListItem Text="Diesel" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="txtOra">Ora</label>
                        <asp:TextBox ID="txtOra" runat="server" CssClass="form-control" placeholder="HH:MM" />
                        <asp:RegularExpressionValidator ID="RegexOra" runat="server" ControlToValidate="txtOra" ValidationExpression="^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$" ErrorMessage="Formato non valido" ForeColor="Red" ValidationGroup="bt" Display="Dynamic" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Stan</label>
                        <asp:TextBox ID="txtStan" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStan" ValidationGroup="bt" ErrorMessage="Inserire Stan" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Litri</label>
                        <asp:TextBox ID="txtLitri" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLitri" ValidationGroup="bt" ErrorMessage="Inserire Litri" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Euro</label>
                        <asp:TextBox ID="txtEuro" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEuro" ValidationGroup="bt" ErrorMessage="Inserire Euro" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Indirizzo Distributore</label>
                        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>
        </div>

        <div id="DivGrid" runat="server" class="section-box">
            <div class="d-flex justify-content-between align-items-center mb-2 px-1">
                <div class="small text-muted">
                    <asp:Label ID="lblInfoPagine" runat="server" Text="Pagina"></asp:Label>
                     <strong><asp:Label ID="lblNumRighe" runat="server" Text=""></asp:Label></strong>
                </div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvDett" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover"
                    OnRowDataBound="gvDett_RowDataBound" OnRowCommand="gvDett_RowCommand"
                    OnDataBound="gvDett_DataBound"
                    AllowPaging="true" PageSize="10" OnPageIndexChanging="gvDett_PageIndexChanging"
                    RowStyle-CssClass="GridViewRow"
                    AlternatingRowStyle-CssClass="GridViewAlternatingRow"
                    PagerSettings-Position="Top"
                    PagerSettings-Mode="NextPreviousFirstLast"
                    PagerSettings-FirstPageText="&laquo; Prima"
                    PagerSettings-LastPageText="Ultima &raquo;"
                    PagerSettings-NextPageText="Succ. &rsaquo;"
                    PagerSettings-PreviousPageText="&lsaquo; Prec.">

                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />

                        <asp:TemplateField HeaderText="Sigla">
                            <HeaderTemplate>
                                Sigla<br />
                                <asp:TextBox ID="txtFilterSigla" runat="server" OnTextChanged="txtFilterSigla_TextChanged" AutoPostBack="True" CssClass="larghezzaText" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("sigla") %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="targa" HeaderText="Targa" />
                        <asp:BoundField DataField="stan" HeaderText="STAN" ItemStyle-HorizontalAlign="Center" />

                        <asp:TemplateField HeaderText="Data">
                            <HeaderTemplate>
                                Data<br />
                                <asp:TextBox ID="txtFilterData" runat="server" OnTextChanged="txtFilterData_TextChanged" AutoPostBack="True" CssClass="larghezzaText" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("data", "{0:dd/MM/yyyy}") %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Ora">
                            <ItemTemplate>
                                '<%# Eval("ora") != null ? Eval("ora").ToString().Substring(0, 5) : "" %>'
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="litri" HeaderText="Litri" />
                        <asp:BoundField DataField="euro" HeaderText="Euro" />
                        <asp:BoundField DataField="indirizzo" HeaderText="Indirizzo" />

                        <asp:TemplateField HeaderText="Verificato" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span>
                                    <%# Eval("verificato").ToString() == "True" ? "SÌ" : "NO" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnSelect" runat="server" Text="OK"
                                    CommandName="Select"
                                    CommandArgument='<%# Eval("id") + "|" + Eval("targa") + "|" + Eval("data") + "|" + Eval("autista") %>'
                                    CssClass="btn btn-success btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
            </div>
        </div>

        <asp:HiddenField ID="Hfuser" runat="server" />
        <asp:HiddenField ID="HfFiltroData" runat="server" />
        <asp:HiddenField ID="HfFiltroSigla" runat="server" />

    </div>

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

    <div class="modal fade" id="ModalAvvertenze" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-warning">
                    <h5 class="modal-title">AVVISO</h5>
                </div>
                <div class="modal-body text-center">
                    <p id="txtAvvertenze" style="color: #856404; font-size: 1.2rem; font-weight: bold;"></p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btChiudiAvvertenze" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="btChiudiAvvertenze_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
