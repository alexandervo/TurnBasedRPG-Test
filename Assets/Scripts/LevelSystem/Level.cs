using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int experience;
    public int currentlevel;
    public Action OnLevelUp;
    public int requiredExp;

    public int MAX_EXP;
    public int NEXT_EXP;
    public int MAX_LEVEL = 99;
    public Level(int level, Action OnLevUp)
    {
        MAX_EXP = GetXPForLevel(MAX_LEVEL);
        currentlevel = level;
        experience = GetXPForLevel(level);
        OnLevelUp = OnLevUp;
        requiredExp = CalculateRequiredExp(level);
    }

    public int GetXPForLevel(int level)
    {
        if (level > MAX_LEVEL) //throw an exception dependant on game design
            return 0;

        int firstPass = 0;
        int secondPass = 0;
        for (int levelCycle = 1; levelCycle < level; levelCycle++)
        {
            firstPass += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
            secondPass = firstPass / 4;
        }
        if (secondPass > MAX_EXP && MAX_EXP != 0)
            return MAX_EXP;

        if (secondPass < 0)
            return MAX_EXP;

        return secondPass;
    }

    public int CalculateRequiredExp(int level)
    {
        int solveForRequiredExp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredExp += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
        }
        return solveForRequiredExp / 4;
    }

    public int GetLevelForXP(int exp)
    {
        if (exp > MAX_EXP)
            return MAX_EXP;

        int firstPass = 0;
        int secondPass = 0;
        for (int levelCycle = 1; levelCycle <= MAX_LEVEL; levelCycle++)
        {
            firstPass += (int)Math.Floor(levelCycle + (300.0f * Math.Pow(2.0f, levelCycle / 7.0f)));
            secondPass = firstPass / 4;
            if (secondPass > exp)
                return levelCycle;
        }

        if (exp > secondPass)
            return MAX_LEVEL;

        return 0;
    }

    public bool AddExp(int amount)
    {
        if (amount + experience < 0 || experience > MAX_EXP)
        {
            if (experience > MAX_EXP)
                experience = MAX_EXP;
            return false;
        }
        int oldLevel = GetLevelForXP(experience);
        int nextLevel = oldLevel + 1;
        experience += amount;
        if(oldLevel < GetLevelForXP(experience))
        {
            if(currentlevel < GetLevelForXP(experience))
            {
                currentlevel = GetLevelForXP(experience);
                if (OnLevelUp != null)
                    OnLevelUp.Invoke();
                    requiredExp = CalculateRequiredExp(nextLevel);
                return true;
            }
        }
        return false;
    }
}
