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

public sealed class Wutaiju : AbstractHiroCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [new DynamicVar("DrawAmount", 1m)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];

    public Wutaiju()
        : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        decimal amount = base.DynamicVars["DrawAmount"].BaseValue;

        await PowerCmd.Apply<WutaijuPower>(
            base.Owner.Creature, 
            amount, 
            base.Owner.Creature, 
            this
        );
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromCard<Secai>(),

    ];
    protected override void OnUpgrade()
    {
		RemoveKeyword(CardKeyword.Ethereal);


    }
}