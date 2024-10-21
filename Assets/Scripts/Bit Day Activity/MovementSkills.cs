using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSkills : MonoBehaviour
{
    public bool sprint=false;
    public bool dash=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<PlayerController>().sprintUnlocked = sprint;
        gameObject.GetComponent<PlayerController>().dashUnlocked = dash;
    }
}
