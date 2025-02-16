using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;

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
                profilo= Session["profilo"].ToString();
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
                if (ruolo == "accertatori" || ruolo == "admin")
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
                DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);
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
            txtNominativo.Text = rap.Rows[0].ItemArray[2].ToString();
            txtIndirizzo.Text = rap.Rows[0].ItemArray[3].ToString();
            if (!string.IsNullOrEmpty(rap.Rows[0].ItemArray[1].ToString()))
            {
                TxtDataIntervento.Text = rap.Rows[0].ItemArray[1].ToString();
            }

            ckDelega.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[5]);
            ckResa.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[6]);
            ckSegnalazione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[7]);
            ckEsposto.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[8]);

            if (String.IsNullOrEmpty(rap.Rows[0].ItemArray[9].ToString()))
            {
                txt_numEspostiSegn.Text = "0";
            }
            else
            {
                txt_numEspostiSegn.Text = rap.Rows[0].ItemArray[9].ToString();
            }
            ckNotifica.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[10]);
            ckIniziativa.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[11]);
            ckCdr.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[12]);
            ckCoordinatore.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[13]);
            ckRelazione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[14]);
            ckCnr.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[15]);
            ckAnnotazionePG.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[16]);
            ckVerbaleSeq.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[17]);
            ckEsitoDelega.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[18]);
            ckContestazioneAmm.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[19]);
            ckConvalida.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[20]);
            ckDisseqDefinitivo.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[21]);
            ckDisseqTemp.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[22]);
            rdRimozione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[23]);
            rdRiapposizione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[24]);
            ckViolazioneSigilli.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[25]);
            ckControlliSCIA.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[26]);
            ckAccertAvvenutoRipr.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[27]);
            rdTotale.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[28]);
            rdParziale.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[29]);
            ckViolazioneBeniCult.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[30]);
            ckContrSuoloPubblico.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[31]);
            ckControlliLavoriEdiliSenzaProt.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[32]);
            ckControlloDaEsposti.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[34]);
            ckControlliCant.Checked= System.Convert.ToBoolean(rap.Rows[0].ItemArray[33]);
            ckControlliDaSegnalazioni.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[35]);
            CkAttivita.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[36]);
            
            txtNote.Text = rap.Rows[0].ItemArray[37].ToString();
            
            if (!string.IsNullOrEmpty(rap.Rows[0].ItemArray[38].ToString()))
            {
                txtDataConsegna.Text = rap.Rows[0].ItemArray[38].ToString();
            }
            //txtCapoPattuglia.Text = rap.Rows[0].ItemArray[39].ToString();
            ddlCapopattuglia.SelectedValue = rap.Rows[0].ItemArray[39].ToString();
            rdUote.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[40]);
            rdUotp.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[41]);
            rdCon.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[43]);
            rdSenza.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[44]);
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
                DataTable schede = mn.GetSchedeBy(numPratica,null,null,ckModAttivitòInterna.Checked);

                if (schede.Rows.Count > 0)
                {
                    GVRicecaScheda.DataSource = schede;
                    GVRicecaScheda.DataBind();

                }
                
            }
            if (!string.IsNullOrEmpty(txtModPattuglia.Text))
            {
               
                Manager mn = new Manager();
                DataTable schede = mn.GetSchedeBy(null,txtModPattuglia.Text,null, ckModAttivitòInterna.Checked);

                if (schede.Rows.Count > 0)
                {
                    GVRicecaScheda.DataSource = schede;
                    GVRicecaScheda.DataBind();

                }

            }
            if (!string.IsNullOrEmpty(txtModDataIntervento.Text))
            {
              
                Manager mn = new Manager();
                DataTable schede = mn.GetSchedeBy(null, null, txtModDataIntervento.Text, ckModAttivitòInterna.Checked);

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
                DataTable schede = mn.GetSchedeBy(null, txtModPattuglia.Text, null, ckModAttivitòInterna.Checked);

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
                if (String.IsNullOrEmpty(txt_numEspostiSegn.Text))
                {
                    rap.num_esposti = 0;
                }
                else
                {
                    rap.num_esposti = System.Convert.ToInt32(txt_numEspostiSegn.Text);
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
                DdlPattuglia.Items.Insert(0, new ListItem("", "0"));
                DdlPattuglia.DataBind();
                //DdlPattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                ddlCapopattuglia.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                ddlCapopattuglia.DataTextField = "Nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                ddlCapopattuglia.Items.Insert(0, new ListItem("", "0"));
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
    }
}