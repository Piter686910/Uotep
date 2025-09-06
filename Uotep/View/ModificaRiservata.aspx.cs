using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.Caching;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;

namespace Uotep
{
    public partial class ModificaRiservata : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        MemoryCache _cache = MemoryCache.Default;
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (Session["user"] != null)
            //{
            //    Vuser = Session["user"].ToString();
            //}
            if (_cache != null)
            {
                // 2. Recuperare un parametro dalla cache
                Vuser = _cache.Get("user") as string;
            }
            else
            {
                Response.Redirect("Default.aspx?user=true");
            }
            if (!IsPostBack)
            {
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                ProtocolloLiteral.Text = decodedText;


                CaricaDLL();
                DivDettagli.Visible = false;
                DivRicerca.Visible = false;

            }

        }
        protected void trova_Click(object sender, EventArgs e)
        {


            //String Vpassw = "";
            // Vuser = TxtMatricola.Text;
            Hmatricola.Value = TxtMatricola.Text;
            //Vpassw = TxtPassw.Text;
            //salvo la matricola
            //Session["user"] = Vuser;
            Manager mn = new Manager();
            DataTable Ricerca = mn.getUserByUserPassw(TxtMatricola.Text, TxtPassw.Text);
            if (Ricerca.Rows.Count > 0)
            {
                //controllo il ruolo
                if (Ricerca.Rows[0].ItemArray[6].ToString() == Enumerate.Ruolo.MasterAG.ToString())
                {
                    pnlLogin.Visible = false;
                    DivRicerca.Visible = true;
                    lblmessage.Text = string.Empty;
                }
                else
                {
                    lblmessage.Text = "Utente non autorizzato".ToUpper();
                }
            }

        }
        protected void Ricerca_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            //DataTable pratica = mn.getListPrototocollo(txtNProtocollo.Text, txtAnnoRicerca.Text);


            DataTable pratica = new DataTable();
            if (!string.IsNullOrEmpty(txtNProtocollo.Text))
            {
                pratica = mn.getListPrototocollo(txtNProtocollo.Text, txtAnnoRicerca.Text);
            }
            if (!string.IsNullOrEmpty(txtProcPenale.Text))
            {
                pratica = mn.getListProcedimento(txtProcPenale.Text);
            }

            if (!string.IsNullOrEmpty(txtDataDa.Text))
            {
                pratica = mn.getListEvasaAg(txtDataDa.Text, txtDataA.Text);
            }
            if (!string.IsNullOrEmpty(txtProtGen.Text))
            {
                pratica = mn.getListProtGen(txtProtGen.Text);
            }
            if (!string.IsNullOrEmpty(txtPraticaR.Text))
            {
                pratica = mn.getListPratica(txtPraticaR.Text);
            }
            if (!string.IsNullOrEmpty(txtRicGiudice.Text))
            {
                pratica = mn.getListGiudice(txtRicGiudice.Text);
            }
            if (!string.IsNullOrEmpty(txtRicProvenienza.Text))
            {
                pratica = mn.getListProvenienza(txtRicProvenienza.Text);
            }
            if (!string.IsNullOrEmpty(txtRicNominativo.Text))
            {
                pratica = mn.getListNominativo(txtRicNominativo.Text);
            }
            if (!string.IsNullOrEmpty(txtRicAccertatori.Text))
            {
                pratica = mn.getListAccertatori(txtRicAccertatori.Text);
            }
            if (!string.IsNullOrEmpty(txtRicIndirizzo.Text))
            {
                pratica = mn.getListIndirizzo(txtRicIndirizzo.Text);
            }
            if (!string.IsNullOrEmpty(txtDataDa.Text))
            {
                pratica = mn.getListDataArrivo(txtDatArrivoDa.Text, txtDatArrivoA.Text);
            }



            if (pratica.Rows.Count > 0)
            {
                apripopupPratica_Click(sender, e);
                gvPopupProtocolli.DataSource = pratica;
                gvPopupProtocolli.DataBind();


            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non trovata." + "'); $('#errorModal').modal('show');", true);
            }
            //DivGrid.Visible = true;
            //DivDettagli.Visible = true;
            //DivRicerca.Visible = false;
            Pulisci();
        }
        public void Convalida()
        {

            if (!String.IsNullOrEmpty(HfGiudice.Value))
                btSalvaGiudice.Visible = true;

            if (!String.IsNullOrEmpty(HfTipoProv.Value))
                btSalvaTipoProvv.Visible = true;

            if (!String.IsNullOrEmpty(HfProvenienza.Value))
                btSalvaProvenienza.Visible = true;

            if (!String.IsNullOrEmpty(HfTipoAtto.Value))
                btSalvaTipoAtto.Visible = true;

            if (!String.IsNullOrEmpty(HfInviata.Value))
                btSalvaInviata.Visible = true;
            if (!String.IsNullOrEmpty(HfScaturito.Value))
                BtSalvaScaturito.Visible = true;

        }
        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
                Principale p = new Principale();
                CultureInfo culturaItaliana = new CultureInfo("it-IT");
                Manager mn = new Manager();
                p.anno = hfAnno.Value;

                //DateTime adesso = DateTime.Now;
                //p.giorno = adesso.ToString("dddd", culturaItaliana);
                p.giorno = Hfgiorno.Value;
                if (!String.IsNullOrEmpty(txtPratica.Text))
                    p.nr_Pratica = txtPratica.Text;

                p.nrProtocollo = System.Convert.ToInt32(txtProt.Text);
                p.sigla = DdlSigla.SelectedItem.Text;
                p.dataArrivo = System.Convert.ToDateTime(txtDataArrivo.Text).ToShortDateString();
                if (!string.IsNullOrEmpty(txtDataCarico.Text))
                {
                    p.dataCarico = System.Convert.ToDateTime(txtDataCarico.Text).ToShortDateString();
                }
                else

                    p.dataCarico = null; //System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
                p.nominativo = txtNominativo.Text;

                if (String.IsNullOrEmpty(txtGiudice.Text))
                {
                    p.giudice = String.Empty;
                }
                else
                {

                    Boolean resp = mn.getGiudice(txtGiudice.Text);
                    if (!resp)
                    {
                        HfGiudice.Value = txtGiudice.Text;
                    }

                    p.giudice = txtGiudice.Text;
                }

                if (String.IsNullOrEmpty(txtProvenienza.Text))
                {

                    p.provenienza = string.Empty;
                }

                else
                {
                    Boolean resp = mn.getProvenienza(txtProvenienza.Text);
                    if (!resp)
                    {

                        HfProvenienza.Value = txtProvenienza.Text;
                    }
                    p.provenienza = txtProvenienza.Text;
                }


                if (String.IsNullOrEmpty(txtTipoAttoR.Text))
                {

                    p.tipologia_atto = String.Empty;
                }
                else
                {
                    Boolean resp = mn.getTipoAtto(txtTipoAttoR.Text);
                    if (!resp)
                    {
                        HfTipoAtto.Value = txtTipoAttoR.Text;
                    }
                    p.tipologia_atto = txtTipoAttoR.Text;
                }


                if (String.IsNullOrEmpty(txtTipoProv.Text))
                {
                    p.tipoProvvedimentoAG = String.Empty;
                }
                else
                {

                    Boolean resp = mn.getTipoProv(txtTipoProv.Text);
                    if (!resp)
                    {
                        HfTipoProv.Value = txtTipoProv.Text;
                    }

                    p.tipoProvvedimentoAG = txtTipoProv.Text;
                }


                p.rif_Prot_Gen = txtRifProtGen.Text;

                //p.indirizzo = DdlIndirizzo.SelectedItem.Text;
                if (String.IsNullOrEmpty(txtIndirizzo.Text))
                {

                    p.indirizzo = String.Empty;
                }
                else
                {
                    Boolean resp = mn.getTipoScaturito(txtIndirizzo.Text);
                    if (!resp)
                    {
                        HfIndirizzo.Value = txtIndirizzo.Text;
                    }
                    p.indirizzo = txtIndirizzo.Text;
                }
                // p.via = txtVia.Text;

                if (String.IsNullOrEmpty(txtQuartiere.Text))
                {
                    p.quartiere = String.Empty;
                }
                else
                {
                    p.quartiere = txtQuartiere.Text;
                    //p.quartiere = lblQuartiere.Text;
                }


                p.note = txtNote.Text;
                p.evasa = CkEvasa.Checked;
                if (!string.IsNullOrEmpty(txtDataDataEvasa.Text))
                {
                    p.evasaData = System.Convert.ToDateTime(txtDataDataEvasa.Text).ToShortDateString();
                }

                p.accertatori = txtAccertatori.Text;

                //p.scaturito = DdlScaturito.SelectedItem.Text;
                if (String.IsNullOrEmpty(txtScaturito.Text))
                {

                    p.scaturito = String.Empty;
                }
                else
                {
                    Boolean resp = mn.getTipoScaturito(txtScaturito.Text);
                    if (!resp)
                    {
                        HfScaturito.Value = txtScaturito.Text;
                    }
                    p.scaturito = txtScaturito.Text;
                }




                if (String.IsNullOrEmpty(txtInviata.Text))
                {
                    p.inviata = String.Empty;
                }
                else
                {
                    Boolean resp = mn.getInviata(txtInviata.Text);
                    if (!resp)
                    {
                        HfInviata.Value = txtInviata.Text;
                    }
                    p.inviata = txtInviata.Text;

                }

                if (!string.IsNullOrEmpty(txtDataInvio.Text))
                {
                    p.dataInvio = System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
                }

                p.procedimentoPen = txtProdPenNr.Text;
                p.matricola = Hmatricola.Value;
                p.data_ins_pratica = DateTime.Now.ToLocalTime();
                DateTime o = System.Convert.ToDateTime(HolDate.Value);


                Boolean ins = mn.SavePraticaTrans(p, Holdmat.Value, o, HoldProtocollo.Value, System.Convert.ToInt32(HidPratica.Value), Vuser);

                //Boolean ins = mn.SavePraticaTrans(p, Holdmat.Value, o, HoldProtocollo.Value, System.Convert.ToInt32(HidPratica.Value), Session["user"].ToString());
                if (!ins)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "errore durante il salvataggio." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Protocollo " + p.nrProtocollo + " modificato correttamente ." + "'); $('#errorModal').modal('show');", true);
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
                    sw.WriteLine(ex.Message + @" - Errore modifica riservata ");
                    sw.Close();
                }


                Response.Redirect("~/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/ModificaRiservata.aspx";
                Response.Redirect("~/Contact.aspx");

                //Session["MessaggioErrore"] = ex.Message;
                //Session["PaginaChiamante"] = "View/Inserimento.aspx";
                //Response.Redirect("~/Contact.aspx");

            }
            //  Ricerca_Click(this, EventArgs.Empty);
        }//popup provenienza
        protected void apripopupProvenienza_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModalProvenienza').modal('show');", true);
        }
        protected void chiudipopupProvenienza_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModalProvenienza')); modal.hide();", true);

        }
        //popup quartiere
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

        }
        //pratica
        protected void apripopupPratica_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalPratica').modal('show');", true);
        }
        protected void chiudipopupPratica_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalPratica')); modal.hide();", true);

        }
        protected void RicercaQuartiere_Click(object sender, EventArgs e)
        {
            string indirizzo = string.Empty;


            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "inserire un indirizzo" + "');", true);
            indirizzo = txtIndirizzoRic.Text.Trim();


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

                    //DdlQuartiere.Text = quartiere.Rows[0].ItemArray[0].ToString();
                    //txtIndirizzo.Text = string.Empty;
                    //txtSpecie.Text = string.Empty;
                    //lblQuartiere.Text = $"Quartiere: {quartiere.Rows[0].ItemArray[0]}";
                }
                else
                {
                    //lblQuartiere.Text = "Quartiere non trovato.";
                }
            }
            else
            {
                //lblQuartiere.Text = "Inserisci un indirizzo valido.";
            }

            // Mantieni il popup aperto dopo l'interazione lato server.
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showPopup", "openPopup();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);


            //DataTable Ricerca = mn.getUserByUserPassw(TxtMatricola.Text, TxtPassw.Text);
            //if (bUser == "admin")
            //{
            //    Response.Redirect("pagina_amministratore.aspx?user=" + Vuser + "&numord=" + VNumOrd + "", false);
            //    return;
            //}

            //if (Ricerca.Rows.Count > 0)
            //{
            //    var Rapportino = new Rapportino();


            //    Rapportino.mat = txt_Operatore.Text;

            //    Rapportino.Show();
            //    this.Close(); ;
            //}

        }

        protected void btSalvaGiudice_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean ins = mn.InserisciGiudice(HfGiudice.Value);
            if (ins)
            {
                HfGiudice.Value = string.Empty;
                txtGiudice.Text = string.Empty;
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

            }
        }
        protected void btSalvaTipoProvv_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean ins = mn.InserisciTipologiaNotaAg(HfTipoProv.Value);
            if (ins)
            {
                HfTipoProv.Value = string.Empty;
                txtTipoProv.Text = string.Empty;
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

            }
        }

        protected void btSalvaProvenienza_Click(object sender, EventArgs e)
        {

            Manager mn = new Manager();
            Boolean ins = mn.InserisciProvenienza(HfProvenienza.Value);
            if (ins)
            {
                HfProvenienza.Value = string.Empty;
                txtProvenienza.Text = string.Empty;
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

            }
        }

        protected void btSalvaScaturito_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean ins = mn.InserisciScaturito(HfScaturito.Value);
            if (ins)
            {
                HfScaturito.Value = string.Empty;
                txtScaturito.Text = string.Empty;
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

            }
        }
        protected void btSalvaTipoAtto_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean ins = mn.InserisciTipologia(HfTipoAtto.Value);
            if (ins)
            {
                HfTipoAtto.Value = string.Empty;
                txtTipoAttoR.Text = string.Empty;
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

            }
        }



        protected void btSalvaInviata_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean ins = mn.InserisciInviata(HfInviata.Value);
            if (ins)
            {
                HfInviata.Value = string.Empty;
                txtInviata.Text = string.Empty;
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento effettuato correttamente" + "'); $('#errorModal').modal('show');", true);

            }
        }

        private void CaricaDLL()
        {
            try
            {


                Manager mn = new Manager();
                DataTable RicercaQuartiere = mn.getListQuartiere();
                DdlQuartiere.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                DdlQuartiere.DataTextField = "Quartiere"; // Il campo visibile
                DdlQuartiere.DataValueField = "ID_quartiere"; // Il valore associato a ogni opzione
                DdlQuartiere.DataBind();
                // DdlQuartiere.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaIndirizzo = mn.getListIndirizzo();
                DdlIndirizzo.DataSource = RicercaIndirizzo; // Imposta il DataSource della DropDownList
                DdlIndirizzo.DataTextField = "SpecieToponimo"; // Il campo visibile
                                                               //      DdlIndirizzo.DataValueField = "ID_quartiere";
                DdlIndirizzo.DataBind();
                //       DdlIndirizzo.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaTipoAtto = mn.getListTipologia();
                DdlTipoAtto.DataSource = RicercaTipoAtto; // Imposta il DataSource della DropDownList
                DdlTipoAtto.DataTextField = "Tipo_Nota"; // Il campo visibile
                DdlTipoAtto.DataValueField = "id_tipo_nota"; // Il valore associato a ogni opzione
                DdlTipoAtto.Items.Insert(0, new ListItem("", "0"));
                DdlTipoAtto.DataBind();
                //     DdlTipoAtto.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaProvenienza = mn.getListProvenienza();
                DdlProvenienza.DataSource = RicercaProvenienza; // Imposta il DataSource della DropDownList
                DdlProvenienza.DataTextField = "Provenienza"; // Il campo visibile
                DdlProvenienza.DataValueField = "id_provenienza"; // Il valore associato a ogni opzione

                DdlProvenienza.DataBind();
                //   DdlProvenienza.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaGiudice = mn.getListGiudice();
                DdlGiudice.DataSource = RicercaGiudice; // Imposta il DataSource della DropDownList
                DdlGiudice.DataTextField = "Giudice"; // Il campo visibile
                DdlGiudice.DataValueField = "ID_giudice"; // Il valore associato a ogni opzione

                DdlGiudice.DataBind();
                // DdlGiudice.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));


                DataTable RicercaProvvAg = mn.getListProvvAg(DdlSigla.SelectedValue.ToString());
                DdlTipoProvvAg.DataSource = RicercaProvvAg; // Imposta il DataSource della DropDownList
                DdlTipoProvvAg.DataTextField = "Tipologia"; // Il campo visibile
                DdlTipoProvvAg.DataValueField = "id_tipo_nota_ag"; // Il valore associato a ogni opzione

                DdlTipoProvvAg.DataBind();
                //DdlTipoProvvAg.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable Scaturito = mn.getListScaturito();
                DdlScaturito.DataSource = Scaturito; // Imposta il DataSource della DropDownList
                DdlScaturito.DataTextField = "Scaturito"; // Il campo visibile
                DdlScaturito.DataValueField = "Id_scaturito"; // Il valore associato a ogni opzione

                DdlScaturito.DataBind();
                //       DdlScaturito.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaInviati = mn.getListInviati();
                DdlInviati.DataSource = RicercaInviati; // Imposta il DataSource della DropDownList
                DdlInviati.DataTextField = "Inviata"; // Il campo visibile
                DdlInviati.DataValueField = "id_inviata"; // Il valore associato a ogni opzione
                DdlInviati.DataBind();
                //DdlInviati.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in carica ddl modificariservata.cs ");
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
                //txtSelectedValue.Text = selectedValue;
                txtQuartiere.Text = selectedValue;
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }

        protected void NuovaRicerca_Click(object sender, EventArgs e)
        {
            Pulisci();
            DivDettagli.Visible = false;
            DivRicerca.Visible = true;
            txtAnnoRicerca.Text = String.Empty;
            txtNProtocollo.Text = String.Empty;

        }
        //protocolli
        protected void gvPopupProtocolli_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore del CommandArgument
                string commandArgument = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = commandArgument.Split('|');

                // Assicurati che ci siano almeno 3 valori
                if (values.Length == 7)
                {

                    Int32 protocollo = System.Convert.ToInt32(values[0]);    // Protocollo
                    string matricola = values[1];     // Matricola
                    string dataInserimento = values[2]; // DataInserimento
                    string sigla = values[3]; // sigla
                    HidPratica.Value = values[4];
                    hfAnno.Value = values[5];
                    Hfgiorno.Value = values[6];
                    //// Ora puoi usare questi valori per aggiornare i tuoi controlli
                    //p.nrProtocollo = System.Convert.ToInt32(protocollo);
                    //conservo la matricola precedente
                    Holdmat.Value = matricola;
                    HolDate.Value = dataInserimento;
                    HoldProtocollo.Value = protocollo.ToString();
                    //p.data_ins_pratica = System.Convert.ToDateTime(dataInserimento).ToLongDateString();
                    Manager mn = new Manager();
                    DataTable pratica = mn.getPraticaId(protocollo, System.Convert.ToDateTime(dataInserimento), sigla, System.Convert.ToInt32(HidPratica.Value));

                    if (pratica.Rows.Count > 0)
                    {
                        //CaricaDLL();
                        txtProt.Text = pratica.Rows[0].ItemArray[1].ToString();
                        DdlSigla.SelectedItem.Text = pratica.Rows[0].ItemArray[2].ToString();
                        DateTime dataarrivo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[3].ToString()); // Recupera la data dal DataTable
                        txtDataArrivo.Text = dataarrivo.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox

                        // txtDataArrivo.Text = pratica.Rows[0].ItemArray[3].ToString();
                        // DdlProvenienza.SelectedItem.Text = pratica.Rows[0].ItemArray[4].ToString();

                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[4].ToString()))
                        {
                            txtProvenienza.Text = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                            txtProvenienza.ToolTip = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                        }
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[5].ToString()))
                        {
                            txtTipoAttoR.Text = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                            txtTipoAttoR.ToolTip = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                        }
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[6].ToString()))

                            txtGiudice.Text = pratica.Rows[0].ItemArray[6].ToString();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[7].ToString()))

                            txtTipoProv.Text = pratica.Rows[0].ItemArray[7].ToString().ToUpper();

                        txtProdPenNr.Text = pratica.Rows[0].ItemArray[8].ToString();
                        txtNominativo.Text = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                        txtNominativo.ToolTip = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[10].ToString()))
                            txtIndirizzo.Text = pratica.Rows[0].ItemArray[10].ToString().ToUpper();
                        //txtVia.Text = pratica.Rows[0].ItemArray[11].ToString();
                        CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0].ItemArray[12]);

                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[13].ToString()))
                        {
                            DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()); // Recupera la data dal DataTable
                            if (dataappo == DateTime.MinValue)
                            {
                                txtDataDataEvasa.Text = string.Empty;
                            }
                            else
                                txtDataDataEvasa.Text = dataappo.ToString("dd/MM/yyyy");
                        }
                        else
                            txtDataDataEvasa.Text = string.Empty;
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[14].ToString()))
                            txtInviata.Text = pratica.Rows[0].ItemArray[14].ToString().ToUpper();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[15].ToString()))
                        {
                            DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[15].ToString()); // Recupera la data dal DataTable
                            if (dataappo == DateTime.MinValue)
                            {
                                txtDataInvio.Text = string.Empty;
                            }
                            else
                                txtDataInvio.Text = dataappo.ToString("dd/MM/yyyy");
                        }
                        else
                            txtDataInvio.Text = string.Empty;
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[16].ToString()))

                            txtScaturito.Text = pratica.Rows[0].ItemArray[16].ToString().ToUpper();

                        txtAccertatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
                        txtAccertatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[18].ToString()))
                        {
                            DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()); // Recupera la data dal DataTable
                            if (dataappo == DateTime.MinValue)
                            {
                                txtDataCarico.Text = string.Empty;
                            }
                            else
                                txtDataCarico.Text = dataappo.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
                        }
                        else
                            txtDataCarico.Text = string.Empty;
                        txtPratica.Text = pratica.Rows[0].ItemArray[19].ToString();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[20].ToString()))

                            txtQuartiere.Text = pratica.Rows[0].ItemArray[20].ToString();
                        txtNote.Text = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                        txtNote.ToolTip = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                        txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[22].ToString();
                        //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                        txtRifProtGen.Text = pratica.Rows[0].ItemArray[24].ToString();
                        DivDettagli.Visible = true;
                        DivRicercaButton.Visible = false;
                        // Puoi anche chiudere il popup se necessario
                        ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#myModal').modal('hide');", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non presente in database." + "'); $('#errorModal').modal('show');", true);
                    }
                }
            }
        }

        protected void gvPopupProtocolli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
            if (gvPopupProtocolli.TopPagerRow != null && gvPopupProtocolli.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)gvPopupProtocolli.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = gvPopupProtocolli.PageIndex + 1;
                    int totalPages = gvPopupProtocolli.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }

        }
        private void Pulisci()
        {
            Convalida();
            //txtProt.Text = String.Empty;


            if (String.IsNullOrEmpty(HfGiudice.Value))
            {
                txtGiudice.Text = string.Empty;

            }
            if (String.IsNullOrEmpty(HfTipoProv.Value))
            {
                txtTipoProv.Text = string.Empty;
            }

            txtQuartiere.Text = string.Empty;
            if (String.IsNullOrEmpty(HfInviata.Value))
            {
                txtInviata.Text = string.Empty;
            }
            if (String.IsNullOrEmpty(HfProvenienza.Value))
            {
                txtTipoAttoR.Text = string.Empty;
            }
            if (String.IsNullOrEmpty(HfScaturito.Value))
            {
                txtScaturito.Text = string.Empty;
            }

            txtIndirizzo.Text = string.Empty;
            HfIndirizzo.Value = string.Empty;
            if (String.IsNullOrEmpty(HfProvenienza.Value))
            {
                txtProvenienza.Text = string.Empty;
            }
            txtPratica.Text = String.Empty;
            txtPraticaR.Text = String.Empty;
            txtDataArrivo.Text = String.Empty;
            txtRifProtGen.Text = String.Empty;
            //  txtVia.Text = String.Empty;
            txtProdPenNr.Text = String.Empty;

            txtNominativo.Text = String.Empty;
            txtNote.Text = String.Empty;
            txtDataDataEvasa.Text = String.Empty;
            // txtAnnoRicerca.Text = string.Empty;
            txtDataInvio.Text = String.Empty;
            CkEvasa.Checked = false;

            CaricaDLL();

        }
        protected void btInsProvenienza_Click(object sender, EventArgs e)
        {
            DdlProvenienza.SelectedItem.Text = txtTestoProvenienza.Text;
        }
        //inizio bottoni ricerca
        protected void btNProtocollo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProtocollo.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtPraticaR.Text = string.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtRicGiudice.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProcPenale.Text = string.Empty;

        }

        protected void btProcPenale_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProcPenale.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            Pulisci();
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtPraticaR.Text = string.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtRicGiudice.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;
        }

        protected void btProtGen_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProtGen.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtPraticaR.Text = string.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtRicGiudice.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtProcPenale.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;
        }

        protected void btEvaseAg_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivEvasaAg.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtPraticaR.Text = string.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtRicGiudice.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProcPenale.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;
        }

        protected void btNpratica_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivPratica.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            Pulisci();
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtRicGiudice.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProcPenale.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;
        }

        protected void btGiudice_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivGiudice.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtPraticaR.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProcPenale.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;
        }

        protected void btProvenienza_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivRicerca.Visible = true;
            DivProvenienza.Visible = true;
            DivRicercaButton.Visible = true;
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtPraticaR.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProcPenale.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;
        }

        protected void btNominativo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivNominativo.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtPraticaR.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProcPenale.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;
        }

        protected void btDataArrivo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivDataArrivo.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            txtRicNominativo.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtPraticaR.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProcPenale.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;

        }

        protected void btAccertatori_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivAccertatori.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            txtRicNominativo.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtIndirizzoRic.Text = String.Empty;
            txtPraticaR.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProcPenale.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;
        }

        protected void btIndirizzo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivIndirizzo.Visible = true;
            DivRicerca.Visible = true;
            DivRicercaButton.Visible = true;
            txtRicNominativo.Text = string.Empty;
            txtDataA.Text = string.Empty;
            txtDataDa.Text = string.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
            txtDatArrivoA.Text = string.Empty;
            txtDatArrivoDa.Text = string.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtPraticaR.Text = string.Empty;
            txtProtGen.Text = string.Empty;
            txtProcPenale.Text = string.Empty;
            txtNProtocollo.Text = string.Empty;
            txtAnnoRicerca.Text = string.Empty;
        }

        protected void NascondiDiv()
        {
            DivProtocollo.Visible = false;
            DivEvasaAg.Visible = false;
            DivProtGen.Visible = false;
            DivIndirizzo.Visible = false;
            DivAccertatori.Visible = false;
            DivDataArrivo.Visible = false;
            DivNominativo.Visible = false;
            DivProvenienza.Visible = false;
            DivGiudice.Visible = false;
            DivPratica.Visible = false;
            DivProcPenale.Visible = false;
        }
        protected void gvPopupProtocolli_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }
            gvPopupProtocolli.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            Ricerca_Click(sender, e);
        }
        //fine
    }
}