using System;
using System.Collections.Generic;
using System.Linq;
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
using Hiro.Scripts.Cards.Tokens;

namespace Hiro.Scripts.Powers
{
    public sealed class NuoyaxifaPower : AbstractHiroPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();


        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (Amount <= 0)
                return;

            if (!IsSecaiOrHeibaiCard(cardPlay.Card))
                return;

            Creature? randomEnemy = GetRandomEnemy();
            if (randomEnemy == null || !randomEnemy.IsAlive)
                return;


            Flash();
            await CreatureCmd.Damage(
                context,
                randomEnemy,
                6m, 
                ValueProp.Unpowered, 
                Owner,
                null
            );
        }


        private bool IsSecaiOrHeibaiCard(CardModel card)
        {
            return card is Secai || card is Heibai;

    
        }

        private Creature? GetRandomEnemy()
        {
            var aliveEnemies = Owner.CombatState!.Enemies
                .Where(e => e.IsAlive)
                .ToList();

            if (aliveEnemies.Count == 0)
                return null;

            var random = new Random();
            int index = random.Next(aliveEnemies.Count);
            return aliveEnemies[index];
        }
    }
}