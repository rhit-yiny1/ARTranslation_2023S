using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class textScriptV1 : MonoBehaviour
{
    public GameObject textPrefab;
    public Transform canvasTransform;

    private List<GameObject> createdTextObjects = new List<GameObject>();

    public void CreateTextOnCanvas(string textToDisplay, float x, float y)
    {
        // Create a new Text GameObject using the textPrefab
        GameObject newTextObject = Instantiate(textPrefab, canvasTransform);

        // Get the Text component from the new GameObject
        Text newTextComponent = newTextObject.GetComponent<Text>();

        // Set the text to display on the Text component
        newTextComponent.text = textToDisplay;

        // Set the position of the Text GameObject on the canvas
        RectTransform rectTransform = newTextObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, y);

        // Add the newly created Text GameObject to the list
        createdTextObjects.Add(newTextObject);
    }

    public void ClearAllTextOnCanvas()
    {
        // Destroy all the dynamically created Text GameObjects
        foreach (GameObject textObject in createdTextObjects)
        {
            Destroy(textObject);
        }

        // Clear the list of created Text GameObjects
        createdTextObjects.Clear();
    }
}



