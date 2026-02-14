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
                                <asp:TextBox ID="txtMese" runat="server" CssClass="form-control" Width="110px" autofocus="" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtAnno">Anno</label>
                                <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control" Width="110px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAnno" ValidationGroup="bt" ErrorMessage="Inserire anno" ForeColor="Red">
                                </asp:RequiredFieldValidator>
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

    <%-- panel dei dettagli --%>
    <div class="panel panel-default">
        <div id="divDettagli" runat="server" visible="false">
            <div class="panel-heading">
                <h3 class="panel-title" style="font-weight: bold;">Dettagli</h3>
            </div>

            <div class="panel-body">
                <div class="container">

                    <div class="tab-content">
                        <p style="font-weight: bold;">Fonte intervento</p>
                        <div class="row custom-border">
                            <div class="col-md-3">
                                <div class="form-group mb-3">
                                    <label for="txtDelegheRicevute">Deleghe Ricevute</label>
                                    <asp:TextBox ID="txtDelegheRicevute" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtEspostiRicevuti">Esposti Ricevuti</label>
                                    <asp:TextBox ID="txtEspostiRicevuti" runat="server" CssClass="form-control larghezzaText" />
                                </div>


                            </div>
                            <div class="col-md-3">
                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtDelegheEsitate">
                                                Deleghe Esitate

                                                <asp:LinkButton ID="LinkButton9" runat="server"
                                                    CommandArgument="DelegheEsitate"
                                                    OnClick="BtnInfo_Click"
                                                    CausesValidation="false"
                                                    CssClass="text-info"
                                                    Style="margin-left: 5px;">
                                             <i class="fa fa-info-circle">ⓘ</i>
                                                </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtDelegheEsitate" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">
                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtEspostiEvasi">
                                                Esposti Evasi
                                
                                <asp:LinkButton ID="BtnInfoEsposti" runat="server"
                                    CommandArgument="EspostiEvasi"
                                    OnClick="BtnInfo_Click"
                                    CausesValidation="false"
                                    CssClass="text-info"
                                    Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtEspostiEvasi" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>


                            </div>
                            <div class="col-md-3">
                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtNotifiche">
                                                Notifiche

                                        <asp:LinkButton ID="LinkButton5" runat="server"
                                            CommandArgument="Notifiche"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtNotifiche" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>

                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtNotificheNoAg">
                                                Notifiche Non AG

                                        <asp:LinkButton ID="LinkButton6" runat="server"
                                            CommandArgument="NotificheNoAG"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtNotificheNoAg" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>
                            </div>
                            <div class="col-md-3">

                                <div class="form-group mb-3">
                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtAccertAltriEnti">
                                                Num Contr. Accert. altri enti

                                       <asp:LinkButton ID="LinkButton23" runat="server"
                                           CommandArgument="AccertAltriEnti"
                                           OnClick="BtnInfo_Click"
                                           CausesValidation="false"
                                           CssClass="text-info"
                                           Style="margin-left: 5px;">
                                   <i class="fa fa-info-circle">ⓘ</i>
                                       </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtAccertAltriEnti" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>


                            </div>
                        </div>


                        <p style="font-weight: bold;">Atti Redatti</p>
                        <div class="row custom-border">
                            <div class="col-md-3 ">
                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtEspostiEvasi">
                                                Relazioni

                                                <asp:LinkButton ID="BtnInfoRelazioni" runat="server"
                                                    CommandArgument="Relazioni"
                                                    OnClick="BtnInfo_Click"
                                                    CausesValidation="false"
                                                    CssClass="text-info"
                                                    Style="margin-left: 5px;">
                                            <i class="fa fa-info-circle">ⓘ</i>
                                                </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtRelazioni" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>

                                </div>
                                <div class="form-group mb-3">
                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtCNR">
                                                CNR

                                    <asp:LinkButton ID="LinkButton24" runat="server"
                                        CommandArgument="CNR"
                                        OnClick="BtnInfo_Click"
                                        CausesValidation="false"
                                        CssClass="text-info"
                                        Style="margin-left: 5px;">
                                <i class="fa fa-info-circle">ⓘ</i>
                                    </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtCNR" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">
                                    <contenttemplate>
                                        <div class="form-group mb-3">
                                            <label for="txtEspostiEvasi">
                                                Controlli Scia

                                        <asp:LinkButton ID="LinkButton3" runat="server"
                                            CommandArgument="SCIA"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtControlliScia" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtDenunceUff">Denunce Uff</label>
                                    <asp:TextBox ID="txtDenunceUff" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                            </div>

                            <div class="col-md-3 ">
                                <div class="form-group mb-3">

                                    <contenttemplate>
                                        <div class="form-group mb-3">
                                            <label for="txtConvalide">
                                                Convalide

                                        <asp:LinkButton ID="LinkButton10" runat="server"
                                            CommandArgument="Convalide"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtConvalide" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtEspostiEvasi">
                                                Annotazioni

                                        <asp:LinkButton ID="LinkButton4" runat="server"
                                            CommandArgument="Annotazioni"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtAnnotazioni" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>

                                <div class="form-group mb-3">
                                    <label for="txtInterrogatori">Interrogatori</label>
                                    <asp:TextBox ID="txtInterrogatori" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtRipristino">Ripr. Tot. Parz.</label>
                                    <asp:TextBox ID="txtRipristino" runat="server" CssClass="form-control larghezzaText" />
                                </div>
                            </div>
                            <div class="col-md-3 ">
                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtEspostiEvasi">
                                                Sequestri

                                    <asp:LinkButton ID="LinkButton7" runat="server"
                                        CommandArgument="Sequestri"
                                        OnClick="BtnInfo_Click"
                                        CausesValidation="false"
                                        CssClass="text-info"
                                        Style="margin-left: 5px;">
                                <i class="fa fa-info-circle">ⓘ</i>
                                    </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtSequestri" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>

                                </div>

                                <div class="form-group mb-3">

                                    <contenttemplate>
                                        <div class="form-group mb-3">
                                            <label for="txtDissequestriTemp">
                                                Dissequestri Temp.

                                            <asp:LinkButton ID="LinkButton12" runat="server"
                                                CommandArgument="DissequestriTemp"
                                                OnClick="BtnInfo_Click"
                                                CausesValidation="false"
                                                CssClass="text-info"
                                                Style="margin-left: 5px;">
                                        <i class="fa fa-info-circle">ⓘ</i>
                                            </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtDissequestriTemp" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtRiapposizioneSigilli">
                                                Riapposizione Sigilli

                                    <asp:LinkButton ID="LinkButton8" runat="server"
                                        CommandArgument="RiappSigilli"
                                        OnClick="BtnInfo_Click"
                                        CausesValidation="false"
                                        CssClass="text-info"
                                        Style="margin-left: 5px;">
                                <i class="fa fa-info-circle">ⓘ</i>
                                    </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtRiapposizioneSigilli" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>



                                </div>
                            </div>
                            <div class="col-md-3 ">
                                <div class="form-group mb-3">

                                    <contenttemplate>
                                        <div class="form-group mb-3">
                                            <label for="txtViolazioneSigilli">
                                                Violazione Sigilli

                                        <asp:LinkButton ID="LinkButton11" runat="server"
                                            CommandArgument="ViolazioneSigilli"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtViolazioneSigilli" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">
                                    <label for="txtRimozioneSigilli"></label>
                                    <contenttemplate>
                                        <div class="form-group mb-3">
                                            <label for="txtRimozioneSigilli">
                                                Rimozione Sigilli

                                            <asp:LinkButton ID="LinkButton14" runat="server"
                                                CommandArgument="RimozioneSigilli"
                                                OnClick="BtnInfo_Click"
                                                CausesValidation="false"
                                                CssClass="text-info"
                                                Style="margin-left: 5px;">
                                        <i class="fa fa-info-circle">ⓘ</i>
                                            </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtRimozioneSigilli" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>

                                <div class="form-group mb-3">

                                    <contenttemplate>
                                        <div class="form-group mb-3">
                                            <label for="txtDissequestri">
                                                Dissequestri

                                        <asp:LinkButton ID="LinkButton13" runat="server"
                                            CommandArgument="Dissequestri"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtDissequestri" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>

                            </div>
                        </div>



                        <p style="font-weight: bold;">Controlli e Ordinanze</p>
                        <div class="row custom-border">
                            <div class="col-md-3 ">

                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtEspostiEvasi">
                                                Ponteggi

                                        <asp:LinkButton ID="LinkButton1" runat="server"
                                            CommandArgument="Ponteggi"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="TxtPonteggi" runat="server" CssClass="form-control larghezzaText" />
                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">
                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtEspostiEvasi">
                                                DPI

                                        <asp:LinkButton ID="LinkButton2" runat="server"
                                            CommandArgument="DPI"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtDPI" runat="server" CssClass="form-control larghezzaText" />

                                        </div>
                                    </contenttemplate>
                                </div>

                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtViol_amm_reg_com">
                                                Viol. Amm. Reg. Comunali

                                    <asp:LinkButton ID="LinkButton17" runat="server"
                                        CommandArgument="ViolAmmRegCom"
                                        OnClick="BtnInfo_Click"
                                        CausesValidation="false"
                                        CssClass="text-info"
                                        Style="margin-left: 5px;">
                                <i class="fa fa-info-circle">ⓘ</i>
                                    </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtViol_amm_reg_com" runat="server" CssClass="form-control larghezzaText" />

                                        </div>
                                    </contenttemplate>
                                </div>
                            </div>
                            <div class="col-md-3 ">


                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtOccupAbusivaAbit">
                                                Occup. abusiva abitativa

                                        <asp:LinkButton ID="LinkButton19" runat="server"
                                            CommandArgument="OccupAbusivaAbit"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtOccupAbusivaAbit" runat="server" CssClass="form-control larghezzaText" />

                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtOccupAbusivaNoAbit">
                                                Occup. abusiva non abitativa

                                            <asp:LinkButton ID="LinkButton20" runat="server"
                                                CommandArgument="OccupAbusivaNoAbit"
                                                OnClick="BtnInfo_Click"
                                                CausesValidation="false"
                                                CssClass="text-info"
                                                Style="margin-left: 5px;">
                                        <i class="fa fa-info-circle">ⓘ</i>
                                            </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtOccupAbusivaNoAbit" runat="server" CssClass="form-control larghezzaText" />

                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">
                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtCensimentoAllPubb">
                                                Cens. nuclei fam. alloggi pubb.

                                        <asp:LinkButton ID="LinkButton18" runat="server"
                                            CommandArgument="CensimentoAllPubb"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtCensimentoAllPubb" runat="server" CssClass="form-control larghezzaText" />

                                        </div>
                                    </contenttemplate>
                                </div>

                            </div>


                            <div class="col-md-3">


                                <div class="form-group mb-3">

                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtControlliCant">
                                                Controlli C/Ri Giornalieri

                                            <asp:LinkButton ID="LinkButton16" runat="server"
                                                CommandArgument="ControlliCant"
                                                OnClick="BtnInfo_Click"
                                                CausesValidation="false"
                                                CssClass="text-info"
                                                Style="margin-left: 5px;">
                                        <i class="fa fa-info-circle">ⓘ</i>
                                            </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtControlliCant" runat="server" CssClass="form-control larghezzaText" />

                                        </div>
                                    </contenttemplate>
                                </div>


                                <div class="form-group mb-3">
                                    <label for="txtDemolizioni">Demolizioni</label>
                                    <asp:TextBox ID="txtDemolizioni" runat="server" CssClass="form-control larghezzaText" />
                                </div>

                                <div class="form-group mb-3">
                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtControlliDLGS">
                                                Controlli D.LGS 42/04

                                        <asp:LinkButton ID="LinkButton15" runat="server"
                                            CommandArgument="ControlliDLGS"
                                            OnClick="BtnInfo_Click"
                                            CausesValidation="false"
                                            CssClass="text-info"
                                            Style="margin-left: 5px;">
                                    <i class="fa fa-info-circle">ⓘ</i>
                                        </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtControlliDLGS" runat="server" CssClass="form-control larghezzaText  " />

                                        </div>
                                    </contenttemplate>
                                </div>

                            </div>
                            <div class="col-md-3">
                                <div class="form-group mb-3">
                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtSgomberiAbus">
                                                Sgomberi occup. abusiva

                                       <asp:LinkButton ID="LinkButton21" runat="server"
                                           CommandArgument="SgomberiAbus"
                                           OnClick="BtnInfo_Click"
                                           CausesValidation="false"
                                           CssClass="text-info"
                                           Style="margin-left: 5px;">
                                   <i class="fa fa-info-circle">ⓘ</i>
                                       </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtSgomberiAbus" runat="server" CssClass="form-control larghezzaText  " />

                                        </div>
                                    </contenttemplate>
                                </div>
                                <div class="form-group mb-3">
                                    <contenttemplate>

                                        <div class="form-group mb-3">
                                            <label for="txtSgomberiAbus">
                                                Sgomberi occup. immobili/aree

                                       <asp:LinkButton ID="LinkButton22" runat="server"
                                           CommandArgument="SgomberiImmobili"
                                           OnClick="BtnInfo_Click"
                                           CausesValidation="false"
                                           CssClass="text-info"
                                           Style="margin-left: 5px;">
                                   <i class="fa fa-info-circle">ⓘ</i>
                                       </asp:LinkButton>
                                            </label>
                                            <asp:TextBox ID="txtSgomberiImmobili" runat="server" CssClass="form-control larghezzaText  " />

                                        </div>
                                    </contenttemplate>
                                </div>
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
    <%-- Modale ricerca scheda --%>
    <div class="modal fade" id="ModalRicerca" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel2">Schede contenenti il dato richiesto</h5>

                </div>
                <div class="modal-body">

                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="GVRicercaScheda" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="GVRicercaScheda_RowDataBound" OnRowCommand="GVRicercaScheda_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="GVRicercaScheda_PageIndexChanging" RowStyle-CssClass="GridViewRow"
                            AlternatingRowStyle-CssClass="GridViewAlternatingRow">
                            <Columns>
                                <asp:BoundField DataField="id_rapp_scheda" HeaderText="ID" />
                                <asp:BoundField DataField="rapp_numero_pratica" HeaderText="Numero Pratica" />
                                <asp:BoundField DataField="rapp_nominativo" HeaderText="Nominativo" />
                                <asp:BoundField DataField="rapp_pattuglia" HeaderText="Pattuglia" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%--<asp:Button ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%#  Eval("id_rapp_scheda")   %>' CssClass="btn btn-success btn-sm" />--%>

                                        <asp:HyperLink ID="lnkScheda" runat="server"
                                            NavigateUrl='<%# Eval("id_rapp_scheda", "~/View/RicercaScheda.aspx?idscheda={0}") %>'
                                            Target="_blank">
                                            <asp:Image ID="imgApri" runat="server"
                                                ImageUrl="~/FileComuni/lente.png"
                                                Width="20px" Height="20px"
                                                AlternateText="Apri" />

                                        </asp:HyperLink>





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
                <asp:HiddenField ID="HfIdScheda" runat="server" />
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                </div>
            </div>
        </div>
    </div>



</asp:Content>
