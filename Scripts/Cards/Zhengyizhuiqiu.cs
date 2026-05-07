using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Cards;
using Hiro.Scripts.Powers;
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
                PreviewValue = 10 + 3 * justice;
            }
        }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public Zhengyizhuiqiu()
            : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new JusticeDamageVar(0m, ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {        await base.OnPlay(choiceContext, cardPlay);

            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            int justice = Owner.Creature.GetPowerAmount<Justice>();
            int finalDamage = 10 + 3 * justice;

            await DamageCmd.Attack(finalDamage)
                .FromCard(this)
                .Targeting(cardPlay.Target!)
                .Execute(choiceContext);

            if (justice > 0)
            {
                await PowerCmd.Apply<Justice>(Owner.Creature, -justice, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
                    base.EnergyCost.UpgradeBy(-1);

        }
    }
}