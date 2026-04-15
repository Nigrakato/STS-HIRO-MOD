using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiro.Scripts.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hiro.Scripts.Cards;

public sealed class FanboWoxiangxindajia : AbstractHiroCard
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [base.EnergyHoverTip];
        public override IEnumerable<CardKeyword> CanonicalKeywords => 
        [
            CardKeyword.Exhaust
        ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(3),
        new CardsVar(6)
    ];

    public FanboWoxiangxindajia()
        : base(3, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        IEnumerable<Player> allPlayers = base.CombatState!.Players.Where(p => p.Creature is { IsAlive: true });

        foreach (Player player in allPlayers)
        {
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, player);
            
            await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, player);
        }

        PlayerCmd.EndTurn(base.Owner, canBackOut: false);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Energy.UpgradeValueBy(1m);
        base.DynamicVars.Cards.UpgradeValueBy(1m);
    }
}