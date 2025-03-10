﻿using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;

namespace Uotep
{
    public partial class Modifica : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        Principale p = new Principale();
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
            //Manager mn = new Manager();
            //DataTable ricerca = mn.GetRuolo(Vuser);
            //if (ricerca.Rows.Count > 0)
            //{

            //Session["profilo"] = ricerca.Rows[0].ItemArray[0];

            if (!IsPostBack)
            {
                DivRicerca.Visible = false;
                CaricaDLL();

            }
            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "UTENTE NON AUTORIZZATO" + "'); $('#errorModal').modal('show');", true);
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
                //DdlQuartiere.DataValueField = "ID_quartiere"; // Il valore associato a ogni opzione
                DdlQuartiere.DataBind();
                DdlQuartiere.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaIndirizzo = mn.getListIndirizzo();
                DdlIndirizzo.DataSource = RicercaIndirizzo; // Imposta il DataSource della DropDownList
                DdlIndirizzo.DataTextField = "Toponimo"; // Il campo visibile
                DdlIndirizzo.DataBind();
                DdlIndirizzo.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable Scaturito = mn.getListScaturito();
                DdlScaturito.DataSource = Scaturito; // Imposta il DataSource della DropDownList
                DdlScaturito.DataTextField = "Scaturito"; // Il campo visibile
                DdlScaturito.DataValueField = "Id_scaturito"; // Il valore associato a ogni opzione

                DdlScaturito.DataBind();
                DdlScaturito.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable Inviati = mn.getListInviati();
                DdlInviati.DataSource = Inviati; // Imposta il DataSource della DropDownList
                DdlInviati.DataTextField = "Inviata"; // Il campo visibile
                DdlInviati.DataValueField = "Id_inviata"; // Il valore associato a ogni opzione

                DdlInviati.DataBind();
                DdlInviati.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
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
            Principale p = new Principale();
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
            }
            //p.anno = annoCorr;
            //p.giorno = DateTime.Now.Day.ToString();
            //p.nrProtocollo = System.Convert.ToInt32(txtProt.Text);
            //p.sigla = DdlSigla.SelectedItem.Text;
            //p.dataArrivo = System.Convert.ToDateTime(txtDataArrivo.Text).ToShortDateString();
            p.dataCarico = DateTime.Now.ToShortDateString();
            p.nominativo = txtNominativo.Text;


            if (DdlIndirizzo.SelectedValue == "0")
            {
                p.indirizzo = String.Empty;
            }
            else
            {
                p.indirizzo = DdlIndirizzo.SelectedItem.Text;
                p.via = txtVia.Text;
            }
            if (DdlQuartiere.SelectedValue == "0")
            {
                p.quartiere = String.Empty;
            }
            else
            {
                p.quartiere = DdlQuartiere.SelectedItem.Text;

            }

            p.note = txtNote.Text;
            p.evasa = CkEvasa.Checked;
            if (!string.IsNullOrEmpty(txtDataDataEvasa.Text))
            {
                p.evasaData = System.Convert.ToDateTime(txtDataDataEvasa.Text).ToShortDateString();
            }

            p.accertatori = txtAccertatori.Text;
            p.scaturito = DdlScaturito.SelectedItem.Text;
            p.inviata = DdlInviati.SelectedItem.Text;
            if (!string.IsNullOrEmpty(txtDataInvio.Text))
            {
                p.dataInvio = System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
            }

            //p.procedimentoPen = txtProdPenNr.Text;
            //matricola del popup
            p.matricola = Vuser;
            //string newMat = ;

            p.data_ins_pratica = DateTime.Now.ToLocalTime();
            p.nrProtocollo = System.Convert.ToInt32(txtProt.Text.Trim());
            DateTime o = System.Convert.ToDateTime(HolDate.Value);

            Manager mn = new Manager();
            Boolean ins = mn.UpdPratica(p, Holdmat.Value, o);
            if (!ins)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "modifica non effettuata, controllare il log." + "'); $('#errorModal').modal('show');", true);
            }
            else
            {
                DivDettagli.Visible = false;
                Pulisci();
            }
        }
        private void Pulisci()
        {
            txtAnnoRicerca.Text = String.Empty;
            txPratica.Text = String.Empty;
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
            txtDatCaricoDa.Text = String.Empty;
            txtDatCaricoDa.Text = String.Empty;

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
                pratica = mn.getListDataCarico(txtDatCaricoDa.Text, txtDatCaricoA.Text);
            }

            if (pratica.Rows.Count > 0)
            {
                gvPopupD.DataSource = pratica;
                gvPopupD.DataBind();
                //DivDettagli.Visible = true;
                //DivRicerca.Visible = false;
                DivGrid.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
            }


            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non trovata." + "'); $('#errorModal').modal('show');", true);
            }
        }
        //protected void Login1_LoginError(object sender, EventArgs e)
        //{
        //    string errorMessage = "errore";
        //    ScriptManager.RegisterStartupScript(this, GetType(), "errorModal", $"customErrorModal('{"hvjcklsdhvsjkl"}');", true);
        //    // Invio dello script al client per mostrare la modale
        //    ScriptManager.RegisterStartupScript(this, GetType(), "showErrorModal", $"showModal('{errorMessage}');", true);
        //}
        protected void RicercaQuartiere_Click(object sender, EventArgs e)
        {
            string indirizzo = string.Empty;


            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "inserire un indirizzo" + "');", true);
            indirizzo = txtIndirizzo.Text.Trim();

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
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Non è stato possibile caricare la tabella quartieri." + "'); $('#errorModal').modal('show');", true);

                }
            }


            // Mantieni il popup aperto dopo l'interazione lato server.
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);


        }

        protected void gvPopupD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore del CommandArgument
                string commandArgument = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = commandArgument.Split('|');

                // Assicurati che ci siano almeno 3 valori
                if (values.Length == 4)
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
                    //p.data_ins_pratica = System.Convert.ToDateTime(dataInserimento).ToLongDateString();
                    Manager mn = new Manager();
                    DataTable pratica = mn.getPratica(protocollo, System.Convert.ToDateTime(dataInserimento), sigla);
                    if (pratica.Rows.Count > 0)
                    {
                        txtProt.Text = pratica.Rows[0].ItemArray[0].ToString();
                        txtSigla.Text = pratica.Rows[0].ItemArray[1].ToString();
                        txtDataArrivo.Text = pratica.Rows[0].ItemArray[2].ToString();
                        txtProvenienza.Text = pratica.Rows[0].ItemArray[3].ToString();
                        txtTipoAtto.Text = pratica.Rows[0].ItemArray[4].ToString();
                        txtGiudice.Text = pratica.Rows[0].ItemArray[5].ToString();
                        TxtTipoProvvAg.Text = pratica.Rows[0].ItemArray[6].ToString();
                        txtProdPenNr.Text = pratica.Rows[0].ItemArray[7].ToString();
                        txtNominativo.Text = pratica.Rows[0].ItemArray[8].ToString();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[9].ToString()))
                            DdlIndirizzo.SelectedItem.Text = pratica.Rows[0].ItemArray[9].ToString();
                        txtVia.Text = pratica.Rows[0].ItemArray[10].ToString();
                        CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0].ItemArray[11]);
                        txtDataDataEvasa.Text = pratica.Rows[0].ItemArray[12].ToString();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[13].ToString()))
                            DdlInviati.SelectedItem.Text = pratica.Rows[0].ItemArray[13].ToString();
                        txtDataInvio.Text = pratica.Rows[0].ItemArray[14].ToString();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[15].ToString()))

                            DdlScaturito.SelectedItem.Text = pratica.Rows[0].ItemArray[15].ToString();
                        txtAccertatori.Text = pratica.Rows[0].ItemArray[16].ToString();
                        txtDataCarico.Text = pratica.Rows[0].ItemArray[17].ToString();
                        txPratica.Text = pratica.Rows[0].ItemArray[18].ToString();
                        if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[19].ToString()))

                            DdlQuartiere.SelectedItem.Text = pratica.Rows[0].ItemArray[19].ToString();
                        txtNote.Text = pratica.Rows[0].ItemArray[20].ToString();
                        txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[21].ToString();
                        //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                        txtRifProtGen.Text = pratica.Rows[0].ItemArray[23].ToString();

                        // Puoi anche chiudere il popup se necessario
                      //  ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#myModal').modal('hide');", true); // Puoi anche chiudere il popup se necessario
                        ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#ModalRicerca').modal('hide');", true);
                        DivDettagli.Visible = true;
                        DivRicerca.Visible = false;
                        Pulisci();
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
            }
        }

        //gridview per quartiere
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
                DdlQuartiere.SelectedItem.Text = selectedValue;
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);
            Pulisci();
        }
        protected void NascondiDiv()
        {
            DivProtocollo.Visible = false;
            DivEvasaAg.Visible = false;
            DivProtGen.Visible = false;
            DivIndirizzo.Visible = false;
            DivAccertatori.Visible = false;
            DivDataCarico.Visible = false;
            DivNominativo.Visible = false;
            DivProvenienza.Visible = false;
            DivGiudice.Visible = false;
            DivPratica.Visible = false;
            DivProcPenale.Visible = false;
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

        protected void btDataCarico_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivDataCarico.Visible = true;
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
    }
}