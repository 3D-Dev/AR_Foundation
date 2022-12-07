# AR_Foundation using Unity3D
# Tested with Android
# Possible to detecting the distance to faces from device's camera
1. Implemented the ARPlane feature with ARPlaneManager 
The plane manager is a type of trackable manager.
The plane manager creates GameObjects for each detected plane in the environment. A plane is a flat surface represented by a pose, dimensions, and boundary points. The boundary points are convex.

Examples of features in the environment that can be detected as planes are horizontal tables, floors, countertops, and vertical walls.

You can specify a detection mode, which can be horizontal, vertical, or both. Some platforms require extra work to perform vertical plane detection, so if you only need horizontal planes, you should disable vertical plane detection.

2. Implemented the ARPointCloud feature with ARPointCloudManager
The point cloud manager is a type of trackable manager.
The point cloud manager creates point clouds, which are sets of feature points. A feature point is a specific point in the point cloud which the device uses to determine its location in the world. Feature points are typically notable features in the environment that the device can track between frames, such as a knot in a wooden table.

A point cloud is a set of feature points that can change from frame to frame. Some platforms only produce one point cloud, while others organize their feature points into different point clouds in different areas of space.

A point cloud is considered a trackable, while individual feature points are not. However, feature points can be uniquely identified between frames as they have unique identifiers.

3. Implemented the Place Object on plane with PlaceObjectOnPlane
When click the screen after detect the plane, display the Cube object