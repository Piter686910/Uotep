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
        DataTable profilo = new DataTable();
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
                            case "coordinamentoatti":
                                // Mostra voci specifiche per coordinamento ag
                                menuCoordinamentoAtti.Visible = true;
                                menuAccertatori.Visible = false;
                                menuAmministratore.Visible = false;
                                menuManTabelle.Visible = true;
                                menuSegreteria.Visible = false;
                                menuEsci.Visible = true;
                                menuHome.Visible = true;
                                if (Session["profilo"].ToString() != "1")
                                {
                                    menuNuovaScheda.Visible = false;
                                    menuRicercaScheda.Visible = true;
                                    Statistiche.Visible= true;

                                }
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;

                                break;
                            case "coordinamentopg":
                                // Mostra voci specifiche per coordinamento pg
                                menuCoordinamentoAtti.Visible = true;
                                menuAccertatori.Visible = true;
                                menuAmministratore.Visible = false;
                                menuManTabelle.Visible = true;
                                menuSegreteria.Visible = false;
                                menuEsci.Visible = true;
                                menuHome.Visible = true;
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
                                menuCoordinamentoAtti.Visible = false;
                                menuAccertatori.Visible = true;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = false;
                                menuEsci.Visible = true;
                                menuHome.Visible = true;

                                if (Session["profilo"].ToString() == "1")
                                {
                                    menuNuovaScheda.Visible = true;
                                    menuRicercaScheda.Visible = true;
                                    Statistiche.Visible = false;
                                }
                                break;
                            case "PG":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = false;
                                menuAccertatori.Visible = false;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = false;
                                menuEsci.Visible = true;
                                menuHome.Visible = true;
                                StatistichePg.Visible=true;
                                PG.Visible = true;
                                break;
                            case "archivio":
                                // Mostra voci per utenti standard
                                menuHome.Visible = true;
                                menuArchivioUote.Visible = true;
                                menuCoordinamentoAtti.Visible = false;
                                menuAccertatori.Visible = false;
                                menuSegreteria.Visible = false;
                                menuAmministratore.Visible = false;

                                menuEsci.Visible = true;
                                menuHome.Visible = true;
                                RicercaArchivio.Visible = true;
                                InserimentoArchivio.Visible = true;
                                break;
                            case "admin":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = true;
                                menuArchivioUote.Visible = true;
                                menuAccertatori.Visible = true;
                                menuNuovaScheda.Visible = true;
                                menuRicercaScheda.Visible = true;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = true;
                                menuManTabelle.Visible = true;
                                menuEsci.Visible = true;
                                menuHome.Visible = true;
                                RicercaArchivio.Visible = true;
                                InserimentoArchivio.Visible = true;
                                StatistichePg.Visible = true;
                                PG.Visible = true;


                                //*
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                Statistiche.Visible = true;
                                //*
                                break;
                            case "superAdmin":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = true;
                                menuArchivioUote.Visible = true;
                                menuAccertatori.Visible = true;
                                menuNuovaScheda.Visible = true;
                                menuRicercaScheda.Visible = true;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = false;
                                menuManTabelle.Visible = true;
                                menuEsci.Visible = true;
                                menuHome.Visible = true;
                                StatistichePg.Visible = true;
                                PG.Visible = true;

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
            Session.Remove("POP");
            Session.Remove("profilo");
            Session.Remove("ruolo");
            Session.Remove("ListRicerca");
            Session.Abandon();
            Response.Redirect("Default.aspx", false);
        }
    }
}