using UnityEngine;

[CreateAssetMenu(fileName = "Stand Name", menuName = "ScienceFair/StandStats", order = 1)]
public class StandScriptableObject : ScriptableObject
{
    [Range(0.0f, 1.0f)]
    public float science;
    [Range(0.0f, 1.0f)]
    public float technology;
}

