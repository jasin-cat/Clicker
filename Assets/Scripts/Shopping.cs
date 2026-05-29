using UnityEngine;

public class Shopping
{
    private SOGold _gold;
    private float _baseCost = 20;
    public Shopping(SOGold gold)
    {
        _gold = gold;
    }

    public void Attack()
    {
        var cost = _baseCost * Mathf.Pow(1.07f, GameManager.UpGradeLevel);

        Debug.Log($"Cost:{cost}");
        Debug.Log(_gold.Gold < Mathf.FloorToInt(cost));

        if(_gold.Gold < Mathf.FloorToInt(cost))
        {
            Debug.Log("できないよ");
        }
        else
        {
            _gold.DecreaseGold(Mathf.FloorToInt(cost));
            Player.MaxEnablePlayer++;

            GameManager.UpGradeLevel++;
        }
    }
}