using System;
using System.Globalization;
using System.Xml;
using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.PlatformServices;

namespace ImageOverlay2{
    internal static class IOL2Locale{
        private const ulong m_thisModID = 2616880500;
        private static XmlDocument m_xmlLocale;
        private static string m_directory;
        private static bool isInitialized = false;


        public static void OnLocaleChanged() {
            string locale = SingletonLite<LocaleManager>.instance.language;
            if (locale == @"zh") {
                if (CultureInfo.InstalledUICulture.Name == @"zh-TW") {
                    locale = @"zh-TW";
                }else{
                    locale = @"zh-CN";
                }
            }else if(locale == @"pt") { 
                if(CultureInfo.InstalledUICulture.Name==@"pt-BR"){ 
                locale = @"pt-BR";
                }
            }else{
                switch (CultureInfo.InstalledUICulture.Name) {
                    case @"ms":
                    case @"ms-MY":
                        locale = @"ms";
                        break;
                    case @"ja":
                    case @"jia-JP":
                        locale = @"ja";
                        break ;
                }
            }
            LoadLocale(locale);
            IOL2OptionPanel[] optionPanel = UnityEngine.Object.FindObjectsOfType<IOL2OptionPanel>();
            foreach (var panel in optionPanel) {
                panel.Invalidate();
            }
        }

        private static void LoadLocale(string culture) {
            string localeFile = m_directory + @"ImageOverlay2." + culture + @".locale";
            XmlDocument locale = new XmlDocument();
            try {
                locale.Load(localeFile);
            }catch{
                localeFile = m_directory + @"ImageOverlay2.en.locale";
                locale.Load(localeFile);
            }
            m_xmlLocale = locale;
        }


        internal static void Init() {
            if (!isInitialized){
                try{
                    foreach (PublishedFileId fileID in PlatformService.workshop.GetSubscribedItems()){
                        if (fileID.AsUInt64 == m_thisModID){
                            m_directory = PlatformService.workshop.GetSubscribedItemPath(fileID) + @"/Locale/";
                            break;
                        }
                    }
                    if (m_directory == null){
                        m_directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"/Colossal Order/Cities_Skylines/Addons/Mods/ImageOverlay2/Locale";
                    }
                }catch (Exception e){
                    UnityEngine.Debug.LogException(e);
                }
                isInitialized = true;
            }
        }


        internal static void Destroy() {
            LocaleManager.eventLocaleChanged -= OnLocaleChanged;
        }

        internal static string GetLocale(string name)=>m_xmlLocale.GetElementById(name).InnerText;


    }
}
