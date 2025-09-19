using System;
using UnityEngine;

namespace LittleSword.InputSystem
{
    public interface IInputEvent
    {
        event Action<Vector2> OnMove;
        event Action OnAttack;
    }

}
