using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Commands;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Hiro.Scripts.Powers;

public sealed class BinfensecaiPower : AbstractHiroPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.FromCard<Secai>()];

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (player == base.Owner.Player)
        {
            Flash(); 
            
            for (int i = 0; i < (int)Amount; i++)
            {
                await CardPileCmd.AddGeneratedCardToCombat(
                    combatState.CreateCard<Secai>(player),
                    PileType.Hand,
                    addedByPlayer: true
                );
            }
        }
    }
}