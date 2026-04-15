using BaseLib.Abstracts;
using BaseLib.Utils;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Combat;
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
    public class BrokenPen : AbstracHiroRelic
    {
        public override RelicRarity Rarity => RelicRarity.Ancient;

        protected override IEnumerable<DynamicVar> CanonicalVars => new[] 
        { 
        new DynamicVar("Justice",10m)
        };

        public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
   {
		if (side == base.Owner.Creature.Side && combatState.RoundNumber <= 1)
		{
			Flash();
			await PowerCmd.Apply<Justice>(base.Owner.Creature, base.DynamicVars["Justice"].IntValue, base.Owner.Creature, null);
		}
    }
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<Justice>(),

    ];
}}