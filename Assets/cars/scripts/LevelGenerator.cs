using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
    public int platformsCount = 40;
    [Range(0f, Mathf.PI / 3)]
    public float standartAngleDelta = Mathf.PI / 6;
    public float complexityProgression = 2f;

    public GameObject platformPrefab;

    public void RegenerateLevel() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        generateLevel();
    }

	// Use this for initialization
	void Start () {
        generateLevel();
	}

    void generateLevel() {
        GameObject platform = createPlatform();
        SpriteRenderer rend = platform.GetComponent<SpriteRenderer>(); 
        Vector3 size = rend.sprite.bounds.size;
        Vector3 scale = platform.transform.lossyScale;
        Destroy(platform);

        Vector2 realSize = new Vector2(size.x * scale.x, size.y * scale.y);
        Vector3 delta = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 localDelta = new Vector3();

        float angle; 
        for (int i = 0; i <= platformsCount; i++) {
            platform = createPlatform();

            if (i == platformsCount) {
                angle = Mathf.PI / 2;
            } else {
                float range = Mathf.PI / 4 * 
                    (complexityProgression * i / platformsCount);
                angle = Random.Range(-range, range);
            }

            platform.transform.Rotate(0, 0, Mathf.Rad2Deg * angle);
            platform.transform.parent = transform;

            delta += cornerCoords(localDelta, angle, realSize, -1);
            platform.transform.position = delta;
            delta += cornerCoords(localDelta, angle, realSize, 1);
        }

    }

    Vector3 cornerCoords(Vector3 pVector, float pAngle, Vector2 realSize, int side) {
        pVector.x = (Mathf.Cos(pAngle) * realSize.x - Mathf.Sin(pAngle) * realSize.y * side) / 2;
        pVector.y = (Mathf.Sin(pAngle) * realSize.x + Mathf.Cos(pAngle) * realSize.y * side) / 2; 

        return pVector;
    }

    GameObject createPlatform() {
        GameObject platform = GameObject.Instantiate(platformPrefab);

        return platform;
    }
}
