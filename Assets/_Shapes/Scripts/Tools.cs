using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Networking;

public class Tools {
	

	public static string LoadAsText(string sheetName)
	{
		return LoadAsText(sheetName,"txt");
	}
	
	public static string LoadAsText(string sheetName,string format)
	{
		string text = string.Empty;
		string path = string.Format("{0}{1}{2}.{3}", GetDocumentPath(), "gamedata/", sheetName, format);
		if (File.Exists(path))
		{
			Stream stream = File.Open(path, FileMode.Open);
			StreamReader reader = new StreamReader(stream);
			text = reader.ReadToEnd();
			reader.Close();
			stream.Close();
			return text;
		}
		TextAsset asset = Resources.Load<TextAsset>(sheetName);
		if (asset != null)
		{
			text = asset.text;
		}
		return text;
	}

	public static string GetDocumentPath()
	{
		string persistentDataPath = Application.persistentDataPath;
		if (Application.platform == RuntimePlatform.Android)
		{
			return (Application.persistentDataPath.Replace(".apk", "/") + "/");
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return (Application.persistentDataPath + "/");
		}
		if ((!Application.isEditor && (Application.platform != RuntimePlatform.OSXPlayer)) && (Application.platform != RuntimePlatform.WindowsPlayer))
		{
			return persistentDataPath;
		}
		return string.Empty;
	}
	
	private static string GetHtmlFromUri(string resource)
	{
		string html = string.Empty;
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
		req.Timeout = 3; //3 secoonds
		try
		{
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
				if (isSuccess)
				{
					using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
					{
						//We are limiting the array to 80 so we don't have
						//to parse the entire html document feel free to 
						//adjust (probably stay under 300)
						char[] cs = new char[80];
						reader.Read(cs, 0, cs.Length);
						foreach(char ch in cs)
						{
							html +=ch;
						}
					}
				}
			}
		}
		catch
		{
			return "";
		}
		return html;
	}
	
	public static bool CheckConnection()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
			return false;
		var htmlText = GetHtmlFromUri("http://google.com");
		return htmlText != "";
	}
}
