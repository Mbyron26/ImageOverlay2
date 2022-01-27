using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using UnityEngine;
using System.IO;
using ColossalFramework.Math;
using ColossalFramework.UI;

namespace ImageOverlay2{
    public class ImageOverlay : MonoBehaviour{
        public static Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
        public static Vector3 ps, rt, sc;
        public static bool isMovable = true;
        public static bool active = true;
        
        


        public static string[] TextureLoad() {
            string[] fileList = Directory.GetFiles("Files/", "*.png");
            return fileList;
        }

        public static void Apply() {
            ApplayOpacity(IOL2Mod.texture, "no");
        }

        public void Update() {
            ps= IOL2Mod.go.transform.position;
            rt = IOL2Mod.go.transform.eulerAngles;
            sc= IOL2Mod.go.transform.localScale;

            /*bool controlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool altDown = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);


            if (shiftDown && Input.GetKeyDown(KeyCode.Return)) {
                if (!active) { 
                    isMovable = true;
                    active = true;
                }else{
                    isMovable = false;
                    active = false;
                }
            }*/
        }


        public static Texture2D ApplayOpacity(Texture2D texture, string flip) {
            if (IOL2Mod.isInLoaded) {
                if (flip == "no") {
                    Color32[] oldColors = texture.GetPixels32();
                    for (int i = 0; i < oldColors.Length; i++){
                        if (oldColors[i].a != 0f) {
                            Color32 newColor = new Color32(oldColors[i].r, oldColors[i].g, oldColors[i].b, (byte)(IOL2Mod.Transparency / 255f * 255));
                            oldColors[i] = newColor;
                        }
                    }
                    texture.SetPixels32(oldColors);
                }else if(flip == "both") {
                    texture = FlipTexture(texture);
                    Color32[] oldColors = texture.GetPixels32();
                    for (int i = 0; i < oldColors.Length; i++) {
                        if (oldColors[i].a != 0f) {
                            Color32 newColor = new Color32(oldColors[i].r, oldColors[i].g, oldColors[i].b, (byte)(IOL2Mod.Transparency / 255f * 255));
                            oldColors[i] = newColor;
                        }
                    }
                    texture.SetPixels32(oldColors);
                }texture.Apply();
                return texture;
            }
            return null;
        }

        public static Texture2D FlipTexture(Texture2D textureToFlip) { 
            Texture2D texture = new Texture2D(textureToFlip.width, textureToFlip.height);
            for (int y = 0; y < textureToFlip.height; ++y) {
                for (int x = 0; x < textureToFlip.width; ++x) { 
                    texture.SetPixel(x, y, textureToFlip.GetPixel(y, x));
                }
            }return texture;
        }

        public static void UnLoad(GameObject go) {
            Destroy(go);
            textureDict.Clear();
        }

        internal class RenderOver : SimulationManagerBase<RenderOver, MonoBehaviour>, ISimulationManager, IRenderableManager {
            public static void OnLevelLoaded(){
                SimulationManager.RegisterManager(instance);
            }

            protected override void EndOverlayImpl(RenderManager.CameraInfo cameraInfo){
                base.EndOverlayImpl(cameraInfo);
                if (!active) return;
                float x = ps.x, y = ps.z;
                float sclx = sc.x, scly = sc.z;

                Quaternion rot = Quaternion.Euler(rt.x, rt.y, rt.z);
                Vector3 center = new Vector3(x, 0, y);
                Quad3 position = new Quad3(
                    new Vector3(-sclx + x, 0, -scly + y),
                    new Vector3(sclx + x, 0, -scly + y),
                    new Vector3(sclx + x, 0, scly + y),
                    new Vector3(-sclx + x, 0, scly + y)
                );
                position.a = rot * (position.a - center) + center;
                position.b = rot * (position.b - center) + center;
                position.c = rot * (position.c - center) + center;
                position.d = rot * (position.d - center) + center;

                RenderManager.instance.OverlayEffect.DrawQuad(cameraInfo, IOL2Mod.texture, Color.white, position, -1f, 1800f, false, true);
            }
        }

    }
}
