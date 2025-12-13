<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Turnazione.aspx.cs" Inherits="Uotep.Turnazione" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        // Mostra il popup 
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        // Nasconde il popup
        function HideErrorMessage() {
            $('#errorModal').modal('hide');
        }
    </script>
    <style>
        /* Stile per l'intestazione della colonna (il giorno) */
        .giorno-festivo-header {
            background-color: #f7d5d7 !important; /* Rosso chiaro */
            color: #b70014; /* Testo Rosso */
            font-weight: bold;
            border-bottom: 2px solid #b70014;
        }

        /* Stile per le celle della griglia che corrispondono al giorno festivo/weekend */
        .giorno-festivo-cella {
            background-color: #ffeaea; /* Rosso chiarissimo */
        }

            /* Assicurati che le TextBox nelle celle modificate abbiano lo stesso sfondo festivo */
            .giorno-festivo-cella input[type="text"] {
                background-color: #ffeaea;
            }

        /* Stile aggiuntivo per mantenere il testo dell'header centrato */
        .text-center {
            text-align: center !important;
        }

        .gruppo-ufficio-header {
            background-color: #e9ecef; /* Un grigio chiaro, simile a thead */
            font-weight: bold;
            color: #495057;
            font-size: 1.1em;
            text-align: left; /* O 'center' se preferisci */
            padding-left: 10px;
        }

        .group-separator td {
            border-top: 2px solid #333 !important; /* Bordo superiore scuro e ben visibile */
        }

        .ufficio-separator-row td {
            background-color: #343a40; /* Un grigio più scuro e deciso */
            color: white;
            font-weight: bold;
            padding: 8px 12px; /* Un po' più di padding */
            text-transform: uppercase; /* Rende il titolo più evidente */
            font-size: 1em;
        }
    </style>

    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">TURNAZIONE PER IL MESE DI</p>
        </div>
        
        <%-- MODIFICA: Utilizza container-fluid per occupare l'intera larghezza disponibile e rimuovi il margine negativo --%>
        <div class="container-fluid">
            <asp:Literal ID="ltlDebug" runat="server" EnableViewState="false"></asp:Literal>
            <!-- GridView  -->
            <asp:UpdatePanel ID="updPanelGrid" runat="server">
                 
                <ContentTemplate>
                    <%-- Aggiunto un div class="row" per un corretto allineamento dei controlli --%>
                    <div class="row">
                        <div class="col-md-4" style="margin-bottom: 10px; margin-top: 20px; padding-left: 2em">
                            <asp:Label ID="lblMese" runat="server" Text="Seleziona Mese/Anno: "></asp:Label>
                            <asp:DropDownList ID="ddlMese" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlMese_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-4" style="margin-bottom: 10px; margin-top: 40px; padding-left: 2em">
                            <%--<asp:DropDownList ID="ddlAnno" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlAnno_SelectedIndexChanged"></asp:DropDownList>--%>
                            <asp:TextBox ID="txtAnno" runat="server"  CssClass="form-control"></asp:TextBox>

                        </div>
                        <div class="col-md-4" style="margin-bottom: 10px; margin-top: 40px; padding-left: 2em">
                            <asp:Button ID="btnCarica" runat="server" Text="Carica Griglia" CssClass="btn btn-primary" OnClick="btnCarica_Click" />
                        </div>
                        <div class="col-md-4" style="margin-bottom: 10px; margin-top: 40px; padding-left: 2em">
                            <asp:Button ID="btnsalva" runat="server" Text="Salva Turnazione" CssClass="btn btn-primary" OnClick="btnsalva_Click" Enabled="false"/>
                        </div>
                        <asp:Label ID="lblErrore" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <asp:GridView ID="gvCalendario" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-striped"
                        OnRowCommand="gvCalendario_RowCommand"
                        OnRowEditing="gvCalendario_RowEditing"
                        OnRowUpdating="gvCalendario_RowUpdating"
                        ShowHeader="false">
                        <Columns>
                            <asp:BoundField DataField="Nominativo" HeaderText="Dipendente" ReadOnly="True" />

                            <asp:CommandField ShowEditButton="True" HeaderText="Azioni" />
                        </Columns>
                    </asp:GridView>
                    <%--<asp:GridView ID="gvCalendario" runat="server"
                        AutoGenerateColumns="False"
                        DataKeyNames="id_dip"
                        AllowEditing="True"
                        OnRowEditing="gvCalendario_RowEditing"
                        OnRowUpdating="gvCalendario_RowUpdating"
                        OnRowCancelingEdit="gvCalendario_RowCancelingEdit"
                        
                       OnPreRender="gvCalendario_PreRender"
                        CssClass="table table-bordered table-striped"
                        UseAccessibleHeader="true"
                        HeaderStyle-CssClass="thead-light"
                        OnRowDataBound="gvCalendario_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Nominativo" HeaderText="Dipendente" ReadOnly="True" />
                            <%-- Le colonne dei giorni verranno aggiunte dinamicamente dal code-behind 
                    <asp:CommandField ShowEditButton="True" HeaderText="Azioni" />
                    </Columns>
                    </asp:GridView>--%>

                   
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%-- popup errori --%>
    <div class="modal fade" id="errorModal" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-hidden="true">
        <div class="modal-dialog">
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

  
    <script type="text/javascript">
        //dice alla masterpage di trasformare il container in container-fluid
        $(document).ready(function () {
            // Cerca il container genitore (solitamente ha la classe .container e .body-content)
            // e sostituisce la classe 'container' con 'container-fluid'
            var mainContainer = $('#<%= updPanelGrid.ClientID %>').closest('.container');
            if (mainContainer.length > 0) {
                mainContainer.removeClass('container').addClass('container-fluid');
            }
        });
    </script>
</asp:Content>
