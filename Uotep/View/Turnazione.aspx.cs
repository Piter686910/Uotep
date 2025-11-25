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

            //// 1. Esegui la tua query e ottieni il DataTable (già ordinato per ufficio!)
            ////DataTable dt = CaricaDatiDalDatabase(); // Sostituisci con la tua funzione
            //Manager mn = new Manager();
            //DataTable dt = new DataTable();
            //dt = mn.getListDipendentire();
            //// Ottieni un riferimento alla tabella interna della GridView
            //Table gridTable = gvCalendario.Controls.OfType<Table>().FirstOrDefault();
            //if (gridTable == null)
            //{
            //    gridTable = new Table();
            //    gvCalendario.Controls.Add(gridTable);
            //}
            //gridTable.Rows.Clear(); // Pulisci la tabella da esecuzioni precedenti

            //// --- COSTRUZIONE MANUALE DELL'HEADER ---
            //GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            //// Aggiungi qui le celle dell'header una per una, basandoti sulle colonne che crei
            //// Esempio:
            //TableHeaderCell thDipendente = new TableHeaderCell { Text = "Dipendente" };
            //headerRow.Cells.Add(thDipendente);

            //// Aggiungi le celle per i giorni del mese (devi avere questa logica già pronta)
            //int anno = int.Parse(ddlAnno.SelectedValue);
            //int mese = int.Parse(ddlMese.SelectedValue);
            //int giorniNelMese = DateTime.DaysInMonth(anno, mese);
            //for (int i = 1; i <= giorniNelMese; i++)
            //{
            //    TableHeaderCell thGiorno = new TableHeaderCell { Text = i.ToString() };
            //    // Aggiungi qui la logica per il nome del giorno e lo stile per i festivi
            //    headerRow.Cells.Add(thGiorno);
            //}

            //TableHeaderCell thAzioni = new TableHeaderCell { Text = "Azioni" };
            //headerRow.Cells.Add(thAzioni);

            //gridTable.Rows.Add(headerRow); // Aggiungi la riga di header alla tabella


            //// --- COSTRUZIONE MANUALE DELLE RIGHE DI DATI E SEPARATORI ---
            //string _currentUfficio = null;

            //foreach (DataRow dataRow in dt.Rows)
            //{
            //    string ufficio = dataRow["ufficio"].ToString();

            //    // Se l'ufficio cambia, inserisci la riga separatore
            //    if (ufficio != _currentUfficio)
            //    {
            //        GridViewRow separatorRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            //        TableCell cell = new TableCell();
            //        cell.ColumnSpan = headerRow.Cells.Count; // Occupa tutta la larghezza
            //        cell.CssClass = "ufficio-separator-row"; // Il tuo stile
            //        cell.Text = ufficio;
            //        separatorRow.Cells.Add(cell);
            //        gridTable.Rows.Add(separatorRow);

            //        _currentUfficio = ufficio;
            //    }

            //    // Inserisci la riga del dipendente
            //    GridViewRow employeeRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);

            //    // Cella Nominativo
            //    TableCell cellNominativo = new TableCell { Text = dataRow["nominativo"].ToString() };
            //    employeeRow.Cells.Add(cellNominativo);

            //    // Aggiungi le celle per i dati dei giorni (es. turni)
            //    for (int i = 1; i <= giorniNelMese; i++)
            //    {
            //        TableCell cellTurno = new TableCell { Text = "" }; // Metti qui il dato del turno
            //                                                           // Applica lo stile festivo se necessario
            //        employeeRow.Cells.Add(cellTurno);
            //    }

            //    // Cella Azioni (con il bottone "Modifica")
            //    TableCell cellAzioni = new TableCell();
            //    Button btnModifica = new Button { Text = "Modifica", CommandName = "Edit" };
            //    cellAzioni.Controls.Add(btnModifica);
            //    employeeRow.Cells.Add(cellAzioni);

            //    gridTable.Rows.Add(employeeRow);
            //    //CaricaGriglia();
            //}
        }
        //private Dictionary<int, string> CaricaDatiGiorniSpeciali(int anno)
        //{
        //    Manager mn = new Manager();
        //    DataTable quartina = new DataTable();
        //    quartina = mn.getListQuartina(anno);

        //    // Convertiamo il risultato della query in un Dictionary per un accesso rapido
        //    Dictionary<int, string> risultato = new Dictionary<int, string>();
        //    foreach (var item in quartina.Rows)
        //    {
        //        String giorno = item["Gennaio"];
        //        string valore = item["Valore"].ToString();

        //        // Aggiungi la voce al dizionario. Se ci sono più valori per lo stesso giorno,
        //        // questo codice terrà solo l'ultimo.
        //        risultato[giorno] = valore;
        //    }

        //    return risultato;
        //}
        /// <summary>
        /// Processa la DataTable dei gruppi e la trasforma in un dizionario ottimizzato per la ricerca.
        /// </summary>
        /// <param name="dtGruppi">La DataTable contenente i dati dei gruppi.</param>
        /// <param name="mese">Il mese (1-12) da analizzare.</param>
        /// <returns>Un Dictionary dove la chiave è il giorno e il valore è una lista di numeri di gruppo.</returns>
        //private Dictionary<int, List<string>> ProcessaDataTableGruppi(DataTable dtGruppi, int mese)
        //{
        //    var giorniGruppi = new Dictionary<int, List<string>>();
        //    if (dtGruppi == null || dtGruppi.Rows.Count == 0)
        //    {
        //        return giorniGruppi;
        //    }

        //    // Ottieni il nome della colonna del mese in italiano
        //    string nomeColonnaMese = new DateTime(2000, mese, 1).ToString("MMMM", new CultureInfo("it-IT")).ToLower();

        //    // Controlla se la colonna esiste nella DataTable
        //    if (!dtGruppi.Columns.Contains(nomeColonnaMese))
        //    {
        //        return giorniGruppi; // Colonna non trovata, restituisci dizionario vuoto
        //    }

        //    // Itera su ogni riga della DataTable (ogni riga è un gruppo)
        //    foreach (DataRow row in dtGruppi.Rows)
        //    {
        //        string numeroGruppo = row["quartina"].ToString();
        //        string giorniStringa = row[nomeColonnaMese]?.ToString();

        //        // Se la cella del mese è vuota o null, passa al gruppo successivo
        //        if (string.IsNullOrWhiteSpace(giorniStringa))
        //        {
        //            continue;
        //        }

        //        // Dividi la stringa dei giorni (es. "1,10,25") in un array di giorni
        //        string[] giorniSeparati = giorniStringa.Split(',');

        //        foreach (string giornoStr in giorniSeparati)
        //        {
        //            // Tenta di convertire ogni pezzo in un numero.
        //            // Questo ignora eventuali spazi vuoti o valori non validi (es. "1,,15")
        //            if (int.TryParse(giornoStr.Trim(), out int giorno))
        //            {
        //                // Se non abbiamo ancora una voce per questo giorno, creiamola
        //                if (!giorniGruppi.ContainsKey(giorno))
        //                {
        //                    giorniGruppi[giorno] = new List<string>();
        //                }
        //                // Aggiungi il numero del gruppo corrente alla lista di quel giorno
        //                giorniGruppi[giorno].Add(numeroGruppo);
        //            }
        //        }
        //    }
        //    return giorniGruppi;
        //}
        private Dictionary<int, List<string>> ProcessaDataTableGruppi(DataTable dtGruppi, int mese)
        {
            var giorniGruppi = new Dictionary<int, List<string>>();

            // --- INIZIO SEZIONE DI DEBUG ---
            string debugOutput = "";
            if (dtGruppi == null || dtGruppi.Rows.Count == 0)
            {
                debugOutput = "DEBUG: La DataTable dei gruppi è vuota o null.";
                ltlDebug.Text = $"<div class='alert alert-danger'>{debugOutput}</div>";
                return giorniGruppi;
            }

            string nomeColonnaMeseAtteso = new DateTime(2000, mese, 1).ToString("MMMM", new CultureInfo("it-IT")).ToLower();

            debugOutput += $"<b>Nome Colonna Atteso:</b> '{nomeColonnaMeseAtteso}'<br/>";

            // Ottieni e stampa la lista di tutte le colonne reali nella DataTable
            string colonneReali = string.Join(", ", dtGruppi.Columns.Cast<DataColumn>().Select(c => $"'{c.ColumnName}'"));
            debugOutput += $"<b>Lista Colonne Reali:</b> {colonneReali}<br/><hr/>";

            // --- FINE SEZIONE DI DEBUG ---

            // Usiamo una ricerca case-insensitive per trovare la colonna
            DataColumn colonnaMese = dtGruppi.Columns
                .Cast<DataColumn>()
                .FirstOrDefault(c => c.ColumnName.Equals(nomeColonnaMeseAtteso, StringComparison.OrdinalIgnoreCase));

            if (colonnaMese == null)
            {
                debugOutput += "<b style='color:red;'>ERRORE: La colonna del mese non è stata trovata nella DataTable!</b>";
                ltlDebug.Text = $"<div class='alert alert-warning'>{debugOutput}</div>";
                return giorniGruppi;
            }

            foreach (DataRow row in dtGruppi.Rows)
            {
                string numeroGruppo = row["quartina"].ToString();
                string giorniStringa = row[colonnaMese]?.ToString();

                if (string.IsNullOrWhiteSpace(giorniStringa))
                {
                    continue;
                }

                debugOutput += $"Trovati dati per Gruppo '{numeroGruppo}': '{giorniStringa}'<br/>";

                string[] giorniSeparati = giorniStringa.Split(',');

                foreach (string giornoStr in giorniSeparati)
                {
                    if (int.TryParse(giornoStr.Trim(), out int giorno))
                    {
                        if (!giorniGruppi.ContainsKey(giorno))
                        {
                            giorniGruppi[giorno] = new List<string>();
                        }
                        giorniGruppi[giorno].Add(numeroGruppo);
                    }
                }
            }

            // Mostra il risultato del debug
            ltlDebug.Text = $"<div class='alert alert-info'>{debugOutput}</div>";
            return giorniGruppi;
        }
        private void GeneraGriglia()
        {


            Manager mn = new Manager();

            // Carica la DataTable dei dipendenti una sola volta
            DataTable dt = mn.getListDipendentire();

            Table gridTable = gvCalendario.Controls.OfType<Table>().FirstOrDefault();
            if (gridTable == null)
            {
                gridTable = new Table();
                gvCalendario.Controls.Add(gridTable);
            }
            gridTable.Rows.Clear();

            // --- Logica Anno e Mese ---
            int anno = System.Convert.ToInt32(DateTime.Now.Year);
            if (!string.IsNullOrEmpty(txtAnno.Text))
            {
                anno = int.Parse(txtAnno.Text);
            }
            else
                txtAnno.Text = anno.ToString();

            int mese = int.Parse(ddlMese.SelectedValue);
            int giorniNelMese = DateTime.DaysInMonth(anno, mese);

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // PASSO 1: CARICA E PREPARA I DATI DELLA RIGA "QUARTINA"
            DataTable quartina = mn.getListQuartina(anno);
            Dictionary<int, List<string>> datiGiorniGruppi = ProcessaDataTableGruppi(quartina, mese);

            // PASSO 2: COSTRUISCI E AGGIUNGI LA RIGA "QUARTINA"
            GridViewRow specialRow = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
            specialRow.CssClass = "riga-eventi-speciale";

            // **** MODIFICA RICHIESTA: Aggiungi il testo "quartina" alla prima cella ****
            TableCell quartinaCell = new TableCell();
            quartinaCell.Text = "Quartina";
            quartinaCell.Font.Bold = true; // Applica lo stile Grassetto (Bold)
            specialRow.Cells.Add(quartinaCell);

            // Popola le altre celle con i dati dei gruppi
            for (int i = 1; i <= giorniNelMese; i++)
            {
                TableCell cell = new TableCell();
                if (datiGiorniGruppi.ContainsKey(i))
                {
                    cell.Text = string.Join(", ", datiGiorniGruppi[i]);
                }
                specialRow.Cells.Add(cell);
            }

            // Aggiungi cella vuota per la colonna Azioni
            specialRow.Cells.Add(new TableCell());

            gridTable.Rows.Add(specialRow);
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // --- COSTRUZIONE HEADER PRINCIPALE ---
            GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            headerRow.Cells.Add(new TableHeaderCell { Text = "Dipendente" });

            for (int i = 1; i <= giorniNelMese; i++)
            {
                DateTime currentDate = new DateTime(anno, mese, i);
                TableHeaderCell thGiorno = new TableHeaderCell();
                thGiorno.Text = $"<small>{currentDate:ddd}</small><br/>{i}";

                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    thGiorno.CssClass = "giorno-festivo-header text-center";
                }
                else
                {
                    thGiorno.CssClass = "text-center";
                }
                headerRow.Cells.Add(thGiorno);
            }
            headerRow.Cells.Add(new TableHeaderCell { Text = "Azioni" });
            gridTable.Rows.Add(headerRow);

            // --- COSTRUZIONE RIGHE DATI E SEPARATORI UFFICIO ---
            string _currentUfficio = null;
            int visualRowIndex = 0;

            foreach (DataRow dataRow in dt.Rows)
            {
                string ufficio = dataRow["ufficio"].ToString().ToUpper();
                string idDipendente = dataRow["id_dip"].ToString();

                // Inserisci riga separatore ufficio
                if (ufficio != _currentUfficio)
                {
                    GridViewRow separatorRow = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                    TableCell cell = new TableCell { Text = ufficio, ColumnSpan = headerRow.Cells.Count, CssClass = "ufficio-separator-row" };
                    separatorRow.Cells.Add(cell);
                    separatorRow.Font.Bold = true;
                    separatorRow.ForeColor = Color.Red;
                    gridTable.Rows.Add(separatorRow);
                    _currentUfficio = ufficio;
                }

                // Costruisci riga dipendente
                bool isEditMode = (gvCalendario.EditIndex == visualRowIndex);
                GridViewRow employeeRow = new GridViewRow(visualRowIndex, visualRowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

                employeeRow.Cells.Add(new TableCell { Text = dataRow["nominativo"].ToString() });
                employeeRow.Font.Bold = true;

                for (int i = 1; i <= giorniNelMese; i++)
                {
                    TableCell cellTurno = new TableCell();
                    DateTime currentDate = new DateTime(anno, mese, i);

                    if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        cellTurno.CssClass = "giorno-festivo-cella";
                    }

                    if (isEditMode)
                    {
                        TextBox txtTurno = new TextBox();
                        txtTurno.ID = "txtGiorno_" + i;
                        txtTurno.Text = ""; // Popola con dati turno
                        txtTurno.CssClass = "form-control input-sm";
                        cellTurno.Controls.Add(txtTurno);
                    }
                    else
                    {
                        cellTurno.Text = ""; // Popola con dati turno
                    }
                    employeeRow.Cells.Add(cellTurno);
                }

                // Cella Azioni
                TableCell cellAzioni = new TableCell();
                if (isEditMode)
                {
                    cellAzioni.Controls.Add(new Button { Text = "Aggiorna", CommandName = "Update", CommandArgument = idDipendente, CssClass = "btn btn-success btn-xs" });
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

