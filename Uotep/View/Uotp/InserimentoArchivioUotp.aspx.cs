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
        
        String status = String.Empty;   
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["PaginaChiamante"] != null)

            //    Session.Remove("PaginaChiamante");

            HfStato.Value = Request.QueryString["status"];
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
                //txtPratN.Text = Convert.ToInt32(prot.GetPraticaTp()).ToString();
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
                    //btCercaQuartiere.Visible = false;
                }
                //RicercaNew(sender, e);

                CaricaDLL();

                if (HfStato.Value == "M")
                {
                    if (Session["arc"] != null)
                    {
                        DataTable arc = (DataTable)Session["arc"];
                        FillScheda(arc);
                    }
                }
                else
                    Session.Remove("arc");
            }
            else
            {
                //aspetta la conferma da parte utente
                //if (Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"] == btSalva.UniqueID && hdnConfermaUtente.Value == "true")
                //{
                //    EseguiAzioneConfermata(); // Chiama la funzione per eseguire l'azione dopo la conferma OK
                //    hdnConfermaUtente.Value = "false"; // Resetta il valore del campo nascosto
                //    Session["POP"] = "no";
                //}
            }

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
        //protected void gvPopup_RowCommandP(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Select")
        //    {
        //        // Ottieni il valore dell'ID dalla CommandArgument
        //        //string selectedValue = e.CommandArgument.ToString();


        //        string[] args = e.CommandArgument.ToString().Split(';');
        //        int idP = System.Convert.ToInt32(args[0]);
        //        string Npratica = args[1];


        //        // Imposta il valore nel TextBox
        //        //txtSelectedValue.Text = selectedValue;
        //        txtPratN.Text = Npratica;

        //        Manager mn = new Manager();

        //        DataTable pratica = mn.getPraticaArchivioUOTPById(idP);
        //        if (pratica.Rows.Count > 0)
        //        {
        //            FillScheda(pratica);

        //        }
        //        Session.Remove("ListRicerca");
        //        // Chiudi il popup
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
        //    }
        //}
        protected void FillScheda(DataTable arc)
        {
            //            txtNumProTp.Text = arc.Rows[0].ItemArray[1].ToString();
            //  txtProGenTp.Text = arc.Rows[0].ItemArray[107].ToString();
            //  txtProProcTp.Text = arc.Rows[0].ItemArray[115].ToString().ToUpper();
            txtCognomeTp.Text = arc.Rows[0].ItemArray[45].ToString();
            txtBUAlloggioTp.Text = arc.Rows[0].ItemArray[42].ToString().ToUpper();
            txtCartellinaTp.Text = arc.Rows[0].ItemArray[111].ToString().ToUpper();
            txtNotaTp.Text = arc.Rows[0].ItemArray[104].ToString().ToUpper();
            txtNotaTp.ToolTip = arc.Rows[0].ItemArray[104].ToString().ToUpper();
            txtOggettoTp.Text = arc.Rows[0].ItemArray[19].ToString().ToUpper() ;
            txtOggettoTp.ToolTip = arc.Rows[0].ItemArray[19].ToString().ToUpper() ;
            txtOggettoTp2.Text =  arc.Rows[0].ItemArray[20].ToString().ToUpper();
            txtOggettoTp2.ToolTip = arc.Rows[0].ItemArray[20].ToString().ToUpper();
            txtDestinatarioTp.Text = arc.Rows[0].ItemArray[27].ToString().ToUpper();
            txtDestinatarioTp.ToolTip = arc.Rows[0].ItemArray[27].ToString().ToUpper();
            txtBuEdificioTp.Text = arc.Rows[0].ItemArray[37].ToString().ToUpper();
            string val = string.Empty;
            for (int i = 0; i < DdlQuartiere.Items.Count; i++)
            {
                if (DdlQuartiere.Items[i].Text == arc.Rows[0].ItemArray[40].ToString().ToUpper())
                {
                    val = DdlQuartiere.Items[i].Value;


                }
            }
            DdlQuartiere.SelectedValue = val;


            //txtDataProtProc.Text = arc.Rows[0].ItemArray[116].ToString();// data.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            TxtIndirizzoTp.Text = arc.Rows[0].ItemArray[47].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[48].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[49].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[50].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[51].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[52].ToString().ToUpper();
            TxtIndirizzoTp.ToolTip = arc.Rows[0].ItemArray[47].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[48].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[49].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[50].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[51].ToString().ToUpper() + " " + arc.Rows[0].ItemArray[52].ToString().ToUpper();
        }
      

        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
              
                Manager mn = new Manager();

                ArchivioUotp arch = new ArchivioUotp();
                // arch.arch_Num_Prot = Convert.ToInt32(txtPratN.Text);
                arch.arch_cartellina = txtCartellinaTp.Text;
                arch.arch_note = txtNotaTp.Text.ToUpper();
                // arch.arch_quartiere = txtQuartiereTp.Text.ToUpper();
                arch.arch_quartiere = DdlQuartiere.SelectedItem.Text.ToUpper();
                arch.arch_codice = txtBUAlloggioTp.Text.ToUpper();
                arch.arch_edificio = txtBuEdificioTp.Text.ToUpper();

                // arch.arch_dataArrivo = txtDataProtGen.Text;
                arch.arch_dataInserimento = txtDataInserimentoTp.Text;
                arch.arch_oggetto = txtOggettoTp.Text.ToUpper();
                arch.arch_oggetto2 = txtOggettoTp2.Text.ToUpper();
                arch.arch_destinatario = txtDestinatarioTp.Text.ToUpper();
                // arch.arch_ProtGen = txtProGenTp.Text;
                // arch.arch_Protocollo_Procura = txtDataProtProc.Text;
                // arch.arch_dataProtProcura = txtDataProtProc.Text;
                arch.arch_indirizzo = TxtIndirizzoTp.Text;
                arch.arch_cognome = txtCognomeTp.Text.ToUpper();

                Boolean ins = false;


                if (HfStato.Value == "M")
                {
                    ins = mn.UpdPraticaArchivioUotp(arch);
                    
                }
                else
                    ins = mn.SavePraticaArchivioUotp(arch);



//                Boolean ins = mn.SavePraticaArchivioUotp(arch);
                if (!ins)
                {
                    errorMessage.InnerText = "Inserimento della pratica non riuscito, controllare il log.";

                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    if (HfStato.Value == "M")

                        errorMessage.InnerText = "Pratica " + arch.arch_cartellina + " modificata correttamente .";

                    else
                        errorMessage.InnerText = "Pratica " + arch.arch_cartellina + " inserita correttamente .";
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica " + errorMessage.InnerText + " inserita correttamente ." + "'); $('#errorModal').modal('show');", true);
                    HfStato.Value = string.Empty;
                    Session["POP"] = "si";
                    Session.Remove("ListRicerca");
                    Session.Remove("arc");
                    Pulisci();
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

            //  txtPratN.Text = String.Empty;
            txtBUAlloggioTp.Text = String.Empty;
            txtCartellinaTp.Text = String.Empty;
            txtDataInserimentoTp.Text = DateTime.Now.Date.ToShortDateString();
            //  txtDataProtGen.Text = String.Empty;
            //  txtDataProtProc.Text = String.Empty;
            txtDestinatarioTp.Text = String.Empty;
            txtNotaTp.Text = String.Empty;
            txtOggettoTp.Text = String.Empty;
            txtOggettoTp2.Text = String.Empty;
            txtDestinatarioTp.Text = String.Empty;
            txtCognomeTp.Text = String.Empty;
            txtBuEdificioTp.Text = string.Empty;
            //            DdlQuartiere.ClearSelection();


            CaricaDLL();

        }

        protected void apripopupPratica_Click(object sender, EventArgs e)
        {
            // ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalPratica').modal('show');", true);
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
        private void CaricaDLL()
        {
            try
            {
                Manager mn = new Manager();
                DataTable RicercaQuartiere = mn.getListQuartiereTP();
                DdlQuartiere.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                DdlQuartiere.DataTextField = "Quartiere"; // Il campo visibile
                DdlQuartiere.DataValueField = "id";
                DdlQuartiere.DataBind();
                DdlQuartiere.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

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
                // txtQuartiereTp.Text = selectedValue;
                DdlQuartiere.SelectedValue = selectedValue;
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
                // ArchivioUote arc = new ArchivioUote();


                if (arc.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    GVRicercaPratica.DataSource = arc;
                    GVRicercaPratica.DataBind();
                    //segnalo he sono in modifica prartica
                    //           HfStato.Value = "Mod";
                    // txtPratN.Enabled = false;
                }
            }
            else
            {
                // txtPratN.Enabled = true;
            }
            return arc;
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


        protected void DdlQuartiere_SelectedIndexChanged(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            //  int cartellina = mn.GetCartellinaByQuartiere(txtQuartiereTp.Text.ToUpper());
            int cartellina = mn.GetCartellinaByQuartiere(DdlQuartiere.SelectedItem.Text.ToUpper());
            txtCartellinaTp.Text = cartellina.ToString();
        }


    }
}