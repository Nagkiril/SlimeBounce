using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.UI.Windows.ShopComponents;
using SlimeBounce.UI.Windows.ShopComponents.Tabs.Content;
using SlimeBounce.Settings;
using SlimeBounce.Player;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs
{
    public class PassivesTab : ContentTab
    {
        [SerializeField] PassiveCard cardPrefab;

        Dictionary<PassiveCard, UpgradeType> _cards;

        protected override void ShowTab()
        {
            base.ShowTab();

            CreateCards();

        }


        private void CreateCards()
        {
            if (_cards == null)
            {
                _cards = new Dictionary<PassiveCard, UpgradeType>();

                foreach (var viewData in UpgradeViewSettings.GetViewCategory(UpgradeViewCategory.Passive))
                {
                    var newCard = Instantiate(cardPrefab);
                    container.RegisterContent(newCard);
                    _cards.Add(newCard, viewData.Key);
                    newCard.SetCardView(viewData.Value.Name, viewData.Value.Icon, viewData.Value.Description, UpgradeController.GetMaxUpgradeLevel(viewData.Key));
                    newCard.OnUpgradeCommand += OnCardUpgraded;
                    UpdateCardProgress(newCard);
                }
            }
        }

        void OnCardUpgraded(PassiveCard target)
        {
            if (UpgradeController.PerformUpgrade(_cards[target]))
            {
                UpdateCardProgress(target);
            }
        }

        void UpdateCardProgress(PassiveCard target)
        {
            var currentValue = UpgradeController.GetUpgradeValue(_cards[target]);
            var deltaValue = UpgradeController.GetUpgradeValue(_cards[target], true) - currentValue;
            target.SetCardProgress(currentValue, deltaValue, UpgradeController.GetUpgradeCost(_cards[target]), UpgradeController.GetCurrentUpgradeLevel(_cards[target]));
        }
    }
}