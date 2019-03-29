using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the outcome of a dance off between 2 dancers, determines the strength of the victory form -1 to 1
/// 
/// TODO:
///     Handle GameEvents.OnFightRequested, resolve based on stats and respond with GameEvents.FightCompleted
///         This will require a winner and defeated in the fight to be determined.
///         This may be where characters are set as selected when they are in a dance off and when they leave the dance off
///         This may also be where you use the BattleLog to output the status of fights
///     This may also be where characters suffer mojo (hp) loss when they are defeated
/// </summary>
public class FightManager : MonoBehaviour
{
    public Color drawCol = Color.gray;

    public float fightAnimTime = 2;

    private void OnEnable()
    {
        GameEvents.OnFightRequested += Fight;
    }

    private void OnDisable()
    {
        GameEvents.OnFightRequested -= Fight;
    }

    public void Fight(FightEventData data)
    {
        StartCoroutine(Attack(data.lhs, data.rhs));
    }

    IEnumerator Attack(Character lhs, Character rhs)
    {
        lhs.isSelected = true;
        rhs.isSelected = true;
        lhs.GetComponent<AnimationController>().Dance();
        rhs.GetComponent<AnimationController>().Dance();

        yield return new WaitForSeconds(fightAnimTime);

        int outcome = 0;
        Character winner = lhs, defeated = rhs;

        // Roll a random 6 sided die.
        int d6 = Random.Range(1, 7);
        //Debug.Log(d6 + " d6");

        // Get a temporary variable to store the rhythm and style for the player and NPC.
        int tempPlayerRhythm = lhs.rhythm;
        int tempPlayerStyle = lhs.style;
        int tempNpcRhytm = rhs.rhythm;
        int tempNpcStyle = rhs.style;

        // If the dice roll is less than 3...
        if (d6 < 3)
        {
            // ...Increase the players temporary rhythm and style by one, resulting in the player winning.
            tempPlayerRhythm += 1;
            tempPlayerStyle += 1;

            /*Debug.Log(tempPlayerRhythm + " tpr");
            Debug.Log(tempPlayerStyle + " tps");*/
        }
        // else if the dice roll is greater than 3...
        else if (d6 > 3)
        {
            // Increase the NPC's temporary rhythm and style by one, resulting in the NPC winning.
            tempNpcRhytm += 1;
            tempNpcStyle += 1;

            /*Debug.Log(tempNpcRhytm + " tnr");
            Debug.Log(tempNpcStyle + " tns");*/
        }
        else
        {
            // It's a draw :)
            tempPlayerRhythm += Random.Range(0, 2);
            tempPlayerStyle += Random.Range(0, 2);
            tempNpcRhytm += Random.Range(0, 2);
            tempNpcStyle += Random.Range(0, 2);

            /*Debug.Log(tempPlayerRhythm + " tpr");
            Debug.Log(tempPlayerStyle + " tps");
            Debug.Log(tempNpcRhytm + " tnr");
            Debug.Log(tempNpcStyle + " tns ");*/
        }

        // Your code between here

        // Set outcome to 1 if the player won
        if (tempPlayerRhythm > tempNpcRhytm || tempPlayerStyle > tempNpcStyle && lhs.luck > rhs.luck)
        {
            outcome = 1;
            winner = lhs;
            defeated = rhs;
            Debug.Log("lhs");
        }
        // Set outcome to -1 if the player lost
        else if (tempPlayerRhythm < tempNpcRhytm && tempPlayerStyle < tempNpcStyle && rhs.luck > lhs.luck) 
        {
            outcome = 0;
            winner = rhs;
            defeated = lhs;
            Debug.Log("rhs");
        }

        //else if (tempPlayerRhythm == tempNpcRhytm && lhs.luck == rhs.luck && tempPlayerStyle == tempNpcStyle)
            //outcome = 0;


        // I do not like this setup and I want to change how I determine a winner.

        //defaulting to draw 
        Debug.LogWarning("Attack called, needs to use character stats to determine winner with win strength from 1 to -1. This can most likely be ported from previous brief work.");

        Debug.LogWarning("Attack called, may want to use the BattleLog to report the dancers and the outcome of their dance off.");

        var results = new FightResultData(winner, defeated, outcome);

        lhs.isSelected = false;
        rhs.isSelected = false;
        GameEvents.FightCompleted(results);

        yield return null;
    }
}
