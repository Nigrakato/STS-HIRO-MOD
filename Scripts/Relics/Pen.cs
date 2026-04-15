using BaseLib.Abstracts;
using BaseLib.Utils;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hiro.Scripts.Relics
{
    [Pool(typeof(HiroRelicPool))] 
    public class Pen : AbstracHiroRelic
    {
public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<BrokenPen>();
        public override RelicRarity Rarity => RelicRarity.Starter;

        protected override IEnumerable<DynamicVar> CanonicalVars => new[] 
        { 
            new DynamicVar("VigorAmount", 1) 
        };

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
 
            if (player != Owner)
            {
                return;
            }

            await PowerCmd.Apply<VigorPower>(Owner.Creature, 1, Owner.Creature, null);
        }
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<Justice>(),

    ];
    }
}