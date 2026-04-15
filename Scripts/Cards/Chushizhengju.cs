using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Cards;

public sealed class Chushizhengju : AbstractHiroCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new RepeatVar(1)  
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public Chushizhengju()
        : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1)
        {
            PretendCardsCanBePlayed = true
        };

        var selected = await CardSelectCmd.FromHand(
            choiceContext, 
            base.Owner, 
            prefs, 
            (CardModel c) => (c.Type == CardType.Attack || c.Type == CardType.Skill) 
                          && !c.Keywords.Contains(CardKeyword.Unplayable), 
            this);

        CardModel? card = selected.FirstOrDefault();
        if (card is not null)
        {
            for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
            {
                await CardCmd.AutoPlay(choiceContext, card, null);
            }

            await CardCmd.Exhaust(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Repeat.UpgradeValueBy(1m);
    }
}