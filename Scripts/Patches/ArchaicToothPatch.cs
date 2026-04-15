using HarmonyLib;
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using Hiro.Scripts.Cards;

namespace KomeijiKoishi.Patches 
{
    [HarmonyPatch(typeof(ArchaicTooth), "get_TranscendenceUpgrades")]
    public static class ArchaicToothPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref Dictionary<ModelId, CardModel> __result)
        {
            
            __result[ModelDb.Card<Jianyichangmao>().Id] = ModelDb.Card<Yishijian>();
            
        }
    }
}