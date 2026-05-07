using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Cards;

public class Nuoyagongyu : AbstractHiroCard
{
    public Nuoyagongyu() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override void OnUpgrade()
    {
        
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {        await base.OnPlay(choiceContext, cardPlay);

        if (IsUpgraded)
        {
   
            var selectedCard = (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), // 复用“选择一张牌消耗”的提示
                context: choiceContext,
                player: base.Owner,
                filter: null, 
                source: this
            )).FirstOrDefault();

            if (selectedCard != null)
            {
                await CardCmd.Exhaust(choiceContext, selectedCard);
                
                var colorCard = CombatState!.CreateCard<Secai>(Owner);
                await CardPileCmd.AddGeneratedCardToCombat(colorCard, PileType.Hand, addedByPlayer: true);
            }
        }
        else
        {
            var handPile = PileType.Hand.GetPile(Owner);
            var handCards = handPile.Cards.ToList();
            if (handCards.Count == 0)
                return;

            var rng = Owner.RunState.Rng.CombatCardSelection;
            var cardToTransform = rng.NextItem(handCards);
            if (cardToTransform != null)
            {
                var colorCard = CombatState!.CreateCard<Secai>(Owner);
                await CardPileCmd.AddGeneratedCardToCombat(colorCard, PileType.Hand, addedByPlayer: true);
                await CardPileCmd.RemoveFromCombat(cardToTransform);
            }
        }
        
    }
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Secai>(),

    ];
}