# BBImageHandler for DNN 8

## Project description ##
The BBImagehandler for DNN8+ is an extension to the new imagehandler in DNN8. It adds a lot of functionality back again that was formerly contained in the old BBImagehandler and was stripped out while integrated into the core.

## Installation ##
Install the same way as every module, skin and other extension for dnn via host/extensions.

## Whats new ? ##
*Version 2.0.0 08.07.2016*
- Initial version for DNN8
 
## Methods ##

### Database image (mode=dbimage) ###

Display an image stored in a database field 

Sample: 

**/dnnimagehandler.ashx?mode=dbimage&table=MyImages&ImageField=ImageData&idField=ImageID&idValue=3**

![Database Image](dbimage.jpg)

Parameters:
- **table**: Name of the table in database
- **imagefield**: Name of the image- ield containing the image data
- **idfield**: Name of the field containing the primary key (must be integer)
- **idvalue**: value of id, determines the record from which the image is fetched
- **connection**: name of sql connection in web.config. (Default: SiteSqlServer)

### Barcodes (mode=barcode) ###

With this extension it is possible to create barcodes of different types. For more infos see [https://zxingnet.codeplex.com/](https://zxingnet.codeplex.com/)

Sample:

**/dnnImagehandler.ashx?mode=barcode&width=150&Height=150&type=qrcode&content=this%20is%20the%20barcode%20content**

![Barcode Image](barcode.jpg)

Parameters:
- **type**: upca, ean8, ean13, code39, code128, itf, codabar, plessey, msi, qrcode, pdf417, aztec, datamatrix 
- **width**: Width of resulting barcode 
- **height**: Height of resulting barcode 
- **border**: Width of border (Specifies margin, in pixels, to use when generating the barcode. The meaning can vary by format; for example it controls margin before and after the barcode horizontally for most 1D formats.) 
- **content**: The content of the barcode (numeric or alphanumeric,depends on barcode type) 

### Thermometer (mode=thermometer) ###

I built this thermometer display for my [DNNConnect 2015 session](http://dnn-connect.org/events/2015/sessions/moduleId/731/conferenceId/1/sessionId/40/controller/Session/action/View) .

Sample:

**/dnnImagehandler.ashx?mode=thermometer&degree=0&h=150** (degree in steps a 10 from 0 to 100)

![Thermometer](thermometer.jpg)

Parameters:
- **degree**: value between 0 and 100
- **areas**:  comma separated list of degree values defining the color areas (Default: 20,30,40,60)

### Percentage (mode=percent) ###

Two different types of percentage display. The first (circle) is perfect to visualize 1 percentage value while the other one (bar) runs best in a grid with percentage values in a column

Sample:

**/dnnImagehandler.ashx?mode=percent&type=circle&percentage=40&color=orange&w=100**

![Percent type=circle](percent1.jpg)

**/dnnImagehandler.ashx?mode=percent&type=bar&percentage=40&color=orange&w=200**

![Percent type=bar](percent2.jpg)

Parameters:
- **type**: circle, bar
- **percentage**: value between 0 and 100
- **color**: color of percentage bar

### Counter (mode=counter) ###

This is an old fashion counter formerly seen on nearly every webpage - now a little bit outdated but perhaps usable for some retro stuff.

Sample:

**/dnnImagehandler.ashx?mode=counter&filename=images/counter.gif&Value=1024&digits=5**

![Counter image](counter.jpg)

Parameters:
- **filename**: Must be special counter image file with digits 0 to 9 with similar width per digit
- **digits**: No of digits. 
- **counter**: Value to display 

### Web Thumbnail (mode=webthumb) ###

Creates a thumbnail of a webpage given by an url. Try with care, seems to be a little bit buggy!

Sample:

**/dnnImagehandler.ashx?mode=webthumb&url=http%3A%2F%2Fwww.huffingtonpost.de&ratio=screen&w=400**

![Web thumbnail image](webthumb.jpg)

Parameters:
- **url**: url-encoded web address
- **ratio**: full (whole page), screen (2:3) , cinema (16:9)


### Year schedule (mode=yearschedule) ###

Useful for websites that do daywise rental (holiday homes, appartments etc.)

Sample:

**/dnnImagehandler.ashx?mode=yearschedule&culture=de-de&matrix=1116611111661111166111...**

![Year schedule image](yearschedule.jpg)

Parameters:
- **color**: color of background (optional, default = white) 
- **culture**: Culture abbrev (e.g. "en-us"). Needed for displaying month names (optional, default is CurrentCulture) 
- **matrix**: String of length 12 * 31 = 372 (0 = no valid date (eg. Feb. 30), 1 = free, 2 = reserved, 3 = occupied, 4 = selected. Add +5 if weekend) 

### Bar chart (mode=barchart) ###

Ugly but maybe useful for a quick and dirty bar graph

Sample:

**/dnnImagehandler.ashx?mode=barchart&xaxis=man,woman,kids,grandparents&yaxis=10,20,30,20&color=green**

![Bar chart](barchart.jpg) 

Parameters:
- **xaxis**: comma seperated list of bar captions 
- **yaxis**: comma seperated list of bar values
- **color**: bar color

### Module info (mode=modinfo) ###

Not very useful for normal purposes but could be cool addition to the dnn action menu (see below)

Sample:

**/dnnimagehandler.ashx?mode=modinfo&tabid=59&moduleid=414**

![Module Info](modinfo.jpg)

Parameters:
- **tabid**: id of the tab (page) the module sits on
- **moduleid**: id of the module

<hr/>

## Extending the DNN action menu ##

![Info menu](infomenu.jpg)

To add an "info" action menu to your module toolbar, do the following hack. **Please be aware that this hack will be overwritten with every DNN update !**

- Open */admin/Menus/ModuleActions/ModuleActions.js* for editing
- Search (~ line 372): *var menuRoot = menu.find("ul");*
- Insert after:  

```javascript
buildInfoMenu(menuRoot, "Info", "actionMenuInfo","info");
```

- Search (~ line 231): *function buildMoveMenu(root, rootText, rootClass, rootIcon) {*
- Insert before:

```javascript
function buildInfoMenu(root, rootText, rootClass, rootIcon) {
    var parent = buildMenuRoot(root, rootText, rootClass, rootIcon);
    var htmlString = "<img src=\"dnnimagehandler.ashx?mode=modinfo&moduleid=" + moduleId + "&tabid=" + tabId + "\" />"
    parent.append(htmlString);
}
```

<!-- Post Configuration -->
<!--
```xml
<abstract>
DNNImageHandler8 is an extension for DNN8. It provides additional creation of images like counters, barcodes, bar graphs and other stuff.
</abstract>
<categories>

</categories>
<postid></postid>
<keywords>

</keywords>
<weblog>
bitboxx.dnn.blog.
</weblog>
```
-->
<!-- End Post Configuration -->
