using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoryController : MonoBehaviour
{

    public enum Chapter { Chapter1, Chapter2, Chapter3 };
    public Chapter chapter;
    private int numRooms;
    private int roomsCleared;

    public MapGenerator mapGen;
    public ResourcePlacer resourcePlacer;

    //ref planet
    //ref map generator
    // have some generate next level function based on num rooms
    // call 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


#if UNITY_EDITOR

    [CustomEditor(typeof(StoryController))]
    public class StoryControllerEditor : Editor
    {


        public override void OnInspectorGUI()
        {

            StoryController story = (StoryController)target;
            base.OnInspectorGUI();
          
            if (GUILayout.Button("New Level"))
            {
                story.mapGen.GenerateNewMap();
                story.resourcePlacer.PlaceNewResources();
            }
        }
    }
#endif
}
