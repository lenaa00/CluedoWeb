using CluedoWeb.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CluedoWeb.Tests;

// ════════════════════════════════════════════════════════════
//  TESTS — Salle 1 : énigme de la lettre
// ════════════════════════════════════════════════════════════
public class Salle1LetterTests
{
    private static Salle1Model PostLettre(string mot1, string mot2, string mot3, string mot4)
    {
        var model = new Salle1Model
        {
            Mot1 = mot1, Mot2 = mot2, Mot3 = mot3, Mot4 = mot4
        };
        model.PageContext = new PageContext { HttpContext = new DefaultHttpContext() };
        model.OnPostEnigmeLettre();
        return model;
    }

    [Fact]
    public void Lettre_TousMotsCorrects_EnigmeReussie()
    {
        var model = PostLettre("payer", "mal", "planter", "talon");
        Assert.True(model.EnigmeReussie);
    }

    [Fact]
    public void Lettre_MotsEnMajuscules_EnigmeReussie()
    {
        // La comparaison doit être insensible à la casse
        var model = PostLettre("PAYER", "MAL", "PLANTER", "TALON");
        Assert.True(model.EnigmeReussie);
    }

    [Fact]
    public void Lettre_UnMotFaux_EnigmePasReussie()
    {
        var model = PostLettre("payer", "mal", "planter", "chaussure");
        Assert.False(model.EnigmeReussie);
    }

    [Fact]
    public void Lettre_ChampsVides_EnigmePasReussie()
    {
        var model = PostLettre("", "", "", "");
        Assert.False(model.EnigmeReussie);
    }

    [Fact]
    public void Lettre_ResultatsMots_CorrectEtIncorrectBienDistingues()
    {
        var model = PostLettre("payer", "FAUX", "planter", "talon");
        Assert.True(model.ResultatsMots["Mot1"]);   // payer ✓
        Assert.False(model.ResultatsMots["Mot2"]);  // FAUX  ✗
        Assert.True(model.ResultatsMots["Mot3"]);   // planter ✓
        Assert.True(model.ResultatsMots["Mot4"]);   // talon ✓
    }

    [Fact]
    public void Lettre_LetterTokens_ContientBienQuatreMotsMelanges()
    {
        var model = new Salle1Model();
        model.PageContext = new PageContext { HttpContext = new DefaultHttpContext() };

        var motsMelanges = model.LetterTokens.Where(t => t.EstMotMelange).ToList();
        Assert.Equal(4, motsMelanges.Count);
    }
}

// ════════════════════════════════════════════════════════════
//  TESTS — Salle 1 : énigme du triangle
// ════════════════════════════════════════════════════════════
public class Salle1TriangleTests
{
    private static Salle1Model PostTriangle(string reponse)
    {
        var model = new Salle1Model { ReponseTriangle = reponse };
        model.PageContext = new PageContext { HttpContext = new DefaultHttpContext() };
        model.OnPostTriangle();
        return model;
    }

    [Fact]
    public void Triangle_BonneReponse_TriangleReussi()
    {
        var model = PostTriangle("78");
        Assert.True(model.TriangleReussi);
        Assert.False(model.MauvaiseReponse);
    }

    [Fact]
    public void Triangle_MauvaiseReponse_EchecSignale()
    {
        var model = PostTriangle("42");
        Assert.False(model.TriangleReussi);
        Assert.True(model.MauvaiseReponse);
    }

    [Fact]
    public void Triangle_ReponseAvecEspaces_ReussieQuandValide()
    {
        // .Trim() doit accepter "  78  "
        var model = PostTriangle("  78  ");
        Assert.True(model.TriangleReussi);
    }
}

// ════════════════════════════════════════════════════════════
//  TESTS — Meurtrier : accusation et gestion du poids
//  Suspects : Leblanc (B+, G, 62 kg) <- coupable
//             Rose    (B+, G, 71 kg)
//             Moutarde(A-, D, 94 kg)
//             Violet  (O+, D, 78 kg)
// ════════════════════════════════════════════════════════════
public class MeurtrierAccusationTests
{
    private static MeurtrierModel PostAccuser(int poids)
    {
        var model = new MeurtrierModel { PoidsSaisi = poids };
        model.PageContext = new PageContext { HttpContext = new DefaultHttpContext() };
        model.OnPostAccuser();
        return model;
    }

    [Fact]
    public void Accusation_PoidsExact62_BonneAccusation()
    {
        var model = PostAccuser(62);
        Assert.True(model.BonneAccusation);
        Assert.Contains("Leblanc", model.MessageFin, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(60)] // limite basse tolérance ±2
    [InlineData(64)] // limite haute tolérance ±2
    public void Accusation_DansTolérance_BonneAccusation(int poids)
    {
        var model = PostAccuser(poids);
        Assert.True(model.BonneAccusation,
            $"Poids {poids} kg devrait être accepté (tolérance ±2 autour de 62)");
    }

    [Fact]
    public void Accusation_TropLeger_MessageErreurAdapté()
    {
        var model = PostAccuser(50);
        Assert.False(model.BonneAccusation);
        Assert.Contains("trop léger", model.MessageFin, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Accusation_TropLourd_MessageErreurAdapté()
    {
        var model = PostAccuser(90);
        Assert.False(model.BonneAccusation);
        Assert.Contains("trop lourd", model.MessageFin, StringComparison.OrdinalIgnoreCase);
    }
}
