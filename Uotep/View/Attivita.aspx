<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attivita.aspx.cs" Inherits="Uotep._Attivita" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }

    </script>
    <style>
        .uppercase-text {
            text-transform: uppercase;
        }

        .GridViewRow {
            background-color: white;
        }

        /* Stile per la riga alternata (azzurro chiaro) */
        .GridViewAlternatingRow {
            background-color: #E6F3FF; /* Un azzurro molto chiaro */
            /* background-color: #F0F8FF;  Un altro azzurro molto chiaro (AliceBlue) */
        }
    </style>
    <div class="jumbotron" style="text-align-last:center">
        <h1>Pratiche Assegnate</h1>
        <p class="lead"></p>
    </div>
    <%-- LOGIN --%>
    <asp:Panel ID="pnlAttivita" runat="server" CssClass="text-center">
        <div class="row d-flex justify-content-center align-items-center vh-100">
            <div class="container">
                <div class="row">

                    <asp:Button ID="InCarico" Text="Attività In Carico" runat="server" OnClick="btAttivitaInCarico_Click" ToolTip="Attività In Carico" CssClass="btn btn-primary px-4" />
                    <asp:Button ID="Concluse" Text="Attività Concluse" runat="server" OnClick="btAttivitaConcluse_Click" ToolTip="Attività Concluse" CssClass="btn btn-primary px-4" />


                </div>
            </div>

            <div class="form-group">

                <asp:GridView ID="GVAttivita" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                    OnRowDataBound="gvPopup_RowDataBoundP" OnRowCommand="gvPopup_RowCommandP" AllowPaging="true" PageSize="10" OnPageIndexChanging="GVAttivita_PageIndexChanging" RowStyle-CssClass="GridViewRow"
                    AlternatingRowStyle-CssClass="GridViewAlternatingRow">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="nr_protocollo" HeaderText="Pratica/Cart." HeaderStyle-CssClass="wrap-text" />
                        <asp:BoundField DataField="quartiere" HeaderText="Quartiere" HeaderStyle-CssClass="wrap-text-40" ItemStyle-CssClass="uppercase-text" />
                        <asp:BoundField DataField="decr_decretato" HeaderText="Assegnato" HeaderStyle-CssClass="wrap-text-40" ItemStyle-CssClass="uppercase-text" />

                        <asp:TemplateField HeaderText="Trasmesso" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Eval("decr_chiuso").ToString() == "True" ? "Si" : "No" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Chiusura" ItemStyle-CssClass="uppercase-text">
                            <%-- data default non visualizzata --%>
                            <ItemTemplate>
                                <%# Eval("decr_dataChiusura") is DBNull ? "" : (Convert.ToDateTime(Eval("decr_dataChiusura")).Year == 1900) ? "" : Eval("decr_dataChiusura", "{0:d}") %>
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
    </asp:Panel>

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
</asp:Content>
