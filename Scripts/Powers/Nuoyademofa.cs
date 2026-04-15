using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Cards
{
    public sealed class Nuoyademofa : AbstractHiroCard
    {
        protected override IEnumerable<IHoverTip> ExtraHoverTips => new[] { HoverTipFactory.FromCard<Secai>(base.IsUpgraded) };
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public Nuoyademofa() 
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

            List<CardModel> handCards = PileType.Hand.GetPile(base.Owner).Cards
                .Where(c => c != null && c.IsTransformable)
                .ToList();

            foreach (var card in handCards)
            {
                CardModel secaiCard = base.CombatState!.CreateCard<Secai>(base.Owner);

                if (base.IsUpgraded)
                {
                    CardCmd.Upgrade(secaiCard);
                }

                await CardCmd.Transform(card, secaiCard);
            }
        }
    }
}