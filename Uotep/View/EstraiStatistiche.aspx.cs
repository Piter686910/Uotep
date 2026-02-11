using AjaxControlToolkit;
using AjaxControlToolkit.HtmlEditor.Popups;
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
        int totalNumcontrNatoDaAcc = 0;
        int totalCantSeq = 0;
        int totalEsposti = 0;
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Manager mn = new Manager();
        String profilo = string.Empty;
        String ruolo = string.Empty;
        string paginaChiamante = "~/View/EstraiStatistiche.aspx";
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
                // ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);


            }

        }

        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //azzero i dati della gridview e chiudo il popup e imposto a 0 la pagina della gridview per le nuove ricerche
            GVRicercaScheda.DataSource = null;
            GVRicercaScheda.DataBind();
            GVRicercaScheda.PageIndex = 0;
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);

        }



        protected void FillScheda(DataTable stat)
        {
            Manager mn = new Manager();
            string msg = string.Empty;
            int anno = System.Convert.ToInt32(txtAnno.Text.Trim());
            int number = 0;


            txtRelazioni.Text = mn.GetNumRelazione(txtMese.Text.Trim(), anno);

            TxtPonteggi.Text = mn.GetNumPonteggi(txtMese.Text.Trim(), anno);

            txtDPI.Text = mn.GetNumDpi(txtMese.Text.Trim(), anno);
            //prelevo il numero degli esposti ricevuti
            number = mn.GetEspostiRicevute(txtMese.Text.Trim(), anno, out msg);
            if (!String.IsNullOrWhiteSpace(msg))
            {

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(msg + @" - Errore estrai statistiche riga 137 ");
                    sw.Close();
                }
                Session["MessaggioErrore"] = msg;
                Session["PaginaChiamante"] = paginaChiamante;
                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + msg);
            }

            txtEspostiRicevuti.Text = Convert.ToString(number);
            number = 0;
            //

            txtEspostiEvasi.Text = mn.GetEspostiEvasi(txtMese.Text.Trim(), anno);
            txtRipristino.Text = mn.GetNumRipristino(txtMese.Text.Trim(), anno);

            txtControlliScia.Text = mn.GetNumcontrolliScia(txtMese.Text.Trim(), anno);
            txtCNR.Text = mn.GetNumCnr(txtMese.Text.Trim(), anno);

            txtAnnotazioni.Text = mn.GetNumAnnotazioni(txtMese.Text.Trim(), anno);

            txtNotifiche.Text = mn.GetNumNotifiche(txtMese.Text.Trim(), anno);


            txtRiapposizioneSigilli.Text = mn.GetNumRiappSigilli(txtMese.Text.Trim(), anno);

            //prelevo le deleghe ricevute dalla procura
            number = mn.GetDelegheRicevute(txtMese.Text.Trim(), anno);
            txtDelegheRicevute.Text = Convert.ToString(number);
            //

            txtDelegheEsitate.Text = mn.GetNumDelegheEsitate(txtMese.Text.Trim(), anno);
            //SiteMaster myMaster = this.Master as SiteMaster;

            //if (myMaster != null)
            //{
            //    // 2. Chiamo il metodo pubblico
            //    myMaster.MostraMessaggio("ATTENZIONE", txtDelegheEsitate.Text, "warning");
            //}
            txtInterrogatori.Text = stat.Rows[0].ItemArray[19].ToString();
            txtDenunceUff.Text = stat.Rows[0].ItemArray[20].ToString();

            txtConvalide.Text = mn.GetNumConvalide(txtMese.Text.Trim(), anno);

            txtDemolizioni.Text = stat.Rows[0].ItemArray[22].ToString();
            txtViolazioneSigilli.Text = mn.GetNumViolSigilli(txtMese.Text.Trim(), anno);

            txtDissequestri.Text = mn.GetNumDissequestri(txtMese.Text.Trim(), anno);

            txtDissequestriTemp.Text = mn.GetNumDisseqTemp(txtMese.Text.Trim(), anno);
            txtSequestri.Text = mn.GetNumSequestri(txtMese.Text.Trim(), anno);//num verbali sequestri

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
            txtAccertAltriEnti.Text = mn.GetNumAccertAltriEnti(txtMese.Text.Trim(), anno);
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
                totalNumcontrNatoDaAcc += Convert.ToInt32(drv["rapp_NumcontrNatoDaAcc"]);
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
                // Colonna Numero controli Nato Da accertamenti (8)
                e.Row.Cells[8].Text = totalNumcontrNatoDaAcc.ToString();
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[8].Font.Bold = true;

            }
        }

        protected void GvStatAnnuale_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void BtnInfo_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string parametro = btn.CommandArgument;
            Manager mn = new Manager();
            DataTable schede = mn.GetSchedeInfo(parametro, txtMese.Text, txtAnno.Text);

            if (schede.Rows.Count > 0)
            {
                GVRicercaScheda.DataSource = schede;
                GVRicercaScheda.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
            }
            else
            {
                //richiama popup dalla site master
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.NoStatistiche.GetDescription(), "warning");
                }
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non trovata." + "'); $('#errorModal').modal('show');", true);

            }
        }

        protected void GVRicercaScheda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id_rapp_scheda").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
                if (GVRicercaScheda.TopPagerRow != null)
                {
                    // Trova il controllo Label all'interno del PagerTemplate
                    Label lblPageInfo = (Label)GVRicercaScheda.TopPagerRow.FindControl("lblPageInfo");
                    if (lblPageInfo != null)
                    {
                        // Calcola e imposta il testo
                        int currentPage = GVRicercaScheda.PageIndex + 1;
                        int totalPages = GVRicercaScheda.PageCount;
                        lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                    }
                }

            }
        }

        protected void GVRicercaScheda_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void RicaricaGrid()
        {
            Manager mn = new Manager();
            DataTable dt = new DataTable();
            int anno = System.Convert.ToInt32(txtAnno.Text.Trim());
            if (!String.IsNullOrEmpty(txtMese.Text.Trim()))


                dt = mn.GetSchedeInfo("ControlliCant", txtMese.Text.Trim(), txtAnno.Text.Trim());

            if (dt.Rows.Count > 0)
            {
                GVRicercaScheda.DataSource = dt;
                GVRicercaScheda.DataBind();
            }


        }
        protected void GVRicercaScheda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVRicercaScheda.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina

            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }
            RicaricaGrid();



            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);

        }
    }
}