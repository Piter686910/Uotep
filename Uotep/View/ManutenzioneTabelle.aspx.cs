using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;

namespace Uotep
{
    public partial class ManutenzioneTabelle : Page
    {
        String Vuser = String.Empty;

        String Ruolo = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Vuser = Session["user"].ToString();
                Ruolo = Session["Ruolo"].ToString();

            }
            else
            {
                Response.Redirect("Default.aspx?user=true");
            }
            if (!IsPostBack)
            {
                if (Ruolo.ToUpper() != Enumerate.Ruolo.Archivio.ToString().ToUpper() && Ruolo.ToUpper() != Enumerate.Ruolo.Admin.ToString().ToUpper() && Ruolo.ToUpper() != Enumerate.Ruolo.SuperAdmin.ToString().ToUpper())
                {
                    btTipoAbuso.Enabled = false;

                }
                if (Ruolo.ToUpper() == Enumerate.Ruolo.Archivio.ToString().ToUpper())
                {
                    btScaturito.Enabled = false;
                    btTipologia.Enabled = false;
                    btTipologiaNotaAg.Enabled = false;
                    btGiudice.Enabled = false;
                    btInviati.Enabled = false;
                    btProvenienza.Enabled = false;

                }
            }
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

        protected void btGiudice_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivGiudice.Visible = true;
            Manager mn = new Manager();
            DataTable giudice = new DataTable();
            if (!string.IsNullOrEmpty(txtGiudice.Text))
            {
                giudice = mn.getListGiudice();
            }
            //if (giudice.Rows.Count > 0)
            //{
            //    gvPopupGiudice.DataSource = giudice;
            //    gvPopupGiudice.DataBind();
            //    DivGrid.Visible = true;
            //}
        }

        protected void btScaturito_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivScaturito.Visible = true;
        }

        protected void btTipologia_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivTipologia.Visible = true;
        }

        protected void btTipologiaNotaAg_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivTipologiaNotaAg.Visible = true;
        }

        protected void btInviati_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivInviati.Visible = true;
        }

        protected void btInserisci_Click(object sender, EventArgs e)
        {
            Manager mn = new Manager();
            if (!String.IsNullOrEmpty(txtGiudice.Text))
            {
                Boolean resp = mn.InserisciGiudice(txtGiudice.Text.ToUpper());
                if (resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Giudice: " + txtGiudice.Text + " inserito." + "'); $('#errorModal').modal('show');", true);

                }
            }
           
            if (!String.IsNullOrEmpty(txtInviati.Text))
            {
                Boolean resp = mn.InserisciInviata(txtInviati.Text.ToUpper());
                if (resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Giudice: " + txtInviati.Text + " inserito." + "'); $('#errorModal').modal('show');", true);

                }
            }
            if (!String.IsNullOrEmpty(txtScaturito.Text))
            {
                Boolean resp = mn.InserisciScaturito(txtScaturito.Text.ToUpper());
                if (resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Scaturito: " + txtScaturito.Text + " inserito." + "'); $('#errorModal').modal('show');", true);

                }
            }
            if (!String.IsNullOrEmpty(txtProvenienza.Text))
            {
                Boolean resp = mn.InserisciProvenienza(txtProvenienza.Text.ToUpper());
                if (resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Provenienza: " + txtProvenienza.Text + " inserito." + "'); $('#errorModal').modal('show');", true);

                }
            }
            if (!String.IsNullOrEmpty(txtTipologia.Text))
            {
                Boolean resp = mn.InserisciTipologia(txtTipologia.Text.ToUpper());
                if (resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Tipologia: " + txtTipologia.Text + " inserita." + "'); $('#errorModal').modal('show');", true);

                }
            }
            if (!String.IsNullOrEmpty(txtTipologiaNotaAg.Text))
            {
                Boolean resp = mn.InserisciTipologia(txtTipologiaNotaAg.Text.ToUpper());
                if (resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Tipologia Nota Ag: " + txtTipologiaNotaAg.Text + " inserita." + "'); $('#errorModal').modal('show');", true);

                }
            }

            if (!String.IsNullOrEmpty(txtTipoAbuso.Text))
            {
                Boolean resp = mn.InserisciTipologiaAbuso(txtTipoAbuso.Text.ToUpper());
                if (resp)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "Tipologia Abuso: " + txtTipoAbuso.Text + " inserita." + "'); $('#errorModal').modal('show');", true);

                }
            }
        }
        protected void NascondiDiv()
        {

            DivInviati.Visible = false;
            DivTipologia.Visible = false;
            DivTipologiaNotaAg.Visible = false;
            DivProvenienza.Visible = false;
            DivScaturito.Visible = false;
            DivGiudice.Visible = false;
            DivtipoAbuso.Visible = false;
        }

        protected void btProvenienza_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivProvenienza.Visible = true;
        }

        protected void gvPopupGiudice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Ottieni il valore del CommandArgument
                string commandArgument = e.CommandArgument.ToString();

                // Separare i valori del CommandArgument usando il delimitatore "|"
                string[] values = commandArgument.Split('|');

                // Assicurati che ci siano almeno 3 valori
                if (values.Length == 2)
                {
                    Int32 giudice = System.Convert.ToInt32(values[0]);    // giudice
                    string id = values[1];     // id



                    //Manager mn = new Manager();
                    //DataTable Dtgiudice = mn.getListGiudice();
                    //if (Dtgiudice.Rows.Count > 0)
                    //{
                    //    gvPopupGiudice.DataSource = Dtgiudice; 
                    //    gvPopupGiudice.DataBind();                       

                    //    // Puoi anche chiudere il popup se necessario
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "$('#myModal').modal('hide');", true);
                    //}
                    //else
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "modalScript", "$('#errorMessage').text('" + "giudice: " + giudice + " non trovato." + "'); $('#errorModal').modal('show');", true);

                    //}
                }
            }
        }

        protected void gvPopupGiudice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Ottieni il valore della colonna "ID"
                string id = DataBinder.Eval(e.Row.DataItem, "Giudice").ToString();

                // Aggiungi l'attributo per il doppio clic
                e.Row.Attributes["ondblclick"] = $"selectRow('{id}')";
                e.Row.Style["cursor"] = "pointer";
            }
        }

        protected void btTipoAbuso_Click(object sender, EventArgs e)
        {
            NascondiDiv();
            DivtipoAbuso.Visible = true;
        }
    }
}