using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawprint : MonoBehaviour
{
    private Animal animal;

    public void SetAnimal(Animal ani)
    {
        animal = ani;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // do shit
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
