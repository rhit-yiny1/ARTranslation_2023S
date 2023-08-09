using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;
using System.Web;
using System.Net;
using System;
using System.Text;
using System.Text.RegularExpressions;

public class WebCamTextureToCloudVision : MonoBehaviour
{

	public string url = "https://vision.googleapis.com/v1/images:annotate?key=";
	public string apiKey = "AIzaSyB2uoWjdfv6LxtOyvoTxRLCBV94STh7IjA"; //Put your google cloud vision api key here
	public float captureIntervalSeconds = 1f; //Capture pictures each second
	public int requestedWidth = 640;
	public int requestedHeight = 480; //height and width of the captured picture
	public FeatureType featureType = FeatureType.TEXT_DETECTION;
	public int maxResults = 10;
	//public GameObject resPanel;
	//public Text responseText, responseArray;
	public textScriptV1 textCreator;


	WebCamTexture webcamTexture;
	Texture2D texture2D; //transformed pictures' data
	Dictionary<string, string> headers; //storing HTTP headers

	[System.Serializable] //新加入类
	public class TextLineInfo
	{
		public string text;
		public Vector2 topLeft;     // 左上角坐标
		public Vector2 topRight;
		public Vector2 bottomLeft;
		public Vector2 bottomRight;
		public bool containsChinese;
		public string translatedText;

		public TextLineInfo(string text, Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
		{
			this.text = text;
			this.topLeft = topLeft;
			this.topRight = topRight;
			this.bottomLeft = bottomLeft;
			this.bottomRight = bottomRight;
			containsChinese = false;
			translatedText = "";
		}
	}

	[System.Serializable]
	public class AnnotateImageRequests
	{
		public List<AnnotateImageRequest> requests;
	}

	[System.Serializable]
	public class AnnotateImageRequest
	{
		public Image image;
		public List<Feature> features;
	}

	[System.Serializable]
	public class Image
	{
		public string content;
	}

	[System.Serializable]
	public class Feature
	{
		public string type;
		public int maxResults;
	}

	public enum FeatureType
	{
		TYPE_UNSPECIFIED,
		FACE_DETECTION,
		LANDMARK_DETECTION,
		LOGO_DETECTION,
		LABEL_DETECTION,
		TEXT_DETECTION, //using text detection in this script
		SAFE_SEARCH_DETECTION,
		IMAGE_PROPERTIES
	}

	// Use this for initialization
	void Start()
	{
		Debug.Log("From: " + menuScript.fromLanguage + " " + "To: " + menuScript.toLanguage);
		Texture2D texture = new Texture2D(128, 128);
		GetComponent<Renderer>().material.mainTexture = texture;

		for (int y = 0; y < texture.height; y++)
		{
			for (int x = 0; x < texture.width; x++)
			{
				Color color = ((x & y) != 0 ? Color.white : Color.gray);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();

		headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json; charset=UTF-8");

		if (apiKey == null || apiKey == "")
			Debug.LogError("No API key. Please set your API key into the \"Web Cam Texture To Cloud Vision(Script)\" component.");

		WebCamDevice[] devices = WebCamTexture.devices;
		for (var i = 0; i < devices.Length; i++)
		{
			Debug.Log(devices[i].name);
		}
		if (devices.Length > 0)
		{
			webcamTexture = new WebCamTexture(devices[0].name, requestedWidth, requestedHeight);
			Renderer r = GetComponent<Renderer>();
			if (r != null)
			{
				Material m = r.material;
				if (m != null)
				{
					m.mainTexture = webcamTexture;
				}
			}
			webcamTexture.Play();
			StartCoroutine("Capture");
		}
	}

	// Update is called once per frame
	void Update(){}

	// Setting the range of language detection to CJK.
	private static readonly Regex cjkCharRegex = new Regex(@"\p{IsCJKUnifiedIdeographs}");
	public static bool GetUnicodeString(char c)
	{
		return cjkCharRegex.IsMatch(c.ToString());
	}

	// Detect whether the input contains pure/part CJK characters or not.
	public bool containsCJK(string s)
	{
		foreach (char c in s)
		{
			if (GetUnicodeString(c))
			{
				return true;
			}
		}
		return false;
	}

	public string translate(string input, string from, string to)
	{
		var fromLanguage = from;
		var toLanguage = to;
		var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(input)}";
		var webclient = new WebClient
		{
			Encoding = System.Text.Encoding.UTF8
		};
		var result = webclient.DownloadString(url);
		try
		{
			result = result.Substring(4, result.IndexOf("\"", 4
				, StringComparison.Ordinal) - 4);
			return result;
		}
		catch (Exception e1)
		{
			return "error";
		}
	}

	private IEnumerator Capture()
	{
		while (true)
		{
			if (this.apiKey == null)
				yield return null;

			yield return new WaitForSeconds(captureIntervalSeconds);

			Color[] pixels = webcamTexture.GetPixels();
			if (pixels.Length == 0)
				yield return null;
			if (texture2D == null || webcamTexture.width != texture2D.width || webcamTexture.height != texture2D.height)
			{
				texture2D = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
			}

			texture2D.SetPixels(pixels);
			// texture2D.Apply(false); // Not required. Because we do not need to be uploaded it to GPU
			byte[] jpg = texture2D.EncodeToJPG();
			string base64 = System.Convert.ToBase64String(jpg);
			// #if UNITY_WEBGL	
			// 			Application.ExternalCall("post", this.gameObject.name, "OnSuccessFromBrowser", "OnErrorFromBrowser", this.url + this.apiKey, base64, this.featureType.ToString(), this.maxResults);
			// #else

			AnnotateImageRequests requests = new AnnotateImageRequests();
			requests.requests = new List<AnnotateImageRequest>();

			AnnotateImageRequest request = new AnnotateImageRequest();
			request.image = new Image();
			request.image.content = base64;
			request.features = new List<Feature>();
			Feature feature = new Feature();
			feature.type = this.featureType.ToString();
			feature.maxResults = this.maxResults;
			request.features.Add(feature);
			requests.requests.Add(request);

			string jsonData = JsonUtility.ToJson(requests, false);
			if (jsonData != string.Empty)
			{
				string url = this.url + this.apiKey;
				byte[] postData = System.Text.Encoding.Default.GetBytes(jsonData);
				using (WWW www = new WWW(url, postData, headers))
				{
					yield return www;
					if (string.IsNullOrEmpty(www.error))
					{
						string responses = www.text.Replace("\n", "").Replace(" ", "");
						JSONNode res = JSON.Parse(responses);
						string fullText = res["responses"][0]["textAnnotations"][0]["description"].ToString().Trim('"');

						//此行新加入
						JSONArray textAnnotations = res["responses"][0]["textAnnotations"].AsArray;

						if (fullText != "")
						{
							fullText = fullText.Replace("\\n", ";");
							string[] texts = fullText.Split(';');
							List<TextLineInfo> textLineInfos = new List<TextLineInfo>(); // 创建一个空的List<TextLineInfo>，用于保存每个单词和边界框信息

							fullText = res["responses"][0]["textAnnotations"][0]["description"].ToString().Trim('"');
							string[] textLines = fullText.Split(new string[] { "\\n" }, StringSplitOptions.None);

							for (int i = 1; i < textAnnotations.Count; i++)
							{
								JSONNode textAnnotation = textAnnotations[i];
								string text = textAnnotation["description"]; // 获取每行文本

								// 获取每行文本的坐标信息
								JSONNode boundingPolyNode = textAnnotation["boundingPoly"]["vertices"];
								Vector2 topLeft = new Vector2(boundingPolyNode[0]["x"].AsFloat, boundingPolyNode[0]["y"].AsFloat);
								Vector2 topRight = new Vector2(boundingPolyNode[1]["x"].AsFloat, boundingPolyNode[1]["y"].AsFloat);
								Vector2 bottomLeft = new Vector2(boundingPolyNode[3]["x"].AsFloat, boundingPolyNode[3]["y"].AsFloat);
								Vector2 bottomRight = new Vector2(boundingPolyNode[2]["x"].AsFloat, boundingPolyNode[2]["y"].AsFloat);

								// 创建一个TextLineInfo对象，保存文本信息和坐标信息
								TextLineInfo lineInfo = new TextLineInfo(text, topLeft, topRight, bottomLeft, bottomRight);
								lineInfo.containsChinese = containsCJK(text);
								if (lineInfo.containsChinese)
								{
									lineInfo.translatedText = translate(text, InputFieldScript.inputText, OutputFieldScript.inputText);
								}

								textLineInfos.Add(lineInfo);
							}
							List<TextLineInfo> newTextLineInfos = new List<TextLineInfo>(); // 创建一个空的List<TextLineInfo>，用于保存每行文本和边界框信息
							textCreator.ClearAllTextOnCanvas();
							int aIndex = 0; // 字符串数组texts的下标
							int bIndex = 0; // 字符串数组textLineInfos[].text的下标
							while (aIndex < textLines.Length && bIndex < textLineInfos.Count)
							{
								if (textLines[aIndex] == textLineInfos[bIndex].text)
								{
									// 字符串匹配，记录下标
									TextLineInfo lineInfo = new TextLineInfo(textLineInfos[bIndex].text, textLineInfos[bIndex].topLeft, textLineInfos[bIndex].topRight, textLineInfos[bIndex].bottomLeft, textLineInfos[bIndex].bottomRight);
									lineInfo.containsChinese = textLineInfos[bIndex].containsChinese;
									lineInfo.translatedText = textLineInfos[bIndex].translatedText;
									newTextLineInfos.Add(lineInfo);

									aIndex++;
									bIndex++;
								}
								else
								{
									// 字符串不匹配，尝试合并B中的下一个字符串
									string mergedString = textLineInfos[bIndex].text;
									int tempBIndex = bIndex + 1;
									int originIndex = bIndex;

									while (tempBIndex < textLineInfos.Count)
									{
										mergedString += textLineInfos[tempBIndex].text;

										if (textLines[aIndex] == mergedString)
										{
											// 合并后的字符串匹配，记录下标
											TextLineInfo lineInfo = new TextLineInfo(mergedString, textLineInfos[originIndex].topLeft, textLineInfos[tempBIndex].topRight, textLineInfos[originIndex].bottomLeft, textLineInfos[tempBIndex].bottomRight);
											lineInfo.containsChinese = containsCJK(mergedString);
											if (lineInfo.containsChinese)
											{
												lineInfo.translatedText = translate(mergedString, InputFieldScript.inputText, OutputFieldScript.inputText);
											}
											newTextLineInfos.Add(lineInfo);
											aIndex++;
											bIndex = tempBIndex + 1;
											break;
										}

										tempBIndex++;
									}

									if (tempBIndex == textLineInfos.Count)
									{
										Debug.Log("Failed to match A[" + aIndex + "]");
										aIndex++;
										bIndex = bIndex + 1;
									}
								}
							}
							if(textLineInfos.Count == 0){
								textCreator.ClearAllTextOnCanvas();
							}

							foreach (TextLineInfo lineInfo in newTextLineInfos)
							{
								float x = lineInfo.topLeft.x;
								float y = lineInfo.topLeft.y;
								if(lineInfo.topLeft.x > 0 && lineInfo.topLeft.y > 0){
									x = lineInfo.topLeft.x - 160;
									y = lineInfo.topLeft.y + 60;
								}else if(lineInfo.topLeft.x > 0 && lineInfo.topLeft.y < 0){
									x = lineInfo.topLeft.x - 160;
									y = lineInfo.topLeft.y - 60;
								}else if(lineInfo.topLeft.x < 0 && lineInfo.topLeft.y > 0){
									x = lineInfo.topLeft.x + 160;
									y = lineInfo.topLeft.y + 60;
								}else if(lineInfo.topLeft.x < 0 && lineInfo.topLeft.y < 0){
									x = lineInfo.topLeft.x + 160;
									y = lineInfo.topLeft.y - 60;
								}
								textCreator.CreateTextOnCanvas(lineInfo.translatedText, x, -y+300);
							}
						}
					}
					else
					{
						Debug.Log("Error: " + www.error);
					}
				}
			}
			// #endif
		}
	}

#if UNITY_WEBGL
	void OnSuccessFromBrowser(string jsonString) {
		Debug.Log(jsonString);	
	}

	void OnErrorFromBrowser(string jsonString) {
		Debug.Log(jsonString);	
	}
#endif

}
