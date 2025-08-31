using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uotep.Classi
{
    public class Decretazione
    {
        public Int32 idPratica = 0;
        public String Npratica = string.Empty;
        public String decretante = string.Empty;
        public String decretato = string.Empty;
        public DateTime data;
        public String nota = string.Empty;
        public DateTime dataChiusura;
        public Boolean chiuso;
    }
}