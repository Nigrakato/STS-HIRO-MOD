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


                string cardId = __instance.Id?.Entry ?? string.Empty;
                if (IsTemporaryChoiceCard(cardId))
                {
                    GD.Print($"[错误关键词] 跳过临时选择卡牌：{cardId}");
                    return;
                }

                GD.Print($"[错误关键词] 卡牌 {cardId} 带错误关键词，准备获得杀意冲动");

                if (choiceContext == null)
                {
                    GD.Print("[错误关键词] choiceContext 为空，跳过");
                    return;
                }

                if (__instance.Owner?.Creature == null || !__instance.Owner.Creature.IsAlive)
                {
                    GD.Print("[错误关键词] 持有者为空或已死亡，跳过");
                    return;
                }


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
                GD.PrintErr($"[错误关键词] ❌ 报错：{e.Message}\n{e.StackTrace}");
            }
        }


        private static bool IsTemporaryChoiceCard(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
                return true;

            string lowerId = cardId.ToLowerInvariant();
            return lowerId.Contains("picnic") ||
                   lowerId.Contains("movie") ||
                   lowerId.Contains("game");
        }
    }
}