<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RicercaArchivioUotp.aspx.cs" Inherits="Uotep.RicercaArchivioUotp" %>


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
    <style>
        .uppercase-text {
            text-transform: uppercase;
        }

        .custom-border {
            border: 2px solid #007bff; /* Cornice blu */
            border-radius: 8px; /* Angoli arrotondati */
            padding: 15px; /* Spazio interno */
            margin: 5px 0; /* Spazio esterno */
            margin-left: -30px;
        }

        .wrap-text {
            max-width: 10px;
            /* Proprietà standard per andare a capo */
            white-space: normal !important;
            /* Proprietà essenziale per spezzare parole lunghe e senza spazi */
            word-wrap: break-word !important;
            /* o in alternativa: word-break: break-all !important; */
        }

        .wrap-text-40 {
            max-width: 40px;
            /* Proprietà standard per andare a capo */
            white-space: normal !important;
            /* Proprietà essenziale per spezzare parole lunghe e senza spazi */
        }
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

        /* Stile per rendere i bottoni stessa altezza e larghezza */
        .uniform-button {
            width: 180px !important; /* Esempio: Larghezza fissa per i bottoni */
            height: 45px !important; /* Esempio: Altezza fissa per i bottoni */
            display: flex !important;
            justify-content: center !important;
            align-items: center !important;
            text-align: center !important;
            white-space: normal !important;
            word-break: break-word !important;
            margin-left: 10px !important;
        }
    </style>
    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">RICERCA UNA PRATICA</p>
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
                        <%-- <--- AGGIUNGI class="center-table" QUI --%>
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
                                <asp:Button ID="btNpratica" runat="server" OnClick="btNpratica_Click" Text="Prot. Nr" ToolTip="Ricerca Per Pratica" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                        </tr>
                        <tr>
                            <%-- Seconda riga: altri 4 pulsanti --%>
                            <td>
                                <asp:Button ID="btNota" runat="server" OnClick="btNota_Click" Text="Nota" ToolTip="Ricerca Per Nota" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btBU" runat="server" OnClick="btBU_Click" Text="BU" ToolTip="Ricerca Per BU" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <%-- <td>
                                <asp:Button ID="btEstraiParziale" runat="server" OnClick="btEstraiParziale_Click" Text="Estrazione Parziale" ToolTip="Estrazione Parziale" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                            <td>
                                <asp:Button ID="btEstraiTotale" runat="server" OnClick="btEstraiTotale_Click" Text="Estrai DB" ToolTip="Estrazione Totale" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>--%>
                        </tr>
                        <%--                        <tr>
                            <td>
                                <asp:Button ID="btNpratica" runat="server" OnClick="btNpratica_Click" CommandArgument="Pratica" Text="Nr. Pratica" ToolTip="Ricerca Pratica" CssClass="btn btn-primary mx-2 uniform-button" />
                            </td>
                        </tr>--%>
                    </table>
                    <%-- Fine Tabella Pulsanti --%>
                </div>
            </asp:Panel>
        </div>

        <%--Sezione di ricerca  --%>
        <div id="DivRicerca" runat="server" class="row d-flex justify-content-center align-items-center vh-100" style="height: 300px; margin-left: 400px!important">
            <!-- Righe di input  -->
            <div class="col-md-4 ">

                <%-- DIV RICERCA pratica --%>
                <div id="DivPratica" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label6" runat="server" Text="Prot.Nr." CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtPratN" runat="server" CssClass="form-control" placeholder="Prot.Nr." />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Oggetto --%>
                <div id="DivOggetto" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label11" runat="server" Text="Oggetto" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtOggetto" runat="server" CssClass="form-control" placeholder="Oggetto" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>

                <%-- DIV RICERCA Nota --%>
                <div id="DivNota" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label4" runat="server" Text="Nota" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtNota" runat="server" CssClass="form-control" placeholder="Nota" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA BU --%>
                <div id="DivBU" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label5" runat="server" Text="BU" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtBU" runat="server" CssClass="form-control" placeholder="codice BU" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Destinatario --%>
                <div id="DivDestinatario" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label1" runat="server" Text="BU" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDestinatario" runat="server" CssClass="form-control" placeholder="Destinatario" />
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
                                    <label for="txtNumProTp">Prot.Nr.</label>
                                    <asp:TextBox ID="txtNumProTp" runat="server" CssClass="form-control" ForeColor="Red" Font-Bold="true"  Enabled="false"/>

                                    <label for="txtProGenTp">Prot. Gen.</label>
                                    <asp:TextBox ID="txtProGenTp" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>


                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtDataProtGen">Data Prot. Gen.</label>
                                    <asp:TextBox ID="txtDataProtGen" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"/>

                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-check mb-2">

                                    <div class="form-check mb-2">
                                        <label for="txtDataInserimentoTp">Data Inserimento</label>
                                        <asp:TextBox ID="txtDataInserimentoTp" runat="server" CssClass="form-control" Enabled="false" Font-Bold="true" />


                                    </div>

                                </div>

                                <div class="form-check mb-2">
                                    <label for="txtProProcTp">Prot. Proc.</label>
                                    <asp:TextBox ID="txtProProcTp" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtDataProtProc">Data Prot. Proc.</label>
                                    <asp:TextBox ID="txtDataProtProc" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"/>

                                </div>
                            </div>


                        </div>
                    </div>
                    <div class="tab-content">
                        <p style="font-weight: bold;">Dettagli</p>
                        <div class="row custom-border">

                            <div class="col-md-6">
                                <div class="form-check mb-2">
                                    <label for="txtDestinatarioTp">Destinatario</label>
                                    <asp:TextBox ID="txtDestinatarioTp" runat="server" CssClass="form-control" Enabled="false"/>
                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtQuartiereTp">Quartiere</label>
                                    <asp:TextBox ID="txtQuartiereTp" runat="server" CssClass="form-control" Enabled="false"/>

                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtBUTp">BU</label>
                                    <asp:TextBox ID="txtBUTp" runat="server" CssClass="form-control" Enabled="false"/>
                                </div>

                            </div>
                            <div class="col-md-6">
                                <div class="form-check mb-2">
                                    <label for="txtNotaTp">Nota</label>
                                    <asp:TextBox ID="txtNotaTp" runat="server" CssClass="form-control" Enabled="false"/>
                                </div>
                                <div class="form-check mb-2">
                                    <label for="txtCartellinaTp">Cartellina</label>
                                    <asp:TextBox ID="txtCartellinaTp" runat="server" CssClass="form-control" Enabled="false"/>
                                </div>
                                <div class="form-check mb-2">
                                    <label for="TxtIndirizzoTp">Indirizzo</label>
                                    <asp:TextBox ID="TxtIndirizzoTp" runat="server" CssClass="form-control" Enabled="false"/>
                                </div>

                            </div>
                            <div class="col-md-12">
                                <div class="form-check mb-2">
                                    <label for="txtOggettoTp">Oggetto</label>
                                    <asp:TextBox ID="txtOggettoTp" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" Style="margin-left: -10px; width: 100%; max-width: 800px;" Enabled="false"/>

                                </div>
                            </div>
                        </div>

                    </div>
                    <asp:Button ID="BtNewRicerca" Text="Nuova Ricerca" runat="server" OnClick="BtNewRicerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />

                </div>
            </div>
        </div>
    </div>

    <%-- Modale ricerca pratica --%>
    <div class="modal fade" id="ModalPratica" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 100%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel10">Ricerca Pratica</h5>

                </div>
                <div class="modal-body">

                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="GVRicercaPratica" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopup_RowDataBoundP" OnRowCommand="gvPopup_RowCommandP" AllowPaging="true" PageSize="10" OnPageIndexChanging="GVRicercaPratica_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="Num_Prot" HeaderText="Prot. Nr." HeaderStyle-CssClass="wrap-text-40" />

                                <asp:TemplateField HeaderText="Oggetto" ItemStyle-CssClass="uppercase-text wrap-text">
                                    <HeaderTemplate>
                                        Oggetto
                                    <br />
                                        <asp:TextBox ID="txtFilterOggetto" runat="server" OnTextChanged="txtFilterOggetto_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        Filtro
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
                                        <asp:TextBox ID="txtFilterNote" runat="server" OnTextChanged="txtFilterNote_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        Filtro
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("note") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Destinatario" ItemStyle-CssClass="uppercase-text wrap-text">
                                    <HeaderTemplate>
                                        Destinatario
                                         <br />
                                        <asp:TextBox ID="txtFilterDestinatario" runat="server" OnTextChanged="txtFilterDestinatario_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        Filtro
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("destinatario1") %>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="wrap-text-40">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("id") + ";" + Eval("Num_Prot")   %>' CssClass="btn btn-success btn-sm" />
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
                <asp:HiddenField ID="HfFiltroNote" runat="server" />
                <asp:HiddenField ID="HfFiltroDestinatario" runat="server" />
                <asp:HiddenField ID="HfFiltroOggetto" runat="server" />
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
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
