<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Uote._test" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        $(document).ready(function () {
            // Funzione per aprire il modal e impostare il messaggio
            function showModal(message) {
                $('#modalMessage').text(message);  // Imposta il messaggio nel modal
                $('#myModal').modal('show');       // Mostra il modal
            }

            // Esempio di come chiamare la funzione
            $('#btnopen').click(function () {
                showModal('Questo è un messaggio di esempio!'); // Passa il messaggio al modal
            });
        });


        //function showModal() {
        //    //$('#myModal').modal({ show: true });

        // var myModal = new bootstrap.Modal(document.getElementById('myModal'));
        // myModal.show();

        //function ShowErrorMessage(message) {
        //    $('#errorModal').modal('show');
        //}
        //$('#openModal').click(function () {
        //           const myModal = new bootstrap.Modal(document.getElementById('myModal'));
        //           myModal.show();
        //       });
        // document.getElementById('openModal').addEventListener('click', function () {
        //const myModal = new bootstrap.Modal(document.getElementById('myModal'));
        //myModal.show();
        //}
    </script>



    <div class="jumbotron">
        <h1>ARCHIVIO PRATICHE U.O.T.E.</h1>
        <p class="lead"></p>
    </div>




       <div class="panel panel-default">
        <div class="form-group mb-3">
        </div>
        <div class="panel-heading">
            <h3 class="panel-title" style="font-weight: bold;">Intervento</h3>
        </div>
        <div class="form-check">
            <asp:CheckBox ID="CkAttivita" runat="server" AutoPostBack="true" OnCheckedChanged="CkAttivita_CheckedChanged1" />
            <%--<asp:CheckBox ID="CkAttivita" runat="server" CssClass="form-check-input" AutoPostBack="false" onchange='toggleDiv(this.id, \"txtPratica\")' />--%>
            <label class="form-check-label ms-3" for="CkAttivita">Attività Interna</label>
        </div>
        <div class="panel-body" id="divTesta" runat="server">
            <div class="jumbotron">
                <div style="margin-top: -50px!important">
                    <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
                    <p class="text-center lead">COMPILAZIONE SCHEDA INTERVENTO</p>
                </div>

                <div class="container">
                    <div class="row">
                        <!-- Colonna 1 -->
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <label for="txtPratica">Nr Pratica</label>
                                <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" Font-Bold="true" ForeColor="Red" />
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPratica" ErrorMessage="Inserire numero pratica" ForeColor="Red">
                                </asp:RequiredFieldValidator>--%>
                            </div>

                            <div class="form-group mb-3">
                                <label for="txtIndirizzo">Indirizzo</label>
                                <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" />
                            </div>

                            <div class="form-group mb-3">
                                <label for="txtNominativo">Nominativo</label>
                                <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNominativo" ErrorMessage="Inserire nominativo" ForeColor="Red">
                                </asp:RequiredFieldValidator>--%>
                            </div>


                        </div>

                        <!-- Colonna 2 -->
                        <div class="col-md-3 d-flex flex-column justify-content-center">
                            <div class="form-group mb-3">
                                <label for="TxtDataIntervento">Data Intervento</label>
                                <asp:TextBox ID="TxtDataIntervento" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtDataIntervento" ErrorMessage="Inserire data intervento" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group mb-3" style="margin-top: -20px!important">
                                <label for="txtDataConsegna">Data Consegna</label>
                                <asp:TextBox ID="txtDataConsegna" runat="server" CssClass="form-control" />
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDataConsegna" ErrorMessage="Inserire data intervento" ForeColor="Red">
                                </asp:RequiredFieldValidator>--%>
                            </div>

                            <div class="form-group mb-3">
                                <label for="DdlPattuglia">Pattuglia</label>
                                <asp:DropDownList ID="DdlPattuglia" runat="server" CssClass="form-control" />
                                <asp:ListBox ID="LPattugliaCompleta" runat="server" CssClass="form-control"></asp:ListBox>
                            </div>

                        </div>

                        <!-- Colonna 3 -->
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <label for="txtNote">Note</label>
                                <asp:TextBox ID="txtNote" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" />

                            </div>

                        </div>

                    </div>
                     <div class="row">
                    <div class="col-12 text-center">
                        <asp:Button ID="btA" runat="server" Text="Aggiungi" CssClass="btn btn-primary me-3" OnClick="btA_Click" />
                       <%-- <asp:Button Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary mt-3" />--%>
                    </div>
                </div>
                </div>
               
            </div>
        </div>
    </div>
</asp:Content>
