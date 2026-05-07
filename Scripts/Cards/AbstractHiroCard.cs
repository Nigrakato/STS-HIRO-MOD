using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Pools;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

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

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var creature = Owner?.Creature;
        if (creature != null && !creature.IsDead) 
        {
            if (CanonicalKeywords.Contains(HiroCardKeywords.Zhengyi))
            {
                await PowerCmd.Apply<Justice>(creature, 1m, creature, this);
            }

            if (CanonicalKeywords.Contains(HiroCardKeywords.Error))
            {
                string cardId = Id?.Entry ?? string.Empty;
                if (!IsTemporaryChoiceCard(cardId))
                {
                    await KillImpulsePower.GainStacks(context, creature, 2m, creature, this);
                }
            }
        }

        await base.OnPlay(context, cardPlay);
    }

    private bool IsTemporaryChoiceCard(string cardId)
    {
        if (string.IsNullOrEmpty(cardId)) return true;
        string lowerId = cardId.ToLowerInvariant();
        return lowerId.Contains("picnic") || lowerId.Contains("movie") || lowerId.Contains("game");
    }
}