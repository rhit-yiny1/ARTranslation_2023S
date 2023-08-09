# ARTranslation_2023S
A translation tool based on Google Cloud Vision API in Unity.

The code base is largely based on this demo project here: https://github.com/codemaker2015/google-cloud-vision-api-ocr-unity3d-demo.

The project takes from the camera and recognizes the potential texts based on a pre-trained ML model provided by Google Cloud Vision. It is then able to identify all the CJK (Chinese, Japanese, Korean) characters and consider words that have them as one CJK word. The word is then transferred to Google Translate Api and return a JSON response. 

TO USE:
You need to have a Unity Editor with version 2019.4.17f1 to run the application without potential crashes. It is recommended that you get it from Unity's Official Website. 
Clone and current repo and put it in a local file location. Open the cloned folder with Unity Hub Editor 2019.4.17f1 to run the application. 

At the moment, the application is only able to identify and translate CJK words and characters due to the fact that the initial goal of the application was to only translate CJK words and characters but it is possible to modify the scripts so that all languages can be identified and translated. 

A warm thanks to the dev team. 
