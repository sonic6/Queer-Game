using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QueerGame;

public class GroupTool : MonoBehaviour
{
    public List<NpcBehaviour> npcs; //The NPC members of this group

    [SerializeField]int celebrityRequired;
    [SerializeField]int cultureRequired;

    [HideInInspector] public int cultureUsed;
    [HideInInspector] public int celebrityUsed;

    [Tooltip("Insert a points canvas prefab here")]
    [SerializeField] GameObject pointsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 2; //Ignore raycast layer
        GetComponent<Collider>().isTrigger = true;
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;

        foreach(NpcBehaviour npc in npcs)
        {
            npc.partOfGroup = this;

            celebrityRequired += npc.cultureRequired;
            cultureRequired += npc.celebrityRequired;
            npc.GetComponentInChildren<SphereCollider>().enabled = false; //Disable the individual sphere colliders for the members in the group
        }
        celebrityRequired = celebrityRequired - (npcs.Count * 2);
        cultureRequired = cultureRequired - (npcs.Count * 2);

        Invoke("SetupPointsCanvas", 0.1f); //To make sure all individual NPCs have setup their Canvases onStart first, then do this after .1 seconds

    }

    void SetupPointsCanvas()
    {
        GameObject points = Instantiate(pointsCanvas, transform);
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
        points.transform.position = PointsCanvasCenter();
        points.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        foreach(NpcBehaviour npc in npcs)
        {
            Destroy(npc.pointsCanvas);
        }
        StartCoroutine(QueerFunctions.CanvasLookAtCamera(points.GetComponent<Canvas>()));
    }

    Vector3 PointsCanvasCenter()
    {
        var bounds = new Bounds(npcs[0].transform.position, Vector3.zero);
        for(int i = 0; i < npcs.Count; i++)
        {
            bounds.Encapsulate(npcs[i].transform.position);
        }
        return bounds.center;
    }

    public void GroupFollowPlayer() //Gets called in "Verses" script
    {
        if (cultureRequired <= cultureUsed && celebrityRequired <= celebrityUsed)
        {
            foreach (NpcBehaviour npc in npcs)
            {
                npc.isFollower = true;
                FollowerCounter.AddFollower(); //Because this is in a foreach loop. It will correctly add the number of followers in the group
                npc.aiAgent.stoppingDistance = 2; //The distance each npc will keep from the player while following
                npc.CallFollow();
            }

            BookManager.manager.DrawNewCards();

            Verses[] cardsInHand = BookManager.manager.pagesHolder.GetComponentsInChildren<Verses>();
            Verses.extraStrength += npcs.Count;
            foreach (Verses card in cardsInHand)
            {
                card.strength += Verses.extraStrength;
                card.myExtraPoints.text = "+" + Verses.extraStrength.ToString();
            }

            Verses.myGroup = null;
            Destroy(gameObject); //Destroys the gameobject attached to this GroupTool
        }
    }
}
