using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CluedoWeb.Pages;

public class AutopsieModel : PageModel
{
  
    public const int BonneFilole = 4;

    [BindProperty]
    public int FioleChoisie { get; set; }

    public bool FioleValidee { get; set; }
    public bool FioleEchouee { get; set; }
    public bool OuvrirFioles { get; set; }

    public string MessageFiole { get; set; } = string.Empty;

   
    public const string BonneLateralite = "gaucher";

    [BindProperty]
    public string LateraliteChoisie { get; set; } = string.Empty;

    public bool LateraliteValidee { get; set; }
    public bool LateraliteEchouee { get; set; }
    public bool OuvrirLateralite { get; set; }

    public string MessageLateralite { get; set; } = string.Empty;

  

    public void OnGet() { }

    public IActionResult OnPostValiderFiole()
    {
        OuvrirFioles = true;

        if (FioleChoisie == BonneFilole)
        {
            FioleValidee = true;
            MessageFiole = "Bonne analyse ! Le groupe sanguin B+ confirme l'identité de la victime.";
        }
        else
        {
            FioleEchouee = true;
            MessageFiole = $"Ce n'est pas le bon échantillon. La fiole n°{FioleChoisie} ne correspond pas au groupe sanguin de la victime.";
        }

        return Page();
    }

    public IActionResult OnPostValiderLateralite()
    {
        OuvrirLateralite = true;

        if (!string.IsNullOrWhiteSpace(LateraliteChoisie) &&
            LateraliteChoisie.Trim().ToLowerInvariant() == BonneLateralite)
        {
            LateraliteValidee = true;
            MessageLateralite = "Exactement. La trajectoire de la blessure, de droite à gauche, trahit un meurtrier gaucher.";
        }
        else
        {
            LateraliteEchouee = true;
            MessageLateralite = "Incorrect. Observez à nouveau l'angle et la direction de la plaie.";
        }

        return Page();
    }
}
