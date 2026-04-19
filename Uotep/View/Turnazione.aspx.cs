using AjaxControlToolkit.HtmlEditor.Popups;
using ClosedXML.Excel;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Ajax.Utilities;
using Microsoft.Office.Interop.Word;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uote;
using Uotep.Classi;
using WebGrease.Activities;
using static Uotep.Classi.Enumerate;
using Cell = iText.Layout.Element.Cell;
using Color = iText.Kernel.Colors.Color;
using DataTable = System.Data.DataTable;
using Document = iText.Layout.Document;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = System.Web.UI.WebControls.Table;




namespace Uotep
{
    public class RegolaRSNL
    {
        public string Gruppo { get; set; }
        public DateTime? DataRS { get; set; }
        public DateTime? DataNL { get; set; }
        public int Quartina { get; set; }
        public string Mese { get; set; }
    }
    public partial class Turnazione : System.Web.UI.Page
    {
        String annoCorr = DateTime.Now.Year.ToString();
        String Vuser = String.Empty;
        String ruolo = String.Empty;
        String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        String FileCalendarioRSNL = ConfigurationManager.AppSettings["CartellaFureria"];
        //bool isEditMode = false;
        // Dizionario per annotare DOVE inserire le intestazioni.
        // La chiave (int) è l'indice della riga, il valore (string) è il nome dell'ufficio.
        private Dictionary<int, string> _headerRowsToInsert = new Dictionary<int, string>();
       // private string _currentUfficio = null;

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
                // Legge il valore dal Web.config
                string protocolloText = ConfigurationManager.AppSettings["Titolo"];

                // Decodifica il contenuto HTML (per supportare tag HTML come <h2>)
                string decodedText = HttpUtility.HtmlDecode(protocolloText);

                // Assegna il valore decodificato al Literal
                //ProtocolloLiteral.Text = decodedText;
                txtAnno.Text = System.Convert.ToInt32(DateTime.Now.Year).ToString();
                if (ruolo != "fureria" && ruolo !="admin")
                {
                    btImportaMatriceExcel.Visible = false;
                    btnExportPdf.Visible = false;
                    btnExportExcel.Visible = false;
                    btnsalva.Visible = false;
                    btnCarica.Visible = false;
                }
            }
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
       
        private void RecalcolaPercentuali(List<DipendenteTurno> lista, int giorniMese)
        {
            foreach (var dip in lista)
            {
                int c1 = 0, c2 = 0;
                for (int i = 1; i <= giorniMese; i++)
                {
                    if (dip.TurniMensili[i] == "1") c1++;
                    else if (dip.TurniMensili[i] == "2") c2++;
                }
                int tot = c1 + c2;
                if (tot > 0) dip.StatisticaPerc = ((double)c1 / tot * 100).ToString("0") + "%";
                else dip.StatisticaPerc = "N/A";
            }
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

        protected void btnCarica_Click(object sender, EventArgs e)
        {
           // lblError.Text = "⏳ btn click";
            // Rimuovo la cache vecchia per forzare il ricalcolo dentro GeneraGriglia
            Session.Remove("ListaDipendentiTurni");
            int anno = int.Parse(txtAnno.Text);
            int mese = int.Parse(ddlMese.SelectedValue);
          //  lblError.Text = "⏳ Caricamento dati...list dip";
            Manager mn = new Manager();
            DataTable dtDip = mn.getListDipendenti();
            //var listaDipendenti = MappaDaDataTable(dt, mese);
            // DataTable delle quartine (ID e colonne mesi)
            DataTable dtQuartine = mn.getListQuartina(anno);
            // 3. Applico l'algoritmo (Q, Sabati, Regola 60/40)
            // CalcolaLogicaTurni(listaDipendenti, anno, mese);
            // 2. CREO UNA MAPPA DI QUARTINE (IdQuartina -> Stringa Giorni)
            // Questo serve per non ciclare la tabella quartine per ogni dipendente
            Dictionary<int, string> mappaGiorniQuartina = CostruisciMappaQuartine(dtQuartine, mese);


            // 3. MAPPO E CALCOLO I TURNI
           // lblError.Text = "⏳ Caricamento dati...elabora";
            List<DipendenteTurno> listaDipendenti = ElaboraDati(dtDip, mappaGiorniQuartina, anno, mese);
            Session["ListaDipendentiTurni"] = listaDipendenti;
            // 4. GENERO L'HTML (Usando il metodo grafico fatto prima)
            //lblError.Text = "⏳ Caricamento html";
            GeneraHtml(listaDipendenti, anno, mese);

            //btnsalva.Enabled = true;

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

        private Dictionary<int, string> CostruisciMappaQuartine(DataTable dt, int meseInt)
        {
            var mappa = new Dictionary<int, string>();

            // Array per convertire numero mese in nome colonna
            string[] nomiMesi = { "", "gennaio", "febbraio", "marzo", "aprile", "maggio", "giugno", "luglio", "agosto", "settembre", "ottobre", "novembre", "dicembre" };
            string nomeColonna = nomiMesi[meseInt]; // es: "marzo"

            // Controlla se la colonna esiste
            if (!dt.Columns.Contains(nomeColonna)) return mappa;

            foreach (DataRow row in dt.Rows)
            {
                if (row["quartina"] != DBNull.Value)
                {
                    int idQ = Convert.ToInt32(row["quartina"]);
                    string giorni = row[nomeColonna] != DBNull.Value ? row[nomeColonna].ToString() : "";

                    if (!mappa.ContainsKey(idQ))
                    {
                        mappa.Add(idQ, giorni);
                    }
                }
            }
            return mappa;
        }
        protected void btGetTurnoMensile_Click(object sender, EventArgs e)
        {
            int anno = Convert.ToInt32(txtAnno.Text);
            int mese = Convert.ToInt32(ddlMese.SelectedValue);
            Manager mn = new Manager();
            // 1. Recupera i dati dal DB (Unione tra Anagrafica e Turni Salvati)
            List<DipendenteTurno> listaDalDB = mn.GetTurniMensile(anno, ddlMese.SelectedItem.Text);
           // verifico che l'array turni mensili abbia almeno un elemento valido da mostrare
            bool esisteElementoValido = listaDalDB[0].TurniMensili != null && listaDalDB[0].TurniMensili.Any(x => x != null);

            if (!esisteElementoValido)
            {
                // lblError.Text = "⚠️ Nessun turno trovato nel database per questo periodo.";
                // lblError.ForeColor = System.Drawing.Color.Orange;
                errorMessage.InnerText = @"⚠️ Nessun turno trovato nel database per questo periodo.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
                ltlTabella.Text = ""; // Pulisce la tabella
                return;
            }

            // 2. Calcola le percentuali in base ai dati caricati
            // (Fondamentale perché nel DB salviamo solo "1" o "2", non la %)
            RecalcolaPercentuali(listaDalDB, DateTime.DaysInMonth(anno, mese));

            // 3. Genera l'HTML (usa la stessa funzione di prima, così sono modificabili!)
            GeneraHtml(listaDalDB, anno, mese);

            lblError.Text = "📂 Dati caricati dal Database.";
            lblError.ForeColor = System.Drawing.Color.Blue;


            // 3. MAPPO E CALCOLO I TURNI
            Session["ListaDipendentiTurni"] = listaDalDB;

        }
        private void GeneraHtml(List<DipendenteTurno> lista, int anno, int mese)
        {
            int giorniMese = DateTime.DaysInMonth(anno, mese); // Gestione bisestile automatica
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class='tabella-turni'>");

            // --- HEADER DELLA TABELLA ---
            sb.Append("<thead><tr>");
            sb.Append("<th class='col-dip-header'>DIPENDENTE</th>");
            sb.Append("<th class='col-stats-header'>%1°/2°</th>");

            for (int i = 1; i <= giorniMese; i++)
            {
                DateTime dt = new DateTime(anno, mese, i);
                bool isWeekend = (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);
                string classHeader = isWeekend ? "giorno-header weekend-h" : "giorno-header";
                string lettera = dt.ToString("ddd", new System.Globalization.CultureInfo("it-IT")).Substring(0, 1).ToUpper();
                sb.AppendFormat("<th class='{0}'>{1}<br/><small>{2}</small></th>", classHeader, i, lettera);
            }
            sb.Append("</tr></thead><tbody>");

            // --- BODY: 1° LIVELLO RAGGRUPPAMENTO (UFFICIO) ---
            var gruppiUfficio = lista.GroupBy(x => x.Ufficio).OrderBy(k => k.Key);

            foreach (var gu in gruppiUfficio)
            {
                string nomeUfficio = gu.Key.ToUpper().Trim();

                // RIGA INTESTAZIONE UFFICIO
                sb.AppendFormat("<tr class='tr-ufficio'><td colspan='{0}'>{1}</td></tr>",
                    giorniMese + 2, nomeUfficio);

                if (nomeUfficio.StartsWith("MACRO"))
                {
                    // --- 2° LIVELLO RAGGRUPPAMENTO PER AREA (UOTE / UOTP) ---
                    var gruppiArea = gu.GroupBy(d => string.IsNullOrEmpty(d.Area) ? "NON DEFINITA" : d.Area.ToUpper().Trim())
                                       .OrderBy(k => k.Key); // Ordina UOTE, poi UOTP

                    foreach (var ga in gruppiArea)
                    {
                        // RIGA DIVISORE AREA
                        sb.AppendFormat("<tr class='tr-area-divisore'><td colspan='{0}' style='background-color: #e9ecef; color: #495057; font-weight: bold; padding-left: 25px; border-left: 5px solid #007bff;'>{1}</td></tr>",
                            giorniMese + 2, ga.Key);

                        foreach (var dip in ga)
                        {
                            GeneraRigaDipendente(sb, dip, giorniMese, anno, mese);
                        }
                    }
                }
                else
                {
                    // UFFICI NON MACRO (CDR, GIRO, etc.) - Elenco semplice
                    foreach (var dip in gu)
                    {
                        GeneraRigaDipendente(sb, dip, giorniMese, anno, mese);
                    }
                }
            }

            sb.Append("</tbody></table>");
            ltlTabella.Text = sb.ToString();
        }

        private void GeneraRigaDipendente(StringBuilder sb, DipendenteTurno dip, int giorniMese, int anno, int mese)
        {
            sb.Append("<tr>");
            // NOME
            sb.AppendFormat("<td class='col-dipendente' title='{0}'>{1}<span class='badge-q'>Q{2}</span><span class='badge-q'>Gr.{3}</span></td>",
                dip.Nominativo, dip.Nominativo, dip.QuartinaID, dip.Gruppo);

            // STATISTICA
            string valPerc = string.IsNullOrEmpty(dip.StatisticaPerc) ? "0" : dip.StatisticaPerc.Replace("%", "");
            string styleColor = (int.TryParse(valPerc, out int p) && p > 60) ? "style='color:red;'" : "style='color:green;'";
            sb.AppendFormat("<td class='col-stats' {0}>{1}</td>", styleColor, dip.StatisticaPerc);

            // CELLE GIORNI
            for (int i = 1; i <= giorniMese; i++)
            {
                string val = (dip.TurniMensili != null && i < dip.TurniMensili.Length) ? dip.TurniMensili[i] : "";
                DateTime dt = new DateTime(anno, mese, i);
                bool isWeekend = (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);

                string cssClass = isWeekend ? "weekend-col" : "";
                if (val == "Q") cssClass += " t-q";
                else if (val == "1") cssClass += " t-1";
                else if (val == "2") cssClass += " t-2";
                else if (val == "RF") cssClass += " t-rf";
                else if (val == "RS") cssClass += " t-rs";
                else if (val == "NL") cssClass += " t-nl";

                // Dati puliti
                string matricolaClean = dip.Matricola.Trim();
                string areaStr = string.IsNullOrEmpty(dip.Area) ? "NESSUNA" : dip.Area.Trim().ToUpper();
                string isAutistaStr = dip.IsAutista.ToString().ToLower(); // "true" o "false"
                string ufficioStr = dip.Ufficio.Trim().ToUpper();
                string inputId = $"T_{matricolaClean}_{i}";

                // COSTRUZIONE INPUT MANUALE E PULITA
                // Nota come onchange chiama window.GestisciCambioTurnoJS
                sb.AppendFormat("<td class='{0}' style='padding:0;'>", cssClass);
                sb.Append("<input type='text' ");
                sb.AppendFormat("id='{0}' name='{0}' value='{1}' ", inputId, val);
                sb.Append("class='shift-input' maxlength='4' autocomplete='off' ");

                // Data Attributes
                sb.AppendFormat("data-matricola='{0}' ", matricolaClean);
                sb.AppendFormat("data-ufficio='{0}' ", ufficioStr);
                sb.AppendFormat("data-area='{0}' ", areaStr);
                sb.AppendFormat("data-autista='{0}' ", isAutistaStr);
                sb.AppendFormat("data-giorno='{0}' ", i);

                // Evento JS (Fondamentale)
                sb.Append("onchange='window.GestisciCambioTurnoJS(this)' ");

                sb.Append("/>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
        }
        // --- LOGICA UFFICI NUMEROSI AVANZATA ---
        // Target: 50% Turno 1. Max: 60%. Min 2 dipendenti per turno.
        private void RiempimentoUfficioMultiplo(List<DipendenteTurno> gruppo, int giorniMese)
        {
            for (int i = 1; i <= giorniMese; i++)
            {
                // 1. STATO ATTUALE (Chi è bloccato da Q, Sabati, RF?)
                int count1 = 0;
                int count2 = 0;
                List<DipendenteTurno> liberi = new List<DipendenteTurno>();

                foreach (var dip in gruppo)
                {
                    string t = dip.TurniMensili[i];
                    if (t == "1") count1++;
                    else if (t == "2") count2++;
                    else if (t == null) liberi.Add(dip);
                }

                // 2. FABBISOGNO (Minimo 2 per turno)
                int missing1 = Math.Max(0, 2 - count1);
                int missing2 = Math.Max(0, 2 - count2);

                // 3. ANALISI DEI VINCOLI CONSECUTIVI 
                var obbligatiA1 = new List<DipendenteTurno>();
                var obbligatiA2 = new List<DipendenteTurno>();
                var flessibili = new List<DipendenteTurno>();

                foreach (var dip in liberi)
                {
                    string p1 = (i > 1) ? dip.TurniMensili[i - 1] : "";
                    string p2 = (i > 2) ? dip.TurniMensili[i - 2] : "";

                    bool no1 = (p1 == "1" && p2 == "1"); // Vietato 1
                    bool no2 = (p1 == "2" && p2 == "2"); // Vietato 2

                    if (no1 && !no2) obbligatiA2.Add(dip);
                    else if (no2 && !no1) obbligatiA1.Add(dip);
                    else if (no1 && no2) flessibili.Add(dip); // Caso raro, mettiamo flessibile
                    else flessibili.Add(dip); // Jolly: può fare tutto
                }

                // 4. ASSEGNAZIONE OBBLIGATA (Priorità massima per non rompere i turni)
                foreach (var dip in obbligatiA1) { dip.TurniMensili[i] = "1"; if (missing1 > 0) missing1--; }
                foreach (var dip in obbligatiA2) { dip.TurniMensili[i] = "2"; if (missing2 > 0) missing2--; }

                // 5. ASSEGNAZIONE FLESSIBILI BILANCIATA (Il cuore della modifica)

                // Creiamo una lista temporanea con la % attuale di ognuno per poter ordinare
                var candidati = flessibili.Select(d => new
                {
                    Dip = d,
                    Perc1 = GetPercTurno1Attuale(d.TurniMensili, i)
                }).ToList();

                // A. Copriamo i buchi del Turno 2 (missing2)
                // Chi scegliamo? Chi ha la % di "1" PIÙ ALTA (sopra il 60% o 50%), così gli diamo un "2" e scende.
                while (missing2 > 0 && candidati.Count > 0)
                {
                    // Ordiniamo Decrescente: chi ha 70% di "1" va in cima alla lista
                    var scelto = candidati.OrderByDescending(x => x.Perc1).First();

                    scelto.Dip.TurniMensili[i] = "2";
                    candidati.Remove(scelto);
                    missing2--;
                }

                // B. Copriamo i buchi del Turno 1 (missing1)
                // Chi scegliamo? Chi ha la % di "1" PIÙ BASSA, così gli diamo un "1" e sale.
                while (missing1 > 0 && candidati.Count > 0)
                {
                    // Ordiniamo Crescente: chi ha 30% di "1" va in cima alla lista
                    var scelto = candidati.OrderBy(x => x.Perc1).First();

                    scelto.Dip.TurniMensili[i] = "1";
                    candidati.Remove(scelto);
                    missing1--;
                }

                // C. Assegnazione Eccedenze (Target 50%)
                // I minimi sono coperti, rimangono dipendenti extra. Assegniamo per bilanciare la loro media.
                foreach (var item in candidati)
                {
                    // Se hai più del 50% di "1", ti do "2".
                    // Se hai meno del 50% di "1", ti do "1".
                    // Questo rispetta rigorosamente il limite del 60%.
                    if (item.Perc1 > 50.0)
                    {
                        item.Dip.TurniMensili[i] = "2";
                    }
                    else
                    {
                        item.Dip.TurniMensili[i] = "1";
                    }
                }
            }
        }
        private List<DipendenteTurno> ElaboraDati(DataTable dtDip, Dictionary<int, string> mappaQuartine, int anno, int mese)
        {
            var lista = new List<DipendenteTurno>();
            int giorniMese = DateTime.DaysInMonth(anno, mese);
            string meseStringa = new DateTime(anno, mese, 1)
                .ToString("MMMM", new System.Globalization.CultureInfo("it-IT"))
                .Substring(0, 3)
                .ToUpper();

            // Caricamento Regole RSNL dal Database
            List<RegolaRSNL> regoleRSNL = CaricaRegoleRSNL(anno, mese);

            // --- PRIMA PASSATA: Creazione e Vincoli Assoluti ---
            foreach (DataRow row in dtDip.Rows)
            {
                var dip = new DipendenteTurno();
                dip.Nominativo = row["nominativo"].ToString();
                dip.Ufficio = row["ufficio"].ToString();
                dip.Matricola = row["matricola_ced"].ToString(); // Attenzione: verifica se usare matricola o matricola_ced
                dip.Gruppo = row["gruppo_quartina"].ToString();
                dip.TurniMensili = new string[32];
                dip.Area = row["area"].ToString();

                // LETTURA AUTISTA
                if (row.Table.Columns.Contains("autista") && row["autista"] != DBNull.Value)
                {
                    dip.IsAutista = Convert.ToBoolean(row["autista"]);
                }
                else
                {
                    dip.IsAutista = false; // Default
                }

                int idQuartina = (row.Table.Columns.Contains("quartina") && row["quartina"] != DBNull.Value)
                                    ? Convert.ToInt32(row["quartina"]) : 0;
                dip.QuartinaID = idQuartina;

                string stringaGiorni = mappaQuartine.ContainsKey(idQuartina) ? mappaQuartine[idQuartina] : "";
                dip.TurniMensili = new string[giorniMese + 1];

                // --- APPLICAZIONE REGOLE VINCOLANTI ---

                // 1. Quartine (Q)
                List<int> giorniQ = ApplicaRegolaQ(dip.TurniMensili, stringaGiorni, giorniMese);

                // 2. Sabati Ancorati
                ApplicaRegolaSabati(dip.TurniMensili, giorniQ, giorniMese, anno, mese);

                // 3. Regole Speciali (RS/NL)
                ApplicaRegolaRSNL(dip, regoleRSNL, giorniMese, anno, mese, meseStringa);

                // 4. Festivi (RF)
                ApplicaRegolaFestivi(dip.TurniMensili, giorniMese, anno, mese);

                // 5. NUOVA REGOLA: PREFERENZE (turni_pref)
                // La applichiamo qui in modo che rispetti Q e RF già impostati
                ApplicaRegolaPreferenze(dip, row, giorniMese, anno, mese);

                lista.Add(dip);
            }

            // --- SECONDA PASSATA: Riempimento per Gruppi ---
            var gruppiUfficio = lista.GroupBy(d => d.Ufficio).ToList();

            foreach (var gruppo in gruppiUfficio)
            {
                string nomeUfficio = gruppo.Key.ToUpper().Trim();
                List<DipendenteTurno> dipsDelGruppo = gruppo.ToList();

                if (dipsDelGruppo.Count == 1)
                {
                    RiempimentoUfficioSingolo(dipsDelGruppo[0], giorniMese);
                }
                else if (nomeUfficio == "CDR")
                {
                    RiempimentoUfficioCDR(dipsDelGruppo, giorniMese);
                }
                else if ((nomeUfficio == "GIRO" || nomeUfficio == "NOTIFICHE") && dipsDelGruppo.Count == 2)
                {
                    RiempimentoUfficioGemelli(dipsDelGruppo[0], dipsDelGruppo[1], giorniMese);
                }
                else if (nomeUfficio.StartsWith("MACRO"))
                {
                    RiempimentoUfficioConAutista(dipsDelGruppo, giorniMese);
                }
                else if (nomeUfficio == "FURERIA")
                {
                    RiempimentoUfficioFureria(dipsDelGruppo, giorniMese, anno, mese);
                }
                else if (dipsDelGruppo.Count == 2)
                {
                    RiempimentoUfficioCoppia(dipsDelGruppo[0], dipsDelGruppo[1], giorniMese);
                }
                else
                {
                    RiempimentoUfficioMultiplo(dipsDelGruppo, giorniMese);
                }
            }

            // --- TERZA PASSATA: CALCOLO STATISTICHE (%) ---
            foreach (var dip in lista)
            {
                int count1 = 0;
                int count2 = 0;

                for (int i = 1; i <= giorniMese; i++)
                {
                    string t = dip.TurniMensili[i];
                    if (t == "1") count1++;
                    else if (t == "2") count2++;
                }

                int totaleLavorati = count1 + count2;

                if (totaleLavorati > 0)
                {
                    double perc = (double)count1 / totaleLavorati * 100;
                    dip.StatisticaPerc = perc.ToString("0") + "%";
                }
                else
                {
                    dip.StatisticaPerc = "N/A";
                }
            }
            return lista;
        }
        private void ApplicaRegolaPreferenze(DipendenteTurno dip, DataRow row, int giorniMese, int anno, int mese)
        {
            // Verifica se la colonna esiste e se c'è un valore
            if (!row.Table.Columns.Contains("turni_pref") || row["turni_pref"] == DBNull.Value)
                return;

            string preferenze = row["turni_pref"].ToString().ToUpper();
            if (string.IsNullOrEmpty(preferenze)) return;

            // CultureInfo per ottenere i nomi dei giorni in Italiano
            var culture = new System.Globalization.CultureInfo("it-IT");

            for (int i = 1; i <= giorniMese; i++)
            {
                // Se il giorno è già bloccato da regole superiori (Riposo Q, Festivo RF, ecc.), saltiamo
                string turnoAttuale = dip.TurniMensili[i];
                if (turnoAttuale == "Q" || turnoAttuale == "RF" || turnoAttuale == "RS" || turnoAttuale == "NL")
                {
                    continue;
                }

                DateTime d = new DateTime(anno, mese, i);
                // Ottiene "LUN", "MAR", "MER", etc.
                string giornoAbbr = d.ToString("ddd", culture).ToUpper().Replace(".", "");

                // Controllo se la stringa preferenze contiene il giorno + turno (es. "LUN2")
                if (preferenze.Contains($"{giornoAbbr}2"))
                {
                    dip.TurniMensili[i] = "2";
                }
                else if (preferenze.Contains($"{giornoAbbr}1"))
                {
                    dip.TurniMensili[i] = "1";
                }
            }
        }
        private void ApplicaRegolaRSNL(DipendenteTurno dip, List<RegolaRSNL> regole, int giorniMese, int anno, int mese, string meseStringa)
        {
            // Filtro per Gruppo, Quartina e Prefisso Mese (es. "GEN")
            var regoleSoggetta = regole.Where(r =>
                r.Gruppo.Trim().Equals(dip.Gruppo.Trim(), StringComparison.OrdinalIgnoreCase) &&
                r.Quartina == dip.QuartinaID &&
                r.Mese.Trim().StartsWith(meseStringa, StringComparison.OrdinalIgnoreCase)

            ).ToList();
            if (regoleSoggetta.Count > 0)
            {


                if (regoleSoggetta[0].DataRS.HasValue)
                {
                    DateTime dtRS = regoleSoggetta[0].DataRS.Value;
                    int g = dtRS.Day;

                    if (g <= giorniMese && g < dip.TurniMensili.Length)
                    {
                        // CONTROLLO SABATO: dtRS.DayOfWeek == DayOfWeek.Saturday
                        if (dtRS.DayOfWeek == DayOfWeek.Saturday)
                        {
                            // Forzatura: Se è sabato, assegniamo RS comunque
                            dip.TurniMensili[g] = "RS";
                        }
                        else
                        {
                            // Per gli altri giorni, applichiamo solo se non è già ferie (Q)
                            if (dip.TurniMensili[g] != "Q")
                            {
                                dip.TurniMensili[g] = "RS";
                            }
                        }
                    }
                }

                // --- GESTIONE NL ---
                if (regoleSoggetta[0].DataNL.HasValue)
                {
                    DateTime dtNL = regoleSoggetta[0].DataNL.Value;
                    int g = dtNL.Day;

                    if (g <= giorniMese && g < dip.TurniMensili.Length)
                    {
                        // CONTROLLO SABATO: dtNL.DayOfWeek == DayOfWeek.Saturday
                        if (dtNL.DayOfWeek == DayOfWeek.Saturday)
                        {
                            // Forzatura: Se è sabato, assegniamo NL comunque
                            dip.TurniMensili[g] = "NL";
                        }
                        else
                        {
                            if (dip.TurniMensili[g] != "Q")
                            {
                                dip.TurniMensili[g] = "NL";
                            }
                        }
                    }
                }
            }
            //foreach (var regola in regoleSoggetta)
            //{
            //    // --- GESTIONE RS ---
            //    if (regola.DataRS.HasValue)
            //    {
            //        DateTime dtRS = regola.DataRS.Value;
            //        int g = dtRS.Day;

            //        if (g <= giorniMese && g < dip.TurniMensili.Length)
            //        {
            //            // CONTROLLO SABATO: dtRS.DayOfWeek == DayOfWeek.Saturday
            //            if (dtRS.DayOfWeek == DayOfWeek.Saturday)
            //            {
            //                // Forzatura: Se è sabato, assegniamo RS comunque
            //                dip.TurniMensili[g] = "RS";
            //            }
            //            else
            //            {
            //                // Per gli altri giorni, applichiamo solo se non è già ferie (Q)
            //                if (dip.TurniMensili[g] != "Q")
            //                {
            //                    dip.TurniMensili[g] = "RS";
            //                }
            //            }
            //        }
            //    }

            //    // --- GESTIONE NL ---
            //    if (regola.DataNL.HasValue)
            //    {
            //        DateTime dtNL = regola.DataNL.Value;
            //        int g = dtNL.Day;

            //        if (g <= giorniMese && g < dip.TurniMensili.Length)
            //        {
            //            // CONTROLLO SABATO: dtNL.DayOfWeek == DayOfWeek.Saturday
            //            if (dtNL.DayOfWeek == DayOfWeek.Saturday)
            //            {
            //                // Forzatura: Se è sabato, assegniamo NL comunque
            //                dip.TurniMensili[g] = "NL";
            //            }
            //            else
            //            {
            //                if (dip.TurniMensili[g] != "Q")
            //                {
            //                    dip.TurniMensili[g] = "NL";
            //                }
            //            }
            //        }
            //    }
            //}
        }
        private List<int> ApplicaRegolaQ(string[] turni, string stringaGiorni, int maxGiorni)
        {
            List<int> qIdx = new List<int>();
            if (!string.IsNullOrEmpty(stringaGiorni))
            {
                foreach (var p in stringaGiorni.Split(','))
                {
                    if (int.TryParse(p.Trim(), out int g) && g >= 1 && g <= maxGiorni)
                    {
                        turni[g] = "Q";
                        qIdx.Add(g);
                        if (g > 1 && turni[g - 1] == null) turni[g - 1] = "1";
                        if (g < maxGiorni && turni[g + 1] == null) turni[g + 1] = "2";
                    }
                }
            }
            qIdx.Sort();
            return qIdx;
        }

        private void ApplicaRegolaSabati(string[] turni, List<int> giorniQ, int maxGiorni, int anno, int mese)
        {
            List<int> sabati = new List<int>();
            for (int i = 1; i <= maxGiorni; i++)
            {
                if (new DateTime(anno, mese, i).DayOfWeek == DayOfWeek.Saturday) sabati.Add(i);
            }

            if (sabati.Count == 0) return;

            // Logica Anchor Q (Sabato prima=1, Sabato dopo=2)
            if (giorniQ.Count > 0)
            {
                int primaQ = giorniQ.First();
                int ultimaQ = giorniQ.Last();

                int sPre = sabati.Where(s => s < primaQ).LastOrDefault();
                if (sPre > 0 && turni[sPre] != "Q") turni[sPre] = "1";

                int sPost = sabati.Where(s => s > ultimaQ).FirstOrDefault();
                if (sPost > 0 && turni[sPost] != "Q") turni[sPost] = "2";
            }
            else
            {
                // Se non ci sono Q, inizia il primo sabato con 1 (o logica a piacere)
                if (turni[sabati[0]] == null) turni[sabati[0]] = "1";
            }

            // Propagazione alternata (Avanti e Indietro)
            bool modificato = true;
            while (modificato)
            {
                modificato = false;
                for (int i = 0; i < sabati.Count; i++)
                {
                    int oggi = sabati[i];
                    if (turni[oggi] == "Q") continue;

                    if (turni[oggi] == null)
                    {
                        // Guarda indietro
                        if (i > 0 && turni[sabati[i - 1]] != null && turni[sabati[i - 1]] != "Q")
                        {
                            turni[oggi] = (turni[sabati[i - 1]] == "1") ? "2" : "1";
                            modificato = true;
                        }
                        // Guarda avanti
                        else if (i < sabati.Count - 1 && turni[sabati[i + 1]] != null && turni[sabati[i + 1]] != "Q")
                        {
                            turni[oggi] = (turni[sabati[i + 1]] == "1") ? "2" : "1";
                            modificato = true;
                        }
                    }
                }
            }
        }

        private void ApplicaRegolaFestivi(string[] turni, int maxGiorni, int anno, int mese)
        {
            for (int i = 1; i <= maxGiorni; i++)
            {
                // Usa il tuo metodo IsGiornoFestivo creato prima
                if (IsGiornoFestivo(new DateTime(anno, mese, i)))
                {
                    if (turni[i] != "Q") turni[i] = "RF";
                }
            }
        }
        private bool IsGiornoFestivo(DateTime dt)
        {
            // 1. Festività Fisse
            if (dt.Day == 1 && dt.Month == 1) return true;   // Capodanno
            if (dt.Day == 6 && dt.Month == 1) return true;   // Epifania
            if (dt.Day == 25 && dt.Month == 4) return true;  // Liberazione
            if (dt.Day == 1 && dt.Month == 5) return true;   // Festa Lavoro
            if (dt.Day == 2 && dt.Month == 6) return true;   // Repubblica
            if (dt.Day == 15 && dt.Month == 8) return true;  // Ferragosto
            if (dt.Day == 1 && dt.Month == 11) return true;  // Ognissanti
            if (dt.Day == 8 && dt.Month == 12) return true;  // Immacolata
            if (dt.Day == 25 && dt.Month == 12) return true; // Natale
            if (dt.Day == 26 && dt.Month == 12) return true; // Santo Stefano

            // 2. Pasqua e Pasquetta
            int year = dt.Year;
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int iPasqua = c / 4; // rinominata da i a iPasqua
            int k = c % 4;
            int l = (32 + 2 * e + 2 * iPasqua - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;

            DateTime pasqua = new DateTime(year, month, day);
            DateTime pasquetta = pasqua.AddDays(1);

            if (dt.Date == pasqua.Date || dt.Date == pasquetta.Date) return true;

            // 3. Domenica
            if (dt.DayOfWeek == DayOfWeek.Sunday) return true;

            return false;
        }

        // Calcola la % di turni "1" fatti fino al giorno corrente (escluso)
        private double GetPercTurno1Attuale(string[] turni, int giornoCorrente)
        {
            int c1 = 0;
            int c2 = 0;
            // Guarda lo storico dall'inizio del mese fino a ieri
            for (int k = 1; k < giornoCorrente; k++)
            {
                if (turni[k] == "1") c1++;
                else if (turni[k] == "2") c2++;
            }

            int tot = c1 + c2;
            if (tot == 0) return 0.0; // Inizio mese, neutrale

            return (double)c1 / tot * 100.0; // Ritorna es. 55.0 per 55%
        }

        // --- LOGICA GEMELLI (Stesso Turno: 1-1 o 2-2) ---
        // Usata per uffici GIRO e NOTIFICHE
        private void RiempimentoUfficioGemelli(DipendenteTurno d1, DipendenteTurno d2, int giorniMese)
        {
            string[] t1 = d1.TurniMensili;
            string[] t2 = d2.TurniMensili;

            for (int i = 1; i <= giorniMese; i++)
            {
                // Se entrambi sono occupati (es. Domenica RF o entrambi Q), passa oltre
                if (t1[i] != null && t2[i] != null) continue;

                // 1. Determina il turno "Guida" per oggi.
                // Controlliamo se uno dei due ha già un vincolo lavorativo (1 o 2)
                // Esempio: Il Sabato ancorato di D1 era "1" -> Allora anche D2 deve fare "1"
                string turnoTarget = null;

                if (t1[i] == "1" || t2[i] == "1") turnoTarget = "1";
                else if (t1[i] == "2" || t2[i] == "2") turnoTarget = "2";

                // 2. Se nessuno ha vincoli, calcoliamo il turno ideale standard
                // Basandoci sullo storico di D1 per evitare 3 consecutivi
                if (turnoTarget == null)
                {
                    turnoTarget = CalcolaTurnoStandard(t1, i, true); // true = usa ratio equilibrato
                }

                // 3. Applica il turno a chi è libero
                // Se D1 è libero (non ha Q o RF), prende il turno target
                if (t1[i] == null) t1[i] = turnoTarget;

                // Se D2 è libero (non ha Q o RF), prende LO STESSO turno target
                if (t2[i] == null) t2[i] = turnoTarget;
            }
        }



        /// <summary>
        /// caloclo turni alternati per l'ufficio con u solo dipendente
        /// </summary>
        /// <param name="dip"></param>
        /// <param name="giorniMese"></param>
        private void RiempimentoUfficioSingolo(DipendenteTurno dip, int giorniMese)
        {
            string[] t = dip.TurniMensili;

            for (int i = 1; i <= giorniMese; i++)
            {
                // Interveniamo solo dove non ci sono già regole (Q, Sabati ancorati, RF, RS, NL)
                if (t[i] == null)
                {
                    // Cerchiamo l'ultimo turno effettivo (1 o 2) fatto in precedenza nel mese
                    string ultimo = GetUltimoTurnoEffettivo(t, i);

                    if (ultimo == "1")
                    {
                        t[i] = "2";
                    }
                    else if (ultimo == "2")
                    {
                        t[i] = "1";
                    }
                    else
                    {
                        // Se è l'inizio del mese e non c'è storico, partiamo col turno 1
                        t[i] = "1";
                    }
                }
            }
        }


        // --- LOGICA COPPIA (2 DIPENDENTI)  ---
        // Alterna giorno per giorno (1->2->1) e tra colleghi (A=1, B=2)
        private void RiempimentoUfficioCoppia(DipendenteTurno d1, DipendenteTurno d2, int giorniMese)
        {
            string[] t1 = d1.TurniMensili;
            string[] t2 = d2.TurniMensili;

            // Default iniziale: se il primo giorno è vuoto, partiamo con 1 per il dipendente A
            // (A meno che non ci siano vincoli successivi che propagano indietro, ma semplifichiamo partendo da 1)
            string turnoAttesoD1 = "1";

            for (int i = 1; i <= giorniMese; i++)
            {
                // 1. Calcoliamo quale dovrebbe essere il turno teorico per D1 oggi
                // Guardiamo indietro: qual è stato l'ultimo turno "1" o "2" assegnato a D1?
                string ultimo = GetUltimoTurnoEffettivo(t1, i);
                if (ultimo == "1") turnoAttesoD1 = "2";
                else if (ultimo == "2") turnoAttesoD1 = "1";
                // Se ultimo è nullo (inizio mese), resta il default (es. "1" o continua dal mese prima se implementato)

                // 2. Verifichiamo se ci sono blocchi (Q, RF o Sabato Ancorato)
                bool d1Bloccato = (t1[i] != null);
                bool d2Bloccato = (t2[i] != null);

                // CASO A: Entrambi bloccati (Es. Domenica RF, o conflitti Q)
                if (d1Bloccato && d2Bloccato)
                {
                    // Non facciamo nulla, vincono i blocchi.
                    // L'alternanza riprenderà dal prossimo giorno basandosi su questi valori se sono 1 o 2.
                    continue;
                }

                // CASO B: D1 è bloccato, D2 è libero
                if (d1Bloccato && !d2Bloccato)
                {
                    // D2 deve essere l'opposto di D1 (se D1 è 1 o 2)
                    if (t1[i] == "1") t2[i] = "2";
                    else if (t1[i] == "2") t2[i] = "1";
                    else
                    {
                        // Se D1 è Q o RF, D2 non ha un opposto diretto.
                        // Facciamo continuare D2 con la sua alternanza personale
                        string ultimoD2 = GetUltimoTurnoEffettivo(t2, i);
                        t2[i] = (ultimoD2 == "1") ? "2" : "1";
                    }
                }
                // CASO C: D2 è bloccato, D1 è libero
                else if (!d1Bloccato && d2Bloccato)
                {
                    // D1 deve essere l'opposto di D2
                    if (t2[i] == "1") t1[i] = "2";
                    else if (t2[i] == "2") t1[i] = "1";
                    else
                    {
                        // Se D2 è Q o RF, D1 segue la sua alternanza teorica
                        t1[i] = turnoAttesoD1;
                    }
                }
                // CASO D: Entrambi liberi (Giornata standard)
                else
                {
                    // D1 prende il suo turno atteso (calcolato dall'alternanza storica)
                    t1[i] = turnoAttesoD1;

                    // D2 prende l'opposto di D1
                    t2[i] = (t1[i] == "1") ? "2" : "1";
                }
            }
        }

        // Funzione Helper per trovare l'ultimo turno "vero" (1 o 2) ignorando RF, Q e buchi
        private string GetUltimoTurnoEffettivo(string[] turni, int giornoCorrente)
        {
            // Scorriamo all'indietro partendo da ieri
            for (int k = giornoCorrente - 1; k >= 1; k--)
            {
                string val = turni[k];
                if (val == "1" || val == "2")
                {
                    return val;
                }
                // Se troviamo Q o RF, li ignoriamo e cerchiamo ancora indietro
                // per mantenere la sequenza 1-2-1-2 "attraverso" i riposi.
            }
            return null; // Nessun storico trovato (inizio mese)
        }

        private void RiempimentoUfficioCDR(List<DipendenteTurno> dipendenti, int giorniMese)
        {
            foreach (var dip in dipendenti)
            {
                for (int i = 1; i <= giorniMese; i++)
                {
                    // Se non è Q, non è Sabato Anchor, non è RF... metti 1
                    if (dip.TurniMensili[i] == null)
                    {
                        dip.TurniMensili[i] = "1";
                    }
                }
            }
        }

        private void RiempimentoUfficioFureria(List<DipendenteTurno> gruppo, int giorniMese, int anno, int mese)
        {
            // ---------------------------------------------------
            // FASE 0: APPLICAZIONE REGOLA RIGIDA "SABATO = 1"
            // ---------------------------------------------------
            for (int k = 1; k <= giorniMese; k++)
            {
                DateTime dt = new DateTime(anno, mese, k);

                // Se è Sabato
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    foreach (var dip in gruppo)
                    {
                        // Se non è in Ferie (Q), forza Turno 1
                        // (Se fosse già impostato a RF o 2 da regole precedenti, lo sovrascriviamo per ordine di servizio)
                        if (dip.TurniMensili[k] != "Q")
                        {
                            dip.TurniMensili[k] = "1";
                        }
                    }
                }
            }

            // ---------------------------------------------------
            // FASE SUCCESSIVA: RIEMPIMENTO DEGLI ALTRI GIORNI
            // Usiamo la logica standard (Min 2 persone, Ratio 50%),
            // che si adatterà automaticamente ai sabati già fissati a 1.
            // ---------------------------------------------------

            for (int i = 1; i <= giorniMese; i++)
            {
                // 1. STATO ATTUALE
                int count1 = 0;
                int count2 = 0;
                List<DipendenteTurno> liberi = new List<DipendenteTurno>();

                foreach (var dip in gruppo)
                {
                    string t = dip.TurniMensili[i];
                    if (t == "1") count1++;
                    else if (t == "2") count2++;
                    else if (t == null) liberi.Add(dip);
                }

                // 2. CONSECUTIVI (Safety)
                // Particolare attenzione qui: Se Sabato è 1 forzato, Venerdì dovrà evitare l'1 se Giovedì era 1.
                // Se Sabato è 1, Domenica tenderà ad essere 2 (o RF).
                var candidati = new List<DipendenteTurno>();

                foreach (var dip in liberi)
                {
                    string p1 = (i > 1) ? dip.TurniMensili[i - 1] : "";
                    string p2 = (i > 2) ? dip.TurniMensili[i - 2] : "";
                    bool no1 = (p1 == "1" && p2 == "1");
                    bool no2 = (p1 == "2" && p2 == "2");

                    if (no1 && !no2) { dip.TurniMensili[i] = "2"; count2++; }
                    else if (no2 && !no1) { dip.TurniMensili[i] = "1"; count1++; }
                    else candidati.Add(dip);
                }

                // 3. RIEMPIMENTO FINALE (Ratio 50/50 + Minimo Persone)
                // Anche se Fureria fa tutti 1 il sabato, cerchiamo di bilanciare negli altri giorni

                var coda = candidati.Select(d => new
                {
                    Dip = d,
                    Perc = GetPercTurno1Attuale(d.TurniMensili, i)
                }).ToList();

                while (coda.Count > 0)
                {
                    var item = coda.OrderByDescending(x => Math.Abs(x.Perc - 50.0)).First();
                    string decisione = null;

                    // Logica copertura buchi (cerca di avere almeno 2 persone se possibile)
                    if (count1 < 2 && count2 >= 2) decisione = "1";
                    else if (count2 < 2 && count1 >= 2) decisione = "2";
                    else
                    {
                        // Bilanciamento numerico semplice
                        if (count1 > count2) decisione = "2";
                        else if (count2 > count1) decisione = "1";
                        else decisione = (item.Perc < 50.0) ? "1" : "2"; // Preferenza personale
                    }

                    item.Dip.TurniMensili[i] = decisione;
                    if (decisione == "1") count1++; else count2++;

                    coda.Remove(item);
                }
            }
        }
        // ---  (Min 2 Dipendenti) ---
        // --- UFFICI CON AUTISTA (MACRO) ---
        // Priorità: 
        // 1. Presenza Autista (Bloccante)
        // 2. Bilanciamento Sabati (Evitare tutti 1 o tutti 2)
        // 3. Ratio 50% (Bilanciamento generale)
        private void RiempimentoUfficioConAutista(List<DipendenteTurno> gruppo, int giorniMese)
        {
            // 1. PRE-BILANCIAMENTO (Invariato)
            EseguiPreBilanciamentoSabati(gruppo, giorniMese);

            for (int i = 1; i <= giorniMese; i++)
            {
                // ------------------------------------------------------------
                // NUOVA REGOLA: GESTIONE TRIPLETTO PER AREA (Prima di tutto)
                // ------------------------------------------------------------
                var aree = gruppo.GroupBy(d => d.Area ?? "NESSUNA").ToList();

                foreach (var areaGroup in aree)
                {
                    // Troviamo chi lavora oggi in questa Area (escludiamo ferie/malattia)
                    var disponibiliArea = areaGroup.Where(d =>
                        d.TurniMensili[i] != "RF" &&
                        d.TurniMensili[i] != "Q" &&
                        d.TurniMensili[i] != "RS" &&
                        d.TurniMensili[i] != "NL").ToList();

                    // SE SONO ESATTAMENTE 3: NON POSSONO DIVIDERSI (2+1 vietato) -> UNIAMOLI
                    if (disponibiliArea.Count == 3)
                    {
                        // Decidiamo il turno target:
                        // 1. Se c'è un Autista già assegnato a 1 o 2 (da regole precedenti), seguiamo lui.
                        // 2. Altrimenti valutiamo bilanciamento o giorni precedenti.

                        string target = "1"; // Default Mattina

                        // Se qualcuno è già fissato su "2" ed è Autista, vincono tutti su "2"
                        if (disponibiliArea.Any(d => d.TurniMensili[i] == "2" && d.IsAutista)) target = "2";

                        // Se la maggioranza è già orientata sul 2 (perché spostata da regole precedenti), andiamo tutti su 2
                        else if (disponibiliArea.Count(d => d.TurniMensili[i] == "2") >= 2) target = "2";

                        // Verifica sicurezza: Se target è "1", controlliamo che nessuno abbia fatto "2" ieri
                        if (target == "1")
                        {
                            bool qualcunoHaFattoPomeIeri = disponibiliArea.Any(d => HasPrevShift(d.TurniMensili, i, "2"));
                            if (qualcunoHaFattoPomeIeri) target = "2"; // Meglio tutti pome che rompere il riposo
                        }

                        // APPLICAZIONE FORZATA: TUTTI INSIEME
                        foreach (var dip in disponibiliArea)
                        {
                            dip.TurniMensili[i] = target;
                        }

                        // Rimuoviamoli dal pool successivo per non farli toccare dall'algoritmo standard
                        // (Li consideriamo "Sistemati")
                    }
                }

                // ------------------------------------------------------------
                // PROCEDURA STANDARD (PER GLI ALTRI GRUPPI)
                // ------------------------------------------------------------

                // 1. FOTOGRAFIA
                int count1 = 0; int count2 = 0;
                bool hasAutista1 = false; bool hasAutista2 = false;
                List<DipendenteTurno> liberi = new List<DipendenteTurno>();

                foreach (var dip in gruppo)
                {
                    // Se sono già stati sistemati dalla regola del 3, li contiamo e basta
                    string t = dip.TurniMensili[i];
                    if (t == "1") { count1++; if (dip.IsAutista) hasAutista1 = true; }
                    else if (t == "2") { count2++; if (dip.IsAutista) hasAutista2 = true; }
                    else if (t == null) { liberi.Add(dip); }
                }

                // 2. CONSECUTIVI E ASSEGNAZIONE STANDARD
                // (Solo per chi è rimasto null/libero)
                var poolLavoro = new List<DipendenteTurno>();
                foreach (var dip in liberi)
                {
                    string p1 = (i > 1) ? dip.TurniMensili[i - 1] : "";
                    string p2 = (i > 2) ? dip.TurniMensili[i - 2] : "";
                    bool no1 = (p1 == "1" && p2 == "1");
                    bool no2 = (p1 == "2" && p2 == "2");

                    if (no1 && !no2) { dip.TurniMensili[i] = "2"; count2++; if (dip.IsAutista) hasAutista2 = true; }
                    else if (no2 && !no1) { dip.TurniMensili[i] = "1"; count1++; if (dip.IsAutista) hasAutista1 = true; }
                    else poolLavoro.Add(dip);
                }

                // 3. GARANZIA AUTISTA STANDARD
                var autistiDisponibili = poolLavoro.Where(d => d.IsAutista).ToList();
                foreach (var a in autistiDisponibili) poolLavoro.Remove(a);

                if (!hasAutista1 && autistiDisponibili.Count > 0)
                {
                    var a = autistiDisponibili.OrderBy(x => GetPercTurno1Attuale(x.TurniMensili, i)).First();
                    a.TurniMensili[i] = "1"; hasAutista1 = true; count1++;
                    autistiDisponibili.Remove(a);
                }
                if (!hasAutista2 && autistiDisponibili.Count > 0)
                {
                    var a = autistiDisponibili.OrderByDescending(x => GetPercTurno1Attuale(x.TurniMensili, i)).First();
                    a.TurniMensili[i] = "2"; hasAutista2 = true; count2++;
                    autistiDisponibili.Remove(a);
                }
                poolLavoro.AddRange(autistiDisponibili);

                // 4. RIEMPIMENTO FINALE
                var coda = poolLavoro.Select(d => new { Dip = d, Perc = GetPercTurno1Attuale(d.TurniMensili, i), IsDriver = d.IsAutista }).ToList();

                while (coda.Count > 0)
                {
                    var item = coda.OrderByDescending(x => Math.Abs(x.Perc - 50.0)).First();
                    bool canGo1 = hasAutista1 || item.IsDriver;
                    bool canGo2 = hasAutista2 || item.IsDriver;
                    string decisione = null;

                    if (!canGo1 && !canGo2) decisione = "RF";
                    else if (canGo1 && !canGo2) decisione = "1";
                    else if (!canGo1 && canGo2) decisione = "2";
                    else
                    {
                        // Controllo se nell'area del dipendente sono rimasti solo in 2 a dover decidere
                        // Ma qui la Regola del 3 ha già agito a monte, quindi usiamo la logica standard
                        int colleghiAreaSu1 = gruppo.Count(d => d.Area == item.Dip.Area && d.TurniMensili[i] == "1");
                        int colleghiAreaSu2 = gruppo.Count(d => d.Area == item.Dip.Area && d.TurniMensili[i] == "2");

                        if (colleghiAreaSu1 > colleghiAreaSu2 + 1) decisione = "2";
                        else if (colleghiAreaSu2 > colleghiAreaSu1 + 1) decisione = "1";
                        else
                        {
                            if (count1 < 2 && count2 >= 2) decisione = "1";
                            else if (count2 < 2 && count1 >= 2) decisione = "2";
                            else decisione = (item.Perc < 50.0) ? "1" : "2";
                        }
                    }
                    item.Dip.TurniMensili[i] = decisione;
                    if (decisione == "1") { count1++; if (item.IsDriver) hasAutista1 = true; }
                    else if (decisione == "2") { count2++; if (item.IsDriver) hasAutista2 = true; }
                    coda.Remove(item);
                }

                // Eseguo comunque la correzione coppie standard (per gruppi > 3)
                EseguiCorrezioneMinimoDuePerArea(gruppo, i);
            }
        }

        // Funzione Helper per C#
        private bool HasPrevShift(string[] turni, int oggi, string target)
        {
            if (oggi > 1 && turni[oggi - 1] == target) return true;
            return false;
        }

        // --- METODO FONDAMENTALE PER ROMPERE I BLOCCHI "TUTTI SU 1" ---
        private void EseguiPreBilanciamentoSabati(List<DipendenteTurno> gruppo, int giorniMese)
        {
            // RAGGRUPPA PER AREA (UOTE, UOTP, ecc...)
            // Se Area è vuota, raggruppa sotto "NESSUNA_AREA"
            var aree = gruppo.GroupBy(d => d.Area ?? "NESSUNA_AREA").ToList();

            foreach (var areaGroup in aree)
            {
                var dipendentiArea = areaGroup.ToList();

                // Se c'è un solo dipendente nell'area, non possiamo bilanciare nulla
                if (dipendentiArea.Count < 2) continue;

                // SCORRI TUTTI I GIORNI (Sabati e festivi ancorati)
                for (int k = 1; k <= giorniMese; k++)
                {
                    // Analizza lo sbilanciamento INTERNO ALL'AREA
                    var fissatiSu1 = dipendentiArea.Where(d => d.TurniMensili[k] == "1").ToList();
                    var fissatiSu2 = dipendentiArea.Where(d => d.TurniMensili[k] == "2").ToList();

                    // Saltiamo se il giorno è vuoto o se è già misto
                    if ((fissatiSu1.Count == 0 && fissatiSu2.Count == 0) ||
                        (fissatiSu1.Count > 0 && fissatiSu2.Count > 0))
                    {
                        continue;
                    }

                    // --- CASO: TUTTA L'AREA SU 1 ---
                    if (fissatiSu1.Count > 1 && fissatiSu2.Count == 0)
                    {
                        // Spostiamo la metà esatta dei dipendenti di quest'area
                        int daSpostare = Math.Max(1, fissatiSu1.Count / 2);

                        var candidati = fissatiSu1
                            .OrderByDescending(d => d.IsAutista) // Priorità spostamento Autisti
                            .ThenByDescending(d => GetPercTurno1Attuale(d.TurniMensili, k)) // Poi chi ha troppi "1"
                            .ToList();

                        for (int x = 0; x < daSpostare; x++)
                        {
                            candidati[x].TurniMensili[k] = "2"; // FORZA SPOSTAMENTO
                        }
                    }

                    // --- CASO: TUTTA L'AREA SU 2 ---
                    else if (fissatiSu2.Count > 1 && fissatiSu1.Count == 0)
                    {
                        int daSpostare = Math.Max(1, fissatiSu2.Count / 2);

                        var candidati = fissatiSu2
                            .OrderByDescending(d => d.IsAutista)
                            .ThenBy(d => GetPercTurno1Attuale(d.TurniMensili, k))
                            .ToList();

                        for (int x = 0; x < daSpostare; x++)
                        {
                            candidati[x].TurniMensili[k] = "1"; // FORZA SPOSTAMENTO
                        }
                    }
                }
            }
        }
        private void EseguiCorrezioneMinimoDuePerArea(List<DipendenteTurno> gruppo, int giornoIdx)
        {
            // Analizziamo area per area (UOTE, UOTP)
            var aree = gruppo.GroupBy(d => d.Area ?? "NESSUNA").ToList();

            foreach (var areaGroup in aree)
            {
                var dipendentiArea = areaGroup.ToList();

                // Identifichiamo chi è finito su turno 1 e turno 2 OGGI
                var su1 = dipendentiArea.Where(d => d.TurniMensili[giornoIdx] == "1").ToList();
                var su2 = dipendentiArea.Where(d => d.TurniMensili[giornoIdx] == "2").ToList();

                // CASO A: C'è SOLO 1 persona sul Turno 1, e abbiamo risorse sul Turno 2 da rubare
                if (su1.Count == 1 && su2.Count > 1)
                {
                    var solitario = su1.First();

                    // Dobbiamo spostare qualcuno da 'su2' a 'su1'.
                    // REGOLA COPPIA: La coppia finale (solitario + nuovo) deve avere ALMENO un autista.
                    // QUINDI: O il solitario è autista, o il candidato che sposto DEVE essere autista.

                    // Cerchiamo candidati validi nel turno 2
                    var candidati = su2
                        .Where(c => (solitario.IsAutista || c.IsAutista)) // Soddisfa la regola coppia
                        .OrderBy(c => c.IsAutista) // Preferiamo non spostare autisti se non necessario, ma va bene tutto
                        .ThenBy(c => GetPercTurno1Attuale(c.TurniMensili, giornoIdx)) // Chi ha fatto pochi 1
                        .ToList();

                    if (candidati.Count > 0)
                    {
                        var sposto = candidati.First();
                        sposto.TurniMensili[giornoIdx] = "1"; // SPOSTAMENTO

                        // CORREZIONE GIORNO PRECEDENTE (fondamentale!)
                        // Se sposto uno da 2 a 1, devo controllare che ieri non avesse 2.
                        CorreggiTurnoPrecedente(sposto, giornoIdx, "1");
                    }
                }

                // CASO B: C'è SOLO 1 persona sul Turno 2, e abbiamo risorse sul Turno 1
                else if (su2.Count == 1 && su1.Count > 1)
                {
                    var solitario = su2.First();

                    // Cerchiamo candidati nel turno 1
                    var candidati = su1
                        .Where(c => (solitario.IsAutista || c.IsAutista)) // Regola coppia
                        .OrderBy(c => c.IsAutista)
                        .ThenByDescending(c => GetPercTurno1Attuale(c.TurniMensili, giornoIdx))
                        .ToList();

                    if (candidati.Count > 0)
                    {
                        var sposto = candidati.First();
                        sposto.TurniMensili[giornoIdx] = "2"; // SPOSTAMENTO

                        // Correggere il precedente da 1 a 2 solitamente non crea problemi (1->2 è lecito),
                        // ma per sicurezza potremmo controllare non si creino blocchi strani.
                        // In generale 1->2 ok. 
                    }
                }

                // CASO SPECIALE: 1 su Turno 1 e 1 su Turno 2 (Totale 2 persone)
                // Se non formano coppia valida, o se è meglio stare insieme...
                else if (su1.Count == 1 && su2.Count == 1)
                {
                    var p1 = su1.First();
                    var p2 = su2.First();

                    // Se insieme formano una coppia con autista (almeno uno dei due lo è),
                    // conviene metterli entrambi sullo stesso turno (es. 1) per non lasciarli soli?
                    // L'utente chiede: "Non puoi fare coppia se uno non è autista".
                    // Se li mettiamo insieme devono essere validi.

                    if (p1.IsAutista || p2.IsAutista)
                    {
                        // Li uniamo sul turno 1 (standard)
                        p2.TurniMensili[giornoIdx] = "1";
                        CorreggiTurnoPrecedente(p2, giornoIdx, "1");
                    }
                    // Se nessuno è autista, non possiamo fare coppia.
                    // Li lasciamo separati? O mettiamo RF?
                    // Qui dipende dalle regole estreme. Per ora lasciamo invariato se non hanno autista.
                }
            }
        }

        // Funzione ricorsiva per sistemare il passato ed evitare 2->1
        private void CorreggiTurnoPrecedente(DipendenteTurno dip, int giornoOggi, string nuovoTurnoOggi)
        {
            // Se oggi ho messo "1", controllo ieri
            if (nuovoTurnoOggi == "1")
            {
                int ieri = giornoOggi - 1;
                if (ieri < 1) return;

                // Se ieri era "2", è illegale fare 2->1. Devo cambiare ieri in "1".
                if (dip.TurniMensili[ieri] == "2")
                {
                    dip.TurniMensili[ieri] = "1";
                    // E siccome ho cambiato ieri in 1, devo controllare l'altro ieri! (Ricorsione)
                    CorreggiTurnoPrecedente(dip, ieri, "1");
                }
            }
        }

        // Funzione Helper che decide 1 o 2 in base alla storia precedente e ratio
        private string CalcolaTurnoStandard(string[] t, int i, bool usaRatio)
        {
            // Controllo consecutivi
            string p1 = (i > 1) ? t[i - 1] : "";
            string p2 = (i > 2) ? t[i - 2] : "";

            if (p1 == "1" && p2 == "1") return "2";
            if (p1 == "2" && p2 == "2") return "1";

            if (usaRatio)
            {
                // Calcolo percentuale attuale Turno 1
                double perc1 = GetPercTurno1Attuale(t, i);

                // Se sei già sopra il 60%, DEVI fare turno 2 (Blocco di sicurezza)
                if (perc1 >= 60.0) return "2";

                // Se sei sopra il 50% (es 55%), PREFERISCO darti 2 per portarti al 50
                if (perc1 > 50.0) return "2";

                // Se sei sotto il 50%, ti do 1
                return "1";
            }

            // Default se ratio disattivato
            return "1";
        }
        //STAMPE ED EXCEL
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

            try
            {
                int anno = Convert.ToInt32(txtAnno.Text);
                int mese = Convert.ToInt32(ddlMese.SelectedValue);
                int giorniMese = DateTime.DaysInMonth(anno, mese);

                // 1. RECUPERA DATI
                Manager mn = new Manager();
                // 1. Recupera i dati dal DB (Unione tra Anagrafica e Turni Salvati)
                List<DipendenteTurno> listaDati = mn.GetTurniMensile(anno, ddlMese.SelectedItem.Text);
                // verifico che l'array turni mensili abbia almeno un elemento valido da mostrare
                bool esisteElementoValido = listaDati[0].TurniMensili != null && listaDati[0].TurniMensili.Any(x => x != null);

                if (!esisteElementoValido)
                {
                    //lblError.Text = "⚠️ Nessun dato da esportare.";

                    errorMessage.InnerText = @"⚠️ Nessun dato da esportare."; ;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
                    return;
                }

                Routine stampa = new Routine();
                stampa.CreaExcelTurnazioneMensile(listaDati, anno, mese, giorniMese, Context);

            }
            catch (Exception ex)
            {
                //lblError.Text = "Errore Excel: " + ex.Message;
                //lblError.ForeColor = System.Drawing.Color.Red;
                errorMessage.InnerText = @"Errore Excel: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
            }
        }


        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            //try
            //{
            int anno = Convert.ToInt32(txtAnno.Text);
            int mese = Convert.ToInt32(ddlMese.SelectedValue);
            int giorniMese = DateTime.DaysInMonth(anno, mese);
            string nomeMeseTesto = ddlMese.SelectedItem.Text;

            Manager mn = new Manager();
            // ATTENZIONE: Se hai appena modificato i dati a video e non hai salvato, 
            // GetTurniMensile caricherà i vecchi dati dal DB.
            List<DipendenteTurno> listaDati = mn.GetTurniMensile(anno, nomeMeseTesto);

            // verifico che l'array turni mensili abbia almeno un elemento valido da mostrare
            bool esisteElementoValido = listaDati[0].TurniMensili != null && listaDati[0].TurniMensili.Any(x => x != null);

            if (!esisteElementoValido)

            {
                //lblError.Text = "⚠️ Nessun dato trovato nel database per questo mese. Salva prima di stampare.";
                //lblError.ForeColor = System.Drawing.Color.Orange;
                errorMessage.InnerText = @"⚠️ Nessun dato trovato nel database per questo mese. Salva prima di stampare.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
                return;
            }

            Routine stampa = new Routine();
            stampa.CreaPdfTurnazioneMensile(listaDati, nomeMeseTesto, anno, mese, giorniMese);


        }


        protected void btnsalva_Click(object sender, EventArgs e)
        {
            try
            {
                int giorniMese = 0;
                int anno = Convert.ToInt32(txtAnno.Text);
                //int mese = int.Parse(ddlMese.SelectedValue);
                int mese = System.Convert.ToInt32(ddlMese.SelectedValue);
                // 1. RICALCOLA I TURNI 
                // (È necessario perché in WebForms lo stato si perde tra i postback, 
                // a meno che tu non abbia salvato la "listaDipendenti" in Session)
                Manager mn = new Manager();
                DataTable dtDipendenti = mn.getListDipendenti(); // dipendenti
                DataTable dtQuartine = mn.getListQuartina(anno); // lista quartine
                var mappaGiorniQuartina = CostruisciMappaQuartine(dtQuartine, mese);
                List<DipendenteTurno> listaDaSalvare = new List<DipendenteTurno>();


                foreach (DataRow row in dtDipendenti.Rows)
                {
                    DipendenteTurno dip = new DipendenteTurno();
                    dip.Matricola = row["matricola_ced"].ToString().Trim();
                    dip.Nominativo = row["nominativo"].ToString().Trim();
                    dip.Ufficio = row["ufficio"].ToString().Trim();
                    dip.Gruppo = row["gruppo_quartina"].ToString().Trim();
                    // Inizializza array vuoto
                    dip.TurniMensili = new string[32];

                    // 2. LEGGI LE MODIFICHE DAL FORM HTML
                    giorniMese = DateTime.DaysInMonth(anno, mese);
                    for (int i = 1; i <= giorniMese; i++)
                    {
                        // Ricostruisco la chiave "name" che ho generato nell'HTML
                        // es: "T_12345_1"
                        string key = $"T_{dip.Matricola}_{i}";

                        // Leggo il valore inviato dal browser
                        string valUtente = Request.Form[key];

                        if (!string.IsNullOrEmpty(valUtente))
                        {
                            dip.TurniMensili[i] = valUtente.ToUpper().Trim();
                        }
                    }

                    listaDaSalvare.Add(dip);
                }
                //}
                // 2. ESEGUE IL SALVATAGGIO
                Boolean resp = mn.SalvaTurnoMensileN(listaDaSalvare, anno, ddlMese.SelectedItem.Text, dtDipendenti);
                //errorMessage.Style["font-size"] = "12px";
                if (resp)
                {
                    //lblError.Visible = false;

                    //errorMessage.Style["font-family"] = "Verdana, sans-serif";
                    //errorMessage.Style["font-weight"] = "bold";

                    //lblError.Text = "✅ Salvataggio completato con successo!";
                    //lblError.ForeColor = System.Drawing.Color.Green;
                    RecalcolaPercentuali(listaDaSalvare, giorniMese);
                    GeneraHtml(listaDaSalvare, anno, mese);
                    Session.Remove("ListaDipendentiTurni");
                    errorMessage.InnerText = @"✅ Salvataggio completato con successo!";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
                }

            }
            catch (Exception ex)
            {
                errorMessage.InnerText = @"❌ Errore durante il salvataggio: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);

                //lblError.Text = "❌ Errore durante il salvataggio: " + ex.Message;
                //lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btImportaMatriceExcel_Click(object sender, EventArgs e)
        {

            List<RecordRsnl> datiDaInserire = new List<RecordRsnl>();
            if (File.Exists(FileCalendarioRSNL))
            {
                //lblError.Text = "entrato in file exist";
                datiDaInserire = LeggiFileExcel(FileCalendarioRSNL);
                SalvaSuSql(datiDaInserire);
               // lblError.Text = "esco da salva file";
            }
            else
            {
               // lblError.Text = "entrato in errore";
                errorMessage.InnerText = @"⚠️ Nessun file calendario trovato.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
            }
        }
        public void SalvaSuSql(List<RecordRsnl> records)
        {
            Manager mn = new Manager();



            string resp = mn.InsRSNL(records);
            if (!String.IsNullOrEmpty(resp))
            {
                errorMessage.InnerText = @"⚠️ errore in inserimento rsnl. " + resp;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#errorModal').modal('show');", true);
            }

        }

        static List<RecordRsnl> LeggiFileExcel(string path)
        {
            var output = new List<RecordRsnl>();

            using (var wb = new XLWorkbook(path))
            {
                var ws = wb.Worksheet(1);
                int lastRow = ws.LastRowUsed().RowNumber();
                int startRow = 4; // Modifica se i dati iniziano diversamente

                for (int r = startRow; r <= lastRow; r++)
                {
                    var row = ws.Row(r);

                    // Verifica se questa riga è una Domenica attiva (Colonna AF / 32)
                    string valAF = row.Cell(32).GetValue<string>();

                    if (int.TryParse(valAF, out int quartina) && quartina >= 1 && quartina <= 4)
                    {
                        // Dati della riga Domenica
                        string giornoStr = row.Cell(1).GetValue<string>();
                        string meseStr = row.Cell(2).GetValue<string>().Trim(); // Colonna B (Mese stringa)

                        // Saltiamo se il mese non è riconosciuto o manca il giorno
                        if (!int.TryParse(giornoStr, out int gDom) || !MappaMesi.ContainsKey(meseStr))
                            continue;

                        // Creiamo la data ancora della Domenica (es. 01/02/2026)
                        DateTime dataDomenica;
                        try
                        {
                            dataDomenica = new DateTime(2026, MappaMesi[meseStr], gDom);
                        }
                        catch { continue; } // Salta date non valide (es. 30 Febbraio)

                        // Impostiamo i range colonne in base alla Quartina (gestendo le colonne vuote)
                        int colStart = 0, colEnd = 0;
                        string prefix = "";

                        switch (quartina)
                        {
                            case 1: colStart = 5; colEnd = 10; prefix = "A"; break; // A1-A6 (E-J)
                            case 2: colStart = 12; colEnd = 17; prefix = "B"; break; // B1-B6 (L-Q) - Salta K
                            case 3: colStart = 19; colEnd = 24; prefix = "C"; break; // C1-C6 (S-X) - Salta R
                            case 4: colStart = 26; colEnd = 31; prefix = "D"; break; // D1-D6 (Z-AE) - Salta Y
                        }

                        // Analisi per ogni gruppo della quartina
                        for (int c = colStart; c <= colEnd; c++)
                        {
                            int subIndex = c - colStart + 1;
                            string nomeGruppo = $"{prefix}{subIndex}";

                            // CERCA RS: Settimana corrente (fino alla domenica inclusa) -> Offset -6 a 0
                            DateTime? dataRS = CercaDataEvento(ws, "RS", c, r, -6, 0, dataDomenica);

                            // CERCA NL: Settimana successiva -> Offset +1 a +7
                            DateTime? dataNL = CercaDataEvento(ws, "NL", c, r, 1, 7, dataDomenica);

                            // Se troviamo qualcosa, aggiungiamo alla lista
                            if (dataRS.HasValue || dataNL.HasValue)
                            {
                                output.Add(new RecordRsnl
                                {
                                    Gruppo = nomeGruppo,
                                    Quartina = quartina,
                                    DataRS = dataRS,
                                    DataNL = dataNL,
                                    MeseStringa = meseStr // Manteniamo la stringa originale (es. "GEN")
                                });
                            }
                        }
                    }
                }
            }
            return output;
        }
        // Cerca "RS" o "NL" e restituisce la DATA CALCOLATA (gestisce cambio mese/anno)
        static DateTime? CercaDataEvento(IXLWorksheet ws, string target, int colIdx, int baseRow, int minOff, int maxOff, DateTime baseDate)
        {
            int maxExcelRows = ws.LastRowUsed().RowNumber();

            for (int offset = minOff; offset <= maxOff; offset++)
            {
                int targetRow = baseRow + offset;

                // Controllo limiti foglio
                if (targetRow < 1 || targetRow > maxExcelRows) continue;

                string val = ws.Cell(targetRow, colIdx).GetValue<string>().Trim().ToUpper();

                if (val == target)
                {
                    // La magia di DateTime: se aggiungo -2 giorni al 1 Febbraio, ottengo 30 Gennaio
                    return baseDate.AddDays(offset);
                }
            }
            return null;
        }

        // Funzione Helper per cercare un valore in una colonna specifica tra due righe
        static RisultatoRicerca CercaValoreNelRange(IXLWorksheet ws, string targetVal, int colIndex, int rStart, int rEnd)
        {
            // Gestione limiti foglio (non andare sotto riga 1 o oltre la fine)
            int lastRowReal = ws.LastRowUsed().RowNumber();
            if (rStart < 1) rStart = 1;
            if (rEnd > lastRowReal) rEnd = lastRowReal;

            for (int i = rStart; i <= rEnd; i++)
            {
                string val = ws.Cell(i, colIndex).GetValue<string>().Trim().ToUpper();
                if (val == targetVal)
                {
                    // Trovato! Recuperiamo il giorno (Colonna A = 1)
                    string gStr = ws.Cell(i, 1).GetValue<string>();
                    if (int.TryParse(gStr, out int g))
                    {
                        return new RisultatoRicerca { Trovato = true, Giorno = g };
                    }
                }
            }
            return new RisultatoRicerca { Trovato = false, Giorno = null };
        }
        // Dizionario per convertire Mese stringa in numero
        static readonly Dictionary<string, int> MappaMesi = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "GEN", 1 }, { "FEB", 2 }, { "MAR", 3 }, { "APR", 4 }, { "MAG", 5 }, { "GIU", 6 },
            { "LUG", 7 }, { "AGO", 8 }, { "SET", 9 }, { "OTT", 10 }, { "NOV", 11 }, { "DIC", 12 }
        };


        private List<RegolaRSNL> CaricaRegoleRSNL(int anno, int mese)
        {
            List<RegolaRSNL> lista = new List<RegolaRSNL>();


            Manager mn = new Manager();
            return lista = mn.getRsNlnlByAnnoMese(anno, mese);



        }
    }
}

