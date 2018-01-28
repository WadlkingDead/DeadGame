using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "CreatNewAnimalBundle", fileName = "NewAnimalBundle")]
public class AnimalBundle : ScriptableObject
{

    public RuntimeAnimatorController controler;
    public AnimationClip[] clips;

}
