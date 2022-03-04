using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMN : Singleton <GameMN>
{
    [SerializeField] PopUpMN prePopUpClose;
    [SerializeField] Transform trans;
    public bool isMoving = false;

    [SerializeField] GameObject preFadeBall;
    [HideInInspector]
    public GameObject currentFadeBall;

    

    private void Start()
    {
        currentFadeBall = Instantiate(preFadeBall, trans);
        currentFadeBall.SetActive(false);
    }
    public void GameOver ()
    {
        ScoreMN.Instance.SetHighScore();
        PopUpMN pop = Instantiate(prePopUpClose, trans);
    }

    public void ClickNewBall (Vector3 _trans)
    {
        
        currentFadeBall.SetActive(true);
        currentFadeBall.transform.position = _trans;
    }
}
