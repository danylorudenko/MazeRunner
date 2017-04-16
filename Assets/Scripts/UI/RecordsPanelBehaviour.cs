using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class RecordsPanelBehaviour : MonoBehaviour
{
    public GameObject recordEntryPrefab;
    public Transform content;

    private List<GameObject> spawnedEntries;

    /// <summary>
    /// Clening up serialized data, closing records panel, opening main menu
    /// </summary>
    public void CloseRecordsWindow()
    {
        ClearContent();
        gameObject.SetActive(false);
        MainMenuManager.Instance.gameObject.SetActive(true);
    }

    /// <summary>
    /// Filling table with xml data
    /// </summary>
    private void OnEnable()
    {
        if (content == null) {
            throw new System.Exception("Records panel didn't find the Content transform");
        }
        FillContent();
    }

    /// <summary>
    /// Filling content of the panel with xml data
    /// </summary>
    private void FillContent()
    {
        ClearContent();

        spawnedEntries = new List<GameObject>();

        List<RecordEntry> recordEntries = XmlManager.ReadAllRecords();
        recordEntries.Sort(RecordEntry.CompareIfBigger);

        int recordsCount = recordEntries.Count;
        if (recordsCount > 0) {
            for (int i = 0; i < recordsCount; i++) {
                GameObject record = Instantiate(recordEntryPrefab, content);
                spawnedEntries.Add(record);

                Text userNameText = record.transform.Find("UserName").GetComponent<Text>();
                userNameText.text = recordEntries[i].userName;

                Text coinCountText = record.transform.Find("CollectedCoins").GetComponent<Text>();
                coinCountText.text = recordEntries[i].collectedCoins.ToString();
            }
        }

    }

    /// <summary>
    /// Cleaning up contet object
    /// </summary>
    private void ClearContent()
    {
        if (spawnedEntries != null) {
            int spawnedEntriesCount = spawnedEntries.Count;
            for (int i = 0; i < spawnedEntriesCount; i++) {
                Destroy(spawnedEntries[i]);
            }
        }
    }
}
