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
        new(typeof(VigorPower),100),          //活力
        new(typeof(SummonNextTurnPower),100), //下回合召唤
        new(typeof(StrengthPower), 80),       //力量
        new(typeof(DexterityPower), 80),      //敏捷
        new(typeof(ThornsPower), 70),         //荆棘
        new(typeof(RetainHandPower),70),      //保留手牌
        new(typeof(InfiniteBladesPower),50),  //无尽刀刃
        new(typeof(RollingBoulderPower),30),  //滚石
        new(typeof(VulnerablePower),30),      //易伤
        new(typeof(WeakPower),30),            //虚弱
        new(typeof(DoomPower),30),            //灾厄
        new(typeof(MachineLearningPower),20), //机器学习
        new(typeof(MonologuePower),20),       //独白
        new(typeof(NostalgiaPower),20),       //怀旧
        new(typeof(PrepTimePower),20),        //准备时间
        new(typeof(RitualPower),10),          //仪式
        new(typeof(FocusPower),10),           //集中
        new(typeof(FrailPower),10),           //脆弱
        new(typeof(FurnacePower),50),         //熔炉
        new(typeof(MayhemPower),40),          //乱战
        new(typeof(IntangiblePower),10),      //无实体
        new(typeof(MindRotPower),10),         //心灵腐化
        new(typeof(NeurosurgePower),10),      //精神过载
        new(typeof(PyrePower),20),            //薪火之源
        new(typeof(SerpentFormPower),10),     //群蛇形态
        new(typeof(ShadowmeldPower),5),       //融入暗影
        new(typeof(SleightOfFleshPower),10),  //血肉戏法
        new(typeof(SpeedsterPower),10),       //速行者
        new(typeof(SwordSagePower),10),       //剑圣
        new(typeof(SeekingEdgePower),10),     //追踪之刃
        new(typeof(TheSealedThronePower),10), //封印王座
        new(typeof(CallOfTheVoidPower),10),   //虚空之唤
        new(typeof(AccelerantPower),10),      //触媒
        new(typeof(AutomationPower),20),      //自动化
        new(typeof(CreativeAiPower),20),      //AI
        new(typeof(ForbiddenGrimoirePower),1),//禁忌魔典
        new(typeof(DemonFormPower),30),       //恶魔形态
        new(typeof(BufferPower),10),          //缓冲
        new(typeof(BarricadePower),10),       //壁垒
        new(typeof(GigantificationPower),20), //极巨化
        new(typeof(WasteAwayPower),1),        //衰朽
    };

    public static readonly List<PowerEntry> HeibaiPool = new()
    {
        new(typeof(VulnerablePower),40),  //易伤
        new(typeof(WeakPower),40),        //虚弱
        new(typeof(VigorPower),7),        //活力
        new(typeof(DebilitatePower),5),   //摧残
        new(typeof(PoisonPower),20),      //中毒
        new(typeof(DemisePower),10),      //消亡
        new(typeof(DoomPower),20),        //灾厄
        new(typeof(StrengthPower),5),     //力量
        new(typeof(IntangiblePower),1),   //无实体
        new(typeof(BufferPower),1),       //缓冲
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