using Hiro.Scripts.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public class Fengbineixin : AbstractHiroCard
{
    public Fengbineixin() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(16, ValueProp.Move)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(6);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<NoDrawPower>(Owner.Creature, 1m, Owner.Creature, this);
    }
}