using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using Hiro.Scripts.Powers; 
using Hiro.Scripts.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Hiro.Scripts.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class Secai : AbstractHiroCard
{
    public Secai()
        : base(0, CardType.Skill, CardRarity.Token, TargetType.Self, true)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var rng = Owner.RunState.Rng.CombatCardGeneration;

        int times = IsUpgraded ? 2 : 1;

        bool hasXuehudie = Owner.Creature.HasPower<XuehudiePower>();

        List<Creature> targets = new List<Creature>();
        if (hasXuehudie)
        {
            if (Owner.RunState.Players != null)
            {
                foreach (var player in Owner.RunState.Players)
                {
                    if (player.Creature != null)
                    {
                        targets.Add(player.Creature);
                    }
                }
            }
            
            if (targets.Count == 0)
            {
                targets.Add(Owner.Creature);
            }
        }
        else
        {
            targets.Add(Owner.Creature);
        }

        for (int i = 0; i < times; i++)
        {
            int totalWeight = PowerHelper.GetTotalWeight(PowerHelper.SecaiPool);
            int roll = rng.NextInt(totalWeight);
            var powerType = PowerHelper.GetRandomWeighted(PowerHelper.SecaiPool, roll);

            foreach (var target in targets)
            {
                if (target != null) 
                {
                    await PowerHelper.ApplyPowerDynamic(
                        ctx,
                        powerType,
                        Owner.Creature, 
                        target,         
                        this
                    );
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}