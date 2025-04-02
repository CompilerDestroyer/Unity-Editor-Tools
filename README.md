<!----------------------------------------------------Main Header Part------------------------------------------------------------------ -->
<h1 align="center">Unity Editor Tools</h1>

<p align="center"> Unity Editor Tools are a collection of tools, libraries and projects</p>
 <div align="center">
<img align= "center" src= https://github.com/user-attachments/assets/84d389a1-df42-46e8-889d-687fad040e25 width="600">
</div>

<br>

<!-- ----------------------------------------------------Table of Contents----------------------------------------------------- -->
<h2 align= "center">Table of Contents</h2>

<table align="center" border="1" cellpadding="10" cellspacing="0">
  <thead>
    <tr>
      <th>Section</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="#Libraries">Libraries</a></td>
      <td>How to install Unity Editor Visual using local or Git-based methods</td>
    </tr>
      <tr>
      <td><a href="#Attributes">Attributes</a></td>
      <td>How to install Unity Editor Visual using local or Git-based methods</td>
    </tr>
      <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#1--ReadonlyAttribute">Readonly fields</a></td>
      <td>Customize folder visuals with custom icons</td>
    </tr>
     <tr>
      <td><a href="#UI-Elements">UI Elements</a></td>
      <td></td>
    </tr>
    <tr>
      <td><a href="#Tools">Tools</a></td>
      <td>Overview of available customization packages</td>
    </tr>
    <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#1--Package-Initializer">Folder Icons</a></td>
      <td>Customize folder visuals with custom icons</td>
    </tr>
       <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#2--Roughness-Converter">Folder Icons</a></td>
      <td>Customize folder visuals with custom icons</td>
    </tr>
    <tr>
      <td><a href="#Utilities">Support</a></td>
      <td>Where to report bugs or request help</td>
    </tr>
  </tbody>
</table>

<!-- -------------------------------------------------------------------------------------------------------------------------- -->


<br><br>

<!----------------------------------------------------Installation Part------------------------------------------------------------------ -->
<h2 align="left">Installation</h2>

<!--Local Installation Part-->
<p>Unity Editor Tools can be installed locally with unity package manager</p>

<!--Git Installation Part-->
<br>


<p>Or</p>

<p>Can be installed through git link with unity package manager:</p>

```
https://github.com/CompilerDestroyer/Unity-Editor-Tools.git
```

<!-- ------------------------------------------------------------------------------------------------------------------------------- -->


<h2 align="center">Libraries</h2>
<br>
<br>

<h3>Attributes</h3>
<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;1- ReadonlyAttribute</h5>
<p>Allows you to make fields readonly.</p>

```csharp
[ReadOnly] public int health;
```
<br>

---

<br>
<h3>UI Elements <sub>(namespace: CompilerDestroyer.Editor.UIElements)</sub></h3>



<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;1- Header</h5>
<p>Basic general label for headers. Default font size is 18.</p>

```csharp
Header header = new Header("Basic Header");
```

<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;2- InfoBox</h5>
<p>A custom box. InfoBoxIconType can be used to determine icon type.</p>

```cs
InfoBox infoBox = new InfoBox("An infobox can be used to give information", InfoBoxIconType.Info, 3f);
```

<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;3- Line</h5>
<p>A line that can be used to draw lines.</p>

```csharp
Line line = new Line(4f, false, Color.red);
```

<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;4- SettingsPanel</h5>
<p>You can use SettingsPanel to create Unity's project settings-like UIElements.
 In order to add items, you need to use TreeViewItemData<string> and in order to add functionality to it, you need to add a VisualElement to a dictionary with the same name as TreeViewItemData<string>.</p>

```cs
List<TreeViewItemData<string>> items = new List<TreeViewItemData<string>>();
TreeViewItemData<string> example1TreeViewItemData = new TreeViewItemData<string>(0, "Example 1");
TreeViewItemData<string> example2TreeViewItemData = new TreeViewItemData<string>(1, "Example 2");
items.Add(example1TreeViewItemData);
items.Add(example2TreeViewItemData);
Dictionary<string, VisualElement> itemsVisualElementsDict = new Dictionary<string, VisualElement>();
itemsVisualElementsDict.Add("Example 1", new Label("I am example 1"));
itemsVisualElementsDict.Add("Example 2", new Label("I am example 2"));

SettingsPanel panel = new SettingsPanel(ref items, ref itemsVisualElementsDict);
```

<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;5- ToolbarSearchPanel</h5>
<p>Same as ToolbarSearchField but search implemented with strings.</p>

```cs
List<string> toolbarSearchList = new List<string>() { "Level Editor", "Terrain Licker", "Inspector Destroyer", "Mesh Consumer" };
List<string> resultList = new List<string>();

VisualElement searchBarContainer = new VisualElement();


ListView listView = new ListView(toolbarSearchList, 15);
listView.makeItem = () => new Label();
listView.bindItem = (element, index) => (element as Label).text = listView.itemsSource[index] as string;

void OnEmpty()
{
    listView.itemsSource = toolbarSearchList;
    listView.Rebuild();
}
void OnFilled()
{
    listView.itemsSource = resultList;
    listView.Rebuild();
}
ToolbarSearchPanel toolbarSearchPanel = new ToolbarSearchPanel(toolbarSearchList, resultList, OnEmpty, OnFilled);
```

<h2 align="center">Tools</h2>
<h3 align="left">1- Package Initializer</h3>

![Package Initializer](https://github.com/user-attachments/assets/57c7a094-89a2-41cf-88fa-f5ecfb66477b)

<p>Package Initializer will automatically install/remove built-in, git and asset store packages based on the toggles in the settings when editor tools first installed. For safety reasons, Asset Store packages will not be removed.<br>
After adjusting toggle of packages, settings will be saved to default preferences path. <br>Every first install of Editor Tools will automatically update packages accordingly.<br><br>
Package Initializer can be found in the "Tools > Compiler Destroyer > Editor Tools > Tools > Package Initializer"</p>


<h3 align="left">2- Roughness Converter</h3>

![Roughness Converter](https://github.com/user-attachments/assets/22f3d77b-a445-4f31-8e42-8b25aa5ae2ec)

<p>The Roughness Converter allows you to generate a Metallic Smoothness Map by combining a Metallic Map with a Roughness Map.<br>
 Alternatively, you can create a Smoothness Map directly from a Roughness Map.<br><br>Roughness Converter can be found in the "Tools > Compiler Destroyer > Editor Tools > Tools > Roughness Converter"</p>
 


<h2 align="center">Utilities</h2>
<p>Add scripting define symbols to RemoveScriptingDefineSymbolsFromBuild.defines list.<br>
 These symbols will be automatically removed during the build process, but restored afterwards to maintain your development environment settings.</p>


<div align="left">


</div>
<br>


<!-- ------------------------------------------------------------------------------------------------------------------------------- -->

<!-- Support -->
<div align= "center">
<h2 align="center">Support</h2>
<p align="center">If you encounter any problems or bugs, create new issue in Issues page:
  <a href="https://github.com/compilerdestroyer/Unity-Editor-Tools/issues">Issues</a>
</p>

<h2 align="center">License</h2>
<p align="center">MIT LICENSE:
<a href="https://github.com/compilerdestroyer/Unity-Editor-Tools/blob/main/LICENSE">LICENSE</a>
 <p align="center">You can do whatever you want. Just don't try to re-upload and sell it on anywhere.</p>
</div>



<style>
.flex-container {
    display: flex;
    gap: 1rem;
}
</style>
