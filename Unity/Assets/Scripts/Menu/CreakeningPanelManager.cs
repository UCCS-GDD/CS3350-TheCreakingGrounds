using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class CreakeningPanelManager : MonoBehaviour
    {
        public Text notification;
        public Text briefing;

        public void ShowCreakeningPanel(string playerName, string curseName, string briefingCurse, string briefingNonCurse)
        {
            Player.Instance.ShowMouse();
            Player.Instance.isInMenu = true;
            gameObject.SetActive(true);

            bool wasMe = playerName == Player.Instance.gameObject.name;

            if (wasMe)
                notification.text = "You have awakened " + curseName;
            else
                notification.text = playerName + " has awakened " + curseName;

            if (wasMe)
                briefing.text = briefingCurse;
            else
                briefing.text = briefingNonCurse;
        }

        public void HidePanel()
        {
            gameObject.SetActive(false);
            Player.Instance.HideMouse();
            Player.Instance.isInMenu = false;
        }

        public void ReadyUp()
        {
            Player.Instance.CmdReadyForCreakening();
        }
    }
}
