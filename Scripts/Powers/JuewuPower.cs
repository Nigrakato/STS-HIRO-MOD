using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Hiro.Scripts.Powers;

public sealed class JuewuPower : AbstractHiroPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == base.Owner.Side)
        {
            Flash(); 
            await PowerCmd.Apply<Justice>(base.Owner, (decimal)base.Amount, base.Owner, null);
        }
    }
}