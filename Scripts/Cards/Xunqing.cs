using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public class Xunqing : AbstractHiroCard
    {
        private const string _enemyStrengthKey = "EnemyStrength";

        public Xunqing() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(0m, ValueProp.Move),
            new PowerVar<StrengthPower>(1m),
            new DynamicVar(_enemyStrengthKey, 1m)
        };

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<StrengthPower>()
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");


            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);

            await PowerCmd.Apply<StrengthPower>(
                Owner.Creature,
                DynamicVars.Strength.BaseValue,
                Owner.Creature,
                this);

            await PowerCmd.Apply<StrengthPower>(
                cardPlay.Target,
                DynamicVars[_enemyStrengthKey].BaseValue,
                Owner.Creature,
                this);
        }

        protected override void OnUpgrade()
        {

            DynamicVars.Strength.UpgradeValueBy(1m);
        }
    }
}