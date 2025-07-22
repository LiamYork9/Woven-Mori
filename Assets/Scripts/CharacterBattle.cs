using UnityEngine;

public class CharachterBattle : MonoBehaviour
{
   
    private SpriteRenderer setSprite;

    

    void Start()
    {

    }

    public void Setup(bool isPlayerTeam)
    {
        if (isPlayerTeam)
        {
   
           setSprite = BattleManager.GetInstance().pfCharacterBattle.GetComponent<SpriteRenderer>();
           setSprite.sprite = BattleManager.GetInstance().playerSprite;

             
         
        }
        else
        {
            
           setSprite = BattleManager.GetInstance().pfCharacterBattle.GetComponent<SpriteRenderer>();
           setSprite.sprite = BattleManager.GetInstance().enemySprite;

        }
    }

  
    
}
