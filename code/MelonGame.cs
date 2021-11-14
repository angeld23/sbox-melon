using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelonSimulator
{
	public partial class MelonGame : Game
	{
		[ClientRpc]
		public static void PlaySoundAtPos( string soundName, Vector3 pos )
		{
			Sound.FromWorld( soundName, pos );
		}

		[Net, Predicted]
		public float MusicStartTime { get; set; }

		public MelonGame()
		{
			if (IsServer)
			{
				new MelonHud();
			}
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			if (IsClient)
			{
				if (Input.Pressed(InputButton.Attack2))
				{
					Sound.FromScreen( "buttondown" );
				}
				if ( Input.Released(InputButton.Attack2))
				{
					Sound.FromScreen( "buttonup" );
				}
				if (Input.Pressed(InputButton.Zoom))
				{
					Sound.FromScreen( "clear" );
				}
			}
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			var player = new MelonPlayer();
			cl.Pawn = player;

			player.Respawn();
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			var gibs = ((MelonPlayer)cl.Pawn).AllGibs;
			foreach (ModelEntity gib in gibs)
			{
				gib.Delete();
			}
			gibs.Clear();
			base.ClientDisconnect( cl, reason );
		}
	}
}
