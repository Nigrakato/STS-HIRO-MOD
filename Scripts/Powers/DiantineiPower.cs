using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hiro.Scripts.Powers;

public class DiantineiPower : AbstractHiroPower
{
    public const int GainAmountPerTurn = 3;

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Gain", GainAmountPerTurn)
    ];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner == null || Owner.IsDead || Owner.Player != player)
        {
            return;
        }

        Flash();

        await KillImpulsePower.GainStacks(
            choiceContext,
            Owner,
            GainAmountPerTurn,
            Owner,
            null
        );
    }
}