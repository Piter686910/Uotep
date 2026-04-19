<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatistichePg.aspx.cs" Inherits="Uotep.StatistichePg" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        // Nasconde il popup
        function hideModal() {
            $('#ModalInterrogatori').modal('hide');

        }
    </script>

    <div class="panel panel-default">
        <div class="form-group mb-3"></div>

        <div class="panel-body" id="divStat" runat="server">
            <div class="jumbotron">
                <div style="margin-top: -90px!important">
                    <%--<asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>--%>
                    <%--<p class="text-center lead">INSERISCI STATISTICHE</p>--%>
                    <div class="dashboard-header">
                        <h1><span class="fa-solid fa-gear fa-spin"></span> INSERISCI INTERROGATORI</h1>
                    </div>
                </div>

                <div class="container">
                    <div class="row align-items-end">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtMM">Mese</label>
                                <%-- <asp:TextBox ID="txtMM" runat="server" CssClass="form-control" Width="110px" autofocus="" />--%>
                                <asp:DropDownList ID="ddlMese" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">Seleziona mese</asp:ListItem>
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
                                <%-- <asp:RequiredFieldValidator ID="rqMM" runat="server" ControlToValidate="txtMM" ErrorMessage="inserire il mese" ValidationGroup="bt" ForeColor="Red">
                                </asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtYYYY">Anno</label>
                                <asp:TextBox ID="txtYYYY" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                            <asp:RequiredFieldValidator ID="rqAnno" runat="server" ControlToValidate="txtYYYY" ErrorMessage="inserire l'anno" ValidationGroup="bt" ForeColor="Red">
                            </asp:RequiredFieldValidator>

                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtInterrogatorio">Interrogatori</label>
                                <asp:TextBox ID="txtInterrogatorio" runat="server" CssClass="form-control larghezzaText70" />
                                <asp:RequiredFieldValidator ID="rqInterrogatorio" runat="server" ControlToValidate="txtInterrogatorio" ErrorMessage="inserire un numero" ValidationGroup="bt" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row align-items-end">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtDataInterr">Data</label>
                                <asp:TextBox ID="txtDataInterr" runat="server" CssClass="form-control larghezzaText data-auto" placeholder="gg/mm/yyyy" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtPratica">Pratica</label>
                                <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtProcPen">Proc. Penale</label>
                                <asp:TextBox ID="txtProcPen" runat="server" CssClass="form-control larghezzaText" />
                            </div>
                        </div>
                    </div>


                    <div class="row align-items-end">
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="txtNominativo1">Nominativo</label>
                                <asp:TextBox ID="txtNominativo1" runat="server" CssClass="form-control larghezzaText180" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="txtNominativo2">Nominativo</label>
                                <asp:TextBox ID="txtNominativo2" runat="server" CssClass="form-control larghezzaText180" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="txtNominativo3">Nominativo</label>
                                <asp:TextBox ID="txtNominativo3" runat="server" CssClass="form-control larghezzaText180" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="txtNominativo4">Nominativo</label>
                                <asp:TextBox ID="txtNominativo4" runat="server" CssClass="form-control larghezzaText180" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Bottone Salva -->
                <div class="col-md-4 ">
                    <div class="form-group">
                        <asp:Button ID="btSalva" runat="server" ValidationGroup="bt" Text="💾 Inserisci" CssClass="btn btn-primary" OnClick="btInserisci_Click" />
                        <asp:Button ID="btCerca" runat="server" Text="📂 Ricerca" CssClass="btn btn-primary" OnClick="btCerca_Click" />
                    </div>
                </div>
            </div>
        </div>

    </div>

    <div class="modal fade" id="ModalInterrogatori" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 100%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel2">Interrogatori</h5>

                </div>


                <!-- GridView Interrogatori -->
                <div id="DivInterrogatori" runat="server" class="form-group" style="padding-left: -50px">

                    <asp:GridView ID="gvInterrogatori" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered"
                        AllowPaging="true"
                        PageSize="10"
                        DataKeyNames="id"
                        OnRowDataBound="gvInterrogatori_RowDataBound"
                        OnPageIndexChanging="gvInterrogatori_PageIndexChanging"
                        OnRowEditing="gvInterrogatori_RowEditing"
                        OnRowCancelingEdit="gvInterrogatori_RowCancelingEdit"
                        OnRowDeleting="gvInterrogatori_RowDeleting"
                        OnRowUpdating="gvInterrogatori_RowUpdating"
                        RowStyle-CssClass="GridViewRow"
                        AlternatingRowStyle-CssClass="GridViewAlternatingRow">

                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" Visible="false" ReadOnly="true" />

                            <%--  CAMPO EDITABILE: ProcPenale --%>
                            <asp:TemplateField HeaderText="Proc. Penale" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <%# Eval("ProcPenale") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtProcPenGrid" runat="server" Text='<%# Bind("ProcPenale") %>' CssClass="form-control input-sm"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%--  CAMPO DATA EDITABILE --%>
                            <asp:TemplateField HeaderText="Data Interr." ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%# Eval("DataInterrogatorio") != DBNull.Value ? string.Format("{0:dd/MM/yyyy}", Eval("DataInterrogatorio")) : "" %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDataInterrogatorioGrid" runat="server" Text='<%# Bind("DataInterrogatorio", "{0:dd/MM/yyyy}") %>' CssClass="form-control input-sm" placeholder="gg/mm/yyyy"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Nr Pratica">
                                <ItemTemplate><%# Eval("Npratica") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNPraticaGrid" runat="server" Text='<%# Bind("Npratica") %>' CssClass="form-control input-sm" Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Nominativo">
                                <ItemTemplate><%# Eval("Nominativo1") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNominativo1Grid" runat="server" Text='<%# Bind("Nominativo1") %>' CssClass="form-control input-sm"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Nominativo">
                                <ItemTemplate><%# Eval("Nominativo2") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNominativo2Grid" runat="server" Text='<%# Bind("Nominativo2") %>' CssClass="form-control input-sm"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Nominativo">
                                <ItemTemplate><%# Eval("Nominativo3") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNominativo3Grid" runat="server" Text='<%# Bind("Nominativo3") %>' CssClass="form-control input-sm"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Nominativo">
                                <ItemTemplate><%# Eval("Nominativo4") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNominativo4Grid" runat="server" Text='<%# Bind("Nominativo4") %>' CssClass="form-control input-sm"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Inserim." ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <%# Eval("DataInserimento") != DBNull.Value ? string.Format("{0:dd/MM/yyyy}", Eval("DataInserimento")) : "" %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDataInserimentoGrid" runat="server" Text='<%# Bind("DataInserimento", "{0:dd/MM/yyyy}") %>' CssClass="form-control input-sm" placeholder="gg/mm/yyyy"></asp:TextBox>
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
                                    <asp:Button ID="btnElimina" runat="server" Text="Del." CommandName="Delete" CommandArgument='<%# Eval("id") %>' CssClass="btn btn-danger btn-sm" />
                                    <%-- OnClientClick="return confirm('Sei sicuro di voler eliminare questa riga?');" --%>
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
                        <asp:Button ID="Button3" runat="server" class="btn btn-secondary" Text="Chiudi" OnClientClick="hideModal()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
