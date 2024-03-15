using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int currSceneIdx;
    private string leaderBoardKey = "";
    private string memberID = "";
    private float elapsedTime, startTime;
    public int selectedEggIndex;
    [SerializeField] GameObject hud, finishHUD, lvlSelectHUD, errorHud, nextBtn, pauseHUD, pauseButton;
    [SerializeField] TMP_Dropdown eggTypeFinish, eggTypePause;
    [SerializeField] TMP_Text timerText, finishTimeText, eggTypeText, errorText, leaderBoardCentered, leaderBoard10,
                              leaderBoardEgg, leaderBoardEggTitle;
    [SerializeField] TMP_InputField nameInput;
    EventSystem es;
    Coroutine countTime;
    
    void Start()
    {
        es = GetComponentInChildren<EventSystem>();
        DontDestroyOnLoad(gameObject);
        selectedEggIndex = -1;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                errorHud.SetActive(true);
                errorText.text = "Failed to start session : " + response.errorData.message;
                return;
            }
            memberID = response.player_id.ToString();
            LootLockerSDKManager.GetPlayerName((response) =>
            {
                if (response.success)
                {
                    string playerName = response.name;
                    if (playerName.Length > 0) nameInput.text = playerName;
                }
            });
        });
    }

    private void Update()
    {
        if (es.currentSelectedGameObject != null && es.currentSelectedGameObject.name != "NameInput")
        {
            es.SetSelectedGameObject(null);
        }
    }

    void UploadScore(string name, int score, string eggType)
    {
        leaderBoardEggTitle.text = "Egg : " + eggType;
        LootLockerSDKManager.SubmitScore(name, score, leaderBoardKey, eggType, (response) =>
        {
            if (response.statusCode == 200)
            {
                LootLockerSDKManager.SubmitScore(name, score, string.Concat(leaderBoardKey, eggType), (response) =>
                {
                    if (response.success)
                    {
                        UpdateLeaderboardCenteredEgg(eggType);
                    } else
                    {
                        errorHud.SetActive(true);
                        errorText.text = "Failed to upload egg score: " + response.errorData.message;
                    }
                });
                UpdateLeaderboardCentered();
                UpdateLeaderboardTop10();
            }
            else
            {
                errorHud.SetActive(true);
                errorText.text = "Failed to upload score: " + response.errorData.message;
            }
        });
    }

    void UpdateLeaderboardCentered()
    {
        LootLockerSDKManager.GetMemberRank(leaderBoardKey, memberID, (memberResponse) =>
        {
            if (memberResponse.success)
            {
                if (memberResponse.rank == 0)
                {
                    leaderBoardCentered.text = "Upload score to see centered";
                    return;
                }
                int playerRank = memberResponse.rank;
                int count = 5;
                /*
                 * Set "after" to 2 below and 2 above the rank for the current player.
                 * "after" means where to start fetch the leaderboard entries.
                 */
                int after = playerRank < 4 ? 0 : playerRank - 2;

                LootLockerSDKManager.GetScoreList(leaderBoardKey, count, after, (scoreResponse) =>
                {
                    if (scoreResponse.success)
                    {
                        /*
                         * Format the leaderboard
                         */
                        string leaderboardText = "";
                        for (int i = 0; i < scoreResponse.items.Length; i++)
                        {
                            LootLockerLeaderboardMember currentEntry = scoreResponse.items[i];

                            /*
                             * Highlight the player with rich text
                             */
                            if (currentEntry.rank == playerRank)
                            {
                                leaderboardText += "<color=yellow>";
                            }

                            leaderboardText += currentEntry.rank + ". ";
                            leaderboardText += currentEntry.player.name;
                            leaderboardText += " - ";
                            leaderboardText += currentEntry.score / 10000f + " s";
                            leaderboardText += " - ";
                            leaderboardText += currentEntry.metadata;

                            /*
                            * End highlighting the player
                            */
                            if (currentEntry.rank == playerRank)
                            {
                                leaderboardText += "</color>";
                            }
                            leaderboardText += "\n";
                        }
                        leaderBoardCentered.text = leaderboardText;
                    }
                    else
                    {
                        errorHud.SetActive(true);
                        errorText.text = "Could not update centered scores:" + scoreResponse.errorData.message;
                    }
                });
            }
            else
            {
                errorHud.SetActive(true);
                errorText.text = "Could not get member rank:" + memberResponse.errorData.message;
            }
        });
    }

    void UpdateLeaderboardCenteredEgg(string eggType)
    {
        LootLockerSDKManager.GetMemberRank(string.Concat(leaderBoardKey, eggType), memberID, (memberResponse) =>
        {
            if (memberResponse.success)
            {
                if (memberResponse.rank == 0)
                {
                    leaderBoardEgg.text = "Upload score to see centered";
                    return;
                }
                int playerRank = memberResponse.rank;
                int count = 5;
                /*
                 * Set "after" to 2 below and 2 above the rank for the current player.
                 * "after" means where to start fetch the leaderboard entries.
                 */
                int after = playerRank < 4 ? 0 : playerRank - 2;

                LootLockerSDKManager.GetScoreList(string.Concat(leaderBoardKey, eggType), count, after, (scoreResponse) =>
                {
                    if (scoreResponse.success)
                    {
                        /*
                         * Format the leaderboard
                         */
                        string leaderboardText = "";
                        for (int i = 0; i < scoreResponse.items.Length; i++)
                        {
                            LootLockerLeaderboardMember currentEntry = scoreResponse.items[i];

                            /*
                             * Highlight the player with rich text
                             */
                            if (currentEntry.rank == playerRank)
                            {
                                leaderboardText += "<color=yellow>";
                            }

                            leaderboardText += currentEntry.rank + ". ";
                            leaderboardText += currentEntry.player.name;
                            leaderboardText += " - ";
                            leaderboardText += currentEntry.score / 10000f + " s";

                            /*
                            * End highlighting the player
                            */
                            if (currentEntry.rank == playerRank)
                            {
                                leaderboardText += "</color>";
                            }
                            leaderboardText += "\n";
                        }
                        leaderBoardEgg.text = leaderboardText;
                    }
                    else
                    {
                        errorHud.SetActive(true);
                        errorText.text = "Could not update centered egg scores:" + scoreResponse.errorData.message;
                    }
                });
            }
            else
            {
                errorHud.SetActive(true);
                errorText.text = "Could not get egg member rank:" + memberResponse.errorData.message;
            }
        });
    }

    void UpdateLeaderboardTop10()
    {
        LootLockerSDKManager.GetScoreList(leaderBoardKey, 10, (response) =>
        {
            if (response.success)
            {
                /*
                 * Format the leaderboard
                 */
                string leaderboardText = "";
                for (int i = 0; i < response.items.Length; i++)
                {
                    LootLockerLeaderboardMember currentEntry = response.items[i];
                    leaderboardText += currentEntry.rank + ".";
                    leaderboardText += currentEntry.player.name;
                    leaderboardText += " - ";
                    leaderboardText += currentEntry.score / 10000f + " s";
                    leaderboardText += " - ";
                    leaderboardText += currentEntry.metadata;
                    leaderboardText += "\n";
                }
                leaderBoard10.text = leaderboardText;
            }
            else
            {
                errorHud.SetActive(true);
                errorText.text = "Error updating Top 10 leaderboard";
            }
        });
    }

    private void Awake()
    {
        countTime = null;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += StartLevel;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= StartLevel;
    }

    public void ChangeName(string newName)
    {
        LootLockerSDKManager.SetPlayerName(newName, (response) =>
        {
            if (response.success)
            {
            }
            else
            {
                errorHud.SetActive(true);
                errorText.text = "Error setting player name";
            }
        });
    }

    public void LevelFinish(string eggType)
    {
        StopCoroutine(countTime);
        string type = eggType.Replace("(Clone)", "");
        int currTime = Mathf.RoundToInt(elapsedTime * 10000f);
        UploadScore(memberID, currTime, type);
        finishHUD.SetActive(true);
        finishTimeText.text = "Time : " + (currTime / 10000f).ToString()  + " s";
        eggTypeText.text = "Egg : " + type;
        if (currSceneIdx == SceneManager.sceneCountInBuildSettings -1)
        {
            nextBtn.SetActive(false);
        } else
        {
            nextBtn.SetActive(true);
        }
        hud.SetActive(false);
    }

    public void ReduceElapsedTime(float t, string eggType)
    {
        elapsedTime -= t;
        string type = eggType.Replace("(Clone)", "");
        int currTime = Mathf.RoundToInt(elapsedTime * 10000f);
        finishTimeText.text = "Time : " + (currTime / 10000f).ToString() + " s";
        UploadScore(memberID, currTime, type);
    }

    IEnumerator CountTimer()
    {
        while (true)
        {
            elapsedTime = Mathf.Round((Time.time - startTime) * 10000f) / 10000f;
            timerText.text = elapsedTime.ToString();
            yield return null;
        }
    }

    public void NextLevel()
    {
        Resume();
        if (countTime != null) StopCoroutine(countTime);
        SceneManager.LoadScene(currSceneIdx + 1);
    }

    public void RestartLevel()
    {
        Resume();
        if (countTime != null) StopCoroutine(countTime);
        SceneManager.LoadScene(currSceneIdx);
    }

    private void StartLevel(Scene scene, LoadSceneMode mode)
    {
        pauseButton.SetActive(true);
        finishHUD.SetActive(false);
        lvlSelectHUD.SetActive(false);
        errorHud.SetActive(false);
        pauseHUD.SetActive(false);
        currSceneIdx = scene.buildIndex;
        if (currSceneIdx > 0)
        {
            hud.SetActive(true);
            leaderBoardKey = string.Concat("EgliolioLevel", currSceneIdx.ToString());
            // Start counting time
            startTime = Time.time;
            countTime = StartCoroutine(CountTimer());
        }
    }

    public void SelectLevel()
    {
        hud.SetActive(false);
        finishHUD.SetActive(false);
        errorHud.SetActive(false);
        pauseHUD.SetActive(false);
        lvlSelectHUD.SetActive(true);
    }

    public void LoadLevel(int level)
    {
        Resume();
        if (countTime != null) StopCoroutine(countTime);
        SceneManager.LoadScene(level);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void ChangeEgg(TMP_Dropdown dropdown)
    {
        eggTypeFinish.value = dropdown.value;
        eggTypePause.value = dropdown.value;
        selectedEggIndex = dropdown.value - 1;
    }
}
