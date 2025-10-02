namespace LittleSword.Enemy.FSM
{
    public class StateMachine
    {
        private Enemy enemy;

        //생성자
        public StateMachine(Enemy enemy)
        {
            this.enemy = enemy;
        }

        //현재상태 저장 변수
        public IState currentState { get; private set; }

        //상태전환 메서드
        public void ChangeState(IState newState)
        {
            currentState?.Exit(enemy); //만약 currentState가 null이여도 이 부분만 실행이 안되서 예외발생이 안됨.
            currentState = newState;
            currentState.Enter(enemy);
        }

        //현재 상태 update 메서드 호출
        public void Update()
        {
            currentState?.Update(enemy);
        }
    
    }

}
