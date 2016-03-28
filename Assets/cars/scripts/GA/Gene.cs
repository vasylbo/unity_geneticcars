using UnityEngine;
using System.Collections;

[System.Serializable]
public class Gene {
    public string name;
    public float value;
    public float max;
    public float min;

    public Gene(string pName, float pValue, float pMin, float pMax) {
        name = pName;
        value = pValue;
        min = pMin;
        max = pMax;
    }

    public Gene Clone() {
        return new Gene(name, value, min, max);
    }

    public Gene CloneWithValue(float pValue) {
        return new Gene(name, pValue, min, max);
    }
}
