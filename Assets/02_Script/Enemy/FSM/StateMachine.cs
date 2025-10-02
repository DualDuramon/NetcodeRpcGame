namespace LittleSword.Enemy.FSM
{
    public class StateMachine
    {
        private Enemy enemy;

        //������
        public StateMachine(Enemy enemy)
        {
            this.enemy = enemy;
        }

        //������� ���� ����
        public IState currentState { get; private set; }

        //������ȯ �޼���
        public void ChangeState(IState newState)
        {
            currentState?.Exit(enemy); //���� currentState�� null�̿��� �� �κи� ������ �ȵǼ� ���ܹ߻��� �ȵ�.
            currentState = newState;
            currentState.Enter(enemy);
        }

        //���� ���� update �޼��� ȣ��
        public void Update()
        {
            currentState?.Update(enemy);
        }
    
    }

}
