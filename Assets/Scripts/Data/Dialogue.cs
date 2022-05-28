using System.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea]
    public string text;
    public AudioClip textSound;
    [Range(1f,10f)]
    public float textSpeed = 5f;
    public Color dialogueColor = Color.white;
}