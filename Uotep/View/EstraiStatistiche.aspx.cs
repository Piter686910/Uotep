using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Layout.Element;
using iText.Forms.Fields;
using iText.Forms;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Draw;
using System.Diagnostics;
using iText.Kernel.Pdf.Canvas;
using System.Data.SqlTypes;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using AjaxControlToolkit;
using static Uotep.Classi.Enumerate;
using System.Drawing;
using static System.Windows.Forms.AxHost;



namespace Uotep
{
    public partial class EstraiStatistiche : Page
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
            // Legge il valore dal Web.config
            string protocolloText = ConfigurationManager.AppSettings["Titolo"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            //string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            //ProtocolloLiteral.Text = decodedText;
            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);

             
                
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

    

        protected void FillScheda(DataTable stat)
        {
            txtRelazioni.Text= stat.Rows[0].ItemArray[3].ToString();
            TxtPonteggi.Text = stat.Rows[0].ItemArray[4].ToString();
            txtDPI.Text = stat.Rows[0].ItemArray[5].ToString();
            txtEspostiRicevuti.Text = stat.Rows[0].ItemArray[6].ToString();
            txtEspostiEvasi.Text = stat.Rows[0].ItemArray[7].ToString();
            txtRipristino.Text = stat.Rows[0].ItemArray[8].ToString();
            txtControlliScia.Text = stat.Rows[0].ItemArray[9].ToString();
            txtControlliCant.Text = stat.Rows[0].ItemArray[10].ToString();
            txtCNR.Text = stat.Rows[0].ItemArray[11].ToString();
            txtAnnotazioni.Text = stat.Rows[0].ItemArray[12].ToString();
            txtNotifiche.Text = stat.Rows[0].ItemArray[13].ToString();
            txtSequestri.Text = stat.Rows[0].ItemArray[14].ToString();
            txtRiapposizioneSigilli.Text = stat.Rows[0].ItemArray[15].ToString();
            txtDelegheRicevute.Text = stat.Rows[0].ItemArray[16].ToString();
            txtDelegheEsitate.Text= stat.Rows[0].ItemArray[17].ToString();
            txtCnrAnnotazioni.Text = stat.Rows[0].ItemArray[18].ToString();
            txtInterrogatori.Text = stat.Rows[0].ItemArray[19].ToString();
            txtDenunceUff.Text = stat.Rows[0].ItemArray[20].ToString();
            txtConvalide.Text = stat.Rows[0].ItemArray[21].ToString();
            txtDemolizioni.Text = stat.Rows[0].ItemArray[22].ToString();
            txtViolazioneSigilli.Text = stat.Rows[0].ItemArray[23].ToString();
            txtDissequestri.Text = stat.Rows[0].ItemArray[24].ToString();
            txtDissequestriTemp.Text = stat.Rows[0].ItemArray[25].ToString();
            txtRimozioneSigilli.Text = stat.Rows[0].ItemArray[26].ToString();
            txtControlliDLGS.Text = stat.Rows[0].ItemArray[27].ToString();
            //txtControlliCant.Text = stat.Rows[0].ItemArray[28].ToString();
            //txt.Text = stat.Rows[0].ItemArray[29].ToString();
        }

     
        private Boolean Convalida()
        {
            Boolean ret = true;
            

            return ret;
        }
       
       
        private void Pulisci()
        {
           

        }
       

        protected void btEsegui_Click(object sender, EventArgs e)
        {
            Manager mn= new Manager();
            DataTable dt = new DataTable();
            int anno= System.Convert.ToInt32(txtAnno.Text.Trim());
            dt = mn.GetStatistiche(txtMese.Text.Trim(), anno);
            if (dt.Rows.Count > 0)
            { 
                FillScheda(dt);
            }
        }
    }
}