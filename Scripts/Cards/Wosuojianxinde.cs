using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Powers; 
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Cards;

public class KillImpulseMultiplierVar : DynamicVar
{
    public KillImpulseMultiplierVar() : base("PlatVal", 0m)
    {
    }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        int amount = card.Owner?.Creature.GetPowerAmount<KillImpulsePower>() ?? 0;
        this.PreviewValue = amount * 3m;
    }
}

public sealed class Wosuojianxinde : AbstractHiroCard
{
    public Wosuojianxinde()
        : base(3, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new KillImpulseMultiplierVar() 
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<KillImpulsePower>(),
        HoverTipFactory.FromPower<PlatingPower>(),
        HoverTipFactory.FromPower<WitchFormPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        int killImpulseAmount = base.Owner.Creature.GetPowerAmount<KillImpulsePower>();
        int platingToGain = killImpulseAmount * 3;

        if (platingToGain > 0)
        {
            await PowerCmd.Apply<PlatingPower>(
                base.Owner.Creature, 
                platingToGain, 
                base.Owner.Creature, 
                this);
        }
    }
    

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}