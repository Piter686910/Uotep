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
    public partial class Inserimento : Page
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
            string protocolloText = ConfigurationManager.AppSettings["Titolo"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            ProtocolloLiteral.Text = decodedText;
            if (!IsPostBack)
            {
                CaricaDLL();
                if (Ruolo.ToUpper() == Enumerate.Profilo.CoordinamentoPg.ToString().ToUpper())
                {
                    DdlSigla.SelectedValue = "PG";
                }
                //Manager mn = new Manager();
                //DataTable tb = mn.MaxNPr(annoCorr);
                txtDataArrivo.Text = DateTime.Now.Date.ToShortDateString();
                //if (tb.Rows.Count > 0)
                //{
                //    txtDataArrivo.Text = DateTime.Now.Date.ToShortDateString();
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
           
            if (!String.IsNullOrEmpty( HfGiudice.Value))
            {
                txtInsGiudice.Text = HfGiudice.Value;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModalGiudice').modal('show');", true);
              
            }
            if (!String.IsNullOrEmpty(HfTipoProv.Value))
            {
                txtTipoProv.Text = HfTipoProv.Value;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModaTipoProv').modal('show');", true);

            }


        }

        protected void Salva_Click(object sender, EventArgs e)
        {
           
            int protocollo = 0;
            Principale p = new Principale();
            p.anno = annoCorr;
            DateTime giorno = DateTime.Now;
            p.giorno = giorno.ToString("dddd", new CultureInfo("it-IT"));


            //
            Manager mn = new Manager();
            DataTable tb = mn.MaxNPr(annoCorr);
            txtDataArrivo.Text = DateTime.Now.Date.ToShortDateString();
            if (tb.Rows.Count > 0)
            {
                txtDataArrivo.Text = DateTime.Now.Date.ToShortDateString();
                int annoMAx = System.Convert.ToInt16(tb.Rows[0].ItemArray[0]);

                if (System.Convert.ToInt16(annoCorr) <= annoMAx)
                {
                    protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[1]) + 1;
                    txtProt.Text = protocollo.ToString();//tb.Rows[0].ItemArray[1].ToString();
                }
                else
                {
                    protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[1]) + 1;
                    txtProt.Text = protocollo.ToString();

                }
            }
            else
            {
                txtProt.Text = "1";

            }
            p.nrProtocollo = System.Convert.ToInt32(txtProt.Text);
            //



            p.sigla = DdlSigla.SelectedItem.Text;
            p.dataArrivo = System.Convert.ToDateTime(txtDataArrivo.Text).ToShortDateString();
            //p.dataCarico = null; //System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
            p.nominativo = txtNominativo.Text;
            //if (DdlGiudice.SelectedValue == "0")
            //{
            //    p.giudice = String.Empty;
            //}
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
                if (!String.IsNullOrEmpty(txtTestoProvenienza.Text))
                {
                    p.provenienza = txtProvenienza.Text;
                }

                else
                    p.provenienza = String.Empty;
            }
            else
            {
                p.provenienza = txtProvenienza.Text;
            }
            if (String.IsNullOrEmpty(txtTipoAtto.Text))
            {

                p.tipologia_atto = String.Empty;
            }
            else
            {
                p.tipologia_atto = txtTipoAtto.Text;
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
           
            p.rif_Prot_Gen = txtRifProtGen.Text;
           

            p.note = txtNote.Text;
            p.evasa = CkEvasa.Checked;
            if (!string.IsNullOrEmpty(txtDataDataEvasa.Text))
            {
                p.evasaData = System.Convert.ToDateTime(txtDataDataEvasa.Text).ToShortDateString();
            }

            //p.accertatori = null;
            //p.scaturito = null;
            if (String.IsNullOrEmpty(txtInviata.Text))
            {
                p.inviata = String.Empty;
            }
            else
            {
                p.inviata = txtInviata.Text;
               
            }
            //p.inviata = DdlInviati.SelectedItem.Text;
            if (!string.IsNullOrEmpty(txtDataInvio.Text))
            {
                p.dataInvio = System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
            }

            p.procedimentoPen = txtProdPenNr.Text;
            p.matricola = Vuser;
            p.data_ins_pratica = DateTime.Now.ToLocalTime();


            Boolean ins = mn.SavePratica(p);
            if (!ins)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
            }
            else
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Protocollo " + p.nrProtocollo + " inserito correttamente ." + "'); $('#errorModal').modal('show');", true);

                Pulisci();
            }
        }
        private void Pulisci()
        {
            Convalida();
            //txtProt.Text = String.Empty;
            txtTestoProvenienza.Text = string.Empty;
            txtGiudice.Text = string.Empty; 
            txtInsGiudice.Text = string.Empty;
            HfGiudice.Value = string.Empty;
            txtQuartiere.Text = string.Empty;   
            HfQuartiere.Value = string.Empty;
            txtInviata.Text = string.Empty;
            HfInviata.Value = string.Empty;     
            txtTipoAtto.Text = string.Empty;
            HfTipoAtto.Value = string.Empty;
            txtIndirizzo.Text = string.Empty;
            HfIndirizzo.Value = string.Empty;
            txtProvenienza.Text = string.Empty;
            HfProvenienza.Value = string.Empty;
            txtTipoProv.Text = string.Empty;
            HfTipoProv.Value = string.Empty;
            txPratica.Text = String.Empty;
            txtDataArrivo.Text = String.Empty;
            txtRifProtGen.Text = String.Empty;
          //  txtVia.Text = String.Empty;
            txtProdPenNr.Text = String.Empty;
            txtNominativo.Text = String.Empty;
            txPratica.Text = String.Empty;
            txtNote.Text = String.Empty;
            txtDataDataEvasa.Text = String.Empty;

            txtDataInvio.Text = String.Empty;
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
        protected void chiudipopupTipoProv_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModaTipoProv')); modal.hide();", true);

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
               // DdlQuartiere.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaIndirizzo = mn.getListIndirizzo();
                DdlIndirizzo.DataSource = RicercaIndirizzo; // Imposta il DataSource della DropDownList
                DdlIndirizzo.DataTextField = "SpecieToponimo"; // Il campo visibile
                DdlQuartiere.DataValueField = "ID_quartiere"; // Il valore associato a ogni opzione
                DdlIndirizzo.DataBind();
               // DdlIndirizzo.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

                DataTable RicercaTipoAtto = mn.getListTipologia();
                DdlTipoAtto.DataSource = RicercaTipoAtto; // Imposta il DataSource della DropDownList
                DdlTipoAtto.DataTextField = "Tipo_Nota"; // Il campo visibile
                DdlTipoAtto.DataValueField = "id_tipo_nota"; // Il valore associato a ogni opzione
                DdlTipoAtto.DataBind();
                // DdlTipoAtto.Items.Insert(0, new ListItem("", "0"));

                // DdlTipoAtto.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

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
                //DdlGiudice.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));


                DataTable RicercaProvvAg = mn.getListProvvAg();
                DdlTipoProvvAg.DataSource = RicercaProvvAg; // Imposta il DataSource della DropDownList
                DdlTipoProvvAg.DataTextField = "Tipologia"; // Il campo visibile
                DdlTipoProvvAg.DataValueField = "id_tipo_nota_ag"; // Il valore associato a ogni opzione

                DdlTipoProvvAg.DataBind();
             //   DdlTipoProvvAg.Items.Insert(0, new ListItem("-- Seleziona un'opzione --", "0"));

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
                //txtSelectedValue.Text = selectedValue;
                txtQuartiere.Text = selectedValue;
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }



        protected void btInsProvenienza_Click(object sender, EventArgs e)
        {
            txtProvenienza.Text = txtTestoProvenienza.Text;
        }
        protected void btInsGiudice_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean ins = mn.InserisciGiudice(HfGiudice.Value);
            //DdlProvenienza.SelectedItem.Text = txtTestoProvenienza.Text;
        }

        protected void btProv_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean ins = mn.InserisciTipologiaNotaAg(HfTipoProv.Value);
        }
    }
}