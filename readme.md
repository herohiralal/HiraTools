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
 
 - ### **[Extension Methods](HiraTools/Extension%20Methods)**
 > A bunch of neat extension methods (some of them are plain static methods) to speed up certain things.

 - ### **[HiraCoroutines](HiraTools/HiraCoroutines)**
 > ``HiraCoroutines`` is a dummy MonoBehaviour singleton that gets created when it's needed, and is then used to run any coroutines that [**HiraTimer**](../HiraTimer) or [**HiraTweener**](../HiraTweener) might require.
 
 > ~~There is nothing else to do here, unless you are looking for a template singleton that creates itself and maintains the reference in a safe way with minimal runtime checks.~~
 > For a template singleton, you can check out HiraManagers.

 - ### **[HiraEvents](HiraTools/HiraEvents)**
 > You know how Unity doesn't really support generics in its reflection, in the way UE4 does, and you have to create a dummy UnityEvent every time you want a different dynamic invokation signature?
 
 > Yeah here's all the boilerplate for that with support for a few types. It's not everything, but it's better than nothing.

 - ### **[HiraLogger](HiraTools/HiraLogger)**
 > Generally, ``Debug.Log("Message")`` is much more expensive than people think it is. So logging every frame might not be a good idea.
   
 > But working in game dev, sometimes you need those per frame logs. HiraLogger provides an alternate way to do so, using C#'s ``Conditional`` method attribute.

 - ### **[HiraManagers](HiraTools/HiraManagers)**
 > An attribute to add to a MonoBehaviour to automate singleton instantiation.

 - ### **[HiraPoolTool](HiraTools/HiraPoolTool)**
 > A pooling solution for Unity, with an emphasis on ease of use, while retaining the maximum possible performance.

 - ### **[HiraPoolTool-Addressable](HiraTools-Addressables/HiraPoolTool)**
 > An alternate version of [**HiraPoolTool**](../../HiraTools/HiraPoolTool), that uses Unity's excellent Addressables package and AssetReference to select the target, instead of a GameObject.

 - ### **[HiraSaves](HiraTools/HiraSaves)**
 > A simple API to create persistent saves for your game.
   
 > HiraSaves uses a BinaryFormatter, with a few provided Serialization Surrogates, to save your data.
 
 > Note that it uses ``Application.persistentDataPath``

 - ### **[HiraTimer](HiraTools/HiraTimer)**
 > If you've ever used ``MonoBehaviour.Invoke`` to time a function, you know how limited it is.
   
 > You can't pause the timer, modify it after having called it, there's no real way to ignore the TimeScale and what not.
   
 > Enter HiraTimer, which gives you some of that control. Not a lot, but better than none.

 - ### **[HiraTweener](HiraTools/HiraTweener)**
 > A tweening library which cuts down on a lot of code.
   
 > Also provides quite a few handy MonoBehaviours, that let you do a lot of things directly from the editor window.
   
 > As a result, the iteration times at design are significantly lowered.

 - ### **[ScriptableObject Variables](HiraTools/ScriptableObject%20Variables)**
 > Check out Ryan Hipple's excellent Unite Austin 2017 [Presentation](https://youtu.be/raQ3iHhE_Kk?t=1057) for an idea of what ScriptableObject Variables are and how to make the best use of them.
   
 > It also includes information about Game Events, which is another thing this module implements - albeit in a more nuanced manner.

 - ### **[SerializedInstance](HiraTools/SerializedInstance)**
 > This one time I was supposed to make a system where I needed to let the designers pick from a list what kind of damage mitigation system they want to use.
   
 > For faster iterations, they wanted to have several such methods, and test them out comparatively, switching them repeatedly.
  
 > So I came up with this.
  
 > Note that this was before SerializedReference was a thing in Unity.


# Created by Rohan.
### Thanks.