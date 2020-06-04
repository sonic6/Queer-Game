using UnityEngine;
using TMPro;

public class FollowerCounter : MonoBehaviour
{
    public static int followers;
    [SerializeField] TMP_Text followersUi;
    private static TMP_Text followerCount;

    public static int npcCount;
    public static int pollutedCount;

    [Tooltip("The required amount of NPCs to be converted to win this level")]
    [SerializeField] int requiredConverts;
    public static int required;

    private void Start()
    {
        required = requiredConverts;

        NpcBehaviour[] myNpcs = FindObjectsOfType<NpcBehaviour>();
        npcCount = myNpcs.Length;

        followerCount = followersUi;
        followerCount.text = "Followers: 0/" + required;
    }

    public static void AddFollower()
    {
        followers++;
        followerCount.text = "Followers: " + followers.ToString() + "/" + required;
        if (followers == required)
            WinOrLose.WinGame();
    }

    //Is used by EnemyController script
    public static void CheckNonPollutedNpcs()
    {
        pollutedCount++;
        if (npcCount - required < pollutedCount)
            WinOrLose.LoseGame();
    }
}
