using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;

namespace Uotep
{
    public partial class SchedaDipendente : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String Area = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Manager mn = new Manager();
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Area = Session["area"].ToString();
            }

            if (!IsPostBack)
            {
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
               // ProtocolloLiteral.Text = decodedText;
                TxtDataAssunzione.Attributes["placeholder"] = "gg/mm/aaaa";
                txtDataProssimaSorveglianza.Attributes["placeholder"] = "gg/mm/aaaa";
                //CaricaDLL();
            }

        }

        protected void Salva_Click(object sender, EventArgs e)
        {
            //Boolean continua = Convalida();
            //String MeseCorrente = DateTime.Now.ToString("MMMM");
            //String AnnoCorrente = DateTime.Now.ToString("yyyy");
            SchedaDipendenteClass scheda = new SchedaDipendenteClass();
            scheda.Matricola = txtMatricola.Text.Trim();
            scheda.Nominativo = txtNominativo.Text.ToUpper().Trim();
            scheda.Ufficio = txtUfficio.Text.ToUpper().Trim();
            scheda.dataAssunzione = Convert.ToDateTime(TxtDataAssunzione.Text.Trim());
            DateTime dataTemp;
            if (DateTime.TryParse(txtDataProssimaSorveglianza.Text.Trim(), out dataTemp))
            {
                scheda.dataSorveglianza = dataTemp;
            }
            else
            {
                // Assegna un valore di default o gestisci l'errore
                scheda.dataSorveglianza = DateTime.MinValue;
            }
            // scheda.dataSorveglianza = Convert.ToDateTime(txtDataProssimaSorveglianza.Text.Trim());
            scheda.MacroArea = txtMacroArea.Text.ToUpper().Trim();
            scheda.GruppoReperibilita = txtGruppoRep.Text.ToUpper().Trim();
            scheda.TurnoPref = txtTurnoPref.Text.ToUpper().Trim();
            scheda.Grado = txtGrado.Text.ToUpper().Trim();
            if (rdUote.Checked)
            {
                scheda.Area = "uote";
            }
            else
            {
                scheda.Area = "uotp";
            }
            if (rdQ1.Checked)
            {
                scheda.Quartina = 1;
            }
            else if (rdQ2.Checked)
            {
                scheda.Quartina = 2;
            }
            else if (rdQ3.Checked)
            {
                scheda.Quartina = 3;
            }
            else if (rdQ4.Checked)
            {
                scheda.Quartina = 4;
            }
            scheda.IsAutista = ckAutista.Checked;
            scheda.Armato = ckArmato.Checked;
            scheda.l104 = ckL104.Checked;
            scheda.limitazione = ckLimitazioni.Checked;
            scheda.l53 = ckArt53.Checked;
            scheda.GruppoQuartina = txtGruppoQ.Text.ToUpper().Trim();
            Boolean resp = mn.InsSchedaDipendente(scheda);
 SiteMaster myMaster = this.Master as SiteMaster;
            if (!resp)
            {
                //errorMessage.InnerText = "Inserimento della scheda non riuscito";
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della scheda non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
               

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.ErrorLog.GetDescription(), "danger");

                }
            }
            else
            {
                //errorMessage.InnerText = "Inserimento scheda effettuato";
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento scheda effettuato." + "'); $('#errorModal').modal('show');", true);
                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("✅  ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");

                }
                Pulisci();

            }
        }


        private void Pulisci()
        {
            try
            {
                rdQ1.Checked = false;
                rdQ2.Checked = false;
                rdQ3.Checked = false;
                rdQ4.Checked = false;
                txtMatricola.Text = string.Empty;
                txtGruppoRep.Text = string.Empty;
                txtGruppoQ.Text = string.Empty;
                txtNominativo.Text = string.Empty;
                txtDataProssimaSorveglianza.Text = string.Empty;
                TxtDataAssunzione.Text = string.Empty;
                txtGruppoRep.Text = string.Empty;
                txtMacroArea.Text = string.Empty;
                txtUfficio.Text = string.Empty;
                txtGrado.Text = string.Empty;
                txtTurnoPref.Text = string.Empty;
                ckArmato.Checked = false;
                ckAutista.Checked = false;
                ckArt53.Checked = false;
                ckL104.Checked = false;
                rdUote.Checked = false;
                rdUotp.Checked = false;
                ckLimitazioni.Checked = false;
                TxtCategoriaEconomica.Text = string.Empty;

            }
            catch (Exception ex)
            {

                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in pulisci() turnazioneDipendeti.cs ");
                    sw.Close();
                }
            }
        }
        //private Boolean Convalida()
        //{
        //    Boolean ret = true;


        //    if (String.IsNullOrEmpty(txtMatricola.Text))
        //    {

        //            ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "In." + "'); $('#errorModal').modal('show');", true);

        //            ret = false;

        //    }
        //    return ret;
        //}
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

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

        protected void btCerca_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(txtMatricola.Text) && string.IsNullOrEmpty(txtNominativo.Text.ToUpper()))
            {
                //errorMessage.InnerText = @"⚠️ Inserire Matricola o Nominativo.";
                //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
                SiteMaster myMaster = this.Master as SiteMaster;
                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.FiledRegìquired.GetDescription(), "warning");
                    return;
                }
            }
            else
                dt = mn.getSchedaDip(txtMatricola.Text.Trim(), txtNominativo.Text.ToUpper().Trim());
            if (dt.Rows.Count > 0)
            {
                txtMatricola.Text = dt.Rows[0]["matricola_ced"].ToString().Trim();
                txtNominativo.Text = dt.Rows[0]["nominativo"].ToString().ToUpper().Trim();
                txtGruppoRep.Text = dt.Rows[0]["gruppo_reperibilita"].ToString();
                txtUfficio.Text = dt.Rows[0]["ufficio"].ToString().ToUpper().Trim();
                txtMacroArea.Text = dt.Rows[0]["macro_area"].ToString().ToUpper().Trim();
                TxtDataAssunzione.Text = Convert.ToDateTime(dt.Rows[0]["data_assunzione"].ToString()).ToString("dd/MM/yyyy");
                txtGrado.Text = dt.Rows[0]["grado"].ToString().ToUpper().Trim();
                var valoreData = dt.Rows[0]["data_sorv_sanitaria"];

                if (valoreData != DBNull.Value && valoreData.ToString() != "")
                {
                    // Converti in Data e poi prendi solo "dd/MM/yyyy"
                    txtDataProssimaSorveglianza.Text = Convert.ToDateTime(valoreData).ToString("dd/MM/yyyy");
                }
                else
                {
                    txtDataProssimaSorveglianza.Text = "";
                }
                txtTurnoPref.Text = dt.Rows[0]["turni_pref"].ToString().ToUpper();
                //txtQuartina.Text = dt.Rows[0]["quartina"].ToString();
                txtGruppoQ.Text = dt.Rows[0]["gruppo_quartina"].ToString().ToUpper().Trim();
                ckL104.Checked = dt.Rows[0]["perm_104"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["perm_104"]) : false;
                ckAutista.Checked = dt.Rows[0]["autista"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["autista"]) : false;
                ckArt53.Checked = dt.Rows[0]["perm_53"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["perm_53"]) : false;

                ckArmato.Checked = dt.Rows[0]["armato"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["armato"]) : false;
                ckLimitazioni.Checked = dt.Rows[0]["limitazioni"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["limitazioni"]) : false;

                if (dt.Rows[0]["area"].ToString() == "uotp")
                {
                    rdUotp.Checked = true;
                }
                else
                {
                    rdUote.Checked = true;
                }
                int q = Convert.ToInt32(dt.Rows[0]["quartina"]);
                switch (q)
                {
                    case 1:
                        rdQ1.Checked = true;
                        break;
                    case 2:
                        rdQ2.Checked = true;
                        break;
                    case 3:
                        rdQ3.Checked = true;
                        break;
                    case 4:
                        rdQ4.Checked = true;
                        break;
                }
            }
            else
            {
                //errorMessage.InnerText = @"⚠️ Nessun record trovato.";
                //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.Notfound.GetDescription(), "warning");

                }
            }
        }


    }

}