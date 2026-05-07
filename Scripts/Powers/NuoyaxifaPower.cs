using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Hiro.Scripts.Cards;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Hiro.Scripts.Powers;

public sealed class NuoyaxifaPower : AbstractHiroPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Amount <= 0)
            return;

        if (cardPlay.Card is not Secai && cardPlay.Card is not Heibai)
            return;

        var hittableEnemies = base.CombatState.HittableEnemies;
        if (hittableEnemies.Count == 0)
            return;

        var target = base.Owner.Player!.RunState.Rng.CombatTargets.NextItem(hittableEnemies);

        Flash();
        await CreatureCmd.Damage(context, target!, base.Amount, ValueProp.Unpowered, base.Owner, null);    }
}