<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Segreteria.aspx.cs" Inherits="Uotep.Segreteria" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }

    </script>
    <div class="jumbotron">
        <h1>Caricamento file</h1>
        <p class="lead">Il nome del file da caricare deve composto nel seguente modo: fascicolo_data</p>
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

                        <div class="form-group">
                            <!-- GridView nel popup -->
                            <asp:GridView ID="GVRicercaFile" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                OnRowDataBound="gvPopup_RowDataBound" OnRowCommand="gvPopup_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="id_file" HeaderText="ID" />
                                    <asp:BoundField DataField="fascicolo" HeaderText="Numero Fascicolo" />
                                    <%-- formatta la data DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" --%>
                                    <asp:BoundField DataField="data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                                    <asp:BoundField DataField="nomefile" HeaderText="Nome File" ItemStyle-Width="100%" />
                                    <asp:BoundField DataField="folder" HeaderText="folder" Visible="false" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Container.DataItemIndex + ";" + Eval("fascicolo") + ";" + Eval("data") + ";"  + Eval("nomefile") + ";" + Eval("folder") + ";"  %>' CssClass="btn btn-success btn-sm" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                   <%-- <asp:TemplateField HeaderText="Link">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Text="Apri File" CommandName="AddLink" CommandArgument='<%# Container.DataItemIndex + ";" + Eval("fascicolo") + ";" + Eval("data") + ";"  + Eval("nomefile") + ";" + Eval("folder") + ";"  %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>

                        </div>


                    </div>

                    <!-- Colonna Destra -->
                    <div id="divDestra" runat="server" class="col-md-6">


                        <div class="col-md-6" style="margin-top: 20px!important">
                            <div class="form-group mb-3">
                                <asp:Button ID="btCarica" Text="Carica" runat="server" OnClick="Carica_Click" ToolTip="Carica File" CssClass="btn btn-primary px-4" Visible="false" />
                                <asp:Button ID="btRicerca" Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Cerca File" CssClass="btn btn-primary px-4" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
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
                    <%--<asp:Button ID="Button2" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
