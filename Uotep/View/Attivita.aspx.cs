using DocumentFormat.OpenXml;
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
    public partial class _Attivita : Page
    {
        String Vuser = String.Empty;
        String area = String.Empty;
       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                area = Session["MacroArea"].ToString();

            }
            //if (!String.IsNullOrEmpty(ruolo))
            //{
            //    if (ruolo != "admin")
            //    {
            //        string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
            //        Response.Redirect(url, false);
            //       // Response.Redirect("~/View/default.aspx");
            //    }
            //}
            //if (!IsPostBack)
            //{
            //    Manager mn = new Manager();
            //    DataTable CaricaOperatori = mn.getListOperatore();
            //    DdlPersonale.DataSource = CaricaOperatori; // Imposta il DataSource della DropDownList
            //    DdlPersonale.DataTextField = "Nominativo"; // Il campo visibile
            //    DdlPersonale.Items.Insert(0, new ListItem("", "0"));
            //    DdlPersonale.DataBind();
            //}
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

        protected void gvPopup_RowDataBoundP(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "id").ToString();
                
                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";


               
            }
            if (GVAttivita.TopPagerRow != null && GVAttivita.TopPagerRow.Visible)
            {
                // Trova il controllo Label all'interno del PagerTemplate
                Label lblPageInfo = (Label)GVAttivita.TopPagerRow.FindControl("lblPageInfo");
                if (lblPageInfo != null)
                {
                    // Calcola e imposta il testo
                    int currentPage = GVAttivita.PageIndex + 1;
                    int totalPages = GVAttivita.PageCount;
                    lblPageInfo.Text = $"Pagina {currentPage} di {totalPages}";
                }
            }
        }
        protected void GVAttivita_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //GVRicercaPratica.PageIndex = e.NewPageIndex; // Imposta il nuovo indice di pagina
            //if (String.IsNullOrEmpty(HfFiltroNote.Value) && String.IsNullOrEmpty(HfFiltroIndirizzo.Value) && String.IsNullOrEmpty(HfFiltroResponsabile.Value))
            //{
            //    RicercaNew(sender, e);
            //}
            //else
            //{
            //    if (!String.IsNullOrEmpty(HfFiltroNote.Value))
            //    {
            //        PopulateGridView("arch_note", HfFiltroNote.Value);
            //        apripopupPratica_Click(sender, e);
            //    }
            //    else
            //    {
            //        if (!String.IsNullOrEmpty(HfFiltroIndirizzo.Value))
            //        {
            //            PopulateGridView("arch_indirizzo", HfFiltroIndirizzo.Value);
            //            apripopupPratica_Click(sender, e);
            //        }
            //        else
            //        {
            //            if (!String.IsNullOrEmpty(HfFiltroResponsabile.Value))
            //            {
            //                PopulateGridView("arch_responsabile", HfFiltroResponsabile.Value);
            //                apripopupPratica_Click(sender, e);
            //            }
            //        }
            //    }
            //}


        }
        protected void gvPopup_RowCommandP(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore dell'ID dalla CommandArgument
                //string selectedValue = e.CommandArgument.ToString();


                string[] args = e.CommandArgument.ToString().Split(';');
                int idP = System.Convert.ToInt32(args[0]);
                string Npratica = args[1];


                // Imposta il valore nel TextBox
                //txtSelectedValue.Text = selectedValue;
               // txtPratica.Text = Npratica;

                Manager mn = new Manager();
                //DataTable scheda = mn.GetScheda(txtPratica.Text.Trim(), txtNominativo.Text, LPattugliaCompleta.Items[0].Text);

                DataTable pratica = mn.getPraticaArchivioUoteById(idP);
                //if (pratica.Rows.Count > 0)
                //{
                //    FillScheda(pratica);

                //}
                //Session.Remove("ListRicerca");
                // Chiudi il popup
                ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
            }
        }

        protected void btAttivitaInCarico_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = mn.getAttivita(area, false);
            GVAttivita.DataSource = dt;
            GVAttivita.DataBind();
        }

        protected void btAttivitaConcluse_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            DataTable dt = mn.getAttivita(area,true);
            GVAttivita.DataSource = dt;
            GVAttivita.DataBind();
        }
    }
}