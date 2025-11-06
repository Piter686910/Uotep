using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uotep.Classi
{
    public class RappUote
    {
        public string pratica = string.Empty;
        //public DateTime ora;
        public DateTime data;
        public string quartiere = string.Empty;

        public string nominativo = string.Empty;
        public string indirizzo = string.Empty;
        public string pattuglia = string.Empty;
        public Boolean delegaAG = false;
        public Boolean resa = false;
        public Boolean segnalazione = false;
        public Boolean esposti = false;
        public string num_esposti = string.Empty;
        public Boolean notifica = false;
        public Boolean iniziativa = false;
        public Boolean cdr = false;
        public Boolean coordinatore = false;
        public Boolean relazione = false;
        public Boolean cnr = false;
        public Boolean annotazionePG = false;
        public Boolean verbaleSeq = false;
        public Boolean esitoDelega = false;
        public Boolean contestazioneAmm = false;
        public Boolean convalida = false;
        public Boolean dissequestroDef = false;
        public Boolean dissequestroTemp = false;
        public Boolean disseq_temp_Rim = false;
        public Boolean disseq_temp_Riapp = false;
        public Boolean rimozione = false;
        public Boolean riapposizione = false;
        public Boolean violazioneSigilli = false;
        public Boolean controlliScia = false;
        public Boolean accertAvvenutoRip = false;
        public Boolean totale = false;
        public Boolean parziale = false;
        public Boolean violazioneBeniCult = false;
        public Boolean contrCantSuoloPubb = false;
        public Boolean contrEdiliDPI = false;
        public Boolean contr_cantiereSeq = false;
        public Boolean contrDaEsposti = false;
        public Boolean contrDaSegn = false;
        public Boolean attività_interna = false;
        public string nota = string.Empty;
        public DateTime data_consegna_intervento;
        public string capopattuglia = string.Empty;
        public Boolean uote = false;
        public Boolean uotp = false;
        public DateTime dataInserimento;
        public Boolean conProt = false;
        public Boolean senzaProt = false;
        public string matricola = string.Empty;
        public Boolean non_avvenuto = false;
        public Boolean censimento_all_pubb = false;
        public Int32 num_censimento_all_pubb = 0;
        public Boolean contr_occupazione_abus = false;
        public Boolean contr_occ_abitativo = false;
        public Boolean contr_occ_no_abitativo = false;
        public Boolean sgomberi = false;
        public Boolean sgomberi_abus = false;
        public Boolean sgomberi_immobili = false;
        public Boolean notifica_no_ag = false;

    }
}