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
                    Manager mn = new Manager();
                    DataTable Ricerca = mn.getUserRules(Vuser);
                    if (Ricerca.Rows.Count > 0)
                    {

                        switch (Ricerca.Rows[0].ItemArray[6].ToString())
                        {

                            //case "coordinamentoatti":
                            //case "mastag":
                            //    // Mostra voci specifiche per coordinamento ag
                            //    menuCoordinamentoAtti.Visible = true;
                            //    menuArchivioUote.Visible = true;
                            //    RicercaArchivio.Visible = true;
                            //    menuAccertatori.Visible = false;
                            //    menuAmministratore.Visible = false;
                            //    menuManTabelle.Visible = true;
                            //    menuSegreteria.Visible = false;
                            //    menuEsci.Visible = true;
                            //    menuHome.Visible = true;
                            //    if (Session["profilo"].ToString() != "1")
                            //    {
                            //        menuNuovaScheda.Visible = false;
                            //        menuRicercaScheda.Visible = true;
                            //        Statistiche.Visible = true;

                            //    }
                            //    InserimentoAtti.Visible = true;
                            //    ModificaAtti.Visible = true;
                            //    ModificaRiservata.Visible = true;
                            //    RicercaAtti.Visible = true;

                            //    break;
                            case "coordinamentopg":
                            case "MasterAG":
                            case "coordinamentoatti":
                                // Mostra voci specifiche per coordinamento pg
                                menuCoordinamentoAtti.Visible = true;
                                menuArchivioUote.Visible = true;
                                RicercaArchivio.Visible = true;
                                menuAccertatori.Visible = true;
                                menuAmministratore.Visible = false;
                                menuManTabelle.Visible = true;
                                menuSegreteria.Visible = false;
                                menuEsci.Visible = true;
                                menuHome.Visible = true;
                                if (Profilo != Enumerate.Profilo.accertatore.GetHashCode().ToString())
                                {
                                    menuNuovaScheda.Visible = false;
                                    menuRicercaScheda.Visible = true;
                                }
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                ModificaRiservata.Visible = true;
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
                                menuHome.Visible = true;
                                PG.Visible = true;
                                GestionePratica.Visible = true;
                                if (Profilo == Enumerate.Profilo.accertatore.GetHashCode().ToString())
                                {
                                    menuNuovaScheda.Visible = true;
                                    menuRicercaScheda.Visible = true;
                                    EstraiStatistiche.Visible = false;
                                }
                                menuArchivioUote.Visible = true;
                                RicercaArchivio.Visible = true;

                                break;
                            case "PG":
                                // Mostra voci per utenti standard
                                menuCoordinamentoAtti.Visible = false;
                                menuAccertatori.Visible = false;
                                menuSegreteria.Visible = true;
                                menuAmministratore.Visible = false;
                                menuEsci.Visible = true;
                                menuHome.Visible = true;
                                StatistichePg.Visible = true;
                                PG.Visible = true;
                                menuArchivioUote.Visible = true;
                                RicercaArchivio.Visible = true;

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
                                GestionePratica.Visible = true;
                                menuManTabelle.Visible = true;
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
                                StatisticheAtti.Visible = true;

                                //*
                                InserimentoAtti.Visible = true;
                                ModificaAtti.Visible = true;
                                ModificaRiservata.Visible = true;
                                RicercaAtti.Visible = true;
                                EstraiStatistiche.Visible = true;
                                GestionePratica.Visible = true;
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
                                GestionePratica.Visible = true;
                                //*
                                break;
                            default:
                                menuHome.Visible = true;
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
            // Session.Remove("user");
            Session.Remove("POP");
            Session.Remove("filetemp");
            // Session.Remove("profilo");
            // Session.Remove("ruolo");
            Session.Remove("ListRicerca");
            Session.Remove("popAperto");
            Session.Remove("popApertoRicercaScheda");
            // Ottieni tutti gli elementi della cache
            var cacheKeys = _cache.Select(kvp => kvp.Key).ToList();

            // Rimuovi ogni elemento
            foreach (string cacheKey in cacheKeys)
            {
                _cache.Remove(cacheKey);
            }
            Session.Abandon();
            Response.Redirect("Default.aspx", false);
        }
    }
}