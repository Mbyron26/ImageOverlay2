using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using ICities;
using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;
using ColossalFramework.Globalization;
using static ImageOverlay2.ImageOverlay;

namespace ImageOverlay2{
    public class IOL2Mod : IUserMod, ILoadingExtension { 
        internal const string m_modName = @"Image Overlay 2";
        internal const string m_modVersion = @"1.6.3";
        private const string m_modDesc =  @"Overlay your image on the top of map";
        internal const string KeybindingConfigFile = @"ImageOverlayKeyBindSetting";
        private const string m_debugLogFile = @"ImageOverlay2Debug.log";
        private static float m_TransparencyFactor = 255f;

        public static bool EnableModFuntion = true;
        public static bool isInLoaded;
        public static bool wake = true;

        internal static bool IsInGame = false;
        public static int Percentage {
            get => (int)(m_TransparencyFactor / 255f * 100);
        }

        public static float Transparency {
            get => m_TransparencyFactor;
            set {
                m_TransparencyFactor = value; }
        }

        public static GameObject go;
        public static Texture2D texture;


        public static float posx = 0f;
        public static float posy = 200f;
        public static float posz = 0f;
        public static float rotx = 0f;
        public static float roty = 180f;
        public static float rotz = 0f;
        public static float sclx = 960f;
        public static float scly = 1f;
        public static float sclz = 960f;

        public static float speedModifier = 1f;
        public static float slowSpeedFactor = 0.1f;
        public static float fastSpeedFactor = 3f;
        public static float positionDelta = 400f * speedModifier * Time.deltaTime;
        public static float slowPositionDelta = 400f * slowSpeedFactor * Time.deltaTime;
        public static float fastPositionDelta = 400f * fastSpeedFactor * Time.deltaTime;
        

        public string Name => m_modName + " " + m_modVersion;
        public string Description => m_modDesc;//IOL2Locale.GetLocale(@"ModDescription"); //

        public IOL2Mod() {
            try {
                CreateDebugFile();
            } catch (Exception e) {
                UnityEngine.Debug.LogException(e);
            }
        }

        public void OnEnabled() {
            GameSettings.AddSettingsFile(new SettingsFile[] {
                new SettingsFile() { fileName = KeybindingConfigFile }
            });
            IOL2Locale.Init();
            for (int loadTries = 0; loadTries < 2; loadTries++){
                if (LoadSettings()) break;
            }
        }

        public void OnDisabled() {
            IOL2Locale.Destroy();
            SaveSettings();
        }

        public void OnCreated(ILoading loading) { }

        public void OnReleased() { }

        public void OnLevelLoaded(LoadMode mode) {
            isInLoaded = true;
            go = new GameObject();
            texture = new Texture2D(1, 1);
            string[] deffile = TextureLoad();
            try {
                byte[] bytes = File.ReadAllBytes(deffile[0]);
                texture.LoadImage(bytes);
            }catch{
                UnityEngine.Debug.Log(@"[Image Overlay 2]Error while loading image! Are you sure there is images in Files?");
                return;
            }
            go.AddComponent<ImageOverlay>();
            go.transform.position = new Vector3(posx, posy, posz);
            go.transform.eulerAngles = new Vector3(rotx, roty, rotz);
            go.transform.localScale = new Vector3(sclx, scly, sclz);
            texture = ApplayOpacity(texture, "both");
            textureDict.Add(deffile[0], texture);
            RenderOver.OnLevelLoaded();

        }

        public void OnLevelUnloading() { 
            isInLoaded = false;
            ThreadPool.QueueUserWorkItem(SaveSettings);
            UnLoad(go);
        }

        public void OnSettingsUI(UIHelper helper) {
            IOL2Locale.OnLocaleChanged();
            LocaleManager.eventLocaleChanged += IOL2Locale.OnLocaleChanged;
            ((helper.AddGroup(m_modName + @" -- Version " + m_modVersion) as UIHelper).self as UIPanel).AddUIComponent<IOL2OptionPanel>();
        }

        private const string SettingsFileName = @"ImageOverlay2Config.xml";

        internal static bool LoadSettings() {
            try {
                if (!File.Exists(SettingsFileName)) {
                    SaveSettings();
                }
                XmlDocument xmlConfig = new XmlDocument{
                    XmlResolver = null
                };
                xmlConfig.Load(SettingsFileName);
                EnableModFuntion = bool.Parse(xmlConfig.DocumentElement.GetAttribute(@"EnableModFuntion"));
                m_TransparencyFactor = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"TransparencyFactor"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                posx = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"posx"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                posy = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"posy"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                posz = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"posz"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                rotx = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"rotx"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                roty = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"roty"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                rotz = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"rotz"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                sclx = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"sclx"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                scly = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"scly"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                sclz = float.Parse(xmlConfig.DocumentElement.GetAttribute(@"sclz"), NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat);
                isMovable = bool.Parse(xmlConfig.DocumentElement.GetAttribute(@"isMovable"));
                wake = bool.Parse(xmlConfig.DocumentElement.GetAttribute(@"wake"));
            }
            catch{
                SaveSettings();
                return false;
            }
            return true;
        }

        private static readonly object settingsLock = new object();
        internal static void SaveSettings(object _ = null) {
            Monitor.Enter(settingsLock);
            try {
                XmlDocument xmlConfig = new XmlDocument {
                    XmlResolver = null
                };
            XmlElement root = xmlConfig.CreateElement(@"ImageOverlay2Config");
            _ = root.Attributes.Append(AddElement(xmlConfig, @"EnableModFuntion", EnableModFuntion));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"TransparencyFactor", m_TransparencyFactor));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"posx", posx));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"posy", posy));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"posz", posz));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"rotx", rotx));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"roty", roty));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"rotz", rotz));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"sclx", sclx));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"scly", scly));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"sclz", sclz));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"isMovable", isMovable));
            _ = root.Attributes.Append(AddElement(xmlConfig, @"wake", wake));
            xmlConfig.AppendChild(root);
            xmlConfig.Save(SettingsFileName);
            } finally {
                Monitor.Exit(settingsLock);
            }
        }

      


        private static XmlAttribute AddElement<T>(XmlDocument doc, string name, T t) {
            XmlAttribute attr = doc.CreateAttribute(name);
            attr.Value = t.ToString();
            return attr;
        }


        private static readonly Stopwatch profiler = new Stopwatch();
        private static readonly object fileLock = new object();
        private void CreateDebugFile() {
            profiler.Start();
            /* Create Debug Log File */
            string path = Path.Combine(Application.dataPath, m_debugLogFile);
            using (FileStream debugFile = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            using (StreamWriter sw = new StreamWriter(debugFile)) {
                sw.WriteLine(@"--- " + m_modName + ' ' + m_modVersion + " Debug File ---");
                sw.WriteLine(Environment.OSVersion);
                sw.WriteLine(@"C# CLR Version " + Environment.Version);
                sw.WriteLine(@"Unity Version " + Application.unityVersion);
                sw.WriteLine(@"-------------------------------------");
            }
        }
        


      


    }

}
