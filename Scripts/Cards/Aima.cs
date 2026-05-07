using BaseLib.Utils;
using Godot;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Hiro.Scripts.Cards
{
    [Pool(typeof(HiroCardPool))]
    public class Aima : AbstractHiroCard
    {
        public Aima() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("Draw", 4m),
            new DynamicVar("Power", 3m),
            new EnergyVar(4) 

        ];

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            await PlayerCmd.GainEnergy(4, Owner);
            await CardPileCmd.Draw(choiceContext, DynamicVars["Draw"].IntValue, Owner);
            await PowerCmd.Apply<DiantineiPower>(Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, this);
        }
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<KillImpulsePower>(),
        HoverTipFactory.FromPower<WitchFormPower>()

    ];
    }
    
}