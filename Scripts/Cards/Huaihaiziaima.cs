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

public sealed class Huaihaiziaima : AbstractHiroCard
{
    public Huaihaiziaima() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new[] { new DamageVar(7m, ValueProp.Move) };

    protected override void OnUpgrade() => base.DynamicVars.Damage.UpgradeValueBy(2m);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState!)
            .Execute(choiceContext);

        var heibaiList = new List<CardModel> 
        { 
            base.CombatState!.CreateCard<Heibai>(base.Owner), 
        };
        await CardPileCmd.AddGeneratedCardsToCombat(heibaiList, PileType.Hand, addedByPlayer: true);
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Heibai>(),

    ];
}