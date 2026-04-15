using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using Hiro.Scripts.Cards.Tokens;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Hiro.Scripts.Cards;

[Pool(typeof(HiroCardPool))]
public class Ganhedewenquan : AbstractHiroCard
{

    public Ganhedewenquan() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [

        new DynamicVar("Power", 1m)
    ];

    protected override void OnUpgrade()
    {

        base.EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);


        await PowerCmd.Apply<GanhedewenquanPower>(
            base.Owner.Creature, 
            base.DynamicVars["Power"].IntValue, 
            base.Owner.Creature, 
            this);
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromCard<Heibai>(),

    ];
}