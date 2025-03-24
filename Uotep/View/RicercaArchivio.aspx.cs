using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;


namespace Uotep
{
    public partial class RicercaArchivio : Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {


           
            // Legge il valore dal Web.config
            string protocolloText = ConfigurationManager.AppSettings["TitoloArchivioUote"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            ProtocolloLiteral.Text = decodedText;
            

        }


        private void Pulisci()
        {

            txtPratica.Text = String.Empty;
            txtSub.Text = string.Empty;
            txtParticella.Text = string.Empty;
            txtFoglio.Text = string.Empty;
            txtSez.Text = string.Empty;
            txtIndirizzo.Text = string.Empty;
            txtNominativo.Text = String.Empty;


        }


        protected void Ricerca_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtNominativo.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Nominativo", txtNominativo.Text };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;

            }
            if (!string.IsNullOrEmpty(txtPratica.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Pratica", txtPratica.Text };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;
            }
            if (!string.IsNullOrEmpty(txtIndirizzo.Text))
            {
                // Crea una lista
                List<string> ListRicerca = new List<string> { "Indirizzo", txtIndirizzo.Text };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;
            }
            if (!string.IsNullOrEmpty(txtSez.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Catasto", txtSez.Text, txtFoglio.Text, txtParticella.Text, txtSub.Text  };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;

            }
            // Reindirizza alla pagina di destinazione
            Response.Redirect("InserimentoArchivio.aspx");
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);
            Pulisci();
        }
        protected void chiudipopupErrore_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('errorModal')); modal.hide();", true);

        }

        protected void btDatiCatastali_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = false;
            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = true;
            DivPratica.Visible = false;
        }

        protected void btIndirizzo_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = false;
            DivIndirizzo.Visible = true;
            DivDatiCatastali.Visible = false;
            DivPratica.Visible = false;
        }

        protected void btNominativo_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = true;
            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = false;
            DivPratica.Visible = false;
        }

        protected void btNpratica_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = false;
            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = false;
            DivPratica.Visible = true;
        }


    }
}