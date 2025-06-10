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
    public partial class StatistichePg : Page
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
            else
            {
                Response.Redirect("Default.aspx?user=true");
            }
            if (!IsPostBack)
            {
              //  ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);

            }

        }

        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);

        }



        protected void btInserisci_Click(object sender, EventArgs e)
        {
            Boolean exist = false;
            Boolean resp = false;
            Statistiche stat = new Statistiche();
            DataTable dt = new DataTable();
            int anno = System.Convert.ToInt32(txtYYYY.Text.Trim());
            int interrogatori = System.Convert.ToInt32(txtInterrogatorio.Text.Trim());
           
            
            dt = mn.getStatisticaByMeseAnno(txtMM.Text.Trim(), anno);
           
            if (dt.Rows.Count > 0)
            {
                exist = true; //eseguo update del campo interrogatori
                interrogatori += System.Convert.ToInt32(dt.Rows[0].ItemArray[19]);
                stat.interrogazioni = interrogatori;
                stat.mese = txtMM.Text.Trim();
                stat.anno = anno;
                resp = mn.InsStatPg(exist, stat);
            }
            else
            {
                //non esiste il nuovo mese anno quindi inserisco un nuovo record
                stat.interrogazioni = interrogatori;
                stat.mese = txtMM.Text.Trim();
                stat.anno = anno;
                resp = mn.InsStatPg(exist, stat);
            }
        }
    }
}