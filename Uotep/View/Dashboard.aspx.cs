using System;
using System.Data;
using System.Runtime.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;

namespace Uotep
{
    public partial class _Dashboard : Page
    {
        String Vuser = String.Empty;
        String ruolo = String.Empty;
        MemoryCache _cache = MemoryCache.Default;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_cache != null)
            {
                // 2. Recuperare un parametro dalla cache
                Vuser = _cache.Get("user") as string;
                ruolo = _cache.Get("ruolo") as string;
            }

            //if (user != null)
            //{
            //    Vuser = user;
            //    ruolo = ruolo1;

            //}
            //if (Session["user"] != null)
            //{
            //    Vuser = Session["user"].ToString();
            //    ruolo = Session["ruolo"].ToString();

            //}
            if (!String.IsNullOrEmpty(ruolo))
            {
                if (ruolo != "admin")
                {

                    Response.Redirect("~/View/default.aspx");
                }
            }
            if (!IsPostBack)
            {
                Manager mn = new Manager();
                DataTable CaricaOperatori = mn.getListOperatore();
                DdlPersonale.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
                DdlPersonale.DataTextField = "Nominativo"; // Il campo visibile
                DdlPersonale.Items.Insert(0, new ListItem("", "0"));
                DdlPersonale.DataBind();
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            divNewUtente.Visible = false;
            divDestra.Visible = false;
            divReset.Visible = true;
            
        }
        protected void ModificaP_Click(object sender, EventArgs e)
        {
            //cripto la passowrd
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(txtResetMatricola.Text + "old", 13);
            Manager mn = new Manager();
            Boolean upd = mn.ResetPassw(passwordHash, txtResetMatricola.Text);
            if (upd)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Password resettata." + "'); $('#errorModal').modal('show');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Password non resettata." + "'); $('#errorModal').modal('show');", true);

            }
        }
        protected void InsOpetratore_Click(object sender, EventArgs e)
        {
            Operatore op = new Operatore();
            op.matricola = TxtMatricola.Text;
            //cripto la passowrd
            op.passw = BCrypt.Net.BCrypt.HashPassword(TxtMatricola.Text + "old", 13);
            op.pwstandard = TxtMatricola.Text + "old";
            op.profilo = TxtProfilo.Text;
            op.nota = TxtNota.Text;
            op.ruolo = DdlRuolo.SelectedItem.Text;
            op.reset = System.Convert.ToBoolean("false");
            op.area = txtArea.Text.ToLower();
            op.macroarea = txtMacroArea.Text;
            op.nominativo = txtNominativo.Text;
            Manager mn = new Manager();
            Boolean ins = mn.InsOperatore(op);
            if (ins)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Operatore inserito correttamente." + "'); $('#errorModal').modal('show');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Operatore non inserito." + "'); $('#errorModal').modal('show');", true);

            }
        }
        protected void NuovoUt_Click(object sender, EventArgs e)
        {
            divNewUtente.Visible = true;
            divDestra.Visible = true;
            divReset.Visible = false;

        }
        protected void Login1_LoginError(object sender, EventArgs e)
        {

            // Mostra il modale con uno script
            ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#errorModal').modal('show');", true);
        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$('#myModal').modal('show');", true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('myModal')); modal.hide();", true);

        }

        protected void Elimina_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            Boolean del = mn.DeleteMatricola(txtResetMatricola.Text);
            if (del)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Matricola cancellata." + "'); $('#errorModal').modal('show');", true);

            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Matricola non trovata." + "'); $('#errorModal').modal('show');", true);
        }
    }
}