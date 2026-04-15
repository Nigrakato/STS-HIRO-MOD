using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Hiro.Scripts.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards
{
    public class Shaonvderichang : AbstractHiroCard
    {
        private const int BlockPerTurn = 5;
        private const int Duration = 3;

        public Shaonvderichang() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
        public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi];


        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("Turns", Duration),
            new BlockVar(BlockPerTurn, ValueProp.Move),
            new EnergyVar(3),
            new PowerVar<DexterityPower>(2),
            new GoldVar(15) 
        };

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new[]
        {
            HoverTipFactory.Static(StaticHoverTip.Block)
        };

        protected override void OnUpgrade()
        {
            DynamicVars["Energy"].UpgradeValueBy(1);
            DynamicVars["DexterityPower"].UpgradeValueBy(1);
            DynamicVars["Gold"].UpgradeValueBy(5); 
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            decimal blockAmount = await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            var power = await PowerCmd.Apply<ToricToughnessPower>(Owner.Creature, DynamicVars["Turns"].BaseValue, Owner.Creature, this);
            power?.SetBlock(blockAmount);

            var picnic = (CardModel)ModelDb.Card<Picnic>().MutableClone();
            var movie = (CardModel)ModelDb.Card<Movie>().MutableClone();
            var game = (CardModel)ModelDb.Card<Game>().MutableClone();

            picnic.DynamicVars["Energy"].BaseValue = DynamicVars["Energy"].IntValue;
            movie.DynamicVars["DexterityPower"].BaseValue = DynamicVars["DexterityPower"].IntValue;
            game.DynamicVars["Gold"].BaseValue = DynamicVars["Gold"].IntValue;

            picnic.Owner = Owner;
            movie.Owner = Owner;
            game.Owner = Owner;

            var selected = await CardSelectCmd.FromChooseACardScreen(
                choiceContext,
                new List<CardModel> { picnic, movie, game },
                Owner,
                false
            );

            if (selected is IOption option)
                await option.OnOptionChosen(choiceContext, cardPlay);
        }
    }
}