using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using Uotep.Classi;

namespace Uotep
{
    public partial class PagComandi : Page
    {
        String Vuser = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Legge il valore dal Web.config
            string protocolloText = ConfigurationManager.AppSettings["Titolo"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            ProtocolloLiteral.Text = decodedText;
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
            }
            Manager mn = new Manager();
            //DataTable ricerca = mn.GetRuolo(Vuser);
            //Session["profilo"] = ricerca.Rows[0].ItemArray[0];

            if (!IsPostBack)
            {

                CaricaProfili();
                if (!string.IsNullOrEmpty(Vuser))
                {

                    pnlButton.Visible = true;
                }
                else
                {
                    pnlButton.Visible = false;
                }
            }
            // Response.Write("Parametro id: " + Vuser);
        }
        private void CaricaProfili()
        {
            switch (Session["profilo"].ToString())
            {

                case "1": //accertatori
                    btModifica.Enabled = false;
                    btModRiserv.Enabled = false;
                    btInserimento.Enabled = false;
                    break;
                case "2": //fureria
                    btModifica.Enabled = false;
                    btModRiserv.Enabled = false;
                    break;
                case "3": //admin
                    btModifica.Enabled = true;
                    btModRiserv.Enabled = false;
                    btInserimento.Enabled = true;
                    break;
                case "4": // master per ag
                    btModifica.Enabled = true;
                    btModRiserv.Enabled = true;
                    btInserimento.Enabled = true;
                    break;

                default:
                    btModifica.Enabled = true;
                    btModRiserv.Enabled = false;
                    btInserimento.Enabled = true;
                    break;
            }
        }
        protected void Ricerca_Click(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Response.Redirect("Visualizza.aspx", false);

            }
            //else
            //Response.Redirect("Visualizza.aspx?user=" + TxtMatricola.Text + "", false);


        }
        protected void ModRiserv_Click(object sender, EventArgs e)
        {

            Response.Redirect("ModificaRiservata.aspx", false);


        }
        protected void Modifica_Click(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Response.Redirect("Modifica.aspx", false);

            }
            //else
            //    Response.Redirect("Modifica.aspx?user=" + TxtMatricola.Text + "", false);
        }
        protected void InsDati_Click(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Response.Redirect("Inserimento.aspx", false);

            }
            //else
            //    Response.Redirect("Inserimento.aspx?user=" + TxtMatricola.Text + "", false);
        }
    }
}