using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiro.Scripts.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards; 

public sealed class Chengshengzhuiji : AbstractHiroCard
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(9m, ValueProp.Move)
    ];

    protected override bool ShouldGlowGoldInternal => WasLastCardPlayedAttack;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain,HiroCardKeywords.Zhengyi];


private bool WasLastCardPlayedAttack
{
    get
    {
        var history = CombatManager.Instance?.History;
        if (history == null) return false;


        CardPlayStartedEntry? lastEntry = history.CardPlaysStarted.LastOrDefault(
            (CardPlayStartedEntry e) => e.CardPlay.Card.Owner == base.Owner 
                                     && e.HappenedThisTurn(base.CombatState) 
                                     && e.CardPlay.Card != this);
        
        if (lastEntry == null)
        {
            return false;
        }

        return lastEntry.CardPlay.Card.Type == CardType.Attack;
    }
}

    public Chengshengzhuiji()
        : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Attack", base.Owner.Character.AttackAnimDelay);

        int hitCount = WasLastCardPlayedAttack ? 2 : 1;

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .WithHitCount(hitCount)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}