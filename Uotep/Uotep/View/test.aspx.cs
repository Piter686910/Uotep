using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Uote
{
    public partial class _test : Page
    {
        String Vuser = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();

            }
        }
        protected void btA_Click(object sender, EventArgs e)
        {

            // Crea un nuovo ListViewDataItem con un valore di esempio
            ListViewDataItem itemToAdd = new ListViewDataItem(0, 0);
            itemToAdd.DataItem = DdlPattuglia.SelectedItem.Text;

            // Verifica se l'elemento è già presente nella ListView
            bool itemExists = false;

            foreach (var item in LPattugliaCompleta.Items)
            {
                if (item.ToString() == itemToAdd.DataItem.ToString())
                {
                    itemExists = true;
                    break;
                }
            }

            // Aggiungi l'elemento solo se non esiste già
            if (!itemExists)
            {
                LPattugliaCompleta.Items.Add(DdlPattuglia.SelectedItem.Text);
            }

        }
        protected void CkAttivita_CheckedChanged1(object sender, EventArgs e)
        {
            //ToggleDivControls(divTesta, CkAttivita.Checked);
            //ToggleDivControls(divDettagli, CkAttivita.Checked);

        }
        protected void apripopup_Click(object sender, EventArgs e)
        {
            string script = @"$(document).ready(function(){$('body').append('<div class=\'modal fade\' id=\'myModal\' ...>...</div>'); " +
           " $('#myModal').modal('show');});";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowModal", script, true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#myModal').addClass('show').css('display', 'block'); $('body').addClass('modal-open'); $('<div class=\"modal-backdrop fade show\"></div>').appendTo('body');", true);

            //ScriptManager.RegisterStartupScript(this, GetType(), "Showpopup", "$('#myModal').modal('show')",true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "$(“#myModal”).show()");
            //ScriptManager.RegisterStartupScript(this, GetType(), "HideModal", "$('#myModal').modal('hide');", true);
            //string script = "var myModal = new bootstrap.Modal(document.getElementById('myModal')); myModal.show();";
            //ClientScript.RegisterStartupScript(this.GetType(), "ShowModalScript", script, true);
        }
        protected void chiudipopup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "$('#myModal').modal('hide');", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "var modal = bootstrap.Modal.getInstance(document.getElementById('PP')); modal.hide();", true);

        }
        protected void gvPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "ID").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
        }
        protected void RicercaQuartiere_Click(object sender, EventArgs e)
        {
            string indirizzo = string.Empty;


            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "inserire un indirizzo" + "');", true);
            //indirizzo = txtIndirizzo.Text.Trim();


            //string specie = txtSpecie.Text.Trim();

            //if (!string.IsNullOrEmpty(indirizzo))
            //{
            //    // Simula il recupero del quartiere dal database o da una logica interna.
            //    Manager mn = new Manager();
            //    DataTable quartiere = mn.getQuartiere(indirizzo);

            //    if (quartiere.Rows.Count > 0)
            //    {
            //        gvPopup.DataSource = quartiere;
            //        gvPopup.DataBind();

            //        //DdlQuartiere.Text = quartiere.Rows[0].ItemArray[0].ToString();
            //        //txtIndirizzo.Text = string.Empty;
            //        //txtSpecie.Text = string.Empty;
            //        //lblQuartiere.Text = $"Quartiere: {quartiere.Rows[0].ItemArray[0]}";
            //    }
            //    else
            //    {
            //        //lblQuartiere.Text = "Quartiere non trovato.";
            //    }
            //}
            //else
            //{
            //    //lblQuartiere.Text = "Inserisci un indirizzo valido.";
            //}

            // Mantieni il popup aperto dopo l'interazione lato server.
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showPopup", "openPopup();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowPopup", "showModal();", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "HideModal", "$('#myModal').modal('hide');", true);

            //DataTable Ricerca = mn.getUserByUserPassw(TxtMatricola.Text, TxtPassw.Text);
            //if (bUser == "admin")
            //{
            //    Response.Redirect("pagina_amministratore.aspx?user=" + Vuser + "&numord=" + VNumOrd + "", false);
            //    return;
            //}

            //if (Ricerca.Rows.Count > 0)
            //{
            //    var Rapportino = new Rapportino();


            //    Rapportino.mat = txt_Operatore.Text;

            //    Rapportino.Show();
            //    this.Close(); ;
            //}

        }
        //protected void gvPopup_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Select")
        //    {
        //        // Ottieni il valore dell'ID dalla CommandArgument
        //        string selectedValue = e.CommandArgument.ToString();

        //        // Imposta il valore nel TextBox
        //        //txtSelectedValue.Text = selectedValue;
        //        DdlQuartiere.SelectedItem.Text = selectedValue;
        //        // Chiudi il popup
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ClosePopup", "closeModal();", true);
        //    }
        //}
    }
}