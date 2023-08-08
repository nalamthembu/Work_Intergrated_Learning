using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Footprints : MonoBehaviour
{
    // When the player interacts, all footprints will begin to glow
    // after a while it will stop glowing

    public List<GameObject> prints;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < prints.Count; i++)
            {
                prints[i].SetActive(true);
            }
        }
    }
}
