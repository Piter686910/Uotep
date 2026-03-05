using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.CustomXmlSchemaReferences;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Ajax.Utilities;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Interop;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static System.Windows.Forms.AxHost;
using static Uote.test;
using static Uotep.Classi.Enumerate;
using DataTable = System.Data.DataTable;

namespace Uotep.Classi
{
    public class Manager
    {

        //public String ConnString = ConfigurationManager.AppSettings["ConnString"];
        public String ConnString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
        public String ConnStringTp = ConfigurationManager.ConnectionStrings["ConnStringTp"].ToString();
        public String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        public string msg = string.Empty;
        //delete
        public Boolean DeleteTran(String numero_pratica)
        {

            String Del_ana = "delete from anagrafica1 where numero_pratica_accertamenti = '" + numero_pratica + "'";
            String Del_acc = "delete from Accertamenti where num_pratica_accertamenti = '" + numero_pratica + "'";
            String testoSql = String.Empty;
            //OleDbTransaction Tran;
            //OleDbConnection conn = new OleDbConnection(ConnString);
            //Tran = conn.BeginTransaction();
            // conn.Open();
            //OleDbCommand cmd;
            Boolean resp = false;

            using (SqlConnection conn1 = new SqlConnection(ConnString))
            {
                conn1.Open();
                SqlCommand command = conn1.CreateCommand();
                SqlTransaction tran;
                tran = conn1.BeginTransaction("trans");
                command.Transaction = tran;
                try
                {

                    command.CommandText = Del_ana;
                    testoSql = "Anagrafica";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                    {
                        command.CommandText = Del_acc;
                        //comm.Parameters.AddWithValue("@LastRun", DateTime.Now);
                        testoSql = "Accertamenti";
                        command.ExecuteNonQuery();

                        tran.Commit();
                        tran.Dispose();
                        resp = true;
                    }
                    else
                    {
                        //MessageBox.Show("La pratica selezionata non esiste in archivio", "Attenzione!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                }

                catch (Exception)
                {

                    tran.Rollback();

                    resp = false;


                }
                conn1.Close();
                return resp;
            }
        }
        //delete
        public Boolean DeleteRappUote(String numero_pratica)
        {

            String Del_RappUote = "delete from RappUote where rapp_numero_pratica = '" + numero_pratica + "'";

            String testoSql = String.Empty;

            Boolean resp = false;

            using (SqlConnection conn1 = new SqlConnection(ConnString))
            {
                conn1.Open();
                SqlCommand command = conn1.CreateCommand();

                try
                {

                    command.CommandText = Del_RappUote;
                    testoSql = "RappUote";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                    {

                        resp = true;
                    }


                }

                catch (Exception)
                {
                    resp = false;
                }
                conn1.Close();
                return resp;
            }
        }
        public Boolean DeleteMatricola(String matricola)
        {

            String Del_operatore = "delete from operatore where matricola = '" + matricola + "'";

            String testoSql = String.Empty;

            Boolean resp = false;

            using (SqlConnection conn1 = new SqlConnection(ConnString))
            {
                conn1.Open();
                SqlCommand command = conn1.CreateCommand();

                try
                {

                    command.CommandText = Del_operatore;
                    testoSql = "operatore";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn1.Close();
                return resp;
            }
        }
        /// <summary>
        /// cancella i file per nome
        /// </summary>
        /// <param name="nomefile"></param>
        /// <returns></returns>
        public Boolean DeleteFileCaricati(String nomefile)
        {

            String Del_FileCaricati = "delete from File_Caricati where nomefile = '" + nomefile + "'";

            String testoSql = String.Empty;

            Boolean resp = false;

            using (SqlConnection conn1 = new SqlConnection(ConnString))
            {
                conn1.Open();
                SqlCommand command = conn1.CreateCommand();

                try
                {

                    command.CommandText = Del_FileCaricati;
                    testoSql = "FileCaricati";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn1.Close();
                return resp;
            }
        }
        public Boolean DelInterrogatorioById(int id)
        {
            string sql = string.Empty;
            string Del_Interrogatori = "delete  FROM Interrogatori where id = " + id;
            Boolean resp = false;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                try
                {

                    command.CommandText = Del_Interrogatori;
                    //testoSql = "Registro";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }
        public Boolean DelRegistroById(int id)
        {
            string sql = string.Empty;
            string Del_Registro = "delete  FROM RegistroUrp where id_registro = " + id;
            Boolean resp = false;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                try
                {

                    command.CommandText = Del_Registro;
                    //testoSql = "Registro";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }
        /// <summary>
        /// cancella i file con flag cancella a true
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteFileScaricati()
        {
            String Del_FileCaricati = "delete from File_Caricati where cancella = 'True'";

            String testoSql = String.Empty;
            Boolean resp = false;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                try
                {

                    command.CommandText = Del_FileCaricati;
                    testoSql = "FileCaricati";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }

        /// <summary>
        /// delete la tabella rsnl
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteRSNL()
        {
            String Del_FileCaricati = "delete from rsnl ";

            String testoSql = String.Empty;
            Boolean resp = false;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                try
                {

                    command.CommandText = Del_FileCaricati;
                    testoSql = "rsnl";
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                        resp = true;
                }

                catch (Exception)
                {
                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }
        //get
        public DataTable getPass(String user)
        {

            DataTable tb = new DataTable();
            string sql = "SELECT passw FROM Operatore where Matricola= '" + user + "'";
            //string sql = "SELECT * FROM Operatore where Matricola= '" + user + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }


        }
        public DataTable GetRuolo(String user)
        {

            DataTable tb = new DataTable();
            string sql = "SELECT profilo,ruolo,area,macroarea FROM Operatore where Matricola= '" + user + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn, out msg);
            }


        }
        public DataTable getUserByUserPassw(String user, string pasw)
        {

            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Operatore where Matricola= '" + user + "' and passw = '" + pasw + "'";
            //string sql = "SELECT * FROM Operatore where Matricola= '" + user + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn, out msg);
            }


        }
        public DataTable getUserRules(String user)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Operatore where Matricola= '" + user + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn, out msg);
            }


        }
        public DataTable getListGiudice(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Giudice order by Giudice";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListProvenienza(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Provenienza order by Provenienza";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListTipologiaAbuso(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM TipologiaAbuso order by tipologia";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListTipologia(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Tipologia order by tipo_nota";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListScaturito(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Scaturito order by scaturito";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListRicercaEsitoUrp(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM EsitoUrp order by descrizione";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListQuartiere(out string msg)
        {
            DataTable tb = new DataTable();
            // string sql = "SELECT distinct quartiere FROM Quart order by quartiere";
            string sql = "SELECT MIN(id_quartiere) AS id_quartiere, quartiere FROM Quart GROUP BY quartiere ORDER BY quartiere";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListQuartiereTP(out string msg)
        {
            DataTable tb = new DataTable();
            // string sql = "SELECT distinct quartiere FROM Quart order by quartiere";
            string sql = "SELECT id, quartiere FROM Quartiere ORDER BY quartiere";
            using (SqlConnection conn = new SqlConnection(ConnStringTp))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getQuartiere(string indirizzo, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Quart where toponimo like '%" + indirizzo.Replace("'", "''") + "%'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable MaxNPr(string anno)
        {
            DataTable tb = new DataTable();

            //string sql = " SELECT anno, MAX(CAST(nr_protocollo AS INT)) AS MaxNumero FROM principale WHERE ISNUMERIC(nr_protocollo) = 1 AND ANNO ='" + anno + "'";
            string sql = "SELECT ANNO, MAX(CAST(nr_protocollo AS INT)) AS MaxNumero FROM principale WHERE ISNUMERIC(nr_protocollo) = 1 AND ANNO = '" + anno + "' GROUP BY ANNO";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da;
                DataSet ds;
                da = new SqlDataAdapter(sql, conn);
                ds = new DataSet();
                da.Fill(ds);

                tb = ds.Tables[0];
                conn.Close();
                conn.Dispose();
                return tb;
            }
        }
        /// <summary>
        /// max num pratica tp +1
        /// </summary>
        /// <returns></returns>
        public DataTable MaxNPrTp()
        {
            DataTable tb = new DataTable();

            //string sql = " SELECT anno, MAX(CAST(nr_protocollo AS INT)) AS MaxNumero FROM principale WHERE ISNUMERIC(nr_protocollo) = 1 AND ANNO ='" + anno + "'";
            string sql = "SELECT MAX(Num_Prot) FROM ArchivioTp";
            using (SqlConnection conn = new SqlConnection(ConnStringTp))
            {
                SqlDataAdapter da;
                DataSet ds;
                da = new SqlDataAdapter(sql, conn);
                ds = new DataSet();
                da.Fill(ds);

                tb = ds.Tables[0];
                conn.Close();
                conn.Dispose();
                return tb;
            }
        }
        public DataTable getListQuartina(Int32 anno)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT anno as anno,quartina as quartina,gennaio as gennaio,febbraio as febbraio, marzo as marzo, aprile as aprile, maggio as maggio, giugno as giugno, luglio as luglio, agosto as agosto, settembre as settembre, ottobre as ottobre, novembre as novembre,dicembre as dicembre FROM quartina where anno = " + anno;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ottiene nominativo operatore dalla matricola
        /// </summary>
        /// <param name="matricola"></param>
        /// <returns></returns>
        public DataTable getNominativoOperatore(string matricola)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT nominativo FROM operatore where matricola = '" + matricola.Replace("'", "''") + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ottiene la matricola dal nominativo
        /// </summary>
        /// <param name="nominativo"></param>
        /// <returns></returns>
        public DataTable getMatricolaOperatore(string nominativo)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT matricola FROM operatore where nominativo = '" + nominativo.Replace("'", "''") + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListOperatore(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT nominativo FROM operatore where nominativo <> '' order by nominativo ";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca solo gli accertatori
        /// </summary>
        /// <returns></returns>
        public DataTable getListAccertatori(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT nominativo FROM operatore where ruolo = '" + Enumerate.Ruolo.accertatori.GetDescription() + "' or ruolo= '" + Enumerate.Ruolo.PG.GetDescription() + "' order by nominativo ";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable GetListRegistro(out string msg)
        {
            DataTable dt = new DataTable();

            string sql = "SELECT oggetto,dataPresentRichiesta, nrPgTrasmissioneRichiesto, uffDetentore,controInteressati,esito,motivazione, nrPgTrasmissioneRiscontro, dataConclProcedimento FROM RegistroUrp " +
    " ORDER BY dataConclProcedimento";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return dt = FillTable(sql, conn, out msg);
            }

        }
        public List<DipendenteTurno> GetTurniMensile(int anno, string mese)
        {
            List<DipendenteTurno> lista = new List<DipendenteTurno>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                // QUERY:
                // Prendo TUTTI i dipendenti attivi.
                // Faccio LEFT JOIN con i turni per il mese specifico.
                // In questo modo ottengo Nome, Ufficio, Autista (dall'anagrafica) 
                // e i turni (dallo storico).
                string query = @"
        SELECT 
            d.matricola_ced, 
            d.nominativo, 
            d.ufficio, 
            d.quartina, 
            d.autista,
            t.giorno, 
            t.CodiceTurno,
            d.gruppo_quartina,
            d.area
        FROM SchedaDipendente d
        LEFT JOIN TurniMensile t 
            ON d.matricola_ced = t.matricola 
            AND t.anno = @anno 
            AND t.mese = @mese
        ORDER BY d.ufficio, d.nominativo";
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
                        string matricola = r["matricola_ced"].ToString().Trim();

                        // Se è la prima volta che incontro questa matricola, creo l'oggetto
                        if (!dictDipendenti.ContainsKey(matricola))
                        {
                            DipendenteTurno dip = new DipendenteTurno();
                            dip.Matricola = matricola;
                            dip.Nominativo = r["nominativo"].ToString();
                            dip.Gruppo = r["gruppo_quartina"].ToString();
                            dip.Ufficio = r["ufficio"] != DBNull.Value ? r["ufficio"].ToString() : "Nessun Ufficio";
                            dip.Area = r["area"] != DBNull.Value ? r["area"].ToString() : "Nessuna Area";
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
        public DataTable getListDipendenti()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT ufficio, nominativo,matricola_ced, grado, data_assunzione, id_dip, autista, armato, quartina,gruppo_quartina,gruppo_reperibilita,permessi_studio," +
                         "perm_53,perm_104,limitazioni,turni_pref,Macro_area,area,data_sorv_sanitaria FROM SchedaDipendente  ORDER BY ufficio,nominativo";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListDipendentiById(int id)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT ufficio, nominativo,matricola, grado, data_assunzione, id_dip, autista, armato, quartina,gruppo_quartina,gruppo_reperibilita,permessi_studio," +
                         "perm_53,perm_104,limitazioni,turni_pref,turni_blocc,Macro_area,area,data_sorv_sanitaria FROM SchedaDipendente  ORDER BY ufficio,nominativo";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn, out msg);
            }
        }
        public Boolean getTipoProv(string tipo)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM TipoNotaAG where tipologia = '" + tipo + "'";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da;
                DataSet ds;

                da = new SqlDataAdapter(sql, conn);
                ds = new DataSet();
                da.Fill(ds);

                tb = ds.Tables[0];
                if (tb.Rows.Count > 0)
                    return true;
                else

                    return false; ;
            }

        }
        public DataTable getListProvvAg(String sigla, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM TipoNotaAG where sigla = '" + sigla + "' order by tipologia";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListInviati(out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM inviati order by inviata";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// cerca indirizzo nella tabella quartiere
        /// </summary>
        /// <returns></returns>
        public DataTable getListIndirizzo(out string msg)
        {
            DataTable tb = new DataTable();
            // string sql = "SELECT specie,toponimo  FROM Quart order by toponimo";
            string sql = "SELECT ISNULL(Specie, '') + ' ' + ISNULL(toponimo, '') AS SpecieToponimo FROM  Quart";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListPraticheVal(string pratica, string anno, out string msg)
        {
            DataTable tb = new DataTable();


            string sql = "SELECT id,nr_Pratica ,nr_protocollo, evasa,anno FROM [principale] WHERE Anno ='" + anno + "' and nr_Pratica = '" + pratica +
                                     "' GROUP BY nr_Pratica ,nr_protocollo,evasa,anno,id HAVING COUNT(Nr_Protocollo) >= 1 ORDER BY Nr_Protocollo DESC";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca per protocollo
        /// </summary>
        /// <param name="protocollo"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable getListPrototocollo(string protocollo, string anno, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola WHERE P.Nr_Protocollo = " + protocollo + " and anno = '" + anno + "' order by dataarrivo desc";
            //"SELECT * FROM Principale where Nr_Protocollo = " + protocollo + " and anno = '" + anno + "' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca le diverse decretazioni per la pratica selezionata
        /// </summary>
        /// <param name="pratica"></param>
        /// <param name="idPratica"></param>
        /// <returns></returns>
        public DataTable getListDecretazione(string pratica, string idPratica)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM decretazione where decr_pratica = '" + pratica + "' and decr_idPratica = '" + idPratica + "' order by decr_data desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca per procedimento penale
        /// </summary>
        /// <param name="procedimento"></param>

        /// <returns></returns>
        public DataTable getListProcedimento(string procedimento, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola  where ProcedimentoPen like '%" + procedimento.Replace("'", "''").Replace("*", "%") + "%' order by dataarrivo desc";
            //"SELECT * FROM Principale where ProcedimentoPen like '%" + procedimento.Replace("'", "''").Replace("*", "%") + "%' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca per evasa
        /// </summary>
        /// <param name="datada"></param>
        ///  /// <param name="dataa"></param>
        /// <returns></returns>
        public DataTable getListEvasaAg(string datada, string dataa, out string msg)
        {
            DataTable tb = new DataTable();

            DateTime dtda = System.Convert.ToDateTime(datada);
            DateTime dta = System.Convert.ToDateTime(dataa);
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola  where EvasaData BETWEEN '" + dtda.ToShortDateString() + "' and '" + dta.ToShortDateString() + "'  order by dataarrivo desc";

            //string sql = "SELECT * FROM Principale where EvasaData BETWEEN '" + dtda.ToShortDateString() + "' and '" + dta.ToShortDateString() + "'  order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca per protocollo generale
        /// </summary>
        /// <param name="protgen"></param>
        /// <returns></returns>
        public DataTable getListProtGen(string protgen, out string msg)
        {
            DataTable tb = new DataTable();


            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola  where Rif_Prot_Gen like '%" + protgen.Replace("'", "''").Replace("*", "%") + "%'  order by dataarrivo desc";

            //string sql = "SELECT * FROM Principale where Rif_Prot_Gen like '%" + protgen.Replace("'", "''").Replace("*", "%") + "%'  order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListProtGenInDecretazione(string protgen, out string msg)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT p.Id,p.Nr_Protocollo,p.Sigla,p.DataArrivo,p.Provenienza,p.Tipologia_atto,p.Giudice,p.TipoProvvedimentoAG,p.ProcedimentoPen,p.Nominativo,p.Indirizzo,p.via,p.Evasa" +
                            ",p.EvasaData,p.Inviata,p.DataInvio,p.Scaturito,p.Accertatori,p.DataCarico,p.nr_Pratica,p.Quartiere,p.Note,p.Anno,p.Giorno,p.Rif_Prot_Gen,p.Matricola,p.DataInserimento,p.Macro_area" +
                            ",p.UlterioreTipoAtto,p.BU,p.CodiceEdificio FROM principale p LEFT JOIN decretazione d ON d.decr_idPratica = p.id where " +
                            "d.decr_nota like '%" + protgen.Replace("'", "''").Replace("*", "%") + "%'" + "ORDER BY p.dataarrivo";

            // string sql = "SELECT decr_idPratica FROM decretazione where decr_nota like '%" + protgen.Replace("'", "''").Replace("*", "%") + "%'  order by decr_data desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca per giudice
        /// </summary>
        /// <param name="giudice"></param>
        /// <returns></returns>
        public DataTable getListGiudice(string giudice, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola  where giudice like '" + giudice.Replace("'", "''") + "%' order by dataarrivo desc";

            //   string sql = "SELECT * FROM Principale where giudice like '" + giudice.Replace("'", "''") + "%' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// preleva le statistiche del mese e anno selezionati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable getStatisticaByMeseAnno(string mese, int anno)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM statistiche where mese = '" + mese.ToUpper() + "' and anno =" + anno;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }

        }

        /// <summary>
        /// ricerca singolo inviata
        /// </summary>
        /// <param name="inviata"></param>
        /// <returns></returns>
        public Boolean getGiudice(string giudice)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM giudice where giudice = '" + giudice.Replace("'", "''") + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da;
                DataSet ds;

                da = new SqlDataAdapter(sql, conn);
                ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }

        }
        /// <summary>
        /// ricerca singolo inviati
        /// </summary>
        /// <param name="inviata"></param>
        /// <returns></returns>
        public Boolean getInviata(string inviata)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM inviati where inviata = '" + inviata.Replace("'", "''") + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da;
                DataSet ds;

                da = new SqlDataAdapter(sql, conn);
                ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false; ;
            }
        }
        /// <summary>
        /// ricerca la singola provenienza
        /// </summary
        /// <param name="provenienza"></param>
        /// <returns></returns>
        public Boolean getProvenienza(string provenienza)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM provenienza where provenienza = '" + provenienza.Replace("'", "''").Replace("*", "%") + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da;
                DataSet ds;

                da = new SqlDataAdapter(sql, conn);
                ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false; ;
            }
        }
        /// <summary>
        /// ricerca singolo tipo atto
        /// </summary>
        /// <param name="tipoatto"></param>
        /// <returns></returns>
        public Boolean getTipoAtto(string tipoatto)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM tipologia where tipo_nota = '" + tipoatto.Replace("'", "''") + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da;
                DataSet ds;

                da = new SqlDataAdapter(sql, conn);
                ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false; ;
            }
        }
        public Boolean getTipoScaturito(string scaturito)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM scaturito where scaturito = '" + scaturito.Replace("'", "''") + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da;
                DataSet ds;

                da = new SqlDataAdapter(sql, conn);
                ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false; ;
            }
        }
        /// <summary>
        /// ricerca lista per provenienza
        /// </summary>
        /// <param name="provenienza"></param>
        /// <returns></returns>
        public DataTable getListProvenienza(string provenienza, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola  where provenienza like '%" + provenienza.Replace("'", "''").Replace("*", "%") + "%'  order by dataarrivo desc";
            //            string sql = "SELECT * FROM Principale where provenienza like '%" + provenienza.Replace("'", "''").Replace("*", "%") + "%'  order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca per nominativo
        /// </summary>
        /// <param name="nominativo"></param>
        /// <returns></returns>
        public DataTable getListNominativo(string nominativo, out string msg)
        {
            DataTable tb = new DataTable();

            // string sql = "SELECT * FROM Principale where nominativo like '" + nominativo.Replace("'", "''").Replace("*", "%") + "%' order by dataarrivo desc";
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore " +
              "FROM Principale P " +
              "LEFT JOIN operatore S ON P.matricola = S.matricola " +
              "WHERE P.nominativo LIKE '" + nominativo.Replace("'", "''").Replace("*", "%") + "%' " +
              "ORDER BY P.dataarrivo DESC";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }

        /// <summary>
        /// ricerca per indirizzo  in tabella pricipale
        /// </summary>
        /// <param name="indirizzo"></param>
        /// <returns></returns>
        public DataTable getListIndirizzo(string indirizzo, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola where indirizzo like '%" + indirizzo.Replace("'", "''").Replace("*", "%") + "%' order by dataarrivo desc";
            //string sql = "SELECT * FROM Principale where indirizzo like '%" + indirizzo.Replace("'", "''").Replace("*", "%") + "%' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca per data inserimento
        /// </summary>
        /// <param name="dataarrivo"></param>
        /// <returns></returns>
        public DataTable getListDataArrivo(string dataArrivoDa, string dataArrivoA, out string msg)
        {
            DataTable tb = new DataTable();
            DateTime dtda = System.Convert.ToDateTime(dataArrivoDa);
            DateTime dta = System.Convert.ToDateTime(dataArrivoA);

            //string sql = "SELECT * FROM Principale where DataArrivo BETWEEN '" + dtda.ToShortDateString() + "' and '" + dta.ToShortDateString() + "'  order by dataarrivo desc";
            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola where DataArrivo BETWEEN '" + dtda.ToShortDateString() + "' and '" + dta.ToShortDateString() + "'  order by dataarrivo desc";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca per accertatori
        /// </summary>
        /// <param name="accertatori"></param>
        /// <returns></returns>
        public DataTable getListAccertatori(string accertatori, out string msg)
        {
            DataTable tb = new DataTable();
            string valoreCerca = accertatori.Replace("'", "''").Replace("*", "%");
            //string sql = "SELECT * FROM Principale WHERE (" +
            //            "Accertatori LIKE '" + valoreCerca + "%' OR " +
            //            "Accertatori2 LIKE '" + valoreCerca + "%' OR " +
            //            "Accertatori3 LIKE '" + valoreCerca + "%') " +
            //            "ORDER BY dataarrivo DESC";


            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola WHERE (" +
                        "Accertatori LIKE '" + valoreCerca + "%' OR " +
                        "Accertatori2 LIKE '" + valoreCerca + "%' OR " +
                        "Accertatori3 LIKE '" + valoreCerca + "%') " +
                        "ORDER BY dataarrivo DESC";



            // string sql = "SELECT * FROM Principale where accertatori like '" + accertatori.Replace("'", "''").Replace("*", "%") + "%'  order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }

        public DataTable getListInterrogatori(Interrogatorio interr)
        {
            DataTable tb = new DataTable();


            string sql = "SELECT * FROM interrogatori WHERE mese = '" + interr.Mese + "' and anno=" + interr.Anno + " ORDER BY DataInterrogatorio DESC";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListPratica(string nominativo, string indirizzo, string accertatori, out string msg)
        {
            DataTable tb = new DataTable();
            string sql = string.Empty;
            if (!String.IsNullOrEmpty(nominativo))
                sql = "SELECT * FROM Principale where nominativo like '%" + nominativo.Replace("'", "''") + "%'";
            if (!String.IsNullOrEmpty(accertatori))
                sql = "SELECT * FROM Principale where accertatori like '%" + accertatori.Replace("'", "''") + "%'";
            if (!String.IsNullOrEmpty(indirizzo))
                sql = "SELECT * FROM Principale where indirizzo like '%" + indirizzo.Replace("'", "''") + "%'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca per pratica
        /// </summary>
        /// <param name="pratica"></param>
        /// <returns></returns>
        public DataTable getListPratica(string pratica, out string msg)
        {
            DataTable tb = new DataTable();


            string sql = "SELECT P.*, S.Nominativo AS NomeOperatore FROM Principale P LEFT JOIN operatore S ON P.matricola = S.matricola  where nr_pratica = '" + pratica + "' order by dataarrivo desc";

            //string sql = "SELECT * FROM Principale where nr_pratica = '" + pratica + "' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable GetFileByOperatore(string matricola)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM File_Caricati where matricola = '" + matricola + "'";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        //public DataTable GetTurnoMensile(string mese, int anno)
        //{
        //    // Calcola il numero di giorni nel mese
        //    //   int mes = int.Parse(mese);

        //    int giorniNelMese = DateTime.DaysInMonth(anno, 12);
        //    // 1. Costruisci la lista dinamica delle colonne [1], [2], [3], ... [giorniNelMese]
        //    StringBuilder colonnePivot = new StringBuilder();
        //    for (int i = 1; i <= giorniNelMese; i++)
        //    {
        //        colonnePivot.Append($"[{i}],");
        //    }
        //    // Rimuovi l'ultima virgola
        //    string colonnePivotList = colonnePivot.ToString().TrimEnd(',');
        //    string queryPivot =
        //        $@"
        //WITH TurniGrezzi AS (
        //    SELECT 
        //        t.nominativo,
        //        T.codiceturno,
        //        CAST(T.giorno AS INT) AS GiornoDelMese 
        //    FROM TurniMensile T

        //    WHERE t.anno = {anno} AND t.mese = '{mese}'
        //)
        //SELECT 
        //    nominativo, 
        //    {colonnePivotList} 
        //FROM 
        //    TurniGrezzi
        //PIVOT (
        //    MAX(codiceturno) 
        //    FOR GiornoDelMese IN ({colonnePivotList})
        //) AS PivotTable
        //ORDER BY nominativo;";


        //    //  string  sql = "SELECT matricola,nominativo,giorno,codiceturno  FROM TurniMensile where mese ='" + mese + "' and anno= " + anno;


        //    String testoSql = String.Empty;
        //    DataTable tb = new DataTable();

        //    using (SqlConnection conn = new SqlConnection(ConnString))
        //    {
        //        // 3. Aggiungi i parametri


        //        using (SqlCommand cmd = new SqlCommand(queryPivot, conn))
        //        {
        //            //cmd.Parameters.AddWithValue("@Anno", anno);
        //            //cmd.Parameters.AddWithValue("@Mese", mese);
        //            conn.Open();
        //            DataTable dtTurni = new DataTable();
        //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //            {
        //                da.Fill(dtTurni);
        //                return dtTurni;
        //            }
        //        }

        //    }
        //}


        /// <summary>
        /// elenca i file cancellabili
        /// </summary>
        /// <returns>datatable</returns>
        public DataTable GetFileScaricati()
        {

            string sql = "select ISNULL(folder, '') +  ISNULL(nomefile, '') AS percorso  from File_Caricati where cancella = 'True'";


            String testoSql = String.Empty;
            DataTable tb = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }

        public Int32 GetCartellinaByQuartiere(string quartiere)
        {
            string sql = string.Empty;
            int progressivo = 0;
            DataTable tb = new DataTable();
            sql = "SELECT progressivo FROM ProgCartelline where quartiere like '%" + quartiere.Replace("'", "''") + "%'";

            using (SqlConnection conn = new SqlConnection(ConnStringTp))
            {

                tb = FillTable(sql, conn, out msg);
                if (tb.Rows.Count > 0)
                    progressivo = Convert.ToInt32(tb.Rows[0]["progressivo"].ToString());
                else
                    return 0;

                return progressivo + 1;
            }
        }

        public DataTable GetFileByFascicoloData(CaricaFile fl)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();
            if (!String.IsNullOrEmpty(fl.fascicolo) && !String.IsNullOrEmpty(fl.data))
                sql = "SELECT * FROM File_Caricati where fascicolo = " + @fl.fascicolo + " and Data = '" + fl.data + "'";
            else if (!String.IsNullOrEmpty(fl.fascicolo))
            {
                sql = "SELECT * FROM File_Caricati where fascicolo = '" + @fl.fascicolo + "'";
            }
            else
            {
                sql = "SELECT * FROM File_Caricati where Data = '" + fl.data + "'";

            }

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }


        public List<RegolaRSNL> getRsNlnlByAnnoMese(int anno, int mese)
        {
            string sql = string.Empty;
            List<RegolaRSNL> lista = new List<RegolaRSNL>();
            DataTable tb = new DataTable();
            sql = "SELECT gruppo, DataRS, DataNL, quartina,mese FROM rsnl " +
                             "WHERE (MONTH(DataRS) = @mese AND YEAR(DataRS) = @anno) " +
                             "OR (MONTH(DataNL) = @mese AND YEAR(DataNL) = @anno)";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@mese", mese);
                    cmd.Parameters.AddWithValue("@anno", anno);
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        lista.Add(new RegolaRSNL
                        {
                            Gruppo = rdr["gruppo"].ToString().Trim(),
                            Quartina = Convert.ToInt32(rdr["quartina"]),
                            DataRS = rdr["DataRS"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["DataRS"]) : null,
                            DataNL = rdr["DataNL"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["DataNL"]) : null,
                            Mese = rdr["mese"].ToString().Trim()
                        });
                    }
                }
            }

            return lista;

        }
        public DataTable getPraticaArchivioUOTPById(int id)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM ArchivioTp where id = '" + id + "'";


            using (SqlConnection conn = new SqlConnection(ConnStringTp))
            {
                return tb = FillTable(sql, conn, out msg);
            }


        }
        public DataTable getPraticaArchivioUotpById(int id)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM Archiviotp where id = '" + id + "'";


            using (SqlConnection conn = new SqlConnection(ConnStringTp))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getPraticaArchivioUoteById(int id)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM ArchivioUote where id_Archivio = '" + id + "'";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// estrazione parziale, esegue merge del datatable per ogni ck selezionato
        /// 
        /// </summary>
        /// <param name="ckevasa"></param>
        /// <param name="ck1089"></param>
        /// <param name="cksp"></param>
        /// <param name="ckvincoli"></param>
        /// <param name="ckdemolita"></param>
        /// <param name="ckpp"></param>
        /// <param name="ckpc"></param>
        /// <param name="ckpbc"></param>
        /// <param name="ckae"></param>
        /// <returns></returns>

        public DataTable getArchivioUoteParziale(String[] ar)
        {
            string sql = string.Empty; ;
            DataTable tb = new DataTable();
            //eseguo un merge al  dtatatable per ogni ck selezionato
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da;
                DataSet ds;
                if (!string.IsNullOrEmpty(ar[0]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_evasa = 'True'";

                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb = ds.Tables[0];
                }

                if (!string.IsNullOrEmpty(ar[1]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_1089 = 'True'";
                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb.Merge(ds.Tables[0]);
                }
                if (!string.IsNullOrEmpty(ar[2]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_suoloPub = 'True'";
                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb.Merge(ds.Tables[0]);
                }
                if (!string.IsNullOrEmpty(ar[3]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_vincoli = 'True'";
                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb.Merge(ds.Tables[0]);
                }
                if (!string.IsNullOrEmpty(ar[4]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_demolita = 'True'";
                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb.Merge(ds.Tables[0]);
                }
                if (!string.IsNullOrEmpty(ar[5]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_propPriv = 'True'";
                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb.Merge(ds.Tables[0]);
                }
                if (!string.IsNullOrEmpty(ar[6]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_propComune = 'True'";
                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb.Merge(ds.Tables[0]);
                }
                if (!string.IsNullOrEmpty(ar[7]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_propBeniCult ='True'";
                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb.Merge(ds.Tables[0]);
                }
                if (!string.IsNullOrEmpty(ar[8]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_propAltriEnti ='True'";
                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb.Merge(ds.Tables[0]);
                }
                if (!string.IsNullOrEmpty(ar[9]))
                {
                    sql = "SELECT * FROM ArchivioUote where arch_beniConfiscati ='True'";
                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    tb.Merge(ds.Tables[0]);
                }
            }

            return tb;
        }
        public DataTable getGestionePraticaById(string id_fascicolo)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM gestionePratiche where id_gestionePratica = '" + id_fascicolo + "'";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }

        public DataTable getGestionePraticaByFascicolo(string fascicolo)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM gestionePratiche where fascicolo = '" + fascicolo + "'";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getAttivitaAdmin(string area, Boolean val)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT a.Nr_Protocollo, a.quartiere, a.Macro_area, MIN(b.decr_decretato) as decr_decretato, b.decr_chiuso, a.id, decr_dataChiusura FROM principale AS a INNER JOIN Decretazione AS b ON a.Nr_Protocollo = b.decr_pratica WHERE a.Macro_area = '" + area + "' AND b.decr_chiuso ='" + val + "' GROUP BY a.Nr_Protocollo, a.Macro_area, b.decr_chiuso,a.id,decr_dataChiusura, a.quartiere order by a.nr_protocollo";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// get auto sigla
        /// </summary>
        /// <returns></returns>
        public DataTable getSiglaAuto()
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM parcoauto order by sigla";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable GetScadenziarioById(int id)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM ScadenziarioUrp where id_scadenziario = " + id;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable GetListRegistroUrp()
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM RegistroUrp order by dataConclProcedimento";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable GetRegistroById(int id)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM RegistroUrp where id_registro = " + id;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable GetListScadenziarioUrpEsitoVuoto(string filterValue)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM ScadenziarioUrp where esito='" + filterValue + "'";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable GetListScadenziarioUrp()
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM ScadenziarioUrp order by nr_carico, dataArrivo";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getListAuto(string mese, int anno)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM GestioneAuto where mese ='" + mese + "' and anno = " + anno + " order by sigla, data";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getAutoBySigla(string sigla)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT * FROM parcoauto where sigla ='" + sigla + "'";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getAttivita(string area, Boolean val)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            sql = "SELECT a.Nr_Protocollo, a.quartiere, a.Macro_area, MIN(b.decr_decretato) as decr_decretato, b.decr_chiuso, a.id, decr_dataChiusura FROM principale AS a INNER JOIN Decretazione AS b ON a.Nr_Protocollo = b.decr_pratica WHERE a.Macro_area = '" + area + "' AND b.decr_chiuso ='" + val + "' GROUP BY a.Nr_Protocollo, a.Macro_area, b.decr_chiuso,a.id,decr_dataChiusura, a.quartiere order by a.nr_protocollo";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// ricerca la sche dipendente
        /// </summary>
        /// <param name="matricola"></param>
        /// <param name="nominativo"></param>
        /// <returns></returns>
        public DataTable getSchedaDip(string matricola, string nominativo)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();


            if (!String.IsNullOrEmpty(nominativo))
            {
                sql = "SELECT * FROM SchedaDipendente where nominativo like '" + nominativo + "%'";
            }
            else if (!String.IsNullOrEmpty(matricola))

                sql = "SELECT * FROM SchedaDipendente where matricola_ced = '" + matricola + "'";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }

        /// <summary>
        /// estrazione totale del DB
        /// </summary>
        /// <returns></returns>
        public DataTable getArchivioUoteTotale()
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM ArchivioUote";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// Estrae la teballa
        /// </summary>
        /// <returns></returns>
        public DataTable getGestionePraticaTotale()
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM GestionePratiche";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getPraticaArchivioUotp(string[] pratica, string oggetto, string bu, string nota, string destinatario, string indirizzo, string intestatario, string edificio)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();
            if (pratica != null)
            {
                //  if (!String.IsNullOrEmpty(pratica[2].ToString()))
                if (!String.IsNullOrEmpty(pratica[1].ToString()) && !String.IsNullOrEmpty(pratica[2].ToString())) //+ + 

                    sql = "SELECT  * FROM Archiviotp where quartiere = '" + pratica[1] + "' and cartellina = '" + pratica[2] + "' ORDER BY cartellina asc ";
                else if (!String.IsNullOrEmpty(pratica[1].ToString()) && String.IsNullOrEmpty(pratica[2].ToString())) //  + -
                    sql = "SELECT  * FROM Archiviotp where quartiere = '" + pratica[1] + "' ORDER BY cartellina asc ";
                else if (String.IsNullOrEmpty(pratica[1].ToString()) && !String.IsNullOrEmpty(pratica[2].ToString())) // - +
                    sql = "SELECT  * FROM Archiviotp where cartellina = '" + pratica[2] + "' ORDER BY quartiere asc ";

            }
            if (!String.IsNullOrEmpty(oggetto))
                sql = "SELECT * FROM Archiviotp where oggetto1 like '%" + oggetto.Replace("'", "''").Replace("*", "%") + "%'";
            if (!String.IsNullOrEmpty(bu))
                sql = "SELECT * FROM Archiviotp where codice like '%" + bu.Replace("'", "''") + "%'";
            if (!String.IsNullOrEmpty(edificio))
                sql = "SELECT * FROM Archiviotp where codice_edificio like '%" + edificio.Replace("'", "''") + "%'";

            if (!String.IsNullOrEmpty(nota))
                sql = "SELECT * FROM Archiviotp where note like '%" + nota.Replace("'", "''").Replace("*", "%") + "%'";
            if (!String.IsNullOrEmpty(destinatario))
                sql = "SELECT * FROM Archiviotp  WHERE destinatario like '%" + destinatario.Replace("'", "''").Replace("*", "%") + "%'";
            if (!String.IsNullOrEmpty(indirizzo))
                sql = "SELECT * FROM Archiviotp  WHERE via like '%" + indirizzo.Replace("'", "''").Replace("*", "%") + "%'";
            if (!String.IsNullOrEmpty(intestatario))
                sql = "SELECT * FROM Archiviotp  WHERE cognome like '%" + intestatario.Replace("'", "''").Replace("*", "%") + "%'";
            using (SqlConnection conn = new SqlConnection(ConnStringTp))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getPraticaArchivioUote(string[] pratica, string nominativo, string indirizzo, string[] catasto, string nota, string[] annomese)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();
            if (pratica != null)
            {
                switch (pratica[0])
                {
                    case "Pratica":
                        if (pratica[1] == "Doppione")
                            sql = "SELECT  * FROM ArchivioUote where arch_numPratica = '" + pratica[2].Replace("'", "''") + "' ORDER BY arch_datault_intervento desc ";
                        else
                        {
                            if (!String.IsNullOrEmpty(pratica[1]))
                                sql = "SELECT top 1 * FROM ArchivioUote where arch_numPratica = '" + pratica[1].Replace("'", "''") + "' order by id_Archivio desc";

                        }

                        break;
                    case "StoricoPratica":
                        if (!String.IsNullOrEmpty(pratica[1]))
                            sql = "SELECT * FROM ArchivioUote where arch_numPratica = '" + pratica[1].Replace("'", "''") + "'";
                        break;
                        //case "PraticaVal":
                        //    if (!String.IsNullOrEmpty(pratica[1]))
                        //        // sql = "SELECT * FROM principale where nr_Pratica = '" + pratica[1].Replace("'", "''") + "' and anno= '" + pratica[2] + "'";
                        //        sql = "SELECT id,nr_Pratica ,nr_protocollo, evasa,anno FROM [DB_ArchivioPratiche].[dbo].[principale] WHERE Anno ='" + pratica[2] + "' and nr_Pratica = '" + pratica[1] +
                        //                 "' GROUP BY nr_Pratica ,nr_protocollo,evasa,anno,id HAVING COUNT(Nr_Protocollo) >= 1 ORDER BY Nr_Protocollo DESC";
                        //    break;
                }
            }


            if (!String.IsNullOrEmpty(nominativo))
                sql = "SELECT * FROM ArchivioUote where arch_responsabile like '%" + nominativo.Replace("'", "''").Replace("*", "%") + "%'";
            if (!String.IsNullOrEmpty(indirizzo))
                sql = "SELECT * FROM ArchivioUote where arch_indirizzo like '%" + indirizzo.Replace("'", "''").Replace("*", "%") + "%'";

            if (catasto != null)
                sql = "SELECT * FROM ArchivioUote where arch_sezione = '" + catasto[1] + "' and arch_foglio = '" + catasto[2] + "' and arch_particella = '" + catasto[3] +
                   "' and arch_sub= '" + catasto[4] + "'";
            if (!String.IsNullOrEmpty(nota))
                sql = "SELECT * FROM ArchivioUote where arch_note like '%" + nota.Replace("'", "''").Replace("*", "%") + "%'";
            if (annomese != null)
                if (!String.IsNullOrEmpty(annomese[2]))
                    sql = "SELECT * FROM ArchivioUote  WHERE YEAR(arch_dataIns) =" + annomese[1] + " and MONTH(arch_dataIns) =" + annomese[2];
                else
                    sql = "SELECT * FROM ArchivioUote  WHERE YEAR(arch_dataIns) =" + annomese[1];


            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getPraticaId(Int32 protocollo, DateTime data, string sigla, Int32 id)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Principale where Nr_Protocollo = " + protocollo + " and DataInserimento = '" + data + "' and sigla = '" + sigla + "'" + " and id = " + id;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getPratica(Int32 protocollo, DateTime data, string sigla)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Principale where Nr_Protocollo = " + protocollo + " and DataInserimento = '" + data + "' and sigla = '" + sigla + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }
        public DataTable getPraticaModificaRiservata(string protocollo, string anno)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Principale where Nr_Protocollo = '" + protocollo + "' and anno = '" + anno + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }
        }


        private DataTable FillTable(String sql, SqlConnection conn, out string msg)
        {
            DataTable table = new DataTable();
            msg = string.Empty;
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 240;
                    SqlDataAdapter da;
                    DataSet ds;

                    da = new SqlDataAdapter(sql, conn);
                    ds = new DataSet();
                    da.Fill(ds);

                    table = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                conn.Close();
                conn.Dispose();
                return table;
            }
            conn.Close();
            conn.Dispose();
            return table;

        }
        //INSERIMENTI
        /// <summary>
        /// inserimento tabella Tipologia Nota Ag
        /// </summary>
        /// <param name="TipologiaNotaAg"></param>
        /// <returns></returns>
        public Boolean InserisciTipologiaNotaAg(string TipologiaNotaAg)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "insert into TipoNotaAG (Tipologia)" +
                   " Values('" + TipologiaNotaAg.Replace("'", "''") + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "TipologiaNotaAg";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Tipologia Nota Ag:" + TipologiaNotaAg + ", " + ex.Message + @" - Errore in inserimento tabella Tipologia Nota Ag ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        /// <summary>
        /// inseriemento tabella Tipologia
        /// </summary>
        /// <param name="Tipologia"></param>
        /// <returns></returns>
        public Boolean InserisciTipologia(string Tipologia)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "insert into Tipologia (tipo_nota)" +
                   " Values('" + Tipologia.Replace("'", "''") + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "Tipologia";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Tipologia:" + Tipologia + ", " + ex.Message + @" - Errore in inserimento tabella Tipologia ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }

        /// <summary>
        /// inserimento in tabella tipologia abuso
        /// </summary>
        /// <param name="Tipologia"></param>
        /// <returns></returns>
        public Boolean InserisciTipologiaAbuso(string Tipologia)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "insert into TipologiaAbuso (tipologia)" +
                   " Values('" + Tipologia.Replace("'", "''") + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "Tipologia abuso";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Tipologia Abuso:" + Tipologia + ", " + ex.Message + @" - Errore in inserimento tabella Tipologia Abuso ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;


        }
        /// <summary>
        /// inserimento tabella provenienza
        /// </summary>
        /// <param name="Provenienza"></param>
        /// <returns></returns>
        public Boolean InserisciProvenienza(string Provenienza)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "insert into Provenienza (Provenienza)" +
                   " Values('" + Provenienza.Replace("'", "''") + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "Provenienza";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Provenienza:" + Provenienza + ", " + ex.Message + @" - Errore in inserimento tabella Provenienza ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        /// <summary>
        /// inserimento tabella scaturito
        /// </summary>
        /// <param name="Scaturito"></param>
        /// <returns></returns>
        public Boolean InserisciScaturito(string Scaturito)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "insert into Scaturito (Scaturito)" +
                   " Values('" + Scaturito.Replace("'", "''") + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "Scaturito";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Scaturito:" + Scaturito + ", " + ex.Message + @" - Errore in inserimento tabella Scaturito ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        /// <summary>
        /// isnserimento tabella incviata
        /// </summary>
        /// <param name="inviata"></param>
        /// <returns></returns>
        public Boolean InserisciInviata(string inviata)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "insert into inviati (inviata)" +
                   " Values('" + inviata.Replace("'", "''") + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "inviata";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Inviata:" + inviata + ", " + ex.Message + @" - Errore in inserimento tabella inviata ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        /// <summary>
        /// isnserimento tabella giudice
        /// </summary>
        /// <param name="giudice"></param>
        /// <returns></returns>
        /// 
        public Boolean InserisciGiudice(string giudice)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "insert into giudice (giudice)" +
                   " Values('" + @giudice.Replace("'", "''") + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "giudice";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Giudice:" + giudice + ", " + ex.Message + @" - Errore in inserimento tabella giudice ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }

        /// <summary>
        /// carica file in tabella 
        /// </summary>
        /// <param name="fl"></param>
        /// <returns></returns>

        public Boolean InsFile(CaricaFile fl)
        {
            bool resp = true;
            string sql_file = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_file = "insert into file_caricati (fascicolo, data,matricola, folder,nomefile,cancella)" +
                   " Values('" + @fl.fascicolo.Replace("'", "''") + "','" + @fl.data + "','" + fl.matricola.Replace("'", "''") + "','" + fl.folder.Replace("'", "''") +
                   "','" + fl.nomefile.Replace("'", "''") + "','" + fl.cancella + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_file;
                        testoSql = "carica file";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("matricola:" + fl.matricola + ", data: " + fl.data + "nomefile: " + fl.nomefile + " - " + ex.Message + @" - Errore in inserimento dati in tabella carica file");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        /// <summary>
        /// inserisce una gestione auto e converte string in float
        /// </summary>
        /// <param name="auto"></param>
        /// <returns></returns>
        public Boolean InsGestioneAuto(GestAuto auto)
        {
            bool resp = true;
            string sql = String.Empty;
            string testoSql = string.Empty;

            try
            {
                string inputlitri = auto.litri.ToString();
                string inputEuro = auto.euro.ToString();
                decimal importoFloat;
                decimal litriFloat;
                // 1. Definisci la cultura di parsing (italiana: virgola come decimale)
                CultureInfo culturaItaliana = new CultureInfo("it-IT");

                // 2. Pulizia: Rimuovi il simbolo € e spazi, poi tenta la conversione C#
                if (decimal.TryParse(
                        inputEuro.Replace("€", "").Trim(),
                        NumberStyles.Any, // Accetta formati diversi (separatori di migliaia, ecc.)
                        culturaItaliana,
                        out importoFloat) &&


                        decimal.TryParse(
                        inputlitri.Replace("€", "").Trim(),
                        NumberStyles.Any, // Accetta formati diversi (separatori di migliaia, ecc.)
                        culturaItaliana,
                        out litriFloat))

                {
                    decimal valoreArrotondatoE = Math.Round(importoFloat, 2);
                    decimal valoreArrotondatoL = Math.Round(litriFloat, 2);

                    sql = "insert into gestioneauto (sigla, targa,stan,data,ora,litri,tipoCarburante,euro,indirizzo,autista,mese,anno,nota)" +
                          " Values('" + auto.sigla + "','" + auto.targa + "','" + auto.stan + "','" + auto.data + "','" + auto.ora + "', @valore ,'" + auto.tipoCarburante + "', @valore1 ,'" + auto.indirizzo.Replace("'", "''") + "','" + auto.autista.Replace("'", "''") + "','" + auto.mese + "'," + auto.anno + "," + "'" + auto.nota.Replace("'", "''") + "')";


                    using (SqlConnection conn = new SqlConnection(ConnString))
                    {
                        conn.Open();

                        SqlCommand command = conn.CreateCommand();
                        command.Parameters.Add("@valore1", SqlDbType.Float).Value = (double)importoFloat;
                        command.Parameters.Add("@valore", SqlDbType.Float).Value = (double)litriFloat;

                        try
                        {
                            command.CommandText = sql;
                            testoSql = "gestioneauto";
                            int res = command.ExecuteNonQuery();
                        }

                        catch (Exception ex)
                        {
                            if (!File.Exists(LogFile))
                            {
                                using (StreamWriter sw = File.CreateText(LogFile)) { }
                            }

                            using (StreamWriter sw = File.AppendText(LogFile))
                            {
                                sw.WriteLine("Sigla:" + auto.sigla + ", " + ex.Message + @" - Errore in inserimento tabella gestione auto ");
                                sw.Close();
                            }

                            resp = false;


                        }
                        conn.Close();
                        conn.Dispose();
                        return resp;

                    }

                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        public Boolean InsSchedaDipendente(SchedaDipendenteClass scheda)
        {
            Boolean resp = true;

            string sql = String.Empty;
            string testoSql = string.Empty;
            //sql = "IF NOT EXISTS (SELECT 1 FROM SchedaDipendente WHERE matricola_ced = '" + scheda.Matricola + "') BEGIN INSERT INTO SchedaDipendente (matricola_ced, nominativo,grado," +
            //     "data_assunzione,categ_economica,autista, armato, quartina,gruppo_quartina,gruppo_reperibilita,perm_53,perm_104,limitazioni,turni_pref,Macro_area,area,data_sorv_sanitaria) " +
            //     " VALUES ('" + scheda.Matricola + "','" + scheda.Nominativo + "','" + scheda.Grado + "','" + scheda.dataAssunzione.ToString("yyyy-MM-dd") + "','" + scheda.CategoriaEconomica + "','" + scheda.IsAutista + "','" + scheda.Armato + "','" + scheda.Quartina + "','" +
            //     scheda.GruppoQuartina + "','" + scheda.GruppoReperibilita + "','" + scheda.l53 + "','" + scheda.l104 + "','" + scheda.limitazione + "','" + scheda.TurnoPref + "','" + scheda.MacroArea + "','" + scheda.Area + "','" + scheda.dataSorveglianza.ToString("yyyy-MM-dd") + "')  END";
            sql =
    "IF EXISTS (SELECT 1 FROM SchedaDipendente WHERE matricola_ced = '" + scheda.Matricola + "') " +
    "BEGIN " +
        "UPDATE SchedaDipendente SET " +
            "nominativo = '" + scheda.Nominativo + "', " +
            "grado = '" + scheda.Grado + "', " +
            "data_assunzione = '" + scheda.dataAssunzione.ToString("yyyy-MM-dd") + "', " +
            "categ_economica = '" + scheda.CategoriaEconomica + "', " +
            "autista = '" + scheda.IsAutista + "', " +
            "armato = '" + scheda.Armato + "', " +
            "quartina = '" + scheda.Quartina + "', " +
            "gruppo_quartina = '" + scheda.GruppoQuartina + "', " +
            "gruppo_reperibilita = '" + scheda.GruppoReperibilita + "', " +
            "perm_53 = '" + scheda.l53 + "', " +
            "perm_104 = '" + scheda.l104 + "', " +
            "limitazioni = '" + scheda.limitazione + "', " +
            "turni_pref = '" + scheda.TurnoPref + "', " +
            "Macro_area = '" + scheda.MacroArea + "', " +
            "area = '" + scheda.Area + "', " +
            "data_sorv_sanitaria = '" + scheda.dataSorveglianza.ToString("yyyy-MM-dd") + "' " +
        "WHERE matricola_ced = '" + scheda.Matricola + "' " +
    "END " +
    "ELSE " +
    "BEGIN " +
        "INSERT INTO SchedaDipendente (matricola_ced, nominativo, grado, data_assunzione, categ_economica, autista, armato, quartina, gruppo_quartina, gruppo_reperibilita, perm_53, perm_104, limitazioni, turni_pref, Macro_area, area, data_sorv_sanitaria) " +
        "VALUES ('" + scheda.Matricola + "','" + scheda.Nominativo + "','" + scheda.Grado + "','" + scheda.dataAssunzione.ToString("yyyy-MM-dd") + "','" + scheda.CategoriaEconomica + "','" + scheda.IsAutista + "','" + scheda.Armato + "','" + scheda.Quartina + "','" +
        scheda.GruppoQuartina + "','" + scheda.GruppoReperibilita + "','" + scheda.l53 + "','" + scheda.l104 + "','" + scheda.limitazione + "','" + scheda.TurnoPref + "','" + scheda.MacroArea + "','" + scheda.Area + "','" + scheda.dataSorveglianza.ToString("yyyy-MM-dd") + "') " +
    "END";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();


                    try
                    {
                        // conn.Open();

                        // Esegue il comando
                        int righeCoinvolte = cmd.ExecuteNonQuery();

                        if (righeCoinvolte > 0)
                        {
                            // Inserimento riuscito
                            resp = true;
                        }
                        else
                        {
                            // La matricola esisteva già, l'insert non è scattato
                            resp = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Gestione errori
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("InsSchedaDipendente:" + ex.Message + @" - Errore in inserimento tabella scheda dipendente ");
                            sw.Close();
                        }
                    }

                }


                conn.Close();
                conn.Dispose();
                return resp;

            }
        }
        /// <summary>
        /// Inserisce una decretazione e aggiorna la tabella principale con il nuovo accertatore
        /// </summary>
        /// <param name="decr"></param>
        /// <returns></returns>
        public Boolean InsDecretazione(Decretazione decr)
        {
            bool resp = true;
            string sql_decretazione = String.Empty;
            string sql_update = String.Empty;
            // int res1 = 0;
            string testoSql = string.Empty;

            try
            {
                sql_decretazione = "insert into decretazione (decr_idPratica, decr_pratica,decr_decretante, decr_decretato,decr_data,decr_nota," +
                    "decr_dataChiusura, decr_chiuso)" +
                   " Values('" + decr.idPratica + "','" + decr.Npratica + "','" + decr.decretante.Replace("'", "''") + "','" + decr.decretato.Replace("'", "''") +
                   "','" + decr.data + "','" + decr.nota.Replace("'", "''") + "','" + null + "','" + decr.chiuso + "')";

                //sql_update = "update principale set accertatori= accertatori '" + decr.decretato.Replace("'", "''") + "'";
                //+ "where  and  CHARINDEX('" + @p.accertatori.Replace("'", "''") + "', accertatori) = 0";
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();

                    SqlCommand command = conn.CreateCommand();



                    try
                    {
                        command.CommandText = sql_decretazione;
                        testoSql = "DECTRETAZIONE";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Decretazione:" + decr.Npratica + ", " + ex.Message + @" - Errore in inserimento tabella decretazione ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;



                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        /// <summary>
        /// inserisce un interrogatorio 
        /// </summary>
        /// <param name="interr"></param>
        /// <returns></returns>
        public Boolean InsInterrogatorio(Interrogatorio interr)
        {
            bool resp = true;
            string sql_interrogatorio = String.Empty;
            string sql_update = String.Empty;
            // int res1 = 0;
            string testoSql = string.Empty;

            try
            {
                sql_interrogatorio = "insert into interrogatori (ProcPenale, DataInterrogatorio,Npratica, Nominativo1,Nominativo2,Nominativo3,Nominativo4,DataInserimento,Matricola,mese,anno)" +
                   " Values('" + interr.ProcPenale.Replace("'", "") + "','" + interr.DataInterrogatorio + "','" + interr.Npratica + "','" + interr.Nominativo1.Replace("'", "''") +
                   "','" + interr.Nominativo2.Replace("'", "''") + "','" + interr.Nominativo3.Replace("'", "''") + "','" + interr.Nominativo4.Replace("'", "''") +
                   "','" + interr.DataInserimento + "','" + interr.Matricola + "','" + interr.Mese + "'," + interr.Anno + ")";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();

                    SqlCommand command = conn.CreateCommand();



                    try
                    {
                        command.CommandText = sql_interrogatorio;
                        testoSql = "Interrogatorio";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Interrogatorio:" + interr.ProcPenale.Trim() + ", " + ex.Message + @" - Errore in inserimento tabella Interrogatorio ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;



                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        public Boolean InsScadenziarioRegistro(UrpScadenziario scadenziario, int id, String HfRegistro, UrpRegistro registro, String newScadenza)
        {
            bool resp = true;
            string sql_scadenziario = String.Empty;
            string sql_registro = String.Empty;
            DateTime dataScadenzaNew = new DateTime();
            string testoSql = string.Empty;
            if (!string.IsNullOrWhiteSpace(newScadenza))
                dataScadenzaNew = Convert.ToDateTime(newScadenza);
            try
            {

                sql_scadenziario = "UPDATE ScadenziarioUrp SET " +
        "nr_carico = '" + scadenziario.nr_carico.Trim() + "', " +
        "anno = " + scadenziario.anno + ", " +
        "nr_pratica = '" + scadenziario.nr_pratica.Trim() + "', " +
        "richiedente = '" + scadenziario.richiedente.Replace("'", "''").Trim() + "', " +
        "protGen = '" + scadenziario.protGen.Trim() + "', " +
        "dataArrivo = '" + scadenziario.dataArrivo.ToString("yyyy-MM-dd") + "', " +
        "dataScadenza = '" + scadenziario.dataScadenza.ToString("yyyy-MM-dd") + "', " +
        "controInteressati = '" + scadenziario.controInteressati + "', " +
        "esito = '" + scadenziario.esito.Replace("'", "''").Trim() + "', " +
        "motivazione = '" + scadenziario.motivazione.Replace("'", "''").Trim() + "', " +
        "protUscita = '" + scadenziario.protUscita.Trim() + "', " +
        "dataUscita = '" + scadenziario.dataUscita.ToString("yyyy-MM-dd") + "', " +
        "ric24190 = '" + scadenziario.ric24190 + "', " +
        "ric3313 = '" + scadenziario.ric3313 + "' " + // TOLTA LA VIRGOLA QUI
    "WHERE id_scadenziario = " + id + " ";
                // scadenziario.dataScadenza = dataScadenzaNew;
                string sql_ins = "INSERT INTO ScadenziarioUrp (nr_carico, anno, nr_pratica, richiedente, protGen, dataArrivo, dataScadenza, " +
                "controInteressati, esito, motivazione, protUscita, dataUscita, ric24190, ric3313) " +
    "VALUES ('" + scadenziario.nr_carico.Trim() + "'," +
                  scadenziario.anno + ",'" +
                  scadenziario.nr_pratica.Trim() + "','" +
                  scadenziario.richiedente.Replace("'", "''").Trim() + "','" +
                  scadenziario.protGen.Trim() + "','" +
                  scadenziario.dataArrivo.ToString("yyyy-MM-dd") + "','" +  // AGGIUNTA FORMATTAZIONE
                  dataScadenzaNew.ToString("yyyy-MM-dd") + "','" +  // AGGIUNTA FORMATTAZIONE
                  false + "','" + //controinteressati
                  null + "','" + // esito vuoto
                  null + "','" + // motivazione vuota
                  null + "','" + // prot uscita vuoto
                  null + "','" + //  data uscita vuota
                  scadenziario.ric24190 + "','" +
                  scadenziario.ric3313 + "') ";


                sql_registro = "INSERT INTO RegistroUrp (oggetto, dataPresentRichiesta, nrPgTrasmissioneRichiesto, uffDetentore, controInteressati, esito, motivazione, nrPgTrasmissioneRiscontro, dataConclProcedimento) " +
                "VALUES ('" + registro.oggetto.Trim() + "','" +
                     registro.dataPresentRichiesta.ToString("yyyy-MM-dd") + "','" +  // AGGIUNTA FORMATTAZIONE
                     registro.nrPgTrasmissioneRichiesto + "','" +
                     registro.uffDetentore.Replace("'", "''").Trim() + "','" +
                     registro.controInteressati + "','" +
                     registro.esito.Replace("'", "''").Trim() + "','" +
                     registro.motivazione.Replace("'", "''").Trim() + "','" +
                     registro.nrPgTrasmissioneRiscontro + "','" +
                     registro.dataConclProcedimento.ToString("yyyy-MM-dd") + "') ";

                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction("trans"))
                    {

                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.Transaction = tran;

                            try
                            {
                                command.CommandText = sql_scadenziario;
                                testoSql = "UrpScadenziario";
                                int res = command.ExecuteNonQuery();

                                if (res > 0)
                                {
                                    if (!String.IsNullOrWhiteSpace(HfRegistro))
                                    {
                                        switch (HfRegistro)
                                        {
                                            case "duplica":
                                                // SECONDA OPERAZIONE: solo se duplico
                                                command.CommandText = sql_ins;

                                                command.ExecuteNonQuery();
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    // terza operazione
                                    command.CommandText = sql_registro;

                                    command.ExecuteNonQuery();

                                    tran.Commit();
                                    resp = true;
                                }
                                else
                                {
                                    // Se il primo inserimento non ha modificato righe, annulla tutto.
                                    tran.Rollback();
                                    resp = false;
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
                                    sw.WriteLine("carico:" + scadenziario.nr_carico.Trim() + ", " + scadenziario.anno + " - " + ex.Message + @" - Errore in inserimento tabella UrpScadenziario ");
                                    sw.Close();
                                }
                                tran.Rollback();
                                resp = false;


                            }
                        }
                        conn.Close();
                        conn.Dispose();
                        return resp;
                    }
                }

            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }

        public Boolean InsScadenziario(UrpScadenziario scadenziario, int id)
        {
            bool resp = true;
            string sql_scadenziario = String.Empty;
            string sql_registro = String.Empty;
            string testoSql = string.Empty;

            try
            {

                sql_scadenziario =
    "IF EXISTS (SELECT 1 FROM ScadenziarioUrp WHERE id_scadenziario = " + id + ") " +
"BEGIN " +
    "UPDATE ScadenziarioUrp SET " +
        "nr_carico = '" + scadenziario.nr_carico.Trim() + "', " +
        "anno = " + scadenziario.anno + ", " +
        "nr_pratica = '" + scadenziario.nr_pratica.Trim() + "', " +
        "richiedente = '" + scadenziario.richiedente.Replace("'", "''").Trim() + "', " +
        "protGen = '" + scadenziario.protGen.Trim() + "', " +
        "dataArrivo = '" + scadenziario.dataArrivo.ToString("yyyy-MM-dd") + "', " +
        "dataScadenza = '" + scadenziario.dataScadenza.ToString("yyyy-MM-dd") + "', " +
        "controInteressati = '" + scadenziario.controInteressati + "', " +
        "esito = '" + scadenziario.esito.Replace("'", "''").Trim() + "', " +
        "motivazione = '" + scadenziario.motivazione.Replace("'", "''").Trim() + "', " +
        "protUscita = '" + scadenziario.protUscita.Trim() + "', " +
        "dataUscita = '" + scadenziario.dataUscita.ToString("yyyy-MM-dd") + "', " +
        "ric24190 = '" + scadenziario.ric24190 + "', " +
        "ric3313 = '" + scadenziario.ric3313 + "' " + // TOLTA LA VIRGOLA QUI
    "WHERE id_scadenziario = " + id + " " +
"END " +
"ELSE " +
"BEGIN " +
    "INSERT INTO ScadenziarioUrp (nr_carico, anno, nr_pratica, richiedente, protGen, dataArrivo, dataScadenza, " +
                "controInteressati, esito, motivazione, protUscita, dataUscita, ric24190, ric3313) " +
    "VALUES ('" + scadenziario.nr_carico.Trim() + "'," +
                  scadenziario.anno + ",'" +
                  scadenziario.nr_pratica.Trim() + "','" +
                  scadenziario.richiedente.Replace("'", "''").Trim() + "','" +
                  scadenziario.protGen.Trim() + "','" +
                  scadenziario.dataArrivo.ToString("yyyy-MM-dd") + "','" +  // AGGIUNTA FORMATTAZIONE
                  scadenziario.dataScadenza.ToString("yyyy-MM-dd") + "','" + // AGGIUNTA FORMATTAZIONE
                  scadenziario.controInteressati + "','" +
                  scadenziario.esito.Replace("'", "''").Trim() + "','" +
                  scadenziario.motivazione.Replace("'", "''").Trim() + "','" +
                  scadenziario.protUscita.Trim() + "','" +
                  scadenziario.dataUscita.ToString("yyyy-MM-dd") + "','" + // AGGIUNTA FORMATTAZIONE
                  scadenziario.ric24190 + "','" +
                  scadenziario.ric3313 + "') " +
"END";



                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction("trans"))
                    {

                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.Transaction = tran;

                            try
                            {
                                command.CommandText = sql_scadenziario;
                                testoSql = "UrpScadenziario";
                                int res = command.ExecuteNonQuery();

                                if (res > 0)
                                {

                                    tran.Commit();
                                    resp = true;
                                }
                                else
                                {
                                    // Se il primo inserimento non ha modificato righe, annulla tutto.
                                    tran.Rollback();
                                    resp = false;
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
                                    sw.WriteLine("carico:" + scadenziario.nr_carico.Trim() + ", " + scadenziario.anno + " - " + ex.Message + @" - Errore in inserimento tabella UrpScadenziario ");
                                    sw.Close();
                                }
                                tran.Rollback();
                                resp = false;


                            }
                        }
                        conn.Close();
                        conn.Dispose();
                        return resp;
                    }
                }

            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }

        /// <summary>
        /// inserimento tabella RSNL annuale
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="giorno"></param>
        /// <param name="gruppo"></param>
        /// <param name="codice"></param>
        /// <returns></returns>
        public string InsRSNL(List<RecordRsnl> records)
        {
            string resp = string.Empty;

            string sql = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql = @"INSERT INTO RSNL (Gruppo, DataRS, DataNL, Mese, Quartina) 
                                       VALUES (@grp, @drs, @dnl, @mese, @qrt)";

                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();

                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // (Opzionale) Pulisci tabella
                            new SqlCommand("TRUNCATE TABLE RSNL", conn, trans).ExecuteNonQuery();



                            foreach (var item in records)
                            {
                                using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@grp", item.Gruppo);
                                    // Passiamo DateTime oggetti -> SQL Server gestirà il formato 'YYYY-MM-DD'
                                    cmd.Parameters.AddWithValue("@drs", (object)item.DataRS ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@dnl", (object)item.DataNL ?? DBNull.Value);
                                    // Passiamo anche la stringa originale
                                    cmd.Parameters.AddWithValue("@mese", item.MeseStringa);
                                    cmd.Parameters.AddWithValue("@qrt", item.Quartina);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            trans.Commit();
                        }
                        catch (Exception ex1)
                        {
                            trans.Rollback();
                            if (!File.Exists(LogFile))
                            {
                                using (StreamWriter sw = File.CreateText(LogFile)) { }
                            }

                            using (StreamWriter sw = File.AppendText(LogFile))
                            {
                                sw.WriteLine("RSNL:" + ex1.Message + @" - Errore in inserimento tabella RSNL ");
                                sw.Close();
                            }
                            resp = ex1.Message + "--" + ConnString;
                            //throw; // Rilancia errore
                        }
                    }

                    conn.Close();
                    conn.Dispose();
                    return resp;

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
                    sw.WriteLine("RSNL:" + ex.Message + @" - Errore in inserimento tabella RSNL ");
                    sw.Close();
                }

                resp = ex.Message + " 2";


            }
            return resp;

        }
        public Boolean InsOperatore(Operatore op)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "insert into operatore (matricola, passw, profilo,nota, area, macroarea,ruolo, reset, pwstandard,nominativo)" +
                   " Values('" + @op.matricola + "','" + @op.passw.Replace("'", "''") + "','" + @op.profilo + "','" + @op.nota.Replace("'", "''") + "','" + @op.area.Replace("'", "''") +
                   "','" + @op.macroarea.Replace("'", "''") + "','" + @op.ruolo.Replace("'", "''") + "','" + @op.reset + "','" + @op.pwstandard + "','" + @op.nominativo.Replace("'", "''") + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "operatore";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("matricola:" + op.matricola + ", " + ex.Message + @" - Errore in inserimento dati ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        public Boolean InsStatatti(Boolean exist, Statistiche stat)
        {
            bool resp = true;

            string sql_Statistiche = String.Empty;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();


                SqlCommand command = conn.CreateCommand();


                try
                {
                    if (!exist)

                        sql_Statistiche = "insert into statistiche (mese,anno,relazioni,ponteggi,dpi,esposti_ricevuti,esposti_evasi,ripristino_tot_par,controlli_scia,contr_cant_daily,cnr,annotazioni,notifiche" +
                            ",sequestri,riapp_sigilli,deleghe_ricevute,deleghe_esitate,cnr_annotazioni,interrogazioni,denunce_uff,convalide,demolizioni" +
                            ",violazione_sigilli,dissequestri,dissequestri_temp,rimozione_sigilli,controlli_42_04,contr_cant_suolo_pubb,contr_lavori_edili,contr_cant,contr_nato_da_esposti,viol_amm_reg_com,censimentoAllPubb) " +
                            ",Sgomberi_immobili,Abitativo,nonAbitativo,Sgomberi_abus, NotificaTp" +
                        " Values('" + stat.mese.ToUpper() + "'," + stat.anno + "," + stat.relazioni + "," + stat.ponteggi + "," + stat.dpi + "," +
                          stat.esposti_ricevuti + "," + stat.esposti_evasi + "," + stat.ripristino_tot_par + "," + stat.controlli_scia + "," + stat.contr_cant_daily + "," + stat.cnr + "," +
                          stat.annotazioni + "," + stat.notifiche + "," + stat.sequestri + "," + stat.riapp_sigilli + "," + stat.deleghe_ricevute + "," +
                          stat.deleghe_esitate + "," + stat.cnr_annotazioni + "," + stat.interrogazioni + "," + stat.denunce_uff + "," + stat.convalide + "," +
                          stat.demolizioni + "," + stat.violazione_sigilli + "," + stat.dissequestri + "," + stat.dissequestri_temp + "," + stat.riapp_sigilli + "," +
                          stat.controlli_42_04 + "," + stat.contr_cant_suolo_pubb + "," + stat.contr_lavori_edili + "," + stat.contr_cant + "," + stat.contr_nato_da_esposti + "," + stat.viol_amm_reg_com + "," +
                          stat.censimentoAllPubb + "," + stat.Sgomberi_immobili + "," + stat.Abitativo + "," + stat.NonAbitativo + "," + stat.Sgomberi_abus + "," + stat.NotificaTp + ")";


                    else
                    {
                        sql_Statistiche = "update statistiche set esposti_ricevuti = +" + stat.esposti_ricevuti + ", denunce_uff = +" + stat.denunce_uff +

                        " where mese = '" + @stat.mese + "' and anno = " + stat.anno;


                    }

                    command.CommandText = sql_Statistiche;
                    command.ExecuteNonQuery();

                    resp = true;
                }


                catch (Exception ex)
                {


                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine(ex.Message + @" - Errore in inserimento statistiche ");
                        sw.Close();
                    }

                    resp = false;


                }
                conn.Close();
                return resp;
            }

        }
        public Boolean InsStatPg(Boolean exist, Statistiche stat, Interrogatorio interr)
        {
            bool resp = true;

            string sql_Statistiche = String.Empty;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                SqlTransaction transaction = null;
                SqlCommand command = conn.CreateCommand();
                transaction = conn.BeginTransaction("trans");
                command.Transaction = transaction;

                try
                {
                    if (!exist)

                        sql_Statistiche = "insert into statistiche (mese,anno,relazioni,ponteggi,dpi,esposti_ricevuti,esposti_evasi,ripristino_tot_par,controlli_scia,contr_cant_daily,cnr,annotazioni,notifiche" +
                            ",sequestri,riapp_sigilli,deleghe_ricevute,deleghe_esitate,cnr_annotazioni,interrogazioni,denunce_uff,convalide,demolizioni" +
                            ",violazione_sigilli,dissequestri,dissequestri_temp,rimozione_sigilli,controlli_42_04,contr_cant_suolo_pubb,contr_lavori_edili,contr_cant,contr_nato_da_esposti,viol_amm_reg_com,censimentoAllPubb" +
                            ",Abitativo,nonAbitativo,Sgomberi_abus,Sgomberi_immobili,NotificaTp) " +
                        " Values('" + stat.mese.ToUpper() + "'," + stat.anno + "," + stat.relazioni + "," + stat.ponteggi + "," + stat.dpi + "," +
                          stat.esposti_ricevuti + "," + stat.esposti_evasi + "," + stat.ripristino_tot_par + "," + stat.controlli_scia + "," + stat.contr_cant_daily + "," + stat.cnr + "," +
                          stat.annotazioni + "," + stat.notifiche + "," + stat.sequestri + "," + stat.riapp_sigilli + "," + stat.deleghe_ricevute + "," +
                          stat.deleghe_esitate + "," + stat.cnr_annotazioni + "," + stat.interrogazioni + "," + stat.denunce_uff + "," + stat.convalide + "," +
                          stat.demolizioni + "," + stat.violazione_sigilli + "," + stat.dissequestri + "," + stat.dissequestri_temp + "," + stat.riapp_sigilli + "," +
                          stat.controlli_42_04 + "," + stat.contr_cant_suolo_pubb + "," + stat.contr_lavori_edili + "," + stat.contr_cant + "," + stat.contr_nato_da_esposti + "," + stat.viol_amm_reg_com + "," +
                          stat.censimentoAllPubb + "," + stat.Abitativo + "," + stat.NonAbitativo + "," + stat.Sgomberi_abus + "," + stat.Sgomberi_immobili + "," + stat.NotificaTp + ")";

                    else
                    {
                        sql_Statistiche = "update statistiche set interrogazioni = " + stat.interrogazioni +


                        " where mese = '" + @stat.mese + "' and anno = " + stat.anno;

                    }
                    int esito = 0;
                    command.CommandText = sql_Statistiche;
                    esito = command.ExecuteNonQuery();

                    if (esito > 0)
                    {
                        resp = InsInterrogatorio(interr);
                    }
                    if (resp == false)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    else
                    {
                        transaction.Commit();
                        resp = true;
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
                        sw.WriteLine(ex.Message + @" - Errore in inserimento statistiche ");
                        sw.Close();
                    }

                    resp = false;


                }
                conn.Close();
                return resp;
            }

        }
        /// <summary>
        /// cancella la scheda da modificare, modifica le statistiche
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="idScheda"></param>
        /// <returns></returns>
        public Boolean DeleteTranSchedaStatistiche(Statistiche stat, Int32 idScheda)
        {
            bool resp = true;
            string sql_insRap = String.Empty;
            string sql_Statistiche = String.Empty;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                SqlTransaction transaction = null;
                SqlCommand command = conn.CreateCommand();

                transaction = conn.BeginTransaction("trans");
                command.Transaction = transaction;


                try
                {
                    String Del_RappUote = "delete from RappUote where id_rapp_scheda = '" + idScheda + "'";

                    sql_Statistiche = "update statistiche set relazioni =  +  " + @stat.relazioni + ",ponteggi = +" + @stat.ponteggi + ",dpi =+" + @stat.dpi + ",esposti_ricevuti=  +" + @stat.esposti_ricevuti +
                    ",esposti_evasi =  +" + @stat.esposti_evasi + ",ripristino_tot_par =  +" + @stat.ripristino_tot_par + ",controlli_scia =  + " + @stat.controlli_scia +
                    ",contr_cant_daily =  +" + @stat.contr_cant_daily + ",cnr =   +" + stat.cnr + ", notifiche = +" + @stat.notifiche +
                    ",annotazioni = +" + @stat.annotazioni + ",deleghe_esitate =  + " + @stat.deleghe_esitate +
                    ",sequestri =  +" + @stat.sequestri + ",riapp_sigilli =  + " + @stat.riapp_sigilli + ",deleghe_ricevute =  +" + @stat.deleghe_ricevute +
                    ",cnr_annotazioni =  +" + @stat.cnr_annotazioni + ",interrogazioni =  +" + @stat.interrogazioni + ",denunce_uff =  +" + @stat.denunce_uff + ",convalide = +" + @stat.convalide +
                    ",demolizioni =  +" + @stat.demolizioni + ",violazione_sigilli =  +" + @stat.violazione_sigilli + ",dissequestri =  +" + @stat.dissequestri +
                    ",dissequestri_temp =" + @stat.dissequestri_temp + ",rimozione_sigilli =" + @stat.rimozione_sigilli + ",controlli_42_04 =" + @stat.controlli_42_04 +
                    ",contr_cant_suolo_pubb =  +" + @stat.contr_cant_suolo_pubb + ",contr_lavori_edili =  +" + @stat.contr_lavori_edili + ",contr_cant =  +" + @stat.contr_cant +
                    ",contr_nato_da_esposti =  + " + @stat.contr_nato_da_esposti +
                    ", viol_amm_reg_com =+ " + stat.viol_amm_reg_com + ",censimentoAllPubb =+ " + stat.censimentoAllPubb + ", Sgomberi_immobili =+ " + stat.Sgomberi_immobili +
                    ",Abitativo =+ " + stat.Abitativo + ",nonAbitativo =+ " + stat.NonAbitativo + ",Sgomberi_abus =+ " + stat.Sgomberi_abus + ", NotificaTp =+" + stat.NotificaTp +
                    " where mese = '" + @stat.mese + "' and anno = " + stat.anno;


                    command.CommandText = Del_RappUote;
                    object a = command.ExecuteScalar();


                    command.CommandText = sql_Statistiche;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    //  idN = Convert.ToInt32(a);
                    resp = true;
                }

                catch (Exception)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            // sw.WriteLine("matricola:" + rapp.matricola + ",data ins:" + rapp.data + ", " + ex.Message + @" - Errore in inserimento scheda intervento uote ");
                            sw.Close();
                        }
                    }
                    resp = false;


                }
                conn.Close();
                return resp;
            }

        }
        /// <summary>
        /// cancella la gestione pratica
        /// </summary>
        /// <param name="id_fascicolo"></param>
        /// <returns></returns>
        public Boolean DeleteGestionePraticaById(string id_fascicolo)
        {

            string sql = "delete  FROM gestionePratiche where id_gestionePratica = '" + id_fascicolo + "'";

            Boolean resp = false;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {

                    command.CommandText = sql;
                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                    {

                        resp = true;
                    }


                }

                catch (Exception)
                {
                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }

        /// <summary>
        /// Inserisce scheda nuova e alimenta la tabella statistiche
        /// </summary>
        /// <param name="rapp"></param>
        /// <param name="stat"></param>
        /// <param name="txt"></param>
        /// <param name="idN"></param>
        /// <returns></returns>
        public Boolean InsRappUoteStatistiche(RappUote rapp, Statistiche stat, string txt, out Int32 idN)
        {
            bool resp = true;
            string sql_insRap = String.Empty;
            string sql_Statistiche = String.Empty;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                SqlTransaction transaction = null;
                SqlCommand command = conn.CreateCommand();

                transaction = conn.BeginTransaction("trans");
                command.Transaction = transaction;
                idN = -1;

                try
                {
                    if (txt == "ins")
                    {


                        sql_Statistiche = "insert into statistiche (mese,anno,relazioni,ponteggi,dpi,esposti_ricevuti,esposti_evasi,ripristino_tot_par,controlli_scia,contr_cant_daily,cnr,annotazioni,notifiche" +
                            ",sequestri,riapp_sigilli,deleghe_ricevute,deleghe_esitate,cnr_annotazioni,interrogazioni,denunce_uff,convalide,demolizioni" +
                            ",violazione_sigilli,dissequestri,dissequestri_temp,rimozione_sigilli,controlli_42_04,contr_cant_suolo_pubb,contr_lavori_edili,contr_cant,contr_nato_da_esposti,viol_amm_reg_com,censimentoAllPubb" +
                            ",Abitativo,nonAbitativo,Sgomberi_abus,Sgomberi_immobili,NotificaTp) " +
                        " Values('" + stat.mese.ToUpper() + "'," + stat.anno + "," + stat.relazioni + "," + stat.ponteggi + "," + stat.dpi + "," +
                          stat.esposti_ricevuti + "," + stat.esposti_evasi + "," + stat.ripristino_tot_par + "," + stat.controlli_scia + "," + stat.contr_cant_daily + "," + stat.cnr + "," +
                          stat.annotazioni + "," + stat.notifiche + "," + stat.sequestri + "," + stat.riapp_sigilli + "," + stat.deleghe_ricevute + "," +
                          stat.deleghe_esitate + "," + stat.cnr_annotazioni + "," + stat.interrogazioni + "," + stat.denunce_uff + "," + stat.convalide + "," +
                          stat.demolizioni + "," + stat.violazione_sigilli + "," + stat.dissequestri + "," + stat.dissequestri_temp + "," + stat.riapp_sigilli + "," +
                          stat.controlli_42_04 + "," + stat.contr_cant_suolo_pubb + "," + stat.contr_lavori_edili + "," + stat.contr_cant + "," + stat.contr_nato_da_esposti + "," + stat.viol_amm_reg_com + "," +
                          stat.censimentoAllPubb + "," + stat.Abitativo + "," + stat.NonAbitativo + "," + stat.Sgomberi_abus + "," + stat.Sgomberi_immobili + "," + stat.NotificaTp + ")";

                    }
                    else
                    {
                        sql_Statistiche = "update statistiche set relazioni =  +" + @stat.relazioni + ",ponteggi = +" + @stat.ponteggi + ",dpi =+" + @stat.dpi + ",esposti_ricevuti=  +" + @stat.esposti_ricevuti +
                        ",esposti_evasi =  +" + @stat.esposti_evasi + ",ripristino_tot_par =  +" + @stat.ripristino_tot_par + ",controlli_scia =  +" + @stat.controlli_scia +
                        ",contr_cant_daily =  +" + @stat.contr_cant_daily + ",cnr = +" + stat.cnr + ", notifiche = +" + @stat.notifiche +
                        ",annotazioni = +" + @stat.annotazioni + ",deleghe_esitate = +" + @stat.deleghe_esitate +
                        ",sequestri =  +" + @stat.sequestri + ",riapp_sigilli = +" + @stat.riapp_sigilli + ",deleghe_ricevute =  +" + @stat.deleghe_ricevute +
                        ",cnr_annotazioni =  +" + @stat.cnr_annotazioni + ",interrogazioni =  +" + @stat.interrogazioni + ",denunce_uff =  +" + @stat.denunce_uff + ",convalide = +" + @stat.convalide +
                        ",demolizioni =  +" + @stat.demolizioni + ",violazione_sigilli =  +" + @stat.violazione_sigilli + ",dissequestri =  +" + @stat.dissequestri +
                        ",dissequestri_temp =+" + @stat.dissequestri_temp + ",rimozione_sigilli =+" + @stat.rimozione_sigilli + ",controlli_42_04 =+" + @stat.controlli_42_04 +
                        ",contr_cant_suolo_pubb =  +" + @stat.contr_cant_suolo_pubb + ",contr_lavori_edili =  +" + @stat.contr_lavori_edili + ",contr_cant =  +" + @stat.contr_cant +
                        ",contr_nato_da_esposti =  +" + @stat.contr_nato_da_esposti +
                        ", viol_amm_reg_com =+ " + stat.viol_amm_reg_com + ",censimentoAllPubb = +" + stat.censimentoAllPubb + ", Sgomberi_immobili = +" + stat.Sgomberi_immobili +
                        ",Abitativo = +" + stat.Abitativo + ",nonAbitativo = +" + stat.NonAbitativo + ",Sgomberi_abus = +" + stat.Sgomberi_abus + ", NotificaTp =+" + stat.NotificaTp +
                        " where mese = '" + @stat.mese + "' and anno = " + stat.anno;



                    }

                    sql_insRap = "insert into RappUote (rapp_numero_pratica, rapp_data,	rapp_nominativo,rapp_indirizzo,rapp_pattuglia," +
                     "rapp_delegaAG,	rapp_resa,	rapp_segnalazione,	rapp_esposto,rapp_numEsposti,rapp_notifica,	rapp_iniziativa,rapp_comandante," +
                     "rapp_coordinatore,	rapp_relazione,	rapp_cnr,rapp_annotazionePG,rapp_verbale_seq,rapp_esito_delega,	rapp_contestaz_amm," +
                     "rapp_convalida,rapp_disseq_def,rapp_disseq_temp,rapp_disseq_temp_Rim,rapp_disseq_temp_Riapp,rapp_violazione_sigilli," +
                     "rapp_controlliScia,rapp_accert_avvenuto,rapp_totale,rapp_parziale,	rapp_violazioneBeniCult,rapp_contr_cantiere_suolo_pubb," +
                     "rapp_contr_lavori_edili,rapp_contr_cantieri_seq,rapp_contr_da_esposti,rapp_contr_da_segn,rapp_attivita_interna,rapp_nota,rapp_data_consegna_intervento, rapp_capopattuglia,rapp_uote,rapp_uotp,rapp_dataInserimento, " +
                     "rapp_con_protezioni,rapp_senza_protezioni,rapp_matricola,rapp_non_avvenuto," +
                     "rapp_censimento_all_pubb,rapp_contr_occupazione_abus,rapp_contr_occ_abitativo,rapp_contr_occ_no_abitativo,rapp_sgomberi,rapp_sgomberi_abus,rapp_sgomberi_immobili,rapp_notifica_no_ag, " +
                     "rapp_quartiere,rapp_num_censimento_all_pubb,rapp_numero_controlli_cant_seq,rapp_giro_cantieri,rapp_accRichiesti,rapp_numAccRichiesti,rapp_verbOccCensimento,rapp_contrNatoDaAcc,rapp_NumcontrNatoDaAcc)" +
               " Values('" + rapp.pratica + "','" +
                 //@rapp.ora + "','" +
                 @rapp.data + "','" +
                 @rapp.nominativo.Replace("'", "''") + "','" +
                 @rapp.indirizzo.Replace("'", "''") + "','" +
                 @rapp.pattuglia.Replace("'", "''") + "','" +
                 @rapp.delegaAG + "','" +
                 @rapp.resa + "','" +
                 @rapp.segnalazione + "','" +
                 @rapp.esposti + "','" +
                 @rapp.num_esposti + "','" +
                 @rapp.notifica + "','" +
                 @rapp.iniziativa + "','" +
                 @rapp.cdr + "','" +
                 @rapp.coordinatore + "','" +
                 @rapp.relazione + "','" +
                 @rapp.cnr + "','" +
                 @rapp.annotazionePG + "','" +
                 @rapp.verbaleSeq + "','" +
                 @rapp.esitoDelega + "','" +
                 @rapp.contestazioneAmm + "','" +
                 @rapp.convalida + "','" +
                 @rapp.dissequestroDef + "','" +
                 @rapp.dissequestroTemp + "','" +
                 @rapp.rimozione + "','" +
                 @rapp.riapposizione + "','" +
                 @rapp.violazioneSigilli + "','" +
                 @rapp.controlliScia + "','" +
                 @rapp.accertAvvenutoRip + "','" +
                 @rapp.totale + "','" +
                 @rapp.parziale + "','" +
                 @rapp.violazioneBeniCult + "','" +
                 @rapp.contrCantSuoloPubb + "','" +
                 @rapp.contrEdiliDPI + "','" +
                 @rapp.contr_cantiereSeq + "','" +
                 @rapp.contrDaEsposti + "','" +
                 @rapp.contrDaSegn + "','" +
                 @rapp.attività_interna + "','" +
                 @rapp.nota.Replace("'", "''") + "','" +
                 @rapp.data_consegna_intervento + "','" + @rapp.capopattuglia.Replace("'", "''") + "','" +
                 @rapp.uote + "','" + @rapp.uotp + "','" + @rapp.dataInserimento + "','" + @rapp.conProt + "','" + @rapp.senzaProt + "','" +
                 @rapp.matricola.Replace("'", "''") + "','" + @rapp.non_avvenuto + "','" +
                 @rapp.censimento_all_pubb + "','" + @rapp.contr_occupazione_abus + "','" + @rapp.contr_occ_abitativo + "','" + @rapp.contr_occ_no_abitativo + "','" + @rapp.sgomberi + "','" +
                 @rapp.sgomberi_abus + "','" + @rapp.sgomberi_immobili + "','" + @rapp.notifica_no_ag + "','" + @rapp.quartiere.Replace("'", "''") + "'," +
                 @rapp.num_censimento_all_pubb + "," + @rapp.numero_controlli_cant_seq + ",'" + @rapp.giro_controlli +
                 //I- mod 31/01/2026 scheda int
                 "','" + @rapp.accRichiesti + "','" + @rapp.numAccRichiesti + "','" + @rapp.verbOccCensimento + "','" +
                 @rapp.contrNatoDaAcc + "','" + @rapp.NumcontrNatoDaAcc + "'" +
                 //F- mod 31/01/2026 scheda int
                 "); SELECT SCOPE_IDENTITY();";
                    command.CommandText = sql_insRap;
                    object a = command.ExecuteScalar();


                    command.CommandText = sql_Statistiche;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    idN = Convert.ToInt32(a);
                    resp = true;
                }

                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("matricola:" + rapp.matricola + ",data ins:" + rapp.data + ", " + ex.Message + @" - Errore in inserimento scheda intervento uote ");
                            sw.Close();
                        }
                    }
                    resp = false;


                }
                conn.Close();
                return resp;
            }

        }
        /// <summary>
        /// inseriesce la scheda intervento e update statistiche aggiungendo i nuovi valori statistici inseriti
        /// </summary>
        /// <param name="rapp"></param>
        /// <param name="stat"></param>
        /// <param name="txt"></param>
        /// <param name="idN"></param>
        /// <returns></returns>
        public Boolean InsRappUote(RappUote rapp, Statistiche stat, out Int32 idN)
        {
            bool resp = true;
            string sql_insRap = String.Empty;
            string sql_Statistiche = String.Empty;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                SqlTransaction transaction = null;
                SqlCommand command = conn.CreateCommand();

                transaction = conn.BeginTransaction("trans");
                command.Transaction = transaction;
                idN = -1;

                try
                {
                    sql_Statistiche = "update statistiche set relazioni =  +  " + @stat.relazioni + ",ponteggi = +" + @stat.ponteggi + ",dpi =+" + @stat.dpi + ",esposti_ricevuti=  +" + @stat.esposti_ricevuti +
                    ",esposti_evasi =  +" + @stat.esposti_evasi + ",ripristino_tot_par =  +" + @stat.ripristino_tot_par + ",controlli_scia =  + " + @stat.controlli_scia +
                    ",contr_cant_daily =  +" + @stat.contr_cant_daily + ",cnr =   +" + stat.cnr + ", notifiche = +" + @stat.notifiche +
                    ",annotazioni = +" + @stat.annotazioni + ",deleghe_esitate =  + " + @stat.deleghe_esitate +
                    ",sequestri =  +" + @stat.sequestri + ",riapp_sigilli =  + " + @stat.riapp_sigilli + ",deleghe_ricevute =  +" + @stat.deleghe_ricevute +
                    ",cnr_annotazioni =  +" + @stat.cnr_annotazioni + ",interrogazioni =  +" + @stat.interrogazioni + ",denunce_uff =  +" + @stat.denunce_uff + ",convalide = +" + @stat.convalide +
                    ",demolizioni =  +" + @stat.demolizioni + ",violazione_sigilli =  +" + @stat.violazione_sigilli + ",dissequestri =  +" + @stat.dissequestri +
                    ",dissequestri_temp =" + @stat.dissequestri_temp + ",rimozione_sigilli =" + @stat.rimozione_sigilli + ",controlli_42_04 =" + @stat.controlli_42_04 +
                    ",contr_cant_suolo_pubb =  +" + @stat.contr_cant_suolo_pubb + ",contr_lavori_edili =  +" + @stat.contr_lavori_edili + ",contr_cant =  +" + @stat.contr_cant +
                    ",contr_nato_da_esposti =  + " + @stat.contr_nato_da_esposti +
                    ", viol_amm_reg_com =+ " + stat.viol_amm_reg_com + ",censimentoAllPubb =+ " + stat.censimentoAllPubb + ", Sgomberi_immobili =+ " + stat.Sgomberi_immobili +
                    ",Abitativo =+ " + stat.Abitativo + ",nonAbitativo =+ " + stat.NonAbitativo + ",Sgomberi_abus =+ " + stat.Sgomberi_abus + ", NotificaTp =+" + stat.NotificaTp +
                    " where mese = '" + @stat.mese + "' and anno = " + stat.anno;


                    sql_insRap = "insert into RappUote (rapp_numero_pratica, rapp_data,	rapp_nominativo,rapp_indirizzo,rapp_pattuglia," +
                     "rapp_delegaAG,	rapp_resa,	rapp_segnalazione,	rapp_esposto,rapp_numEsposti,rapp_notifica,	rapp_iniziativa,rapp_comandante," +
                     "rapp_coordinatore,	rapp_relazione,	rapp_cnr,rapp_annotazionePG,rapp_verbale_seq,rapp_esito_delega,	rapp_contestaz_amm," +
                     "rapp_convalida,rapp_disseq_def,rapp_disseq_temp,rapp_disseq_temp_Rim,rapp_disseq_temp_Riapp,rapp_violazione_sigilli," +
                     "rapp_controlliScia,rapp_accert_avvenuto,rapp_totale,rapp_parziale,	rapp_violazioneBeniCult,rapp_contr_cantiere_suolo_pubb," +
                     "rapp_contr_lavori_edili,rapp_contr_cantieri_seq,rapp_contr_da_esposti,rapp_contr_da_segn,rapp_attivita_interna,rapp_nota,rapp_data_consegna_intervento, rapp_capopattuglia,rapp_uote,rapp_uotp,rapp_dataInserimento, " +
                     "rapp_con_protezioni,rapp_senza_protezioni,rapp_matricola,rapp_non_avvenuto," +
                     "rapp_censimento_all_pubb,rapp_contr_occupazione_abus,rapp_contr_occ_abitativo,rapp_contr_occ_no_abitativo,rapp_sgomberi,rapp_sgomberi_abus,rapp_sgomberi_immobili,rapp_notifica_no_ag, " +
                     "rapp_quartiere,rapp_num_censimento_all_pubb,rapp_numero_controlli_cant_seq,rapp_giro_cantieri,rapp_accRichiesti,rapp_numAccRichiesti,rapp_verbOccCensimento,rapp_contrNatoDaAcc,rapp_NumcontrNatoDaAcc,rapp_NumNotificheNoAg)" +
               " Values('" + rapp.pratica + "','" +
                 //@rapp.ora + "','" +                                                                              
                 @rapp.data + "','" +
                 @rapp.nominativo.Replace("'", "''") + "','" +
                 @rapp.indirizzo.Replace("'", "''") + "','" +
                 @rapp.pattuglia.Replace("'", "''") + "','" +
                 @rapp.delegaAG + "','" +
                 @rapp.resa + "','" +
                 @rapp.segnalazione + "','" +
                 @rapp.esposti + "','" +
                 @rapp.num_esposti + "','" +
                 @rapp.notifica + "','" +
                 @rapp.iniziativa + "','" +
                 @rapp.cdr + "','" +
                 @rapp.coordinatore + "','" +
                 @rapp.relazione + "','" +
                 @rapp.cnr + "','" +
                 @rapp.annotazionePG + "','" +
                 @rapp.verbaleSeq + "','" +
                 @rapp.esitoDelega + "','" +
                 @rapp.contestazioneAmm + "','" +
                 @rapp.convalida + "','" +
                 @rapp.dissequestroDef + "','" +
                 @rapp.dissequestroTemp + "','" +
                 @rapp.rimozione + "','" +
                 @rapp.riapposizione + "','" +
                 @rapp.violazioneSigilli + "','" +
                 @rapp.controlliScia + "','" +
                 @rapp.accertAvvenutoRip + "','" +
                 @rapp.totale + "','" +
                 @rapp.parziale + "','" +
                 @rapp.violazioneBeniCult + "','" +
                 @rapp.contrCantSuoloPubb + "','" +
                 @rapp.contrEdiliDPI + "','" +
                 @rapp.contr_cantiereSeq + "','" +
                 @rapp.contrDaEsposti + "','" +
                 @rapp.contrDaSegn + "','" +
                 @rapp.attività_interna + "','" +
                 @rapp.nota.Replace("'", "''") + "','" +
                 @rapp.data_consegna_intervento + "','" + @rapp.capopattuglia.Replace("'", "''") + "','" +
                 @rapp.uote + "','" + @rapp.uotp + "','" + @rapp.dataInserimento + "','" + @rapp.conProt + "','" + @rapp.senzaProt + "','" +
                 @rapp.matricola.Replace("'", "''") + "','" + @rapp.non_avvenuto + "','" +
                 @rapp.censimento_all_pubb + "','" + @rapp.contr_occupazione_abus + "','" + @rapp.contr_occ_abitativo + "','" + @rapp.contr_occ_no_abitativo + "','" + @rapp.sgomberi + "','" +
                 @rapp.sgomberi_abus + "','" + @rapp.sgomberi_immobili + "','" + @rapp.notifica_no_ag + "','" + @rapp.quartiere.Replace("'", "''") + "'," +
                 @rapp.num_censimento_all_pubb + "," + @rapp.numero_controlli_cant_seq + ",'" + @rapp.giro_controlli +
                 //I- mod 31/01/2026 scheda int
                 "','" + @rapp.accRichiesti + "','" + @rapp.numAccRichiesti + "','" + @rapp.verbOccCensimento + "','" +
                 @rapp.contrNatoDaAcc + "'," + @rapp.NumcontrNatoDaAcc + "," +
                 @rapp.NumNotificheNoAg + "" +
                 //F- mod 31/01/2026 scheda int
                 "); SELECT SCOPE_IDENTITY();";
                    command.CommandText = sql_insRap;
                    object a = command.ExecuteScalar();


                    command.CommandText = sql_Statistiche;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    idN = Convert.ToInt32(a);
                    resp = true;
                }

                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("matricola:" + rapp.matricola + ",data ins:" + rapp.data + ", " + ex.Message + @" - Errore in inserimento scheda intervento uote ");
                            sw.Close();
                        }
                    }
                    resp = false;


                }
                conn.Close();
                return resp;
            }

        }
        /// <summary>
        /// inserisce scheda intervento
        /// </summary>
        /// <param name="rapp"></param>
        /// <returns></returns>
        public Boolean InsRappUote(RappUote rapp, out Int32 idN)
        {
            bool resp = true;
            string sql_insRap = String.Empty;
            string sql_Statistiche = String.Empty;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                //SqlTransaction transaction = null;
                SqlCommand command = conn.CreateCommand();

                // transaction = conn.BeginTransaction("trans");
                //command.Transaction = transaction;
                idN = -1;

                try
                {

                    sql_insRap = "insert into RappUote (rapp_numero_pratica, rapp_data,	rapp_nominativo,rapp_indirizzo,rapp_pattuglia," +
                     "rapp_delegaAG,	rapp_resa,	rapp_segnalazione,	rapp_esposto,rapp_numEsposti,rapp_notifica,	rapp_iniziativa,rapp_comandante," +
                     "rapp_coordinatore,	rapp_relazione,	rapp_cnr,rapp_annotazionePG,rapp_verbale_seq,rapp_esito_delega,	rapp_contestaz_amm," +
                     "rapp_convalida,rapp_disseq_def,rapp_disseq_temp,rapp_disseq_temp_Rim,rapp_disseq_temp_Riapp,rapp_violazione_sigilli," +
                     "rapp_controlliScia,rapp_accert_avvenuto,rapp_totale,rapp_parziale,	rapp_violazioneBeniCult,rapp_contr_cantiere_suolo_pubb," +
                     "rapp_contr_lavori_edili,rapp_contr_cantieri_seq,rapp_contr_da_esposti,rapp_contr_da_segn,rapp_attivita_interna,rapp_nota,rapp_data_consegna_intervento, rapp_capopattuglia,rapp_uote,rapp_uotp,rapp_dataInserimento, " +
                     "rapp_con_protezioni,rapp_senza_protezioni,rapp_matricola,rapp_non_avvenuto," +
                     "rapp_censimento_all_pubb,rapp_contr_occupazione_abus,rapp_contr_occ_abitativo,rapp_contr_occ_no_abitativo,rapp_sgomberi,rapp_sgomberi_abus,rapp_sgomberi_immobili,rapp_notifica_no_ag, " +
                     "rapp_quartiere,rapp_num_censimento_all_pubb,rapp_numero_controlli_cant_seq,rapp_giro_cantieri,rapp_accRichiesti,rapp_numAccRichiesti,rapp_verbOccCensimento,rapp_contrNatoDaAcc,rapp_NumcontrNatoDaAcc)" +
        " Values('" + rapp.pratica + "','" +
        //@rapp.ora + "','" +
        @rapp.data + "','" +
        @rapp.nominativo.Replace("'", "''") + "','" +
        @rapp.indirizzo.Replace("'", "''") + "','" +
        @rapp.pattuglia.Replace("'", "''") + "','" +
        @rapp.delegaAG + "','" +
        @rapp.resa + "','" +
        @rapp.segnalazione + "','" +
        @rapp.esposti + "','" +
        @rapp.num_esposti + "','" +
        @rapp.notifica + "','" +
        @rapp.iniziativa + "','" +
        @rapp.cdr + "','" +
        @rapp.coordinatore + "','" +
        @rapp.relazione + "','" +
        @rapp.cnr + "','" +
        @rapp.annotazionePG + "','" +
        @rapp.verbaleSeq + "','" +
        @rapp.esitoDelega + "','" +
        @rapp.contestazioneAmm + "','" +
        @rapp.convalida + "','" +
        @rapp.dissequestroDef + "','" +
        @rapp.dissequestroTemp + "','" +
        @rapp.rimozione + "','" +
        @rapp.riapposizione + "','" +
        @rapp.violazioneSigilli + "','" +
        @rapp.controlliScia + "','" +
        @rapp.accertAvvenutoRip + "','" +
        @rapp.totale + "','" +
        @rapp.parziale + "','" +
        @rapp.violazioneBeniCult + "','" +
        @rapp.contrCantSuoloPubb + "','" +
        @rapp.contrEdiliDPI + "','" +
        @rapp.contr_cantiereSeq + "','" +
        @rapp.contrDaEsposti + "','" +
        @rapp.contrDaSegn + "','" +
        @rapp.attività_interna + "','" +
        @rapp.nota.Replace("'", "''") + "','" +
        @rapp.data_consegna_intervento + "','" + @rapp.capopattuglia.Replace("'", "''") + "','" +
        @rapp.uote + "','" + @rapp.uotp + "','" + @rapp.dataInserimento + "','" + @rapp.conProt + "','" + @rapp.senzaProt + "','" +
        @rapp.matricola.Replace("'", "''") + "','" + @rapp.non_avvenuto + "','" +
        @rapp.censimento_all_pubb + "','" + @rapp.contr_occupazione_abus + "','" + @rapp.contr_occ_abitativo + "','" + @rapp.contr_occ_no_abitativo + "','" + @rapp.sgomberi + "','" +
        @rapp.sgomberi_abus + "','" + @rapp.sgomberi_immobili + "','" + @rapp.notifica_no_ag + "','" + @rapp.quartiere.Replace("'", "''") + "'," +
        @rapp.num_censimento_all_pubb + "," + @rapp.numero_controlli_cant_seq + ",'" + @rapp.giro_controlli +
                 //I- mod 31/01/2026 scheda int
                 "','" + @rapp.accRichiesti + "','" + @rapp.numAccRichiesti + "','" + @rapp.verbOccCensimento + "','" +
                 @rapp.contrNatoDaAcc + "','" + @rapp.NumcontrNatoDaAcc + "'" +
                 //F- mod 31/01/2026 scheda int
                 "); SELECT SCOPE_IDENTITY();";

                    command.CommandText = sql_insRap;
                    object a = command.ExecuteScalar();
                    idN = Convert.ToInt32(a);
                    resp = true;
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("matricola:" + rapp.matricola + ",data ins:" + rapp.data + ", " + ex.Message + @" - Errore in inserimento scheda intervento uote ");
                        sw.Close();
                    }
                    resp = false;


                }
                conn.Close();
                return resp;
            }

        }

        public Boolean InsGestionePratica(GestionePratiche pr)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {

                sql_pratica = "insert into gestionePratiche (fascicolo, assegnato, data_uscita, data_rientro, data_spostamenti, data_riscontro,note,NOTA_SPOSTAMENTO,NOTA_RISCONTRO,QUARTIERE)" +
                   " Values('" + @pr.fascicolo + "','" + @pr.assegnato.Replace("'", "''") + "','" + @pr.data_uscita + "','" + @pr.data_rientro.Replace("'", "''") + "','" + @pr.data_spostamenti.Replace("'", "''") + "','" +
                   @pr.data_rientro.Replace("'", "''") + "','" + @pr.note.Replace("'", "''") + "','" + @pr.notaSpostamento.Replace("'", "''") + "','" + @pr.notariscontro.Replace("'", "''") + "','" + @pr.quartiere.Replace("'", "''") + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "gestione pratica";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("fascicolo " + pr.fascicolo + ", assegnato:" + pr.assegnato + ", data ins:" + pr.data_uscita + ", " + ex.Message + @" - Errore in inserimento gestione pratica ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        //FINE INSERIMENTO
        public DataTable GetSchedeInfo(string boxChiamante, string mese, string anno)
        {
            string sql = string.Empty;
            //  trasformo stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DataTable tb = new DataTable();
            switch (boxChiamante)
            {
                case "EspostiEvasi":
                    sql = "SELECT * FROM rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_esposto='true'";
                    break;
                case "Relazioni":
                    sql = "SELECT * FROM rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_relazione='true'";
                    break;
                case "Ponteggi":
                    sql = "SELECT * FROM rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_contr_cantiere_suolo_pubb='true'";
                    break;
                case "DPI":
                    sql = "SELECT * FROM rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_contr_lavori_edili='true'";
                    break;
                case "SCIA":
                    sql = "SELECT * FROM rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_controlliScia='true'";
                    break;
                case "Annotazioni":
                    sql = "SELECT * FROM rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_annotazionePG='true'";
                    break;
                case "Notifiche":
                    sql = "SELECT * FROM rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_notifica='true'";
                    break;
                case "NotificheNoAG":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_notifica_no_ag='true'";
                    break;
                case "Sequestri":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_verbale_seq='true'";
                    break;
                case "RiappSigilli":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_disseq_temp_Riapp='true'";
                    break;
                case "DelegheEsitate":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_esito_delega='true'";
                    break;
                case "Convalide":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_convalida='true'";
                    break;
                case "ViolazioneSigilli":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_violazione_sigilli='true'";
                    break;
                case "DissequestriTemp":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_disseq_temp='true'";
                    break;
                case "Dissequestri":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_disseq_def='true'";
                    break;
                case "RimozioneSigilli":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_disseq_temp_Rim='true'";
                    break;
                case "ControlliDLGS":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_violazioneBeniCult='true'";
                    break;
                case "ControlliCant":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_contr_cantieri_seq='true' or rapp_giro_cantieri='true'";
                    break;
                case "ViolAmmRegCom":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_contestaz_amm='true'";
                    break;
                case "CensimentoAllPubb":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_censimento_all_pubb='true'";
                    break;
                case "OccupAbusivaAbit":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_contr_occ_abitativo='true'";
                    break;
                case "OccupAbusivaNoAbit":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_contr_occ_no_abitativo='true'";
                    break;
                case "SgomberiAbus":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and txtSgomberiAbus='true'";
                    break;
                case "SgomberiImmobili":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_sgomberi_immobili='true'";
                    break;
                case "AccertAltriEnti":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_contrNatoDaAcc='true'";
                    break;
                case "CNR":
                    sql = "SELECT *  from rappuote where Year(rapp_data_consegna_intervento) ='" + anno + "' AND month(rapp_data_consegna_intervento)='" + meseS + "' and rapp_cnr='true'";
                    break;
                default:
                    break;
            }


            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string msg = string.Empty;
                return tb = FillTable(sql, conn, out msg);
            }


        }
        public DataTable GetSchedeBy(string numPratica, string pattuglia, string dataI, Boolean attivita, int id, string quartiere)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            if (!String.IsNullOrEmpty(pattuglia))
            {

                sql = "SELECT * FROM RappUote where rapp_pattuglia like '%" + pattuglia + "%'" + " order by rapp_pattuglia";
            }

            if (!String.IsNullOrEmpty(numPratica))
            {
                if (!String.IsNullOrEmpty(quartiere))

                    sql = "SELECT * FROM RappUote where rapp_numero_pratica = '" + numPratica + "' and rapp_quartiere = '" + quartiere.Replace("'", "''") + "' order by rapp_numero_pratica";

                else
                    sql = "SELECT * FROM RappUote where rapp_numero_pratica = '" + numPratica + "'" + " order by rapp_numero_pratica";
            }

            if (!String.IsNullOrEmpty(dataI))

            {
                DateTime dtI = System.Convert.ToDateTime(dataI);
                sql = "SELECT * FROM RappUote where rapp_data = '" + dtI.ToShortDateString() + "' order by rapp_data";
            }
            if (id > 0)
            {

                sql = "SELECT * FROM RappUote where id_rapp_scheda =" + id + "";
            }
            if (attivita == true)
            {
                sql = "SELECT * FROM RappUote where rapp_attivita_interna ='" + "True' order by rapp_data";

                if (!String.IsNullOrEmpty(pattuglia))
                {
                    sql = "SELECT * FROM RappUote where rapp_attivita_interna ='" + "True' and rapp_pattuglia like '%" + pattuglia + "%'" + " order by rapp_pattuglia";
                }
                if (!String.IsNullOrEmpty(dataI))
                {
                    DateTime dtI = System.Convert.ToDateTime(dataI);
                    sql = "SELECT * FROM RappUote where rapp_attivita_interna ='" + "True' and rapp_data = '" + dtI.ToShortDateString() + "' order by rapp_data";
                }

            }
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string msg = string.Empty;
                return tb = FillTable(sql, conn, out msg);
            }


        }
        public DataTable GetScheda(string numeroP, string nominativo, string pattuglia)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            if (!String.IsNullOrEmpty(pattuglia))
            {
                sql = "SELECT * FROM RappUote where rapp_pattuglia like '" + pattuglia + "%'" + " order by rapp_pattuglia";
            }
            else if (!String.IsNullOrEmpty(numeroP))
            {

                sql = "SELECT * FROM RappUote where rapp_numero_pratica = '" + numeroP + "' order by rapp_numero_pratica";
            }
            else if (!String.IsNullOrEmpty(nominativo))
            {

                sql = "SELECT * FROM RappUote where rapp_nominativo like '" + nominativo + "%'" + " order by rapp_nominativo";
            }
            //if (dtConsegna.ToShortDateString() != "01/01/2000")
            //{

            //    sql = "SELECT * FROM RappUote where rapp_data_consegna_intervento = '" + dtConsegna.ToShortDateString() + "'";
            //}
            //if (dtIntervento.ToShortDateString() != "01/01/2000")
            //{

            //    sql = "SELECT * FROM RappUote where rapp_data = '" + dtIntervento.ToShortDateString() + "'";
            //}
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string msg = string.Empty;
                return tb = FillTable(sql, conn, out msg);
            }


        }
        public DataTable getObiettivi(int anno)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();
            sql = "SELECT impalcature,dpi,contcantseq,contr_esposti,cens_allogg_pubb,occ_prop_com_abit,occ_prop_com_no_abit,contr_nati_da_accer_richiesti FROM obiettivi where anno =" + anno;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string msg = string.Empty;
                return tb = FillTable(sql, conn, out msg);
            }
        }
        /// <summary>
        /// preleva le statistiche per mese e anno
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable GetStatistiche(string mese, int anno)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();
            sql = "SELECT * FROM statistiche where mese = '" + mese + "' and anno =" + anno;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string msg = string.Empty;
                return tb = FillTable(sql, conn, out msg);
            }


        }
        /// <summary>
        /// calcola statistiche annuali
        /// </summary>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable GetStatisticheAnnuali(int anno)
        {

            string sql = string.Empty;
            DataTable tb = new DataTable();
            sql = "SELECT DATENAME(month, rapp_data_consegna_intervento) AS Mese," +
                "SUM(CASE WHEN rapp_contr_cantiere_suolo_pubb = 'true' THEN 1 ELSE 0 END) as rapp_contr_cantiere_suolo_pubb, " +
                "SUM(CASE WHEN rapp_contr_lavori_edili = 'true' THEN 1 ELSE 0 END) as rapp_contr_lavori_edili, " +
                "SUM((CASE WHEN rapp_contr_cantieri_seq = 'true' THEN 1 ELSE 0 END) + (CASE WHEN rapp_giro_cantieri = 'true' THEN rapp_numero_controlli_cant_seq ELSE 0 END)) as rapp_contr_cantieri_seq, " +
                "SUM(CASE WHEN ISNUMERIC(rapp_numEsposti) = 1 THEN CAST(rapp_numEsposti AS DECIMAL(18, 0))  ELSE 0 END) as rapp_numEsposti, " +
                "SUM(CASE WHEN rapp_censimento_all_pubb = 1 THEN rapp_num_censimento_all_pubb ELSE 0 END) as rapp_censimento_all_pubb, " +
                "SUM(CASE WHEN rapp_contr_occ_abitativo = 'true' THEN 1 ELSE 0 END) as rapp_contr_occ_abitativo, " +
                "SUM(CASE WHEN rapp_contr_occ_no_abitativo = 'true' THEN 1 ELSE 0 END)  as rapp_contr_occ_no_abitativo, " +
                "SUM(CASE WHEN ISNUMERIC(rapp_NumcontrNatoDaAcc) = 1 THEN CAST(rapp_NumcontrNatoDaAcc AS DECIMAL(18, 0))  ELSE 0 END) as rapp_NumcontrNatoDaAcc " +
                //"SUM(CASE WHEN rapp_NumcontrNatoDaAcc = 'true' THEN 1 ELSE 0 END)  as rapp_NumcontrNatoDaAcc " +
                "FROM rappuote WHERE DATEPART(year, rapp_data_consegna_intervento) =" + anno +
                " GROUP BY DATENAME(month, rapp_data_consegna_intervento), DATEPART(month, rapp_data_consegna_intervento)  " +
                " ORDER BY DATEPART(month, rapp_data_consegna_intervento)";
            //sql = "SELECT DATENAME(month, rapp_data_consegna_intervento) AS Mese," +
            //     "SUM(CASE WHEN rapp_contr_cantiere_suolo_pubb = 'true' THEN 1 ELSE 0 END) AS Impalcature, " +
            //     "SUM(CASE WHEN rapp_contr_lavori_edili = 'true' THEN 1 ELSE 0 END) AS DPI, " +
            //     "Sum(case WHEN rapp_contr_cantieri_seq = 'true' THEN 1 ELSE 0 END ) as Contr_Cant_Squestrati, " +
            //     "SUM(CASE WHEN ISNUMERIC(rapp_numEsposti) = 1 THEN CAST(rapp_numEsposti AS DECIMAL(18, 0))  ELSE 0 END) AS Controlli_Esposti, " +
            //     "SUM(CASE WHEN rapp_censimento_all_pubb = 1 THEN rapp_num_censimento_all_pubb ELSE 0 END) AS Cens_Nuclei_Alloggi_Pubb, " +
            //     "SUM(CASE WHEN rapp_contr_occ_abitativo = 'true' THEN 1 ELSE 0 END) AS Occ_Prop_Com_uso_Abit, " +
            //     "SUM(CASE WHEN rapp_contr_occ_no_abitativo = 'true' THEN 1 ELSE 0 END) AS Occ_Prop_Com_uso_No_Abit " +
            //     "FROM rappuote WHERE DATEPART(year, rapp_data_consegna_intervento) =" + anno +
            //     " GROUP BY DATENAME(month, rapp_data_consegna_intervento), DATEPART(month, rapp_data_consegna_intervento)  " +
            //     " ORDER BY DATEPART(month, rapp_data_consegna_intervento)";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string msg = string.Empty;
                return tb = FillTable(sql, conn, out msg);
            }


        }
        /// <summary>
        /// preleva il numero di relazioni redatte
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumRelazione(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformo stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_relazione) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_relazione='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// prteleva il numero di annotazioni fatte
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumAnnotazioni(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_annotazionePG) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_annotazionePG='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di notifiche fatte
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumNotifiche(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_notifica) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_notifica='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// prreleva il numero di deleghe esitate
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumDelegheEsitate(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_esito_delega) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_esito_delega='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// prelevo numero esposti evasi
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetEspostiEvasi(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT SUM(TRY_CAST(rapp_numEsposti AS DECIMAL(18, 0))) AS SommaTotale FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di ponteggi controllati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumPonteggi(string mese, int anno)
        {
            string sql = string.Empty;
            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));

            sql = "SELECT count(rapp_contr_cantiere_suolo_pubb) as n FROM rappuote where rapp_data_consegna_intervento between '" + @dataInizio.ToShortDateString() + "' AND '" + @dataFine.ToShortDateString() + "' and rapp_contr_cantiere_suolo_pubb='true'";

            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// prelevo num vilazioni amministrative
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumViolAmm(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_contestaz_amm) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_contestaz_amm='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di ripristini effettuati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumRipristino(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_accert_avvenuto) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_accert_avvenuto='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di controlli scia effettuati  
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumcontrolliScia(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_controlliScia) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_controlliScia='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di sequestri effettuati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumSequestri(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_verbale_seq) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_verbale_seq='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di riapposizioni sigilli effettuati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumRiappSigilli(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_disseq_temp_Riapp) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_disseq_temp_Riapp='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di convalide effettuate
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumConvalide(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_convalida) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_convalida='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di violazioni sigilli effettuate
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumViolSigilli(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_violazione_sigilli) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_violazione_sigilli='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di dissequestri temporanei effettuati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumDisseqTemp(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_disseq_temp) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_disseq_temp='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di dissequestri definitivi effettuati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumDissequestri(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_disseq_def) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_disseq_def='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di rimozioni sigilli effettuati   
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumRimozSigilli(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_disseq_temp_Rim) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_disseq_temp_Rim='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di controlli beni culturali effettuati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumControlliDlgs(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_violazioneBeniCult) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_violazioneBeniCult='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        public string GetNumControlliCant(string mese, int anno)
        {
            string sql = string.Empty;
            string giro = string.Empty;
            //  trasformo stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_contr_cantieri_seq) as cantieri FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_contr_cantieri_seq='true'";
            giro = "SELECT sum(isnull(rapp_numero_controlli_cant_seq,0)) as giro FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "'";

            int tot = 0;
            string res = string.Empty;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    //return res = command.ExecuteScalar().ToString();
                    res = command.ExecuteScalar().ToString();
                    tot = System.Convert.ToInt32(res);
                    command.CommandText = giro;
                    string a = command.ExecuteScalar().ToString();
                    tot += Convert.ToInt32(a); //sommo i due valori provenienti dai controlli cantieri e dai numeri dei controlli cantieri fatti nei giri
                    return tot.ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        public string GetNumCensimentoAllPubb(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT Sum(rapp_num_censimento_all_pubb) as n FROM rappuote where rapp_data_consegna_intervento between'" + @dataInizio.ToShortDateString() + "' AND '" + @dataFine.ToShortDateString() + "'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        public string GetNumSgomberiAbus(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_sgomberi_abus) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_sgomberi_abus='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        public string GetNumSgomberiImmobili(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_sgomberi_immobili) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_sgomberi_immobili='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        public string GetNumAccertAltriEnti(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT SUM(TRY_CAST(rapp_numcontrNatoDaAcc AS DECIMAL(18, 0))) AS SommaTotale FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "'";

            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore GetNumAccertAltriEnti ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        public string GetNumNotificheNoAg(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformo stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            // sql = "SELECT count(rapp_notifica_no_ag) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_notifica_no_ag='true'";
            sql = "SELECT SUM(TRY_CAST(rapp_NumNotificheNoAg AS DECIMAL(18, 0))) AS SommaTotale FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "'";

            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - GetNumNotificheNoAg ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        public string GetNumOccAbusAbitat(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_contr_occ_abitativo) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_contr_occ_abitativo='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        public string GetNumOccAbusNoAbitat(string mese, int anno)
        {
            string sql = string.Empty;

            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_contr_occ_no_abitativo) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_contr_occ_no_abitativo='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero dei dpi controllati
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public string GetNumDpi(string mese, int anno)
        {
            string sql = string.Empty;
            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_contr_lavori_edili) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_contr_lavori_edili='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }

        public string GetNumCnr(string mese, int anno)
        {
            string sql = string.Empty;
            //  trasformio stringa mese in numero;
            string meseS = GetNumeroMeseByText(mese);
            DateTime dataInizio = new DateTime(anno, System.Convert.ToInt32(meseS), 1);
            DateTime dataFine = new DateTime(anno, System.Convert.ToInt32(meseS), DateTime.DaysInMonth(anno, System.Convert.ToInt32(meseS)));
            sql = "SELECT count(rapp_cnr) as n FROM rappuote where rapp_data_consegna_intervento >='" + @dataInizio.ToShortDateString() + "' AND rapp_data_consegna_intervento<='" + @dataFine.ToShortDateString() + "' and rapp_cnr='true'";
            string res = null;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql;
                    return res = command.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("mese " + mese + ", anno: " + anno + ex.Message + @" - Errore in inserimento dati ");
                        sw.Close();
                    }

                }
                conn.Close();
                conn.Dispose();
                return res;
            }

        }
        /// <summary>
        /// preleva il numero di deleghe ricevute dalla procura
        /// </summary>
        /// <param name="mese"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public int GetDelegheRicevute(string mese, int anno)
        {
            int number = 0;
            string sql = string.Empty;
            DataTable tb = new DataTable();
            String meseN = string.Empty;
            Manager mn = new Manager();
            //trasforma il mese in numero
            string meseS = mn.GetNumeroMeseByText(mese);
            Tipologie delIndagine = Tipologie.DelegaIndagine;
            string DelegaIndagine = delIndagine.GetDescription();

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                sql = "SELECT count(tipoProvvedimentoAg) FROM principale where  TIPOPROVVEDIMENTOAG = '" + DelegaIndagine + "' AND dataarrivo LIKE '" + anno + "-" + meseS + "%'";
                string msg = string.Empty;
                return number = Convert.ToInt32(FillTable(sql, conn, out msg).Rows[0][0]);
            }

        }
        public int GetEspostiRicevute(string mese, int anno, out string msg)
        {
            int number = 0;
            string sql = string.Empty;
            DataTable tb = new DataTable();
            String meseN = string.Empty;

            msg = string.Empty;
            Manager mn = new Manager();
            //trasforma il mese in numero  
            string meseS = mn.GetNumeroMeseByText(mese);
            Tipologie esp = Tipologie.EspostoSegnalazione;
            string Esposti = esp.GetDescription();

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                //sql = "SELECT count(Tipologia_atto) FROM principale " +
                //    "WHERE Tipologia_atto = '" + Esposti + "' " +
                //    "AND YEAR(dataarrivo) = " + anno + " " +
                //    "AND MONTH(dataarrivo) = " + meseS;



                sql = "SELECT sum(NumProtRicStessoCarico) FROM principale " +
                    "WHERE Tipologia_atto = '" + Esposti + "' " +
                    "AND YEAR(dataarrivo) = " + anno + " " +
                    "AND MONTH(dataarrivo) = " + meseS;

                try
                {
                    // Esegue la query
                    // Nota: presumo che FillTable apra la connessione se chiusa
                    DataTable dt = FillTable(sql, conn, out msg);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        // Gestione DBNull: Se il conteggio è nullo restituisce 0
                        object result = dt.Rows[0][0];
                        return (result == DBNull.Value) ? 0 : Convert.ToInt32(result);
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    // 3. ASSEGNAZIONE OBBLIGATORIA DI MSG NEL CATCH
                    msg = "Errore durante il conteggio esposti: " + ex.Message;
                    return 0;
                }


            }

        }
        /// <summary>
        /// trasforma il mese in numero
        /// </summary>
        /// <param name="mese"></param>
        /// <returns></returns>
        public string GetNumeroMeseByText(string mese)
        {

            string sql = string.Empty;
            String meseN = string.Empty;
            //trasforma il mese in numero   
            string meseS = @"DECLARE @NomeMese NVARCHAR(20) SET @NomeMese ='" + mese + "' SELECT FORMAT(MONTH(CAST(@NomeMese +' 1, 2000' AS DATETIME)), 'D2') AS NumeroMese";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                meseN = Convert.ToString(FillTable(meseS, conn, out msg).Rows[0][0]);
            }

            return meseN;

        }
        /// <summary>
        /// cercala scheda per id 
        /// </summary>

        /// <param name="pattuglia"></param>
        /// <returns></returns>
        public DataTable GetSchedaById(string Idschedaa)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();
            sql = "SELECT * FROM RappUote where id_rapp_scheda = '" + Idschedaa + "'";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn, out msg);
            }


        }
        public Boolean SavePraticaArchivioUote(ArchivioUote arch)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {

                sql_pratica = "insert into ArchivioUote (arch_numPratica,arch_doppione,arch_dataIns,arch_datault_intervento,arch_indirizzo,arch_responsabile,arch_natoA,arch_dataNascita," +
                    "arch_inCarico,arch_evasa,arch_note,arch_tipologia,arch_quartiere,arch_suoloPub,arch_vincoli,arch_1089,arch_demolita,arch_allegati,arch_matricola,arch_sezione,arch_foglio,arch_particella,arch_sub,arch_dataInizioAttivita,arch_propPriv,arch_propComune,arch_propBeniCult,arch_propAltriEnti,arch_foglionct,arch_particellanct,arch_beniConfiscati)" +
                   " Values('" + @arch.arch_numPratica + "','" + @arch.arch_bis + "','" + @arch.arch_dataIns + "','" +
                   @arch.arch_datault_intervento + "','" + @arch.arch_indirizzo.Replace("'", "''") + "','" +
                   @arch.arch_responsabile.Replace("'", "''") + "','" + @arch.arch_natoA.Replace("'", "''") + "','" + @arch.arch_dataNascita + "','" +
                   @arch.arch_inCarico.Replace("'", "''") + "','" + @arch.arch_evasa + "','" + @arch.arch_note.Replace("'", "''") + "','" +
                   @arch.arch_tipologia.Replace("'", "''") + "','" + @arch.arch_quartiere.Replace("'", "''") + "','" + @arch.arch_suoloPub + "','" +
                   @arch.arch_vincoli + "','" + @arch.arch_1089 + "','" + @arch.arch_demolita + "','" +
                   @arch.arch_allegati.Replace("'", "''") + "','" + @arch.arch_matricola + "','" + @arch.arch_sezione.Replace("'", "''") + "','" + @arch.arch_foglio + "','" + @arch.arch_particella + "','" + @arch.arch_sub + "','" + @arch.arch_dataInizioAttivita + "','" +
                   @arch.arch_propPriv + "','" + @arch.arch_propBeniCult + "','" + @arch.arch_propComune + "','" + @arch.arch_propAltriEnti + "','" + @arch.arch_foglioNct + "','" + @arch.arch_particellaNct + "','" + @arch.arch_beniConfiscati + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "ArchivioUote";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("pratica " + arch.arch_numPratica + ", matricola:" + arch.arch_matricola + ", data ins:" + arch.arch_dataIns + ", " + ex.Message + @" - Errore in inserimento dati ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        /// <summary>
        /// salva pratica uotp
        /// </summary>
        /// <param name="arch"></param>
        /// <returns></returns>
        public Boolean SavePraticaArchivioUotp(ArchivioUotp arch, out string msg)
        {
            bool resp = true;
            msg = string.Empty;
            string sql_pratica = String.Empty;
            string sql_cartellina = String.Empty;
            string sql_Verificacartellina = String.Empty;
            string testoSql = string.Empty;
            int NCartella = 0;
            int @max = 0;
            sql_pratica = "insert into Archiviotp (Num_Prot,ProtGen,data1,data_Arrivo,Protocollo_Procura,del,codice,cartellina,note,oggetto1,destinatario1,quartiere,via,cognome,codice_edificio)" +
               " Values('" + @arch.arch_Num_Prot + "','" + @arch.arch_ProtGen + "','" + @arch.arch_dataInserimento + "','" + @arch.arch_dataArrivo + "','" + @arch.arch_Protocollo_Procura + "','" +
               @arch.arch_dataProtProcura + "','" + @arch.arch_codice + "','" + @arch.arch_cartellina + "','" + @arch.arch_note.Replace("'", "''") + "','" + @arch.arch_oggetto.Replace("'", "''") + "','" +
               @arch.arch_destinatario.Replace("'", "''") + "','" + @arch.arch_quartiere.Replace("'", "''") + "','" + @arch.arch_indirizzo.Replace("'", "''") + "','" + @arch.arch_cognome.Replace("'", "''") + "','" +
               @arch.arch_edificio.Replace("'", "''") + "')";

            sql_cartellina = "update ProgCartelline set progressivo = " + @arch.arch_cartellina + " where quartiere like '%" + @arch.arch_quartiere.Replace("'", "''") + "%'";
            sql_Verificacartellina = "select progressivo from  ProgCartelline where progressivo = " + @arch.arch_cartellina + " and quartiere like '%" + @arch.arch_quartiere.Replace("'", "''") + "%'";
            // sql_Verificacartellina = "select Max(progressivo) as n from  ProgCartelline where quartiere like '%" + @arch.arch_quartiere.Replace("'", "''") + "%'";
            int res = 0;
            using (SqlConnection conn = new SqlConnection(ConnStringTp))
            {
                conn.Open();

                // 2. Inizio la transazione per le modifiche e insert
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.Transaction = tran;

                        try
                        {
                            command.CommandText = sql_Verificacartellina;
                            object result = command.ExecuteScalar();
                            if (Convert.ToInt32(result) == Convert.ToInt32(arch.arch_cartellina))
                            {
                                res = -1; // La cartellina esiste già, non procedere con l'update e l'insert
                                msg = "DUPLICATO";
                            }
                            else
                            {
                                res = 1;
                            }
                            // res = command.ExecuteNonQuery();
                            if (res > 0)
                            {
                                command.CommandText = sql_pratica;


                                res = command.ExecuteNonQuery();
                                if (res > 0)
                                {
                                    command.CommandText = sql_cartellina;
                                    res = command.ExecuteNonQuery();
                                    if (res > 0)
                                    {

                                        tran.Commit();
                                        resp = true;
                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        resp = false;
                                    }
                                }
                                else
                                {
                                    // Se la cartellina non esiste, esegui il rollback e imposta la risposta a false
                                    tran.Rollback();
                                    resp = false;
                                }



                            }

                            else
                            {
                                // Se l'UPDATE fallisce, esegui il rollback e imposta la risposta a false
                                tran.Rollback();
                                resp = false;
                            }
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            //ScriviLog(ex.Message); // Gestione log
                            resp = false;
                        }
                    }
                }
            }
            //        string sql =
            //// 1. CONTROLLO PRELIMINARE: IL PROGRESSIVO ESISTE GIÀ?
            //"IF EXISTS (SELECT 1 FROM ProgCartelline WHERE progressivo = " + @arch.arch_cartellina + " AND quartiere LIKE '%" + @arch.arch_quartiere.Replace("'", "''") + "%') " +
            //"BEGIN " +
            //    "SELECT 'DUPLICATO'; " +
            //"END " +
            //"ELSE " +
            //"BEGIN " +
            //    // 2. SE NON ESISTE, TENTA L'AGGIORNAMENTO
            //    "UPDATE ProgCartelline SET progressivo = " + @arch.arch_cartellina +
            //    " WHERE quartiere LIKE '%" + @arch.arch_quartiere.Replace("'", "''") + "%'; " +

            //    // 3. CONTROLLA SE L'AGGIORNAMENTO HA TROVATO IL QUARTIERE
            //    "IF @@ROWCOUNT > 0 " +
            //    "BEGIN " +
            //        // HA TROVATO LA RIGA: Esegue l'inserimento
            //        "INSERT INTO Archiviotp (Num_Prot, ProtGen, data1, data_Arrivo, Protocollo_Procura, del, codice, cartellina, note, oggetto1, destinatario1, quartiere, via, cognome, codice_edificio) " +
            //        "VALUES ('" + @arch.arch_Num_Prot + "','" +
            //                      @arch.arch_ProtGen + "','" +
            //                      @arch.arch_dataInserimento + "','" + // FIX DATA
            //                      @arch.arch_dataArrivo + "','" +      // FIX DATA
            //                      @arch.arch_Protocollo_Procura + "','" +
            //                      @arch.arch_dataProtProcura + "','" + // FIX DATA
            //                      @arch.arch_codice + "','" +
            //                      @arch.arch_cartellina + "','" +
            //                      @arch.arch_note.Replace("'", "''") + "','" +
            //                      @arch.arch_oggetto.Replace("'", "''") + "','" +
            //                      @arch.arch_destinatario.Replace("'", "''") + "','" +
            //                      @arch.arch_quartiere.Replace("'", "''") + "','" +
            //                      @arch.arch_indirizzo.Replace("'", "''") + "','" +
            //                      @arch.arch_cognome.Replace("'", "''") + "','" +
            //                      @arch.arch_edificio.Replace("'", "''") + "'); " +

            //        // Restituisce Successo
            //        "SELECT 'OK'; " +
            //    "END " +
            //    "ELSE " +
            //    "BEGIN " +
            //        // L'UPDATE HA MODIFICATO 0 RIGHE (Quartiere errato o inesistente)
            //        "SELECT 'ERRORE_QUARTIERE'; " +
            //    "END " +
            //"END;";

            // 1. Il blocco 'using' gestisce già la chiusura e il dispose della connessione.
            //using (SqlConnection conn = new SqlConnection(ConnStringTp))
            //{
            //    using (SqlCommand cmd = new SqlCommand(sql, conn))
            //    {
            //        try
            //        {
            //            conn.Open();

            //            // USIAMO ExecuteScalar INVECE DI ExecuteNonQuery
            //            // Questo ci permette di leggere la stringa 'OK' o 'DUPLICATO' restituita dalla SQL
            //            object result = cmd.ExecuteScalar();

            //            if (result != null)
            //            {
            //                string rispostaSql = result.ToString();

            //                if (rispostaSql == "OK")
            //                {
            //                    // Caso successo: Update e Insert eseguiti
            //                    resp = true;
            //                    msg = "Inserimento e aggiornamento completati con successo.";
            //                }
            //                else if (rispostaSql == "DUPLICATO")
            //                {
            //                    // Caso duplicato: Non ha fatto nulla
            //                    resp = false;
            //                    msg = "DUPLICATO";
            //                }
            //                else
            //                {
            //                    // Caso imprevisto
            //                    resp = false;
            //                    msg = "Risposta imprevista dal database.";
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            resp = false;
            //            msg = "Errore di sistema: " + ex.Message;

            //            // Gestione Log Errori
            //            try
            //            {
            //                if (!File.Exists(LogFile))
            //                {
            //                    using (StreamWriter sw = File.CreateText(LogFile)) { }
            //                }

            //                using (StreamWriter sw = File.AppendText(LogFile))
            //                {
            //                    sw.WriteLine(DateTime.Now + " - InserimentoArchivioUotp: " + ex.Message);
            //                }
            //            }
            //            catch { /* Ignora errori di log per non bloccare tutto */ }
            //        }
            //    }
            //}
            return resp;
        }
        /// <summary>
        /// Salva il nuovo fascicolo protocollo
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Boolean InsCarico(Principale p, Int32 id, Statistiche stat, Boolean exist, out Int32 idN)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string sql_Statistiche = String.Empty;
            string testoSql = string.Empty;
            int res1 = 0;
            object a = null;

            try
            {
                if (String.IsNullOrEmpty(p.matricola))
                {
                    idN = -2;
                    return false;
                }
                sql_pratica =
    // 1. CONTROLLO ESISTENZA
    // Verifica se esiste già una riga con lo stesso Protocollo e lo stesso Anno
    "IF NOT EXISTS (SELECT 1 FROM principale WHERE nr_protocollo = '" + @p.nrProtocollo + "' AND Anno = '" + @p.anno + "') " +
    "BEGIN " +
        // 2. SE NON ESISTE -> ESEGUI INSERT
        "INSERT INTO principale (" +
            "nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen, " +
            "Nominativo, Indirizzo, Evasa, EvasaData, Inviata, DataInvio, Scaturito, Accertatori, DataCarico, nr_Pratica, " +
            "Quartiere, Note, Anno, Giorno, Rif_Prot_Gen, matricola, DataInserimento, macro_area, UlterioreTipoAtto, bu, codiceEdificio," +
            "NumProtRicStessoCarico" +

        ") " +
        "VALUES ('" +
            @p.nrProtocollo + "','" +
            @p.sigla.Replace("'", "''") + "','" +
            @p.dataArrivo + "','" +
            @p.provenienza.Replace("'", "''") + "','" +
            @p.tipologia_atto.Replace("'", "''") + "','" +
            @p.giudice.Replace("'", "''") + "','" +
            @p.tipoProvvedimentoAG.Replace("'", "''") + "','" +
            @p.procedimentoPen + "','" +
            @p.nominativo.Replace("'", "''") + "','" +
            @p.indirizzo.Replace("'", "''") + "','" +
            @p.evasa + "','" +
            @p.evasaData + "','" +
            @p.inviata.Replace("'", "''") + "','" +
            @p.dataInvio + "','" +
            @p.scaturito.Replace("'", "''") + "','" +
            @p.accertatori.Replace("'", "''") + "','" +
            @p.dataCarico + "','" +
            @p.nr_Pratica + "','" +
            @p.quartiere.Replace("'", "''") + "','" +
            @p.note.Replace("'", "''") + "','" +
            @p.anno + "','" +
            @p.giorno + "','" +
            @p.rif_Prot_Gen + "','" +
            @p.matricola + "','" +
            @p.data_ins_pratica + "','" +
            @p.macro_area.Replace("'", "''") + "','" +
            @p.ulterioreTipoAtto.Replace("'", "''") + "','" +
            @p.bu.Replace("'", "''") + "','" +
            @p.codiceEdificio.Replace("'", "''") + "'," +
            @p.NumProtRicStessoCarico +
        "); " +

        // RESTITUISCE IL NUOVO ID GENERATO
        "SELECT SCOPE_IDENTITY(); " +
    "END " +
    "ELSE " +
    "BEGIN " +
        // 3. SE ESISTE GIÀ -> RESTITUISCE -1
        "SELECT -1; " +
    "END";
                //sql_pratica = "insert into principale (nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen," +
                //    "Nominativo,Indirizzo,Evasa,EvasaData,Inviata,DataInvio,Scaturito,Accertatori,DataCarico,nr_Pratica,Quartiere,Note,Anno,Giorno,Rif_Prot_Gen,matricola,DataInserimento, " +
                //    "macro_area,UlterioreTipoAtto,bu,codiceEdificio)" +
                //   " Values('" + @p.nrProtocollo + "','" + @p.sigla.Replace("'", "''") + "','" + @p.dataArrivo + "','" + @p.provenienza.Replace("'", "''") + "','" + @p.tipologia_atto.Replace("'", "''") +
                //   "','" + @p.giudice.Replace("'", "''") + "','" + @p.tipoProvvedimentoAG.Replace("'", "''") + "','" + @p.procedimentoPen + "','" +
                //   @p.nominativo.Replace("'", "''") + "','" + @p.indirizzo.Replace("'", "''") + "','" + @p.evasa + "','" + @p.evasaData + "','" + @p.inviata.Replace("'", "''") + "','" +
                //   @p.dataInvio + "','" + @p.scaturito.Replace("'", "''") + "','" + @p.accertatori.Replace("'", "''") + "','" + @p.dataCarico + "','" + @p.nr_Pratica + "','" +
                //   @p.quartiere.Replace("'", "''") + "','" + @p.note.Replace("'", "''") + "','" + @p.anno + "','" + @p.giorno + "','" + @p.rif_Prot_Gen + "','" + @p.matricola + "','" + @p.data_ins_pratica + "','" +
                //   @p.macro_area.Replace("'", "''") + "','" + @p.ulterioreTipoAtto.Replace("'", "''") + "','" + @p.bu.Replace("'", "''") + "','" + @p.codiceEdificio.Replace("'", "''") +
                //   "'); SELECT SCOPE_IDENTITY();";
                if (exist)
                {
                    sql_Statistiche = "update statistiche set deleghe_ricevute = +" + stat.deleghe_ricevute + ", esposti_ricevuti = + " + stat.esposti_ricevuti +
                           " where mese = '" + @stat.mese + "' and anno = " + stat.anno;

                }
                else
                {
                    sql_Statistiche = "insert into statistiche (mese,anno,relazioni,ponteggi,dpi,esposti_ricevuti,esposti_evasi,ripristino_tot_par,controlli_scia,contr_cant_daily,cnr,annotazioni,notifiche" +
                        ",sequestri,riapp_sigilli,deleghe_ricevute,deleghe_esitate,cnr_annotazioni,interrogazioni,denunce_uff,convalide,demolizioni" +
                        ",violazione_sigilli,dissequestri,dissequestri_temp,rimozione_sigilli,controlli_42_04,contr_cant_suolo_pubb,contr_lavori_edili,contr_cant,contr_nato_da_esposti,viol_amm_reg_com,censimentoAllPubb" +
                        ",Abitativo,nonAbitativo,Sgomberi_abus,Sgomberi_immobili,NotificaTp) " +
                    " Values('" + stat.mese.ToUpper() + "'," + stat.anno + "," + stat.relazioni + "," + stat.ponteggi + "," + stat.dpi + "," +
                      stat.esposti_ricevuti + "," + stat.esposti_evasi + "," + stat.ripristino_tot_par + "," + stat.controlli_scia + "," + stat.contr_cant_daily + "," + stat.cnr + "," +
                      stat.annotazioni + "," + stat.notifiche + "," + stat.sequestri + "," + stat.riapp_sigilli + "," + stat.deleghe_ricevute + "," +
                      stat.deleghe_esitate + "," + stat.cnr_annotazioni + "," + stat.interrogazioni + "," + stat.denunce_uff + "," + stat.convalide + "," +
                      stat.demolizioni + "," + stat.violazione_sigilli + "," + stat.dissequestri + "," + stat.dissequestri_temp + "," + stat.riapp_sigilli + "," +
                      stat.controlli_42_04 + "," + stat.contr_cant_suolo_pubb + "," + stat.contr_lavori_edili + "," + stat.contr_cant + "," + stat.contr_nato_da_esposti + "," + stat.viol_amm_reg_com + "," +
                      stat.censimentoAllPubb + "," + stat.Abitativo + "," + stat.NonAbitativo + "," + stat.Sgomberi_abus + "," + stat.Sgomberi_immobili + "," + stat.NotificaTp + ")";

                }
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    SqlTransaction tran;
                    tran = conn.BeginTransaction("trans");
                    command.Transaction = tran;

                    idN = -1;


                    try
                    {
                        //string sql = "select * from principale where Nr_Protocollo= '" + p.nrProtocollo + "' and anno = '" + p.anno + "'";

                        //SqlDataAdapter da;
                        //DataSet ds;

                        //da = new SqlDataAdapter(sql, conn);
                        //da.SelectCommand.Transaction = tran;
                        //ds = new DataSet();
                        //da.Fill(ds);
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{

                        //    return false;
                        //}
                        //else
                        //{


                        command.CommandText = sql_pratica;
                        testoSql = "Principale";
                        //int res = command.ExecuteNonQuery();
                        //a = command.ExecuteScalar();
                        object result = command.ExecuteScalar();

                        decimal nuovoId = Convert.ToDecimal(result);
                        if (nuovoId == -1)
                        {
                            // IL RECORD ESISTEVA GIÀ
                            tran.Rollback();
                            tran.Dispose();
                            return false;

                        }
                        else
                        {
                            command.CommandText = sql_Statistiche;

                            res1 = command.ExecuteNonQuery();
                            if (res1 > 0)
                            {
                                tran.Commit();
                                tran.Dispose();
                                idN = Convert.ToInt32(result);
                                resp = true;
                            }
                            else
                            {
                                tran.Rollback();
                                tran.Dispose();
                                return false;
                            }


                        }
                        //   idN = Convert.ToInt32(a);
                        //if (res1 > 0)
                        //{
                        //    tran.Commit();
                        //    tran.Dispose();
                        //    idN = Convert.ToInt32(result);
                        //    resp = true;
                        //}

                        //else
                        //{
                        //    tran.Rollback();
                        //    resp = false;
                        //}
                        // }
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("protocollo " + p.nrProtocollo + ", matricola:" + p.matricola + ", data ins:" + p.data_ins_pratica + ", " + ex.Message + @" - Errore in inserimento dati ");
                            sw.Close();
                        }
                        tran.Rollback();
                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();

                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            idN = Convert.ToInt32(a);
            return resp;

        }
        public Boolean SalvaTurnoMensileN(List<DipendenteTurno> lista, int anno, string mese, DataTable dipendente)
        {
            bool resp = true;
            DateTime dt = DateTime.ParseExact(mese, "MMMM", CultureInfo.CreateSpecificCulture("it-IT"));
            int meseN = dt.Month;
            int giorniMese = DateTime.DaysInMonth(anno, meseN);
            //string query = @" MERGE TurniMensile AS Target USING (SELECT @matricola AS matricola, @anno AS anno, @mese AS mese, @giorno AS giorno) AS Source ON (Target.matricola = Source.matricola AND Target.anno = Source.anno AND Target.mese = Source.mese AND Target.giorno = Source.giorno) WHEN MATCHED THEN UPDATE SET CodiceTurno = @CodiceTurno, nominativo = @nominativo WHEN NOT MATCHED BY TARGET THEN INSERT (matricola, nominativo, anno, mese, giorno, CodiceTurno)  VALUES (@matricola, @nominativo, @anno, @mese, @giorno, @CodiceTurno);";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                // Usiamo una transazione per garantire integrità (o tutto o niente)
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // A. PULIZIA DATI ESISTENTI
                    // Prima di inserire, cancelliamo eventuali turni già salvati per questo mese/anno
                    string sqlDelete = "DELETE FROM TurniMensile WHERE mese = @mese AND anno = @anno";
                    using (SqlCommand cmdDel = new SqlCommand(sqlDelete, conn, trans))
                    {
                        cmdDel.Parameters.AddWithValue("@mese", mese);
                        cmdDel.Parameters.AddWithValue("@anno", anno);
                        cmdDel.ExecuteNonQuery();
                    }

                    // B. INSERIMENTO NUOVI DATI
                    string sqlInsert = @"INSERT INTO TurniMensile (mese, anno, matricola, nominativo, giorno, CodiceTurno, DataUltimaModifica,gruppo) VALUES (@mese, @anno, @matricola, @nominativo, @giorno, @codice, @dataMod,@gruppo)";

                    using (SqlCommand cmdIns = new SqlCommand(sqlInsert, conn, trans))
                    {
                        // Parametri riutilizzabili
                        cmdIns.Parameters.Add("@mese", SqlDbType.NChar, 10).Value = mese;
                        cmdIns.Parameters.Add("@anno", SqlDbType.Int).Value = anno;
                        cmdIns.Parameters.Add("@matricola", SqlDbType.NChar, 10); // Adatta lunghezza al DB
                        cmdIns.Parameters.Add("@nominativo", SqlDbType.VarChar, 50);
                        cmdIns.Parameters.Add("@giorno", SqlDbType.Int);
                        cmdIns.Parameters.Add("@codice", SqlDbType.VarChar, 5); // Per "1", "2", "Q", "RF"
                        cmdIns.Parameters.Add("@dataMod", SqlDbType.DateTime).Value = DateTime.Now;
                        cmdIns.Parameters.Add("@gruppo", SqlDbType.VarChar, 2);
                        foreach (var dip in lista)
                        {
                            // Imposta parametri dipendente (fissi per tutti i giorni)
                            cmdIns.Parameters["@matricola"].Value = dip.Matricola ?? ""; // Gestione null
                            cmdIns.Parameters["@nominativo"].Value = dip.Nominativo;
                            cmdIns.Parameters["@gruppo"].Value = dip.Gruppo;
                            // Ciclo sui giorni (1..31)
                            for (int i = 1; i <= giorniMese; i++)
                            {
                                string turno = dip.TurniMensili[i];

                                // Salviamo solo se c'è un turno (se è null, non salviamo niente o salviamo stringa vuota?)
                                // Di solito si salva solo se c'è attività o RF.
                                if (!string.IsNullOrEmpty(turno))
                                {
                                    cmdIns.Parameters["@giorno"].Value = i;
                                    cmdIns.Parameters["@codice"].Value = turno;

                                    cmdIns.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    // C. CONFERMA TRANSAZIONE
                    trans.Commit();
                    return resp;
                }
                catch
                {
                    // In caso di errore, annulla tutto (anche la cancellazione)
                    trans.Rollback();
                    throw; // Rilancia l'errore per mostrarlo nella label

                }
            }
        }
        public Boolean SalvaTurnoMensile(DataTable dip)
        {
            bool resp = true;

            // Se la riga esiste già, aggiorna il codice del turno e la data di modifica
            // Se la riga non esiste, inseriscine una nuova
            string query = @" MERGE TurniMensile AS Target USING (SELECT @matricola AS matricola, @anno AS anno, @mese AS mese, @giorno AS giorno) AS Source ON (Target.matricola = Source.matricola AND Target.anno = Source.anno AND Target.mese = Source.mese AND Target.giorno = Source.giorno) WHEN MATCHED THEN UPDATE SET CodiceTurno = @CodiceTurno, nominativo = @nominativo WHEN NOT MATCHED BY TARGET THEN INSERT (matricola, nominativo, anno, mese, giorno, CodiceTurno)  VALUES (@matricola, @nominativo, @anno, @mese, @giorno, @CodiceTurno);";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {


                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    // 1. Aggiungi i parametri al comando UNA SOLA VOLTA, prima del ciclo.
                    command.Parameters.Add("@matricola", SqlDbType.NVarChar, 10);
                    command.Parameters.Add("@nominativo", SqlDbType.NVarChar, 100);
                    command.Parameters.Add("@anno", SqlDbType.Int);
                    command.Parameters.Add("@mese", SqlDbType.NVarChar, 20);
                    command.Parameters.Add("@giorno", SqlDbType.VarChar, 2);
                    command.Parameters.Add("@CodiceTurno", SqlDbType.NVarChar, 10);
                    conn.Open();
                    foreach (DataRow row in dip.Rows)
                    {
                        // Assegna i valori dei parametri per la riga corrente
                        command.Parameters["@matricola"].Value = row["matricola"];
                        command.Parameters["@nominativo"].Value = row["nominativo"];
                        command.Parameters["@anno"].Value = row["anno"];
                        command.Parameters["@mese"].Value = row["mese"];
                        command.Parameters["@giorno"].Value = row["giorno"];
                        command.Parameters["@CodiceTurno"].Value = row["CodiceTurno"];

                        // Esegui il comando per questa riga
                        command.ExecuteNonQuery();
                    }

                    conn.Close();
                    conn.Dispose();
                    return resp;
                }

            }
        }

        //UPDATE
        public Boolean UpdTurnoMensile(int idDipendente, DataTable dip)
        {
            bool resp = true;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string matricola = string.Empty;


                conn.Open();

                // Inizia una transazione per assicurare che entrambi i passi abbiano successo
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // =================================================================
                    // 1. SELECT: Recupera la Matricola dal Dipendente
                    // =================================================================
                    string querySelect = "SELECT matricola FROM SchedaDipendente WHERE id_dip = " + idDipendente;
                    using (SqlCommand cmdSelect = new SqlCommand(querySelect, conn, transaction))
                    {
                        cmdSelect.Parameters.AddWithValue("@IDDipendente", idDipendente);

                        object result = cmdSelect.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            matricola = result.ToString().Trim();
                        }
                        else
                        {
                            // Nessuna matricola trovata
                            transaction.Rollback();
                            return false;
                        }
                    }

                    // =================================================================
                    // 2. UPDATE: Aggiorna i Turni usando la Matricola recuperata
                    // =================================================================
                    for (int i = 1; i < dip.Rows.Count; i++)
                    {


                        string queryUpdate = "UPDATE TurniMensile SET CodiceTurno = '" + dip.Rows[0].ItemArray[3].ToString() + "' WHERE matricola ='" + matricola + "' and anno = " + dip.Rows[0].ItemArray[0] + " and mese ='" + dip.Rows[0].ItemArray[1].ToString() + "' " +
                            " and giorno ='" + dip.Rows[0].ItemArray[2].ToString() + "'";
                        using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn, transaction))
                        {
                            int rowsAffected = cmdUpdate.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit(); // Successo: conferma entrambe le operazioni
                                conn.Close();
                                conn.Dispose();
                                resp = true;

                            }
                            else
                            {
                                // Nessun turno trovato con quella matricola
                                transaction.Rollback();
                                conn.Close();
                                conn.Dispose();

                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Errore: annulla tutte le operazioni
                    transaction.Rollback();
                    conn.Close();
                    conn.Dispose();
                    // Logga l'errore (ex.Message)

                }
            }
            return resp;
        }
        public Boolean UpdDecretazioneChiusura(Decretazione p)
        {
            bool resp = true;
            string sql_decretazione = String.Empty;
            string sql_updDecretazione = String.Empty;
            string sql_updPrincipale = String.Empty;
            string testoSql = string.Empty;
            int res1 = 0;
            try
            {
                sql_decretazione = "insert into decretazione (decr_idPratica, decr_pratica,decr_decretante, decr_decretato,decr_data,decr_nota, decr_dataChiusura, decr_chiuso)" +
                                    " Values('" + @p.idPratica + "','" + @p.Npratica + "','" + @p.decretante.Replace("'", "''") + "','" + @p.decretato.Replace("'", "''") +
                                    "','" + @p.data + "','" + @p.nota.Replace("'", "''") + "','" + @p.dataChiusura + "','" + @p.chiuso + "')";



                sql_updDecretazione = "update decretazione set decr_chiuso = '" + @p.chiuso + "'" +
                    " where  decr_pratica = '" + p.Npratica + "' and decr_idPratica = " + p.idPratica;

                sql_updPrincipale = "update principale set Evasa = 'True' where  id = " + p.idPratica + " and Nr_Protocollo = " + p.Npratica;

                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    SqlTransaction tran;
                    tran = conn.BeginTransaction("trans");
                    command.Transaction = tran;
                    try
                    {
                        command.CommandText = sql_decretazione;
                        int res = command.ExecuteNonQuery();
                        if (res > 0)
                        {
                            command.CommandText = sql_updDecretazione;

                            res1 = command.ExecuteNonQuery();
                        }
                        if (res1 > 0)
                        {
                            command.CommandText = sql_updPrincipale;

                            command.ExecuteNonQuery();

                            tran.Commit();
                            tran.Dispose();
                            resp = true;

                        }
                        else
                        {
                            tran.Rollback();
                            resp = false;
                        }

                        //command.CommandText = sql_decretazione;

                        //int res = command.ExecuteNonQuery();
                        //if (res > 0)
                        //{
                        //    command.CommandText = sql_updPrincipale;

                        //    command.ExecuteNonQuery();

                        //    tran.Commit();

                        //    resp = true;

                        //}
                        //else
                        //{
                        //    tran.Rollback();
                        //    resp = false;
                        //}

                        // command.CommandText = sql_decretazione;
                        testoSql = "decretazione";

                    }

                    catch (Exception ex)
                    {

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("pratica:" + p.Npratica + " -" + ex.Message + @" - Errore in update dati decretazione ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }
        public Boolean DuplicaCarico(string carico, string sigla, int id)
        {
            bool resp = true;
            string sql_principale = String.Empty;
            string testoSql = string.Empty;
            //arch.arch_dataInserimento = System.Convert.ToString(DateTime.Now.ToString("dd/mm/yyyy"));
            sql_principale = "INSERT INTO principale (Nr_Protocollo, Sigla, DataArrivo, Provenienza, Tipologia_atto, Giudice, TipoProvvedimentoAG, ProcedimentoPen, Nominativo, Indirizzo, via, Evasa, EvasaData, Inviata, " +
       "DataInvio, Scaturito, Accertatori, DataCarico, nr_Pratica, Quartiere, Note, Anno, Giorno, Rif_Prot_Gen, Matricola, DataInserimento, Macro_area, UlterioreTipoAtto, BU, CodiceEdificio) " +
      "SELECT[Nr_Protocollo] ,[Sigla],[DataArrivo],[Provenienza],[Tipologia_atto],[Giudice],[TipoProvvedimentoAG],[ProcedimentoPen],[Nominativo],[Indirizzo],[via],[Evasa],[EvasaData],[Inviata],[DataInvio],[Scaturito]" +
      ",[Accertatori],[DataCarico],[nr_Pratica],[Quartiere],[Note],[Anno],[Giorno],[Rif_Prot_Gen],[Matricola],[DataInserimento],[Macro_area],[UlterioreTipoAtto],[BU],[CodiceEdificio] " +
      "FROM principale where Id = " + id;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql_principale;
                    testoSql = "principale";
                    int res = command.ExecuteNonQuery();
                }

                catch (Exception ex)
                {

                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("carico:" + carico + ", sigla= " + sigla + ": " + ex.Message + @" - Errore in DuplicaCarico ");
                        sw.Close();
                    }

                    resp = false;


                }
                conn.Close();
                conn.Dispose();
                return resp;
            }

        }
        public Boolean UpdPraticaArchivioUotp(ArchivioUotp arch)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;
            //arch.arch_dataInserimento = System.Convert.ToString(DateTime.Now.ToString("dd/mm/yyyy"));
            sql_pratica = "update Archiviotp set destinatario1 = '" + @arch.arch_destinatario.Replace("'", "''") + "', cognome = '" + @arch.arch_cognome.Replace("'", "''") + "', codice ='" + @arch.arch_codice.Replace("'", "''") +
                                 "', via = '" + @arch.arch_indirizzo.Replace("'", "''") + "', codice_edificio = '" + @arch.arch_edificio.Replace("'", "''") + "', note = '" + @arch.arch_note.Replace("'", "''") +
                                 "', data1 = '" + @arch.arch_dataInserimento.Replace("'", "''") +
                                 "', oggetto1 = '" + @arch.arch_oggetto.Replace("'", "''") +
                                 "', oggetto2 = '" + @arch.arch_oggetto2.Replace("'", "''") + "'" +
                                 " where cartellina = '" + @arch.arch_cartellina + "' and quartiere = '" + @arch.arch_quartiere.Replace("'", "''") + "'";
            using (SqlConnection conn = new SqlConnection(ConnStringTp))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    command.CommandText = sql_pratica;
                    testoSql = "archiviotp";
                    int res = command.ExecuteNonQuery();
                }

                catch (Exception ex)
                {

                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("Cartellina:" + @arch.arch_cartellina + ", quartiere= " + @arch.arch_quartiere + ": " + ex.Message + @" - Errore in update archiviotp ");
                        sw.Close();
                    }

                    resp = false;


                }
                conn.Close();
                conn.Dispose();
                return resp;
            }

        }

        /// <summary>
        /// modifica il flag verificato a true nella tabella gestione auto
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Boolean UpdGestioneAutoById(GestAuto p)
        {
            bool resp = true;

            string sql = String.Empty;
            string testoSql = string.Empty;
            int res = 0;
            try
            {

                sql = "update gestioneauto set verificato= 'true', data_verifica= '" + @p.dataVerifica + "',matricola= '" + p.matricola.Trim() + "'" +
                    " where  id = " + @p.id;

                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql;
                        testoSql = "Gestioneauto";
                        res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("targa:" + p.targa + ",data ins:" + p.dataVerifica + ", id riga= " + p.id + ": " + ex.Message + @" - Errore in update gestione auto ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }
        public Boolean UpdDecretazione(Decretazione p)
        {
            bool resp = true;
            string sql_decretazione = String.Empty;
            string sql_updDecretazione = String.Empty;
            string sql_updPrincipale = String.Empty;
            string testoSql = string.Empty;
            int res1 = 0;
            try
            {

                sql_updDecretazione = "update decretazione set decr_data = '" + @p.data + "', decr_nota = '" + p.nota.Replace("'", "''") + "', decr_decretato = '" + p.decretato.Replace("'", "''") + "'" +
                    " where  decr_id = " + p.id;

                //  sql_updPrincipale = "update principale set Evasa = 'True' , EvasaData = '" + @p.dataChiusura + "' where  id = " + p.idPratica + " and Nr_Protocollo = " + p.Npratica;

                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    SqlTransaction tran;
                    tran = conn.BeginTransaction("trans");
                    command.Transaction = tran;
                    try
                    {

                        command.CommandText = sql_updDecretazione;

                        res1 = command.ExecuteNonQuery();

                        if (res1 > 0)
                        {


                            tran.Commit();
                            tran.Dispose();
                            resp = true;

                        }
                        else
                        {
                            tran.Rollback();
                            resp = false;
                        }

                        //command.CommandText = sql_decretazione;

                        //int res = command.ExecuteNonQuery();
                        //if (res > 0)
                        //{
                        //    command.CommandText = sql_updPrincipale;

                        //    command.ExecuteNonQuery();

                        //    tran.Commit();

                        //    resp = true;

                        //}
                        //else
                        //{
                        //    tran.Rollback();
                        //    resp = false;
                        //}

                        // command.CommandText = sql_decretazione;
                        testoSql = "decretazione";

                    }

                    catch (Exception ex)
                    {

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("pratica:" + p.Npratica + " -" + ex.Message + @" - Errore in update dati decretazione ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }


        public Boolean UpdApriDecretazione(string carico, string anno)
        {
            bool resp = true;

            string sql_updDecretazione = String.Empty;
            string sql_updPrincipale = String.Empty;
            string sql_SelPrincipale = String.Empty;
            string testoSql = string.Empty;
            int? idPrincipaleRecuperato = null;
            try
            {
                sql_SelPrincipale = "select id from principale where Nr_Protocollo = '" + carico + "' and anno = '" + anno + "'";

                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();

                    // 1. Eseguo la SELECT per prelevare l'ID del record principale
                    using (SqlCommand cmdSel = new SqlCommand(sql_SelPrincipale, conn))
                    {
                        cmdSel.Parameters.AddWithValue("@protocollo", carico); // Il valore 'carico' ora lo usiamo come protocollo
                        cmdSel.Parameters.AddWithValue("@anno", anno);

                        var resId = cmdSel.ExecuteScalar();
                        if (resId != null)
                        {
                            idPrincipaleRecuperato = Convert.ToInt32(resId);
                        }
                    }

                    // Se non trovo l'ID in principale, esco subito
                    if (!idPrincipaleRecuperato.HasValue)
                    {
                        // Logica se il record non esiste (es: log o return false)
                        return false;
                    }

                    // 2. Inizio la transazione per le modifiche
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            command.Transaction = tran;

                            try
                            {
                                sql_updDecretazione = "update decretazione set decr_dataChiusura = NULL, decr_chiuso = 'False'" +
                              //"OUTPUT inserted.decr_idPratica " +
                              " where decr_pratica ='" + carico + "' and decr_idPratica =" + idPrincipaleRecuperato;
                                // Update Decretazione
                                command.CommandText = sql_updDecretazione;
                                command.Parameters.Clear();

                                command.ExecuteNonQuery();

                                // Update Principale
                                sql_updPrincipale = "update principale set Evasa = 'False' where id = " + idPrincipaleRecuperato;
                                command.CommandText = sql_updPrincipale;
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@idPrincipale", idPrincipaleRecuperato.Value);

                                //command.ExecuteNonQuery();
                                var resDecretazione = command.ExecuteNonQuery();
                                if (resDecretazione > 0)
                                {
                                    tran.Commit();
                                    resp = true;

                                }
                                else
                                {
                                    // Se l'UPDATE di decretazione non trova righe 
                                    tran.Rollback();
                                    resp = false;
                                }
                            }
                            catch (Exception)
                            {
                                tran.Rollback();
                                //ScriviLog(ex.Message); // Gestione log
                                resp = false;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ScriviLog(ex.Message);
                resp = false;
            }

            return resp;

        }
        public Boolean UpdateRegistroById(int id, UrpRegistro reg)
        {
            bool resp = true;
            string sql = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql = "update RegistroUrp set oggetto = '" + reg.oggetto.Replace("'", "''") + "', dataPresentRichiesta= '" + reg.dataPresentRichiesta + "', nrPgTrasmissioneRichiesto= '" + reg.nrPgTrasmissioneRichiesto.Replace("'", "''") +
                   "', uffDetentore= '" + reg.uffDetentore.Replace("'", "''") + "', controInteressati= '" + reg.controInteressati + "', esito = '" + reg.esito.Replace("'", "''") + "', motivazione= '" + reg.motivazione.Replace("'", "''") +
                   "', nrPgTrasmissioneRiscontro = '" + reg.nrPgTrasmissioneRiscontro.Replace("'", "''") + "', dataConclProcedimento = '" + reg.dataConclProcedimento + "'" +
                    " where id_registro = '" + id + "'";





                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql;
                        testoSql = "RegistroUrp";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("numero registro:" + id + ",data :" + reg.dataPresentRichiesta + ", " + ex.Message + @" - Errore in update  RegistroUrp");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }
        public Boolean UpdateInterrogatorioId(int id, Interrogatorio interr)
        {
            bool resp = true;
            string sql = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql = "update interrogatori set Npratica = '" + interr.Npratica.Replace("'", "''") + "', ProcPenale= '" + interr.ProcPenale.Replace("'", "''") + "', DataInterrogatorio= '" + interr.DataInterrogatorio +
                   "', Nominativo1= '" + interr.Nominativo1.Replace("'", "''") + "', Nominativo2= '" + interr.Nominativo2.Replace("'", "''") + "', Nominativo3 = '" + interr.Nominativo3.Replace("'", "''") + "', Nominativo4= '" +
                   interr.Nominativo4.Replace("'", "''") + "' where id = '" + id + "'";



                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql;
                        testoSql = "interrogatori";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("numero id:" + id + ",Pratica :" + interr.Npratica.Trim() + ", " + ex.Message + @" - Errore in update  interrogatori");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }
        public Boolean UpdScheda(RappUote rapp)
        {
            bool resp = true;
            string sql_scheda = String.Empty;
            string testoSql = string.Empty;

            try
            {

                sql_scheda = "update RappUote set rapp_data = '" + rapp.data + "', rapp_nominativo = '" + @rapp.nominativo.Replace("'", "''") + "', rapp_indirizzo ='" + rapp.indirizzo.Replace("'", "''") + "', rapp_pattuglia='" + @rapp.pattuglia.Replace("'", "''") +
                     "', rapp_delegaAG = '" + @rapp.delegaAG + "', rapp_resa = '" + rapp.resa + "', rapp_segnalazione = '" + @rapp.segnalazione +
                     "', rapp_esposto = '" + @rapp.esposti + "', rapp_numEsposti = '" + @rapp.num_esposti + "', rapp_notifica = '" + @rapp.notifica +
                     "', rapp_iniziativa ='" + @rapp.iniziativa + "', rapp_comandante = '" + @rapp.cdr + "', rapp_coordinatore = '" + @rapp.coordinatore +
                     "', rapp_relazione ='" + @rapp.relazione + "', rapp_cnr = '" + @rapp.cnr + "', rapp_annotazionePG = '" + @rapp.annotazionePG +
                     "', rapp_verbale_seq ='" + @rapp.verbaleSeq + "', rapp_esito_delega = '" + @rapp.esitoDelega + "', rapp_contestaz_amm = '" + @rapp.contestazioneAmm +
                     "', rapp_convalida ='" + @rapp.convalida + "', rapp_disseq_def = '" + @rapp.dissequestroDef + "', rapp_disseq_temp = '" + @rapp.dissequestroTemp +
                     "', rapp_disseq_temp_Rim ='" + @rapp.disseq_temp_Rim + "', rapp_disseq_temp_Riapp = '" + @rapp.disseq_temp_Riapp + "', rapp_violazione_sigilli = '" + @rapp.violazioneSigilli +
                     "', rapp_controlliScia ='" + @rapp.controlliScia + "', rapp_accert_avvenuto = '" + @rapp.accertAvvenutoRip + "', rapp_totale = '" + @rapp.totale +
                     "', rapp_parziale ='" + @rapp.parziale + "', rapp_violazioneBeniCult = '" + @rapp.violazioneBeniCult + "', rapp_contr_cantiere_suolo_pubb = '" + @rapp.contrCantSuoloPubb +
                     "', rapp_contr_lavori_edili ='" + @rapp.contrEdiliDPI + "', rapp_contr_cantieri_seq = '" + @rapp.contr_cantiereSeq + "', rapp_contr_da_esposti = '" + @rapp.contrDaEsposti +
                     "', rapp_contr_da_segn ='" + @rapp.contrDaSegn + "', rapp_attivita_interna = '" + @rapp.attività_interna + "', rapp_nota = '" + @rapp.nota +
                     "', rapp_data_consegna_intervento ='" + @rapp.data_consegna_intervento +
                     "', rapp_con_protezioni ='" + @rapp.conProt +
                     "', rapp_senza_protezioni ='" + @rapp.senzaProt +
                     "', rapp_matricola ='" + @rapp.matricola.Trim() +
                     "', rapp_non_avvenuto ='" + @rapp.non_avvenuto + "'" +
                     "', rapp_censimento_all_pubb ='" + @rapp.censimento_all_pubb + "'" +
                     "', rapp_contr_occupazione_abus ='" + @rapp.contr_occupazione_abus + "'" +
                     "', rapp_contr_occ_abitativo ='" + @rapp.contr_occ_abitativo + "'" +
                     "', rapp_contr_occ_no_abitativo ='" + @rapp.contr_occ_no_abitativo + "'" +
                     "', rapp_sgomberi ='" + @rapp.sgomberi + "'" +
                     "', rapp_sgomberi_abus ='" + @rapp.sgomberi_abus + "'" +
                     "', rapp_sgomberi_immobili ='" + @rapp.sgomberi_immobili + "'" +
                     "', rapp_notifica_no_ag ='" + @rapp.notifica_no_ag + "'" +

                     " where rapp_numero_pratica = '" + @rapp.pratica + "'";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_scheda;
                        testoSql = "RappUOTE";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("numero scheda:" + rapp.pratica + ",data ins:" + @rapp.data + ", " + ex.Message + @" - Errore in update dati ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }
        /// <summary>
        /// imposta flag cancellazione
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean UpdFileCaricati(int id)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "update File_Caricati set cancella = 'True'" + " where id_file = " + id;


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "File_Caricati";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("Id FIle:" + id + ", " + ex.Message + @" - Errore in update File_Caricati ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }
        public Boolean UpdGestionePratica(int idFascicolo, GestionePratiche pr)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "update gestionepratiche set assegnato = '" + @pr.assegnato.Replace("'", "''") +
                    "',note = '" + @pr.note.Replace("'", "''") +
                    "',data_rientro ='" + @pr.data_rientro.Replace("'", "''") +
                    "',data_spostamenti='" + @pr.data_spostamenti.Replace("'", "''") +
                    "',DATA_RISCONTRO = '" + @pr.DATA_RISCONTRO.Replace("'", "''") +
                    "',NOTA_SPOSTAMENTO = '" + @pr.notaSpostamento.Replace("'", "''") +
                    "',NOTA_RISCONTRO = '" + @pr.notariscontro.Replace("'", "''") + "'" +
                    " where id_gestionePratica = " + idFascicolo + "";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "gestione pratica";
                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("fascicolo " + pr.fascicolo + ", assegnato:" + pr.assegnato + ", data ins:" + pr.data_uscita + ", " + ex.Message + @" - Errore in inserimento gestione pratica ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }



            }
            catch (Exception)
            {
                resp = false;



            }
            return resp;

        }
        public Boolean UpdPratica(Principale p, string oldMat, int ID, DateTime olddate, string user)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;
            int res1 = 0;
            if (String.IsNullOrWhiteSpace(user))
            {
                return false;
            }
            try
            {
                sql_pratica = "update principale set Sigla= '" + @p.sigla + "', Nominativo = '" + @p.nominativo.Replace("'", "''") + "',Indirizzo = '" + @p.indirizzo.Replace("'", "''") +
                    "',via ='" + @p.via.Replace("'", "''") + "',Inviata = '" + @p.inviata.Replace("'", "''") + "',DataInvio = '" + @p.dataInvio + "',Scaturito = '" + @p.scaturito.Replace("'", "''") +
                    "',accertatori =  '" + @p.accertatori.Replace("'", "''") +
                    "',DataCarico = '" + @p.dataCarico + "',Quartiere = '" + @p.quartiere.Replace("'", "''") + "',nr_Pratica = '" + @p.nr_Pratica + "', giudice = '" + @p.giudice.Replace("'", "''") + "', ProcedimentoPen = '" + @p.procedimentoPen.Replace("'", "''") +
                    "',matricola = '" + @p.matricola + "',DataInserimento = '" + @p.data_ins_pratica + "',macro_area = '" + @p.macro_area.Replace("'", "''") + "',Rif_Prot_Gen = '" + @p.rif_Prot_Gen.Replace("'", "''") +
                    "',dataarrivo = '" + @p.dataArrivo + "', Tipologia_atto ='" + p.tipologia_atto.Replace("'", "''") + "', provenienza ='" + @p.provenienza.Replace("'", "''") + "',TipoProvvedimentoAG ='" + @p.tipoProvvedimentoAG.Replace("'", "''") +
                    "',UlterioreTipoAtto ='" + @p.ulterioreTipoAtto.Replace("'", "''") + "',evasadata = '" + @p.evasaData +
                    "',bu ='" + @p.bu.Replace("'", "''") + "',codiceEdificio ='" + @p.codiceEdificio.Replace("'", "''") + "',accertatori2 ='" + @p.accertatori2.Replace("'", "''") +
                    "',accertatori3 ='" + @p.accertatori3.Replace("'", "''") + "'" + ",NumProtRicStessoCarico =" + @p.NumProtRicStessoCarico +
                    " where  ID = " + ID;
                //accoda senza ripetere quelli esistenti    
                //+ " and  CHARINDEX('" + @p.accertatori.Replace("'", "''") + "', accertatori) = 0";

                string sql_storico =
 "INSERT INTO principalestorico (" +
 "nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen, " +
 "Nominativo, Indirizzo, via, Evasa, EvasaData, Inviata, DataInvio, Scaturito, Accertatori, DataCarico, nr_Pratica, Quartiere, Note, Anno, Giorno, Rif_Prot_Gen, matricola, DataInserimento, " +
 "DataStoricizzazione, MatricolaStoricizzazione, UlterioreTipoAtto, bu, CodiceEdificio, accertatori2, accertatori3, NumProtRicStessoCarico) " +
 "SELECT " +
 "nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen, " +
 "Nominativo, Indirizzo, via, Evasa, EvasaData, Inviata, DataInvio, Scaturito, Accertatori, DataCarico, nr_Pratica, Quartiere, Note, Anno, Giorno, Rif_Prot_Gen, matricola, DataInserimento, " +
 "getdate(), @MatricolaOperatore, UlterioreTipoAtto, bu, CodiceEdificio, accertatori2, accertatori3, NumProtRicStessoCarico " +
 "FROM principale WHERE id = " + ID;
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    SqlTransaction tran;
                    tran = conn.BeginTransaction("trans");
                    command.Transaction = tran;
                    try
                    {

                        command.CommandText = sql_pratica;

                        int res = command.ExecuteNonQuery();
                        if (res > 0)
                        {
                            command.Parameters.AddWithValue("@MatricolaOperatore", user); // aggiungo la matricola di chi esegue la modifica 
                            command.CommandText = sql_storico;
                            res1 = command.ExecuteNonQuery();
                            if (res1 > 0)
                            {
                                tran.Commit();
                                tran.Dispose();
                                resp = true;
                            }
                            else
                            {
                                tran.Rollback();
                                resp = false;
                            }

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
                            sw.WriteLine("matricola:" + p.matricola + ",data ins:" + p.data_ins_pratica + ", " + ex.Message + @" - Errore in salva pratica transaction ");
                            sw.Close();
                            resp = false;
                        }
                        tran.Rollback();

                        resp = false;
                    }
                    conn.Close();
                    return resp;
                }
                //using (SqlConnection conn = new SqlConnection(ConnString))
                //{
                //    conn.Open();
                //    SqlCommand command = conn.CreateCommand();

                //    try
                //    {
                //        command.CommandText = sql_pratica;
                //        testoSql = "Principale";
                //        int res = command.ExecuteNonQuery();
                //    }

                //    catch (Exception ex)
                //    {

                //        if (!File.Exists(LogFile))
                //        {
                //            using (StreamWriter sw = File.CreateText(LogFile)) { }
                //        }

                //        using (StreamWriter sw = File.AppendText(LogFile))
                //        {
                //            sw.WriteLine("matricola:" + p.matricola + ",data ins:" + p.data_ins_pratica + ", " + ex.Message + @" - Errore in update dati ");
                //            sw.Close();
                //        }

                //        resp = false;


                //    }
                //    conn.Close();
                //    conn.Dispose();
                //    return resp;
                //}
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }
        /// <summary>
        /// metodo per modifica riservata
        /// </summary>
        /// <param name="p"></param>
        /// <param name="oldMat"></param>
        /// <param name="olddate"></param>
        /// <param name="oldProtocollo"></param>
        /// <param name="idPratica"></param>
        /// <param name="operatore"></param>
        /// <returns></returns>
        public Boolean SavePraticaTrans(Principale p, string oldMat, DateTime olddate, string oldProtocollo, Int32 idPratica, string operatore)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string sql_storico = String.Empty;
            string testoSql = string.Empty;
            int res1 = 0;
            //try
            //{
            sql_pratica = "insert into principale (nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen," +
                 "Nominativo,Indirizzo,via,Evasa,EvasaData,Inviata,DataInvio,Scaturito,Accertatori,DataCarico,nr_Pratica,Quartiere,Note,Anno,Giorno,Rif_Prot_Gen,matricola,DataInserimento,UlterioreTipoAtto,bu,codiceEdificio)" +
                " Values('" + @p.nrProtocollo + "','" + @p.sigla.Replace("'", "''") + "','" + @p.dataArrivo + "','" + @p.provenienza.Replace("'", "''") + "','" + @p.tipologia_atto.Replace("'", "''") +
                "','" + @p.giudice.Replace("'", "''") + "','" + @p.tipoProvvedimentoAG.Replace("'", "''") + "','" + @p.procedimentoPen + "','" +
                @p.nominativo.Replace("'", "''") + "','" + @p.indirizzo.Replace("'", "''") + "','" + @p.via.Replace("'", "''") + "','" + @p.evasa + "','" + @p.evasaData + "','" + @p.inviata.Replace("'", "''") + "','" +
                @p.dataInvio + "','" + @p.scaturito.Replace("'", "''") + "','" + @p.accertatori.Replace("'", "''") + "','" + @p.dataCarico + "','" + @p.nr_Pratica + "','" +
                 @p.quartiere.Replace("'", "''") + "','" + @p.note.Replace("'", "''") + "','" + @p.anno + "','" + @p.giorno.Replace("'", "''") + "','" + @p.rif_Prot_Gen + "','" + @p.matricola + "','" + @p.data_ins_pratica + "','" + @p.ulterioreTipoAtto.Replace("'", "''") +
                 "','" + @p.bu.Replace("'", "''") + "','" + @p.codiceEdificio.Replace("'", "''") + "')";

            sql_storico = "insert into principalestorico select " +
                "nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen," +
                "Nominativo,Indirizzo,via,Evasa,EvasaData,Inviata,DataInvio,Scaturito,Accertatori,DataCarico,nr_Pratica,Quartiere,Note,Anno,Giorno,Rif_Prot_Gen,matricola,DataInserimento, getdate(), @MatricolaOperatore, UlterioreTipoAtto" +
                " from principale  where nr_protocollo = '" + oldProtocollo + "' and datainserimento = '" + olddate + "' and id = " + idPratica;


            string del = "delete principale where nr_protocollo = '" + oldProtocollo + "' and datainserimento = '" + olddate + "' and matricola = '" + oldMat + "' and id = " + idPratica;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                SqlTransaction tran;
                tran = conn.BeginTransaction("trans");
                command.Transaction = tran;
                try
                {

                    command.CommandText = sql_pratica;

                    int res = command.ExecuteNonQuery();
                    if (res > 0)
                    {
                        command.Parameters.AddWithValue("@MatricolaOperatore", operatore); // aggiungo la matricola di chi esegue la modifica riservata
                        command.CommandText = sql_storico;

                        res1 = command.ExecuteNonQuery();
                    }
                    if (res1 > 0)
                    {
                        command.CommandText = del;

                        command.ExecuteNonQuery();

                        tran.Commit();
                        tran.Dispose();
                        resp = true;

                    }
                    else
                    {
                        tran.Rollback();
                        resp = false;
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
                        sw.WriteLine("matricola:" + p.matricola + ",data ins:" + p.data_ins_pratica + ", " + ex.Message + @" - Errore in salva pratica transaction ");
                        sw.Close();
                        resp = false;
                    }
                    tran.Rollback();

                    resp = false;
                }
                conn.Close();
                return resp;
            }
        }
        public Boolean SavePassword(string password, string matricola)
        {
            bool resp = true;
            string sql_save = String.Empty;
            try
            {
                sql_save = "update operatore set passw = '" + password + "', reset = 'true'" +
                    " where matricola = '" + matricola + "'";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_save;

                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("matricola:" + matricola + " " + ex.Message + @" - Errore in salva password ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }
        public Boolean ResetPassw(string password, string matricola)
        {
            bool resp = true;
            string sql_reset = String.Empty;
            try
            {
                sql_reset = "update operatore set passw = '" + password + "', reset = 'false'" +
                    " where matricola = '" + matricola + "'";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_reset;

                        int res = command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {

                        if (!File.Exists(LogFile))
                        {
                            using (StreamWriter sw = File.CreateText(LogFile)) { }
                        }

                        using (StreamWriter sw = File.AppendText(LogFile))
                        {
                            sw.WriteLine("matricola:" + matricola + " " + ex.Message + @" - Errore in salva password ");
                            sw.Close();
                        }

                        resp = false;


                    }
                    conn.Close();
                    conn.Dispose();
                    return resp;
                }
            }
            catch (Exception)
            {
                resp = false;
            }
            return resp;

        }



    }
}