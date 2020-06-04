using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QueerGame
{
    public class QueerFunctions : MonoBehaviour
    {
        public static bool MoveTowardsIsRunning = false;
        
        /// <summary>
        /// Finds the distance in 3D space between two Vector3 variables
        /// </summary>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts given list of gameObjects into instantiated list of gameObjects
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static List<GameObject> ConvertToSceneRefrence(List<GameObject> sourceList)
        {
            for (int i = 0; i < sourceList.Count; i++) //It's important that this loop exist to convert the refrences in 
            {
                GameObject newCard = Instantiate(sourceList[i]);
                sourceList[i] = newCard;
                sourceList[i].SetActive(false);
            }
            BookManager.cardScale = sourceList[0].transform.localScale; //Tells the BookManager what the original scale of the first card was
            return sourceList;
        }

        public static IEnumerator OpenCloseBook(List<GameObject> cardsInHand, string activeState)
        {
            if (MoveTowardsIsRunning == false)
            {
                MoveTowardsIsRunning = true;

                Cursor.visible = !Cursor.visible;
                //Cursor.lockState = CursorLockMode.None;
                PlayerMovement.MoveActiveState = !PlayerMovement.MoveActiveState;

                foreach (GameObject card in cardsInHand)
                {
                    if (activeState == "yes")
                    {
                        card.gameObject.SetActive(true);
                        if(Verses.extraStrength > 0)
                            card.GetComponent<Verses>().myExtraPoints.text = "+" + Verses.extraStrength.ToString();

                        while (Vector3.Distance(card.transform.position, card.GetComponent<Verses>().myPosition.transform.position) > 1)
                        {
                            card.transform.position = Vector3.MoveTowards(card.transform.position, card.GetComponent<Verses>().myPosition.transform.position, 70);
                            yield return new WaitForEndOfFrame();
                        }
                    }
                    if (activeState == "no")
                    {
                        
                        while (Vector3.Distance(card.transform.position, BookManager.manager.transform.position) > 1)
                        {
                            card.transform.position = Vector3.MoveTowards(card.transform.position, BookManager.manager.transform.position, 70);
                            yield return new WaitForEndOfFrame();
                        }
                        card.gameObject.SetActive(false);
                    }
                }
                MoveTowardsIsRunning = false;
                //Cursor.visible = !Cursor.visible;
            }
            yield break;
        }

        public static IEnumerator CallMethodInDisabledObject(MonoBehaviour script, string message)
        {
            while (script.gameObject.activeSelf == false)
            {
                yield return new WaitForEndOfFrame();
            }
            script.SendMessage(message);
        }

        public static IEnumerator CanvasLookAtCamera(Canvas canvas)
        {
            while(canvas)
            {
                canvas.transform.LookAt(Camera.main.transform);
                canvas.transform.eulerAngles = new Vector3(canvas.transform.eulerAngles.x, canvas.transform.eulerAngles.y, 0);
                yield return new WaitForEndOfFrame();
            }
            yield break;
        }

    }
}
