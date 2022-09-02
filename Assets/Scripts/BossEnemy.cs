using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemy: TestEnemy
{

    private int hp;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        hp = 6;
        base.setHealth(hp);
    }

    public override void die()
    {
        SceneManager.LoadScene(3);
    }
}
