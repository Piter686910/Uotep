using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;

namespace Uotep
{
    public partial class SiteMaster : MasterPage
    {
        DataTable profilo= new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["user"] != null)
                {
                    string Vuser = Session["user"].ToString();
                    Manager mn = new Manager();
                    DataTable Ricerca = mn.getUserRules(Vuser);
                    if (Ricerca.Rows.Count > 0)
                    {
                        switch (Ricerca.Rows[0].ItemArray[6].ToString())
                        {
                            case "coordinamento atti":
                                // Mostra voci specifiche per coordinamento ag
                                menuCoordinamentoAtti.Visible = true;
                                menuAccertatori.Visible = false;
                                menuAmministratore.Visible = false;
                                menuManTabelle.Visible = true;
                                menuFureria.Visible = false;
                                if (Session["profilo"].ToString() != "1")
                                {
                                    menuNuovaScheda.Visible = false;
                                    menuRicercaScheda.Visible = true;
                                }
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                
                                break;
                            case "coordinamento pg":
                                // Mostra voci specifiche per coordinamento pg
                                menuCoordinamentoAtti.Visible = true;
                                menuAccertatori.Visible = true;
                                menuAmministratore.Visible = false;
                                menuManTabelle.Visible = true;
                                menuFureria.Visible = false;
                                if (Session["profilo"].ToString() != "1")
                                {
                                    menuNuovaScheda.Visible = false;
                                    menuRicercaScheda.Visible = true;
                                }
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                break;
                            case "accertatori":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = true;
                                menuAccertatori.Visible = true;
                                menuFureria.Visible = false;
                                menuAmministratore.Visible = false;

                                if (Session["profilo"].ToString() == "1")
                                {
                                    menuNuovaScheda.Visible = true;
                                    menuRicercaScheda.Visible = true;
                                }
                                break;
                            case "admin":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = true;
                                menuAccertatori.Visible = true;
                                menuNuovaScheda.Visible = true;
                                menuRicercaScheda.Visible = true;
                                menuFureria.Visible = true;
                                menuAmministratore.Visible = true;
                                menuManTabelle.Visible = true;

                                //*
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                //*
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
        }

        protected void Esci_Click(object sender, EventArgs e)
        {
            Session.Remove("user");
            Session.Remove("profilo");
            Session.Remove("ruolo");
            Session.Abandon();
            Response.Redirect("Default.aspx", false);
        }
    }
}