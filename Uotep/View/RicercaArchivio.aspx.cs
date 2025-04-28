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
using Page = System.Web.UI.Page;


namespace Uotep
{
    public partial class RicercaArchivio : Page
    {
        public string argomentoPassato = string.Empty;
        public String Filename = ConfigurationManager.AppSettings["CartellaFileArchivio"];
        protected void Page_Load(object sender, EventArgs e)
        {



            // Legge il valore dal Web.config
            string protocolloText = ConfigurationManager.AppSettings["TitoloArchivioUote"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            ProtocolloLiteral.Text = decodedText;


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
                switch (HfPratica.Value)
                {
                    case "Pratica":
                        ListRicerca.Add("Pratica");
                        ListRicerca.Add(txtPratica.Text);

                        break;
                    case "StoricoPratica":
                        ListRicerca.Add("StoricoPratica");
                        ListRicerca.Add(txtPratica.Text);
                        break;

                }
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
            argomentoPassato = clickedButton.CommandArgument;
            HfPratica.Value = argomentoPassato;
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
            string[] ar = new string[9];
            //Boolean ckevasa, Boolean ck1089, Boolean cksp, Boolean ckvincoli, Boolean ckdemolita, Boolean ckpp, Boolean ckpc, Boolean ckpbc, Boolean ckae)

            ar[0] = CkEvasa.Checked ? "evasa" : string.Empty;


            ar[1] = Ck1089.Checked ? "1089" : string.Empty; ;
            ar[2] = CkSuoloPubblico.Checked ? "sp" : string.Empty; ;
            ar[3] = CkVincoli.Checked ? "vincoli" : string.Empty; ;
            ar[4] = CkDemolita.Checked ? "demolita" : string.Empty; ;
            ar[5] = CkPropPriv.Checked ? "pp" : string.Empty; ;
            ar[6] = CkPropComunale.Checked ? "pc" : string.Empty; ;
            ar[7] = CkPropBeniCult.Checked ? "bc" : string.Empty; ;
            ar[8] = CkPropAltri.Checked ? "altri" : string.Empty; ;

            DataTable dt = mn.getArchivioUoteParziale(ar);

            string tempFilePath = System.IO.Path.GetTempFileName(); // Ottieni un nome di file temporaneo univoco
            tempFilePath = System.IO.Path.ChangeExtension(tempFilePath, ".xlsx"); // Cambia l'estensione in .xlsx

            // 2. Esporta la DataTable in Excel
            //string filePath = Path.Combine(Filename, "Estrazione Parziale " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".xlsx");
            string filePath = "Estrazione Parziale " + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".xlsx";
            string temp = System.IO.Path.GetTempPath() + @"\" + filePath;

            Session["filetemp"] = temp;
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Dati");
                //wb.Worksheets.Add(dt, "Dati");  // Crea un foglio Excel con i dati
                ws.Column(1).Delete(); // Elimina la prima colonna
                ws.Columns().AdjustToContents();  //  Auto-fit delle colonne
                                                  //
                Routine al = new Routine();
                al.ConvertiBooleaniInItaliano(ws);

                wb.SaveAs(filePath);  // Salva il file

                string contentType = MimeMapping.GetMimeMapping(filePath);

                byte[] fileBytes = File.ReadAllBytes(filePath);


                // File.Move(tempFilePath, temp);
                try
                {
                    // *** 5. Prepara la risposta HTTP per il download ***
                    Response.Clear();
                    Response.ContentType = contentType; // Imposta il Content-Type corretto (es. application/vnd.openxmlformats-officedocument.spreadsheetml.sheet per .xlsx)
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filePath); // Header Content-Disposition per forzare il download
                    Response.BinaryWrite(fileBytes); // Scrivi i byte del file nel flusso di output
                    Response.Flush();
                    //Response.End();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();



                    // Process.Start(temp);
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