using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using Uotep.Classi;

namespace Uote
{
    public partial class test : System.Web.UI.Page
    {
        // Lista di suggerimenti (esempio)
        private static List<string> nomiSuggeriti = new List<string>()
        {
            "Alice",
            "Bob",
            "Charlie",
            "David",
            "Eve",
            "Federico",
            "Giorgio",
            "Luca",
            "Marco",
            "Paolo",
            "Pietro",
            "Giovanni",
            "Andrea",
            "Simone",
            "Giuseppe"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Manager mn = new Manager();
                DataTable RicercaGiudice = mn.getListGiudice();
                DdlGiudice.DataSource = RicercaGiudice; // Imposta il DataSource della DropDownList
                DdlGiudice.DataTextField = "Giudice"; // Il campo visibile
                DdlGiudice.DataValueField = "ID_giudice"; // Il valore associato a ogni opzione

                DdlGiudice.DataBind();
            }
        }
        

        protected void txtInput_TextChanged(object sender, EventArgs e)
        {
            string inputText = txtInput.Text;
            if (string.IsNullOrEmpty(inputText))
            {
                suggestionsList.InnerHtml = ""; // Pulisci la lista se l'input è vuoto
                suggestionsList.Style.Add("display", "none");
                return;
            }

            string testoInputLower = inputText.ToLower();

            List<string> suggerimentiFiltrati = nomiSuggeriti
                .Where(nome => nome.ToLower().StartsWith(testoInputLower))
                .ToList();

            if (suggerimentiFiltrati.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<ul>");
                foreach (var suggerimento in suggerimentiFiltrati)
                {
                    // Utilizzo di una funzione JavaScript per selezionare il suggerimento al click
                    sb.Append($"<li onclick=\"selezionaSuggerimento('{suggerimento}')\">{suggerimento}</li>");
                }
                sb.Append("</ul>");
                suggestionsList.InnerHtml = sb.ToString();
                suggestionsList.Style.Remove("display"); // Mostra la lista se ci sono suggerimenti
            }
            else
            {
                suggestionsList.InnerHtml = ""; // Pulisci la lista se non ci sono suggerimenti
                suggestionsList.Style.Add("display", "none"); // Nascondi la lista se non ci sono suggerimenti
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static List<string> GetSuggestions(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                return new List<string>();
            }

            string testoInputLower = inputText.ToLower();

            List<string> suggerimentiFiltrati = nomiSuggeriti
                .Where(nome => nome.ToLower().StartsWith(testoInputLower))
                .ToList();

            return suggerimentiFiltrati;
        }
    }
}