using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.HiroVar;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public sealed class Shijianhuisu : AbstractHiroCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new ShayiVar(2),
            new EnergyVar(2)
        ];

        public Shijianhuisu()
            : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await KillImpulsePower.GainStacks(
                choiceContext,
                Owner.Creature,
                DynamicVars[ShayiVar.Key].BaseValue,
                Owner.Creature,
                this
            );

            await PlayerCmd.GainEnergy(
                DynamicVars["Energy"].IntValue,
                Owner
            );

            CardModel card = CombatState!.CreateCard<Lengjing>(Owner);
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Discard, addedByPlayer: true)
            );
            await Cmd.Wait(0.5f);
        }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Lengjing>(), 
        HoverTipFactory.FromPower<WitchFormPower>()
    ];
        protected override void OnUpgrade()
        {
            DynamicVars["Energy"].UpgradeValueBy(1);
        }
    }
}