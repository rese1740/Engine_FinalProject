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

public enum SkillType
{
    None,
    Dash,
    Fireball,
    Heal
}

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string ItemID;
    public string ItemName;
    public string description;
    public GameObject prefab;
    public Sprite icon;
    public ItemType ItemType;
    public ArtifactType ArtifactType;
    public SkillType SkillType;
}
