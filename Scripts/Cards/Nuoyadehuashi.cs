using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Hiro.Scripts.Cards;

public class Nuoyadehuashi : AbstractHiroCard
{
    public Nuoyadehuashi() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.AddGeneratedCardToCombat(
            CombatState!.CreateCard<Secai>(Owner),
            PileType.Hand,
            addedByPlayer: true
        );

        await CardPileCmd.AddGeneratedCardToCombat(
            CombatState!.CreateCard<Heibai>(Owner),
            PileType.Hand,
            addedByPlayer: true
        );
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromCard<Secai>(),
    HoverTipFactory.FromCard<Heibai>()

    ];
}