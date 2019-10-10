using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public Item item1;
    public Item item2;
    public Item resultingItem;
}
