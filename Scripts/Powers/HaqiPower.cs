using Hiro.Scripts.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Powers;

public class HaqiPower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Haqi>();

    protected override bool IsPositive => false;
}