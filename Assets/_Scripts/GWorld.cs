using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;
    private static List<GameObject> guns;
    private static List<GameObject> hiding;
    private static List<GameObject> enemiesChasingPlayer;

    static GWorld()
    {
        world = new WorldStates();
        guns = new List<GameObject>();
        hiding = new List<GameObject>();
        enemiesChasingPlayer = new List<GameObject>();
    }

    private GWorld()
    {

    }

    public List<GameObject> GetAllHidingSpots()
    {
        if (hiding.Count < 1)
        {
            return null;
        }
        return hiding;
    }

    public void RemoveSpots(GameObject spot)
    {
        if (hiding.Count < 1)
        {
            return;
        }
        hiding.Remove(spot);
    }

    public void AddHidingSpots(GameObject hidingSpot)
    {
        hiding.Add(hidingSpot);
    }

    public List<GameObject> GetAllGuns()
    {
        if (guns.Count < 1)
        {
            return null;
        }
        return guns;
    }

    public void RemoveGun(GameObject gun)
    {
        if (guns.Count < 1)
        {
            return;
        }
        guns.Remove(gun);
        world.ModifyState("AvailableGuns", -1);
    }

    public void AddGuns(GameObject gun)
    {
        guns.Add(gun);
        world.ModifyState("AvailableGuns", 1);
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
