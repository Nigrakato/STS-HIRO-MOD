using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public class Tingquzhengju : AbstractHiroCard
{
    public Tingquzhengju() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new BlockVar(8, ValueProp.Move),
    new DynamicVar("Draw", 2m)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {        await base.OnPlay(choiceContext, cardPlay);

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        await PowerCmd.Apply<DrawPower>(
            Owner.Creature,
            DynamicVars["Draw"].BaseValue,
            Owner.Creature,
            this
        );
    }
}