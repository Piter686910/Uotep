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
using Uotep.Classi;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Uotep.Classi.Enumerate;


namespace Uotep
{
    public partial class InserimentoArchivioUotp : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String Ruolo = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Boolean okPopup = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["PaginaChiamante"] != null)

            //    Session.Remove("PaginaChiamante");


            Session["PaginaChiamante"] = "~/View/Uotp/InserimentoArchivio.aspx";
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Ruolo = Session["ruolo"].ToString();

            }
            else
            {

                Response.Redirect("Default.aspx?user=true");
            }

            if (!IsPostBack)
            {
                Routine prot = new Routine();
                txtPratN.Text = Convert.ToInt32(prot.GetPraticaTp()).ToString();
                txtDataInserimentoTp.Text = DateTime.Now.Date.ToShortDateString();
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["TitoloArchivioUote"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                ProtocolloLiteral.Text = decodedText;
                if (Ruolo.ToUpper() != Enumerate.Ruolo.Archivio.ToString().ToUpper() && Ruolo.ToUpper() != Enumerate.Ruolo.Admin.ToString().ToUpper() && Ruolo.ToUpper() != Enumerate.Ruolo.SuperAdmin.ToString().ToUpper())
                {
                    btSalva.Visible = false;
                    btCercaQuartiere.Visible = false;
                }
                //RicercaNew(sender, e);

                CaricaDLL();
                Session["POP"] = "si";

            }
            else
            {
                //aspetta la conferma da parte utente
                if (Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"] == btSalva.UniqueID && hdnConfermaUtente.Value == "true")
                {
                    EseguiAzioneConfermata(); // Chiama la funzione per eseguire l'azione dopo la conferma OK
                    hdnConfermaUtente.Value = "false"; // Resetta il valore del campo nascosto
                    Session["POP"] = "no";
                }
            }

        }
       
        private void EseguiAzioneConfermata()
        {

            okPopup = true;

        }
        protected void gvPopup_RowDataBoundP(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id_Archivio").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
            if (GVRicercaPratica.TopPagerRow != null && GVRicercaPratica.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)GVRicercaPratica.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = GVRicercaPratica.PageIndex + 1;
                    int totalPages = GVRicercaPratica.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
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

                DataTable pratica = mn.getPraticaArchivioUOTPById(idP);
                if (pratica.Rows.Count > 0)
                {
                    FillScheda(pratica);

                }
                Session.Remove("ListRicerca");
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }
        protected void FillScheda(DataTable arc)
        {
            txtPratN.Text = arc.Rows[0].ItemArray[1].ToString();
            //txtSiglaTp.Text = arc.Rows[0].ItemArray[3].ToString();
            txtCartellinaTp.Text = arc.Rows[0].ItemArray[111].ToString();
            //if (!string.IsNullOrEmpty(arc.Rows[0].ItemArray[3].ToString()))
            //{
            //    DateTime dataIntervento = System.Convert.ToDateTime(arc.Rows[0].ItemArray[3].ToString()); // Recupera la data dal DataTable
            //    txtDataInserimento.Text = dataIntervento.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            //                                                                     //  TxtDataIntervento.Text = rap.Rows[0].ItemArray[2].ToString();
            //}
            //if (!string.IsNullOrEmpty(arc.Rows[0].ItemArray[4].ToString()))
            //{
            //    DateTime dataModifica = System.Convert.ToDateTime(arc.Rows[0].ItemArray[4].ToString()); // Recupera la data dal DataTable
            //    txtDataUltimoIntervento.Text = dataModifica.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            //                                                                        //  TxtDataIntervento.Text = rap.Rows[0].ItemArray[2].ToString();
            //}
            //if (!string.IsNullOrEmpty(arc.Rows[0].ItemArray[24].ToString()))
            //{
            //    DateTime dataInizioAtt = System.Convert.ToDateTime(arc.Rows[0].ItemArray[24].ToString()); // Recupera la data dal DataTable
            //    txtDataInizioAttivita.Text = dataInizioAtt.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            //}

            txtNotaTp.Text = arc.Rows[0].ItemArray[11].ToString();
            txtQuartiereTp.Text = arc.Rows[0].ItemArray[13].ToString().ToUpper();

        }
        public Boolean Convalida()
        {
            bool resp = false;

            if (!String.IsNullOrEmpty(txtPratN.Text))
            {
                resp = true;

                if (HfStato.Value != "Mod")
                {


                    //verifica se la pratica sia presente e propongo un popup di conferma se stoinserendo
                    Manager mn = new Manager();
                    String[] ar = new String[2];
                    if (Session["ListRicerca"] != null)
                    {
                        List<string> ListRicerca = (List<string>)Session["ListRicerca"];
                        ar = ListRicerca.ToArray();
                    }
                    else
                    {
                        List<string> ListRicerca = new List<string> { "Pratica", txtPratN.Text };
                        ar = ListRicerca.ToArray();
                    }
                    DataTable dt = mn.getPraticaArchivioUote(ar, null, null, null, null, null);
                    if (dt.Rows.Count > 0)
                    {
                        //  messaggioPopup = @"Dati importanti trovati nel database. Sei sicuro di voler procedere con l'azione?";
                        // **2. Registra JavaScript per mostrare il popup (se necessario)**
                        if (Session["POP"].ToString() == "si")
                        {

                            string script = $@"
            function showConfirmModal() {{
                document.getElementById('modalMessaggioBody').innerText = 'stai modificando un pratica già esistente, confermi?'; 
                $('#confermaModal').modal('show'); // Mostra il modale Bootstrap (jQuery required)
            }}
            window.onload = function() {{ showConfirmModal(); }};
        ";

                            ClientScript.RegisterStartupScript(this.GetType(), "showModalScript", script, true);
                        }




                        //        //se annullo imposto il popup a si
                        if (hdnConfermaUtente.Value == "false")
                        {
                            Session["POP"] = "si";
                        }
                    }
                    else
                        okPopup = true;
                }
                else
                    okPopup = true;
            }


            return resp;
        }

        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean resp = Convalida();
                if (resp)
                {

                    if (okPopup)
                    //se ho ricevuto ok dal popup
                    {

                        okPopup = false;
                        Manager mn = new Manager();

                        ArchivioUotp arch = new ArchivioUotp();
                        arch.arch_Num_Prot = Convert.ToInt32(txtPratN.Text);
                        arch.arch_cartellina = txtCartellinaTp.Text;
                        arch.arch_note = txtNotaTp.Text;
                        arch.arch_quartiere = txtQuartiereTp.Text.ToUpper();
                        arch.arch_codice = txtBUTp.Text.ToUpper();
                        arch.arch_dataArrivo = txtDataProtGen.Text;
                        arch.arch_dataInserimento = txtDataInserimentoTp.Text;
                        arch.arch_oggetto = txtOggettoTp.Text.ToUpper();
                        arch.arch_destinatario = txtDestinatarioTp.Text.ToUpper();
                        arch.arch_ProtGen = txtProGenTp.Text;
                        arch.arch_Protocollo_Procura = txtDataProtProc.Text;
                        arch.arch_dataProtProcura = txtDataProtProc.Text;
                        arch.arch_indirizzo = TxtIndirizzoTp.Text;

                        Boolean ins = mn.SavePraticaArchivioUotp(arch);
                        if (!ins)
                        {
                            errorMessage.InnerText = "Inserimento della pratica non riuscito, controllare il log.";

                            ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
                        }
                        else
                        {
                            if (HfStato.Value == "Mod")

                                errorMessage.InnerText = "Pratica " + arch.arch_Num_Prot + " modificata correttamente .";

                            else
                                errorMessage.InnerText = "Pratica " + arch.arch_Num_Prot + " inserita correttamente .";
                            ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica " + arch.arch_Num_Prot + " inserita correttamente ." + "'); $('#errorModal').modal('show');", true);
                            HfStato.Value = string.Empty;
                            Session["POP"] = "si";
                            Session.Remove("ListRicerca");
                            Pulisci();
                        }
                    }
                }
                else
                {
                    // Mostra il modale con uno script
                    errorMessage.InnerText = @"E' necessario inserire alcuni dati per salvare la pratica.";
                    apripopuperrorModal_Click(sender, e);
                    // ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' necessario inserire alcuni dati per salvare la pratica." + "'); $('#errorModal').modal('show');", true);
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
                    sw.WriteLine(ex.Message + @" - Errore modifica inserimento archivio ");
                    sw.Close();
                }
                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);
                //Response.Redirect("/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/Uotp/InserimentoArchivio.aspx";
                //  Response.Redirect("~/Contact.aspx");

            }
        }
        private void Pulisci()
        {

            txtPratN.Text = String.Empty;
            txtBUTp.Text = String.Empty;
            txtCartellinaTp.Text = String.Empty;
            txtDataInserimentoTp.Text = DateTime.Now.Date.ToShortDateString();
            txtDataProtGen.Text = String.Empty;
            txtDataProtProc.Text = String.Empty;
            txtDestinatarioTp.Text = String.Empty;
            txtNotaTp.Text = String.Empty;
            txtOggettoTp.Text = String.Empty;
            txtDestinatarioTp.Text = String.Empty;


            CaricaDLL();

        }

        protected void apripopupPratica_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalPratica').modal('show');", true);
        }
        protected void apripopuperrorModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }

        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);
            Session.Remove("ListRicerca");
            HfFiltroNote.Value = string.Empty;
            HfFiltroIndirizzo.Value = string.Empty;

        }
        protected void chiudipopupErrore_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('errorModal')); modal.hide();", true);

        }
        protected void RicercaQuartiere_Click(object sender, EventArgs e)
        {
            string indirizzo = string.Empty;

            indirizzo = txtIndirizzoQuartiere.Text.Trim();
            //string specie = txtSpecie.Text.Trim();

            if (!string.IsNullOrEmpty(indirizzo))
            {
                // Simula il recupero del quartiere dal database o da una logica interna.
                Manager mn = new Manager();
                DataTable quartiere = mn.getQuartiere(indirizzo);

                if (quartiere.Rows.Count > 0)
                {
                    gvPopup.DataSource = quartiere;
                    gvPopup.DataBind();

                }

            }

            // Mantieni il popup aperto dopo l'interazione lato server.
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showPopup", "openPopup();", true);

            //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        // (Riusa la funzione GetOriginalData dal mio esempio precedente o la tua logica di recupero dati)

        private void CaricaDLL()
        {
            try
            {
                Manager mn = new Manager();
                DataTable RicercaQuartiere = mn.getListQuartiere();
                DdlQuartiere.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                DdlQuartiere.DataTextField = "Quartiere"; // Il campo visibile
                DdlQuartiere.DataValueField = "id_quartiere";
                DdlQuartiere.DataBind();


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
            }
        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "ID_quartiere").ToString();

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
                string selectedValue = e.CommandArgument.ToString();

                // Imposta il valore nel TextBox
                txtQuartiereTp.Text = selectedValue;
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }

        protected void GVRicercaPratica_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVRicercaPratica.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            if (String.IsNullOrEmpty(HfFiltroNote.Value) && String.IsNullOrEmpty(HfFiltroIndirizzo.Value) && String.IsNullOrEmpty(HfFiltroResponsabile.Value))
            {
                //RicercaNew(sender, e);
            }
            else
            {
                if (!String.IsNullOrEmpty(HfFiltroNote.Value))
                {
                    PopulateGridView("arch_note", HfFiltroNote.Value);
                    apripopupPratica_Click(sender, e);
                }
                else
                {
                    if (!String.IsNullOrEmpty(HfFiltroIndirizzo.Value))
                    {
                        PopulateGridView("arch_indirizzo", HfFiltroIndirizzo.Value);
                        apripopupPratica_Click(sender, e);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(HfFiltroResponsabile.Value))
                        {
                            PopulateGridView("arch_responsabile", HfFiltroResponsabile.Value);
                            apripopupPratica_Click(sender, e);
                        }
                    }
                }
            }


        }
        // esecuzione del filtro ulteriore sulla colonna NOTE
        protected void txtFilterNote_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroNote.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterNote")
            {
                columnName = "arch_note"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroNote.Value); // Esempio di funzione di filtro
            apripopupPratica_Click(sender, e);
        }
        // Funzione di esempio che carica i dati e applica il filtro
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
            ////verifico se provengo da ricerca archivio nel caso procedo con la ricerca in db
            if (Session["ListRicerca"] != null)
            {
                Manager mn = new Manager();
                List<string> ListRicerca = (List<string>)Session["ListRicerca"];
                String[] ar = ListRicerca.ToArray();
                // ArchivioUote arc = new ArchivioUote();

                switch (ar[0])
                {
                    //case "Pratica":
                    //    arc = mn.getPraticaArchivioUote(ar, null, null, null, null, null);
                    //    break;
                    //case "StoricoPratica":
                    //    arc = mn.getPraticaArchivioUote(ar, null, null, null, null, null);
                    //    break;

                    //case "Nominativo":
                    //    arc = mn.getPraticaArchivioUote(null, ar[1], null, null, null, null);
                    //    break;
                    //case "Indirizzo":
                    //    arc = mn.getPraticaArchivioUote(null, null, ar[1], null, null, null);
                    //    break;
                    //case "Catasto":
                    //    arc = mn.getPraticaArchivioUote(null, null, null, ar, null, null);
                    //    break;
                    //case "Note":
                    //    arc = mn.getPraticaArchivioUote(null, null, null, null, ar[1], null);
                    //    break;
                    //case "AnnoMese":
                    //    arc = mn.getPraticaArchivioUote(null, null, null, null, null, ar);
                    //    break;

                }
                if (arc.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    GVRicercaPratica.DataSource = arc;
                    GVRicercaPratica.DataBind();
                    //segnalo he sono in modifica prartica
                    HfStato.Value = "Mod";
                    txtPratN.Enabled = false;
                }
            }
            else
            {
                txtPratN.Enabled = true;
                //txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
            }
            return arc;
            // return dt;
        }
        // esecuzione del filtro ulteriore sulla colonna indirizzo
        protected void txtFilterIndirizzo_TextChanged(object sender, EventArgs e)
        {

            TextBox txtFilter = (TextBox)sender;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroIndirizzo.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterIndirizzo")
            {
                columnName = "arch_indirizzo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroIndirizzo.Value); // Esempio di funzione di filtro
            apripopupPratica_Click(sender, e);
        }

        protected void txtFilterResponsabile_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroResponsabile.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            if (txtFilter.ID == "txtFilterResponsabile")
            {
                columnName = "arch_responsabile"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, HfFiltroResponsabile.Value); // Esempio di funzione di filtro
            apripopupPratica_Click(sender, e);
        }
    }
}