using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Hiro.Scripts.Cards;

namespace Hiro.Scripts.Powers;

public sealed class Shipo : AbstractHiroPower
{
    private class Data
    {
        public AttackCommand? commandToModify;
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [new DynamicVar("BonusPercent", 0m)];

    protected override object InitInternalData() => new Data();

    public override Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this)
        {
            DynamicVars["BonusPercent"].BaseValue = Amount * 10m;
        }
        return Task.CompletedTask;
    }

    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.ModelSource is not CardModel card || card.Owner.Creature != Owner || card.Type != CardType.Attack)
        {
            return Task.CompletedTask;
        }

        if (Owner.HasPower<ShipoLockPower>())
        {
            return Task.CompletedTask;
        }

        Data data = GetInternalData<Data>();
        if (data.commandToModify == null)
        {
            data.commandToModify = command;
        }
        return Task.CompletedTask;
    }

public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (cardSource == null || cardSource.Owner.Creature != Owner || !props.IsPoweredAttack() || Owner.HasPower<ShipoLockPower>())
        {
            return 1m;
        }

        Data data = GetInternalData<Data>();
        if (data.commandToModify == null || cardSource == data.commandToModify.ModelSource)
        {
            decimal perStackBonus = 0.1m;
            if (cardSource is Cuilianhuogun cuilianhuogun)
            {
                perStackBonus *= (1m + cuilianhuogun.DynamicVars["ShipoBonusRate"].BaseValue / 100m);
            }
            return 1m + (Amount * perStackBonus);
        }

        return 1m;
    }
    public override async Task AfterAttack(AttackCommand command)
    {
        Data data = GetInternalData<Data>();
        if (command == data.commandToModify)
        {
            data.commandToModify = null; 
            if (Amount > 0)
            {
                await PowerCmd.ModifyAmount(this, -Amount, Owner, null);
            }
        }
    }
}