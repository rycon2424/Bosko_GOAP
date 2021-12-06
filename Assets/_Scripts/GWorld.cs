using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;
    private static Queue<GameObject> patients;
    private static Queue<GameObject> cubicles;
    private static List<GameObject> guns;
    private static List<GameObject> hiding;

    static GWorld()
    {
        world = new WorldStates();
        patients = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();
        guns = new List<GameObject>();
        hiding = new List<GameObject>();
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

    public GameObject RemoveCubicle()
    {
        if (cubicles.Count < 1)
        {
            return null;
        }
        return cubicles.Dequeue();
    }

    public void AddCubicle(GameObject cubi)
    {
        cubicles.Enqueue(cubi);
        world.ModifyState("FreeCubicle", 1);
    }

    public GameObject RemovePatient()
    {
        if (patients.Count < 1)
        {
            return null;
        }
        return patients.Dequeue();
    }

    public void AddPatient(GameObject patient)
    {
        patients.Enqueue(patient);
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
