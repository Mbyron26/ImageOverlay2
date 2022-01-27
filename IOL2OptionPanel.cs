using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using ICities;
using static ImageOverlay2.IOL2Mod;

namespace ImageOverlay2{
    public class IOL2OptionPanel : UIPanel {
        private const string m_optionPanelName = @"ImageOverlay2OptionPanel";

        public const float TabFontScale = 0.9f;
        private const float MinScaleFactor = 1f;
        private const float MaxScaleFactor = 255f;
        public const float SmallFontScale = 0.85f;
        public const float DefaultFontScale = 0.95f;
        private const string EnableModCBName = @"EnableModCB";

        private static UICheckBox EnabledModCB;
        private static UISlider OverlaySlider;
        private UILabel TransparencyAdjustmentLabel;

        protected IOL2OptionPanel() {
            gameObject.name = m_optionPanelName;
            name = m_optionPanelName;
        }

        public override void Awake(){
            base.OnEnable();
            FitTo(m_Parent);
            isLocalized = true;
            //m_AutoFitChildrenVertically = true;
            //FitChildrenVertically(590f);

            m_AutoLayoutDirection = LayoutDirection.Vertical;
            m_AutoLayout = true;
            UITabstrip tabBar = AddUIComponent<UITabstrip>();
            UITabContainer tabContainer = AddUIComponent<UITabContainer>();
            tabBar.tabPages = tabContainer;
            tabContainer.FitTo(m_Parent);

            UIPanel mainPanel = AddTab(tabBar, IOL2Locale.GetLocale(@"MainOptionTab"), 0, true);
            mainPanel.autoLayout = false;
            mainPanel.autoSize = false;
            ShowStandardOptions(mainPanel);
            UpdateState(IsInGame);

            /*UIPanel keyBinding = AddTab(tabBar, IOL2Locale.GetLocale(@"KeyboardShortcutTab"), 1, true);
            keyBinding.autoLayout = false;
            keyBinding.autoSize = false;*/

            
            AddTab(tabBar, IOL2Locale.GetLocale(@"KeyboardShortcutTab"), 1, true).gameObject.AddComponent<IOL2MainKeyBinding>();
            AddTab(tabBar, IOL2Locale.GetLocale(@"KeyboardDirectionShortcutTab"), 2, true).gameObject.AddComponent<IOL2DirectionKeyBinding>();



        }

        public static void UpdateState(bool isInGame) { 
            if (isInGame) {
                OverlaySlider?.Disable();
                return;
            }
            OverlaySlider?.Enable();       
        }


        private void ShowStandardOptions(UIPanel panel) {
            EnabledModCB = AddCheckBox(panel, IOL2Locale.GetLocale(@"EnableMod"), EnableModFuntion, (_, isChecked) => {
                EnableModFuntion = isChecked;
                ThreadPool.QueueUserWorkItem(SaveSettings);
            });
            EnabledModCB.cachedName = EnableModCBName;
            EnabledModCB.name = EnableModCBName;
            EnabledModCB.AlignTo(panel, UIAlignAnchor.TopLeft);
            EnabledModCB.relativePosition = new Vector3(2, 5);
            EnabledModCB.width = 300;

            UIPanel ScalePanel = (UIPanel)panel.AttachUIComponent(UITemplateManager.GetAsGameObject(@"OptionsSliderTemplate"));
            TransparencyAdjustmentLabel = ScalePanel.Find<UILabel>(@"Label");
            TransparencyAdjustmentLabel.width = panel.width - 100;
            TransparencyAdjustmentLabel.textScale = 1.1f;
            TransparencyAdjustmentLabel.text = String.Format(IOL2Locale.GetLocale(@"VisibilityAdjustment"), Percentage);
            OverlaySlider = AddSlider(ScalePanel, MinScaleFactor, MaxScaleFactor, 1f, Transparency, (_, val) => {
                Transparency = val;
                TransparencyAdjustmentLabel.text = String.Format(IOL2Locale.GetLocale(@"VisibilityAdjustment"), Percentage);
                ThreadPool.QueueUserWorkItem(SaveSettings);
            });
            ScalePanel.AlignTo(EnabledModCB, UIAlignAnchor.BottomLeft);
            ScalePanel.relativePosition = new Vector3(0, EnabledModCB.height+5);
            OverlaySlider.width = panel.width - 150;
            UILabel AttentionLabel = AddDescription(panel, @"AttentionLabel", ScalePanel, DefaultFontScale, IOL2Locale.GetLocale(@"Attention"));
            UIButton Apply = AddButton(panel, 2f, 135f, IOL2Locale.GetLocale("Apply"));
            Apply.eventClick += (components, clickEvent) => { 
                ImageOverlay.Apply();
                
            };

        }


        



        private static UIPanel AddTab(UITabstrip tabStrip, string tabName, int tabIndex, bool autoLayout) { 
            UIButton tabButton = tabStrip.AddTab(tabName);
            tabButton.normalBgSprite = @"SubBarButtonBase";
            tabButton.disabledBgSprite = @"SubBarButtonBaseDisabled";
            tabButton.focusedBgSprite = @"SubBarButtonBaseFocused";
            tabButton.hoveredBgSprite = @"SubBarButtonBaseHovered";
            tabButton.pressedBgSprite = @"SubBarButtonBasePressed";
            tabButton.tooltip = tabName;
            tabButton.width = 175;
            tabButton.textScale = TabFontScale;
            tabStrip.selectedIndex = tabIndex;
            UIPanel rootPanel = tabStrip.tabContainer.components[tabIndex] as UIPanel;
            rootPanel.autoLayout = autoLayout;
            if(autoLayout) {
                rootPanel.autoLayoutDirection = LayoutDirection.Vertical;
                rootPanel.autoLayoutPadding.top = 0;
                rootPanel.autoLayoutPadding.bottom = 0;
                rootPanel.autoLayoutPadding.left = 5;
            }
            return rootPanel;
        }


        private static UICheckBox AddCheckBox(UIPanel panel, string name, bool defaultVal, PropertyChangedEventHandler<bool>callback) {
            UICheckBox cb = (UICheckBox)panel.AttachUIComponent(UITemplateManager.GetAsGameObject(@"OptionsCheckBoxTemplate"));
            cb.eventCheckChanged += callback;
            cb.text = name;
            cb.height += 10;
            cb.isChecked = defaultVal;
            return cb;
        }

        private static void AddSpace(UIPanel panel, float height) { 
            UIPanel space = panel.AddUIComponent<UIPanel>();
            space.name = @"Space";
            space.isInteractive = false;
            space.height = height;  
        }

        private static UILabel AddDescription(UIPanel panel, string name, UIComponent alignTo, float fontScale, string text) { 
            UILabel desc = panel.AddUIComponent<UILabel>();
            if (!(alignTo is null)) desc.AlignTo(alignTo, UIAlignAnchor.BottomLeft);
            desc.name = name;
            desc.width = panel.width - 80;
            desc.wordWrap = true;
            desc.autoHeight = true;
            desc.textScale = fontScale;
            desc.text = text;
            desc.relativePosition = new Vector3(1, 23);
            AddSpace(panel, desc.height);
            return desc;
        }

        private static UISlider AddSlider(UIPanel panel, float min, float max, float step, float defaultVal, PropertyChangedEventHandler<float> callback) {
            UISlider slider = panel.Find<UISlider>(@"Slider");
            slider.minValue = min;
            slider.maxValue = max;
            slider.stepSize = step;
            slider.value = defaultVal;
            slider.eventValueChanged += callback;
            return slider;
        }

        private static UIButton AddButton(UIComponent panel, float posX, float posY, string text, float width = 100f, float height = 30f, float scale = 1f)
        {
            UIButton button = panel.AddUIComponent<UIButton>();
            button.relativePosition = new Vector2(posX, posY);
            button.size = new Vector2(width, height);
            button.textScale = scale;
            button.normalBgSprite = "ButtonMenu";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.disabledTextColor = new Color32(128, 128, 128, 255);
            button.canFocus = false;
            //button.textPadding=new RectOffsett(0, 0, vertPad, 0);
            button.textVerticalAlignment = UIVerticalAlignment.Middle;
            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
            button.text = text;
            return button;
        }


        

















    }
    
}
