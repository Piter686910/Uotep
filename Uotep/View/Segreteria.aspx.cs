using Microsoft.Office.Interop.Word;
using Microsoft.SqlServer.Server;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Uotep.Classi;

namespace Uotep
{
    public partial class Segreteria : System.Web.UI.Page
    {
        String Vuser = String.Empty;
        String CartellaSegreteria = ConfigurationManager.AppSettings["CartellaFileSegreteria"];
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

        String profilo = string.Empty;
        String ruolo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["user"] != null)
                {
                    Vuser = Session["user"].ToString();
                    profilo = Session["profilo"].ToString();
                    ruolo = Session["ruolo"].ToString();
                    if (ruolo.ToUpper() == Enumerate.Profilo.PG.ToString().ToUpper())
                    {
                        divSegreteria.Visible = true;
                        divCaricaFile.Visible = false;
                        btRicerca.Visible = true;
                        btCarica.Visible = false;
                        CaricaDLL();
                    }
                    else
                    {
                        divSegreteria.Visible = false;
                        divCaricaFile.Visible = true;
                        btRicerca.Visible = false;
                        btCarica.Visible = true;
                    }

                }
            }
        }

        private void CaricaDLL()
        {
            try
            {
                Manager mn = new Manager();
                System.Data.DataTable CaricaOperatori = mn.getListOperatore();
                DdlOperatore.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                DdlOperatore.DataTextField = "Nominativo"; // Il campo visibile
                //DdlPattuglia.DataValueField = "Id"; // Il valore associato a ogni opzione
                DdlOperatore.Items.Insert(0, new ListItem("", "0"));
                DdlOperatore.DataBind();
                DdlOperatore.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in carica ddl file Segreteria.cs ");
                    sw.Close();
                }
            }
        }




        protected void Carica_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            CaricaFile fl = new CaricaFile();
            Boolean ins = false;
            string filePath = string.Empty;
            fl.matricola = Session["user"].ToString();

            fl.fascicolo = TxtFascicolo.Text;
            fl.folder = CartellaSegreteria;

            string dataInserita = txtDataI.Text;
            string formatoData = "dd-MM-yyyy";
            DateTime dataValidata;

            if (DateTime.TryParseExact(dataInserita, formatoData, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataValidata))
            {
                // La data è valida e nel formato corretto
                //lbmess.Text = "Data valida: " + dataValidata.ToString("dd-MM-yyyy");

                //fl.data = System.Convert.ToDateTime(txtDataI.Text).ToShortDateString();
                if (FLFilein.HasFile)
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(FLFilein.FileName); // Nome senza estensio
                    string[] a = fileNameWithoutExt.Split('_');


                    bool salva = false;


                    string fileExtension = Path.GetExtension(FLFilein.FileName); // Estrai l'estensione
                    string newFileName = txtNrFascicolo.Text.Trim() + "_" + txtDataI.Text + fileExtension; // Assegna un nuovo nome

                    filePath = Path.Combine(CartellaSegreteria, newFileName); // Percorso completo

                    fl.fascicolo = txtNrFascicolo.Text.Trim();
                    fl.data = txtDataI.Text;
                    //filePath = CartellaSegreteria + FLFilein.FileName;

                    fl.nomefile = newFileName;
                    long maxSizeInBytes = 4 * 1024 * 1024; // 4MB in bytes
                    //FileInfo fileInfo = new FileInfo(FLFilein.FileBytes);
                    //long fileSizeInBytes = fileInfo.Length;
                    if (FLFilein.FileBytes.Length >= maxSizeInBytes)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Il file deve essere minore di 4 mega bytes" + "'); $('#errorModal').modal('show');", true);

                    }
                    else
                    {
                        if (!File.Exists(filePath))
                        {

                            FLFilein.SaveAs(filePath);
                            salva = true;

                            //File.SetAttributes(filePath, FileAttributes.ReadOnly);
                            //Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                            //Document doc = wordApp.Documents.Open(filePath);

                            //// Imposta il documento in modalità sola lettura senza password
                            //doc.Protect(WdProtectionType.wdAllowOnlyReading);
                        }
                        else
                        {
                            File.Delete(filePath);
                            Boolean resp = mn.DeleteFileCaricati(newFileName);
                            FLFilein.SaveAs(filePath);
                            salva = resp;
                        }

                    }
                    if (salva)
                        ins = mn.InsFile(fl);
                    if (ins)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "File " + fl.nomefile + " inserito correttamente." + "'); $('#myModal').modal('show');", true);

                    }
                    else
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Nomefile:" + newFileName + @" - Errore in carica ddl file Segreteria.cs ");
                            sw.Close();
                        }
                        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "File non inserito." + "'); $('#myModal').modal('show');", true);

                    }
                }

            }
            else
            {
                // La data non è valida o non è nel formato corretto
                lbmess.Text = "Data non valida. Inserisci la data nel formato dd-mm-yyyy.";

            }

        }

        protected void Ricerca_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            CaricaFile fl = new CaricaFile();
            string filePath = string.Empty;
            Boolean ric = false;

            if (!string.IsNullOrEmpty(TxtFascicolo.Text))
            {
                fl.fascicolo = TxtFascicolo.Text;
            }
            if (!string.IsNullOrEmpty(TxtData.Text))
            {
                fl.data = System.Convert.ToDateTime(TxtData.Text).ToShortDateString();
            }
            if (DdlOperatore.SelectedIndex != 0)
            {
                System.Data.DataTable operatore = mn.getMatricolaOperatore(DdlOperatore.SelectedItem.Text);

                if (operatore.Rows.Count > 0)
                {
                    fl.matricola = operatore.Rows[0].ItemArray[0].ToString().Trim();
                    ric = true;
                }


            }
            fl.folder = CartellaSegreteria;
            if (!ric)
            {
                System.Data.DataTable dt = mn.GetFileByFascicoloData(fl);
                if (dt.Rows.Count > 0)
                {
                    GVRicercaFile.DataSource = dt;
                    GVRicercaFile.DataBind();

                }
            }
            else
            {
                System.Data.DataTable dtO = mn.GetFileByOperatore(fl.matricola);
                if (dtO.Rows.Count > 0)
                {
                    GVRicercaFile.DataSource = dtO;
                    GVRicercaFile.DataBind();

                }
            }

        }

        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "fascicolo").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
        }
        protected void gvPopup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni l'indice della riga cliccata e altri argomenti
                string[] args = e.CommandArgument.ToString().Split(';');
                int rowIndex = Convert.ToInt32(args[0]);
                string numeroF = args[1]; // Potrebbe non essere usato nel percorso del file
                string data = args[2];    // Potrebbe non essere usato nel percorso del file
                string nomefile = args[3];
                string folder = args[4];   // Potrebbe non essere usato direttamente nel percorso, ma estratto dal codice originale
                int id_file = Convert.ToInt32(args[5]);
                GridViewRow row = GVRicercaFile.Rows[rowIndex];

                // Recupera il nome del file dalla riga (già estratto da CommandArgument come nomefile)
                string fileName = nomefile;

                // **Important Security Note:**
                // Accessing files directly from C:\FileSegreteria from a web application poses significant security risks.
                // Anyone who can access your web application might potentially access any file in that directory if you are not careful.
                // **For a production environment, strongly consider storing files outside the web application's root directory and implementing proper access control mechanisms.**
                //  A safer approach would be to use a dedicated file storage service or database to manage and serve files.

                string baseUrl = @"C:\FileSegreteria\"; // **Directly using C:\FileSegreteria as base path.**
                                                        // **Be extremely cautious with this approach in production.**


                // Costruisci l'URL del file usando Path.Combine per sicurezza
                string fileUrl = Path.Combine(baseUrl, fileName);

                if (File.Exists(fileUrl))
                {
                    try
                    {
                        // Get the file extension to determine content type
                        string fileExtension = Path.GetExtension(fileName).ToLower();
                        string contentType = GetContentType(fileExtension);

                        // **Option 1: Attempt to display in browser (for supported types) - Visualization**
                        if (contentType != "application/octet-stream") // If it's not a generic binary type (like .exe)
                        {
                            Response.ContentType = contentType;
                            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName); // "inline" tries to open in browser
                            Response.TransmitFile(fileUrl);
                            Response.Flush(); // Ensure headers and content are sent immediately
                            Response.End(); // Stop further page processing
                        }
                        else // **Option 2: Force Download for other types or if visualization is not desired - Download**
                        {
                            Response.ContentType = contentType;
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName); // "attachment" forces download
                            Response.TransmitFile(fileUrl);
                            Response.Flush();
                            Response.End();
                        }
                        Manager mn = new Manager();

                        bool upd = mn.UpdFileCaricati(id_file);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., file access errors)
                        Response.Write($"Errore nell'apertura del file: {ex.Message}");
                        // Log the error for debugging purposes
                        // Consider redirecting to an error page instead of just writing to the response in production.
                    }
                }
                else
                {
                    Response.Write($"File non trovato: {fileName} nel percorso {baseUrl}");
                }
            }

        }
        private string GetContentType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".txt":
                    return "text/plain";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".doc":
                case ".docx":
                    return "application/vnd.ms-word";
                case ".xls":
                case ".xlsx":
                    return "application/vnd.ms-excel";
                case ".ppt":
                case ".pptx":
                    return "application/vnd.ms-powerpoint";
                default:
                    return "application/octet-stream"; // Default to binary download for unknown types
            }
        }

        //protected void apripopup_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        //}
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

        }

        protected void btCancellaScaricati_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean upd = false;
            //controllo se ci sono file con flag cancella a true per essere cancellati
            System.Data.DataTable dt = mn.GetFileScaricati();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    File.Delete(dt.Rows[i].ItemArray[i].ToString());
                }

                upd = mn.DeleteFileScaricati();
                if (upd)
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "File cancellati." + "'); $('#myModal').modal('show');", true);


            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Non ci sono file da cancellare." + "'); $('#myModal').modal('show');", true);

            }


        }
    }
}