using System.IO;
using UnityEditor;
using UnityEngine;

namespace HegaCore.Editor
{
    public sealed partial class DependenciesUpdater : MonoBehaviour
    {
        [MenuItem("Hega/Update HegaCore Dependencies", priority = -1)]
        public static void Update()
        {
            var manifestPath = Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");
            var filePath = Path.GetFullPath(manifestPath);
            var json = File.ReadAllText(filePath);
            var manifest = new Manifest().Deserialize(SimpleJSON.Parse(json));
            Ensure(manifest);

            File.WriteAllText($"{filePath}", manifest.Serialize().ToString(2));

            AssetDatabase.Refresh();
        }

        private static void Ensure(Manifest manifest)
        {
            var d = manifest.dependencies;

            d["com.unity.addressables"]                             = "1.16.19";
            d["com.unity.scriptablebuildpipeline"]                  = "1.17.0";
            d["com.unity.textmeshpro"]                              = "2.1.6";

            d["com.hegaglobal.visual-novel-data"]                   = "https://github.com/hegaglobal/VisualNovelData.git#1.4.10";

            d["com.grashaar.uiman-textmeshpro"]                     = "1.4.1";
            d["com.grashaar.unity-google-spreadsheet-downloader"]   = "2.0.1";
            d["com.grashaar.unity-objectpooling"]                   = "1.5.0";

            d["com.laicasaane.texttyper"]                           = "3.1.0";
            d["com.laicasaane.tinycsvparser"]                       = "2.6.3";
            d["com.laicasaane.unity-addressables-manager"]          = "1.2.1";
            d["com.laicasaane.unity-supplements"]                   = "2.6.0";

            d["com.minhdu.uiman"]                                   = "1.4.31";
            d["com.live2d.cubism-cubismloader"]                     = "4.0.301";

            d["com.cysharp.unitask"]                                = "2.2.5";
            d["com.littlebigfun.addressable-importer"]              = "0.11.0";
            d["com.merlin.easyeventeditor"]                         = "1.0.4";
            d["jillejr.newtonsoft.json-for-unity"]                  = "13.0.102";

            var registry = EnsureRegistry(manifest);

            Ensure(registry,
                "com.openupm",
                "com.cysharp.unitask",
                "com.minhdu.uiman",
                "com.grashaar.uiman-textmeshpro",
                "com.grashaar.unity-google-spreadsheet-downloader",
                "com.grashaar.unity-objectpooling",
                "com.laicasaane.texttyper",
                "com.laicasaane.tinycsvparser",
                "com.laicasaane.unity-addressables-manager",
                "com.laicasaane.unity-supplements",
                "com.littlebigfun.addressable-importer",
                "com.live2d.cubism-cubismloader",
                "com.merlin.easyeventeditor",
                "jillejr.newtonsoft.json-for-unity"
            );
        }

        private static ScopedRegistry EnsureRegistry(Manifest manifest)
        {
            const string package_openupm_com = "package.openupm.com";
            var r = manifest.scopedRegistries;

            var index = r.FindIndex(x => x.name.Equals(package_openupm_com));

            ScopedRegistry registry;

            if (index >= 0)
                registry = r[index];
            else
                r.Add(registry = new ScopedRegistry {
                    name = package_openupm_com,
                    url = "https://package.openupm.com"
                });

            return registry;
        }

        private static void Ensure(ScopedRegistry registry, params string[] scopes)
        {
            var s = registry.scopes;

            foreach (var scope in scopes)
            {
                if (string.IsNullOrEmpty(scope))
                    continue;

                if (!s.Contains(scope))
                    s.Add(scope);
            }
        }
    }
}