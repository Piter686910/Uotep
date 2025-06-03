using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Uotep.Classi
{
    public class Routine
    {

        public string GetProtocollo()
        {
            string txt= string.Empty;
            String annoCorr = DateTime.Now.Year.ToString();
            int protocollo = 0;
            Manager mn = new Manager();
            //DataTable tb = mn.MaxNPr(annoCorr);
            DataTable tb = mn.MaxNPr(annoCorr);
            if (tb.Rows.Count > 0)
            {
                //txtDataArrivo.Text = DateTime.Now.Date.ToShortDateString();
                int annoMAx = System.Convert.ToInt16(tb.Rows[0].ItemArray[0]);

                if (System.Convert.ToInt16(annoCorr) <= annoMAx)
                {
                    protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[1]) + 1;
                    txt = protocollo.ToString();//tb.Rows[0].ItemArray[1].ToString();
                }
                else
                {
                    protocollo = System.Convert.ToInt16(tb.Rows[0].ItemArray[1]) + 1;
                    txt = protocollo.ToString();

                }
            }
            else
            {
                txt = "1";

            }
            return txt;
        }
        public void ConvertiBooleaniInItaliano(IXLWorksheet ws)
        {
            IXLWorksheet worksheet = ws;
            //sostituisce booleani con stringa si o no
            int lastRow = worksheet.LastRowUsed().RowNumber();
            int lastColumn = worksheet.LastColumnUsed().ColumnNumber();
            for (int row = 2; row <= lastRow; row++)
            {
                // Itera su tutte le colonne nella riga corrente
                for (int column = 1; column <= lastColumn; column++)
                {
                    IXLCell cell = worksheet.Cell(row, column); // Ottieni la cella corrente
                    XLDataType tipoDatoD1 = cell.DataType;
                    string valore = string.Empty;
                    if (tipoDatoD1 is XLDataType.Boolean)
                        valore = System.Convert.ToString(((bool)cell.Value));

                    if (valore == "True")
                    {

                        cell.Value = "SI"; // Sostituisci "true" (stringa) con "si"
                    }
                    else if (valore == "False")
                    {
                        cell.Value = "NO"; // Sostituisci "false" (stringa) con "no"
                    }
                }
            }
        }
    }
}