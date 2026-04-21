using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CluedoWeb.Pages;

public class SalleArmesModel : PageModel
{
    public const int BonneArme = 2;

    [BindProperty]
    public int ArmeChoisie { get; set; }

    public bool ArmeValidee    { get; set; }
    public bool ArmeEchouee    { get; set; }
    public bool OuvrirDeduction { get; set; }

    public string MessageArme { get; set; } = string.Empty;

    
    public IReadOnlyList<ArmeInfo> Armes { get; } = new List<ArmeInfo>

    {
        new(1, "Marteau",  "Un marteau de charpentier. Tête lourde en acier, manche en bois vieilli. Aucune trace suspecte visible.",
            "Poids estimé : 800g. Surface de frappe plane — incompatible avec la forme elliptique de la blessure à l'épaule.",
            false),

        new(2, "Pioche",   "Une pioche de terrassier. Tête bifide en acier forgé, manche en frêne. Légères traces de terre séchée.",
            "Poids estimé : 1,4 kg. Pointe acérée — correspond à la profondeur et l'angle de 35° de la plaie. Traces de fibre textile sur la pointe.",
            true),

        new(3, "Couteau",  "Un couteau de chasse à lame fixe. Manche en bois de cerf, étui en cuir. Lame affûtée.",
            "Lame de 18 cm — trop courte pour atteindre l'épaule depuis l'arrière sans contact rapproché. Incohérent avec la distance estimée.",
            false),

        new(4, "Pistolet", "Un revolver ancien, calibre .38. Barillet chargé à blanc. Canon propre, sans résidu de poudre récent.",
            "Aucun résidu de déflagration. La victime ne présente aucune brûlure ni blessure par balle. Écarté.",
            false),
    };

    public void OnGet() { }

    public IActionResult OnPostValiderArme()
    {
        OuvrirDeduction = true;

        if (ArmeChoisie == BonneArme)
        {
            ArmeValidee = true;
            MessageArme = "Exact. La pioche correspond à tous les indices relevés sur la scène de crime et en salle d'autopsie.";
        }
        else
        {
            ArmeEchouee = true;
            var arme = Armes.FirstOrDefault(a => a.Numero == ArmeChoisie);
            MessageArme = arme is not null
                ? $"Incorrect. {arme.RaisonElimination}"
                : "Ce choix ne correspond pas aux indices collectés.";
        }

        return Page();
    }
}

public sealed record ArmeInfo(
    int    Numero,
    string Nom,
    string Description,
    string RaisonElimination,
    bool   EstArmeduCrime
);
