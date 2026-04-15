using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Powers;

public sealed class DrawPower : AbstractHiroPower
{
    private sealed class InternalData
    {
        public bool Armed;
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object? InitInternalData()
    {
        return new InternalData();
    }

    public override Task AfterEnergyReset(Player player)
    {
        if (Owner?.Player == null || player != Owner.Player)
        {
            return Task.CompletedTask;
        }

        GetInternalData<InternalData>().Armed = true;
        return Task.CompletedTask;
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (Owner?.Player == null || player != Owner.Player)
        {
            return count;
        }

        InternalData data = GetInternalData<InternalData>();
        if (!data.Armed)
        {
            return count;
        }

        return count + Amount;
    }

    public override Task AfterModifyingHandDraw()
    {
        InternalData data = GetInternalData<InternalData>();
        if (!data.Armed)
        {
            return Task.CompletedTask;
        }

        data.Armed = false;
        Flash();
        RemoveInternal();
        return Task.CompletedTask;
    }
}