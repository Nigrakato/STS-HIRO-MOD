using Hiro.Scripts.HiroVar;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hiro.Scripts.Cards
{
    public class Fanggun : AbstractHiroCard
    {
        public Fanggun() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi];

        protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new DynamicVar("Draw", 2m),         
            new ShipoVar(3)
        ];

        protected override void OnUpgrade()
        {
            DynamicVars["Draw"].UpgradeValueBy(1);      
            DynamicVars[ShipoVar.Key].UpgradeValueBy(2);     
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int drawCount = DynamicVars["Draw"].IntValue;
            await CardPileCmd.Draw(choiceContext, drawCount, Owner);
            await PowerCmd.Apply<Shipo>(Owner.Creature, DynamicVars[ShipoVar.Key].BaseValue, Owner.Creature, this);
        }
    }
}