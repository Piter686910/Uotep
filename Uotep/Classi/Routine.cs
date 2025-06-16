using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Layout.Element;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        private void stampaX(float x, float y, Document document, Boolean X)
        {
            float boxSize = 10;
            float boxVerticalOffset = 5f;
            // Ottieni il PdfDocument e PdfCanvas
            PdfDocument pdfDocument = document.GetPdfDocument();
            PdfCanvas canvas = new PdfCanvas(pdfDocument.GetFirstPage());

            // --- Posizione esatta del riquadro (angolo inferiore sinistro) ---
            float xPosBox = x;
            float yPosBox = y - (boxSize / 2) + boxVerticalOffset;

            // --- Disegna il riquadro ---
            canvas.SetStrokeColor(ColorConstants.BLACK);
            canvas.SetLineWidth(0.8f);
            canvas.Rectangle(xPosBox, yPosBox, boxSize, boxSize).Stroke();

            // --- Posizione della "X" (centro del riquadro) ---
            float xPosText = xPosBox + (boxSize / 2);
            float yPosText = y;
            if (X == true)
            {


                // --- Aggiungi la "X" direttamente usando PdfCanvas.BeginText() ... EndText() ---
                canvas.BeginText()
                         .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 10) // Font e Dimensione
                         .SetColor(ColorConstants.BLACK, true)
                          .MoveText(xPosText - 2.5f, yPosText + 2.5f)
                         .ShowText("X")
                         .EndText();
            }
        }

        /// <summary>
        /// prepara la stampa del pdf
        /// </summary>
        /// <param name="schede"></param>
        public void CreaPdf(DataTable schede)
        {
            float boxSize = 10;
            //float boxVerticalOffset = 4f;
            float startX_270 = 270;
            float startX_290 = 290;
            float startX_70 = 70;
            float startX_55 = 55;
            float startX_50 = 50;
            float startX_400 = 400;
            float startX_350 = 350;
            float startX_370 = 370;
            float startX_450 = 450;
            float startX_470 = 470;
            float startY_430 = 430;

            float lineHeight = 15f;
            float lineHeight1 = 30f;
            float startY = 630;

            using (MemoryStream stream = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(stream))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        using (Document document = new Document(pdf))
                        {
                            // --- Creazione del Contenuto del Documento ---

                            // Titolo
                            DateTime dataIntervento = System.Convert.ToDateTime(schede.Rows[0].ItemArray[2].ToString());
                            string dataFormattata = dataIntervento.ToString("dd/MM/yyyy");

                            document.Add(new Paragraph($"Scheda Intervento del: {dataFormattata}")
                                .SetFixedPosition(70, 800, 400)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(14));

                            // Prima riga: Numero Pratica, Nominativo
                            document.Add(new Paragraph($"Numero Pratica: {schede.Rows[0].ItemArray[1]}").SetFixedPosition(70, 780, 200));
                            document.Add(new Paragraph($"Nominativo: {schede.Rows[0].ItemArray[3]}").SetFixedPosition(250, 780, 500));

                            // Seconda riga: Indirizzo, Data Consegna
                            document.Add(new Paragraph($"Indirizzo: {schede.Rows[0].ItemArray[4]}").SetFixedPosition(70, 760, 800));

                            DateTime dataConsegna = System.Convert.ToDateTime(schede.Rows[0].ItemArray[39].ToString());
                            string dataFormattataConsegna = dataConsegna.ToString("dd/MM/yyyy");

                            document.Add(new Paragraph($"Data Consegna: {dataFormattataConsegna}").SetFixedPosition(70, 740, 800));

                            // Terza riga: Capo pattuglia, pattuglia
                            document.Add(new Paragraph($"Capo Pattuglia: {schede.Rows[0].ItemArray[40]}").SetFixedPosition(70, 720, 200));
                            document.Add(new Paragraph($"Pattuglia: {schede.Rows[0].ItemArray[5]}").SetFixedPosition(70, 700, 600));
                            // Note
                            document.Add(new Paragraph($"Note: {schede.Rows[0].ItemArray[38]}").SetFixedPosition(70, 680, 450));
                            // FONTE INTERVENTO
                            document.Add(new Paragraph("FONTE INTERVENTO").SetFixedPosition(70, 650, 500).SetTextAlignment(TextAlignment.CENTER));
                            //riga interruzione sezione
                            float x = 65;
                            float y = 645;
                            float width = 490;

                            PdfCanvas canvas = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x, y) // Inizia la linea nel punto (x, y)
                                  .LineTo(x + width, y) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // Delega AG
                            bool delegaAG = Convert.ToBoolean(schede.Rows[0].ItemArray[6]);
                            string delegaAGString = delegaAG ? "X" : "";

                            // --- Posizione di riferimento INIZIALE per la X e il Riquadro (lato SINISTRO) ---
                            // float startX_70_DelegaAG = 70; // Posizione X iniziale SPECIFICA per "Delega AG"
                            float startY_DelegaAG = startY; // Use the dynamic startY

                            if (delegaAGString == "X")
                            {
                                stampaX(startX_50, startY_DelegaAG, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Delega AG:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_DelegaAG - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_DelegaAG, document, false);
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Delega AG:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_DelegaAG, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Resa
                            bool? resaNullable = schede.Rows[0].ItemArray[7] as bool?;
                            string resaString = resaNullable.HasValue && resaNullable.Value ? "X" : "";

                            // --- Posizione di riferimento per "Resa" ---
                            // float startX_70_Resa = 70; // Use startX_70 for single column
                            float startY_Resa = startY; // Use the dynamic startY


                            if (resaString == "X")
                            {
                                stampaX(startX_50, startY_Resa, document, true);

                                // --- Paragrafo per la descrizione "Resa:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Resa:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Resa - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Resa, document, false);
                                // --- Solo la descrizione "Resa:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Resa:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Resa, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Segnalazione
                            bool? segnalazioneNullable = schede.Rows[0].ItemArray[8] as bool?;
                            string segnalazioneString = segnalazioneNullable.HasValue && segnalazioneNullable.Value ? "X" : "";

                            // --- Posizione di riferimento per "Segnalazione" ---
                            //float startX_70_Segnalazione = 70; // Use startX_70 for single column
                            float startY_Segnalazione = startY; // Use the dynamic startY


                            if (segnalazioneString == "X")
                            {
                                stampaX(startX_50, startY_Segnalazione, document, true);

                                // --- Paragrafo per la descrizione "Segnalazione:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Segnalazione:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Segnalazione - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Segnalazione, document, false);
                                // --- Solo la descrizione "Segnalazione:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Segnalazione:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Segnalazione, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Esposto
                            bool? espostoNullable = schede.Rows[0].ItemArray[9] as bool?;
                            string espostoString = espostoNullable.HasValue && espostoNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "Esposto" ---

                            float startY_Esposto = startY; // Use the dynamic startY

                            if (espostoString == "X")
                            {
                                stampaX(startX_50, startY_Esposto, document, true);

                                // --- Paragrafo per la descrizione "Esposto:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Esposto:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Esposto - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Esposto, document, false);
                                // --- Solo la descrizione "Esposto:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Esposto:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Esposto, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Num. Esposto
                            // --- Posizione di riferimento per "Num. Esposto" ---

                            float startY_NumEsposto = startY_430; //


                            document.Add(new Paragraph($"Num. Esposto: {schede.Rows[0].ItemArray[10]}").SetFixedPosition(startX_270, startY_Esposto, 200));

                            startY -= lineHeight; // Move to the next line

                            // Notifica
                            bool? notificaNullable = schede.Rows[0].ItemArray[11] as bool?;
                            string notificaString = notificaNullable.HasValue && notificaNullable.Value ? "X" : "";

                            // --- Posizione di riferimento per "Notifica" ---
                            float startY_Notifica = startY; // Use the dynamic startY

                            if (notificaString == "X")
                            {
                                stampaX(startX_50, startY_Notifica, document, true);

                                // --- Paragrafo per la descrizione "Notifica:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Notifica:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Notifica - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Notifica, document, false);
                                // --- Solo la descrizione "Notifica:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Notifica:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Notifica, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line


                            // Iniziativa
                            bool? iniziativaNullable = schede.Rows[0].ItemArray[12] as bool?;
                            string iniziativaString = iniziativaNullable.HasValue && iniziativaNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "Iniziativa" ---

                            float startY_Iniziativa = startY; // Use the dynamic startY
                            if (iniziativaString == "X")
                            {
                                stampaX(startX_50, startY_Iniziativa, document, true);

                                // --- Paragrafo per la descrizione "Iniziativa:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Iniziativa:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Iniziativa - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Iniziativa, document, false);
                                // --- Solo la descrizione "Iniziativa:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Iniziativa:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Iniziativa, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // CDR
                            bool? cdrNullable = schede.Rows[0].ItemArray[13] as bool?;
                            string cdrString = cdrNullable.HasValue && cdrNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "CDR" ---

                            float startY_CDR = startY; // Use the dynamic startY
                            if (cdrString == "X")
                            {
                                stampaX(startX_50, startY_CDR, document, true);

                                // --- Paragrafo per la descrizione "CDR:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("CDR:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_CDR - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_CDR, document, false);
                                // --- Solo la descrizione "CDR:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("CDR:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_CDR, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line 470

                            // Coordinatore di turno
                            bool? coordinatorediturnoNullable = schede.Rows[0].ItemArray[14] as bool?;
                            string coordinatorediturnoString = coordinatorediturnoNullable.HasValue && coordinatorediturnoNullable.Value ? "X" : "";
                            // --- Posizione di riferimento per "Coordinatore di turno" ---
                            // float startX_70_Coord = 70; // Use startX_70 for single column
                            float startY_Coord = startY; // Use the dynamic startY 450
                            if (coordinatorediturnoString == "X")
                            {
                                stampaX(startX_50, startY_Coord, document, true);

                                // --- Paragrafo per la descrizione "Coordinatore:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Coordinatore di turno:");
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Coord - 5, 200);
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Coord, document, false);
                                // --- Solo la descrizione "Coordinatore:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Coordinatore di turno:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Coord, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            //riga interruzione sezione
                            float x1 = 65;
                            float y1 = startY;
                            float width1 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas1 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x1, y1) // Inizia la linea nel punto (x, y)
                                  .LineTo(x1 + width1, y1) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // ATTI REDATTI
                            document.Add(new Paragraph("ATTI REDATTI").SetFixedPosition(70, startY, 500).SetTextAlignment(TextAlignment.CENTER));

                            startY -= lineHeight; // Move to the next line
                            // Relazione
                            bool? relazioneNullable = schede.Rows[0].ItemArray[15] as bool?;
                            string relazioneString = relazioneNullable.HasValue && relazioneNullable.Value ? "X" : "";
                            //float startX_70_Relazione = 70; // Use startX_70 for single column
                            float startY_Relazione = startY; // Use the dynamic startY 


                            if (relazioneString == "X")
                            {
                                stampaX(startX_50, startY_Relazione, document, true);


                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Relazione:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Relazione - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Relazione, document, false);
                                // --- Solo la descrizione "relazione:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Relazione:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Relazione, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // CNR
                            bool? cnrNullable = schede.Rows[0].ItemArray[16] as bool?;
                            string cnrString = cnrNullable.HasValue && cnrNullable.Value ? "X" : "";
                            float startY_CNR = startY; // Use the dynamic startY
                            if (cnrString == "X")
                            {
                                stampaX(startX_50, startY_CNR, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("CNR:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_CNR - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_CNR, document, false);
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("CNR:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_CNR, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Annotazione PG
                            bool? annotazionepgNullable = schede.Rows[0].ItemArray[17] as bool?;
                            string annotazionepgString = annotazionepgNullable.HasValue && annotazionepgNullable.Value ? "X" : "";
                            float startY_AnnotazionePG = startY; // Use the dynamic startY
                            if (annotazionepgString == "X")
                            {
                                stampaX(startX_50, startY_AnnotazionePG, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Annotazione PG:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_AnnotazionePG - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_AnnotazionePG, document, false);
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Annotazione PG:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_AnnotazionePG, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Verbale Sequestro
                            bool? verbalesequestroNullable = schede.Rows[0].ItemArray[18] as bool?;
                            string verbalesequestroString = verbalesequestroNullable.HasValue && verbalesequestroNullable.Value ? "X" : "";
                            float startY_VerbaleSequestro = startY; // Use the dynamic startY
                            if (verbalesequestroString == "X")
                            {
                                stampaX(startX_50, startY_VerbaleSequestro, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Verbale Sequestro:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_VerbaleSequestro - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_VerbaleSequestro, document, false);
                                // --- Solo la descrizione "esito delega:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Verbale Sequestro:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_VerbaleSequestro, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Esito Delega
                            bool? esitodelegaNullable = schede.Rows[0].ItemArray[19] as bool?;
                            string esitodelegaString = esitodelegaNullable.HasValue && esitodelegaNullable.Value ? "X" : "";
                            float startY_EsitoDelega = startY; // Use the dynamic startY
                            if (esitodelegaString == "X")
                            {
                                stampaX(startX_50, startY_EsitoDelega, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Esito Delega:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_EsitoDelega - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_EsitoDelega, document, false);
                                // --- Solo la descrizione "esito Delega :", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Esito Delega:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_EsitoDelega, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Contestazione Amministrativa
                            bool? contestazioneamministrativaNullable = schede.Rows[0].ItemArray[20] as bool?;
                            string contestazioneamministrativaString = contestazioneamministrativaNullable.HasValue && contestazioneamministrativaNullable.Value ? "X" : "";
                            float startY_ContestazioneAmministrativa = startY; // Use the dynamic startY
                            if (contestazioneamministrativaString == "X")
                            {
                                stampaX(startX_50, startY_ContestazioneAmministrativa, document, true);

                                // --- Paragrafo per la descrizione "Delega AG:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Contestazione Amministrativa:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_DelegaAG + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_ContestazioneAmministrativa - 5, 200); // Usa startX_DelegaAG e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_ContestazioneAmministrativa, document, false);
                                // --- Solo la descrizione "Delega AG:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Contestazione Amministrativa:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_ContestazioneAmministrativa, 200); // Usa startX_DelegaAG and startY_DelegaAG
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            //riga interruzione sezione
                            float x2 = 65;
                            float y2 = startY;
                            float width2 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas2 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x2, y2) // Inizia la linea nel punto (x, y)
                                  .LineTo(x2 + width2, y2) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // PROVVEDIMENTI ADOTTATI E ATTIVITA' SVOLTE
                            document.Add(new Paragraph("PROVVEDIMENTI ADOTTATI E ATTIVITA' SVOLTE").SetFixedPosition(70, startY, 500).SetTextAlignment(TextAlignment.CENTER));
                            startY -= lineHeight; // Move to the next line
                            // Convalida
                            //bool? convalidaNullable = schede.Rows[0].ItemArray[21] as bool?;
                            //string convalidaString = convalidaNullable.HasValue && convalidaNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Convalida: {convalidaString}").SetFixedPosition(70, 470, 200));
                            bool? convalidaNullable = schede.Rows[0].ItemArray[21] as bool?;
                            string convalidaString = convalidaNullable.HasValue && convalidaNullable.Value ? "X" : "";
                            float startY_Convalida = startY; // Use the dynamic startY
                            if (convalidaString == "X")
                            {
                                stampaX(startX_50, startY_Convalida, document, true);


                                // --- Paragrafo per la descrizione "Convalida:", posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Convalida:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX_Convalida + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_Convalida - 5, 200); // Usa startX_Convalida e spazio
                                document.Add(descriptionParagraph);
                            }
                            else
                            {
                                stampaX(startX_50, startY_Convalida, document, false);
                                // --- Solo la descrizione "Convalida:", nella posizione originale ---
                                Paragraph descriptionParagraph = new Paragraph("Convalida:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_Convalida, 200); // Usa startX_Convalida
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Dissequestro Definitivo
                            bool? dissequestrodefinitivoNullable = schede.Rows[0].ItemArray[22] as bool?;
                            string dissequestrodefinitivoString = dissequestrodefinitivoNullable.HasValue && dissequestrodefinitivoNullable.Value ? "X" : "";
                            float startY_DissequestroDefinitivo = startY; // Use the dynamic startY
                            if (dissequestrodefinitivoString == "X")
                            {
                                stampaX(startX_50, startY_DissequestroDefinitivo, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Definitivo:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_DissequestroDefinitivo - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_DissequestroDefinitivo, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Definitivo:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_DissequestroDefinitivo, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line

                            // Violazione sigilli
                            bool? violazionesigilliNullable = schede.Rows[0].ItemArray[26] as bool?;
                            string violazionesigilliString = violazionesigilliNullable.HasValue && violazionesigilliNullable.Value ? "X" : "";
                            float startY_violazionesigilli = startY; // Use the dynamic startY
                            if (violazionesigilliString == "X")
                            {
                                stampaX(startX_50, startY_violazionesigilli, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Violazione sigilli:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_violazionesigilli - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_violazionesigilli, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Violazione sigilli:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_violazionesigilli, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Dissequestro Temporaneo
                            bool? dissequestrotemporaneoNullable = schede.Rows[0].ItemArray[23] as bool?;
                            string dissequestrotemporaneoString = dissequestrotemporaneoNullable.HasValue && dissequestrotemporaneoNullable.Value ? "X" : "";
                            float startY_dissequestrotemporaneo = startY; // Use the dynamic startY
                            if (dissequestrotemporaneoString == "X")
                            {
                                stampaX(startX_50, startY_dissequestrotemporaneo, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Temporaneo:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_dissequestrotemporaneo - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_dissequestrotemporaneo, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Dissequestro Temporaneo:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_dissequestrotemporaneo, 200);
                                document.Add(descriptionParagraph);
                            }

                            // Rimozione
                            bool? rimozioneNullable = schede.Rows[0].ItemArray[24] as bool?;
                            string rimozioneString = rimozioneNullable.HasValue && rimozioneNullable.Value ? "X" : "";
                            // float startY_Rimozione = startX_270; // Use the dynamic startY
                            if (rimozioneString == "X")
                            {
                                stampaX(startX_270, startY_dissequestrotemporaneo, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Rimozione:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_290 + boxSize + 5, startY_dissequestrotemporaneo - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_270, startY_dissequestrotemporaneo, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Rimozione:");
                                descriptionParagraph.SetFixedPosition(startX_290, startY_dissequestrotemporaneo, 200);
                                document.Add(descriptionParagraph);
                            }
                            // Riapposizione
                            bool? riapposizioneNullable = schede.Rows[0].ItemArray[25] as bool?;
                            string riapposizioneString = riapposizioneNullable.HasValue && riapposizioneNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Riapposizione: {riapposizioneString}").SetFixedPosition(400, 430, 80));
                            if (riapposizioneString == "X")
                            {
                                stampaX(startX_400, startY_dissequestrotemporaneo, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Riapposizione:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_450 + boxSize + 5, startY_dissequestrotemporaneo - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_400, startY_dissequestrotemporaneo, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Riapposizione:");
                                descriptionParagraph.SetFixedPosition(startX_450, startY_dissequestrotemporaneo, 100);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Violaz. Cod. Beni Culturali
                            bool? violazioniCodiciNullable = schede.Rows[0].ItemArray[31] as bool?;
                            string violazioniCodiciString = violazioniCodiciNullable.HasValue && violazioniCodiciNullable.Value ? "X" : "";
                            float startY_violazioniCodici = startY; // Use the dynamic startY
                            if (violazioniCodiciString == "X")
                            {
                                stampaX(startX_50, startY_violazioniCodici, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Violaz. Cod. Beni Culturali:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_violazioniCodici - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_violazioniCodici, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Violaz. Cod. Beni Culturali:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_violazioniCodici, 200);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Accertamento avvenuto ripristino
                            bool? accertamentoRipNullable = schede.Rows[0].ItemArray[28] as bool?;
                            string accertamentoRipString = accertamentoRipNullable.HasValue && accertamentoRipNullable.Value ? "X" : "";
                            float startY_accertamentoRip = startY; // Use the dynamic startY
                            if (accertamentoRipString == "X")
                            {
                                stampaX(startX_50, startY_accertamentoRip, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Accertamento avvenuto ripristino:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_accertamentoRip - 5, 200); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_accertamentoRip, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Accertamento avvenuto ripristino:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_accertamentoRip, 200);
                                document.Add(descriptionParagraph);
                            }
                            // Totale 
                            bool? totaleRipNullable = schede.Rows[0].ItemArray[29] as bool?;
                            string totaleRipString = totaleRipNullable.HasValue && totaleRipNullable.Value ? "X" : "";
                            // document.Add(new Paragraph($"Totale: {totaleRipString}").SetFixedPosition(270, 390, 100));
                            if (totaleRipString == "X")
                            {
                                stampaX(startX_270, startY_accertamentoRip, document, true);
                                //// Ottieni il PdfDocument e PdfCanvas

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Totale:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(280 + boxSize + 5, startY_accertamentoRip - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_270, startY_accertamentoRip, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Totale:");
                                descriptionParagraph.SetFixedPosition(280, startY_accertamentoRip, 100);
                                document.Add(descriptionParagraph);
                            }
                            // Parziale 
                            bool? parzialeRipNullable = schede.Rows[0].ItemArray[30] as bool?;
                            string parzialeRipString = parzialeRipNullable.HasValue && parzialeRipNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Parziale:  {parzialeRipString}").SetFixedPosition(350, 390, 100));
                            if (parzialeRipString == "X")
                            {
                                stampaX(startX_350, startY_accertamentoRip, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Parziale:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_370 + boxSize + 5, startY_accertamentoRip - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_350, startY_accertamentoRip, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Parziale:");
                                descriptionParagraph.SetFixedPosition(startX_370, startY_accertamentoRip, 100);
                                document.Add(descriptionParagraph);
                            }
                            // Non Avvenuto 
                            bool? NonAvvenutoRipNullable = schede.Rows[0].ItemArray[47] as bool?;
                            string NonAvvenutoRipString = NonAvvenutoRipNullable.HasValue && NonAvvenutoRipNullable.Value ? "X" : "";

                            if (NonAvvenutoRipString == "X")
                            {
                                stampaX(startX_450, startY_accertamentoRip, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Non Avvenuto:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_470 + boxSize + 5, startY_accertamentoRip - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_450, startY_accertamentoRip, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Non Avvenuto:");
                                descriptionParagraph.SetFixedPosition(startX_470, startY_accertamentoRip, 100);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controlli Scia
                            bool? sciaNullable = schede.Rows[0].ItemArray[27] as bool?;
                            string sciaString = sciaNullable.HasValue && sciaNullable.Value ? "X" : "";
                            float startY_scia = startY; // Use the dynamic startY
                            if (sciaString == "X")
                            {
                                stampaX(startX_50, startY_scia, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controlli Scia:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_scia - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_scia, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controlli Scia:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_scia, 100);
                                document.Add(descriptionParagraph);
                            }
                            //riga interruzione sezione
                            float x3 = 65;
                            float y3 = startY;
                            float width3 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas3 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x3, y3) // Inizia la linea nel punto (x, y)
                                  .LineTo(x3 + width3, y3) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight; // Move to the next line
                            // QUALIFICAZIONE INTERVENTO
                            document.Add(new Paragraph("QUALIFICAZIONE INTERVENTO").SetFixedPosition(70, startY, 500).SetTextAlignment(TextAlignment.CENTER));
                            startY -= lineHeight; // Move to the next line
                            // Controllo aree cantiere su suolo pubblico
                            bool? contrAreeNullable = schede.Rows[0].ItemArray[32] as bool?;
                            string contrAreeString = contrAreeNullable.HasValue && contrAreeNullable.Value ? "X" : "";
                            float startY_contrAree = startY; // Use the dynamic startY
                            if (contrAreeString == "X")
                            {
                                stampaX(startX_50, startY_contrAree, document, true);
                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo aree cantiere su suolo pubblico (impalcature):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrAree - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrAree, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo aree cantiere su suolo pubblico (impalcature):");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrAree, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controllo Cantiere
                            bool? contrSeqNullable = schede.Rows[0].ItemArray[34] as bool?;
                            string contrSeqString = contrSeqNullable.HasValue && contrSeqNullable.Value ? "X" : "";
                            float startY_contrSeq = startY; // Use the dynamic startY
                            if (contrSeqString == "X")
                            {
                                stampaX(startX_50, startY_contrSeq, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo Cantiere rientrano i controlli dei cantieri a sequestro:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrSeq - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrSeq, document, false);
                                // --- Solo la descrizione, nella posizione originale ---rel
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo Cantiere rientrano i controlli dei cantieri a sequestro:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrSeq, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controllo nato da esposti
                            bool? contrEspNullable = schede.Rows[0].ItemArray[35] as bool?;
                            string contrEspString = contrEspNullable.HasValue && contrEspNullable.Value ? "X" : "";
                            float startY_contrEsp = startY; // Use the dynamic startY
                            if (contrEspString == "X")
                            {
                                stampaX(startX_50, startY_contrEsp, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da esposti:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrEsp - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrEsp, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da esposti:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrEsp, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controllo nato da segnalazioni
                            bool? contrSegnNullable = schede.Rows[0].ItemArray[36] as bool?;
                            string contrSegnString = contrSegnNullable.HasValue && contrSegnNullable.Value ? "X" : "";
                            float startY_contrSegn = startY; // Use the dynamic startY
                            if (contrSegnString == "X")
                            {
                                stampaX(startX_50, startY_contrSegn, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da segnalazioni:");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrSegn - 5, 800); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_50, startY_contrSegn, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controllo nato da segnalazioni:");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrSegn, 800);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            // Controlli lavori edili con/senza protezione (d.p.i.)
                            bool? contrEdilNullable = schede.Rows[0].ItemArray[33] as bool?;
                            string contrEdilString = contrEdilNullable.HasValue && contrEdilNullable.Value ? "X" : "";
                            float startY_contrEdil = startY; // Use the dynamic startY
                            if (contrEdilString == "X")
                            {
                                stampaX(startX_50, startY_contrEdil, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Controlli lavori edili con/senza protezione (d.p.i.):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_55 + boxSize + 5, startY_contrEdil - 5, 600); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                                stampaX(startX_50, startY_contrEdil, document, false);
                            {
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Controlli lavori edili con/senza protezione (d.p.i.):");
                                descriptionParagraph.SetFixedPosition(startX_70, startY_contrEdil, 800);
                                document.Add(descriptionParagraph);
                            }

                            // Con (d.p.i.)
                            bool? contrConDpiNullable = schede.Rows[0].ItemArray[44] as bool?;
                            string contrConDpiString = contrConDpiNullable.HasValue && contrConDpiNullable.Value ? "X" : "";
                            //document.Add(new Paragraph($"Con  {contrConDpiString}").SetFixedPosition(350, 250, 70));
                            if (contrConDpiString == "X")
                            {
                                stampaX(startX_350, startY_contrEdil, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Con (d.p.i.):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_350 + boxSize + 5, startY_contrEdil - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_350, startY_contrEdil, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Con (d.p.i.):");
                                descriptionParagraph.SetFixedPosition(startX_370, startY_contrEdil, 100);
                                document.Add(descriptionParagraph);
                            }
                            // Senza (d.p.i.)
                            bool? contrSenzaDpiNullable = schede.Rows[0].ItemArray[45] as bool?;
                            string contrSenzaDpiString = contrSenzaDpiNullable.HasValue && contrSenzaDpiNullable.Value ? "X" : "";
                            // document.Add(new Paragraph($"Senza  {contrSenzaDpiString}").SetFixedPosition(450, 250, 70));
                            if (contrSenzaDpiString == "X")
                            {
                                stampaX(startX_450, startY_contrEdil, document, true);

                                // --- Paragrafo per la descrizione, posizionato *A DESTRA* del riquadro ---
                                Paragraph descriptionParagraph = new Paragraph("Senza (d.p.i.):");
                                // La descrizione inizia *dopo* la X e il riquadro: startX + boxSize + spazio
                                descriptionParagraph.SetFixedPosition(startX_470 + boxSize + 5, startY_contrEdil - 5, 100); // Spazio di 5 pixel tra riquadro e descrizione
                                document.Add(descriptionParagraph);

                            }
                            else
                            {
                                stampaX(startX_450, startY_contrEdil, document, false);
                                // --- Solo la descrizione, nella posizione originale ---
                                // La descrizione inizia a startX ora (senza X e riquadro a sinistra)
                                Paragraph descriptionParagraph = new Paragraph("Senza (d.p.i.):");
                                descriptionParagraph.SetFixedPosition(startX_470, startY_contrEdil, 100);
                                document.Add(descriptionParagraph);
                            }
                            startY -= lineHeight; // Move to the next line
                            //riga interruzione sezione
                            float x4 = 65;
                            float y4 = startY;
                            float width4 = 490;
                            // float height = 82;  non necessario per una linea orizzontale semplice
                            PdfCanvas canvas4 = new PdfCanvas(pdf.GetFirstPage());
                            canvas.MoveTo(x4, y4) // Inizia la linea nel punto (x, y)
                                  .LineTo(x4 + width4, y4) // Traccia la linea orizzontale fino a (x + width, y)
                                  .Stroke(); // Applica il tratto per rendere la linea visibile
                            startY -= lineHeight1; // Move to the next line
                            // La PG Operante - Sezione firma
                            document.Add(new Paragraph($"La PG Operante").SetFixedPosition(280, startY, 500));
                            startY -= lineHeight1; // Move to the next line
                            document.Add(new Paragraph($"_______________________").SetFixedPosition(260, startY, 500));
                            startY -= lineHeight1; // Move to the next line
                            document.Add(new Paragraph($"_______________________").SetFixedPosition(260, startY, 500));

                            document.Close(); // Chiude il documento.


                        }

                    }
                }

                // Invia l'output PDF direttamente al browser.
                byte[] pdfBytes = stream.ToArray();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/pdf";
                response.AddHeader("Content-Disposition", "inline; filename=SchedaIntervento.pdf");
                response.BinaryWrite(pdfBytes);
                response.Flush();
                response.End();
            }

        }
    }
}