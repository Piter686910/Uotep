using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uotep.Classi;
using static Uotep.Classi.Enumerate;
using System.Runtime.Caching;
using Microsoft.Ajax.Utilities;

namespace Uotep
{
    public partial class SiteMaster : MasterPage
    {
        DataTable profilo = new DataTable();
        MemoryCache _cache = MemoryCache.Default;
        String user = string.Empty;
        String Profilo = string.Empty;
        String ruolo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["user"] != null)
                {
                    string Vuser = Session["user"].ToString();
                    string ruolo = Session["ruolo"].ToString();
                    Manager mn = new Manager();
                    DataTable Ricerca = mn.getUserRules(Vuser);
                    if (Ricerca.Rows.Count > 0)
                    {

                        switch (Ricerca.Rows[0].ItemArray[6].ToString())
                        {
                            case "coordinamentopg":
                            case "MasterAG":
                            case "coordinamentoatti":
                                // Mostra voci specifiche per coordinamento pg
                                menuCoordinamentoAtti.Visible = true;
                                menuArchivio.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;
                                RicercaArchivioUote.Visible = true;
                                menuAccertatori.Visible = true;
                                menuAmministratore.Visible = false;
                                menuManTabelle.Visible = true;
                                menuSegreteria.Visible = false;
                                menuEsci.Visible = true;
                                //menuHome.Visible = true;
                                if (Session["profilo"].ToString() != Enumerate.Profilo.accertatore.GetHashCode().ToString())
                                {
                                    menuNuovaScheda.Visible = false;
                                    menuRicercaScheda.Visible = true;
                                }
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                // ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                if (ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoAtti.ToString().ToUpper())
                                    StatisticheAtti.Visible = true;
                                if (ruolo.ToUpper() == Enumerate.Ruolo.CoordinamentoPg.ToString().ToUpper())
                                    EstraiStatistiche.Visible = true;

                                break;
                            case "accertatori":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = false;
                                menuAccertatori.Visible = true;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = false;
                                menuEsci.Visible = true;
                                // menuHome.Visible = true;
                                PG.Visible = true;
                                GestionePraticaUote.Visible = true;
                                if (Session["profilo"].ToString().Contains(Enumerate.Profilo.accertatore.GetHashCode().ToStringInvariant()))
                                {
                                    menuNuovaScheda.Visible = true;
                                    menuRicercaScheda.Visible = true;
                                    EstraiStatistiche.Visible = false;
                                }
                                menuArchivio.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;
                                RicercaArchivioUotp.Visible = true;

                                RicercaArchivioUote.Visible = true;
                                if (Session["profilo"].ToString().Contains(Enumerate.Profilo.tre.GetHashCode().ToStringInvariant()))
                                    Decretazione.Visible = true;

                                break;
                            case "PG":
                                // Mostra voci per utenti standard
                                //menuCoordinamentoAtti.Visible = false;
                                menuAccertatori.Visible = false;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = false;
                                menuEsci.Visible = true;
                                // menuHome.Visible = true;
                                StatistichePg.Visible = true;
                                PG.Visible = true;
                                menuArchivio.Visible = true;
                                RicercaArchivioUote.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;
                                RicercaArchivioUotp.Visible = true;

                                if (ruolo.ToUpper() == Enumerate.Ruolo.PG.ToString().ToUpper())
                                {
                                    menuCoordinamentoAtti.Visible = true;
                                    InserimentoAtti.Visible = false;
                                    ModificaAtti.Visible = false;
                                    RicercaAtti.Visible = true;
                                }
                                break;
                            case "archivio":
                                // Mostra voci per utenti standard
                                // menuHome.Visible = true;
                                menuArchivio.Visible = true;
                                menuCoordinamentoAtti.Visible = false;
                                menuAccertatori.Visible = false;
                                menuSegreteria.Visible = false;
                                menuAmministratore.Visible = false;

                                menuEsci.Visible = true;
                                // menuHome.Visible = true;
                                RicercaArchivioUote.Visible = true;
                                InserimentoArchivioUote.Visible = true;
                                GestionePraticaUote.Visible = true;
                                menuManTabelle.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;
                                InserimentoArchivioUotp.Visible = true;
                                RicercaArchivioUotp.Visible = true;

                                break;
                            case "admin":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = true;
                                menuArchivio.Visible = true;
                                menuAccertatori.Visible = true;
                                menuNuovaScheda.Visible = true;
                                menuRicercaScheda.Visible = true;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = true;
                                menuManTabelle.Visible = true;
                                menuEsci.Visible = true;
                                //  menuHome.Visible = true;
                                RicercaArchivioUote.Visible = true;
                                InserimentoArchivioUote.Visible = true;
                                StatistichePg.Visible = true;
                                PG.Visible = true;
                                StatisticheAtti.Visible = true;

                                //*
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                //  ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                EstraiStatistiche.Visible = true;
                                GestionePraticaUote.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;
                                InserimentoArchivioUotp.Visible = true;
                                RicercaArchivioUotp.Visible = true;
                                //*
                                break;
                            case "superAdmin":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = true;
                                menuArchivio.Visible = true;
                                menuAccertatori.Visible = true;
                                menuNuovaScheda.Visible = true;
                                menuRicercaScheda.Visible = true;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = false;
                                menuManTabelle.Visible = true;
                                menuEsci.Visible = true;
                                // menuHome.Visible = true;
                                StatistichePg.Visible = true;
                                PG.Visible = true;

                                //*
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                //ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                GestionePraticaUote.Visible = true;
                                subMenuUOTE.Visible = true;
                                subMenuUOTP.Visible = true;
                                InserimentoArchivioUotp.Visible = true;
                                RicercaArchivioUotp.Visible = true;

                                //*
                                break;
                            default:
                                // menuHome.Visible = true;
                                break;
                        }
                    }
                    //else
                    //    lblMsg.Text = "Matricola assente";
                }
                //else
                //    lblMsg.Text = "Utente non loggato";

            }
        }

        protected void Esci_Click(object sender, EventArgs e)
        {
            Session.Remove("user");
            Session.Remove("POP");
            Session.Remove("filetemp");
            Session.Remove("profilo");
            Session.Remove("ruolo");
            Session.Remove("ListRicerca");
            Session.Remove("popAperto");
            Session.Remove("popApertoRicercaScheda");
            Session.Remove("ListPratiche");
            Session.Abandon();
            string url = VirtualPathUtility.ToAbsolute("~/View/Default.aspx");
            Response.Redirect(url, false);
            //Response.Redirect(/Default.aspx"), false);
        }
    }
}