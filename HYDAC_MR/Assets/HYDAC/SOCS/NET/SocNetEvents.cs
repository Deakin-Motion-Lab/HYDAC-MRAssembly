﻿using System;
using UnityEngine;

namespace HYDAC.SOCS.NET
{
    [CreateAssetMenu(menuName = "NetSocs/NetEvents", fileName = "SOC_NetEvents")]
    public class SocNetEvents : ScriptableObject
    {
        public event Action ENetworkConnect;
        public event Action ENetworkDisconnect;
        public event Action EJoinRoom;
        public event Action EJoinRoomFailed;

        internal void OnNetworkConnect()
        {
            ENetworkConnect?.Invoke();
        }

        internal void OnNetworkDisconnect()
        {
            ENetworkDisconnect?.Invoke();
        }
        
        internal void InvokeJoinedRoom()
        {
            EJoinRoom?.Invoke();
        }

        internal void OnJoinRoomFailed()
        {
            EJoinRoomFailed?.Invoke();
        }



        public event Action ENetRoomSetup;

        internal void SetupNetRoom()
        {
            ENetRoomSetup?.Invoke();
        }
        

        public event Action<Transform> ELocalUserReady;
        public event Action EPlayerJoined;
        public event Action EPlayerLeft;

        internal void OnPlayerReady(Transform playerTransform)
        {
            ELocalUserReady?.Invoke(playerTransform);
        }
        
        internal void OnPlayerJoined()
        {
            EPlayerJoined?.Invoke();
        }

        internal void OnPlayerLeft()
        {
            EPlayerLeft?.Invoke();
        }
        
    }
}