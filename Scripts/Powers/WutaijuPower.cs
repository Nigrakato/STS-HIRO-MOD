using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens; 
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Hiro.Scripts.Powers;

public sealed class WutaijuPower : AbstractHiroPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card is Secai && cardPlay.Card.Owner == base.Owner.Player)
        {

            this.Flash();

            await CardPileCmd.Draw(choiceContext, (int)base.Amount, base.Owner.Player);
        }
    }
}