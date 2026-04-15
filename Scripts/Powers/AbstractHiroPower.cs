using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Hiro.Scripts.Powers;

public abstract class AbstractHiroPower: CustomPowerModel
{
	public abstract override PowerType Type { get; }
	public abstract override  PowerStackType StackType { get; }

	public override string CustomPackedIconPath => $"res://Hiro/images/powers/{Id.Entry.ToLowerInvariant()}.png";

	public override string CustomBigIconPath => $"res://Hiro/images/powers/{Id.Entry.ToLowerInvariant()}.png";

	public override string CustomBigBetaIconPath => $"res://Hiro/images/powers/{Id.Entry.ToLowerInvariant()}.png";
}
