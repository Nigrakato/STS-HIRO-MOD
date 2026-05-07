using System.Linq;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Hiro.Scripts.Cards;


public class Qifuxiaohai : AbstractHiroCard
{
    public Qifuxiaohai() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override void OnUpgrade()

    {
    RemoveKeyword(CardKeyword.Exhaust);

    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {        await base.OnPlay(choiceContext, cardPlay);

        var exhaustPile = PileType.Exhaust.GetPile(Owner);
        int exhaustCount = exhaustPile.Cards.Count;

        for (int i = 0; i < exhaustCount; i++)
        {
            var heibaiCard = CombatState!.CreateCard<Heibai>(Owner);
            await CardPileCmd.AddGeneratedCardToCombat(heibaiCard, PileType.Hand, addedByPlayer: true);
        }
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromCard<Heibai>(),

    ];
}