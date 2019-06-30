/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using UnityEditor;



namespace BuildReportTool.Window
{

public static class Utility
{
	public static void DrawCentralMessage(Rect position, string msg)
	{
		float w = 300;
		float h = 100;
		float x = (position.width - w) * 0.5f;
		float y = (position.height - h) * 0.25f;

		GUI.Label(new Rect(x, y, w, h), msg);
	}

	public static void PingAssetInProject(string file)
	{
		if (!file.StartsWith("Assets/"))
		{
			return;
		}

		// thanks to http://answers.unity3d.com/questions/37180/how-to-highlight-or-select-an-asset-in-project-win.html
		var asset = AssetDatabase.LoadMainAssetAtPath(file);
		if (asset != null)
		{
			GUISkin temp = GUI.skin;
			GUI.skin = null;

			//EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(file, typeof(Object)));
			EditorGUIUtility.PingObject(asset);
			Selection.activeObject = asset;

			GUI.skin = temp;
		}
	}



	public static string GetProperBuildSizeDesc(BuildInfo buildReportToDisplay)
	{
		BuildReportTool.BuildPlatform buildPlatform = BuildReportTool.ReportGenerator.GetBuildPlatformFromString(buildReportToDisplay.BuildType, buildReportToDisplay.BuildTargetUsed);

		switch (buildPlatform)
		{
			case BuildReportTool.BuildPlatform.MacOSX32:
				return Labels.BUILD_SIZE_MACOSX_DESC;
			case BuildReportTool.BuildPlatform.MacOSX64:
				return Labels.BUILD_SIZE_MACOSX_DESC;
			case BuildReportTool.BuildPlatform.MacOSXUniversal:
				return Labels.BUILD_SIZE_MACOSX_DESC;

			case BuildReportTool.BuildPlatform.Windows32:
				return Labels.BUILD_SIZE_WINDOWS_DESC;
			case BuildReportTool.BuildPlatform.Windows64:
				return Labels.BUILD_SIZE_WINDOWS_DESC;

			case BuildReportTool.BuildPlatform.Linux32:
				return Labels.BUILD_SIZE_STANDALONE_DESC;
			case BuildReportTool.BuildPlatform.Linux64:
				return Labels.BUILD_SIZE_STANDALONE_DESC;
			case BuildReportTool.BuildPlatform.LinuxUniversal:
				return Labels.BUILD_SIZE_LINUX_UNIVERSAL_DESC;

			case BuildReportTool.BuildPlatform.Android:
				if (buildReportToDisplay.AndroidCreateProject)
				{
					return Labels.BUILD_SIZE_ANDROID_WITH_PROJECT_DESC;
				}
				if (buildReportToDisplay.AndroidUseAPKExpansionFiles)
				{
					return Labels.BUILD_SIZE_ANDROID_WITH_OBB_DESC;
				}
				return Labels.BUILD_SIZE_ANDROID_DESC;

			case BuildReportTool.BuildPlatform.iOS:
				return Labels.BUILD_SIZE_IOS_DESC;

			case BuildReportTool.BuildPlatform.Web:
				return Labels.BUILD_SIZE_WEB_DESC;
		}
		return "";
	}




	public static void DrawLargeSizeDisplay(string label, string desc, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return;
		}

		GUILayout.BeginVertical();
			GUILayout.Label(label, BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);
			GUILayout.Label(desc, BuildReportTool.Window.Settings.TINY_HELP_STYLE_NAME);
			GUILayout.Label(value, BuildReportTool.Window.Settings.BIG_NUMBER_STYLE_NAME);
		GUILayout.EndVertical();
	}
}

}