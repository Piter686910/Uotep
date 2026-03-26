<%@ Page Title="Dashboard Amministratore" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Uotep._Dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   

    <script>
        function ShowErrorMessage(message) {
            document.getElementById('errorMessage').innerText = message;
            $('#errorModal').modal('show');
        }
    </script>

    <div class="container-fluid mt-4">
        <div class="dashboard-header">
            <h1><span class="glyphicon glyphicon-cog"></span>DASHBOARD AMMINISTRATORE</h1>
        </div>

        <asp:Panel ID="pnlGestUtenti" runat="server">

            <div class="row">
                <div id="divNewUtente" runat="server" visible="false" class="col-md-6">
                    <div class="section-box">
                        <h4 class="mb-4" style="color: #337ab7; font-weight: bold;">Dati Operatore</h4>
                        <div class="form-group mb-3">
                            <asp:Label ID="lblm" runat="server" Text="Matricola" />
                            <asp:TextBox ID="TxtMatricola" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label1" runat="server" Text="Profilo" />
                            <asp:TextBox ID="TxtProfilo" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label2" runat="server" Text="Area" />
                            <asp:TextBox ID="txtArea" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label4" runat="server" Text="Macro Area" />
                            <asp:TextBox ID="txtMacroArea" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label7" runat="server" Text="Nominativo" />
                            <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>

                <div id="divDestra" runat="server" visible="false" class="col-md-6">
                    <div class="section-box">
                        <h4 class="mb-4" style="color: #337ab7; font-weight: bold;">Configurazione Ruolo</h4>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label5" runat="server" Text="Nota" />
                            <asp:TextBox ID="TxtNota" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label6" runat="server" Text="Ruolo" />
                            <asp:DropDownList ID="DdlRuolo" runat="server" CssClass="form-control">
                                <asp:ListItem Text="admin" />
                                <asp:ListItem Text="accertatori" />
                                <asp:ListItem Text="archivio" />
                                <asp:ListItem Text="coordinamentoatti" />
                                <asp:ListItem Text="coordinamentopg" />
                                <asp:ListItem Text="Fureria" />
                                <asp:ListItem Text="PG" />
                                <asp:ListItem Text="superAdmin" />
                            </asp:DropDownList>
                        </div>
                        <div class="form-group mb-4">
                            <asp:Label ID="Label8" runat="server" Text="Elenco Personale" />
                            <asp:DropDownList ID="DdlPersonale" runat="server" CssClass="form-control" />
                        </div>
                        <asp:Button Text="SALVA OPERATORE" runat="server" OnClick="InsOpetratore_Click" CssClass="btn btn-primary w-100" />
                    </div>
                </div>
            </div>

            <div id="divCheck" runat="server" visible="false" class="section-box">
                <div class="row d-flex align-items-end">
                    <div class="col-md-4">
                        <div class="form-group mb-0">
                            <asp:Label ID="Label9" runat="server" Text="Numero Pratica" />
                            <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-0">
                            <asp:Label ID="Label10" runat="server" Text="Anno" />
                            <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-0">
                            <asp:Button Text="AVVIA RICERCA" runat="server" OnClick="Cerca_Click" CssClass="btn btn-primary w-100 btn-allineato" />
                        </div>
                    </div>
                </div>

                <div class="table-responsive mt-4">
                    <asp:GridView ID="GVcheck" runat="server"
                        CssClass="table table-bordered table-hover compact-grid"
                        OnRowDataBound="GVcheck_RowDataBound"
                        OnRowCommand="GVcheck_RowCommand"
                        AllowPaging="true" PageSize="10"
                        OnPageIndexChanging="GVcheck_PageIndexChanging"
                        RowStyle-CssClass="GridViewRow"
                        AlternatingRowStyle-CssClass="GridViewAlternatingRow">
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>

            <div id="divReset" runat="server" visible="false" class="section-box text-center">
                <div class="row justify-content-center">
                    <div class="col-md-4">
                        <asp:Label ID="Label3" runat="server" Text="Matricola per Reset" CssClass="fw-bold" />
                        <asp:TextBox ID="txtResetMatricola" runat="server" CssClass="form-control mt-2" />
                        <div class="mt-3">
                            <asp:Button Text="RESET PASSWORD" runat="server" OnClick="ModificaP_Click" CssClass="btn btn-warning px-4" />
                            <asp:Button Text="ELIMINA UTENTE" runat="server" OnClick="Elimina_Click" CssClass="btn btn-danger px-4" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group text-center mt-4 mb-5">
                <asp:Button Text="Reset Password" runat="server" OnClick="Reset_Click" CssClass="btn btn-primary mx-2" />
                <asp:Button Text="Nuovo Utente" runat="server" OnClick="NuovoUt_Click" CssClass="btn btn-primary mx-2" />
                <asp:Button Text="Verifica Accessi" runat="server" OnClick="Check_Click" CssClass="btn btn-primary mx-2" />
            </div>

        </asp:Panel>
    </div>

    <%-- Popup Errori --%>
    <div class="modal fade" id="errorModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">ATTENZIONE</h5>
                </div>
                <div class="modal-body text-center">
                    <p id="errorMessage" class="lead"></p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button2" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                </div>
            </div>
        </div>
    </div>
     
</asp:Content>
