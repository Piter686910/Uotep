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
using System.Globalization;
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
        int totalImpalcature = 0;
        int totalCensimento = 0;
        int totalDPI = 0;
        int totalOccAbitativo = 0;
        int totalOccNoAbitativo = 0;
        int totalCantSeq = 0;
        int totalEsposti = 0;
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
            DataTable ob = new DataTable();
            int anno = System.Convert.ToInt32(txtAnno.Text.Trim());
            if (String.IsNullOrEmpty(txtMese.Text.Trim()))
            {
                dt = mn.GetStatisticheAnnuali(anno);
                if (dt.Rows.Count > 0)
                {
                    GvStatAnnuale.DataSource = dt;
                    GvStatAnnuale.DataBind();
                    divDettagli.Visible = false;
                    DivAnnuale.Visible = true;
                    ob = mn.getObiettivi(anno);
                    if (ob.Rows.Count > 0)
                    {
                        GvObiettivi.DataSource = ob;
                        GvObiettivi.DataBind();
                        DivObiettivi.Visible = true;
                    }
                }
            }
            else
            {
                dt = mn.GetStatistiche(txtMese.Text.Trim(), anno);

                //dt = mn.GetStatistiche(txtMese.Text.Trim(), anno);
                if (dt.Rows.Count > 0)
                {
                    divDettagli.Visible = true;
                    DivAnnuale.Visible = false;
                    DivObiettivi.Visible = false;
                    FillScheda(dt);
                }
            }

        }

        protected void GvStatAnnuale_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // 1. Logica del Nome del Mese (vedi risposta precedente)
                // La tua colonna "Mese" è Cells[0]
                if (int.TryParse(e.Row.Cells[0].Text, out int month))
                {
                    // Usa il DataItem per leggere il valore originale se hai cambiato il testo prima!
                    // Altrimenti, se usi DATENAME in SQL, e.Row.Cells[0].Text è già il nome del mese.
                }

                // 2. Accumulo dei Totali
                // Ipotizzando che le colonne siano in questo ordine: 
                // 0=Mese, 1=Impalcature, 2=DPI, 3=OccAbitativo, 4=OccNoAbitativo

                // Per sicurezza, usa il DataItem per prendere i valori originali (che sono numeri interi)
                DataRowView drv = (DataRowView)e.Row.DataItem;
                totalImpalcature += Convert.ToInt32(drv["rapp_contr_cantiere_suolo_pubb"]);
                totalDPI += Convert.ToInt32(drv["rapp_contr_lavori_edili"]);
                totalCantSeq += Convert.ToInt32(drv["rapp_contr_cantieri_seq"]);
                totalEsposti += Convert.ToInt32(drv["rapp_numEsposti"]);
                totalCensimento += Convert.ToInt32(drv["rapp_censimento_all_pubb"]);

                totalOccAbitativo += Convert.ToInt32(drv["rapp_contr_occ_abitativo"]);
                totalOccNoAbitativo += Convert.ToInt32(drv["rapp_contr_occ_no_abitativo"]);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                // 3. Visualizzazione dei Totali nel Footer

                // Colonna Mese (0) - Intestazione
                e.Row.Cells[0].Text = "TOTALE:";
                e.Row.Cells[0].Font.Bold = true;
                // Colonna Impalcature (1)
                e.Row.Cells[1].Text = totalImpalcature.ToString();
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right; // Opzionale: allinea a destra
                e.Row.Cells[1].Font.Bold = true;
                // Colonna DPI (2)
                e.Row.Cells[2].Text = totalDPI.ToString();
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Font.Bold = true;
                // Colonna contr. cant sequestrati (3)
                e.Row.Cells[3].Text = totalCantSeq.ToString();
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].Font.Bold = true;
                // Colonna esposti (4)
                e.Row.Cells[4].Text = totalEsposti.ToString();
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].Font.Bold = true;

                // Colonna Censimento (5)
                e.Row.Cells[5].Text = totalCensimento.ToString();
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right; // Opzionale: allinea a destra
                e.Row.Cells[5].Font.Bold = true;




                // Colonna Occ. Abitat. (6)
                e.Row.Cells[6].Text = totalOccAbitativo.ToString();
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].Font.Bold = true;

                // Colonna Occ. No Abitat. (7)
                e.Row.Cells[7].Text = totalOccNoAbitativo.ToString();
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[7].Font.Bold = true;
            }
        }

        protected void GvStatAnnuale_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}