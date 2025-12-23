using ClosedXML.Excel;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using Uotep.Classi;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Geom;
using Table = iText.Layout.Element.Table;

namespace Uote
{
    public partial class test : System.Web.UI.Page
    {
        // Classe helper per i dati

        //public class DipendenteTurno
        //{
        //    public string Matricola { get; set; }
        //    public string Nominativo { get; set; }
        //    public string Ufficio { get; set; }
        //    public bool IsAutista { get; set; }
        //    public int QuartinaID { get; set; }
        //    public string StringaGiorniQ { get; set; } // Es: "5,12,21"
        //    public string[] TurniMensili { get; set; } // Array [32] (indice 1-31)
        //    public string StatisticaPerc { get; set; }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtAnno.Text = System.Convert.ToInt32(DateTime.Now.Year).ToString();
            }
        }

        protected void btnCalcola_Click(object sender, EventArgs e)
        {
            int anno = int.Parse(txtAnno.Text);
            int mese = int.Parse(ddlMese.SelectedValue);

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
            List<DipendenteTurno> listaDipendenti = ElaboraDati(dtDip, mappaGiorniQuartina, anno, mese);
            Session["ListaDipendentiTurni"] = listaDipendenti;
            // 4. GENERO L'HTML (Usando il metodo grafico fatto prima)
            GeneraHtml(listaDipendenti, anno, mese);



        }
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

        private void GeneraHtml(List<DipendenteTurno> lista, int anno, int mese)
        {
            // ... controlli iniziali ...
            int giorniMese = DateTime.DaysInMonth(anno, mese);
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class='tabella-turni'>");

            // --- HEADER ---
            sb.Append("<thead><tr>");
            sb.Append("<th class='col-dip-header'>DIPENDENTE</th>");

            // NUOVA COLONNA HEADER
            sb.Append("<th class='col-stats-header'>%1°/2°</th>");

            for (int i = 1; i <= giorniMese; i++)
            {

                // Esempio:
                DateTime dt = new DateTime(anno, mese, i);
                bool isWeekend = (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);
                string classHeader = isWeekend ? "giorno-header weekend-h" : "giorno-header";
                string lettera = dt.ToString("ddd").Substring(0, 1).ToUpper();
                sb.AppendFormat("<th class='{0}'>{1}<br/><small>{2}</small></th>", classHeader, i, lettera);
            }
            sb.Append("</tr></thead><tbody>");

            // --- BODY ---
            var gruppi = lista.GroupBy(x => x.Ufficio).OrderBy(k => k.Key);

            foreach (var g in gruppi)
            {
                // ATTENZIONE AL COLSPAN: Ora è giorniMese + 2 (Nome + Stats)
                sb.AppendFormat("<tr class='tr-ufficio'><td colspan='{0}'>{1}</td></tr>",
                    giorniMese + 2, g.Key.ToUpper());

                foreach (var dip in g)
                {
                    sb.Append("<tr>");

                    // Colonna Nome
                    sb.AppendFormat("<td class='col-dipendente' title='{0}'>{1}<span class='badge-q'>Q{2}</span></td>",
                        dip.Nominativo, dip.Nominativo, dip.QuartinaID);

                    // Colonna Percentuale (Aggiungo classe per JS)
                    string valPerc = dip.StatisticaPerc.Replace("%", "");
                    string styleColor = "";
                    if (int.TryParse(valPerc, out int p) && (p < 50 || p > 60)) styleColor = "style='color:red;'";
                    else styleColor = "style='color:green;'";

                    sb.AppendFormat("<td class='col-stats' {0}>{1}</td>", styleColor, dip.StatisticaPerc);

                    // COLONNE GIORNI MODIFICABILI
                    for (int i = 1; i <= giorniMese; i++)
                    {
                        string val = dip.TurniMensili[i] ?? ""; // Gestione null
                        DateTime dt = new DateTime(anno, mese, i);
                        bool isWeekend = (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);

                        // Determina classe CSS per colore sfondo iniziale
                        string cssClass = isWeekend ? "weekend-col" : "";
                        if (val == "Q") cssClass += " t-q";
                        else if (val == "1") cssClass += " t-1";
                        else if (val == "2") cssClass += " t-2";
                        else if (val == "RF") cssClass += " t-rf";

                        // --- PUNTO CHIAVE: INPUT INVECE DI TESTO ---
                        // Name format: "T_{Matricola}_{Giorno}" es: "T_12345_1", "T_12345_2"
                        // OnChange: Chiama la funzione JS per ricalcolare
                        string inputHtml = string.Format(
                            "<input type='text' name='T_{0}_{1}' value='{2}' class='shift-input' maxlength='2' onchange='ricalcolaRiga(this)' autocomplete='off' />",
                            dip.Matricola.Trim(), // Importante: Matricola pulita per il nome univoco
                            i,
                            val
                        );

                        sb.AppendFormat("<td class='{0}' style='padding:0;'>{1}</td>", cssClass, inputHtml);
                    }
                    sb.Append("</tr>");
                }
            }
            sb.Append("</tbody></table>");
            ltlTabella.Text = sb.ToString();
        }
        private List<DipendenteTurno> ElaboraDati(DataTable dtDip, Dictionary<int, string> mappaQuartine, int anno, int mese)
        {
            var lista = new List<DipendenteTurno>();
            int giorniMese = DateTime.DaysInMonth(anno, mese);

            // --- PRIMA PASSATA: Creazione e Vincoli Assoluti ---
            foreach (DataRow row in dtDip.Rows)
            {
                var dip = new DipendenteTurno();
                dip.Nominativo = row["nominativo"].ToString();
                dip.Ufficio = row["ufficio"].ToString();
                dip.Matricola = row["matricola"].ToString();

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

                // Applica vincoli base (Q, Sabati, Festivi)
                List<int> giorniQ = ApplicaRegolaQ(dip.TurniMensili, stringaGiorni, giorniMese);
                ApplicaRegolaSabati(dip.TurniMensili, giorniQ, giorniMese, anno, mese);
                ApplicaRegolaFestivi(dip.TurniMensili, giorniMese, anno, mese);

                lista.Add(dip);
            }

            // --- SECONDA PASSATA: Riempimento per Gruppi ---
            var gruppiUfficio = lista.GroupBy(d => d.Ufficio).ToList();

            foreach (var gruppo in gruppiUfficio)
            {
                string nomeUfficio = gruppo.Key.ToUpper().Trim();
                List<DipendenteTurno> dipsDelGruppo = gruppo.ToList();

                if (nomeUfficio == "CDR")
                {
                    RiempimentoUfficioCDR(dipsDelGruppo, giorniMese);
                }
                else if ((nomeUfficio == "GIRO" || nomeUfficio == "NOTIFICHE") && dipsDelGruppo.Count == 2)
                {
                    RiempimentoUfficioGemelli(dipsDelGruppo[0], dipsDelGruppo[1], giorniMese);
                }
                else if (nomeUfficio.StartsWith("MACRO")) // Intercetta MACRO 1, MACRO 2, ecc.
                {
                    //  Copertura Autista Obbligatoria
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
                    // Uffici numerosi standard (senza vincolo autista)
                    RiempimentoUfficioMultiplo(dipsDelGruppo, giorniMese);
                }
            }





            // --- TERZA PASSATA: CALCOLO STATISTICHE (%) ---
            foreach (var dip in lista)
            {
                int count1 = 0;
                int count2 = 0;

                // Scansioniamo l'array (saltando l'indice 0)
                for (int i = 1; i <= giorniMese; i++)
                {
                    string t = dip.TurniMensili[i];
                    if (t == "1") count1++;
                    else if (t == "2") count2++;
                    // Ignoriamo Q e RF dal calcolo percentuale lavorativo
                }

                int totaleLavorati = count1 + count2;

                if (totaleLavorati > 0)
                {
                    // Calcolo percentuale Turno 1
                    double perc = (double)count1 / totaleLavorati * 100;
                    dip.StatisticaPerc = perc.ToString("0") + "%"; // Es: "60%"
                }
                else
                {
                    dip.StatisticaPerc = "N/A"; // Solo ferie o malattie
                }
            }
            return lista;
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

                // 3. ANALISI DEI VINCOLI CONSECUTIVI (Non si discute)
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
        // --- LOGICA COPPIA (2 DIPENDENTI) AGGIORNATA ---
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

        // --- LOGICA CDR ---
        // Mette sempre 1 nei buchi vuoti
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

            // NOTA: Copiamo la logica di loop giornaliero. Non chiamiamo "RiempimentoUfficioMultiplo"
            // direttamente perché quel metodo potrebbe contenere il "PreBilanciamentoSabati" 
            // che romperebbe la nostra regola dell'1 fisso.

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
        // --- LOGICA MACRO AREE CORRETTA (Min 2 Dipendenti) ---
        // Priorità: 
        // 1. Presenza Autista (Bloccante)
        // 2. Minimo 2 Dipendenti per turno (Operativo)
        // 3. Ratio 50% (Bilanciamento)
        private void RiempimentoUfficioConAutista(List<DipendenteTurno> gruppo, int giorniMese)
        {
            EseguiPreBilanciamentoSabati(gruppo, giorniMese);
            // =========================================================================
            // FASE 0: PRE-BILANCIAMENTO SABATI (CORREZIONE MACRO)
            // =========================================================================
            // Questa fase "rompe" la regola dell'ancoraggio se tutti i dipendenti sono finiti
            // sullo stesso turno di Sabato, garantendo copertura su entrambi i turni.

            // Troviamo tutti i sabati del mese
            List<int> sabati = new List<int>();


            // Nota: Il metodo attuale accetta (gruppo, giorniMese). Non ho anno/mese qui dentro,
            // ma posso dedurre i sabati verificando la posizione se avessi la data.
            // PER SEMPLICITÀ: Scorro tutti i giorni. Se trovo un giorno dove sono TUTTI bloccati
            // su un solo turno e l'altro è vuoto, intervengo. (Vale per i Sabati e per i festivi ancorati).

            for (int k = 1; k <= giorniMese; k++)
            {
                // Analizza chi è già fissato in questo giorno (dalla Fase 1: Q, Sabati Ancorati)
                var fissatiSu1 = gruppo.Where(d => d.TurniMensili[k] == "1").ToList();
                var fissatiSu2 = gruppo.Where(d => d.TurniMensili[k] == "2").ToList();

                // Se non c'è nessuno fissato (giorno lavorativo normale), saltiamo (ci penserà il riempimento dopo)
                if (fissatiSu1.Count == 0 && fissatiSu2.Count == 0) continue;

                // Se siamo equilibrati (almeno uno di qua e uno di là), saltiamo.
                if (fissatiSu1.Count > 0 && fissatiSu2.Count > 0) continue;

                // --- CASO CRITICO: TUTTI SU 1 ---
                if (fissatiSu1.Count > 1 && fissatiSu2.Count == 0)
                {
                    // Dobbiamo spostarne alcuni sul 2. Quanti? Metà del gruppo o almeno 1/2.
                    int daSpostare = Math.Max(1, fissatiSu1.Count / 2);

                    // CHI SPOSTIAMO? 
                    // 1. Priorità: Chi NON rompe un consecutivo (se i giorni prima sono già fissati, cosa rara ma possibile)
                    // 2. Priorità: Autista (se serve garantire autista anche sul turno 2)

                    // Ordiniamo: Prima gli Autisti (per coprire il turno 2 che è vuoto), poi chi ha più bisogno di turno 2
                    var candidati = fissatiSu1
                        .OrderByDescending(d => d.IsAutista) // Mette autisti in cima
                        .ThenByDescending(d => GetPercTurno1Attuale(d.TurniMensili, k)) // Mette chi ha tanti "1"
                        .ToList();

                    for (int x = 0; x < daSpostare; x++)
                    {
                        // Verifica di sicurezza (opzionale): non spostare se il giorno prima era 2 e l'altro prima 2.
                        // Ma essendo sabato, spesso venerdì è vuoto (null), quindi è sicuro.
                        candidati[x].TurniMensili[k] = "2";

                        // NOTA: Poiché l'utente chiede di forzare anche i successivi, l'algoritmo di riempimento
                        // che gira DOPO (Fase 4) si adatterà a questo nuovo valore "2" per calcolare domenica/lunedì.
                        // Per i precedenti (Venerdì), essendo null, verranno riempiti coerentemente.
                    }
                }

                // --- CASO CRITICO: TUTTI SU 2 ---
                else if (fissatiSu2.Count > 1 && fissatiSu1.Count == 0)
                {
                    int daSpostare = Math.Max(1, fissatiSu2.Count / 2);

                    var candidati = fissatiSu2
                        .OrderByDescending(d => d.IsAutista) // Serve autista sull'1?
                        .ThenBy(d => GetPercTurno1Attuale(d.TurniMensili, k)) // Mette chi ha pochi "1"
                        .ToList();

                    for (int x = 0; x < daSpostare; x++)
                    {
                        candidati[x].TurniMensili[k] = "1";
                    }
                }
            }


            // =========================================================================
            // FASE 1-4: RIEMPIMENTO GIORNALIERO 
            // =========================================================================
            for (int i = 1; i <= giorniMese; i++)
            {
                // ... (Copia qui tutto il codice "Fase 1: Fotografia" fino alla fine del metodo
                // che ti ho dato nella risposta precedente "Codice Corretto e Indistruttibile") ...

                // 1. FOTOGRAFIA
                int count1 = 0;
                int count2 = 0;
                bool hasAutista1 = false;
                bool hasAutista2 = false;
                List<DipendenteTurno> liberi = new List<DipendenteTurno>();

                foreach (var dip in gruppo)
                {
                    string t = dip.TurniMensili[i];
                    if (t == "1") { count1++; if (dip.IsAutista) hasAutista1 = true; }
                    else if (t == "2") { count2++; if (dip.IsAutista) hasAutista2 = true; }
                    else if (t == null) { liberi.Add(dip); }
                }

                // 2. GESTIONE CONSECUTIVI
                var poolLavoro = new List<DipendenteTurno>();
                foreach (var dip in liberi)
                {
                    string p1 = (i > 1) ? dip.TurniMensili[i - 1] : "";
                    string p2 = (i > 2) ? dip.TurniMensili[i - 2] : "";
                    bool no1 = (p1 == "1" && p2 == "1");
                    bool no2 = (p1 == "2" && p2 == "2");

                    if (no1 && !no2)
                    {
                        dip.TurniMensili[i] = "2"; count2++;
                        if (dip.IsAutista) hasAutista2 = true;
                    }
                    else if (no2 && !no1)
                    {
                        dip.TurniMensili[i] = "1"; count1++;
                        if (dip.IsAutista) hasAutista1 = true;
                    }
                    else poolLavoro.Add(dip);
                }

                // 3. GARANZIA AUTISTA
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

                // 4. CICLO ASSEGNAZIONE DEFINITIVO
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
                        if (count1 < 2 && count2 >= 2) decisione = "1";
                        else if (count2 < 2 && count1 >= 2) decisione = "2";
                        else if (count1 > count2) decisione = "2";
                        else if (count2 > count1) decisione = "1";
                        else decisione = (item.Perc < 50.0) ? "1" : "2";
                    }

                    item.Dip.TurniMensili[i] = decisione;
                    if (decisione == "1") { count1++; if (item.IsDriver) hasAutista1 = true; }
                    else if (decisione == "2") { count2++; if (item.IsDriver) hasAutista2 = true; }
                    coda.Remove(item);
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
            // 1. Controlla le festività fisse (giorno, mese)
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

            // 2. Calcolo della Pasqua (Algoritmo standard)
            int year = dt.Year;
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            DateTime pasqua = new DateTime(year, month, day);
            DateTime pasquetta = pasqua.AddDays(1);

            // Controlla Pasqua e Pasquetta
            if (dt.Date == pasqua.Date) return true;
            if (dt.Date == pasquetta.Date) return true;

            // 3. Controlla la Domenica
            if (dt.DayOfWeek == DayOfWeek.Sunday) return true;

            // Nota: Se devi gestire il Santo Patrono locale, aggiungi qui la data specifica
            // es: if (dt.Day == 24 && dt.Month == 6) return true; // San Giovanni

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalva_Click(object sender, EventArgs e)
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

                //if (Session["ListaDipendentiTurni"] == null)
                //{
                //    // Rilanciamo l'algoritmo completo
                //     //listaDaSalvare = ElaboraDati(dtDipendenti, mappaGiorniQuartina, anno, mese);
                //}
                //else
                //{
                foreach (DataRow row in dtDipendenti.Rows)
                {
                    DipendenteTurno dip = new DipendenteTurno();
                    dip.Matricola = row["matricola"].ToString().Trim();
                    dip.Nominativo = row["nominativo"].ToString().Trim();
                    dip.Ufficio = row["ufficio"].ToString().Trim();

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

                lblError.Text = "✅ Salvataggio completato con successo!";
                lblError.ForeColor = System.Drawing.Color.Green;
                RecalcolaPercentuali(listaDaSalvare, giorniMese);
                GeneraHtml(listaDaSalvare, anno, mese);
                Session.Remove("ListaDipendentiTurni");
            }
            catch (Exception ex)
            {
                lblError.Text = "❌ Errore durante il salvataggio: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
        // Piccolo helper per aggiornare le percentuali nel model prima di ridisegnare la tabella dopo il salvataggio
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

        protected void btnVisualizzaDB_Click(object sender, EventArgs e)
        {
            try
            {
                int anno = Convert.ToInt32(txtAnno.Text);
                int mese = Convert.ToInt32(ddlMese.SelectedValue);
                Manager mn = new Manager();
                // 1. Recupera i dati dal DB (Unione tra Anagrafica e Turni Salvati)
                List<DipendenteTurno> listaDalDB = mn.GetTurniMensile(anno, ddlMese.SelectedItem.Text);

                if (listaDalDB.Count == 0)
                {
                    lblError.Text = "⚠️ Nessun turno trovato nel database per questo periodo.";
                    lblError.ForeColor = System.Drawing.Color.Orange;
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
            }
            catch (Exception ex)
            {
                lblError.Text = "❌ Errore caricamento: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
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
                if (listaDati.Count == 0)
                {
                    lblError.Text = "⚠️ Nessun dato da esportare.";
                    return;
                }

                // 2. CREA IL WORKBOOK
                using (var wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add($"Turni {mese}-{anno}");
                    var itCulture = CultureInfo.GetCultureInfo("it-IT"); // Per i nomi giorni in Italiano

                    // --- STILI ---
                    var stileCellaBase = wb.Style;
                    stileCellaBase.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    stileCellaBase.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    stileCellaBase.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    stileCellaBase.Font.FontSize = 10;

                    // --- RIGA 1: INTESTAZIONI ---
                    int riga = 1;

                    // Colonne Fisse (Senza Matricola)
                    ws.Cell(riga, 1).Value = "Nominativo";
                    ws.Cell(riga, 2).Value = "Q";

                    // Stile Intestazione Fissa
                    var headFixed = ws.Range(riga, 1, riga, 2);
                    headFixed.Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
                    headFixed.Style.Font.FontColor = XLColor.White;
                    headFixed.Style.Font.Bold = true;
                    headFixed.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    // Colonne Giorni
                    for (int i = 1; i <= giorniMese; i++)
                    {
                        var dt = new DateTime(anno, mese, i);

                        // Calcolo lettera giorno (es. "lun" -> "L")
                        string nomeGiorno = dt.ToString("ddd", itCulture).ToUpper(); // LUN, MAR...
                        string lettera = nomeGiorno.Substring(0, 1);

                        // Scrittura Header: "1 L"
                        var cella = ws.Cell(riga, 2 + i); // Offset di 2 colonne (Nom, Q)
                        cella.Value = $"{i} {lettera}";

                        // Colora Header Festivi/Weekend
                        if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday || IsGiornoFestivo(dt))
                        {
                            cella.Style.Fill.BackgroundColor = XLColor.DarkRed;
                        }
                        else
                        {
                            cella.Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
                        }
                        cella.Style.Font.FontColor = XLColor.White;
                        cella.Style.Font.Bold = true;
                        cella.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cella.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }

                    // --- CORPO DEL REPORT ---
                    var gruppi = listaDati.GroupBy(x => x.Ufficio).OrderBy(k => k.Key);

                    riga++;

                    foreach (var gruppo in gruppi)
                    {
                        // RIGA UFFICIO (Colspan adattato: giorniMese + 2 colonne fisse)
                        var cellaUfficio = ws.Range(riga, 1, riga, 2 + giorniMese);
                        cellaUfficio.Merge();
                        cellaUfficio.Value = "📂 " + gruppo.Key.ToUpper();
                        cellaUfficio.Style.Fill.BackgroundColor = XLColor.LightGray;
                        cellaUfficio.Style.Font.Bold = true;
                        cellaUfficio.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        riga++;

                        foreach (var dip in gruppo)
                        {
                            // 1. NOMINATIVO
                            ws.Cell(riga, 1).Value = dip.Nominativo;
                            ws.Cell(riga, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left; // Nome a sx

                            // 2. QUARTINA
                            ws.Cell(riga, 2).Value = "Q" + dip.QuartinaID;

                            // Bordo per anagrafica
                            ws.Range(riga, 1, riga, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            // 3. GIORNI
                            for (int i = 1; i <= giorniMese; i++)
                            {
                                var cella = ws.Cell(riga, 2 + i); // Offset 2
                                string turno = dip.TurniMensili[i] ?? "";

                                cella.Value = turno;
                                cella.Style = stileCellaBase; // Applica stile centrato con bordi

                                // --- COLORI (Uguali al web) ---
                                if (turno == "Q")
                                {
                                    cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#ffcdd2");
                                    cella.Style.Font.FontColor = XLColor.DarkRed;
                                    cella.Style.Font.Bold = true;
                                }
                                else if (turno == "1")
                                {
                                    cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#e3f2fd");
                                    cella.Style.Font.FontColor = XLColor.DarkBlue;
                                    cella.Style.Font.Bold = true;
                                }
                                else if (turno == "2")
                                {
                                    cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#fff8e1");
                                    cella.Style.Font.FontColor = XLColor.FromHtml("#e65100");
                                    cella.Style.Font.Bold = true;
                                }
                                else if (turno == "RF")
                                {
                                    cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#c8e6c9");
                                    cella.Style.Font.FontColor = XLColor.DarkGreen;
                                    cella.Style.Font.Bold = true;
                                }
                                else
                                {
                                    // Celle vuote weekend grigie
                                    DateTime dt = new DateTime(anno, mese, i);
                                    if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                                    {
                                        cella.Style.Fill.BackgroundColor = XLColor.FromHtml("#f2f2f2");
                                    }
                                }
                            }
                            riga++;
                        }
                    }

                    // --- FORMATTAZIONE LARGHEZZE ---
                    ws.Column(1).Width = 35;  // Nominativo più largo
                    ws.Column(2).Width = 5;   // Q stretta

                    // Colonne Giorni
                    for (int i = 1; i <= giorniMese; i++)
                    {
                        ws.Column(2 + i).Width = 4; // Larghezza fissa per i giorni
                    }

                    // --- DOWNLOAD ---
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", $"attachment;filename=Turni_{mese}_{anno}.xlsx");

                    using (MemoryStream ms = new MemoryStream())
                    {
                        wb.SaveAs(ms);
                        ms.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.SuppressContent = true;
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Errore Excel: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void btnExportPdf_Click(object sender, EventArgs e)
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
                if (listaDati.Count == 0)
                {
                    lblError.Text = "⚠️ Nessun dato da stampare.";
                    return;
                }

                // Configurazione memoria per il file
                using (MemoryStream stream = new MemoryStream())
                {
                    // 2. INIZIALIZZA IL DOCUMENTO PDF
                    PdfWriter writer = new PdfWriter(stream);
                    PdfDocument pdf = new PdfDocument(writer);

                    // Imposta pagina ORIZZONTALE (Landscape) per far entrare 31 giorni
                    pdf.SetDefaultPageSize(PageSize.A4.Rotate());

                    Document document = new Document(pdf);
                    document.SetMargins(10, 10, 10, 10); // Margini stretti

                    // Font piccolo per far stare tutto
                    PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    float fontSize = 7f; // Molto piccolo ma leggibile

                    // Titolo
                    // --- DEFINIZIONE FONT (Metodo Robusto) ---
                    PdfFont fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD); // Grassetto esplicito

                    Paragraph titolo = new Paragraph($"TURNI DEL MESE: {mese}/{anno}")
                        .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fontBold)
                        .SetFontSize(14);

                    //.SetBold();

                    document.Add(titolo);

                    // 3. DEFINIZIONE TABELLA
                    // Colonne: Nominativo (larga), Q (stretta), 31 giorni (stretti uguali)
                    // Usiamo pesi relativi per le larghezze
                    float[] colWidths = new float[2 + giorniMese];
                    colWidths[0] = 10f; // Nominativo largo
                    colWidths[1] = 2f;  // Q stretta
                    for (int i = 0; i < giorniMese; i++) colWidths[2 + i] = 1.3f; // Giorni

                    Table table = new Table(UnitValue.CreatePercentArray(colWidths));
                    table.SetWidth(UnitValue.CreatePercentValue(100)); // Tabella larga 100% pagina

                    // --- HEADER (RIGA 1) ---
                    // Intestazioni Fisse
                    AddCellHeader(table, "Nominativo", ColorConstants.DARK_GRAY, fontBold);
                    AddCellHeader(table, "Q", ColorConstants.DARK_GRAY, fontBold);

                    var itCulture = CultureInfo.GetCultureInfo("it-IT");

                    for (int i = 1; i <= giorniMese; i++)
                    {
                        var dt = new DateTime(anno, mese, i);
                        string lettera = dt.ToString("ddd", itCulture).Substring(0, 1).ToUpper();
                        string testo = $"{i}\n{lettera}";

                        // Colore Header (Weekend Rosso, Feriale Grigio)
                        Color bgColor = ColorConstants.DARK_GRAY;
                        if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday || IsGiornoFestivo(dt))
                        {
                            bgColor = new DeviceRgb(139, 0, 0); // Rosso Scuro
                        }

                        AddCellHeader(table, testo, bgColor, fontBold);
                    }

                    // --- CORPO DEL REPORT ---
                    var gruppi = listaDati.GroupBy(x => x.Ufficio).OrderBy(k => k.Key);

                    foreach (var gruppo in gruppi)
                    {
                        // RIGA GRUPPO UFFICIO
                        Cell cellUfficio = new Cell(1, 2 + giorniMese) // Colspan
                            .Add(new Paragraph("📂 " + gruppo.Key.ToUpper()))
                            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                            .SetFont(font)
                            .SetFontSize(8)
                            .SetFont(fontBold)
                            .SetPadding(2);
                        table.AddCell(cellUfficio);

                        foreach (var dip in gruppo)
                        {
                            // Anagrafica
                            AddCellBody(table, dip.Nominativo, ColorConstants.WHITE, font, fontSize, TextAlignment.LEFT);
                            AddCellBody(table, "Q" + dip.QuartinaID, ColorConstants.WHITE, font, fontSize, TextAlignment.CENTER);

                            // Giorni
                            for (int i = 1; i <= giorniMese; i++)
                            {
                                string turno = dip.TurniMensili[i] ?? "";

                                // Mappatura Colori (uguali all'HTML/Excel)
                                Color bgCell = ColorConstants.WHITE;
                                Color txtColor = ColorConstants.BLACK;

                                if (turno == "Q")
                                {
                                    bgCell = new DeviceRgb(255, 205, 210); // #ffcdd2 Rosso chiaro
                                    txtColor = new DeviceRgb(183, 28, 28); // Rosso scuro
                                }
                                else if (turno == "1")
                                {
                                    bgCell = new DeviceRgb(227, 242, 253); // #e3f2fd Blu chiaro
                                    txtColor = new DeviceRgb(13, 71, 161); // Blu scuro
                                }
                                else if (turno == "2")
                                {
                                    bgCell = new DeviceRgb(255, 248, 225); // #fff8e1 Arancio chiaro
                                    txtColor = new DeviceRgb(230, 81, 0);  // Arancio scuro
                                }
                                else if (turno == "RF")
                                {
                                    bgCell = new DeviceRgb(200, 230, 201); // #c8e6c9 Verde chiaro
                                    txtColor = new DeviceRgb(27, 94, 32);  // Verde scuro
                                }
                                else
                                {
                                    // Celle vuote weekend grigine
                                    DateTime dt = new DateTime(anno, mese, i);
                                    if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                                    {
                                        bgCell = new DeviceRgb(242, 242, 242);
                                    }
                                }

                                Cell c = new Cell().Add(new Paragraph(turno));
                                c.SetBackgroundColor(bgCell);
                                c.SetFontColor(txtColor);
                                c.SetFont(font).SetFontSize(fontSize);
                                c.SetTextAlignment(TextAlignment.CENTER);
                                c.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                                c.SetPadding(0); // Compatta
                                table.AddCell(c);
                            }
                        }
                    }

                    document.Add(table);
                    document.Close();

                    // 4. INVIO FILE AL BROWSER
                    byte[] bytes = stream.ToArray();
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment; filename=Turni_{mese}_{anno}.pdf");
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.SuppressContent = true;
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Errore PDF: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
        // --- HELPER CORRETTO ---
        // Accetta il font come parametro invece di chiamare .SetBold()
        private void AddCellHeader(Table table, string text, Color bgColor, PdfFont font)
        {
            Cell c = new Cell().Add(new Paragraph(text));
            c.SetBackgroundColor(bgColor);
            c.SetFontColor(ColorConstants.WHITE);
            c.SetFont(font); // Qui applico Helvetica Bold
            c.SetFontSize(7);
            c.SetTextAlignment(TextAlignment.CENTER);
            c.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            table.AddCell(c);
        }

        private void AddCellBody(Table table, string text, Color bgColor, PdfFont font, float fontSize, TextAlignment align)
        {
            Cell c = new Cell().Add(new Paragraph(text));
            c.SetBackgroundColor(bgColor);
            c.SetFont(font);
            c.SetFontSize(fontSize);
            c.SetTextAlignment(align);
            c.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            c.SetPaddingLeft(2);
            table.AddCell(c);
        }
        private void EseguiPreBilanciamentoSabati(List<DipendenteTurno> gruppo, int giorniMese)
        {
            // Scorre tutti i giorni (inclusi i sabati ancorati dalla Fase 1)
            for (int k = 1; k <= giorniMese; k++)
            {
                // 1. Conta chi è GIA' fissato su 1 e 2
                var fissatiSu1 = gruppo.Where(d => d.TurniMensili[k] == "1").ToList();
                var fissatiSu2 = gruppo.Where(d => d.TurniMensili[k] == "2").ToList();

                // Se non c'è nessuno fissato (giorno vuoto) o se c'è già equilibrio, salta.
                if ((fissatiSu1.Count == 0 && fissatiSu2.Count == 0) ||
                    (fissatiSu1.Count > 0 && fissatiSu2.Count > 0))
                {
                    continue;
                }

                // --- CASO: TUTTI SU 1 ---
                // Se ci sono più di 2 persone e sono tutte sull'1...
                if (fissatiSu1.Count > 1 && fissatiSu2.Count == 0)
                {
                    int daSpostare = Math.Max(1, fissatiSu1.Count / 2); // Sposta il 50%

                    // Scegliamo chi spostare:
                    // 1. Priorità Autisti (se ce ne sono, per coprire il turno 2)
                    // 2. Chi ha più % di Turno 1 (così gli facciamo un favore dandogli il 2)
                    var candidati = fissatiSu1
                        .OrderByDescending(d => d.IsAutista)
                        .ThenByDescending(d => GetPercTurno1Attuale(d.TurniMensili, k))
                        .ToList();

                    for (int x = 0; x < daSpostare; x++)
                    {
                        candidati[x].TurniMensili[k] = "2"; // Forza spostamento
                    }
                }

                // --- CASO: TUTTI SU 2 ---
                else if (fissatiSu2.Count > 1 && fissatiSu1.Count == 0)
                {
                    int daSpostare = Math.Max(1, fissatiSu2.Count / 2);

                    var candidati = fissatiSu2
                        .OrderByDescending(d => d.IsAutista)
                        .ThenBy(d => GetPercTurno1Attuale(d.TurniMensili, k)) // Chi ha pochi "1"
                        .ToList();

                    for (int x = 0; x < daSpostare; x++)
                    {
                        candidati[x].TurniMensili[k] = "1"; // Forza spostamento
                    }
                }
            }
        }
    }
}