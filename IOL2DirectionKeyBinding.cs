using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework;
using ColossalFramework.UI;
using System.Threading;
using UnityEngine;
using static ImageOverlay2.IOL2Mod;
using static ImageOverlay2.ImageOverlay;
using System.IO;
using ICities;


namespace ImageOverlay2{
    internal class IOL2DirectionKeyBinding : UICustomControl {
        private const string thisCategory = @"ImageOverlay2";
        //private SavedInputKey m_EditingBinding;

        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleMoveUp = @"toggleMoveUp";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleMoveDown = @"toggleMoveDown";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleMoveLeft = @"toggleMoveLeft";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleMoveRight = @"toggleMoveRight";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleSlowMoveUp = @"toggleSlowMoveUp";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleSlowMoveDown = @"toggleSlowMoveDown";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleSlowMoveLeft = @"toggleSlowMoveLeft";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleSlowMoveRight = @"toggleSlowMoveRight";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleFastMoveUp = @"toggleFastMoveUp";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleFastMoveDown = @"toggleFastMoveDown";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleFastMoveLeft = @"toggleFastMoveLeft";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleFastMoveRight = @"toggleFastMoveRight";



        private static readonly InputKey defaultToggleMoveUpKey = SavedInputKey.Encode(KeyCode.UpArrow, false, true, false);
        private static readonly InputKey defaultToggleMoveDownKey = SavedInputKey.Encode(KeyCode.DownArrow, false, true, false);
        private static readonly InputKey defaultToggleMoveLeftKey = SavedInputKey.Encode(KeyCode.LeftArrow, false, true, false);
        private static readonly InputKey defaultToggleMoveRightKey = SavedInputKey.Encode(KeyCode.RightArrow, false, true, false);
        private static readonly InputKey defaultToggleSlowMoveUpKey = SavedInputKey.Encode(KeyCode.None, false, false, false);
        private static readonly InputKey defaultToggleSlowMoveDownKey = SavedInputKey.Encode(KeyCode.None, false, false, false);
        private static readonly InputKey defaultToggleSlowMoveLeftKey = SavedInputKey.Encode(KeyCode.None, false, false, false);
        private static readonly InputKey defaultToggleSlowMoveRightKey = SavedInputKey.Encode(KeyCode.None, false, false, false);
        private static readonly InputKey defaultToggleFastMoveUpKey = SavedInputKey.Encode(KeyCode.None, false, false, false);
        private static readonly InputKey defaultToggleFastMoveDownKey = SavedInputKey.Encode(KeyCode.None, false, false, false);
        private static readonly InputKey defaultToggleFastMoveLeftKey = SavedInputKey.Encode(KeyCode.None, false, false, false);
        private static readonly InputKey defaultToggleFastMoveRightKey = SavedInputKey.Encode(KeyCode.None, false, false, false);




        private static readonly SavedInputKey m_moveUp = new SavedInputKey(toggleMoveUp, KeybindingConfigFile, defaultToggleMoveUpKey, true);
        private static readonly SavedInputKey m_moveDown = new SavedInputKey(toggleMoveDown, KeybindingConfigFile, defaultToggleMoveDownKey, true);
        private static readonly SavedInputKey m_moveLeft = new SavedInputKey(toggleMoveLeft, KeybindingConfigFile, defaultToggleMoveLeftKey, true);
        private static readonly SavedInputKey m_moveRight = new SavedInputKey(toggleMoveRight, KeybindingConfigFile, defaultToggleMoveRightKey, true);
        private static readonly SavedInputKey m_slowMoveUp = new SavedInputKey(toggleSlowMoveUp, KeybindingConfigFile, defaultToggleSlowMoveUpKey, true);
        private static readonly SavedInputKey m_slowMoveDown = new SavedInputKey(toggleSlowMoveDown, KeybindingConfigFile, defaultToggleSlowMoveDownKey, true);
        private static readonly SavedInputKey m_slowMoveLeft = new SavedInputKey(toggleSlowMoveLeft, KeybindingConfigFile, defaultToggleSlowMoveLeftKey, true);
        private static readonly SavedInputKey m_slowMoveRight = new SavedInputKey(toggleSlowMoveRight, KeybindingConfigFile, defaultToggleSlowMoveRightKey, true);
        private static readonly SavedInputKey m_fastMoveUp = new SavedInputKey(toggleFastMoveUp, KeybindingConfigFile, defaultToggleFastMoveUpKey, true);
        private static readonly SavedInputKey m_fastMoveDown = new SavedInputKey(toggleFastMoveDown, KeybindingConfigFile, defaultToggleFastMoveDownKey, true);
        private static readonly SavedInputKey m_fastMoveLeft = new SavedInputKey(toggleFastMoveLeft, KeybindingConfigFile, defaultToggleFastMoveLeftKey, true);
        private static readonly SavedInputKey m_fastMoveRight = new SavedInputKey(toggleFastMoveRight, KeybindingConfigFile, defaultToggleFastMoveRightKey, true);

        /*protected void LateUpdate() {
            ImageOverlay.ps = IOL2Mod.gameObject.transform.position;
            ImageOverlay.rt = IOL2Mod.gameObject.transform.eulerAngles;
            ImageOverlay.sc = IOL2Mod.gameObject.transform.localScale;
        }*/

        protected void Update(){
            /*ImageOverlay.ps = IOL2Mod.gameObject.transform.position;
            ImageOverlay.rt = IOL2Mod.gameObject.transform.eulerAngles;
            ImageOverlay.sc = IOL2Mod.gameObject.transform.localScale;
            /*float speedModifier = 1f;
            float slowSpeedFactor = 0.1f;
            float fastSpeedFactor = 3f;
            float positionDelta = 400f * speedModifier * Time.deltaTime;
            float slowPositionDelta = 400f * slowSpeedFactor * Time.deltaTime;
            float fastPositionDelta = 400f * fastSpeedFactor * Time.deltaTime;
            Vector3 scaleDelta = new Vector3(2.5f, 0f, 2.5f) * speedModifier;
            Vector3 rotate90Degree = new Vector3(0f, 90f, 0f);
            Vector3 rotationDelta = new Vector3(0f, 1f, 0f) * speedModifier;*/

            if (!UIView.HasModalInput() && !UIView.HasInputFocus()){
                Event e = Event.current;
                if (EnableModFuntion){
                    if (wake && isMovable && IsCustomPressed(m_moveUp, e)) {
                        go.transform.position += new Vector3(0f, 0f, positionDelta);
                        posz += positionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_moveDown, e)) {
                        go.transform.position += new Vector3(0f, 0f, -positionDelta);
                        posz -= positionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_moveLeft, e)) {
                        go.transform.position += new Vector3(-positionDelta, 0f, 0f);
                        posx -= positionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_moveRight, e)) {
                        go.transform.position += new Vector3(positionDelta, 0f, 0f);
                        posx += positionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_slowMoveUp, e)) {
                        go.transform.position += new Vector3(0f, 0f, slowPositionDelta);
                        posz += slowPositionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_slowMoveDown, e)) {
                        go.transform.position += new Vector3(0f, 0f, -slowPositionDelta);
                        posz -= slowPositionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_slowMoveLeft, e)) {
                        go.transform.position += new Vector3(-slowPositionDelta, 0f, 0f);
                        posx -= slowPositionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_slowMoveRight, e)) {
                        go.transform.position += new Vector3(slowPositionDelta, 0f, 0f);
                        posx += slowPositionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_fastMoveUp, e)) {
                        go.transform.position += new Vector3(0f, 0f, fastPositionDelta);
                        posz += fastPositionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_fastMoveDown, e)) {
                        go.transform.position += new Vector3(0f, 0f, -fastPositionDelta);
                        posz -= fastPositionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_fastMoveLeft, e)) {
                        go.transform.position += new Vector3(-fastPositionDelta, 0f, 0f);
                        posx -= fastPositionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if (wake && isMovable && IsCustomPressed(m_fastMoveRight, e)) {
                        go.transform.position += new Vector3(fastPositionDelta, 0f, 0f);
                        posx += fastPositionDelta;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }
                }
            }
        }





        protected void Awake(){
            UILabel desc = component.AddUIComponent<UILabel>();
            desc.padding.top = 10;
            desc.width = component.width - 50;
            desc.autoHeight = true;
            desc.wordWrap = true;
            desc.textScale = IOL2OptionPanel.SmallFontScale;
            desc.text = IOL2Locale.GetLocale(@"DirectionKeyBindDescription");


            AddKeymapping(@"MoveUp", m_moveUp);
            AddKeymapping(@"MoveDown", m_moveDown);
            AddKeymapping(@"MoveLeft", m_moveLeft);
            AddKeymapping(@"MoveRight", m_moveRight);
            AddKeymapping(@"SlowMoveUp", m_slowMoveUp);
            AddKeymapping(@"SlowMoveDown", m_slowMoveDown);
            AddKeymapping(@"SlowMoveLeft", m_slowMoveLeft);
            AddKeymapping(@"SlowMoveRight", m_slowMoveRight);
            AddKeymapping(@"FastMoveUp", m_fastMoveUp);
            AddKeymapping(@"FastMoveDown", m_fastMoveDown);
            AddKeymapping(@"FastMoveLeft", m_fastMoveLeft);
            AddKeymapping(@"FastMoveRight", m_fastMoveRight);


        }


        


        private bool IsCustomPressed(SavedInputKey inputKey, Event e){
            if (e.type != EventType.KeyDown) return false;
            return Input.GetKey(inputKey.Key) &&
                (e.modifiers & EventModifiers.Control) == EventModifiers.Control == inputKey.Control &&
                (e.modifiers & EventModifiers.Shift) == EventModifiers.Shift == inputKey.Shift &&
                (e.modifiers & EventModifiers.Alt) == EventModifiers.Alt == inputKey.Alt;
        }


        private int SecondlistCount = 0;
        private void AddKeymapping(string key, SavedInputKey savedInputKey){
            UIPanel uIPanel = component.AttachUIComponent(UITemplateManager.GetAsGameObject(@"KeyBindingTemplate")) as UIPanel;
            if (SecondlistCount++ % 2 == 1) uIPanel.backgroundSprite = null;
            UILabel uILabel = uIPanel.Find<UILabel>(@"Name");
            UIButton uIButton = uIPanel.Find<UIButton>(@"Binding");
            //uIPanel.autoLayout = true;
            //uIPanel.autoFitChildrenVertically = true;
            uIButton.eventKeyDown += new KeyPressHandler(OnBindingKeyDown);
            uIButton.eventMouseDown += new MouseEventHandler(OnBindingMouseDown);
            uILabel.stringUserData = key;
            uILabel.text = IOL2Locale.GetLocale(key);
            uIButton.text = savedInputKey.ToLocalizedString(@"KEYNAME");
            uIButton.objectUserData = savedInputKey;
            uIButton.stringUserData = thisCategory; // used for localization TODO:
        }





        private void OnBindingKeyDown(UIComponent comp, UIKeyEventParameter p){
            if (!(IOL2MainKeyBinding.m_EditingBinding is null) && !IsModifierKey(p.keycode)){
                p.Use();
                UIView.PopModal();
                KeyCode keycode = p.keycode;
                InputKey inputKey = (p.keycode == KeyCode.Escape) ? IOL2MainKeyBinding.m_EditingBinding.value : SavedInputKey.Encode(keycode, p.control, p.shift, p.alt);
                if (p.keycode == KeyCode.Backspace){
                    inputKey = SavedInputKey.Empty;
                }
                IOL2MainKeyBinding.m_EditingBinding.value = inputKey;
                UITextComponent uITextComponent = p.source as UITextComponent;
                uITextComponent.text = IOL2MainKeyBinding.m_EditingBinding.ToLocalizedString(@"KEYNAME");
                IOL2MainKeyBinding.m_EditingBinding = null;
            }
        }



        private void OnBindingMouseDown(UIComponent comp, UIMouseEventParameter p){
            if (IOL2MainKeyBinding.m_EditingBinding is null){
                p.Use();
                IOL2MainKeyBinding.m_EditingBinding = (SavedInputKey)p.source.objectUserData;
                UIButton uIButton = p.source as UIButton;
                uIButton.buttonsMask = (UIMouseButton.Left | UIMouseButton.Right | UIMouseButton.Middle | UIMouseButton.Special0 | UIMouseButton.Special1 | UIMouseButton.Special2 | UIMouseButton.Special3);
                uIButton.text = IOL2Locale.GetLocale(@"PressAnyKey");
                p.source.Focus();
                UIView.PushModal(p.source);
            }
            else if (!IsUnbindableMouseButton(p.buttons)){
                p.Use();
                UIView.PopModal();
                InputKey inputKey = SavedInputKey.Encode(ButtonToKeycode(p.buttons), IsControlDown(), IsShiftDown(), IsAltDown());
                IOL2MainKeyBinding.m_EditingBinding.value = inputKey;
                UIButton uIButton2 = p.source as UIButton;
                uIButton2.text = IOL2MainKeyBinding.m_EditingBinding.ToLocalizedString(@"KEYNAME");
                uIButton2.buttonsMask = UIMouseButton.Left;
                IOL2MainKeyBinding.m_EditingBinding = null;
            }
        }


        private KeyCode ButtonToKeycode(UIMouseButton button){
            switch (button){
                case UIMouseButton.Left: return KeyCode.Mouse0;
                case UIMouseButton.Right: return KeyCode.Mouse1;
                case UIMouseButton.Middle: return KeyCode.Mouse2;
                case UIMouseButton.Special0: return KeyCode.Mouse3;
                case UIMouseButton.Special1: return KeyCode.Mouse4;
                case UIMouseButton.Special2: return KeyCode.Mouse5;
                case UIMouseButton.Special3: return KeyCode.Mouse6;
                default: return KeyCode.None;
            }
        }


        private bool IsUnbindableMouseButton(UIMouseButton code) => (code == UIMouseButton.Left || code == UIMouseButton.Right);
        private bool IsModifierKey(KeyCode code) => code == KeyCode.LeftControl || code == KeyCode.RightControl || code == KeyCode.LeftShift ||
                                                    code == KeyCode.RightShift || code == KeyCode.LeftAlt || code == KeyCode.RightAlt;
        private bool IsControlDown() => (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
        private bool IsShiftDown() => (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
        private bool IsAltDown() => (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));





    }
}
