using System.Collections.Generic;
using MoriSkills;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassGrowth", menuName = "Scriptable Objects/ClassGrowth")]
public class ClassGrowth : ScriptableObject
{
    public Classes playerClass;
    public List<int> statGrowth = new List<int> {0,0,0,0,0};
    public List <int> levels;

    public List <SkillId> skill;


}
