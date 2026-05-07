using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.HiroVar;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public sealed class Yuanliang : AbstractHiroCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new ShayiVar(-3m)

        ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi,CardKeyword.Exhaust];

        public Yuanliang() 
            : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            await KillImpulsePower.GainStacks(
                choiceContext,
                Owner.Creature,
                DynamicVars[ShayiVar.Key].BaseValue,
                Owner.Creature,
                this
            );


            
        }

        protected override void OnUpgrade()
        {
    		RemoveKeyword(CardKeyword.Exhaust);
        }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromPower<WitchFormPower>(),

    ];
    }
}