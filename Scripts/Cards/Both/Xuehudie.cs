using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Hiro.Scripts.Cards;

public class Xuehudie : AbstractHiroCard
{
    public Xuehudie()
        : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        await PowerCmd.Apply<XuehudiePower>(base.Owner.Creature, 1m, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
            base.EnergyCost.UpgradeBy(-1);
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromCard<Secai>(),

    ];
}