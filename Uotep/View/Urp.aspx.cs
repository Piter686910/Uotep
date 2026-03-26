using AjaxControlToolkit.HtmlEditor.Popups;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
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



namespace Uotep
{
    public partial class Urp : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String ruolo = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        public String Filename = ConfigurationManager.AppSettings["CartellaUrp"];
        string msg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["PaginaChiamante"] = "~/View/Urp.aspx";

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

            if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
            {
                Manager mn = new Manager();
               
                Boolean resp = mn.DelRegistroById(System.Convert.ToInt32(Request.QueryString["id"]));
                if (resp)
                {
                    DataTable dt = mn.GetListRegistroUrp();
                    if (dt.Rows.Count > 0)
                    {
                        gvRegistro.DataSource = dt;
                        gvRegistro.DataBind();
                    }
                    else
                    {
                        gvRegistro.DataSource = null;
                        gvRegistro.DataBind();
                    }
                    // Esce dalla modalità modifica
                    gvRegistro.EditIndex = -1;

                    // Lancia script per pulire URL
                    string script = "window.history.replaceState(null, null, window.location.pathname);";
                    ScriptManager.RegisterStartupScript(this, GetType(), "PulisciUrl", script, true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaRegistro').modal('show');", true);
                }
                // Esce dalla modalità modifica
                //gvRegistro.EditIndex = -1;

            }
            //     CaricaDLL();
            if (!IsPostBack)
            {

                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
             //   ProtocolloLiteral.Text = decodedText;
                CaricaDLL();
                //  btRegistro_Click(sender, e);
            }

        }
        public void Convalida()
        {

            //if (!String.IsNullOrEmpty(HfGiudice.Value))
            //    btSalvaGiudice.Visible = true;

            //if (!String.IsNullOrEmpty(HfTipoProv.Value))
            //    btSalvaTipoProvv.Visible = true;



        }

        protected Boolean Verifica()
        {
            Boolean resp = true;

            if (DdlEsito.SelectedIndex >= 0)
            {
                if (!rdCopiaVisione.Checked && !rdRicCopia.Checked && !rdRicVisione.Checked)
                {
                    resp = false;

                }

            }
            return resp;
        }

        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean resp = Verifica();
                if (resp)
                {

                    UrpScadenziario scadenziario = new UrpScadenziario();
                    UrpRegistro registro = new UrpRegistro();

                    scadenziario.nr_carico = txtCarico.Text;
                    scadenziario.anno = System.Convert.ToInt32(txtAnno.Text);
                    scadenziario.nr_pratica = txtPratica.Text;
                    scadenziario.richiedente = txtRichiedente.Text;
                    scadenziario.protGen = txtProtGen.Text.Trim();


                    scadenziario.controInteressati = rbControInteressatiSi.Checked;
                    switch (DdlEsito.SelectedItem.Text)
                    {
                        case "Differimento":
                        case "Differimento Ulteriore":
                            HfRegistro.Value = "duplica";// INSERISCO NUOVA RIGA IN SCADENZIARIO 

                            break;
                        case "Digiego Parziale/Accoglimento Parziale":
                        case "Accoglimento":
                        case "Diniego Totale":
                            HfRegistro.Value = "registro";

                            break;
                        default:
                            break;
                    }
                    scadenziario.esito = DdlEsito.SelectedItem.Text;
                    //preparo classe registro
                    if (!String.IsNullOrEmpty(DdlEsito.SelectedItem.Text))
                    {

                        registro.controInteressati = rbControInteressatiSi.Checked;
                        registro.motivazione = txtMotivazione.Text.Trim().Replace("'", "''");
                        registro.nrPgTrasmissioneRichiesto = txtProtGen.Text.Trim();
                        if (!string.IsNullOrEmpty(txtDataArrivo.Text))
                            registro.dataPresentRichiesta = System.Convert.ToDateTime(txtDataArrivo.Text);
                        registro.uffDetentore = "10DPSC1043-Servizio Polizia Locale";
                        registro.nrPgTrasmissioneRiscontro = txtProtUscita.Text.Trim();

                        if (!string.IsNullOrEmpty(txtDataUscita.Text))
                            registro.dataConclProcedimento = System.Convert.ToDateTime(txtDataUscita.Text);
                        else
                            registro.dataConclProcedimento = DateTime.MinValue;
                        registro.esito = DdlEsito.SelectedItem.Text;
                        if (rdCopiaVisione.Checked)
                        {
                            registro.oggetto = "Copia/Visione";
                        }
                        if (rdRicCopia.Checked)
                        {
                            registro.oggetto = "Richiesta Copia Atti";
                        }
                        if (rdRicVisione.Checked)
                        {
                            registro.oggetto = "Richiesta Visione Atti";
                        }
                    }
                    scadenziario.motivazione = txtMotivazione.Text;
                    scadenziario.protUscita = txtProtUscita.Text;
                    if (!string.IsNullOrEmpty(txtDataScadenza.Text))
                    {
                        scadenziario.dataScadenza = System.Convert.ToDateTime(txtDataScadenza.Text);
                    }
                    else
                    {
                        scadenziario.dataScadenza = DateTime.MinValue;
                    }
                    if (!string.IsNullOrEmpty(txtDataArrivo.Text))
                    {
                        scadenziario.dataArrivo = System.Convert.ToDateTime(txtDataArrivo.Text);
                    }
                    else
                    {
                        scadenziario.dataArrivo = DateTime.MinValue;
                    }
                    if (!string.IsNullOrEmpty(txtDataUscita.Text))
                    {
                        scadenziario.dataUscita = System.Convert.ToDateTime(txtDataUscita.Text);
                    }
                    else
                    {
                        scadenziario.dataUscita = DateTime.MinValue;
                    }
                    scadenziario.ric24190 = rd241_90.Checked;
                    scadenziario.ric3313 = rd33_2013.Checked;
                    Boolean ins;
                    Manager mn = new Manager();
                    int id = 0;
                    if (!String.IsNullOrEmpty(HfId.Value))
                        id = Convert.ToInt32(HfId.Value); //effettuo modifica

                    if (!String.IsNullOrEmpty(HfRegistro.Value))
                        ins = mn.InsScadenziarioRegistro(scadenziario, id, HfRegistro.Value, registro, HfNewDataScadenza.Value); //effettuo inserimento anche in registro
                    else
                        ins = mn.InsScadenziario(scadenziario, id);
                    if (ins)
                    {
                        HfRegistro.Value = string.Empty;
                        HfId.Value = string.Empty;
                        //errorMessage.InnerText = "Inserimento effettuato correttamente";
                        //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);
                        SiteMaster myMaster = this.Master as SiteMaster;

                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");


                        }
                        btNewIns.Visible = true;
                    }
                }
                else
                {
                    //errorMessage.InnerText = "Seleziona Oggetto";
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Seleziona Oggetto" + "'); $('#errorModal').modal('show');", true);
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("ATTENZIONE", "Seleziona Oggetto", "warning");


                    }
                }

            }
            catch (Exception ex)
            {

                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/Urp.aspx";
            }
        }
        private void Pulisci()
        {
            Convalida();
            txtCarico.Text = String.Empty;
            txtDataArrivo.Text = String.Empty;
            txtDataScadenza.Text = String.Empty;
            txtDataUscita.Text = String.Empty;
            txtRichiedente.Text = string.Empty;
            txtPratica.Text = string.Empty;
            txtAnno.Text = string.Empty;
            DdlEsito.ClearSelection();
            txtMotivazione.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProtUscita.Text = String.Empty;
            rbControInteressatiNo.Checked = false;
            rd241_90.Checked = false;
            rd33_2013.Checked = false;
            rbControInteressatiSi.Checked = false;
            rdCopiaVisione.Checked = false;
            rdRicCopia.Checked = false;
            rdRicVisione.Checked = false;
            CaricaDLL();

        }

        private void CaricaDLL()
        {
            try
            {
                Manager mn = new Manager();
                DataTable RicercaEsito = mn.getListRicercaEsitoUrp(out msg);
                DdlEsito.DataSource = RicercaEsito; // Imposta il DataSource della DropDownList
                DdlEsito.DataTextField = "Descrizione"; // Il campo visibile
                DdlEsito.DataValueField = "ID_esito"; // Il valore associato a ogni opzione
                DdlEsito.DataBind();
                DdlEsito.Items.Insert(0, new ListItem("", "0"));

                //  DropDownList ddl = (DropDownList)e.Row.FindControl("DdlEsitoFiltro");




            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in carica ddl file Urp.cs ");
                    sw.Close();
                }
                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);
                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/Urp.aspx";

            }
        }


        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalQuartiere').modal('show');", true);
        }

        protected void btNewIns_Click(object sender, EventArgs e)
        {
            Pulisci();
            //Routine prot = new Routine();
            //txtProt.Text = prot.GetProtocollo();
            //txtDataInsCarico.Text = DateTime.Now.Date.ToShortDateString();
            //btNewIns.Visible = false;
            //btSalva.Visible = true;
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

        protected void gvScadenziario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore dell'ID dalla CommandArgument
                string selectedValue = e.CommandArgument.ToString();
                string[] ar = null;
                // Imposta il valore nel TextBox
                //txtSelectedValue.Text = selectedValue;
                ar = selectedValue.Split('|');
                HfId.Value = ar[0];
                Manager mn = new Manager();

                DataTable dt = mn.GetScadenziarioById(System.Convert.ToInt32(ar[0]));
                if (dt.Rows.Count > 0)
                {
                    FillScheda(dt);
                }
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicercaScadenziario')); modal.hide();", true);
            }
        }
        protected void gvScadenziario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "ID_Scadenziario").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";

                if (gvScadenziario.TopPagerRow != null)
                {
                    // Trova il controllo Label all'interno del PagerTemplate
                    Label lblPageInfo = (Label)gvScadenziario.TopPagerRow.FindControl("lblPageInfo");
                    if (lblPageInfo != null)
                    {
                        // Calcola e imposta il testo
                        int currentPage = gvScadenziario.PageIndex + 1;
                        int totalPages = gvScadenziario.PageCount;
                        lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                    }
                }


            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // 1. Cerca il controllo nel Template
                DropDownList ddlFiltro = (DropDownList)e.Row.FindControl("DdlEsitoFiltro");

                // 2. Se lo trovi, caricalo
                if (ddlFiltro != null)
                {

                    Manager mn = new Manager();
                    DataTable RicercaEsito = mn.getListRicercaEsitoUrp(out msg);

                    ddlFiltro.DataSource = RicercaEsito;
                    ddlFiltro.DataTextField = "Descrizione";
                    ddlFiltro.DataValueField = "ID_esito";
                    ddlFiltro.DataBind();

                    ddlFiltro.Items.Insert(0, new ListItem("", "6"));
                    ddlFiltro.Items.Insert(0, new ListItem("Tutti", "7"));
                }
            }
        }
        protected void gvScadenziario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }


            gvScadenziario.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            btRicerca_Click(sender, e);


        }

        protected void txtFilterDataArrivo_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "dataArrivo", txtDataArrivo.Text };

            // Salva la lista nella Sessione
            Session["ListRicercaFiltro"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfDataArrivo.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterDataArrivo")
            {
                columnName = "dataArrivo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfDataArrivo.Value); // Esempio di funzione di filtro

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaScadenziario').modal('show');", true);
        }

        protected void txtFilterDataScadenza_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "dataScadenza", txtDataArrivo.Text };

            // Salva la lista nella Sessione
            Session["ListRicercaFiltro"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfDataScadenza.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterDataScadenza")
            {
                columnName = "dataScadenza"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfDataScadenza.Value); // Esempio di funzione di filtro

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaScadenziario').modal('show');", true);
        }

        protected void txtFilterDataUscita_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "dataUscita", txtDataArrivo.Text };

            // Salva la lista nella Sessione
            Session["ListRicercaFiltro"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfDataUscita.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterDataUscita")
            {
                columnName = "dataUscita"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfDataUscita.Value); // Esempio di funzione di filtro

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaScadenziario').modal('show');", true);
        }

        protected void btRicerca_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = mn.GetListScadenziarioUrp();
            if (dt.Rows.Count > 0)
            {
                gvScadenziario.DataSource = dt;
                gvScadenziario.DataBind();
                Session["ListScadenziario"] = dt;
            }
            else
            {
                gvScadenziario.DataSource = null;
                gvScadenziario.DataBind();
            }
            Pulisci(); 
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaScadenziario').modal('show');", true);

        }
        protected void btRegistro_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = mn.GetListRegistroUrp();
            if (dt.Rows.Count > 0)
            {
                gvRegistro.DataSource = dt;
                gvRegistro.DataBind();
            }
            else
            {
                gvRegistro.DataSource = null;
                gvRegistro.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaRegistro').modal('show');", true);
        }
        protected void FillScheda(DataTable dt)
        {
            txtCarico.Text = dt.Rows[0]["nr_carico"].ToString();
            txtAnno.Text = dt.Rows[0]["anno"].ToString();
            txtPratica.Text = string.IsNullOrWhiteSpace(dt.Rows[0]["nr_pratica"].ToString()) ? string.Empty : dt.Rows[0]["nr_pratica"].ToString().Trim();


            txtRichiedente.Text = string.IsNullOrWhiteSpace(dt.Rows[0]["richiedente"].ToString()) ? string.Empty : dt.Rows[0]["richiedente"].ToString().Trim();

            txtProtGen.Text = dt.Rows[0]["protGen"].ToString();
            if (dt.Rows[0]["controInteressati"] != DBNull.Value)
            {
                Boolean controInt = System.Convert.ToBoolean(dt.Rows[0]["controInteressati"]);
                if (controInt)
                {
                    rbControInteressatiSi.Checked = true;
                }
                else
                {
                    rbControInteressatiNo.Checked = true;
                }
            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["esito"].ToString()))
                DdlEsito.SelectedItem.Text = dt.Rows[0]["esito"].ToString();
            txtMotivazione.Text = string.IsNullOrWhiteSpace(dt.Rows[0]["motivazione"].ToString()) ? string.Empty : dt.Rows[0]["motivazione"].ToString().Trim();
            txtProtUscita.Text = dt.Rows[0]["protUscita"].ToString();
            if (dt.Rows[0]["dataScadenza"] != DBNull.Value)
            {
                DateTime data = Convert.ToDateTime(dt.Rows[0]["dataScadenza"]);
                txtDataScadenza.Text = (data == DateTime.MinValue) ? string.Empty : data.ToString("dd/MM/yyyy");
                txtDataScadenza.Text = FormatMyDate(txtDataScadenza.Text);

            }
            if (dt.Rows[0]["dataArrivo"] != DBNull.Value)
            {
                DateTime data = Convert.ToDateTime(dt.Rows[0]["dataArrivo"]);
                txtDataArrivo.Text = (data == DateTime.MinValue) ? string.Empty : data.ToString("dd/MM/yyyy");
            }
            if (dt.Rows[0]["dataUscita"] != DBNull.Value)
            {

                txtDataUscita.Text = (Convert.ToDateTime(dt.Rows[0]["dataUscita"]) == DateTime.MinValue) ? "   " : Convert.ToDateTime(dt.Rows[0]["dataUscita"]).ToString("dd/MM/yyyy");
                txtDataUscita.Text= FormatMyDate(txtDataUscita.Text); // sostituisce data minima in spazio
            }

            if (dt.Rows[0]["ric24190"] != DBNull.Value)
            {
                Boolean ric24190 = System.Convert.ToBoolean(dt.Rows[0]["ric24190"]);
                if (ric24190)
                {
                    rd241_90.Checked = true;
                }
            }
            if (dt.Rows[0]["ric3313"] != DBNull.Value)
            {
                Boolean ric3313 = System.Convert.ToBoolean(dt.Rows[0]["ric3313"]);
                if (ric3313)
                {
                    rd33_2013.Checked = true;
                }
            }
        }

        protected void ModalChiudiDataScadenza_Click(object sender, EventArgs e)
        {
            HfNewDataScadenza.Value = txtdataScadenzaPopup.Text;
            Salva_Click(sender, e);
        }

        protected void DdlEsito_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!rdCopiaVisione.Checked && !rdRicCopia.Checked && !rdRicVisione.Checked)
            {
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("ATTENZIONE", "Seleziona Oggetto", "warning");
                   // DdlEsito.SelectedItem.Text = DdlEsito.SelectedItem.Text;
                    DdlEsito.ClearSelection();
                }

            }
            else
            {



                if (DdlEsito.SelectedItem.Text == "Differimento" || DdlEsito.SelectedItem.Text == "Differimento Ulteriore")
                {
                    //string script = "$('#ModalDataScadenza').appendTo('body').modal('show');";
                    string script = "$('#ModalDataScadenza').appendTo('form:first').modal('show');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ApriModalLocale", script, true);
                    //  ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDataScadenza').modal('show');", true);
                }
            }
        }



        protected void gvRegistro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "ID_Registro").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";

                if (gvRegistro.TopPagerRow != null)
                {
                    // Trova il controllo Label all'interno del PagerTemplate
                    Label lblPageInfo = (Label)gvRegistro.TopPagerRow.FindControl("lblPageInfo");
                    if (lblPageInfo != null)
                    {
                        // Calcola e imposta il testo
                        int currentPage = gvRegistro.PageIndex + 1;
                        int totalPages = gvRegistro.PageCount;
                        lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                    }
                }


            }
        }

        protected void gvRegistro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Manager mn = new Manager();
            if (e.CommandName == "Select")
            {
                // Ottieni il valore dell'ID dalla CommandArgument
                string selectedValue = e.CommandArgument.ToString();
                string[] ar = null;
                // Imposta il valore nel TextBox
                //txtSelectedValue.Text = selectedValue;
                ar = selectedValue.Split('|');
                HfId.Value = ar[0];


                DataTable dt = mn.GetRegistroById(System.Convert.ToInt32(ar[0]));
                if (dt.Rows.Count > 0)
                {
                    FillScheda(dt);
                }
            }

        }

        protected void gvRegistro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }


            gvRegistro.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            btRegistro_Click(sender, e);
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {

                // 1. RECUPERA DATI
                Manager mn = new Manager();
                // 1. Recupera i dati dal DB (tabella registro)
                DataTable listaRegistro = mn.GetListRegistro(out msg);

                if (listaRegistro.Rows.Count == 0)
                {
                    //lblError.Text = "⚠️ Nessun dato da esportare.";

                    errorMessage.InnerText = @"⚠️ Nessun dato da esportare."; ;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
                    return;
                }
                else
                {
                    Routine stampa = new Routine();
                    stampa.CreaExcelRegistro(listaRegistro, Filename, Context);
                }

            }
            catch (Exception ex)
            {
                //lblError.Text = "Errore Excel: " + ex.Message;
                //lblError.ForeColor = System.Drawing.Color.Red;
                errorMessage.InnerText = @"Errore Excel: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
            }
        }

        protected void DdlEsitoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCheHaScatenatoEvento = (DropDownList)sender;

            // Ora puoi leggere il valore
            string filtroSelezionato = ddlCheHaScatenatoEvento.SelectedItem.Text;

            //// TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            string filterValue = filtroSelezionato;
            HfFiltroEsito.Value = filterValue;
            List<string> ListRicerca = new List<string> { "Esito", filterValue };

            // // Salva la lista nella Sessione
            Session["ListRicercaFiltro"] = ListRicerca;
            // string filterValue = filtroSelezionato;
            // HfFiltroAccertatori.Value = filterValue;
            // // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            // string columnName = ""; // Devi decidere su quale campo del DB filtrare
            // if (txtFilter.ID == "DdlEsitoFiltro")
            // {
            string columnName = "Esito";
            // }

            // // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroEsito.Value); // Esempio di funzione di filtro
                                                               //            apripopup_Click(sender, e);
                                                               // Session.Remove("ListScadenziario");
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaScadenziario').modal('show');", true);
        }
        // Funzione  che carica i dati e applica il filtro
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

                    if (filterValue.Replace("'", "''") == "Differimento")
                    {
                        filterExpression = $"{filterColumn} = ('{filterValue.Replace("'", "''")}')";
                    }
                    else if (filterColumn == "dataArrivo" || filterColumn == "dataScadenza" || filterColumn == "dataUscita")
                    {
                        filterExpression = $"{filterColumn} IN ('{filterValue.Replace("'", "''")}')";
                    }
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
                        gvScadenziario.DataSource = filteredDt;
                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                        gvScadenziario.DataSource = null;

                    }

                }
                else if (String.IsNullOrWhiteSpace(filterValue))
                {
                    Manager mn = new Manager();
                    dt = mn.GetListScadenziarioUrpEsitoVuoto(filterValue);
                    gvScadenziario.DataSource = dt;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                    gvScadenziario.DataSource = dt; // Nessun filtro
                }
                gvScadenziario.DataBind();
            }
            catch (Exception)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                // throw;
            }
        }
        private DataTable GetOriginalData()
        {
            DataTable scadenziario = new DataTable();
            DataView dv = new DataView();
            Manager mn = new Manager();
            string filtro = string.Empty;
            ////verifico se provengo da ricerca archivio nel caso procedo con la ricerca in db
            if (Session["ListRicercaFiltro"] != null)
            {


                List<string> ListRicerca = (List<string>)Session["ListRicercaFiltro"];
                String[] ar = ListRicerca.ToArray();

                if (Session["ListScadenziario"] != null)
                {
                    // Recupera la DataTable originale dalla Sessione
                    scadenziario = (DataTable)Session["ListScadenziario"];
                }
                if (ar[0].ToString() == "dataArrivo")
                {
                    filtro = $"dataArrivo in ('{HfDataArrivo.Value}')";
                    dv = new DataView(scadenziario);

                    dv.RowFilter = filtro;
                }
                else if (ar[0].ToString() == "dataScadenza")
                {
                    filtro = $"dataScadenza in ('{HfDataScadenza.Value}')";
                    dv = new DataView(scadenziario);

                    dv.RowFilter = filtro;
                }
                else if (ar[0].ToString() == "dataUscita")
                {
                    filtro = $"dataUscita in ('{HfDataUscita.Value}')";
                    dv = new DataView(scadenziario);

                    dv.RowFilter = filtro;
                }
                else
                {


                    switch (ar[1])
                    {
                        case "dataArrivo":


                            filtro = $"dataArrivo in ('{HfDataArrivo.Value}')";
                            dv = new DataView(scadenziario);

                            dv.RowFilter = filtro;

                            break;
                        case "Accoglimento":


                            filtro = $"Esito like '%{HfFiltroEsito.Value}%'";
                            dv = new DataView(scadenziario);

                            dv.RowFilter = filtro;

                            break;
                        case "Differimento":

                            filtro = $"Esito = '{HfFiltroEsito.Value}'";
                            dv = new DataView(scadenziario);

                            dv.RowFilter = filtro;
                            break;
                        case "Diniego Totale":

                            filtro = $"Esito like '%{HfFiltroEsito.Value}%'";
                            dv = new DataView(scadenziario);

                            dv.RowFilter = filtro;

                            break;
                        case "Digiego Parziale/Accoglimento Parziale":

                            filtro = $"Esito like '%{HfFiltroEsito.Value}%'";
                            dv = new DataView(scadenziario);

                            dv.RowFilter = filtro;

                            break;
                        case "Differimento Ulteriore":

                            filtro = $"Esito like '%{HfFiltroEsito.Value}%'";
                            dv = new DataView(scadenziario);

                            dv.RowFilter = filtro;

                            break;
                        case "Tutti":

                            //filtro = $"Esito like '%{HfFiltroEsito.Value}%'";
                            dv = new DataView(scadenziario);

                            dv.RowFilter = filtro;

                            break;
                        case null:
                        case "":
                        case " ":

                            if (!string.IsNullOrEmpty(HfFiltroEsito.Value))
                            {
                                // 1. Pulisco il valore per gestire gli apostrofi (es. L'Aquila -> L''Aquila)
                                string valoreFiltro = HfFiltroEsito.Value.Replace("'", "''");

                                // 2. Applico il filtro
                                dv.RowFilter = $"Esito LIKE '%{valoreFiltro}%'";
                            }
                            else
                            {
                                // 3. Se è vuoto o null, azzero il filtro (mostra tutte le righe)
                                dv.RowFilter = "Esito IS NULL OR Esito = ''";
                            }

                            break;

                    }
                }
                if (scadenziario.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    gvScadenziario.DataSource = dv;
                    gvScadenziario.DataBind();


                }
            }
            else
            {
                //txtPratica.Enabled = true;
                //txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
            }
            return scadenziario;
            // return dt;
        }

        protected void btBack_Click(object sender, EventArgs e)
        {
            gvScadenziario.DataSource = Session["ListScadenziario"];
            gvScadenziario.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicercaScadenziario').modal('show');", true);
        }

        protected void gvRegistro_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Imposta l'indice della riga in modalità modifica
            gvRegistro.EditIndex = e.NewEditIndex;

            // Ricarica i dati per mostrare le TextBox
            btRegistro_Click(sender, e);

        }
        protected void gvRegistro_RowDeleting(object sender, GridViewCancelEditEventArgs e)
        {
            Manager mn = new Manager();
            Boolean resp = mn.DelRegistroById(System.Convert.ToInt32(HfId.Value));
            if (resp)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#TextMessage').text('" + "Registro eliminato correttamente" + "'); $('#MsgModal').modal('show');", true);
                //btOKDup.Enabled = false;
                //btBack_Click(sender, e);
                // Esce dalla modalità modifica
                gvRegistro.EditIndex = -1;
                btRegistro_Click(sender, e);
            }
            // Esce dalla modalità modifica
            gvRegistro.EditIndex = -1;
            btRegistro_Click(sender, e);
        }

        ////////////////////////////////////////

        // 1. EVENTO SCATTATO QUANDO PREMI "MOD."


        // 2. EVENTO SCATTATO QUANDO PREMI "ANNULLA" (X)
        protected void gvRegistro_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Esce dalla modalità modifica
            gvRegistro.EditIndex = -1;
            btRegistro_Click(sender, e);
        }

        // 3. EVENTO SCATTATO QUANDO PREMI "SALVA"
        protected void gvRegistro_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // A. Recupera l'ID del record (da DataKeyNames)
                int idRegistro = Convert.ToInt32(gvRegistro.DataKeys[e.RowIndex].Value);

                // B. Recupera i nuovi valori inseriti nelle TextBox
                // Devi cercare i controlli usando l'ID che hai dato nell'EditItemTemplate
                GridViewRow row = gvRegistro.Rows[e.RowIndex];
                UrpRegistro reg = new UrpRegistro();
                TextBox txtOggetto = (TextBox)row.FindControl("txtOggetto");
                TextBox txtDataPres = (TextBox)row.FindControl("txtDataPres");
                TextBox txtPgTrasmissioneRichiesto = (TextBox)row.FindControl("txtPgTrasmissioneRichiesto");
                TextBox txtUffDetentore = (TextBox)row.FindControl("txtUffDetentore");
                CheckBox chkContro = (CheckBox)row.FindControl("chkControInteressati");
                TextBox txtEsito = (TextBox)row.FindControl("txtEsito");

                reg.controInteressati = chkContro.Checked;
                reg.oggetto = txtOggetto.Text;
                reg.dataPresentRichiesta = Convert.ToDateTime(txtDataPres.Text);
                reg.nrPgTrasmissioneRichiesto = txtPgTrasmissioneRichiesto.Text;
                reg.uffDetentore = txtUffDetentore.Text;
                reg.esito = txtEsito.Text;
                // reg.idRegistro = idRegistro;


                Manager mn = new Manager();
                Boolean resp = mn.UpdateRegistroById(idRegistro, reg);
                if (resp)
                {
                    //richiama popup dalla site master
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        // myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.UpdRegistroOk.GetDescription(), "success");
                        gvRegistro.EditIndex = -1;
                        btRegistro_Click(sender, e);
                    }
                }
                // Messaggio di successo (se usi il popup della master page)
                // Master.MostraMessaggio("Successo", "Dati aggiornati correttamente", "success");
            }
            catch (Exception ex)
            {
                // Gestione Errore
                //richiama popup dalla site master
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.UpdRegistroKo.GetDescription(), "danger");
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine(ex.Message + @" - Errore in gvRegistro_RowUpdating urp.cs ");
                        sw.Close();
                    }

                }
            }

        }

        protected void btChiudiMsgModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('MsgModal')); modal.hide();", true);
            Session.Remove("ListRicercaFiltro");
            Session.Remove("ListScadenziario");
            //string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
            //Response.Redirect(url, false);
        }

        protected void gvRegistro_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // 1. Recupera l'ID del record dalla collezione DataKeys usando l'indice della riga
                // e.RowIndex è l'indice della riga che hai cliccato
                int idDaCancellare = Convert.ToInt32(gvRegistro.DataKeys[e.RowIndex].Value);

                // Costruisco l'URL dove l'utente verrà mandato se clicca "Prosegui"
                string urlAzione = ResolveUrl("~/View/URP.aspx?id=" + idDaCancellare);


                // Richiamo la Master Page
                SiteMaster myMaster = this.Master as SiteMaster;
                if (myMaster != null)
                {
                    myMaster.MostraConferma(
                        "Attenzione",
                        "Sei sicuro di voler eliminare definitivamente questa pratica?",
                        urlAzione
                    );
                }


            }
            catch (Exception ex)
            {
                // Gestione errore
                // SiteMaster master = this.Master as SiteMaster;
                // master.MostraMessaggio("Errore", "Impossibile eliminare: " + ex.Message, "danger");
            }

        }


        protected void btOKCan_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean resp = mn.DelRegistroById(System.Convert.ToInt32(HfId.Value));
            if (resp)
            {
                gvRegistro.EditIndex = -1;
                btRegistro_Click(sender, e);
            }
        }

    }
}