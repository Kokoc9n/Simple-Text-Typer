using UnityEngine;

namespace DialogueExtension
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/DialogueScriptableObject", order = 1), System.Serializable]
    public class MessageArray : ScriptableObject
    {
        public Message[] Messages;
        public int currentMessage = 0;
    }
}