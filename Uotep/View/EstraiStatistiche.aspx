<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EstraiStatistiche.aspx.cs" Inherits="Uotep.EstraiStatistiche" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }

        // Mostra il popup ricerca
        function showModal() {
            $('#ModalRicerca').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalRicerca').modal('hide');
        }
        // Funzione per aggiungere testo a un TextBox
        function appendToTextBox(TextPattugliaCompleta, DdlPattuglia) {
            // Ottieni il TextBox tramite il suo ID
            const textBox = document.getElementById(TextPattugliaCompleta);
            const dropDown = document.getElementById(DdlPattuglia);
            // Aggiungi il valore al contenuto corrente
            if (textBox && dropDown) {
                // Ottieni il valore selezionato nella DropDownList
                const selectedValue = dropDown.value;

                // Aggiungi il valore selezionato al contenuto del TextBox
                textBox.value += selectedValue;
            }
        }


    </script>

    <div class="panel panel-default">
        <div class="form-group mb-3"></div>

        <div class="panel-body" id="divTesta" runat="server">
            <div class="jumbotron">
                <div style="margin-top: -90px!important">
                    <%--<asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>--%>
                    <p class="text-center lead">ESTRAZIONE STATISTICHE</p>
                    <p class="text-center lead">Inserendo solo l'anno di riferimento si estraggono gli obiettivi raggiunti</p>
                </div>

                <div class="container">
                    <div class="row align-items-end">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtMese">Mese</label>
                                <asp:TextBox ID="txtMese" runat="server" CssClass="form-control" Width="110px" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtAnno">Anno</label>
                                <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control" Width="110px" />
                            </div>
                        </div>


                    </div>
                </div>

                <!-- Bottoni -->
                <div class="col-md-4 ">
                    <div class="form-group">

                        <asp:Button ID="btEsegui" runat="server" ValidationGroup="bt" Text="Esegui" CssClass="btn btn-primary" OnClick="btEsegui_Click" />
                    </div>
                </div>
            </div>
        </div>

    </div>



    <%-- il seguente style serve per i bordi azzurri --%>
    <style>
        

       

      
        .table-layout-fixed {
            table-layout: fixed;
            width: 100%; /* Assicura che la tabella occupi il 100% del suo contenitore */
        }

            /* Opzionale: assicura che il testo troppo lungo non rompa il layout */
            .table-layout-fixed td,
            .table-layout-fixed th {
                overflow: hidden;
                text-overflow: ellipsis;
                white-space: nowrap;
            }
    </style>
    <%-- panel dei dettagli --%>
    <div class="panel panel-default">
        <div id="divDettagli" runat="server" visible="false">
            <div class="panel-heading">
                <h3 class="panel-title" style="font-weight: bold;">Dettagli</h3>
            </div>

            <div class="panel-body">
                <div class="container">

                    <div class="tab-content">

                        <div class="row custom-border">
                            <div class="col-md-6 ">
                                <div class="form-group mb-3">
                                    <label for="txtRelazioni">Relazioni</label>
                                    <asp:TextBox ID="txtRelazioni" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="TxtPonteggi">Ponteggi</label>
                                    <asp:TextBox ID="TxtPonteggi" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtDPI">DPI</label>
                                    <asp:TextBox ID="txtDPI" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtEspostiRicevuti">Esposti Ricevuti</label>
                                    <asp:TextBox ID="txtEspostiRicevuti" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtCNR">CNR</label>
                                    <asp:TextBox ID="txtCNR" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtAnnotazioni">Annotazioni</label>
                                    <asp:TextBox ID="txtAnnotazioni" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtDelegheEsitate">Deleghe Esitate</label>
                                    <asp:TextBox ID="txtDelegheEsitate" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtViol_amm_reg_com">Viol. Amm. Reg. Comunali</label>
                                    <asp:TextBox ID="txtViol_amm_reg_com" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtInterrogatori">Interrogatori</label>
                                    <asp:TextBox ID="txtInterrogatori" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtConvalide">Convalide</label>
                                    <asp:TextBox ID="txtConvalide" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtViolazioneSigilli">Violazione Sigilli</label>
                                    <asp:TextBox ID="txtViolazioneSigilli" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtDissequestriTemp">Dissequestri Temp.</label>
                                    <asp:TextBox ID="txtDissequestriTemp" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtRimozioneSigilli">Rimozione Sigilli</label>
                                    <asp:TextBox ID="txtRimozioneSigilli" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtOccupAbusivaAbit">Occupazione abusiva abitativa</label>
                                    <asp:TextBox ID="txtOccupAbusivaAbit" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtOccupAbusivaNoAbit">Occupazione abusiva non abitativa</label>
                                    <asp:TextBox ID="txtOccupAbusivaNoAbit" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtCensimentoAllPubb">Censimento nuclei familiari alloggi pubb.</label>
                                    <asp:TextBox ID="txtCensimentoAllPubb" runat="server" CssClass="form-control larghezzaText" />
                                </div>

                            </div>


                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtEspostiEvasi">Esposti Evasi</label>
                                    <asp:TextBox ID="txtEspostiEvasi" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtRipristino">Ripr. Tot. Parz.</label>
                                    <asp:TextBox ID="txtRipristino" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtControlliScia">Controlli Scia</label>
                                    <asp:TextBox ID="txtControlliScia" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtControlliCant">Controlli C/Ri Giornalieri</label>
                                    <asp:TextBox ID="txtControlliCant" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtNotifiche">Notifiche</label>
                                    <asp:TextBox ID="txtNotifiche" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtNotificheNoAg">Notifiche Non AG</label>
                                    <asp:TextBox ID="txtNotificheNoAg" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtSequestri">Sequestri</label>
                                    <asp:TextBox ID="txtSequestri" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtRiapposizioneSigilli">Riapposizione Sigilli</label>
                                    <asp:TextBox ID="txtRiapposizioneSigilli" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtDelegheRicevute">Deleghe Ricevute</label>
                                    <asp:TextBox ID="txtDelegheRicevute" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtDenunceUff">Denunce Uff</label>
                                    <asp:TextBox ID="txtDenunceUff" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtDemolizioni">Demolizioni</label>
                                    <asp:TextBox ID="txtDemolizioni" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtDissequestri">Dissequestri</label>
                                    <asp:TextBox ID="txtDissequestri" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtControlliDLGS">Controlli D.LGS 42/04</label>
                                    <asp:TextBox ID="txtControlliDLGS" runat="server" CssClass="form-control larghezzaText  " />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtSgomberiAbus">Sgomberi occup. abusiva</label>
                                    <asp:TextBox ID="txtSgomberiAbus" runat="server" CssClass="form-control larghezzaText  " />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtSgomberiImmobili">Sgomberi occup. immobili/aree</label>
                                    <asp:TextBox ID="txtSgomberiImmobili" runat="server" CssClass="form-control larghezzaText  " />
                                </div>

                            </div>
                        </div>


                    </div>
                </div>
            </div>

        </div>
    <%-- Griglia statistiche annuali--%>
    <%--<div id="DivAnnuale" runat="server" class="form-group">
            <asp:GridView ID="GvStatAnnuale" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                OnRowDataBound="GvStatAnnuale_RowDataBound" OnRowCommand="GvStatAnnuale_RowCommand" RowStyle-CssClass="GridViewRow" 
                AlternatingRowStyle-CssClass="GridViewAlternatingRow" ShowFooter="true">
                 <Columns>
     <asp:BoundField DataField="mese" HeaderText="mese"  HeaderStyle-Width="50px"/>
     <asp:BoundField DataField="rapp_contr_cantiere_suolo_pubb" HeaderText="Impalcature" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
     <asp:BoundField DataField="rapp_contr_lavori_edili" HeaderText="DPI"  HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
     <asp:BoundField DataField="rapp_contr_cantieri_seq" HeaderText="Cantieri Seq."  HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
     <asp:BoundField DataField="rapp_numEsposti" HeaderText="Esposti Evasi" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
     <asp:BoundField DataField="rapp_censimento_all_pubb" HeaderText="Cens. All. Pubb."  HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
     <asp:BoundField DataField="rapp_contr_occ_abitativo" HeaderText="Occ. Uso Abit"  HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
     <asp:BoundField DataField="rapp_contr_occ_no_abitativo" HeaderText="Occ. Uso Non Abit" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
     

 </Columns>
            </asp:GridView>

        </div>--%>
    <div id="DivAnnuale" runat="server" class="form-group">
        <asp:GridView ID="GvStatAnnuale" runat="server" AutoGenerateColumns="false"
            CssClass="table table-bordered table-layout-fixed"
            OnRowDataBound="GvStatAnnuale_RowDataBound" OnRowCommand="GvStatAnnuale_RowCommand"
            RowStyle-CssClass="GridViewRow"
            AlternatingRowStyle-CssClass="GridViewAlternatingRow" ShowFooter="true">
            <Columns>
                <asp:BoundField DataField="Mese" HeaderText="Mese" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Justify" />
                <%-- Mese a sinistra --%>
                <asp:BoundField DataField="rapp_contr_cantiere_suolo_pubb" HeaderText="Impalcature" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Justify" />
                <asp:BoundField DataField="rapp_contr_lavori_edili" HeaderText="DPI" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Justify" />
                <asp:BoundField DataField="rapp_contr_cantieri_seq" HeaderText="Cantieri Seq." ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Justify" />
                <asp:BoundField DataField="rapp_numEsposti" HeaderText="Esposti Evasi" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Justify" />
                <asp:BoundField DataField="rapp_censimento_all_pubb" HeaderText="Cens. All. Pubb." ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Justify" />
                <asp:BoundField DataField="rapp_contr_occ_abitativo" HeaderText="Occ. Uso Abit" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Justify" />
                <asp:BoundField DataField="rapp_contr_occ_no_abitativo" HeaderText="Occ. Uso Non Abit" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Justify" />
                <asp:BoundField DataField="rapp_NumcontrNatoDaAcc" HeaderText="Acc. Rich. Da Altri" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Justify" />
            </Columns>
        </asp:GridView>

    </div>
    <%-- Griglia obiettivi--%>
    <div id="DivObiettivi" runat="server" class="form-group" visible="false">
        <asp:GridView ID="GvObiettivi" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-layout-fixed" RowStyle-CssClass="GridViewRow"
            AlternatingRowStyle-CssClass="GridViewAlternatingRow">
            <Columns>
                <asp:BoundField DataField="ID_obiettivi" HeaderText="ID" Visible="false" />
                <%-- <asp:BoundField DataField="" HeaderText="" ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Right"/>--%>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="OBIETTIVI"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="impalcature" HeaderText="Impalcature" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="dpi" HeaderText="DPI" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="contcantseq" HeaderText="Cantieri Seq." ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="contr_esposti" HeaderText="Esposti Evasi" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="cens_allogg_pubb" HeaderText="Cens.m All. Pubb." ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="occ_prop_com_abit" HeaderText="Occ. Uso Abit" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="occ_prop_com_no_abit" HeaderText="Occ. Uso Non Abit" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="contr_nati_da_accer_richiesti" HeaderText="Acc. Rich. Da Altri" ItemStyle-HorizontalAlign="Right" />

            </Columns>
        </asp:GridView>

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

</asp:Content>
