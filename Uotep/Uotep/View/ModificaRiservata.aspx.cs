using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;

namespace Uotep
{
    public partial class ModificaRiservata : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        protected void Page_Load(object sender, EventArgs e)
        {

            // Legge il valore dal Web.config
            string protocolloText = ConfigurationManager.AppSettings["Titolo"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            ProtocolloLiteral.Text = decodedText;

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
            }
            if (!IsPostBack)
            {
                CaricaDLL();
                DivDettagli.Visible = false;
                DivRicerca.Visible = false;

            }

        }
        protected void trova_Click(object sender, EventArgs e)
        {


            String Vpassw = "";
            Vuser = TxtMatricola.Text;
            Hmatricola.Value = TxtMatricola.Text;
            Vpassw = TxtPassw.Text;
            //salvo la matricola
            Session["user"] = Vuser;
            Manager mn = new Manager();
            DataTable Ricerca = mn.getUserByUserPassw(TxtMatricola.Text, TxtPassw.Text);
            if (Ricerca.Rows.Count > 0)
            {
                //controllo il profilo
                if (Ricerca.Rows[0].ItemArray[6].ToString() == Enumerate.Profilo.MasterAG.ToString())
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
            DataTable pratica = mn.getListPrototocollo(txtProtoloccolRicerca.Text, txtAnnoRicerca.Text);
            if (pratica.Rows.Count > 0)
            {
                gvPopupProtocolli.DataSource = pratica;
                gvPopupProtocolli.DataBind();


            }
            DivGrid.Visible = true;
            DivDettagli.Visible = true;
            DivRicerca.Visible = false;

        }
        protected void Salva_Click(object sender, EventArgs e)
        {
            Principale p = new Principale();
            p.anno = annoCorr;
            p.giorno = DateTime.Now.Day.ToString();
            p.nrProtocollo = System.Convert.ToInt32(txtProt.Text);
            p.sigla = DdlSigla.SelectedItem.Text;
            p.dataArrivo = System.Convert.ToDateTime(txtDataArrivo.Text).ToShortDateString();
            //p.dataCarico = null; //System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
            p.nominativo = txtNominativo.Text;

            p.giudice = DdlGiudice.SelectedItem.Text;


            p.provenienza = DdlProvenienza.SelectedItem.Text;

            p.tipologia_atto = DdlTipoAtto.SelectedItem.Text;


            p.tipoProvvedimentoAG = DdlTipoProvvAg.SelectedItem.Text;


            p.rif_Prot_Gen = txtRifProtGen.Text;

            p.indirizzo = DdlIndirizzo.SelectedItem.Text;
            p.via = txtVia.Text;

            p.quartiere = DdlQuartiere.SelectedItem.Text;


            p.note = txtNote.Text;
            p.evasa = CkEvasa.Checked;
            if (!string.IsNullOrEmpty(txtDataDataEvasa.Text))
            {
                p.evasaData = System.Convert.ToDateTime(txtDataDataEvasa.Text).ToShortDateString();
            }

            p.accertatori = txtAccertatori.Text;

            p.scaturito = DdlScaturito.SelectedItem.Text;

            p.inviata = txtinviata.Text;
            if (!string.IsNullOrEmpty(txtDataInvio.Text))
            {
                p.dataInvio = System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
            }

            p.procedimentoPen = txtProdPenNr.Text;
            p.matricola = Vuser;
            p.data_ins_pratica = DateTime.Now.ToLocalTime();
            DateTime o = System.Convert.ToDateTime(HolDate.Value);


            Manager mn = new Manager();
            Boolean ins = mn.SavePraticaTrans(p, Holdmat.Value, o, HoldProtocollo.Value);
            if (!ins)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "errore durante il salvataggio." + "'); $('#errorModal').modal('show');", true);
            }

            Ricerca_Click(this, EventArgs.Empty);
        }

        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

        }
        protected void RicercaQuartiere_Click(object sender, EventArgs e)
        {
            string indirizzo = string.Empty;


            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "inserire un indirizzo" + "');", true);
            indirizzo = txtIndirizzo.Text.Trim();


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


        private void CaricaDLL()
        {
            try
            {


                Manager mn = new Manager();
                DataTable RicercaQuartiere = mn.getListQuartiere();
                DdlQuartiere.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                DdlQuartiere.DataTextField = "Quartiere"; // Il campo visibile
                                                          // DdlQuartiere.DataValueField = "Id"; // Il valore associato a ogni opzione
                DdlQuartiere.DataBind();
                DdlQuartiere.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaIndirizzo = mn.getListIndirizzo();
                DdlIndirizzo.DataSource = RicercaIndirizzo; // Imposta il DataSource della DropDownList
                DdlIndirizzo.DataTextField = "Toponimo"; // Il campo visibile
                DdlIndirizzo.DataBind();
                DdlIndirizzo.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaTipoAtto = mn.getListTipologia();
                DdlTipoAtto.DataSource = RicercaTipoAtto; // Imposta il DataSource della DropDownList
                DdlTipoAtto.DataTextField = "Tipo_Nota"; // Il campo visibile
                DdlTipoAtto.DataValueField = "Id"; // Il valore associato a ogni opzione
                DdlTipoAtto.Items.Insert(0, new ListItem("", "0"));
                DdlTipoAtto.DataBind();
                DdlTipoAtto.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaProvenienza = mn.getListProvenienza();
                DdlProvenienza.DataSource = RicercaProvenienza; // Imposta il DataSource della DropDownList
                DdlProvenienza.DataTextField = "Provenienza"; // Il campo visibile
                                                              //DdlProvenienza.DataValueField = "Id"; // Il valore associato a ogni opzione

                DdlProvenienza.DataBind();
                DdlProvenienza.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaGiudice = mn.getListGiudice();
                DdlGiudice.DataSource = RicercaGiudice; // Imposta il DataSource della DropDownList
                DdlGiudice.DataTextField = "Giudice"; // Il campo visibile
                DdlGiudice.DataValueField = "Id"; // Il valore associato a ogni opzione

                DdlGiudice.DataBind();
                DdlGiudice.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));


                DataTable RicercaProvvAg = mn.getListProvvAg();
                DdlTipoProvvAg.DataSource = RicercaProvvAg; // Imposta il DataSource della DropDownList
                DdlTipoProvvAg.DataTextField = "Tipologia"; // Il campo visibile
                                                            //DdlTipoProvvAg.DataValueField = "Id"; // Il valore associato a ogni opzione

                DdlTipoProvvAg.DataBind();
                DdlTipoProvvAg.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable Scaturito = mn.getListScaturito();
                DdlScaturito.DataSource = Scaturito; // Imposta il DataSource della DropDownList
                DdlScaturito.DataTextField = "Scaturito"; // Il campo visibile
                                                          //DdlScaturito.DataValueField = "Id"; // Il valore associato a ogni opzione

                DdlScaturito.DataBind();
                DdlScaturito.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
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
                string id = DataBinder.Eval(e.Row.DataItem, "ID").ToString();

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
                DdlQuartiere.SelectedItem.Text = selectedValue;
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
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
                if (values.Length == 3)
                {
                    Int32 protocollo = System.Convert.ToInt32(values[0]);    // Protocollo
                    string matricola = values[1];     // Matricola
                    string dataInserimento = values[2]; // DataInserimento
                    string sigla = values[3]; // sigla
                    //// Ora puoi usare questi valori per aggiornare i tuoi controlli
                    //p.nrProtocollo = System.Convert.ToInt32(protocollo);
                    //conservo la matricola precedente
                    Holdmat.Value = matricola;
                    HolDate.Value = dataInserimento;
                    HoldProtocollo.Value = protocollo.ToString();
                    //p.data_ins_pratica = System.Convert.ToDateTime(dataInserimento).ToLongDateString();
                    Manager mn = new Manager();
                    DataTable pratica = mn.getPratica(protocollo, System.Convert.ToDateTime(dataInserimento), matricola, sigla);

                    if (pratica.Rows.Count > 0)
                    {
                        txtProt.Text = pratica.Rows[0].ItemArray[0].ToString();
                        DdlSigla.SelectedItem.Text = pratica.Rows[0].ItemArray[1].ToString();
                        txtDataArrivo.Text = pratica.Rows[0].ItemArray[2].ToString();
                        DdlProvenienza.SelectedItem.Text = pratica.Rows[0].ItemArray[3].ToString();
                        DdlTipoAtto.SelectedItem.Text = pratica.Rows[0].ItemArray[4].ToString();
                        DdlGiudice.SelectedItem.Text = pratica.Rows[0].ItemArray[5].ToString();
                        DdlTipoProvvAg.SelectedItem.Text = pratica.Rows[0].ItemArray[6].ToString();
                        txtProdPenNr.Text = pratica.Rows[0].ItemArray[7].ToString();
                        txtNominativo.Text = pratica.Rows[0].ItemArray[8].ToString();
                        DdlIndirizzo.SelectedItem.Text = pratica.Rows[0].ItemArray[9].ToString();
                        txtVia.Text = pratica.Rows[0].ItemArray[10].ToString();
                        CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0].ItemArray[11]);
                        txtDataDataEvasa.Text = pratica.Rows[0].ItemArray[12].ToString();
                        txtinviata.Text = pratica.Rows[0].ItemArray[13].ToString();
                        txtDataInvio.Text = pratica.Rows[0].ItemArray[14].ToString();
                        DdlScaturito.SelectedItem.Text = pratica.Rows[0].ItemArray[15].ToString();
                        txtAccertatori.Text = pratica.Rows[0].ItemArray[16].ToString();
                        txtDataCarico.Text = pratica.Rows[0].ItemArray[17].ToString();
                        txPratica.Text = pratica.Rows[0].ItemArray[18].ToString();
                        DdlQuartiere.SelectedItem.Text = pratica.Rows[0].ItemArray[19].ToString();
                        txtNote.Text = pratica.Rows[0].ItemArray[20].ToString();
                        txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[21].ToString();
                        //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                        txtRifProtGen.Text = pratica.Rows[0].ItemArray[23].ToString();

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
        protected void NuovaRicerca_Click(object sender, EventArgs e)
        {
            DivDettagli.Visible = false;
            DivRicerca.Visible = true;
            DivGrid.Visible = false;
            txtAnnoRicerca.Text = String.Empty;
            txtProtoloccolRicerca.Text = String.Empty;
        }
        protected void gvPopupProtocolli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "Nr_Protocollo").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
        }

    }
}