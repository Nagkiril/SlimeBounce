using SlimeBounce.Player;
using SlimeBounce.Player.Settings;
using SlimeBounce.Settings;
using SlimeBounce.UI.Windows.ShopComponents.Tabs.Content;
using SlimeBounce.UI.Settings;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace SlimeBounce.UI.Windows.ShopComponents.Tabs
{
    public class UpgradeCardTab : ContentTab
    {
        [SerializeField] private UpgradeCard _cardPrefab;
        [SerializeField] private UpgradeViewCategory _targetCategory;
        private Dictionary<UpgradeCard, UpgradeType> _cards;

        [Inject]
        private IUpgradeViewSettings _viewSettings;
        [Inject]
        private IUpgradeDataProvider _upgradeData;
        [Inject]
        private IUpgradeActor _upgradeActor;

        private void CreateCards()
        {
            if (_cards == null)
            {
                _cards = new Dictionary<UpgradeCard, UpgradeType>();

                foreach (var viewData in _viewSettings.GetViewCategory(_targetCategory))
                {
                    var newCard = Instantiate(_cardPrefab);
                    _container.RegisterContent(newCard);
                    _cards.Add(newCard, viewData.Key);
                    newCard.SetCardView(viewData.Value.Name, viewData.Value.Icon, viewData.Value.Description, _upgradeData.GetMaxUpgradeLevel(viewData.Key));
                    newCard.OnUpgradeCommand += OnCardUpgraded;
                    UpdateCardProgress(newCard);
                }
            }
        }

        private void OnCardUpgraded(UpgradeCard target)
        {
            if (_upgradeActor.PerformUpgrade(_cards[target]))
            {
                UpdateCardProgress(target);
            }
        }

        private void UpdateCardProgress(UpgradeCard target)
        {
            var currentValues = _upgradeData.GetUpgradeValues(_cards[target]);
            var nextValues = _upgradeData.GetUpgradeValues(_cards[target], true);
            List<float> descriptionValues = new List<float>();
            for (var i = 0; i < currentValues.Count; i++)
            {
                descriptionValues.Add(currentValues[i]);
                descriptionValues.Add(nextValues[i] - currentValues[i]);
            }
            target.SetCardProgress(descriptionValues, _upgradeData.GetUpgradeCost(_cards[target]), _upgradeData.GetCurrentUpgradeLevel(_cards[target]));
        }

        public override void Prepare()
        {
            CreateCards();
        }
    }
}