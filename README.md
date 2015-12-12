TypeText
=========================

Very simple text typing effect for Unity (both uGUI and NGUI)

At a Glance
-----------
- **uGUI:**
```csharp
Text.TypeText("Some text", 0.05f);
```
- **NGUI:**
```csharp
UILabel.TypeText("Some text", 0.05f);
```
- **Common:**
```csharp
if (Text.IsSkippable())
  Text.SkipTypeText();
```


Features
--------
- **Support Text Speed**: ```[speed=0.05]Hello! my name is[speed=0.2]typetext.```
- **Support uGUI Tags**: ```<b><i><size><color>...```
- **Support NGUI Tags**: ```[b][i][s][u][sup][sub][FFFFFF][-]...```
- **Support Skip**
- **Unity4/Unity5 Compatible**
- **Very Simple**


Screenshots
--------
![TypeText Screenshot GIF](https://cloud.githubusercontent.com/assets/1309940/11761765/f897ff48-a112-11e5-97c7-f9bbdef387bc.gif)
![TypeText Screenshot](https://cloud.githubusercontent.com/assets/1309940/11761719/06acaf9a-a111-11e5-8c35-1ec0bc06b470.PNG)


License
-------
**TypeText** is under MIT license. See the LICENSE file for more info.
