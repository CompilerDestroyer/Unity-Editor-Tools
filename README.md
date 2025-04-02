<!----------------------------------------------------Main Header Part------------------------------------------------------------------ -->
<h1 align="center">Unity Editor Tools</h1>

<p align="center"> Unity Editor Tools are a collection of tools, libraries and projects</p>
 <div align="center">
<img align= "center" src= https://github.com/user-attachments/assets/84d389a1-df42-46e8-889d-687fad040e25 width="600">
</div>

<br>

<!-- ----------------------------------------------------Table of Contents----------------------------------------------------- -->
## Table of Contents

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
      <td></td>
    </tr>
      <tr>
      <td>&nbsp;&nbsp;<a href="#Attributes">Attributes</a></td>
      <td>Change fields and Methods functionality with attributes</td>
    </tr>
      <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#1--ReadonlyAttribute">ReadonlyAttribute</a></td>
      <td>Allows you to make fields readonly</td>
    </tr>
     <tr>
      <td>&nbsp;&nbsp;<a href="#UI-Elements">UI Elements</a></td>
      <td>Custom UI Elements</td>
    </tr>
    <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#1--Header">Header</a></td>
      <td>Basic general label for headers. Default font size is 18</td>
    </tr>
     <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#2--InfoBox">InfoBox</a></td>
      <td>A custom box, InfoBoxIconType can be used to determine icon type</td>
    </tr>
     <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#3--Line">Line</a></td>
      <td>Can be used to draw lines</td>
    </tr>
     <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#4--SettingsPanel">SettingsPanel</a></td>
      <td>You can use SettingsPanel to create unity's project settings like UIElements</td>
    </tr>
     <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#5--ToolbarSearchPanel">ToolbarSearchPanel</a></td>
      <td>Same as ToolbarSearchField but search implemented with strings</td>
    </tr>
    <tr>
      <td><a href="#Tools">Tools</a></td>
      <td></td>
    </tr>
    <tr>
      <td>&nbsp;&nbsp;<a href="#1--Package-Initializer">Package Initializer</a></td>
      <td>Automatically install or remove packages based on the settings</td>
    </tr>
       <tr>
      <td>&nbsp;&nbsp;<a href="#2--Roughness-Converter">Roughness Converter</a></td>
      <td>The Roughness Converter allows you to generate a Metallic Smoothness Map from a roughness map</td>
    </tr>
  </tbody>
</table>
<!-- -------------------------------------------------------------------------------------------------------------------------- -->




<!----------------------------------------------------Installation Part------------------------------------------------------------------ -->
## Installation

<!--Local Installation Part-->
 Unity Editor Tools can be installed locally with unity package manager

<!--Git Installation Part-->
<br>


OR

Can be installed through git link with unity package manager:


```
https://github.com/CompilerDestroyer/Unity-Editor-Tools.git
```

<!-- ------------------------------------------------------------------------------------------------------------------------------- -->


<h2 align="center">Libraries</h2>



<div align= "center">
 <h3 align="left">Attributes</h3>
 ${\color{grey}namespace: CompilerDestroyer.Editor.Attributes}$
 
</div>






<hr style="border: 0.2px solid lightgray;">

<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;1- ReadonlyAttribute</h5>
<p>Allows you to make fields readonly.</p>

```csharp
    [ReadOnly] public int health;
```

<h3 align="left">UI Elements</h3>

<hr style="border: 0.2px solid lightgray;">

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
<p>Package Initializer will automatically install/remove built-in, git and asset store packages based on the toggles in the settings when editor tools first installed.
 For safety reasons, Asset Store packages will not be removed.</p>


<h3 align="left">2- Roughness Converter</h3>

<p>The Roughness Converter allows you to generate a Metallic Smoothness Map by combining a Metallic Map with a Roughness Map.
 Alternatively, you can create a Smoothness Map directly from a Roughness Map.</p>


<h2 align="center">Utilities</h2>
<p>Add scripting define symbols to RemoveScriptingDefineSymbolsFromBuild.defines list.
 These symbols will be automatically removed during the build process, but restored afterwards to maintain your development environment settings.</p>





<div align="left">


</div>
<br>


<!-- ------------------------------------------------------------------------------------------------------------------------------- -->

<!-- Support -->
<div align= "center">
<h2 align="center">Support</h2>
<p align="center">If you encounter any problems or bugs, create new issue in Issues page:
  <a href="https://github.com/compilerbutcher/Unity-Editor-Visual/issues">Issues</a>
</p>

<h2 align="center">License</h2>
<p align="center">MIT LICENSE:
<a href="https://github.com/compilerbutcher/Unity-Editor-Visual/blob/main/LICENSE">LICENSE</a>
 <p align="center">You can do whatever you want. Just don't try to re-upload and sell it on anywhere.</p>
</div>



<style>
.flex-container {
    display: flex;
    gap: 1rem;
}
</style>
