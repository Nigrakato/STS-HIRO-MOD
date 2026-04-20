using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public sealed class Dudududu : AbstractHiroCard
    {
        public Dudududu() : base(5, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DamageVar(6m, ValueProp.Move),
            new RepeatVar(7)
        ];

        protected override bool IsPlayable => base.Owner.Creature.HasPower<WitchFormPower>();

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [
            HoverTipFactory.FromPower<WitchFormPower>()
        ];

        protected override void OnUpgrade()
        {
            base.DynamicVars["Repeat"].UpgradeValueBy(2);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(base.DynamicVars["Repeat"].IntValue) 
                .FromCard(this)
                .TargetingAllOpponents(base.CombatState!)
                .WithHitFx("vfx/vfx_attack_blunt") 
                .Execute(choiceContext);
        }
    }
}