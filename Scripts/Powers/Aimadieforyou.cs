using BaseLib.Extensions;
using Hiro.Scripts.Relics;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Powers
{
    public  class MyOstyDieForYouPower : AbstractHiroPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        public override bool ShouldPlayVfx => false;

        public override Creature ModifyUnblockedDamageTarget(Creature target, decimal _, ValueProp props, Creature? __)
        {
            if (target != base.Owner.PetOwner?.Creature)
                return target;
            if (base.Owner.IsDead)
                return target;
            if (!props.IsPoweredAttack_())
                return target;

            return base.Owner;
        }

        public override bool ShouldAllowHitting(Creature creature)
        {
            return creature.IsAlive;
        }

        public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
        {
            return creature != base.Owner;
        }

        public override bool ShouldPowerBeRemovedAfterOwnerDeath()
        {
            return false;
        }
    }
}