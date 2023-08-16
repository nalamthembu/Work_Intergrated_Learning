using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawprint : MonoBehaviour
{
    private Animal animal;
    public GameObject waypoint;
    private float timer = 0;

    private bool created = false;

    public void SetAnimal(Animal ani)
    {
        animal = ani;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            HUDManager.instance.ShowNotification("Press \"E\" to interact");
        }
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && created != true)
        {
            Instantiate(waypoint, new Vector3(animal.transform.position.x, animal.transform.position.y + 0.5f, animal.transform.position.z), Quaternion.identity);
            Destroy(gameObject, 5);
            created = true;
        }
    }
}
