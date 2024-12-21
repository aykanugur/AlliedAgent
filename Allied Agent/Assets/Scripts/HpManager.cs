using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class HpManager : MonoBehaviour
{
    public Object script;

    public void Decreasehp(float hp)
    {
        
        if (script.GetType() == typeof(Cover))
        {
            var x = (Cover)script;
            x.hp -= hp;
        }
        else
        {
            if (script.GetType() == typeof(EnemyAnimations))
            {
                var x = (EnemyAnimations)script;
                x.hp -= hp;
            }
            else
            {
                if (script.GetType() == typeof(Boss))
                {
                    var x = (Boss)script;
                    x.hp -= hp;
                }
            }
        }
        
        
    }
}
