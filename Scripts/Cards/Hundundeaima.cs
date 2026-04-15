using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public sealed class Hundundeaima : AbstractHiroCard
{
    public Hundundeaima() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[] { new DamageVar(9m, ValueProp.Move) };

    protected override void OnUpgrade() => base.DynamicVars.Damage.UpgradeValueBy(3m);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!) 
            .Execute(choiceContext);

        var hand = CardPile.GetCards(base.Owner, PileType.Hand)
            .Where(c => c != this && c.EnergyCost.Canonical >= 0)
            .ToList();

        if (hand.Count > 0)
        {
            var targetCard = hand.TakeRandom(1, base.Owner.RunState.Rng.CombatCardSelection).FirstOrDefault();
            if (targetCard != null)
            {
                int newCost = base.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4); 
                targetCard.EnergyCost.SetThisCombat(newCost);
                NCard.FindOnTable(targetCard)?.PlayRandomizeCostAnim();
            }
        }
    }
}