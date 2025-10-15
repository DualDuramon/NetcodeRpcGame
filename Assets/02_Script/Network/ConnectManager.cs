using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;
using Logger = LittleSword.Common.Logger;

namespace LttieleSword.Network
{
    public class ConnectManager : MonoBehaviour
    {
        [SerializeField] private Button serverButton;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private Button closedButton;

        private void Start()
        {
            serverButton.onClick.AddListener(OnClickServer);
            hostButton.onClick.AddListener(OnClickHost);
            clientButton.onClick.AddListener(OnClickClient);
            closedButton.onClick.AddListener(OnClickClosed);

            BindingServerCallbacks();
        }

        private void OnDisable()
        {
            serverButton.onClick.RemoveListener(OnClickServer);
            hostButton.onClick.RemoveListener(OnClickHost);
            clientButton.onClick.RemoveListener(OnClickClient);
            closedButton.onClick.RemoveListener(OnClickClosed);

            UnBindingServerCallbacks();
        }

        #region 서버콜백
        private void BindingServerCallbacks()
        {
            NetworkManager.Singleton.OnServerStarted += OnServerStartedCallback;
            NetworkManager.Singleton.OnServerStopped += OnServerStoppedCallback;
            NetworkManager.Singleton.OnClientStarted += OnClientStartedCallback;
        }

        private void UnBindingServerCallbacks()
        {
            if (NetworkManager.Singleton == null) return;

            NetworkManager.Singleton.OnServerStarted -= OnServerStartedCallback;
            NetworkManager.Singleton.OnServerStopped -= OnServerStoppedCallback;
            NetworkManager.Singleton.OnClientStarted -= OnClientStartedCallback;
        }
        private void OnServerStartedCallback()
        {
            Logger.Log("서버 시작");
        }

        private void OnServerStoppedCallback(bool obj)
        {
            Logger.Log("서버 종료");
        }


        private void OnClientStartedCallback()
        {
            Logger.Log("클라이언트 접속");
        }
        #endregion

        #region 버튼 콜백
        private void OnClickServer()
        {
            NetworkManager.Singleton.StartServer();
        }

        private void OnClickHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        private void OnClickClient()
        {
            NetworkManager.Singleton.StartClient();
        }

        private void OnClickClosed()
        {
            NetworkManager.Singleton.Shutdown();
        }
        #endregion
    }
}