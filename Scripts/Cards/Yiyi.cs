using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Powers; 
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Hiro.Scripts.Cards;

public class Yiyi : AbstractHiroCard
{

    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [new DynamicVar("Multiplier", 2m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Shipo>()];

    public Yiyi()
        : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {        await base.OnPlay(choiceContext, cardPlay);

        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        int currentAmount = base.Owner.Creature.GetPowerAmount<Shipo>();

        if (currentAmount > 0)
        {
            decimal multiplier = base.DynamicVars["Multiplier"].BaseValue;
            decimal amountToAdd = (multiplier - 1m) * (decimal)currentAmount;

            await PowerCmd.Apply<Shipo>(
                base.Owner.Creature, 
                amountToAdd, 
                base.Owner.Creature, 
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Multiplier"].UpgradeValueBy(1m);
    }
}