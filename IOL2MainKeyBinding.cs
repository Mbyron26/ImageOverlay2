using System;
using ColossalFramework;
using ColossalFramework.UI;
using System.Threading;
using UnityEngine;
using static ImageOverlay2.IOL2Mod;
using static ImageOverlay2.ImageOverlay;
using System.IO;


namespace ImageOverlay2{
    internal class IOL2MainKeyBinding : UICustomControl {
        private const string thisCategory = @"ImageOverlay2";
        public static SavedInputKey m_EditingBinding;

        int c = 1;
        int count = 0;
        
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleFitToTiles = @"toggleFitToTiles";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleCycleThroughImages = @"toggleCycleThroughImages";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleActive = @"toggleActive";


        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleResetPositionToDefault = @"toggleResetPositionToDefault";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleLock = @"toggleLock";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleScaleUp = @"toggleScaleUp";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleScaleDown = @"toggleScaleDown";


        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleRotating90DegressClockwise = @"toggleRotating90DegressClockwise";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleRotating90DegressClockwiseRevert = @"toggleRotating90DegressClockwiseRevert";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleRotatingClockwise = @"toggleRotatingClockwise";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string toggleRotatingClockwiseRevert = @"toggleRotatingClockwiseRevert";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string togglePreciseRotatingClockwise = @"togglePreciseRotatingClockwise";
        [RebindableKey(@"ImageOverlay2")]
        private static readonly string togglePreciseRotatingClockwiseRevert = @"togglePreciseRotatingClockwiseRevert";


        private static readonly InputKey defaultToggleFitToTilesKey = SavedInputKey.Encode(KeyCode.T, false, true, false);
        private static readonly InputKey defaultToggleCycleThroughImagesKey = SavedInputKey.Encode(KeyCode.R, false, true, false);
        private static readonly InputKey defaultToggleActiveKey = SavedInputKey.Encode(KeyCode.Return, false, true, false);


        private static readonly InputKey defaultToggleResetPositionToDefaultKey = SavedInputKey.Encode(KeyCode.B, false, true, false);
        private static readonly InputKey defaultToggleLockKey = SavedInputKey.Encode(KeyCode.V, false, true, false);
        private static readonly InputKey defaultToggleScaleUpKey = SavedInputKey.Encode(KeyCode.Equals, false, true, false);
        private static readonly InputKey defaultToggleScaleDownKey = SavedInputKey.Encode(KeyCode.Minus, false, true, false);

        private static readonly InputKey defaultToggleRotating90DegressClockwiseKey = SavedInputKey.Encode(KeyCode.RightBracket, false, true, false);
        private static readonly InputKey defaultToggleRotating90DegressClockwiseRevertKey = SavedInputKey.Encode(KeyCode.LeftBracket, false, true, false);
        private static readonly InputKey defaultToggleRotatingClockwiseKey = SavedInputKey.Encode(KeyCode.E, false, true, false);
        private static readonly InputKey defaultToggleRotatingClockwiseRevertKey = SavedInputKey.Encode(KeyCode.Q, false, true, false);
        private static readonly InputKey defaultTogglePreciseRotatingClockwiseKey = SavedInputKey.Encode(KeyCode.None, false, false, false);
        private static readonly InputKey defaultTogglePreciseRotatingClockwiseRevertKey = SavedInputKey.Encode(KeyCode.None, false, false, false);

        private static readonly SavedInputKey m_fitToTile = new SavedInputKey(toggleFitToTiles, KeybindingConfigFile, defaultToggleFitToTilesKey, true);
        private static readonly SavedInputKey m_cycleThroughImages = new SavedInputKey(toggleCycleThroughImages, KeybindingConfigFile, defaultToggleCycleThroughImagesKey, true);
        private static readonly SavedInputKey m_active = new SavedInputKey(toggleActive, KeybindingConfigFile, defaultToggleActiveKey, true);


        private static readonly SavedInputKey m_resetPositionToDefault = new SavedInputKey(toggleResetPositionToDefault, KeybindingConfigFile, defaultToggleResetPositionToDefaultKey, true);
        private static readonly SavedInputKey m_lock = new SavedInputKey(toggleLock, KeybindingConfigFile, defaultToggleLockKey, true);
        private static readonly SavedInputKey m_scaleUp = new SavedInputKey(toggleScaleUp, KeybindingConfigFile, defaultToggleScaleUpKey, true);
        private static readonly SavedInputKey m_scaleDown = new SavedInputKey(toggleScaleDown, KeybindingConfigFile, defaultToggleScaleDownKey, true);

        private static readonly SavedInputKey m_rotating90DegressClockwise = new SavedInputKey(toggleRotating90DegressClockwise, KeybindingConfigFile, defaultToggleRotating90DegressClockwiseKey, true);
        private static readonly SavedInputKey m_rotating90DegressClockwiseRevert = new SavedInputKey(toggleRotating90DegressClockwiseRevert, KeybindingConfigFile, defaultToggleRotating90DegressClockwiseRevertKey, true);
        private static readonly SavedInputKey m_rotatingClockwise = new SavedInputKey(toggleRotatingClockwise, KeybindingConfigFile, defaultToggleRotatingClockwiseKey, true);
        private static readonly SavedInputKey m_rotatingClockwiseRevert = new SavedInputKey(toggleRotatingClockwiseRevert, KeybindingConfigFile, defaultToggleRotatingClockwiseRevertKey, true);
        private static readonly SavedInputKey m_preciseRotatingClockwise = new SavedInputKey(togglePreciseRotatingClockwise, KeybindingConfigFile, defaultTogglePreciseRotatingClockwiseKey, true);
        private static readonly SavedInputKey m_preciseRotatingClockwiseRevert = new SavedInputKey(togglePreciseRotatingClockwiseRevert, KeybindingConfigFile, defaultTogglePreciseRotatingClockwiseRevertKey, true);

        


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
            */
            /*gameObject.GetComponent<ImageOverlay>();
            ps = go.transform.position;
            rt = go.transform.eulerAngles;
            sc = go.transform.localScale;*/
            Vector3 scaleDelta = new Vector3(2.5f, 0f, 2.5f) * speedModifier;
            Vector3 rotate90Degree = new Vector3(0f, 90f, 0f);
            Vector3 rotationDelta = new Vector3(0f, 1f, 0f) * speedModifier;
            Vector3 preciseRotationDelta = new Vector3(0f, 1f, 0f) * slowSpeedFactor;

            if (!UIView.HasModalInput() && !UIView.HasInputFocus()){
                Event e = Event.current;
                if (EnableModFuntion){
                    if (wake && isMovable && IsCustomPressed(m_fitToTile, e)) {
                    c += 1;
                    if(c == 5) c = 1;
                    switch (c) { 
                        case 1: go.transform.localScale = new Vector3(960f, 1f, 960f);
                                sclx = 960f; scly = 1f; sclz = 960f;
                                ThreadPool.QueueUserWorkItem(SaveSettings);
                                break;
                        case 2: go.transform.localScale = new Vector3(2880f, 1f, 2880f);
                                sclx = 2880f; scly = 1f; sclz = 2880f;
                                ThreadPool.QueueUserWorkItem(SaveSettings);
                                break;
                        case 3: go.transform.localScale = new Vector3(4800f, 1f, 4800f);
                                sclx = 4800f; scly = 1f; sclz = 4800f;
                                ThreadPool.QueueUserWorkItem(SaveSettings);
                                break ;
                        case 4: go.transform.localScale = new Vector3(8640f, 1f, 8640f);
                                sclx = 8640f; scly = 1f; sclz = 8640f;
                                ThreadPool.QueueUserWorkItem(SaveSettings);
                                break;
                    }
                    }else if(wake && isMovable && IsCustomPressed(m_cycleThroughImages, e)) {
                    string[] files = TextureLoad();
                    //gameObject.AddComponent<ImageOverlay>();
                    Texture2D texture = new Texture2D(1, 1);
                    if (textureDict.ContainsKey(files[count])) {
                        texture = textureDict[files[count]];
                    }else { 
                        byte[] bytes = File.ReadAllBytes(files[count]);
                        texture.LoadImage(bytes);
                        texture = ApplayOpacity(texture, "both");
                        textureDict.Add(files[count], texture);
                    }
                    texture.Apply();
                    IOL2Mod.texture = texture;
                    count += 1;
                    if(count == files.Length) count = 0;
                    }else if(IsCustomPressed(m_active, e)) {
                    if (!active) {
                        //ImageOverlay.isMovable = true;
                        active = true;
                        wake   = true;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else { 
                        //ImageOverlay.isMovable=false;
                        active = false;
                        wake = false;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }
                    }else if(wake && isMovable && IsCustomPressed(m_resetPositionToDefault, e)) { 
                        go.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                        go.transform.position = new Vector3(0f, 200f, 0f);
                        rotx = 0f; roty = 180f; rotz = 0f;
                        posx = 0f; posy = 200f; posz = 0f;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if(IsCustomPressed(m_lock, e)) { 
                        if(wake) {
                        if(active) isMovable = !isMovable;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                        }
                    }else if(wake && isMovable && IsCustomPressed(m_scaleUp, e)) {
                        go.transform.localScale += scaleDelta;
                        sclx += 2.5f; scly += 0f; sclz += 2.5f;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if(wake && isMovable && IsCustomPressed(m_scaleDown, e)) { 
                        go.transform.localScale -= scaleDelta * speedModifier;
                        sclx -= 2.5f; scly -= 0f; sclz -= 2.5f;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if(wake && isMovable && IsCustomPressed(m_rotating90DegressClockwise, e)) {
                        go.transform.localEulerAngles += rotate90Degree;
                        rotx += 0f; roty += 90f; rotz += 0f;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if(wake && isMovable && IsCustomPressed(m_rotating90DegressClockwiseRevert, e)) {
                        go.transform.localEulerAngles -= rotate90Degree;
                        rotx -= 0f; roty -= 90f; rotz -= 0f;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if(wake && isMovable && IsCustomPressed(m_rotatingClockwise, e)) {
                        go.transform.localEulerAngles += rotationDelta;
                        rotx += 0f; roty += 1f; rotz += 0f;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if(wake && isMovable && IsCustomPressed(m_rotatingClockwiseRevert, e)) { 
                        go.transform.localEulerAngles -= rotationDelta;
                        rotx -= 0f; roty -= 1f; rotz -= 0f;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if(wake && isMovable && IsCustomPressed(m_preciseRotatingClockwise, e)) {
                        go.transform.localEulerAngles += preciseRotationDelta;
                        rotx += 0f; roty += 0.1f; rotz += 0f;
                        ThreadPool.QueueUserWorkItem(SaveSettings);
                    }else if(wake && isMovable && IsCustomPressed(m_preciseRotatingClockwiseRevert, e)) { 
                        go.transform.localEulerAngles -= preciseRotationDelta;
                        rotx -= 0f; roty -= 0.1f; rotz -= 0f;
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
            desc.text = IOL2Locale.GetLocale(@"KeyBindDescription");

            
            AddKeymapping(@"Active", m_active);
            AddKeymapping(@"FitToTile", m_fitToTile);
            AddKeymapping(@"CycleThroughImages", m_cycleThroughImages);
            AddKeymapping(@"ResetPositionToDefault", m_resetPositionToDefault);
            AddKeymapping(@"Lock", m_lock);
            AddKeymapping(@"ScaleUp", m_scaleUp);
            AddKeymapping(@"ScaleDown", m_scaleDown);
            AddKeymapping(@"Rotating90DegressClockwise", m_rotating90DegressClockwise);
            AddKeymapping(@"Rotating90DegressClockwiseRevert", m_rotating90DegressClockwiseRevert);
            AddKeymapping(@"RotatingClockwise", m_rotatingClockwise);
            AddKeymapping(@"RotatingClockwiseRevert", m_rotatingClockwiseRevert);
            AddKeymapping(@"PreciseClockwiseRotation", m_preciseRotatingClockwise);
            AddKeymapping(@"PreciseCounterclockwiseRotation", m_preciseRotatingClockwiseRevert);

        }


        


        private bool IsCustomPressed(SavedInputKey inputKey, Event e){
            if (e.type != EventType.KeyDown) return false;
            return Input.GetKey(inputKey.Key) &&
                (e.modifiers & EventModifiers.Control) == EventModifiers.Control == inputKey.Control &&
                (e.modifiers & EventModifiers.Shift) == EventModifiers.Shift == inputKey.Shift &&
                (e.modifiers & EventModifiers.Alt) == EventModifiers.Alt == inputKey.Alt;
        }


        private int listCount = 0;
        private void AddKeymapping(string key, SavedInputKey savedInputKey){
            UIPanel uIPanel = component.AttachUIComponent(UITemplateManager.GetAsGameObject(@"KeyBindingTemplate")) as UIPanel;
            if (listCount++ % 2 == 1) uIPanel.backgroundSprite = null;
            UILabel uILabel = uIPanel.Find<UILabel>(@"Name");
            UIButton uIButton = uIPanel.Find<UIButton>(@"Binding");
            uIButton.eventKeyDown += new KeyPressHandler(OnBindingKeyDown);
            uIButton.eventMouseDown += new MouseEventHandler(OnBindingMouseDown);
            uILabel.stringUserData = key;
            uILabel.text = IOL2Locale.GetLocale(key);
            uIButton.text = savedInputKey.ToLocalizedString(@"KEYNAME");
            uIButton.objectUserData = savedInputKey;
            uIButton.stringUserData = thisCategory; // used for localization TODO:
        }





        private void OnBindingKeyDown(UIComponent comp, UIKeyEventParameter p){
            if (!(m_EditingBinding is null) && !IsModifierKey(p.keycode)){
                p.Use();
                UIView.PopModal();
                KeyCode keycode = p.keycode;
                InputKey inputKey = (p.keycode == KeyCode.Escape) ? m_EditingBinding.value : SavedInputKey.Encode(keycode, p.control, p.shift, p.alt);
                if (p.keycode == KeyCode.Backspace){
                    inputKey = SavedInputKey.Empty;
                }
                m_EditingBinding.value = inputKey;
                UITextComponent uITextComponent = p.source as UITextComponent;
                uITextComponent.text = m_EditingBinding.ToLocalizedString(@"KEYNAME");
                m_EditingBinding = null;
            }
        }



        private void OnBindingMouseDown(UIComponent comp, UIMouseEventParameter p){
            if (m_EditingBinding is null){
                p.Use();
                m_EditingBinding = (SavedInputKey)p.source.objectUserData;
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
                m_EditingBinding.value = inputKey;
                UIButton uIButton2 = p.source as UIButton;
                uIButton2.text = m_EditingBinding.ToLocalizedString(@"KEYNAME");
                uIButton2.buttonsMask = UIMouseButton.Left;
                m_EditingBinding = null;
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
