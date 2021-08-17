﻿
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

namespace REngine
{
	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	[Library("rengine", Title = "Renewed Engine")]

	public partial class REngine : Sandbox.Game
	{
		public REngine()
		{
			if (IsServer)
			{
				_ = new REngineHUD();
			}
		}

	    /// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined(Client cl)
		{
			base.ClientJoined(cl);

			var player = new REnginePlayer();
			cl.Pawn = player;

			player.Respawn();
			player.Tags.Add("initialized");
			Event.Run("OnClientInitialized", cl);
		}

		public override void DoPlayerNoclip(Client cl) {
			if (cl.Pawn.Tags.Has("isAdmin") && cl.Pawn is Player basePlayer)
			{
				base.DoPlayerNoclip(cl);
				Event.Run("PostPlayerNoclipped", cl);
			}
		}

		public override void PostLevelLoaded()
		{
			base.PostLevelLoaded();
			Event.Run("PostLevelLoaded");
		}

		public override void DoPlayerSuicide(Client cl) { }

		[Event("OnClientInitialized")]
		public async void CheckAdmins(Client cl)
		{
			if (cl.SteamId == 76561198799754743)
				cl.Pawn.Tags.Add("isAdmin");
		}

		[Event("PostLevelLoaded")]
		public void Callback()
		{
			if (IsClient) { }
		}
	}
}
