using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils; 
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;


namespace Hiro.Scripts.Cards
{
    public sealed class Yishijian : AbstractHiroCard
    {

        public Yishijian() 
            : base(0, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, true)
        {
        }



        protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> 
        { 
            new DamageVar(3m, ValueProp.Move), 
            new RepeatVar(6),
            new PowerVar<VulnerablePower>(2m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Target == null) return;
            var player = base.Owner;
            if (player == null) return;

            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(base.DynamicVars.Repeat.IntValue) 
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
            await PowerCmd.Apply<VulnerablePower>(cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);

        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Damage.UpgradeValueBy(1m);
            base.DynamicVars.Repeat.UpgradeValueBy(1m);
        }

        public void Activate()
        {
        }
    }
}