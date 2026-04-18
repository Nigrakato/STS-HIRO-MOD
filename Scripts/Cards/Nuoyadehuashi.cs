using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Cards;

public class Nuoyadehuashi : AbstractHiroCard
{
    public Nuoyadehuashi() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel secaiCard = CombatState!.CreateCard<Secai>(Owner);
        if (IsUpgraded)
        {
            CardCmd.Upgrade(secaiCard);
        }
        await CardPileCmd.AddGeneratedCardToCombat(
            secaiCard,
            PileType.Hand,
            addedByPlayer: true
        );

        CardModel heibaiCard = CombatState!.CreateCard<Heibai>(Owner);
        if (IsUpgraded)
        {
            CardCmd.Upgrade(heibaiCard);
        }
        await CardPileCmd.AddGeneratedCardToCombat(
            heibaiCard,
            PileType.Hand,
            addedByPlayer: true
        );
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Secai>(IsUpgraded), // 显示升级后的预览
        HoverTipFactory.FromCard<Heibai>(IsUpgraded)
    ];
}