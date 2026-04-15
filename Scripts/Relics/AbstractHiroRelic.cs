using BaseLib.Abstracts;

namespace Hiro.Scripts.Relics
{

    public abstract class AbstracHiroRelic : CustomRelicModel
    {


        public override string PackedIconPath => $"res://Hiro/images/relics/{Id.Entry.ToLowerInvariant()}.png";


        protected override string PackedIconOutlinePath =>$"res://Hiro/images/relics/{Id.Entry.ToLowerInvariant()}.png";



        protected override string BigIconPath => $"res://Hiro/images/relics/{Id.Entry.ToLowerInvariant()}.png";
    }
}