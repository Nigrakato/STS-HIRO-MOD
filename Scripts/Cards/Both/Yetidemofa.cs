using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Cards;

public sealed class Yetidemofa : AbstractHiroCard
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public Yetidemofa()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
    {

    }
        public override IEnumerable<CardKeyword> CanonicalKeywords => 
        [
            CardKeyword.Exhaust
        ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await base.OnPlay(choiceContext, cardPlay);

        IEnumerable<Player> allPlayers = base.CombatState!.Players.Where(p => p.Creature is { IsAlive: true });

        foreach (Player player in allPlayers)
        {
            for (int i = 0; i < 2; i++)
            {
                CardModel secai = CombatState!.CreateCard<Secai>(player);

                if (base.IsUpgraded)
                {
                    CardCmd.Upgrade(secai);
                }

                await CardPileCmd.AddGeneratedCardToCombat(
                    secai,
                    PileType.Hand,
                    addedByPlayer: true
                );
            }
        }
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromCard<Secai>(),

    ];
    protected override void OnUpgrade()
    {
    }
}