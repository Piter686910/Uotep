using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.Caching;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;

namespace Uotep
{
    public partial class SchedaIntervento : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        //String Area = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Manager mn = new Manager();
        MemoryCache _cache = MemoryCache.Default;
        protected void Page_Load(object sender, EventArgs e)
        {


            //if (Session["user"] != null)
            //{
            //    Vuser = Session["user"].ToString();
            //    //Area = Session["area"].ToString();
            //}
            if (_cache != null)
            {
                // 2. Recuperare un parametro dalla cache
                Vuser = _cache.Get("user") as string;
            }
            else
            {
                Response.Redirect("Default.aspx?user=true");
            }

            if (!IsPostBack)
            {
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                ProtocolloLiteral.Text = decodedText;
                TxtDataIntervento.Attributes["placeholder"] = "gg/mm/aaaa";
                txtDataConsegna.Attributes["placeholder"] = "gg/mm/aaaa";
                //if (Area == "uote")

                rdUote.Checked = true;
                //else
                //  rdUotp.Checked = true;

                CaricaDLL();
            }

        }

        protected void Salva_Click(object sender, EventArgs e)
        {
            Boolean continua = Convalida();
            String MeseCorrente = DateTime.Now.ToString("MMMM");
            String AnnoCorrente = DateTime.Now.ToString("yyyy");
            Manager mn = new Manager();

            if (continua)
            {
                RappUote rap = new RappUote();
                Statistiche stat = new Statistiche();

                rap.nominativo = txtNominativo.Text.ToUpper();
                rap.indirizzo = txtIndirizzo.Text;
                if (DdlPattuglia.SelectedValue != "0")
                {
                    for (int i = 0; i < LPattugliaCompleta.Items.Count; i++)
                    {
                        rap.pattuglia += LPattugliaCompleta.Items[i].Value + "/";
                    }
                }

                if (!string.IsNullOrEmpty(TxtDataIntervento.Text))
                {
                    rap.data = System.Convert.ToDateTime(TxtDataIntervento.Text);
                }
                if (ddlCapopattuglia.SelectedValue != "0")
                {
                    if (!string.IsNullOrEmpty(ddlCapopattuglia.SelectedItem.Text))
                    {
                        rap.capopattuglia = ddlCapopattuglia.SelectedItem.Text;
                    }
                }
                rap.matricola = Vuser;
                //oraapp = dtOra.Value);
                //rap.ora = dtOra.Value;
                rap.pratica = txtPratica.Text;
                //if (ckDelega.Checked)
                //{
                //    stat.deleghe_ricevute = 1;
                //}
                rap.delegaAG = ckDelega.Checked;
                rap.resa = ckResa.Checked;
                rap.segnalazione = ckSegnalazione.Checked;

                rap.esposti = ckEsposto.Checked;
                if (ckNotifica.Checked)
                {
                    stat.notifiche = 1;
                }
                rap.notifica = ckNotifica.Checked;
                rap.iniziativa = ckIniziativa.Checked;
                rap.cdr = ckCdr.Checked;
                rap.coordinatore = ckCoordinatore.Checked;
                if (ckRelazione.Checked)
                {
                    stat.relazioni = 1;
                }

                rap.relazione = ckRelazione.Checked;
                if (ckCnr.Checked)
                {
                    stat.cnr = 1;
                }
                rap.cnr = ckCnr.Checked;
                if (ckAnnotazionePG.Checked)
                {
                    stat.annotazioni = 1;
                }
                rap.annotazionePG = ckAnnotazionePG.Checked;
                if (ckVerbaleSeq.Checked)
                {
                    stat.sequestri = 1;
                }
                rap.verbaleSeq = ckVerbaleSeq.Checked;
                if (ckEsitoDelega.Checked)
                {
                    stat.deleghe_esitate = 1;
                }
                rap.esitoDelega = ckEsitoDelega.Checked;
                if (ckContestazioneAmm.Checked)
                {
                    stat.viol_amm_reg_com = 1;
                }
                rap.contestazioneAmm = ckContestazioneAmm.Checked;
                if (ckConvalida.Checked)
                {
                    stat.convalide = 1;
                }
                rap.convalida = ckConvalida.Checked;
                if (ckDisseqDefinitivo.Checked)
                {
                    stat.dissequestri = 1;
                }
                rap.dissequestroDef = ckDisseqDefinitivo.Checked;
                if (ckDisseqTemp.Checked)
                {
                    stat.dissequestri_temp = 1;
                }
                rap.dissequestroTemp = ckDisseqTemp.Checked;
                if (ckRimozione.Checked)
                {
                    stat.rimozione_sigilli = 1;
                }
                rap.rimozione = ckRimozione.Checked;
                if (ckRiapposizione.Checked)
                {
                    stat.riapp_sigilli = 1;
                }
                rap.riapposizione = ckRiapposizione.Checked;
                if (ckViolazioneSigilli.Checked)
                {
                    stat.violazione_sigilli = 1;
                }
                rap.violazioneSigilli = ckViolazioneSigilli.Checked;
                if (ckControlliSCIA.Checked)
                {
                    stat.controlli_scia = 1;
                }
                rap.controlliScia = ckControlliSCIA.Checked;
                rap.accertAvvenutoRip = ckAccertAvvenutoRipr.Checked;
                if (ckAccertAvvenutoRipr.Checked)
                {
                    stat.ripristino_tot_par = 1;
                }
                rap.totale = rdTotale.Checked;
                rap.parziale = rdParziale.Checked;
                rap.non_avvenuto = rdNonAvvenuto.Checked;

                rap.conProt = rdCon.Checked;
                rap.senzaProt = rdSenza.Checked;
                if (ckViolazioneBeniCult.Checked)
                {
                    stat.controlli_42_04 = 1;
                }
                rap.violazioneBeniCult = ckViolazioneBeniCult.Checked;
                if (ckContrSuoloPubblico.Checked)
                {
                    stat.contr_cant_suolo_pubb = 1;
                    stat.ponteggi = 1;

                }
                rap.contrCantSuoloPubb = ckContrSuoloPubblico.Checked;
                if (ckControlliCant.Checked)
                {
                    stat.contr_cant = 1;
                }
                rap.contr_cantiereSeq = ckControlliCant.Checked;

                if (ckControlliLavoriEdiliSenzaProt.Checked)
                {
                    stat.dpi = 1;
                }
                rap.contrEdiliDPI = ckControlliLavoriEdiliSenzaProt.Checked;
                //if (ckControlloDaEsposti.Checked || ckControlliDaSegnalazioni.Checked)
                //{
                //    stat.esposti_ricevuti = 1;
                //}
                rap.contrDaEsposti = ckControlloDaEsposti.Checked;
                rap.contrDaSegn = ckControlliDaSegnalazioni.Checked;
                rap.uote = rdUote.Checked;
                rap.uotp = rdUotp.Checked;
                if (String.IsNullOrEmpty(txt_numEspostiSegn.Text))
                {
                    rap.num_esposti = string.Empty;
                }
                else
                {
                    rap.num_esposti = txt_numEspostiSegn.Text;
                    stat.esposti_evasi = System.Convert.ToInt32(rap.num_esposti);
                }

                rap.nota = txtNote.Text;
                rap.attività_interna = CkAttivita.Checked;
                if (!String.IsNullOrEmpty(txtDataConsegna.Text))
                {
                    rap.data_consegna_intervento = System.Convert.ToDateTime(txtDataConsegna.Text);
                }
                rap.dataInserimento = DateTime.Now;
                stat.mese = MeseCorrente;
                stat.anno = System.Convert.ToInt16(AnnoCorrente);
                string txt = string.Empty;
                VerificaStatistiche(stat, out txt);
                Int32 idN = 0;
                Boolean resp = mn.InsRappUoteStatistiche(rap, stat, txt, out idN);
                if (!resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica scheda non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    HfIdScheda.Value = idN.ToString();
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#MsgStampa').text('" + "Inserimento scheda effettuato. Vuoi stampare?" + "'); $('#PopStampa').modal('show');", true);

                    Pulisci();
                }
            }
        }
        private Statistiche VerificaStatistiche(Statistiche stat, out string txt)
        {

            Manager mn = new Manager();
            DataTable dt = new DataTable();
            dt = mn.getStatisticaByMeseAnno(stat.mese, stat.anno);
            if (dt.Rows.Count > 0)
            {
                stat.mese = dt.Rows[0].ItemArray[1].ToString().Trim();
                stat.anno = System.Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                stat.relazioni += System.Convert.ToInt32(dt.Rows[0].ItemArray[3]);
                stat.ponteggi += System.Convert.ToInt32(dt.Rows[0].ItemArray[4]);
                stat.dpi += System.Convert.ToInt32(dt.Rows[0].ItemArray[5]);
                stat.esposti_ricevuti += System.Convert.ToInt32(dt.Rows[0].ItemArray[6]);
                stat.esposti_evasi += System.Convert.ToInt32(dt.Rows[0].ItemArray[7]);
                stat.ripristino_tot_par += System.Convert.ToInt32(dt.Rows[0].ItemArray[8]);
                stat.controlli_scia += System.Convert.ToInt32(dt.Rows[0].ItemArray[9]);
                //stat.contr_cant_daily += System.Convert.ToInt32(dt.Rows[0].ItemArray[10]);//???
                stat.cnr += System.Convert.ToInt32(dt.Rows[0].ItemArray[11]);
                stat.annotazioni += System.Convert.ToInt32(dt.Rows[0].ItemArray[12]);
                stat.notifiche += System.Convert.ToInt32(dt.Rows[0].ItemArray[13]);
                stat.sequestri += System.Convert.ToInt32(dt.Rows[0].ItemArray[14]);
                stat.riapp_sigilli += System.Convert.ToInt32(dt.Rows[0].ItemArray[15]);
                stat.deleghe_ricevute += System.Convert.ToInt32(dt.Rows[0].ItemArray[16]);
                stat.deleghe_esitate += System.Convert.ToInt32(dt.Rows[0].ItemArray[17]);
                stat.cnr_annotazioni += System.Convert.ToInt32(dt.Rows[0].ItemArray[18]);//??
                stat.interrogazioni += System.Convert.ToInt32(dt.Rows[0].ItemArray[19]);
                stat.denunce_uff += System.Convert.ToInt32(dt.Rows[0].ItemArray[20]);
                stat.convalide += System.Convert.ToInt32(dt.Rows[0].ItemArray[21]);
                //stat.demolizioni += System.Convert.ToInt32(dt.Rows[0].ItemArray[22]);
                stat.violazione_sigilli += System.Convert.ToInt32(dt.Rows[0].ItemArray[23]);
                stat.dissequestri += System.Convert.ToInt32(dt.Rows[0].ItemArray[24]);
                stat.dissequestri_temp += System.Convert.ToInt32(dt.Rows[0].ItemArray[25]);
                stat.rimozione_sigilli += System.Convert.ToInt32(dt.Rows[0].ItemArray[26]);
                stat.controlli_42_04 += System.Convert.ToInt32(dt.Rows[0].ItemArray[27]);
                stat.contr_cant_suolo_pubb += System.Convert.ToInt32(dt.Rows[0].ItemArray[28]);
                stat.contr_lavori_edili += System.Convert.ToInt32(dt.Rows[0].ItemArray[29]);//??
                stat.contr_cant += System.Convert.ToInt32(dt.Rows[0].ItemArray[30]);
                stat.contr_nato_da_esposti += System.Convert.ToInt32(dt.Rows[0].ItemArray[31]);
                stat.viol_amm_reg_com += System.Convert.ToInt32(dt.Rows[0].ItemArray[32]);
                txt = "upd";
            }
            else
                txt = "ins";

            return stat;
        }
        private void Pulisci()
        {
            txtPratica.Text = string.Empty;
            txtIndirizzo.Text = string.Empty;
            TxtDataIntervento.Text = string.Empty;
            txtNominativo.Text = string.Empty;
            txtNote.Text = string.Empty;
            txtDataConsegna.Text = string.Empty;
            txtPratica.Text = string.Empty;
            LPattugliaCompleta.Items.Clear();
            rdParziale.Checked = false;
            ckRiapposizione.Checked = false;
            rdTotale.Checked = false;
            rdNonAvvenuto.Checked = false;
            ckRimozione.Checked = false;
            rdUote.Checked = false;
            rdUotp.Checked = false;
            rdCon.Checked = false;
            rdSenza.Checked = false;
            CkAttivita.Checked = false;
            ckDelega.Checked = false;
            ckSegnalazione.Checked = false;
            ckNotifica.Checked = false;
            ckCdr.Checked = false;
            ckResa.Checked = false;
            ckEsposto.Checked = false;
            ckCoordinatore.Checked = false;
            ckRelazione.Checked = false;
            ckAnnotazionePG.Checked = false;
            ckEsitoDelega.Checked = false;
            ckVerbaleSeq.Checked = false;
            ckContestazioneAmm.Checked = false;
            ckConvalida.Checked = false;
            ckViolazioneBeniCult.Checked = false;
            ckDisseqDefinitivo.Checked = false;
            ckDisseqTemp.Checked = false;
            ckAccertAvvenutoRipr.Checked = false;
            ckControlliSCIA.Checked = false;
            ckContrSuoloPubblico.Checked = false;
            ckControlliCant.Checked = false;
            ckControlloDaEsposti.Checked = false;
            ckControlliDaSegnalazioni.Checked = false;
            ckControlliLavoriEdiliSenzaProt.Checked = false;
            ckIniziativa.Checked = false;
            ckCnr.Checked = false;
            ckViolazioneSigilli.Checked = false;

        }
        private Boolean Convalida()
        {
            Boolean ret = true;
            if (String.IsNullOrEmpty(LPattugliaCompleta.ToString()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserire la pattuglia." + "'); $('#errorModal').modal('show');", true);

                ret = false;

            }
            if (ckEsposto.Checked && String.IsNullOrEmpty(txt_numEspostiSegn.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Se hai selezionato esposti è necessario inserire il numero di esposti." + "'); $('#errorModal').modal('show');", true);

                ret = false;
            }
            //if (ckDisseqTemp.Checked && (ckRimozione.Checked == false && ckRiapposizione.Checked == false))
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Selezionare Rimozione o Riapposizione." + "'); $('#errorModal').modal('show');", true);

            //    ret = false;
            //}
            if (ckAccertAvvenutoRipr.Checked == true)
            {
                if (rdTotale.Checked == false && rdParziale.Checked == false && rdNonAvvenuto.Checked == false)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Selezionare Totale o Parziale o Non Avvenuto ." + "'); $('#errorModal').modal('show');", true);

                    ret = false;
                }
            }
            if (ckControlliLavoriEdiliSenzaProt.Checked && (rdCon.Checked == false && rdSenza.Checked == false))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Selezionare Con o Senza." + "'); $('#errorModal').modal('show');", true);

                ret = false;
            }
            return ret;
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

        }
        protected void RicercaQuartiere_Click(object sender, EventArgs e)
        {
            string indirizzo = string.Empty;


            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "inserire un indirizzo" + "');", true);
            //indirizzo = txtIndirizzo.Text.Trim();


            //string specie = txtSpecie.Text.Trim();

            if (!string.IsNullOrEmpty(indirizzo))
            {
                // Simula il recupero del quartiere dal database o da una logica interna.
                Manager mn = new Manager();
                DataTable quartiere = mn.getQuartiere(indirizzo);

                if (quartiere.Rows.Count > 0)
                {
                    gvPopup.DataSource = quartiere;
                    gvPopup.DataBind();

                }
                else
                {
                    //lblQuartiere.Text = "Quartiere non trovato.";
                }
            }
            else
            {
                //lblQuartiere.Text = "Inserisci un indirizzo valido.";
            }

            // Mantieni il popup aperto dopo l'interazione lato server.
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showPopup", "openPopup();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
        }
        private void CaricaDLL()
        {
            try
            {

                DataTable CaricaOperatori = mn.getListOperatore();
                DdlPattuglia.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                DdlPattuglia.DataTextField = "Nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                DdlPattuglia.Items.Insert(0, new ListItem("", "0"));
                DdlPattuglia.DataBind();
                DdlPattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                //DdlPattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
                //DataTable CaricaOperatori = mn.getListOperatore();
                ddlCapopattuglia.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                ddlCapopattuglia.DataTextField = "Nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                ddlCapopattuglia.Items.Insert(0, new ListItem("", "0"));
                ddlCapopattuglia.DataBind();
                ddlCapopattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));


            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in carica ddl file inserimento.cs ");
                    sw.Close();
                }
            }
        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "ID").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
        }
        protected void gvPopup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore dell'ID dalla CommandArgument
                string selectedValue = e.CommandArgument.ToString();

                // Imposta il valore nel TextBox
                //txtSelectedValue.Text = selectedValue;
                //DdlQuartiere.SelectedItem.Text = selectedValue;
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }

        protected void CkAttivita_CheckedChanged1(object sender, EventArgs e)
        {
            //ToggleDivControls(divTesta, CkAttivita.Checked);
            ToggleDivControls(divDettagli, CkAttivita.Checked);

        }
        private void ToggleDivControls(Control container, bool isEnabled)
        {
            foreach (Control control in container.Controls)
            {
                // Verifica se il controllo è un WebControl (TextBox, Button, DropDownList, ecc.)
                if (control is WebControl webControl)
                {
                    webControl.Enabled = !isEnabled;
                }

                // Se il controllo ha figli, chiama ricorsivamente la funzione
                if (control.HasControls())
                {
                    ToggleDivControls(control, !isEnabled);
                }
            }
        }



        protected void Aggiungi_Click(object sender, EventArgs e)
        {

            // Crea un nuovo ListViewDataItem con un valore di esempio
            ListViewDataItem itemToAdd = new ListViewDataItem(0, 0);
            itemToAdd.DataItem = DdlPattuglia.SelectedItem.Text;

            // Verifica se l'elemento è già presente nella ListView
            bool itemExists = false;

            foreach (var item in LPattugliaCompleta.Items)
            {
                if (item.ToString() == itemToAdd.DataItem.ToString())
                {
                    itemExists = true;
                    break;
                }
            }

            // Aggiungi l'elemento solo se non esiste già
            if (!itemExists)
            {
                LPattugliaCompleta.Items.Add(DdlPattuglia.SelectedItem.Text);
            }

        }

        protected void btElimina_Click(object sender, EventArgs e)
        {
            if (LPattugliaCompleta.SelectedItem != null)
                LPattugliaCompleta.Items.Remove(LPattugliaCompleta.SelectedItem);
        }

        protected void btStampa_Click(object sender, EventArgs e)
        {
            // Percorso del file RDLC
            string reportPath = @"C:\Uotep\Report.rdlc";

            // Percorso del file PDF da creare
            string pdfPath = @"C:\UotepOutput\Report.pdf";
            try
            {
                // Configura il ReportViewer
                LocalReport report = new LocalReport();
                report.ReportPath = reportPath;

                // Aggiungi il DataSource (sostituisci con i tuoi dati)
                report.DataSources.Add(new ReportDataSource("DataSet1", GetSampleData(txtPratica.Text, null, null, false)));

                // Esporta il report in PDF
                ExportToPdf(report, pdfPath);

                Console.WriteLine($"PDF generato con successo: {pdfPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }
        // Metodo per creare dati di esempio (da sostituire con i dati reali)
        static DataTable GetSampleData(string nrPratica, string pattuglia, string dat, Boolean at)
        {
            Manager mn = new Manager();
            //DataTable schede =
            DataTable table = mn.GetSchedeBy(nrPratica, null, null, at, 0);
            if (table.Rows.Count > 0)
            {
                //GVRicecaScheda.DataSource = table;
                //GVRicecaScheda.DataBind();



                table.Columns.Add("Pratica", typeof(string));
                table.Columns.Add("Data", typeof(string));
                table.Columns.Add("Nominativo", typeof(int));

                table.Rows.Add(table.Rows[0].ItemArray[0], table.Rows[0].ItemArray[1], 30);
                //table.Rows.Add("Mario", "Rossi", 30);
                //table.Rows.Add("Luigi", "Verdi", 25);
                //table.Rows.Add("Anna", "Bianchi", 28);
            }
            return table;

        }
        // Metodo per esportare il report in PDF
        static void ExportToPdf(LocalReport report, string filePath)
        {
            string mimeType, encoding, fileNameExtension;
            Warning[] warnings;
            string[] streamIds;

            // Renderizza il report in formato PDF
            byte[] bytes = report.Render(
                "PDF", null, out mimeType, out encoding, out fileNameExtension, out streamIds, out warnings);

            // Salva il file PDF
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        protected void btPopStampa_Click(object sender, EventArgs e)
        {
            int id = System.Convert.ToInt32(HfIdScheda.Value);
            DataTable schede = mn.GetSchedeBy(null, null, null, CkAttivita.Checked, id);

            Routine stampa = new Routine();
            stampa.CreaPdf(schede);
        }
    }

}