using AjaxControlToolkit.Bundling;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Runtime.Caching;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;


namespace Uotep
{
    public partial class _Default : Page
    {
        String Vuser = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            String categoria = Request.QueryString["user"];


            if (categoria == "true")
            {
                // ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.SScaduta.GetDescription() + "'); $('#errorModal').modal('show');", true);

                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.SScaduta.GetDescription(), "danger");
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
                    Response.Redirect(url, false);
                    return;

                }

                Session.Abandon();

            }
            if (Session["user"] != null)
            {
                if (!String.IsNullOrEmpty(Session["user"].ToString()))
                {
                    Manager mn = new Manager();
                    Vuser = Session["user"].ToString();
                    DataTable ricerca = mn.GetRuolo(Vuser);
                    Session["profilo"] = ricerca.Rows[0].ItemArray[0];
                    Session["ruolo"] = ricerca.Rows[0].ItemArray[1];
                    Session["area"] = ricerca.Rows[0].ItemArray[2];
                    Session["MacroArea"] = ricerca.Rows[0].ItemArray[3];
                    pnlLogin.Visible = false;

                }
            }
        }


        protected void trova_Click(object sender, EventArgs e)
        {
            String Vpassw = "";
            Vuser = TxtMatricola.Text.ToUpper();
            Hmatricola.Value = TxtMatricola.Text.ToUpper();
            Vpassw = TxtPassw.Text;

            DataTable Ricerca = new DataTable();

            Manager mn = new Manager();

            //prendo la password registrata in db per la verifica
            DataTable RicercaP = mn.getPass(TxtMatricola.Text.ToUpper());


            if (RicercaP.Rows.Count > 0)
            {

                //esiste matricola e passw
                string pwDB = RicercaP.Rows[0].ItemArray[0].ToString();
                //verifico correttezza passw inserita
                Ricerca = mn.getUserByUserPassw(TxtMatricola.Text.ToUpper(), pwDB);
                if (Ricerca.Rows.Count > 0)
                {
                    //I 26/03/2026 - aggiunta variabile per abilitazione operatore
                    //verifico se abilitato a loggare
                    Boolean abilitato = System.Convert.ToBoolean(Ricerca.Rows[0]["abilitato"]);
                    if (abilitato == false)
                    {
                        SiteMaster myMaster = this.Master as SiteMaster;

                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.UserDisable.GetDescription(), "danger");
                            Session.Abandon();
                            return;
                        }
                    }
                    Boolean modifico = System.Convert.ToBoolean(Ricerca.Rows[0]["reset"]);
                    if (modifico == false)
                    {
                        DivNewPassw.Visible = true;
                        btsave.Visible = true;
                        btLogin.Visible = false;
                        TxtPassw.Enabled = false;
                        return;
                    }


                    //F 26/03/2026 - aggiunta variabile per abilitazione operatore
                    //verifico la correttezza della password criptata
                    string hashedPasswordSalvataNelDatabase = Ricerca.Rows[0].ItemArray[1].ToString();
                    bool isMatch = BCrypt.Net.BCrypt.Verify(Vpassw, hashedPasswordSalvataNelDatabase);
                    if (isMatch)
                    {
                        //Boolean modifico = System.Convert.ToBoolean(Ricerca.Rows[0].ItemArray[7]);
                        //if (modifico == false)
                        //{
                        //    DivNewPassw.Visible = true;
                        //    btsave.Visible = true;
                        //    btLogin.Visible = false;
                        //    TxtPassw.Enabled = false;
                        //}
                       // else
                      //  {
                            //salvo la matricola
                            Session["user"] = Vuser;

                            string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
                            Response.Redirect(url, false);
                            //Response.Redirect("~/View/Default.aspx");
                      //  }
                    }
                    else
                    {
                        SiteMaster myMaster = this.Master as SiteMaster;

                        if (myMaster != null)
                        {
                            // 2. Chiamo il metodo pubblico
                            myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.UserWrong.GetDescription(), "danger");
                            Session.Abandon();
                            return;
                        }


                        //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.UserWrong.GetDescription() + "'); $('#errorModal').modal('show');", true);

                    }
                }
                else
                {
                    SiteMaster myMaster = this.Master as SiteMaster;

                    if (myMaster != null)
                    {
                        // 2. Chiamo il metodo pubblico
                        myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.NoUser.GetDescription(), "danger");
                        Session.Abandon();
                        return;
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.NoUser.GetDescription() + "'); $('#errorModal').modal('show');", true);
                    //Session.Abandon();
                }

            }
            else
            {
                SiteMaster myMaster = this.Master as SiteMaster;

                if (myMaster != null)
                {
                    // 2. Chiamo il metodo pubblico
                    myMaster.MostraMessaggio("⚠️ ATTENZIONE", Enumerate.MsgOutput.UserWrong.GetDescription(), "danger");
                    Session.Abandon();
                    return;
                }
                //ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.UserWrong.GetDescription() + "'); $('#errorModal').modal('show');", true);
                //Session.Abandon();
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
            Boolean ins = mn.SavePassword(passwordHash, TxtMatricola.Text.ToUpper());

            if (ins)
            {
                DivNewPassw.Visible = false;
                btsave.Visible = false;
                btLogin.Visible = true;
                TxtPassw.Enabled = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.PwNoSave.GetDescription() + "'); $('#errorModal').modal('show');", true);
                Session.Abandon();
            }

        }

        protected void lkreset_Click(object sender, EventArgs e)
        {
            //cripto la passowrd
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(TxtMatricola.Text.ToUpper() + "old", 13);
            Manager mn = new Manager();
            Boolean upd = mn.ResetPassw(passwordHash, TxtMatricola.Text.ToUpper());
            if (upd)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + Enumerate.MsgOutput.PwResetOk.GetDescription() + "'); $('#errorModal').modal('show');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Password non resettata." + "'); $('#errorModal').modal('show');", true);

            }
        }


        protected void btChiudiPop_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('errorModal')); modal.hide();", true);
            string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx?user=false");
            Response.Redirect(url);
            //Response.Redirect("Default.aspx?user=false");
        }
    }
}