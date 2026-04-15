using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Powers
{
    public class ShipoRefund : AbstractHiroPower
    {
        private bool _refunding;

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("RefundPercent", 0m)
        ];

        public override Task AfterPowerAmountChanged(
            PowerModel power,
            decimal amount,
            Creature? applier,
            CardModel? cardSource)
        {
            if (power == this)
            {
                DynamicVars["RefundPercent"].BaseValue = Amount * 50m;
            }

            if (_refunding)
            {
                return Task.CompletedTask;
            }

            if (power is not Shipo shipo || shipo.Owner != Owner)
            {
                return Task.CompletedTask;
            }

            if (amount >= 0)
            {
                return Task.CompletedTask;
            }

            int consumed = (int)(-amount);
            int refund = consumed * Amount / 2;

            if (refund <= 0)
            {
                return Task.CompletedTask;
            }

            return Refund(shipo, refund);
        }

        private async Task Refund(Shipo shipo, int refund)
        {
            _refunding = true;
            try
            {
                await PowerCmd.ModifyAmount(shipo, refund, Owner, null);
            }
            finally
            {
                _refunding = false;
            }
        }
    }
}