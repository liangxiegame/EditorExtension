using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorExtension
{
    [AddComponentMenu("EditorExtension/Bind")]
    public class Bind : MonoBehaviour
    {
        public string ComponentName
        {
            get
            {
                if (GetComponent<MeshRenderer>())
                {
                    return "MeshRenderer";
                }
                else if (GetComponent<SpriteRenderer>())
                {
                    return "SpriteRenderer";
                }
                
                return "Transform";
            }
        }
    }
}