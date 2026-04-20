using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System;
using System.Reflection;
using MegaCrit.Sts2.Core.GameActions;

namespace Hiro.Scripts.Patches
{
    // ------------------------------
    // 【核心补丁】拦截 NCardPlayQueue 的误操作
    // ------------------------------
    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Nodes.Combat.NCardPlayQueue))]
    [HarmonyPatch("ReAddCardAfterPlayerChoice")]
    public static class NCardPlayQueue_ReAddCardAfterPlayerChoice_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(NCard card, GameAction action)
        {
            try
            {
                // ------------------------------
                // 【修复点 1】用反射或更安全的方式判断
                // ------------------------------
                if (IsTemporaryChoiceCard(card) || IsCardUnsafe(card))
                {
                    GD.Print("[NCardPlayQueue 补丁] 跳过不安全/临时卡牌的 ReAddCardAfterPlayerChoice");
                    return false; // 返回 false = 跳过原方法
                }
            }
            catch (Exception e)
            {
                GD.PrintErr($"[NCardPlayQueue 补丁] 判断出错，直接跳过：{e.Message}");
                return false; // 出错也跳过，避免崩溃
            }

            return true; // 返回 true = 执行原方法
        }

        // ------------------------------
        // 【辅助方法 1】判断卡牌是否不安全（没有父节点等）
        // ------------------------------
        private static bool IsCardUnsafe(NCard card)
        {
            if (card == null)
                return true;

            // 检查卡牌是否有父节点
            if (card.GetParent() == null)
                return true;

            // 检查卡牌是否在场景树中
            if (!card.IsInsideTree())
                return true;

            return false;
        }

        // ------------------------------
        // 【辅助方法 2】尝试用反射获取卡牌 ID 判断
        // ------------------------------
        private static bool IsTemporaryChoiceCard(NCard card)
        {
            if (card == null)
                return true;

            try
            {
                // 尝试通过反射获取可能的属性名：Model / CardModel / Card
                PropertyInfo? modelProp = card.GetType().GetProperty("Model", BindingFlags.Public | BindingFlags.Instance);
                modelProp ??= card.GetType().GetProperty("CardModel", BindingFlags.Public | BindingFlags.Instance);
                modelProp ??= card.GetType().GetProperty("Card", BindingFlags.Public | BindingFlags.Instance);

                if (modelProp == null)
                    return false; // 找不到属性，暂时不判断

                var model = modelProp.GetValue(card);
                if (model == null)
                    return true;

                // 尝试获取模型的 ID
                PropertyInfo? idProp = model.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
                if (idProp == null)
                    return false;

                var idObj = idProp.GetValue(model);
                if (idObj == null)
                    return true;

                // 尝试获取 ID 的 Entry 属性
                PropertyInfo? entryProp = idObj.GetType().GetProperty("Entry", BindingFlags.Public | BindingFlags.Instance);
                if (entryProp == null)
                    return false;

                string? cardId = entryProp.GetValue(idObj) as string;
                if (string.IsNullOrEmpty(cardId))
                    return true;

                // 根据 ID 判断是否为临时卡牌
                string lowerId = cardId.ToLowerInvariant();
                return lowerId.Contains("picnic") ||
                       lowerId.Contains("movie") ||
                       lowerId.Contains("game");
            }
            catch
            {
                return false; // 反射出错，暂时不判断
            }
        }
    }

    // ------------------------------
    // 【配套补丁】拦截另一个误操作方法
    // ------------------------------
    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Nodes.Combat.NCardPlayQueue))]
    [HarmonyPatch("BeforeRemoteCardPlayResumedAfterPlayerChoice")]
    public static class NCardPlayQueue_BeforeRemoteCardPlayResumedAfterPlayerChoice_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(GameAction action)
        {
            GD.Print("[NCardPlayQueue 补丁] 跳过 BeforeRemoteCardPlayResumedAfterPlayerChoice");
            return false; // 直接跳过原方法
        }
    }
}