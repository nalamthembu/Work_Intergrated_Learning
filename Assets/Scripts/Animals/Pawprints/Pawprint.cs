using UnityEngine;

public class Pawprint : MonoBehaviour
{
    private Animal animal;
    public GameObject waypoint;

    private bool created = false;

    public void SetAnimal(Animal animal)
    {
        this.animal = animal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            HUDManager.instance.ShowNotification("Press \"E\" to interact");
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