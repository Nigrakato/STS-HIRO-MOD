using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Hiro.Scripts.Cards;

public class Juewu : AbstractHiroCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [new PowerVar<JuewuPower>(2m)];



    public Juewu()
        : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {        await base.OnPlay(choiceContext, cardPlay);

        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        await PowerCmd.Apply<JuewuPower>(
            base.Owner.Creature, 
            base.DynamicVars["JuewuPower"].BaseValue, 
            base.Owner.Creature, 
            this
        );
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["JuewuPower"].UpgradeValueBy(1m);
    }
}