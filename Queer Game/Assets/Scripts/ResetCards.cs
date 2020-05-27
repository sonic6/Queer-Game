using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCards : MonoBehaviour
{
    private void Awake()
    {
        InfoDealer.cardsInHand.Clear();
    }
}
