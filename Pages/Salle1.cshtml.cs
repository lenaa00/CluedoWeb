using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Text;

namespace CluedoWeb.Pages;

public class Salle1Model : PageModel
{
    [BindProperty]
    public string Reponse { get; set; } = string.Empty;

    [BindProperty]
    public string Mot1 { get; set; } = string.Empty;

    [BindProperty]
    public string Mot2 { get; set; } = string.Empty;

    [BindProperty]
    public string Mot3 { get; set; } = string.Empty;

    [BindProperty]
    public string Mot4 { get; set; } = string.Empty;

    public bool Reussi { get; set; }

    public bool EnigmeReussie { get; set; }

    public bool OuvrirTapis { get; set; }

    public string MessageEnigme { get; set; } = string.Empty;

    public IReadOnlyList<TextToken> LetterTokens => ConstruireTokens();

    public Dictionary<string, bool?> ResultatsMots { get; } = new();

    public void OnGet()
    {
    }

    public void OnPost()
    {
        string solution = "tu vas payer pour le mal que tu m as fais";

        if (!string.IsNullOrEmpty(Reponse) && Reponse.ToLower().Trim() == solution)
        {
            Reussi = true;
        }
    }

    public void OnPostEnigmeLettre()
    {
        OuvrirTapis = true;

        bool mot1Ok = MotCorrect(Mot1, "payer");
        bool mot2Ok = MotCorrect(Mot2, "mal");
        bool mot3Ok = MotCorrect(Mot3, "planter");
        bool mot4Ok = MotCorrect(Mot4, "talon");

        ResultatsMots[nameof(Mot1)] = mot1Ok;
        ResultatsMots[nameof(Mot2)] = mot2Ok;
        ResultatsMots[nameof(Mot3)] = mot3Ok;
        ResultatsMots[nameof(Mot4)] = mot4Ok;

        if (mot1Ok && mot2Ok && mot3Ok && mot4Ok)
        {
            EnigmeReussie = true;
            MessageEnigme = "Bravo ! Tu as reconstitue les mots de la lettre.";
        }
        else
        {
            MessageEnigme = "Certains mots ne sont pas encore corrects.";
        }
    }

    private IReadOnlyList<TextToken> ConstruireTokens()
    {
        return
        [
            new("Tu"),
            new("vas"),
            new("ypear", nameof(Mot1), Mot1, EtatMot(nameof(Mot1))),
            new("pour"),
            new("le"),
            new("aml", nameof(Mot2), Mot2, EtatMot(nameof(Mot2))),
            new("que"),
            new("tu"),
            new("m'as"),
            new("fais"),
            new("!"),
            new("Je"),
            new("n'hesiterais"),
            new("pas"),
            new("a"),
            new("te"),
            new("ltarepn", nameof(Mot3), Mot3, EtatMot(nameof(Mot3))),
            new("mon"),
            new("lanot", nameof(Mot4), Mot4, EtatMot(nameof(Mot4))),
            new("dans"),
            new("le"),
            new("dos.")
        ];
    }

    private bool? EtatMot(string champ)
    {
        return ResultatsMots.TryGetValue(champ, out bool? resultat) ? resultat : null;
    }

    private static bool MotCorrect(string valeur, string solution)
    {
        return Normaliser(valeur) == Normaliser(solution);
    }

    private static string Normaliser(string texte)
    {
        string normalise = texte.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var resultat = new StringBuilder();

        foreach (char caractere in normalise)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(caractere) != UnicodeCategory.NonSpacingMark)
            {
                resultat.Append(caractere);
            }
        }

        return resultat.ToString().Normalize(NormalizationForm.FormC);
    }
}

public sealed record TextToken(string Texte, string Champ = "", string Valeur = "", bool? EstCorrect = null)
{
    public bool EstMotMelange => !string.IsNullOrWhiteSpace(Champ);
}
