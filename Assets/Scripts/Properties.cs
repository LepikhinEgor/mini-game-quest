using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour {

    public static float timeBetweenTools = 4F;

    private static float inventoryShare;
    public static float InventoryShare
    {
        get { return inventoryShare; }
    }

    public static int inventoryItemsCount;
    public static float InventoryItemsCount
    {
        get { return inventoryItemsCount; }
    }

    private void Awake()
    {
        inventoryShare = 1F / 6F;
        inventoryItemsCount = 4;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
