using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Powers;

public class KillImpulsePower : AbstractHiroPower
{
    private class Data
    {
        public bool triggered;
    }

    public const int TriggerThreshold = 10;
    //public const int LoseMaxHpAmount = 5; 
    public const int GainStrengthAmount = 5;
    public const int WitchFormTurns = 6;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool IsInstanced => false;
    
    public override bool AllowNegative => false;

    protected override object InitInternalData() => new Data();

    public bool HasTriggered
    {
        get => GetInternalData<Data>().triggered;
        set => GetInternalData<Data>().triggered = value;
    }

    public void TriggerFlash() => Flash();

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power != this) return;

        if (this.Amount < 0)
        {
            await PowerCmd.SetAmount<KillImpulsePower>(Owner, 0, applier, cardSource);
        }
    }

    public static async Task GainStacks(
        PlayerChoiceContext choiceContext,
        Creature target,
        decimal amount,
        Creature applier,
        CardModel? cardSource)
    {
        if (target.GetPower<WitchFormPower>() != null) return;
        await PowerCmd.Apply<KillImpulsePower>(target, amount, applier, cardSource);

        KillImpulsePower? power = target.GetPower<KillImpulsePower>();
        if (power == null || power.HasTriggered) return;

        if (power.Amount >= TriggerThreshold)
        {
            power.HasTriggered = true;
            await Resolve(choiceContext, target, cardSource);
        }
    }

    public static async Task Resolve(
        PlayerChoiceContext choiceContext,
        Creature target,
        CardModel? cardSource)
    {
        KillImpulsePower? power = target.GetPower<KillImpulsePower>();
        if (power == null) return;

        Player? player = target.Player;
        if (player == null) return;

        power.TriggerFlash();

        await PowerCmd.Apply<StrengthPower>(target, GainStrengthAmount, target, cardSource);
        await WitchFormPower.ApplyWithImmediateEffect(choiceContext, target, WitchFormTurns, cardSource);
        await PowerCmd.ModifyAmount(power, -power.Amount, target, cardSource);
    }
}