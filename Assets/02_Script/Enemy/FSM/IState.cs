using UnityEngine;

namespace LittleSword.Enemy.FSM
{
    public interface IState
    {
        void Enter(Enemy enemy);
        void Update(Enemy enemy); //exceute
        void Exit(Enemy enemy);

    }
}
