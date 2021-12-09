using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;
    private static List<GameObject> weapons;
    private static List<GameObject> hidingSpots;
    private static List<GameObject> enemiesChasingPlayer;

    static GWorld()
    {
        world = new WorldStates();
        weapons = new List<GameObject>();
        hidingSpots = new List<GameObject>();
        enemiesChasingPlayer = new List<GameObject>();
    }

    private GWorld()
    {

    }

    public List<GameObject> GetAllHidingSpots()
    {
        if (hidingSpots.Count < 1)
        {
            return null;
        }
        return hidingSpots;
    }

    public void RemoveSpots(GameObject spot)
    {
        if (hidingSpots.Count < 1)
        {
            return;
        }
        hidingSpots.Remove(spot);
    }

    public void AddHidingSpots(GameObject hidingSpot)
    {
        hidingSpots.Add(hidingSpot);
    }

    public List<GameObject> GetAllWeapons()
    {
        if (weapons.Count < 1)
        {
            return null;
        }
        return weapons;
    }

    public void RemoveWeapon(GameObject weapon)
    {
        if (weapons.Count < 1)
        {
            return;
        }
        weapons.Remove(weapon);
        world.ModifyState("AvailableWeapons", -1);
    }

    public void AddWeapon(GameObject weapon)
    {
        weapons.Add(weapon);
        world.ModifyState("AvailableWeapons", 1);
    }

    public List<GameObject> GetChasingEnemy()
    {
        if (enemiesChasingPlayer.Count < 1)
        {
            return null;
        }
        return enemiesChasingPlayer;
    }

    public void RemoveChasingEnemy(GameObject enemy)
    {
        if (enemiesChasingPlayer.Count < 1)
        {
            return;
        }
        enemiesChasingPlayer.Remove(enemy);
    }

    public void AddChasingEnemy(GameObject enemy)
    {
        enemiesChasingPlayer.Add(enemy);
    }

    public static GWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
