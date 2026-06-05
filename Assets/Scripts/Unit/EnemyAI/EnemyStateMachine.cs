using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class EnemyStateMachine
{
    public EnemyState currentState;
    public List<EnemyState> states;
}
