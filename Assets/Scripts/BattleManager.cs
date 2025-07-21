using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;

    public static BattleManager GetInstance()
    {
        return instance;
    }
    public Transform pfCharacterBattle;

    public SpriteRenderer teamColor;

    public Sprite  playerSprite;

    public Sprite enemySprite;

    public CharachterBattle cB;

    public void Awake()
    {
        instance = this;
    }


    void Start()
    {
        SpawnCharchter(true);
        SpawnCharchter(false);
    }

    private void SpawnCharchter(bool isPlayerTeam)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            position = new Vector3(-5, 0);
            teamColor = pfCharacterBattle.GetComponent<SpriteRenderer>();
            teamColor.color = Color.blue;
        }
        else
        {
            position = new Vector3(+5, 0);
            teamColor = pfCharacterBattle.GetComponent<SpriteRenderer>();
            teamColor.color = Color.red;
        }
        Instantiate(pfCharacterBattle, position, Quaternion.identity);
        
        cB.Setup(isPlayerTeam);
    }
}
