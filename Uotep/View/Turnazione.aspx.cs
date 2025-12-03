using AjaxControlToolkit.HtmlEditor.Popups;
using Microsoft.Ajax.Utilities;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uote;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;
using DataTable = System.Data.DataTable;
using Table = System.Web.UI.WebControls.Table;



namespace Uotep
{
    public partial class Turnazione : System.Web.UI.Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String ruolo = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        // Dizionario per annotare DOVE inserire le intestazioni.
        // La chiave (int) è l'indice della riga, il valore (string) è il nome dell'ufficio.
        private Dictionary<int, string> _headerRowsToInsert = new Dictionary<int, string>();
        private string _currentUfficio = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            Session["PaginaChiamante"] = "~/View/Turnazione.aspx";

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                // Hfuser.Value = Session["ruolo"].ToString();
                ruolo = Session["ruolo"].ToString();

            }
            else
            {
                string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true");
                Response.Redirect(url);

            }
            if (!IsPostBack)
            {
                PopolaDropdowns();
                // Carica la griglia con i valori di default (mese corrente)
                CaricaGriglia();
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                ProtocolloLiteral.Text = decodedText;
                txtAnno.Text = System.Convert.ToInt32(DateTime.Now.Year).ToString();
            }
            //if (!IsPostBack)
            //{

            //    // Legge il valore dal Web.config
            //    string protocolloText = ConfigurationManager.AppSettings["Titolo"];

            //    // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
            //    string decodedText = HttpUtility.HtmlDecode(protocolloText);

            //    // Assegna il valore decodificato al Literal
            //    ProtocolloLiteral.Text = decodedText;
            //    CaricaDLL();
            //    txtMese.Text = DateTime.Now.ToString("MMMM").ToUpper();
            //    txtAnno.Text = DateTime.Now.ToString("yyyy");
            //    //TxtData.Text = DateTime.Now.ToShortDateString();// ToString("yyyy");
            //    if (ruolo == Enumerate.Ruolo.accertatori.ToString())
            //    {
            //        btCerca.Visible = false;
            //        btStampa.Visible = false;
            //    }
            //    else
            //    {
            //        txtAnno.Enabled = true;
            //        txtMese.Enabled = true;
            //      //  divInserimento.Visible = false;
            //        btSalva.Visible = false;
            //    }
            // }

        }
        private void PopolaDropdowns()
        {
            // Popola i mesi
            for (int i = 1; i <= 12; i++)
            {
                ddlMese.Items.Add(new ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i), i.ToString()));
            }
            ddlMese.SelectedValue = DateTime.Now.Month.ToString();

            //// Popola gli anni (es. 5 anni passati e 5 futuri)
            //int annoCorrente = DateTime.Now.Year;
            //for (int i = annoCorrente - 2; i <= annoCorrente + 3; i++)
            //{
            //    txtAnno.Text..Add(new ListItem(i.ToString(), i.ToString()));
            //}
            //txtAnno.Text= annoCorrente.ToString();
        }
        protected void btChiudiAvvertenze_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalAvvertenze')); modal.hide();", true);

        }
        public float ConvertiStringaInFloat(string inputStringa)
        {
            float valoreConvertito = 0f;
            CultureInfo culturaItaliana = new CultureInfo("it-IT");
            NumberStyles stile = NumberStyles.Currency;

            if (float.TryParse(
                    inputStringa,
                    stile,
                    culturaItaliana,
                    out valoreConvertito))
            {
                return valoreConvertito;
            }
            else
            {
                throw new FormatException($"La stringa '{inputStringa}' non è un formato numerico valido per float.");
            }
        }
        protected void Salva_Click(object sender, EventArgs e)
        {
            try
            {
                //GestAuto auto = new GestAuto();
                //auto.sigla = DdlSigla.SelectedItem.Text;
                //auto.targa = txtTarga.Text;
                //auto.data = System.Convert.ToDateTime(TxtData.Text);
                //auto.ora = System.Convert.ToDateTime(txtOra.Text);
                //auto.stan = txtStan.Text;
                //auto.litri = ConvertiStringaInFloat(txtLitri.Text);
                //auto.tipoCarburante = DdlCarburante.SelectedItem.Text;
                //auto.euro = ConvertiStringaInFloat(txtEuro.Text);
                //auto.indirizzo = txtIndirizzo.Text.ToUpper();
                //auto.autista = txtAutista.Text.ToUpper();
                //auto.mese = txtMese.Text;
                //auto.anno = System.Convert.ToInt16(txtAnno.Text);
                //auto.verificato = false;
                //if (!string.IsNullOrEmpty(Vuser))
                //{
                //    auto.matricola = Vuser;
                //}
                //else
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Sessione scaduta effettuare login" + "'); $('#errorModal').modal('show');", true);

                //     string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=true");
                //            Response.Redirect(url, false);
                //}

                //Manager mn = new Manager();
                //Boolean ins = mn.InsGestioneAuto(auto);
                //if (!ins)
                //{


                //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Inserimento non riuscito" + "'); $('#errorModal').modal('show');", true);
                //}
                //else
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#Message').text('" + "Sigla " + auto.sigla + " inserita correttamente" + "'); $('#ModalRicDecretazione').modal('show');", true);

                //    Pulisci();

                //}
            }
            catch (Exception ex)
            {

                string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                Response.Redirect(url + ex.Message);

                Session["MessaggioErrore"] = ex.Message;
                Session["PaginaChiamante"] = "~/View/GestioneAuto.aspx";
            }
        }
        private void Pulisci()
        {
            //txtTarga.Text = string.Empty;
            //TxtData.Text = string.Empty;
            //txtOra.Text = string.Empty;
            //txtStan.Text = string.Empty;
            //txtLitri.Text = string.Empty;
            //DdlCarburante.ClearSelection();

            //txtEuro.Text = string.Empty;
            //txtIndirizzo.Text = string.Empty;
            //txtAutista.Text = string.Empty;
            //DdlSigla.ClearSelection();


        }



        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalQuartiere').modal('show');", true);
        }


        protected void btSalvaGiudice_Click(object sender, EventArgs e)
        {
            Salva_Click(sender, e); ;
        }




        protected void apripopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDecretazione').modal('show');", true);

        }
        protected void chiudipopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('ModalDecretazione')); modal.hide();", true);
            // Pulisci();
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


        protected void btStampa_Click(object sender, EventArgs e)
        {
            //Manager mn = new Manager();
            //System.Data.DataTable scheda = mn.getListAuto(txtMese.Text, Convert.ToInt32(txtAnno.Text));

            //Routine stampa = new Routine();
            //stampa.CreaPdfSchedaCarburante(scheda);
            // stampa.CreaPdfLetteraAccompagnamento(scheda, PathLetteraAccompagnamento, "LetteraAccompagnamento.pdf");
        }




        private void CaricaGriglia()
        {
            //try
            //{
            //int mese = int.Parse(ddlMese.SelectedValue);
            //int anno = int.Parse(ddlAnno.SelectedValue);
            //int giorniNelMese = DateTime.DaysInMonth(anno, mese);



            ////    // 1. Pulisce le colonne dinamiche esistenti
            //while (gvCalendario.Columns.Count > 2)
            //{
            //    gvCalendario.Columns.RemoveAt(1);
            //}

            //    // 2. Aggiunge le colonne dei giorni (D1, D2, ...)
            //for (int i = 1; i <= giorniNelMese; i++)
            //{
            //    DateTime dataCorrente = new DateTime(anno, mese, i);
            //    TemplateField giornoField = new TemplateField();

            //    giornoField.HeaderText = $"{dataCorrente.ToString("ddd", CultureInfo.CurrentCulture)}\n{i}";
            //    giornoField.HeaderStyle.CssClass = "text-center";
            //    giornoField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

            //    //        // ***  Segnalazione delle festività/weekend ***
            //    if (IsGiornoFestivo(anno, mese, i))
            //    {
            //        // Applica una classe CSS all'Header e alla cella
            //        giornoField.HeaderStyle.CssClass += " giorno-festivo-header";
            //        giornoField.ItemStyle.CssClass = "giorno-festivo-cella";
            //    }
            //    //        // *******************************************************

            //    //        // Definizione del campo per la modalità di visualizzazione (ItemTemplate)
            //    giornoField.ItemTemplate = new GridViewTemplate(ListItemType.Item, $"D{i}", "Label");

            //    //        // Definizione del campo per la modalità di modifica (EditItemTemplate)
            //    giornoField.EditItemTemplate = new GridViewTemplate(ListItemType.EditItem, $"D{i}", "TextBox");

            //    gvCalendario.Columns.Insert(i, giornoField);
            //}

            ////    // 3. Associa la DataTable
            //DataTable dt = GetDataTable();
            //gvCalendario.DataSource = dt;
            //  gvCalendario.DataBind();

            //}
            //catch (Exception ex)
            //{
            //    lblErrore.Text = $"Errore nel caricamento della griglia: {ex.Message}";
            //}
        }
        protected void ddlMese_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaricaGriglia();
        }

        protected void ddlAnno_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCarica_Click(object sender, EventArgs e)
        {
            gvCalendario.EditIndex = -1;
            GeneraGriglia();


        }

        private Tuple<Dictionary<int, List<string>>, Dictionary<string, List<int>>> ProcessaDataTableGruppi(DataTable dtGruppi, int mese)
        {
            // Dizionario 1: Mappa Giorno -> Lista di Gruppi (per la riga speciale "Quartina")
            var giornoPerGruppo = new Dictionary<int, List<string>>();
            // Dizionario 2: Mappa Gruppo -> Lista di Giorni (per le righe dei dipendenti, per inserire la "Q")
            var gruppoPerGiorno = new Dictionary<string, List<int>>();

            if (dtGruppi == null || dtGruppi.Rows.Count == 0)
            {
                return Tuple.Create(giornoPerGruppo, gruppoPerGiorno);
            }

            string nomeColonnaMese = new DateTime(2000, mese, 1).ToString("MMMM", new CultureInfo("it-IT"));
            DataColumn colonnaMese = dtGruppi.Columns
                .Cast<DataColumn>()
                .FirstOrDefault(c => c.ColumnName.Equals(nomeColonnaMese, StringComparison.OrdinalIgnoreCase));

            if (colonnaMese == null)
            {
                return Tuple.Create(giornoPerGruppo, gruppoPerGiorno);
            }

            foreach (DataRow row in dtGruppi.Rows)
            {
                // ASSICURATI CHE LA COLONNA SI CHIAMI 'quartina' o 'numero gruppo'
                string numeroGruppo = row["quartina"].ToString(); // O "numero gruppo"
                string giorniStringa = row[colonnaMese]?.ToString();

                if (string.IsNullOrWhiteSpace(giorniStringa)) continue;

                string[] giorniSeparati = giorniStringa.Split(',');
                foreach (string giornoStr in giorniSeparati)
                {
                    if (int.TryParse(giornoStr.Trim(), out int giorno))
                    {
                        // Popola il primo dizionario (Giorno -> Gruppi)
                        if (!giornoPerGruppo.ContainsKey(giorno))
                            giornoPerGruppo[giorno] = new List<string>();
                        giornoPerGruppo[giorno].Add(numeroGruppo);

                        // Popola il secondo dizionario (Gruppo -> Giorni)
                        if (!gruppoPerGiorno.ContainsKey(numeroGruppo))
                            gruppoPerGiorno[numeroGruppo] = new List<int>();
                        gruppoPerGiorno[numeroGruppo].Add(giorno);
                    }
                }
            }
            return Tuple.Create(giornoPerGruppo, gruppoPerGiorno);
        }

        //private void GeneraGriglia()
        //{


        //    Manager mn = new Manager();

        //    // Carica la DataTable dei dipendenti una sola volta
        //    DataTable dt = mn.getListDipendentire();

        //    Table gridTable = gvCalendario.Controls.OfType<Table>().FirstOrDefault();
        //    if (gridTable == null)
        //    {
        //        gridTable = new Table();
        //        gvCalendario.Controls.Add(gridTable);
        //    }
        //    gridTable.Rows.Clear();

        //    // --- Logica Anno e Mese ---
        //    int anno = System.Convert.ToInt32(DateTime.Now.Year);
        //    if (!string.IsNullOrEmpty(txtAnno.Text))
        //    {
        //        anno = int.Parse(txtAnno.Text);
        //    }
        //    else
        //        txtAnno.Text = anno.ToString();

        //    int mese = int.Parse(ddlMese.SelectedValue);
        //    int giorniNelMese = DateTime.DaysInMonth(anno, mese);
        //    //*
        //    // --- 2. PREPARAZIONE DATI QUARTINA E FESTIVI ---
        //    DataTable quartina = mn.getListQuartina(anno);
        //    var datiProcessati = ProcessaDataTableGruppi(quartina, mese);
        //    Dictionary<int, List<string>> datiGiorniGruppi = datiProcessati.Item1;
        //    Dictionary<string, List<int>> datiGruppoPerGiorno = datiProcessati.Item2;
        //    HashSet<int> giorniFestivi = CalcolaGiorniFestiviDelMese(anno, mese);


        //    // --- 3. COSTRUZIONE RIGA SPECIALE "QUARTINA" ---
        //    GridViewRow specialRow = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
        //    specialRow.CssClass = "riga-eventi-speciale";
        //    TableCell quartinaCell = new TableCell { Text = "Quartina", Font = { Bold = true } };
        //    specialRow.Cells.Add(quartinaCell);

        //    //*


        //    for (int i = 1; i <= giorniNelMese; i++)
        //    {
        //        TableCell cell = new TableCell();
        //        if (datiGiorniGruppi.ContainsKey(i))
        //            cell.Text = string.Join(", ", datiGiorniGruppi[i]);
        //        specialRow.Cells.Add(cell);
        //    }
        //    specialRow.Cells.Add(new TableCell()); // Azioni
        //    gridTable.Rows.Add(specialRow);

        //    // --- 4. COSTRUZIONE HEADER PRINCIPALE ---
        //    GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //    headerRow.Cells.Add(new TableHeaderCell { Text = "Dipendente" });
        //    for (int i = 1; i <= giorniNelMese; i++)
        //    {
        //        DateTime currentDate = new DateTime(anno, mese, i);
        //        TableHeaderCell thGiorno = new TableHeaderCell();
        //        thGiorno.Text = $"<small>{currentDate:ddd}</small><br/>{i}";
        //        if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
        //            thGiorno.CssClass = "giorno-festivo-header text-center";
        //        else
        //            thGiorno.CssClass = "text-center";
        //        headerRow.Cells.Add(thGiorno);
        //    }
        //    headerRow.Cells.Add(new TableHeaderCell { Text = "Azioni" });
        //    gridTable.Rows.Add(headerRow);

        //    // --- 5. COSTRUZIONE RIGHE DATI E SEPARATORI ---
        //    string _currentUfficio = null;
        //    int visualRowIndex = 0;

        //    foreach (DataRow dataRow in dt.Rows)
        //    {
        //        string ufficio = dataRow["ufficio"].ToString().ToUpper();
        //        string idDipendente = dataRow["id_dip"].ToString();

        //        // ** GESTIONE SEPARATORI UFFICIO (RIPRISTINATO) **
        //        if (ufficio != _currentUfficio)
        //        {
        //            GridViewRow separatorRow = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
        //            TableCell cell = new TableCell { Text = ufficio, ColumnSpan = headerRow.Cells.Count, CssClass = "ufficio-separator-row" };
        //            separatorRow.Cells.Add(cell);
        //            separatorRow.Font.Bold = true;
        //            separatorRow.ForeColor = Color.Red;
        //            gridTable.Rows.Add(separatorRow);
        //            _currentUfficio = ufficio;
        //        }

        //        // --- CALCOLO TURNI CON ALGORITMO A STRATI (PER QUESTO DIPENDENTE) ---
        //        string quartinaDelDipendente = dataRow["quartina"]?.ToString();
        //        List<int> giorniDaMarcareConQ = datiGruppoPerGiorno.ContainsKey(quartinaDelDipendente) ? datiGruppoPerGiorno[quartinaDelDipendente] : new List<int>();

        //        var scheduleDelDipendente = new Dictionary<int, string>();
        //        // 1. Strato Base: RF
        //        for (int i = 1; i <= giorniNelMese; i++)
        //        {
        //            DateTime currentDate = new DateTime(anno, mese, i);
        //            if ((currentDate.DayOfWeek == DayOfWeek.Sunday) || giorniFestivi.Contains(i))
        //                scheduleDelDipendente[i] = "RF";
        //        }
        //        // 2. Strato Sovrascrittura: Q
        //        foreach (int giornoQ in giorniDaMarcareConQ)
        //            scheduleDelDipendente[giornoQ] = "Q";
        //        // 3. Strato Riempimento: 1 e 2
        //        foreach (int giornoQ in giorniDaMarcareConQ)
        //        {
        //            if (giornoQ - 1 > 0 && !scheduleDelDipendente.ContainsKey(giornoQ - 1)) scheduleDelDipendente[giornoQ - 1] = "1";
        //            if (giornoQ + 1 <= giorniNelMese && !scheduleDelDipendente.ContainsKey(giornoQ + 1)) scheduleDelDipendente[giornoQ + 1] = "2";
        //        }

        //        // --- COSTRUZIONE RIGA DIPENDENTE ---
        //        GridViewRow employeeRow = new GridViewRow(visualRowIndex, visualRowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);
        //        employeeRow.Cells.Add(new TableCell { Text = dataRow["nominativo"].ToString() });
        //        employeeRow.Font.Bold = true;

        //        for (int i = 1; i <= giorniNelMese; i++)
        //        {
        //            TableCell cellTurno = new TableCell();
        //            DateTime currentDate = new DateTime(anno, mese, i);
        //            if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
        //                cellTurno.CssClass = "giorno-festivo-cella";

        //            string cellText = scheduleDelDipendente.ContainsKey(i) ? scheduleDelDipendente[i] : "";

        //            // Stile e contenuto
        //            if (gvCalendario.EditIndex == visualRowIndex)
        //            {
        //                TextBox txtTurno = new TextBox { ID = $"txtGiorno_{i}", CssClass = "form-control input-sm", Text = cellText };
        //                cellTurno.Controls.Add(txtTurno);
        //            }
        //            else
        //            {
        //                cellTurno.Text = cellText;
        //                if (cellText == "RF") { cellTurno.CssClass += " text-center"; cellTurno.Font.Bold = true; cellTurno.ForeColor = Color.Blue; }
        //                else if (cellText == "Q") { cellTurno.CssClass += " text-center"; cellTurno.Font.Bold = true; cellTurno.BackColor = Color.DarkOrange; }
        //                else if (cellText == "1" || cellText == "2") { cellTurno.CssClass += " text-center"; }
        //            }
        //            employeeRow.Cells.Add(cellTurno);
        //        }

        //        // Cella Azioni
        //        TableCell cellAzioni = new TableCell();
        //        if (gvCalendario.EditIndex == visualRowIndex) { /* ... bottoni Aggiorna/Annulla ... */ }
        //        else { /* ... bottone Modifica ... */ }
        //        employeeRow.Cells.Add(cellAzioni);
        //        gridTable.Rows.Add(employeeRow);

        //        visualRowIndex++;
        //    }
        //}


        //ottimo
        //private void GeneraGriglia()
        //{
        //    // --- 1. SETUP INIZIALE E CARICAMENTO DATI ---
        //    Manager mn = new Manager();
        //    DataTable dt = mn.getListDipendentire();

        //    Table gridTable = gvCalendario.Controls.OfType<Table>().FirstOrDefault();
        //    if (gridTable == null)
        //    {
        //        gridTable = new Table();
        //        gvCalendario.Controls.Add(gridTable);
        //    }
        //    gridTable.Rows.Clear();

        //    int anno = System.Convert.ToInt32(DateTime.Now.Year);
        //    if (!string.IsNullOrEmpty(txtAnno.Text))
        //    {
        //        anno = int.Parse(txtAnno.Text);
        //    }
        //    else
        //        txtAnno.Text = anno.ToString();

        //    int mese = int.Parse(ddlMese.SelectedValue);
        //    int giorniNelMese = DateTime.DaysInMonth(anno, mese);

        //    // --- 2. PREPARAZIONE DATI QUARTINA E FESTIVI ---
        //    DataTable quartina = mn.getListQuartina(anno);
        //    var datiProcessati = ProcessaDataTableGruppi(quartina, mese);
        //    Dictionary<int, List<string>> datiGiorniGruppi = datiProcessati.Item1;
        //    Dictionary<string, List<int>> datiGruppoPerGiorno = datiProcessati.Item2;
        //    HashSet<int> giorniFestivi = CalcolaGiorniFestiviDelMese(anno, mese);

        //    // --- 3. COSTRUZIONE RIGA SPECIALE "QUARTINA" ---
        //    GridViewRow specialRow = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
        //    specialRow.CssClass = "riga-eventi-speciale";
        //    TableCell quartinaCell = new TableCell { Text = "Quartina", Font = { Bold = true } };
        //    specialRow.Cells.Add(quartinaCell);

        //    for (int i = 1; i <= giorniNelMese; i++)
        //    {
        //        TableCell cell = new TableCell();
        //        if (datiGiorniGruppi.ContainsKey(i))
        //            cell.Text = string.Join(", ", datiGiorniGruppi[i]);
        //        specialRow.Cells.Add(cell);
        //    }
        //    specialRow.Cells.Add(new TableCell()); // Azioni
        //    gridTable.Rows.Add(specialRow);

        //    // --- 4. COSTRUZIONE HEADER PRINCIPALE (con logica Sabato modificata) ---
        //    GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //    headerRow.Cells.Add(new TableHeaderCell { Text = "Dipendente" });
        //    for (int i = 1; i <= giorniNelMese; i++)
        //    {
        //        DateTime currentDate = new DateTime(anno, mese, i);
        //        TableHeaderCell thGiorno = new TableHeaderCell();
        //        thGiorno.Text = $"<small>{currentDate:ddd}</small><br/>{i}";

        //        // MODIFICATO: Lo stile festivo si applica solo a Domeniche e festività nazionali
        //        if (currentDate.DayOfWeek == DayOfWeek.Sunday || giorniFestivi.Contains(i))
        //            thGiorno.CssClass = "giorno-festivo-header text-center";
        //        else
        //            thGiorno.CssClass = "text-center";
        //        headerRow.Cells.Add(thGiorno);
        //    }
        //    headerRow.Cells.Add(new TableHeaderCell { Text = "Azioni" });
        //    gridTable.Rows.Add(headerRow);

        //    // --- 5. COSTRUZIONE RIGHE DATI E SEPARATORI ---
        //    string _currentUfficio = null;
        //    int visualRowIndex = 0;

        //    foreach (DataRow dataRow in dt.Rows)
        //    {
        //        string ufficio = dataRow["ufficio"].ToString().ToUpper();
        //        string idDipendente = dataRow["id_dip"].ToString();

        //        // ** GESTIONE SEPARATORI UFFICIO **
        //        if (ufficio != _currentUfficio)
        //        {
        //            GridViewRow separatorRow = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
        //            TableCell cell = new TableCell { Text = ufficio, ColumnSpan = headerRow.Cells.Count, CssClass = "ufficio-separator-row" };
        //            separatorRow.Cells.Add(cell);
        //            separatorRow.Font.Bold = true;
        //            separatorRow.ForeColor = Color.Red;
        //            gridTable.Rows.Add(separatorRow);
        //            _currentUfficio = ufficio;
        //        }

        //        // --- CALCOLO TURNI CON ALGORITMO A STRATI (PER QUESTO DIPENDENTE) ---
        //        string quartinaDelDipendente = dataRow["quartina"]?.ToString();
        //        List<int> giorniDaMarcareConQ = datiGruppoPerGiorno.ContainsKey(quartinaDelDipendente) ? datiGruppoPerGiorno[quartinaDelDipendente] : new List<int>();
        //        var scheduleDelDipendente = new Dictionary<int, string>();

        //        // 1. STRATO BASE: Riempi tutti i giorni con un'alternanza di 1 e 2
        //        bool assegnaUno = true;
        //        for (int i = 1; i <= giorniNelMese; i++)
        //        {
        //            scheduleDelDipendente[i] = assegnaUno ? "1" : "2";
        //            assegnaUno = !assegnaUno;
        //        }

        //        // 2. STRATO SOVRASCRITTURA RF: Piazza RF sopra 1 e 2 (MODIFICATO: Sabato escluso)
        //        for (int i = 1; i <= giorniNelMese; i++)
        //        {
        //            DateTime currentDate = new DateTime(anno, mese, i);
        //            if (currentDate.DayOfWeek == DayOfWeek.Sunday || giorniFestivi.Contains(i))
        //            {
        //                scheduleDelDipendente[i] = "RF";
        //            }
        //        }

        //        // 3. STRATO SOVRASCRITTURA FINALE Q (ha la massima priorità)
        //        foreach (int giornoQ in giorniDaMarcareConQ)
        //        {
        //            scheduleDelDipendente[giornoQ] = "Q";
        //            DateTime dataQ = new DateTime(anno, mese, giornoQ);
        //            if (dataQ.DayOfWeek == DayOfWeek.Sunday)
        //            {
        //                if (giornoQ - 1 > 0) scheduleDelDipendente[giornoQ - 1] = "1";
        //                if (giornoQ + 1 <= giorniNelMese) scheduleDelDipendente[giornoQ + 1] = "2";
        //            }
        //        }

        //        // --- COSTRUZIONE RIGA DIPENDENTE ---
        //        bool isEditMode = (gvCalendario.EditIndex == visualRowIndex);
        //        GridViewRow employeeRow = new GridViewRow(visualRowIndex, visualRowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);
        //        employeeRow.Cells.Add(new TableCell { Text = dataRow["nominativo"].ToString() });
        //        employeeRow.Font.Bold = true;

        //        for (int i = 1; i <= giorniNelMese; i++)
        //        {
        //            TableCell cellTurno = new TableCell();
        //            DateTime currentDate = new DateTime(anno, mese, i);

        //            // MODIFICATO: Lo sfondo festivo si applica solo a Domeniche e festività nazionali
        //            if (currentDate.DayOfWeek == DayOfWeek.Sunday || giorniFestivi.Contains(i))
        //                cellTurno.CssClass = "giorno-festivo-cella";

        //            string cellText = scheduleDelDipendente.ContainsKey(i) ? scheduleDelDipendente[i] : "";

        //            // Stile e contenuto (formattazione conservata)
        //            if (isEditMode)
        //            {
        //                TextBox txtTurno = new TextBox { ID = $"txtGiorno_{i}", CssClass = "form-control input-sm", Text = cellText };
        //                cellTurno.Controls.Add(txtTurno);
        //            }
        //            else
        //            {
        //                cellTurno.Text = cellText;
        //                if (cellText == "RF") { cellTurno.CssClass += " text-center"; cellTurno.Font.Bold = true; cellTurno.ForeColor = Color.Blue; }
        //                else if (cellText == "Q") { cellTurno.CssClass += " text-center"; cellTurno.Font.Bold = true; cellTurno.BackColor = Color.DarkOrange; }
        //                else if (cellText == "1" || cellText == "2") { cellTurno.CssClass += " text-center"; }
        //            }
        //            employeeRow.Cells.Add(cellTurno);
        //        }

        //        // Cella Azioni (logica bottoni completa)
        //        TableCell cellAzioni = new TableCell();
        //        if (isEditMode)
        //        {
        //            cellAzioni.Controls.Add(new Button { Text = "Aggiorna", CommandName = "Update", CommandArgument = idDipendente, CssClass = "btn btn-success btn-xs" });
        //            cellAzioni.Controls.Add(new LiteralControl(" "));
        //            cellAzioni.Controls.Add(new Button { Text = "Annulla", CommandName = "Cancel", CssClass = "btn btn-default btn-xs" });
        //        }
        //        else
        //        {
        //            cellAzioni.Controls.Add(new Button { Text = "Modifica", CommandName = "Edit", CommandArgument = visualRowIndex.ToString(), CssClass = "btn btn-primary btn-xs" });
        //        }
        //        employeeRow.Cells.Add(cellAzioni);
        //        gridTable.Rows.Add(employeeRow);

        //        visualRowIndex++;
        //    }
        //}


        private void GeneraGriglia()
        {
            // --- 1. SETUP INIZIALE E CARICAMENTO DATI ---
            Manager mn = new Manager();
            DataTable dt = mn.getListDipendentire();

            Table gridTable = gvCalendario.Controls.OfType<Table>().FirstOrDefault();
            if (gridTable == null)
            {
                gridTable = new Table();
                gvCalendario.Controls.Add(gridTable);
            }
            gridTable.Rows.Clear();

            int anno = System.Convert.ToInt32(DateTime.Now.Year);
            if (!string.IsNullOrEmpty(txtAnno.Text))
            {
                anno = int.Parse(txtAnno.Text);
            }
            else
                txtAnno.Text = anno.ToString();

            int mese = int.Parse(ddlMese.SelectedValue);
            int giorniNelMese = DateTime.DaysInMonth(anno, mese);

            // --- 2. PREPARAZIONE DATI QUARTINA E FESTIVI ---
            DataTable quartina = mn.getListQuartina(anno);
            var datiProcessati = ProcessaDataTableGruppi(quartina, mese);
            Dictionary<int, List<string>> datiGiorniGruppi = datiProcessati.Item1;
            Dictionary<string, List<int>> datiGruppoPerGiorno = datiProcessati.Item2;
            HashSet<int> giorniFestivi = CalcolaGiorniFestiviDelMese(anno, mese);

            // --- 3. ESECUZIONE DELLA LOGICA DI BUSINESS COMPLESSA ---
            var turniCalcolati = CalcolaTurniComplessi(dt, datiGruppoPerGiorno, giorniFestivi, anno, mese);

            // --- 4. COSTRUZIONE RIGA "QUARTINA" E HEADER (Layout Invariato) ---
            GridViewRow specialRow = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
            specialRow.CssClass = "riga-eventi-speciale";
            TableCell quartinaCell = new TableCell { Text = "Quartina", Font = { Bold = true } };
            specialRow.Cells.Add(quartinaCell);
            for (int i = 1; i <= giorniNelMese; i++)
            {
                TableCell cell = new TableCell();
                if (datiGiorniGruppi.ContainsKey(i))
                    cell.Text = string.Join(", ", datiGiorniGruppi[i]);
                specialRow.ForeColor = Color.Red;
                specialRow.Font.Bold = true;
                specialRow.BackColor = Color.SkyBlue;
                specialRow.HorizontalAlign = HorizontalAlign.Center;
                specialRow.Cells.Add(cell);
            }
            specialRow.Cells.Add(new TableCell()); // Azioni
            gridTable.Rows.Add(specialRow);

            GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            headerRow.Cells.Add(new TableHeaderCell { Text = "Dipendente" });
            for (int i = 1; i <= giorniNelMese; i++)
            {
                DateTime currentDate = new DateTime(anno, mese, i);
                TableHeaderCell thGiorno = new TableHeaderCell();
                thGiorno.Text = $"<small>{currentDate:ddd}</small><br/>{i}";
                if (currentDate.DayOfWeek == DayOfWeek.Sunday || giorniFestivi.Contains(i))
                    thGiorno.CssClass = "giorno-festivo-header text-center";
                else
                    thGiorno.CssClass = "text-center";
                headerRow.Cells.Add(thGiorno);
            }
            headerRow.Cells.Add(new TableHeaderCell { Text = "Azioni" });
            gridTable.Rows.Add(headerRow);

            // --- 5. CICLO DI RENDERIZZAZIONE (SOLO VISUALIZZAZIONE) ---
            int visualRowIndex = 0;
            var gruppiUfficio = dt.AsEnumerable().GroupBy(r => r.Field<string>("ufficio").ToUpper());

            foreach (var gruppo in gruppiUfficio)
            {
                string nomeUfficio = gruppo.Key;
                List<DataRow> dipendentiDelGruppo = gruppo.ToList();

                // ** GESTIONE SEPARATORI UFFICIO (Layout Invariato) **
                GridViewRow separatorRow = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                TableCell cellUfficio = new TableCell { Text = nomeUfficio, ColumnSpan = headerRow.Cells.Count, CssClass = "ufficio-separator-row" };
                separatorRow.Cells.Add(cellUfficio);
                separatorRow.Font.Bold = true;
                separatorRow.ForeColor = Color.Red;
                gridTable.Rows.Add(separatorRow);

                foreach (DataRow dataRow in dipendentiDelGruppo)
                {
                    string idDipendente = dataRow["id_dip"].ToString();
                    var scheduleDelDipendente = turniCalcolati[idDipendente];

                    bool isEditMode = (gvCalendario.EditIndex == visualRowIndex);
                    GridViewRow employeeRow = new GridViewRow(visualRowIndex, visualRowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);
                    employeeRow.Cells.Add(new TableCell { Text = dataRow["nominativo"].ToString() });
                    employeeRow.Font.Bold = true;

                    for (int i = 1; i <= giorniNelMese; i++)
                    {
                        TableCell cellTurno = new TableCell();
                        DateTime currentDate = new DateTime(anno, mese, i);
                        if (currentDate.DayOfWeek == DayOfWeek.Sunday || giorniFestivi.Contains(i) || currentDate.DayOfWeek == DayOfWeek.Saturday)
                            cellTurno.CssClass = "giorno-festivo-cella";

                        string cellText = scheduleDelDipendente.ContainsKey(i) ? scheduleDelDipendente[i] : "";

                        if (isEditMode)
                        {
                            TextBox txtTurno = new TextBox { ID = $"txtGiorno_{i}", CssClass = "form-control input-sm", Text = cellText };
                            cellTurno.Controls.Add(txtTurno);
                        }
                        else
                        {
                            cellTurno.Text = cellText;
                            if (cellText == "RF") { cellTurno.CssClass += " text-center"; cellTurno.Font.Bold = true; cellTurno.ForeColor = Color.Blue; }
                            else if (cellText == "Q") { cellTurno.CssClass += " text-center"; cellTurno.Font.Bold = true; cellTurno.BackColor = Color.DarkOrange; }
                            else if (cellText == "1" || cellText == "2") { cellTurno.CssClass += " text-center"; }
                        }
                        employeeRow.Cells.Add(cellTurno);
                    }

                    TableCell cellAzioni = new TableCell();
                    if (isEditMode)
                    {
                        cellAzioni.Controls.Add(new Button { Text = "Aggiorna", CommandName = "Update", CommandArgument = idDipendente, CssClass = "btn btn-success btn-xs" });
                        cellAzioni.Controls.Add(new LiteralControl(" "));
                        cellAzioni.Controls.Add(new Button { Text = "Annulla", CommandName = "Cancel", CssClass = "btn btn-default btn-xs" });
                    }
                    else
                    {
                        cellAzioni.Controls.Add(new Button { Text = "Modifica", CommandName = "Edit", CommandArgument = visualRowIndex.ToString(), CssClass = "btn btn-primary btn-xs" });
                    }
                    employeeRow.Cells.Add(cellAzioni);
                    gridTable.Rows.Add(employeeRow);

                    visualRowIndex++;
                }
            }
        }

        /// <summary>
        /// METODO DI CALCOLO: Contiene tutta la logica di business per calcolare i turni di tutti i dipendenti.
        /// </summary>
        private Dictionary<string, Dictionary<int, string>> CalcolaTurniComplessi(DataTable dtDipendenti, Dictionary<string, List<int>> datiGruppoPerGiorno, HashSet<int> giorniFestivi, int anno, int mese)
        {
            var scheduleCompleto = new Dictionary<string, Dictionary<int, string>>();
            var rnd = new Random();
            int giorniNelMese = DateTime.DaysInMonth(anno, mese);

            var gruppiUfficio = dtDipendenti.AsEnumerable().GroupBy(r => r.Field<string>("ufficio").ToUpper());

            foreach (var gruppo in gruppiUfficio)
            {
                string nomeUfficio = gruppo.Key;
                List<DataRow> dipendentiDelGruppo = gruppo.ToList();

                // Inizializzazione per l'ufficio
                foreach (DataRow dip in dipendentiDelGruppo)
                {
                    scheduleCompleto[dip["id_dip"].ToString()] = new Dictionary<int, string>();
                }

                if (dipendentiDelGruppo.Count <= 2)
                {
                    // LOGICA SEMPLICE per uffici piccoli
                    for (int giorno = 1; giorno <= giorniNelMese; giorno++)
                    {
                        var candidati = dipendentiDelGruppo.OrderBy(x => x["id_dip"]).ToList();
                        if (candidati.Count > 0) scheduleCompleto[candidati[0]["id_dip"].ToString()][giorno] = "1";
                        if (candidati.Count > 1) scheduleCompleto[candidati[1]["id_dip"].ToString()][giorno] = "2";
                    }
                }
                else // LOGICA COMPLESSA per uffici grandi
                {
                    // Calcolo sequenza Sabati
                    var turniSabatoUfficio = new Dictionary<int, string>();
                    var sabatiDelMese = Enumerable.Range(1, giorniNelMese).Where(g => new DateTime(anno, mese, g).DayOfWeek == DayOfWeek.Saturday).ToList();
                    int? primoSabatoPreQ = dipendentiDelGruppo
                        .SelectMany(d => datiGruppoPerGiorno.ContainsKey(d["quartina"]?.ToString() ?? "") ? datiGruppoPerGiorno[d["quartina"].ToString()] : new List<int>())
                        .Where(giornoQ => new DateTime(anno, mese, giornoQ).DayOfWeek == DayOfWeek.Sunday)
                        .Select(giornoQ => (int?)(giornoQ - 1))
                        .Where(sabato => sabatiDelMese.Contains(sabato.Value))
                        .OrderBy(sabato => sabato)
                        .FirstOrDefault();

                    if (primoSabatoPreQ.HasValue)
                    {
                        turniSabatoUfficio[primoSabatoPreQ.Value] = "1";
                        int anchorIndex = sabatiDelMese.IndexOf(primoSabatoPreQ.Value);
                        for (int i = anchorIndex - 1; i >= 0; i--) turniSabatoUfficio[sabatiDelMese[i]] = (turniSabatoUfficio[sabatiDelMese[i + 1]] == "1") ? "2" : "1";
                        for (int i = anchorIndex + 1; i < sabatiDelMese.Count; i++) turniSabatoUfficio[sabatiDelMese[i]] = (turniSabatoUfficio[sabatiDelMese[i - 1]] == "1") ? "2" : "1";
                    }

                    // Base: alterna verticalmente solo i giorni feriali (non sabato)
                    for (int giorno = 1; giorno <= giorniNelMese; giorno++)
                    {
                        DateTime d = new DateTime(anno, mese, giorno);
                        if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                        {
                            var candidati = dipendentiDelGruppo.OrderBy(x => rnd.Next()).ToList();
                            int num1 = (int)Math.Ceiling(candidati.Count / 2.0);
                            foreach (var dip in candidati.Take(num1)) scheduleCompleto[dip["id_dip"].ToString()][giorno] = "1";
                            foreach (var dip in candidati.Skip(num1)) scheduleCompleto[dip["id_dip"].ToString()][giorno] = "2";
                        }
                    }

                    // Sovrascrivi con i turni calcolati per i sabati
                    foreach (var dip in dipendentiDelGruppo)
                        foreach (var turnoSabato in turniSabatoUfficio)
                            scheduleCompleto[dip["id_dip"].ToString()][turnoSabato.Key] = turnoSabato.Value;

                    // Correzioni Finali per l'ufficio grande
                    foreach (var dip in dipendentiDelGruppo)
                    { // Correzione Orizzontale
                        for (int i = 3; i <= giorniNelMese; i++)
                        {
                            string t1 = scheduleCompleto[dip["id_dip"].ToString()].ContainsKey(i - 2) ? scheduleCompleto[dip["id_dip"].ToString()][i - 2] : "";
                            string t2 = scheduleCompleto[dip["id_dip"].ToString()].ContainsKey(i - 1) ? scheduleCompleto[dip["id_dip"].ToString()][i - 1] : "";
                            string t3 = scheduleCompleto[dip["id_dip"].ToString()].ContainsKey(i) ? scheduleCompleto[dip["id_dip"].ToString()][i] : "";
                            if (t1 == t2 && t2 == t3 && (t1 == "1" || t1 == "2"))
                                scheduleCompleto[dip["id_dip"].ToString()][i] = (t1 == "1") ? "2" : "1";
                        }
                    }
                    for (int i = 1; i <= giorniNelMese; i++)
                    { // Correzione Verticale
                        var turniDelGiorno = dipendentiDelGruppo.Select(d => scheduleCompleto[d["id_dip"].ToString()].ContainsKey(i) ? scheduleCompleto[d["id_dip"].ToString()][i] : "").Where(t => t == "1" || t == "2").ToList();
                        if (turniDelGiorno.Count > 1 && turniDelGiorno.Distinct().Count() == 1)
                            scheduleCompleto[dipendentiDelGruppo[0]["id_dip"].ToString()][i] = (turniDelGiorno[0] == "1") ? "2" : "1";
                    }
                }

                // SOVRASCRITTURA FINALE CON PRIORITÀ ALTA (per tutti gli uffici)
                foreach (DataRow dip in dipendentiDelGruppo)
                {
                    string idDip = dip["id_dip"].ToString();

                    // Strato RF
                    for (int i = 1; i <= giorniNelMese; i++)
                    {
                        DateTime d = new DateTime(anno, mese, i);
                        if (d.DayOfWeek == DayOfWeek.Sunday || giorniFestivi.Contains(i))
                            scheduleCompleto[idDip][i] = "RF";
                    }

                    // Strato Fureria (sovrascrive tutto, anche RF)
                    if (nomeUfficio == "FURERIA")
                        for (int i = 1; i <= giorniNelMese; i++)
                            if (new DateTime(anno, mese, i).DayOfWeek == DayOfWeek.Saturday)
                                scheduleCompleto[idDip][i] = "1";

                    // Strato 1/2 adiacenti a Q
                    string quartinaDip = dip["quartina"]?.ToString() ?? "";
                    List<int> giorniDaMarcareConQ = datiGruppoPerGiorno.ContainsKey(quartinaDip) ? datiGruppoPerGiorno[quartinaDip] : new List<int>();
                    foreach (int giornoQ in giorniDaMarcareConQ)
                    {
                        if (giornoQ - 1 > 0)
                        {
                            DateTime d = new DateTime(anno, mese, giornoQ - 1);
                            if (d.DayOfWeek != DayOfWeek.Sunday && !giorniFestivi.Contains(giornoQ - 1))
                                scheduleCompleto[idDip][giornoQ - 1] = "1";
                        }
                        if (giornoQ + 1 <= giorniNelMese)
                        {
                            DateTime d = new DateTime(anno, mese, giornoQ + 1);
                            if (d.DayOfWeek != DayOfWeek.Sunday && !giorniFestivi.Contains(giornoQ + 1))
                                scheduleCompleto[idDip][giornoQ + 1] = "2";
                        }
                    }

                    // Strato Q (massima priorità)
                    foreach (int giornoQ in giorniDaMarcareConQ)
                    {
                        scheduleCompleto[idDip][giornoQ] = "Q";
                    }
                }
            }
            return scheduleCompleto;
        }







        /// <summary>
        /// Restituisce un HashSet contenente i numeri dei giorni che sono festivi in un dato mese e anno.
        /// (Esclude i dati dalla tabella reperibilita).
        /// </summary>
        private HashSet<int> CalcolaGiorniFestiviDelMese(int anno, int mese)
        {
            var giorniFestivi = new HashSet<int>();

            // Aggiungi le festività fisse che cadono nel mese corrente
            foreach (var (meseFestivo, giornoFestivo) in FestivitaFisse)
            {
                if (meseFestivo == mese)
                {
                    giorniFestivi.Add(giornoFestivo);
                }
            }

            // Calcola e aggiungi Pasqua e Pasquetta se cadono nel mese corrente
            DateTime pasqua = CalcolaDataPasqua(anno);
            if (pasqua.Month == mese)
            {
                giorniFestivi.Add(pasqua.Day);
            }
            DateTime pasquetta = pasqua.AddDays(1);
            if (pasquetta.Month == mese)
            {
                giorniFestivi.Add(pasquetta.Day);
            }

            // // La logica per la tabella "reperibilita" è stata rimossa
            // DataTable dtReperibilita = mn.getListReperibilita(anno, mese);
            // ...

            return giorniFestivi;
        }
        /// <summary>
        /// Calcola la data della Domenica di Pasqua per un dato anno usando l'algoritmo di Gauss.
        /// </summary>
        private DateTime CalcolaDataPasqua(int anno)
        {
            int a = anno % 19;
            int b = anno / 100;
            int c = anno % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int mese = (h + l - 7 * m + 114) / 31;
            int giorno = ((h + l - 7 * m + 114) % 31) + 1;
            return new DateTime(anno, mese, giorno);
        }

        protected void gvCalendario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                // Imposta l'indice della riga da modificare e rigenera la griglia
                gvCalendario.EditIndex = Convert.ToInt32(e.CommandArgument);
                GeneraGriglia();
            }
            else if (e.CommandName == "Cancel")
            {
                // Annulla la modalità di modifica e rigenera la griglia
                gvCalendario.EditIndex = -1;
                GeneraGriglia();
            }
            else if (e.CommandName == "Update")
            {
                // Logica per salvare i dati
                int rowIndex = gvCalendario.EditIndex;
                GridViewRow row = gvCalendario.Rows[rowIndex]; // Attenzione, questo potrebbe non funzionare con i separatori. Troviamo la riga manualmente.

                // Trova la riga corretta nella tabella generata manualmente
                Table gridTable = (Table)gvCalendario.Controls[0];
                int employeeRowCounter = -1;
                GridViewRow rowToUpdate = null;
                foreach (TableRow tr in gridTable.Rows)
                {
                    if (tr is GridViewRow && ((GridViewRow)tr).RowType == DataControlRowType.DataRow)
                    {
                        // Contiamo solo le righe dei dipendenti, ignorando i separatori
                        if (!tr.Cells[0].CssClass.Contains("ufficio-separator-row"))
                        {
                            employeeRowCounter++;
                            if (employeeRowCounter == rowIndex)
                            {
                                rowToUpdate = (GridViewRow)tr;
                                break;
                            }
                        }
                    }
                }

                if (rowToUpdate != null)
                {
                    string idDipendente = e.CommandArgument.ToString();

                    // Itera sulle celle, trova le TextBox e leggi i valori
                    for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
                    {
                        TextBox txtTurno = rowToUpdate.FindControl("txtGiorno_" + i) as TextBox;
                        if (txtTurno != null)
                        {
                            string valoreTurno = txtTurno.Text;
                            // QUI VA LA LOGICA DI SALVATAGGIO SUL DATABASE
                            // Esempio: SalvaDatoTurno(idDipendente, i, valoreTurno);
                        }
                    }
                }


                // Esci dalla modalità di modifica e ricarica la griglia
                gvCalendario.EditIndex = -1;
                GeneraGriglia();

            }
        }
        protected void gvCalendario_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCalendario.EditIndex = e.NewEditIndex;
            CaricaGriglia(); // Ricarica la griglia per mostrare le TextBox
        }

        protected void gvCalendario_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                System.Data.DataTable dt = GetDataTable();
                int rowIndex = e.RowIndex;

                // Trova la riga nella DataTable
                // Assumo che l'ordine delle righe nella GridView sia lo stesso della DataTable
                DataRow dr = dt.Rows[rowIndex];

                // Itera sulle colonne dei giorni (saltando le colonne IDDipendente e NomeDipendente)
                // L'indice 1 corrisponde al primo giorno (D1)
                for (int i = 1; i <= DateTime.DaysInMonth(int.Parse(txtAnno.Text), int.Parse(ddlMese.SelectedValue)); i++)
                {
                    // La colonna Giorno "Di" si trova all'indice i+1 nella GridView
                    TemplateField colonnaGiorno = (TemplateField)gvCalendario.Columns[i + 1];

                    // Trova la TextBox nel TemplateField
                    TextBox txtGiorno = (TextBox)gvCalendario.Rows[rowIndex].Cells[i + 1].Controls[0];

                    // Aggiorna il valore nella DataTable
                    dr[$"D{i}"] = txtGiorno.Text.Trim();
                }

                // Aggiorna il ViewState
                ViewState["CalendarioData"] = dt;

                // Esci dalla modalità di modifica e ricarica
                gvCalendario.EditIndex = -1;
                CaricaGriglia();
            }
            catch (Exception ex)
            {
                lblErrore.Text = $"Errore nell'aggiornamento: {ex.Message}";
            }
            //}
        }

        protected void gvCalendario_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //    gvCalendario.EditIndex = -1;
            //  CaricaGriglia();
        }
        private DataTable GetDataTable()
        {
            if (ViewState["CalendarioData"] == null)
            {

                Manager mn = new Manager();
                DataTable dt = new DataTable();
                dt = mn.getListDipendentire();
                //dt.Columns.Add("IDDipendente", typeof(int));
                //dt.Columns.Add("NomeDipendente", typeof(string));

                // Aggiunge le colonne dei giorni (D1, D2, ..., D31)
                for (int i = 1; i <= 31; i++)
                {
                    dt.Columns.Add($"D{i}", typeof(string)); // Usa string per un codice (P=Presente, A=Assente, F=Ferie)
                }



                ViewState["CalendarioData"] = dt;
            }
            return (DataTable)ViewState["CalendarioData"];
        }

        // Funzione modificata per determinare se un giorno è festivo o weekend
        private bool IsGiornoFestivo(int anno, int mese, int giorno)
        {
            DateTime data = new DateTime(anno, mese, giorno);

            // 1. Controlla il Weekend (Sabato = 6, Domenica = 0)
            if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }

            // 2. Controlla le Festività Fisse
            if (FestivitaFisse.Any(f => f.mese == mese && f.giorno == giorno))
            {
                return true;
            }

            // 3. (OPZIONALE) Controlla Pasqua/Pasquetta
            // La logica per Pasqua è complessa e viene omessa per semplicità in questo esempio.

            return false;
        }

        // Lista (semplificata) delle festività fisse italiane (mese, giorno)
        private static readonly List<(int mese, int giorno)> FestivitaFisse = new List<(int mese, int giorno)>
{
    (1, 1),   // Capodanno
    (1, 6),   // Epifania
    (4, 25),  // Festa della Liberazione
    (5, 1),   // Festa del Lavoro
    (6, 2),   // Festa della Repubblica
    (8, 15),  // Ferragosto
    (11, 1),  // Ognissanti
    (12, 8),  // Immacolata
    (12, 25), // Natale
    (12, 26)  // Santo Stefano
    // Nota: Pasqua e Lunedì dell'Angelo sono mobili e richiederebbero un calcolo separato.
};

        protected void txtAnno_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

