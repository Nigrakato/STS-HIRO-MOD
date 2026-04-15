using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiro.Scripts.HiroVar;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public class Siqudehuiyi : AbstractHiroCard
{
public Siqudehuiyi() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
}


protected override IEnumerable<DynamicVar> CanonicalVars => [
    new ShipoVar(3),
    new BlockVar(3, ValueProp.Move),

];

protected override void OnUpgrade()
{
    base.OnUpgrade();
    DynamicVars[ShipoVar.Key].UpgradeValueBy(2);
    DynamicVars.Block.UpgradeValueBy(1m);

}

protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
{
    await PowerCmd.Apply<Shipo>(Owner.Creature, DynamicVars[ShipoVar.Key].BaseValue, Owner.Creature, this);

    CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
    CardPile pile = PileType.Discard.GetPile(Owner);
    CardModel cardModel = (await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, Owner, prefs)).FirstOrDefault()!;
    if (cardModel != null)
    {
        await CardPileCmd.Add(cardModel, PileType.Hand);
    }
}}