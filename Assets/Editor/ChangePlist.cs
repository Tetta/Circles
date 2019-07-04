#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;
using UnityEngine;
public class ChangePlist : MonoBehaviour {

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject) {
        if (target.ToString() == "iOS" || target.ToString() == "iPhone") {
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            // Change value of CFBundleVersion in Xcode plist
            rootDict.SetString("NSLocationAlwaysUsageDescription", "Application needs Location access for ads optimization!");
            rootDict.SetString("NSLocationWhenInUseUsageDescription", "Application needs Location access for ads optimization!");

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());

        }
    }

}
#endif