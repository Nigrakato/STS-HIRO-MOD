using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hiro.Scripts.Powers;

public static class NativeRandomPowerHelper
{
    private static readonly Type[] AllowedBuffs =
    [
        typeof(AccuracyPower),
        typeof(AccelerantPower),
        typeof(AfterimagePower),
        typeof(AggressionPower),
        typeof(ArsenalPower),
        typeof(ArtifactPower),
        typeof(AutomationPower),
        typeof(BarricadePower),
        typeof(BlackHolePower),
        typeof(BladeOfInkPower),
        typeof(BlockNextTurnPower),
        typeof(BlurPower),
        typeof(BufferPower),
        typeof(BurstPower),
        typeof(CalamityPower),
        typeof(ChildOfTheStarsPower),
        typeof(ClarityPower),
        typeof(ConsumingShadowPower),
        typeof(CoolantPower),
        typeof(CorrosiveWavePower),
        typeof(CountdownPower),
        typeof(CreativeAiPower),
        typeof(CuriousPower),
        typeof(DanseMacabrePower),
        typeof(DarkEmbracePower),
        typeof(DemesnePower),
        typeof(DemonFormPower),
        typeof(DevourLifePower),
        typeof(DexterityPower),
        typeof(DoubleDamagePower),
        typeof(DrawCardsNextTurnPower),
        typeof(DuplicationPower),
        typeof(EchoFormPower),
        typeof(EnergyNextTurnPower),
        typeof(EnragePower),
        typeof(EnvenomPower),
        typeof(FastenPower),
        typeof(FeelNoPainPower),
        typeof(FeralPower),
        typeof(FlameBarrierPower),
        typeof(FocusPower),
        typeof(FreeAttackPower),
        typeof(FreePowerPower),
        typeof(FreeSkillPower),
        typeof(FriendshipPower),
        typeof(FurnacePower),
        typeof(GenesisPower),
        typeof(GigantificationPower),
        typeof(HailstormPower),
        typeof(HauntPower),
        typeof(HelloWorldPower),
        typeof(InfernoPower),
        typeof(InfiniteBladesPower),
        typeof(IntangiblePower),
        typeof(IterationPower),
        typeof(JuggernautPower),
        typeof(JugglingPower),
        typeof(LethalityPower),
        typeof(LightningRodPower),
        typeof(LoopPower),
        typeof(MachineLearningPower),
        typeof(MayhemPower),
        typeof(MonologuePower),
        typeof(NemesisPower),
        typeof(NightmarePower),
        typeof(NostalgiaPower),
        typeof(NoxiousFumesPower),
        typeof(OrbitPower),
        typeof(OutbreakPower),
        typeof(PagestormPower),
        typeof(PaleBlueDotPower),
        typeof(PanachePower),
        typeof(ParryPower),
        typeof(PhantomBladesPower),
        typeof(PillarOfCreationPower),
        typeof(PlatingPower),
        typeof(PrepTimePower),
        typeof(PyrePower),
        typeof(RadiancePower),
        typeof(RagePower),
        typeof(ReboundPower),
        typeof(RegenPower),
        typeof(ReflectPower),
        typeof(RetainHandPower),
        typeof(RitualPower),
        typeof(RupturePower),
        typeof(SelfFormingClayPower),
        typeof(SerpentFormPower),
        typeof(ShadowmeldPower),
        typeof(ShadowStepPower),
        typeof(ShroudPower),
        typeof(SignalBoostPower),
        typeof(SmokestackPower),
        typeof(SpectrumShiftPower),
        typeof(SpeedsterPower),
        typeof(SpiritOfAshPower),
        typeof(StampedePower),
        typeof(StormPower),
        typeof(StratagemPower),
        typeof(StrengthPower),
        typeof(SubroutinePower),
        typeof(TemporaryDexterityPower),
        typeof(TemporaryFocusPower),
        typeof(TemporaryStrengthPower),
        typeof(TheBombPower),
        typeof(TheSealedThronePower),
        typeof(ThornsPower),
        typeof(ThunderPower),
        typeof(ToolsOfTheTradePower),
        typeof(ToricToughnessPower),
        typeof(TrashToTreasurePower),
        typeof(VeilpiercerPower),
        typeof(ViciousPower),
        typeof(VigorPower),
        typeof(VoidFormPower),
        typeof(WellLaidPlansPower)
    ];

    private static readonly Type[] AllowedDebuffs =
    [
        typeof(ChainsOfBindingPower),
        typeof(ConfusedPower),
        typeof(ConstrictPower),
        typeof(DebilitatePower),
        typeof(DisintegrationPower),
        typeof(DoomPower),
        typeof(FrailPower),
        typeof(MagicBombPower),
        typeof(MindRotPower),
        typeof(NoBlockPower),
        typeof(NoDrawPower),
        typeof(OblivionPower),
        typeof(PainfulStabsPower),
        typeof(PoisonPower),
        typeof(RingingPower),
        typeof(SlowPower),
        typeof(SmoggyPower),
        typeof(StranglePower),
        typeof(TangledPower),
        typeof(TenderPower),
        typeof(VulnerablePower),
        typeof(WasteAwayPower),
        typeof(WeakPower)
    ];

    public static async Task ApplyRandomNativeBuff(Creature target, Creature applier, CardModel? source)
    {
        if (AllowedBuffs.Length == 0)
        {
            return;
        }

        int index = Random.Shared.Next(AllowedBuffs.Length);
        Type chosen = AllowedBuffs[index];
        await ApplyPowerByType(chosen, target, applier, source);
    }

    public static async Task ApplyRandomNativeDebuff(Creature target, Creature applier, CardModel? source)
    {
        if (AllowedDebuffs.Length == 0)
        {
            return;
        }

        int index = Random.Shared.Next(AllowedDebuffs.Length);
        Type chosen = AllowedDebuffs[index];
        await ApplyPowerByType(chosen, target, applier, source);
    }

    private static async Task ApplyPowerByType(Type type, Creature target, Creature applier, CardModel? source)
    {
        try
        {
            var method = typeof(PowerCmd)
                .GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "Apply" &&
                    m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 4);

            if (method == null)
            {
                return;
            }

            var generic = method.MakeGenericMethod(type);
            object? result = generic.Invoke(null, [target, 1m, applier, source]);

            if (result is Task task)
            {
                await task;
            }
        }
        catch
        {
            // 忽略单个 power 的失败，避免整张卡崩掉
        }
    }
}