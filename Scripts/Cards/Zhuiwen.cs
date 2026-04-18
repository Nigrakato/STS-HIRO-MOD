using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.HiroVar;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public class Zhuiwen : AbstractHiroCard
    {
        protected override bool HasEnergyCostX => true;

        public Zhuiwen() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi,CardKeyword.Retain];

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new ShipoVar(6m)
        ];

        protected override void OnUpgrade()
        {
            DynamicVars[ShipoVar.Key].UpgradeValueBy(1m);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

            int xValue = ResolveEnergyXValue();

            if (xValue > 0)
            {
                decimal totalShipo = xValue * DynamicVars[ShipoVar.Key].BaseValue;

                await PowerCmd.Apply<Shipo>(
                    Owner.Creature, 
                    totalShipo, 
                    Owner.Creature, 
                    this
                );
            }
        }
    }
}
