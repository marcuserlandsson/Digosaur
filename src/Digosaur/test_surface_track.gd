extends Node3D

# Test script for Surface track detection
var surface_track_node: SurfaceTrackNode

func _ready():
	# Create the Surface track node
	surface_track_node = SurfaceTrackNode.new()
	add_child(surface_track_node)
	
	# Connect to signals
	surface_track_node.touch_detected.connect(_on_touch_detected)
	surface_track_node.touch_moved.connect(_on_touch_moved)
	surface_track_node.touch_ended.connect(_on_touch_ended)
	
	print("Surface track test ready! Waiting for UDP messages...")

func _on_touch_detected(touch_index: int, position: Vector2, size: Vector2):
	print("Touch detected: ", touch_index, " at ", position, " size ", size)
	
	# Convert screen position to world position
	var world_pos = _screen_to_world_position(position)
	
	# Create a simple visual indicator
	var indicator = MeshInstance3D.new()
	var sphere = SphereMesh.new()
	sphere.radius = 0.1
	indicator.mesh = sphere
	indicator.position = world_pos
	add_child(indicator)
	
	# Remove after 2 seconds
	var timer = Timer.new()
	timer.wait_time = 2.0
	timer.one_shot = true
	timer.timeout.connect(func(): indicator.queue_free())
	add_child(timer)
	timer.start()

func _on_touch_moved(touch_index: int, position: Vector2, size: Vector2):
	print("Touch moved: ", touch_index, " to ", position)

func _on_touch_ended(touch_index: int):
	print("Touch ended: ", touch_index)

func _screen_to_world_position(screen_pos: Vector2) -> Vector3:
	# Convert 2D screen position to 3D world position
	# This is a simple conversion - you might need to adjust based on your camera setup
	var camera = get_viewport().get_camera_3d()
	if camera:
		var viewport_size = get_viewport().get_visible_rect().size
		var normalized_pos = Vector2(screen_pos.x / viewport_size.x, screen_pos.y / viewport_size.y)
		var world_pos = camera.project_ray_origin(normalized_pos)
		return world_pos
	else:
		# Fallback if no camera
		return Vector3(screen_pos.x / 100.0, 0, screen_pos.y / 100.0)
