<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Uotep._Default" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Sfondo professionale e font */
        body {
            background-color: #f0f2f5;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        /* Contenitore per centrare la card */
        .main-wrapper {
            min-height: 80vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
        }

        /* Card in stile moderno */
        .login-card {
            background: #ffffff;
            border: none;
            border-radius: 12px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.1);
            width: 100%;
            max-width: 400px;
            overflow: hidden;

        }

        .card-header-gradient {
            background: linear-gradient(135deg, #0062cc 0%, #004085 100%);
            color: white;
            padding: 25px;
            text-align: center;
        }

        .card-header-gradient h2 {
            margin: 0;
            font-size: 1.5rem;
            font-weight: 600;
            letter-spacing: 1px;
        }

        .card-body-custom {
            padding: 30px;

        }

        /* Input personalizzati */
        .form-control-custom {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 12px;
            transition: all 0.3s;
        }

        .form-control-custom:focus {
            border-color: #0062cc;
            box-shadow: 0 0 0 0.2rem rgba(0,98,204,0.15);
        }

        /* Pulsanti */
        .btn-custom {
            border-radius: 6px;
            padding: 12px;
            font-weight: 600;
            transition: transform 0.2s;
        }

        .btn-custom:hover {
            transform: translateY(-1px);
        }

        .reset-link-container {
            text-align: center;
            margin-top: 15px;
        }

        .reset-link-container a {
            color: #6c757d;
            font-size: 0.9rem;
            text-decoration: none;
        }

        .reset-link-container a:hover {
            color: #0062cc;
            text-decoration: underline;
        }
    </style>

    <script>
        function ShowErrorMessage(message) {
            $('#errorMessage').text(message);
            $('#errorModal').modal('show');
        }
    </script>

    <div class="main-wrapper">
        <asp:Panel ID="pnlLogin" runat="server" CssClass="login-card">
            <div class="card-header-gradient">
                <h2>U.O.T.E.P.</h2>
                <small style="opacity: 0.8;">Area Riservata - Accesso</small>
            </div>

            <div class="card-body-custom" >
                <div class="mb-3">
                    <asp:Label ID="lblm" runat="server" Text="Matricola" CssClass="form-label fw-bold"></asp:Label>
                    <asp:TextBox ID="TxtMatricola" runat="server" CssClass="form-control form-control-custom" placeholder="Inserisci matricola" TabIndex="1"></asp:TextBox>
                    <asp:HiddenField ID="Hmatricola" runat="server" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="Label1" runat="server" Text="Password" CssClass="form-label fw-bold"></asp:Label>
                    <asp:TextBox ID="TxtPassw" runat="server" TextMode="Password" CssClass="form-control form-control-custom" placeholder="••••••••" TabIndex="2"></asp:TextBox>
                </div>

                <div id="DivNewPassw" runat="server" class="mb-3 p-3 bg-light border border-info rounded" visible="false">
                    <asp:Label ID="Label2" runat="server" Text="Nuova Password" CssClass="form-label fw-bold text-info"></asp:Label>
                    <asp:TextBox ID="txtNewPassw" runat="server" TextMode="Password" CssClass="form-control form-control-custom border-info" ></asp:TextBox>
                </div>

                <div class="d-grid gap-2 mt-4">
                    <asp:Button ID="btLogin" Text="Accedi" runat="server" OnClick="trova_Click" CssClass="btn btn-primary btn-custom" />
                    <asp:Button ID="btsave" Text="Salva Nuova Password" runat="server" OnClick="SalvaPassw_Click" CssClass="btn btn-success btn-custom" Visible="false" />
                </div>

                <div class="reset-link-container">
                    <asp:LinkButton ID="lkreset" OnClick="lkreset_Click" runat="server">Dimenticato la password? Reset</asp:LinkButton>
                </div>
            </div>
        </asp:Panel>
    </div>

    <%-- Modal Errori --%>
    <div class="modal fade" id="errorModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 shadow">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">ATTENZIONE</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body p-4 text-center">
                    <p id="errorMessage" class="text-danger fw-bold mb-0"></p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btChiudiPop" runat="server" CssClass="btn btn-secondary w-100" Text="Ho capito" OnClick="btChiudiPop_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>