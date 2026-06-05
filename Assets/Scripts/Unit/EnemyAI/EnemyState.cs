using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyState", menuName = "ScriptableObjects/EnemyStateMachine/EnemyState/Generic", order = 1)]
[Serializable]
public class EnemyState : ScriptableObject
{
    public TargetingSystem activeTargeting;
    public List<TargetingSystem> alternateSystems;
}
