using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders; 
using MegaCrit.Sts2.Core.Models;
using Hiro.Scripts.Powers; 
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Commands;

namespace Hiro.Scripts.Patches;

[HarmonyPatch(typeof(AttackCommand), nameof(AttackCommand.Execute))]
public static class AnxiyishiSplashPatch
{
    private static bool _isProcessingSplash = false;

    private static readonly FieldInfo DamageField = 
        AccessTools.Field(typeof(AttackCommand), "_damagePerHit") ?? 
        AccessTools.Field(typeof(AttackCommand), "damagePerHit");

    private static readonly FieldInfo HitCountField = 
        AccessTools.Field(typeof(AttackCommand), "_hitCount") ?? 
        AccessTools.Field(typeof(AttackCommand), "hitCount");

    private static readonly FieldInfo TargetsListField = 
        AccessTools.Field(typeof(AttackCommand), "_targets") ?? 
        AccessTools.Field(typeof(AttackCommand), "targets");

    private static readonly FieldInfo SingleTargetField = 
        AccessTools.Field(typeof(AttackCommand), "_target") ?? 
        AccessTools.Field(typeof(AttackCommand), "target");

    public static void Prefix(AttackCommand __instance)
    {
        if (_isProcessingSplash) return;

        if (__instance.ModelSource is CardModel card && card.Type == CardType.Attack && 
            card.Owner?.Creature != null && card.Owner.Creature.HasPower<AnxiyishiPower>())
        {
            var combatState = card.CombatState;
            if (combatState == null) return;

            decimal finalDmg = (decimal)(DamageField?.GetValue(__instance) ?? card.DynamicVars.Damage.BaseValue);
            
            int hitCount = (int)(HitCountField?.GetValue(__instance) ?? 1);

            var originalTargets = new HashSet<Creature>();
            if (TargetsListField?.GetValue(__instance) is IEnumerable<Creature> list)
                foreach (var t in list) originalTargets.Add(t);
            if (SingleTargetField?.GetValue(__instance) is Creature single)
                originalTargets.Add(single);

            var others = combatState.HittableEnemies.Where(e => !originalTargets.Contains(e)).ToList();

            if (others.Count > 0)
            {
                _isProcessingSplash = true; 
                try 
                {
                    foreach (var enemy in others)
                    {
                        DamageCmd.Attack(finalDmg)
                            .FromCard(card)
                            .Targeting(enemy)
                            .WithHitCount(hitCount) 
                            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                            .Execute(null); 
                    }
                }
                finally
                {
                    _isProcessingSplash = false;
                }
            }
        }
    }
}