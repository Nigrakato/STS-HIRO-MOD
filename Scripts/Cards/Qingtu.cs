using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public class Qingtu : AbstractHiroCard
    {
        public Qingtu() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(6m, ValueProp.Move),
            new PowerVar<VulnerablePower>(1m)

        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Attack", Owner.Character.AttackAnimDelay);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(CombatState!)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
		await PowerCmd.Apply<VulnerablePower>(base.CombatState!.HittableEnemies, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);

        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<VulnerablePower>(),

    ];
}}