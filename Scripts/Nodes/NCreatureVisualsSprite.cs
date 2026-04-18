using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Hiro.Scripts.Nodes;

public partial class NCreatureVisualsSprite : NCreatureVisuals
{
	private const string ANIM_IDLE = "idle";
	private const string ANIM_DEAD = "dead";

	private AnimatedSprite2D? _sprite;
	private bool _isDead = false;

	public override void _Ready()
	{
		base._Ready();

		// ✅ 修复：用 GetCurrentBody() 代替原来的 Body
		var currentBody = GetCurrentBody();
		_sprite = currentBody.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");

		if (_sprite == null)
		{
			GD.PushWarning(
				$"[NCreatureVisualsSprite] 在 {currentBody.GetPath()} 下未找到 AnimatedSprite2D！" +
                "请检查场景结构，确保 %Visuals 节点下有名为 AnimatedSprite2D 的子节点。"
			);
			return;
		}

		_sprite.AnimationFinished += OnAnimationFinished;
		PlayAnimation(ANIM_IDLE);
	}

	public void SetSpriteAnimationTrigger(string trigger)
	{
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

			default:
				break;
		}
	}

	public bool IsPlayingAnimation(string animName)
	{
		if (_sprite == null) return false;
		return _sprite.IsPlaying() && _sprite.Animation == animName;
	}

	private void PlayAnimation(string animName)
	{
		if (_sprite == null) return;

		if (_sprite.Animation == animName && _sprite.IsPlaying())
			return;

		_sprite.Play(animName);
	}

	private void OnAnimationFinished()
	{
		if (_sprite == null) return;

		if (_sprite.Animation == ANIM_DEAD)
		{
			_sprite.Stop();
			var frameCount = _sprite.SpriteFrames?.GetFrameCount(ANIM_DEAD) ?? 1;
			_sprite.Frame = frameCount - 1;
		}
	}
}
