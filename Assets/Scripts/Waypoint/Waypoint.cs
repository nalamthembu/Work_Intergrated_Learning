using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private PlayerCharacter player;
    public Image image;

    void Start()
    {
        player = FindObjectOfType<PlayerCharacter>();
       
    }

    
    void Update()
    {
        transform.LookAt(player.transform);
        
        image.rectTransform.localScale = Mathf.Clamp(Vector3.Distance(transform.position, player.transform.position)/10,1,10) * Vector3.one;

        image.rectTransform.position = new Vector3(transform.position.x, Mathf.Clamp(Vector3.Distance(transform.position, player.transform.position) / 10, 1, 10), transform.position.z);

        if ((player.transform.position - this.transform.position).sqrMagnitude < 100)
        {
            Destroy(gameObject);
        }
    }
}
