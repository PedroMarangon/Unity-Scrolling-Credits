# Scrolling Credits
**Scrolling Credits** is a Unity package created to facilitate adding a credits screen to your game, similar to the scrolling credits from movies.

This package (before it's modularization and release) was initially made for it's use in the Steam game [Faunamorph](https://store.steampowered.com/app/2461980/Faunamorph/) and then used again in [The Single-Eye Nightmare](https://store.steampowered.com/app/3919680/The_SingleEye_Nightmare/).


<img width="1535" height="856" alt="image" src="https://github.com/user-attachments/assets/79f546ad-2ad3-4821-90a4-18eed603a287" />



## Installation

Install via the Package Manager's ['Install Via Git URL'](https://docs.unity3d.com/Manual/upm-ui-giturl.html), using the following URL:

```
https://github.com/PedroMarangon/Scrolling-Credits.git
```

## Usage
It's very simple to use the Scrolling Credis package. All you need to do is to add the Scrolling Credits prefab to your scene (under `<Right click on Hierarchy>/UI/Scrolling Credits`), set the `Credits Text Asset` field and customize the background sprite and text colors to your will.

<img width="545" height="735" alt="image" src="https://github.com/user-attachments/assets/c6c15d64-46f3-4070-b7a2-eb7aa9a3dc58" />


## Making a Credits Text Asset
The Credits Text Asset is just a plain `.txt` file that follows a specific formatting for the code to understand what goes where. You can make the file by hand or use the **Credits Editor** window to make your own credits file.

### Manual Creation
Creating the manual way is simple. Create a text file using your operational system's file explorer, open it in any text editing software and start writing the credits for your game.

The code detects for specific characters at the start of every line, so there's some basic formatting needed to be followed. You can check the table below for guidance. Any lines that don't start with these characters are automatically skipped.

| **Type of credit line** | **Character at start of line** | **Example**       |
| ----------------------- | ------------------------------ | ----------------- |
| Game Name               | #                              | #[Your game name] |
| Role                    | !                              | !3D Artist        |
| Person                  | _                              | _John Smith       |
| Blank line              | -                              | -                 |

- **Game Name** is the name of the actual game, as the name implies.
- **Role** is what the people under the role is credited for. You can use this also to outline special sections of your credits, like a 'Special Thanks' or 'Asset Packs used'.
- **Person** is the developer that is being credited at.
- **Blank lines** are used to skip a line and give some breathing room. 
> My personal use case for blank lines is when separating types of roles. For example, I can group all the art-related roles without blank lines and then add one blank line before going to the sound-related roles.

Writing the file manually can be error-prone, so the easier way to make the credits file is using the Credits Editor.

Here's an example of what dummy credits file looks like:

```
#Game Name
!Role 01
_Person name 01
_Person name 02
-
!Role 02
_Another person name 01
_Another person name 02
```

---

### Credits Editor

The Credits Editor can be found at `Tools > Credits Editor` and it lets you add and remove roles, persons and line breaks with a single button click.

<img width="603" height="761" alt="image" src="https://github.com/user-attachments/assets/f9fe0549-895e-496b-aa94-810cd18f303f" />


After finishing all your credits, you can save the file with the `Save Credits to New file` at the top, which will open up a dialog window asking you where to save the credits text file. You can also load an existing credits file by assigning it to the TextAsset field above the save button and clicking on `Load From File`, in case you need to edit the values.
> ⚠️ When a TextAsset file is assigned, the `Save Credits to New File` changes to `Save Credits to Existing File`. Clicking it will **not** open the save dialog window, as it'll completely override the currently assigned TextAsset file.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

## License
This project is under **MIT License**. For more details, check the [LICENSE.md](https://github.com/PedroMarangon/Unity-Scrolling-Credits/blob/main/LICENSE.md) file.
