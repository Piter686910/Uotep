using System;
using System.Data;
using System.Runtime.Caching;
using System.Web;
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
        string msg = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                ruolo = Session["ruolo"].ToString();

            }
            if (!String.IsNullOrEmpty(ruolo))
            {
                if (ruolo != "admin")
                {
                    string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
                    Response.Redirect(url, false);
                    // Response.Redirect("~/View/default.aspx");
                }
            }
            if (!IsPostBack)
            {
                Manager mn = new Manager();
                DataTable CaricaOperatori = mn.getListOperatore(out msg);
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
            divCheck.Visible = false;
            GVcheck.Visible = false;
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
            op.matricola = TxtMatricola.Text.ToUpper();
            //cripto la passowrd
            op.passw = BCrypt.Net.BCrypt.HashPassword(TxtMatricola.Text.ToUpper() + "old", 13);
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
        public void pulisci()
        {
            txtAnno.Text = string.Empty;
            txtPratica.Text = string.Empty;
            GVcheck.DataSource = null;
            GVcheck.DataBind();
            GVcheck.DataBind();
        }
        protected void NuovoUt_Click(object sender, EventArgs e)
        {
            divNewUtente.Visible = true;
            divDestra.Visible = true;
            divReset.Visible = false;
            divCheck.Visible = false;
            GVcheck.Visible = false;
            pulisci();

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

        protected void Check_Click(object sender, EventArgs e)
        {
            divCheck.Visible = true;
            divNewUtente.Visible = false;
            divDestra.Visible = false;
            divReset.Visible = false;
            GVcheck.Visible = true;
        }

        protected void Cerca_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = mn.getCheck(txtPratica.Text.Trim(), txtAnno.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                GVcheck.DataSource = dt;
                GVcheck.DataBind();
            }
        }

        protected void GVcheck_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Colonna 0: Data Accesso (es: 10/03/2026 14:30)
            //    e.Row.Cells[0].Width = Unit.Pixel(100);
            //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            //    // Colonna 1: Operatore (Nominativo lungo)
            //    e.Row.Cells[1].Width = Unit.Pixel(100);

            //    // Colonna 2: Account SQL
            //    e.Row.Cells[2].Width = Unit.Pixel(120);

            //    // Colonna 3: Indirizzo IP
            //    e.Row.Cells[3].Width = Unit.Pixel(110);
            //    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //}
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                // 1. Forza la tabella intera a essere fissa (non autofit)
                e.Row.TableSection = TableRowSection.TableBody;

                // 2. Colonna Data Accesso
                e.Row.Cells[0].Width = Unit.Pixel(100);
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[0].Attributes.Add("style", "border-left: 1px solid #dee2e6; white-space:nowrap;");

                // 3. Colonna Operatore
                e.Row.Cells[1].Width = Unit.Pixel(120); // Più spazio qui

                // 4. Colonna Account SQL
                e.Row.Cells[2].Width = Unit.Pixel(120);

                // 5. Colonna Indirizzo IP
                e.Row.Cells[3].Width = Unit.Pixel(120);
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].Attributes.Add("style", "border-right: 1px solid #dee2e6; white-space:nowrap;");
            }
        }

        protected void GVcheck_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GVcheck_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
    }
}