using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Interop;
using Uotep.Classi;
using WebGrease.Activities;
using static Uotep.Classi.Enumerate;

namespace Uotep
{
    public partial class Visualizza : Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String Ruolo = String.Empty;
        String Profilo = String.Empty;
        Principale p = new Principale(); public String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        string paginaChiamante = "~/View/Visualizza.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PaginaChiamante"] = paginaChiamante;
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Ruolo = Session["ruolo"].ToString();
                Profilo = Session["profilo"].ToString();
                //btOKDup.Visible = true;
                if (Ruolo.ToUpper() == Enumerate.Ruolo.Archivio.GetDescription().ToUpper())
                    //{
                    btModifica.Visible = false;
                //}
                //else
                //    btDuplica.Visible = true;
            }
            else
            {
                //btOKDup.Visible = false; // non visualizzo ok per non confondere l'utente perchè in questo caso non serve il button ok
                //TextMessage.InnerText = Enumerate.MsgOutput.SScaduta.GetDescription();
                ////TextMessage.InnerHtml = "style=""";
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#TextMessage').text('" + ".." + "'); $('#MsgModal').modal('show');", true);
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=false");
                    Response.Redirect(url, false);
                    return;
                }
            }


            // Legge il valore dal Web.config
            string protocolloText = ConfigurationManager.AppSettings["Titolo"];

            // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            string decodedText = HttpUtility.HtmlDecode(protocolloText);

            // Assegna il valore decodificato al Literal
            // ProtocolloLiteral.Text = decodedText;
            //int protocollo = 0;
            if (!IsPostBack)
            {
                DivDettagli.Visible = false;
                string idCarico = Request.QueryString["idscheda"];
                if (!String.IsNullOrEmpty(idCarico))
                {
                    txtNProtocollo.Text = Request.QueryString["Nr_Protocollo"];
                    txtAnnoRicerca.Text = Request.QueryString["Anno"];
                    //Ricerca_Click(sender, e);
                    Manager mn = new Manager();
                    DataTable pratica = mn.getPraticaId(Convert.ToInt32(idCarico));
                    FillScheda(pratica, mn);
                }

            }

        }

        protected void NuovaRicerca_Click(object sender, EventArgs e)
        {
            NascondiDiv();

            Pulisci();
        }
        protected void Ricerca_Click(object sender, EventArgs e)
        {
            String msg = string.Empty;
            Manager mn = new Manager();
            Boolean validazione = false;
            DataTable pratica = new DataTable();
            if (txtRicPraticaVal.Text != string.Empty && txtRicAnnoVal.Text != string.Empty)
            {
                pratica = mn.getListPraticheVal(txtRicPraticaVal.Text, txtRicAnnoVal.Text, out msg);
                validazione = true;
            }
            if (txtNProtocollo.Text != string.Empty && txtAnnoRicerca.Text != string.Empty)
            {
                pratica = mn.getListPrototocollo(Vuser, txtNProtocollo.Text, txtAnnoRicerca.Text, out msg);
            }
            if (txtProcPenale.Text != string.Empty)
            {
                pratica = mn.getListProcedimento(txtProcPenale.Text, out msg);
            }
            if (txtDataDa.Text != string.Empty && txtDataA.Text != string.Empty)
            {
                pratica = mn.getListEvasaAg(txtDataDa.Text, txtDataA.Text, out msg);
            }
            if (txtProtGen.Text != string.Empty)
            {
                pratica = mn.getListProtGen(txtProtGen.Text, out msg);
                if (pratica.Rows.Count == 0)
                {
                    pratica = mn.getListProtGenInDecretazione(txtProtGen.Text, out msg);
                }
            }
            if (txtPratica.Text != string.Empty)
            {
                pratica = mn.getListPratica(txtPratica.Text.Trim(), out msg);
            }
            if (txtRicGiudice.Text != string.Empty)
            {
                pratica = mn.getListGiudice(txtRicGiudice.Text, out msg);
            }
            if (txtRicProvenienza.Text != string.Empty)
            {
                pratica = mn.getListProvenienza(txtRicProvenienza.Text, out msg);
            }
            if (txtRicNominativo.Text != string.Empty)
            {
                pratica = mn.getListNominativo(txtRicNominativo.Text, out msg);


            }
            if (txtRicAccertatori.Text != string.Empty)
            {
                pratica = mn.getListAccertatori(txtRicAccertatori.Text, out msg);
            }
            if (txtRicIndirizzo.Text != string.Empty)
            {
                pratica = mn.getListIndirizzo(txtRicIndirizzo.Text, out msg);
            }
            if (txtDatArrivoDa.Text != string.Empty && txtDatArrivoA.Text != string.Empty)
            {
                pratica = mn.getListDataArrivo(txtDatArrivoDa.Text, txtDatArrivoA.Text, out msg);
            }
            if (txtNote.Text != string.Empty)
            {
                pratica = mn.getListByNote(txtNote.Text, out msg);
            }
            if (pratica.Rows.Count > 0)
            {
                // Salva datatable pratica  nella Sessione
                Session["ListPratiche"] = pratica;
                //gvPopup.DataSource = pratica;
                //gvPopup.DataBind();
                if (validazione)
                {
                    GVPratica.PageIndex = 0;
                    GVPratica.DataSource = pratica;
                    GVPratica.DataBind();
                    DivGridVal.Visible = true;
                }
                else
                {
                    //          gvPopup.PageIndex = 0;
                    gvPopup.DataSource = pratica;
                    gvPopup.DataBind();
                    DivGrid.Visible = true;
                    DivGridVal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
                }
                string a = pratica.Rows[0].ItemArray[1].ToString();
                //DataTable decretazione = new DataTable();

                //decretazione =  mn.getListDecretazione(pratica.Rows[0].ItemArray[1].ToString(), pratica.Rows[0].ItemArray[0].ToString());
                //if (decretazione.Rows.Count > 0)
                //{
                //    GVDecretazione.DataSource = decretazione;
                //    GVDecretazione.DataBind();
                //    divDecretazione.Visible = true;
                //}
                //else
                //    divDecretazione.Visible = false;


            }
            else
            {
                if (!String.IsNullOrWhiteSpace(msg))
                {

                    Routine R = new Routine();
                    R.PagError(msg, paginaChiamante);
                }
                else
                {

                    //richiama popup dalla site master
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.PraticaNotFound.GetDescription(), "warning");
                    }
                }
                // ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Pratica non presente in database." + "'); $('#errorModal').modal('show');", true);
            }

        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {

            //  ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalRicerca')); modal.hide();", true);
            //adegua chiusura popup bootstrap 5
            string script = @"
    var modalElement = document.getElementById('ModalRicerca');
    if (modalElement) {
        var modalInstance = bootstrap.Modal.getInstance(modalElement);
        if (!modalInstance) {
            modalInstance = new bootstrap.Modal(modalElement);
        }
        modalInstance.hide();
    }";
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", script, true);
            Pulisci();
            Session.Remove("ListPratiche");
            Session.Remove("ListRicerca");
            gvPopup.PageIndex = 0;

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
                    if (values.Length == 5)
                    {
                        protocollo = System.Convert.ToInt32(values[0]);    // Protocollo
                        matricola = values[1];     // Matricola
                        dataInserimento = values[2]; // DataInserimento
                        sigla = values[3];  // sigla
                        HidPratica.Value = values[4]; // id



                        //p.nrProtocollo = System.Convert.ToInt32(protocollo);
                        //p.matricola = matricola;
                        if (!String.IsNullOrEmpty(dataInserimento))
                        {
                            p.data_ins_pratica = System.Convert.ToDateTime(dataInserimento).ToLocalTime();


                        }
                        Manager mn = new Manager();
                        DataTable pratica = mn.getPraticaProtocolloDataSiglaId(protocollo, System.Convert.ToDateTime(dataInserimento), sigla, System.Convert.ToInt32(HidPratica.Value));
                        FillScheda(pratica, mn);
                        //if (pratica.Rows.Count > 0)
                        //{
                        //    Pulisci();
                        //    txtProt.Text = pratica.Rows[0].ItemArray[1].ToString() + " - " + pratica.Rows[0].ItemArray[2].ToString();
                        //    // txtSigla.Text = pratica.Rows[0].ItemArray[2].ToString();
                        //    if (pratica.Rows[0].ItemArray[2].ToString() == Enumerate.Sigla.AG.ToString().ToUpper())
                        //    {
                        //        divAg.Visible = true;
                        //    }
                        //    else
                        //    {
                        //        divAg.Visible = false;

                        //    }
                        //    if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[3].ToString()))

                        //        txtDataInsCarico.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[3].ToString()).ToShortDateString();


                        //    txtProvenienza.Text = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                        //    txtProvenienza.ToolTip = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                        //    txtTipoAtto.Text = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                        //    txtTipoAtto.ToolTip = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                        //    txtUltTipoAtto.Text = pratica.Rows[0].ItemArray[28].ToString().ToUpper();
                        //    txtUltTipoAtto.ToolTip = pratica.Rows[0].ItemArray[28].ToString().ToUpper();
                        //    txtGiudice.Text = pratica.Rows[0].ItemArray[6].ToString().ToUpper();
                        //    TxtTipoProvvAg.Text = pratica.Rows[0].ItemArray[7].ToString();
                        //    TxtTipoProvvAg.ToolTip = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
                        //    txtProdPenNr.Text = pratica.Rows[0].ItemArray[8].ToString();
                        //    txtNominativo.Text = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                        //    txtNominativo.ToolTip = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                        //    txtIndirizzo.Text = pratica.Rows[0].ItemArray[10].ToString().ToUpper() + " " + pratica.Rows[0].ItemArray[11].ToString().ToUpper();
                        //    txtIndirizzo.ToolTip = pratica.Rows[0].ItemArray[10].ToString().ToUpper();

                        //    CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0].ItemArray[12]);

                        //    if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[13].ToString()))
                        //    {
                        //        //converte la data 01-01-1900 in SPACE
                        //        DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()); // Recupera la data dal DataTable
                        //        if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                        //        {
                        //            txtDataDataEvasa.Text = ""; // Metti una stringa vuota
                        //        }
                        //        else
                        //        {
                        //            txtDataDataEvasa.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                        //        }
                        //    }
                        //    //  txtDataDataEvasa.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()).ToShortDateString();


                        //    //     txtinviata.Text = pratica.Rows[0].ItemArray[14].ToString().ToUpper();

                        //    //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[15].ToString()))

                        //    //    txtDataInvio.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[15].ToString()).ToShortDateString();

                        //    txtEsito.Text = pratica.Rows[0].ItemArray[16].ToString().ToUpper();
                        //    txtEsito.ToolTip = pratica.Rows[0].ItemArray[16].ToString().ToUpper();
                        //    //if (pratica.Rows[0].ItemArray[17].ToString().ToUpper().StartsWith("-") || pratica.Rows[0].ItemArray[17].ToString().ToUpper().StartsWith("/"))
                        //    //{
                        //    //    txtAccertatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
                        //    //    txtAccertatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
                        //    //}
                        //    //else
                        //    //{
                        //    //    txtAccertatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
                        //    //    txtAccertatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
                        //    //}

                        //    //I- mod 02/02/2026 accertatori in lista
                        //    if (!String.IsNullOrEmpty(pratica.Rows[0]["accertatori"].ToString())) // 17
                        //    {

                        //        ListAccertatori.Items.Add(pratica.Rows[0]["accertatori"].ToString());
                        //    }
                        //    if (!String.IsNullOrEmpty(pratica.Rows[0]["accertatori2"].ToString()))
                        //    {
                        //        ListAccertatori.Items.Add(pratica.Rows[0]["accertatori2"].ToString());
                        //    }
                        //    if (!String.IsNullOrEmpty(pratica.Rows[0]["accertatori3"].ToString()))
                        //    {
                        //        ListAccertatori.Items.Add(pratica.Rows[0]["accertatori3"].ToString());
                        //    }
                        //    //F- mod 02/02/2026 accertatori in lista

                        //    //I- mod 02/06/2026 numero esposti
                        //    if (!String.IsNullOrEmpty(pratica.Rows[0]["NumProtRicStessoCarico"].ToString()))
                        //    {
                        //        txtNumProtRicStessoCarico.Text = pratica.Rows[0]["NumProtRicStessoCarico"].ToString();
                        //    }

                        //    //F- mod 02/06/2026 numero esposti

                        //    if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[18].ToString()))
                        //    {
                        //        //converte la data 01-01-1900 in SPACE
                        //        DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()); // Recupera la data dal DataTable
                        //        if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                        //        {
                        //            txtDataCarico.Text = ""; // Metti una stringa vuota
                        //        }
                        //        else
                        //        {
                        //            txtDataCarico.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                        //        }
                        //    }
                        //    //txtDataCarico.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()).ToShortDateString();

                        //    txtPraticaOut.Text = pratica.Rows[0].ItemArray[19].ToString();
                        //    TxtQuartiere.Text = pratica.Rows[0].ItemArray[20].ToString();
                        //    //txtNote.Text = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                        //    //txtNote.ToolTip = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                        //    txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[22].ToString();
                        //    //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                        //    txtRifProtGen.Text = Regex.Replace(pratica.Rows[0]["Rif_Prot_Gen"].ToString(), @"[^0-9/]", ";");  //pratica.Rows[0].ItemArray[24].ToString();
                        //    if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[27].ToString()))
                        //    {
                        //        txtAreaCompetenza.Text = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
                        //        txtAreaCompetenza.ToolTip = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
                        //    }
                        //    //I 23/04/2026 controllo deleghe
                        //    if (!String.IsNullOrEmpty(pratica.Rows[0]["DataDelega"].ToString()))
                        //        txtDataDelega.Text = System.Convert.ToDateTime(pratica.Rows[0]["DataDelega"].ToString()).ToShortDateString();

                        //    txtGgDelega.Text = pratica.Rows[0]["GgDelega"].ToString();
                        //    //F 23/04/2026 controllo deleghe
                        //    // Salva la lista nella Sessione
                        //    Session["ListRicerca"] = pratica;
                        //    // Puoi anche chiudere il popup se necessario
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#ModalRicerca').modal('hide');", true);
                        //    DivDettagli.Visible = true;
                        //    DivRicerca.Visible = false;


                        //    DataTable decretazione = new DataTable();

                        //    decretazione = mn.getListDecretazione(pratica.Rows[0].ItemArray[1].ToString(), pratica.Rows[0].ItemArray[0].ToString());
                        //    if (decretazione.Rows.Count > 0)
                        //    {
                        //        GVDecretazione.DataSource = decretazione;
                        //        GVDecretazione.DataBind();
                        //        divDecretazione.Visible = true;
                        //    }
                        //    else
                        //        divDecretazione.Visible = false;

                        //}

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
        protected void FillScheda(DataTable pratica, Manager mn)
        {
            if (pratica.Rows.Count > 0)
            {
                Pulisci();
                txtProt.Text = pratica.Rows[0].ItemArray[1].ToString() + " - " + pratica.Rows[0].ItemArray[2].ToString();
                // txtSigla.Text = pratica.Rows[0].ItemArray[2].ToString();
                if (pratica.Rows[0].ItemArray[2].ToString() == Enumerate.Sigla.AG.ToString().ToUpper())
                {
                    divAg.Visible = true;
                }
                else
                {
                    divAg.Visible = false;

                }
                if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[3].ToString()))

                    txtDataInsCarico.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[3].ToString()).ToShortDateString();


                txtProvenienza.Text = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                txtProvenienza.ToolTip = pratica.Rows[0].ItemArray[4].ToString().ToUpper();
                txtTipoAtto.Text = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                txtTipoAtto.ToolTip = pratica.Rows[0].ItemArray[5].ToString().ToUpper();
                txtUltTipoAtto.Text = pratica.Rows[0].ItemArray[28].ToString().ToUpper();
                txtUltTipoAtto.ToolTip = pratica.Rows[0].ItemArray[28].ToString().ToUpper();
                txtGiudice.Text = pratica.Rows[0].ItemArray[6].ToString().ToUpper();
                TxtTipoProvvAg.Text = pratica.Rows[0].ItemArray[7].ToString();
                TxtTipoProvvAg.ToolTip = pratica.Rows[0].ItemArray[7].ToString().ToUpper();
                txtProdPenNr.Text = pratica.Rows[0].ItemArray[8].ToString();
                txtNominativo.Text = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                txtNominativo.ToolTip = pratica.Rows[0].ItemArray[9].ToString().ToUpper();
                txtIndirizzo.Text = pratica.Rows[0].ItemArray[10].ToString().ToUpper() + " " + pratica.Rows[0].ItemArray[11].ToString().ToUpper();
                txtIndirizzo.ToolTip = pratica.Rows[0].ItemArray[10].ToString().ToUpper();

                CkEvasa.Checked = System.Convert.ToBoolean(pratica.Rows[0].ItemArray[12]);

                if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[13].ToString()))
                {
                    //converte la data 01-01-1900 in SPACE
                    DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()); // Recupera la data dal DataTable
                    if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                    {
                        txtDataDataEvasa.Text = ""; // Metti una stringa vuota
                    }
                    else
                    {
                        txtDataDataEvasa.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                    }
                }
                //  txtDataDataEvasa.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[13].ToString()).ToShortDateString();


                //     txtinviata.Text = pratica.Rows[0].ItemArray[14].ToString().ToUpper();

                //if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[15].ToString()))

                //    txtDataInvio.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[15].ToString()).ToShortDateString();

                txtEsito.Text = pratica.Rows[0].ItemArray[16].ToString().ToUpper();
                txtEsito.ToolTip = pratica.Rows[0].ItemArray[16].ToString().ToUpper();
                //if (pratica.Rows[0].ItemArray[17].ToString().ToUpper().StartsWith("-") || pratica.Rows[0].ItemArray[17].ToString().ToUpper().StartsWith("/"))
                //{
                //    txtAccertatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
                //    txtAccertatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper().Substring(1);
                //}
                //else
                //{
                //    txtAccertatori.Text = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
                //    txtAccertatori.ToolTip = pratica.Rows[0].ItemArray[17].ToString().ToUpper();
                //}

                //I- mod 02/02/2026 accertatori in lista
                if (!String.IsNullOrEmpty(pratica.Rows[0]["accertatori"].ToString())) // 17
                {

                    ListAccertatori.Items.Add(pratica.Rows[0]["accertatori"].ToString());
                }
                if (!String.IsNullOrEmpty(pratica.Rows[0]["accertatori2"].ToString()))
                {
                    ListAccertatori.Items.Add(pratica.Rows[0]["accertatori2"].ToString());
                }
                if (!String.IsNullOrEmpty(pratica.Rows[0]["accertatori3"].ToString()))
                {
                    ListAccertatori.Items.Add(pratica.Rows[0]["accertatori3"].ToString());
                }
                //F- mod 02/02/2026 accertatori in lista

                //I- mod 02/06/2026 numero esposti
                if (!String.IsNullOrEmpty(pratica.Rows[0]["NumProtRicStessoCarico"].ToString()))
                {
                    txtNumProtRicStessoCarico.Text = pratica.Rows[0]["NumProtRicStessoCarico"].ToString();
                }

                //F- mod 02/06/2026 numero esposti

                if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[18].ToString()))
                {
                    //converte la data 01-01-1900 in SPACE
                    DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()); // Recupera la data dal DataTable
                    if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                    {
                        txtDataCarico.Text = ""; // Metti una stringa vuota
                    }
                    else
                    {
                        txtDataCarico.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                    }
                }
                //txtDataCarico.Text = System.Convert.ToDateTime(pratica.Rows[0].ItemArray[18].ToString()).ToShortDateString();

                txtPraticaOut.Text = pratica.Rows[0].ItemArray[19].ToString();
                TxtQuartiere.Text = pratica.Rows[0].ItemArray[20].ToString();
                //txtNote.Text = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                //txtNote.ToolTip = pratica.Rows[0].ItemArray[21].ToString().ToUpper();
                txtAnnoRicerca.Text = pratica.Rows[0].ItemArray[22].ToString();
                //lblGiorno.Text = pratica.Rows[0].ItemArray[21].ToString();
                txtRifProtGen.Text = Regex.Replace(pratica.Rows[0]["Rif_Prot_Gen"].ToString(), @"[^0-9/]", ";");  //pratica.Rows[0].ItemArray[24].ToString();
                if (!String.IsNullOrEmpty(pratica.Rows[0].ItemArray[27].ToString()))
                {
                    txtAreaCompetenza.Text = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
                    txtAreaCompetenza.ToolTip = pratica.Rows[0].ItemArray[27].ToString().ToUpper();
                }
                //I 23/04/2026 controllo deleghe
                if (!String.IsNullOrEmpty(pratica.Rows[0]["DataDelega"].ToString()))
                {
                    //converte la data 01-01-1900 in SPACE
                    DateTime dataappo = System.Convert.ToDateTime(pratica.Rows[0]["DataDelega"].ToString()); // Recupera la data dal DataTable
                    if (dataappo == new DateTime(1900, 1, 1) || dataappo == new DateTime(1, 1, 1))
                    {
                        txtDataDelega.Text = ""; // Metti una stringa vuota
                    }
                    else
                    {
                        txtDataDelega.Text = dataappo.ToShortDateString(); // Formatta la data come preferisci
                    }
                    // txtDataDelega.Text = System.Convert.ToDateTime(pratica.Rows[0]["DataDelega"].ToString()).ToShortDateString();
                }
                txtGgDelega.Text = pratica.Rows[0]["GgDelega"].ToString();
                //F 23/04/2026 controllo deleghe
                // Salva la lista nella Sessione
                Session["ListRicerca"] = pratica;
                // Puoi anche chiudere il popup se necessario
                ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#ModalRicerca').modal('hide');", true);
                DivDettagli.Visible = true;
                DivRicerca.Visible = false;


                DataTable decretazione = new DataTable();

                decretazione = mn.getListDecretazione(pratica.Rows[0].ItemArray[1].ToString(), pratica.Rows[0].ItemArray[0].ToString());
                if (decretazione.Rows.Count > 0)
                {
                    GVDecretazione.DataSource = decretazione;
                    GVDecretazione.DataBind();
                    divDecretazione.Visible = true;
                }
                else
                    divDecretazione.Visible = false;

            }


        }
        private void Pulisci()
        {
            txtAnnoRicerca.Text = String.Empty;
            txtNProtocollo.Text = String.Empty;
            txtProcPenale.Text = String.Empty;
            //date per div evasa
            txtDataDa.Text = String.Empty;
            txtDataA.Text = String.Empty;
            txtRicAnnoVal.Text = string.Empty;
            txtRicPraticaVal.Text = string.Empty;
            txtProtGen.Text = String.Empty;
            txtPratica.Text = String.Empty;
            txtRicProvenienza.Text = String.Empty;
            txtRicNominativo.Text = String.Empty;
            txtRicAccertatori.Text = String.Empty;
            txtRicIndirizzo.Text = String.Empty;
            txtDatArrivoA.Text = String.Empty;
            txtDatArrivoDa.Text = String.Empty;
            txtRicGiudice.Text = String.Empty;
            txtDataCarico.Text = String.Empty;
            txtDataDataEvasa.Text = String.Empty;
            txtDataInsCarico.Text = String.Empty;
            txtTipoAtto.Text = String.Empty;
            txtProvenienza.Text = String.Empty;
            txtIndirizzo.Text = String.Empty;
            TxtQuartiere.Text = String.Empty;
            txtNominativo.Text = String.Empty;
            txtPratica.Text = String.Empty;
            txtAreaCompetenza.Text = String.Empty;
            txtEsito.Text = String.Empty;
            //I 23/04/2026 controllo deleghe
            txtDataDelega.Text = string.Empty;
            txtGgDelega.Text = string.Empty;
            //F 23/04/2026 controllo deleghe
            //  txtAccertatori.Text = String.Empty;
            List<string> accertatoriList = new List<string>();
            CkEvasa.Checked = false;
            txtNote.Text = String.Empty;
            HfFiltroNota.Value = string.Empty;
            HfFiltroIndirizzo.Value = string.Empty;
            HfFiltroAccertatori.Value = string.Empty;
            HfFiltroSigla.Value = string.Empty;
            HfFiltroNominativo.Value = string.Empty;


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
            if (gvPopup.TopPagerRow != null)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)gvPopup.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = gvPopup.PageIndex + 1;
                    int totalPages = gvPopup.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }

        }

        protected void btNProtocollo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProtocollo.Visible = true;
        }

        protected void btProcPenale_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProcPenale.Visible = true;
        }

        protected void btEvaseAg_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivEvasaAg.Visible = true;
        }

        protected void btProtGen_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProtGen.Visible = true;
        }

        protected void btNpratica_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivPratica.Visible = true;
        }
        protected void btValidaPratica_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivValidazione.Visible = true;
        }
        protected void btGiudice_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivGiudice.Visible = true;
        }

        protected void btProvenienza_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivProvenienza.Visible = true;
        }

        protected void btNominativo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivNominativo.Visible = true;
        }

        protected void btAccertatori_Click(object sender, EventArgs e)
        {

            NascondiDiv();
            Pulisci();
            DivAccertatori.Visible = true;

        }

        protected void btIndirizzo_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivIndirizzo.Visible = true;
        }
        protected void btDataCarico_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivDataArrivo.Visible = true;
        }
        protected void btNote_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            Pulisci();
            DivNote.Visible = true;
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
            DivDataArrivo.Visible = false;
            DivDettagli.Visible = false;
            DivValidazione.Visible = false;
            DivGridVal.Visible = false;
            DivNote.Visible = false;
            Session.Remove("ListPratiche");
            Session.Remove("ListRicerca");
        }
        protected void gvPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPopup.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            if (String.IsNullOrEmpty(HfFiltroAccertatori.Value) && String.IsNullOrEmpty(HfFiltroSigla.Value) && String.IsNullOrEmpty(HfFiltroNominativo.Value))
            {
                Ricerca_Click(sender, e);
            }
            else
            {
                if (!String.IsNullOrEmpty(HfFiltroAccertatori.Value))
                {
                    PopulateGridView("accertatori", HfFiltroAccertatori.Value);
                    // apripopup_Click(sender, e);
                }
                else
                {
                    if (!String.IsNullOrEmpty(HfFiltroSigla.Value))
                    {
                        PopulateGridView("Sigla", HfFiltroSigla.Value);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);

                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(HfFiltroNominativo.Value))
                        {
                            PopulateGridView("Nominativo", HfFiltroNominativo.Value);
                            //  apripopup_Click(sender, e);
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);

            }
            //  Ricerca_Click(sender, e);
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
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterNominativo")
            {
                columnName = "Nominativo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.
            PopulateGridView(columnName, columnName1, columnName2, HfFiltroNominativo.Value);
            //PopulateGridView(columnName, HfFiltroNominativo.Value); // Esempio di funzione di filtro
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
            string columnName = ""; //campo db
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterAccertatori")
            {
                columnName = "Accertatori"; // Assumi che "arch_note" sia il campo del tuo DataSource
                columnName1 = "Accertatori2";
                columnName2 = "Accertatori3";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, columnName1, columnName2, HfFiltroAccertatori.Value); // Esempio di funzione di filtro
                                                                                               //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
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
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterIndirizzo")
            {
                columnName = "indirizzo"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.
            PopulateGridView(columnName, columnName1, columnName2, HfFiltroIndirizzo.Value); // Esempio di funzione di filtro
                                                                                             //PopulateGridView(columnName, HfFiltroIndirizzo.Value); // Esempio di funzione di filtro
                                                                                             //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);

        }
        // Funzione  che carica i dati e applica il filtro
        private void PopulateGridView(string filterColumn = "", string filterColumn1 = "", string filterColumn2 = "", string filterValue = "")
        {

            DataTable dt = new DataTable();
            string filterExpression = string.Empty;
            dt = GetOriginalData(); // ricerco la lista nuovamente
            try
            {
                //applico il filtro
                if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
                {


                    if (String.IsNullOrWhiteSpace(filterColumn1) && String.IsNullOrWhiteSpace(filterColumn2))
                    {
                        filterExpression = $"{filterColumn} LIKE ('%{filterValue.Replace("'", "''")}%')";
                    }
                    else if (String.IsNullOrWhiteSpace(filterColumn2))
                    {
                        filterExpression = $"{filterColumn} LIKE ('%{filterValue.Replace("'", "''")}%') or {filterColumn1} LIKE ('%{filterValue.Replace("'", "''")}%')";
                    }
                    else
                    {
                        filterExpression = $"{filterColumn} LIKE ('%{filterValue.Replace("'", "''")}%') or {filterColumn1} LIKE ('%{filterValue.Replace("'", "''")}%') or {filterColumn2} LIKE ('%{filterValue.Replace("'", "''")}%')";
                    }


                    DataRow[] filteredRows = dt.Select(filterExpression);

                    if (filteredRows.Length > 0)
                    {
                        DataTable filteredDt = dt.Clone();
                        foreach (DataRow row in filteredRows)
                        {
                            filteredDt.ImportRow(row);
                        }
                        gvPopup.DataSource = filteredDt;
                    }
                    else
                    {
                        gvPopup.DataSource = null;

                    }

                }
                else
                {
                    gvPopup.DataSource = dt; // Nessun filtro
                }
                gvPopup.DataBind();
            }
            catch (Exception ex)
            {
                if (!File.Exists(LogFile))
                {
                    using (StreamWriter sw = File.CreateText(LogFile)) { }
                }

                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.WriteLine(ex.Message + @" - Errore visualizza ");
                    sw.Close();
                }
                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = paginaChiamante;
                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);
                // Response.Redirect("~/Contact.aspx?errore=" + ex.Message);


                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "E' probabile che l'indirizzo non sia presente in archivio" + "'); $('#errorModal').modal('show');", true);
                // throw;
            }
        }
        //gridview per decretazione
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
            //Decretazione_Click(sender, e);

        }
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

                        filtro = $"Indirizzo LIKE '%{HfFiltroIndirizzo.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;
                        break;
                    case "Accertatori":
                        string valoreCerca = HfFiltroAccertatori.Value.Replace("'", "''");
                        filtro = $"(Accertatori LIKE '%{valoreCerca}%' OR " +
                                 $"Accertatori2 LIKE '%{valoreCerca}%' OR " +
                                $"Accertatori3 LIKE '%{valoreCerca}%')";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;

                        break;
                    case "Sigla":

                        filtro = $"Sigla LIKE '%{HfFiltroSigla.Value}%'";
                        dv = new DataView(pratica);

                        dv.RowFilter = filtro;

                        break;


                }
                if (pratica.Rows.Count > 0)
                {
                    //   apripopupPratica_Click(sender, e);
                    gvPopup.DataSource = dv;
                    gvPopup.DataBind();

                    txtPratica.Enabled = false;
                    // Salva la lista nella Sessione
                    Session["ListRicerca"] = ListRicerca;
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
        //protected string FormatMyDate(object dateValue)
        //{
        //    if (dateValue == null || dateValue == DBNull.Value)
        //    {
        //        return "";
        //    }

        //    DateTime date;
        //    if (DateTime.TryParse(dateValue.ToString(), out date))
        //    {
        //        if (date == new DateTime(1900, 1, 1) || date == new DateTime(1, 1, 1))
        //        {
        //            return ""; // O " " se vuoi uno spazio fisico
        //        }
        //        return date.ToString("dd/MM/yyyy");
        //    }
        //    return ""; // Gestione di valori non validi
        //}

        protected void txtFilterSigla_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            //// Crea una lista
            //List<string> ListRicerca = new List<string> { "Sigla", txtRicNominativo.Text };
            //// Salva la lista nella Sessione
            //Session["ListRicerca"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim().ToUpper();
            HfFiltroSigla.Value = filterValue;
            List<string> ListRicerca = new List<string> { "Sigla", filterValue };
            Session["ListRicerca"] = ListRicerca;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; // Devi decidere su quale campo del DB filtrare
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterSigla")
            {
                columnName = "Sigla"; // Assumi che "arch_note" sia il campo del tuo DataSource
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.
            PopulateGridView(columnName, columnName1, columnName2, HfFiltroSigla.Value);
            //PopulateGridView(columnName,"","", HfFiltroSigla.Value); // Esempio di funzione di filtro
            //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }

        protected void btModifica_Click(object sender, EventArgs e)
        {
            string url = VirtualPathUtility.ToAbsolute("~/View/Modifica.aspx");
            Response.Redirect(url, false);
        }

        //protected void btDuplica_Click(object sender, EventArgs e)
        //{
        //    btOKDup.Enabled = true;
        //    TextMessage.InnerText = "Sei sicuro di voler duplicare il carico corrente?";
        //    //TextMessage.InnerHtml = "style=""";
        //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#TextMessage').text('" + ".." + "'); $('#MsgModal').modal('show');", true);

        //}

        //protected void btOKDup_Click(object sender, EventArgs e)
        //{
        //    Manager mn = new Manager();
        //    String carico = txtProt.Text.Split('-')[0].Trim();
        //    String sigla = txtProt.Text.Split('-')[1].Trim();

        //    Boolean resp = mn.DuplicaCarico(carico, sigla, Convert.ToInt32(HidPratica.Value));
        //    if (resp)
        //    {
        //        TextMessage.InnerText = "Carico " + carico + " duplicato";
        //        //TextMessage.InnerHtml = "style=""";
        //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#TextMessage').text('" + ".." + "'); $('#MsgModal').modal('show');", true);
        //        btOKDup.Enabled = false;

        //    }
        //    else
        //    {
        //        TextMessage.InnerText = "Errore " + carico + " duplicato";
        //        //TextMessage.InnerHtml = "style=""";
        //        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#TextMessage').text('" + ".." + "'); $('#MsgModal').modal('show');", true);
        //    }
        //    HidPratica.Value = string.Empty;
        //    Pulisci();
        //}

        //protected void btChiudiMsgModal_Click(object sender, EventArgs e)
        //{
        //    Session.Remove("ListPratiche");
        //    Session.Remove("ListRicerca");
        //    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
        //    Response.Redirect(url, false);
        //}

        protected void btBack_Click(object sender, EventArgs e)
        {
            gvPopup.DataSource = Session["ListPratiche"];
            gvPopup.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }


        protected void GVPratica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Verifichiamo che la riga sia una riga di dati 
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Recuperiamo il valore del campo "evasa" dal data item

                bool isEvasa = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "evasa"));

                if (!isEvasa)
                {
                    // Opzione A: Colore diretto tramite codice
                    e.Row.BackColor = System.Drawing.Color.LightCoral;

                    // Opzione B (Consigliata): Aggiungi una classe CSS per avere più controllo
                    //  e.Row.CssClass += " riga-non-evasa";
                }
            }
        }

        protected void GVPratica_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GVPratica_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btValidazione_Click(object sender, EventArgs e)
        {

        }

        protected void btDecreta_Click(object sender, EventArgs e)
        {
            Session["decr"] = "true";//segnalo che provengo da visualizza
            string url = VirtualPathUtility.ToAbsolute("~/View/Modifica.aspx");
            Response.Redirect(url, false);
        }

        protected void gvPopup_DataBound(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            if (gv.PageCount > 0)
            {
                lblInfoPagine.Text = $"Pagina {gv.PageIndex + 1} di {gv.PageCount}";
            }
            else
            {
                lblInfoPagine.Text = "Nessun record trovato";
            }
        }

        //protected void lnkPratica_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    string argument = btn.CommandArgument;

        //    if (!string.IsNullOrEmpty(argument))
        //    {
        //        // Divido la stringa in base al separatore '|'
        //        string[] parts = argument.Split('|');
        //        string idScheda = parts[0];
        //        string nrPratica = parts[1];

        //        // Costruisco l'URL e reindirizzo
        //        string url = $"~/View/GestionePratica.aspx?idscheda={idScheda}&nrPratica={nrPratica}";

        //        // Se hai usato OnClientClick per il _blank, questo redirect avverrà nella nuova tab
        //        Response.Redirect(ResolveUrl(url));
        //    }
        //}

        protected void BtDuplica_Click1(object sender, EventArgs e)
        {
            string url = VirtualPathUtility.ToAbsolute("~/View/Inserimento.aspx?dup=y");
            Response.Redirect(url, false);
        }

        protected void txtFilterNota_TextChanged(object sender, EventArgs e)
        {
            TextBox txtFilter = (TextBox)sender;
            // Crea una lista
            List<string> ListRicerca = new List<string> { "Nota", txtNote.Text };

            // Salva la lista nella Sessione
            Session["ListRicerca"] = ListRicerca;
            string filterValue = txtFilter.Text.Trim();
            HfFiltroNota.Value = filterValue;
            // Trova l'ID della TextBox che ha scatenato l'evento per sapere quale colonna filtrare
            string columnName = ""; //campo db
            string columnName1 = "";
            string columnName2 = "";
            if (txtFilter.ID == "txtFilterNota")
            {
                columnName = "Note"; // 
                columnName1 = "decr_nota";
            }
            // Puoi aggiungere altri if/else per altre TextBox di filtro

            // Ora puoi usare 'filterValue' e 'columnName' per rifiltrare i tuoi dati
            // e ribindare la GridView, in modo simile a quanto mostrato nella precedente risposta programmatica.

            PopulateGridView(columnName, columnName1, columnName2, HfFiltroNota.Value); // Esempio di funzione di filtro
                                                                                        //            apripopup_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalRicerca').modal('show');", true);
        }
    }
}