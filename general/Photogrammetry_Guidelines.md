Photogrammetry Guidelines
===

About
---
Good quality photogrammetry or LIDAR scanning typically results in multi-GB meshes.  
Only a lunatic would dare to put such a large mesh in a game, let alone an Oculus Quest game.  
Thankfully there are workflows that allow us to _dramatically_ reduce the poly-count of a mesh, without a **perceptible** loss in visual quality.  
These workflows can be implemented in various software, and you can find variations of them in online tutorials.  
We will attempt to present them in a general way, providin as many alternatives as possible.

[Step 0: Read the Guide](https://unity3d.com/files/solutions/photogrammetry/Unity-Photogrammetry-Workflow_2017-07_v2.pdf)
---
Pay special attention to chapter 5.

Step 1: High - To - Low
---
### Software needed
* [**Instant Meshes**](https://github.com/wjakob/instant-meshes)  
  + [Windows](https://instant-meshes.s3.eu-central-1.amazonaws.com/Release/instant-meshes-windows.zip)
  + [Mac](https://instant-meshes.s3.eu-central-1.amazonaws.com/instant-meshes-macos.zip)
  + [Linux](https://instant-meshes.s3.eu-central-1.amazonaws.com/instant-meshes-linux.zip)

### Description
First step in our process is to reduce the polygon count of our mesh.  

Lots of software can do that, but we find Instant Meshes to be one of the fastest, most robust, and easiest to use.   
On top of that it's free & open source! Yay!  

Instant Meshes tries to align the generated polygons to the curvature field of your original mesh, attempting to preserve features and sharp creases.  

As a rule of thumb, try to reduce the triangle count to **1/10** of the original, and adjust accordingly, until you are satisfied with the quality. Keep in mind, the polygon count of the mesh **can** and **should** be as low as possible.  

Don't worry if your model looks like it lost all of its beauty. We will get that back in a while!  

_Patience you must have, young padawan!_

### Usage of Instant Meshes
[Video](https://www.youtube.com/watch?v=U6wtw6W4x3I)  
To get started, launch the binary and select a dataset using the "Open mesh" button on the top left (the application must be located in the same directory as the 'datasets' folder, otherwise the panel will be empty).

The standard workflow is to solve for an orientation field (first blue button) and a position field (second blue button) in sequence, after which the 'Export mesh' button becomes active. Many user interface elements display a descriptive message when hovering the mouse cursor above for a second.

A range of additional information about the input mesh, the computed fields, and the output mesh can be visualized using the check boxes accessible via the 'Advanced' panel.

Clicking the left mouse button and dragging rotates the object; right-dragging (or shift+left-dragging) translates, and the mouse wheel zooms. The fields can also be manipulated using brush tools that are accessible by clicking the first icon in each 'Tool' row.

Step 2: UV Unwrapping
---
### Software Needed
* **Any major 3D package (Maya, 3ds Max, Cinema4D, Blender, Modo etc)**  
or
* [**Rizom UV**](https://www.rizom-lab.com/rizomuv-vs/) (70% student discount)  

### Description
Once our low-poly mesh is exported, we need to make sure its UV coordinates are not overlapping.  

We do not necessarily need to create a perfect UV unwrapping, minimizing distortion, hiding seams, packing as tightly as possible etc, although learning how to do the above will certainly help in your career.  

For now what's enough is for the UVs to be auto-split and auto-packed by our software.

### Automatic UV-Unwrapping in Cinema4D

#### Switch Layout to UV Edit
![](https://github.com/GeorgeAdamon/dfpi/tree/master/general/Photogrammetry_Resources_Screenshots/UV1.png "Switch Layout")  

#### Prepare for Auto-Layout

#### Cubic Auto-Layout

#### Angle Auto-Layout

#### Re-Align 
