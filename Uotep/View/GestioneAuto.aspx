<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestioneAuto.aspx.cs" Inherits="Uotep.GestioneAuto" %>


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

        // Mostra il popup 
        function ShowErrorMessage(message) {
            $('#ModalAvvertenze').modal('show');
        }
        // Nasconde il popup
        function HideErrorMessage() {
            $('#ModalAvvertenze').modal('hide');
        }
    </script>
    <%-- il seguente style serve per i bordi azzurri --%>
    <style>
        .GridViewRow {
            background-color: white;
        }

        /* Stile per la riga alternata (azzurro chiaro) */
        .GridViewAlternatingRow {
            background-color: #E6F3FF; /* Un azzurro molto chiaro */
            /* background-color: #F0F8FF;  Un altro azzurro molto chiaro (AliceBlue) */
        }

        .custom-border {
            border: 2px solid #007bff; /* Cornice blu */
            border-radius: 8px; /* Angoli arrotondati */
            padding: 15px; /* Spazio interno */
            margin: 5px 0; /* Spazio esterno */
            /*margin-left: -30px;*/
        }

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

    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">GESTIONE CARTE CARBURANTE</p>

            <div class="col-md-4 " style="margin-bottom: 10px; margin-top: 20px; padding-left: 2em">
                <asp:TextBox ID="txtMese" runat="server" CssClass="form-control" Enabled="false" />
            </div>
            <div class="col-md-4 " style="margin-bottom: 10px; margin-top: 20px; padding-left: 2em">
                <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control" Enabled="false" />
            </div>
             <div class="col-md-4 " style="margin-bottom: 10px; margin-top: 20px; padding-left: 2em">
                    <asp:Button ID="btSalva" Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    <asp:Button ID="btCerca" Text="Cerca" runat="server" OnClick="btCerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" />
                    <asp:Button ID="btStampa" Text="Stampa" runat="server" OnClick="btStampa_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" />



            </div>
        </div>



        <div class="container">

            <div class="tab-content">

                <div class="row custom-border" id="divInserimento" runat="server">
                    <div class="col-md-4 ">
                        <div class="form-group mb-3" style="margin-left: -25px;">
                            <label for="DdlSigla">Sigla</label>
                            <asp:DropDownList ID="DdlSigla" runat="server" CssClass="form-control" OnSelectedIndexChanged="DdlSigla_SelectedIndexChanged1" AutoPostBack="true">
                            </asp:DropDownList>

                        </div>
                        <div class="form-group mb-3" style="margin-left: -25px;">
                            <label for="txtAutista">Autista</label>
                            <asp:TextBox ID="txtAutista" runat="server" CssClass="form-control" />

                        </div>
                    </div>
                    <div class="col-md-4">

                        <div class="form-group mb-3">
                            <label for="txtTarga">Targa</label>
                            <asp:TextBox ID="txtTarga" runat="server" CssClass="form-control" Font-Bold="true" />
                        </div>
                        <div class="form-group mb-3">
                            <label for="txtStan">Data</label>
                            <asp:TextBox ID="TxtData" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtData" ValidationGroup="bt" ErrorMessage="Inserire data" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator
                                ID="RegularExpressionValidator1"
                                runat="server"
                                ControlToValidate="TxtData"
                                ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$"
                                ErrorMessage="la data deve essere dd/mm/aaaa"
                                ForeColor="Red"
                                ValidationGroup="bt"
                                Display="Static">
                            </asp:RegularExpressionValidator>
                        </div>


                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="DdlCarburante">Tipo Carburante</label>

                            <asp:DropDownList ID="DdlCarburante" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Benzina"> </asp:ListItem>
                                <asp:ListItem Text="Diesel"> </asp:ListItem>
                            </asp:DropDownList>

                        </div>

                    </div>

                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtStan">Ora</label>
                            <asp:TextBox ID="txtOra" runat="server" CssClass="form-control" />
                            <asp:RegularExpressionValidator
                                ID="RegexOra"
                                runat="server"
                                ControlToValidate="txtOra"
                                ValidationExpression="^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$"
                                ErrorMessage="Formato ora non valido (usa HH:MM)."
                                ForeColor="Red"
                                ValidationGroup="bt" />
                        </div>




                    </div>
                    <div class="col-md-4">

                        <div class="form-group mb-3" style="margin-left: -25px;">
                            <label for="txtStan">Stan</label>
                            <asp:TextBox ID="txtStan" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStan" ValidationGroup="bt" ErrorMessage="Inserire Stan" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </div>

                    </div>
                    <div class="col-md-4">

                        <div class="form-group mb-3">
                            <label for="txtLitri">Litri</label>
                            <asp:TextBox ID="txtLitri" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLitri" ValidationGroup="bt" ErrorMessage="Inserire Litri" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator
                                ID="RegularExpressionValidator2"
                                runat="server"
                                ControlToValidate="txtLitri"
                                ValidationExpression="^\s*([0-9]{1,3}(\.([0-9]{3}))*|([0-9]+))(\,[0-9]{1,2})?\s*$"
                                ErrorMessage="Formato Litri. Usa la virgola per i decimali (es: 10,01)."
                                ForeColor="Red"
                                ValidationGroup="bt" />
                        </div>

                    </div>
                    <div class="col-md-4">

                        <div class="form-group mb-3">
                            <label for="txtEuro">Euro</label>
                            <asp:TextBox ID="txtEuro" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEuro" ValidationGroup="bt" ErrorMessage="Inserire Euro" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator
                                ID="RegexEuro"
                                runat="server"
                                ControlToValidate="txtEuro"
                                ValidationExpression="^\s*([0-9]{1,3}(\.([0-9]{3}))*|([0-9]+))(\,[0-9]{1,2})?\s*$"
                                ErrorMessage="Formato Euro non valido. Usa la virgola per i decimali (es: 1.234,56)."
                                ForeColor="Red"
                                ValidationGroup="bt" />
                        </div>

                    </div>
                    <div class="col-md-4">

                        <div class="form-group mb-3" style="margin-left: -25px;">
                            <label for="txtStan">Indirizzo Distributore</label>
                            <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" />

                        </div>

                    </div>

                </div>





            </div>

            <asp:HiddenField ID="Hfuser" runat="server" />
            <asp:HiddenField ID="HfFiltroData" runat="server" />


        </div>

    </div>
    <!-- GridView  -->
    <div id="DivGrid" runat="server" class="form-group" style="padding-left: -50px">

        <%--CssClass="table table-bordered table-layout-fixed" colonne larghezza fisse--%>
        <asp:GridView ID="gvDett" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
            OnRowDataBound="gvDett_RowDataBound" OnRowCommand="gvDett_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvDett_PageIndexChanging" RowStyle-CssClass="GridViewRow"
            AlternatingRowStyle-CssClass="GridViewAlternatingRow">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                <asp:BoundField DataField="sigla" HeaderText="Sigla" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                <asp:BoundField DataField="targa" HeaderText="Targa" ItemStyle-Width="40px" />
                <asp:BoundField DataField="stan" HeaderText="STAN" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />

                <asp:TemplateField HeaderText="Data" ItemStyle-Width="50px">
                    <HeaderTemplate>
                        data
                          <br />
                        <asp:TextBox ID="txtFilterData" runat="server" OnTextChanged="txtFilterData_TextChanged" AutoPostBack="True"></asp:TextBox>
                        Filtro
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("data", "{0:dd/MM/yyyy}") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Ora" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblOra" runat="server"
                            Text='<%# Eval("ora") != null ? Eval("ora").ToString().Substring(0, 5) : "" %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:BoundField DataField="litri" HeaderText="Litri" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="tipocarburante" HeaderText="Carburante" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="euro" HeaderText="Euro" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="indirizzo" HeaderText="Indirizzo" ItemStyle-Width="100%" />
                <asp:BoundField DataField="autista" HeaderText="Autista" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="matricola" HeaderText="Resp." ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Verificato" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("verificato").ToString() == "True" ? "Si" : "No" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Button ID="btnSelect" runat="server" Text="OK"
                            CommandName="Select"
                            CommandArgument='<%# Eval("id")  + "|" + Eval("targa") + "|" + Eval("data")+ "|" + Eval("autista")%>'
                            CssClass="btn btn-success btn-sm" />
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

                    <asp:Button ID="btClose" runat="server" class="btn btn-secondary" Text="Chiudi" OnClientClick="HideErrorMessage()" />
                </div>
            </div>
        </div>
    </div>
    <%-- popup avvertenze --%>
    <div class="modal fade" id="ModalAvvertenze" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-hidden="true">
        <div class="modal-dialog"
            role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel6">ATTENZIONE</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">

                        <p id="txtAvvertenze" style="color: red"></p>

                    </div>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btChiudiAvvertenze" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="btChiudiAvvertenze_Click" />
                </div>
            </div>
        </div>
    </div>


</asp:Content>
