using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace StringTypeService
{
    public partial class StringTypeService
    {
        public float SymbolDelay { get => symbolDelay; set => symbolDelay = value; }
        public float PauseDelay { get => pauseDelay; set => pauseDelay = value; }
        public float BlinkDelay { get => blinkDelay; set => blinkDelay = value; }
        public KeyCode InteractionKey { get => interactionKey; set => interactionKey = value; }
        public char BlinkChar { get => blinkChar; set => blinkChar = value; }
        public char[] SlowChars { get => slowChars; set => slowChars = value; }

        private ICoroutineRunner coroutineRunner;
        private float symbolDelay = 0.1f;
        private float pauseDelay = 0.2f;
        private float blinkDelay = 0.5f;
        private KeyCode interactionKey = KeyCode.E;
        private char blinkChar = '•';
        private char[] slowChars = new char[] { ' ', ',', '.', '?', '!', ';'};
        private protected bool interacted;
        private protected Coroutine blinkCoroutine;

        public StringTypeService(ICoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }
        private readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();


        private WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
        public Coroutine TypeString(string stringToType, TMP_Text textObject)
        {
            return coroutineRunner.StartCoroutine(TypeStringCoroutine(stringToType, textObject));
        }
        public void TypeStringSequence(List<string> arrayToType, TMP_Text textObject, bool blink = false)
        {
            coroutineRunner.StartCoroutine(TypeStringSequenceCoroutine(arrayToType, textObject, blink));
        }
        private IEnumerator TypeStringCoroutine(string stringToType, TMP_Text textObject)
        {
            interacted = false;
            yield return new WaitForFixedUpdate();
            coroutineRunner.StartCoroutine(WaitForInteract());
            textObject.maxVisibleCharacters = 0;
            textObject.text = stringToType;
            foreach (char c in stringToType)
            {
                textObject.maxVisibleCharacters++;
                if (slowChars.Contains(c))
                    yield return GetWait(pauseDelay);
                else yield return GetWait(symbolDelay);
                if(interacted)
                {
                    textObject.maxVisibleCharacters = stringToType.Length;
                    break;
                }
            }
        }
        private IEnumerator TypeStringSequenceCoroutine(List<string> arrayToType, TMP_Text textObject, bool blink)
        {
            for (int i = 0; i < arrayToType.Count; i++)
            {
                yield return coroutineRunner.StartCoroutine(TypeStringCoroutine(arrayToType[i], textObject));
                if (blink)
                    blinkCoroutine = coroutineRunner.StartCoroutine(BlinkCoroutine(textObject));
                yield return WaitForKeyDown(interactionKey);
                if(blink)
                    coroutineRunner.StopCoroutine(blinkCoroutine);
            }
        }
        private IEnumerator WaitForInteract()
        {
            while(true)
            {
                if(Input.GetKeyDown(interactionKey))
                {
                    interacted = true;
                    break;
                }
                yield return null;
            }
        }
        private IEnumerator WaitForKeyDown(KeyCode keyCode)
        {
            while (!Input.GetKeyDown(keyCode))
                yield return null;
        }

        private IEnumerator BlinkCoroutine(TMP_Text text)
        {
            text.text = string.Format("{0} {1}", text.text, blinkChar);
            text.maxVisibleCharacters++;
            while (true)
            {
                text.maxVisibleCharacters++;
                yield return GetWait(blinkDelay);

                text.maxVisibleCharacters--;
                yield return GetWait(blinkDelay);
            }
        }
    }
}