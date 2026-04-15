using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public class Caicaibei : AbstractHiroCard
    {
        public Caicaibei() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(8m, ValueProp.Move),
            new EnergyVar(1)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<EnergyNextTurnPower>(Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Energy.UpgradeValueBy(1);
        }
    }
}