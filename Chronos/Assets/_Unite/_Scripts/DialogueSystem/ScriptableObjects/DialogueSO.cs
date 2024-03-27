﻿using System.Collections.Generic;
using UnityEngine;

namespace Unite.DialogueSystem
{
    [CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue System/Dialogue SO")]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField]
        private List<DialogueLine> lines;

        [SerializeField] private bool isQueued;

        public List<DialogueLine> Lines => lines;
        public bool IsQueued => isQueued;
    }
}