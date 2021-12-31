using System.Collections;
using UnityEngine;

public class EnemyPreSpawn : MonoBehaviour
{
    private const float SpawnDelay = 1f;

    [SerializeField] private Transform holder;
    public Enemy SpawnEnemyPrefab { get; set; }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        StartCoroutine(AnimateSpawn(SpawnEnemyPrefab));
    }

    /// <summary>
    /// Wait for the intro animation to end then spawn an enemy.
    /// </summary>
    private IEnumerator AnimateSpawn(Enemy enemy)
    {
        yield return new WaitForSeconds(SpawnDelay);

        var spawnEnemy = Instantiate(enemy.gameObject, transform.position, transform.rotation);
        spawnEnemy.transform.parent = holder;
        GetComponent<Animator>().SetTrigger(Animator.StringToHash("spawn"));

        yield return new WaitForSeconds(SpawnDelay);

        spawnEnemy.transform.parent = null;
        Destroy(gameObject);
    }
}
