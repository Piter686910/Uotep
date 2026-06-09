<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RicercaArchivioUotp.aspx.cs" Inherits="Uotep.RicercaArchivioUotp" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <style>
        /*.wrap-text {
            max-width: 10px;*/
        /* Proprietà standard per andare a capo */
        /*white-space: normal !important;*/
        /* Proprietà essenziale per spezzare parole lunghe e senza spazi */
        /*word-wrap: break-word !important;*/
        /* o in alternativa: word-break: break-all !important; */
        /*}

        .wrap-text-40 {
            max-width: 40px;*/
        /* Proprietà standard per andare a capo */
        /*white-space: normal !important;*/
        /* Proprietà essenziale per spezzare parole lunghe e senza spazi */
        /*}*/
        /* Stile per centrare orizzontalmente un elemento a blocco come una tabella */
                .center-table {
    margin-left: auto;
    margin-right: auto;
    /* In breve: margin: 0 auto; */
    /* Opzionale: potresti voler collassare i bordi se li usi */
    /* border-collapse: collapse; */
    /* Puoi usare border-spacing se vuoi spazio tra le celle (sia riga che colonna) */
    /* border-spacing: 0 15px; /* 0 orizzontale, 15px verticale */
}

    /* Stile per aggiungere spazio interno alle celle (padding), crea distanza tra i bottoni */
    /* Aggiunge padding a tutte le celle della tabella con classe center-table */
    .center-table td {
        padding-bottom: 15px; /* Aggiunge 15px di spazio SOTTO il contenuto della cella */
        padding-top: 5px; /* Opzionale: Aggiunge un po' di spazio SOPRA */
        /* Puoi anche aggiungere padding orizzontale se necessario, ma mx-2 sul bottone già lo fa */
        /* padding-left: 5px; */
        /* padding-right: 5px; */
    }
    </style>
    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <%-- <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">RICERCA UNA PRATICA</p>--%>
            <div class="dashboard-header">
                <h1><span class="fa-solid fa-gear fa-spin"></span> RICERCA UNA PRATICA IN ARCHIVIO PATRIMONIO</h1>
            </div>
            <!-- Contenitore per centrare -->

            <asp:Panel ID="pnlButton" runat="server" CssClass="text-center" Visible="true">
                <%--
        Il div sottostante con d-flex e justify-content-center sta cercando di centrare
        il contenuto interno usando Flexbox. Può essere utile per il mt-4 (margin top).
        Aggiungendo margin: 0 auto; alla tabella, centriamo la tabella stessa come blocco.
                --%>
                <div class="d-flex justify-content-center mt-4">
                    <%-- Inizio Tabella per i Pulsanti --%>
                    <table class="center-table">
                        
                        <tr>
                            <%-- Prima riga: 4 pulsanti --%>
                            <td>
                                <asp:Button ID="btOggetto" runat="server" OnClick="btOggetto_Click" Text="Oggetto" ToolTip="Ricerca Oggetto" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btDestinatario" runat="server" OnClick="btDestinatario_Click" Text="Destinatario" ToolTip="Ricerca Per Destinatarioo" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <%--<td>
                                <asp:Button ID="btDatiCatastali" runat="server" OnClick="btDatiCatastali_Click" Text="Dati Catastali" ToolTip="Dati Catastali" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>--%>
                            <td>
                                <asp:Button ID="btNCartellina" runat="server" OnClick="btNCartellina_Click" Text="Cartellina" ToolTip="Ricerca Per Quartiere e Cartelllina" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btIntestatario" runat="server" OnClick="btIntestatario_Click" Text="Intestatario" ToolTip="Ricerca Intestatario" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                        </tr>
                        <tr>
                            <%-- Seconda riga: altri 4 pulsanti --%>
                            <td>
                                <asp:Button ID="btNota" runat="server" OnClick="btNota_Click" Text="Nota" ToolTip="Ricerca Per Nota" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btBU" runat="server" OnClick="btBU_Click" Text="BU Alloggio" ToolTip="BU Alloggio" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <%-- <td>
                                <asp:Button ID="btEstraiParziale" runat="server" OnClick="btEstraiParziale_Click" Text="Estrazione Parziale" ToolTip="Estrazione Parziale" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>--%>
                            <td>
                                <asp:Button ID="btEdificio" runat="server" OnClick="btEdificio_Click" Text="Bu Edificio" ToolTip="Bu Edificio" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btIndirizzo" runat="server" OnClick="btIndirizzo_Click" Text="Indirizzo" ToolTip="Ricerca Indirizzo" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>

                        </tr>

                    </table>
                    <%-- Fine Tabella Pulsanti --%>
                </div>
            </asp:Panel>
        </div>

        <%--Sezione di ricerca  --%>
        <div id="DivRicerca" runat="server" class="row d-flex justify-content-center align-items-center vh-100" style="height: 300px; margin-left: 250px!important">
            <!-- Righe di input  -->
            <div class="col-md-4 ">

                <%-- DIV RICERCA pratica --%>
                <div id="DivPratica" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label2" runat="server" Text="Quartiere" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:TextBox ID="txtQuartiere" runat="server" CssClass="form-control" placeholder="Quartiere" />
                    <asp:Label ID="Label6" runat="server" Text="Cartellina" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:TextBox ID="txtCartellina" runat="server" CssClass="form-control" placeholder="Cartellina" autofocus="" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Oggetto --%>
                <div id="DivOggetto" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label11" runat="server" Text="Oggetto" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtOggetto" runat="server" CssClass="form-control" placeholder="Oggetto" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>

                <%-- DIV RICERCA Nota --%>
                <div id="DivNota" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label4" runat="server" Text="Nota" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtNota" runat="server" CssClass="form-control" placeholder="Nota" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA BU --%>
                <div id="DivBU" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label5" runat="server" Text="BU" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtBU" runat="server" CssClass="form-control" placeholder="BU Alloggio" autofocus="" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Destinatario --%>
                <div id="DivDestinatario" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label1" runat="server" Text="Destinatario" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDestinatario" runat="server" CssClass="form-control" placeholder="Destinatario" autofocus="" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Indirizzo --%>
                <div id="DivIndirizzo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label3" runat="server" Text="Indirizzo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" placeholder="Indirizzo" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Intestatario --%>
                <div id="DivIntestatario" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label7" runat="server" Text="Intestatario" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtIntestatario" runat="server" CssClass="form-control" placeholder="Intestatario" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Edificio --%>
                <div id="DivEdificio" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label8" runat="server" Text="Edificio" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtBuEdificio" runat="server" CssClass="form-control" placeholder="BU Edificio" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
            </div>

        </div>

        <%-- PANEL VISUALIZZAZIONE DETTAGLI DELLA RICERCA --%>
        <div id="pnDettagli" runat="server" class="panel panel-default" visible="false">
            <div class="panel-body">
                <div class="container">

                    <div class="tab-content">
                        <p style="font-weight: bold;">Informazioni Generali</p>

                        <div class="row custom-border">
                            <div class="col-md-6 ">
                                <div class="form-check mb-2">
                                    <label for="txtQuartiereTp">Quartiere</label>
                                    <asp:TextBox ID="txtQuartiereTp" runat="server" CssClass="form-control" Enabled="false" />


                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtCartellinaTp">Cartellina</label>
                                    <asp:TextBox ID="txtCartellinaTp" runat="server" CssClass="form-control" Enabled="false" ForeColor="Red" Font-Bold="true" />
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-check mb-2">

                                    <div class="form-check mb-2">
                                        <label for="txtDataInserimentoTp">Data Inserimento</label>
                                        <asp:TextBox ID="txtDataInserimentoTp" runat="server" CssClass="form-control data-auto" Enabled="false" Font-Bold="true" />


                                    </div>

                                </div>

                                <%--<div class="form-check mb-2">
                                    <label for="txtProProcTp">Prot. Proc.</label>
                                    <asp:TextBox ID="txtProProcTp" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtDataProtProc">Data Prot. Proc.</label>
                                    <asp:TextBox ID="txtDataProtProc" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false" />

                                </div>--%>
                            </div>


                        </div>
                    </div>
                    <div class="tab-content">
                        <p style="font-weight: bold;">Dettagli</p>
                        <div class="row custom-border">

                            <div class="col-md-6">
                                <div class="form-check mb-2">
                                    <label for="txtDestinatarioTp">Destinatario</label>
                                    <asp:TextBox ID="txtDestinatarioTp" runat="server" CssClass="form-control" Enabled="false" />
                                </div>
                                <%--<div class="form-check mb-2">
                                    <label for="txtQuartiereTp">Quartiere</label>
                                    <asp:TextBox ID="txtQuartiereTp" runat="server" CssClass="form-control" Enabled="false" />

                                </div>--%>
                                <div class="form-check mb-2">
                                    <label for="txtBUAlloggioTp">BU Alloggio</label>
                                    <asp:TextBox ID="txtBUAlloggioTp" runat="server" CssClass="form-control" Enabled="false" />
                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtBUAlloggioTp">BU Edificio</label>
                                    <asp:TextBox ID="txtBuEdificioTp" runat="server" CssClass="form-control" Enabled="false" />
                                </div>

                            </div>
                            <div class="col-md-6">
                                <div class="form-check mb-2">
                                    <label for="txtCognomeTp">Intestatario</label>
                                    <asp:TextBox ID="txtCognomeTp" runat="server" CssClass="form-control" Enabled="false" />
                                </div>

                                <%--<div class="form-check mb-2">
                                    <label for="txtCartellinaTp">Cartellina</label>
                                    <asp:TextBox ID="txtCartellinaTp" runat="server" CssClass="form-control" Enabled="false" />
                                </div>--%>
                                <div class="form-check mb-2">
                                    <label for="TxtIndirizzoTp">Indirizzo</label>
                                    <asp:TextBox ID="TxtIndirizzoTp" runat="server" CssClass="form-control" Enabled="false" />
                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtNotaTp">Nota</label>
                                    <asp:TextBox ID="txtNotaTp" runat="server" CssClass="form-control" Enabled="false" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-check mb-2">
                                    <label for="txtOggettoTp">Oggetto</label>
                                    <asp:TextBox ID="txtOggettoTp" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" Style="margin-left: -10px; width: 100%; max-width: 800px;" Enabled="false" />

                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-check mb-2">
                                    <label for="txtOggettoTp2">Oggetto 2</label>
                                    <asp:TextBox ID="txtOggettoTp2" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" Style="margin-left: -10px; width: 100%; max-width: 800px;" Enabled="false" />

                                </div>
                            </div>
                        </div>

                    </div>
                    <asp:Button ID="BtNewRicerca" Text="Nuova Ricerca" runat="server" OnClick="BtNewRicerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    <asp:Button ID="btModifica" Text="Modifica" runat="server" OnClick="btModifica_Click" ToolTip="Modifica" CssClass="btn btn-primary mt-3" />
                     <asp:Button id="btReturn" runat="server" Text="<< Back" OnClick="btReturn_Click" CssClass="btn btn-primary mt-3" />


                </div>
            </div>
        </div>
    </div>

    <%-- Modale ricerca pratica --%>
    <div class="modal fade" id="ModalPratica" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" style="width: 100%">
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title" id="modalLabel10">Ricerca Pratica</h5>

                </div>
                <div class="modal-body">
                    <div class="d-flex justify-content-between align-items-center mb-2 px-1">
                        <div class="small text-muted">
                            <asp:Label ID="lblInfoPagine" runat="server" Text="Pagina 1 di 10 "></asp:Label>
                            <strong><asp:Label ID="lblNumRighe" runat="server" Text=""></asp:Label></strong>
                        </div>
                        <strong> <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label></strong>            
                    </div>
                    <div class="table-responsive">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="GVRicercaPratica" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            OnRowCommand="GVRicercaPratica_RowCommand" AllowPaging="true" PageSize="10"
                            OnPageIndexChanging="GVRicercaPratica_PageIndexChanging"
                            OnDataBound="GVRicercaPratica_DataBound"
                            OnRowDataBound="GVRicercaPratica_RowDataBound"
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
                                <asp:BoundField DataField="Cartellina" HeaderText="Cart." HeaderStyle-CssClass="wrap-text" />
                                <asp:BoundField DataField="Quartiere" HeaderText="Quartiere" HeaderStyle-CssClass="wrap-text-40" ItemStyle-CssClass="uppercase-text" />
                                <asp:TemplateField HeaderText="Oggetto" ItemStyle-CssClass="uppercase-text wrap-text">
                                    <HeaderTemplate>
                                        Oggetto
                                    <br />
                                        <asp:TextBox ID="txtFilterOggetto" runat="server" OnTextChanged="txtFilterOggetto_TextChanged" AutoPostBack="True" CssClass="form-control form-control-sm" placeholder="Filtra..."></asp:TextBox>

                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("oggetto1") %>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:BoundField DataField="codice" HeaderText="BU" HtmlEncode="false" HeaderStyle-CssClass="uppercase-text wrap-text-40" />

                                <asp:TemplateField HeaderText="Note" ItemStyle-CssClass="uppercase-text wrap-text">
                                    <HeaderTemplate>
                                        Note
                                     <br />
                                        <asp:TextBox ID="txtFilterNote" runat="server" OnTextChanged="txtFilterNote_TextChanged" AutoPostBack="True" CssClass="form-control form-control-sm" placeholder="Filtra..."></asp:TextBox>

                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("note") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="Cognome" HeaderText="Cognome" HeaderStyle-CssClass="wrap-text-40" />--%>
                                <asp:TemplateField HeaderText="Cognome" ItemStyle-CssClass="uppercase-text wrap-text">
                                    <HeaderTemplate>
                                        Cognome
         <br />
                                        <asp:TextBox ID="txtFilterCognome" runat="server" OnTextChanged="txtFilterCognome_TextChanged" AutoPostBack="True" CssClass="form-control form-control-sm" placeholder="Filtra..."></asp:TextBox>

                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Cognome") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Destinatario" ItemStyle-CssClass="uppercase-text wrap-text">
                                    <HeaderTemplate>
                                        Destinatario
                                         <br />
                                        <asp:TextBox ID="txtFilterDestinatario" runat="server" OnTextChanged="txtFilterDestinatario_TextChanged" AutoPostBack="True" CssClass="form-control form-control-sm" placeholder="Filtra..."></asp:TextBox>

                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("destinatario1") %>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="wrap-text-40">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Text="Sel." CommandName="Select" CommandArgument='<%# Eval("id") + ";" + Eval("Num_Prot")   %>' CssClass="btn btn-success btn-sm" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                        </asp:GridView>

                    </div>
                </div>
                <asp:HiddenField ID="HfFiltroNote" runat="server" />
                 <asp:HiddenField ID="HfId" runat="server" />
                <asp:HiddenField ID="HfFiltroDestinatario" runat="server" />
                <asp:HiddenField ID="HfFiltroOggetto" runat="server" />
                <asp:HiddenField ID="HfFiltroCognome" runat="server" />

                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                    <asp:Button ID="btBack" runat="server" class="btn btn-secondary" Text="Azzera Filtri" OnClick="btBack_Click" ToolTip="Torna alla lista completa" />

                </div>
            </div>
        </div>
    </div>

</asp:Content>
