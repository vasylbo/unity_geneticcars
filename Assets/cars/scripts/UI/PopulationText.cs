using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopulationText : MonoBehaviour {
    public GameObject Spawner;

	// Use this for initialization
	void Start () {
        Spawner.GetComponent<CarSpawner>().PopulationNumUpdated += OnPopulationUpdate;
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnPopulationUpdate(int pNum) {
        Text txt = GetComponent<Text>();
        txt.text = "Population #" + pNum;
    }
}
