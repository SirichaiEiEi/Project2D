using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] MonsterController monster;
    public void OnAttackEnd()
    {
        monster.DealDamageToPlayer();
    }
}





