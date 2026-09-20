using System.Collections.Generic;
using MoriSkills;
using UnityEngine;

public enum TargetGroup
{
    LowHP,
    HighHP,
    MissingHPPercent,   
}

[CreateAssetMenu(fileName = "UnitBrain2", menuName = "Scriptable Objects/EnemyBrain/T2")]
public class Tier2Brain: UnitBrain
{
    public List<Category> preferredCategory;
    public List<Target> preferredTargetMethod;

}
