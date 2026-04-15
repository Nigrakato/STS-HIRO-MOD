using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public sealed class Shanliangdeaima : AbstractHiroCard
{
    public override bool GainsBlock => true;

    public Shanliangdeaima() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[] { new DamageVar(13m, ValueProp.Move) };

    protected override void OnUpgrade() => base.DynamicVars.Damage.UpgradeValueBy(2m);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        decimal blockAmount = attackCommand.Results.Sum(r => r.TotalDamage + r.OverkillDamage);
        
        await CreatureCmd.GainBlock(base.Owner.Creature, blockAmount, ValueProp.Move, cardPlay);
    }
}