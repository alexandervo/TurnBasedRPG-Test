public enum CombatStyle
{
    Melee = 1,
    Range = 2,
    Magic = 3
}

public enum CostType
{
    MP = 1,
    TP = 2,
    HP = 3
}

public enum SelectionMode
{
    Single = 1,
    All = 2
}

[System.Flags]
public enum SelectionType
{
    Self = 1,
    Foe = 2,
    Ally = 4
}

public enum StatusCalcType
{
    Flat = 1,
    Percentage = 2
}


