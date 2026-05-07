using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public sealed class Haohaiziaima : AbstractHiroCard
{
    public Haohaiziaima() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(2m, ValueProp.Move),
        new RepeatVar(4) 
    };

    protected override void OnUpgrade()
    {
        base.DynamicVars["Repeat"].UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {        await base.OnPlay(choiceContext, cardPlay);

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .WithHitCount(base.DynamicVars["Repeat"].IntValue) 
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState!)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);

        var secaiList = new List<CardModel>
        {
            base.CombatState!.CreateCard<Secai>(base.Owner),
            base.CombatState.CreateCard<Secai>(base.Owner)
        };
        await CardPileCmd.AddGeneratedCardsToCombat(secaiList, PileType.Hand, addedByPlayer: true);
    }
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Secai>(),

    ];
}