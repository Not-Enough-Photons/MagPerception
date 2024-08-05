using System.Linq;
using System.Reflection;

using MelonLoader;

using UnityEngine;

using BoneLib;
using BoneLib.BoneMenu;

using NEP.MagPerception.UI;

namespace NEP.MagPerception
{
    public static class BuildInfo
    {
        public const string Name = "MagPerception"; // Name of the Mod.  (MUST BE SET)
        public const string Author = null; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.2.2"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class Main : MelonMod
    {
        public static AssetBundle resources { get; private set; }

        public override void OnInitializeMelon()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string bundlePath = "NEP.MagPerception.Resources.";
            string targetBundle = HelperMethods.IsAndroid() ? "mp_resources_quest.pack" : "mp_resources_pcvr.pack";

            resources = HelperMethods.LoadEmbeddedAssetBundle(assembly, bundlePath + targetBundle);

            if (resources == null)
            {
                throw new System.Exception(
                    "Resources file is missing/invalid!");
            }

            SetupBonemenu();

            Hooking.OnLevelLoaded += OnSceneWasLoaded;
        }

        private void SetupBonemenu()
        {
            var nepCategory = Page.Root.CreatePage("Not Enough Photons", Color.white);
            var mainCategory = nepCategory.CreatePage("MagPerception", Color.white);
            var offsetCategory = mainCategory.CreatePage("Offset", Color.white);

            mainCategory.CreateFloat("Scale", Color.white, 0.75f, 0.25f, 0.25f, 1.5f, (value) => Settings.InfoScale = value);
            mainCategory.CreateEnum("Show Type", Color.white, UIShowType.FadeShow, (showType) => Settings.ShowType = (UIShowType)showType);
            mainCategory.CreateFloat("Time Until Hidden", Color.white, 3f, 0.5f, 0f, 10f, (value) => Settings.TimeUntilHidden = value);
            mainCategory.CreateBool("Show With Gun", Color.white, false, (value) => Settings.ShowWithGun = value);

            offsetCategory.CreateFloat("X", Color.red, 0.075f, 0.025f, -1f, 1f, (value) => Settings.Offset.x = value);
            offsetCategory.CreateFloat("Y", Color.green, 0f, 0.025f, -1f, 1f, (value) => Settings.Offset.y = value);
            offsetCategory.CreateFloat("Z", Color.blue, 0f, 0.025f, -1f, 1f, (value) => Settings.Offset.z = value);
        }

        public void OnSceneWasLoaded(LevelInfo info)
        {
            new GameObject("Mag Perception Manager").AddComponent<MagPerceptionManager>();
        }

        public static Object GetObjectFromResources(string name)
        {
            Object[] objects = resources.LoadAllAssets();

            return objects.FirstOrDefault((asset) => asset.name == name);
        }
    }
}
