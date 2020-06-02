using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnim : MonoBehaviour
{
    public Transform boyle;
    public PlayMakerFSM fsm;

    public void BoyleForward()
    {
        boyle.GetComponent<Animator>().SetTrigger("move");
    }

    public void BoyleSpeak()
    {
        fsm.SendEvent("talk");
    }
}
