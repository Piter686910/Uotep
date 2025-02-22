using Microsoft.SqlServer.Server;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Uotep.Classi;

namespace Uotep
{
    public partial class Segreteria : Page
    {
        String Vuser = String.Empty;
        String CartellaSegreteria = ConfigurationManager.AppSettings["CartellaFileSegreteria"];
        String profilo = string.Empty;
        String ruolo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                profilo = Session["profilo"].ToString();
                ruolo = Session["ruolo"].ToString();
                if (ruolo == "segreteria")
                {
                    divSegreteria.Visible = true;
                    divCaricaFile.Visible = false;
                    btRicerca.Visible = true;
                    btCarica.Visible = false;
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

        protected void Reset_Click(object sender, EventArgs e)
        {
            //divNewUtente.Visible = false;
            //divDestra.Visible = false;
            //divReset.Visible = true;
        }


        protected void NuovoUt_Click(object sender, EventArgs e)
        {
            //    divNewUtente.Visible = true;
            //    divDestra.Visible = true;
            //    divReset.Visible = false;

        }


        protected void Carica_Click(object sender, EventArgs e)
        {
            CaricaFile fl = new CaricaFile();
            string filePath = string.Empty;
            fl.matricola = Vuser;

            fl.fascicolo = TxtFascicolo.Text;
            fl.folder = CartellaSegreteria;
            fl.data = System.Convert.ToDateTime(TxtData.Text).ToShortDateString();
            if (FLFilein.HasFile)
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(FLFilein.FileName); // Nome senza estensio
                string[] a = fileNameWithoutExt.Split('_');
                fl.fascicolo = a[0];
                fl.data = a[1];
                filePath = CartellaSegreteria + FLFilein.FileName;
                FLFilein.SaveAs(filePath);
            }
            Manager mn = new Manager();
            fl.nomefile = filePath;

            Boolean ins = mn.InsFile(fl);
            if (ins)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "File " + fl.nomefile + " inserito correttamente." + "'); $('#errorModal').modal('show');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "File non inserito." + "'); $('#errorModal').modal('show');", true);

            }
        }

        protected void Ricerca_Click(object sender, EventArgs e)
        {
            CaricaFile fl = new CaricaFile();
            string filePath = string.Empty;
            //fl.matricola = Vuser;
            if (!string.IsNullOrEmpty(TxtFascicolo.Text))
            {
                fl.fascicolo = TxtFascicolo.Text;
            }
            if (!string.IsNullOrEmpty(TxtData.Text))
            {
                fl.data = System.Convert.ToDateTime(TxtData.Text).ToShortDateString();
            }
            fl.folder = CartellaSegreteria;
            
            
            Manager mn = new Manager();
           

            DataTable dt = mn.GetFileByFascicoloData(fl);
            if (dt.Rows.Count > 0)
            {
                GVRicercaFile.DataSource = dt;
                GVRicercaFile.DataBind();
               


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
                // Ottieni l'indice della riga cliccata
                string[] args = e.CommandArgument.ToString().Split(';');
                int rowIndex = Convert.ToInt32(args[0]);
                string numeroF = args[1];
                string data = args[2];
                string nomefile = args[3];
                string folder = args[4];

              

                GridViewRow row = GVRicercaFile.Rows[rowIndex];

                // Recupera il nome del file dalla riga
                string fileName = nomefile; // row.Cells[0].Text; // Assumi che sia nella prima colonna
                string baseUrl = "http://10.6.1.250/uotep/files/";
                string userId = Vuser; // Se necessario, recupera dinamicamente l'ID utente

                // Costruisci l'URL del file
                string fileUrl = $"{baseUrl}{fileName}?user={userId}";

                // Apri il file in una nuova scheda
                Response.Redirect(fileUrl, false);

                // Trova il controllo HyperLink nella riga e aggiorna l'URL
               // HyperLink link = (HyperLink)row.FindControl("btnSelect");
                //if (link != null)
                //{
                //    //link.NavigateUrl = wordOnlineUrl;
                //    link.NavigateUrl = fileUrl;
                //    link.Visible = true; // Mostra il link
                //}
            }
            
        }
        
        //protected void apripopup_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        //}
        //protected void chiudipopup_Click(object sender, EventArgs e)
        //{
        //    //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

        // }
    }
}