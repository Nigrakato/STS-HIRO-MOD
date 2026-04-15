using System.Threading.Tasks;
using BaseLib.Utils;
using Hiro.Scripts.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Hiro.Scripts.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class Heibai : AbstractHiroCard
{
    public Heibai()
        : base(0, CardType.Skill, CardRarity.Token, TargetType.AnyEnemy, true)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Draw", 1m) 
    ];

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var rng = Owner.RunState.Rng.CombatCardGeneration;

        var target = cardPlay.Target;
        if (target == null)
            return;

        int times = IsUpgraded ? 2 : 1;

        for (int i = 0; i < times; i++)
        {
            int totalWeight = PowerHelper.GetTotalWeight(PowerHelper.HeibaiPool);
            int roll = rng.NextInt(totalWeight);
            var type = PowerHelper.GetRandomWeighted(PowerHelper.HeibaiPool, roll);

            await PowerHelper.ApplyPowerDynamic(
                ctx,
                type,
                Owner.Creature,
                target,
                this
            );
        }

        int drawCount = DynamicVars["Draw"].IntValue;
        await CardPileCmd.Draw(ctx, drawCount, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Draw"].UpgradeValueBy(1m);
    }
}