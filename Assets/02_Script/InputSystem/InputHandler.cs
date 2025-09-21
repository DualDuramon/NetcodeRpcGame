using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Logger = LittleSword.Common.Logger; //�ΰŰ� ������ �־ �츮�� ������ �ΰŸ� ����

namespace LittleSword.InputSystem {
   public class InputHandler : MonoBehaviour, IInputEvent
    {
        public event Action<Vector2> OnMove;    //�̵��Է��� ���� ��� �߻���ų �̺�Ʈ
        public event Action OnAttack;           //�����Է��� ���� ��� �߻���ų �̺�Ʈ

        //inputAction ����
        private InputSystem_Actions inputActions;   //#1
        private InputAction moveAction;             //#2
        private InputAction attackAction;

        private void Awake()
        {
            inputActions = new InputSystem_Actions(); 
            moveAction = inputActions.Player.Move;      
            attackAction = inputActions.Player.Attack;  
        }

        private void OnEnable()
        {
            inputActions.Enable();

            //�̺�Ʈ ����
            //������ ���� : inputAction.Player.Move���� -> moveAction ���� -> HandleMove���� -> ��ϵ� OnMove(BasePlayer�� move�Լ�) �����.
            moveAction.performed += HandleMove;
            moveAction.canceled += HandleMove;
            attackAction.performed += HandleAttack;
        }

        private void OnDisable()
        {
            inputActions.Disable();              

            //�̺�Ʈ ����
            moveAction.performed -= HandleMove;
            moveAction.canceled -= HandleMove;
            attackAction.performed -= HandleAttack;
        }

        private void HandleMove(InputAction.CallbackContext ctx) //�ش� ctx�ȿ� ������ Ÿ���� ������ ����
        {
            OnMove?.Invoke(ctx.ReadValue<Vector2>());
        }

        private void HandleAttack(InputAction.CallbackContext ctx)
        {
            OnAttack?.Invoke();
        }
    }
}//end of namespace LittleSword.InputSystem
