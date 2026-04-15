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

public class Binfensecai : AbstractHiroCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.FromCard<Secai>()];

    public Binfensecai() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BinfensecaiPower>(1m)
    ];

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        
        await PowerCmd.Apply<BinfensecaiPower>(
            Owner.Creature, 
            DynamicVars["BinfensecaiPower"].BaseValue, 
            Owner.Creature, 
            this
        );
    }
}