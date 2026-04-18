using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Hiro.Scripts.Vars
{
    public sealed class AimaSummonVar : DynamicVar
    {
        public const string DefaultName = "AimaSummon";

        public AimaSummonVar(decimal summonAmount)
            : base(DefaultName, summonAmount)
        {
        }

        public AimaSummonVar(string name, decimal summonAmount)
            : base(name, summonAmount)
        {
        }

        public override void UpdateCardPreview(
            CardModel card,
            CardPreviewMode previewMode,
            Creature? target,
            bool runGlobalHooks)
        {
            if (runGlobalHooks)
            {
                PreviewValue = Hook.ModifySummonAmount(
                    card.CombatState!,
                    card.Owner,
                    BaseValue,
                    card
                );
            }
            else
            {
                PreviewValue = BaseValue;
            }
        }
    }
}