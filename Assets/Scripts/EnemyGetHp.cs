using UnityEngine;


public class EnemyGetHp
{
    float rate = 1.10f;
    public int GetHp(float baseHp, int level)
    {
        if(level < 20)
            rate = 1.15f;
        else if(level < 50)
            rate = 1.20f;
        else if(level < 100)
            rate = 1.25f;
        else 
            rate = 1.3f;

        
        return Mathf.FloorToInt(baseHp * Mathf.Pow(rate, level));
    }
}