using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [SerializeField]
    protected RoleType _roletype;
    public RoleType RoleType => _roletype;
    private PoolManager _pool;
    public PoolManager Pool {get => _pool; set => _pool = value;}
    protected virtual void Release()
    {
        _pool.ReturnPool(this);
    }
}