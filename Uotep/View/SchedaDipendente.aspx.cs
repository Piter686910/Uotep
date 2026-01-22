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
                ProtocolloLiteral.Text = decodedText;
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
            scheda.dataSorveglianza = Convert.ToDateTime(txtDataProssimaSorveglianza.Text.Trim());
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

            if (!resp)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della scheda non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#MsgStampa').text('" + "Inserimento scheda effettuato." + "'); $('#PopStampa').modal('show');", true);

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

        private void CaricaDLL()
        {
            try
            {

                //DataTable CaricaOperatori = mn.getListOperatore();
                //DdlPattuglia.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                //DdlPattuglia.DataTextField = "Nominativo"; // Il campo visibile
                ////DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                //DdlPattuglia.Items.Insert(0, new ListItem("", "0"));
                //DdlPattuglia.DataBind();
                //DdlPattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                ////DdlPattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
                ////DataTable CaricaOperatori = mn.getListOperatore();
                //ddlCapopattuglia.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                //ddlCapopattuglia.DataTextField = "Nominativo"; // Il campo visibile
                ////DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                //ddlCapopattuglia.Items.Insert(0, new ListItem("", "0"));
                //ddlCapopattuglia.DataBind();
                //ddlCapopattuglia.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                //DataTable RicercaQuartiere = mn.getListQuartiereTP();
                //DdlQuartiere.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                //DdlQuartiere.DataTextField = "Quartiere"; // Il campo visibile
                //DdlQuartiere.DataValueField = "id";
                //DdlQuartiere.DataBind();
                //DdlQuartiere.Items.Insert(0, new ListItem("", "0"));


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
                errorMessage.InnerText = @"⚠️ Inserire Matricola o Nominativo.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
            }
            else
                dt = mn.getSchedaDip(txtMatricola.Text.Trim(), txtNominativo.Text.ToUpper().Trim());
            if (dt.Rows.Count > 0)
            {
                txtMatricola.Text = dt.Rows[0]["matricola"].ToString().Trim();
                txtNominativo.Text = dt.Rows[0]["nominativo"].ToString().ToUpper().Trim();
                txtGruppoRep.Text = dt.Rows[0]["gruppo_reper"].ToString();
                txtUfficio.Text = dt.Rows[0]["ufficio"].ToString().ToUpper().Trim();
                txtMacroArea.Text = dt.Rows[0]["macro_area"].ToString().ToUpper().Trim();
                TxtDataAssunzione.Text = Convert.ToDateTime(dt.Rows[0]["data_assunzione"].ToString()).ToString("dd/MM/yyyy");
                txtGrado.Text = dt.Rows[0]["grado"].ToString().ToUpper().Trim();

                txtTurnoPref.Text = dt.Rows[0]["turni_pref"].ToString().ToUpper();
                //txtQuartina.Text = dt.Rows[0]["quartina"].ToString();
                txtGruppoQ.Text = dt.Rows[0]["gruppo"].ToString().ToUpper().Trim();
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
                errorMessage.InnerText = @"⚠️ Nessun record trovato.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
            }
        }


    }

}