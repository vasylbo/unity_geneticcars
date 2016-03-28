using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CarChromosome {
    public List<Gene> genes;

    private float _fitness;
    public float fitness {
        get { return _fitness;}
        set { _fitness = value;}
    }

    public CarChromosome(List<Gene> pGenes) {
        genes = pGenes;
    }
}
