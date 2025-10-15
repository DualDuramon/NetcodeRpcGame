using UnityEngine;
using LittleSword.Enemy.FSM;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using System.Linq;
using LittleSword.Interfaces;
using LittleSword.Player;
using Unity.Netcode;
using Logger = LittleSword.Common.Logger;

namespace LittleSword.Enemy
{
    public class Enemy : NetworkBehaviour, IDamageable
    {
        //상태머신
        private StateMachine stateMachine;
        public StateMachine StateMachine => stateMachine;

        //상태이름(에디터용). ??는 Nullable로 앞쪽의 피연산자가 null이면 뒤의 것을 반환.
        public string CurrentStatename => StateMachine?.currentState?.GetType().Name ?? "None";

        //상태 저장 Dictionary 선언 및 초깃값 설정
        private Dictionary<Type, IState> states;

        //컴포넌트 캐싱 변수들
        [NonSerialized] public Rigidbody2D rb;
        [NonSerialized] public SpriteRenderer spriteRenderer;
        [NonSerialized] public Animator animator;

        //애니메이션 해시값 추출
        public static readonly int hashIsRun = Animator.StringToHash("IsRun");
        public static readonly int hashAttack = Animator.StringToHash("Attack");
        public static readonly int hashDie = Animator.StringToHash("Die");
        public static readonly int hashHit = Animator.StringToHash("Hit");

        //Enemy Stats
        [SerializeField] private EnemyStat enemyStat;

        //추적 대상
        [SerializeField] private Transform target;

        public LayerMask playerLayer;

        //프로퍼티들
        public Transform Target => target;
        public bool IsDead => CurrentHP <= 0;
        public int CurrentHP { get; private set; }
        
        #region 네트워크 변수 선언
        NetworkVariable<bool> networkIsFacingRight = new NetworkVariable<bool>(
            true, //초기값
            NetworkVariableReadPermission.Everyone, //읽기 권한. 누구나 읽을 수 있음을 의미
            NetworkVariableWritePermission.Server    //쓰기 권한. 서버만 쓸 수 있음을 의미
        );

        #endregion



        #region 상태 관련 메서드
        public void ChangeState<T>() where T : IState
        {
            //서버에서만 상태 변경 가능
            if(!IsServer) return;


            //사망 상태에서 다른 상태로 전이 불가하게끔
            if (IsDead && typeof(T) != typeof(DieState)) return;

            if(states.TryGetValue(typeof(T), out IState newState))
            {
                stateMachine.ChangeState(newState);
            }
        }
        
        //주인공 검출 메서드
        public bool DetectPlayer()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyStat.chaseDistance, playerLayer);

            if (colliders.Length > 0)
            {
                target = colliders.OrderBy(c => (transform.position - c.transform.position).sqrMagnitude)
                    .Where(c => c.GetComponent<BasePlayer>()?.IsDead == false)
                    .First()
                    .transform;

                return target != null;
            }
            else
            {
                target = null;
                return false;
            }
        }

        //추적 로직
        public void MoveToPlayer()
        {
            if(target == null) return;

            //이동방향 계산
            Vector2 direction = (target.position - transform.position).normalized;

            //적 이동
            if(direction.x < 0) //스프라이트 flip
            {
                spriteRenderer.flipX = true;
            }
            else if(direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }

            rb.linearVelocity = direction * enemyStat.moveSpeed;

            //네트워크 변수 값 저장
            networkIsFacingRight.Value = !spriteRenderer.flipX;
        }

        public void StopMoving()
        {
            rb.linearVelocity = Vector2.zero;
        }

        public bool IsInAttackRange()
        {
            if (target == null) return false;

            float targetDistance = (transform.position - target.position).sqrMagnitude;
            return targetDistance <= enemyStat.attackDistance * enemyStat.attackDistance;

        }

        #endregion
        #region 유니티 이벤트
        private void Awake()
        {
            InitState();
            InitComponents();

            networkIsFacingRight.OnValueChanged += (value, newValune) =>
            {
                spriteRenderer.flipX = !newValune;
            };
        }

        private void Start()
        {
            //상태머신 초기화
            stateMachine = new StateMachine(this);

            //초기상태 설정
            ChangeState<IdleState>();
        }

        private void Update()
        {
            if(!IsServer) return;

            //상태머신 갱신
            stateMachine.Update();
            TestFSM();
        }
        #endregion
        #region 초기화
        private void InitState()
        {
            states = new Dictionary<Type, IState>
            {
                [typeof(IdleState)] = new IdleState(enemyStat.detecteInterval),
                [typeof(ChaseState)] = new ChaseState(enemyStat.detecteInterval),
                [typeof(AttackState)] = new AttackState(enemyStat.attackCooldown),
                [typeof(DieState)] = new DieState()
            };
        }
        
        private void InitComponents()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            rb.gravityScale = 0.0f;
            rb.freezeRotation = true;

            CurrentHP = enemyStat.maxHP;
        }
        #endregion
        #region 테스트용 코드
        private void TestFSM()
        {
            //레거시 코드인 Input.GetKey보다 신규 인풋시스템인 이것을 사용.
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                ChangeState<IdleState>();
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                ChangeState<ChaseState>();
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                ChangeState<AttackState>();
            }

        }
        #endregion
        #region Gizmos
        private void OnDrawGizmos()
        {
            //추적 범위
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, enemyStat.chaseDistance);

            //공격 범위
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyStat.attackDistance);
        }
        #endregion
        #region 애니메이션 이벤트
        //공격 애니메이션에서 호출할 메서드
        public void OnAttackAnimationEvent()
        {
            if(!IsServer) return;
            if (target == null) return;
            //target.GetComponent<IDamageable>().TakeDamage(enemyStat.attackDamage);

            //RPC 이용 데미지 처리
            ulong targetID = target.GetComponent<NetworkObject>().NetworkObjectId;
            //TakeDamageClientRpc(targetID, enemyStat.attackDamage);
            TakeDamageRpc(targetID, enemyStat.attackDamage);
        }

        [ClientRpc] //클라만 실행
        private void TakeDamageClientRpc(ulong targetId, int damage)
        {
            Logger.Log($"RPC 수신 : {targetId}, {damage}");
            NetworkObject targetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetId];
            targetObj.GetComponent<IDamageable>().TakeDamage(damage);
        }

        [Rpc(SendTo.Everyone)] //클라만 실행
        private void TakeDamageRpc(ulong targetId, int damage)
        {
            Logger.Log($"RPC 수신 : {targetId}, {damage}");
            NetworkObject targetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetId];
            targetObj.GetComponent<IDamageable>().TakeDamage(damage);
        }


        #endregion
        #region 데미지 처리
        public void TakeDamage(int damage)
        {
            if (IsDead) return;
            CurrentHP -= damage;
            if(IsDead)
            {
                Die();
            }
            else
            {
                animator.SetTrigger(hashHit);
            }
        }

        public void Die()
        {
            ChangeState<DieState>();
        }
        #endregion
    }
}
