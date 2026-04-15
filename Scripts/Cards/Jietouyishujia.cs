using System.Collections.Generic;
using System.Threading.Tasks;
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

public class Jietouyishujia : AbstractHiroCard
{

    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [new DynamicVar("BlockAmount", 2m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.FromPower<JietouyishujiaPower>(),
        HoverTipFactory.FromCard<Secai>(),
        HoverTipFactory.FromCard<Heibai>()
        ];

    public Jietouyishujia()
        : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        decimal amount = base.DynamicVars["BlockAmount"].BaseValue;

        await PowerCmd.Apply<JietouyishujiaPower>(
            base.Owner.Creature, 
            amount, 
            base.Owner.Creature, 
            this
        );
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BlockAmount"].UpgradeValueBy(2m);
    }
    
}