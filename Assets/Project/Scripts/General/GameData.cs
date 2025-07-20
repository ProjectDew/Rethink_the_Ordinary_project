using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField]
    private float initialTime;

    [SerializeField]
    private float characterSpeed;

    [SerializeField]
    private int initialPropertyValue;
    
    [SerializeField]
    private int propertyValueChange;

    [SerializeField]
    private int propertyDangerThreshold;

    public float CharacterSpeed => characterSpeed;

    public float InitialTime => initialTime;

    public int InitialPropertyValue => initialPropertyValue;

    public int PropertyValueChange => propertyValueChange;

    public int PropertyDangerThreshold => propertyDangerThreshold;
}
