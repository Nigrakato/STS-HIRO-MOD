using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Utils;

public static class PowerHelper
{
    public static readonly List<PowerEntry> SecaiPool = new()
    {
        new(typeof(VigorPower),7),        //活力
        new(typeof(PlatingPower),7),      //覆甲
        new(typeof(SummonNextTurnPower),7),//下回合召唤
        new(typeof(StrengthPower), 5),    //力量
        new(typeof(DexterityPower), 5),   //敏捷
        new(typeof(ThornsPower), 5),      //荆棘
        new(typeof(RetainHandPower),5),   //保留手牌
        new(typeof(ArtifactPower),5),    //人工
        new(typeof(InfiniteBladesPower),3), //无尽刀刃
        new(typeof(RollingBoulderPower),3),//滚石
        new(typeof(VulnerablePower),3),  //易伤
        new(typeof(WeakPower),3),        //虚弱
        new(typeof(DoomPower),3),        //灾厄
        new(typeof(MachineLearningPower),3),//机器学习
        new(typeof(MonologuePower),3),    //独白
        new(typeof(NostalgiaPower),3),   //怀旧
        new(typeof(PrepTimePower),3),   //准备时间
        new(typeof(RitualPower),3),     //仪式
        new(typeof(FocusPower),3),     //集中
        new(typeof(FrailPower),3),    //脆弱
        new(typeof(FurnacePower),3), //熔炉
        new(typeof(MayhemPower),1),    //乱战
        new(typeof(IntangiblePower),1),   //无实体
        new(typeof(MindRotPower),1),      //心灵腐化
        new(typeof(NeurosurgePower),1),   //精神过载
        new(typeof(PyrePower),1),         //薪火之源
        new(typeof(SerpentFormPower),1), //群蛇形态
        new(typeof(ShadowmeldPower),1),  //融入暗影
        new(typeof(SleightOfFleshPower),1), //血肉戏法
        new(typeof(SpeedsterPower),1),    //速行者
        new(typeof(SwordSagePower),1),    //剑圣
        new(typeof(SeekingEdgePower),1),  //追踪之刃
        new(typeof(TheSealedThronePower),1),//封印王座
        new(typeof(CallOfTheVoidPower),1),//虚空之唤
        new(typeof(AccelerantPower),1), //触媒
        new(typeof(AutomationPower),1),  //自动化
        new(typeof(CreativeAiPower),1),  //AI
        new(typeof(ForbiddenGrimoirePower),1), //禁忌魔典
        new(typeof(DemonFormPower),1),   //恶魔形态
        new(typeof(BufferPower),1),   //缓冲
        new(typeof(BarricadePower),1),  //壁垒
        new(typeof(GigantificationPower),1)  //极巨化
    };

    public static readonly List<PowerEntry> HeibaiPool = new()
    {
        new(typeof(VulnerablePower),7),  //易伤
        new(typeof(WeakPower),7),        //虚弱
        new(typeof(VigorPower),7),        //活力
        new(typeof(PlatingPower),5),      //覆甲
        new(typeof(DebilitatePower),5),  //摧残
        new(typeof(PoisonPower),3),   //中毒
        new(typeof(DemisePower),3),  //消亡
        new(typeof(DoomPower),3),        //灾厄
        new(typeof(StrengthPower), 1),    //力量
        new(typeof(IntangiblePower),1),   //无实体
        new(typeof(ArtifactPower),1),    //人工
        new(typeof(BufferPower),1),   //缓冲
    };

    public class PowerEntry
    {
        public Type Type { get; }
        public int Weight { get; }

        public PowerEntry(Type type, int weight)
        {
            Type = type;
            Weight = weight;
        }
    }

    public static int GetTotalWeight(List<PowerEntry> pool)
    {
        return pool.Sum(p => p.Weight);
    }

    public static Type GetRandomWeighted(List<PowerEntry> pool, int roll)
    {
        int cumulative = 0;

        foreach (var entry in pool)
        {
            cumulative += entry.Weight;
            if (roll < cumulative)
                return entry.Type;
        }

        return pool[0].Type;
    }

    public static async Task ApplyPowerDynamic(
        PlayerChoiceContext ctx,
        Type powerType,
        Creature source,
        Creature target,
        CardModel? cardSource)
    {
        bool isEnemyTarget = target.Side == CombatSide.Enemy;
        bool isProblematicPower = powerType == typeof(PlatingPower) || powerType == typeof(ArtifactPower);

        if (isEnemyTarget && isProblematicPower)
        {
            var applyMethod = typeof(PowerCmd)
                .GetMethods()
                .First(m =>
                {
                    if (m.Name != "Apply" || !m.IsGenericMethod)
                        return false;
                    var ps = m.GetParameters();
                    return ps.Length == 5
                        && ps[0].ParameterType == typeof(Creature)
                        && ps[1].ParameterType == typeof(decimal)
                        && ps[2].ParameterType == typeof(Creature)
                        && ps[3].ParameterType == typeof(CardModel)
                        && ps[4].ParameterType == typeof(bool);
                });

            var applyGeneric = applyMethod.MakeGenericMethod(powerType);
            var applyTask = (Task)applyGeneric.Invoke(null, new object?[] { target, 0m, source, cardSource, false })!;
            await applyTask;

            var setAmountMethod = typeof(PowerCmd)
                .GetMethods()
                .First(m =>
                {
                    if (m.Name != "SetAmount" || !m.IsGenericMethod)
                        return false;
                    var ps = m.GetParameters();
                    return ps.Length == 4 
                        && ps[0].ParameterType == typeof(Creature)
                        && ps[1].ParameterType == typeof(decimal)
                        && ps[2].ParameterType == typeof(Creature)
                        && ps[3].ParameterType == typeof(CardModel);
                });

            var setAmountGeneric = setAmountMethod.MakeGenericMethod(powerType);
            var setAmountTask = (Task)setAmountGeneric.Invoke(null, new object?[] { target, 1m, source, cardSource })!;
            await setAmountTask;
            return;
        }

        var method = typeof(PowerCmd)
            .GetMethods()
            .First(m =>
            {
                if (m.Name != "Apply" || !m.IsGenericMethod)
                    return false;
                var ps = m.GetParameters();
                return ps.Length == 5
                    && ps[0].ParameterType == typeof(Creature)
                    && ps[1].ParameterType == typeof(decimal)
                    && ps[2].ParameterType == typeof(Creature)
                    && ps[3].ParameterType == typeof(CardModel)
                    && ps[4].ParameterType == typeof(bool);
            });

        var generic = method.MakeGenericMethod(powerType);
        var result = generic.Invoke(null, new object?[] { target, 1m, source, cardSource, false })!;
        if (result is Task task)
        {
            await task;
        }
    }
}