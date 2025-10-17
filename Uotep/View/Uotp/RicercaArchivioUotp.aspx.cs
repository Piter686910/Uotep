using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using iText.StyledXmlParser.Jsoup.Nodes;
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
using Path = System.IO.Path;


namespace Uotep
{
    public partial class RicercaArchivioUotp : Page
    {
        public string argomentoPassato = string.Empty;
        public String Filename = ConfigurationManager.AppSettings["CartellaFileArchivio"];
        String Vuser = String.Empty;
        String Ruolo = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PaginaChiamante"] = "~/View/uotp/RicercaArchivio.aspx";

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

        }


        private void Pulisci()
        {

            txtPratN.Text = String.Empty;
            txtBU.Text = String.Empty;
            txtDestinatario.Text = String.Empty;
            txtNota.Text = String.Empty;
            txtOggetto.Text = String.Empty;
            txtDestinatario.Text = String.Empty;


        }


        protected void Ricerca_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNota.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Nota", txtNota.Text.Replace("*", "%") };

                // Salva la lista nella Sessione
                Session["ListRicercaTp"] = ListRicerca;

            }
            if (!string.IsNullOrEmpty(txtOggetto.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Oggetto", txtOggetto.Text.Replace("*", "%")};

                // Salva la lista nella Sessione
                Session["ListRicercaTp"] = ListRicerca;

            }
            if (!string.IsNullOrEmpty(txtPratN.Text))
            {
                // Crea una lista 
                List<string> ListRicerca = new List<string> { "Pratica", txtPratN.Text };


                // Salva la lista nella Sessione
                Session["ListRicercaTp"] = ListRicerca;
            }
            if (!string.IsNullOrEmpty(txtBU.Text))
            {
                // Crea una lista
                List<string> ListRicerca = new List<string> { "BU", txtBU.Text };

                // Salva la lista nella Sessione
                Session["ListRicercaTp"] = ListRicerca;
            }
            if (!string.IsNullOrEmpty(txtDestinatario.Text))
            {
                // Crea una lista
                List<string> ListRicerca = new List<string> { "Destinatario", txtDestinatario.Text.Replace("*", "%") };

                // Salva la lista nella Sessione
                Session["ListRicercaTp"] = ListRicerca;
            }

            // Reindirizza alla pagina di destinazione
            //string url = VirtualPathUtility.ToAbsolute("~/View/InserimentoArchivio.aspx");
            //Response.Redirect(url, false);
            // Response.Redirect("InserimentoArchivio.aspx");
            if (Session["ListRicercaTp"] != null)
            {
                Manager mn = new Manager();
                List<string> ListRicerca = (List<string>)Session["ListRicercaTp"];
                String[] ar = ListRicerca.ToArray();
                // ArchivioUote arc = new ArchivioUote();
                DataTable arc = new DataTable();
                switch (ar[0])
                {
                    case "Pratica":
                        arc = mn.getPraticaArchivioUotp(Convert.ToInt32(ar[1]), null, null, null, null);
                        break;
                    case "Oggetto":
                        arc = mn.getPraticaArchivioUotp(0, ar[1], null, null, null);
                        break;
                    case "Destinatario":
                        arc = mn.getPraticaArchivioUotp(0, null, null, null, ar[1]);
                        break;
                    case "Nota":
                        arc = mn.getPraticaArchivioUotp(0, null, null, ar[1], null);
                        break;
                    case "BU":
                        arc = mn.getPraticaArchivioUotp(0, null, ar[1], null, null);
                        break;

                }
                if (arc.Rows.Count > 0)
                {
                    GVRicercaPratica.DataSource = arc;
                    GVRicercaPratica.DataBind();
                    // Salva datatable pratica  nella Sessione
                    Session["ListPraticheTp"] = arc;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalPratica').modal('show');", true);
                }
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "nessun dato trovato" + "'); $('#errorModal').modal('show');", true);
            }
            else
            {
                txtPratN.Enabled = true;
                //txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
            }
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



        protected void btDestinatario_Click(object sender, EventArgs e)
        {
            DivOggetto.Visible = false;
            DivBU.Visible = false;
            DivPratica.Visible = false;
            DivNota.Visible = false;
            DivRicerca.Visible = true;
            DivDestinatario.Visible = true;
            Nascondi();
        }

        protected void btOggetto_Click(object sender, EventArgs e)
        {
            Nascondi();
            DivDestinatario.Visible = false;
            DivPratica.Visible = false;
            DivNota.Visible = false;
            DivBU.Visible = false;
            DivRicerca.Visible = true;
            DivOggetto.Visible = true;
        }
        protected void Nascondi()
        {
            pnDettagli.Visible = false;
        }
        protected void btNpratica_Click(object sender, EventArgs e)
        {
            DivDestinatario.Visible = false;
            DivPratica.Visible = true;
            DivNota.Visible = false;
            DivBU.Visible = false;
            DivRicerca.Visible = true;
            DivOggetto.Visible = false;
            Nascondi();
        }
        protected void btNota_Click(object sender, EventArgs e)
        {
            DivDestinatario.Visible = false;
            DivPratica.Visible = false;
            DivBU.Visible = false;
            DivRicerca.Visible = true;
            DivOggetto.Visible = false;
            DivNota.Visible = true;
            Nascondi();
        }
        protected void btBU_Click(object sender, EventArgs e)
        {
            DivDestinatario.Visible = false;
            DivPratica.Visible = false;
            DivBU.Visible = true;
            DivRicerca.Visible = true;
            DivOggetto.Visible = false;
            DivNota.Visible = false;
            Nascondi();
        }

        protected void txtFilterOggetto_TextChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TextBox txtFilter = (System.Web.UI.WebControls.TextBox)sender;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroOggetto.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterOggetto")
            {
                columnName = "oggetto1"; // Assumi che "oggetto1" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroOggetto.Value); // Esempio di funzione di filtro
            apripopupPratica_Click(sender, e);
        }

        protected void txtFilterNote_TextChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TextBox txtFilter = (System.Web.UI.WebControls.TextBox)sender;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroNote.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterNote")
            {
                columnName = "note"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroNote.Value); // Esempio di funzione di filtro
            apripopupPratica_Click(sender, e);
        }
        protected void apripopupPratica_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalPratica').modal('show');", true);
        }
        protected void txtFilterDestinatario_TextChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TextBox txtFilter = (System.Web.UI.WebControls.TextBox)sender;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroDestinatario.Value = filterValue;
            List<string> ListRicerca = new List<string> { "Destinatario", filterValue };
            Session["ListRicercaTp"] = ListRicerca;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterDestinatario")
            {
                columnName = "destinatario1"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroDestinatario.Value); // Esempio di funzione di filtro
            apripopupPratica_Click(sender, e);
        }
        protected void FillScheda(DataTable arc)
        {
            txtNumProTp.Text = arc.Rows[0].ItemArray[1].ToString();
            txtProGenTp.Text = arc.Rows[0].ItemArray[107].ToString();
            txtProProcTp.Text = arc.Rows[0].ItemArray[115].ToString().ToUpper();
            txtDataInserimentoTp.Text = arc.Rows[0].ItemArray[6].ToString();
            txtBUTp.Text = arc.Rows[0].ItemArray[42].ToString().ToUpper();
            txtCartellinaTp.Text = arc.Rows[0].ItemArray[111].ToString().ToUpper();
            txtNotaTp.Text = arc.Rows[0].ItemArray[104].ToString().ToUpper();
            txtNotaTp.ToolTip = arc.Rows[0].ItemArray[104].ToString().ToUpper();
            txtOggettoTp.Text = arc.Rows[0].ItemArray[19].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[20].ToString().ToUpper();
            txtOggettoTp.ToolTip = arc.Rows[0].ItemArray[19].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[20].ToString().ToUpper();
            txtDestinatarioTp.Text = arc.Rows[0].ItemArray[27].ToString().ToUpper();
            txtDestinatarioTp.ToolTip = arc.Rows[0].ItemArray[27].ToString().ToUpper();
            txtQuartiereTp.Text = arc.Rows[0].ItemArray[40].ToString().ToUpper();
            txtDataProtProc.Text = arc.Rows[0].ItemArray[116].ToString();// data.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            TxtIndirizzoTp.Text = arc.Rows[0].ItemArray[47].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[48].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[49].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[50].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[51].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[52].ToString().ToUpper();
            TxtIndirizzoTp.ToolTip = arc.Rows[0].ItemArray[47].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[48].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[49].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[50].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[51].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[52].ToString().ToUpper();
            //txtDataProtGen.Text = arc.Rows[0].ItemArray[116].ToString();

        }
        protected void gvPopup_RowCommandP(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore dell'ID dalla CommandArgument
                //string selectedValue = e.CommandArgument.ToString();


                string[] args = e.CommandArgument.ToString().Split(';');
                int idP = System.Convert.ToInt32(args[0]);
                string Npratica = args[1];


                // Imposta il valore nel TextBox
                //txtSelectedValue.Text = selectedValue;
                txtPratN.Text = Npratica;

                Manager mn = new Manager();
                //DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);

                DataTable pratica = mn.getPraticaArchivioUotpById(idP);
                if (pratica.Rows.Count > 0)
                {
                    FillScheda(pratica);
                    DivRicerca.Visible = false;
                    pnDettagli.Visible = true;
                    Pulisci();
                }
                Session.Remove("ListRicerca");
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }
        protected void gvPopup_RowDataBoundP(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
            if (GVRicercaPratica.TopPagerRow != null && GVRicercaPratica.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                System.Web.UI.WebControls.Label lblPageInfo = (System.Web.UI.WebControls.Label)GVRicercaPratica.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = GVRicercaPratica.PageIndex + 1;
                    int totalPages = GVRicercaPratica.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
        }
        protected void GVRicercaPratica_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVRicercaPratica.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            if (String.IsNullOrEmpty(HfFiltroNote.Value) && String.IsNullOrEmpty(HfFiltroDestinatario.Value) && String.IsNullOrEmpty(HfFiltroOggetto.Value))
            {
                //RicercaNew(sender, e);
            }
            else
            {
                if (!String.IsNullOrEmpty(HfFiltroNote.Value))
                {
                    PopulateGridView("note", HfFiltroNote.Value);
                    //    apripopupPratica_Click(sender, e);
                }
                else
                {
                    if (!String.IsNullOrEmpty(HfFiltroDestinatario.Value))
                    {
                        PopulateGridView("detinatario1", HfFiltroDestinatario.Value);
                        //apripopupPratica_Click(sender, e);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(HfFiltroOggetto.Value))
                        {
                            PopulateGridView("oggetto1", HfFiltroOggetto.Value);
                            //          apripopupPratica_Click(sender, e);
                        }
                    }
                }
            }
        }
        private void PopulateGridView(string filterColumn = "", string filterValue = "")
        {

            DataTable dt = new DataTable();

            dt = GetOriginalData(); // ricerco la lista nuovamente
            try
            {
                //applico il filtro
                if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
                {



                    string filterExpression = $"{filterColumn} LIKE '%{filterValue.Replace("'", "''")}%'";
                    DataRow[] filteredRows = dt.Select(filterExpression);

                    if (filteredRows.Length > 0)
                    {
                        DataTable filteredDt = dt.Clone();
                        foreach (DataRow row in filteredRows)
                        {
                            filteredDt.ImportRow(row);
                        }
                        GVRicercaPratica.DataSource = filteredDt;
                    }
                    else
                    {
                        GVRicercaPratica.DataSource = null;
                    }

                }
                else
                {
                    GVRicercaPratica.DataSource = dt; // Nessun filtro
                }
                GVRicercaPratica.DataBind();
            }
            catch (Exception)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                // throw;
            }
        }
        private DataTable GetOriginalData()
        {
            DataTable arc = new DataTable();
            DataView dv = new DataView();
            string filtro = string.Empty;
            Manager mn = new Manager();
            ////verifico se provengo da ricerca archivio nel caso procedo con la ricerca in db
            if (Session["ListRicercaTp"] != null)
            {
                List<string> ListRicerca = (List<string>)Session["ListRicercaTp"];
                String[] ar = ListRicerca.ToArray();
                // ArchivioUote arc = new ArchivioUote();

                if (Session["ListPraticheTp"] != null)
                {
                    // Recupera la DataTable originale dalla Sessione
                    arc = (DataTable)Session["ListPraticheTp"];
                }
                switch (ar[0])
                {
                    case "Pratica":
                        arc = mn.getPraticaArchivioUotp(Convert.ToInt32(ar[1]), null, null, null, null);
                        break;
                    case "Oggetto":
                        //arc = mn.getPraticaArchivioUotp(0, ar[1], null, null, null);
                        filtro = $"Oggetto1 LIKE '%{HfFiltroOggetto.Value}%'";
                        dv = new DataView(arc);

                        dv.RowFilter = filtro;
                        break;
                    case "Destinatario":
                        //arc = mn.getPraticaArchivioUotp(0, null, null, null, ar[1]);
                        filtro = $"destinatario1 LIKE '%{HfFiltroNote.Value}%'";
                        dv = new DataView(arc);
                        break;
                    case "Nota":
                        //                        arc = mn.getPraticaArchivioUotp(0, null, null, ar[1], null);
                        filtro = $"note LIKE '%{HfFiltroNote.Value}%'";
                        dv = new DataView(arc);

                        break;
                    case "BU":
                        arc = mn.getPraticaArchivioUotp(0, null, ar[1], null, null);
                        break;

                }
                if (arc.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    GVRicercaPratica.DataSource = arc;
                    GVRicercaPratica.DataBind();
                    //segnalo he sono in modifica prartica
                    txtPratN.Enabled = false;
                }
            }
            else
            {
                txtPratN.Enabled = true;
            }
            return arc;
            // return dt;
        }

        protected void BtNewRicerca_Click(object sender, EventArgs e)
        {
            DivRicerca.Visible = true;
            pnDettagli.Visible = false;
        }
    }
}