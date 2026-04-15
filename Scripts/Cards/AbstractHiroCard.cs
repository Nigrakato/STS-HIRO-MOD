using BaseLib.Abstracts;
using BaseLib.Utils;
using Hiro.Scripts.Pools;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Hiro.Scripts.Cards;

[Pool(typeof(HiroCardPool))] 
public abstract class AbstractHiroCard : CustomCardModel
{
    public override string PortraitPath => $"res://Hiro/images/cards/{Id.Entry.ToLowerInvariant()}.png";

    protected AbstractHiroCard(int energyCost, CardType type, CardRarity rarity, TargetType targetType,
        bool showInCardLibrary = true, bool autoAdd = true)
        : base(energyCost, type, rarity, targetType, showInCardLibrary, autoAdd)
    {
    }


}