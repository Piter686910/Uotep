using iText.StyledXmlParser.Jsoup.Nodes;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static System.Windows.Forms.AxHost;




namespace Uotep
{
    public partial class GestionePratica : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Manager mn = new Manager();
        String profilo = string.Empty;
        String ruolo = string.Empty;
        GestionePratiche pratica = new GestionePratiche();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                profilo = Session["profilo"].ToString();
                ruolo = Session["ruolo"].ToString();
            }

            if (!IsPostBack)
            {
                //  ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
                CaricaDLL();
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

        private void CaricaDLL()
        {
            try
            {

                DataTable CaricaOperatori = mn.getListOperatore();
                ddlOperatore.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                ddlOperatore.DataTextField = "Nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                ddlOperatore.Items.Insert(0, new ListItem("", "0"));
                ddlOperatore.DataBind();
                ddlOperatore.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));




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
        private void ValorizzaPratica()
        {
            

                
                pratica.fascicolo = txtFascicolo.Text;
                pratica.assegnato = ddlOperatore.SelectedValue;
                pratica.note = txtNota.Text;
                pratica.data_uscita = System.Convert.ToDateTime(TxtDataUscita.Text);
                if (!String.IsNullOrEmpty(txtDataRientro.Text))
                {
                    pratica.data_rientro = System.Convert.ToDateTime(txtDataRientro.Text);
                }
                if (!String.IsNullOrEmpty(txtDataSpostamento.Text))
                {
                    pratica.data_spostamento = System.Convert.ToDateTime(txtDataSpostamento.Text);
                }
                if (!String.IsNullOrEmpty(txtDataRiscontro.Text))
                {
                    pratica.data_riscontro_in_ufficio = System.Convert.ToDateTime(txtDataRiscontro.Text);
                }
            
        }

        protected void btInserisci_Click(object sender, EventArgs e)
        {

            Boolean resp = false;
            ValorizzaPratica();
            
            resp = mn.InsGestionePratica(pratica);

            if (resp)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica effettuato." + "'); $('#errorModal').modal('show');", true);
                Pulisci();
            }


        }
        private void Pulisci()
        {
            txtFascicolo.Text = string.Empty;
            txtDataRientro.Text = string.Empty;
            TxtDataUscita.Text = string.Empty;
            txtDataSpostamento.Text = string.Empty;
            txtNota.Text = string.Empty;
            txtDataRiscontro.Text = string.Empty;
            CaricaDLL();
            
        }

        protected void btRicerca_Click(object sender, EventArgs e)
        {
            string dtS = string.Empty;
            if (String.IsNullOrEmpty(txtFascicolo.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserire il Fascicolo." + "'); $('#errorModal').modal('show');", true);

            }
            else
            {

                Manager mn = new Manager();
                GestionePratiche pratica = new GestionePratiche();
                pratica.fascicolo = txtFascicolo.Text;
                DataTable dt = new DataTable();
                dt = mn.getGestionePraticaByFascicolo(txtFascicolo.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    ddlOperatore.SelectedItem.Text = dt.Rows[0].ItemArray[2].ToString();
                    DateTime appo = System.Convert.ToDateTime(dt.Rows[0].ItemArray[3].ToString());
                    if (appo == DateTime.MinValue)
                        TxtDataUscita.Text = string.Empty;
                    else
                        TxtDataUscita.Text = appo.ToString("dd/MM/yyyy");

                    appo = System.Convert.ToDateTime(dt.Rows[0].ItemArray[4].ToString());
                    if (appo == DateTime.MinValue)
                        txtDataRientro.Text = string.Empty;
                    else
                        txtDataRientro.Text = appo.ToString("dd/MM/yyyy");
                    appo = System.Convert.ToDateTime(dt.Rows[0].ItemArray[5].ToString());
                    if (appo == DateTime.MinValue)
                        txtDataSpostamento.Text = string.Empty;
                    else
                        txtDataSpostamento.Text = appo.ToString("dd/MM/yyyy");
                    appo = System.Convert.ToDateTime(dt.Rows[0].ItemArray[6].ToString());
                    if (appo == DateTime.MinValue)
                        txtDataRiscontro.Text = string.Empty;
                    else
                        txtDataRiscontro.Text = appo.ToString("dd/MM/yyyy");
                    txtNota.Text = dt.Rows[0].ItemArray[7].ToString();

                }
            }
        }

        protected void btModifica_Click(object sender, EventArgs e)
        {
            Boolean resp = false;
            ValorizzaPratica();

            resp = mn.UpdGestionePratica(pratica);

            if (resp)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Modifica della pratica effettuato." + "'); $('#errorModal').modal('show');", true);
                Pulisci();
            }
        }
    }
}