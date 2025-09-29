using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;

namespace Uotep
{
    public partial class Modifica : Page
    {
        String profilo = string.Empty;
        string ruolo = string.Empty;
        String Vuser = String.Empty;
        Principale p = new Principale();
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                profilo = Session["profilo"].ToString();
                ruolo = Session["ruolo"].ToString();
            }
            Session["PaginaChiamante"] = "~/View/Modifica.aspx";

            if (!IsPostBack)
            {
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                ProtocolloLiteral.Text = decodedText;
                DivRicerca.Visible = false;
                NascondiDiv();
                CaricaDLL();
                if (ruolo == Enumerate.Ruolo.accertatori.ToString())
                {
                    btSalva.Visible = false;
                    btCercaQuartiere.Visible = false;
                    btChiudiDecretazione.Visible = false;

                }


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
                //DdlQuartiere.DataValueField = "ID_quartiere"; // Il valore associato a ogni opzione
                DdlQuartiere.DataBind();
                //DdlQuartiere.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaIndirizzo = mn.getListIndirizzo();
                DdlIndirizzo.DataSource = RicercaIndirizzo; // Imposta il DataSource della DropDownList
                DdlIndirizzo.DataTextField = "SpecieToponimo"; // Il campo visibile
                DdlIndirizzo.DataBind();
                //DdlIndirizzo.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
                DataTable RicercaProvvAg = mn.getListProvvAg(DdlSigla.SelectedValue.ToString());
                DdlTipoProvvAg.DataSource = RicercaProvvAg; // Imposta il DataSource della DropDownList
                DdlTipoProvvAg.DataTextField = "Tipologia"; // Il campo visibile
                DdlTipoProvvAg.DataValueField = "id_tipo_nota_ag"; // Il valore associato a ogni opzione

                DdlTipoProvvAg.DataBind();

                DataTable Esito = mn.getListScaturito();
                DdlEsito.DataSource = Esito; // Imposta il DataSource della DropDownList
                DdlEsito.DataTextField = "Scaturito"; // Il campo visibile
                DdlEsito.DataValueField = "Id_scaturito"; // Il valore associato a ogni opzione

                DdlEsito.DataBind();


                DataTable RicercaTipoAtto = mn.getListTipologia();
                DdlTipoAtto.DataSource = RicercaTipoAtto; // Imposta il DataSource della DropDownList
                DdlTipoAtto.DataTextField = "Tipo_Nota"; // Il campo visibile
                DdlTipoAtto.DataValueField = "id_tipo_nota"; // Il valore associato a ogni opzione
                DdlTipoAtto.DataBind();

                DataTable RicercaProvenienza = mn.getListProvenienza();
                DdlProvenienza.DataSource = RicercaProvenienza; // Imposta il DataSource della DropDownList
                DdlProvenienza.DataTextField = "Provenienza"; // Il campo visibile
                DdlProvenienza.DataValueField = "id_provenienza"; // Il valore associato a ogni opzione

                DdlProvenienza.DataBind();

            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in carica ddl modifica.cs ");
                    sw.Close();
                }
            }

        }

        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {


                Principale p = new Principale();
                //if (Session["user"] != null)
                //{
                //    Vuser = Session["user"].ToString();
                //}
                //p.anno = annoCorr;
                //p.giorno = DateTime.Now.Day.ToString();
                //p.nrProtocollo = System.Convert.ToInt32(txtProt.Text);
                //p.sigla = DdlSigla.SelectedItem.Text;
                p.dataArrivo = System.Convert.ToDateTime(txtDataInsCarico.Text).ToShortDateString();
                if (!string.IsNullOrEmpty(txtDataCarico.Text))
                {
                    p.dataCarico = System.Convert.ToDateTime(txtDataCarico.Text).ToShortDateString();
                }

                p.nominativo = txtNominativo.Text;

                if (!String.IsNullOrEmpty(txPratica.Text))
                {
                    p.nr_Pratica = txPratica.Text.Trim();
                }
                if (String.IsNullOrEmpty(txtIndirizzo.Text))
                {
                    p.indirizzo = String.Empty;
                }
                else
                {
                    p.indirizzo = txtIndirizzo.Text;
                    p.via = string.Empty;

                }
                if (String.IsNullOrEmpty(txtQuartiere.Text))
                {
                    p.quartiere = String.Empty;
                }
                else
                {
                    p.quartiere = txtQuartiere.Text;
                    //p.quartiere = lblQuartiere.Text;
                }
                p.provenienza = txtProvenienza.Text;
                p.tipologia_atto = txtTipoAtto.Text;
                p.rif_Prot_Gen = txtRifProtGen.Text;
                p.giudice = txtGiudice.Text;
                if (!string.IsNullOrEmpty(DdlTipoProvvAg.SelectedItem.Text))

                    p.tipoProvvedimentoAG = DdlTipoProvvAg.SelectedItem.Text;
                //if (DdlQuartiere.SelectedValue == "0")
                //{
                //    p.quartiere = String.Empty;
                //}
                //else
                //{
                //    p.quartiere = DdlQuartiere.SelectedItem.Text;

                //}

                // p.note = txtNote.Text;
                //p.evasa = CkEvasa.Checked;
                if (!string.IsNullOrEmpty(TxtDataEsito.Text))
                {
                    p.evasaData = System.Convert.ToDateTime(TxtDataEsito.Text).ToShortDateString();
                }

                if (!String.IsNullOrEmpty(txtAreaCompetenza.Text))
                {
                    p.macro_area = txtAreaCompetenza.Text.ToUpper();
                }
                else
                    p.macro_area = string.Empty;

                // p.accertatori = string.Empty;

                if (!string.IsNullOrEmpty(txtEsito.Text))
                    p.scaturito = txtEsito.Text;
                //if (!string.IsNullOrEmpty(txtInviata.Text))
                //    p.inviata = txtInviata.Text;
                //if (!string.IsNullOrEmpty(txtDataInvio.Text))
                //{
                //    p.dataInvio = System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
                //}

                p.procedimentoPen = txtProdPenNr.Text;
                //matricola del popup
                p.matricola = Vuser;
                //string newMat = ;

                p.data_ins_pratica = DateTime.Now.ToLocalTime();
                p.nrProtocollo = System.Convert.ToInt32(txtProt.Text.Trim());
                DateTime o = System.Convert.ToDateTime(HolDate.Value);

                Manager mn = new Manager();

                // id proveniente dalla selezione della pratica
                int ID = System.Convert.ToInt32(Hid.Value);
                //
                Boolean ins = mn.UpdPratica(p, Holdmat.Value, ID, o);
                if (!ins)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "modifica non effettuata, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "modifica effettuata correttamente." + "'); $('#errorModal').modal('show');", true);

                    DivDettagli.Visible = false;
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
                    sw.WriteLine(ex.Message + @" - Errore in modifica ");
                    sw.Close();
                }

                Response.Redirect("~/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/Modifica.aspx";
                Response.Redirect("~/Contact.aspx");

            }
        }
        private void Pulisci()
        {
            txtEsito.Text = string.Empty;
            HfEsito.Value = string.Empty;
            txtQuartiere.Text = string.Empty;
            HfQuartiere.Value = string.Empty;
            // txtInviata.Text = string.Empty;
            // HfInviata.Value = string.Empty;
            txtIndirizzo.Text = string.Empty;
            HfIndirizzo.Value = string.Empty;
            txtAnnoRicerca.Text = String.Empty;
            //txPratica.Text = String.Empty;
            txtNProtocollo.Text = String.Empty;
            txtProcPenale.Text = String.Empty;
            txtDataDa.Text = String.Empty;
            txtDataA.Text = String.Empty;
            txtProtGen.Text = String.Empty;
            txtPratica.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtDatArrivoDa.Text = String.Empty;
            txtDatArrivoDa.Text = String.Empty;
            txtNotaDecretazione.Text = String.Empty;
            txtDecretante.Text = String.Empty;
            txtDecretato.Text = String.Empty;
            txtDataDecretazione.Text = String.Empty;
            TxtDataEsito.Text = String.Empty;
            txPratica.Text = String.Empty;
            txtTipoAtto.Text = String.Empty;
            txtProvenienza.Text = String.Empty;
            txtRifProtGen.Text = String.Empty;
            txtNominativo.Text = String.Empty;
            txtAreaCompetenza.Text = string.Empty;
            txtDataCarico.Text = String.Empty;
            txtDataInsCarico.Text = String.Empty;
            txtProt.Text = String.Empty;

        }

        protected void NuovaRicerca_Click(object sender, EventArgs e)
        {
            DivDettagli.Visible = false;
            DivRicerca.Visible = true;
            DivGrid.Visible = false;
            txtAnnoRicerca.Text = String.Empty;
            txPratica.Text = String.Empty;
        }
        protected void Ricerca_Click(object sender, EventArgs e)
        {

            Manager mn = new Manager();
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
            if (!string.IsNullOrEmpty(txtPratica.Text))
            {
                pratica = mn.getListPratica(txtPratica.Text);
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
                // Salva datatable pratica  nella Sessione
                Session["ListPratiche"] = pratica;
            
                gvPopupD.DataSource = pratica;
                gvPopupD.DataBind();
                //DivDettagli.Visible = true;
                //DivRicerca.Visible = false;
                DivGrid.Visible = true;

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
            }


            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non trovata." + "'); $('#errorModal').modal('show');", true);
            }
        }

        protected void RicercaQuartiere_Click(object sender, EventArgs e)
        {
            string indirizzo = string.Empty;


            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "inserire un indirizzo" + "');", true);
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
            //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalQuartiere').modal('show');", true);





        }

        protected void gvPopupD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore del CommandArgument
                string commandArgument = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = commandArgument.Split('|');

                // Assicurati che ci siano almeno 5 valori
                if (values.Length == 5)
                {
                    Int32 protocollo = System.Convert.ToInt32(values[0]);    // Protocollo
                    string matricola = values[1];     // Matricola
                    string dataInserimento = values[2]; // DataInserimento
                    string sigla = values[3]; // sigla
                    Hid.Value = values[4]; // id


                    //// Ora puoi usare questi valori per aggiornare i tuoi controlli
                    //p.nrProtocollo = System.Convert.ToInt32(protocollo);
                    //conservo la matricola precedente
                    Holdmat.Value = matricola;
                    HolDate.Value = dataInserimento;
                    //p.data_ins_pratica = System.Convert.ToDateTime(dataInserimento).ToLongDateString();
                    Manager mn = new Manager();
                    DataTable pratica = mn.getPraticaId(protocollo, System.Convert.ToDateTime(dataInserimento), sigla, System.Convert.ToInt32(Hid.Value));
                    if (pratica.Rows.Count > 0)
                    {
                        DivDettagli.Visible = true;
                        txtProt.Text = pratica.Rows[0].ItemArray[1].ToString();
                        //txtSigla.Text = pratica.Rows[0].ItemArray[2].ToString();
                        DdlSigla.SelectedItem.Text = pratica.Rows[0].ItemArray[2].ToString();
                        switch (DdlSigla.SelectedItem.Text)
                        {
                            case "AG":
                                divAg.Visible = true;
                                CaricaDLL();
                                break;
                            default:
                                divAg.Visible = false;
                                break;
                        }
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[3].ToString()))
                        {
                            DateTime dataappo1 = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[3].ToString()); // Recupera la data dal DataTable
                            txtDataInsCarico.Text = dataappo1.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox
                        }
                        else
                            txtDataInsCarico.Text = string.Empty;

                        txtProvenienza.Text = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                        txtProvenienza.ToolTip = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                        txtTipoAtto.Text = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                        txtTipoAtto.ToolTip = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                        txtGiudice.Text = pratica.Rows[0].ItemArray[6].ToString();
                        DdlTipoProvvAg.Items.Insert(0, new ListItem(pratica.Rows[0].ItemArray[7].ToString().ToUpper()));
                        // DdlTipoProvvAg.SelectedValue = "1";
                        DdlTipoProvvAg.ToolTip = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
                        TxtTipoProvvAg.Text = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
                        TxtTipoProvvAg.ToolTip = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
                        txtProdPenNr.Text = pratica.Rows[0].ItemArray[8].ToString();
                        txtNominativo.Text = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                        txtNominativo.ToolTip = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[10].ToString()))
                        {
                            txtIndirizzo.Text = pratica.Rows[0].ItemArray[10].ToString().ToUpper();
                            txtIndirizzo.ToolTip = pratica.Rows[0].ItemArray[10].ToString().ToUpper();
                        }
                        //txtVia.Text = pratica.Rows[0].ItemArray[10].ToString();
                        //CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0].ItemArray[12]);

                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[13].ToString()))
                        {
                            DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()); // Recupera la data dal DataTable
                            //converte la data 01-01-1900 in SPACE

                            if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                            {
                                TxtDataEsito.Text = ""; // Metti una stringa vuota
                            }
                            else
                            {
                                TxtDataEsito.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                            }
                        }
                        else
                            TxtDataEsito.Text = string.Empty;


                        //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[14].ToString()))

                        //    txtInviata.Text = pratica.Rows[0].ItemArray[14].ToString().ToUpper();

                        //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[15].ToString()))
                        //{
                        //    DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[15].ToString()); // Recupera la data dal DataTable
                        //    //converte la data 01-01-1900 in SPACE

                        //    if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                        //    {
                        //        txtDataInvio.Text = ""; // Metti una stringa vuota
                        //    }
                        //    else
                        //    {
                        //        txtDataInvio.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                        //    }
                        //}
                        //else
                        //    txtDataInvio.Text = string.Empty;

                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[16].ToString()))

                            txtEsito.Text = pratica.Rows[0].ItemArray[16].ToString().ToUpper();

                        txtAreaCompetenza.Text = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[18].ToString()))
                        {
                            DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()); // Recupera la data dal DataTable
                            //if (dataappo == DateTime.MinValue)
                            //{
                            //    txtDataCarico.Text = string.Empty;
                            //}
                            //else
                            //    txtDataCarico.Text = dataappo.ToString("dd/MM/yyyy"); // Formatta la data e imposta il testo del TextBox

                            //converte la data 01-01-1900 in SPACE

                            if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                            {
                                txtDataCarico.Text = ""; // Metti una stringa vuota
                            }
                            else
                            {
                                txtDataCarico.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                            }
                        }
                        else
                            txtDataCarico.Text = string.Empty;
                        txPratica.Text = pratica.Rows[0].ItemArray[19].ToString();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[20].ToString()))
                            txtQuartiere.Text = pratica.Rows[0].ItemArray[20].ToString();
                        //txtNote.Text = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                        //txtNote.ToolTip = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                        txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[22].ToString();
                        //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                        txtRifProtGen.Text = pratica.Rows[0].ItemArray[24].ToString();


                        // Puoi anche chiudere il popup se necessario
                        ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#ModalRicerca').modal('hide');", true);
                        DivDettagli.Visible = true;
                        DivRicerca.Visible = false;
                        // Pulisci();
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica: " + protocollo + " non trovata." + "'); $('#errorModal').modal('show');", true);

                    }
                }
            }
        }

        protected void gvPopupD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "Nr_Protocollo").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
                if (gvPopupD.TopPagerRow != null)
                {
                    // Trova il controllo Label all'interno del PagerTemplate
                    Label lblPageInfo = (Label)gvPopupD.TopPagerRow.FindControl("lblPageInfo");
                    if (lblPageInfo != null)
                    {
                        // Calcola e imposta il testo
                        int currentPage = gvPopupD.PageIndex + 1;
                        int totalPages = gvPopupD.PageCount;
                        lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                    }
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
        //gridview per decretazione
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

        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalQuartiere').modal('show');", true);
        }
        protected void apripopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDecretazione').modal('show');", true);

        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);
            Pulisci();
        }
        protected void chiudipopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalDecretazione')); modal.hide();", true);
            // Pulisci();
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
            DivDettagli.Visible = false;
            Session.Remove("ListPratiche");
        }

        protected void btNProtocollo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProtocollo.Visible = true;
            DivRicerca.Visible = true;

        }

        protected void btProcPenale_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProcPenale.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btProtGen_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProtGen.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btEvaseAg_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivEvasaAg.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btNpratica_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivPratica.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btGiudice_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivGiudice.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btProvenienza_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivRicerca.Visible = true;
            DivProvenienza.Visible = true;
        }

        protected void btNominativo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivNominativo.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btDataArrivo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivDataArrivo.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btAccertatori_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivAccertatori.Visible = true;
            DivRicerca.Visible = true;
        }

        protected void btIndirizzo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivIndirizzo.Visible = true;
            DivRicerca.Visible = true;
        }
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
            Decretazione_Click(sender, e);

        }
        protected void gvPopupD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }


            gvPopupD.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            Ricerca_Click(sender, e);

        }
        protected void GVRicecaScheda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            switch (e.NewPageIndex)
            {
                case -1:
                    e.NewPageIndex = 0;
                    break;
                default:
                    break;
            }
            gvPopupD.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            Ricerca_Click(sender, e);
        }

        protected void btAggiungiDecretazione_Click(object sender, EventArgs e)
        {
            try
            {
                Session["PaginaChiamante"] = "~/View/Modifica.aspx";
                Manager mn = new Manager();
                Decretazione decr = new Decretazione();
                decr.idPratica = System.Convert.ToInt32(Hid.Value);
                decr.Npratica = txtPraticaDecr.Text;
                decr.decretante = txtDecretante.Text;
                decr.decretato = txtDecretato.Text.ToUpper();
                decr.data = System.Convert.ToDateTime(txtDataDecretazione.Text);
                decr.nota = txtNotaDecretazione.Text.ToUpper();

                Boolean ins = mn.InsDecretazione(decr);
                if (!ins)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "inserimento non effettuato, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "inserimento effettuato correttamente." + "'); $('#errorModal').modal('show');", true);
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
                    sw.WriteLine(ex.Message + @" - Errore in inserimento decretazione ");
                    sw.Close();
                }

                Response.Redirect("/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/Modifica.aspx";
                Response.Redirect("~/Contact.aspx");

            }
        }

        protected void Decretazione_Click(object sender, EventArgs e)
        {
            txtPraticaDecr.Text = txtProt.Text;
            txtDataDecretazione.Text = DateTime.Now.ToString("dd/MM/yyyy");

            Manager mn = new Manager();
            DataTable operatore = mn.getNominativoOperatore(Vuser);
            if (operatore.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(operatore.Rows[0].ItemArray[0].ToString()))
                    txtDecretante.Text = operatore.Rows[0].ItemArray[0].ToString().ToUpper();
            }

            DataTable decretazione = new DataTable();
            if (!string.IsNullOrEmpty(txtPraticaDecr.Text))
            {
                decretazione = mn.getListDecretazione(txtPraticaDecr.Text, Hid.Value);
            }
            if (decretazione.Rows.Count > 0)
            {
                
                GVDecretazione.DataSource = decretazione;
                GVDecretazione.DataBind();
                Boolean a = System.Convert.ToBoolean(decretazione.Rows[0].ItemArray[8]);
                if (a == true)
                {
                    btAggiungiDecretazione.Enabled = false;
                    btChiudiDecretazione.Enabled = false;
                }
                else

                {
                    if (ruolo == Enumerate.Ruolo.accertatori.ToString())
                    {

                        btSalva.Visible = false;
                        btCercaQuartiere.Visible = false;
                        btChiudiDecretazione.Visible = false;
                    }
                    else
                        btChiudiDecretazione.Visible = true;
                    //    btAggiungiDecretazione.Enabled = true;
                    //    btChiudiDecretazione.Enabled = true;
                }

            }
            else
                btChiudiDecretazione.Visible = false;
            apripopupDecretazione_Click(sender, e);
        }

        protected void btChiudiDecretazione_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDataEvasa').modal('show');", true);

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
                        gvPopupD.DataSource = filteredDt;
                    }
                    else
                    {
                        gvPopupD.DataSource = null;
                    }

                }
                else
                {
                    gvPopupD.DataSource = dt; // Nessun filtro
                }
                gvPopupD.DataBind();
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                // throw;
            }
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
                    gvPopupD.DataSource = dv;
                    gvPopupD.DataBind();

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
        /// <summary>
        /// CHIUSURA DECRETAZIONE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ModalChiudiDecretazione_Click(object sender, EventArgs e)
        {
            try
            {
                Decretazione decr = new Decretazione();
                decr.idPratica = System.Convert.ToInt32(Hid.Value);
                decr.Npratica = txtPraticaDecr.Text;
                decr.decretante = txtDecretante.Text.ToUpper();
                decr.nota = txtNotaDecretazione.Text.ToUpper();
                if (!String.IsNullOrEmpty(txtDataDecretazione.Text))
                    decr.data = System.Convert.ToDateTime(txtDataDecretazione.Text);
                decr.chiuso = true;
                if (!String.IsNullOrEmpty(txtdataEvasaPopup.Text))
                {
                    //string dataFormattata = DateTime.Now.ToString("dd/MM/yyyy");
                    decr.dataChiusura = System.Convert.ToDateTime(txtdataEvasaPopup.Text);
                }

                //string dataFormattata = DateTime.Now.ToString("dd/MM/yyyy");
                //decr.dataChiusura = System.Convert.ToDateTime(dataFormattata);

                Manager mn = new Manager();
                Boolean upd = mn.UpdDecretazioneChiusura(decr);
                if (!upd)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "chiusura non effettuata, controllare il log." + "'); $('#errorModal').modal('show');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "chiusura effettuata correttamente." + "'); $('#errorModal').modal('show');", true);

                }
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalDataEvasa')); modal.hide();", true);
            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore in chiusura decretazione ");
                    sw.Close();
                }

                Response.Redirect("~/Contact.aspx?errore=" + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/Modifica.aspx";
                Response.Redirect("~/Contact.aspx");

            }

        }
        protected void DdlSigla_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaricaDLL();
            if (DdlSigla.SelectedItem.Text == Enumerate.Sigla.AG.ToString().ToUpper())
            {
                divAg.Visible = true;
            }
            else
            {
                divAg.Visible = false;
                txtGiudice.Text = string.Empty;

                txtProdPenNr.Text = string.Empty;
            }
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
    }
}