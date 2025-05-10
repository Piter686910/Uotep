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


namespace Uotep
{
    public partial class InserimentoArchivio : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String Ruolo = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        Boolean okPopup = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PaginaChiamante"] != null)

                Session.Remove("PaginaChiamante");

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Ruolo = Session["ruolo"].ToString();

            }
            // Legge il valore dal Web.config
            string protocolloText = ConfigurationManager.AppSettings["TitoloArchivioUote"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            ProtocolloLiteral.Text = decodedText;
            if (!IsPostBack)
            {

                //verifico se provengo da ricerca archivio nel caso procedo con la ricerca in db
                if (Session["ListRicerca"] != null)
                {
                    Manager mn = new Manager();
                    List<string> ListRicerca = (List<string>)Session["ListRicerca"];
                    String[] ar = ListRicerca.ToArray();
                    // ArchivioUote arc = new ArchivioUote();
                    DataTable arc = new DataTable();
                    switch (ar[0])
                    {
                        case "Pratica":
                            arc = mn.getPraticaArchivioUote(ar, null, null, null, null, null);
                            break;
                        case "StoricoPratica":
                            arc = mn.getPraticaArchivioUote(ar, null, null, null, null, null);
                            break;

                        case "Nominativo":
                            arc = mn.getPraticaArchivioUote(null, ar[1], null, null, null, null);
                            break;
                        case "Indirizzo":
                            arc = mn.getPraticaArchivioUote(null, null, ar[1], null, null, null);
                            break;
                        case "Catasto":
                            arc = mn.getPraticaArchivioUote(null, null, null, ar, null, null);
                            break;
                        case "Note":
                            arc = mn.getPraticaArchivioUote(null, null, null, null, ar[1], null);
                            break;
                        case "AnnoMese":
                            arc = mn.getPraticaArchivioUote(null, null, null, null, null, ar);
                            break;

                    }
                    if (arc.Rows.Count > 0)
                    {
                        apripopupPratica_Click(sender, e);
                        GVRicercaPratica.DataSource = arc;
                        GVRicercaPratica.DataBind();
                        //segnalo he sono in modifica prartica
                        HfStato.Value = "Mod";
                        txtPratica.Enabled = false;
                    }
                }
                else
                {
                    txtPratica.Enabled = true;
                    txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
                }
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
                txtPratica.Text = Npratica;

                Manager mn = new Manager();
                //DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);

                DataTable pratica = mn.getPraticaArchivioUoteById(idP);
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


            if (!string.IsNullOrEmpty(arc.Rows[0].ItemArray[2].ToString()))
            {
                switch (arc.Rows[0].ItemArray[2].ToString().ToUpper())
                {
                    case "BIS":
                        CkBis.Checked = true;
                        break;
                    case "TRIS":
                        CkTris.Checked = true;
                        break;
                    case "QUATER":
                        CkQuater.Checked = true;
                        break;

                }

            }

            if (!string.IsNullOrEmpty(arc.Rows[0].ItemArray[3].ToString()))
            {
                DateTime dataIntervento = System.Convert.ToDateTime(arc.Rows[0].ItemArray[3].ToString()); // Recupera la data dal DataTable
                txtDataInserimento.Text = dataIntervento.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
                                                                                 //  TxtDataIntervento.Text = rap.Rows[0].ItemArray[2].ToString();
            }
            if (!string.IsNullOrEmpty(arc.Rows[0].ItemArray[4].ToString()))
            {
                DateTime dataModifica = System.Convert.ToDateTime(arc.Rows[0].ItemArray[4].ToString()); // Recupera la data dal DataTable
                txtDataUltimoIntervento.Text = dataModifica.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
                                                                                    //  TxtDataIntervento.Text = rap.Rows[0].ItemArray[2].ToString();
            }
            if (!string.IsNullOrEmpty(arc.Rows[0].ItemArray[24].ToString()))
            {
                DateTime dataInizioAtt = System.Convert.ToDateTime(arc.Rows[0].ItemArray[24].ToString()); // Recupera la data dal DataTable
                txtDataInizioAttivita.Text = dataInizioAtt.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
            }

            txtIndirizzo.Text = arc.Rows[0].ItemArray[5].ToString().ToUpper();
            txtResponsabile.Text = arc.Rows[0].ItemArray[6].ToString().ToUpper();
            txtNatoA.Text = arc.Rows[0].ItemArray[7].ToString().ToUpper();
            if (!string.IsNullOrEmpty(arc.Rows[0].ItemArray[8].ToString()))
            {
                DateTime dataNascita = System.Convert.ToDateTime(arc.Rows[0].ItemArray[8].ToString()); // Recupera la data dal DataTable
                txtDataNascita.Text = dataNascita.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
                                                                          //  TxtDataIntervento.Text = rap.Rows[0].ItemArray[2].ToString();
            }
            txtInCarico.Text = arc.Rows[0].ItemArray[9].ToString();
            CkEvasa.Checked = System.Convert.ToBoolean(arc.Rows[0].ItemArray[10]);
            txtNote.Text = arc.Rows[0].ItemArray[11].ToString();
            txtTipoAtto.Text = arc.Rows[0].ItemArray[12].ToString();
            txtQuartiere.Text = arc.Rows[0].ItemArray[13].ToString().ToUpper();
            CkSuoloPubblico.Checked = System.Convert.ToBoolean(arc.Rows[0].ItemArray[14]);
            CkVincoli.Checked = System.Convert.ToBoolean(arc.Rows[0].ItemArray[15]);
            Ck1089.Checked = System.Convert.ToBoolean(arc.Rows[0].ItemArray[16]);
            CkDemolita.Checked = System.Convert.ToBoolean(arc.Rows[0].ItemArray[17]);
            //txtAllegati.Text= arc.Rows[0].ItemArray[18].ToString();
            //txtmatricola.Text= arc.Rows[0].ItemArray[19].ToString();
            if (!String.IsNullOrEmpty(arc.Rows[0].ItemArray[20].ToString()))
                txtSezione.Text = arc.Rows[0].ItemArray[20].ToString().ToUpper();
            if (!String.IsNullOrEmpty(arc.Rows[0].ItemArray[21].ToString()))
                TxtFoglio.Text = arc.Rows[0].ItemArray[21].ToString();
            if (!String.IsNullOrEmpty(arc.Rows[0].ItemArray[22].ToString()))
                TxtParticella.Text = arc.Rows[0].ItemArray[22].ToString();
            if (!String.IsNullOrEmpty(arc.Rows[0].ItemArray[23].ToString()))
                TxtSub.Text = arc.Rows[0].ItemArray[23].ToString();
            CkPropPriv.Checked = System.Convert.ToBoolean(arc.Rows[0].ItemArray[25]);
            CkPropComunale.Checked = System.Convert.ToBoolean(arc.Rows[0].ItemArray[26]);
            CkPropBeniCult.Checked = System.Convert.ToBoolean(arc.Rows[0].ItemArray[27]);
            CkPropAltri.Checked = System.Convert.ToBoolean(arc.Rows[0].ItemArray[28]);


        }
        public Boolean Convalida()
        {
            bool resp = false;

            if (!String.IsNullOrEmpty(txtPratica.Text))
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
                        List<string> ListRicerca = new List<string> { "Pratica", txtPratica.Text };
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

                        ArchivioUote arch = new ArchivioUote();
                        arch.arch_numPratica = txtPratica.Text;
                        if (CkBis.Checked && CkTris.Checked && CkQuater.Checked)
                        {
                            errorMessage.InnerText = @"Non è possibile selezionare BIS, TRIS E QUATER CONTEMPORANEAMENTE.";
                            apripopuperrorModal_Click(sender, e);

                        }
                        else
                        {


                            if (CkBis.Checked)
                            {
                                arch.arch_bis = "BIS";
                            }
                            if (CkTris.Checked)
                            {
                                arch.arch_bis = "TRIS";
                            }
                            if (CkQuater.Checked)
                            {
                                arch.arch_bis = "QUATER";
                            }
                            //DateTime giorno = DateTime.Now;
                            arch.arch_dataIns = System.Convert.ToDateTime(txtDataInserimento.Text);   // giorno.ToString("dddd", new CultureInfo("it-IT"));
                                                                                                      //if (HfStato.Value == "Mod")
                                                                                                      //{
                            if (!String.IsNullOrEmpty(txtDataUltimoIntervento.Text))
                              
                                arch.arch_datault_intervento = System.Convert.ToDateTime(txtDataUltimoIntervento.Text);
                                                 }
                            if (!String.IsNullOrEmpty(txtDataNascita.Text))
                            {

                                arch.arch_dataNascita = System.Convert.ToDateTime(txtDataNascita.Text);

                            }
                            if (!String.IsNullOrEmpty(txtDataInizioAttivita.Text))
                            {
                                arch.arch_dataInizioAttivita = System.Convert.ToDateTime(txtDataInizioAttivita.Text);

                            }
                            arch.arch_inCarico = txtInCarico.Text;
                            arch.arch_tipologia = txtTipoAtto.Text;
                            arch.arch_note = txtNote.Text;
                            arch.arch_quartiere = txtQuartiere.Text;
                            arch.arch_matricola = Session["user"].ToString();
                            arch.arch_indirizzo = txtIndirizzo.Text;
                            arch.arch_natoA = txtNatoA.Text;
                            arch.arch_responsabile = txtResponsabile.Text;
                            arch.arch_vincoli = CkVincoli.Checked;
                            arch.arch_suoloPub = CkSuoloPubblico.Checked;
                            arch.arch_1089 = Ck1089.Checked;
                            arch.arch_evasa = CkEvasa.Checked;
                            arch.arch_demolita = CkDemolita.Checked;
                            //arch.arch_allegati = txtAllegati.Text;
                            arch.arch_sezione = txtSezione.Text;
                            arch.arch_foglio = TxtFoglio.Text;
                            arch.arch_particella = TxtParticella.Text;
                            arch.arch_sub = TxtSub.Text;
                            arch.arch_propPriv = CkPropPriv.Checked;
                            arch.arch_propBeniCult = CkPropBeniCult.Checked;
                            arch.arch_propComune = CkPropComunale.Checked;
                            arch.arch_propAltriEnti = CkPropAltri.Checked;

                            Boolean ins = mn.SavePraticaArchivioUote(arch);
                            if (!ins)
                            {
                                errorMessage.InnerText = "Inserimento della pratica non riuscito, controllare il log.";

                                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
                            }
                            else
                            {
                                if (HfStato.Value == "Mod")

                                    errorMessage.InnerText = "Pratica " + arch.arch_numPratica + " modificata correttamente .";

                                else
                                    errorMessage.InnerText = "Pratica " + arch.arch_numPratica + " inserita correttamente .";
                                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica " + arch.arch_numPratica + " inserita correttamente ." + "'); $('#errorModal').modal('show');", true);
                                HfStato.Value = string.Empty;
                                Session["POP"] = "si";
                                Session.Remove("ListRicerca");
                                Pulisci();
                                txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
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
            }
            catch (Exception ex)
            {
                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "View/InserimentoArchivio.aspx";
                Response.Redirect("~/Contact.aspx");
            }
        }
        private void Pulisci()
        {

            txtPratica.Text = String.Empty;
            txtGiudice.Text = string.Empty;
            txtQuartiere.Text = string.Empty;
            txtInCarico.Text = string.Empty;
            txtTipoAtto.Text = string.Empty;
            txtIndirizzo.Text = string.Empty;
            txtResponsabile.Text = string.Empty;
            txtDataInserimento.Text = String.Empty;
            txtDataInizioAttivita.Text = String.Empty;
            txtNatoA.Text = String.Empty;
            //txtAllegati.Text = String.Empty;
            txtDataUltimoIntervento.Text = String.Empty;
            txtNote.Text = String.Empty;
            txtDataNascita.Text = String.Empty;
            Ck1089.Checked = false;
            CkSuoloPubblico.Checked = false;
            CkDemolita.Checked = false;
            CkVincoli.Checked = false;
            CkEvasa.Checked = false;
            CkBis.Checked = false;
            CkTris.Checked = false;
            CkQuater.Checked = false;
            CkPropAltri.Checked = false;
            CkPropBeniCult.Checked = false;
            CkPropComunale.Checked = false;
            CkPropPriv.Checked = false;
            txtSezione.Text = string.Empty;
            TxtFoglio.Text = string.Empty;
            TxtParticella.Text = string.Empty;
            TxtSub.Text = string.Empty;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
        }
        private void CaricaDLL()
        {
            try
            {
                Manager mn = new Manager();
                DataTable RicercaQuartiere = mn.getListQuartiere();
                DdlQuartiereI.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                DdlQuartiereI.DataTextField = "Quartiere"; // Il campo visibile
                DdlQuartiereI.DataBind();

                DataTable RicercaIndirizzo = mn.getListIndirizzo();
                DdlIndirizzoI.DataSource = RicercaIndirizzo; // Imposta il DataSource della DropDownList
                DdlIndirizzoI.DataTextField = "SpecieToponimo"; // Il campo visibile
                DdlIndirizzoI.DataBind();

                DataTable RicercaTipoAtto = mn.getListTipologia();
                DdlTipoAttoI.DataSource = RicercaTipoAtto; // Imposta il DataSource della DropDownList
                DdlTipoAttoI.DataTextField = "Tipo_Nota"; // Il campo visibile
                DdlTipoAttoI.DataValueField = "id_tipo_nota"; // Il valore associato a ogni opzione
                DdlTipoAttoI.DataBind();

                DataTable RicercaGiudice = mn.getListGiudice();
                DdlGiudiceI.DataSource = RicercaGiudice; // Imposta il DataSource della DropDownList
                DdlGiudiceI.DataTextField = "Giudice"; // Il campo visibile
                DdlGiudiceI.DataValueField = "ID_giudice"; // Il valore associato a ogni opzione
                DdlGiudiceI.DataBind();

                DataTable RicercaInviati = mn.getListInviati();
                DdlInviatiI.DataSource = RicercaInviati; // Imposta il DataSource //della DropDownList
                DdlInviatiI.DataTextField = "Inviata"; // Il campo visibile
                DdlInviatiI.DataValueField = "id_inviata"; // Il valore associato a ogni opzione
                DdlInviatiI.DataBind();
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
                txtQuartiere.Text = selectedValue;
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }
    }
}