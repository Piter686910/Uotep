case "accertatori":
    // Mostra voci per utenti standard
    menuCoordinamentoAtti.Visible = false;
    menuAccertatori.Visible = true;
    menuSegreteria.Visible = true;
    menuAmministratore.Visible = false;
    menuEsci.Visible = true;
    menuHome.Visible = true;
    PG.Visible = true;
    if (Session["profilo"].ToString() == "1")
    {
        menuNuovaScheda.Visible = true;
        menuRicercaScheda.Visible = true;
        Statistiche.Visible = false;
    }

    break;

