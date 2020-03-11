# UISpatialSelectors
This tool is intended for Magic Leap developers who want to to interact with world canvas UI elements using their hands and the controller as input.

![76205271-465fd980-61d0-11ea-96c1-66f1ffba4968](https://user-images.githubusercontent.com/3331628/76226063-b895e580-61f3-11ea-81b6-3faf47a400b3.gif)

Drag and drop the selector prefabs you want to use into your scene (I have one for a controller raycast and one that uses hand tracking key points).

Inside your world UI canvas, drag and drop SpatialSelectable prefabs (or add a UISpatialSelectable component to an element) like you would regular Unity buttons. These components have OnClick, OnHoverBegin and OnHoverEnd events that you can hook into. UISpatialSelectable extends Unity's Selectable class so it will behave very similar to Unity's buttons and will play nice with their built in event system.

![spatialselectorguide](https://user-images.githubusercontent.com/3331628/76340862-9fb03180-62d2-11ea-84f4-51af465af2a7.png)
