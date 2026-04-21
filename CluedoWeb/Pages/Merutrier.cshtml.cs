using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CluedoWeb.Pages;

public class MeurtrierModel : PageModel
{
    private const string BonSuspect = "Mme Leblanc";
    private const int    BonPoids   = 75;
    private const int    ToleranceKg = 2; 
    [BindProperty]
    public int PoidsSaisi { get; set; }

    public bool   AccusationFaite  { get; set; }
    public bool   BonneAccusation  { get; set; }
    public string MessageFin       { get; set; } = string.Empty;

    public void OnGet() { }

    public IActionResult OnPostAccuser()
    {
        AccusationFaite = true;

        var ecart = Math.Abs(PoidsSaisi - BonPoids);

        if (ecart <= ToleranceKg)
        {
            BonneAccusation = true;
            MessageFin = $"Exact. {BonSuspect} — {BonPoids} kg, groupe B+, gauchère. L'affaire est résolue.";
        }
        else
        {
            BonneAccusation = false;
            MessageFin = PoidsSaisi < BonPoids - ToleranceKg
                ? $"{PoidsSaisi} kg — trop léger. Relisez les données de pression relevées sur la table."
                : $"{PoidsSaisi} kg — trop lourd. Ce poids ne correspond à aucun suspect compatible.";
        }

        return Page();
    }
}