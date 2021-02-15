using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEngine.Networking;
public class DialogueSystem : MonoBehaviour
{
    //will hold all "narrative events" that will happen in the game
    private List<DialogueLine> allDialogueEvents;

    //just an auxiliary array to support file reading across platforms (sync or async).
    //we just have it here because if we read the file asynchronously we need it to last through the whole class'
    //scope
    private string[] allLinesFromFile;

    //holds the final portion of path to the file we'll be reading - set now at the Inspector
    public string filepath;
    
    
    void Awake()
    {

        //uses a preprocessor to check whether the file will be read synchronously (local) or async (web)
        #if UNITY_WEBGL
            StartCoroutine("ReadFileAsync");            
        #else
            ReadExternalFile();
        #endif
    }


    //this coroutine is used to read the file asynchronously (when in a WebGL environment)
    IEnumerator ReadFileAsync()
    {
        UnityWebRequest www = UnityWebRequest.Get(Application.dataPath + filepath);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //read results as text
            allLinesFromFile = www.downloadHandler.text.Split("\n".ToCharArray());               
        }
        ReadExternalFile();
    }
     

    /// <summary>
    /// retrieves a narrative event object using its id in the file
    /// </summary>
    /// <param name="id">the id of the event you want to retrieve in the text file</param>
    /// <returns>the whole "narrative event" object as a DialogueLine</returns>
    public DialogueLine GetEvent(int id)
    {
        // more readable, simple loop
        foreach (DialogueLine line in allDialogueEvents)
        {
            if (line.id == id)
            {
                return line;
            }
        }

        return null;
        
        //this one also works using a predicate and List.Find (the result will be the same in the end)
        //return allDialogueEvents.Find(x => x.id == id);

    }


    /// <summary>
    /// reads the external file and populates the list of narrative events
    /// </summary>
    void ReadExternalFile()
    {
        allDialogueEvents = new List<DialogueLine>();



        #if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            StreamReader r = new StreamReader(Application.dataPath + filepath);

        // "\n" is the same as an "Enter"
        //this command "breaks" the whole textfile, now saved at string "r" into an array of strings separated by enters
        //this means allLinesFromFile[0] will hold the first line, allLinesFromFile[1] will hold the second...
        allLinesFromFile = r.ReadToEnd().Split("\n".ToCharArray());
        #endif

        //for each line we read from the file, we create a new DialogueLine object (the "narrative event") and record
        //the relevant data

        //we start from item 1 as the first line (0) is just a header, so we ignore it.
        for (int i = 1; i < allLinesFromFile.Length; i++)
        {
            // "\t" is the same as a tab
            //this command "breaks" a line, now saved at allLinesFromFile[index] into an array of strings separated by tab
            //this means dialogueLineFromFile[0] will hold the first column of that line, dialogueLineFromFile[1] will hold the second column
            string[] dialogueLineFromFile = allLinesFromFile[i].Split("\t".ToCharArray());

            
            //create new DialogueLine object and stores the data from file
            DialogueLine newLine = ScriptableObject.CreateInstance<DialogueLine>();

            //all data is originally a string, so numbers must be parsed into the correct type
            newLine.id = int.Parse(dialogueLineFromFile[0]);            
            
            newLine.speaker = dialogueLineFromFile[1];            

            newLine.text = dialogueLineFromFile[2];            

            newLine.nextEvent = int.Parse(dialogueLineFromFile[3]);            

            //this is a boolean and what is saved is whether that line is the same as "Y". If so, we have a true
            //otherwise, it is false
            newLine.lastEventInSequence = (dialogueLineFromFile[4] == "Y");            

            //this will be the name of the file asset
            newLine.imgAsset = dialogueLineFromFile[5];            


            //same as above - name of the asset

            //TrimEnd removes any character listed within the brackets from the end of the text
            //this ensures that there is no remaining "Enter" or "Tab" in the file name as read from the text
            // - if there is, we won't be able to find the audiofile in the folder...
            newLine.audioAsset = dialogueLineFromFile[6].TrimEnd('\r', '\n', '\t');            


            //after the object is created and all data is stored, it is added to the list of "narrative events".

            allDialogueEvents.Add(newLine);
        }

    }


    


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
