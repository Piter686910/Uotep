using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using Uotep.Classi;

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
                    // NUOVO ALGORITMO: Copertura Autista Obbligatoria
                    RiempimentoUfficioConAutista(dipsDelGruppo, giorniMese);
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


        // --- LOGICA MACRO AREE CORRETTA (Min 2 Dipendenti) ---
        // Priorità: 
        // 1. Presenza Autista (Bloccante)
        // 2. Minimo 2 Dipendenti per turno (Operativo)
        // 3. Ratio 50% (Bilanciamento)
        private void RiempimentoUfficioConAutista(List<DipendenteTurno> gruppo, int giorniMese)
        {
            for (int i = 1; i <= giorniMese; i++)
            {
                // ---------------------------------------------------
                // 1. FOTOGRAFIA INIZIALE DEL GIORNO
                // ---------------------------------------------------
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
                    // Ignoriamo Q e RF ai fini del conteggio autisti attivi
                }

                // ---------------------------------------------------
                // 2. GESTIONE CONSECUTIVI (Safety First)
                // ---------------------------------------------------
                var poolLavoro = new List<DipendenteTurno>();

                foreach (var dip in liberi)
                {
                    string p1 = (i > 1) ? dip.TurniMensili[i - 1] : "";
                    string p2 = (i > 2) ? dip.TurniMensili[i - 2] : "";

                    bool no1 = (p1 == "1" && p2 == "1");
                    bool no2 = (p1 == "2" && p2 == "2");

                    // Assegnazione forzata per consecutivi
                    if (no1 && !no2)
                    {
                        dip.TurniMensili[i] = "2";
                        count2++;
                        if (dip.IsAutista) hasAutista2 = true;
                    }
                    else if (no2 && !no1)
                    {
                        dip.TurniMensili[i] = "1";
                        count1++;
                        if (dip.IsAutista) hasAutista1 = true;
                    }
                    else
                    {
                        // Se bloccato da entrambi o libero, lo gestiamo dopo
                        poolLavoro.Add(dip);
                    }
                }

                // Separiamo temporaneamente gli autisti per garantire copertura
                var autistiDisponibili = poolLavoro.Where(d => d.IsAutista).ToList();

                // RIMUOVIAMO gli autisti dalla pool generale per processarli PRIMA
                foreach (var a in autistiDisponibili) poolLavoro.Remove(a);

                // ---------------------------------------------------
                // 3. PIAZZAMENTO AUTISTI (Priorità: Coprire i turni scoperti)
                // ---------------------------------------------------

                // Serve autista su 1?
                if (!hasAutista1 && autistiDisponibili.Count > 0)
                {
                    var a = autistiDisponibili.OrderBy(x => GetPercTurno1Attuale(x.TurniMensili, i)).First();
                    a.TurniMensili[i] = "1";
                    hasAutista1 = true; count1++;
                    autistiDisponibili.Remove(a);
                }

                // Serve autista su 2?
                if (!hasAutista2 && autistiDisponibili.Count > 0)
                {
                    var a = autistiDisponibili.OrderByDescending(x => GetPercTurno1Attuale(x.TurniMensili, i)).First();
                    a.TurniMensili[i] = "2";
                    hasAutista2 = true; count2++;
                    autistiDisponibili.Remove(a);
                }

                // Reimmettiamo gli autisti avanzati nel pool generale
                poolLavoro.AddRange(autistiDisponibili);

                // ---------------------------------------------------
                // 4. CICLO DI ASSEGNAZIONE DEFINITIVO (Decision Tree)
                // ---------------------------------------------------

                // Ordiniamo per chi ha più bisogno di bilanciamento personale
                // Questo determina CHI processiamo, ma il DOVE dipenderà dai vincoli
                var coda = poolLavoro.Select(d => new {
                    Dip = d,
                    Perc = GetPercTurno1Attuale(d.TurniMensili, i),
                    IsDriver = d.IsAutista
                }).ToList();

                while (coda.Count > 0)
                {
                    // Processiamo prima chi è più sbilanciato (lontano dal 50%)
                    var item = coda.OrderByDescending(x => Math.Abs(x.Perc - 50.0)).First();

                    // ANALISI DI FATTIBILITÀ (Semafori)
                    // Un non-autista può andare sul turno X solo se c'è un autista
                    bool canGo1 = hasAutista1 || item.IsDriver;
                    bool canGo2 = hasAutista2 || item.IsDriver;

                    string decisione = null;

                    // --- LIVELLO 1: IMPOSSIBILITÀ TECNICA ---
                    if (!canGo1 && !canGo2)
                    {
                        // Disastro: non ci sono autisti su nessun turno.
                        // Assegniamo "RF" (Riposo Forzato) o "1" come default emergenza
                        decisione = "RF";
                    }
                    // --- LIVELLO 2: SCELTA OBBLIGATA (Solo una strada aperta) ---
                    else if (canGo1 && !canGo2) decisione = "1";
                    else if (!canGo1 && canGo2) decisione = "2";

                    // --- LIVELLO 3: SCELTA LIBERA (Entrambe le strade aperte) ---
                    else // (canGo1 && canGo2)
                    {
                        // Qui decidiamo in base alle priorità numeriche

                        // A. Minimo Sindacale (2 persone)
                        if (count1 < 2 && count2 >= 2) decisione = "1";
                        else if (count2 < 2 && count1 >= 2) decisione = "2";
                        else if (count1 < 2 && count2 < 2)
                        {
                            // Mancano a entrambi: seguo la preferenza personale
                            decisione = (item.Perc < 50.0) ? "1" : "2";
                        }

                        // B. Bilanciamento Numerico (Evitare 5 persone su un turno e 2 sull'altro)
                        // Se count1 supera count2 di 1 unità o più, mandiamo al 2
                        else if (count1 > count2) decisione = "2";
                        else if (count2 > count1) decisione = "1";

                        // C. Bilanciamento Personale (Se i numeri sono pari)
                        else
                        {
                            decisione = (item.Perc < 50.0) ? "1" : "2";
                        }
                    }

                    // APPLICAZIONE
                    item.Dip.TurniMensili[i] = decisione;

                    if (decisione == "1")
                    {
                        count1++;
                        if (item.IsDriver) hasAutista1 = true; // Apre la porta per i successivi
                    }
                    else if (decisione == "2")
                    {
                        count2++;
                        if (item.IsDriver) hasAutista2 = true;
                    }

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
                        dip.Ufficio= row["ufficio"].ToString().Trim();

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
        // Metodo per estrarre e mappare i dati SQL
        private List<DipendenteTurno> GetTurniSalvati(int anno, int mese)
        {
            List<DipendenteTurno> lista = new List<DipendenteTurno>();
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["TuaConnectionString"].ConnectionString;

            // QUERY:
            // Prendo TUTTI i dipendenti attivi.
            // Faccio LEFT JOIN con i turni per il mese specifico.
            // In questo modo ottengo Nome, Ufficio, Autista (dall'anagrafica) 
            // e i turni (dallo storico).
            string query = @"
        SELECT 
            d.matricola, 
            d.nominativo, 
            d.ufficio, 
            d.quartina, 
            d.autista,
            t.giorno, 
            t.CodiceTurno
        FROM Dipendenti d
        LEFT JOIN TurniMensile t 
            ON d.matricola = t.matricola 
            AND t.anno = @anno 
            AND t.mese = @mese
        ORDER BY d.ufficio, d.nominativo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@anno", anno);
                cmd.Parameters.AddWithValue("@mese", mese);

                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    // Usiamo un dizionario temporaneo per raggruppare le righe SQL
                    // (La query restituisce N righe per ogni dipendente, una per ogni giorno salvato)
                    var dictDipendenti = new Dictionary<string, DipendenteTurno>();

                    while (r.Read())
                    {
                        string matricola = r["matricola"].ToString().Trim();

                        // Se è la prima volta che incontro questa matricola, creo l'oggetto
                        if (!dictDipendenti.ContainsKey(matricola))
                        {
                            DipendenteTurno dip = new DipendenteTurno();
                            dip.Matricola = matricola;
                            dip.Nominativo = r["nominativo"].ToString();
                            dip.Ufficio = r["ufficio"] != DBNull.Value ? r["ufficio"].ToString() : "Nessun Ufficio";

                            // Gestione Autista
                            if (r["autista"] != DBNull.Value)
                                dip.IsAutista = Convert.ToBoolean(r["autista"]);
                            else
                                dip.IsAutista = false;

                            // Gestione Quartina
                            if (r["quartina"] != DBNull.Value)
                                dip.QuartinaID = Convert.ToInt32(r["quartina"]);
                            else
                                dip.QuartinaID = 0;

                            // Inizializza array vuoto (1-31)
                            dip.TurniMensili = new string[32];

                            dictDipendenti.Add(matricola, dip);
                        }

                        // Ora popolo il giorno specifico, se presente nel DB
                        if (r["giorno"] != DBNull.Value && r["CodiceTurno"] != DBNull.Value)
                        {
                            int giorno = Convert.ToInt32(r["giorno"]);
                            string turno = r["CodiceTurno"].ToString().Trim().ToUpper();

                            // Controllo di sicurezza sull'indice array
                            if (giorno >= 1 && giorno <= 31)
                            {
                                dictDipendenti[matricola].TurniMensili[giorno] = turno;
                            }
                        }
                    }

                    // Convertiamo i valori del dizionario in Lista
                    lista = dictDipendenti.Values.ToList();
                }
            }
            return lista;
        }
    }
}