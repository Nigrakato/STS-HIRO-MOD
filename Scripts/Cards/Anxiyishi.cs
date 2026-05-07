using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Cards;

public class Anxiyishi : AbstractHiroCard
{

    public Anxiyishi() : base(2, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
    }


    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("RitualPower", 3) 
    ];

    protected override bool IsPlayable => base.IsPlayable && (Owner?.Creature?.GetPowerAmount<Justice>() > 6);

    protected override void OnUpgrade()
    {
        DynamicVars["RitualPower"].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<RitualPower>(Owner.Creature,  DynamicVars["RitualPower"].BaseValue, Owner.Creature, this);
        await base.OnPlay(choiceContext, cardPlay);

        await PowerCmd.Apply<AnxiyishiPower>(Owner.Creature, 1, Owner.Creature, this);
    }
}