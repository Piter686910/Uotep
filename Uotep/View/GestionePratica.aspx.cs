using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using iText.StyledXmlParser.Jsoup.Nodes;
using System;
using System.Collections.Generic;
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
        public String Filename = ConfigurationManager.AppSettings["CartellaFileArchivio"];

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
            Session["PaginaChiamante"] = "~/View/GestionePratica.aspx";
            if (!IsPostBack)
            {
                if (ruolo.ToUpper() == Enumerate.Ruolo.accertatori.ToString().ToUpper())
                {
                    btSalva.Enabled = false;
                    btModifica.Enabled = false;
                }
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
            }
            if (e.CommandName == "Delete")
            {
                // Ottieni il valore dell'ID dalla CommandArgument
                //string selectedValue = e.CommandArgument.ToString();

                string[] args = e.CommandArgument.ToString().Split(';');

                string id_Fascicolo = args[0];
                HfIdFascicolo.Value = id_Fascicolo;


            }
            // Chiudi il popup
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);


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

        protected void GVRicercaFascicolo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //int idFascicolo = Convert.ToInt32(GVRicercaFascicolo.DataKeys[e.RowIndex].Value);
            //HfIdFascicolo.Value = Convert.ToString(idFascicolo);
            Manager mn = new Manager();

            Boolean resp = mn.DeleteGestionePraticaById(HfIdFascicolo.Value);
            if (resp)
            {
                GestionePratiche pratica = new GestionePratiche();
                pratica.fascicolo = txtFascicolo.Text;
                getPratica(pratica);

            }
        }
        /// <summary>
        /// Estrai la tabella GestionePratica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtEstraiTabella_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = mn.getGestionePraticaTotale();
            //string tempFilePath = System.IO.Path.GetTempFileName(); // Ottieni un nome di file temporaneo univoco
            //tempFilePath = System.IO.Path.ChangeExtension(tempFilePath, ".xlsx"); // Cambia l'estensione in .xlsx
            // 2. Esporta la DataTable in Excel
            // string filePath = Path.Combine(Filename, "Estrazione del " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".xlsx");
            string filePath = "Estrazione Gestione Pratica del " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".xlsx";
            //string temp = System.IO.Path.GetTempPath() + @"\" + filePath;
            Session["filetemp"] = filePath;
            using (XLWorkbook wb = new XLWorkbook())
            {
                var worksheet = wb.Worksheets.Add("Dati Estratti"); // Aggiunge un foglio di lavoro
                                                                    //worksheet.Cell(1, 1).InsertTable(dt); // Inserisce il DataTable nel foglio a partire dalla cella A1
                var columnRenameMap = new Dictionary<string, string>
        {
            { "fascicolo", "Fascicolo" },
            { "assegnato", "Assegnato" },
            { "DATA_USCITA", "Data Uscita" },
            { "DATA_rientro", "Data Rientro" },
            { "DATA_SPOSTAMENTI", "Data Spostamenti" },
            { "data_riscontro", "Data Riscontro " },
            { "note", "Note" },
            { "NOTA_SPOSTAMENTO", "Nota Spostamento" },
            { "NOTA_riscontro", "NOTA Riscontro" },

        };

                foreach (var entry in columnRenameMap)
                {
                    if (dt.Columns.Contains(entry.Key))
                    {
                        dt.Columns[entry.Key].ColumnName = entry.Value;
                    }
                }
                worksheet.Cell(1, 1).InsertTable(dt);
                worksheet.Column(1).Delete(); // Elimina la prima colonna
                worksheet.Columns().AdjustToContents();  //  Auto-fit delle colonne
                worksheet.Range(1, 1, 1, dt.Columns.Count).Style.Font.Bold = true;
                //Routine al = new Routine();
                //al.ConvertiBooleaniInItaliano(worksheet);
                string fileNameForDownload = Filename + @"\\Estrazione Gestione Pratica_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                wb.SaveAs(fileNameForDownload);

            }
        }
    }
}