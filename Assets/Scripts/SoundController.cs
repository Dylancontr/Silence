using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundController : MonoBehaviour
{
    public static bool enemyAttack, enemyDmg, playerWalking, playerDmg;
    public AudioClip[] sound;
    public static AudioSource[] audio;

    // Start is called before the first frame update
    void Start()
    {
        enemyAttack = false;
        enemyDmg = false;
        playerWalking = false;
        playerDmg = false;
        audio = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gamePaused)
        {
            AudioListener.volume = 1f;
            if (enemyAttack && !audio[0].isPlaying)
            {
                audio[0].PlayOneShot(sound[3]);
                enemyAttack = false;
            }
            if (enemyDmg && !audio[0].isPlaying)
            {
                audio[0].PlayOneShot(sound[2]);
                enemyDmg = false;
            }
            if (playerWalking&&!audio[1].isPlaying)
            {
                audio[1].PlayOneShot(sound[0]);
            }
            if (playerDmg && !audio[0].isPlaying)
            {
                audio[0].PlayOneShot(sound[1]);
                playerDmg = false;
            }
        }
        else
        {
            AudioListener.volume = 0f;
        }
    }
}
