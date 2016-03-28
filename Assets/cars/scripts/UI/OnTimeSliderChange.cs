using UnityEngine;
using System.Collections;

public class OnTimeSliderChange : MonoBehaviour {
    public void onValueChange(float pValue) {
        Debug.Log("on value change");
        Time.timeScale = pValue;
    }
}
