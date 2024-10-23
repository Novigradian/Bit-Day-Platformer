using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpModifier : MonoBehaviour
{

    public bool doubleJump=false;
    [Range(1,1.6f)]public float jumpHeightMultiplier=1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<PlayerController>().jumpUnlocked = doubleJump;
        gameObject.GetComponent<PlayerController>().jumpMultiplier = jumpHeightMultiplier;
    }
}
