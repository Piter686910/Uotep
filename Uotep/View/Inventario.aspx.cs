using AjaxControlToolkit.HtmlEditor.Popups;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uote;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;



namespace Uotep
{
    public partial class Inventario : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String ruolo = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string Isdecr = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {

            Session["PaginaChiamante"] = "~/View/Inventario.aspx";

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                ruolo = Session["ruolo"].ToString();

            }
            else
            {
                string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true");
                Response.Redirect(url);

            }

            if (!IsPostBack)
            {

                //CaricaDLL();
                TxtData.Text = DateTime.Now.ToShortDateString();// ToString("yyyy");
                caricaGriglia();


            }

        }

        protected void btChiudiAvvertenze_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalProtocollo')); modal.hide();", true);
            txtProtocollo.Text = string.Empty;
            txtDataProt.Text = string.Empty;

        }

        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean ins = false;
                SiteMaster myMaster = this.Master as SiteMaster;
                Manager mn = new Manager();
                InventarioObj inv = new InventarioObj();
                if (String.IsNullOrWhiteSpace(txtProgrBene.Text))
                {
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.InserireProgressivoBene.GetDescription(), "danger");
                    return;
                }
                else
                    inv.progr_codep = Convert.ToInt32(txtProgrBene.Text);
                inv.categoria = txtCategoria.Text.ToUpper();
                inv.codep = DdlCodep.SelectedItem.Text;
                inv.data_inserimento = System.Convert.ToDateTime(TxtData.Text);
                inv.ufficio = DdlUfficio.SelectedItem.Text;
                inv.etichetta = txtEtichetta.Text.ToUpper();
                inv.descrizione = txtDescrizione.Text.ToUpper();
                inv.dismesso = CkDismesso.Checked;
                inv.tipologia = txtTipologia.Text.ToUpper();
                inv.seriale = txtSeriale.Text.ToUpper();
                inv.note = txtNote.Text.ToUpper();

                switch (hfMod.Value)
                {
                    case "si":
                        ins = mn.updInventarioById(Hfid.Value, inv);
                        hfMod.Value = "no";
                        if (!string.IsNullOrEmpty(DdlCodep.SelectedValue))
                        {
                            int nuovoMax = mn.MaxNInventarioByCodep(DdlCodep.SelectedValue);
                            txtProgrBene.Text = nuovoMax.ToString();
                        }
                        break;
                    default:
                        ins = mn.InserisciInventario(inv);
                        break;
                }

                if (!ins)
                {

                    if (myMaster != null)

                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.InsKo.GetDescription(), "danger");

                }

                else
                {
                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");
                        Pulisci();
                    }
                }
                caricaGriglia();
            }
            catch (Exception ex)
            {

                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/Inventario.aspx";
            }
        }
        protected void btCerca_Click(object sender, EventArgs e)
        {
            Session.Remove("ListInventario");
            Manager mn = new Manager();
            DataTable listaInventario = mn.GetlistInventario();


            if (listaInventario.Rows.Count > 0)
            {
                var datiOrdinati = listaInventario.AsEnumerable()
    .OrderBy(row => row.Field<string>("codep"))
    .CopyToDataTable();

                Session["ListInventario"] = datiOrdinati;
                lblNumRighe.Text = " - Num. righe trovate: " + listaInventario.Rows.Count.ToString();
                gvDett.DataSource = datiOrdinati;
                gvDett.DataBind();
                //btBack.Visible = true;
                //DivGrid.Visible = true;
            }
        }
        private void caricaGriglia()
        {
            Session.Remove("ListInventario");
            Manager mn = new Manager();
            DataTable listaInventario = mn.GetlistInventario();


            if (listaInventario.Rows.Count > 0)
            {
                var datiOrdinati = listaInventario.AsEnumerable()
    .OrderBy(row => row.Field<string>("codep"))
    .CopyToDataTable();

                Session["ListInventario"] = datiOrdinati;
                lblNumRighe.Text = " - Num. righe trovate: " + listaInventario.Rows.Count.ToString();
                gvDett.DataSource = datiOrdinati;
                gvDett.DataBind();
                //btBack.Visible = true;
                //DivGrid.Visible = true;
            }
            //Session.Remove("ListInventario");
            //Manager mn = new Manager();
            //DataTable inv = mn.GetlistInventario();
            //gvDett.DataSource = inv;
            //gvDett.DataBind();
            //Session["ListInventario"] = inv;
        }
        private void Pulisci()
        {
            txtCategoria.Text = string.Empty;
            TxtData.Text = string.Empty;
            txtDescrizione.Text = string.Empty;
            txtTipologia.Text = string.Empty;
            txtSeriale.Text = string.Empty;
            DdlUfficio.ClearSelection();
            DdlCodep.ClearSelection();
            txtEtichetta.Text = string.Empty;
            CkDismesso.Checked = false;
            txtNote.Text = string.Empty;
            TxtData.Text = DateTime.Now.ToShortDateString();
            txtProtocollo.Text = string.Empty;
            txtProtocolloRiscontro.Text = string.Empty;
            txtDataProtRiscontro.Text = string.Empty;
        }



        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalProtocollo').modal('show');", true);
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

        protected void btStampa_Click(object sender, EventArgs e)
        {
            apripopup_Click(sender, e);
        }
        protected void btContinua_Click(object sender, EventArgs e)
        {
            string protocollo = txtProtocollo.Text;
            string data = txtDataProt.Text;
            string protocolloRiscontro = txtProtocolloRiscontro.Text;
            string dataRiscontro = txtDataProtRiscontro.Text;
            Manager mn = new Manager();
            DataTable listaInventario = mn.GetlistInventarioToPdf();
            // Ordina i dati per codep
            var datiOrdinati = listaInventario.AsEnumerable()
              .OrderBy(row => row.Field<string>("tipologia_categoria"))
              .CopyToDataTable();

            string anno = DateTime.Now.ToString("yyyy");
            Routine stampa = new Routine();
            stampa.CreaPdfInventario(datiOrdinati, anno, protocollo, data, protocolloRiscontro,dataRiscontro);
        }
       

        protected void gvDett_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }


            gvDett.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            //btCerca_Click(sender, e);
            caricaGriglia(); // Ricarica i dati nella GridView

        }
        protected void gvDett_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id_inventario").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
                if (gvDett.TopPagerRow != null)
                {
                    // Trova il controllo Label all'interno del PagerTemplate
                    Label lblPageInfo = (Label)gvDett.TopPagerRow.FindControl("lblPageInfo");
                    if (lblPageInfo != null)
                    {
                        // Calcola e imposta il testo
                        int currentPage = gvDett.PageIndex + 1;
                        int totalPages = gvDett.PageCount;
                        lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                    }
                }

            }

        }
        protected void gvDett_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore del CommandArgument
                string commandArgument = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = commandArgument.Split('|');


                // Assicurati che ci siano almeno 5 valori
                if (values.Length == 1)
                {
                    Hfid.Value = values[0]; // id

                    Manager mn = new Manager();
                    DataTable inv = mn.GetInventarioById(System.Convert.ToInt32(Hfid.Value));
                    if (inv.Rows.Count > 0)
                    {
                        txtCategoria.Text = inv.Rows[0]["categoria"].ToString();
                        txtDescrizione.Text = inv.Rows[0]["descrizione"].ToString();
                        txtNote.Text = inv.Rows[0]["note"].ToString();
                        txtTipologia.Text = inv.Rows[0]["tipologia"].ToString();
                        txtSeriale.Text = inv.Rows[0]["seriale"].ToString();
                        txtEtichetta.Text = inv.Rows[0]["etichetta"].ToString();
                        DdlCodep.SelectedItem.Text = inv.Rows[0]["codep"].ToString();
                        DdlUfficio.SelectedItem.Text = inv.Rows[0]["ufficio"].ToString();
                       //
                       //CkDismesso.Checked = System.Convert.ToBoolean(inv.Rows[0]["dismesso"]);
                        CkDismesso.Checked = inv.Rows[0]["dismesso"] != DBNull.Value ? Convert.ToBoolean(inv.Rows[0]["dismesso"]) : false;
                        txtProgrBene.Text = inv.Rows[0]["progr_codep"].ToString();
                        hfMod.Value = "si";

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
                string filterExpression = string.Empty;
                if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
                {

                    if (filterColumn == "data")
                        filterExpression = $"{filterColumn} IN ('{filterValue.Replace("'", "''")}')";
                    else
                        filterExpression = $"{filterColumn} LIKE ('%{filterValue.Replace("'", "''")}%')";
                    DataRow[] filteredRows = dt.Select(filterExpression);

                    if (filteredRows.Length > 0)
                    {
                        DataTable filteredDt = dt.Clone();
                        foreach (DataRow row in filteredRows)
                        {
                            filteredDt.ImportRow(row);
                        }
                        gvDett.DataSource = filteredDt;
                    }
                    else
                    {
                        gvDett.DataSource = null;
                    }

                }
                else
                {
                    gvDett.DataSource = dt; // Nessun filtro
                }
                gvDett.DataBind();
            }
            catch (Exception)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                // throw;
            }
        }
        private DataTable GetOriginalData()
        {
            DataTable inventario = new DataTable();
            DataView dv = new DataView();
            Manager mn = new Manager();
            string filtro = string.Empty;
            ////verifico se provengo da ricerca archivio nel caso procedo con la ricerca in db
            if (Session["ListInventario"] != null)
            {


                List<string> ListRicerca = (List<string>)Session["ListFiltrataInventario"];
                String[] ar = ListRicerca.ToArray();
                // ArchivioUote arc = new ArchivioUote();
                if (Session["ListInventario"] != null)
                {
                    // Recupera la DataTable originale dalla Sessione
                    inventario = (DataTable)Session["ListInventario"];
                }
                switch (ar[0])
                {
                    //case "data":


                    //    filtro = $"data in ('{HfFiltroSeriale.Value}')";
                    //    dv = new DataView(inventario);

                    //    dv.RowFilter = filtro;

                    //    break;
                    case "seriale":

                        filtro = $"seriale LIKE '%{HfFiltroSeriale.Value}%'";
                        dv = new DataView(inventario);

                        dv.RowFilter = filtro;
                        break;
                    case "Ufficio":

                        filtro = $"ufficio LIKE '%{HfFiltroUfficio.Value}%'";
                        dv = new DataView(inventario);

                        dv.RowFilter = filtro;

                        break;
                    case "Descrizione":

                        filtro = $"descrizione LIKE '%{HfFiltroDescrizione.Value}%'";
                        dv = new DataView(inventario);

                        dv.RowFilter = filtro;

                        break;
                    case "Etichetta":

                        filtro = $"etichetta LIKE '%{HfFiltroEtichetta  .Value}%'";
                        dv = new DataView(inventario);

                        dv.RowFilter = filtro;

                        break;
                    case "Tipologia":

                        filtro = $"tipologia LIKE '%{HfFiltroTipologia.Value}%'";
                        dv = new DataView(inventario);

                        dv.RowFilter = filtro;

                        break;
                    case "Categoria":

                        filtro = $"categoria LIKE '%{HfFiltroCategoria  .Value}%'";
                        dv = new DataView(inventario);

                        dv.RowFilter = filtro;

                        break;
                    case "codep":

                        filtro = $"codep LIKE '%{HfFiltroCodep.Value}%'";
                        dv = new DataView(inventario);

                        dv.RowFilter = filtro;
                        break;

                }
                if (inventario.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    gvDett.DataSource = dv;
                    gvDett.DataBind();

                    
                }
            }
            else
            {
                //txtPratica.Enabled = true;
                //txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
            }
            return inventario;
          
        }

        protected void gvDett_DataBound(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            if (gv.PageCount > 0)
            {
                lblInfoPagine.Text = $"Pagina {gv.PageIndex + 1} di {gv.PageCount}";

            }
            else
            {
                lblInfoPagine.Text = "Nessun record trovato";
            }
        }

        protected void txtSiglaMod_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSiglaMod = (TextBox)sender;

            // 2. riga della GridView che contiene questa TextBox
            GridViewRow row = (GridViewRow)txtSiglaMod.NamingContainer;

            // 3. Preleva il valore modificato
            string siglaInserita = txtSiglaMod.Text;
            Manager mn = new Manager();
            DataTable Sigla = mn.getAutoBySigla(siglaInserita);

            if (Sigla.Rows.Count > 0)
            {
                // 5. Cerca la TextBox "txtTarga" all'interno della stessa riga
                TextBox txtTargaMod = (TextBox)row.FindControl("txtTargaMod");
                txtTargaMod.Text = Sigla.Rows[0]["targa"].ToString();

            }
        }

        protected void txtFilterCodep_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "codep", txtFilter.Text };
            // Salva la lista nella Sessione
            Session["ListAuto"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroCodep.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterCodep")
            {
                columnName = "codep";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroCodep.Value);
        }

        protected void txtCodStanzaMod_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "codep", txtFilter.Text };
            // Salva la lista nella Sessione
            Session["ListFiltrataInventario"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroCodep.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterCodep")
            {
                columnName = "codep";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroCodep.Value);
        }

        protected void txtFilterSeriale_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "seriale", txtFilter.Text };
            // Salva la lista nella Sessione
            Session["ListFiltrataInventario"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroSeriale.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterSeriale")
            {
                columnName = "seriale";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

             PopulateGridView(columnName, HfFiltroSeriale.Value);
        }

        protected void txtFilterNote_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "note", txtFilter.Text };
            // Salva la lista nella Sessione
            Session["ListFiltrataInventario"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroNote.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterNote")
            {
                columnName = "note";
            }


              PopulateGridView(columnName, HfFiltroNote.Value);
        }

        protected void btBack_Click(object sender, EventArgs e)
        {
            gvDett.DataSource = Session["ListInventario"];
            gvDett.DataBind();
        }

        protected void DdlCodep_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(DdlCodep.SelectedValue))
            {
                txtProgrBene.Text = string.Empty;
                return;
            }

            Manager mn = new Manager();
            int n = mn.MaxNInventarioByCodep(DdlCodep.SelectedValue);
            txtProgrBene.Text = n.ToString();
        }

        protected void txtFilterUfficio_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "ufficio", txtFilter.Text };
            // Salva la lista nella Sessione
            Session["ListFiltrataInventario"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroUfficio.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterUfficio")
            {
                columnName = "ufficio";
            }
            PopulateGridView(columnName, HfFiltroUfficio.Value);
        }

        protected void txtFilterEtichetta_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "etichetta", txtFilter.Text };
            // Salva la lista nella Sessione
            Session["ListFiltrataInventario"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroEtichetta.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterEtichetta")
            {
                columnName = "etichetta";
            }
            PopulateGridView(columnName, HfFiltroEtichetta.Value);
        }

        protected void txtFilterDescrizione_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "Descrizione", txtFilter.Text };
            // Salva la lista nella Sessione
            Session["ListFiltrataInventario"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroDescrizione.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterDescrizione")
            {
                columnName = "descrizione";
            }
            PopulateGridView(columnName, HfFiltroDescrizione.Value);
        }

        protected void txtFilterCategoria_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "categoria", txtFilter.Text };
            // Salva la lista nella Sessione
            Session["ListFiltrataInventario"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroCategoria.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterCategoria")
            {
                columnName = "categoria";
            }
            PopulateGridView(columnName, HfFiltroCategoria.Value);
        }

        protected void txtFilterTipologia_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "tipologia", txtFilter.Text };
            // Salva la lista nella Sessione
            Session["ListFiltrataInventario"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroTipologia.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterTipologia")
            {
                columnName = "tipologia";
            }
            PopulateGridView(columnName, HfFiltroTipologia.Value);
        }

        protected void btnFiltra_Click(object sender, EventArgs e)
        {

        }

       
        protected void btEstrai_Click(object sender, EventArgs e)
        {

        }
    }
}