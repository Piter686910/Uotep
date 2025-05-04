using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uotep.Classi
{
    public class GestionePratiche
    {
        public String fascicolo = string.Empty;
        public String assegnato = string.Empty;
        public DateTime data_uscita;
        public DateTime data_rientro ;
        public DateTime data_spostamento;
        public DateTime data_riscontro_in_ufficio;
        public String note = string.Empty;
    }
}