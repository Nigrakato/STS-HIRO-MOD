using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hiro.Scripts.HiroVar;

public class ShayiVar : DynamicVar
{
    public const string Key = "Hiro-Shayi";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public ShayiVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}