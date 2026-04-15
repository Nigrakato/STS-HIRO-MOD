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
    public class Zhikong : AbstractHiroCard
    {
        public Zhikong() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {
        }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Error];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(12m, ValueProp.Move),
            new DynamicVar("Draw", 1m)
        ];

        protected override void OnUpgrade()
        {
            DynamicVars["Draw"].UpgradeValueBy(1);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);

            await CardPileCmd.Draw(choiceContext, DynamicVars["Draw"].IntValue, Owner);
        }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromPower<KillImpulsePower>(),
    HoverTipFactory.FromPower<WitchFormPower>()

    ];
    }
}