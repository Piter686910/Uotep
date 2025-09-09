using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static System.Windows.Forms.AxHost;




namespace Uotep
{
    public partial class StatisticheAtti : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Manager mn = new Manager();
        String profilo = string.Empty;
        String ruolo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                profilo = Session["profilo"].ToString();
                ruolo = Session["ruolo"].ToString();
            }
           
            if (!IsPostBack)
            {
                //  ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);

            }

        }



        private void Pulisci()
        {


            txtDenunceUff.Text = string.Empty;
            txtEspostiRicevuti.Text = string.Empty;
            txtMM.Text = string.Empty;
            txtYYYY.Text = string.Empty;

        }


        protected void btInserisci_Click(object sender, EventArgs e)
        {
            Boolean exist = false;
            Boolean resp = false;
            Statistiche stat = new Statistiche();
            DataTable dt = new DataTable();
            int EspostiRicevuti = 0;
            int denunceUff = 0;
            int anno = System.Convert.ToInt32(txtYYYY.Text.Trim());
            if (!String.IsNullOrEmpty(txtEspostiRicevuti.Text.Trim()))

               EspostiRicevuti = System.Convert.ToInt32(txtEspostiRicevuti.Text.Trim());
            

            if (!String.IsNullOrEmpty(txtDenunceUff.Text.Trim()))

                 denunceUff = System.Convert.ToInt32(txtDenunceUff.Text.Trim());

            dt = mn.getStatisticaByMeseAnno(txtMM.Text.Trim(), anno);

            if (dt.Rows.Count > 0)
            {
                exist = true; //eseguo update del campo interrogatori
                EspostiRicevuti += System.Convert.ToInt32(dt.Rows[0].ItemArray[6]);
                denunceUff += System.Convert.ToInt32(dt.Rows[0].ItemArray[20]);
                stat.esposti_ricevuti = EspostiRicevuti;
                stat.denunce_uff = denunceUff;
                stat.mese = txtMM.Text.Trim().ToUpper();
                stat.anno = anno;
                resp = mn.InsStatatti(exist, stat);
            }
            else
            {
                //non esiste il nuovo mese anno quindi inserisco un nuovo record
                stat.esposti_ricevuti = EspostiRicevuti;
                stat.denunce_uff = denunceUff;
                stat.mese = txtMM.Text.Trim().ToUpper();
                stat.anno = anno;
                resp = mn.InsStatatti(exist, stat);
            }
            if (resp)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Dati  inserita correttamente ." + "'); $('#errorModal').modal('show');", true);
                Pulisci();
            }
        }
    }
}