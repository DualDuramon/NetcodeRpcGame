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
            moveAction = inputActions.Player.Move;      //#2
            attackAction = inputActions.Player.Attack;  //#2
        }

        private void OnEnable()
        {
            inputActions.Enable();  //#4

            //�̺�Ʈ ����
            moveAction.performed += HandleMove; //#3
        }

        private void OnDisable()
        {
            inputActions.Disable();              //#5

            //�̺�Ʈ ����
            moveAction.performed -= HandleMove; //#5
        }

        private void HandleMove(InputAction.CallbackContext ctx) //�ش� ctx�ȿ� ������ Ÿ���� ������ ����
        {
            Logger.Log($"move : {ctx.ReadValue<Vector2>()}");
        }
    }
}//end of namespace LittleSword.InputSystem
