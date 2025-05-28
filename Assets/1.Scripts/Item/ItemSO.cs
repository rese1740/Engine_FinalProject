using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArtifactType
{
    Passive,
    Active
}

public enum ItemType
{
    consumption,
    Artifact
}

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    [Header("Info")]
    public string ItemID;
    public string ItemName;
    public ItemType ItemType;
    public ArtifactType ArtifactType;
}
