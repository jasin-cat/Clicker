using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private PoolManager _pool;
    public PoolManager Pool {get => _pool; set => _pool = value;}
    protected void Release()
    {
        _pool.ReturnPool(this);
    }
}