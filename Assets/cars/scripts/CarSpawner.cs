using UnityEngine;

public class CarSpawner : MonoBehaviour {
    // car prefab that will be used for car creation
    public GameObject carPrefab;

    public delegate void NumUpdated(int num);
    public NumUpdated PopulationNumUpdated;

    // current num of population
    private int _populationNum;

    // cars in population that are still running
    private float _carsToWait;

    void Start() {
        _carsToWait = 0;
        _populationNum = 0;
    }

    void Update() {
        // check if last population ended
        if (_carsToWait < 1) {
            updatePopulation();
            // spawn cars 
            SpawnCars();
        }
    }

    void updatePopulation() {
        _populationNum++;
        if (PopulationNumUpdated != null) {
            PopulationNumUpdated(_populationNum);
        }
    }

    /// <summary>
    /// Spawn cars based on chromosomes got from genetic algorithm
    /// </summary>
    void SpawnCars() {
        GeneticAlgorithm geneticAlgorithm = GetComponentInParent<GeneticAlgorithm>();

        geneticAlgorithm.GenerateNewPopulation();

        // create a car for each chromosome in new population
        foreach (CarChromosome chromosome in geneticAlgorithm.currentPopulation) {
            // instantiate a car from prefab
            GameObject car = GameObject.Instantiate(carPrefab);

            CarTracker tracker = car.GetComponent<CarTracker>();
            // init cars tracker with a chromosome and  
            tracker.init(chromosome, this);
            car.transform.position = transform.position;
            car.transform.parent = transform;
        }

        // set up cars to wait number so we wait with next population 
        _carsToWait = geneticAlgorithm.currentPopulation.Count;
    }

    public void skipToNextPopulation() {
        foreach (Transform child in transform) {
            RemoveCar(child.gameObject);
        }
    }

    public void resetPopulation() {
        // remove all cars
        skipToNextPopulation();

        // reset population number
        _populationNum = 0;

        // set algotithm back to first population
        GeneticAlgorithm geneticAlgorithm = 
            GetComponentInParent<GeneticAlgorithm>();
        geneticAlgorithm.FirstPopulation = true;
    }

    // todo: refactor to delegate
    public void RemoveCar(GameObject pCar) {
        --_carsToWait;
        Destroy(pCar);
    }
}
