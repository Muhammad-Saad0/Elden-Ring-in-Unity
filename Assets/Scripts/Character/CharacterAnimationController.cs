using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    CharacterManager character;
    [SerializeField] private float animationDamptime = 0.1f;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateCharacterAnimatorParameters(float horizontalValue, float verticalValue)
    {
        //CHANGING ANIMATION PARAMETERS
        character.characterAnimator.SetFloat("Horizontal", horizontalValue, animationDamptime, Time.deltaTime);
        character.characterAnimator.SetFloat("Vertical", verticalValue, animationDamptime, Time.deltaTime);
    }
}
