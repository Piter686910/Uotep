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
        bool isEditMode = false;
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
                //CaricaGriglia();
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                ProtocolloLiteral.Text = decodedText;
                txtAnno.Text = System.Convert.ToInt32(DateTime.Now.Year).ToString();
                
            }
            else
                CaricaGriglia();



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

        protected void apripopupDecretazione_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#ModalDecretazione').modal('show');", true);

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
            try
            {
                int mese = int.Parse(ddlMese.SelectedValue);
                int anno = int.Parse(txtAnno.Text);
                int giorniNelMese = DateTime.DaysInMonth(anno, mese);



                //    // 1. Pulisce le colonne dinamiche esistenti
                //while (gvCalendario.Columns.Count > 2)
                //{
                //    gvCalendario.Columns.RemoveAt(1);
                //}

                // 2. Aggiunge le colonne dei giorni (D1, D2, ...)
                for (int i = 1; i <= giorniNelMese; i++)
                {
                    DateTime dataCorrente = new DateTime(anno, mese, i);
                    TemplateField giornoField = new TemplateField();

                    giornoField.HeaderText = $"{dataCorrente.ToString("ddd", CultureInfo.CurrentCulture)}\n{i}";
                    giornoField.HeaderStyle.CssClass = "text-center";
                    giornoField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                    //        // ***  Segnalazione delle festività/weekend ***
                    if (IsGiornoFestivo(anno, mese, i))
                    {
                        // Applica una classe CSS all'Header e alla cella
                        giornoField.HeaderStyle.CssClass += " giorno-festivo-header";
                        giornoField.ItemStyle.CssClass = "giorno-festivo-cella";
                    }
                    //        // *******************************************************

                    //        // Definizione del campo per la modalità di visualizzazione (ItemTemplate)
                    giornoField.ItemTemplate = new GridViewTemplate(ListItemType.Item, $"D{i}", "Label");

                    //        // Definizione del campo per la modalità di modifica (EditItemTemplate)
                    giornoField.EditItemTemplate = new GridViewTemplate(ListItemType.EditItem, $"D{i}", "TextBox");

                    gvCalendario.Columns.Insert(i, giornoField);
                }
                GeneraGriglia();
                ////    // 3. Associa la DataTable
                //DataTable dt = GetDataTable();
                //gvCalendario.DataSource = dt;
                //gvCalendario.DataBind();

            }
            catch (Exception ex)
            {
                lblErrore.Text = $"Errore nel caricamento della griglia: {ex.Message}";
            }
        }
        protected void ddlMese_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaricaGriglia();
        }

        

        protected void btnCarica_Click(object sender, EventArgs e)
        {
            gvCalendario.EditIndex = -1;
            GeneraGriglia();
            btnsalva.Enabled = true;

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

        private void GeneraGriglia()
        {
            // --- 1. SETUP INIZIALE E CARICAMENTO DATI ---
            Manager mn = new Manager();
            DataTable dt = mn.getListDipendenti();

            Table gridTable = gvCalendario.Controls.OfType<Table>().FirstOrDefault();
            if (gridTable == null)
            {
                gridTable = new Table();
                gvCalendario.Controls.Add(gridTable);
            }
            gridTable.Rows.Clear();

            int anno = System.Convert.ToInt32(txtAnno.Text);
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
            Session["TurniMensili"] = turniCalcolati;
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

            // --- 5. CICLO DI RENDERIZZAZIONE ---
            int visualRowIndex = 0;
            var gruppiUfficio = dt.AsEnumerable().GroupBy(r => r.Field<string>("ufficio").ToUpper());

            foreach (var gruppo in gruppiUfficio)
            {
                string nomeUfficio = gruppo.Key;
                List<DataRow> dipendentiDelGruppo = gruppo.ToList();

                // ** GESTIONE SEPARATORI UFFICIO **
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

                     isEditMode = (visualRowIndex == gvCalendario.EditIndex);

                    GridViewRow employeeRow = new GridViewRow(visualRowIndex, visualRowIndex, DataControlRowType.DataRow, isEditMode ? DataControlRowState.Edit : DataControlRowState.Normal);
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
                            if (cellText == "RF") {
                                cellTurno.CssClass += " text-center";
                                cellTurno.Font.Bold = true;
                                cellTurno.BackColor = Color.LightBlue;
                                cellTurno.ForeColor = Color.White;
                            } else if (cellText == "Q") {
                                cellTurno.CssClass += " text-center";
                                cellTurno.Font.Bold = true;
                                cellTurno.BackColor = Color.Green;
                                cellTurno.ForeColor = Color.White;
                            } else if (cellText == "1" || cellText == "2") { /* Stili... */ }
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
        /// 
        //private Dictionary<string, Dictionary<int, string>> CalcolaTurniComplessi(DataTable dtDipendenti, Dictionary<string, List<int>> datiGruppoPerGiorno, HashSet<int> giorniFestivi, int anno, int mese)
        //{
        //    var scheduleCompleto = new Dictionary<string, Dictionary<int, string>>();
        //    var rnd = new Random();
        //    int giorniNelMese = DateTime.DaysInMonth(anno, mese);

        //    var gruppiUfficio = dtDipendenti.AsEnumerable().GroupBy(r => r.Field<string>("ufficio").ToUpper());

        //    foreach (var gruppo in gruppiUfficio)
        //    {
        //        string nomeUfficio = gruppo.Key;
        //        List<DataRow> dipendentiDelGruppo = gruppo.ToList();

        //        // Inizializzazione per l'ufficio
        //        foreach (DataRow dip in dipendentiDelGruppo)
        //        {
        //            scheduleCompleto[dip["id_dip"].ToString()] = new Dictionary<int, string>();
        //        }

        //        if (dipendentiDelGruppo.Count <= 2)
        //        {
        //            // ** NUOVA LOGICA SEMPLICE per uffici piccoli con ALTERNANZA **
        //            var candidati = dipendentiDelGruppo.OrderBy(x => x["id_dip"]).ToList();
        //            bool assegnaUnoAlPrimo = true;

        //            for (int giorno = 1; giorno <= giorniNelMese; giorno++)
        //            {
        //                if (candidati.Count > 0)
        //                    scheduleCompleto[candidati[0]["id_dip"].ToString()][giorno] = assegnaUnoAlPrimo ? "1" : "2";
        //                if (candidati.Count > 1)
        //                    scheduleCompleto[candidati[1]["id_dip"].ToString()][giorno] = assegnaUnoAlPrimo ? "2" : "1";

        //                assegnaUnoAlPrimo = !assegnaUnoAlPrimo; // Inverti per il giorno successivo
        //            }
        //        }
        //        else // LOGICA COMPLESSA per uffici grandi
        //        {
        //            // Calcolo sequenza Sabati
        //            var turniSabatoUfficio = new Dictionary<int, string>();
        //            var sabatiDelMese = Enumerable.Range(1, giorniNelMese).Where(g => new DateTime(anno, mese, g).DayOfWeek == DayOfWeek.Saturday).ToList();
        //            int? primoSabatoPreQ = dipendentiDelGruppo
        //                .SelectMany(d => datiGruppoPerGiorno.ContainsKey(d["quartina"]?.ToString() ?? "") ? datiGruppoPerGiorno[d["quartina"].ToString()] : new List<int>())
        //                .Where(giornoQ => new DateTime(anno, mese, giornoQ).DayOfWeek == DayOfWeek.Sunday)
        //                .Select(giornoQ => (int?)(giornoQ - 1))
        //                .Where(sabato => sabatiDelMese.Contains(sabato.Value))
        //                .OrderBy(sabato => sabato)
        //                .FirstOrDefault();

        //            if (primoSabatoPreQ.HasValue)
        //            {
        //                turniSabatoUfficio[primoSabatoPreQ.Value] = "1";
        //                int anchorIndex = sabatiDelMese.IndexOf(primoSabatoPreQ.Value);
        //                for (int i = anchorIndex - 1; i >= 0; i--) turniSabatoUfficio[sabatiDelMese[i]] = (turniSabatoUfficio[sabatiDelMese[i + 1]] == "1") ? "2" : "1";
        //                for (int i = anchorIndex + 1; i < sabatiDelMese.Count; i++) turniSabatoUfficio[sabatiDelMese[i]] = (turniSabatoUfficio[sabatiDelMese[i - 1]] == "1") ? "2" : "1";
        //            }

        //            // Base: alterna verticalmente solo i giorni feriali (non sabato)
        //            for (int giorno = 1; giorno <= giorniNelMese; giorno++)
        //            {
        //                DateTime d = new DateTime(anno, mese, giorno);
        //                if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
        //                {
        //                    var candidati = dipendentiDelGruppo.OrderBy(x => rnd.Next()).ToList();
        //                    int num1 = (int)Math.Ceiling(candidati.Count / 2.0);
        //                    foreach (var dip in candidati.Take(num1)) scheduleCompleto[dip["id_dip"].ToString()][giorno] = "1";
        //                    foreach (var dip in candidati.Skip(num1)) scheduleCompleto[dip["id_dip"].ToString()][giorno] = "2";
        //                }
        //            }

        //            // Sovrascrivi con i turni calcolati per i sabati
        //            foreach (var dip in dipendentiDelGruppo)
        //                foreach (var turnoSabato in turniSabatoUfficio)
        //                    scheduleCompleto[dip["id_dip"].ToString()][turnoSabato.Key] = turnoSabato.Value;

        //            // Correzioni Finali per l'ufficio grande
        //            foreach (var dip in dipendentiDelGruppo)
        //            { // Correzione Orizzontale
        //                for (int i = 3; i <= giorniNelMese; i++)
        //                {
        //                    string t1 = scheduleCompleto[dip["id_dip"].ToString()].ContainsKey(i - 2) ? scheduleCompleto[dip["id_dip"].ToString()][i - 2] : "";
        //                    string t2 = scheduleCompleto[dip["id_dip"].ToString()].ContainsKey(i - 1) ? scheduleCompleto[dip["id_dip"].ToString()][i - 1] : "";
        //                    string t3 = scheduleCompleto[dip["id_dip"].ToString()].ContainsKey(i) ? scheduleCompleto[dip["id_dip"].ToString()][i] : "";
        //                    if (t1 == t2 && t2 == t3 && (t1 == "1" || t1 == "2"))
        //                        scheduleCompleto[dip["id_dip"].ToString()][i] = (t1 == "1") ? "2" : "1";
        //                }
        //            }
        //            for (int i = 1; i <= giorniNelMese; i++)
        //            { // Correzione Verticale
        //                var turniDelGiorno = dipendentiDelGruppo.Select(d => scheduleCompleto[d["id_dip"].ToString()].ContainsKey(i) ? scheduleCompleto[d["id_dip"].ToString()][i] : "").Where(t => t == "1" || t == "2").ToList();
        //                if (turniDelGiorno.Count > 1 && turniDelGiorno.Distinct().Count() == 1)
        //                    scheduleCompleto[dipendentiDelGruppo[0]["id_dip"].ToString()][i] = (turniDelGiorno[0] == "1") ? "2" : "1";
        //            }
        //        }

        //        // SOVRASCRITTURA FINALE CON PRIORITÀ ALTA (per tutti gli uffici)
        //        foreach (DataRow dip in dipendentiDelGruppo)
        //        {
        //            string idDip = dip["id_dip"].ToString();

        //            // Strato RF
        //            for (int i = 1; i <= giorniNelMese; i++)
        //            {
        //                DateTime d = new DateTime(anno, mese, i);
        //                if (d.DayOfWeek == DayOfWeek.Sunday || giorniFestivi.Contains(i))
        //                    scheduleCompleto[idDip][i] = "RF";
        //            }

        //            // Strato Fureria (sovrascrive tutto, anche RF)
        //            if (nomeUfficio == "FURERIA")
        //                for (int i = 1; i <= giorniNelMese; i++)
        //                    if (new DateTime(anno, mese, i).DayOfWeek == DayOfWeek.Saturday)
        //                        scheduleCompleto[idDip][i] = "1";


        //            // Strato Preferenze
        //            string preferenze = dip["turni_pref"]?.ToString();
        //            if (!string.IsNullOrEmpty(preferenze))
        //            {
        //                for (int i = 1; i <= giorniNelMese; i++)
        //                {
        //                    // Cerca una preferenza per il giorno corrente
        //                    DateTime d = new DateTime(anno, mese, i);
        //                    string giornoAbbr = d.ToString("ddd", new CultureInfo("it-IT")).ToUpper();
        //                    string turnoDaForzare = null;
        //                    if (preferenze.Contains($"{giornoAbbr}2")) turnoDaForzare = "2";
        //                    else if (preferenze.Contains($"{giornoAbbr}1")) turnoDaForzare = "1";

        //                    // Se trovi una preferenza e il giorno è lavorativo, applicala e inverti i vicini
        //                    if (turnoDaForzare != null && scheduleCompleto[idDip].ContainsKey(i) && scheduleCompleto[idDip][i] != "RF")
        //                    {
        //                        scheduleCompleto[idDip][i] = turnoDaForzare;

        //                        // Inverti il giorno PRIMA, se è un turno "1" o "2"
        //                        int giornoPrima = i - 1;
        //                        if (giornoPrima > 0 && scheduleCompleto[idDip].ContainsKey(giornoPrima))
        //                        {
        //                            string turnoPrec = scheduleCompleto[idDip][giornoPrima];
        //                            if (turnoPrec == "1") scheduleCompleto[idDip][giornoPrima] = "2";
        //                            else if (turnoPrec == "2") scheduleCompleto[idDip][giornoPrima] = "1";
        //                        }

        //                        // Inverti il giorno DOPO, se è un turno "1" o "2"
        //                        int giornoDopo = i + 1;
        //                        if (giornoDopo <= giorniNelMese && scheduleCompleto[idDip].ContainsKey(giornoDopo))
        //                        {
        //                            string turnoSucc = scheduleCompleto[idDip][giornoDopo];
        //                            if (turnoSucc == "1") scheduleCompleto[idDip][giornoDopo] = "2";
        //                            else if (turnoSucc == "2") scheduleCompleto[idDip][giornoDopo] = "1";
        //                        }
        //                    }
        //                }
        //            }



        //            // Strato 1/2 adiacenti a Q
        //            string quartinaDip = dip["quartina"]?.ToString() ?? "";
        //            List<int> giorniDaMarcareConQ = datiGruppoPerGiorno.ContainsKey(quartinaDip) ? datiGruppoPerGiorno[quartinaDip] : new List<int>();
        //            foreach (int giornoQ in giorniDaMarcareConQ)
        //            {
        //                if (giornoQ - 1 > 0)
        //                {
        //                    DateTime d = new DateTime(anno, mese, giornoQ - 1);
        //                    if (d.DayOfWeek != DayOfWeek.Sunday && !giorniFestivi.Contains(giornoQ - 1))
        //                        scheduleCompleto[idDip][giornoQ - 1] = "1";
        //                }
        //                if (giornoQ + 1 <= giorniNelMese)
        //                {
        //                    DateTime d = new DateTime(anno, mese, giornoQ + 1);
        //                    if (d.DayOfWeek != DayOfWeek.Sunday && !giorniFestivi.Contains(giornoQ + 1))
        //                        scheduleCompleto[idDip][giornoQ + 1] = "2";
        //                }
        //            }

        //            // Strato Q (massima priorità)
        //            foreach (int giornoQ in giorniDaMarcareConQ)
        //            {
        //                scheduleCompleto[idDip][giornoQ] = "Q";
        //            }
        //        }
        //    }
        //    return scheduleCompleto;
        //}


        ///////

        private Dictionary<string, Dictionary<int, string>> CalcolaTurniComplessi(DataTable dtDipendenti, Dictionary<string, List<int>> datiGruppoPerGiorno, HashSet<int> giorniFestivi, int anno, int mese)
        {
            var scheduleCompleto = new Dictionary<string, Dictionary<int, string>>();
            int giorniNelMese = DateTime.DaysInMonth(anno, mese);
            var rnd = new Random();
           
            var UfficiConVincoloAutista = new HashSet<string> { "MACRO1", "MACRO2", "MACRO3" };
            var gruppiUfficio = dtDipendenti.AsEnumerable().GroupBy(r => r.Field<string>("ufficio").ToUpper());

            foreach (var gruppo in gruppiUfficio)
            {
                string nomeUfficio = gruppo.Key;
                List<DataRow> dipendentiDelGruppo = gruppo.ToList();

                foreach (DataRow dip in dipendentiDelGruppo)
                    scheduleCompleto[dip["id_dip"].ToString()] = new Dictionary<int, string>();

                //  uffici con 2 dipendenti
                if (dipendentiDelGruppo.Count <= 2)
                {
                    var candidati = dipendentiDelGruppo.OrderBy(x => x["id_dip"]).ToList();
                    bool assegnaUnoAlPrimo = true;
                    for (int giorno = 1; giorno <= giorniNelMese; giorno++)
                    {
                        if (candidati.Count > 0)
                            scheduleCompleto[candidati[0]["id_dip"].ToString()][giorno] = assegnaUnoAlPrimo ? "1" : "2";
                        if (candidati.Count > 1)
                            scheduleCompleto[candidati[1]["id_dip"].ToString()][giorno] = assegnaUnoAlPrimo ? "2" : "1";
                        assegnaUnoAlPrimo = !assegnaUnoAlPrimo;
                    }
                }
                else
                {
                    // STRATO SABATI alternati
                    int? turnoSabatoPrecedente = null;
                    for (int giorno = 1; giorno <= giorniNelMese; giorno++)
                    {
                        DateTime d = new DateTime(anno, mese, giorno);
                        if (d.DayOfWeek == DayOfWeek.Saturday)
                        {
                            int turnoCorrente = turnoSabatoPrecedente == null ? 1 : (turnoSabatoPrecedente == 1 ? 2 : 1);
                            turnoSabatoPrecedente = turnoCorrente;
                            bool usaPrimo = turnoCorrente == 1;

                            foreach (DataRow dip in dipendentiDelGruppo)
                            {
                                string id = dip["id_dip"].ToString();
                                scheduleCompleto[id][giorno] = usaPrimo ? "1" : "2";
                                usaPrimo = !usaPrimo;
                            }
                        }
                    }

                    // STRATO: Sabato con 2 Q il giorno dopo
                    for (int giorno = 1; giorno <= giorniNelMese; giorno++)
                    {
                        DateTime d = new DateTime(anno, mese, giorno);
                        if (d.DayOfWeek == DayOfWeek.Saturday && giorno + 1 <= giorniNelMese)
                        {
                            int giornoDopo = giorno + 1;
                            var dipConQ = dipendentiDelGruppo
                                .Where(x =>
                                    datiGruppoPerGiorno.ContainsKey(x["quartina"]?.ToString()) &&
                                    datiGruppoPerGiorno[x["quartina"]?.ToString()].Contains(giornoDopo))
                                .ToList();

                            if (dipConQ.Count >= 2)
                            {
                                foreach (DataRow dip in dipendentiDelGruppo)
                                {
                                    string id = dip["id_dip"].ToString();
                                    if (!dipConQ.Contains(dip))
                                        scheduleCompleto[id][giorno] = "2";
                                }
                            }
                        }
                    }
                }

                // APPLICAZIONE STRATI RF, Fureria, Preferenze, Q e cdr e adiacenti
                foreach (DataRow dip in dipendentiDelGruppo)
                {
                    string idDip = dip["id_dip"].ToString();

                    // RF
                    for (int i = 1; i <= giorniNelMese; i++)
                    {
                        DateTime d = new DateTime(anno, mese, i);
                        if (d.DayOfWeek == DayOfWeek.Sunday || giorniFestivi.Contains(i))
                            scheduleCompleto[idDip][i] = "RF";
                    }

                    // Fureria
                    if (nomeUfficio == "FURERIA")
                    {
                        for (int i = 1; i <= giorniNelMese; i++)
                        {
                            DateTime d = new DateTime(anno, mese, i);
                            if (d.DayOfWeek == DayOfWeek.Saturday)
                                scheduleCompleto[idDip][i] = "1";
                        }
                    }
                    // cdr
                    if (nomeUfficio == "CDR")
                    {
                        for (int i = 1; i <= giorniNelMese; i++)
                        {
                            DateTime d = new DateTime(anno, mese, i);
                           // if (d.DayOfWeek == DayOfWeek.Saturday)
                                scheduleCompleto[idDip][i] = "1";
                        }
                    }
                    // Preferenze
                    string preferenze = dip["turni_pref"]?.ToString();
                    if (!string.IsNullOrEmpty(preferenze))
                    {
                        for (int i = 1; i <= giorniNelMese; i++)
                        {
                            if (!scheduleCompleto[idDip].ContainsKey(i) || scheduleCompleto[idDip][i] != "RF")
                            {
                                DateTime d = new DateTime(anno, mese, i);
                                string giornoAbbr = d.ToString("ddd", new CultureInfo("it-IT")).ToUpper();
                                if (preferenze.Contains($"{giornoAbbr}2"))
                                    scheduleCompleto[idDip][i] = "2";
                                else if (preferenze.Contains($"{giornoAbbr}1"))
                                    scheduleCompleto[idDip][i] = "1";
                            }
                        }
                    }

                    // Adiacenti Q
                    string quartinaDip = dip["quartina"]?.ToString() ?? "";
                    List<int> giorniDaMarcareConQ = datiGruppoPerGiorno.ContainsKey(quartinaDip) ? datiGruppoPerGiorno[quartinaDip] : new List<int>();
                    foreach (int giornoQ in giorniDaMarcareConQ)
                    {
                        if (giornoQ - 1 > 0)
                        {
                            DateTime d = new DateTime(anno, mese, giornoQ);
                            if (d.DayOfWeek == DayOfWeek.Sunday)
                                scheduleCompleto[idDip][giornoQ - 1] = "1";
                        }
                        if (giornoQ + 1 <= giorniNelMese)
                        {
                            DateTime d = new DateTime(anno, mese, giornoQ);
                            if (d.DayOfWeek == DayOfWeek.Sunday)
                                scheduleCompleto[idDip][giornoQ + 1] = "2";
                        }
                    }

                    // Q
                    foreach (int giornoQ in giorniDaMarcareConQ)
                        scheduleCompleto[idDip][giornoQ] = "Q";
                    
                }

                // STRATO FINALE: giorni vuoti per uffici >2 dipendenti (alternanza + 50% per ufficio)
                if (dipendentiDelGruppo.Count > 2)
                {
                    //for (int giorno = 1; giorno <= giorniNelMese; giorno++)
                    //{
                    //    var vuoti = dipendentiDelGruppo
                    //        .Where(d => !scheduleCompleto[d["id_dip"].ToString()].ContainsKey(giorno))
                    //        .ToList();
                    //    if (!vuoti.Any()) continue;

                    //    int half = vuoti.Count / 2;
                    //    List<string> turniDisponibili = new List<string>();
                    //    turniDisponibili.AddRange(Enumerable.Repeat("1", half));
                    //    turniDisponibili.AddRange(Enumerable.Repeat("2", vuoti.Count - half));

                    //    // Mescola per non dare sempre lo stesso turno all'ultimo dipendente
                    //    turniDisponibili = turniDisponibili.OrderBy(x => rnd.Next()).ToList();

                    //    for (int i = 0; i < vuoti.Count; i++)
                    //    {
                    //        string id = vuoti[i]["id_dip"].ToString();

                    //        // Controllo sequenza: non più di due consecutivi uguali
                    //        if (scheduleCompleto[id].ContainsKey(giorno - 1) && scheduleCompleto[id].ContainsKey(giorno - 2))
                    //        {
                    //            string t1 = scheduleCompleto[id][giorno - 1];
                    //            string t2 = scheduleCompleto[id][giorno - 2];
                    //            if (t1 == t2 && (t1 == turniDisponibili[i]))
                    //            {
                    //                turniDisponibili[i] = t1 == "1" ? "2" : "1";
                    //            }
                    //        }

                    //        scheduleCompleto[id][giorno] = turniDisponibili[i];
                    //    }
                    //}




                    for (int giorno = 1; giorno <= giorniNelMese; giorno++)
                    {
                        var vuoti = dipendentiDelGruppo
                            .Where(d => !scheduleCompleto[d["id_dip"].ToString()].ContainsKey(giorno))
                            .ToList();
                        if (!vuoti.Any()) continue;

                        // Controlla se l'ufficio ha il vincolo
                        if (UfficiConVincoloAutista.Contains(nomeUfficio))
                        {
                            // LOGICA CON VINCOLO AUTISTA
                            var autistiDisponibili = vuoti.Where(d => d.Field<bool>("autista")).OrderBy(x => rnd.Next()).ToList();
                            var nonAutistiDisponibili = vuoti.Except(autistiDisponibili).OrderBy(x => rnd.Next()).ToList();

                            var assegnazioniGiorno = new Dictionary<string, string>();

                            // Assegna forzatamente autisti a "1" e "2" se possibile
                            var autistaPer1 = autistiDisponibili.FirstOrDefault();
                            if (autistaPer1 != null) assegnazioniGiorno[autistaPer1["id_dip"].ToString()] = "1";

                            var autistaPer2 = autistiDisponibili.Skip(1).FirstOrDefault();
                            if (autistaPer2 != null) assegnazioniGiorno[autistaPer2["id_dip"].ToString()] = "2";

                            // Crea la lista dei rimanenti da assegnare
                            var rimanenti = nonAutistiDisponibili.Concat(autistiDisponibili.Skip(2)).ToList();

                            // Calcola quanti ne mancano per bilanciare i turni
                            int target1 = (int)Math.Ceiling(vuoti.Count / 2.0) - (autistaPer1 != null ? 1 : 0);
                            int target2 = vuoti.Count - (int)Math.Ceiling(vuoti.Count / 2.0) - (autistaPer2 != null ? 1 : 0);

                            // Assegna i rimanenti per raggiungere i target
                            foreach (var r in rimanenti.Take(target1)) assegnazioniGiorno[r["id_dip"].ToString()] = "1";
                            foreach (var r in rimanenti.Skip(target1)) assegnazioniGiorno[r["id_dip"].ToString()] = "2";

                            // Applica le assegnazioni del giorno allo schedule principale
                            foreach (var ass in assegnazioniGiorno)
                                scheduleCompleto[ass.Key][giorno] = ass.Value;
                        }
                        else // LOGICA STANDARD (senza vincolo)
                        {
                            int half = (int)Math.Ceiling(vuoti.Count / 2.0);
                            var assegnatiA1 = vuoti.Take(half).ToList();
                            var assegnatiA2 = vuoti.Skip(half).ToList();
                            foreach (var v in assegnatiA1) scheduleCompleto[v["id_dip"].ToString()][giorno] = "1";
                            foreach (var v in assegnatiA2) scheduleCompleto[v["id_dip"].ToString()][giorno] = "2";
                        }

                        // Correzione Orizzontale (applica a tutti i dipendenti riempiti in questo giorno)
                        foreach (var dip in vuoti)
                        {
                            string id = dip["id_dip"].ToString();
                            if (scheduleCompleto[id].ContainsKey(giorno - 1) && scheduleCompleto[id].ContainsKey(giorno - 2))
                            {
                                string t1 = scheduleCompleto[id][giorno - 1];
                                string t2 = scheduleCompleto[id][giorno - 2];
                                if (t1 == t2 && (t1 == scheduleCompleto[id][giorno]))
                                {
                                    scheduleCompleto[id][giorno] = (t1 == "1" ? "2" : "1");
                                }
                            }
                        }
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
                // Imposta l'indice della riga da modificare
                gvCalendario.EditIndex = Convert.ToInt32(e.CommandArgument);
                // Rigenera la griglia per mostrare le TextBox
                GeneraGriglia();
            }
            else if (e.CommandName == "Cancel")
            {
                // Resetta l'indice per uscire dalla modalità modifica
                gvCalendario.EditIndex = -1;
                // Rigenera la griglia per tornare in modalità visualizzazione
                GeneraGriglia();
            }
            else if (e.CommandName == "Update")
            {
                string idDipendente = e.CommandArgument.ToString();
                Manager mn = new Manager();
                int idDip = Convert.ToInt32(e.CommandArgument);

                // Trova la riga corretta nella tabella generata manualmente
                Table gridTable = (Table)gvCalendario.Controls[0];
                GridViewRow rowToUpdate = null;
                foreach (TableRow row in gridTable.Rows)
                {
                    if (row is GridViewRow && ((GridViewRow)row).RowIndex == gvCalendario.EditIndex)
                    {
                        rowToUpdate = (GridViewRow)row;
                        break;
                    }
                }
                DataTable turniDaSalvare = new DataTable();
                turniDaSalvare.Columns.Add("DataUltimaModifica", typeof(DateTime));
                turniDaSalvare.Columns.Add("CodiceTurno", typeof(string));
                int anno = int.Parse(txtAnno.Text);
                int mese = int.Parse(ddlMese.SelectedValue);
                int giorniNelMese = DateTime.DaysInMonth(anno, mese);

                if (rowToUpdate != null)
                {
                    for (int i = 1; i <= giorniNelMese; i++)
                    {
                        // Cerca la TextBox per ottenere il suo ID renderizzato (es. 'MainContent_gvCalendario_..._txtGiorno_1')
                        TextBox txtTurno = rowToUpdate.Cells[i].Controls.OfType<TextBox>().FirstOrDefault();
                        if (txtTurno != null)
                        {
                            // Usa l'ID renderizzato per leggere il valore direttamente dalla richiesta del browser
                            string valoreNuovo = Request.Form[txtTurno.UniqueID];

                            DateTime dataTurno = new DateTime(anno, mese, i);

                            // Aggiungi la riga con il valore corretto
                            turniDaSalvare.Rows.Add(dataTurno, valoreNuovo.Trim());
                        }
                    }

                    if (turniDaSalvare.Rows.Count > 0)
                    {
                        mn.UpdTurnoMensile(idDip, turniDaSalvare);
                        lblErrore.Text = "Salvataggio completato!";
                        lblErrore.ForeColor = Color.Green;
                    }
                }
                //if (rowToUpdate != null)
                //{
                //    // 1. Prepara una DataTable che abbia LA STESSA STRUTTURA del nostro Tipo SQL 'TurnoType'
                //    DataTable turniDaSalvare = new DataTable();
                //    turniDaSalvare.Columns.Add("DataUltimaModifica", typeof(DateTime));
                //    turniDaSalvare.Columns.Add("CodiceTurno", typeof(string));

                //    int anno = int.Parse(txtAnno.Text);
                //    int mese = int.Parse(ddlMese.SelectedValue);
                //    int giorniNelMese = DateTime.DaysInMonth(anno, mese);

                //    // 2. Raccogli tutti i dati dalle TextBox e popola la DataTable
                //    for (int i = 1; i <= giorniNelMese; i++)
                //    {
                //        TextBox txtTurno = rowToUpdate.Cells[i].Controls.OfType<TextBox>().FirstOrDefault();
                //        if (txtTurno != null)
                //        {
                //            string valoreTurno = txtTurno.Text.Trim();

                //            // Aggiungi solo i turni che non sono vuoti (opzionale, ma consigliato)
                //            if (!string.IsNullOrEmpty(valoreTurno))
                //            {
                //                DateTime dataTurno = new DateTime(anno, mese, i);
                //                turniDaSalvare.Rows.Add(dataTurno, valoreTurno);
                //            }
                //        }
                //    }

                //    // 3. Invia la DataTable al database in un colpo solo
                //    if (turniDaSalvare.Rows.Count > 0)
                //    {
                //        Manager mn = new Manager();
                //        mn.UpdTurnoMensile(idDip, turniDaSalvare);
                //    }
                //}


                // Dopo aver salvato, esci dalla modalità modifica
                gvCalendario.EditIndex = -1;
                // Rigenera la griglia per mostrare i dati aggiornati
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
            gvCalendario.EditIndex = -1;
            CaricaGriglia();
        }
        private DataTable GetDataTable()
        {
            if (ViewState["CalendarioData"] == null)
            {

                Manager mn = new Manager();
                DataTable dt = new DataTable();
                dt = mn.getListDipendenti();
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


        protected void btnsalva_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            int anno = int.Parse(txtAnno.Text);
            int mese = int.Parse(ddlMese.SelectedValue);
            string txtmese = ddlMese.SelectedItem.Text;

            var turniCompleti = Session["TurniMensili"] as Dictionary<string, Dictionary<int, string>>;
            if (turniCompleti == null)
            {
                // Gestisci l'errore della sessione scaduta...
                return;
            }

            // --- OTTIMIZZAZIONE: Carica i dati di TUTTI i dipendenti UNA SOLA VOLTA ---
            DataTable dtTuttiDipendenti = mn.getListDipendenti();
            // Crea un dizionario per una ricerca veloce dei dati del dipendente
            var dipendentiMap = dtTuttiDipendenti.AsEnumerable()
                .ToDictionary(
                    row => row.Field<int>("id_dip").ToString(), // Chiave = ID Dipendente come stringa
                    row => new
                    {
                        Nominativo = row.Field<string>("nominativo"),
                        Matricola = row.Field<string>("matricola") // Assumendo che sia un intero
                    }
                );

            // Prepara la DataTable per il salvataggio
            DataTable turniDaSalvare = new DataTable();
            turniDaSalvare.Columns.Add("matricola", typeof(int));
            turniDaSalvare.Columns.Add("nominativo", typeof(string));
            turniDaSalvare.Columns.Add("anno", typeof(int));
            turniDaSalvare.Columns.Add("mese", typeof(string));
            turniDaSalvare.Columns.Add("giorno", typeof(string));
            turniDaSalvare.Columns.Add("CodiceTurno", typeof(string));

            foreach (var dipendenteEntry in turniCompleti)
            {
                string idDipendenteStr = dipendenteEntry.Key;
                var turniDelDipendente = dipendenteEntry.Value;

                // Se non troviamo i dati del dipendente nella nostra mappa, saltiamo
                if (!dipendentiMap.ContainsKey(idDipendenteStr)) continue;

                var infoDipendente = dipendentiMap[idDipendenteStr];

                foreach (var turnoEntry in turniDelDipendente)
                {
                    int giorno = turnoEntry.Key;
                    string codiceTurno = turnoEntry.Value;

                    if (!string.IsNullOrEmpty(codiceTurno))
                    {
                        // Ora recuperiamo i dati dalla mappa in memoria, senza query al DB
                        turniDaSalvare.Rows.Add(infoDipendente.Matricola, infoDipendente.Nominativo, anno, txtmese, giorno.ToString(), codiceTurno);
                    }
                }
            }

            if (turniDaSalvare.Rows.Count > 0)
            {
                try
                {
                    //  la logica di ciclo è DENTRO il manager avendo utilizzato un merge
                    bool resp = mn.SalvaTurnoMensile(turniDaSalvare);
                    if (resp)
                    {
                        errorMessage.InnerText = "Inserimento turnazione mensile completato correttamente";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
                        //lblErrore.Text = "Salvataggio completato con successo!";
                        //lblErrore.ForeColor = Color.Red;
                        //    lblErrore.Font.Bold = true;
                        btnsalva.Enabled = false;
                        Session.Remove("TurniMensili");

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
                        sw.WriteLine(ex.Message + @" - Errore in salva/modifica turnazione mensile");
                        sw.Close();
                    }
                    string url = VirtualPathUtility.ToAbsolute("~/Contact.aspx?errore=");
                    Response.Redirect(url + ex.Message);

                    Session["MessaggioErrore"] = ex.Message;
                }
            }

        }
    }
}

