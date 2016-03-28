using UnityEngine;
using System.Collections;

public class CarTracker : MonoBehaviour {
    float lastX;
    float lastY;

    float lastChecked;

    public CarChromosome carChromosome;

    public Vector3 center;
    public float fitness;

    public float currentPosition;
    private float _startPosition;

    public void init(CarChromosome pChromosome, CarSpawner pCarSpawner) {
        carChromosome = pChromosome;
        // todo: has to be independed
        CarBuilder.OnCarUpdated(gameObject);
    }

	void Start () {
        _startPosition = getPosition().x;
	}

    /// <summary>
    /// Called when you change chromosome parameters in editor.
    /// </summary>
    void OnValidate() {
        if (CarBuilder.OnCarUpdated != null) {
            CarBuilder.OnCarUpdated(gameObject);
        }
    }

	void Update () {
        float time = Time.time;

        // Calculate fitness every frame
        CalculateFitness();

        // Once in a second check if car still moves
        if (time - lastChecked > 1) {
            Vector3 currentBounds = getPosition();
            float currentX = currentBounds.x;
            float currentY = currentBounds.y;

            if (Mathf.Abs(currentX - lastX) < 0.1 && 
                Mathf.Abs(currentY - lastY) < 0.1) {
                RemoveCar();
                return;
            }

            lastX = currentX;
            lastY = currentY;

            lastChecked = time;
        }
    }

    // Gets bounds of our car's physical body
    public Vector3 getPosition() {
        return transform.GetChild(0).position;
    }

    void RemoveCar() {
        if (transform.parent) {
            transform.parent.GetComponentInParent<CarSpawner>().RemoveCar(gameObject);
        }
    }

    void OnDestroy() {
        carChromosome = null;
    }

    void CalculateFitness() {
        currentPosition = getPosition().x;
        var dX = Mathf.Abs(currentPosition - _startPosition);
        fitness = carChromosome.fitness = Mathf.Max(carChromosome.fitness, dX);
    }

}
