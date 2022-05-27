using System.Collections;
using UnityEngine;
namespace Assets.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "Patient",menuName = "Patient")]
    public class Patient : ScriptableObject
    {

        public float height;
        public float[] heightRanges = new float[4];

        public float depth;
        public float[] depthRanges = new float[2];
    }
}