<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Segreteria.aspx.cs" Inherits="Uotep.Segreteria" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        function CloseErrorMessage(message) {
            $('#errorModal').modal('hide');
        }
        // Nasconde il popup
        function HideErrorMessage() {
            $('#errorModal').modal('hide');
        }
        function showModal() {
            $('#ModalRicercaFile').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalRicercaFile').modal('hide');
        }
    </script>
    <div class="jumbotron">
        <h1>Caricamento file</h1>
        <p class="lead"></p>
    </div>
    <%-- LOGIN --%>
    <asp:Panel ID="pnlGestFile" runat="server" CssClass="text-center">
        <div class="row d-flex justify-content-center align-items-center vh-100">
            <div class="container">
                <div class="row">
                    <div id="divCaricaFile" runat="server" class="col-md-6" visible="false">


                        <!-- Etichetta upload-->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label2" runat="server" Text="File" CssClass="form-label d-block mb-2"></asp:Label>
                                <%--<asp:TextBox ID="txtArea" runat="server" ToolTip="area appartenenza" TabIndex="3" CssClass="form-control"></asp:TextBox>--%>
                                <asp:FileUpload ID="FLFilein" runat="server" />
                                <asp:RequiredFieldValidator ID="RqFile" runat="server" ControlToValidate="FLFilein" ErrorMessage="Selezionare un file" ForeColor="Red" ValidationGroup="bt">

                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label3" runat="server" Text="Numero Fascicolo" CssClass="form-label d-block mb-2"></asp:Label>
                                <%--<label for="txPratica">Numero Fascicolo</label>--%>
                                <asp:TextBox ID="txtNrFascicolo" runat="server" CssClass="form-control form-control-sm w-50" />
                                <asp:RequiredFieldValidator ID="RqFascicolo" runat="server" ControlToValidate="txtNrFascicolo" ErrorMessage="Inserire il numero fascicolo" ForeColor="Red" ValidationGroup="bt">

                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group mb-3">
                                <div class="form-group text-center mt-4" style="text-align: left !important">

                                    <asp:Label ID="Label4" runat="server" Text="Data" CssClass="form-label d-block mb-2"></asp:Label>

                                    <asp:TextBox ID="txtDataI" runat="server" CssClass="form-control" placeholder="dd-MM-yyyy" />
                                    <asp:RequiredFieldValidator ID="RqData" runat="server" ControlToValidate="txtDataI" ErrorMessage="Inserire la data" ForeColor="Red" ValidationGroup="bt">

                                    </asp:RequiredFieldValidator>

                                    <asp:Label ID="lbmess" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div id="divSegreteria" runat="server" class="col-md-6" visible="false">
                        <div class="form-group mb-3">
                            <!-- Etichetta fascicolo -->
                            <div class="form-group text-center" style="text-align: left !important">
                                <asp:Label ID="lblm" runat="server" Text="Fascicolo" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:TextBox ID="TxtFascicolo" runat="server" ToolTip="numero fascicolo" TabIndex="1" CssClass="form-control"></asp:TextBox>

                            </div>
                        </div>
                        <!-- Etichetta Data -->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label1" runat="server" Text="Data" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:TextBox ID="TxtData" runat="server" ToolTip="data" TabIndex="2" CssClass="form-control"></asp:TextBox>

                            </div>
                        </div>
                        <!-- Etichetta Operartore -->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label5" runat="server" Text="Operatore" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:DropDownList ID="DdlOperatore" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <!-- Etichetta flag per la cancellazione -->
                        <div class="form-group mb-3">
                            <div class="form-group text-center mt-4" style="text-align: left !important">
                                <asp:Label ID="Label6" runat="server" Text="Cancella file già scaricati" CssClass="form-label d-block mb-2"></asp:Label>
                                <asp:Button ID="btCancellaScaricati" Text="Cancella" runat="server" OnClick="btCancellaScaricati_Click" ToolTip="Cancella File" CssClass="btn btn-primary px-4" />
                            </div>
                        </div>



                    </div>

                    <!-- Colonna Destra -->
                    <div id="divDestra" runat="server" class="col-md-6">


                        <div class="col-md-6" style="margin-top: 20px!important">
                            <div class="form-group mb-3">
                                <asp:Button ID="btCarica" Text="Carica" runat="server" OnClick="Carica_Click" ToolTip="Carica File" CssClass="btn btn-primary px-4" Visible="false" ValidationGroup="bt" />
                                <asp:Button ID="btRicerca" Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Cerca File" CssClass="btn btn-primary px-4" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </asp:Panel>
    <%-- Modale ricerca file --%>
    <div class="modal fade" id="ModalRicercaFile"  tabindex="-1" aria-labelledby="modalLabel" aria-hidden="false">
        <div class="modal-dialog" style="width: 90%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel10">Ricerca File</h5>

                </div>
                <div class="modal-body">

                    <div class="form-group">
                        <!-- GridView nel popup -->

                        <div class="form-group">
                            <!-- GridView nel popup -->
                            <asp:GridView ID="GVRicercaFile" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                OnRowDataBound="gvPopup_RowDataBound" OnRowCommand="gvPopup_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="GVRicercaFile_PageIndexChanging">

                                <Columns>
                                    <asp:BoundField DataField="id_file" HeaderText="ID" Visible="false"/>
                                    <asp:BoundField DataField="fascicolo" HeaderText="Numero Fascicolo" />
                                    <asp:BoundField DataField="data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                                    <asp:BoundField DataField="nomefile" HeaderText="Nome File" ItemStyle-Width="50%" />
                                    <asp:BoundField DataField="folder" HeaderText="folder" Visible="false" />

                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" Text="Seleziona"  CommandName="Select" CommandArgument='<%# Container.DataItemIndex + ";" + Eval("fascicolo") + ";" + Eval("data") + ";"  + Eval("nomefile") + ";" + Eval("folder") +";"+  Eval("id_file")  +";"  %>' CssClass="btn btn-success btn-sm" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                <PagerStyle HorizontalAlign="Center" />
                                <PagerTemplate>
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
                </div>
             
                <div class="modal-footer">
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi"  OnClick="btChiudi_Click"/>
                </div>
            </div>
        </div>
    </div>
    <%-- popup errori --%>
    <div class="modal fade" id="errorModal" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-hidden="false">
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
                    <asp:Button ID="Button2" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
