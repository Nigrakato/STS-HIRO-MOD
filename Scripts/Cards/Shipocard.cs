using Hiro.Scripts.HiroVar;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public class Shipocard : AbstractHiroCard
{
    public Shipocard() : base(2, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(8, ValueProp.Move),
        new ShipoVar(6)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
        DynamicVars[ShipoVar.Key].UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<Shipo>(Owner.Creature, DynamicVars[ShipoVar.Key].BaseValue, Owner.Creature, this);
        await base.OnPlay(choiceContext, cardPlay);

    }
}