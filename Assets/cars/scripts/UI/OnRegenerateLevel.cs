using UnityEngine;
using System.Collections;

public class OnRegenerateLevel : MonoBehaviour {
    public GameObject Spawner;
    public GameObject Level;

    public void RegenerateLevel() {
        Level.GetComponent<LevelGenerator>().RegenerateLevel();
        Spawner.GetComponent<CarSpawner>().resetPopulation();
    }
}
