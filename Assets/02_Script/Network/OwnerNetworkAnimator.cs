using Unity.Netcode.Components;
using UnityEngine;

namespace LittleSword.Networ
{
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false; // false�� �����Ͽ� Owner�� �ִϸ��̼��� �����ϵ��� ��. ����Ƽ �޴��󿡵� ���� ���
        }
     
    }
}
