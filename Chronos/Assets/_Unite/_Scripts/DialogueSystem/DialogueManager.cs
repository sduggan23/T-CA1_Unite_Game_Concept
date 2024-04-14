﻿using System.Collections;
using System.Collections.Generic;
using Unite.EventSystem;
using UnityEngine;

namespace Unite.DialogueSystem
{
    /// <summary>
    /// Responsible for playing dialogue sequences upon receiving requests.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class DialogueManager : MonoBehaviour
    {
        [Header("Set to false for testing purposes:")]
        [SerializeField]
        private bool playDialogue = true;

        [Header("Send event to analytics manager when playing dialogue")]
        [SerializeField]
        private DialogueSOEvent dialogueAnalyticsEvent;

        [Header("Send event to subtitles UI when playing dialogue line")] 
        [SerializeField]
        private DialogueLineEvent onPlayDialogueLine;
        
        public static DialogueManager Instance { get; private set; }

        private AudioSource audioSource;

        private Coroutine dialogueLinesCoroutine;

        private bool isDialoguePlaying;
        
        private Queue<DialogueSO> dialogueQueue = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one DialogueManager present in the scene! Destroying current instance");
                Destroy(this);
            }

            Instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// If a dialogue has to be queued after the current dialogue is done playing,
        /// then it is enqueued and played after the current dialogue is done.
        ///
        /// If it doesn't need to be queued, then it is played immediately.
        /// If there was a dialogue playing already, it stops and the new dialogue is played.
        /// </summary>
        public void PlayDialogue(DialogueSO dialogue)
        {
            if (!playDialogue) return;

            if (isDialoguePlaying && dialogue.IsQueued)
            {
                dialogueQueue.Enqueue(dialogue);
                return;
            }

            if (dialogue.OverrideQueue)
            {
                dialogueQueue.Clear();
            }
            
            dialogueAnalyticsEvent.Raise(dialogue);
            
            if(dialogueLinesCoroutine != null)
                    StopCoroutine(dialogueLinesCoroutine);
            dialogueLinesCoroutine = StartCoroutine(DialogueLinesCoroutine(dialogue.Lines));
        }

        private void PlayDialogueLine(DialogueLine line)
        {
            if (line.Audio != null)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(line.Audio);
            }
            
            onPlayDialogueLine.Raise(line);
        }

        /// <summary>
        /// Plays dialogue lines while adding a delay between them. The delay is specified in the
        /// DialogueLine object.
        ///
        /// <seealso cref="DialogueLine"/>
        /// </summary>
        /// <returns></returns>
        private IEnumerator DialogueLinesCoroutine(List<DialogueLine> lines)
        {
            isDialoguePlaying = true;
            
            for (int i = 0; i < lines.Count; i++)
            {
                PlayDialogueLine(lines[i]);
                yield return new WaitForSeconds(lines[i].NextLineDelayInSeconds);
            }

            isDialoguePlaying = false;
            if (dialogueQueue.Count <= 0) yield break;
            
            DialogueSO nextDialogue = dialogueQueue.Dequeue();
            PlayDialogue(nextDialogue);
        }
    }
}