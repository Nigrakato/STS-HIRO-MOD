using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Cards
{
    public class Cuowudexuanze : AbstractHiroCard
    {
        public Cuowudexuanze() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
        [
            CardKeyword.Exhaust,
            HiroCardKeywords.Error
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<StrengthPower>(2m),
            new PowerVar<VulnerablePower>(1m)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<StrengthPower>(),
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromPower<WitchFormPower>(),
            HoverTipFactory.FromPower<KillImpulsePower>(),


        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<StrengthPower>(
                Owner.Creature,
                DynamicVars["StrengthPower"].BaseValue,
                Owner.Creature,
                this);

            await PowerCmd.Apply<VulnerablePower>(
                Owner.Creature,
                DynamicVars["VulnerablePower"].BaseValue,
                Owner.Creature,
                this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["StrengthPower"].UpgradeValueBy(1m);
        }

    }
    
}