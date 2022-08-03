using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollectedCollectables : MonoBehaviour
{
    public TMP_Text CollectedDiceText;

    int _amountCollectedDice;
    public void AddCollectableDice()
    {
        _amountCollectedDice++;
        CollectedDiceText.text = _amountCollectedDice.ToString();
    }
}
