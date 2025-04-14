using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uotep.Classi
{
    public class Routine
    {


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