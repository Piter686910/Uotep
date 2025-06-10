using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
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
            else
            {
                Response.Redirect("Default.aspx?user=true");
            }
            if (!IsPostBack)
            {
                //  ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);

            }

        }

        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);

        }
        private Boolean VerificaCorrettezzaInserimento(GestionePratiche pratica)
        {
            Boolean resp = false;
            DateTime appo = new DateTime();
            Manager mn = new Manager();
            DataTable dt = new DataTable();
            dt = mn.getGestionePraticaByFascicolo(pratica.fascicolo);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    appo = System.Convert.ToDateTime(dt.Rows[i].ItemArray[4].ToString());
                    if (appo == DateTime.MinValue || String.IsNullOrEmpty(dt.Rows[i].ItemArray[4].ToString()))
                    {
                        resp = true;
                        break;
                    }
                }
            }
            return resp;
        }
        private void ValorizzaPratica()
        {

            pratica.fascicolo = txtFascicolo.Text;
            pratica.assegnato = txtAssegnato.Text;
            pratica.note = txtNota.Text;
            pratica.data_uscita = System.Convert.ToDateTime(TxtDataUscita.Text);
            pratica.data_rientro = txtDataRientro.Text;
            pratica.data_spostamenti = txtDataSpostamento.Text;
            pratica.DATA_RISCONTRO = txtDataRiscontro.Text;
            pratica.notaSpostamento = txtNotaSpostamento.Text;
            pratica.notariscontro = txtNotaRiscontro.Text;
        }

        protected void btInserisci_Click(object sender, EventArgs e)
        {

            Boolean resp = false;
            ValorizzaPratica();
            //resp = VerificaCorrettezzaInserimento(pratica);
            //if (!resp)
            //{
            resp = mn.InsGestionePratica(pratica);
            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Non è possibile inserire una nuova riga se la data rientro della precedente non è valorizzata." + "'); $('#errorModal').modal('show');", true);
            //    resp = false;
            //}
            if (resp)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica effettuato." + "'); $('#errorModal').modal('show');", true);
                Pulisci();
                getPratica(pratica);
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
            // txtFascicolo.Text = string.Empty;
            txtDataRientro.Text = string.Empty;
            TxtDataUscita.Text = string.Empty;
            txtDataSpostamento.Text = string.Empty;
            txtNota.Text = string.Empty;
            txtDataRiscontro.Text = string.Empty;
            txtAssegnato.Text = string.Empty;
            txtNotaSpostamento.Text = string.Empty;
            txtNotaRiscontro.Text = string.Empty;
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
                HfIdFascicolo.Value = id_Fascicolo;
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
            txtDataRientro.Text = fascicolo.Rows[0].ItemArray[4].ToString();
            txtDataSpostamento.Text = fascicolo.Rows[0].ItemArray[5].ToString();
            txtDataRiscontro.Text = fascicolo.Rows[0].ItemArray[6].ToString();
            txtNota.Text = fascicolo.Rows[0].ItemArray[7].ToString();
            txtNotaSpostamento.Text = fascicolo.Rows[0].ItemArray[8].ToString();
            txtNotaRiscontro.Text = fascicolo.Rows[0].ItemArray[9].ToString();

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
                getPratica(pratica);
            }
        }

        protected void btModifica_Click(object sender, EventArgs e)
        {
            Boolean resp = false;
            ValorizzaPratica();
            int idfascicolo = System.Convert.ToInt32(HfIdFascicolo.Value);

            resp = mn.UpdGestionePratica(idfascicolo, pratica);

            if (resp)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Modifica della pratica effettuato." + "'); $('#errorModal').modal('show');", true);
                //Pulisci();
                getPratica(pratica);


            }
        }
        private void getPratica(GestionePratiche p)
        {
            DataTable dt = new DataTable();
            dt = mn.getGestionePraticaByFascicolo(p.fascicolo.Trim());
            if (dt.Rows.Count > 0)
            {
                GVRicercaFascicolo.DataSource = dt;
                GVRicercaFascicolo.DataBind();

            }
        }
        
    }
}