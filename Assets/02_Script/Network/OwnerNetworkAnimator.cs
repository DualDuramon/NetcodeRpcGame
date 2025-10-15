using Unity.Netcode.Components;
using UnityEngine;

namespace LittleSword.Networ
{
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false; // false로 설정하여 Owner가 애니메이션을 제어하도록 함. 유니티 메뉴얼에도 나온 방법
        }
     
    }
}
