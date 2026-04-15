using Hiro.Scripts.Cards;
using Hiro.Scripts.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Cards
{
    public sealed class Zhengyizhuiqiu : AbstractHiroCard
    {
        private class JusticeDamageVar : DamageVar
        {
            public JusticeDamageVar(decimal baseDamage, ValueProp valueProp) : base(baseDamage, valueProp) { }

            public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
            {
                base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
                
                int justice = card.Owner?.Creature.GetPowerAmount<Justice>() ?? 0;
                this.PreviewValue = base.PreviewValue + justice;
            }
        }

        public Zhengyizhuiqiu() 
            : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new JusticeDamageVar(0m, ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int finalDamage = DynamicVars.Damage.IntValue + Owner.Creature.GetPowerAmount<Justice>();
            
            await DamageCmd.Attack(finalDamage)
                .FromCard(this)
                .Targeting(cardPlay.Target!)
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}