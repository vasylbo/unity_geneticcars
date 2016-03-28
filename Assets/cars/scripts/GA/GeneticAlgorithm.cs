using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneticAlgorithm : MonoBehaviour {
    [Range(0f, 1f)]
    public float MutationRate = 0.04f;
    
    public uint PopulationSize = 8;

    public uint EliteSelection = 1;

    public float MinBodyPartRadius = 0.3f;
    public float MaxBodyPartRadius = 1.3f;

    public int MinBodyPartAngle = 10;
    public int MaxBodyPartAngle = 90;

    public float MinWheelSize = 0.5f;
    public float MaxWheelSize = 1.5f;

    public List<CarChromosome> currentPopulation;

    public bool FirstPopulation;

    void Start() {
        FirstPopulation = true;
    }

    public void GenerateNewPopulation() {
        if (FirstPopulation) {
            FirstPopulation = false;
            GenerateFirstPopulation();
        } else {
            NewPopulation();
        }
    }

    void GenerateFirstPopulation() {
        currentPopulation = new List<CarChromosome>((int) PopulationSize);
        for (var i = 0; i < PopulationSize; i++) {
            currentPopulation.Add(RandomChromosome());
        }
    }

    void NewPopulation() {
        currentPopulation = GenerateCrossoveredPopulation();
    }
    List<CarChromosome> GenerateCrossoveredPopulation() {
        currentPopulation.Sort((a, b) => {
            if (a.fitness > b.fitness) {
                return 1;
            } else if (a.fitness > b.fitness) {
                return -1;
            }

            return 0;
        });

        List<CarChromosome> newPopulation = new List<CarChromosome>((int) PopulationSize);
        for (var i = 0; i < EliteSelection && i < currentPopulation.Count; i++) {
            newPopulation.Add(currentPopulation[i]);
        }

        RouletteWheelSelection.Evolve(currentPopulation, newPopulation, (int) PopulationSize - newPopulation.Count);

        foreach (var chromosome in newPopulation) {
            mutateChromosome(chromosome);
        }

        return newPopulation;
    }

    // mutates single chromosome using mutation rate
    // if MutationRate 1 - every gene will mutate
    void mutateChromosome(CarChromosome pChromosome) {
        // going throu genes of chromosome
        foreach (var gene in pChromosome.genes) {
            var random = Random.Range(0f, 1f);

            // mutate a gene if it got picked by random 
            if (random < MutationRate) {
                // mutate value using min and max values of this gene
                gene.value = Random.Range(gene.min, gene.max);
            }
        }
    }

    CarChromosome RandomChromosome() {
        int vertices = 6;

        List<Gene> genes = new List<Gene>();

        for (var i = 0; i < vertices; i++) {
            genes.Add(new Gene("Body part " + i + " radius", 
                Random.Range(MinBodyPartRadius, MaxBodyPartRadius), MinBodyPartRadius, MaxBodyPartRadius)); // radius from center of car
            genes.Add(new Gene("Body part " + i + " angle",
                Random.Range(MinBodyPartAngle, MaxBodyPartAngle), MinBodyPartAngle, MaxBodyPartAngle)); // vertice angle
        }

        var wheels = 2;
        var addedWheels = 0;

        while (addedWheels < wheels) {
            var j = Random.Range(0, vertices - 1);
            genes.Add(new Gene("Wheel to body index",
                j, 0, vertices - 1)); // vertice index
            genes.Add(new Gene("Wheel " + j + " radius", 
                Random.Range(MinWheelSize, MaxWheelSize), MinWheelSize, MaxWheelSize)); // size of wheel
            ++addedWheels;
        }
        return new CarChromosome(genes);
    }
}
