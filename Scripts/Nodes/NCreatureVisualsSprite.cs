using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Hiro.Scripts.Nodes;

/// <summary>
/// NCreatureVisuals 的纯图片动画子类。
/// 不依赖 Spine，使用 AnimatedSprite2D 实现待机/死亡动画。
/// 
/// 场景结构要求：
///   根节点（本类） Node2D
///   ├── %Visuals (Node2D)
///   │   ├── AnimatedSprite2D    ← 包含 "idle" 和 "dead" 两个 Animation
///   │   └── Bounds (Control)   ← UpdateBounds 依赖此节点
///   ├── %Bounds (Control)
///   ├── %IntentPos (Marker2D)
///   └── %CenterPos (Marker2D)
/// </summary>
public partial class NCreatureVisualsSprite : NCreatureVisuals
{
	// 动画名称常量，和你在 AnimatedSprite2D 里定义的动画名对应
	private const string ANIM_IDLE = "idle";
	private const string ANIM_DEAD = "dead";

	// 我们自己的 AnimatedSprite2D 引用
	private AnimatedSprite2D? _sprite;

	// 当前是否处于死亡状态（死亡后不允许 idle 覆盖）
	private bool _isDead = false;

	public override void _Ready()
	{
		// 必须先调用父类 _Ready()
		// 父类会初始化 Body、Bounds、IntentPosition 等所有属性
		// 因为没有 SpineSprite，HasSpineAnimation 返回 false，SpineBody 保持 null
		base._Ready();

		// 父类 _Ready() 完成后，Body（即 %Visuals 节点）已经就绪
		// 现在在 Body 下查找 AnimatedSprite2D
		_sprite = Body.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");

		if (_sprite == null)
		{
			GD.PushWarning(
				$"[NCreatureVisualsSprite] 在 {Body.GetPath()} 下未找到 AnimatedSprite2D！" +
                "请检查场景结构，确保 %Visuals 节点下有名为 AnimatedSprite2D 的子节点。"
			);
			return;
		}

		// 绑定死亡动画完成事件：死亡动画播放完后停在最后一帧
		_sprite.AnimationFinished += OnAnimationFinished;

		// 启动时默认播放待机
		PlayAnimation(ANIM_IDLE);
	}

	/// <summary>
	/// 外部（NCreature）调用此方法来驱动图片动画状态机。
	/// 触发器字符串与 Spine 版本保持语义一致，方便后续统一管理。
	/// </summary>
	public void SetSpriteAnimationTrigger(string trigger)
	{
		// 死亡是单向状态，一旦进入不可被其他触发器覆盖
		// （除非是 Revive）
		if (_isDead && trigger != "Revive")
			return;

		switch (trigger)
		{
			case "Idle":
				_isDead = false;
				PlayAnimation(ANIM_IDLE);
				break;

			case "Dead":
				_isDead = true;
				PlayAnimation(ANIM_DEAD);
				break;

			case "Revive":
				_isDead = false;
				PlayAnimation(ANIM_IDLE);
				break;

			// "hurt" 等其他触发器：如果你没有对应图片，忽略即可
			// 如果将来想加受击闪烁效果，在这里扩展
			default:
				// 静默忽略未知触发器，不报错
				break;
		}
	}

	/// <summary>
	/// 查询当前是否正在播放某个动画。
	/// 供外部（比如仿 IsPlayingHurtAnimation）调用。
	/// </summary>
	public bool IsPlayingAnimation(string animName)
	{
		if (_sprite == null) return false;
		return _sprite.IsPlaying() && _sprite.Animation == animName;
	}

	// ─────────────────────────────────────────
	// 私有方法
	// ─────────────────────────────────────────

	private void PlayAnimation(string animName)
	{
		if (_sprite == null) return;

		// 避免重复播放相同动画（防止待机动画被每帧重置）
		if (_sprite.Animation == animName && _sprite.IsPlaying())
			return;

		_sprite.Play(animName);
	}

	private void OnAnimationFinished()
	{
		if (_sprite == null) return;

		// 死亡动画播放完毕后，停在最后一帧
		// Godot 的 AnimatedSprite2D 在非循环动画结束后会自动停止
		// 我们只需确保它停在最后一帧（Frame 不重置）
		if (_sprite.Animation == ANIM_DEAD)
		{
			// 停止播放但保持当前帧（最后一帧）显示
			_sprite.Stop();
			// 手动定位到最后一帧，防止某些版本重置到第0帧
			var frameCount = _sprite.SpriteFrames?.GetFrameCount(ANIM_DEAD) ?? 1;
			_sprite.Frame = frameCount - 1;
		}
	}
}
