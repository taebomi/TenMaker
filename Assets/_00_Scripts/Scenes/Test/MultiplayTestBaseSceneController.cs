using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TenMaker.Scenes;
using TenMaker.Services.UGS.Multiplay;
using TenMaker.Core.Scene;
using TenMaker.Services.UGS.Authentication;
using TenMaker.Utility;
using TMPro;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using Unity.Services.Multiplayer;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player = Unity.Services.Matchmaker.Models.Player;

namespace TenMaker.Test
{
    public class MultiplayTestBaseSceneController : BaseSceneController
    {
        public override string SceneName { get; }

        protected override UniTask InitializeSceneAsync()
        {
            throw new NotImplementedException();
        }

        protected override UniTask ProcessSceneAsync()
        {
            throw new NotImplementedException();
        }
        //
        // [SerializeField] private string sceneName;
        // [SerializeField] private TMP_InputField inputfield;
        //
        //
        //
        // protected override UniTask InitializeSceneAsync()
        // {
        //     return UniTask.CompletedTask;
        // }
        //
        // protected override UniTask ProcessSceneAsync()
        // {
        //     return UniTask.CompletedTask;
        // }
        //
        // public void OnClicked()
        // {
        //     if (TMMultiplayService.Instance.IsServer is false) return;
        //
        //     TMMultiplayService.Instance.LoadScene(sceneName);
        // }
        //
        // public void OnHostClicked()
        // {
        //     HostAsync().Forget();
        // }
        //
        // private async UniTask HostAsync()
        // {
        //     var allocation = await RelayService.Instance.CreateAllocationAsync(2);
        //     var relayServerData = allocation.ToRelayServerData("udp");
        //     TMMultiplayService.Instance.SetRelayServerData(relayServerData);
        //     var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        //     inputfield.text = joinCode;
        //     TMMultiplayService.Instance.StartHost();
        // }
        //
        // public void OnJoinClicked()
        // {
        //     JoinAsync().Forget();
        // }
        //
        // private async UniTask JoinAsync()
        // {
        //     var allocation = await RelayService.Instance.JoinAllocationAsync(inputfield.text);
        //     Debug.Log(inputfield.text);
        //     MultiplayManager.Instance.SetRelayServerData(allocation.ToRelayServerData("udp"));
        //     MultiplayManager.Instance.StartClient();
        // }
        //
        // public void OnFindClicked()
        // {
        //     FindAsync().Forget();
        // }
        //
        // private async UniTask FindAsync()
        // {
        //     var players = new List<Player>
        //     {
        //         new(TMAuthenticationManager.Instance.PlayerId)
        //     };
        //     var options = new CreateTicketOptions("Default");
        //     var ticket = await MatchmakerService.Instance.CreateTicketAsync(players, options);
        //
        //
        //     string matchId = string.Empty;
        //     while (true)
        //     {
        //         await UniTask.Delay(3000);
        //         var ticketResponse = await MatchmakerService.Instance.GetTicketAsync(ticket.Id);
        //         if (ticketResponse != null)
        //         {
        //             var assignment = ticketResponse.Value as MatchIdAssignment;
        //             if (assignment.Status != MatchIdAssignment.StatusOptions.Found) continue;
        //
        //             Debug.Log(assignment.AssignmentType);
        //             Debug.Log(assignment.Message);
        //             matchId = assignment.MatchId;
        //             TBMLog.HeaderLog("Match ID: " + matchId);
        //             break;
        //         }
        //     }
        //
        //     TBMLog.HeaderLog("Match found!");
        //
        //
        //     var lobby = await LobbyService.Instance.CreateOrJoinLobbyAsync(matchId, "duel mode", 2,
        //         new CreateLobbyOptions() { IsPrivate = true });
        //     if (lobby.HostId == TMAuthenticationManager.Instance.PlayerId)
        //     {
        //         var allocation = await RelayService.Instance.CreateAllocationAsync(2);
        //         var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        //
        //         await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, new UpdateLobbyOptions()
        //         {
        //             Data = new Dictionary<string, DataObject>
        //             {
        //                 { "JoinCode", new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
        //             }
        //         });
        //
        //
        //         MultiplayManager.Instance.SetRelayServerData(allocation.ToRelayServerData("udp"));
        //         MultiplayManager.Instance.StartHost();
        //     }
        //     else
        //     {
        //         var joinCode = string.Empty;
        //         while (string.IsNullOrEmpty(joinCode))
        //         {
        //             if (lobby.Data.TryGetValue("JoinCode", out var dataObject))
        //             {
        //                 joinCode = dataObject.Value;
        //                 break;
        //             }
        //
        //             await UniTask.Delay(1000);
        //         }
        //
        //         var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        //         MultiplayManager.Instance.SetRelayServerData(joinAllocation.ToRelayServerData("udp"));
        //         MultiplayManager.Instance.StartClient();
        //     }
        // }
    }
}