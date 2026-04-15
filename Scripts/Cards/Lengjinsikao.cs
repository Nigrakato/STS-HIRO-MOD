using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hiro.Scripts.Cards;

public class Lengjinsikao : AbstractHiroCard
{
    public Lengjinsikao() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("PowerAmount", 1)
    ];

    protected override void OnUpgrade()
    {
            base.EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ShipoRefund>(Owner.Creature, DynamicVars["PowerAmount"].BaseValue, Owner.Creature, this);
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromPower<Shipo>(),

    ];
}