using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    [SerializeField] private Collider trigCollider;

    [SerializeField] private EnemyAI behaviour;
    
    private bool _state = true;


    public bool GetState()
    {
        return _state;
    }
    public void SetFlashBanged(bool state)
    //Disable perception of enemy when flash banged
    {
        if (state)
        {
            trigCollider.enabled = false;
            behaviour.enabled = false;
            this._state = false;
        }
        else
        {
            this._state = true;
            behaviour.enabled = true;
            trigCollider.enabled = true;
        }
    }
}
