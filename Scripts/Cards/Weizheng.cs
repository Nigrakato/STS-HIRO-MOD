using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Cards;

public sealed class Weizheng : AbstractHiroCard
{


    public Weizheng() 
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        List<CardModel> handCards = PileType.Hand.GetPile(base.Owner).Cards
            .Where(c => c != null)
            .ToList();

        foreach (CardModel card in handCards)
        {
            await CardCmd.TransformToRandom(card, base.Owner.RunState.Rng.CombatCardSelection);
        }
    }
}