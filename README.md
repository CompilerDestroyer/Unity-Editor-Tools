<!----------------------------------------------------Main Header Part------------------------------------------------------------------ -->
<h1 align="center">Unity Editor Tools</h1>

<p align="center"> Unity Editor Tools are a collection of tools, libraries and projects</p>
 <div align="center">
<img align= "center" src= https://github.com/user-attachments/assets/84d389a1-df42-46e8-889d-687fad040e25 width="600">
</div>

<br>

<!-- ----------------------------------------------------Table of Contents----------------------------------------------------- -->
<h2 align="center">Table of Contents</h2>

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
      <td>"A custom box, InfoBoxIconType can be used to determine icon type</td>
    </tr>
     <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#3--Line">Line</a></td>
      <td>Can be used to draw lines</td>
    </tr>
     <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#3--SettingsPanel">SettingsPanel</a></td>
      <td>You can use SettingsPanel to create unity's project settings like UIElements</td>
    </tr>
     <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#3--ToolbarSearchPanel">ToolbarSearchPanel</a></td>
      <td>Same as ToolbarSearchField but search implemented with strings</td>
    </tr>
    <tr>
      <td><a href="#Tools">Tools</a></td>
      <td></td>
    </tr>
    <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#1--Package-Initializer">Package Initializer</a></td>
      <td>Automatically install or remove packages based on the settings</td>
    </tr>
       <tr>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#2--Roughness-Converter">Roughness Converter</a></td>
      <td>The Roughness Converter allows you to generate a Metallic Smoothness Map from a roughness map</td>
    </tr>
  </tbody>
</table>
<!-- -------------------------------------------------------------------------------------------------------------------------- -->




<!----------------------------------------------------Installation Part------------------------------------------------------------------ -->
<h2 align="center">Installation</h2>

<!--Local Installation Part-->
 <p align= "center"> Unity Editor Tools can be installed locally with unity package manager </p>

<!--Git Installation Part-->
<br>

<div align="center">
 <p><strong>OR</strong></p>
</div>

<br>

<p align="center"> Can be installed through git link with unity package manager:</p>
<div align="center">

 ```
https://github.com/CompilerDestroyer/Unity-Editor-Tools.git
```
</div>
<br>
<!-- ------------------------------------------------------------------------------------------------------------------------------- -->


<h2 align="center">Libraries</h2>
<h3 align="left">Attributes</h3>
<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;1- ReadonlyAttribute</h5>
<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Allows you to make fields readonly</p>
```csharp
    [ReadOnly] public int health;
```

<h3 align="left">UI Elements</h3>
<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;1- Header</h5>
```csharp
    Header header = new Header("Basic Header");
```

<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;2- InfoBox</h5>
```csharp
    InfoBox infoBox = new InfoBox("An infobox can be used to give information", InfoBoxIconType.Info, 3f);
```

<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;3- Line</h5>
```csharp
    Line line = new Line(4f, false, Color.red);
```

<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;4- SettingsPanel</h5>
```csharp
    SettingsPanel panel = new SettingsPanel(ref items, ref itemsVisualElementsDict);
```

<h5 align="left">&nbsp;&nbsp;&nbsp;&nbsp;4- ToolbarSearchFieldPanel</h5>

<h2 align="center">Tools</h2>
<h3 align="left">Package Initializer</h3>
<h3 align="left">Roughness Converter</h3>


<h2 align="center">Utilities</h2>
<h3 align="left">Attributes</h3>
<h5 align="left">&nbsp;&nbsp;&nbsp;1- ReadonlyAttribute</h5>


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

