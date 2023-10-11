using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StringTypeService.Optional
{
    public class Examples : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] TMP_Text textField;
        private StringTypeService typeService;
        private void Start()
        {
            typeService = new StringTypeService(this);
            // Default parameters:
            typeService.InteractionKey = KeyCode.E;
            typeService.BlinkChar = '•';
            typeService.SymbolDelay = 0.1f;
            typeService.PauseDelay = 0.2f;
            typeService.BlinkDelay = 0.5f;
            typeService.SlowChars = new char[] { ' ', ',', '.', '?', '!', ';' };

            StartCoroutine(ExampleMethod());
        }
        private IEnumerator ExampleMethod()
        {
            yield return typeService.TypeString("Example string.", textField);
            yield return new WaitForSeconds(3);
            yield return typeService.TypeString("Typing can be skipped by pressing interaction key (defalut 'E')", textField);
            yield return new WaitForSeconds(3);
            typeService.TypeStringSequence(new List<string>() { "String sequence. Press interaction key (default 'E') " +
                                                                "to skip typing and/or type next string in sequence."
                                                                , "Blinking char can be changed or disabled."
                                                                , "Last string." }, textField, true);
            
        }
    }
}