using UnityEngine;

public class DragonCombat : EnemyCombat
{
    [SerializeField] private DragonBall dragonBallPrefab;

    public void Attack()
    {
        Instantiate(dragonBallPrefab, new Vector2(Random.Range(-4.5f, 4.5f), -50f), Quaternion.identity);
    }
}
