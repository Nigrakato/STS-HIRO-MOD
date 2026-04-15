using System.Collections.Generic;
using System.Threading.Tasks;
using Hiro.Scripts.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hiro.Scripts.Cards;

public class Fangyuxingbianbo : AbstractHiroCard
{
    public Fangyuxingbianbo() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [HiroCardKeywords.Zhengyi];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6, ValueProp.Move),
        new DynamicVar("BlockPerEnemy", 4)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IReadOnlyList<Creature> hittableEnemies = CombatState!.HittableEnemies;
        int enemyCount = hittableEnemies.Count;

        decimal blockAmount = DynamicVars.Block.BaseValue
            + enemyCount * DynamicVars["BlockPerEnemy"].BaseValue;

        await CreatureCmd.GainBlock(
            Owner.Creature,
            new BlockVar(blockAmount, ValueProp.Move),
            cardPlay
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars["BlockPerEnemy"].UpgradeValueBy(1);
    }
}