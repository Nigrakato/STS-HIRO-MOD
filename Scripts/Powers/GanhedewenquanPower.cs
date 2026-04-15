using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Powers;

public sealed class GanhedewenquanPower : AbstractHiroPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.FromKeyword(CardKeyword.Ethereal)];

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        bool isHeibai = cardPlay.Card.Id.Entry.Contains("heibai", StringComparison.OrdinalIgnoreCase);

        if (cardPlay.Card.Owner == base.Owner.Player && isHeibai)
        {
            this.Flash();

            var player = base.Owner.Player;
            var attackPool = player.Character.CardPool
                .GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
                .Where(c => c.Type == CardType.Attack && c.Rarity != CardRarity.Basic)
                .ToList();

            if (attackPool.Count > 0)
            {
                int count = (int)base.Amount;
                CardModel[] generatedCards = new CardModel[count];
                var rng = player.RunState.Rng.CombatCardGeneration;

                for (int i = 0; i < count; i++)
                {
                    var randomCard = CardFactory.GetDistinctForCombat(player, attackPool, 1, rng).First();
                    CardCmd.ApplyKeyword(randomCard, CardKeyword.Ethereal);
                    generatedCards[i] = randomCard;
                }

                await CardPileCmd.AddGeneratedCardsToCombat(generatedCards, PileType.Hand, addedByPlayer: true);
            }
        }
    }
}