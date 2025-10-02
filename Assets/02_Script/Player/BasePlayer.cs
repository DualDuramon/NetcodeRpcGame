using LittleSword.InputSystem;
using LittleSword.Player.Controller;
using LittleSword.Interfaces;
using UnityEngine;
using Logger = LittleSword.Common.Logger; //로거가 여러개 있어서 우리가 정의한 로거를 선택

namespace LittleSword.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(InputHandler), typeof(CapsuleCollider2D))]
    public class BasePlayer : MonoBehaviour, IDamageable
    {   //기본적인 플레이어의 행동들, 공통적인 속성을 관리하는 클래스
        
        //Controllers
        private InputHandler inputHandler;
        private MovementController movementController;
        private AnimationController animationController;

        //components
        protected Rigidbody2D rb;
        protected SpriteRenderer spriteRenderer;
        protected Animator animator;
        protected Collider2D col;

        //플레이어 스탯
        public PlayerStats playerStats;

        public bool IsDead => CurrentHP <= 0;

        public int CurrentHP { get; set; }

        //프로퍼티


        #region 유니티 이벤트
        protected void Awake()
        {
            InitComponents();
            InitControllers();
        }

        protected void OnEnable()
        {
            inputHandler.OnMove += Move;
            inputHandler.OnAttack += Attack;
        }

        protected void OnDisable()
        {
            inputHandler.OnMove -= Move;
            inputHandler.OnAttack -= Attack;
        }

        #endregion

        #region 초기화
        private void InitControllers()
        {
            inputHandler = GetComponent<InputHandler>();
            movementController = new MovementController(rb, spriteRenderer);
            animationController = new AnimationController(animator);
        }
        private void InitComponents()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            rb.gravityScale = 0f;
            rb.freezeRotation = true;

            CurrentHP = playerStats.maxHP;
        }
        #endregion
        
        #region 공통 메서드
        protected virtual void Move(Vector2 dir)
        {
            movementController.Move(dir, playerStats.moveSpeed);
            animationController.Move(dir != Vector2.zero);
        }
        protected virtual void Attack()
        {
            animationController.Attack();
        }
        #endregion
        #region 피격관련 메서드
        public void TakeDamage(int damage)
        {
            if (IsDead) return;

            CurrentHP = Mathf.Max(CurrentHP - damage, 0);
            if (IsDead)
            {
                Die();
            }
            else
            {
                animationController.Hit();
            }
        }

        public void Die()
        {
            animationController.Die();
            inputHandler.enabled = false; //죽으면 입력 비활성화
            col.enabled = false;
            rb.linearVelocity = Vector2.zero;
        }
        #endregion
    }

}