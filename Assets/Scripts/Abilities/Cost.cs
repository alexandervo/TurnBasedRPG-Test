using UnityEngine;

[System.Serializable]
public class Cost
{
    [SerializeField]
    protected CostType type;
    [SerializeField]
    protected int amount;

    public CostType Type { get => type; protected set => type = value; }
    public int Amount { get => amount; protected set => amount = value; }
}
