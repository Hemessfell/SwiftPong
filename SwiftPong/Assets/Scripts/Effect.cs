using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    
}

public interface IMyEffect
{
    void ApplyEffect();

    void DisableEffect();
}
