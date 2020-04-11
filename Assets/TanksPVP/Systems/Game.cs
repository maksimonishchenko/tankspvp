using System.Collections.Generic;
using System.Linq;
using Morpeh;
using Morpeh.Globals;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(Game))]
public sealed class Game : UpdateSystem
{
    
    #region public global events
    public GlobalEvent ConnectToMaster;
    public JoinRoomFailedEvent JoinRandomFailed;
    public DisconnectEvent Disconnected;
    public GlobalEvent JoinedRoom;
    #endregion
    #region Private Serializable Fields
    [Tooltip("The maximum number of players per room")]
    [SerializeField]
    private byte _maxPlayersPerRoom = 2;
    #endregion
    
    #region Private Fields

    private bool _isConnecting;
    private Text _text;
    
    #endregion
    
    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";

    private Filter filter;
    
    public override void OnAwake()
    {
        filter = this.World.Filter.With<TextComponent>();

        ref var textc = ref filter.ElementAt(0).GetComponent<TextComponent>();
        
        _text = textc.text;

        // we want to make sure the log is clear everytime we connect, we might have several failed attempted if connection failed.
        _text.text = "status connecting to random room...";

        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        _isConnecting = true;
        
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            _text.text = "Joining Room...";
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }else{

            _text.text = "Connecting...";
				
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = this.gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        this.ConnectToMaster.Subscribe(OnConnectToMaster);
        this.JoinRandomFailed.Subscribe(OnJoinRandomFailed);
        this.Disconnected.Subscribe(OnDisconnected);
        this.JoinedRoom.Subscribe(OnJoinedRoom);
    }

    private void OnConnectToMaster(IEnumerable<int> enumerable)
    {
        if (_isConnecting)
        {
            _text.text = "OnConnectedToMaster: Next -> try to Join Random Room";
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
		
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    private void OnJoinRandomFailed(IEnumerable<(short,string)> enumerable)
    {
        _text.text = "<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room" + enumerable.GetEnumerator().MoveNext().ToString();
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = this._maxPlayersPerRoom});
    }

    private void OnDisconnected(IEnumerable<DisconnectCause> enumerable)
    {
        _text.text = "<Color=Red>OnDisconnected</Color> ";
        Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");

        _isConnecting = false;
        
    }

    private void OnJoinedRoom(IEnumerable<int> enumerable)
    {
        _text.text = "<Color=Green>OnJoinedRoom</Color> with "+PhotonNetwork.CurrentRoom.PlayerCount+" Player(s)";
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");
        
        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
        	Debug.Log("We load the 'Room for 1' ");

        	// #Critical
        	// Load the Room Level. 
            //todo load prefab of the level
        }
        
    }
    public override void OnUpdate(float deltaTime) {
    }
}