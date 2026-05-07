using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public sealed class Huisudemofa : AbstractHiroCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(6m)];
    public Huisudemofa()
        : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {        await base.OnPlay(choiceContext, cardPlay);

        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_bloody_impact");

        await CreatureCmd.Damage(
            choiceContext, 
            base.Owner.Creature, 
            base.DynamicVars.HpLoss.BaseValue, 
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, 
            this
        );

        AbstractRoom currentRoom = base.CombatState!.RunState.CurrentRoom!;
        if (currentRoom is CombatRoom combatRoom)
        {
            combatRoom.AddExtraReward(
                base.Owner, 
                new CardReward(CardCreationOptions.ForRoom(base.Owner, combatRoom.RoomType), 3, base.Owner)
            );
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.HpLoss.UpgradeValueBy(-3m);
    }
}