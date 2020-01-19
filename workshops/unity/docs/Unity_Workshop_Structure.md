V Title: Unity Workshop Structure  
Authors: Adamopoulos George   
Zisch Dominik  

Unity Workshop Structure
========================

Basics
------------------------
### Basic Unity Ontology
* GameObjects
* Components
* Transforms
* The MonoBehaviour  Class
* …

### Programming Basics
#### Stuff you should already know
* Variables
* Functions
* Classes
* Loops
* Conditionals
* …

#### Stuff you should know but probably don't
* Lists Vs Arrays in C#
* Access modifiers (public, private, protected etc)
* Inheritance (abstract, virtual, sealed, partial etc)
* Value types vs Reference types & the **new** keyword
* …

### How to fix things  

#### How to use the Console & read errors
[Link To Unity Documentation](https://docs.unity3d.com/Manual/Console.html)  

#### How to set up breakpoints  
[Link To Unity Documentation](https://docs.unity3d.com/Manual/ManagedCodeDebugging.html)  

#### How to use the Profiler  
[Link To Unity Documentation](https://docs.unity3d.com/Manual/ProfilerWindow.html)  

#### How to use the adb logcat Package to debug the Quest  
[Link to Unity Documentation](https://docs.unity3d.com/Packages/com.unity.mobile.android-logcat@0.1/manual/index.html)  

#### How to profile your built application
[Link to Unity Documentation](https://docs.unity3d.com/Manual/profiler-profiling-applications.html)  

#### How to Google
???

VR
------------------------
### Intro to VR
* Technologies
* Guidelines for good VR experiences
---
### Oculus

#### [Oculus Unity Getting Started Guide](https://developer.oculus.com/documentation/unity/book-unity-gsg/)  

  * [Compatibility & Version Requirements](https://developer.oculus.com/documentation/unity/unity-req/)  
  * [Preparing for Android Development](https://developer.oculus.com/documentation/unity/unity-mobileprep/)  
  * [Importing the Oculus Utilities Package](https://developer.oculus.com/documentation/unity/unity-import/)  
  * [Building Android Applications](https://developer.oculus.com/documentation/unity/unity-build-android/)  
  * [Unity Build Tools](https://developer.oculus.com/documentation/unity/unity-build-android-tools/)  
  * [Tutorial: Build Your First VR App](https://developer.oculus.com/documentation/unity/unity-tutorial/)  
  * [Other Resources](https://developer.oculus.com/documentation/unity/unity-resources/)

#### The OVR namespace  
[Link To Oculus Scripting Reference Documentation](https://developer.oculus.com/reference/unity/1.43/)  

#### User Input in Oculus
##### [OVRInput](https://developer.oculus.com/documentation/unity/unity-ovrinput/)
##### [OVRBoundary](https://developer.oculus.com/documentation/unity/unity-ovrboundary/)
##### [Hand Tracking](https://developer.oculus.com/documentation/unity/unity-handtracking/)
##### [Haptics](https://developer.oculus.com/documentation/unity/unity-haptics/)

---

### Steam VR

#### [SteamVR Unity Getting Started Guide](https://valvesoftware.github.io/steamvr_unity_plugin/articles/intro.html)  

  * [Quickstart](https://valvesoftware.github.io/steamvr_unity_plugin/articles/Quickstart.html)  
  * [Render Models](https://valvesoftware.github.io/steamvr_unity_plugin/articles/Render-Models.html)    
  * [SteamVR Input](https://valvesoftware.github.io/steamvr_unity_plugin/articles/SteamVR-Input.html)    
  * [Skeleton Input](https://valvesoftware.github.io/steamvr_unity_plugin/articles/Skeleton-Input.html)  
  * [Interaction System](https://valvesoftware.github.io/steamvr_unity_plugin/articles/Interaction-System.html)  
  * [Skeleton Poser](https://valvesoftware.github.io/steamvr_unity_plugin/articles/Skeleton-Poser.html)  

#### The SteamVR namespace  
[Link to SteamVR Scipting Reference Documentation](https://valvesoftware.github.io/steamvr_unity_plugin/api/index.html)

#### User Input In SteamVR  
##### [Skeleton Poser](https://valvesoftware.github.io/steamvr_unity_plugin/tutorials/Skeleton-Poser.html)  
* [Introduction](https://valvesoftware.github.io/steamvr_unity_plugin/tutorials/Skeleton-Poser.html#introduction)  
* [Pose Editor](https://valvesoftware.github.io/steamvr_unity_plugin/tutorials/Skeleton-Poser.html#pose-editor)  
* [Blending Editor](https://valvesoftware.github.io/steamvr_unity_plugin/tutorials/Skeleton-Poser.html#blending-editor)  
* [Manual Behaviour](https://valvesoftware.github.io/steamvr_unity_plugin/tutorials/Skeleton-Poser.html#manual-behaviours)  
* [Scaling](https://valvesoftware.github.io/steamvr_unity_plugin/tutorials/Skeleton-Poser.html#scaling)  
* [Conclusion](https://valvesoftware.github.io/steamvr_unity_plugin/tutorials/Skeleton-Poser.html#conclusion)  

##### [SteamVR Input](https://valvesoftware.github.io/steamvr_unity_plugin/tutorials/SteamVR-Input.html)
<ul class="level1 nav bs-docs-sidenav"><li><a href="#overview">Overview</a></li><li><a href="#video-version">Video version</a></li><li><a href="#downloading-the-plugin">Downloading the Plugin</a></li><li><a href="#steamvr-interaction-system">SteamVR Interaction System</a></li><li><a href="#the-steamvr-input-window">The SteamVR Input window</a></li><li><a href="#testing">Testing</a></li><li><a href="#adding-an-action">Adding an action</a></li><li><a href="#binding-actions">Binding actions</a></li><li><a href="#assigning-actions-in-the-editor">Assigning actions in the editor</a></li><li><a href="#working-with-actions-in-the-code">Working with actions in the code</a></li><li><a href="#working-with-actions-in-the-editor">Working with actions in the Editor</a></li><li><a href="#troubleshooting-actions">Troubleshooting actions</a></li><li><a href="#creating-a-build">Creating a build</a></li></ul>
---
Visuals
------------------------
### The Rendering Pipeline
* Lights
* Materials
* Global Illumination
* Post Effects

### ShaderGraph
* Input: UV Space, Vertex Position, Camera, User Data
* Texture Sampling
* Math recipes: SDFs, Vertex Displacement, Noise, Flipbook animation
* C# -> ShaderGraph -> C#

Animation
------------------------
### Finite State Machine
* Working with Animation Clips
* Triggers/Value Binding
* Blend Trees

### Timeline
* …

### The Alembic Workflow
* Exporting Alembic from C4D
* Importing/Playing back Alembic

### Blending States / C4D Morph Targets
* Generating Morph targets in C4D and exporting for Unity
* Creating Blend Trees from imported .fbx data

Geometry
------------------------
* Mesh Basics
* Mesh manipulation
* Mesh from scratch

Data & Interop
------------------------
* Communicating with Arduino (send-receive)
* Accessing Web APIs
* Reading/Writing Files (.csv, .binary)
