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
using System.Collections.Generic;
using System.Linq;



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
             //   ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica scheda non riuscito, controllare il log." + "'); $('#ModalRicerca').modal('show');", true);

                Session["popApertoRicercaScheda"] = "si";
                SetControlsEnabled(divDettagli, false);
                SetControlsEnabled(divTesta, false);
                //SetControlsVisible(divDDLPattuglia, false);
                //SetControlsVisible(divPattuglia, true);
                //if (ruolo == "admin")
                //    btModificaScheda.Enabled = true;
                //else
                //    btModificaScheda.Enabled = false;
            }
            //else
           // {
                //if (Session["popApertoRicercaScheda"] != null)
                //{
                //    apripopup_Click(sender, e);
                //}
           // }

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
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
            Session["popApertoRicercaScheda"] = "si";
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
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#ModalRicerca').modal('hide');", true);

                //                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "hideModal();", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);

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
            ckRimozione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[24]);
            ckRiapposizione.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[25]);
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
            rdNonAvvenuto.Checked = System.Convert.ToBoolean(rap.Rows[0].ItemArray[47]);
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
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#ModalRicerca').modal('hide');", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);

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
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non trovata." + "'); $('#errorModal').modal('show');", true);

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
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non trovata." + "'); $('#errorModal').modal('show');", true);

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
//            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal(ModalRicerca);", true);
            ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica scheda non riuscito, controllare il log." + "'); $('#ModalRicerca').modal('show');", true);

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
            Boolean val = false;
            Statistiche stat = new Statistiche();
            DataTable scheda = mn.GetSchedaById(HfIdScheda.Value);
            string mese = string.Empty;
            int anno = 0;
            if (scheda.Rows.Count > 0)
            {
                DateTime dataConsegna = System.Convert.ToDateTime(scheda.Rows[0].ItemArray[39].ToString()); // Recupera la data dal DataTable
                //recupero mese ed anno dalla scheda intervento per poi modificafre le statistiche
                mese = dataConsegna.ToString("MMMM").ToUpper();
                anno = System.Convert.ToInt32(dataConsegna.ToString("yyyy"));
                String meseCorr = DateTime.Now.ToString("MMMM").ToUpper();
                int annoCorr = System.Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                List<string> list = new List<string>();
#if DEBUG
                mese = "APRILE";
                anno = 2025;
                meseCorr = "APRILE";
#endif
                //controllo se diverso da anno e mese corrente, in questo caso non si può modificare
                if (anno == annoCorr)
                {
                    if (mese == meseCorr)
                    {


                        DataTable dt = new DataTable();
                        dt = mn.getStatisticaByMeseAnno(mese, anno);
                        if (dt.Rows.Count > 0)
                        {
                            stat.mese = dt.Rows[0].ItemArray[1].ToString().Trim();
                            stat.anno = System.Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                            stat.relazioni = System.Convert.ToInt32(dt.Rows[0].ItemArray[3]);//-
                            stat.ponteggi = System.Convert.ToInt32(dt.Rows[0].ItemArray[4]);//-
                            stat.dpi = System.Convert.ToInt32(dt.Rows[0].ItemArray[5]);//-
                            stat.esposti_evasi = System.Convert.ToInt32(dt.Rows[0].ItemArray[7]);//-
                            stat.ripristino_tot_par = System.Convert.ToInt32(dt.Rows[0].ItemArray[8]);//-
                            stat.controlli_scia = System.Convert.ToInt32(dt.Rows[0].ItemArray[9]);//-
                            stat.cnr = System.Convert.ToInt32(dt.Rows[0].ItemArray[11]);//-
                            stat.notifiche = System.Convert.ToInt32(dt.Rows[0].ItemArray[13]);//-
                            stat.sequestri = System.Convert.ToInt32(dt.Rows[0].ItemArray[14]);//-
                            stat.riapp_sigilli = System.Convert.ToInt32(dt.Rows[0].ItemArray[15]);//-
                            stat.cnr_annotazioni = System.Convert.ToInt32(dt.Rows[0].ItemArray[18]);
                            stat.convalide = System.Convert.ToInt32(dt.Rows[0].ItemArray[21]);//-
                            stat.violazione_sigilli = System.Convert.ToInt32(dt.Rows[0].ItemArray[23]);//-
                            stat.dissequestri = System.Convert.ToInt32(dt.Rows[0].ItemArray[24]);//-
                            stat.dissequestri_temp = System.Convert.ToInt32(dt.Rows[0].ItemArray[25]);//-
                            stat.rimozione_sigilli = System.Convert.ToInt32(dt.Rows[0].ItemArray[26]);//-
                            stat.controlli_42_04 = System.Convert.ToInt32(dt.Rows[0].ItemArray[27]);//-
                            stat.contr_cant_suolo_pubb = System.Convert.ToInt32(dt.Rows[0].ItemArray[28]);//-
                            stat.contr_lavori_edili = System.Convert.ToInt32(dt.Rows[0].ItemArray[29]);//??
                            stat.contr_cant = System.Convert.ToInt32(dt.Rows[0].ItemArray[30]);//-
                            stat.contr_nato_da_esposti = System.Convert.ToInt32(dt.Rows[0].ItemArray[31]);
                            stat.viol_amm_reg_com = System.Convert.ToInt32(dt.Rows[0].ItemArray[32]);//-
                            stat.deleghe_esitate = System.Convert.ToInt32(dt.Rows[0].ItemArray[17]);//-
                            stat.annotazioni = System.Convert.ToInt32(dt.Rows[0].ItemArray[12]);//-

                            //sottraggo i valori nella tabella statistiche
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[15]));//relazioni
                            if (val)
                            {
                                stat.relazioni = System.Convert.ToInt32(dt.Rows[0].ItemArray[3]) - 1;
                                if (stat.relazioni < 0)
                                {
                                    stat.relazioni = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.relazione.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[9]));//esposti evasi
                            if (val)
                            {
                                stat.esposti_evasi = System.Convert.ToInt32(dt.Rows[0].ItemArray[7]) - 1;
                                if (stat.esposti_evasi < 0)
                                {
                                    stat.esposti_evasi = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.espoEvasi.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[28]));//ripristino tot par
                            if (val)
                            {
                                stat.ripristino_tot_par = System.Convert.ToInt32(dt.Rows[0].ItemArray[8]) - 1;
                                if (stat.ripristino_tot_par < 0)
                                {
                                    stat.ripristino_tot_par = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.ripTotPar.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[27]));//controlli scia
                            if (val)
                            {
                                stat.controlli_scia = System.Convert.ToInt32(dt.Rows[0].ItemArray[9]) - 1;
                                if (stat.controlli_scia < 0)
                                {
                                    stat.controlli_scia = 0;
                                }
                                list.Add(Enumerate.CampiXStatistiche.contrScia.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[11]));//notifiche
                            if (val)
                            {
                                stat.notifiche = System.Convert.ToInt32(dt.Rows[0].ItemArray[13]) - 1;
                                if (stat.notifiche < 0)
                                {
                                    stat.notifiche = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.notifiche.ToString());

                            }

                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[16]));//cnr
                            if (val)
                            {
                                stat.cnr = System.Convert.ToInt32(dt.Rows[0].ItemArray[11]) - 1;
                                if (stat.cnr < 0)
                                {
                                    stat.cnr = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.cnr.ToString());

                            }

                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[18]));//sequestri
                            if (val)
                            {
                                stat.sequestri = System.Convert.ToInt32(dt.Rows[0].ItemArray[14]) - 1;
                                if (stat.sequestri < 0)
                                {
                                    stat.sequestri = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.sequestri.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[25]));//riapposizione sigilli
                            if (val)
                            {
                                stat.riapp_sigilli = System.Convert.ToInt32(dt.Rows[0].ItemArray[15]) - 1;
                                if (stat.riapp_sigilli < 0)
                                {
                                    stat.riapp_sigilli = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.riappSigilli.ToString());
                            }

                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[17]));//annotazioni
                            if (val)
                            {
                                stat.annotazioni = System.Convert.ToInt32(dt.Rows[0].ItemArray[12]) - 1;
                                if (stat.annotazioni < 0)
                                {
                                    stat.annotazioni = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.annotazioni.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[19]));//deleghe esitate
                            if (val)
                            {
                                stat.deleghe_esitate = System.Convert.ToInt32(dt.Rows[0].ItemArray[17]) - 1;
                                if (stat.deleghe_esitate < 0)
                                {
                                    stat.deleghe_esitate = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.delegheEsitate.ToString());
                            }

                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[20]));//violazione amm
                            if (val)
                            {
                                stat.viol_amm_reg_com = System.Convert.ToInt32(dt.Rows[0].ItemArray[32]) - 1;
                                if (stat.viol_amm_reg_com < 0)
                                {
                                    stat.viol_amm_reg_com = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.violAmm.ToString());
                            }

                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[21]));//convalide
                            if (val)
                            {
                                stat.convalide = System.Convert.ToInt32(dt.Rows[0].ItemArray[21]) - 1;
                                if (stat.convalide < 0)
                                {
                                    stat.convalide = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.convalide.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[22]));//dissequestri
                            if (val)
                            {
                                stat.dissequestri = System.Convert.ToInt32(dt.Rows[0].ItemArray[24]) - 1;
                                if (stat.dissequestri < 0)
                                {
                                    stat.dissequestri = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.dissequestri.ToString());
                            }


                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[23]));//dissequestri temp
                            if (val)
                            {
                                stat.dissequestri_temp = System.Convert.ToInt32(dt.Rows[0].ItemArray[25]) - 1;
                                if (stat.dissequestri_temp < 0)
                                {
                                    stat.dissequestri_temp = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.disseqTemp.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[24]));//rimozione sigilli
                            if (val)
                            {
                                stat.rimozione_sigilli = System.Convert.ToInt32(dt.Rows[0].ItemArray[26]) - 1;
                                if (stat.rimozione_sigilli < 0)
                                {
                                    stat.rimozione_sigilli = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.rimozSigilli.ToString());
                            }

                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[26]));//violazione sigilli
                            if (val)
                            {
                                stat.violazione_sigilli = System.Convert.ToInt32(dt.Rows[0].ItemArray[23]) - 1;
                                if (stat.violazione_sigilli < 0)
                                {
                                    stat.violazione_sigilli = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.violSigilli.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[31]));//controlli 42_04 sigilli
                            if (val)
                            {
                                stat.controlli_42_04 = System.Convert.ToInt32(dt.Rows[0].ItemArray[27]) - 1;
                                if (stat.controlli_42_04 < 0)
                                {
                                    stat.controlli_42_04 = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.contr4204.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[32]));//ponteggi / cantieri suolo pubblico
                            if (val)
                            {
                                stat.ponteggi = System.Convert.ToInt32(dt.Rows[0].ItemArray[4]) - 1;
                                if (stat.ponteggi < 0)
                                {
                                    stat.ponteggi = 0;
                                }
                                stat.contr_cant_suolo_pubb = System.Convert.ToInt32(dt.Rows[0].ItemArray[28]) - 1;
                                if (stat.contr_cant_suolo_pubb < 0)
                                {
                                    stat.contr_cant_suolo_pubb = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.ponteggi.ToString());
                            }

                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[33]));//controllo lavori edili e DPI
                            if (val)
                            {
                                stat.contr_lavori_edili = System.Convert.ToInt32(dt.Rows[0].ItemArray[29]) - 1;
                                if (stat.contr_lavori_edili < 0)
                                {
                                    stat.contr_lavori_edili = 0;
                                }
                                stat.dpi = System.Convert.ToInt32(dt.Rows[0].ItemArray[5]) - 1;
                                if (stat.dpi < 0)
                                {
                                    stat.dpi = 0;
                                }

                                list.Add(Enumerate.CampiXStatistiche.dpi.ToString());
                            }
                            val = System.Convert.ToBoolean(System.Convert.ToBoolean(scheda.Rows[0].ItemArray[34]));//controllo cantieri 
                            if (val)
                            {
                                stat.contr_cant = System.Convert.ToInt32(dt.Rows[0].ItemArray[30]) - 1;
                                if (stat.contr_cant < 0)
                                {
                                    stat.contr_cant = 0;
                                }
                                list.Add(Enumerate.CampiXStatistiche.contrCant.ToString());
                            }


                        }

                        Boolean del = mn.DeleteTranSchedaStatistiche(stat, System.Convert.ToInt32(HfIdScheda.Value));
                        if (del)
                        {
                            list.Clear();
                            Boolean continua = Convalida();

                            if (continua)
                            {
                                RappUote rap = new RappUote();


                                rap.nominativo = txtNominativo.Text.ToUpper();
                                rap.indirizzo = txtIndirizzo.Text;
                                if (DdlPattuglia.SelectedValue != "0")
                                {
                                    for (int i = 0; i < LPattugliaCompleta.Items.Count; i++)
                                    {
                                        rap.pattuglia += LPattugliaCompleta.Items[i].Value + "/";
                                    }
                                }
                                if (rap.pattuglia.Length > 1)
                                {
                                    string ultimoC = rap.pattuglia.Substring(rap.pattuglia.Length - 1);
                                    if (ultimoC == "/")
                                    {
                                        rap.pattuglia = rap.pattuglia.Remove(rap.pattuglia.Length - 1, 1);

                                    }
                                    string primoC = rap.pattuglia.Substring(0, 1);

                                    if (primoC == "/")
                                    {
                                        rap.pattuglia = rap.pattuglia.Remove(0, 1);

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
                                rap.pratica = txtPratica.Text;
                                rap.delegaAG = ckDelega.Checked;
                                rap.resa = ckResa.Checked;
                                rap.segnalazione = ckSegnalazione.Checked;

                                rap.esposti = ckEsposto.Checked;
                                if (ckNotifica.Checked)
                                {
                                    stat.notifiche = 1;
                                    list.Add(Enumerate.CampiXStatistiche.notifiche.ToString());
                                }
                                rap.notifica = ckNotifica.Checked;
                                rap.iniziativa = ckIniziativa.Checked;
                                rap.cdr = ckCdr.Checked;
                                rap.coordinatore = ckCoordinatore.Checked;
                                if (ckRelazione.Checked)
                                {
                                    stat.relazioni = 1;
                                    list.Add(Enumerate.CampiXStatistiche.relazione.ToString());
                                }

                                rap.relazione = ckRelazione.Checked;
                                if (ckCnr.Checked)
                                {
                                    stat.cnr = 1;
                                    list.Add(Enumerate.CampiXStatistiche.cnr.ToString());
                                }
                                rap.cnr = ckCnr.Checked;
                                if (ckAnnotazionePG.Checked)
                                {
                                    stat.annotazioni = 1;
                                    list.Add(Enumerate.CampiXStatistiche.annotazioni.ToString());
                                }
                                rap.annotazionePG = ckAnnotazionePG.Checked;
                                if (ckVerbaleSeq.Checked)
                                {
                                    stat.sequestri = 1;
                                    list.Add(Enumerate.CampiXStatistiche.sequestri.ToString());
                                }
                                rap.verbaleSeq = ckVerbaleSeq.Checked;
                                if (ckEsitoDelega.Checked)
                                {
                                    stat.deleghe_esitate = 1;
                                    list.Add(Enumerate.CampiXStatistiche.delegheEsitate.ToString());
                                }
                                rap.esitoDelega = ckEsitoDelega.Checked;
                                if (ckContestazioneAmm.Checked)
                                {
                                    stat.viol_amm_reg_com = 1;
                                    list.Add(Enumerate.CampiXStatistiche.violAmm.ToString());
                                }
                                rap.contestazioneAmm = ckContestazioneAmm.Checked;
                                if (ckConvalida.Checked)
                                {
                                    stat.convalide = 1;
                                    list.Add(Enumerate.CampiXStatistiche.convalide.ToString());
                                }
                                rap.convalida = ckConvalida.Checked;
                                if (ckDisseqDefinitivo.Checked)
                                {
                                    stat.dissequestri = 1;
                                    list.Add(Enumerate.CampiXStatistiche.dissequestri.ToString());
                                }
                                rap.dissequestroDef = ckDisseqDefinitivo.Checked;
                                if (ckDisseqTemp.Checked)
                                {
                                    stat.dissequestri_temp = 1;
                                    list.Add(Enumerate.CampiXStatistiche.disseqTemp.ToString());
                                }
                                rap.dissequestroTemp = ckDisseqTemp.Checked;
                                if (ckRimozione.Checked)
                                {
                                    stat.rimozione_sigilli = 1;
                                    list.Add(Enumerate.CampiXStatistiche.rimozSigilli.ToString());
                                }
                                rap.rimozione = ckRimozione.Checked;
                                if (ckRiapposizione.Checked)
                                {
                                    stat.riapp_sigilli = 1;
                                    list.Add(Enumerate.CampiXStatistiche.riappSigilli.ToString());
                                }
                                rap.riapposizione = ckRiapposizione.Checked;
                                if (ckViolazioneSigilli.Checked)
                                {
                                    stat.violazione_sigilli = 1;
                                    list.Add(Enumerate.CampiXStatistiche.violSigilli.ToString());
                                }
                                rap.violazioneSigilli = ckViolazioneSigilli.Checked;
                                if (ckControlliSCIA.Checked)
                                {
                                    stat.controlli_scia = 1;
                                    list.Add(Enumerate.CampiXStatistiche.contrScia.ToString());
                                }
                                rap.controlliScia = ckControlliSCIA.Checked;
                                rap.accertAvvenutoRip = ckAccertAvvenutoRipr.Checked;
                                if (ckAccertAvvenutoRipr.Checked)
                                {
                                    stat.ripristino_tot_par = 1;
                                    list.Add(Enumerate.CampiXStatistiche.ripTotPar.ToString());
                                }
                                rap.totale = rdTotale.Checked;
                                rap.parziale = rdParziale.Checked;
                                rap.non_avvenuto = rdNonAvvenuto.Checked;

                                rap.conProt = rdCon.Checked;
                                rap.senzaProt = rdSenza.Checked;
                                if (ckViolazioneBeniCult.Checked)
                                {
                                    stat.controlli_42_04 = 1;
                                    list.Add(Enumerate.CampiXStatistiche.contr4204.ToString());
                                }
                                rap.violazioneBeniCult = ckViolazioneBeniCult.Checked;
                                if (ckContrSuoloPubblico.Checked)
                                {
                                    stat.contr_cant_suolo_pubb = 1;
                                    list.Add(Enumerate.CampiXStatistiche.contr_cant_suolo_pubb.ToString());
                                    stat.ponteggi = 1;
                                    list.Add(Enumerate.CampiXStatistiche.ponteggi.ToString());

                                }
                                rap.contrCantSuoloPubb = ckContrSuoloPubblico.Checked;
                                if (ckControlliCant.Checked) //cantieri a sequestro
                                {
                                    stat.contr_cant = 1;

                                    list.Add(Enumerate.CampiXStatistiche.contrCant.ToString());
                                }
                                rap.contr_cantiereSeq = ckControlliCant.Checked;

                                if (ckControlliLavoriEdiliSenzaProt.Checked)
                                {
                                    stat.dpi = 1;
                                    list.Add(Enumerate.CampiXStatistiche.dpi.ToString());
                                }
                                rap.contrEdiliDPI = ckControlliLavoriEdiliSenzaProt.Checked;
                                rap.contrDaEsposti = ckControlloDaEsposti.Checked;
                                rap.contrDaSegn = ckControlliDaSegnalazioni.Checked;
                                rap.uote = rdUote.Checked;
                                rap.uotp = rdUotp.Checked;
                                if (txt_numEspostiSegn.Text == "0")
                                {
                                    rap.num_esposti = "0";
                                }
                                else
                                {
                                    rap.num_esposti = txt_numEspostiSegn.Text;
                                    stat.esposti_evasi = System.Convert.ToInt32(rap.num_esposti);
                                    list.Add(Enumerate.CampiXStatistiche.espoEvasi.ToString());
                                }

                                rap.nota = txtNote.Text;
                                rap.attività_interna = CkAttivita.Checked;
                                if (!String.IsNullOrEmpty(txtDataConsegna.Text))
                                {
                                    rap.data_consegna_intervento = System.Convert.ToDateTime(txtDataConsegna.Text);
                                }
                                rap.dataInserimento = DateTime.Now;
                                stat.mese = mese;//MeseCorrente;
                                stat.anno = anno;//System.Convert.ToInt16(AnnoCorrente);
                                Boolean resp = false;
                                Int32 idN = 0;

                                VerificaStatistiche(stat, list);

                                resp = mn.InsRappUote(rap, stat, out idN);
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
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Non è possibile modificare la scheda del mese precedente." + "'); $('#errorModal').modal('show');", true);

                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Non è possibile modificare la scheda dell'anno precedente." + "'); $('#errorModal').modal('show');", true);
                }
            }
        }
        private Statistiche VerificaStatistiche(Statistiche stat, List<string> list)
        {

            Manager mn = new Manager();
            DataTable dt = new DataTable();
            dt = mn.getStatisticaByMeseAnno(stat.mese, stat.anno);
            if (dt.Rows.Count > 0)
            {
                stat.mese = dt.Rows[0].ItemArray[1].ToString().Trim();
                stat.anno = System.Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                var noDupes = list.Distinct().ToList();
                foreach (string item in noDupes)
                {

                    if (item == Enumerate.CampiXStatistiche.relazione.ToString())
                    {
                        if (stat.relazioni > 0)
                            stat.relazioni = System.Convert.ToInt32(dt.Rows[0].ItemArray[3]) + 1;

                    }
                    if (item == Enumerate.CampiXStatistiche.ponteggi.ToString())
                    {
                        if (stat.ponteggi > 0)
                            stat.ponteggi = System.Convert.ToInt32(dt.Rows[0].ItemArray[4]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.dpi.ToString())
                    {
                        if (stat.dpi > 0)
                            stat.dpi = System.Convert.ToInt32(dt.Rows[0].ItemArray[5]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.espoEvasi.ToString())
                    {
                        if (stat.esposti_evasi > 0)
                            stat.esposti_evasi = System.Convert.ToInt32(dt.Rows[0].ItemArray[7]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.ripTotPar.ToString())
                    {
                        if (stat.ripristino_tot_par > 0)
                            stat.ripristino_tot_par = System.Convert.ToInt32(dt.Rows[0].ItemArray[8]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.contrScia.ToString())
                    {
                        if (stat.controlli_scia > 0)
                            stat.controlli_scia = System.Convert.ToInt32(dt.Rows[0].ItemArray[9]) + 1;
                    }


                    if (item == Enumerate.CampiXStatistiche.cnr.ToString())
                    {
                        if (stat.cnr > 0)
                            stat.cnr = System.Convert.ToInt32(dt.Rows[0].ItemArray[11]) + 1;

                    }
                    if (item == Enumerate.CampiXStatistiche.annotazioni.ToString())

                    {
                        if (stat.annotazioni > 0)

                            stat.annotazioni = System.Convert.ToInt32(dt.Rows[0].ItemArray[12]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.notifiche.ToString())

                    {
                        if (stat.notifiche > 0)
                            stat.notifiche = System.Convert.ToInt32(dt.Rows[0].ItemArray[13]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.sequestri.ToString())
                    {
                        if (stat.sequestri > 0)

                            stat.sequestri = System.Convert.ToInt32(dt.Rows[0].ItemArray[14]) + 1;

                    }
                    if (item == Enumerate.CampiXStatistiche.riappSigilli.ToString())
                    {
                        if (stat.riapp_sigilli > 0)
                            stat.riapp_sigilli = System.Convert.ToInt32(dt.Rows[0].ItemArray[15]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.convalide.ToString())
                    {
                        if (stat.convalide > 0)
                            stat.convalide = System.Convert.ToInt32(dt.Rows[0].ItemArray[21]) + 1;

                    }
                    if (item == Enumerate.CampiXStatistiche.violSigilli.ToString())
                    {
                        if (stat.violazione_sigilli > 0)
                            stat.violazione_sigilli = System.Convert.ToInt32(dt.Rows[0].ItemArray[23]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.dissequestri.ToString())
                    {
                        if (stat.dissequestri > 0)
                            stat.dissequestri = System.Convert.ToInt32(dt.Rows[0].ItemArray[24]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.disseqTemp.ToString())
                    {
                        if (stat.dissequestri_temp > 0)
                            stat.dissequestri_temp = System.Convert.ToInt32(dt.Rows[0].ItemArray[25]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.rimozSigilli.ToString())
                    {
                        if (stat.rimozione_sigilli > 0)
                            stat.rimozione_sigilli = System.Convert.ToInt32(dt.Rows[0].ItemArray[26]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.contr4204.ToString())
                    {
                        if (stat.controlli_42_04 > 0)
                            stat.controlli_42_04 = System.Convert.ToInt32(dt.Rows[0].ItemArray[27]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.contrCant.ToString())
                    {
                        if (stat.contr_cant > 0)
                            stat.contr_cant = System.Convert.ToInt32(dt.Rows[0].ItemArray[30]) + 1;
                    }
                    if (item == Enumerate.CampiXStatistiche.contr_cant_suolo_pubb.ToString())
                    {
                        if (stat.contr_cant_suolo_pubb > 0)
                            stat.contr_cant_suolo_pubb = System.Convert.ToInt32(dt.Rows[0].ItemArray[28]);
                    }


                    if (item == Enumerate.CampiXStatistiche.violAmm.ToString())
                    {
                        if (stat.viol_amm_reg_com > 0)
                            stat.viol_amm_reg_com += System.Convert.ToInt32(dt.Rows[0].ItemArray[32]) + 1;
                    }
                }
            }


            return stat;
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
        protected void btPopStampa_Click(object sender, EventArgs e)
        {
            int id = System.Convert.ToInt32(HfIdScheda.Value);
            DataTable schede = mn.GetSchedeBy(null, null, null, CkAttivita.Checked, id);

            Routine stampa = new Routine();
            stampa.CreaPdf(schede);
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
            ckRiapposizione.Checked = false;
            rdCon.Checked = false;
            rdSenza.Checked = false;
            rdTotale.Checked = false;
            ckRimozione.Checked = false;
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
        
        protected void btStampa_Click(object sender, EventArgs e)
        {
            int id = System.Convert.ToInt32(HfIdScheda.Value);
            DataTable schede = mn.GetSchedeBy(null, null, null, ckModAttivitòInterna.Checked, id);

            Routine stampa = new Routine();
            stampa.CreaPdf(schede);

        }
        protected void btChiudi_Click(object sender, EventArgs e)
        {
            Session.Remove("popApertoRicercaScheda");
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#ModalRicerca').modal('hide');", true);
        }
        protected void GVRicecaScheda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVRicecaScheda.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            btRicScheda_Click(sender, e);

        }
    }
}