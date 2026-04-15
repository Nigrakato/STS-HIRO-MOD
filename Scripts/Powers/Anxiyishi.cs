using MegaCrit.Sts2.Core.Entities.Powers;

namespace Hiro.Scripts.Powers;

public sealed class AnxiyishiPower : AbstractHiroPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
}