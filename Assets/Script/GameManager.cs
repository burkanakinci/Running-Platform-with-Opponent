using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform rotatingPlatform;
    [SerializeField] Transform rotater;
    public List<GameObject> character;
    public GameObject boy;
    public bool rotaterIsRot;
    [SerializeField] TextMeshProUGUI sorting;
    void Start()
    {
        rotaterIsRot = true;
    }
    void Update()
    {
        rotatingPlatform.Rotate(0f, 0f, 45f * Time.deltaTime, Space.World);

        SortCharacter();
        sorting.text = $"{SortCharacter()}/{character.Count}";

        if (rotaterIsRot)
            rotater.Rotate(0f, 60f * Time.fixedDeltaTime, 0f, Space.World);
    }

    private int SortCharacter()
    {
        int indexBoy = 11;

        for (int i = 0; i < character.Count; i++)
        {
            if (character[i].transform.position.z <=boy.transform.position.z)
            {
                indexBoy--;
            }
        }
        return indexBoy;
    }
}
