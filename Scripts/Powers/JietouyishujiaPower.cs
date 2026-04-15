using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens; 
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Powers;

public sealed class JietouyishujiaPower : AbstractHiroPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        
        bool isSecai = cardPlay.Card is Secai;
        bool isHeibai = cardPlay.Card.Id.Entry.Contains("heibai", StringComparison.OrdinalIgnoreCase);

        if (cardPlay.Card.Owner == base.Owner.Player && (isSecai || isHeibai))
        {
            this.Flash(); 
            
            await CreatureCmd.GainBlock(base.Owner, (decimal)base.Amount, ValueProp.Unpowered, null);
        }
    }
}