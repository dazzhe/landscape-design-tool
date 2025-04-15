using Landscape2.Runtime.BuildingEditor;
using Landscape2.Runtime.UiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Landscape2.Runtime
{
    public class BuildingCopyListUI : ISubComponent
    {
        public class CopyListElement : IDisposable
        {
            public System.Action<GameObject> OnListClick { get; set; }

            public VisualElement Element { get; private set; }

            GameObject target;

            public CopyListElement(TemplateContainer container, GameObject obj)
            {
                target = obj;

                var listElement = container.Q<VisualElement>("List");

                listElement.RegisterCallback<ClickEvent>(evt =>
                {
                    OnListClick?.Invoke(target);
                });

                var nameLabel = container.Q<Label>("Name");
                nameLabel.text = obj.name;

                Element = container;
            }

            public void Dispose()
            {
                target = null;
            }
        }

        public System.Action<GameObject> OnClickListElement { get; set; }

        VisualElement rootElement;
        VisualElement listRootElement;
        Label emptyLabel;

        VisualTreeAsset listObjectInstance = Resources.Load<VisualTreeAsset>("List_DeleteBuilding");

        CopyListElement ListElementFactory(GameObject obj)
        {
            var listObj = listObjectInstance.CloneTree();
            var elem = new CopyListElement(listObj, obj);

            return elem;
        }

        public BuildingCopyListUI(VisualElement element, BuildingTRSEditor editor)
        {
            rootElement = element.Q<VisualElement>("Panel_CopiedBuilding");
            emptyLabel = rootElement.Q<Label>("Dialogue");

            var listRoot = rootElement.Q<ScrollView>("ScrollView");
            listRootElement = listRoot;

            element.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                if (element.style.display == DisplayStyle.None)
                {
                    OnDisable();
                }
                else if (element.style.display == DisplayStyle.Flex)
                {
                    OnEnable();
                }
            });

            Show(false);
        }

        public void AppendList(GameObject obj)
        {
            if (listRootElement == null)
            {
                Debug.LogWarning($"listRootElementがnullです");
                return;
            }

            var elem = ListElementFactory(obj);

            elem.OnListClick += (go) =>
            {
                OnClickListElement?.Invoke(go);
            };

            listRootElement.Add(elem.Element);
        }

        public void ShowListEmpty(bool show)
        {
            emptyLabel.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void OnDisable()
        {
            Show(false);
        }

        public void OnEnable()
        {
            Show(true);
        }

        public void Update(float deltaTime)
        {
        }

        public void Start()
        {
        }

        public void Show(bool flag)
        {
            if (rootElement != null)
            {
                rootElement.style.display = flag ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public void LateUpdate(float deltaTime)
        {
        }
    }
}
