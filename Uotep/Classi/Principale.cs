using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uotep.Classi
{
    public class Principale
    {
        public Int32 nrProtocollo = 0;
        public String sigla = string.Empty;
        public String dataArrivo = string.Empty;
        public String provenienza = string.Empty;
        public String tipologia_atto = string.Empty;
        public String giudice = string.Empty;
        public String tipoProvvedimentoAG = String.Empty;
        public String procedimentoPen = string.Empty;
        public String nominativo = string.Empty;
        public String indirizzo = string.Empty;
        public String via = string.Empty;
        public Boolean evasa;
        public String evasaData = string.Empty;
        public String inviata = string.Empty;
        public String dataInvio = string.Empty;
        public String scaturito = string.Empty;
        public String accertatori = string.Empty;
        public String dataCarico = string.Empty;
        public String nr_Pratica = string.Empty;
        public String quartiere = string.Empty;
        public String note = string.Empty;
        public String anno = string.Empty;
        public String giorno = string.Empty;
        public String rif_Prot_Gen = string.Empty;
        public String matricola = string.Empty;
        public DateTime data_ins_pratica;
        public String macro_area = string.Empty;
        public String ulterioreTipoAtto = string.Empty;
        public String bu = string.Empty;
        public String codiceEdificio = string.Empty;
        public String accertatori2 = string.Empty;
        public String accertatori3 = string.Empty;
        public Int32 NumProtRicStessoCarico = 0;
        public Boolean validato;
        //I 23/04/2026 controllo deleghe
        public String dataDelega = string.Empty;
        public int ggDelega = 0;
        //F 23/04/2026 controllo deleghe
        //I 22/05/2026 protocollo uscita
        public String rif_Prot_Uscita = string.Empty;
        //F 22/05/2026 protocollo uscita
    }
}