<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PagComandi.aspx.cs" Inherits="Uotep.PagComandi" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Contenitore principale centrato -->
    <div class="container mt-5">
        <!-- Jumbotron -->
        <div class="jumbotron text-center">
            <!-- Titolo -->
            <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="lead">PANNELLO COMANDI</p>
        </div>
        <%-- LOGIN --%>
        <%-- <asp:Panel ID="pnlLogin" runat="server" CssClass="text-center">
            <div class="row d-flex justify-content-center align-items-center vh-100">
                <div class="col-md-4">
                    <!-- Etichetta Matricola e Campo -->
                    <div class="form-group text-center">
                        <asp:Label ID="lblm" runat="server" Text="Matricola" CssClass="form-label d-block mb-2"></asp:Label>
                        <asp:TextBox ID="TxtMatricola" runat="server" ToolTip="matricola" TabIndex="1" CssClass="form-control"></asp:TextBox>
                    <asp:HiddenField ID="Hmatricola" runat="server" />
                    </div>

                    <!-- Etichetta Password e Campo -->
                    <div class="form-group text-center mt-4">
                        <asp:Label ID="Label1" runat="server" Text="Password" CssClass="form-label d-block mb-2"></asp:Label>
                        <asp:TextBox ID="TxtPassw" runat="server" ToolTip="password" MaxLength="8" TextMode="Password" TabIndex="2" CssClass="form-control"></asp:TextBox>
                    </div>

                    <!-- Pulsante Login -->
                    <div class="form-group text-center mt-4">
                        <asp:Button Text="Login" runat="server" OnClick="trova_Click" ToolTip="Login" CssClass="btn btn-primary px-4" />
                    </div>
                </div>
            </div>
        </asp:Panel>--%>
        <!-- Contenitore dei pulsanti -->
        <asp:Panel ID="pnlButton" runat="server" CssClass="text-center" Visible="true">
            <div class="d-flex justify-content-center mt-4">

                <p>
                    <!-- Pulsanti -->
                    <asp:Button ID="btInserimento" runat="server" Text="Inserimento" OnClick="InsDati_Click" CssClass="btn btn-primary mx-2" ToolTip="Inserimento Dati" />
                    <asp:Button ID="btRicerca" runat="server" Text="Ricerche" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-secondary mx-2" />
                    <asp:Button ID="btModifica" runat="server" OnClick="Modifica_Click" Text="Modifica" ToolTip="Modifica" CssClass="btn btn-success mx-2" />
                    <asp:Button ID="btModRiserv" runat="server" OnClick="ModRiserv_Click" Text="Modifica Riservata" ToolTip="Modifica Riservata" CssClass="btn btn-warning mx-2" />
                </p>

            </div>
        </asp:Panel>
    </div>

</asp:Content>

