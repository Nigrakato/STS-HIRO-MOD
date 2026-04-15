using BaseLib.Abstracts;
using Godot;

namespace Hiro.Scripts.Pools;

public class HiroCardPool : CustomCardPoolModel
{
	// 卡池的ID。必须唯一防撞车。
	public override string Title => "Hiro";
	public override string EnergyColorName => "hiro";

	public override string? BigEnergyIconPath => "res://Hiro/images/ui/energy_hiro.png";

	public override string? TextEnergyIconPath => "res://Hiro/images/ui/smallenergy_hiro.png";
	public override Color DeckEntryCardColor =>new Color(0.859f, 0f, 0.071f);
	public override Color ShaderColor => new Color(0.859f, 0f, 0.071f);

	public override Color EnergyOutlineColor =>new Color(0.859f, 0f, 0.071f);

	// 卡池是否是无色。例如事件、状态等卡池就是无色的。
	public override bool IsColorless => false;


}
