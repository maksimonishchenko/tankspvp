using System.Linq;
using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.UI;
using Photon;
using Photon.Pun;


[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(Game))]
public sealed class Game : UpdateSystem
{
    #region Private Serializable Fields
    [Tooltip("The maximum number of players per room")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    #endregion
    
    #region Private Fields
    bool isConnecting;
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
        textc.text.text = "status connecting to random room...";
        
        // we want to make sure the log is clear everytime we connect, we might have several failed attempted if connection failed.
        textc.text.text = "";

        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        isConnecting = true;

      
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        //if (PhotonNetwork.IsConnected)
        //{
        //    textc.text.text = "Joining Room...";
        //    // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
        //    PhotonNetwork.JoinRandomRoom();
        //}else{
//
        //    textc.text.text = "Connecting...";
		//		
        //    // #Critical, we must first and foremost connect to Photon Online Server.
        //    PhotonNetwork.GameVersion = this.gameVersion;
        //    PhotonNetwork.ConnectUsingSettings();
        //}
    }

    public override void OnUpdate(float deltaTime) {
    }
}