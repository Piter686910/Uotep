using AjaxControlToolkit.Bundling;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Runtime.Caching;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;


namespace Uotep
{
    public partial class _Default : Page
    {
        String Vuser = String.Empty;
        MemoryCache _cache = MemoryCache.Default;
        protected void Page_Load(object sender, EventArgs e)
        {
            String categoria = Request.QueryString["user"];
           

            if (categoria == "true")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Sessione scaduta, effettuare il login " + "'); $('#errorModal').modal('show');", true);
                Session.Abandon();

            }
            
            if (_cache != null)
                Vuser = _cache.Get("user") as string;
            if (Vuser != null)
            {
                Manager mn = new Manager();
                //Vuser = Session["user"].ToString();
                DataTable ricerca = mn.GetRuolo(Vuser);
                //Session["profilo"] = ricerca.Rows[0].ItemArray[0];
                //Session["ruolo"] = ricerca.Rows[0].ItemArray[1];
                //Session["area"] = ricerca.Rows[0].ItemArray[2];


                // 1. Inserire un parametro nella cache
                string chiaveP = "profilo";
                string valueP = ricerca.Rows[0].ItemArray[0].ToString();
                string chiaveR = "ruolo";
                string valueR = ricerca.Rows[0].ItemArray[1].ToString();
                string chiaveA = "area";
                string valueA = ricerca.Rows[0].ItemArray[2].ToString();
                // Imposta le opzioni della cache
                CacheItemPolicy policy = new CacheItemPolicy();
                // policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10); // Scadenza assoluta tra 10 minuti
                policy.SlidingExpiration = TimeSpan.FromMinutes(60); // Scadenza scorrevole (se non viene usata per 60 minuti, scade)


                // Aggiungi il parametro alla cache
                _cache.Add(chiaveP, valueP, policy);
                _cache.Add(chiaveR, valueR, policy);
                _cache.Add(chiaveA, valueA, policy);



                pnlLogin.Visible = false;
            }
            //if (Session["user"] != null)
            //{
            //    Manager mn=new Manager();
            //    Vuser = Session["user"].ToString();
            //    DataTable ricerca = mn.GetRuolo(Vuser);
            //    Session["profilo"] = ricerca.Rows[0].ItemArray[0];
            //    Session["ruolo"] = ricerca.Rows[0].ItemArray[1];
            //    Session["area"] = ricerca.Rows[0].ItemArray[2];
            //    pnlLogin.Visible = false;
            //}
        }


        protected void trova_Click(object sender, EventArgs e)
        {
            String Vpassw = "";
            Vuser = TxtMatricola.Text;
            Hmatricola.Value = TxtMatricola.Text;
            Vpassw = TxtPassw.Text;

            DataTable Ricerca = new DataTable();

            Manager mn = new Manager();

            //prendo la password registrata in db per la verifica
            DataTable RicercaP = mn.getPass(TxtMatricola.Text);
            if (RicercaP.Rows.Count > 0)
            {
                //esiste matricola e passw
                string pwDB = RicercaP.Rows[0].ItemArray[0].ToString();
                //verifico correttezza passw inserita
                Ricerca = mn.getUserByUserPassw(TxtMatricola.Text, pwDB);
                if (Ricerca.Rows.Count > 0)
                {
                    //verifico la correttezza della password criptata
                    string hashedPasswordSalvataNelDatabase = Ricerca.Rows[0].ItemArray[1].ToString();
                    bool isMatch = BCrypt.Net.BCrypt.Verify(Vpassw, hashedPasswordSalvataNelDatabase);
                    if (isMatch)
                    {
                        Boolean modifico = System.Convert.ToBoolean(Ricerca.Rows[0].ItemArray[7]);
                        if (modifico == false)
                        {
                            DivNewPassw.Visible = true;
                            btsave.Visible = true;
                            btLogin.Visible = false;
                            TxtPassw.Enabled = false;
                        }
                        else
                        {
                            //salvo la matricola
                        //    Session["user"] = Vuser;
                            
                            // 1. Inserire un parametro nella cache
                            string chiave = "user";
                            string value = Vuser;
                            // Imposta le opzioni della cache
                            CacheItemPolicy policy = new CacheItemPolicy();
                            // policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10); // Scadenza assoluta tra 10 minuti
                            policy.SlidingExpiration = TimeSpan.FromMinutes(60); // Scadenza scorrevole (se non viene usata per 60 minuti, scade)


                            // Aggiungi il parametro alla cache
                            _cache.Add(chiave, value, policy);

                            Response.Redirect("~/View/Default.aspx");
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Nome utente o password errati." + "'); $('#errorModal').modal('show');", true);
                        Session.Abandon();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "La matricola ineserita è inesistente." + "'); $('#errorModal').modal('show');", true);
                    Session.Abandon();
                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Nome utente o password errati." + "'); $('#errorModal').modal('show');", true);
                Session.Abandon();
            }

        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void SalvaPassw_Click(object sender, EventArgs e)
        {

            //cripto la passowrd
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(txtNewPassw.Text, 13);

            Manager mn = new Manager();
            Boolean ins = mn.SavePassword(passwordHash, TxtMatricola.Text);

            if (ins)
            {
                DivNewPassw.Visible = false;
                btsave.Visible = false;
                btLogin.Visible = true;
                TxtPassw.Enabled = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "password non salvata." + "'); $('#errorModal').modal('show');", true);
                Session.Abandon();
            }

        }

        protected void lkreset_Click(object sender, EventArgs e)
        {
            //cripto la passowrd
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(TxtMatricola.Text + "old", 13);
            Manager mn = new Manager();
            Boolean upd = mn.ResetPassw(passwordHash, TxtMatricola.Text);
            if (upd)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Password resettata. La nuova password temporanea è la tua matricola + old. Esempio: 9999old" + "'); $('#errorModal').modal('show');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Password non resettata." + "'); $('#errorModal').modal('show');", true);

            }
        }

      
        protected void btChiudiPop_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('errorModal')); modal.hide();", true);

            Response.Redirect("Default.aspx?user=false");
        }
    }
}