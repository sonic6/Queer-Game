using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QueerGame
{
    public class QueerFunctions : MonoBehaviour
    {
        public static float FindDistanceBetweenVectors(Vector3 position1, Vector3 position2)
        {
            float x1 = position1.x;
            float x2 = position2.x;
            float y1 = position1.y;
            float y2 = position2.y;
            float z1 = position1.z;
            float z2 = position2.z;
            float xPow = Mathf.Pow((x2 - x1), 2);
            float yPow = Mathf.Pow((y2 - y1), 2);
            float zPow = Mathf.Pow((z2 - z1), 2);
            float distance = Mathf.Sqrt(xPow + yPow + zPow);

            return distance;
        }

        /// <summary>
        /// Returns a NpcBehaviour List of NPCs that are available to convert (aren't already followers or polluted)
        /// </summary>
        /// <returns></returns>
        public static List<NpcBehaviour> FindAvailableNpcs()
        {
            List<NpcBehaviour> NPCs = new List<NpcBehaviour>();
            foreach(NpcBehaviour npc in FindObjectsOfType<NpcBehaviour>())
            {
                if(npc.isFollower == false && npc.convertedByEnemy == false)
                    NPCs.Add(npc);
            }

            return NPCs;

        }
    }
}
