using UnityEngine;
using System.Collections.Generic;

public class Crossover : MonoBehaviour {
    public static CarChromosome[] crossover(CarChromosome pair1, CarChromosome pair2) {
        int l = Mathf.Min(pair1.genes.Count, pair2.genes.Count);
        int rand1 = Random.Range(0, l); 
        int rand2 = Random.Range(0, l);
        int from = Mathf.Min(rand1, rand2);
        int to = Mathf.Max(rand1, rand2);

        var child1 = new List<Gene>(l);
        var child2 = new List<Gene>(l);
        for (var i = 0; i < l; i++) {
            if (i < from || i > to) {
                child1.Add(pair1.genes[i].Clone());
                child2.Add(pair2.genes[i].Clone());
            } else {
                child1.Add(pair2.genes[i].Clone());
                child2.Add(pair1.genes[i].Clone());
            }
        }
        
        return new CarChromosome[2]{
            new CarChromosome(child1), new CarChromosome(child2)
        };
    }
}
