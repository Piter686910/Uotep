using Microsoft.Ajax.Utilities;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Web.Services;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Uotep.Classi
{
    public class Manager
    {

        //public String ConnString = ConfigurationManager.AppSettings["ConnString"];
        public String ConnString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
        public String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
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

        //get
        public DataTable getPass(String user)
        {

            DataTable tb = new DataTable();
            string sql = "SELECT passw FROM Operatore where Matricola= '" + user + "'";
            //string sql = "SELECT * FROM Operatore where Matricola= '" + user + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }


        }
        public DataTable GetRuolo(String user)
        {

            DataTable tb = new DataTable();
            string sql = "SELECT profilo,ruolo,area FROM Operatore where Matricola= '" + user + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }


        }
        public DataTable getUserByUserPassw(String user, string pasw)
        {

            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Operatore where Matricola= '" + user + "' and passw = '" + pasw + "'";
            //string sql = "SELECT * FROM Operatore where Matricola= '" + user + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }


        }
        public DataTable getUserRules(String user)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Operatore where Matricola= '" + user + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }


        }
        public DataTable getListGiudice()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Giudice order by Giudice";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getListProvenienza()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Provenienza order by Provenienza";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getListTipologiaAbuso()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM TipologiaAbuso order by tipologia";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getListTipologia()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Tipologia order by tipo_nota";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getListScaturito()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Scaturito order by scaturito";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getListQuartiere()
        {
            DataTable tb = new DataTable();
            // string sql = "SELECT distinct quartiere FROM Quart order by quartiere";
            string sql = "SELECT MIN(id_quartiere) AS id_quartiere, quartiere FROM Quart GROUP BY quartiere ORDER BY quartiere";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getQuartiere(string indirizzo)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Quart where toponimo like '%" + indirizzo.Replace("'", "''") + "%'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);

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

                return tb = FillTable(sql, conn);
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

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getListOperatore()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT nominativo FROM operatore where nominativo <> '' order by nominativo ";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
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
        public DataTable getListProvvAg(String sigla)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM TipoNotaAG where sigla = '" + sigla + "' order by tipologia";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getListInviati()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM inviati order by inviata";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// cerca indirizzo nella tabella quartiere
        /// </summary>
        /// <returns></returns>
        public DataTable getListIndirizzo()
        {
            DataTable tb = new DataTable();
            // string sql = "SELECT specie,toponimo  FROM Quart order by toponimo";
            string sql = "SELECT ISNULL(Specie, '') + ' ' + ISNULL(toponimo, '') AS SpecieToponimo FROM  Quart";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }

        /// <summary>
        /// ricerca per protocollo
        /// </summary>
        /// <param name="protocollo"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public DataTable getListPrototocollo(string protocollo, string anno)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Principale where Nr_Protocollo = " + protocollo + " and anno = '" + anno + "' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
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

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per procedimento penale
        /// </summary>
        /// <param name="procedimento"></param>

        /// <returns></returns>
        public DataTable getListProcedimento(string procedimento)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Principale where ProcedimentoPen like '%" + procedimento.Replace("'", "''").Replace("*", "%") + "%' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per evasa
        /// </summary>
        /// <param name="datada"></param>
        ///  /// <param name="dataa"></param>
        /// <returns></returns>
        public DataTable getListEvasaAg(string datada, string dataa)
        {
            DataTable tb = new DataTable();

            DateTime dtda = System.Convert.ToDateTime(datada);
            DateTime dta = System.Convert.ToDateTime(dataa);

            string sql = "SELECT * FROM Principale where EvasaData BETWEEN '" + dtda.ToShortDateString() + "' and '" + dta.ToShortDateString() + "'  order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per protocollo generale
        /// </summary>
        /// <param name="protgen"></param>
        /// <returns></returns>
        public DataTable getListProtGen(string protgen)
        {
            DataTable tb = new DataTable();



            string sql = "SELECT * FROM Principale where Rif_Prot_Gen like '%" + protgen.Replace("'", "''").Replace("*", "%") + "%'  order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per giudice
        /// </summary>
        /// <param name="giudice"></param>
        /// <returns></returns>
        public DataTable getListGiudice(string giudice)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM Principale where giudice like '" + giudice.Replace("'", "''") + "%' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getStatisticaByMeseAnno(string mese, int anno)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM statistiche where mese = '" + mese.ToUpper() + "' and anno =" + anno;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                return tb = FillTable(sql, conn);

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
        public DataTable getListProvenienza(string provenienza)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM Principale where provenienza like '%" + provenienza.Replace("'", "''").Replace("*", "%") + "%'  order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per nominativo
        /// </summary>
        /// <param name="nominativo"></param>
        /// <returns></returns>
        public DataTable getListNominativo(string nominativo)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM Principale where nominativo like '" + nominativo.Replace("'", "''").Replace("*", "%") + "%' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per indirizzo  in tabella pricipale
        /// </summary>
        /// <param name="indirizzo"></param>
        /// <returns></returns>
        public DataTable getListIndirizzo(string indirizzo)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM Principale where indirizzo like '%" + indirizzo.Replace("'", "''").Replace("*", "%") + "%' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per data arrivo
        /// </summary>
        /// <param name="dataarrivo"></param>
        /// <returns></returns>
        public DataTable getListDataArrivo(string dataArrivoDa, string dataArrivoA)
        {
            DataTable tb = new DataTable();
            DateTime dtda = System.Convert.ToDateTime(dataArrivoDa);
            DateTime dta = System.Convert.ToDateTime(dataArrivoA);

            string sql = "SELECT * FROM Principale where DataArrivo BETWEEN '" + dtda.ToShortDateString() + "' and '" + dta.ToShortDateString() + "'  order by dataarrivo desc";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per accertatori
        /// </summary>
        /// <param name="accertatori"></param>
        /// <returns></returns>
        public DataTable getListAccertatori(string accertatori)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM Principale where accertatori like '" + accertatori.Replace("'", "''").Replace("*", "%") + "%'  order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }

        /// <summary>
        /// ricerca per pratica
        /// </summary>
        /// <param name="pratica"></param>
        /// <returns></returns>
        public DataTable getListPratica(string pratica)
        {
            DataTable tb = new DataTable();



            string sql = "SELECT * FROM Principale where nr_pratica = '" + pratica + "' order by dataarrivo desc";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable GetFileByOperatore(string matricola)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM File_Caricati where matricola = '" + matricola + "'";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
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
                // conn.Open();
                //  SqlCommand command = conn.CreateCommand();

                return tb = FillTable(sql, conn);

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

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getPraticaArchivioUoteById(int id)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM ArchivioUote where id_Archivio = '" + id + "'";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
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

                return tb = FillTable(sql, conn);
            }
        }

        public DataTable getGestionePraticaByFascicolo(string fascicolo)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            sql = "SELECT * FROM gestionePratiche where fascicolo = '" + fascicolo + "'";


            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
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

                return tb = FillTable(sql, conn);
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
                }
            }


            if (!String.IsNullOrEmpty(nominativo))
                sql = "SELECT * FROM ArchivioUote where arch_responsabile like '%" + nominativo.Replace("'", "''") + "%'";
            if (!String.IsNullOrEmpty(indirizzo))
                sql = "SELECT * FROM ArchivioUote where arch_indirizzo like '%" + indirizzo.Replace("'", "''") + "%'";

            if (catasto != null)
                sql = "SELECT * FROM ArchivioUote where arch_sezione = '" + catasto[1] + "' and arch_foglio = '" + catasto[2] + "' and arch_particella = '" + catasto[3] +
                   "' and arch_sub= '" + catasto[4] + "'";
            if (!String.IsNullOrEmpty(nota))
                sql = "SELECT * FROM ArchivioUote where arch_note like '%" + nota.Replace("'", "''") + "%'";
            if (annomese != null)
                if (!String.IsNullOrEmpty(annomese[2]))
                    sql = "SELECT * FROM ArchivioUote  WHERE YEAR(arch_dataIns) =" + annomese[1] + " and MONTH(arch_dataIns) =" + annomese[2];
                else
                    sql = "SELECT * FROM ArchivioUote  WHERE YEAR(arch_dataIns) =" + annomese[1];


            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getPraticaId(Int32 protocollo, DateTime data, string sigla, Int32 id)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Principale where Nr_Protocollo = " + protocollo + " and DataInserimento = '" + data + "' and sigla = '" + sigla + "'" + " and id = " + id;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getPratica(Int32 protocollo, DateTime data, string sigla)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Principale where Nr_Protocollo = " + protocollo + " and DataInserimento = '" + data + "' and sigla = '" + sigla + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getPraticaModificaRiservata(string protocollo, string anno)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM Principale where Nr_Protocollo = '" + protocollo + "' and anno = '" + anno + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }


        private DataTable FillTable(String sql, SqlConnection conn)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            DataTable table = new DataTable();
            //SqlConnection conn = new SqlConnection(ConnString);
            conn.Open();
            // AutoCompleteStringCollection MyComplete = new AutoCompleteStringCollection();
            //cmd = new OleDbCommand(select, conn);
            SqlDataAdapter da;
            DataSet ds;

            da = new SqlDataAdapter(sql, conn);
            ds = new DataSet();
            da.Fill(ds);

            table = ds.Tables[0];
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
        public Boolean InsDecretazione(Decretazione decr)
        {
            bool resp = true;
            string sql_decretazione = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_decretazione = "insert into decretazione (decr_idPratica, decr_pratica,decr_decretante, decr_decretato,decr_data,decr_nota," +
                    "decr_dataChiusura, decr_chiuso)" +
                   " Values('" + decr.idPratica + "','" + decr.Npratica + "','" + decr.decretante.Replace("'", "''") + "','" + decr.decretato.Replace("'", "''") +
                   "','" + decr.data + "','" + decr.nota.Replace("'", "''") + "','" + null + "','" + decr.chiuso + "')";


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_decretazione;
                        testoSql = "decretazione";
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
                            sw.WriteLine("pratica:" + decr.Npratica + " - " + ex.Message + @" - Errore in inserimento dati in tabella decretazione");
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
                            ",violazione_sigilli,dissequestri,dissequestri_temp,rimozione_sigilli,controlli_42_04,contr_cant_suolo_pubb,contr_lavori_edili,contr_cant,contr_nato_da_esposti) " +
                        " Values('" + stat.mese.ToUpper() + "'," + stat.anno + "," + stat.relazioni + "," + stat.ponteggi + "," + stat.dpi + "," +
                          stat.esposti_ricevuti + "," + stat.esposti_evasi + "," + stat.ripristino_tot_par + "," + stat.controlli_scia + "," + stat.contr_cant_daily + "," + stat.cnr + "," +
                          stat.annotazioni + "," + stat.notifiche + "," + stat.sequestri + "," + stat.riapp_sigilli + "," + stat.deleghe_ricevute + "," +
                          stat.deleghe_esitate + "," + stat.cnr_annotazioni + "," + stat.interrogazioni + "," + stat.denunce_uff + "," + stat.convalide + "," +
                          stat.demolizioni + "," + stat.violazione_sigilli + "," + stat.dissequestri + "," + stat.dissequestri_temp + "," + stat.riapp_sigilli + "," +
                          stat.controlli_42_04 + "," + stat.contr_cant_suolo_pubb + "," + stat.contr_lavori_edili + "," + stat.contr_cant + "," + stat.contr_nato_da_esposti + ")";


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
        public Boolean InsStatPg(Boolean exist, Statistiche stat)
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
                            ",violazione_sigilli,dissequestri,dissequestri_temp,rimozione_sigilli,controlli_42_04,contr_cant_suolo_pubb,contr_lavori_edili,contr_cant,contr_nato_da_esposti) " +
                        " Values('" + stat.mese.ToUpper() + "'," + stat.anno + "," + stat.relazioni + "," + stat.ponteggi + "," + stat.dpi + "," +
                          stat.esposti_ricevuti + "," + stat.esposti_evasi + "," + stat.ripristino_tot_par + "," + stat.controlli_scia + "," + stat.contr_cant_daily + "," + stat.cnr + "," +
                          stat.annotazioni + "," + stat.notifiche + "," + stat.sequestri + "," + stat.riapp_sigilli + "," + stat.deleghe_ricevute + "," +
                          stat.deleghe_esitate + "," + stat.cnr_annotazioni + "," + stat.interrogazioni + "," + stat.denunce_uff + "," + stat.convalide + "," +
                          stat.demolizioni + "," + stat.violazione_sigilli + "," + stat.dissequestri + "," + stat.dissequestri_temp + "," + stat.riapp_sigilli + "," +
                          stat.controlli_42_04 + "," + stat.contr_cant_suolo_pubb + "," + stat.contr_lavori_edili + "," + stat.contr_cant + "," + stat.contr_nato_da_esposti + ")";


                    else
                    {
                        sql_Statistiche = "update statistiche set interrogazioni = " + stat.interrogazioni +


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
                //idN = -1;

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
                    ", viol_amm_reg_com = " + stat.viol_amm_reg_com +
                    " where mese = '" + @stat.mese + "' and anno = " + stat.anno;


                    command.CommandText = Del_RappUote;
                    object a = command.ExecuteScalar();


                    command.CommandText = sql_Statistiche;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    //  idN = Convert.ToInt32(a);
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
                            ",violazione_sigilli,dissequestri,dissequestri_temp,rimozione_sigilli,controlli_42_04,contr_cant_suolo_pubb,contr_lavori_edili,contr_cant,contr_nato_da_esposti,viol_amm_reg_com) " +
                        " Values('" + stat.mese.ToUpper() + "'," + stat.anno + "," + stat.relazioni + "," + stat.ponteggi + "," + stat.dpi + "," +
                          stat.esposti_ricevuti + "," + stat.esposti_evasi + "," + stat.ripristino_tot_par + "," + stat.controlli_scia + "," + stat.contr_cant_daily + "," + stat.cnr + "," +
                          stat.annotazioni + "," + stat.notifiche + "," + stat.sequestri + "," + stat.riapp_sigilli + "," + stat.deleghe_ricevute + "," +
                          stat.deleghe_esitate + "," + stat.cnr_annotazioni + "," + stat.interrogazioni + "," + stat.denunce_uff + "," + stat.convalide + "," +
                          stat.demolizioni + "," + stat.violazione_sigilli + "," + stat.dissequestri + "," + stat.dissequestri_temp + "," + stat.rimozione_sigilli + "," +
                          stat.controlli_42_04 + "," + stat.contr_cant_suolo_pubb + "," + stat.contr_lavori_edili + "," + stat.contr_cant + "," + stat.contr_nato_da_esposti + "," + stat.viol_amm_reg_com + ")";

                    }
                    else
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
                        ", viol_amm_reg_com = " + stat.viol_amm_reg_com +
                        " where mese = '" + @stat.mese + "' and anno = " + stat.anno;


                    }

                    sql_insRap = "insert into RappUote (rapp_numero_pratica, rapp_data,	rapp_nominativo,rapp_indirizzo,rapp_pattuglia," +
                     "rapp_delegaAG,	rapp_resa,	rapp_segnalazione,	rapp_esposto,rapp_numEsposti,rapp_notifica,	rapp_iniziativa,rapp_comandante," +
                     "rapp_coordinatore,	rapp_relazione,	rapp_cnr,rapp_annotazionePG,rapp_verbale_seq,rapp_esito_delega,	rapp_contestaz_amm," +
                     "rapp_convalida,rapp_disseq_def,rapp_disseq_temp,rapp_disseq_temp_Rim,rapp_disseq_temp_Riapp,rapp_violazione_sigilli," +
                     "rapp_controlliScia,rapp_accert_avvenuto,rapp_totale,rapp_parziale,	rapp_violazioneBeniCult,rapp_contr_cantiere_suolo_pubb," +
                     "rapp_contr_lavori_edili,rapp_contr_cantieri_seq,rapp_contr_da_esposti,rapp_contr_da_segn,rapp_attivita_interna,rapp_nota,rapp_data_consegna_intervento, rapp_capopattuglia,rapp_uote,rapp_uotp,rapp_dataInserimento,rapp_con_protezioni,rapp_senza_protezioni,rapp_matricola,rapp_non_avvenuto)" +
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
                 @rapp.uote + "','" + @rapp.uotp + "','" + @rapp.dataInserimento + "','" + @rapp.conProt + "','" + @rapp.senzaProt + "','" + rapp.matricola.Replace("'", "''") + "','" + @rapp.non_avvenuto + "'); SELECT SCOPE_IDENTITY();";
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
                    ", viol_amm_reg_com = " + stat.viol_amm_reg_com +
                    " where mese = '" + @stat.mese + "' and anno = " + stat.anno;


                    sql_insRap = "insert into RappUote (rapp_numero_pratica, rapp_data,	rapp_nominativo,rapp_indirizzo,rapp_pattuglia," +
                     "rapp_delegaAG,	rapp_resa,	rapp_segnalazione,	rapp_esposto,rapp_numEsposti,rapp_notifica,	rapp_iniziativa,rapp_comandante," +
                     "rapp_coordinatore,	rapp_relazione,	rapp_cnr,rapp_annotazionePG,rapp_verbale_seq,rapp_esito_delega,	rapp_contestaz_amm," +
                     "rapp_convalida,rapp_disseq_def,rapp_disseq_temp,rapp_disseq_temp_Rim,rapp_disseq_temp_Riapp,rapp_violazione_sigilli," +
                     "rapp_controlliScia,rapp_accert_avvenuto,rapp_totale,rapp_parziale,	rapp_violazioneBeniCult,rapp_contr_cantiere_suolo_pubb," +
                     "rapp_contr_lavori_edili,rapp_contr_cantieri_seq,rapp_contr_da_esposti,rapp_contr_da_segn,rapp_attivita_interna,rapp_nota,rapp_data_consegna_intervento, rapp_capopattuglia,rapp_uote,rapp_uotp,rapp_dataInserimento,rapp_con_protezioni,rapp_senza_protezioni,rapp_matricola,rapp_non_avvenuto)" +
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
                 @rapp.uote + "','" + @rapp.uotp + "','" + @rapp.dataInserimento + "','" + @rapp.conProt + "','" + @rapp.senzaProt + "','" + rapp.matricola.Replace("'", "''") + "','" + @rapp.non_avvenuto + "'); SELECT SCOPE_IDENTITY();";
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
                     "rapp_contr_lavori_edili,rapp_contr_cantieri_seq,rapp_contr_da_esposti,rapp_contr_da_segn,rapp_attivita_interna,rapp_nota,rapp_data_consegna_intervento, rapp_capopattuglia,rapp_uote,rapp_uotp,rapp_dataInserimento,rapp_con_protezioni,rapp_senza_protezioni,rapp_matricola,rapp_non_avvenuto)" +
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
                 @rapp.uote + "','" + @rapp.uotp + "','" + @rapp.dataInserimento + "','" + @rapp.conProt + "','" + @rapp.senzaProt + "','" + rapp.matricola.Replace("'", "''") + "','" + @rapp.non_avvenuto + "'); SELECT SCOPE_IDENTITY();";
                    command.CommandText = sql_insRap;
                    object a = command.ExecuteScalar();


                    //command.CommandText = sql_Statistiche;
                    //command.ExecuteNonQuery();
                    //transaction.Commit();
                    idN = Convert.ToInt32(a);
                    resp = true;
                }

                catch (Exception ex)
                {
                    //if (transaction != null)
                    //{
                    //    transaction.Rollback();

                    if (!File.Exists(LogFile))
                    {
                        using (StreamWriter sw = File.CreateText(LogFile)) { }
                    }

                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine("matricola:" + rapp.matricola + ",data ins:" + rapp.data + ", " + ex.Message + @" - Errore in inserimento scheda intervento uote ");
                        sw.Close();
                    }
                    //}
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

                sql_pratica = "insert into gestionePratiche (fascicolo, assegnato, data_uscita, data_rientro, data_spostamenti, data_riscontro,note,NOTA_SPOSTAMENTO,NOTA_RISCONTRO)" +
                   " Values('" + @pr.fascicolo + "','" + @pr.assegnato.Replace("'", "''") + "','" + @pr.data_uscita + "','" + @pr.data_rientro.Replace("'", "''") + "','" + @pr.data_spostamenti.Replace("'", "''") + "','" +
                   @pr.data_rientro.Replace("'", "''") + "','" + @pr.note.Replace("'", "''") + "','" + @pr.notaSpostamento.Replace("'", "''") + "','" + @pr.notariscontro.Replace("'", "''") + "')";


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
        public DataTable GetSchedeBy(string numPratica, string pattuglia, string dataI, Boolean attivita, int id)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();

            if (!String.IsNullOrEmpty(pattuglia))
            {

                sql = "SELECT * FROM RappUote where rapp_pattuglia like '%" + pattuglia + "%'" + " order by rapp_pattuglia";
            }

            if (!String.IsNullOrEmpty(numPratica))
            {
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
                return tb = FillTable(sql, conn);
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
                return tb = FillTable(sql, conn);
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
                return tb = FillTable(sql, conn);
            }


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
                return tb = FillTable(sql, conn);
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
                    "arch_inCarico,arch_evasa,arch_note,arch_tipologia,arch_quartiere,arch_suoloPub,arch_vincoli,arch_1089,arch_demolita,arch_allegati,arch_matricola,arch_sezione,arch_foglio,arch_particella,arch_sub,arch_dataInizioAttivita,arch_propPriv,arch_propComune,arch_propBeniCult,arch_propAltriEnti,arch_foglionct,arch_particellanct)" +
                   " Values('" + @arch.arch_numPratica + "','" + @arch.arch_bis + "','" + @arch.arch_dataIns + "','" +
                   @arch.arch_datault_intervento + "','" + @arch.arch_indirizzo.Replace("'", "''") + "','" +
                   @arch.arch_responsabile.Replace("'", "''") + "','" + @arch.arch_natoA.Replace("'", "''") + "','" + @arch.arch_dataNascita + "','" +
                   @arch.arch_inCarico.Replace("'", "''") + "','" + @arch.arch_evasa + "','" + @arch.arch_note.Replace("'", "''") + "','" +
                   @arch.arch_tipologia.Replace("'", "''") + "','" + @arch.arch_quartiere.Replace("'", "''") + "','" + @arch.arch_suoloPub + "','" +
                   @arch.arch_vincoli + "','" + @arch.arch_1089 + "','" + @arch.arch_demolita + "','" +
                   @arch.arch_allegati.Replace("'", "''") + "','" + @arch.arch_matricola + "','" + @arch.arch_sezione.Replace("'", "''") + "','" + @arch.arch_foglio + "','" + @arch.arch_particella + "','" + @arch.arch_sub + "','" + @arch.arch_dataInizioAttivita + "','" +
                   @arch.arch_propPriv + "','" + @arch.arch_propBeniCult + "','" + @arch.arch_propComune + "','" + @arch.arch_propAltriEnti + "','" + @arch.arch_foglioNct + "','" + @arch.arch_particellaNct + "')";


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
        /// Salva il nuovo fascicolo protocollo
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Boolean SavePratica(Principale p, Int32 id, Statistiche stat, Boolean exist)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string sql_Statistiche = String.Empty;
            string testoSql = string.Empty;
            int res1 = 0;
            try
            {

                sql_pratica = "insert into principale (nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen," +
                    "Nominativo,Indirizzo,Evasa,EvasaData,Inviata,DataInvio,Scaturito,Accertatori,DataCarico,nr_Pratica,Quartiere,Note,Anno,Giorno,Rif_Prot_Gen,matricola,DataInserimento)" +
                   " Values('" + @p.nrProtocollo + "','" + @p.sigla.Replace("'", "''") + "','" + @p.dataArrivo + "','" + @p.provenienza.Replace("'", "''") + "','" + @p.tipologia_atto.Replace("'", "''") +
                   "','" + @p.giudice.Replace("'", "''") + "','" + @p.tipoProvvedimentoAG.Replace("'", "''") + "','" + @p.procedimentoPen + "','" +
                   @p.nominativo.Replace("'", "''") + "','" + @p.indirizzo.Replace("'", "''") + "','" + @p.evasa + "','" + @p.evasaData + "','" + @p.inviata.Replace("'", "''") + "','" +
                   @p.dataInvio + "','" + @p.scaturito.Replace("'", "''") + "','" + @p.accertatori.Replace("'", "''") + "','" + @p.dataCarico + "','" + @p.nr_Pratica + "','" +
                    @p.quartiere.Replace("'", "''") + "','" + @p.note.Replace("'", "''") + "','" + @p.anno + "','" + @p.giorno + "','" + @p.rif_Prot_Gen + "','" + @p.matricola + "','" + @p.data_ins_pratica + "')";
                if (exist)
                {
                    sql_Statistiche = "update statistiche set deleghe_ricevute = " + stat.deleghe_ricevute + ", esposti_ricevuti = " + stat.esposti_ricevuti +
                           " where mese = '" + @stat.mese + "' and anno = " + stat.anno;

                }
                else
                {
                    sql_Statistiche = "insert into statistiche (mese,anno,relazioni,ponteggi,dpi,esposti_ricevuti,esposti_evasi,ripristino_tot_par,controlli_scia,contr_cant_daily,cnr,annotazioni,notifiche" +
                               ",sequestri,riapp_sigilli,deleghe_ricevute,deleghe_esitate,cnr_annotazioni,interrogazioni,denunce_uff,convalide,demolizioni" +
                               ",violazione_sigilli,dissequestri,dissequestri_temp,rimozione_sigilli,controlli_42_04,contr_cant_suolo_pubb,contr_lavori_edili,contr_cant,contr_nato_da_esposti) " +
                           " Values('" + stat.mese.ToUpper() + "'," + stat.anno + "," + stat.relazioni + "," + stat.ponteggi + "," + stat.dpi + "," +
                             stat.esposti_ricevuti + "," + stat.esposti_evasi + "," + stat.ripristino_tot_par + "," + stat.controlli_scia + "," + stat.contr_cant_daily + "," + stat.cnr + "," +
                             stat.annotazioni + "," + stat.notifiche + "," + stat.sequestri + "," + stat.riapp_sigilli + "," + stat.deleghe_ricevute + "," +
                             stat.deleghe_esitate + "," + stat.cnr_annotazioni + "," + stat.interrogazioni + "," + stat.denunce_uff + "," + stat.convalide + "," +
                             stat.demolizioni + "," + stat.violazione_sigilli + "," + stat.dissequestri + "," + stat.dissequestri_temp + "," + stat.riapp_sigilli + "," +
                             stat.controlli_42_04 + "," + stat.contr_cant_suolo_pubb + "," + stat.contr_lavori_edili + "," + stat.contr_cant + "," + stat.contr_nato_da_esposti + ")";

                }
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    SqlTransaction tran;
                    tran = conn.BeginTransaction("trans");
                    command.Transaction = tran;

                    try
                    {
                        string sql = "select * from principale where Nr_Protocollo= '" + p.nrProtocollo + "' and anno = '" + p.anno + "'";

                        SqlDataAdapter da;
                        DataSet ds;

                        da = new SqlDataAdapter(sql, conn);
                        da.SelectCommand.Transaction = tran;
                        ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                            return false;
                        else
                        {


                            command.CommandText = sql_pratica;
                            testoSql = "Principale";
                            int res = command.ExecuteNonQuery();
                            if (res > 0)
                            {
                                command.CommandText = sql_Statistiche;

                                res1 = command.ExecuteNonQuery();
                            }
                            if (res1 > 0)
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
            return resp;

        }
        //UPDATE
        public Boolean UpdDecretazioneChiusura(Decretazione p)
        {
            bool resp = true;
            string sql_updDecretazione = String.Empty;
            string sql_updPrincipale = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_updDecretazione = "update decretazione set decr_dataChiusura = '" + @p.dataChiusura + "',decr_chiuso = '" + @p.chiuso + "'" +
                    " where  decr_pratica = '" + p.Npratica + "' and decr_idPratica = " + p.idPratica;

                sql_updPrincipale = "update principale set Evasa = 'True' , EvasaData = '" + @p.dataChiusura + "' where  id = " + p.idPratica + " and Nr_Protocollo = " + p.Npratica;

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

                        int res = command.ExecuteNonQuery();
                        if (res > 0)
                        {
                            command.CommandText = sql_updPrincipale;

                            command.ExecuteNonQuery();

                            tran.Commit();

                            resp = true;

                        }
                        else
                        {
                            tran.Rollback();
                            resp = false;
                        }

                        command.CommandText = sql_updDecretazione;
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
        public Boolean UpdPratica(Principale p, string oldMat, int ID, DateTime olddate)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "update principale set Nominativo = '" + @p.nominativo.Replace("'", "''") + "',Indirizzo = '" + @p.indirizzo.Replace("'", "''") + "',via ='" + @p.via.Replace("'", "''") + "',Evasa='" + @p.evasa +
                    "',EvasaData = '" + @p.evasaData + "',Inviata = '" + @p.inviata.Replace("'", "''") + "',DataInvio = '" + @p.dataInvio + "',Scaturito = '" + @p.scaturito.Replace("'", "''") +
                    "',Accertatori = '" + @p.accertatori.Replace("'", "''") + "',DataCarico = '" + @p.dataCarico + "',Quartiere = '" + @p.quartiere.Replace("'", "''") +
                    "',Note ='" + @p.note.Replace("'", "''") + "',matricola = '" + @p.matricola + "',DataInserimento = '" + @p.data_ins_pratica + "'" +


                    " where  ID = " + ID;


                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    try
                    {
                        command.CommandText = sql_pratica;
                        testoSql = "Principale";
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
                            sw.WriteLine("matricola:" + p.matricola + ",data ins:" + p.data_ins_pratica + ", " + ex.Message + @" - Errore in update dati ");
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
                 "Nominativo,Indirizzo,via,Evasa,EvasaData,Inviata,DataInvio,Scaturito,Accertatori,DataCarico,nr_Pratica,Quartiere,Note,Anno,Giorno,Rif_Prot_Gen,matricola,DataInserimento)" +
                " Values('" + @p.nrProtocollo + "','" + @p.sigla.Replace("'", "''") + "','" + @p.dataArrivo + "','" + @p.provenienza.Replace("'", "''") + "','" + @p.tipologia_atto.Replace("'", "''") +
                "','" + @p.giudice.Replace("'", "''") + "','" + @p.tipoProvvedimentoAG.Replace("'", "''") + "','" + @p.procedimentoPen + "','" +
                @p.nominativo.Replace("'", "''") + "','" + @p.indirizzo.Replace("'", "''") + "','" + @p.via.Replace("'", "''") + "','" + @p.evasa + "','" + @p.evasaData + "','" + @p.inviata.Replace("'", "''") + "','" +
                @p.dataInvio + "','" + @p.scaturito.Replace("'", "''") + "','" + @p.accertatori.Replace("'", "''") + "','" + @p.dataCarico + "','" + @p.nr_Pratica + "','" +
                 @p.quartiere.Replace("'", "''") + "','" + @p.note.Replace("'", "''") + "','" + @p.anno + "','" + @p.giorno.Replace("'", "''") + "','" + @p.rif_Prot_Gen + "','" + @p.matricola + "','" + @p.data_ins_pratica + "')";

            sql_storico = "insert into principalestorico select " +
                "nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen," +
                "Nominativo,Indirizzo,via,Evasa,EvasaData,Inviata,DataInvio,Scaturito,Accertatori,DataCarico,nr_Pratica,Quartiere,Note,Anno,Giorno,Rif_Prot_Gen,matricola,DataInserimento, getdate(), @MatricolaOperatore" +
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