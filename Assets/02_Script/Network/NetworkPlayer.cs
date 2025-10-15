using LittleSword.InputSystem;
using LittleSword.Networ;
using LittleSword.Player;
using System;
using Unity.Cinemachine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

using Logger = LittleSword.Common.Logger; //�ΰŰ� ������ �־ �츮�� ������ �ΰŸ� ����

namespace LittleSword.Network
{
    [RequireComponent(typeof(NetworkObject), typeof(NetworkRigidbody2D), typeof(NetworkTransform))]
    [RequireComponent(typeof(OwnerNetworkAnimator))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private BasePlayer basePlayer;
        [SerializeField] private NetworkTransform networkTransform;

        //������Ʈ ĳ��
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private InputHandler inputHandler; //�ڱ� �ڽ��� ĳ����(Owner)�϶��� Ű����� �����ϱ� ���� �̸� Ȱ����.
        private CinemachineCamera cmCamera;

        #region ��Ʈ��ũ ����
        private NetworkVariable<bool> networkIsFacingRight = new NetworkVariable<bool>(
            true, //�ʱⰪ
            NetworkVariableReadPermission.Everyone, //�б� ����. ������ ���� �� ������ �ǹ�
            NetworkVariableWritePermission.Owner    //���� ����. ���ʸ� �� �� ������ �ǹ�
        );

        #endregion

        #region ����Ƽ �̺�Ʈ
        private void Awake()
        {
            basePlayer = GetComponent<BasePlayer>();
            networkTransform = GetComponent<NetworkTransform>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            inputHandler = GetComponent<InputHandler>();
            cmCamera = FindAnyObjectByType<CinemachineCamera>();
        }

        private void Start()
        {
            if (IsOwner)
            {
                inputHandler.OnMove += HandleMove;
            }
        }

        private void HandleMove(Vector2 ctx)
        {
            //���� ��������Ʈ ���� ����
            bool currentFacingRight = !spriteRenderer.flipX; //flipX�� false�� �������� ���� ����

            //������ ����Ǿ��� �� ��Ʈ��ũ ���� ������Ʈ
            if (networkIsFacingRight.Value != currentFacingRight)
            {
                networkIsFacingRight.Value = currentFacingRight;
            }
        }
        #endregion
        #region ��Ʈ��ũ �̺�Ʈ
        public override void OnNetworkSpawn() //��Ʈ��ũ �󿡼� ����������
        {
            //Network ���� �̺�Ʈ ����
            networkIsFacingRight.OnValueChanged += OnFacingRightChanged;

            if (IsOwner) //���ʿ� ���� �̵� �� ���� ������Ʈ���� Ȱ��ȭ/��Ȱ��ȭ
            {
                inputHandler.enabled = true;
                basePlayer.enabled = true;
                cmCamera.Follow = transform; //ī�޶� �÷��̾ ��������� ����
            }
            else
            {
                inputHandler.enabled = false;
                basePlayer.enabled = false;
                spriteRenderer.flipX = !networkIsFacingRight.Value;
            }
            //Logger.Log($"�÷��̾� ���� : IsOwner = {IsOwner}, IsServer = {IsServer}, IsClient = {IsClient}, Client Id = {OwnerClientId}");
        }

        private void OnFacingRightChanged(bool previousValue, bool newValue)
        {
            if (!IsOwner)
            {
                spriteRenderer.flipX = !newValue; //flipX�� false�� �������� ���� ����
            }
        }

        public override void OnNetworkDespawn()
        {
            Logger.Log("�÷��̾� ���� ����");
        }
        #endregion
    }

}