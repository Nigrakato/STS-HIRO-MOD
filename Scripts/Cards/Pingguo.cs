using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hiro.Scripts.Cards;

public class Pingguo : AbstractHiroCard
{
    public Pingguo() : base( 1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];



    public override bool CanBeGeneratedInCombat => false;

    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        new HealVar(5m)
    ]);

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
    }
}