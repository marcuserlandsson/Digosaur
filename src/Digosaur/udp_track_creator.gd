extends Node3D

# Creates tracks from UDP touch data
@onready var cam: Camera3D = $"../Camera3D"
@onready var particles: CPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles"
@onready var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state
@onready var udp_listener: Node = $"../UDPListener"

const INTERACT_RADIUS: int = 15

func _ready():
	# Connect to UDP listener signals
	if udp_listener:
		udp_listener.touch_detected.connect(_on_touch_detected)
		udp_listener.touch_moved.connect(_on_touch_moved)
		udp_listener.touch_ended.connect(_on_touch_ended)

func _on_touch_detected(touch_index: int, position: Vector2, size: Vector2):
	print("Creating track at: ", position)
	create_track_at_position(position)

func _on_touch_moved(touch_index: int, position: Vector2, size: Vector2):
	print("Moving track to: ", position)
	create_track_at_position(position)

func _on_touch_ended(touch_index: int):
	print("Touch ended: ", touch_index)

func create_track_at_position(screen_pos: Vector2):
	# Convert screen position to world position
	var world_pos = _screen_to_world_position(screen_pos)
	
	# Create particle at world position
	if particles:
		particles.position = world_pos
		particles.restart()

func _screen_to_world_position(screen_pos: Vector2) -> Vector3:
	# Convert 2D screen position to 3D world position
	var viewport_size = get_viewport().get_visible_rect().size
	var normalized_pos = Vector2(screen_pos.x / viewport_size.x, screen_pos.y / viewport_size.y)
	
	if cam:
		var world_pos = cam.project_ray_origin(normalized_pos)
		return world_pos
	else:
		# Fallback if no camera
		return Vector3(screen_pos.x / 100.0, 0, screen_pos.y / 100.0)
