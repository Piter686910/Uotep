using AjaxControlToolkit.HtmlEditor.Popups;
using Microsoft.Ajax.Utilities;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uote;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;
using System.Reflection;



namespace Uotep
{
    public partial class GestioneAuto : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String ruolo = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        

        protected void Page_Load(object sender, EventArgs e)
        {

            Session["PaginaChiamante"] = "~/View/GestioneAuto.aspx";

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Hfuser.Value = Session["ruolo"].ToString();
                ruolo = Session["ruolo"].ToString();

            }
            else
            {
                string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true");
                Response.Redirect(url);

            }

            if (!IsPostBack)
            {

                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                ProtocolloLiteral.Text = decodedText;
                CaricaDLL();
                txtMese.Text = DateTime.Now.ToString("MMMM").ToUpper();
                txtAnno.Text = DateTime.Now.ToString("yyyy");
                TxtData.Text = DateTime.Now.ToShortDateString();// ToString("yyyy");
                if (ruolo == Enumerate.Ruolo.accertatori.ToString())
                {
                    btCerca.Visible = false;
                    btStampa.Visible = false;
                }
                else
                {
                    txtAnno.Enabled = true;
                    txtMese.Enabled = true;
                    divInserimento.Visible = false;
                    btSalva.Visible = false;
                }
            }

        }

        protected void btChiudiAvvertenze_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalAvvertenze')); modal.hide();", true);

        }
        public float ConvertiStringaInFloat(string inputStringa)
        {
            float valoreConvertito = 0f;
            CultureInfo culturaItaliana = new CultureInfo("it-IT");
            NumberStyles stile = NumberStyles.Currency;

            if (float.TryParse(
                    inputStringa,
                    stile,
                    culturaItaliana,
                    out valoreConvertito))
            {
                return valoreConvertito;
            }
            else
            {
                throw new FormatException($"La stringa '{inputStringa}' non è un formato numerico valido per float.");
            }
        }
        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
                GestAuto auto = new GestAuto();
                auto.sigla = DdlSigla.SelectedItem.Text;
                auto.targa = txtTarga.Text;
                auto.data = System.Convert.ToDateTime(TxtData.Text);
                auto.ora = System.Convert.ToDateTime(txtOra.Text);
                auto.stan = txtStan.Text;
                auto.litri = ConvertiStringaInFloat(txtLitri.Text);
                auto.tipoCarburante = DdlCarburante.SelectedItem.Text;
                auto.euro = ConvertiStringaInFloat(txtEuro.Text);
                auto.indirizzo = txtIndirizzo.Text.ToUpper();
                auto.autista = txtAutista.Text.ToUpper();
                auto.mese = txtMese.Text;
                auto.anno = System.Convert.ToInt16(txtAnno.Text);





                Manager mn = new Manager();
                Boolean ins = mn.InsGestioneAuto(auto);
                if (!ins)
                {


                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento non riuscito" + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#Message').text('" + "Sigla " + auto.sigla + " inserita correttamente" + "'); $('#ModalRicDecretazione').modal('show');", true);

                    Pulisci();

                }
            }
            catch (Exception ex)
            {

                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/GestioneAuto.aspx";
            }
        }
        private void Pulisci()
        {
            txtTarga.Text = string.Empty;
            TxtData.Text = string.Empty;
            txtOra.Text = string.Empty;
            txtStan.Text = string.Empty;
            txtLitri.Text = string.Empty;
            DdlCarburante.ClearSelection();

            txtEuro.Text = string.Empty;
            txtIndirizzo.Text = string.Empty;
            txtAutista.Text = string.Empty;
            DdlSigla.ClearSelection();


        }



        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalQuartiere').modal('show');", true);
        }

        private void CaricaDLL()
        {
            try
            {
                Manager mn = new Manager();
                DataTable Sigla = mn.getSiglaAuto();
                DdlSigla.DataSource = Sigla; // Imposta il DataSource della DropDownList
                DdlSigla.DataTextField = "sigla"; // Il campo visibile
                DdlSigla.DataValueField = "ID_sigla"; // Il valore associato a ogni opzione
                DdlSigla.DataBind();
                DdlSigla.Items.Insert(0, new ListItem("", "0"));

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
                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);

                // Response.Redirect("/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/GestioneAuto.aspx";
                //  Response.Redirect("~/Contact.aspx");

            }
        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Ottieni il valore della colonna "ID"
            //    string id = DataBinder.Eval(e.Row.DataItem, "ID_quartiere").ToString();

            //    // Aggiungi l'attributo per il doppio clic
            //    e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
            //    e.Row.Style["cursor"] = "pointer";
            //}
        }
        protected void gvPopup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Select")
            //{
            //    // Ottieni il valore dell'ID dalla CommandArgument
            //    string selectedValue = e.CommandArgument.ToString();

            //    // Imposta il valore nel TextBox
            //    //txtSelectedValue.Text = selectedValue;
            //    txtQuartiere.Text = selectedValue;
            //    // Chiudi il popup
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            //}
        }


        protected void DdlSigla_SelectedIndexChanged(object sender, EventArgs e)
        {
            //    CaricaDLL();

        }


        protected void btSalvaGiudice_Click(object sender, EventArgs e)
        {
            Salva_Click(sender, e); ;
        }


        protected void DdlSigla_TextChanged(object sender, EventArgs e)
        {


        }



        protected void apripopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDecretazione').modal('show');", true);

        }
        protected void chiudipopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalDecretazione')); modal.hide();", true);
            // Pulisci();
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
                if (date == new DateTime(1900, 1, 1) || date == new DateTime(1, 1, 1))
                {
                    return ""; // O " " se vuoi uno spazio fisico
                }
                return date.ToString("dd/MM/yyyy");
            }
            return ""; // Gestione di valori non validi
        }


        protected void btStampa_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable scheda = mn.getListAuto(txtMese.Text, Convert.ToInt32(txtAnno.Text));
           
            Routine stampa = new Routine();
            stampa.CreaPdfSchedaCarburante(scheda);
           // stampa.CreaPdfLetteraAccompagnamento(scheda, PathLetteraAccompagnamento, "LetteraAccompagnamento.pdf");
        }

        protected void DdlSigla_SelectedIndexChanged1(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable Sigla = mn.getAutoBySigla(DdlSigla.SelectedItem.Text);
            if (Sigla.Rows.Count > 0)
            {
                txtTarga.Text = Sigla.Rows[0]["targa"].ToString();
            }
        }

        protected void btCerca_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable Sigla = mn.getListAuto(txtMese.Text, Convert.ToInt32(txtAnno.Text));


            if (Sigla.Rows.Count > 0)
            {

                gvDett.DataSource = Sigla;
                gvDett.DataBind();

                DivGrid.Visible = true;
            }
        }

        protected void gvDett_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void gvDett_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvDett_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}