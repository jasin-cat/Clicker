using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private int _size = 20;
    [SerializeField]
    private PooledObject _pooledObjectPrefab;
    private Stack<PooledObject> Pool = new();

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        PooledObject pooledInstance = null;

        for(int i = 0; i < _size; i++)
        {
            pooledInstance =Instantiate(_pooledObjectPrefab, this.transform);
            pooledInstance.Pool = this;

            pooledInstance.gameObject.SetActive(false);
            Pool.Push(pooledInstance);
        }
    }

    public PooledObject GetPooledObject()
    {
        if(Pool.Count == 0)
        {
            PooledObject instance = Instantiate(_pooledObjectPrefab, this.transform);
            instance.Pool = this;

            return instance;
        }

        PooledObject nextObject = Pool.Pop();
        nextObject.gameObject.SetActive(true);

        return nextObject;
    }

    public void ReturnPool(PooledObject obj)
    {
        Pool.Push(obj);
        obj.gameObject.SetActive(false);
    }


}
