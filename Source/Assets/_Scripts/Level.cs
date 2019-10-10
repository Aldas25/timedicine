using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New level", menuName = "Level")]
public class Level : ScriptableObject
{
    public int levelId;
    public string levelName;
    public string introductionText;
    public Item giftItem;
    public Item finalItem;
    public bool hasTLimit;
    public float timeLimit;

    public bool hasTLReduction;
    public string textForTLReduction;
    public float timeLimitReduction;
    public Item itemForTLReduction;
}
