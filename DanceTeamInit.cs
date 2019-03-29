﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This class generates and assigns names to the 2 dance teams in our dance off battle.
/// It also controls the number of dancers on each team via the inspector
/// It also uses the name generator to pass character names to the teams so they can initialise
/// 
/// TODO:
///     Generate unique team names for both teams and assign them via team_.SetTroupeName(str);
///     Use the nameGenerator to get enough names for the number of dancers on both teams and pass the required names via array to each team for init (InitaliseTeamFromNames)
/// </summary>
public class DanceTeamInit : MonoBehaviour
{
    public DanceTeam teamA, teamB;

    public GameObject dancerPrefab;
    public int dancersPerSide = 3;
    public CharacterNameGenerator nameGenerator;

    private void OnEnable()
    {
        GameEvents.OnBattleInitialise += InitTeams;
    }
    private void OnDisable()
    {
        GameEvents.OnBattleInitialise -= InitTeams;
    }

    void InitTeams()
    {

        // - Set the roupe names for TeamA and TeamB
        teamA.SetTroupeName("Bert"); // Can make this whatever I want.
        teamB.SetTroupeName("Ernie"); // Can make this whatever I want.

        // - Get list of names from name generator
        // - Use he variable nameGenerator to generate names
        // - nameGenerator has a GenerateNames function
        var namesForA = nameGenerator.GenerateNames(dancersPerSide);
        var namesForB = nameGenerator.GenerateNames(dancersPerSide);

        // - Initialise the team names for team A and B
        teamA.InitaliseTeamFromNames(dancerPrefab, 1, namesForA);
        teamB.InitaliseTeamFromNames(dancerPrefab, -1, namesForB);

        Debug.LogWarning("InitTeams called, needs to generate names for the teams and set them with teamA.SetTroupeName");

        Debug.LogWarning("InitTeams called, needs to create character names via CharacterNameGenerator and get them into the team.InitaliseTeamFromNames");

    }
}
