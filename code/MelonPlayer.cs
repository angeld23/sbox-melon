using System;
using System.Collections.Generic;
using Sandbox;

namespace MelonSimulator
{
	public partial class MelonPlayer : Player
	{
		public readonly List<ModelEntity> AllGibs = new();
		public bool MusicPlayed;

		[ClientRpc]
		public void PlayMusic ()
		{
			Sound.FromScreen( "music" );
		}

		public override void Respawn ()
		{
			SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
			//RenderColor = new(0xff00ff00);
			var walkController = new WalkController ();
			walkController.WalkSpeed = 1500f;
			walkController.DefaultSpeed = 1500f;
			walkController.SprintSpeed = 1500f;
			walkController.Gravity = 250f;
			walkController.AirAcceleration = 500f;
			walkController.BodyHeight = 30;
			walkController.BodyGirth = 20;

			var thirdPersonCamera = new MelonThirdPersonCamera();
			//_thirdPersonCamera.Pos

			Controller = walkController;
			Animator = new StandardPlayerAnimator();
			Camera = thirdPersonCamera;

			SetupPhysicsFromModel(PhysicsMotionType.Dynamic, false);

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		TimeSince timeSinceDied;

		public override void OnKilled()
		{
			base.OnKilled();

			if ( !MusicPlayed )
			{
				MusicPlayed = true;
				PlayMusic( To.Single( this ) );
			}

			MelonGame.PlaySoundAtPos( "fart2", this.Position );

			timeSinceDied = 0;
			EnableDrawing = false;
			var gibs = new List<string>()
			{
				"models/sbox_props/watermelon/watermelon_gib01.vmdl",
				"models/sbox_props/watermelon/watermelon_gib10.vmdl",
				"models/sbox_props/watermelon/watermelon_gib13.vmdl"
			};

			for (var i = 0; i < 3; i++ )
			{
				foreach ( string modelName in gibs )
				{
					// create a gib from the melon
					var gib = new ModelEntity( modelName );
					gib.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
					gib.Position = this.Position;
					gib.Rotation = this.Rotation;
					gib.Velocity = this.Velocity + new Vector3( Rand.Float( -1, 1 ), Rand.Float( -1, 1 ), Rand.Float( -1, 1 ) ) * 500;
					// add it to the list of gibs for the attack2 function
					AllGibs.Add( gib );
				}
			}
		}

		public override void Simulate( Client cl )
		{
			if (EnableDrawing && IsServer)
			{
				// builtin killbind xdd
				if (Input.Pressed(InputButton.Attack1))
				{
					this.OnKilled();
				}
				// bring all the melon gibs towards you
				if (Input.Pressed(InputButton.Attack2))
				{
					foreach (ModelEntity gib in AllGibs)
					{
						gib.Velocity = (Position - gib.Position).Normal * 1500f;
					}
				}

				if (Input.Pressed(InputButton.Zoom))
				{
					foreach (ModelEntity gib in AllGibs)
					{
						gib.Delete();
					}
					AllGibs.Clear();
				}
			}

			if ( LifeState == LifeState.Dead )
			{
				if ( timeSinceDied > 1 && IsServer )
				{
					Respawn();
				}

				return;
			}

			//UpdatePhysicsHull();

			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );

			// we take over the job of base Player.Simulate completely so xddd
			//base.Simulate( cl );
		}
	}
}
