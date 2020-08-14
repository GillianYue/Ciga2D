using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// Main class for implementing NPC game objects.
    /// </summary>
    public class NPCController : MonoBehaviour
    {
        public ConversationScript[] conversations;

        GameModel model = Schedule.GetModel<GameModel>();

        void OnEnable()
        {
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            var c = GetConversation();
            if (c != null)
            {
                var ev = Schedule.Add<Events.ShowConversation>();
                ev.conversation = c;
                ev.npc = this;
                ev.gameObject = gameObject;
                ev.conversationItemKey = "";
            }
        }


        ConversationScript GetConversation()
        {
            
            return null;
        }
    }
}