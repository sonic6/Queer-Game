using UnityEngine.UI;
using UnityEngine;

public class FollowerCounter : MonoBehaviour
{
    private static int followers;
    [SerializeField] Text followersUi;
    private static Text followerCount;
    private static int required = 5;

    private void Start()
    {
        followerCount = followersUi;
        followerCount.text = "Followers: 0/" + required;
    }

    public static void AddFollower()
    {
        followers++;
        followerCount.text = "Followers: " + followers.ToString() + "/" + required;
        if (followers == required)
            print("Won game");
    }
}
