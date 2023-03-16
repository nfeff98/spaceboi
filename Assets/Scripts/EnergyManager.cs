using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {


    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int energySpent = 5;
    [SerializeField] private int energyGain = 10;
    private static int currentEnergy = 100;


    public void EnergySpent() {
        currentEnergy -= energySpent;
        
        // Code to change energy bar ui
    }

    public void AteZaza() {
        currentEnergy += energyGain;

        // Code to change energy bar ui
    }

}
