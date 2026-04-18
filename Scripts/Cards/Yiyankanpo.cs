using Hiro.Scripts.HiroVar;
using Hiro.Scripts.Keywords;
using Hiro.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hiro.Scripts.Cards;

public class Yiyankanpo : AbstractHiroCard
{
    public Yiyankanpo() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi,CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ShipoVar(10)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars[ShipoVar.Key].UpgradeValueBy(4);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<Shipo>(
            Owner.Creature,
            DynamicVars[ShipoVar.Key].BaseValue,
            Owner.Creature,
            this
        );
    }

} 