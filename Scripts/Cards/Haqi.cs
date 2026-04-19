using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public sealed class Haqi : AbstractHiroCard
{
    private const string _strLossKey = "StrLoss";

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(17m, ValueProp.Move),
        new DynamicVar(_strLossKey, 7m)
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
    {
        HoverTipFactory.FromPower<StrengthPower>()
    };

    public Haqi()
        : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Attack", Owner.Character.AttackAnimDelay);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState!)
            .WithHitFx("vfx/vfx_starry_impact")
            .SpawningHitVfxOnEachCreature()
            .Execute(choiceContext);

        IReadOnlyList<Creature> enemies = CombatState!.HittableEnemies;
        foreach (Creature enemy in enemies)
        {
            await PowerCmd.Apply<HaqiPower>(
                enemy,
                DynamicVars[_strLossKey].BaseValue,
                Owner.Creature,
                this);

            VfxCmd.PlayOnCreature(enemy, "vfx/vfx_attack_slash");
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars[_strLossKey].UpgradeValueBy(2m);
    }
}