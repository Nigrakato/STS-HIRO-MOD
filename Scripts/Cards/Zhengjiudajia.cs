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
    public class Zhengjiudajia : AbstractHiroCard
    {
        public Zhengjiudajia() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Error];


        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(1m, ValueProp.Move),
            new RepeatVar(12)
        };

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars["Repeat"].IntValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromPower<KillImpulsePower>(),
    HoverTipFactory.FromPower<WitchFormPower>()

    ];
    }
}