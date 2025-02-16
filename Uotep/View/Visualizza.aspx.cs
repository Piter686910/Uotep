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
    public partial class Visualizza : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        Principale p = new Principale(); public String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

        protected void Page_Load(object sender, EventArgs e)
        {
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
            txtDatCaricoA.Text = String.Empty;
            txtDatCaricoDa.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
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
            if (txtDatCaricoDa.Text != string.Empty && txtDatCaricoA.Text != string.Empty)
            {
                pratica = mn.getListDataCarico(txtDatCaricoDa.Text, txtDatCaricoA.Text);
            }
            if (pratica.Rows.Count > 0)
            {
                gvPopup.DataSource = pratica;
                gvPopup.DataBind();
                DivDettagli.Visible = true;
                DivRicerca.Visible = false;
                DivGrid.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non presente in database." + "'); $('#errorModal').modal('show');", true);
            }

        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

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
                    if (values.Length == 3)
                    {
                        protocollo = System.Convert.ToInt32(values[0]);    // Protocollo
                        matricola = values[1];     // Matricola
                        dataInserimento = values[2]; // DataInserimento
                        sigla = values[3];  // sigla


                        //// Ora puoi usare questi valori per aggiornare i tuoi controlli
                        //p.nrProtocollo = System.Convert.ToInt32(protocollo);
                        //p.matricola = matricola;
                        p.data_ins_pratica = System.Convert.ToDateTime(dataInserimento).ToLocalTime();
                        Manager mn = new Manager();
                        DataTable pratica = mn.getPratica(protocollo, System.Convert.ToDateTime(dataInserimento), sigla);
                        if (pratica.Rows.Count > 0)
                        {
                            txtProt.Text = pratica.Rows[0].ItemArray[0].ToString();
                            txtSigla.Text = pratica.Rows[0].ItemArray[1].ToString();
                            try
                            {
                                txtDataArrivo.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[2].ToString()).ToShortDateString();
                            }
                            catch (Exception)
                            {

                                txtDataArrivo.Text = string.Empty;
                            }

                            txtProvenienza.Text = pratica.Rows[0].ItemArray[3].ToString();
                            txtTipoAtto.Text = pratica.Rows[0].ItemArray[4].ToString();
                            txtGiudice.Text = pratica.Rows[0].ItemArray[5].ToString();
                            TxtTipoProvvAg.Text = pratica.Rows[0].ItemArray[6].ToString();
                            txtProdPenNr.Text = pratica.Rows[0].ItemArray[7].ToString();
                            txtNominativo.Text = pratica.Rows[0].ItemArray[8].ToString();
                            txtIndirizzo.Text = pratica.Rows[0].ItemArray[9].ToString() + " " + pratica.Rows[0].ItemArray[10].ToString();

                            CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0].ItemArray[11]);
                            try
                            {
                                txtDataDataEvasa.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[12].ToString()).ToShortDateString();
                            }
                            catch (Exception)
                            {

                                txtDataDataEvasa.Text = string.Empty;
                            }

                            txtinviata.Text = pratica.Rows[0].ItemArray[13].ToString();
                            try
                            {
                                txtDataInvio.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[14].ToString()).ToShortDateString();
                            }
                            catch (Exception)
                            {

                                txtDataInvio.Text = string.Empty;
                            }

                            txtScaturito.Text = pratica.Rows[0].ItemArray[15].ToString();
                            txtAccertatori.Text = pratica.Rows[0].ItemArray[16].ToString();
                            try
                            {
                                txtDataCarico.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[17].ToString()).ToShortDateString();
                            }
                            catch (Exception)
                            {

                                txtDataCarico.Text = string.Empty;
                            }

                            txPratica.Text = pratica.Rows[0].ItemArray[18].ToString();
                            TxtQuartiere.Text = pratica.Rows[0].ItemArray[19].ToString();
                            txtNote.Text = pratica.Rows[0].ItemArray[20].ToString();
                            txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[21].ToString();
                            //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                            txtRifProtGen.Text = pratica.Rows[0].ItemArray[23].ToString();

                            // Puoi anche chiudere il popup se necessario
                            //ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#myModal').modal('hide');", true);
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
            DivDataCarico.Visible = true;
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
            DivDataCarico.Visible = false;
        }


    }
}