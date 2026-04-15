using Hiro.Scripts.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Cards  
{
    public class Kantou : AbstractHiroCard
    {
        private const int energyCost = 2;
        private const CardType type = CardType.Skill;
        private const CardRarity rarity = CardRarity.Rare;  
        private const TargetType targetType = TargetType.AnyEnemy;
        private const bool showInCardLibrary = true;

  

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new PowerVar<StrengthPower>(2m)
        };

        
        public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi,CardKeyword.Exhaust];


        public Kantou() : base(energyCost, type, rarity, targetType, showInCardLibrary)
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target != null)
            {
                var strengthPower = target.GetPower<StrengthPower>();
                if (strengthPower != null)
                {
                    await PowerCmd.Remove(strengthPower);
                }
            }

            await PowerCmd.Apply<StrengthPower>(Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            
            DynamicVars.Strength.UpgradeValueBy(1); 
        }
    }
}