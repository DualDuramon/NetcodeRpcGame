using UnityEngine;
using LittleSword.Enemy.FSM;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using System.Linq;
using LittleSword.Interfaces;
using LittleSword.Player;

namespace LittleSword.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        //���¸ӽ�
        private StateMachine stateMachine;
        public StateMachine StateMachine => stateMachine;

        //�����̸�(�����Ϳ�). ??�� Nullable�� ������ �ǿ����ڰ� null�̸� ���� ���� ��ȯ.
        public string CurrentStatename => StateMachine?.currentState?.GetType().Name ?? "None";

        //���� ���� Dictionary ���� �� �ʱ갪 ����
        private Dictionary<Type, IState> states;

        //������Ʈ ĳ�� ������
        [NonSerialized] public Rigidbody2D rb;
        [NonSerialized] public SpriteRenderer spriteRenderer;
        [NonSerialized] public Animator animator;

        //�ִϸ��̼� �ؽð� ����
        public static readonly int hashIsRun = Animator.StringToHash("IsRun");
        public static readonly int hashAttack = Animator.StringToHash("Attack");
        public static readonly int hashDie = Animator.StringToHash("Die");
        public static readonly int hashHit = Animator.StringToHash("Hit");

        //Enemy Stats
        [SerializeField] private EnemyStat enemyStat;

        //���� ���
        [SerializeField] private Transform target;

        public LayerMask playerLayer;

        //������Ƽ��
        public Transform Target => target;
        public bool IsDead => CurrentHP <= 0;
        public int CurrentHP { get; private set; }



        #region ���� ���� �޼���
        public void ChangeState<T>() where T : IState
        {
            //��� ���¿��� �ٸ� ���·� ���� �Ұ��ϰԲ�
            if (IsDead && typeof(T) != typeof(DieState)) return;

            if(states.TryGetValue(typeof(T), out IState newState))
            {
                stateMachine.ChangeState(newState);
            }
        }
        
        //���ΰ� ���� �޼���
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

        //���� ����
        public void MoveToPlayer()
        {
            if(target == null) return;

            //�̵����� ���
            Vector2 direction = (target.position - transform.position).normalized;

            //�� �̵�
            if(direction.x < 0) //��������Ʈ flip
            {
                spriteRenderer.flipX = true;
            }
            else if(direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }

            rb.linearVelocity = direction * enemyStat.moveSpeed;
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
        #region ����Ƽ �̺�Ʈ
        private void Awake()
        {
            InitState();
            InitComponents();
        }

        private void Start()
        {
            //���¸ӽ� �ʱ�ȭ
            stateMachine = new StateMachine(this);

            //�ʱ���� ����
            ChangeState<IdleState>();
        }

        private void Update()
        {
            //���¸ӽ� ����
            stateMachine.Update();
            TestFSM();
        }
        #endregion
        #region �ʱ�ȭ
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
        #region �׽�Ʈ�� �ڵ�
        private void TestFSM()
        {
            //���Ž� �ڵ��� Input.GetKey���� �ű� ��ǲ�ý����� �̰��� ���.
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
            //���� ����
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, enemyStat.chaseDistance);

            //���� ����
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyStat.attackDistance);
        }
        #endregion
        #region �ִϸ��̼� �̺�Ʈ
        //���� �ִϸ��̼ǿ��� ȣ���� �޼���
        public void OnAttackAnimationEvent()
        {
            if (target == null) return;
            target.GetComponent<IDamageable>().TakeDamage(enemyStat.attackDamage);
        }
        #endregion
        #region ������ ó��
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
