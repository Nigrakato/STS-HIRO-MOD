using MegaCrit.Sts2.addons.mega_text;

namespace Hiro.Scripts.Nodes;

public partial class FMegaLabel : MegaLabel
{
		
		public override void _Ready()
		{
		  
			base._Ready();

			AutoSizeEnabled = false;

			AddThemeFontSizeOverride("font_size", 37);
		}
	
	
	
}
