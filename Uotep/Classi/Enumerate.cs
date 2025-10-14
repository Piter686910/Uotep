using System;
using System.ComponentModel;
using System.Reflection;

namespace Uotep.Classi
{
    public static class Enumerate
    {
        public enum Area
        {
            UOTE = 0,
            UOTP = 1
        }
        public enum Ruolo
        {
            Admin = 0,
            accertatori = 1,
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

        public enum Tipologie
        {
            [Description("Altro")]
            Altro = 1,

            [Description("DELEGA INDAGINE")]
            DelegaIndagine = 2,

            [Description("ESPOSTO - SEGNALAZIONE")]
            EspostoSegnalazione = 3                
                

        }
        public enum Sigla
        {
            AG = 1,
            ED = 2,
            TP = 3

        }
        public enum CampiXStatistiche
        {
            relazione = 0,
            espoEvasi = 1,
            ripTotPar = 2,
            contrScia = 3,
            notifiche = 4,
            cnr = 5,
            sequestri = 6,
            riappSigilli = 7,
            annotazioni = 8,
            delegheEsitate = 9,
            violAmm = 10,
            convalide = 11,
            dissequestri = 12,
            disseqTemp = 13,
            rimozSigilli = 14,
            violSigilli = 15,
            contr4204 = 16,
            ponteggi = 17,
            dpi = 18,
            contrCant = 19,
            contr_cant_suolo_pubb = 20


        }

        /// <summary>
        /// preleva la stringa da Enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;

            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));

            // Ritorna la descrizione se presente, altrimenti il nome del membro stesso.
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}