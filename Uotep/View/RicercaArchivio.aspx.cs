using ClosedXML.Excel;
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
        public String Filename = ConfigurationManager.AppSettings["CartellaFileArchivio"];
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
            txtResponsabile.Text = String.Empty;
            Ck1089.Checked = false;
            CkDemolita.Checked = false;
            CkEvasa.Checked = false;
            CkPropAltri.Checked = false;
            CkPropBeniCult.Checked = false;
            CkPropComunale.Checked = false;
            CkPropPriv.Checked = false;
            CkSuoloPubblico.Checked = false;
            CkVincoli.Checked = false;


        }


        protected void Ricerca_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtResponsabile.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Nominativo", txtResponsabile.Text };

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
            DivEstraiDb.Visible = false;
            DivRicerca.Visible = true;
        }

        protected void btIndirizzo_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = false;
            DivIndirizzo.Visible = true;
            DivDatiCatastali.Visible = false;
            DivPratica.Visible = false;
            DivEstraiDb.Visible = false;
            DivRicerca.Visible = true;
        }

        protected void btNominativo_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = true;
            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = false;
            DivPratica.Visible = false;
            DivEstraiDb.Visible = false;
            DivRicerca.Visible = true;
        }

        protected void btNpratica_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = false;
            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = false;
            DivEstraiDb.Visible = false;
            DivPratica.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btEstraiParziale_Click(object sender, EventArgs e)
        {
            DivEstraiDb.Visible = true;
            DivRicerca.Visible = false;
        }

        protected void Estrai_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            //DataTable dt = mn.getArchivioUoteParziale();
        }

        protected void btEstraiTotale_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = mn.getArchivioUoteTotale();
            
            // 2. Esporta la DataTable in Excel
            string filePath = Path.Combine(Filename, "Estrazione del " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".xlsx");
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Dati");
                //wb.Worksheets.Add(dt, "Dati");  // Crea un foglio Excel con i dati
                ws.Column(1).Delete(); // Elimina la prima colonna
                ws.Columns().AdjustToContents();  //  Auto-fit delle colonne

                wb.SaveAs(filePath);  // Salva il file

            }
        }

    }
}