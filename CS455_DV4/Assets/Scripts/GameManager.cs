using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 1f;
    public GameObject completeLevelUI;
    public Text goalText;
    public GOBMovement gobm;

    int qsize = 25;
    Queue myLogQueue = new Queue();

    public void CompleteLevel()
    {
        completeLevelUI.SetActive( true );
    }

    public void EndGame()
    {
        if( !gameHasEnded ) {
	    gameHasEnded = true;
	    Invoke("Restart", restartDelay);
	}
    }

    void Start(){
        
    }

    void OnEnable() {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();
    }

    void OnGUI() {
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, Screen.height / 2));
        GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
        GUILayout.EndArea();
    }

    void Update(){
        if( gobm.gGoals == null ) return; 

        string txt = "Weights: ";

        foreach( Goal g in gobm.gGoals )
            txt += string.Format(" [{0}:{1}]", g.name, g.value);

        goalText.text = txt;
    }
    
    void Restart ()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }
}
