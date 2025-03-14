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
    public partial class RicercaScheda : Page
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
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            ProtocolloLiteral.Text = decodedText;
            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);

                SetControlsEnabled(divDettagli, false);
                SetControlsEnabled(divTesta, false);
                //SetControlsVisible(divDDLPattuglia, false);
                //SetControlsVisible(divPattuglia, true);
                if (ruolo == "admin")
                    btModificaScheda.Enabled = true;
                else
                    btModificaScheda.Enabled = false;
            }

        }
        /// <summary>
        /// abilita/disabilita gli oggetti del div
        /// </summary>
        /// <param name="container"></param>
        /// <param name="isEnabled"></param>
        protected void SetControlsEnabled(Control container, bool isEnabled)
        {
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is WebControl webCtrl)
                {
                    webCtrl.Enabled = isEnabled;
                }

                // Se il controllo ha figli, applica la ricorsione
                if (ctrl.HasControls())
                {
                    SetControlsEnabled(ctrl, isEnabled);
                }
            }
        }
        /// <summary>
        /// visualizzo/non visualizzo gli oggetti del div
        /// </summary>
        /// <param name="container"></param>
        /// <param name="isEnabled"></param>
        protected void SetControlsVisible(Control container, bool isEnabled)
        {
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is WebControl webCtrl)
                {
                    webCtrl.Visible = isEnabled;
                }

                // Se il controllo ha figli, applica la ricorsione
                if (ctrl.HasControls())
                {
                    SetControlsVisible(ctrl, isEnabled);
                }
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

        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "rapp_numero_pratica").ToString();

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
                //string selectedValue = e.CommandArgument.ToString();


                string[] args = e.CommandArgument.ToString().Split(';');
                string numeroP = args[0];
                string nominativo = args[1];
                string pattuglia = args[2];
                string id_scheda = args[3];
                HfIdScheda.Value = System.Convert.ToInt32(id_scheda).ToString();
                // Imposta il valore nel TextBox
                //txtSelectedValue.Text = selectedValue;
                txtPratica.Text = numeroP;
                txtNominativo.Text = nominativo;
                //txtPattuglia.Text = pattuglia;
                string[] columns = pattuglia.Split('/');

                for (int i = 0; i < columns.Length; i++)
                {
                    LPattugliaCompleta.Items.Add(columns[i]); // Colonne successive
                }
                CaricaDLL();
                Manager mn = new Manager();
                //                DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);

                DataTable scheda = mn.GetSchedaById(id_scheda);
                if (scheda.Rows.Count > 0)
                {
                    FillScheda(scheda);

                }
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }

        protected void FillScheda(DataTable rap)
        {
            txtNominativo.Text = rap.Rows[0].ItemArray[3].ToString();
            txtIndirizzo.Text = rap.Rows[0].ItemArray[4].ToString();
            if (!string.IsNullOrEmpty(rap.Rows[0].ItemArray[2].ToString()))
            {
                DateTime dataIntervento = System.Convert.ToDateTime(rap.Rows[0].ItemArray[2].ToString()); // Recupera la data dal DataTable
                TxtDataIntervento.Text = dataIntervento.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
                                                                                //  TxtDataIntervento.Text = rap.Rows[0].ItemArray[2].ToString();
            }

            ckDelega.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[6]);
            ckResa.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[7]);
            ckSegnalazione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[8]);
            ckEsposto.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[9]);

            if (String.IsNullOrEmpty(rap.Rows[0].ItemArray[10].ToString()))
            {
                txt_numEspostiSegn.Text = "0";
            }
            else
            {
                txt_numEspostiSegn.Text = rap.Rows[0].ItemArray[10].ToString();
            }
            ckNotifica.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[11]);
            ckIniziativa.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[12]);
            ckCdr.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[13]);
            ckCoordinatore.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[14]);
            ckRelazione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[15]);
            ckCnr.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[16]);
            ckAnnotazionePG.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[17]);
            ckVerbaleSeq.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[18]);
            ckEsitoDelega.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[19]);
            ckContestazioneAmm.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[20]);
            ckConvalida.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[21]);
            ckDisseqDefinitivo.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[22]);
            ckDisseqTemp.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[23]);
            rdRimozione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[24]);
            rdRiapposizione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[25]);
            ckViolazioneSigilli.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[26]);
            ckControlliSCIA.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[27]);
            ckAccertAvvenutoRipr.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[28]);
            rdTotale.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[29]);
            rdParziale.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[30]);
            ckViolazioneBeniCult.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[31]);
            ckContrSuoloPubblico.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[32]);
            ckControlliLavoriEdiliSenzaProt.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[33]);
            ckControlloDaEsposti.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[35]);
            ckControlliCant.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[34]);
            ckControlliDaSegnalazioni.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[36]);
            CkAttivita.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[37]);

            txtNote.Text = rap.Rows[0].ItemArray[38].ToString();

            if (!string.IsNullOrEmpty(rap.Rows[0].ItemArray[39].ToString()))
            {
                DateTime dataConsegna = System.Convert.ToDateTime(rap.Rows[0].ItemArray[39].ToString()); // Recupera la data dal DataTable
                txtDataConsegna.Text = dataConsegna.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox


                // txtDataConsegna.Text = rap.Rows[0].ItemArray[39].ToString();
            }
            //txtCapoPattuglia.Text = rap.Rows[0].ItemArray[39].ToString();
            if (!String.IsNullOrEmpty(rap.Rows[0].ItemArray[40].ToString()))
            {
                ddlCapopattuglia.SelectedItem.Text = rap.Rows[0].ItemArray[40].ToString();
            }

            rdUote.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[41]);
            rdUotp.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[42]);
            rdCon.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[44]);
            rdSenza.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[45]);
        }

        protected void btA_Click(object sender, EventArgs e)
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

        protected void btRicScheda_Click(object sender, EventArgs e)
        {

            string numPratica = txtModPratica.Text;


            if (!string.IsNullOrEmpty(numPratica))
            {

                Manager mn = new Manager();
                DataTable schede = mn.GetSchedeBy(numPratica, null, null, ckModAttivitòInterna.Checked, 0);

                if (schede.Rows.Count > 0)
                {
                    GVRicecaScheda.DataSource = schede;
                    GVRicecaScheda.DataBind();

                }

            }
            if (!string.IsNullOrEmpty(txtModPattuglia.Text))
            {

                Manager mn = new Manager();
                DataTable schede = mn.GetSchedeBy(null, txtModPattuglia.Text, null, ckModAttivitòInterna.Checked, 0);

                if (schede.Rows.Count > 0)
                {
                    GVRicecaScheda.DataSource = schede;
                    GVRicecaScheda.DataBind();

                }

            }
            if (!string.IsNullOrEmpty(txtModDataIntervento.Text))
            {

                Manager mn = new Manager();
                DataTable schede = mn.GetSchedeBy(null, null, txtModDataIntervento.Text, ckModAttivitòInterna.Checked, 0);

                if (schede.Rows.Count > 0)
                {
                    GVRicecaScheda.DataSource = schede;
                    GVRicecaScheda.DataBind();

                }

            }
            if (ckModAttivitòInterna.Checked)
            {
                //if (String.IsNullOrEmpty(txtModPattuglia.Text))
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "pattuglia " + "'); $('#errorModal').modal('show');", true);

                //}
                Manager mn = new Manager();
                DataTable schede = mn.GetSchedeBy(null, txtModPattuglia.Text, null, ckModAttivitòInterna.Checked, 0);

                if (schede.Rows.Count > 0)
                {
                    GVRicecaScheda.DataSource = schede;
                    GVRicecaScheda.DataBind();

                }

            }
            // Mantieni il popup aperto dopo l'interazione lato server.
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
        }


        protected void btModificaScheda_Click(object sender, EventArgs e)
        {
            SetControlsEnabled(divTesta, true);
            SetControlsEnabled(divDettagli, true);
            //SetControlsVisible(divDDLPattuglia, true);
            //SetControlsVisible(divPattuglia, false);
            CkAttivita.Enabled = true;
            rdUote.Enabled = true;
            rdUotp.Enabled = true;
            //CaricaDLL();
        }
        private Boolean Convalida()
        {
            Boolean ret = true;
            if (String.IsNullOrEmpty(LPattugliaCompleta.ToString()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "inserire la pattuglia." + "'); $('#errorModal').modal('show');", true);

                ret = false;
                return ret;
            }

            return ret;
        }
        protected void btSalva_Click(object sender, EventArgs e)
        {
            Boolean continua = Convalida();

            if (continua)
            {
                RappUote rap = new RappUote();
                for (int i = 0; i < LPattugliaCompleta.Items.Count; i++)
                {
                    rap.pattuglia += LPattugliaCompleta.Items[i].Value + " ";
                }

                rap.nominativo = txtNominativo.Text.ToUpper();
                rap.indirizzo = txtIndirizzo.Text;
                if (!string.IsNullOrEmpty(TxtDataIntervento.Text))
                {
                    rap.data = System.Convert.ToDateTime(TxtDataIntervento.Text);
                }


                //oraapp = dtOra.Value);
                //rap.ora = dtOra.Value;
                rap.pratica = txtPratica.Text;
                rap.delegaAG = ckDelega.Checked;
                rap.resa = ckResa.Checked;
                rap.segnalazione = ckSegnalazione.Checked;
                rap.esposti = ckEsposto.Checked;
                rap.notifica = ckNotifica.Checked;
                rap.iniziativa = ckIniziativa.Checked;
                rap.cdr = ckCdr.Checked;
                rap.coordinatore = ckCoordinatore.Checked;
                rap.relazione = ckRelazione.Checked;
                rap.cnr = ckCnr.Checked;
                rap.annotazionePG = ckAnnotazionePG.Checked;
                rap.verbaleSeq = ckVerbaleSeq.Checked;
                rap.esitoDelega = ckEsitoDelega.Checked;
                rap.contestazioneAmm = ckContestazioneAmm.Checked;
                rap.convalida = ckConvalida.Checked;
                rap.dissequestroDef = ckDisseqDefinitivo.Checked;
                rap.dissequestroTemp = ckDisseqTemp.Checked;
                rap.rimozione = rdRimozione.Checked;
                rap.riapposizione = rdRiapposizione.Checked;
                rap.violazioneSigilli = ckViolazioneSigilli.Checked;
                rap.controlliScia = ckControlliSCIA.Checked;
                rap.accertAvvenutoRip = ckAccertAvvenutoRipr.Checked;
                rap.totale = rdTotale.Checked;
                rap.parziale = rdParziale.Checked;
                rap.conProt = rdCon.Checked;
                rap.senzaProt = rdSenza.Checked;
                rap.violazioneBeniCult = ckViolazioneBeniCult.Checked;
                rap.contrCantSuoloPubb = ckContrSuoloPubblico.Checked;
                rap.contrEdiliDPI = ckControlliLavoriEdiliSenzaProt.Checked;
                rap.contrDaEsposti = ckControlloDaEsposti.Checked;
                rap.contrDaSegn = ckControlliDaSegnalazioni.Checked;
                rap.contr_cantiereSeq = ckControlliCant.Checked;
                rap.matricola = Vuser;
                if (String.IsNullOrEmpty(txt_numEspostiSegn.Text))
                {
                    rap.num_esposti = string.Empty;
                }
                else
                {
                    rap.num_esposti = txt_numEspostiSegn.Text;
                }

                rap.nota = txtNote.Text;
                //rap.attività_interna = CkAttivita.Checked;
                if (!String.IsNullOrEmpty(txtDataConsegna.Text))
                {
                    rap.data_consegna_intervento = System.Convert.ToDateTime(txtDataConsegna.Text);
                }
                if (!string.IsNullOrEmpty(ddlCapopattuglia.SelectedItem.Text))
                {
                    rap.capopattuglia = ddlCapopattuglia.SelectedItem.Text;
                }
                Manager mn = new Manager();

                Boolean resp = mn.UpdScheda(rap);
                if (!resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Modifica della scheda intervento non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                    Pulisci();
            }
        }
        protected void btElimina_Click(object sender, EventArgs e)
        {
            if (LPattugliaCompleta.SelectedItem != null)
                LPattugliaCompleta.Items.Remove(LPattugliaCompleta.SelectedItem);
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
        private void CaricaDLL()
        {
            try
            {

                DataTable CaricaOperatori = mn.getListOperatore();
                DdlPattuglia.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                DdlPattuglia.DataTextField = "Nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                DdlPattuglia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                DdlPattuglia.DataBind();
                //DdlPattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                ddlCapopattuglia.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                ddlCapopattuglia.DataTextField = "Nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                ddlCapopattuglia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                ddlCapopattuglia.DataBind();


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
            rdRiapposizione.Checked = false;
            rdCon.Checked = false;
            rdSenza.Checked = false;
            rdTotale.Checked = false;
            rdRimozione.Checked = false;
            rdUote.Checked = false;
            rdUotp.Checked = false;
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
        //stampa

        public void CreaPdf(DataTable schede)
        {
            float boxSize = 10;
            float boxVerticalOffset = 4f;
            float startX_270 = 270;
;
            float startX_70 = 70;
            float startX_400 = 400;
            float startX_350 = 350;
            float startX_450 = 450;
            float startY_430 = 430;

            float lineHeight = 15f;
            float lineHeight1 = 30f;
            float startY = 630;

            using (MemoryStream stream = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(stream))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        using (Document document = new Document(pdf))
                        {
                            // --- Creazione del Contenuto del Documento ---

                            // Titolo
                            DateTime dataIntervento = System.Convert.ToDateTime(schede.Rows[0].ItemArray[2].ToString());
                            string dataFormattata = dataIntervento.ToString("dd/MM/yyyy");

                            document.Add(new Paragraph($"Scheda Intervento del: {dataFormattata}")
                                .SetFixedPosition(70, 800, 400)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(14));

                            // Prima riga: Numero Pratica, Nominativo
                            document.Add(new Paragraph($"Numero Pratica: {schede.Rows[0].ItemArray[1]}").SetFixedPosition(70, 780, 200));
                            document.Add(new Paragraph($"Nominativo: {schede.Rows[0].ItemArray[3]}").SetFixedPosition(250, 780, 500));

                            // Seconda riga: Indirizzo, Data Consegna
                            document.Add(new Paragraph($"Indirizzo: {schede.Rows[0].ItemArray[4]}").SetFixedPosition(70, 760, 800));

                            DateTime dataConsegna = System.Convert.ToDateTime(schede.Rows[0].ItemArray[39].ToString());
                            string dataFormattataConsegna = dataConsegna.ToString("dd/MM/yyyy");

                            document.Add(new Paragraph($"Data Consegna: {dataFormattataConsegna}").SetFixedPosition(70, 740, 800));

                            // Terza riga: Capo pattuglia, pattuglia
                            document.Add(new Paragraph($"Capo Pattuglia: {schede.Rows[0].ItemArray[40]}").SetFixedPosition(70, 720, 200));
                            document.Add(new Paragraph($"Pattuglia: {schede.Rows[0].ItemArray[5]}").SetFixedPosition(70, 700, 600));
                            // Note
                            document.Add(new Paragraph($"Note: {schede.Rows[0].ItemArray[38]}").SetFixedPosition(70, 680, 450));
                            // FONTE INTERVENTO
                            document.Add(new Paragraph("FONTE INTERVENTO").SetFixedPosition(70, 650, 500).SetTextAlignment(TextAlignment.CENTER));
                            //riga interruzione sezione
                            float x = 65;
                            float y = 645;
                            float width = 490;
                           
                            PdfCanvas canvas = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x, y) // Inizia la linea nel punto (x, y)
                                  .LineTo(x + width, y) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // Delega AG
                            bool delegaAG = Convert.ToBoolean(schede.Rows[0].ItemArray[6]);
                            string delegaAGString = delegaAG ? "X" : "";

                            // --- Posizione di riferimento INIZIALE per la X e il Riquadro (lato SINISTRO) ---
                            float startX_70_DelegaAG = 70; // Posizione X iniziale SPECIFICA per "Delega AG"
                            float startY_DelegaAG = startY; // Use the dynamic startY

                            if (delegaAGString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_Delega = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70_DelegaAG;
                                float yPosBox = startY_DelegaAG - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_Delega.SetStrokeColor(ColorConstants.BLACK);
                                canvas_Delega.SetLineWidth(0.8f);
                                canvas_Delega.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_DelegaAG;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_Delega.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Delega AG:");
                                descriptionParagraph.SetFixedPosition(startX_70_DelegaAG + boxSize + 5, startY_DelegaAG - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Delega AG:");
                                descriptionParagraph.SetFixedPosition(startX_70_DelegaAG, startY_DelegaAG, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Resa
                            bool? resaNullable = schede.Rows[0].ItemArray[7] as bool?;
                            string resaString = resaNullable.HasValue && resaNullable.Value ? "X" : "";

                            // --- Posizione di riferimento per "Resa" ---
                            float startX_70_Resa = 70; // Use startX_70 for single column
                            float startY_Resa = startY; // Use the dynamic startY


                            if (resaString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_Resa = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70_Resa;
                                float yPosBox = startY_Resa - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_Resa.SetStrokeColor(ColorConstants.BLACK);
                                canvas_Resa.SetLineWidth(0.8f);
                                canvas_Resa.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_Resa;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_Resa.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Resa:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Resa:");
                                descriptionParagraph.SetFixedPosition(startX_70_Resa + boxSize + 5, startY_Resa - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Resa:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Resa:");
                                descriptionParagraph.SetFixedPosition(startX_70_Resa, startY_Resa, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Segnalazione
                            bool? segnalazioneNullable = schede.Rows[0].ItemArray[8] as bool?;
                            string segnalazioneString = segnalazioneNullable.HasValue && segnalazioneNullable.Value ? "X" : "";

                            // --- Posizione di riferimento per "Segnalazione" ---
                            float startX_70_Segnalazione = 70; // Use startX_70 for single column
                            float startY_Segnalazione = startY; // Use the dynamic startY


                            if (segnalazioneString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_segnalazione = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70_Segnalazione;
                                float yPosBox = startY_Segnalazione - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_segnalazione.SetStrokeColor(ColorConstants.BLACK);
                                canvas_segnalazione.SetLineWidth(0.8f);
                                canvas_segnalazione.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_Segnalazione;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_segnalazione.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Segnalazione:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Segnalazione:");
                                descriptionParagraph.SetFixedPosition(startX_70_Segnalazione + boxSize + 5, startY_Segnalazione - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Segnalazione:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Segnalazione:");
                                descriptionParagraph.SetFixedPosition(startX_70_Segnalazione, startY_Segnalazione, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Esposto
                            bool? espostoNullable = schede.Rows[0].ItemArray[9] as bool?;
                            string espostoString = espostoNullable.HasValue && espostoNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "Esposto" ---
                            float startX_70_Esposto = 70; // Use startX_70 for single column
                            float startY_Esposto = startY; // Use the dynamic startY

                            if (espostoString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_espostoString = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70_Esposto;
                                float yPosBox = startY_Esposto - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_espostoString.SetStrokeColor(ColorConstants.BLACK);
                                canvas_espostoString.SetLineWidth(0.8f);
                                canvas_espostoString.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_Esposto;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_espostoString.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Esposto:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Esposto:");
                                descriptionParagraph.SetFixedPosition(startX_70_Esposto + boxSize + 5, startY_Esposto - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Esposto:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Esposto:");
                                descriptionParagraph.SetFixedPosition(startX_70_Esposto, startY_Esposto, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Num. Esposto
                            // --- Posizione di riferimento per "Num. Esposto" ---
                            
                            float startY_NumEsposto = startY_430; //
                            

                            document.Add(new Paragraph($"Num. Esposto: {schede.Rows[0].ItemArray[10]}").SetFixedPosition(startX_270, startY_Esposto, 200));
                            
                            startY -= lineHeight; // Move to the next line

                            // Notifica
                            bool? notificaNullable = schede.Rows[0].ItemArray[11] as bool?;
                            string notificaString = notificaNullable.HasValue && notificaNullable.Value ? "X" : "";

                            // --- Posizione di riferimento per "Notifica" ---
                            float startY_Notifica = startY; // Use the dynamic startY

                            if (notificaString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_Notifica = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70;
                                float yPosBox = startY_Notifica - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_Notifica.SetStrokeColor(ColorConstants.BLACK);
                                canvas_Notifica.SetLineWidth(0.8f);
                                canvas_Notifica.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_Notifica;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_Notifica.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Notifica:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Notifica:");
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_Notifica - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Notifica:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Notifica:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Notifica, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line


                            // Iniziativa
                            bool? iniziativaNullable = schede.Rows[0].ItemArray[12] as bool?;
                            string iniziativaString = iniziativaNullable.HasValue && iniziativaNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "Iniziativa" ---
                            float startX_70_Iniziativa = 70; // Use startX_70 for single column
                            float startY_Iniziativa = startY; // Use the dynamic startY
                            if (iniziativaString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_Iniziativa = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70_Iniziativa;
                                float yPosBox = startY_Iniziativa - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_Iniziativa.SetStrokeColor(ColorConstants.BLACK);
                                canvas_Iniziativa.SetLineWidth(0.8f);
                                canvas_Iniziativa.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_Iniziativa;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_Iniziativa.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Iniziativa:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Iniziativa:");
                                descriptionParagraph.SetFixedPosition(startX_70_Iniziativa + boxSize + 5, startY_Iniziativa - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Iniziativa:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Iniziativa:");
                                descriptionParagraph.SetFixedPosition(startX_70_Iniziativa, startY_Iniziativa, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // CDR
                            bool? cdrNullable = schede.Rows[0].ItemArray[13] as bool?;
                            string cdrString = cdrNullable.HasValue && cdrNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "CDR" ---
                            float startX_70_CDR = 70; // Use startX_70 for single column
                            float startY_CDR = startY; // Use the dynamic startY
                            if (cdrString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_CDR = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70_CDR;
                                float yPosBox = startY_CDR - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_CDR.SetStrokeColor(ColorConstants.BLACK);
                                canvas_CDR.SetLineWidth(0.8f);
                                canvas_CDR.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_CDR;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_CDR.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "CDR:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("CDR:");
                                descriptionParagraph.SetFixedPosition(startX_70_CDR + boxSize + 5, startY_CDR - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "CDR:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("CDR:");
                                descriptionParagraph.SetFixedPosition(startX_70_CDR, startY_CDR, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line 470

                            // Coordinatore di turno
                            bool? coordinatorediturnoNullable = schede.Rows[0].ItemArray[14] as bool?;
                            string coordinatorediturnoString = coordinatorediturnoNullable.HasValue && coordinatorediturnoNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "Coordinatore di turno" ---
                           // float startX_70_Coord = 70; // Use startX_70 for single column
                            float startY_Coord = startY; // Use the dynamic startY 450
                            if (coordinatorediturnoString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_Coordinatore = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70;
                                float yPosBox = startY_Coord - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_Coordinatore.SetStrokeColor(ColorConstants.BLACK);
                                canvas_Coordinatore.SetLineWidth(0.8f);
                                canvas_Coordinatore.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_Coord;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_Coordinatore.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Coordinatore:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Coordinatore di turno:");
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_Coord - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Coordinatore:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Coordinatore di turno:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Coord, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            //riga interruzione sezione
                            float x1 = 65;
                            float y1 = startY;
                            float width1 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas1 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x1, y1) // Inizia la linea nel punto (x, y)
                                  .LineTo(x1 + width1, y1) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // ATTI REDATTI
                            document.Add(new Paragraph("ATTI REDATTI").SetFixedPosition(70, startY, 500).SetTextAlignment(TextAlignment.CENTER));
                           
                            startY -= lineHeight; // Move to the next line
                            // Relazione
                            bool? relazioneNullable = schede.Rows[0].ItemArray[15] as bool?;
                            string relazioneString = relazioneNullable.HasValue && relazioneNullable.Value ? "X" : "";
                            //float startX_70_Relazione = 70; // Use startX_70 for single column
                            float startY_Relazione = startY; // Use the dynamic startY 


                            if (relazioneString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_Relazione = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Usa startX_DelegaAG per questo campo
                                float yPosBox = startY_Relazione - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_Relazione.SetStrokeColor(ColorConstants.BLACK);
                                canvas_Relazione.SetLineWidth(0.8f);
                                canvas_Relazione.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_Relazione;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_Relazione.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Relazione:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_Relazione - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "relazione:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Relazione:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Relazione, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // CNR
                            bool? cnrNullable = schede.Rows[0].ItemArray[16] as bool?;
                            string cnrString = cnrNullable.HasValue && cnrNullable.Value ? "X" : "";
                            float startY_CNR = startY; // Use the dynamic startY
                            if (cnrString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_CNR = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Usa startX_DelegaAG per questo campo
                                float yPosBox = startY_CNR - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_CNR.SetStrokeColor(ColorConstants.BLACK);
                                canvas_CNR.SetLineWidth(0.8f);
                                canvas_CNR.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_CNR;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_CNR.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("CNR:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_CNR - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("CNR:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_CNR, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Annotazione PG
                            bool? annotazionepgNullable = schede.Rows[0].ItemArray[17] as bool?;
                            string annotazionepgString = annotazionepgNullable.HasValue && annotazionepgNullable.Value ? "X" : "";
                            float startY_AnnotazionePG = startY; // Use the dynamic startY
                            if (annotazionepgString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_AnnotazionePG = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Usa startY_AnnotazionePG per questo campo
                                float yPosBox = startY_AnnotazionePG - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_AnnotazionePG.SetStrokeColor(ColorConstants.BLACK);
                                canvas_AnnotazionePG.SetLineWidth(0.8f);
                                canvas_AnnotazionePG.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_AnnotazionePG;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_AnnotazionePG.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Annotazione PG:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_AnnotazionePG - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Annotazione PG:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_AnnotazionePG, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Verbale Sequestro
                            bool? verbalesequestroNullable = schede.Rows[0].ItemArray[18] as bool?;
                            string verbalesequestroString = verbalesequestroNullable.HasValue && verbalesequestroNullable.Value ? "X" : "";
                            float startY_VerbaleSequestro = startY; // Use the dynamic startY
                            if (verbalesequestroString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_VerbaleSequestro = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Usa startX_DelegaAG per questo campo
                                float yPosBox = startY_VerbaleSequestro - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_VerbaleSequestro.SetStrokeColor(ColorConstants.BLACK);
                                canvas_VerbaleSequestro.SetLineWidth(0.8f);
                                canvas_VerbaleSequestro.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_VerbaleSequestro;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_VerbaleSequestro.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Verbale Sequestro:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_VerbaleSequestro - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "esito delega:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Verbale Sequestro:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_VerbaleSequestro, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Esito Delega
                            bool? esitodelegaNullable = schede.Rows[0].ItemArray[19] as bool?;
                            string esitodelegaString = esitodelegaNullable.HasValue && esitodelegaNullable.Value ? "X" : "";
                            float startY_EsitoDelega = startY; // Use the dynamic startY
                            if (esitodelegaString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_EsitoDelega = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Usa startY_EsitoDelega per questo campo
                                float yPosBox = startY_EsitoDelega - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_EsitoDelega.SetStrokeColor(ColorConstants.BLACK);
                                canvas_EsitoDelega.SetLineWidth(0.8f);
                                canvas_EsitoDelega.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_EsitoDelega;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_EsitoDelega.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Esito Delega:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_EsitoDelega - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "esito Delega :", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Esito Delega:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_EsitoDelega, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Contestazione Amministrativa
                            bool? contestazioneamministrativaNullable = schede.Rows[0].ItemArray[20] as bool?;
                            string contestazioneamministrativaString = contestazioneamministrativaNullable.HasValue && contestazioneamministrativaNullable.Value ? "X" : "";
                            float startY_ContestazioneAmministrativa = startY; // Use the dynamic startY
                            if (contestazioneamministrativaString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas canvas_ContestazioneAmministrativa = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Usa startX_DelegaAG per questo campo
                                float yPosBox = startY_ContestazioneAmministrativa - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                canvas_ContestazioneAmministrativa.SetStrokeColor(ColorConstants.BLACK);
                                canvas_ContestazioneAmministrativa.SetLineWidth(0.8f);
                                canvas_ContestazioneAmministrativa.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_ContestazioneAmministrativa;

                                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                                canvas_ContestazioneAmministrativa.BeginText()
                                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                         .SetColor(ColorConstants.BLACK, true)
                                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                         .ShowText("X")
                                         .EndText();

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Contestazione Amministrativa:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_ContestazioneAmministrativa - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Contestazione Amministrativa:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_ContestazioneAmministrativa, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            //riga interruzione sezione
                            float x2 = 65;
                            float y2 = startY;
                            float width2 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas2 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x2, y2) // Inizia la linea nel punto (x, y)
                                  .LineTo(x2 + width2, y2) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // PROVVEDIMENTI ADOTTATI E ATTIVITA' SVOLTE
                            document.Add(new Paragraph("PROVVEDIMENTI ADOTTATI E ATTIVITA' SVOLTE").SetFixedPosition(70, startY, 500).SetTextAlignment(TextAlignment.CENTER));
                            startY -= lineHeight; // Move to the next line
                            // Convalida
                            //bool? convalidaNullable = schede.Rows[0].ItemArray[21] as bool?;
                            //string convalidaString = convalidaNullable.HasValue && convalidaNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Convalida: {convalidaString}").SetFixedPosition(70, 470, 200));
                            bool? convalidaNullable = schede.Rows[0].ItemArray[21] as bool?;
                            string convalidaString = convalidaNullable.HasValue && convalidaNullable.Value ? "X" : "";
                            float startY_Convalida = startY; // Use the dynamic startY
                            if (convalidaString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas pdfCanvas_Convalida = new PdfCanvas(pdfDocument.GetFirstPage());

                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Usa startX_Convalida per questo campo
                                float yPosBox = startY_Convalida - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                pdfCanvas_Convalida.SetStrokeColor(ColorConstants.BLACK);
                                pdfCanvas_Convalida.SetLineWidth(0.8f);
                                pdfCanvas_Convalida.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_Convalida;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                pdfCanvas_Convalida.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 //.MoveTextCursorToPosition(xPosText - 2.5f, yPosText + 2.5f) // Sposta il cursore al centro (aggiustamenti)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();

                                // --- Paragrafo per la descrizione "Convalida:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Convalida:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_Convalida + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_Convalida - 5, 200); // Usa startX_Convalida e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                // --- Solo la descrizione "Convalida:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Convalida:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Convalida, 200); // Usa startX_Convalida
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Dissequestro Definitivo
                            bool? dissequestrodefinitivoNullable = schede.Rows[0].ItemArray[22] as bool?;
                            string dissequestrodefinitivoString = dissequestrodefinitivoNullable.HasValue && dissequestrodefinitivoNullable.Value ? "X" : "";
                            float startY_DissequestroDefinitivo = startY; // Use the dynamic startY
                            if (dissequestrodefinitivoString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas pdfCanvasDF = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_DissequestroDefinitivo - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                pdfCanvasDF.SetStrokeColor(ColorConstants.BLACK);
                                pdfCanvasDF.SetLineWidth(0.8f);
                                pdfCanvasDF.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_DissequestroDefinitivo;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                pdfCanvasDF.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 //.MoveTextCursorToPosition(xPosText - 2.5f, yPosText + 2.5f) // Sposta il cursore al centro (aggiustamenti)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Definitivo:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_DissequestroDefinitivo - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Definitivo:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_DissequestroDefinitivo, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                          // Violazione sigilli
                            bool? violazionesigilliNullable = schede.Rows[0].ItemArray[26] as bool?;
                            string violazionesigilliString = violazionesigilliNullable.HasValue && violazionesigilliNullable.Value ? "X" : "";
                            float startY_violazionesigilli = startY; // Use the dynamic startY
                            if (violazionesigilliString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Violazionesigilli = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_violazionesigilli - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Violazionesigilli.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Violazionesigilli.SetLineWidth(0.8f);
                                Canvas_Violazionesigilli.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_violazionesigilli;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Violazionesigilli.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 //.MoveTextCursorToPosition(xPosText - 2.5f, yPosText + 2.5f) // Sposta il cursore al centro (aggiustamenti)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Violazione sigilli:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_violazionesigilli - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Violazione sigilli:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_violazionesigilli, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Dissequestro Temporaneo
                            bool? dissequestrotemporaneoNullable = schede.Rows[0].ItemArray[23] as bool?;
                            string dissequestrotemporaneoString = dissequestrotemporaneoNullable.HasValue && dissequestrotemporaneoNullable.Value ? "X" : "";
                            float startY_dissequestrotemporaneo = startY; // Use the dynamic startY
                            if (dissequestrotemporaneoString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_DissequestroTemporaneo = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_dissequestrotemporaneo - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_DissequestroTemporaneo.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_DissequestroTemporaneo.SetLineWidth(0.8f);
                                Canvas_DissequestroTemporaneo.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_dissequestrotemporaneo;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_DissequestroTemporaneo.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 //.MoveTextCursorToPosition(xPosText - 2.5f, yPosText + 2.5f) // Sposta il cursore al centro (aggiustamenti)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Temporaneo:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_dissequestrotemporaneo - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Temporaneo:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_dissequestrotemporaneo, 200);
                                document.Add(descriptionParagraph);
                            }

                            // Rimozione
                            bool? rimozioneNullable = schede.Rows[0].ItemArray[24] as bool?;
                            string rimozioneString = rimozioneNullable.HasValue && rimozioneNullable.Value ? "X" : "";
                           // float startY_Rimozione = startX_270; // Use the dynamic startY
                            if (rimozioneString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Rimozione = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_270; // Riquadro inizia a startX
                                float yPosBox = startY_dissequestrotemporaneo - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Rimozione.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Rimozione.SetLineWidth(0.8f);
                                Canvas_Rimozione.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_dissequestrotemporaneo;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Rimozione.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 //.MoveTextCursorToPosition(xPosText - 2.5f, yPosText + 2.5f) // Sposta il cursore al centro (aggiustamenti)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Rimozione:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_270 + boxSize + 5, startY_dissequestrotemporaneo - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Rimozione:");
                                descriptionParagraph.SetFixedPosition(startX_270, startY_dissequestrotemporaneo, 200);
                                document.Add(descriptionParagraph);
                            }
                            // Riapposizione
                            bool? riapposizioneNullable = schede.Rows[0].ItemArray[25] as bool?;
                            string riapposizioneString = riapposizioneNullable.HasValue && riapposizioneNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Riapposizione: {riapposizioneString}").SetFixedPosition(400, 430, 80));
                            if (riapposizioneString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Riapposizione = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_400; // Riquadro inizia a startX
                                float yPosBox = startY_dissequestrotemporaneo - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Riapposizione.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Riapposizione.SetLineWidth(0.8f);
                                Canvas_Riapposizione.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_dissequestrotemporaneo;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Riapposizione.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 //.MoveTextCursorToPosition(xPosText - 2.5f, yPosText + 2.5f) // Sposta il cursore al centro (aggiustamenti)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Riapposizione:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_400 + boxSize + 5, startY_dissequestrotemporaneo - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Riapposizione:");
                                descriptionParagraph.SetFixedPosition(startX_400, startY_dissequestrotemporaneo, 100);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Violaz. Cod. Beni Culturali
                            bool? violazioniCodiciNullable = schede.Rows[0].ItemArray[31] as bool?;
                            string violazioniCodiciString = violazioniCodiciNullable.HasValue && violazioniCodiciNullable.Value ? "X" : "";
                            float startY_violazioniCodici = startY; // Use the dynamic startY
                            if (violazioniCodiciString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Violaz = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_violazioniCodici - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Violaz.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Violaz.SetLineWidth(0.8f);
                                Canvas_Violaz.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_violazioniCodici;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Violaz.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 //.MoveTextCursorToPosition(xPosText - 2.5f, yPosText + 2.5f) // Sposta il cursore al centro (aggiustamenti)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Violaz. Cod. Beni Culturali:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_violazioniCodici - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Violaz. Cod. Beni Culturali:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_violazioniCodici, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Accertamento avvenuto ripristino
                            bool? accertamentoRipNullable = schede.Rows[0].ItemArray[28] as bool?;
                            string accertamentoRipString = accertamentoRipNullable.HasValue && accertamentoRipNullable.Value ? "X" : "";
                            float startY_accertamentoRip = startY; // Use the dynamic startY
                            if (accertamentoRipString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Accertamento = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_accertamentoRip - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Accertamento.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Accertamento.SetLineWidth(0.8f);
                                Canvas_Accertamento.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_accertamentoRip;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Accertamento.BeginText()
                             .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                             .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                             .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                             //.MoveTextCursorToPosition(xPosText - 2.5f, yPosText + 2.5f) // Sposta il cursore al centro (aggiustamenti)
                             .ShowText("X") // Scrive la "X"
                             .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Accertamento avvenuto ripristino:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_accertamentoRip - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Accertamento avvenuto ripristino:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_accertamentoRip, 200);
                                document.Add(descriptionParagraph);
                            }
                            // Totale 
                            bool? totaleRipNullable = schede.Rows[0].ItemArray[29] as bool?;
                            string totaleRipString = totaleRipNullable.HasValue && totaleRipNullable.Value ? "X" : "";
                            // document.Add(new Paragraph($"Totale: {totaleRipString}").SetFixedPosition(270, 390, 100));
                            if (totaleRipString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Totale = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_270; // Riquadro inizia a startX
                                float yPosBox = startY_accertamentoRip - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Totale.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Totale.SetLineWidth(0.8f);
                                Canvas_Totale.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_accertamentoRip;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Totale.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Totale:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_270 + boxSize + 5, startY_accertamentoRip - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Totale:");
                                descriptionParagraph.SetFixedPosition(startX_270, startY_accertamentoRip, 100);
                                document.Add(descriptionParagraph);
                            }
                            // Parziale 
                            bool? parzialeRipNullable = schede.Rows[0].ItemArray[30] as bool?;
                            string parzialeRipString = parzialeRipNullable.HasValue && parzialeRipNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Parziale:  {parzialeRipString}").SetFixedPosition(350, 390, 100));
                            if (parzialeRipString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Parziale = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_350; // Riquadro inizia a startX
                                float yPosBox = startY_accertamentoRip - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Parziale.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Parziale.SetLineWidth(0.8f);
                                Canvas_Parziale.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_accertamentoRip;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Parziale.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Parziale:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_350 + boxSize + 5, startY_accertamentoRip - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Parziale:");
                                descriptionParagraph.SetFixedPosition(startX_350, startY_accertamentoRip, 100);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controlli Scia
                            bool? sciaNullable = schede.Rows[0].ItemArray[27] as bool?;
                            string sciaString = sciaNullable.HasValue && sciaNullable.Value ? "X" : "";
                            float startY_scia = startY; // Use the dynamic startY
                            if (sciaString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_ControlliScia = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_scia - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_ControlliScia.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_ControlliScia.SetLineWidth(0.8f);
                                Canvas_ControlliScia.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_scia;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_ControlliScia.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controlli Scia:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_scia - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controlli Scia:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_scia, 100);
                                document.Add(descriptionParagraph);
                            }
                            //riga interruzione sezione
                            float x3 = 65;
                            float y3 = startY;
                            float width3 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas3 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x3, y3) // Inizia la linea nel punto (x, y)
                                  .LineTo(x3 + width3, y3) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // QUALIFICAZIONE INTERVENTO
                            document.Add(new Paragraph("QUALIFICAZIONE INTERVENTO").SetFixedPosition(70, startY, 500).SetTextAlignment(TextAlignment.CENTER));
                            startY -= lineHeight; // Move to the next line
                            // Controllo aree cantiere su suolo pubblico
                            bool? contrAreeNullable = schede.Rows[0].ItemArray[32] as bool?;
                            string contrAreeString = contrAreeNullable.HasValue && contrAreeNullable.Value ? "X" : "";
                            float startY_contrAree = startY; // Use the dynamic startY
                            if (contrAreeString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Controlloaree = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_contrAree - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Controlloaree.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Controlloaree.SetLineWidth(0.8f);
                                Canvas_Controlloaree.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_contrAree;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Controlloaree.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo aree cantiere su suolo pubblico:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_contrAree - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo aree cantiere su suolo pubblico:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrAree, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controllo Cantiere
                            bool? contrSeqNullable = schede.Rows[0].ItemArray[34] as bool?;
                            string contrSeqString = contrSeqNullable.HasValue && contrSeqNullable.Value ? "X" : "";
                            float startY_contrSeq = startY; // Use the dynamic startY
                            if (contrSeqString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_ControlloCantiere = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_contrSeq - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_ControlloCantiere.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_ControlloCantiere.SetLineWidth(0.8f);
                                Canvas_ControlloCantiere.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_contrSeq;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_ControlloCantiere.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo Cantiere:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_contrSeq - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---rel
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo Cantiere:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrSeq, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controllo nato da esposti
                            bool? contrEspNullable = schede.Rows[0].ItemArray[35] as bool?;
                            string contrEspString = contrEspNullable.HasValue && contrEspNullable.Value ? "X" : "";
                            float startY_contrEsp = startY; // Use the dynamic startY
                            if (contrEspString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Controlloesposti = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_contrEsp - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Controlloesposti.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Controlloesposti.SetLineWidth(0.8f);
                                Canvas_Controlloesposti.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_contrEsp;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Controlloesposti.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da esposti:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_contrEsp - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da esposti:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrEsp, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controllo nato da segnalazioni
                            bool? contrSegnNullable = schede.Rows[0].ItemArray[36] as bool?;
                            string contrSegnString = contrSegnNullable.HasValue && contrSegnNullable.Value ? "X" : "";
                            float startY_contrSegn = startY; // Use the dynamic startY
                            if (contrSegnString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Controllosegnalazioni = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_contrSegn - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Controllosegnalazioni.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Controllosegnalazioni.SetLineWidth(0.8f);
                                Canvas_Controllosegnalazioni.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_contrSegn;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Controllosegnalazioni.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da segnalazioni:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_contrSegn - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da segnalazioni:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrSegn, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controlli lavori edili con/senza protezione (d.p.i.)
                            bool? contrEdilNullable = schede.Rows[0].ItemArray[33] as bool?;
                            string contrEdilString = contrEdilNullable.HasValue && contrEdilNullable.Value ? "X" : "";
                            float startY_contrEdil = startY; // Use the dynamic startY
                            if (contrEdilString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Controllilavori = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_70; // Riquadro inizia a startX
                                float yPosBox = startY_contrEdil - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Controllilavori.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Controllilavori.SetLineWidth(0.8f);
                                Canvas_Controllilavori.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_contrEdil;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Controllilavori.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controlli lavori edili con/senza protezione (d.p.i.):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_70 + boxSize + 5, startY_contrEdil - 5, 600); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controlli lavori edili con/senza protezione (d.p.i.):");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrEdil, 800);
                                document.Add(descriptionParagraph);
                            }
                            
                            // Con (d.p.i.)
                            bool? contrConDpiNullable = schede.Rows[0].ItemArray[44] as bool?;
                            string contrConDpiString = contrConDpiNullable.HasValue && contrConDpiNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Con  {contrConDpiString}").SetFixedPosition(350, 250, 70));
                            if (contrConDpiString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_Condpi = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_350; // Riquadro inizia a startX
                                float yPosBox = startY_contrEdil - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_Condpi.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_Condpi.SetLineWidth(0.8f);
                                Canvas_Condpi.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_contrEdil;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_Condpi.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Con (d.p.i.):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_350 + boxSize + 5, startY_contrEdil - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Con (d.p.i.):");
                                descriptionParagraph.SetFixedPosition(startX_350, startY_contrEdil, 100);
                                document.Add(descriptionParagraph);
                            }
                            // Senza (d.p.i.)
                            bool? contrSenzaDpiNullable = schede.Rows[0].ItemArray[45] as bool?;
                            string contrSenzaDpiString = contrSenzaDpiNullable.HasValue && contrSenzaDpiNullable.Value ? "X" : "";
                            // document.Add(new Paragraph($"Senza  {contrSenzaDpiString}").SetFixedPosition(450, 250, 70));
                            if (contrSenzaDpiString == "X")
                            {
                                // Ottieni il PdfDocument e PdfCanvas
                                PdfDocument pdfDocument = document.GetPdfDocument();
                                PdfCanvas Canvas_senzadpi = new PdfCanvas(pdfDocument.GetFirstPage());
                                //float boxVerticalOffsetDf = 4f;
                                // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
                                float xPosBox = startX_450; // Riquadro inizia a startX
                                float yPosBox = startY_contrEdil - (boxSize / 2) + boxVerticalOffset;

                                // --- Disegna il riquadro ---
                                Canvas_senzadpi.SetStrokeColor(ColorConstants.BLACK);
                                Canvas_senzadpi.SetLineWidth(0.8f);
                                Canvas_senzadpi.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

                                // --- Posizione della "X" (centro del riquadro) ---
                                float xPosText = xPosBox + (boxSize / 2);
                                float yPosText = startY_contrEdil;

                                // --- Aggiungi la "X" usando iText.Layout.Canvas ---
                                Canvas_senzadpi.BeginText()
                                 .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                                 .SetColor(ColorConstants.BLACK, true) // Colore della X (nero)
                                 .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                                 .ShowText("X") // Scrive la "X"
                                 .EndText();
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Senza (d.p.i.):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_450 + boxSize + 5, startY_contrEdil - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Senza (d.p.i.):");
                                descriptionParagraph.SetFixedPosition(startX_450, startY_contrEdil, 100);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            //riga interruzione sezione
                            float x4 = 65;
                            float y4 = startY;
                            float width4 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas4 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x4, y4) // Inizia la linea nel punto (x, y)
                                  .LineTo(x4 + width4, y4) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight1; // Move to the next line
                            // La PG Operante - Sezione firma
                            document.Add(new Paragraph($"La PG Operante").SetFixedPosition(280, startY, 500));
                            startY -= lineHeight1; // Move to the next line
                            document.Add(new Paragraph($"_______________________").SetFixedPosition(260, startY, 500));
                            startY -= lineHeight1; // Move to the next line
                            document.Add(new Paragraph($"_______________________").SetFixedPosition(260, startY, 500));

                            document.Close(); // Chiude il documento.


                        }
                    }
                }

                // Invia l'output PDF direttamente al browser.
                byte[] pdfBytes = stream.ToArray();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/pdf";
                response.AddHeader("Content-Disposition", "inline; filename=SchedaIntervento.pdf");
                response.BinaryWrite(pdfBytes);
                response.Flush();
                response.End();
            }

        }
        protected void btStampa_Click(object sender, EventArgs e)
        {
            int id = System.Convert.ToInt32(HfIdScheda.Value);
            DataTable schede = mn.GetSchedeBy(null, null, null, ckModAttivitòInterna.Checked, id);

            CreaPdf(schede);

        }
    }
}