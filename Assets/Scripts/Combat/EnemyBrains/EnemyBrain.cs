using MoriSkills;
using UnityEngine;


public enum TargetingStyle
{
    LowestHealthEnemy,
    HighestHealthEnemy,
    
    LowestHealthAlly,
    HighestHealthAlly,

    EnemyTeam,
    AllyTeam,

    self,
    All
}
public struct SkillUse
{
    public SkillId skillId;
    public UnitBody user;
    public TargetingStyle targeting;

}
[CreateAssetMenu(fileName = "EnemyBrain", menuName = "Scriptable Objects/EnemyBrain")]
public class EnemyBrain : ScriptableObject
{
    
}
