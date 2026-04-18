using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Powers
{
    public class WitchFormPower : AbstractHiroPower
    {
        public const int BaseTurns = 6;
        public const int DrawPerTurn = 3;
        public const int EnergyPerTurn = 3;
        private const int MaxHpLoss = 5;

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("BaseTurns", BaseTurns),
            new DynamicVar("Draw", DrawPerTurn),
            new DynamicVar("Energy", EnergyPerTurn)
        ];

        public static async Task ApplyWithImmediateEffect(
            PlayerChoiceContext choiceContext,
            Creature target,
            decimal amount,
            CardModel? cardSource)
        {
            if (amount <= 0) return;

            await PowerCmd.Apply<WitchFormPower>(target, amount, target, cardSource);

            if (target.Player != null)
            {
                await CreatureCmd.LoseMaxHp(choiceContext, target, MaxHpLoss, false);
                await ResolveGainEffect(choiceContext, target.Player);
            }
        }

        public override decimal ModifyHandDraw(Player player, decimal count)
        {
            if (Owner == null || Owner.IsDead || Owner.Player != player)
                return count;

            return count + DrawPerTurn;
        }

        private static async Task ResolveGainEffect(PlayerChoiceContext choiceContext, Player player)
        {
            await PlayerCmd.GainEnergy(EnergyPerTurn, player);
            await CardPileCmd.Draw(choiceContext, DrawPerTurn, player);
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || Owner.IsDead || Owner.Player != player) return;

            Flash();
            await CreatureCmd.LoseMaxHp(choiceContext, Owner, MaxHpLoss, false);

            await PlayerCmd.GainEnergy(EnergyPerTurn, player);
            await PowerCmd.ModifyAmount(this, -1m, Owner, null);
            if (Amount <= 0)
            {
                await CreatureCmd.Kill(Owner);
            }
        }
    }
}