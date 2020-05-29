using UnityEngine;

public class ResetCards : MonoBehaviour
{
    private void Awake()
    {
        InfoDealer.cardsInHand.Clear();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
