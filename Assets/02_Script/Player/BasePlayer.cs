using LittleSword.InputSystem;
using LittleSword.Player.Controller;
using LittleSword.Interfaces;
using UnityEngine;
using Logger = LittleSword.Common.Logger; //�ΰŰ� ������ �־ �츮�� ������ �ΰŸ� ����

namespace LittleSword.Player
{
    public class BasePlayer : MonoBehaviour, IDamageable
    {   //�⺻���� �÷��̾��� �ൿ��, �������� �Ӽ��� �����ϴ� Ŭ����
        
        //Controllers
        private InputHandler inputHandler;
        private MovementController movementController;
        private AnimationController animationController;

        //components
        protected Rigidbody2D rb;
        protected SpriteRenderer spriteRenderer;
        protected Animator animator;
        protected Collider2D col;

        //�÷��̾� ����
        public PlayerStats playerStats;

        public bool IsDead => CurrentHP <= 0;

        public int CurrentHP { get; set; }

        //������Ƽ


        #region ����Ƽ �̺�Ʈ
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

        #region �ʱ�ȭ
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
        }
        #endregion
        
        #region ���� �޼���
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
        #region �ǰݰ��� �޼���
        public void TakeDamage(int damage)
        {
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
            inputHandler.enabled = false; //������ �Է� ��Ȱ��ȭ
            col.enabled = false;
            rb.linearVelocity = Vector2.zero;
        }
        #endregion
    }

}