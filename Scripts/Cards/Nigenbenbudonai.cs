using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public class HeibaiCountVar : DynamicVar
{
    public HeibaiCountVar() : base("heibaicount", 0m) { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        int count = card.Owner?.PlayerCombatState?.ExhaustPile.Cards.Count(c => c is Heibai) ?? 0;
        

        PreviewValue = count;
    }
}

public sealed class Nigenbenbudongai : AbstractHiroCard
{
    private int HeibaiCount => Owner.PlayerCombatState?.ExhaustPile.Cards.Count(c => c is Heibai) ?? 0;

    public Nigenbenbudongai() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(9m, ValueProp.Move),
        new RepeatVar(1),
        new HeibaiCountVar() 
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Heibai>()
    ];

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        int totalHits = base.DynamicVars["Repeat"].IntValue + HeibaiCount;

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .WithHitCount(totalHits) 
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
}