using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;
using QueerGame;
using System.Collections.Generic;
using System.Collections;
using HutongGames.PlayMaker;

public class NpcBehaviour : MonoBehaviour
{
    /////////Several other scripts inherit variables from this script////////


    [UnityEngine.Tooltip("A random audio clip from this list is played when the player approaches this NPC")]
    public AudioClip[] triggerSounds;

    [UnityEngine.Tooltip("A random audio clip from this list is played when the player recruits this NPC")]
    public AudioClip[] followSounds;

    [UnityEngine.Tooltip("The walking speed of this gameobjects ai agent")]
    public float walkingSpeed;

    [HideInInspector] public GameObject horns = null; //The horns of this NPC
    [HideInInspector] public bool looping = true; //Used to start looping the FollowPlayerPosition coroutine
    
    [HideInInspector] public Animator myAnimator;
    [HideInInspector] public GameObject player; //This is the player gameobject

    [HideInInspector] public bool convertedByEnemy = false; //If this bool is true it means the player shouldn' be able to convert this npc to their side

    [HideInInspector] public bool isFollower = false; //When the value of this bool is true, it means this NPC is a follower

    [UnityEngine.Tooltip("This is how much material points this npc requires")]
    public int celebrityRequired;
    [UnityEngine.Tooltip("This is how much argument points this npc requires")]
    public int cultureRequired;

    //This is how much material points the player has used for this npc
    [HideInInspector] public int celebrityUsed;
    //This is how much argument points the player has used for this npc
    [HideInInspector] public int cultureUsed;

    [HideInInspector] public NavMeshAgent aiAgent;

    [UnityEngine.Tooltip("The time in seconds it takes for this NPC to recruit another NPC")]
    [SerializeField] float timeToGetOtherNpc;

    [UnityEngine.Tooltip("Insert a points canvas prefab here")]
    public GameObject pointsCanvas;

    [HideInInspector] public Canvas drainCanvas;
    

    private void Start()
    {
        drainCanvas = GetComponentInChildren<Canvas>();
        drainCanvas.gameObject.SetActive(false);

        GameObject points = Instantiate(pointsCanvas, transform);
        pointsCanvas = points;
        points.GetComponent<Canvas>().worldCamera = Camera.main;

        int[] requirements = { celebrityRequired, cultureRequired };
        List<Transform> childs = new List<Transform>(points.GetComponentsInChildren<Transform>());
        childs.Remove(childs[0]);

        for (int i = 0; i < 2; i++)
        {
            var txt = childs[i].GetComponent<TMP_Text>();
            txt.SetText(requirements[i].ToString());
            txt.color = BookManager.manager.celebrityColor;
            if (i > 0) txt.color = BookManager.manager.cultureColor;
        }

        StartCoroutine(QueerFunctions.CanvasLookAtCamera(points.GetComponent<Canvas>()));
    }

    private void OnMouseDown()
    {
        if (isFollower && Verses.extraStrength != 0)
        {
            QueerFunctions.CallMethodInDisabledObject(this, "RemoveExtraStrengthUi");
            StartCoroutine(RecruitOthers());
        }
    }

    public void RemoveExtraStrengthUi()
    {
        Verses.extraStrength--;
        Verses[] cards = BookManager.manager.pagesHolder.GetComponentsInChildren<Verses>();

        foreach (Verses card in cards)
        {
            card.myExtraPoints.text = "+" + Verses.extraStrength.ToString();
            if (Verses.extraStrength == 0)
                card.myExtraPoints.text = "";
            card.strength--;
        }
    }

    IEnumerator RecruitOthers()
    {
        
        List<NpcBehaviour> npcs = QueerFunctions.FindAvailableNpcs();
        Transform closest = npcs[0].transform;
        foreach (NpcBehaviour npc in npcs)
        {
            float myDistance = QueerFunctions.FindDistanceBetweenVectors(npc.transform.position, transform.position);
            float closesDistance = QueerFunctions.FindDistanceBetweenVectors(closest.transform.position, transform.position);
            if (myDistance < closesDistance)
                closest = npc.transform;
        }

        looping = false;
        aiAgent.SetDestination(closest.transform.position);

        while(aiAgent.velocity.magnitude > 0)
        {
            //AI keeps walking
            yield return new WaitForSeconds(1f);
        }

        //If while converting other NPC, the player converts them first then move on
        float myTime = timeToGetOtherNpc;
        while(myTime > 0)
        {
            myTime--;
            yield return new WaitForSeconds(1);
            if (closest.GetComponent<NpcBehaviour>().isFollower == true)
                break;
        }

        if (closest.GetComponent<NpcBehaviour>().isFollower == false && convertedByEnemy == false) //Good to check again in case the player or enemy got to the NPC first
        {
            closest.GetComponent<NpcBehaviour>().FollowPlayer(false); //False means this is not the player doing the recruiting, but will still recruit the NPC
        }

        if(WinOrLose.myUiScreen.gameObject.activeSelf == false) //If the game hasn't been won or lost yet
        {
            StartCoroutine(RecruitOthers());
        }
        yield break;
    }

    //Is called by other scripts to make coroutine FollowPlayerPosition work correctly. Otherwise the while loop doesn't actually loop for no apparent reason
    public void CallFollow()
    {
        StartCoroutine(FollowPlayerPosition());
    }

    public IEnumerator FollowPlayerPosition() //Will work as an update to find the player's location but, can be stopped when needed
    {
        while(looping)
        {
            aiAgent.SetDestination(player.transform.position);
            yield return new WaitForFixedUpdate();
        }
        yield break;

    }

    
    public void FollowPlayer(bool isPlayer) //Gets called in "Verses" script
    {
        if (celebrityRequired <= celebrityUsed && cultureRequired <= cultureUsed || isPlayer == false)
        {
            AudioSource source = GetComponent<AudioSource>();
            AudioClip myClip = followSounds[Random.Range(0, followSounds.Length)];
            source.clip = myClip;
            source.Play();

            pointsCanvas.SetActive(false);

            isFollower = true;
            FollowerCounter.AddFollower();
            transform.GetChild(0).GetComponent<SphereCollider>().enabled = false;
            aiAgent.stoppingDistance = 2; //The distance this npc will keep from the player while following

            if (EnemyController.currentTarget == gameObject)
            {
                EnemyController.mainEnemy.FindNewTarget();
            }
            

            Verses[] cardsInHand = BookManager.manager.pagesHolder.GetComponentsInChildren<Verses>();
            Verses.extraStrength++;
            foreach (Verses card in cardsInHand)
            {
                QueerFunctions.CallMethodInDisabledObject(card, "AddExtraStrengthUi")/*  card.AddExtraStrengthUi()*/;
                card.strength += Verses.extraStrength;
                card.myExtraPoints.text = "+" + Verses.extraStrength.ToString();
            }

            if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                BookManager.manager.MoveNext(BookManager.manager.tutorialFsm);
            }

            StartCoroutine(FollowPlayerPosition());

        }
    }

    public Vector2 CalculateDistance()
    {
        float distanceX;
        float distanceZ;
        distanceX = Mathf.Abs(player.transform.position.x - gameObject.transform.position.x);
        distanceZ = Mathf.Abs(player.transform.position.z - gameObject.transform.position.z);
        Vector2 vec = new Vector2(distanceX, distanceZ);
        return vec;
    }
}

public class TalkTrigger : MonoBehaviour
{
    //This class is used to identify the object it's attached to

    
}
