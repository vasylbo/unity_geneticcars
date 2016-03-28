﻿using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Roulette-wheel selection implementation. 
/// Made more readable but less memory 
/// and computational efficient to perfectly 
/// show how its actually working. 
/// </summary>
public class RouletteWheelSelection {

    /// <summary>
    /// Decorator fot round wheel probability checking
    /// </summary>
    class ChromosomeDecorator {
        public CarChromosome chromosome;
        public float probability;
        public ChromosomeDecorator(CarChromosome pChromosome, float pProbability) {
            chromosome = pChromosome;
            probability = pProbability;
        }
    }

    /// <summary>
    /// Evolve given population with a roulette-wheel selection.
    /// </summary>
    /// <param name="pPopulation"></param>
    /// <returns>New population evolved from given</returns>
    public static void Evolve(
            List<CarChromosome> pPopulation, List<CarChromosome> pNewPopulation, int pChromosomesNeeded) {
        List<ChromosomeDecorator> probabilities; 
        List<CarChromosome> population;

        int added = 0;

        while (added < pChromosomesNeeded) {
            // create temporary population 
            population = new List<CarChromosome>(pPopulation);

            // get probabilities
            probabilities = populateProbabilities(population);

            // choose first pair
            CarChromosome pair1 = chooseChromosome(probabilities);

            // remove it from whole population to prevent crossovering between single chromosome
            population.Remove(pair1);

            // recalculate probabilities with out first pair
            probabilities = populateProbabilities(population);

            // get second pair
            CarChromosome pair2 = chooseChromosome(probabilities);

            // populate new chromosomes with crossover 
            pNewPopulation.AddRange(Crossover.crossover(pair1, pair2));

            // increase added counter by two, because two new chromosomes will be generated by crossover
            added += 2;
        }
    }

    /// <summary>
    /// Select a chromosome based on theirs 
    /// probabilities to be selected
    /// </summary>
    /// <param name="pProbabilities"></param>
    /// <returns></returns>
    static CarChromosome chooseChromosome(List<ChromosomeDecorator> pProbabilities) {
        float random = Random.Range(0f, 1f);
        for (var j = 0; j < pProbabilities.Count; j++) {
            ChromosomeDecorator test = pProbabilities[j];

            if (test.probability > random) {
                return test.chromosome;
            }
        }
        return pProbabilities[0].chromosome;
    }


    /// <summary>
    /// Populates array of (chromosome, probability) objects
    /// to simplyfy rest of code
    /// </summary>
    /// <param name="pPopulation"></param>
    /// <returns></returns>
    static List<ChromosomeDecorator> populateProbabilities(List<CarChromosome> pPopulation) {
        List<ChromosomeDecorator> probabilities = 
            new List<ChromosomeDecorator>();
        float totalfitness = sumOfFitnesses(pPopulation);
        float sum = 0;
        foreach (var chromosome in pPopulation) {
            float probability = sum + chromosome.fitness / totalfitness;

            probabilities.Add(
                new ChromosomeDecorator(chromosome, probability));

            sum += probability;
        }
        return probabilities;
    }


    /// <summary>
    /// Sums up fitness of whole chromosome
    /// </summary>
    /// <param name="pPopulation"></param>
    /// <returns></returns>
    static float sumOfFitnesses(List<CarChromosome> pPopulation) {
        float sum = 0;
        foreach (CarChromosome chromosome in pPopulation) {
            sum += chromosome.fitness;
        }
        return sum;
    }
}
