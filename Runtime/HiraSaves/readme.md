# HiraSaves

### What?

A simple API to create persistent saves for your game.

HiraSaves uses a BinaryFormatter, with a few provided Serialization Surrogates, to save your data.

## How To Use

#### 1. Create a serializable class that holds all your saved data.

```c#
[System.Serializable]
public class SaveData
{
    public float playerHealth;
    public int playerAmmo;
}
```

#### 2. Saving the data

> Initialize your SaveData class:
```c#
var saveData = new SaveData { playerHealth = 100f, playerAmmo = 20 };
```

> Access the file with your desired name and save it:
```c#
UnityEngine.HiraSaveUtility.AccessSave<SaveData>("Save1").Data = saveData;
```

> You can also choose to save the data in an alternate manner:
```c#
UnityEngine.HiraSaveUtility.AccessConfig<ConfigData>("PlayerSettings").Data = configData;
```

> This is helpful in keeping your save files and config files separate.

#### 3. Loading the data

>Access the save (or config) file and load its data:
```c#
var configData = HiraSaveUtility.AccessConfig<ConfigData>("PlayerSettings").Data;
// or
var save = HiraSaveUtility.AccessSave<SaveData>("Save1").Data;
```

#### 4. Getting all saves

> You can use this to get a list of all existing save files.
```c#
var saveList = HiraSaveUtility.SaveFilesList;
```

> This can then be used to show a list of existing saves to the player.

> It works the same with config files, if needed.
```c#
var configList = HiraSaveUtility.ConfigFilesList;
```

#### 5. Checking whether a save exists

> You can check whether a save file exists by checking if it's listed in the files list from the previous step, but here's a method that does just that:
```c#
if(HiraSaveUtility.SaveFileExists("Save2")
{
    var saveData = HiraSaveUtility.AccessSave<SaveData>("Save2").Data;
}
```

### 6. Suggestion

> This works wonders when used in conjunction with [**ScriptableObject Variables**](../ScriptableObject%20Variables).

> You can have a SaveManager MonoBehaviour that has serialized fields for ScriptableObject Variables, which are then converted into base C# variables, and a SaveData object is created out of it. The SaveData Object is then saved using HiraSaves API. When loading, it populates the ScriptableObject Variables with the base C# variables from the loaded SaveData object.