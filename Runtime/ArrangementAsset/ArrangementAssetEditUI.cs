using Landscape2.Runtime.UiCommon;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

namespace Landscape2.Runtime
{
    public class ArrangementAssetEditUI : ISubComponent
    {
        private VisualElement contextButtonGroup;
        private GameObject currentTarget;

        public Action OnClickTransButton { get; set; }
        public Action OnClickRotateButton { get; set; }
        public Action OnClickScaleButton { get; set; }
        public Action OnClickDeleteButton { get; set; }
        public Action OnClickFileButton { get; set; }
        public Action OnClickMovieButton { get; set; }

        public ArrangementAssetEditUI(ArrangementAsset arrangeAsset)
        {
            contextButtonGroup = new UIDocumentFactory().CreateWithUxmlName("ContextButtonGroup");

            var moveButton = contextButtonGroup.Q<RadioButton>("MoveButton");
            moveButton.RegisterCallback<ClickEvent>(e => OnClickTransButton?.Invoke());

            var rotateButton = contextButtonGroup.Q<RadioButton>("RotateButton");
            rotateButton.RegisterCallback<ClickEvent>(e => OnClickRotateButton?.Invoke());

            var scaleButton = contextButtonGroup.Q<RadioButton>("ScaleButton");
            scaleButton.RegisterCallback<ClickEvent>(e => OnClickScaleButton?.Invoke());

            var deleteButton = contextButtonGroup.Q<Button>("ContextButton");
            deleteButton.clicked += () => OnClickDeleteButton?.Invoke();

            var succeedButton = contextButtonGroup.Q<Button>("ActionButton");
            if (succeedButton != null)
            {
                succeedButton.style.display = DisplayStyle.None;
            }

            Show(false);
        }

        public void SetTarget(GameObject target)
        {
            currentTarget = target;
            if (currentTarget != null)
            {
                CalcUIDisplayPosition(currentTarget);
                Show(true);
            }
            else
            {
                Show(false);
            }
        }

        private void CalcUIDisplayPosition(GameObject obj)
        {
            if (obj == null) return;

            var renderer = obj.GetComponentInChildren<Renderer>();
            if (renderer == null) return;

            var bounds = renderer.bounds;
            
            var worldPosition = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
            var screenPos = Camera.main.WorldToScreenPoint(worldPosition);

            var viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
            bool isVisible = viewportPosition.z > 0 && 
                             viewportPosition.x >= 0 && viewportPosition.x <= 1 && 
                             viewportPosition.y >= 0 && viewportPosition.y <= 1;

            if (isVisible)
            {
                float xcenter = 80f / 2f;
                contextButtonGroup.style.translate = new Translate(screenPos.x - xcenter, Screen.height - screenPos.y);
            }
        }

        public void ResetButtons()
        {
            var radioButtons = contextButtonGroup.Query<RadioButton>().ToList();
            foreach (var button in radioButtons)
            {
                button.value = false;
            }
            
            var moveButton = contextButtonGroup.Q<RadioButton>("MoveButton");
            if (moveButton != null)
            {
                moveButton.value = true;
            }
        }

        public void Show(bool visible)
        {
            if (contextButtonGroup != null)
            {
                contextButtonGroup.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public void Start() { }
        public void OnEnable() { }
        public void OnDisable()
        {
            Show(false);
            currentTarget = null;
        }

        public void Update(float deltaTime)
        {
            if (currentTarget != null)
            {
                CalcUIDisplayPosition(currentTarget);
            }
        }

        public void LateUpdate(float deltaTime) { }
    }
}
