using HarmonyLib;
using Godot;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Patches
{
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
    public static class ErrorKeywordPatch
    {
        [HarmonyPostfix]
        public static async Task Postfix(
            Task __result,
            CardModel __instance,
            PlayerChoiceContext choiceContext)
        {
            await __result;

            try
            {
                if (!__instance.CanonicalKeywords.Contains(HiroCardKeywords.Error))
                {
                    return;
                }

                if (__instance.Owner?.Creature == null)
                {
                    GD.Print("[错误关键词] 持有者为空，跳过");
                    return;
                }

                GD.Print($"[错误关键词] 卡牌 {__instance.Id.Entry} 带错误关键词，获得1层杀意冲动");

                await KillImpulsePower.GainStacks(
                    choiceContext,
                    __instance.Owner.Creature,
                    2m,
                    __instance.Owner.Creature,
                    __instance
                );

                GD.Print("[错误关键词] ✅ 杀意冲动叠层成功！");
            }
            catch (Exception e)
            {
                GD.PrintErr($"[错误关键词] ❌ 报错：{e}");
            }
        }
    }
}