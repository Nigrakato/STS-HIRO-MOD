using Hiro.Scripts.Cards;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps; 

namespace Hiro.Scripts.Cards;

public sealed class Wucaibanlandehei : AbstractHiroCard
{
    private const string _hitsKey = "ColorHits";

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(2m, ValueProp.Move),

        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),

        new CalculatedVar(_hitsKey).WithMultiplier((card, _) =>
            CombatManager.Instance.History.CardPlaysFinished.Count(e =>
                e.HappenedThisTurn(card.CombatState) &&
                e.CardPlay.Card.Owner == card.Owner &&
                (
                    e.CardPlay.Card is Secai ||
                    e.CardPlay.Card is Heibai   
                )
            ))
    };

    public Wucaibanlandehei()
        : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        int hits = (int)((CalculatedVar)DynamicVars[_hitsKey]).Calculate(null);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(hits)
            .FromCard(this)
            .TargetingAllOpponents(CombatState!)
            .Execute(ctx);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}