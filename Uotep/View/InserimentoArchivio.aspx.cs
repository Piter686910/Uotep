using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;


namespace Uotep
{
    public partial class InserimentoArchivio : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String Ruolo = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

        protected void Page_Load(object sender, EventArgs e)
        {


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
                CaricaDLL();

                //Manager mn = new Manager();
                //DataTable tb = mn.MaxNPr(annoCorr);
                txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
                //if (tb.Rows.Count > 0)
                //{
                //    txtDataInserimento.Text = DateTime.Now.Date.ToShortDateString();
                //    int annoMAx = System.Convert.ToInt16(tb.Rows[0].ItemArray[0]);

                //    if (System.Convert.ToInt16(annoCorr) <= annoMAx)
                //    {
                //        protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[1]) + 1;
                //        txtProt.Text = protocollo.ToString();//tb.Rows[0].ItemArray[1].ToString();
                //    }
                //    else
                //    {
                //        protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[1]) + 1;
                //        txtProt.Text = protocollo.ToString();

                //    }
                //}
                //else
                //{
                //    txtProt.Text = "1";

                //}
            }

        }
        public void Convalida()
        {



            //if (!String.IsNullOrEmpty(HfTipoAtto.Value))
            //    btSalvaTipoAtto.Visible = true;


        }

        protected void Salva_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();

            ArchivioUote arch = new ArchivioUote();
            arch.arch_numPratica = txtPratica.Text;
            //DateTime giorno = DateTime.Now;
            arch.arch_dataIns = System.Convert.ToDateTime(txtDataInserimento.Text);   // giorno.ToString("dddd", new CultureInfo("it-IT"));
            arch.arch_dataNascita = System.Convert.ToDateTime(txtDataNascita.Text);
            arch.arch_datault_intervento = System.Convert.ToDateTime(txtDataUltimoIntervento.Text);
            arch.arch_tipologia = txtTipoAtto.Text;
            arch.arch_note = txtNote.Text;
            arch.arch_quartiere = txtQuartiere.Text;
            arch.arch_matricola = Session["user"].ToString();
            arch.arch_indirizzo = txtIndirizzo.Text;
            arch.arch_nominativo = txtNominativo.Text;
            arch.arch_responsabile = txtResponsabile.Text;
            arch.arch_vincoli = CkVincoli.Checked;
            arch.arch_suoloPub = CkSuoloPubblico.Checked;
            arch.arch_1089 = Ck1089.Checked;
            arch.arch_evasa = CkEvasa.Checked;
            arch.arch_demolita = CkDemolita.Checked;
            arch.arch_inCarico = txtInCarico.Text;


            Boolean ins = mn.SavePraticaArchivioUote(arch);
            if (!ins)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica " + arch.arch_numPratica + " inserita correttamente ." + "'); $('#errorModal').modal('show');", true);

                Pulisci();
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
            txtNominativo.Text = String.Empty;
            txtDataUltimoIntervento.Text = String.Empty;
            txtNote.Text = String.Empty;
            txtDataNascita.Text = String.Empty;
            Ck1089.Checked = false;
            CkSuoloPubblico.Checked = false;
            CkDemolita.Checked = false;
            CkVincoli.Checked = false;
            CkEvasa.Checked = false;
            CaricaDLL();

        }
        //popup giudice
        protected void apripopupGiudice_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModalGiudice').modal('show');", true);
        }
        //tipo prov
        protected void apripopupTipoProv_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModaTipoProv').modal('show');", true);
        }

        protected void chiudipopupTipoAtto_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModalTipoAtto')); modal.hide();", true);

        }
        protected void chiudipopupTipoProv_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModaTipoProv')); modal.hide();", true);

        }
        protected void chiudipopupInviata_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModalInviata')); modal.hide();", true);

        }
        protected void chiudipopupGiudice_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModalGiudice')); modal.hide();", true);

        }
        //popup provenienza
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
        protected void chiudipopupErrore_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
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
                //else
                //{
                //    lblQuartiere.Text = "Quartiere non trovato.";
                //}
            }
            //else
            //{
            //    lblQuartiere.Text = "Inserisci un indirizzo valido.";
            //}

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
                DdlQuartiere.DataSource = RicercaQuartiere; // Imposta il DataSource della DropDownList
                DdlQuartiere.DataTextField = "Quartiere"; // Il campo visibile
                //DdlQuartiere.DataValueField = "ID_quartiere"; // Il valore associato a ogni opzione
                DdlQuartiere.DataBind();
                //// DdlQuartiere.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaIndirizzo = mn.getListIndirizzo();
                DdlIndirizzo.DataSource = RicercaIndirizzo; // Imposta il DataSource della DropDownList
                DdlIndirizzo.DataTextField = "SpecieToponimo"; // Il campo visibile
                DdlQuartiere.DataValueField = "ID_quartiere"; // Il valore associato a ogni opzione
                DdlIndirizzo.DataBind();
                //// DdlIndirizzo.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaTipoAtto = mn.getListTipologia();
                DdlTipoAtto.DataSource = RicercaTipoAtto; // Imposta il DataSource della DropDownList
                DdlTipoAtto.DataTextField = "Tipo_Nota"; // Il campo visibile
                DdlTipoAtto.DataValueField = "id_tipo_nota"; // Il valore associato a ogni opzione
                DdlTipoAtto.DataBind();
                // DdlTipoAtto.Items.Insert(0, new ListItem("", "0"));

                // DdlTipoAtto.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));



                DataTable RicercaGiudice = mn.getListGiudice();
                DdlGiudice.DataSource = RicercaGiudice; // Imposta il DataSource della DropDownList
                DdlGiudice.DataTextField = "Giudice"; // Il campo visibile
                DdlGiudice.DataValueField = "ID_giudice"; // Il valore associato a ogni opzione

                DdlGiudice.DataBind();
                //DdlGiudice.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));



                DataTable RicercaInviati = mn.getListInviati();
                DdlInviati.DataSource = RicercaInviati; // Imposta il DataSource della DropDownList
                DdlInviati.DataTextField = "Inviata"; // Il campo visibile
                DdlInviati.DataValueField = "id_inviata"; // Il valore associato a ogni opzione
                DdlInviati.DataBind();
                // DdlInviati.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));
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