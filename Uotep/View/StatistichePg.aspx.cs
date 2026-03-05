using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static System.Windows.Forms.AxHost;




namespace Uotep
{
    public partial class StatistichePg : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Manager mn = new Manager();
        String profilo = string.Empty;
        String ruolo = string.Empty;
        Interrogatorio interrogatorio = new Interrogatorio();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                profilo = Session["profilo"].ToString();
                ruolo = Session["ruolo"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]) && !string.IsNullOrWhiteSpace(Request.QueryString["anno"]) && !string.IsNullOrWhiteSpace(Request.QueryString["mese"]))
            {
                Manager mn = new Manager();
                Boolean resp = mn.DelInterrogatorioById(System.Convert.ToInt32(Request.QueryString["id"]));
                if (resp)
                {

                    Interrogatorio interrogatorio = new Interrogatorio();
                    interrogatorio.Anno = Convert.ToInt32(Request.QueryString["anno"]);
                    interrogatorio.Mese = Convert.ToString(Request.QueryString["mese"]);

                    DataTable dt = mn.getListInterrogatori(interrogatorio);
                    if (dt.Rows.Count > 0)
                    {
                        gvInterrogatori.DataSource = dt;
                        gvInterrogatori.DataBind();
                    }
                    else
                    {
                        gvInterrogatori.DataSource = null;
                        gvInterrogatori.DataBind();
                    }
                    // Esce dalla modalità modifica
                    gvInterrogatori.EditIndex = -1;

                    // Lancia script per pulire URL
                    string script = "window.history.replaceState(null, null, window.location.pathname);";
                    ScriptManager.RegisterStartupScript(this, GetType(), "PulisciUrl", script, true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalInterrogatori').modal('show');", true);
                }
                // Esce dalla modalità modifica
                //gvRegistro.EditIndex = -1;

            }
            txtYYYY.Text = Convert.ToString(DateTime.Now.Year);
            if (!IsPostBack)
            {
                //  ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);

            }

        }

        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);

        }


        public Interrogatorio CaricaInterrogatorio()
        {

            interrogatorio.Npratica = string.IsNullOrWhiteSpace(txtPratica.Text.Trim()) ? string.Empty : txtPratica.Text.Trim();//piero
            interrogatorio.Nominativo1 = string.IsNullOrWhiteSpace(txtNominativo1.Text.Trim()) ? string.Empty : txtNominativo1.Text.Trim();
            interrogatorio.Nominativo2 = string.IsNullOrWhiteSpace(txtNominativo2.Text.Trim()) ? string.Empty : txtNominativo2.Text.Trim();
            interrogatorio.Nominativo3 = string.IsNullOrWhiteSpace(txtNominativo3.Text.Trim()) ? string.Empty : txtNominativo3.Text.Trim();
            interrogatorio.Nominativo4 = string.IsNullOrWhiteSpace(txtNominativo4.Text.Trim()) ? string.Empty : txtNominativo4.Text.Trim();
            interrogatorio.ProcPenale = string.IsNullOrWhiteSpace(txtProcPen.Text.Trim()) ? string.Empty : txtProcPen.Text.Trim();
            if (!string.IsNullOrEmpty(txtDataInterr.Text))
            {
                interrogatorio.DataInterrogatorio = System.Convert.ToDateTime(txtDataInterr.Text);
            }
            interrogatorio.Matricola = Vuser;
            interrogatorio.ProcPenale = txtProcPen.Text.Trim();
            interrogatorio.DataInserimento = DateTime.Now;
            interrogatorio.Mese = ddlMese.SelectedItem.Text;
            interrogatorio.Anno = System.Convert.ToInt32(txtYYYY.Text.Trim());
            return interrogatorio;

        }
        protected void btInserisci_Click(object sender, EventArgs e)
        {
            Boolean exist = false;
            Boolean resp = false;
            Interrogatorio interrogatorio = new Interrogatorio();
            SiteMaster myMaster = this.Master as SiteMaster;
            if (!String.IsNullOrEmpty(Vuser))
            {
                interrogatorio = CaricaInterrogatorio();
                Statistiche stat = new Statistiche();
                DataTable dt = new DataTable();
                int anno = System.Convert.ToInt32(txtYYYY.Text.Trim());
                int interrogatori = System.Convert.ToInt32(txtInterrogatorio.Text.Trim());
                switch (ddlMese.SelectedItem.Text)
                {
                    case "Seleziona mese":


                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.SelKo.GetDescription(), "warning");
                        }
                        break;
                    default:
                        dt = mn.getStatisticaByMeseAnno(ddlMese.SelectedItem.Text, anno);

                        if (dt.Rows.Count > 0)
                        {
                            exist = true; //eseguo update del campo interrogatori
                            interrogatori += System.Convert.ToInt32(dt.Rows[0].ItemArray[19]);
                            stat.interrogazioni = interrogatori;
                            stat.mese = ddlMese.SelectedItem.Text;
                            stat.anno = anno;
                            resp = mn.InsStatPg(exist, stat, interrogatorio);
                            if (resp)
                            {


                                //if (myMaster != null)
                                //{
                                //    // 2. Chiamo il metodo pubblico
                                //    myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.ModificaCorretta.GetDescription(), "success");
                                //}
                                btCerca_Click(sender, e);
                            }
                            else
                            {


                                if (myMaster != null)
                                {
                                    // 2. Chiamo il metodo pubblico
                                    myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.UpdInterrogatorioKo.GetDescription(), "success");
                                }
                            }
                        }
                        else
                        {
                            //non esiste il nuovo mese anno quindi inserisco un nuovo record
                            stat.interrogazioni = interrogatori;
                            stat.mese = ddlMese.SelectedItem.Text;
                            stat.anno = anno;
                            resp = mn.InsStatPg(exist, stat, interrogatorio);
                            if (resp)
                            {


                                if (myMaster != null)
                                {
                                    // 2. Chiamo il metodo pubblico
                                    myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.InsOk.GetDescription(), "success");
                                }
                            }
                        }

                        break;
                }
                Pulisci();
            }
            else
            {
                // Se l'utente non è autenticato, reindirizza alla pagina di login


                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true"); //segnalo alla pagina di default che la user è vuota
                    Response.Redirect(url, false);
                    return;
                }
            }
        }

        protected void btCerca_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Manager mn = new Manager();
            SiteMaster myMaster = this.Master as SiteMaster;
            Interrogatorio interrogatorio = CaricaInterrogatorio();

            dt = mn.getListInterrogatori(interrogatorio);
            if (dt.Rows.Count > 0)
            {
                gvInterrogatori.DataSource = dt;
                gvInterrogatori.DataBind();
            }
            else
            {
                gvInterrogatori.DataSource = null;
                gvInterrogatori.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalInterrogatori').modal('show');", true);
        }

        protected void gvInterrogatori_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "ID").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";

                if (gvInterrogatori.TopPagerRow != null)
                {
                    // Trova il controllo Label all'interno del PagerTemplate
                    Label lblPageInfo = (Label)gvInterrogatori.TopPagerRow.FindControl("lblPageInfo");
                    if (lblPageInfo != null)
                    {
                        // Calcola e imposta il testo
                        int currentPage = gvInterrogatori.PageIndex + 1;
                        int totalPages = gvInterrogatori.PageCount;
                        lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                    }
                }


            }
        }

        protected void gvInterrogatori_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }


            gvInterrogatori.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            btCerca_Click(sender, e);
        }

        protected void gvInterrogatori_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Imposta l'indice della riga in modalità modifica
            gvInterrogatori.EditIndex = e.NewEditIndex;

            // Ricarica i dati per mostrare le TextBox
            btCerca_Click(sender, e);
        }

        protected void gvInterrogatori_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Esce dalla modalità modifica
            gvInterrogatori.EditIndex = -1;
            // Ricarica i dati per mostrare le TextBox
            btCerca_Click(sender, e);
        }
        //protected void gvInterrogatori_RowDeleting(object sender, GridViewCancelEditEventArgs e)
        //{
        //    Manager mn = new Manager();
        //    Boolean resp = mn.DelInterrogatotioById(System.Convert.ToInt32(HfId.Value));
        //    if (resp)
        //    {

        //        // Esce dalla modalità modifica
        //        gvInterrogatori.EditIndex = -1;

        //    }
        //    // Esce dalla modalità modifica
        //    gvInterrogatori.EditIndex = -1;
        //    btCerca_Click(sender, e);
        //}
        protected void gvInterrogatori_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // 1. Recupera l'ID del record dalla collezione DataKeys usando l'indice della riga
                // e.RowIndex è l'indice della riga che hai cliccato
                int idDaCancellare = Convert.ToInt32(gvInterrogatori.DataKeys[e.RowIndex].Value);

                // Costruisco l'URL dove l'utente verrà mandato se clicca "Prosegui"
                string urlAzione = ResolveUrl("~/View/StatistichePg.aspx?id=" + idDaCancellare + "&anno=" + txtYYYY.Text.Trim() + "&mese=" + ddlMese.SelectedItem.Text);

                //   HfId.Value = Convert.ToString(idDaCancellare);

                // Richiamo la Master Page
                SiteMaster myMaster = this.Master as SiteMaster;
                if (myMaster != null)
                {
                    myMaster.MostraConferma(
                        "Attenzione",
                        "Sei sicuro di voler eliminare definitivamente questa riga?",
                        urlAzione
                    );
                }
                //btCerca_Click(sender, e);

            }
            catch (Exception ex)
            {
                // Gestione errore
                // SiteMaster master = this.Master as SiteMaster;
                // mast
            }
        }

        protected void gvInterrogatori_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // A. Recupera l'ID del record (da DataKeyNames)
                int idInterrogatorio = Convert.ToInt32(gvInterrogatori.DataKeys[e.RowIndex].Value);

                // B. Recupera i nuovi valori inseriti nelle TextBox
                // Devi cercare i controlli usando l'ID che hai dato nell'EditItemTemplate
                GridViewRow row = gvInterrogatori.Rows[e.RowIndex];
                UrpRegistro reg = new UrpRegistro();
                TextBox txtProcPenGrid = (TextBox)row.FindControl("txtProcPenGrid");
                TextBox txtDataInterrogatorioGrid = (TextBox)row.FindControl("txtDataInterrogatorioGrid");
                TextBox txtNPraticaGrid = (TextBox)row.FindControl("txtNPraticaGrid");
                TextBox txtNominativo1Grid = (TextBox)row.FindControl("txtNominativo1Grid");
                TextBox txtNominativo2Grid = (TextBox)row.FindControl("txtNominativo2Grid");
                TextBox txtNominativo3Grid = (TextBox)row.FindControl("txtNominativo3Grid");
                TextBox txtNominativo4Grid = (TextBox)row.FindControl("txtNominativo4Grid");

                TextBox txtDataInserimentoGrid = (TextBox)row.FindControl("txtDataInserimentoGrid");

                interrogatorio.Npratica = string.IsNullOrWhiteSpace(txtNPraticaGrid.Text.Trim()) ? string.Empty : txtNPraticaGrid.Text.Trim();
                interrogatorio.Nominativo1 = string.IsNullOrWhiteSpace(txtNominativo1Grid.Text.Trim()) ? string.Empty : txtNominativo1Grid.Text.Trim();
                interrogatorio.Nominativo2 = string.IsNullOrWhiteSpace(txtNominativo2Grid.Text.Trim()) ? string.Empty : txtNominativo2Grid.Text.Trim();
                interrogatorio.Nominativo3 = string.IsNullOrWhiteSpace(txtNominativo3Grid.Text.Trim()) ? string.Empty : txtNominativo3Grid.Text.Trim();
                interrogatorio.Nominativo4 = string.IsNullOrWhiteSpace(txtNominativo4Grid.Text.Trim()) ? string.Empty : txtNominativo4Grid.Text.Trim();
                interrogatorio.ProcPenale = string.IsNullOrWhiteSpace(txtProcPenGrid.Text.Trim()) ? string.Empty : txtProcPenGrid.Text.Trim();
                if (!string.IsNullOrEmpty(txtDataInterrogatorioGrid.Text))
                {
                    interrogatorio.DataInterrogatorio = System.Convert.ToDateTime(txtDataInterrogatorioGrid.Text);
                }
                interrogatorio.Matricola = Vuser;
                interrogatorio.Mese = ddlMese.SelectedItem.Text;
                interrogatorio.Anno = System.Convert.ToInt32(txtYYYY.Text.Trim());

                Manager mn = new Manager();
                Boolean resp = mn.UpdateInterrogatorioId(idInterrogatorio, interrogatorio);
                if (resp)
                {
                    //richiama popup dalla site master
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        // myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.UpdRegistroOk.GetDescription(), "success");
                        gvInterrogatori.EditIndex = -1;
                        btCerca_Click(sender, e);
                    }
                }

            }
            catch (Exception ex)
            {
                // Gestione Errore
                //richiama popup dalla site master
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("ATTENZIONE", Enumerate.MsgOutput.UpdInterrogatorioKo.GetDescription(), "danger");
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine(ex.Message + @" - Errore in gvInterrogatori_RowUpdating statistichepg.cs ");
                        sw.Close();
                    }

                }
            }
        }
        private void Pulisci()
        {

            ddlMese.ClearSelection();
            txtYYYY.Text = Convert.ToString(DateTime.Now.Year);
            txtInterrogatorio.Text = string.Empty;
            txtDataInterr.Text = string.Empty;
            txtPratica.Text = string.Empty;
            txtProcPen.Text = string.Empty;
            txtNominativo1.Text = String.Empty;
            txtNominativo2.Text = String.Empty;
            txtNominativo3.Text = String.Empty;
            txtNominativo4.Text = String.Empty;

        }
    }
}