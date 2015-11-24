using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Menu;

namespace Assets.Scripts
{
    public class PerkListController : MonoBehaviour
    {
        public Button buttonPrefab;
        List<Perk> perks;
        GridLayoutGroup buttonHolder;
        CharacterCreate menuController;
        Text UIDescription;
        Text UIName;
        Image UIIcon;

        public void Start()
        {
            GameObject parentPanel = transform.parent.gameObject;
            buttonHolder = transform.FindChild("Viewport").FindChild("Content").FindChild("ButtonHolder").GetComponent<GridLayoutGroup>();
            menuController = transform.parent.parent.GetComponent<CharacterCreate>();
            UIIcon = parentPanel.transform.FindChild("CurrentPerk").GetComponent<Image>();
            UIName = UIIcon.transform.GetChild(0).GetComponentInChildren<Text>();
            UIDescription = parentPanel.transform.FindChild("DescriptionPanel").GetChild(0).GetComponent<Text>();

            perks = Resources.LoadAll<Perk>("Data/Perks").Where(p => p.IsStartPerk).ToList();

            foreach (Perk perk in perks.OrderBy(p => p.Name))
            {
                Button button = GameObject.Instantiate<Button>(buttonPrefab);
                if (perk.Icon != null)
                    button.GetComponent<Image>().sprite = perk.Icon;
                else
                    button.GetComponentInChildren<Text>().text = perk.Name;
                button.transform.SetParent(buttonHolder.transform, false);

                Perk temp = perk;
                button.onClick.AddListener(() => { PerkClicked(temp); });
            }
        }

        public void PerkClicked(Perk perk)
        {
            if (menuController.CurrentPerk == perk)
                return;

            UIDescription.text = perk.Description;
            UIName.text = perk.Name;
            UIIcon.sprite = perk.Icon;

            menuController.Player.Perks = new List<Perk>() { perk };

            if (menuController.CurrentPerk != null)
                menuController.CurrentPerk.OnRemove(menuController.Player);

            menuController.CurrentPerk = perk;

            perk.OnAdd(menuController.Player);

            menuController.UpdateDisplay();
        }
    }
}
