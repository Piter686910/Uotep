<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Uotep.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>PAGINA DI ERRORE</h2>
    <h3>Si è verificato il seguente errore</h3>
    <asp:Panel ID="pnlButton" runat="server" CssClass="text-center" Visible="true">
        <div class="d-flex justify-content-center mt-4">
             <asp:Label ID="LabelMessaggioErrore" runat="server" ></asp:Label>
        </div>
        <asp:Button ID="ButtonTornaIndietro" runat="server" Text="Torna alla Pagina Precedente" OnClick="ButtonTornaIndietro_Click" />
    </asp:Panel>
</asp:Content>
