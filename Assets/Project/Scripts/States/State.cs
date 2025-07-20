using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "Scriptable Objects/State")]
public class State : ScriptableObject
{
    [SerializeField]
    private string id;

    public string ID => id;
}
