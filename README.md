# GlanceAR
 
## AR news reader built with Unity, Vuforia SDK, and NewsAPI integration
 
> A mobile AR application that displays real-time news headlines when you scan a target image. Built to explore marker-based AR tracking, RESTful API integration in Unity, and designing mobile UIs optimized for spatial computing.
 
---
 
## Why I Built This
 
During a 4-week AR/VR training program at my college, I wanted to move beyond typical "scan and display 3D object" demos. I was curious whether AR could serve a practical, everyday purpose rather than just novelty—news consumption seemed like the right test case. The challenge wasn't just learning Unity and Vuforia fundamentals, but integrating a live data source (NewsAPI) and designing a UI that felt natural in AR space.
 
I took ownership of the UI/UX design, application workflow architecture, and API integration testing to ensure the data pipeline worked reliably across different scenarios.
 
---
 
## What It Does
 
- **Image target detection**: Point your phone camera at a specific marker image to activate the AR experience
- **Live news fetching**: Pulls top headlines from NewsAPI in real-time using asynchronous HTTP requests
- **Category filtering**: Switch between All, Technology, Business, and Sports categories with a single tap
- **Card-based layout**: Displays 3 news cards showing headline, source, and timestamp
- **Manual refresh**: Fetch the latest headlines on-demand without rescanning the target
- **Smooth transitions**: Animated splash screen and fade-in effects when content loads
 
---
 
## Demo
 
<!-- Screenshots will go here -->
![Splash Screen](docs/splash.png)
*Animated splash screen with college branding*
 
![AR View](docs/ar-view.png)
*News cards anchored to target image with category filters*
 
![Categories](docs/categories.png)
*Switching between news categories*
 
> **Note**: Add screenshots to a `docs/` folder in your repo
 
---
 
## Tech Stack
 
| Layer | Technology | Why I Chose It |
|---|---|---|
| **Game Engine** | Unity 6.0 | Industry standard for AR development with extensive documentation |
| **AR Framework** | Vuforia SDK 11.4.4 | Robust marker-based tracking; easier learning curve than ARCore/ARKit |
| **Language** | C# | Native to Unity; strong typing helped catch bugs during API integration |
| **API** | NewsAPI (REST) | Free tier with good coverage; simple JSON responses |
| **JSON Parser** | JsonUtility | Unity's built-in parser — no external dependencies needed |
| **Platform** | Android | Simpler build pipeline for testing; most team members had Android devices |
 
---
 
## What I Learned
 
**Asynchronous operations in Unity** — Initially tried using regular C# `async/await` patterns and hit threading issues because Unity's main thread doesn't like being blocked. Switched to `UnityWebRequest` with coroutines, which taught me the difference between general C# async patterns and Unity's specific execution model.
 
**AR UI design constraints** — Spent two days fighting with Canvas placement before realizing that UI elements in "World Space" mode need completely different scale/positioning than standard mobile UIs. Had to rethink sizing (using meters instead of pixels) and test readability at different distances from the target.
 
**API rate limiting and caching** — NewsAPI has a 100 requests/day limit on the free tier. Learned this the hard way when the app stopped working during testing. Realized I needed to cache responses instead of fetching on every category switch. This taught me the importance of understanding API quotas before building dependencies around them.
 
**JSON deserialization edge cases** — NewsAPI sometimes returns `null` for `urlToImage` or incomplete data. My app crashed initially because I didn't account for optional fields. Learned to use nullable types in C# data models and always validate API responses before trying to render them.
 
**State management across scenes** — Splash screen and AR scene needed to share data (like whether user had already seen the intro). Discovered `DontDestroyOnLoad()` and singleton patterns for persisting game objects across scene transitions.
 
---
 
## Challenges & How I Solved Them
 
### Cards not staying anchored to target image
The news cards would drift or disappear when I moved the camera slightly. Realized I had parented the Canvas to the wrong game object — it was attached to ARCamera instead of ImageTarget. Moving it under the Vuforia ImageTarget's hierarchy fixed the anchoring immediately. This taught me the importance of understanding Unity's parent-child transform relationships in AR contexts.
 
### API calls blocking the main thread
When fetching news, the entire app would freeze for 1-2 seconds. Tried using `UnityWebRequest.Get()` directly in `Update()`, which was a mistake. Switched to coroutines with `yield return` to allow Unity to continue rendering frames while waiting for the HTTP response. Added a simple loading state (though not visible in UI yet) to track request status.
 
### Category buttons not updating UI
Clicking category buttons would fetch new data, but the cards showed old headlines. The issue was I was updating the data model but not calling the UI refresh method. Learned to separate data fetching logic from UI update logic — now `FetchNews()` updates the model, and a separate `UpdateCardDisplay()` method handles rendering. This also made debugging easier since I could test API calls independently from UI rendering.
 
---
 
## Getting Started
 
### Prerequisites
- Unity Hub with Unity 6.0 installed
- Android device with USB debugging enabled (or use Unity's device simulator)
- NewsAPI key (free tier available at [newsapi.org](https://newsapi.org))
 
### Installation
 
```bash
git clone https://github.com/yourusername/GlanceAR.git
cd GlanceAR
```
 
1. Open Unity Hub and click **"Add"** → select the `GlanceAR` folder
2. Open the project (Unity will import Vuforia SDK and dependencies automatically)
 
### Configuration
 
**Add your NewsAPI key:**
1. Navigate to `Assets/Scripts/NewsManager.cs` (or similar)
2. Find the line: `string apiKey = "PLACEHOLDER";`
3. Replace with your key: `string apiKey = "your_actual_key_here";`
 
**Vuforia setup** (if needed):
- The project includes a pre-configured Vuforia license and target database
- If you want to use your own target image:
  1. Go to `Assets/Resources/VuforiaConfiguration`
  2. Import your own image database from Vuforia Developer Portal
 
### Build and Run
 
**For Android:**
```
File > Build Settings > Select Android > Build and Run
```
Connect your phone via USB, ensure developer mode is on, and Unity will deploy the APK.
 
**Target image:** Located in `Assets/TargetImages/` — print this or display on another screen to test the AR detection.
 
---
 
## What I'd Do Differently
 
**Better error handling from the start** — Currently the app just freezes or shows nothing if the API call fails or returns unexpected data. Should have implemented try-catch blocks around JSON parsing and shown user-friendly error messages ("Connection lost — showing cached headlines") instead of silent failures.
 
**Modular architecture** — Everything is in a few monolithic scripts. Would separate concerns better: one script for API calls (`NewsService.cs`), one for UI updates (`UIController.cs`), one for AR state management (`ARManager.cs`). This would make testing individual components way easier.
 
**Implement local caching properly** — Right now there's no offline mode. Would use Unity's `PlayerPrefs` or a simple JSON file to cache the last successful API response, so users could still see headlines even without internet. This would also help with API rate limits.
 
**Design for multiple target images** — The app only works with one hardcoded target. Would generalize the system to support multiple targets triggering different news categories (e.g., tech target shows tech news, sports target shows sports news). This would make the AR feel more contextual and less like a gimmick.
 
**Add proper loading states** — The UI freezes briefly when fetching news with no visual feedback. Should have added a spinner or skeleton cards during the loading phase. It's a small detail but makes the app feel way more polished.
 
---
 
## My Contributions & Skills Demonstrated
 
### Technical Skills Applied
 
**Mobile AR Development**  
Image target-based tracking and spatial UI rendering with Vuforia SDK; understanding of marker detection pipelines and AR coordinate systems
 
**RESTful API Integration**  
Asynchronous HTTP requests using `UnityWebRequest` with coroutines; JSON parsing with `JsonUtility`; handling rate limits and response validation
 
**State Management**  
Managing dynamic content across multiple UI components; implementing data flow between API responses and card displays; scene persistence with `DontDestroyOnLoad()`
 
**Unity UI System**  
Canvas-based responsive layouts in World Space mode; adapting mobile UI patterns for AR contexts; optimizing UI rendering for different screen sizes
 
**C# Programming**  
Event-driven architecture for button interactions; coroutine-based async patterns; nullable types for data validation; object-oriented design with scriptable components
 
**Cross-Platform Development**  
Android build pipeline configuration; APK deployment and device testing; debugging on physical hardware vs. Unity simulator
 
### Individual Contributions (Team Project)
 
**UI/UX Design**  
- Designed the card-based interface with visual hierarchy (headline → source → timestamp)
- Created category navigation system with clear active/inactive button states  
- Implemented spacing, typography, and color scheme for readability in AR
- Designed splash screen animation and camera transition flow
 
**Workflow Architecture**  
- Structured complete user journey: splash screen → camera initialization → AR detection → content display
- Defined scene transitions and state persistence logic
- Organized GameObject hierarchy for Canvas, buttons, and news cards
- Established data flow pattern from API call → parsing → UI update
 
**API Integration Testing & Debugging**  
- Validated NewsAPI responses across different categories and edge cases
- Debugged JSON parsing errors when API returned `null` or incomplete data
- Ensured data consistency when switching between category filters
- Tested error scenarios (no internet, API timeout, rate limit exceeded)
- Verified correct headline display after refresh and category changes
 
### Training Program Outcomes
 
**AR/VR Technology Landscape**  
Gained foundational understanding of marker-based vs. markerless tracking, SLAM algorithms, spatial computing concepts, and industry applications (retail, education, industrial training)
 
**Unity Development Workflows**  
Hands-on experience with Unity editor, scene management, prefab systems, component-based architecture, and C# scripting for game objects
 
**Collaborative Development**  
Worked in a team environment with clear role division; practiced version control (Git); conducted peer code reviews; integrated individual components into unified build
 
**Problem-Solving in Emerging Tech**  
Navigated limited documentation for Unity-Vuforia integration; debugged platform-specific issues (Android permissions, camera access); adapted general programming patterns to AR-specific constraints
 
---
 
## Project Structure
 
```
GlanceAR/
├── Assets/
│   ├── Scenes/
│   │   ├── SplashScene.unity          # Initial loading screen
│   │   └── SampleScene.unity          # Main AR experience
│   ├── Scripts/
│   │   ├── NewsManager.cs             # API calls and data handling
│   │   ├── CategoryButton.cs          # Category filter logic
│   │   └── SplashController.cs        # Splash animations
│   ├── Resources/
│   │   └── VuforiaConfiguration       # AR tracking settings
│   ├── StreamingAssets/
│   │   └── Vuforia/                   # Target image database
│   └── TargetImages/                  # Printable AR markers
└── README.md
```
 
---
 
## Acknowledgments
 
Built during a 4-week AR/VR training program organized by my college. Thanks to the training instructors for Unity and Vuforia guidance, project team members for collaborative development, and NewsAPI for providing free tier access.
 
---
 
**Want to contribute or report issues?** Open an issue or pull request on this repo.