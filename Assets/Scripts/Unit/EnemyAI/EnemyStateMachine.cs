using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class EnemyStateMachine
{
    public EnemyState currentState;
    public List<EnemyState> states;

    [Serializable]
    struct StateTransition
    {
        public EnemyState targetState;

        public float weight;
        public int priority;

        [Serializable]
        public struct TransitionConditions
        {
            [Header("AP")]
            public bool CheckAP;
            public int MinAP;
            public int MaxAP;
            [Header("UnitsAlive")]
            public bool CheckAllyCount;
            public bool CheckAllyDeadCount;
            public bool CheckEnemyCount;
            public int Allies;
            public int AlliesDead;
            public int Enemies;
            [Header("TurnCount")]
            public bool CheckLocalTurnsCount;
            public bool CheckGlobalTurnsCount;
            public int LocalTurnCount;
            public int GlobalTurnCount;
            [Header("Fated")]
            public bool CheckFated;
            public string FateCode;
            [Header("Health")]
            public bool CheckSelfHP;
            public bool CheckEnemyHP;
            public bool CheckParyHPRatio;
            public bool CheckEnemyHPRatio;
            public float TargetHPRatio;
            public bool SelfHPGreater;
            public List<float> PartyHPRatio;
            public List<float> EnemyHPRatio;
            public List<int> EnemyHP;
            [Header("Conditions")]
            public bool CheckAllyConditions;
            public bool CheckEnemyConditions;
            public List<String> AllyConditionCheck;
            public List<String> EnemyConditionCheck;
        }
    }
}
