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

        private void ValorizzaPratica()
        {



            pratica.fascicolo = txtFascicolo.Text;
            pratica.assegnato = txtAssegnato.Text;
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
        /// <summary>
        /// funzione che inserisce spaces al posto del min data value
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        protected string FormatMyDate(object dateValue)
        {
            if (dateValue == null || dateValue == DBNull.Value)
            {
                return ""; 
            }

            DateTime date;
            if (DateTime.TryParse(dateValue.ToString(), out date))
            {
                if (date == DateTime.MinValue)
                {
                    return ""; // O " " se vuoi uno spazio fisico
                }
                return date.ToString("dd/MM/yyyy");
            }
            return ""; // Gestione di valori non validi
        }
        private void Pulisci()
        {
            txtFascicolo.Text = string.Empty;
            txtDataRientro.Text = string.Empty;
            TxtDataUscita.Text = string.Empty;
            txtDataSpostamento.Text = string.Empty;
            txtNota.Text = string.Empty;
            txtDataRiscontro.Text = string.Empty;
            txtAssegnato.Text = string.Empty;
        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id_gestionePratica").ToString();

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

                string id_Fascicolo = args[0];

                Manager mn = new Manager();

                DataTable scheda = mn.getGestionePraticaById(id_Fascicolo);
                if (scheda.Rows.Count > 0)
                {
                    FillScheda(scheda);

                }
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }
        protected void FillScheda(DataTable fascicolo)
        {
            txtFascicolo.Text = fascicolo.Rows[0].ItemArray[1].ToString();
            txtAssegnato.Text = fascicolo.Rows[0].ItemArray[2].ToString();

            DateTime datauscita = System.Convert.ToDateTime(fascicolo.Rows[0].ItemArray[3].ToString()); // Recupera la data dal DataTable
            TxtDataUscita.Text = datauscita.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox

            DateTime dataRientro = new DateTime();
            if (!String.IsNullOrEmpty(fascicolo.Rows[0].ItemArray[4].ToString()))
            {
                dataRientro = System.Convert.ToDateTime(fascicolo.Rows[0].ItemArray[4].ToString()); // Recupera la data dal DataTable
                if (dataRientro == DateTime.MinValue)
                    txtDataRientro.Text = string.Empty;
                else
                    txtDataRientro.Text = dataRientro.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            }
            else
            {
                if (dataRientro == DateTime.MinValue)
                    txtDataRientro.Text = string.Empty;
            }


            DateTime dataSpostamento = new DateTime();
            if (!String.IsNullOrEmpty(fascicolo.Rows[0].ItemArray[5].ToString()))
            {
                dataSpostamento = System.Convert.ToDateTime(fascicolo.Rows[0].ItemArray[5].ToString()); // Recupera la data dal DataTable
                if (dataSpostamento == DateTime.MinValue)

                    txtDataSpostamento.Text = string.Empty;
                else
                    txtDataSpostamento.Text = dataSpostamento.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            }
            else
            {
                if (dataSpostamento == DateTime.MinValue)

                    txtDataSpostamento.Text = string.Empty;
            }
            DateTime dataRiscontro = new DateTime();
            if (!String.IsNullOrEmpty(fascicolo.Rows[0].ItemArray[6].ToString()))
            {
                dataRiscontro = System.Convert.ToDateTime(fascicolo.Rows[0].ItemArray[6].ToString()); // Recupera la data dal DataTable
                if (dataRiscontro == DateTime.MinValue)
                    txtDataRiscontro.Text = string.Empty;
                else
                    txtDataRiscontro.Text = dataRiscontro.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            }
            else
            {
                    txtDataRiscontro.Text = string.Empty;
            }


            txtNota.Text = fascicolo.Rows[0].ItemArray[7].ToString();

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
                    GVRicecaFascicolo.DataSource = dt;
                    GVRicecaFascicolo.DataBind();

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