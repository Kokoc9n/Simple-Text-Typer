using UnityEngine;

namespace DialogueExtension
{
    public class Narrator : MonoBehaviour
    {
        [SerializeField] private MessageArray arrayToLoad;
        [HideInInspector] public MessageArray Array;
        public bool MovePanel;
        public bool UseDefaultPosition;
        [HideInInspector] public bool Completed;
        void Start()
        {
            Array = (MessageArray)ScriptableObject.CreateInstance("MessageArray");
            Array.Messages = arrayToLoad.Messages;
            Array.currentMessage = 0;
        }
    }
}