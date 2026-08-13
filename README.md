# GlanceAR ![College Project](https://img.shields.io/badge/type-College%20Project-purple)
 
## AR news reader built with Unity and Vuforia
 
> A mobile AR app that shows real-time news headlines when you scan a target image. Built as a final submission task during a 4-week college AR/VR training program.
 
---
 
## Why We Built This
 
At the end of a 4-week AR/VR training program at our college, our team was given a final submission task: build a working AR application that goes beyond the usual "scan and see a 3D model" demos. We were asked to come up with something practical, so we landed on an AR news reader — a real use case that felt relevant and achievable within the scope of the program.
 
As part of the team, I focused on designing the UI, planning the user flow, and testing to make sure everything worked smoothly.
 
---
 
## What It Does
 
- Scan a target image with your phone camera to activate the AR experience
- Displays 3 news headlines with source and timestamp
- Filter news by category: All, Tech, Business, or Sports
- Refresh button to get the latest headlines
- Smooth splash screen animation on startup
 
---
 
## Demo

### Screenshots

<table align="center">
  <tr>
    <td align="center">
      <img src="docs/splash.png" alt="Splash Screen" width="200" height="350"><br>
      <sub><b>Splash Screen</b> — App startup animation</sub>
    </td>
    <td align="center">
      <img src="docs/ar-view.png" alt="AR View" width="200" height="350"><br>
      <sub><b>AR View</b> — Live news headlines overlaid on the target image</sub>
    </td>
  </tr>
  <tr>
    <td align="center" colspan="2">
      <img src="docs/categories.png" alt="Category Filters" width="300" height="200"><br>
      <sub><b>Category Filters</b> — Switch between All, Tech, Business, and Sports</sub>
    </td>
  </tr>
</table>

<p align="center">
  <em>Figure 1: GlanceAR screenshots — splash screen, AR view with live headlines, and category filter switching</em>
</p>

### Working demo

<div align="center">
  <video src="https://github.com/user-attachments/assets/18f31b82-98b5-46fb-b5bc-5dad4cb1b7fd" width="200" height="300" controls></video>
  <br/>
  <em>Working demo of GlanceAR — scan, AR view, and category filtering in action</em>
</div>

---
 
## Tech Stack

<div align="center">

<table width="100%" style="text-align: center; border-collapse: collapse;">
  <thead>
    <tr style="border-bottom: 2px solid #ccc;">
      <th style="padding: 10px;">What</th>
      <th style="padding: 10px;">Technology</th>
    </tr>
  </thead>
  <tbody>
    <tr style="border-bottom: 1px solid #ddd;">
      <td style="padding: 10px;"><b>Game Engine</b></td>
      <td style="padding: 10px;">Unity 6.0</td>
    </tr>
    <tr style="border-bottom: 1px solid #ddd;">
      <td style="padding: 10px;"><b>AR Framework</b></td>
      <td style="padding: 10px;">Vuforia SDK 11.4.4</td>
    </tr>
    <tr style="border-bottom: 1px solid #ddd;">
      <td style="padding: 10px;"><b>Language</b></td>
      <td style="padding: 10px;">C#</td>
    </tr>
    <tr style="border-bottom: 1px solid #ddd;">
      <td style="padding: 10px;"><b>API</b></td>
      <td style="padding: 10px;">NewsAPI</td>
    </tr>
    <tr style="border-bottom: 1px solid #ddd;">
      <td style="padding: 10px;"><b>Platform</b></td>
      <td style="padding: 10px;">Android</td>
    </tr>
  </tbody>
</table>

<p style="margin-top: 10px;"><b>Table 1. Technology stack.</b></p>

</div>
 
---

## Project Files

```
glanceAR/
├── Assets/
│   ├── Scenes/
│   │   └── MainScene.unity            # Main AR + splash scene
│   ├── Scripts/
│   │   ├── NewsAPIHandler.cs          # Fetches headlines from NewsAPI
│   │   ├── NewsDisplay.cs             # Renders news cards in AR
│   │   ├── CategoryManager.cs         # Category filter logic
│   │   ├── SplashAnimator.cs          # Splash screen animation
│   │   ├── SplashManager.cs           # Splash lifecycle control
│   │   ├── ContentAnimator.cs         # Card animation effects
│   │   ├── ButtonPressEffect.cs       # Button press feedback
│   │   ├── RefreshButtonController.cs # Refresh button handling
│   │   ├── SpinnerRotate.cs           # Loading spinner
│   │   └── ExitApp.cs                 # App exit handling
│   ├── Images/                        # UI icons and assets
│   ├── Resources/                     # Vuforia configuration
│   └── StreamingAssets/Vuforia/       # AR target database
├── Packages/                          # Unity package manifest
├── ProjectSettings/                   # Build and player settings
├── docs/                              # README screenshots
└── README.md
```

---
 
## How to Run This

### You'll Need
- Unity Hub with Unity 6.0
- An Android phone with USB debugging enabled
- A free NewsAPI key from [newsapi.org](https://newsapi.org)
- A free Vuforia license key from [developer.vuforia.com](https://developer.vuforia.com)

### Setup

```bash
git clone https://github.com/harsh-space/glanceAR.git
```

1. Open **Unity Hub** → **Add project from disk** → select the `glanceAR` folder
2. Open the project — Unity will auto-import packages from `Packages/manifest.json`
3. Open `Assets/Scenes/MainScene.unity`

**Add your NewsAPI key:**
- In the Hierarchy, select the GameObject that has `NewsAPIHandler` attached
- In the Inspector, paste your key into the **Api Key** field

**Add your Vuforia license key:**
- Go to **Window → Vuforia Configuration**
- Paste your key into the **App License Key** field

**Build for Android:**
- File → Build Settings → Switch Platform to **Android** → Build and Run
- Connect your phone via USB with USB debugging enabled

**Target image:** The AR database is already included — no setup needed. Just print or display this image to trigger the AR view:

<p align="center">
  <img src="docs/target.jpg" alt="AR Target Image" width="250"/>
  <br/>
  <em>Figure 2: AR target image — point your phone camera at this to activate the AR experience</em>
</p>
 
---

## My Role in This Project

Within the team, I contributed across two areas:

**UI Design & User Flow**
- Designed the news card layout and category filter buttons
- Defined the end-to-end user flow from splash screen to AR view, focusing on clarity and minimal interactions

**QA & Testing**
- Tested edge cases including no internet, slow connections, and varying AR tracking distances
- Identified and helped resolve issues with data rendering and UI state updates

---

## Challenges I Faced
 
**Cards drifting from the target**  
The news cards weren't staying in place. Found out I had attached them to the wrong parent object. Moving them to the ImageTarget fixed it.
 
**Category buttons not updating content**  
The app was fetching new data but not showing it on screen. Separated the data fetching code from the UI update code, which made debugging much easier.

---

## What I'd Improve
 
- **Error messages** — Currently nothing shows if the API fails. Would add friendly error messages.
- **Better code organization** — Would split the code into separate files for API calls, UI updates, and AR management.
- **Offline mode** — Would cache headlines so the app works without internet.
- **Multiple targets** — Would make different images trigger different news categories.
- **Loading indicator** — Would add a spinner while fetching news instead of just freezing.