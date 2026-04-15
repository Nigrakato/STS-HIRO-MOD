using Hiro.Scripts.HiroVar;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public class Wuweikunan : AbstractHiroCard
{
    public Wuweikunan() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(4, ValueProp.Move),
        new DynamicVar("Justice",2m)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<Justice>(Owner.Creature, DynamicVars["Justice"].BaseValue, Owner.Creature, this);
    }
}