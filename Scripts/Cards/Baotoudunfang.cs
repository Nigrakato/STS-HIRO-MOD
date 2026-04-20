using Hiro.Scripts.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public class Baotoudunfang : AbstractHiroCard
    {
        public Baotoudunfang() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(14m, ValueProp.Move)];

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(4m);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }
}