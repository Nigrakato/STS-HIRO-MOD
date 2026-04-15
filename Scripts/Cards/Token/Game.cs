using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Cards
{
    [Pool(typeof(TokenCardPool))]
    public sealed class Game : AbstractHiroCard, IOption
    {
        public Game() : base(-1, CardType.Skill, CardRarity.Token, TargetType.Self) { }

        public override CardPoolModel Pool => ModelDb.CardPool<TokenCardPool>();

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(15)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await OnOptionChosen(choiceContext, cardPlay);
        }

        public async Task OnOptionChosen(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            VfxCmd.PlayOnCreature(Owner.Creature, "vfx/vfx_coin_explosion_regular");
            await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Gold.UpgradeValueBy(5);
        }
    }
}