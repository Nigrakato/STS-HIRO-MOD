using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Hiro.Scripts.Cards;

namespace Hiro.Scripts.Powers
{
    public sealed class Shipo : AbstractHiroPower
    {
        private CardModel? _consumingCard;
        private decimal _consumedAmountForCurrentCard;

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("BonusPercent", 0m)
        ];

        private bool IsSuppressed()
        {
            return Owner.HasPower<ShipoLockPower>();
        }

        public override Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power == this)
            {
                DynamicVars["BonusPercent"].BaseValue = Amount * 10m;
            }
            return Task.CompletedTask;
        }

        public override Task BeforeCardPlayed(CardPlay cardPlay)
        {
            if (IsSuppressed())
            {
                return Task.CompletedTask;
            }

            if (cardPlay.Card.Type != CardType.Attack)
            {
                return Task.CompletedTask;
            }

            if (cardPlay.Card.Owner.Creature != Owner)
            {
                return Task.CompletedTask;
            }

            if (Amount <= 0)
            {
                return Task.CompletedTask;
            }

            _consumingCard = cardPlay.Card;
            _consumedAmountForCurrentCard = Amount;

            return Task.CompletedTask;
        }

        public override decimal ModifyDamageMultiplicative(
            Creature? target,
            decimal amount,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource)
        {
            if (IsSuppressed())
            {
                return 1m;
            }

            if (cardSource == null || cardSource.Type != CardType.Attack)
            {
                return 1m;
            }

            if (cardSource.Owner.Creature != Owner)
            {
                return 1m;
            }

            decimal effectiveAmount = Amount;

            if (cardSource == _consumingCard)
            {
                effectiveAmount = _consumedAmountForCurrentCard;
            }

            if (effectiveAmount <= 0)
            {
                return 1m;
            }

            decimal perStackBonus = 0.1m;

            if (cardSource is Cuilianhuogun cuilianhuogun)
            {
                decimal extraRatePercent = cuilianhuogun.DynamicVars["ShipoBonusRate"].BaseValue;
                decimal extraRate = extraRatePercent / 100m;
                perStackBonus *= (1m + extraRate);
            }

            return 1m + (effectiveAmount * perStackBonus);
        }

        public override async Task AfterCardPlayedLate(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card != _consumingCard)
            {
                return;
            }

            decimal consumeAmount = _consumedAmountForCurrentCard;

            _consumingCard = null;
            _consumedAmountForCurrentCard = 0m;

            if (consumeAmount <= 0)
            {
                return;
            }

            decimal actualConsume = Amount < consumeAmount ? Amount : consumeAmount;
            if (actualConsume > 0)
            {
                await PowerCmd.ModifyAmount(this, -actualConsume, Owner, null);
            }
        }
    }
}