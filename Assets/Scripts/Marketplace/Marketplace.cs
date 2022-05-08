using UnityEngine;

public class Marketplace : MonoBehaviour
{
    private VendingMachineJetpack[] _vendingMachineJetpackPrefabs;
    private readonly Vector2 _jetpackSpawnPosition = new Vector2(-4f, -2.5f);

    #region Unity Event

    private void Awake()
    {
        _vendingMachineJetpackPrefabs = Resources.LoadAll<VendingMachineJetpack>("VendingMachines/VendingMachinesJetpack");
    }

    private void Start()
    {
        Instantiate(_vendingMachineJetpackPrefabs[Random.Range(0, _vendingMachineJetpackPrefabs.Length)], _jetpackSpawnPosition, Quaternion.identity);
    }

    #endregion
}
