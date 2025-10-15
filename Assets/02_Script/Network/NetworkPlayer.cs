using LittleSword.InputSystem;
using LittleSword.Networ;
using LittleSword.Player;
using System;
using Unity.Cinemachine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

using Logger = LittleSword.Common.Logger; //로거가 여러개 있어서 우리가 정의한 로거를 선택

namespace LittleSword.Network
{
    [RequireComponent(typeof(NetworkObject), typeof(NetworkRigidbody2D), typeof(NetworkTransform))]
    [RequireComponent(typeof(OwnerNetworkAnimator))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private BasePlayer basePlayer;
        [SerializeField] private NetworkTransform networkTransform;

        //컴포넌트 캐싱
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private InputHandler inputHandler; //자기 자신의 캐릭터(Owner)일때만 키보드로 조작하기 위해 이를 활용함.
        private CinemachineCamera cmCamera;

        #region 네트워크 변수
        private NetworkVariable<bool> networkIsFacingRight = new NetworkVariable<bool>(
            true, //초기값
            NetworkVariableReadPermission.Everyone, //읽기 권한. 누구나 읽을 수 있음을 의미
            NetworkVariableWritePermission.Owner    //쓰기 권한. 오너만 쓸 수 있음을 의미
        );

        #endregion

        #region 유니티 이벤트
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
            //현재 스프라이트 방향 저장
            bool currentFacingRight = !spriteRenderer.flipX; //flipX가 false면 오른쪽을 보고 있음

            //방향이 변경되었을 때 네트워크 변수 업데이트
            if (networkIsFacingRight.Value != currentFacingRight)
            {
                networkIsFacingRight.Value = currentFacingRight;
            }
        }
        #endregion
        #region 네트워크 이벤트
        public override void OnNetworkSpawn() //네트워크 상에서 스폰됐을때
        {
            //Network 변수 이벤트 연결
            networkIsFacingRight.OnValueChanged += OnFacingRightChanged;

            if (IsOwner) //오너에 따라 이동 및 로직 컴포넌트들을 활성화/비활성화
            {
                inputHandler.enabled = true;
                basePlayer.enabled = true;
                cmCamera.Follow = transform; //카메라가 플레이어를 따라오도록 설정
            }
            else
            {
                inputHandler.enabled = false;
                basePlayer.enabled = false;
                spriteRenderer.flipX = !networkIsFacingRight.Value;
            }
            //Logger.Log($"플레이어 접속 : IsOwner = {IsOwner}, IsServer = {IsServer}, IsClient = {IsClient}, Client Id = {OwnerClientId}");
        }

        private void OnFacingRightChanged(bool previousValue, bool newValue)
        {
            if (!IsOwner)
            {
                spriteRenderer.flipX = !newValue; //flipX가 false면 오른쪽을 보고 있음
            }
        }

        public override void OnNetworkDespawn()
        {
            Logger.Log("플레이어 접속 종료");
        }
        #endregion
    }

}