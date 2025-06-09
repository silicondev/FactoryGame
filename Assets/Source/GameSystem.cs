using Assets.Source.Systems;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static InGameObjectCollection LoadedObjects { get; } = new();
    public static bool Paused { get; set; } = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
