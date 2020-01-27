How To Find The Property Names of Any Shader
===

Quite often we find ourselves dealing with a complex built-in Unity Shader, or a Shader that someone else wrote.  

In such cases, commands like `material.color` or `material.mainTexture` might not work.  

In other cases, we want to access some very obscure property of the Shader, and we find ourselves not knowing how to refer to it, in commands such as `Material.SetColor(string, Color)`.

What we essentially need, is some list of properties of the Shader, so that we can easily find out the name of the property we want to change.  

Luckily, that's easy:

Step 1
---
Select the Material that carries the Shader you want to inspect.
**missing Image**

Step 2
---
Click on the gear/cog at the top right of the Material.  
**missing Image**

Step 3
---
On the dropdown menu that shows up, click on `Select Shader`. This will select the shader, wherever it is located inside your Assets folder. **missing Image**

Step 4
---
What just appeared on the Inspector window, is a list of all the information about our Shader. If we scroll down a little bit, we can see a section named `Properties`. On the **left side** of this section, we can see the names of all our properties of our shader. Copying and pasting these strings in our C# code, ensures that we refer to the property we want to change using the right name.
**missing Image**

Examples
---
### Setting base color
**missing Image**  
`GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(1,0,0));`


### Setting emission
**missing Image**  
`GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(1,0,0));`


