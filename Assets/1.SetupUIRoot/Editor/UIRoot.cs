using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EditorExtension
{
    public class UIRoot : MonoBehaviour
    {
        public Transform Bg;

        public Transform Common;

        public Transform PopUp;

        public Transform Forward;

        [SerializeField] private Canvas mRootCanvas;
    }
}