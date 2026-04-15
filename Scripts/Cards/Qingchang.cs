using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Cards
{
    public class Qingchang : AbstractHiroCard
    {
        public Qingchang() : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,CardKeyword.Retain];



        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            List<Creature> enemies = CombatManager.Instance.DebugOnlyGetState()!.Enemies.ToList();

            foreach (Creature creature in enemies)
            {
                if (creature.IsAlive && creature.GetPower<MinionPower>() != null)
                {
                    await CreatureCmd.Kill(creature);
                }
            }
        }
    }
}