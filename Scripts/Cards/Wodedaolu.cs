using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hiro.Scripts.Cards;

public class Wodedaolu : AbstractHiroCard
{
    private const string VarKey = "WodedaoluPower";

    public Wodedaolu() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new PowerVar<WodedaoluPower>(2m)
    };

    protected override void OnUpgrade()
    {
        DynamicVars[VarKey].UpgradeValueBy(2m);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        await PowerCmd.Apply<WodedaoluPower>(
            base.Owner.Creature, 
            DynamicVars[VarKey].BaseValue, 
            base.Owner.Creature, 
            this
        );
    }
}