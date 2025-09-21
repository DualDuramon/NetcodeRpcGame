using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Logger = LittleSword.Common.Logger; //로거가 여러개 있어서 우리가 정의한 로거를 선택

namespace LittleSword.InputSystem {
   public class InputHandler : MonoBehaviour, IInputEvent
    {
        public event Action<Vector2> OnMove;    //이동입력이 들어올 경우 발생시킬 이벤트
        public event Action OnAttack;           //공격입력이 들어올 경우 발생시킬 이벤트

        //inputAction 연결
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

            //이벤트 연결
            //대충의 서순 : inputAction.Player.Move실행 -> moveAction 실행 -> HandleMove실행 -> 등록된 OnMove(BasePlayer의 move함수) 실행됨.
            moveAction.performed += HandleMove;
            moveAction.canceled += HandleMove;
            attackAction.performed += HandleAttack;
        }

        private void OnDisable()
        {
            inputActions.Disable();              

            //이벤트 해제
            moveAction.performed -= HandleMove;
            moveAction.canceled -= HandleMove;
            attackAction.performed -= HandleAttack;
        }

        private void HandleMove(InputAction.CallbackContext ctx) //해당 ctx안에 설정한 타입의 값들이 들어옴
        {
            OnMove?.Invoke(ctx.ReadValue<Vector2>());
        }

        private void HandleAttack(InputAction.CallbackContext ctx)
        {
            OnAttack?.Invoke();
        }
    }
}//end of namespace LittleSword.InputSystem
