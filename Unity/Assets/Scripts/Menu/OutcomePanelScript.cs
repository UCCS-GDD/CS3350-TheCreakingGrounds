using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class OutcomePanelScript : MonoBehaviour
    {
        public Text outcomeText;

        public void ShowOutcome(string outcome)
        {
            Player.Instance.ShowMouse();
            Player.Instance.isInMenu = true;
            gameObject.SetActive(true);
            outcomeText.text = outcome;
        }
    }
}
