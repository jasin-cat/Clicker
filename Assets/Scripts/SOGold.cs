using UnityEngine;

[CreateAssetMenu(fileName = "SOGold", menuName = "Scriptable Objects/SOGold")]
public class SOGold : ScriptableObject
{
    [SerializeField]
    private int _gold;

    public void AddGold(int i)
    {
        _gold += i;
    }

    public void DecreaseGold(int i)
    {
        _gold -= i;
    }
}
