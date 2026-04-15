using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hiro.Scripts.HiroVar;

public class ShipoVar : DynamicVar
{
    public const string Key = "Hiro-Shipo";
    public static readonly string LocKey = Key.ToUpperInvariant();

    public ShipoVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }
} 