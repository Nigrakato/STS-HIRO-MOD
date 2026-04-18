using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hiro.Scripts.Powers
{
    public class Justice : AbstractHiroPower
    {
        private class Data
        {
            public bool hasReached10;
            public bool hasReached20;
            public bool hasReached30;
            public bool hasTriggeredInstant10;
            public bool hasTriggeredInstant20;
        }

        public const int JusticeWinThreshold = 40;
        public const int MaxHpGainPerJusticeWhileWitchForm = 1;

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        private bool _hasTriggeredWin = false;

        protected override object InitInternalData() => new Data();
        private Data JusticeData => GetInternalData<Data>();

        public static async Task GainStacks(
            Creature target,
            decimal amount,
            Creature applier,
            CardModel? cardSource)
        {
            if (amount <= 0) return;
            await PowerCmd.Apply<Justice>(target, amount, applier, cardSource);
        }

        public override decimal ModifyHandDraw(Player player, decimal count)
        {
            if (Owner == null || Owner.IsDead || Owner.Player != player)
                return count;

            if (JusticeData.hasReached10)
                return count + 1m;

            return count;
        }

        public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power != this || Owner == null || Owner.Player == null || !CombatManager.Instance.IsInProgress)
                return;

            if (Owner.GetPower<WitchFormPower>() != null)
            {
                int gainMaxHp = (int)amount * MaxHpGainPerJusticeWhileWitchForm;
                if (gainMaxHp > 0)
                    await CreatureCmd.GainMaxHp(Owner, gainMaxHp);
            }

            if (!JusticeData.hasReached10 && Amount >= 10)
            {
                JusticeData.hasReached10 = true;
                Flash();

                try
                {
                    await CardPileCmd.Draw(null, 1, Owner.Player);
                    JusticeData.hasTriggeredInstant10 = true;
                }
                catch
                {
                }
            }

            if (!JusticeData.hasReached20 && Amount >= 20)
            {
                JusticeData.hasReached20 = true;
                Flash();

                try
                {
                    await PlayerCmd.GainEnergy(1, Owner.Player);
                    JusticeData.hasTriggeredInstant20 = true;
                }
                catch
                {
                }
            }

            if (!JusticeData.hasReached30 && Amount >= 30)
            {
                JusticeData.hasReached30 = true;
                Flash();
                await PowerCmd.Apply<IntangiblePower>(Owner, 1, Owner, cardSource);
            }

            if (!_hasTriggeredWin && Amount >= JusticeWinThreshold)
            {
                _hasTriggeredWin = true;
                await ExecuteCombatVictory();
            }
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || Owner.IsDead || Owner.Player != player)
                return;

            if (JusticeData.hasReached20)
            {
                if (!JusticeData.hasTriggeredInstant20)
                {
                    JusticeData.hasTriggeredInstant20 = true;
                    await PlayerCmd.GainEnergy(1, player);
                }
                await PlayerCmd.GainEnergy(1, player);
            }
        }

        private async Task ExecuteCombatVictory()
        {
            var combatState = CombatManager.Instance.DebugOnlyGetState();
            List<Creature> allEnemies = combatState!.Enemies.ToList();

            if (allEnemies.Count == 0)
            {
                await CombatManager.Instance.CheckWinCondition();
                return;
            }

            foreach (var enemy in allEnemies)
            {
                enemy.RemoveAllPowersInternalExcept();
                await CreatureCmd.Kill(enemy);
            }

            await CombatManager.Instance.CheckWinCondition();
        }
    }
}