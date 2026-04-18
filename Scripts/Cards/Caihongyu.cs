using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
namespace Hiro.Scripts.Cards;

public sealed class Caihongyu : AbstractHiroCard
{
    private const string _hitsKey = "SecaiHits";

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(3m, ValueProp.Move),

        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),

        new CalculatedVar(_hitsKey).WithMultiplier((card, _) =>
            CombatManager.Instance.History.CardPlaysFinished.Count(e =>
                e.CardPlay.Card.Owner == card.Owner &&
                e.CardPlay.Card is Secai   
            ))
    };

    public  Caihongyu()
        : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);

        int hits = (int)((CalculatedVar)DynamicVars[_hitsKey]).Calculate(play.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(hits)
            .FromCard(this)
            .Targeting(play.Target)
            .Execute(ctx);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
} 