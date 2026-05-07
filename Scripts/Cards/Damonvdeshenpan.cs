using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Cards
{
    public sealed class Damonvdeshenpan : AbstractHiroCard
    {
        protected override bool HasEnergyCostX => true;
        
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


        public Damonvdeshenpan()
            : base(-1, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

            int xValue = ResolveEnergyXValue();

            int totalToGenerate = 2 * xValue;
            if (IsUpgraded)
            {
                totalToGenerate += 1;
            }

            if (totalToGenerate <= 0) return;

            var powerCardsQuery = from c in Owner.Character.CardPool.GetUnlockedCards(
                                      Owner.UnlockState, 
                                      Owner.RunState.CardMultiplayerConstraint)
                                  where c.Type == CardType.Power
                                  select c;

            IEnumerable<CardModel> generatedCards = CardFactory.GetDistinctForCombat(
                Owner,
                powerCardsQuery,
                totalToGenerate,
                Owner.RunState.Rng.CombatCardGeneration);

            foreach (CardModel cardModel in generatedCards)
            {
                if (cardModel != null)
                {
                    await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, addedByPlayer: true);
                }
            }
        }

        protected override void OnUpgrade()
        {
        }


            }
        }

