using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public class Diandianxingxing : AbstractHiroCard
    {
        public Diandianxingxing() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new BlockVar(9m, ValueProp.Move),
            new PowerVar<VigorPower>(2m)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(2m);
            DynamicVars["VigorPower"].UpgradeValueBy(1m);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            
            await PowerCmd.Apply<VigorPower>(Owner.Creature, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
        }
    }
}