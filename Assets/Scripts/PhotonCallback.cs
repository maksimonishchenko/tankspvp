using System.Collections;
using System.Collections.Generic;
using Morpeh.Globals;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class PhotonCallback : MonoBehaviourPunCallbacks
{
		public GlobalEvent ConnectToMaster;
		public JoinRoomFailedEvent JoinRandomFailed;
		public DisconnectEvent Disconnected;
		public GlobalEvent JoinedRoom;
		
        #region MonoBehaviourPunCallbacks CallBacks
        // below, we implement some callbacks of PUN
        // you can find PUN's callbacks in the class MonoBehaviourPunCallbacks


        /// <summary>
        /// Called after the connection to the master is established and authenticated
        /// </summary>
        public override void OnConnectedToMaster()
		{
             //we don't want to do anything if we are not attempting to join a room. 
			 //this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
			 //we don't want to do anything.
			 ConnectToMaster.Publish(); 
				 
		}

		/// <summary>
		/// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
		/// </summary>
		/// <remarks>
		/// Most likely all rooms are full or no rooms are available. <br/>
		/// </remarks>
		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			JoinRandomFailed.Publish((returnCode,message));
		}


		/// <summary>
		/// Called after disconnecting from the Photon server.
		/// </summary>
		public override void OnDisconnected(DisconnectCause cause)
		{
			Disconnected.Publish(cause);
		}

		/// <summary>
		/// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
		/// </summary>
		/// <remarks>
		/// This method is commonly used to instantiate player characters.
		/// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
		///
		/// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
		/// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
		/// enough players are in the room to start playing.
		/// </remarks>
		public override void OnJoinedRoom()
		{
			JoinedRoom.Publish();
		}

		#endregion
}
