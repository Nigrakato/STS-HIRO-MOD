using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Powers
{
    public sealed class Shipo : AbstractHiroPower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("BonusPercent", 0m)
        ];

        public override Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power == this)
            {
                DynamicVars["BonusPercent"].BaseValue = Amount * 10m;
            }
            return Task.CompletedTask;
        }

        public override decimal ModifyDamageMultiplicative(
            Creature? target,
            decimal amount,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource)
        {
            if (cardSource == null || cardSource.Type != CardType.Attack)
            {
                return 1m;
            }

            if (cardSource.Owner.Creature != Owner)
            {
                return 1m;
            }

            return 1m + (Amount * 0.1m);
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card.Type != CardType.Attack)
            {
                return;
            }

            if (cardPlay.Card.Owner.Creature == Owner)
            {
                if (Amount > 0)
                {
                    await PowerCmd.ModifyAmount(this, -Amount, Owner, null);
                }
            }
        }

    }
    
}