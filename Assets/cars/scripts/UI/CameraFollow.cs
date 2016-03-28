using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public GameObject carsSpawner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Transform parent = carsSpawner.transform;
        if (parent.childCount > 0) {
            var bestChild = parent.GetChild(0).gameObject.GetComponent<CarTracker>().getPosition();
            for (var i = 1; i < parent.childCount; i++) {
                var child = parent.GetChild(i).gameObject.GetComponent<CarTracker>().getPosition();
                if (child.x > bestChild.x) {
                    bestChild = child;
                }
            }

            Camera cam = GetComponent<Camera>();
            var camCenter = cam.WorldToViewportPoint(bestChild);
            var deltaPos = bestChild - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camCenter.z));
            var temp = transform.position + deltaPos;
            transform.position = new Vector3(temp.x, temp.y, -10f);
        } else {
            transform.position = new Vector3(0f, 0f, -10f);
        }
	}
}
