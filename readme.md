# HiraTools

### What?

 A bunch of tools that cut down significantly on the boilerplate you might need to write.
 
### How to use?

 Just grab the latest release and put it in your Unity Project's `Assets/Plugins` folder.
 
 This repository contains all of the code involved in this project, so if you wish to build it yourself, you can do that as well. Or you can just import the *.cs files directly into your Unity project.
 
 The detailed documentation for any specific API is in the corresponding folder. Follow the links in the [Contents](#contents) section. 
 
 Keep package dependencies in mind however.
 
 - `HiraTools-Addressables.dll` requires the Addressables package from Unity Package Manager.
 
> #### Please also keep in mind to mark ``HiraTools-Editor.dll`` assembly as Editor-Only in your Unity Inspector.
> ![IMAGEPLACEHOLDER - EDITORONLY](.images/editoronly.png)
 
## Contents
 
 - **[Extension Methods](HiraTools/Extension%20Methods)**
 > Some extension methods and static methods to cut down on the boilerplate.

 - **[HiraCoroutines](HiraTools/HiraCoroutines)**
 > A template singleton that does nothing else.

 - **[HiraEvents](HiraTools/HiraEvents)**
 > Boilerplate child classes of UnityEvents.

 - **[HiraLogger](HiraTools/HiraLogger)**
 > Conditional logging, using scripting define symbols. More boilerplate.

 - **[HiraPoolTool](HiraTools/HiraPoolTool)**
 > A pooling solution.

 - **[HiraPoolTool-Addressable](HiraTools-Addressables/HiraPoolTool)**
 > A pooling solution ft. Addressables.

 - **[HiraSaves](HiraTools/HiraSaves)**
 > Persistent saving solution.

 - **[HiraTimer](HiraTools/HiraTimer)**
 > A better version of Unity MonoBehaviour's Invoke calls.

 - **[HiraTweener](HiraTools/HiraTweener)**
 > Tweening library with a sharp focus on ease of use, so you can do a lot of things from the Inspector itself.

 - **[ScriptableObject Variables](HiraTools/ScriptableObject%20Variables)**
 > ScriptableObject Variables, like the ones from Ryan Hipple's Unite Austin 2017 talk. Also includes some extra functionality.

 - **[SerializedInstance](HiraTools/SerializedInstance)**
 > Too long to explain, LOL.


# Created by Rohan.
### Thanks.