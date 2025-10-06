<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionePratica.aspx.cs" Inherits="Uotep.GestionePratica" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }

    </script>

    <div class="panel panel-default">
        <div class="form-group mb-3"></div>

        <div class="panel-body" id="divStat" runat="server">
            <div class="jumbotron">
                <div style="margin-top: -90px!important">
                    <%--<asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>--%>
                    <p class="text-center lead">GESTIONE PRATICHE</p>
                </div>

                <div class="container">
                    <div class="row">
                        <!-- Colonna 1 -->
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <asp:Label ID="Label6" runat="server">Fascicolo</asp:Label>
                                <asp:TextBox ID="txtFascicolo" runat="server" CssClass="form-control" Font-Bold="true" ForeColor="Red" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFascicolo" ValidationGroup="bt" ErrorMessage="Inserire numero fascicolo" ForeColor="Red">
                                </asp:RequiredFieldValidator>

                            </div>

                            <div class="form-group mb-3">
                                <asp:Label ID="Label5" runat="server">Assegnato</asp:Label>
                                <asp:TextBox ID="txtAssegnato" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAssegnato" ValidationGroup="bt" ErrorMessage="Inserire data uscita" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Colonna 2 -->
                        <div class="col-md-3 d-flex flex-column justify-content-center">
                            <div class="form-group mb-3">
                                <asp:Label ID="Label4" runat="server">Data Uscita</asp:Label>
                                <asp:TextBox ID="TxtDataUscita" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtDataUscita" ValidationGroup="bt" ErrorMessage="Inserire data uscita" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator1"
                                    runat="server"
                                    ControlToValidate="TxtDataUscita"
                                    ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$"
                                    ErrorMessage="la data deve essere dd/mm/aaaa"
                                    ForeColor="Red"
                                    ValidationGroup="bt"
                                    Display="Static">
                                </asp:RegularExpressionValidator>
                            </div>

                            <div class="form-group mb-3" style="margin-top: -15px!important">
                                <asp:Label ID="Label3" runat="server">Data Rientro</asp:Label>
                                <asp:TextBox ID="txtDataRientro" runat="server" CssClass="form-control" />

                            </div>


                        </div>
                        <!-- Colonna 3 -->
                        <div class="col-md-3">



                            <div class="form-group mb-3">
                                <div class="form-group mb-3">
                                    <asp:Label ID="Label2" runat="server">Data Spostamento</asp:Label>
                                    <asp:TextBox ID="txtDataSpostamento" runat="server" CssClass="form-control" />

                                </div>
                            </div>

                            <div class="form-group mb-3" style="margin-top: 40px!important">
                                <div class="form-group mb-3">
                                    <asp:Label ID="Label1" runat="server">Data Riscontro</asp:Label>
                                    <asp:TextBox ID="txtDataRiscontro" runat="server" CssClass="form-control" />

                                </div>
                            </div>

                        </div>
                        <%-- colonna4 --%>
                        <div class="col-md-3">



                            <div class="form-group mb-3">
                                <label for="txtNotaSpostamento"></label>
                                <asp:Label ID="Label7" runat="server">Nota Spostamento</asp:Label>
                                <asp:TextBox ID="txtNotaSpostamento" runat="server" CssClass="form-control" />
                            </div>

                            <div class="form-group mb-3" style="margin-top: 40px!important">
                                <asp:Label ID="Label8" runat="server">Nota Riscontro</asp:Label>
                                <asp:TextBox ID="txtNotaRiscontro" runat="server" CssClass="form-control" />
                            </div>

                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group mb-3">
                            <asp:Label ID="lbl" runat="server" Style="margin-left: 20px">Note</asp:Label>
                            <asp:TextBox ID="txtNota" runat="server" CssClass="form-control" Height="100px" TextMode="MultiLine" Style="margin-left: 20px; width: 100%; max-width: 800px;" />
                        </div>
                    </div>
                </div>
                <!-- Bottone Salva -->
                <div class="col-md-12 ">
                    <div class="form-group">
                        <asp:Button ID="btSalva" runat="server" ValidationGroup="bt" Text="Inserisci" CssClass="btn btn-primary" OnClick="btInserisci_Click" />

                        <asp:Button ID="btRicerca" runat="server" Text="Ricerca" CssClass="btn btn-primary" OnClick="btRicerca_Click" />
                        <asp:Button ID="btModifica" runat="server" Text="Modifica" CssClass="btn btn-primary" OnClick="btModifica_Click" />
                        <asp:Button ID="BtEstraiTabella" runat="server" Text="Estrazione" CssClass="btn btn-primary" OnClick="BtEstraiTabella_Click" />

                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <asp:GridView ID="GVRicercaFascicolo" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                OnRowDataBound="gvPopup_RowDataBound" OnRowCommand="gvPopup_RowCommand" OnRowDeleting="GVRicercaFascicolo_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="id_gestionePratica" HeaderText="ID" Visible="false" />
                    <asp:BoundField DataField="Fascicolo" HeaderText="Numero Fascicolo" />
                    <asp:BoundField DataField="Assegnato" HeaderText="Assegnato" />
                    <asp:BoundField DataField="data_uscita" HeaderText="Data Uscita" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Data_Rientro" HeaderText="Data Rientro" />
                    <%--<asp:TemplateField HeaderText="Data Rientro">
                        <ItemTemplate>
                            <%# FormatMyDate(Eval("data_rientro")) %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="Data_Spostamenti" HeaderText="Data Spostamento" />
                    <asp:BoundField DataField="NOTA_SPOSTAMENTO" HeaderText="Nota Spostamento" />

                    <asp:BoundField DataField="Data_Riscontro" HeaderText="Data Riscontro" />
                    <asp:BoundField DataField="NOTA_RISCONTRO" HeaderText="Nota Riscontro" />

                    <asp:BoundField DataField="note" HeaderText="Note" />

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("id_gestionePratica") %>' CssClass="btn btn-success btn-sm" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Elimina" CommandName="Delete" CommandArgument='<%# Eval("id_gestionePratica") %>' CssClass="btn btn-success btn-sm" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

        </div>
    </div>
    <asp:HiddenField ID="HfIdFascicolo" runat="server" />


    <%-- il seguente style serve per i bordi azzurri --%>
    <style>
        .custom-border {
            border: 2px solid #007bff; /* Cornice blu */
            border-radius: 8px; /* Angoli arrotondati */
            padding: 15px; /* Spazio interno */
            margin: 5px 0; /* Spazio esterno */
            margin-left: -10px;
        }

        .larghezzaText {
            width: 110px;
        }
    </style>
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
