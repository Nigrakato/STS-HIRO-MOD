using BaseLib.Utils;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Hiro.Scripts.Cards;

    [Pool(typeof(StatusCardPool))]

public class Lengjing : AbstractHiroCard
{
    private const string JusticeKey = "Justice";
    private const string ShipoKey = "Shipo";

    public Lengjing() : base(-1, CardType.Status, CardRarity.Status, TargetType.None)
    {
    }

    public override int MaxUpgradeLevel => 0;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable,CardKeyword.Ethereal];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(JusticeKey, 1m),
        new DynamicVar(ShipoKey, 3m)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
            HoverTipFactory.FromPower<Shipo>(),


    ];
    protected override void OnUpgrade()
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await Task.CompletedTask;
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card != this)
        {
            return;
        }

        await PowerCmd.Apply<Justice>(
            Owner.Creature,
            DynamicVars[JusticeKey].BaseValue,
            Owner.Creature,
            this
        );

        await PowerCmd.Apply<Shipo>(
            Owner.Creature,
            DynamicVars[ShipoKey].BaseValue,
            Owner.Creature,
            this
        );
    }
}