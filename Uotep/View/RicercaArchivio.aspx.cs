using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Uotep.Classi;
using WebGrease.Activities;
using static Uotep.Classi.Enumerate;
using Page = System.Web.UI.Page;


namespace Uotep
{
    public partial class RicercaArchivio : Page
    {
        public string argomentoPassato = string.Empty;
        public String Filename = ConfigurationManager.AppSettings["CartellaFileArchivio"];
        String Vuser = String.Empty;
        String Ruolo = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PaginaChiamante"] = "~/View/RicercaArchivio.aspx";
           
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Ruolo = Session["ruolo"].ToString();

            }

            if (!IsPostBack)
            {
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["TitoloArchivioUote"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                ProtocolloLiteral.Text = decodedText;
            }
            if (Ruolo.ToUpper() != Enumerate.Ruolo.Archivio.ToString().ToUpper() && Ruolo.ToUpper() != Enumerate.Ruolo.Admin.ToString().ToUpper() && Ruolo.ToUpper() != Enumerate.Ruolo.SuperAdmin.ToString().ToUpper())
            {
                btEstraiParziale.Enabled = false;
                btEstraiTotale.Enabled = false;
            }
        }


        private void Pulisci()
        {

            txtPratica.Text = String.Empty;
            txtSub.Text = string.Empty;
            txtParticella.Text = string.Empty;
            txtFoglio.Text = string.Empty;
            txtSez.Text = string.Empty;
            txtIndirizzo.Text = string.Empty;
            txtResponsabile.Text = String.Empty;
            Ck1089.Checked = false;
            CkDemolita.Checked = false;
            CkEvasa.Checked = false;
            CkPropAltri.Checked = false;
            CkPropBeniCult.Checked = false;
            CkPropComunale.Checked = false;
            CkPropPriv.Checked = false;
            CkSuoloPubblico.Checked = false;
            CkVincoli.Checked = false;
            txtRicNota.Text = string.Empty;
            txtAnno.Text = string.Empty;
            txtMese.Text = string.Empty;
            CkBeniConfiscati.Checked = false;


        }


        protected void Ricerca_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRicNota.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Note", txtRicNota.Text };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;

            }
            if (!string.IsNullOrEmpty(txtResponsabile.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Nominativo", txtResponsabile.Text };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;

            }
            if (!string.IsNullOrEmpty(txtPratica.Text))
            {
                List<string> ListRicerca = new List<string>();
                // Crea una lista 
                if (ckStorico.Checked)
                {
                    ListRicerca.Add("StoricoPratica");
                    ListRicerca.Add(txtPratica.Text);
                }
                else
                {

                    ListRicerca.Add("Pratica");
                    if (ckDoppioni.Checked)
                    {
                        ListRicerca.Add("Doppione");
                    }
                    ListRicerca.Add(txtPratica.Text);
                }
                //switch (HfPratica.Value)
                //{
                //    case "Pratica":
                //        ListRicerca.Add("Pratica");
                //        ListRicerca.Add(txtPratica.Text);

                //        break;
                //    case "StoricoPratica":
                //        ListRicerca.Add("StoricoPratica");
                //        ListRicerca.Add(txtPratica.Text);
                //        break;

                //}
                // List<string> ListRicerca = new List<string> { "Pratica", txtPratica.Text };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;
            }
            if (!string.IsNullOrEmpty(txtIndirizzo.Text))
            {
                // Crea una lista
                List<string> ListRicerca = new List<string> { "Indirizzo", txtIndirizzo.Text };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;
            }
            if (!string.IsNullOrEmpty(txtSez.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Catasto", txtSez.Text, txtFoglio.Text, txtParticella.Text, txtSub.Text };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;

            }
            if (!string.IsNullOrEmpty(txtAnno.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "AnnoMese", txtAnno.Text, txtMese.Text };

                // Salva la lista nella Sessione
                Session["ListRicerca"] = ListRicerca;

            }

            // Reindirizza alla pagina di destinazione
            Response.Redirect("InserimentoArchivio.aspx");
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);
            Pulisci();
        }
        protected void chiudipopupErrore_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('errorModal')); modal.hide();", true);

        }

        protected void btDatiCatastali_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = false;
            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = true;
            DivPratica.Visible = false;
            DivEstraiDb.Visible = false;
            DivNote.Visible = false;
            DivAnnoMese.Visible = false;
            DivRicerca.Visible = true;
        }

        protected void btIndirizzo_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = false;
            DivIndirizzo.Visible = true;
            DivDatiCatastali.Visible = false;
            DivPratica.Visible = false;
            DivEstraiDb.Visible = false;
            DivNote.Visible = false;
            DivAnnoMese.Visible = false;
            DivRicerca.Visible = true;
        }

        protected void btNominativo_Click(object sender, EventArgs e)
        {

            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = false;
            DivPratica.Visible = false;
            DivEstraiDb.Visible = false;
            DivNote.Visible = false;
            DivAnnoMese.Visible = false;
            DivRicerca.Visible = true;
            DivNominativo.Visible = true;
        }

        protected void btNpratica_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Button clickedButton = (System.Web.UI.WebControls.Button)sender;
            //argomentoPassato = clickedButton.CommandArgument;

            DivNominativo.Visible = false;
            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = false;
            DivEstraiDb.Visible = false;
            DivNote.Visible = false;
            DivAnnoMese.Visible = false;
            DivPratica.Visible = true;
            DivRicerca.Visible = true;
        }
        protected void btNota_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = false;
            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = false;
            DivEstraiDb.Visible = false;
            DivPratica.Visible = false;
            DivRicerca.Visible = false;
            DivAnnoMese.Visible = false;
            DivRicerca.Visible = true;
            DivNote.Visible = true;
        }
        protected void btAnnoMese_Click(object sender, EventArgs e)
        {
            DivNominativo.Visible = false;
            DivIndirizzo.Visible = false;
            DivDatiCatastali.Visible = false;
            DivEstraiDb.Visible = false;
            DivPratica.Visible = false;
            DivRicerca.Visible = false;
            DivRicerca.Visible = true;
            DivNote.Visible = false;
            DivAnnoMese.Visible = true;
        }

        protected void btEstraiParziale_Click(object sender, EventArgs e)
        {
            DivEstraiDb.Visible = true;
            DivRicerca.Visible = false;
        }

        protected void Estrai_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            string[] ar = new string[10];
            //Boolean ckevasa, Boolean ck1089, Boolean cksp, Boolean ckvincoli, Boolean ckdemolita, Boolean ckpp, Boolean ckpc, Boolean ckpbc, Boolean ckae, boolean ckbeniconf)

            ar[0] = CkEvasa.Checked ? "evasa" : string.Empty;


            ar[1] = Ck1089.Checked ? "1089" : string.Empty; ;
            ar[2] = CkSuoloPubblico.Checked ? "sp" : string.Empty; ;
            ar[3] = CkVincoli.Checked ? "vincoli" : string.Empty; ;
            ar[4] = CkDemolita.Checked ? "demolita" : string.Empty; ;
            ar[5] = CkPropPriv.Checked ? "pp" : string.Empty; ;
            ar[6] = CkPropComunale.Checked ? "pc" : string.Empty; ;
            ar[7] = CkPropBeniCult.Checked ? "bc" : string.Empty; ;
            ar[8] = CkPropAltri.Checked ? "altri" : string.Empty; ;
            ar[9] = CkBeniConfiscati.Checked ? "confiscati" : string.Empty; 

            DataTable dt = mn.getArchivioUoteParziale(ar);

            string tempFilePath = System.IO.Path.GetTempFileName(); // Ottieni un nome di file temporaneo univoco
            tempFilePath = System.IO.Path.ChangeExtension(tempFilePath, ".xlsx"); // Cambia l'estensione in .xlsx

            // 2. Esporta la DataTable in Excel
            //string filePath = Path.Combine(Filename, "Estrazione Parziale " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".xlsx");
            string filePath = "Estrazione Parziale " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".xlsx";
            string temp = System.IO.Path.GetTempPath() + @"\" + filePath;

            Session["filetemp"] = temp;
            using (var workbook = new XLWorkbook()) // Il 'using' garantisce che il workbook sia gestito correttamente
            // Il 'using' garantisce che il MemoryStream sia chiuso
            {
                var worksheet = workbook.Worksheets.Add(dt, "Dati"); // Aggiunge un foglio di lavoro
                                                                     //worksheet.Cell(1, 1).InsertTable(dt); // Inserisce il DataTable nel foglio a partire dalla cella A1
                                                                     //worksheet.Column(1).Delete(); // Elimina la prima colonna
                worksheet.Columns().AdjustToContents();  //  Auto-fit delle colonne
                                                         //
                Routine al = new Routine();
                al.ConvertiBooleaniInItaliano(worksheet);
               

                worksheet.Cell("A1").Value = "Nome";
                worksheet.Cell("B1").Value = "Età";
                worksheet.Cell("A2").Value = "Mario";
                worksheet.Cell("B2").Value = 35;

                //workbook.SaveAs("EsportazioneData.xlsx");
                // Opzionale: Formatta l'intestazione
                // worksheet.Row(1).Style.Font.Bold = true;

                // Opzionale: Adatta le colonne al contenuto
                // worksheet.Columns().AdjustToContents();

                // Salva il workbook nel MemoryStream
                // workbook.SaveAs(memoryStream);

                // *** Prepara la risposta HTTP per il download ***
                // Importante: posiziona il puntatore del MemoryStream all'inizio prima di leggerlo
                // memoryStream.Seek(0, SeekOrigin.Begin);

                // Ottieni i byte dal MemoryStream
                // byte[] excelBytes = memoryStream.ToArray();

                // Imposta il nome del file per il download
                string fileNameForDownload = "EsportazioneDati_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; // MIME type standard per .xlsx

                try
                {
                    //Response.Clear(); // Pulisci la risposta corrente
                    //Response.ClearHeaders();
                    //Response.ClearContent();
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.ContentType = contentType; // Imposta il Content-Type corretto
                    //Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileNameForDownload + "\""); // Header per il download (con virgolette per nomi con spazi)
                    //Response.AppendHeader("Content-Encoding", "identity");
                    // *** FONDAMENTALE: Sopprimi il rendering standard della pagina ***
                    // Questo impedisce ad ASP.NET di scrivere contenuto HTML dopo i byte del file
                    //Response.SuppressContent = true;
                    //if (excelBytes == null || excelBytes.Length == 0)
                    //{
                    //    Trace.Write("ERROR DOWNLOAD: excelBytes is null or empty after generation.");
                    //    Response.Write("Errore: File generato vuoto.");
                    //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //    return;
                    //}
                    //else
                    //{
                    //    Trace.Write($"DEBUG DOWNLOAD: excelBytes length BEFORE BinaryWrite: {excelBytes.Length}");
                    //}
                    using (var memoryStream = new MemoryStream())
                    {
                        // *** Scrivi i byte del file nel flusso di output della risposta ***
                        //Response.BinaryWrite(excelBytes);

                        // *** Completa la richiesta ***
                        // Response.Flush(); // Forza l'invio immediato del buffer al client
                        // Response.End(); // NON usare Response.End()
                        workbook.SaveAs(memoryStream);
                    
                        byte[] content = memoryStream.ToArray();
                        Response.Clear(); // Pulisci la risposta corrente
                        Response.ClearHeaders();
                        Response.ClearContent();

                        Response.ContentType = contentType; // Imposta il Content-Type corretto
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileNameForDownload + "\""); // Header per il download (con virgolette per nomi con spazi)
                        Response.BinaryWrite(content);


                        // Scrivi i byte dello stream nella risposta
                        // memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        //Response.End();

                        HttpContext.Current.ApplicationInstance.CompleteRequest(); // Termina il ciclo di vita della richiesta in modo pulito
                    }
                }
                catch (Exception ex)
                {
                    // Gestisci eventuali errori durante il processo di download
                    Response.Clear(); // Pulisci la risposta in caso di errore
                    Response.ContentType = "text/plain"; // Imposta ContentType per un messaggio di testo
                    Response.Write("Errore durante il download del file: " + ex.Message);
                    // Log dell'errore per debugging
                    // System.Diagnostics.Trace.WriteLine("Errore download file Excel ClosedXML via MemoryStream: " + ex.ToString());
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Termina anche in caso di errore
                }

            }
            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    var ws = wb.Worksheets.Add(dt, "Dati");
            //    ws.Cell(1, 1).InsertTable(dt);
            //    //wb.Worksheets.Add(dt, "Dati");  // Crea un foglio Excel con i dati
            //    ws.Column(1).Delete(); // Elimina la prima colonna
            //    ws.Columns().AdjustToContents();  //  Auto-fit delle colonne
            //                                      //
            //    Routine al = new Routine();
            //    al.ConvertiBooleaniInItaliano(ws);

            //    wb.SaveAs(temp);  // Salva il file

            //    string contentType = MimeMapping.GetMimeMapping(temp);

            //    byte[] fileBytes = File.ReadAllBytes(temp);


            //    //File.Move(tempFilePath, temp);
            //    try
            //    {
            //        // *** 5. Prepara la risposta HTTP per il download ***
            //        Response.Clear();
            //        Response.ContentType = contentType; // Imposta il Content-Type corretto (es. application/vnd.openxmlformats-officedocument.spreadsheetml.sheet per .xlsx)
            //        Response.AddHeader("Content-Disposition", "attachment; filename=" + temp); // Header Content-Disposition per forzare il download
            //        // *** PASSO FONDAMENTALE: Sopprimi il rendering standard della pagina ***
            //        Response.SuppressContent = true;
            //        Response.BinaryWrite(fileBytes); // Scrivi i byte del file nel flusso di output
            //        Response.Flush();
            //        //Response.End();
            //        HttpContext.Current.ApplicationInstance.CompleteRequest();



            //        // Process.Start(temp);
            //        //File.Delete(tempFilePath);
            //        //tempFilePath = System.IO.Path.ChangeExtension(tempFilePath, ".tmp"); // Cambia l'estensione in .temp
            //        //File.Delete(tempFilePath);

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Errore nell'apertura del file temporaneo: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }

            //}
        }

        protected void btEstraiTotale_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = mn.getArchivioUoteTotale();
            //string tempFilePath = System.IO.Path.GetTempFileName(); // Ottieni un nome di file temporaneo univoco
            //tempFilePath = System.IO.Path.ChangeExtension(tempFilePath, ".xlsx"); // Cambia l'estensione in .xlsx
            // 2. Esporta la DataTable in Excel
            // string filePath = Path.Combine(Filename, "Estrazione del " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".xlsx");
            string filePath = "Estrazione del " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".xlsx";
            //string temp = System.IO.Path.GetTempPath() + @"\" + filePath;
            Session["filetemp"] = filePath;
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Dati");
                //wb.Worksheets.Add(dt, "Dati");  // Crea un foglio Excel con i dati
                ws.Column(1).Delete(); // Elimina la prima colonna
                ws.Columns().AdjustToContents();  //  Auto-fit delle colonne
                Routine al = new Routine();
                al.ConvertiBooleaniInItaliano(ws);
                wb.SaveAs(filePath);  // Salva il file
                string contentType = MimeMapping.GetMimeMapping(filePath);

                byte[] fileBytes = File.ReadAllBytes(filePath);
                //File.Move(tempFilePath, temp);
                try
                {
                    Response.Clear();
                    Response.ContentType = contentType; // Imposta il Content-Type corretto (es. application/vnd.openxmlformats-officedocument.spreadsheetml.sheet per .xlsx)
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filePath); // Header Content-Disposition per forzare il download
                                                                                                   // *** PASSO FONDAMENTALE: Sopprimi il rendering standard della pagina ***
                    Response.SuppressContent = true;
                    Response.BinaryWrite(fileBytes); // Scrivi i byte del file nel flusso di output
                    Response.Flush();
                    //Response.End();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                    //Process.Start(temp);
                    //File.Delete(tempFilePath);
                    //tempFilePath = System.IO.Path.ChangeExtension(tempFilePath, ".tmp"); // Cambia l'estensione in .temp
                    //File.Delete(tempFilePath);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore nell'apertura del file temporaneo: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

    }
}