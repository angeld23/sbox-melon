using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace MelonSimulator
{
	public partial class MelonHud : HudEntity<RootPanel>
	{
		public MelonHud ()
		{
			if (IsClient)
			{

				RootPanel.AddChild<NameTags>();
				RootPanel.AddChild<CrosshairCanvas>();
				CrosshairCanvas.SetCrosshair( RootPanel.AddChild<StandardCrosshair>() );
				RootPanel.AddChild<ChatBox>();
				RootPanel.AddChild<VoiceList>();
				RootPanel.AddChild<KillFeed>();
				RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();

				var tipContainer = RootPanel.AddChild<Panel>( "tipContainer" );
				tipContainer.StyleSheet.Load( "/MelonHud.scss" );
				tipContainer.SetClass( "tipContainer", true );
				var tipTitle = tipContainer.AddChild<Label>( "tipText tipTitle" );
				tipTitle.Text = "Melon Simulator";
				var tipBody = tipContainer.AddChild<Label>( "tipText tipBody" );
				tipBody.Text = "Fulfill your purpose as a melon and fill the map with your melon-y goodness.\n\nMouse1 - Explode\nMouse2 - Attract Gibs\nMouse3 - Delete Gibs";
			}
		}
	}
}
