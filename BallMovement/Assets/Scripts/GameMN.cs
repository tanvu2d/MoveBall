using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMN : Singleton <GameMN>
{
    [SerializeField] PopUpMN prePopUpClose;
    [SerializeField] Transform trans; 
   public void GameOver ()
    {
        ScoreMN.Instance.SetHighScore();
        PopUpMN pop = Instantiate(prePopUpClose, trans);
    }
}
