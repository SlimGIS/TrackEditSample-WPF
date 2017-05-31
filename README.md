# Draw and Edit Geometries

In GIS application, Drawing and Editing geometries is a very normal function. SlimGIS built a serial operation to make this job very easy and convenient to use. Follow my article to see how we support it. Here is a short video.

<iframe width="560" height="315" src="https://www.youtube.com/embed/CRxryzTTZi0" frameborder="0" allowfullscreen></iframe>

Get source code for WPF [here](https://github.com/SlimGIS/TrackAndEdit-WPF).

> This project already includes the runtime license. If your trial license is expired, it still can run the project by "ctrl + F5". Or [try executable](https://github.com/SlimGIS/TrackAndEdit-WPF/releases). 

## Drawing Behavior
We will first talk about drawing. We support drawing point, line, polygon and inner rings in polygon. Use this code to enable drawing polygon:

```
Map1.Behaviors.TrackBehavior.TrackMode = TrackMode.Polygon;
```

- __Draw Point__: single click on the map to draw a point geometry.
- __Draw Line__: single click to start drawing. It plots a point on the map, while you moving mouse, a dashline will connect between the previous point and the moving position. Single click another place on the map to plot another vertex for the line geometry. Try to continuously plot several vertices, the vertices are connected as a line. When the line is good as drawn, double click on the map to finish the drawing, and the double click place will be the last vertex in the line geometry.
- __Draw Polygon__: the behavior is the same as drawing a line. The difference is that, the polygon is always a closed line which we call it `Ring`. 
- __Draw a Hole in Polygon__: Some polygon might contains a hole. Imagine an island in a lake on the map. The surface of lake is the polygon, while the island is the hole. To draw a hole, we need to use a modifier key `ALT`, it is the same as Photoshop's `selection tool`. Press `ALT` key and select means unselect. So let's first draw a polygon and double click to complete the drawing. Then press `ALT` key and hold, then draw a new polygon in the drawn polygon will become a hole inside the existing polygon.

> During drawing line or polygon, you could press `ESC` key to cancel the previous drawn vertex. You can press `ESC` to delete the last vertex till removing the entire drawn geometry.   

> The drawn features can be found in `TrackBehavior.Features` property.

Pretty straight forward right? Now let's see how to edit a geometry.

## Edit Behavior
To edit a geometry, we need to first add a geometry into our _EditBehavior_. Code below:
```
map.Behaviors.EditBehavior.Features.Add(feature);
```

We have several editing options that allow to turn on/off.
```
map.Behaviors.EditBehavior.CanTranslate = true;
map.Behaviors.EditBehavior.CanReshape = true;
map.Behaviors.EditBehavior.CanResize = true;
map.Behaviors.EditBehavior.CanRotation = true;
```

To start editing, click on a geometry to make it selected. The color will be different from the other geometries.

![geometry-to-be-editing](https://github.com/SlimGIS/TrackAndEdit-WPF/blob/master/Images/edit-selected.png?raw=true)

- __Move__: directly mouse down on the selected geometry, keep it down and move your mouse around, the selected geometry will move along with your mouse moving.
- __Resize__ and __Rotate__: when the two options are turned on, there will be a handler point on the left bottom corner of the selected geometry. Drag it around, you will see how it changes along with your mouse moving.
- __Reshape__: when this option is on, we will put a handler on each vertex on the geometry. 
    - Move a vertex, drag one handler vertex around to reshape it. 
    - Insert a vertex, mouse down on the middle of the line segment and drag it out. 
    - Remove a vertex, double click the handler vertex to delete it.

- __Select multiple geometries__: hold _Shift Key_ and click a new geometry to select it as in editing mode.

> The edited features can be found in `EditBehaviors.Features` property. This property contains the features that can be edit, and the features that are in editing. We have another property `InEditingFeatureIds` could help you to filter if a feature is selected to edit.

When we understand the operation, we could start to edit any data source like shapefile and save the editing geometry back into the source.

> __A Tip for Performance__: the drawing and editing geometries are mantained in memory, and it is rendered dynamically during the operation. Please keep the editing or drawing features collection lite to get better performance. A scenario is adjusting a country's border. A bad experience is to add all the countries geometry in a shapefile to the edit behavior. The right solution is to select the single country that is going to edit, add this selected country into edit behavior. Once the editing complete, move it back to its source.

That's all for drawing and editing geometries.
