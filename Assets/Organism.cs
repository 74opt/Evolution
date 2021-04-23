using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism {
    // Standard Traits
    public float hunger;

    // Evolution Traits 
    public readonly float speed, reproductionValue, deathValue;
    
    public Organism(float hunger, float speed, float reproductionValue, float deathValue) {
        this.hunger = hunger;
        this.speed = speed;
        this.reproductionValue = reproductionValue;
        this.deathValue = deathValue;
    }
}
