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

    public sealed class Movie : AbstractHiroCard, IOption
    {
        public Movie() : base(-1, CardType.Power, CardRarity.Token, TargetType.Self) { }

        public override CardPoolModel Pool => ModelDb.CardPool<TokenCardPool>();

       protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<DexterityPower>(2)  
    ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await OnOptionChosen(choiceContext, cardPlay);
        }

        public async Task OnOptionChosen(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            await PowerCmd.Apply<DexterityPower>(Owner.Creature, DynamicVars["DexterityPower"].IntValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["DexterityPower"].UpgradeValueBy(1);
        }
    }
}
