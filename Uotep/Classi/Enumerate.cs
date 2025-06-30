namespace Uotep.Classi
{
    public class Enumerate
    {
        public enum Area
        {
            UOTE = 0,
            UOTP = 1
        }
        public enum Ruolo
        {
            Admin = 0,
            Accertatore = 1,
            MasterAG = 2,
            Archivio = 3,
            CoordinamentoAtti = 4,
            PG = 5,
            SuperAdmin = 6,
            CoordinamentoPg = 7
        }
        public enum Profilo
        {
            accertatore = 1, //accertatori
            due = 2,
            tre = 3, //admin, responsabili, sa

        }
    }
}