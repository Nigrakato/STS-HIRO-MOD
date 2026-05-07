using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public class Nuoyaxifa : AbstractHiroCard
    {

        public Nuoyaxifa() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }


        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DynamicVar("NuoyaxifaPower",6m)];

        protected override void OnUpgrade()
        {
        base.EnergyCost.UpgradeBy(-1);

        }


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            await PowerCmd.Apply<NuoyaxifaPower>(
                Owner.Creature,
                DynamicVars["NuoyaxifaPower"].BaseValue,
                Owner.Creature,
                this
            );
        }
    }
}