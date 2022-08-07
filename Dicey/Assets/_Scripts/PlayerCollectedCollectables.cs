using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollectedCollectables : MonoBehaviour
{
    public TMP_Text CollectedDiceText;

    int _amountCollectedDice;

    private void OnLevelWasLoaded(int level)
    {
        CollectedDiceText = GameObject.Find("DiceCollectedCounter").GetComponent<TMP_Text>();
    }


    public void AddCollectableDice()
    {
        _amountCollectedDice++;
        CollectedDiceText.text = _amountCollectedDice.ToString();
    }
}
