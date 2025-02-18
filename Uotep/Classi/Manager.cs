using Microsoft.Ajax.Utilities;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace Uotep.Classi
{
    public class Manager
    {

        //public String ConnString = ConfigurationManager.AppSettings["ConnString"];
        public String ConnString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
        public String LogFile = ConfigurationManager.AppSettings["LogFile"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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
        //SqlConnection conn = new SqlConnection(ConnString);
        //conn.Open();
        //SqlTransaction transaction = conn.BeginTransaction();
        //SqlCommand cmd = new SqlCommand(Del_ana, conn);
        //testoSql = "anagrafica";
        //cmd.ExecuteNonQuery();


        //SqlCommand cmd1 = new SqlCommand(Del_acc, conn);
        //testoSql = "accerdtamenti";
        //cmd1.ExecuteNonQuery();

        //transaction.Commit();
        //if (transaction != null)
        //{
        //    transaction.Rollback();
        //}
        //cmd = new OleDbCommand(Del_ana, conn, Tran);







        //cmd.ExecuteNonQuery();

        //cmd = new OleDbCommand(Del_acc, conn, Tran);
        //testoSql = "Accertamenti";
        //cmd.ExecuteNonQuery();

        //Tran.Commit();

        //resp = true;
        //conn.Close();

        //conn.Dispose();
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

        //public Boolean UpdAna(Anagrafica ana, Accertamenti acc, String id)
        //{
        //    bool resp = true;
        //    string sql_ana = String.Empty;
        //    string sql_acc = String.Empty;
        //    String testoSql = String.Empty;
        //    try
        //    {
        //        sql_ana = "update  anagrafica1 set cognome = '" + @ana.cognome.Replace("'", "''") + "'," + " nome = '"
        //            + @ana.nome.Replace("'", "''") + "'," +
        //            " indirizzo = '" + @ana.indirizzo.Replace("'", "''") + "', data_nascita= '" + @ana.data_nascita + "', luogo_nascita = '" + @ana.luogo_nascita
        //             + "', telefono = '" + @ana.telefono + "', cellulare = '" + ana.cellulare + "' where numero_pratica_accertamenti = '" + id + "'";


        //        sql_acc = "update Accertamenti set indirizzo_manufatto = '" + @acc.indirizzo_manufatto.Replace("'", "''") + "'," +

        //            " tipologia_pratica = '" + @acc.tipologia_pratica.Replace("'", "''") + "'," +
        //            " stato_pratica = '" + @acc.tipologia_pratica.Replace("'", "''") + "'," +
        //            " data_sopralluogo = '" + @acc.data_sopralluogo + "'," +
        //            " data_carico = '" + @acc.data_carico + "'," +
        //            " data_scarico = '" + @acc.data_scarico + "'," +
        //            " operatore1 = '" + @acc.operatore1.Replace("'", "''") + "'," +
        //            " operatore2 = '" + @acc.operatore2.Replace("'", "''") + "'," +
        //            " operatore3 = '" + @acc.operatore3.Replace("'", "''") + "'," +
        //            " operatore4 = '" + @acc.operatore4.Replace("'", "''") + "'," +
        //            " grado1  = '" + @acc.grado1 + "'," +
        //            " grado2  = '" + @acc.grado2 + "'," +
        //            " grado3  = '" + @acc.grado3 + "'," +
        //            " grado4  = '" + @acc.grado4 + "'," +
        //            " quartiere = '" + @acc.quartiere.Replace("'", "''") + "'," +
        //            " tipo_abuso = '" + @acc.tipo_abuso.Replace("'", "''") + "'," +
        //            " annotazione = '" + @acc.annotazione.Replace("'", "''") +
        //            "' where num_pratica_accertamenti = '" + @acc.num_pratica_accertamenti + "'";

        //        //OleDbTransaction Tran;
        //        //OleDbConnection conn = new OleDbConnection(ConnString);
        //        //conn.Open();
        //        //Tran = conn.BeginTransaction();
        //        //OleDbCommand cmd;
        //        //cmd = new OleDbCommand(sql_ana, conn, Tran);
        //        //testoSql = "Anagrafica";
        //        //cmd.ExecuteNonQuery();

        //        //cmd = new OleDbCommand(sql_acc, conn, Tran);
        //        //testoSql = "Accertamenti";
        //        //cmd.ExecuteNonQuery();

        //        //Tran.Commit();
        //        //resp = true;
        //        //conn.Close();
        //        using (SqlConnection conn1 = new SqlConnection(ConnString))
        //        {
        //            conn1.Open();
        //            SqlCommand command = conn1.CreateCommand();
        //            SqlTransaction tran;
        //            tran = conn1.BeginTransaction("trans");
        //            command.Transaction = tran;
        //            try
        //            {

        //                command.CommandText = sql_ana;
        //                testoSql = "Anagrafica";
        //                int res = command.ExecuteNonQuery();

        //                if (res > 0)
        //                {
        //                    command.CommandText = sql_acc;
        //                    testoSql = "Accertamenti";
        //                    command.ExecuteNonQuery();

        //                    tran.Commit();

        //                    resp = true;
        //                }

        //            }

        //            catch (Exception)
        //            {

        //                tran.Rollback();
        //                resp = false;


        //            }
        //            conn1.Close();
        //            return resp;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        resp = false;

        //        MessageBox.Show("Errore durante salvataggio", "Attenzione!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        if (!File.Exists(LogFile))
        //        {
        //            using (StreamWriter sw = File.CreateText(LogFile)) { }
        //        }

        //        using (StreamWriter sw = File.AppendText(LogFile))
        //        {

        //            switch (testoSql)
        //            {
        //                case "Anagrafica":
        //                    sw.WriteLine(ex.Message + " " + sql_ana + " " + testoSql);
        //                    break;
        //                case "Accertamenti":
        //                    sw.WriteLine(ex.Message + " " + sql_acc + " " + testoSql);
        //                    break;
        //            }
        //            sw.Close();
        //        }

        //    }
        //    return resp;

        //}
        //public Boolean InsNuovoUtente(Utenti ut)
        //{
        //    bool resp = true;
        //    string sql_ins = String.Empty;
        //    String testoSql = String.Empty;
        //    //  OleDbConnection conn = new OleDbConnection(ConnString);
        //    //conn.Open();
        //    using (SqlConnection conn1 = new SqlConnection(ConnString))
        //    {
        //        conn1.Open();
        //        SqlCommand command = conn1.CreateCommand();
        //        //  SqlTransaction tran;
        //        //  tran = conn1.BeginTransaction("trans");
        //        // command.Transaction = tran;
        //        try
        //        {
        //            testoSql = "Utenti";
        //            string sql = "select * from Operatore where matricola = '" + ut.Matricola + "'";
        //            command.CommandText = sql;

        //            int res = command.ExecuteNonQuery();
        //            if (res == -1)
        //            {
        //                sql_ins = "insert into Operatore (Matricola,Passw,Profilo,Nota)" +
        //                             " Values('" + ut.Matricola + "','" +
        //                               @ut.Password.Replace("'", "''") + "','" +
        //                               @ut.Profilo.Replace("'", "''") + "','" +
        //                               @ut.Nota.Replace("'", "''") + "')";
        //            }

        //            command.CommandText = sql_ins;
        //            command.ExecuteNonQuery();
        //            //  tran.Commit();

        //            resp = true;


        //        }

        //        catch (Exception)
        //        {

        //            //tran.Rollback();
        //            resp = false;


        //        }
        //        conn1.Close();
        //        return resp;
        //    }



        //}
        //public Boolean InsRappUote(Rapp rapp)
        //{
        //    bool resp = true;
        //    string sql_ins = String.Empty;

        //    using (SqlConnection conn1 = new SqlConnection(ConnString))
        //    {
        //        conn1.Open();
        //        SqlCommand command = conn1.CreateCommand();

        //        try
        //        {
        //            sql_ins = "insert into RappUote (rapp_numero_pratica, rapp_ora,rapp_data,	rapp_nominativo,rapp_indirizzo,rapp_pattuglia," +
        //            "rapp_delegaAG,	rapp_resa,	rapp_segnalazione,	rapp_esposto,rapp_numEsposti,rapp_notifica,	rapp_iniziativa,rapp_comandante," +
        //            "rapp_coordinatore,	rapp_relazione,	rapp_cnr,rapp_annotazionePG,rapp_verbale_seq,rapp_esito_delega,	rapp_contestaz_amm," +
        //            "rapp_convalida,rapp_disseq_def,rapp_disseq_temp,rapp_disseq_temp_Rim,rapp_disseq_temp_Riapp,rapp_violazione_sigilli," +
        //            "rapp_controlliScia,rapp_accert_avvenuto,rapp_totale,rapp_parziale,	rapp_violazioneBeniCult,rapp_contr_cantiere_suolo_pubb," +
        //            "rapp_contr_lavori_edili,rapp_contr_cantieri_seq,rapp_contr_da_esposti,rapp_contr_da_segn,rapp_attivita_interna,rapp_nota,rapp_data_consegna_intervento)" +
        //      " Values('" + rapp.pratica + "','" +
        //        @rapp.ora + "','" +
        //        @rapp.data + "','" +
        //        @rapp.nominativo.Replace("'", "''") + "','" +
        //        @rapp.indirizzo.Replace("'", "''") + "','" +
        //        @rapp.pattuglia.Replace("'", "''") + "','" +
        //        @rapp.delegaAG + "','" +
        //        @rapp.resa + "','" +
        //        @rapp.segnalazione + "','" +
        //        @rapp.esposti + "','" +
        //        @rapp.num_esposti + "','" +
        //        @rapp.notifica + "','" +
        //        @rapp.iniziativa + "','" +
        //        @rapp.cdr + "','" +
        //        @rapp.coordinatore + "','" +
        //        @rapp.relazione + "','" +
        //        @rapp.cnr + "','" +
        //        @rapp.annotazionePG + "','" +
        //        @rapp.verbaleSeq + "','" +
        //        @rapp.esitoDelega + "','" +
        //        @rapp.contestazioneAmm + "','" +
        //        @rapp.convalida + "','" +
        //        @rapp.dissequestroDef + "','" +
        //        @rapp.dissequestroTemp + "','" +
        //        @rapp.rimozione + "','" +
        //        @rapp.riapposizione + "','" +
        //        @rapp.violazioneSigilli + "','" +
        //        @rapp.controlliScia + "','" +
        //        @rapp.accertAvvenutoRip + "','" +
        //        @rapp.totale + "','" +
        //        @rapp.parziale + "','" +
        //        @rapp.vilazioneBeniCult + "','" +
        //        @rapp.contrCantSuoloPubb + "','" +
        //        @rapp.contrEdiliDPI + "','" +
        //        @rapp.contr_cantiereSeq + "','" +
        //        @rapp.contrDaEsposti + "','" +
        //        @rapp.contrDaSegn + "','" +
        //        @rapp.attività_interna + "','" +
        //        @rapp.nota.Replace("'", "''") + "','" +
        //        rapp.data_consegna_intervento + "')";
        //            command.CommandText = sql_ins;
        //            command.ExecuteNonQuery();

        //            resp = true;


        //        }

        //        catch (Exception)
        //        {
        //            resp = false;


        //        }
        //        conn1.Close();
        //        return resp;
        //    }




        //}
        //public Boolean SaveAna(Anagrafica ana, Accertamenti acc)
        //{
        //    bool resp = true;
        //    string sql_ana = String.Empty;
        //    String sql_acc = String.Empty;
        //    String testoSql = String.Empty;

        //    try
        //    {
        //        sql_ana = "insert into anagrafica1 (cognome, nome, indirizzo, data_nascita, luogo_nascita, numero_pratica_accertamenti, telefono, cellulare)" +
        //           " Values('" + @ana.cognome.Replace("'", "''") + "','" + @ana.nome.Replace("'", "''") + "','" + @ana.indirizzo.Replace("'", "''") + "','" +
        //           @ana.data_nascita.ToShortDateString() + "','" + @ana.luogo_nascita.Replace("'", "''") + "','" +
        //           @ana.numero_pratica_accertamenti + "','" + @ana.telefono + "','" + ana.cellulare + "')";

        //        sql_acc = "insert into Accertamenti (num_pratica_accertamenti, indirizzo_manufatto, tipologia_pratica, stato_pratica, data_sopralluogo, data_carico, data_scarico, cartella_doc, operatore1, operatore2, operatore3, operatore4, quartiere, grado1, grado2, grado3, grado4, tipo_abuso, annotazione)" +
        //           " Values('" + acc.num_pratica_accertamenti + "','" +
        //             @acc.indirizzo_manufatto.Replace("'", "''") + "','" +
        //             @acc.tipologia_pratica.Replace("'", "''") + "','" +
        //             @acc.stato_pratica.Replace("'", "''") + "','" +
        //             @acc.data_sopralluogo + "','" +
        //             @acc.data_carico + "','" +
        //             @acc.data_scarico + "','" +
        //             @acc.cartella_doc.Replace("'", "''") + "','" +
        //             @acc.operatore1.Replace("'", "''") + "','" +
        //             @acc.operatore2.Replace("'", "''") + "','" +
        //             @acc.operatore3.Replace("'", "''") + "','" +
        //             @acc.operatore4.Replace("'", "''") + "','" +
        //             @acc.quartiere.Replace("'", "''") + "','" +
        //             @acc.grado1 + "','" +
        //             @acc.grado2 + "','" +
        //             @acc.grado3 + "','" +
        //             @acc.grado4 + "','" +
        //             @acc.tipo_abuso.Replace("'", "''") + "','" +
        //             @acc.annotazione.Replace("'", "''") + "')";
        //        //*
        //        using (SqlConnection conn1 = new SqlConnection(ConnString))
        //        {
        //            conn1.Open();
        //            SqlCommand command = conn1.CreateCommand();
        //            SqlTransaction tran;
        //            tran = conn1.BeginTransaction("trans");
        //            command.Transaction = tran;
        //            try
        //            {

        //                command.CommandText = sql_ana;
        //                testoSql = "Anagrafica";
        //                int res = command.ExecuteNonQuery();

        //                if (res > 0)
        //                {
        //                    command.CommandText = sql_acc;
        //                    testoSql = "Accertamenti";
        //                    command.ExecuteNonQuery();

        //                    tran.Commit();

        //                    resp = true;
        //                }

        //            }

        //            catch (Exception)
        //            {

        //                tran.Rollback();
        //                resp = false;


        //            }
        //            conn1.Close();
        //            return resp;
        //        }



        //        ////OleDbTransaction Tran;
        //        ////OleDbConnection conn = new OleDbConnection(ConnString);
        //        //conn.Open();
        //        //Tran = conn.BeginTransaction();
        //        //OleDbCommand cmd;

        //        //cmd = new OleDbCommand(sql_ana, conn, Tran);
        //        //testoSql = "Anagrafica";
        //        //cmd.ExecuteNonQuery();
        //        //cmd = new OleDbCommand(sql_acc, conn, Tran);
        //        //testoSql = "Accertamenti";
        //        //cmd.ExecuteNonQuery();

        //        //Tran.Commit();
        //        //resp = true;
        //        //conn.Close();
        //        //cmd = new OleDbCommand(sql_ana, conn);
        //        //cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        resp = false;

        //        MessageBox.Show("Errore durante salvataggio " + testoSql, "Attenzione!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        if (!File.Exists(LogFile))
        //        {
        //            using (StreamWriter sw = File.CreateText(LogFile)) { }
        //        }

        //        using (StreamWriter sw = File.AppendText(LogFile))
        //        {
        //            switch (testoSql)
        //            {
        //                case "Anagrafica":
        //                    sw.WriteLine(ex.Message + " " + sql_ana + " " + testoSql);
        //                    break;
        //                case "Accertamenti":
        //                    sw.WriteLine(ex.Message + " " + sql_acc + " " + testoSql);
        //                    break;
        //            }
        //            sw.Close();
        //        }

        //    }
        //    return resp;

        //}
        ///// <summary>
        ///// conta i records della tebella Anagrafica
        ///// </summary>
        ///// <returns></returns>
        //public Int32 ContaRec()
        //{
        //    int cnt = 0;
        //    String select = "SELECT count(cognome) as CNT FROM anagrafica1";

        //    // OleDbConnection conn = new OleDbConnection(ConnString);

        //    // conn.Open();

        //    // cmd = new OleDbCommand(select, conn);
        //    //  OleDbDataReader Reader = cmd.ExecuteReader();
        //    //  if (Reader.HasRows)
        //    //   {
        //    //       Reader.Read();
        //    //       cnt = Reader.GetInt32(0);
        //    //   }


        //    SqlConnection conn = new SqlConnection(ConnString);
        //    conn.Open();

        //    SqlDataAdapter da;
        //    DataSet ds;

        //    da = new SqlDataAdapter(select, conn);
        //    ds = new DataSet();
        //    da.Fill(ds);
        //    DataTable tb = ds.Tables[0];
        //    foreach (DataRow row in tb.Rows)
        //    {
        //        cnt = cnt + 1;
        //    }
        //    return cnt;

        //}
        ///// <summary>
        ///// autocomplete operatori
        ///// </summary>
        ///// <returns></returns>
        //public AutoCompleteStringCollection GetAutoCompleteOperatori(Manager mn)
        //{
        //    AutoCompleteStringCollection txtBox = new AutoCompleteStringCollection();
        //    //OleDbCommand cmd = null;
        //    String select = "SELECT dip_cognome FROM Dipendenti";

        //    //OleDbConnection conn = new OleDbConnection(ConnString);
        //    SqlConnection conn = new SqlConnection(ConnString);
        //    conn.Open();
        //    AutoCompleteStringCollection MyComplete = new AutoCompleteStringCollection();
        //    //cmd = new OleDbCommand(select, conn);
        //    SqlDataAdapter da;
        //    DataSet ds;

        //    da = new SqlDataAdapter(select, conn);
        //    ds = new DataSet();
        //    da.Fill(ds);

        //    //OleDbDataReader reader = cmd.ExecuteReader();
        //    //while (reader.Read())
        //    //{

        //    //    MyComplete.Add(reader.GetString(0));
        //    //}
        //    DataTable tb = ds.Tables[0];
        //    foreach (DataRow row in tb.Rows)
        //    {
        //        MyComplete.Add(Convert.ToString(row[0]));
        //    }
        //    txtBox = MyComplete;
        //    //reader.Close();
        //    conn.Close();

        //    return txtBox;

        //}
        ///// <summary>
        ///// genera autocomplete per un textbox
        ///// </summary>
        ///// <returns></returns>
        //public AutoCompleteStringCollection GetAutoComplete()
        //{
        //    AutoCompleteStringCollection txtBox = new AutoCompleteStringCollection();
        //    //OleDbCommand cmd = null;
        //    String select = "SELECT cognome FROM anagrafica1";

        //    //OleDbConnection conn = new OleDbConnection(ConnString);
        //    SqlConnection conn = new SqlConnection(ConnString);
        //    conn.Open();
        //    AutoCompleteStringCollection MyComplete = new AutoCompleteStringCollection();
        //    //cmd = new OleDbCommand(select, conn);
        //    SqlDataAdapter da;
        //    DataSet ds;

        //    da = new SqlDataAdapter(select, conn);
        //    ds = new DataSet();
        //    da.Fill(ds);

        //    //OleDbDataReader reader = cmd.ExecuteReader();
        //    //while (reader.Read())
        //    //{

        //    //    MyComplete.Add(reader.GetString(0));
        //    //}
        //    DataTable tb = ds.Tables[0];
        //    foreach (DataRow row in tb.Rows)
        //    {
        //        MyComplete.Add(Convert.ToString(row[0]));
        //    }
        //    txtBox = MyComplete;
        //    //reader.Close();
        //    conn.Close();

        //    return txtBox;

        //}
        //public AutoCompleteStringCollection GetAutoCompleteOperatore()
        //{
        //    AutoCompleteStringCollection txtBox = new AutoCompleteStringCollection();
        //    //OleDbCommand cmd = null;
        //    String sql = "SELECT operatore1, operatore2, operatore3, operatore4 FROM accertamenti";

        //    //OleDbConnection conn = new OleDbConnection(ConnString);
        //    //SqlConnection conn = new SqlConnection(ConnString);
        //    //conn.Open();

        //    AutoCompleteStringCollection MyComplete = new AutoCompleteStringCollection();
        //    //SqlDataAdapter da;
        //    //DataSet ds;
        //    //DataTable dt = new DataTable();
        //    //da = new SqlDataAdapter(select, conn);
        //    //ds = new DataSet();
        //    //da.Fill(ds);

        //    ////cmd = new OleDbCommand(select, conn);
        //    ////OleDbDataReader reader = cmd.ExecuteReader();
        //    //dt = ds.Tables[0];

        //    //DataTable tb = ds.Tables[0];
        //    DataTable tb = FillTable(sql);
        //    foreach (DataRow row in tb.Rows)
        //    {
        //        MyComplete.Add(Convert.ToString(row[0]));
        //    }
        //    txtBox = MyComplete;
        //    //reader.Close();
        //    //conn.Close();

        //    return txtBox;

        //}
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
            string sql = "SELECT MIN(id_quartiere) AS ID, quartiere FROM Quart GROUP BY quartiere ORDER BY quartiere";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getQuartiere(string indirizzo)
        {
            DataTable tb = new DataTable();
            string sql = "SELECT  * FROM Quart where toponimo like '%" + indirizzo + "%'";
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
        public DataTable getListOperatore()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT nominativo FROM operatore where nominativo <> '' order by nominativo ";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable getListProvvAg()
        {
            DataTable tb = new DataTable();
            string sql = "SELECT * FROM TipoNotaAG order by tipologia";
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
            string sql = "SELECT toponimo  FROM Quart order by toponimo";
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
            string sql = "SELECT * FROM Principale where Nr_Protocollo = " + protocollo + " and anno = '" + anno + "'";
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
            string sql = "SELECT * FROM Principale where ProcedimentoPen = " + procedimento;
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

            string sql = "SELECT * FROM Principale where EvasaData BETWEEN '" + dtda.ToShortDateString() + "' and '" + dta.ToShortDateString() + "'";
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



            string sql = "SELECT * FROM Principale where Rif_Prot_Gen = '" + protgen + "'";
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

            string sql = "SELECT * FROM Principale where giudice like '" + giudice + "%'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per provenienza
        /// </summary>
        /// <param name="provenienza"></param>
        /// <returns></returns>
        public DataTable getListProvenienza(string provenienza)
        {
            DataTable tb = new DataTable();

            string sql = "SELECT * FROM Principale where provenienza like '%" + provenienza + "%'";
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

            string sql = "SELECT * FROM Principale where nominativo like '" + nominativo + "%'";
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

            string sql = "SELECT * FROM Principale where indirizzo like '%" + indirizzo + "%'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        /// <summary>
        /// ricerca per data carico
        /// </summary>
        /// <param name="datacarico"></param>
        /// <returns></returns>
        public DataTable getListDataCarico(string datacaricoDa, string datacaricoA)
        {
            DataTable tb = new DataTable();
            DateTime dtda = System.Convert.ToDateTime(datacaricoDa);
            DateTime dta = System.Convert.ToDateTime(datacaricoA);

            string sql = "SELECT * FROM Principale where DataCarico BETWEEN '" + dtda.ToShortDateString() + "' and '" + dta.ToShortDateString() + "'";

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

            string sql = "SELECT * FROM Principale where accertatori like '" + accertatori + "%'";
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



            string sql = "SELECT * FROM Principale where nr_pratica = '" + pratica + "'";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                return tb = FillTable(sql, conn);
            }
        }
        public DataTable GetFileByFascicoloData(CaricaFile fl)
        {
            string sql = string.Empty;
            DataTable tb = new DataTable();
            if (!String.IsNullOrEmpty( fl.fascicolo) && !String.IsNullOrEmpty( fl.data))
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

        //public DataTable GetListAccertamenti()
        //{
        //    string sql = string.Empty;
        //    DataTable tb = new DataTable();

        //    sql = "SELECT * FROM accertamenti order by operatore1";

        //    return tb = FillTable(sql);

        //}
        //public DataTable GetListAccertamentiByOperatore(string operatore)
        //{
        //    string sql = string.Empty;
        //    DataTable tb = new DataTable();
        //    if (String.IsNullOrEmpty(operatore))
        //    {
        //        sql = "SELECT * FROM accertamenti order by operatore1";
        //    }
        //    else
        //    {
        //        sql = "SELECT * FROM accertamenti where operatore1 like '%" + operatore + "%' or operatore2 like '%"
        //           + operatore + "%' or operatore3 like '%" + operatore + "%' or operatore4 like '%" + operatore + "%'";
        //    }
        //    return tb = FillTable(sql);
        //    //SqlConnection conn = new SqlConnection(ConnString);
        //    //conn.Open();
        //    //SqlDataAdapter da;
        //    //DataSet ds;

        //    //da = new SqlDataAdapter(sql, conn);
        //    //ds = new DataSet();
        //    //da.Fill(ds);
        //    //tb = ds.Tables[0];
        //    //conn.Close();

        //    //return tb;
        //}
        //public DataTable GetListDipendenti()
        //{
        //    string sql = string.Empty;
        //    DataTable tb = new DataTable();

        //    sql = "SELECT * FROM Dipendenti order by dip_cognome";

        //    return tb = FillTable(sql);

        //}
        //public string GetDipendente(string mat)
        //{
        //    string sql = string.Empty;
        //    string dip = string.Empty;
        //    DataTable tb = new DataTable();

        //    sql = "SELECT dip_cognome FROM Dipendenti where dip_matricola = " + mat;
        //    tb = FillTable(sql);

        //    return dip = tb.Rows[0].ItemArray[0].ToString();

        //}
        //public DataTable GetData(string numeroPratica)
        //{
        //    DataTable tb = new DataTable();

        //    string sql = "SELECT * FROM accertamenti where num_pratica_accertamenti =" + numeroPratica;
        //    SqlConnection conn = new SqlConnection(ConnString);
        //    conn.Open();
        //    return tb = FillTable(sql);
        //    //SqlDataAdapter da;
        //    //DataSet ds;

        //    //da = new SqlDataAdapter(sql, conn);
        //    //ds = new DataSet();
        //    //da.Fill(ds);
        //    //tb = ds.Tables[0];
        //    //conn.Close();
        //    //return tb;
        //}
        //public DataTable GetListAna(string cognome, string numeroPratica)
        //{
        //    string sql = string.Empty;
        //    DataTable tb = new DataTable();
        //    if (!String.IsNullOrEmpty(cognome))
        //    {
        //        //sql = "SELECT * FROM anagrafica1 where cognome = '" + cognome + "'" + " order by cognome";
        //        sql = "SELECT * FROM anagrafica1 where cognome like '%" + cognome + "%'";
        //    }
        //    else
        //    {
        //        if (!String.IsNullOrEmpty(numeroPratica))
        //            //tb = FillTable("SELECT * FROM anagrafica1 where numero_pratica_accertamenti like '%" + numeroPratica + "%'");
        //            //tb = FillTable("SELECT * FROM anagrafica1 order by cognome");
        //            sql = "SELECT * FROM anagrafica1 where numero_pratica_accertamenti = '" + numeroPratica + "'";
        //        else
        //            sql = "SELECT * FROM anagrafica1 order by cognome";
        //    }
        //    return tb = FillTable(sql);

        //}
        //public DataTable GetListAccertamentiByPratica(string pratica)
        //{
        //    string sql = string.Empty;
        //    DataTable tb = new DataTable();
        //    if (!String.IsNullOrEmpty(pratica))
        //    {
        //        sql = "SELECT * FROM accertamenti where num_pratica_accertamenti = '" + pratica + "'";
        //        //tb = FillTable("SELECT * FROM accertamenti where num_pratica_accertamenti = '" + pratica + "'");
        //    }
        //    return tb = FillTable(sql);
        //    //SqlConnection conn = new SqlConnection(ConnString);
        //    //conn.Open();
        //    //SqlDataAdapter da;
        //    //DataSet ds;

        //    //da = new SqlDataAdapter(sql, conn);
        //    //ds = new DataSet();
        //    //da.Fill(ds);
        //    //tb = ds.Tables[0];
        //    //conn.Close();
        //    //return tb;
        //}
        ///// <summary>
        ///// statistiche
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetStatiscticheUote(Boolean suoloPub, Boolean lavEdili, Boolean contrCant, Boolean esposti)
        //{
        //    string sql = string.Empty;
        //    String a1 = String.Empty;

        //    a1 = suoloPub.ToString() + lavEdili.ToString() + contrCant.ToString() + esposti.ToString();
        //    switch (a1)
        //    {
        //        case "TrueTrueTrueTrue": //1111
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 1 and rapp_contr_lavori_edili = 1 and rapp_contr_cantieri_seq = 1" +
        //            " and rapp_contr_da_esposti = 1";
        //            break;
        //        case "TrueFalseFalseFalse": //1000
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 1 and rapp_contr_lavori_edili = 0 and rapp_contr_cantieri_seq = 0" +
        //                " and rapp_contr_da_esposti = 0";
        //            break;
        //        case "TrueTrueFalseFalse": //1100
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 1 and rapp_contr_lavori_edili = 1 and rapp_contr_cantieri_seq = 0" +
        //                " and rapp_contr_da_esposti = 0";
        //            break;
        //        case "TrueTrueTrueFalse": //1110
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 1 and rapp_contr_lavori_edili = 1 and rapp_contr_cantieri_seq = 1" +
        //                " and rapp_contr_da_esposti = 0";
        //            break;
        //        case "FalseFalseTrueTrue": //0011
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 0 and rapp_contr_lavori_edili = 0 and rapp_contr_cantieri_seq = 1" +
        //                " and rapp_contr_da_esposti = 1";
        //            break;
        //        case "FalseTrueTrueTrue": //0111
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 0 and rapp_contr_lavori_edili = 1 and rapp_contr_cantieri_seq = 1" +
        //                " and rapp_contr_da_esposti = 1";
        //            break;
        //        case "FalseTrueFalseFalse": //0100
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 0 and rapp_contr_lavori_edili = 1 and rapp_contr_cantieri_seq = 0" +
        //                " and rapp_contr_da_esposti = 0";
        //            break;
        //        case "FalseFalseTrueFalse": //0010
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 0 and rapp_contr_lavori_edili = 0 and rapp_contr_cantieri_seq = 1" +
        //                " and rapp_contr_da_esposti = 0";
        //            break;
        //        case "FalseFalseFalseTrue": //0001
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 0 and rapp_contr_lavori_edili = 0 and rapp_contr_cantieri_seq = 0" +
        //                " and rapp_contr_da_esposti = 1";
        //            break;
        //        case "TrueFalseFalseTrue": //1001
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 1 and rapp_contr_lavori_edili = 0 and rapp_contr_cantieri_seq = 0" +
        //                " and rapp_contr_da_esposti = 0";
        //            break;
        //        case "FalseTrueTrueFalse": //0110
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 0 and rapp_contr_lavori_edili = 1 and rapp_contr_cantieri_seq = 1" +
        //                " and rapp_contr_da_esposti = 0";
        //            break;
        //        case "TrueFalseTrueFalse": //1010
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 1 and rapp_contr_lavori_edili = 0 and rapp_contr_cantieri_seq = 1" +
        //                " and rapp_contr_da_esposti = 0";
        //            break;
        //        case "FalseFalseFalseFalse": //0000
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 0 and rapp_contr_lavori_edili = 0 and rapp_contr_cantieri_seq = 0" +
        //                " and rapp_contr_da_esposti = 0";
        //            break;
        //        case "FalseTrueFalseTrue": //0101
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 0 and rapp_contr_lavori_edili = 1 and rapp_contr_cantieri_seq = 0" +
        //                " and rapp_contr_da_esposti = 1";
        //            break;
        //        case "TrueTrueFalseTrue": //1101
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 1 and rapp_contr_lavori_edili = 1 and rapp_contr_cantieri_seq = 0" +
        //                " and rapp_contr_da_esposti = 1";
        //            break;
        //        case "TrueFalseTrueTrue": //1011
        //            sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 1 and rapp_contr_lavori_edili = 0 and rapp_contr_cantieri_seq = 1" +
        //                " and rapp_contr_da_esposti = 1";
        //            break;
        //        default:
        //            MessageBox.Show(a1, "Attenzione!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

        //            break;
        //    }
        //    DataTable tb = new DataTable();
        //    //if (suoloPub == true && lavEdili == true && contrCant == true && esposti == true )
        //    //{
        //    //    sql = "SELECT * FROM RappUote where rapp_contr_cantiere_suolo_pubb = 1 and rapp_contr_lavori_edili = 1 and rapp_contr_cantieri_seq = 1" + 
        //    //        " and rapp_contr_da_esposti = 1";

        //    //}



        //    return tb = FillTable(sql);
        //}
        ///// <summary>
        ///// ottiene lista schede intervento
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetListRappUote()
        //{
        //    string sql = string.Empty;
        //    DataTable tb = new DataTable();

        //    sql = "SELECT * FROM RappUote " + " order by rapp_numero_pratica";
        //    return tb = FillTable(sql);

        //}
        //public DataTable GetRappUoteByPratica(string pratica)
        //{
        //    string sql = string.Empty;
        //    DataTable tb = new DataTable();
        //    if (!String.IsNullOrEmpty(pratica))
        //    {
        //        sql = "SELECT * FROM RappUote where rapp_numero_pratica = '" + pratica + "'";

        //        //tb = FillTable("SELECT * FROM accertamenti where num_pratica_accertamenti = '" + pratica + "'");
        //    }
        //    return tb = FillTable(sql);

        //}
        //public DataTable GetGroupByNum(int num)
        //{
        //    DataTable tb = new DataTable();

        //    tb = FillTable("SELECT * FROM accertamenti order by nominativo where numero = " + num);

        //    //else
        //    //{
        //    //    tb = FillTable("SELECT * FROM accertamenti where matricola like '%" + matricola.ToUpper() + "%'");
        //    //}

        //    return tb;
        //}
        //public DataTable getAnagraficaById(String id)
        //{
        //    DataTable tb = new DataTable();
        //    SqlConnection conn = new SqlConnection(ConnString);
        //    conn.Open();
        //    string sql = "SELECT * FROM anagrafica1 where numero_pratica_accertamenti= '" + id + "'";
        //    return tb = FillTable(sql);

        //}
        //public DataTable getAccertamentoById(String id)
        //{
        //    DataTable tb = new DataTable();

        //    SqlConnection conn = new SqlConnection(ConnString);
        //    conn.Open();
        //    string sql = "SELECT * FROM accertamenti as acc inner join anagrafica1 as ana" +
        //        " on acc.num_pratica_accertamenti = ana.numero_pratica_accertamenti" +
        //        " where acc.num_pratica_accertamenti= '" + id + "' and acc.num_pratica_accertamenti = ana.numero_pratica_accertamenti";
        //    return tb = FillTable(sql);

        //}
        private DataTable FillTable(String sql, SqlConnection conn)
        {
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
                sql_pratica = "insert into Tipologia (Tipologia)" +
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
                sql_pratica = "insert into Tipologia (Tipologia)" +
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
                sql_pratica = "insert into inviata (inviata)" +
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
                sql_file = "insert into file_caricati (fascicolo, data,matricola, folder,nomefile)" +
                   " Values('" + @fl.fascicolo.Replace("'", "''") + "','" + @fl.data + "','" + fl.matricola.Replace("'", "''") + "','" + fl.folder.Replace("'", "''") +
                   "','" + fl.nomefile.Replace("'", "''") + "')";


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


        public Boolean InsRappUote(RappUote rapp)
        {
            bool resp = true;
            string sql_ins = String.Empty;

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                try
                {
                    sql_ins = "insert into RappUote (rapp_numero_pratica, rapp_data,	rapp_nominativo,rapp_indirizzo,rapp_pattuglia," +
                    "rapp_delegaAG,	rapp_resa,	rapp_segnalazione,	rapp_esposto,rapp_numEsposti,rapp_notifica,	rapp_iniziativa,rapp_comandante," +
                    "rapp_coordinatore,	rapp_relazione,	rapp_cnr,rapp_annotazionePG,rapp_verbale_seq,rapp_esito_delega,	rapp_contestaz_amm," +
                    "rapp_convalida,rapp_disseq_def,rapp_disseq_temp,rapp_disseq_temp_Rim,rapp_disseq_temp_Riapp,rapp_violazione_sigilli," +
                    "rapp_controlliScia,rapp_accert_avvenuto,rapp_totale,rapp_parziale,	rapp_violazioneBeniCult,rapp_contr_cantiere_suolo_pubb," +
                    "rapp_contr_lavori_edili,rapp_contr_cantieri_seq,rapp_contr_da_esposti,rapp_contr_da_segn,rapp_attivita_interna,rapp_nota,rapp_data_consegna_intervento, rapp_capopattuglia,rapp_uote,rapp_uotp,rapp_dataInserimento,rapp_con_protezioni,rapp_senza_protezioni,rapp_matricola)" +
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
                @rapp.uote + "','" + @rapp.uotp + "','" + @rapp.dataInserimento + "','" + @rapp.conProt + "','" + @rapp.senzaProt + "','" + rapp.matricola.Replace("'", "''") + "')";
                    command.CommandText = sql_ins;
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
                        sw.WriteLine("matricola:" + rapp.matricola + ",data ins:" + rapp.data + ", " + ex.Message + @" - Errore in inserimento scheda intervento uote ");
                        sw.Close();
                    }

                    resp = false;


                }
                conn.Close();
                return resp;
            }

        }
        //FINE INSERIMENTO
        public DataTable GetSchedeBy(string numPratica, string pattuglia, string dataI, Boolean attivita)
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
        public Boolean SavePratica(Principale p)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {

                sql_pratica = "insert into principale (nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen," +
                    "Nominativo,Indirizzo,Evasa,EvasaData,Inviata,DataInvio,Scaturito,Accertatori,DataCarico,nr_Pratica,Quartiere,Note,Anno,Giorno,Rif_Prot_Gen,matricola,DataInserimento)" +
                   " Values('" + @p.nrProtocollo + "','" + @p.sigla.Replace("'", "''") + "','" + @p.dataArrivo + "','" + @p.provenienza.Replace("'", "''") + "','" + @p.tipologia_atto.Replace("'", "''") +
                   "','" + @p.giudice.Replace("'", "''") + "','" + @p.tipoProvvedimentoAG.Replace("'", "''") + "','" + @p.procedimentoPen + "','" +
                   @p.nominativo.Replace("'", "''") + "','" + @p.indirizzo.Replace("'", "''") + "','" + @p.evasa + "','" + @p.evasaData + "','" + @p.inviata.Replace("'", "''") + "','" +
                   @p.dataInvio + "','" + @p.scaturito.Replace("'", "''") + "','" + @p.accertatori.Replace("'", "''") + "','" + @p.dataCarico + "','" + @p.nr_Pratica + "','" +
                    @p.quartiere.Replace("'", "''") + "','" + @p.note.Replace("'", "''") + "','" + @p.anno + "','" + @p.giorno + "','" + @p.rif_Prot_Gen + "','" + @p.matricola + "','" + @p.data_ins_pratica + "')";


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
                            sw.WriteLine("protocollo " + p.nrProtocollo + ", matricola:" + p.matricola + ", data ins:" + p.data_ins_pratica + ", " + ex.Message + @" - Errore in inserimento dati ");
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
                     "', rapp_matricola ='" + @rapp.matricola.Trim() + "'" +

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

        public Boolean UpdPratica(Principale p, string oldMat, DateTime olddate)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            try
            {
                sql_pratica = "update principale set Nominativo = '" + @p.nominativo.Replace("'", "''") + "',Indirizzo = '" + @p.indirizzo.Replace("'", "''") + "',via ='" + @p.via.Replace("'", "''") + "',Evasa='" + @p.evasa +
                    "',EvasaData = '" + @p.evasaData + "',Inviata = '" + @p.inviata.Replace("'", "''") + "',Scaturito = '" + @p.scaturito.Replace("'", "''") +
                    "',Accertatori = '" + @p.accertatori.Replace("'", "''") + "',DataCarico = '" + @p.dataCarico + "',Quartiere = '" + @p.quartiere.Replace("'", "''") +
                    "',Note ='" + @p.note.Replace("'", "''") + "',matricola = '" + @p.matricola + "',DataInserimento = '" + @p.data_ins_pratica + "'" +


                    " where Nr_protocollo = '" + @p.nrProtocollo + "' and datainserimento = '" + olddate + "' and matricola = '" + oldMat + "'";


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
        public Boolean SavePraticaTrans(Principale p, string oldMat, DateTime olddate, string oldProtocollo)
        {
            bool resp = true;
            string sql_pratica = String.Empty;
            string testoSql = string.Empty;

            //try
            //{
            sql_pratica = "insert into principale (nr_protocollo, sigla, DataArrivo, Provenienza, Tipologia_atto, giudice, TipoProvvedimentoAG, ProcedimentoPen," +
                "Nominativo,Indirizzo,via,Evasa,EvasaData,Inviata,DataInvio,Scaturito,Accertatori,DataCarico,nr_Pratica,Quartiere,Note,Anno,Giorno,Rif_Prot_Gen,matricola,DataInserimento)" +
               " Values('" + @p.nrProtocollo + "','" + @p.sigla.Replace("'", "''") + "','" + @p.dataArrivo + "','" + @p.provenienza.Replace("'", "''") + "','" + @p.tipologia_atto.Replace("'", "''") +
               "','" + @p.giudice.Replace("'", "''") + "','" + @p.tipoProvvedimentoAG.Replace("'", "''") + "','" + @p.procedimentoPen + "','" +
               @p.nominativo.Replace("'", "''") + "','" + @p.indirizzo.Replace("'", "''") + "','" + @p.via.Replace("'", "''") + "','" + @p.evasa + "','" + @p.evasaData + "','" + @p.inviata.Replace("'", "''") + "','" +
               @p.dataInvio + "','" + @p.scaturito.Replace("'", "''") + "','" + @p.accertatori.Replace("'", "''") + "','" + @p.dataCarico + "','" + @p.nr_Pratica + "','" +
                @p.quartiere.Replace("'", "''") + "','" + @p.note.Replace("'", "''") + "','" + @p.anno + "','" + @p.giorno + "','" + @p.rif_Prot_Gen + "','" + @p.matricola + "','" + @p.data_ins_pratica + "')";



            string del = "delete principale where nr_protocollo = '" + oldProtocollo +
                        "' and datainserimento = '" + olddate + "' and matricola = '" + oldMat + "'"; ;
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
                        command.CommandText = del;

                        command.ExecuteNonQuery();

                        tran.Commit();

                        resp = true;
                    }
                    else
                    {
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