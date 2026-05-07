using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public class Lingqiaohuogun : AbstractHiroCard
    {
        private const int BaseCardCost = 0;

        [SavedProperty]
        public int Hiro_CombatHurtCount { get; set; } = 0;

        public Lingqiaohuogun() : base(BaseCardCost, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(12m, ValueProp.Move),
            new DynamicVar("HurtCount", 0)
        ];

        private void RefreshDynamicCost()
        {
            int newCost = Math.Max(0, BaseCardCost + Hiro_CombatHurtCount);

            DynamicVars["HurtCount"].BaseValue = Hiro_CombatHurtCount;

            EnergyCost.SetThisCombat(newCost);
            InvokeEnergyCostChanged();
        }

        public override async Task BeforeCombatStart()
        {
            Hiro_CombatHurtCount = 0;
            RefreshDynamicCost();
            await Task.CompletedTask;
        }

        public override async Task AfterDamageReceived(
            PlayerChoiceContext choiceContext,
            Creature target,
            DamageResult result,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource)
        {
            if (target != Owner.Creature || result.UnblockedDamage <= 0)
                return;

            Hiro_CombatHurtCount++;
            RefreshDynamicCost();

            await Task.CompletedTask;
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}