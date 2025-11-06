using AjaxControlToolkit;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static System.Windows.Forms.AxHost;
using static Uotep.Classi.Enumerate;



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



            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            //string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            //ProtocolloLiteral.Text = decodedText;
            if (!IsPostBack)
            {
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];
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
            Manager mn = new Manager();
            int anno = System.Convert.ToInt32(txtAnno.Text.Trim());
            int number = 0;
            // txtRelazioni.Text = stat.Rows[0].ItemArray[3].ToString();
            //TxtPonteggi.Text = stat.Rows[0].ItemArray[4].ToString();
            //txtDPI.Text = stat.Rows[0].ItemArray[5].ToString();
            //txtEspostiEvasi.Text = stat.Rows[0].ItemArray[7].ToString();
            //txtRipristino.Text = stat.Rows[0].ItemArray[8].ToString();
            //txtControlliScia.Text = stat.Rows[0].ItemArray[9].ToString();
            //txtCNR.Text = stat.Rows[0].ItemArray[11].ToString();
            //txtAnnotazioni.Text = stat.Rows[0].ItemArray[12].ToString();
            //txtRiapposizioneSigilli.Text = stat.Rows[0].ItemArray[15].ToString();
            //txtSequestri.Text = stat.Rows[0].ItemArray[14].ToString();
            //txtNotifiche.Text = stat.Rows[0].ItemArray[13].ToString();
            //txtDelegheEsitate.Text = stat.Rows[0].ItemArray[17].ToString();
            //txtConvalide.Text = stat.Rows[0].ItemArray[21].ToString();
            //txtViolazioneSigilli.Text = stat.Rows[0].ItemArray[23].ToString();
            // txtDissequestri.Text = stat.Rows[0].ItemArray[24].ToString();
            //txtDissequestriTemp.Text = stat.Rows[0].ItemArray[25].ToString();
            //txtRimozioneSigilli.Text = stat.Rows[0].ItemArray[26].ToString();
            //txtControlliDLGS.Text = stat.Rows[0].ItemArray[27].ToString();
            //txtViol_amm_reg_com.Text = stat.Rows[0].ItemArray[32].ToString();
            // txtControlliCant.Text = stat.Rows[0].ItemArray[30].ToString();
            //txtCensimentoAllPubb.Text = stat.Rows[0].ItemArray[33].ToString();
            //txtSgomberiAbus.Text = stat.Rows[0].ItemArray[36].ToString();
            //txtSgomberiImmobili.Text = stat.Rows[0].ItemArray[37].ToString();
            // txtNotificheNoAg.Text = stat.Rows[0].ItemArray[38].ToString();
            //txtOccupAbusivaAbit.Text = stat.Rows[0].ItemArray[34].ToString();
            //txtOccupAbusivaNoAbit.Text = stat.Rows[0].ItemArray[35].ToString();

            txtRelazioni.Text = mn.GetNumRelazione(txtMese.Text.Trim(), anno);

            TxtPonteggi.Text = mn.GetNumPonteggi(txtMese.Text.Trim(), anno);

            txtDPI.Text = mn.GetNumDpi(txtMese.Text.Trim(), anno);
            //prelevo il numero degli esposti ricevuti
            number = mn.GetEspostiRicevute(txtMese.Text.Trim(), anno);

            txtEspostiRicevuti.Text = Convert.ToString(number);
            number = 0;
            //

            txtEspostiEvasi.Text = mn.GetEspostiEvasi(txtMese.Text.Trim(), anno);
            txtRipristino.Text = mn.GetNumRipristino(txtMese.Text.Trim(), anno);

            txtControlliScia.Text = mn.GetNumcontrolliScia(txtMese.Text.Trim(), anno);
            txtCNR.Text = mn.GetNumCnr(txtMese.Text.Trim(), anno);

            txtAnnotazioni.Text = mn.GetNumAnnotazioni(txtMese.Text.Trim(), anno);

            txtNotifiche.Text = mn.GetNumNotifiche(txtMese.Text.Trim(), anno);
            txtSequestri.Text = mn.GetNumSequestri(txtMese.Text.Trim(), anno);

            txtRiapposizioneSigilli.Text = mn.GetNumRiappSigilli(txtMese.Text.Trim(), anno);

            //prelevo le deleghe ricevute dalla procura
            number = mn.GetDelegheRicevute(txtMese.Text.Trim(), anno);
            txtDelegheRicevute.Text = Convert.ToString(number);
            //

            txtDelegheEsitate.Text = mn.GetNumDelegheEsitate(txtMese.Text.Trim(), anno);
            txtInterrogatori.Text = stat.Rows[0].ItemArray[19].ToString();
            txtDenunceUff.Text = stat.Rows[0].ItemArray[20].ToString();

            txtConvalide.Text = mn.GetNumConvalide(txtMese.Text.Trim(), anno);
           
            txtDemolizioni.Text = stat.Rows[0].ItemArray[22].ToString();
            txtViolazioneSigilli.Text = mn.GetNumViolSigilli(txtMese.Text.Trim(), anno);
           
            txtDissequestri.Text = mn.GetNumDissequestri(txtMese.Text.Trim(), anno);
           
            txtDissequestriTemp.Text = mn.GetNumDisseqTemp(txtMese.Text.Trim(), anno);
           
            txtRimozioneSigilli.Text = mn.GetNumRimozSigilli(txtMese.Text.Trim(), anno);
            
            //beni culturali
            txtControlliDLGS.Text = mn.GetNumControlliDlgs(txtMese.Text.Trim(), anno);
            
            txtControlliCant.Text = mn.GetNumControlliCant(txtMese.Text.Trim(), anno);
            txtViol_amm_reg_com.Text = mn.GetNumViolAmm(txtMese.Text.Trim(), anno);
            txtCensimentoAllPubb.Text = mn.GetNumCensimentoAllPubb(txtMese.Text.Trim(), anno);
            txtOccupAbusivaAbit.Text = mn.GetNumOccAbusAbitat(txtMese.Text.Trim(), anno);
            txtOccupAbusivaNoAbit.Text = mn.GetNumOccAbusNoAbitat(txtMese.Text.Trim(), anno);


            txtSgomberiAbus.Text = mn.GetNumSgomberiAbus(txtMese.Text.Trim(), anno);
            txtSgomberiImmobili.Text = mn.GetNumSgomberiImmobili(txtMese.Text.Trim(), anno);
            txtNotificheNoAg.Text = mn.GetNumNotificheNoAg(txtMese.Text.Trim(), anno);

        }




        protected void btEsegui_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = new DataTable();
            int anno = System.Convert.ToInt32(txtAnno.Text.Trim());
            dt = mn.GetStatistiche(txtMese.Text.Trim(), anno);
            if (dt.Rows.Count > 0)
            {
                FillScheda(dt);
            }
        }
    }
}