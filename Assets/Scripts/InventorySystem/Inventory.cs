using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    protected Dictionary<string, StoredItem?> storedItems = new();
    
    protected int maxItems;

    protected int itemCount;

    public void AddItem(string name, IStorable item)
    {
        if (itemCount <= 0)
        {
            storedItems.Add(name, new StoredItem(name, item.GetGameObject(), transform));
        }

        if (!storedItems[name].HasValue)
        {
            storedItems[name].Value.SetQuantity(storedItems[name].Value.quantity + 1);
        }
    }

    public void SpawnStoredItem(string name, Vector3 position)
    {
        if (storedItems[name].HasValue)
        {
            storedItems[name].Value.gameObject.transform.SetParent(null);
            storedItems[name].Value.gameObject.transform.position = position;
            storedItems[name].Value.gameObject.SetActive(true);

            return;
        }

        Debug.LogError("Could not find item : " + name);
    }
}

[System.Serializable]
public struct StoredItem
{
    public string name;
    public GameObject gameObject;
    public int quantity;

    public StoredItem(string name, GameObject gameObject, Transform inventoryTransform, int quantity = 1)
    {
        this.name = name;
        this.gameObject = gameObject;
        this.quantity = quantity;

        gameObject.SetActive(false);
        gameObject.transform.SetParent(inventoryTransform);
        gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
    }

    public void SetQuantity(int newQuantity) => quantity = newQuantity;
}