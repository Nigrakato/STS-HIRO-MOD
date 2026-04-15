using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using Hiro.Scripts.Powers;
using System;
using System.Threading.Tasks;
using Hiro.Scripts.Keywords;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Patches
{
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
    public static class ZhengyiKeywordPatch
    {
        [HarmonyPostfix]
        public static async Task Postfix(Task __result, CardModel __instance)
        {
            await __result;

            try
            {
                if (!__instance.CanonicalKeywords.Contains(HiroCardKeywords.Zhengyi))
                {
                    return;
                }

                GD.Print($"[正义关键词] 卡牌 {__instance.Id.Entry} 带正义关键词，准备叠层");

                if (__instance.Owner?.Creature == null)
                {
                    GD.Print("[正义关键词] 持有者为空，跳过");
                    return;
                }

                await Justice.GainStacks(
                    target: __instance.Owner.Creature,
                    amount: 1m,
                    applier: __instance.Owner.Creature,
                    cardSource: __instance
                );

                GD.Print("[正义关键词] ✅ 叠层成功！");
            }
            catch (Exception e)
            {
                GD.PrintErr($"[正义关键词] ❌ 报错：{e}");
            }
        }
    }
}