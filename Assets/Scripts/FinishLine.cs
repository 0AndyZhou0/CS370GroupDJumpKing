using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{

	public Font titlesFont;
	//public Font leadersFont;
	string playerName = "";
	float time = 0.0f;
	bool finished;

	public GameObject player;
	public GameObject timer;

	enum gameState
	{
		waiting,
		running,
		enterscore,
		leaderboard
	};

	gameState gs;


	// Reference to the dreamloLeaderboard prefab in the scene
	dreamloLeaderBoard dl;

	// Start is called before the first frame update
	void Start()
	{
		// get the reference here...
		this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		this.gs = gameState.waiting;
		finished = false;
	}

	void OnTriggerEnter2D(Collider2D finish)
	{
		Time.timeScale = 0f;
		this.gs = gameState.enterscore;
		finished = true;
	}
	void Update()
	{
		time = PlayerPrefs.GetFloat("timer", time);
	}
		void OnGUI()
	{
		if (finished)
		{
			GUI.skin.font = titlesFont;
			
			var width200 = new GUILayoutOption[] { GUILayout.Width(350) };
			var width = 500;  // Make this smaller to add more columns
			var height = 200;

			var r = new Rect(0, 0, Screen.width - width, Screen.height - height);
			r.center = new Vector2(Screen.width / 2, Screen.height / 2);
			GUILayout.BeginArea(r, new GUIStyle("box"));
			GUILayout.BeginVertical();

			
			
			//GUILayout.Label("Total Time: " + this.time.ToString("f1"));

			if (this.gs == gameState.enterscore)
			{
				GUILayout.Space(175);
				GUILayout.Label("Your time to reach the top: " + this.time.ToString("f1"));
				GUILayout.Space(20);
				GUILayout.BeginHorizontal();
				GUILayout.Label("Your Name: ");
				this.playerName = GUILayout.TextField(this.playerName, width200);

				if (GUILayout.Button("Save Score"))
				{
					// add the score...
					dl.AddScore(this.playerName, (int)time * -1, (int)time);
					this.gs = gameState.leaderboard;
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(50);
				GUILayout.Label("If your name has been previously entered only the shortest time will be saved.");
			}

			if (this.gs == gameState.leaderboard)
			{
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", width200);
				GUILayout.Label("JUMPMASTER LEADERBOARD", width200);
				GUILayout.EndHorizontal();
				GUI.skin.font = titlesFont;
				GUILayout.BeginHorizontal();
				GUILayout.Label("Name", width200);
				GUILayout.Label("Time", width200);
				GUILayout.Label("Date", width200);
				GUILayout.EndHorizontal();
				List<dreamloLeaderBoard.Score> scoreList = dl.ToListLowToHigh();

				if (scoreList == null)
				{
					GUILayout.Label("(loading...)");
				}
				else
				{
					int maxToDisplay = 20;
					int count = 0;
					foreach (dreamloLeaderBoard.Score currentScore in scoreList)
					{
						count++;
						GUILayout.BeginHorizontal();
						//GUI.skin.font = leadersFont;
						GUILayout.Label(count + " " + currentScore.playerName, width200);
						GUILayout.Label((currentScore.seconds/60).ToString() + " Min : " + (currentScore.seconds % 60).ToString() + " Sec", width200);
						GUILayout.Label(currentScore.dateString, width200);
						GUILayout.EndHorizontal();

						if (count >= maxToDisplay) break;
					}
				}
				GUILayout.Space(30);

				if (GUILayout.Button("Done"))
				{
					player.SetActive(false);
					timer.SetActive(false);
					PlayerPrefs.DeleteAll();
					PlayerPrefs.Save();
					SceneManager.LoadScene("TitleScreenBasic");
				}
			}
			GUILayout.EndVertical();


			r.y = r.y + r.height + 20;

			GUILayout.EndArea();
		}
	}

}



