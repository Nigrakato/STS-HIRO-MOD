using BaseLib.Abstracts;
using Godot;
using Hiro.Scripts.Cards;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Relics;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Hiro.Scripts.Characters;


public class HiroCharacter : PlaceholderCharacterModel
{
	private const string V = "res://Hiro/voices/weizheng.ogg";

	// 角色名称颜色
	public override Color NameColor => new(0.235f, 0.002f, 0.055f);
	// 能量图标轮廓颜色
	public override Color EnergyLabelOutlineColor => new(0.235f, 0.002f, 0.055f);

	// 人物性别（男女中立）
	public override CharacterGender Gender => CharacterGender.Feminine;

	// 初始血量
	public override int StartingHp => 70;

	// 人物模型tscn路径。要自定义见下。
	public override string CustomVisualPath => "res://Hiro/scenes/hiro_character.tscn";
	// 卡牌拖尾路径。
	// public override string CustomTrailPath => "res://scenes/vfx/card_trail_ironclad.tscn";
	// 人物头像路径。
	public override string CustomIconTexturePath => "res://Hiro/images/hiroicon.png";
	// 人物头像2号。
	public override string CustomIconPath => "res://Hiro/scenes/hiro_icon.tscn";
	// 能量表盘tscn路径。要自定义见下。
	public override string CustomEnergyCounterPath => "res://Hiro/scenes/hiro_energy_counter.tscn";
	// 篝火休息动画。
	public override string CustomRestSiteAnimPath => "res://Hiro/scenes/hiro_rest.tscn";
	// 商店人物动画。
	public override string CustomMerchantAnimPath => "res://Hiro/scenes/hiro_merchant.tscn";
	// 多人模式-手指。
	public override string CustomArmPointingTexturePath => "res://Hiro/images/hand/zhi.png";
	// 多人模式剪刀石头布-石头。

	public override string CustomArmRockTexturePath => "res://Hiro/images/hand/quan.png";
	// 多人模式剪刀石头布-布。
	public override string CustomArmPaperTexturePath => "res://Hiro/images/hand/bu.png";
	// 多人模式剪刀石头布-剪刀。
	public override string CustomArmScissorsTexturePath => "res://Hiro/images/hand/ye.png";

	// 人物选择背景。
	public override string CustomCharacterSelectBg => "res://Hiro/scenes/hiro_bg.tscn";
	// 人物选择图标。
	public override string CustomCharacterSelectIconPath => "res://Hiro/images/hiro_select.png";
	// 人物选择图标-锁定状态。
	public override string CustomCharacterSelectLockedIconPath => "res://Hiro/images/hiro_select_lock.png";
	// 人物选择过渡动画。
	// public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";
	// 地图上的角色标记图标、表情轮盘上的角色头像
	public override string CustomMapMarkerPath => "res://Hiro/images/ui/hiromapmark.png";
	// 攻击音效
	//public override string CustomAttackSfx => null;
	// 施法音效
	// public override string CustomCastSfx => null;
	// 死亡音效
	//public override string CustomDeathSfx => null;
	// 角色选择音效
	public override string CharacterSelectSfx => null;
	// 过渡音效。这个不能删。
	public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

	public override CardPoolModel CardPool => ModelDb.CardPool<HiroCardPool>();
	public override RelicPoolModel RelicPool => ModelDb.RelicPool<HiroRelicPool>();
	public override PotionPoolModel PotionPool => ModelDb.PotionPool<HiroPotionPool>();

	// 初始卡组
	public override IEnumerable<CardModel> StartingDeck => [
	   ModelDb.Card<Strike>(),
	   ModelDb.Card<Strike>(),
	   ModelDb.Card<Strike>(),
	   ModelDb.Card<Strike>(),
	   ModelDb.Card<Defend>(),
	   ModelDb.Card<Defend>(),
	   ModelDb.Card<Defend>(),
	   ModelDb.Card<Defend>(),
	   ModelDb.Card<Shipocard>(),
	   ModelDb.Card<Jianyichangmao>(),
	   // ModelDb.Card<>(),
	];

	// 初始遗物
	public override IReadOnlyList<RelicModel> StartingRelics => [
	   ModelDb.Relic<Pen>(),
	];

	// 攻击建筑师的攻击特效列表
	public override List<string> GetArchitectAttackVfx() => [
		"vfx/vfx_attack_blunt",
		"vfx/vfx_heavy_blunt",
		"vfx/vfx_attack_slash",
		"vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
	];
}
