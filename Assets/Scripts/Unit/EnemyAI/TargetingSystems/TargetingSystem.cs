using System.Collections.Generic;
using MoriSkills;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "TargetingSystem", menuName = "ScriptableObjects/EnemyStateMachine/TargetingSystem/Generic", order = 1)]
[Serializable]
public class TargetingSystem : ScriptableObject
{
    public List<SkillId> possibleSkills;
    
}
