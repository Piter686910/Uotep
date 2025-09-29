using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;

namespace Uotep
{
    public partial class Visualizza : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        Principale p = new Principale(); public String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();

            }

            // Legge il valore dal Web.config
            string protocolloText = ConfigurationManager.AppSettings["Titolo"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            ProtocolloLiteral.Text = decodedText;
            //int protocollo = 0;
            if (!IsPostBack)
            {
                DivDettagli.Visible = false;

            }

        }

        protected void NuovaRicerca_Click(object sender, EventArgs e)
        {
            NascondiDiv();

            Pulisci();
        }
        protected void Ricerca_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable pratica = new DataTable();
            if (txtNProtocollo.Text != string.Empty && txtAnnoRicerca.Text != string.Empty)
            {
                pratica = mn.getListPrototocollo(txtNProtocollo.Text, txtAnnoRicerca.Text);
            }
            if (txtProcPenale.Text != string.Empty)
            {
                pratica = mn.getListProcedimento(txtProcPenale.Text);
            }
            if (txtDataDa.Text != string.Empty && txtDataA.Text != string.Empty)
            {
                pratica = mn.getListEvasaAg(txtDataDa.Text, txtDataA.Text);
            }
            if (txtProtGen.Text != string.Empty)
            {
                pratica = mn.getListProtGen(txtProtGen.Text);
            }
            if (txtPratica.Text != string.Empty)
            {
                pratica = mn.getListPratica(txtPratica.Text);
            }
            if (txtRicGiudice.Text != string.Empty)
            {
                pratica = mn.getListGiudice(txtRicGiudice.Text);
            }
            if (txtRicProvenienza.Text != string.Empty)
            {
                pratica = mn.getListProvenienza(txtRicProvenienza.Text);
            }
            if (txtRicNominativo.Text != string.Empty)
            {
                pratica = mn.getListNominativo(txtRicNominativo.Text);
            }
            if (txtRicAccertatori.Text != string.Empty)
            {
                pratica = mn.getListAccertatori(txtRicAccertatori.Text);
            }
            if (txtRicIndirizzo.Text != string.Empty)
            {
                pratica = mn.getListIndirizzo(txtRicIndirizzo.Text);
            }
            if (txtDatArrivoDa.Text != string.Empty && txtDatArrivoA.Text != string.Empty)
            {
                pratica = mn.getListDataArrivo(txtDatArrivoDa.Text, txtDatArrivoA.Text);
            }
            if (pratica.Rows.Count > 0)
            {
                // Salva datatable pratica  nella Sessione
                Session["ListPratiche"] = pratica;
                gvPopup.DataSource = pratica;
                gvPopup.DataBind();

                DivGrid.Visible = true;
                string a = pratica.Rows[0].ItemArray[1].ToString();
                DataTable decretazione = new DataTable();

                decretazione =  mn.getListDecretazione(pratica.Rows[0].ItemArray[1].ToString(), pratica.Rows[0].ItemArray[0].ToString());
                if (decretazione.Rows.Count > 0)
                {
                    GVDecretazione.DataSource = decretazione;
                    GVDecretazione.DataBind();
                    divDecretazione.Visible = true;
                }
                else
                    divDecretazione.Visible = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non presente in database." + "'); $('#errorModal').modal('show');", true);
            }

        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('errorModal')); modal.hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);
            Pulisci();
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);

        }
        protected void gvPopup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 protocollo = 0;
            string matricola = String.Empty;
            string sigla = String.Empty;
            string dataInserimento = String.Empty;
            if (e.CommandName == "Select")
            {
                try
                {


                    // Ottieni il valore del CommandArgument
                    string commandArgument = e.CommandArgument.ToString();

                    // Separare i valori del CommandArgument usando il delimitatore "|"
                    string[] values = commandArgument.Split('|');

                    // Assicurati che ci siano almeno 3 valori
                    if (values.Length == 5)
                    {
                        protocollo = System.Convert.ToInt32(values[0]);    // Protocollo
                        matricola = values[1];     // Matricola
                        dataInserimento = values[2]; // DataInserimento
                        sigla = values[3];  // sigla
                        HidPratica.Value = values[4]; // id


                        //// Ora puoi usare questi valori per aggiornare i tuoi controlli
                        //p.nrProtocollo = System.Convert.ToInt32(protocollo);
                        //p.matricola = matricola;
                        if (!String.IsNullOrEmpty(dataInserimento))
                        {
                            p.data_ins_pratica = System.Convert.ToDateTime(dataInserimento).ToLocalTime();


                        }
                        Manager mn = new Manager();
                        DataTable pratica = mn.getPraticaId(protocollo, System.Convert.ToDateTime(dataInserimento), sigla, System.Convert.ToInt32(HidPratica.Value));
                        if (pratica.Rows.Count > 0)
                        {
                            txtProt.Text = pratica.Rows[0].ItemArray[1].ToString() + " - " + pratica.Rows[0].ItemArray[2].ToString();
                            // txtSigla.Text = pratica.Rows[0].ItemArray[2].ToString();
                            if (pratica.Rows[0].ItemArray[2].ToString() == Enumerate.Sigla.AG.ToString().ToUpper())
                            {
                                divAg.Visible = true;
                            }
                            else
                            {
                                divAg.Visible = false;

                            }
                            if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[3].ToString()))

                                txtDataInsCarico.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[3].ToString()).ToShortDateString();


                            txtProvenienza.Text = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                            txtProvenienza.ToolTip = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                            txtTipoAtto.Text = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                            txtTipoAtto.ToolTip = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                            txtGiudice.Text = pratica.Rows[0].ItemArray[6].ToString().ToUpper();
                            TxtTipoProvvAg.Text = pratica.Rows[0].ItemArray[7].ToString();
                            TxtTipoProvvAg.ToolTip = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
                            txtProdPenNr.Text = pratica.Rows[0].ItemArray[8].ToString();
                            txtNominativo.Text = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                            txtNominativo.ToolTip = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                            txtIndirizzo.Text = pratica.Rows[0].ItemArray[10].ToString().ToUpper() + " " + pratica.Rows[0].ItemArray[11].ToString().ToUpper();
                            txtIndirizzo.ToolTip = pratica.Rows[0].ItemArray[10].ToString().ToUpper();

                            CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0].ItemArray[12]);

                            if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[13].ToString()))
                            {
                                //converte la data 01-01-1900 in SPACE
                                DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()); // Recupera la data dal DataTable
                                if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                                {
                                    txtDataDataEvasa.Text = ""; // Metti una stringa vuota
                                }
                                else
                                {
                                    txtDataDataEvasa.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                                }
                            }
                            //  txtDataDataEvasa.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()).ToShortDateString();


                            //     txtinviata.Text = pratica.Rows[0].ItemArray[14].ToString().ToUpper();

                            //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[15].ToString()))

                            //    txtDataInvio.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[15].ToString()).ToShortDateString();

                            txtEsito.Text = pratica.Rows[0].ItemArray[16].ToString().ToUpper();
                            txtEsito.ToolTip = pratica.Rows[0].ItemArray[16].ToString().ToUpper();
                            if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[17].ToString()))
                            {


                                txtAccertatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
                                txtAccertatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
                            }
                            if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[18].ToString()))
                            {
                                //converte la data 01-01-1900 in SPACE
                                DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()); // Recupera la data dal DataTable
                                if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                                {
                                    txtDataCarico.Text = ""; // Metti una stringa vuota
                                }
                                else
                                {
                                    txtDataCarico.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                                }
                            }
                            //txtDataCarico.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()).ToShortDateString();

                            txtPraticaOut.Text = pratica.Rows[0].ItemArray[19].ToString();
                            TxtQuartiere.Text = pratica.Rows[0].ItemArray[20].ToString();
                            //txtNote.Text = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                            //txtNote.ToolTip = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                            txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[22].ToString();
                            //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                            txtRifProtGen.Text = pratica.Rows[0].ItemArray[24].ToString();
                            if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[27].ToString()))
                            {
                                txtAreaCompetenza.Text = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
                                txtAreaCompetenza.ToolTip = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
                            }
                            // Puoi anche chiudere il popup se necessario
                            ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#ModalRicerca').modal('hide');", true);
                            DivDettagli.Visible = true;
                            DivRicerca.Visible = false;
                            Pulisci();
                        }
                        else
                        {

                            //lblmessage.Text = "Pratica non trovata";
                        }
                    }
                }
                catch (Exception ex)
                {

                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("dataInserimento:" + dataInserimento + ",data ins:" + p.data_ins_pratica + ", " + ex.Message + @" - Errore in update dati ");
                        sw.Close();
                    }
                }
            }
        }
        private void Pulisci()
        {
            txtAnnoRicerca.Text = String.Empty;
            txtNProtocollo.Text = String.Empty;
            txtProcPenale.Text = String.Empty;
            //date per div evasa
            txtDataDa.Text = String.Empty;
            txtDataA.Text = String.Empty;
            //
            txtProtGen.Text = String.Empty;
            txtPratica.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtDatArrivoA.Text = String.Empty;
            txtDatArrivoDa.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;

        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "Nr_Protocollo").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
            if (gvPopup.TopPagerRow != null)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)gvPopup.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = gvPopup.PageIndex + 1;
                    int totalPages = gvPopup.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }

        }

        protected void btNProtocollo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProtocollo.Visible = true;
        }

        protected void btProcPenale_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProcPenale.Visible = true;
        }

        protected void btEvaseAg_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivEvasaAg.Visible = true;
        }

        protected void btProtGen_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProtGen.Visible = true;
        }

        protected void btNpratica_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivPratica.Visible = true;
        }

        protected void btGiudice_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivGiudice.Visible = true;
        }

        protected void btProvenienza_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProvenienza.Visible = true;
        }

        protected void btNominativo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivNominativo.Visible = true;
        }

        protected void btAccertatori_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivAccertatori.Visible = true;

        }

        protected void btIndirizzo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivIndirizzo.Visible = true;
        }
        protected void btDataCarico_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivDataArrivo.Visible = true;
        }
        public void NascondiDiv()
        {
            DivRicerca.Visible = true;
            DivProtocollo.Visible = false;
            DivProcPenale.Visible = false;
            DivEvasaAg.Visible = false;
            DivProtGen.Visible = false;
            DivPratica.Visible = false;
            DivGiudice.Visible = false;
            DivProvenienza.Visible = false;
            DivNominativo.Visible = false;
            DivAccertatori.Visible = false;
            DivIndirizzo.Visible = false;
            DivDataArrivo.Visible = false;
            DivDettagli.Visible = false;
            Session.Remove("ListPratiche");
        }
        protected void gvPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPopup.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            Ricerca_Click(sender, e);
        }
        protected void txtFilterNominativo_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "Nominativo", txtRicNominativo.Text };
            // Salva la lista nella Sessione
            Session["ListRicerca"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroNominativo.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterNominativo")
            {
                columnName = "Nominativo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroNominativo.Value); // Esempio di funzione di filtro
                                                                    //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }

        protected void txtFilterAccertatori_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "Accertatori", txtRicAccertatori.Text };

            // Salva la lista nella Sessione
            Session["ListRicerca"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroAccertatori.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterAccertatori")
            {
                columnName = "Accertatori"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroAccertatori.Value); // Esempio di funzione di filtro
                                                                     //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }
        // esecuzione del filtro ulteriore sulla colonna indirizzo
        protected void txtFilterIndirizzo_TextChanged(object sender, EventArgs e)
        {

            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "Indirizzo", txtRicIndirizzo.Text };
            // Salva la lista nella Sessione
            Session["ListRicerca"] = ListRicerca;

            string filterValue = txtFilter.Text.Trim();
            HfFiltroIndirizzo.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterIndirizzo")
            {
                columnName = "indirizzo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroIndirizzo.Value); // Esempio di funzione di filtro
                                                                   //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);

        }
        // Funzione  che carica i dati e applica il filtro
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
                        gvPopup.DataSource = filteredDt;
                    }
                    else
                    {
                        gvPopup.DataSource = null;
                    }

                }
                else
                {
                    gvPopup.DataSource = dt; // Nessun filtro
                }
                gvPopup.DataBind();
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                // throw;
            }
        }
        //gridview per decretazione
        protected void GVDecretazione_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }


            GVDecretazione.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            //Decretazione_Click(sender, e);

        }
        protected void GVDecretazione_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Ottieni il valore della colonna "ID"
            //    string id = DataBinder.Eval(e.Row.DataItem, "ID_quartiere").ToString();

            //    // Aggiungi l'attributo per il doppio clic
            //    e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
            //    e.Row.Style["cursor"] = "pointer";
            //}
            if (GVDecretazione.TopPagerRow != null && GVDecretazione.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)GVDecretazione.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = GVDecretazione.PageIndex + 1;
                    int totalPages = GVDecretazione.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
        }
        protected void GVDecretazione_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Select")
            //{
            //    // Ottieni il valore dell'ID dalla CommandArgument
            //    string selectedValue = e.CommandArgument.ToString();

            //    // Imposta il valore nel TextBox
            //    //txtSelectedValue.Text = selectedValue;
            //    txtIndirizzo.Text = selectedValue;
            //    // Chiudi il popup
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            //}
        }
        private DataTable GetOriginalData()
        {
            DataTable pratica = new DataTable();
            DataView dv = new DataView();
            Manager mn = new Manager();
            string filtro = string.Empty;
            ////verifico se provengo da ricerca archivio nel caso procedo con la ricerca in db
            if (Session["ListRicerca"] != null)
            {


                List<string> ListRicerca = (List<string>)Session["ListRicerca"];
                String[] ar = ListRicerca.ToArray();
                // ArchivioUote arc = new ArchivioUote();
                if (Session["ListPratiche"] != null)
                {
                    // Recupera la DataTable originale dalla Sessione
                    pratica = (DataTable)Session["ListPratiche"];
                }
                switch (ar[0])
                {
                    case "Nominativo":


                        filtro = $"Nominativo LIKE '%{HfFiltroNominativo.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;

                        break;
                    case "Indirizzo":

                        filtro = $"Nominativo LIKE '%{HfFiltroNominativo.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;
                        break;
                    case "Accertatori":

                        filtro = $"Accertatori LIKE '%{HfFiltroAccertatori.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;

                        break;


                }
                if (pratica.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    gvPopup.DataSource = dv;
                    gvPopup.DataBind();

                    txtPratica.Enabled = false;
                }
            }
            else
            {
                //txtPratica.Enabled = true;
                //txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
            }
            return pratica;
            // return dt;
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
    }
}