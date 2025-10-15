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

        #region �����ݹ�
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
            Logger.Log("���� ����");
        }

        private void OnServerStoppedCallback(bool obj)
        {
            Logger.Log("���� ����");
        }


        private void OnClientStartedCallback()
        {
            Logger.Log("Ŭ���̾�Ʈ ����");
        }
        #endregion

        #region ��ư �ݹ�
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