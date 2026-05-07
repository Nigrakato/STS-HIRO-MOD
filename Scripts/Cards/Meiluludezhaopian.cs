using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Cards;

public sealed class Meiluludezhaopian : AbstractHiroCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromCard<Regret>(),
        HoverTipFactory.FromPower<IntangiblePower>()
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new PowerVar<IntangiblePower>(1m)
    };

    public Meiluludezhaopian() : base(0, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {        await base.OnPlay(choiceContext, cardPlay);

        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        await PowerCmd.Apply<IntangiblePower>(
            base.Owner.Creature, 
            base.DynamicVars["IntangiblePower"].BaseValue, 
            base.Owner.Creature, 
            this
        );

        var debuffs = base.Owner.Creature.Powers.Where(p => p.Type == PowerType.Debuff).ToList();
        foreach (var debuff in debuffs)
        {
            await PowerCmd.Remove(debuff);
        }

        int currentHandCount = CardPile.GetCards(base.Owner, PileType.Hand).Count();
        int spaceLeft = 10 - currentHandCount;

        if (spaceLeft > 0)
        {
            List<CardModel> regrets = new List<CardModel>();
            for (int i = 0; i < spaceLeft; i++)
            {
                regrets.Add(base.CombatState!.CreateCard<Regret>(base.Owner));
            }
            await CardPileCmd.AddGeneratedCardsToCombat(regrets, PileType.Hand, addedByPlayer: true);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["IntangiblePower"].UpgradeValueBy(1m);
    }    

}