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
    public partial class Inserimento : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String Vruolo = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

        protected void Page_Load(object sender, EventArgs e)
        {

            int protocollo = 0;
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Vruolo = Session["ruolo"].ToString();
            }
            // Legge il valore dal Web.config
            string protocolloText = ConfigurationManager.AppSettings["Titolo"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            ProtocolloLiteral.Text = decodedText;
            if (!IsPostBack)
            {
                if (Vruolo == "coordinamento pg")
                {
                    DdlSigla.SelectedItem.Text = "PG";
                }
                else
                    DdlSigla.SelectedItem.Text = "ED";
                CaricaDLL();
                Manager mn = new Manager();
                DataTable tb = mn.MaxNPr(annoCorr);
                txtDataArrivo.Text = DateTime.Now.Date.ToShortDateString();
                if (tb.Rows.Count > 0)
                {
                    txtDataArrivo.Text = DateTime.Now.Date.ToShortDateString();
                    
                    int annoMAx = System.Convert.ToInt16(tb.Rows[0].ItemArray[0]);
                    if (System.Convert.ToInt16(annoCorr) <= annoMAx)
                    {
                        protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[0]) + 1;
                        txtProt.Text = protocollo.ToString();//tb.Rows[0].ItemArray[1].ToString();
                    }
                    else
                    {
                        protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[0]) + 1;
                        txtProt.Text = protocollo.ToString();
                    }
                }
                else
                {
                    txtProt.Text = "1";

                }
            }

        }

        protected void Salva_Click(object sender, EventArgs e)
        {
            Principale p = new Principale();
            p.anno = annoCorr;
            DateTime giorno = DateTime.Now;
            p.giorno = System.Globalization.CultureInfo.GetCultureInfo("it-IT").DateTimeFormat.GetDayName(giorno.DayOfWeek);

           
           
            p.nrProtocollo = System.Convert.ToInt32(txtProt.Text);
            
            p.sigla = DdlSigla.SelectedItem.Text;
            
           
            p.dataArrivo = System.Convert.ToDateTime(txtDataArrivo.Text).ToShortDateString();
            //p.dataCarico = null; //System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
            p.nominativo = txtNominativo.Text;

            p.giudice = DdlGiudice.SelectedItem.Text;

            if (DdlProvenienza.SelectedValue == "0")
            {
                p.provenienza = String.Empty;
            }
            else
            {
                p.provenienza = DdlProvenienza.SelectedItem.Text;
            }
            if (DdlTipoAtto.SelectedValue == "0")
            {
                p.tipologia_atto = String.Empty;
            }
            else
            {
                p.tipologia_atto = DdlTipoAtto.SelectedItem.Text;
            }
            if (DdlTipoProvvAg.SelectedValue == "0")
            {
                p.tipoProvvedimentoAG = String.Empty;
            }
            else
            {
                p.tipoProvvedimentoAG = DdlTipoProvvAg.SelectedItem.Text;
            }

            p.rif_Prot_Gen = txtRifProtGen.Text;
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
                //p.quartiere = lblQuartiere.Text;
            }

            p.note = txtNote.Text;
            p.evasa = CkEvasa.Checked;
            if (!string.IsNullOrEmpty(txtDataDataEvasa.Text))
            {
                p.evasaData = System.Convert.ToDateTime(txtDataDataEvasa.Text).ToShortDateString();
            }

            //p.accertatori = null;
            //p.scaturito = null;
            p.inviata = txtinviata.Text;
            if (!string.IsNullOrEmpty(txtDataInvio.Text))
            {
                p.dataInvio = System.Convert.ToDateTime(txtDataInvio.Text).ToShortDateString();
            }

            p.procedimentoPen = txtProdPenNr.Text;
            p.matricola = Vuser;
            p.data_ins_pratica = DateTime.Now.ToLocalTime();



            Manager mn = new Manager();
            Boolean ins = mn.SavePratica(p);
            if (!ins)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento della pratica non riuscito, controllare il log." + "'); $('#errorModal').modal('show');", true);
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

    }
}