using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening; // Import DOTween for smooth animations

public class LinearSearchVisualizer : MonoBehaviour
{
    public GameObject barPrefab;
    public Transform container;
    public int[] data;

    private List<GameObject> visualBars = new List<GameObject>();

    void Start()
    {
        GenerateArray(new int[] { 10, 25, 5, 40, 15, 30 }); // Example data
    }

    public void GenerateArray(int[] newData)
    {
        // Clear old bars
        foreach (var bar in visualBars) Destroy(bar);
        visualBars.Clear();

        // Create new bars
        foreach (int val in newData)
        {
            GameObject newBar = Instantiate(barPrefab, container);
            newBar.GetComponentInChildren<TMP_Text>().text = val.ToString();

            // Set height based on value (optional)
            RectTransform rt = newBar.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, val * 5);

            visualBars.Add(newBar);
        }
    }

    public void StartLinearSearch(int target)
    {
        StartCoroutine(LinearSearchCoroutine(target));
    }

    IEnumerator LinearSearchCoroutine(int target)
    {
        for (int i = 0; i < visualBars.Count; i++)
        {
            Image barImage = visualBars[i].GetComponent<Image>();

            // 1. Highlight current bar (Yellow)
            barImage.DOColor(Color.yellow, 0.3f);
            visualBars[i].transform.DOScale(1.2f, 0.2f); // Slight "pop" effect

            yield return new WaitForSeconds(0.5f); // Wait for viewer to see

            int val = int.Parse(visualBars[i].GetComponentInChildren<TMP_Text>().text);

            if (val == target)
            {
                // 2. Found it! (Green)
                barImage.DOColor(Color.green, 0.3f);
                yield break;
            }
            else
            {
                // 3. Not this one (Red/Back to Gray)
                barImage.DOColor(Color.red, 0.2f);
                visualBars[i].transform.DOScale(1.0f, 0.2f);
                yield return new WaitForSeconds(0.2f);
                barImage.DOColor(Color.white, 0.3f);
            }
        }
    }
}