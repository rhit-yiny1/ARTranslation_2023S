# AR Translation

A Unity/C# augmented-reality prototype for real-time camera text translation. The application captures camera input, uses Google Cloud Vision for text recognition, translates detected text, and displays translated text within the Unity scene.

Developed as part of a 2023 summer research project at Shanghai Jiao Tong University.

## Overview

The project combines computer vision, machine translation, and augmented-reality rendering in a Unity application.

The main workflow is:

1. Capture frames from the camera in Unity.
2. Send image data to Google Cloud Vision for text detection.
3. Process detected text, including CJK text handling.
4. Translate detected content.
5. Display translated text within the Unity scene.

## Technologies

- **Unity**
- **C#**
- **Google Cloud Vision**
- **Google Translate**
- **MySQL**

## Project Contributions

Built as a three-person research project at Shanghai Jiao Tong University.

The project extended an existing Google Cloud Vision Unity OCR demo with additional functionality including:

- CJK-aware text processing
- Translation workflows
- AR text placement and UI
- Application-level Unity integration
- Translation-data persistence

## Research Publication

This project led to the following publication:

**Youding Yin, Guanzheng Liu, and Shiqi Zhang**  
*Augmented Reality Text Translation: A Unity-Based Real-Time Approach*  
June 2024

**DOI:** https://doi.org/10.61173/60910s92

## Attribution

The initial Google Cloud Vision OCR integration was based on the following open-source Unity demo:

https://github.com/codemaker2015/google-cloud-vision-api-ocr-unity3d-demo

This repository contains the research team's extensions and application implementation built on top of that baseline.

## Contributors

- Youding Yin
- Guanzheng Liu
- Shiqi Zhang

## License

This project is licensed under the MIT License. See [`LICENSE`](LICENSE) for details.
