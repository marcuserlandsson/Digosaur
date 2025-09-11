# Surface table track script
# Creates tracks from Surface table touch input via native DLL
extends Node3D
class_name SurfaceTrack

@onready var cam: Camera3D = $"../Camera3D"
@onready var particles: CPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles"
@onready var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state

const INTERACT_RADIUS: int = 15

var query := PhysicsRayQueryParameters3D.new()
var surface_track_node: SurfaceTrackNode
var is_touching: bool = false
var current_touch_position: Vector2

func _ready():
	query.set_collide_with_areas(true)
	# Start with particles disabled
	particles.emitting = false
	
	# Create and setup the Surface track node
	surface_track_node = SurfaceTrackNode.new()
	add_child(surface_track_node)
	
	# Connect to touch signals
	surface_track_node.touch_detected.connect(_on_touch_detected)
	surface_track_node.touch_moved.connect(_on_touch_moved)
	surface_track_node.touch_ended.connect(_on_touch_ended)
	
	print("Surface track system ready. Using native DLL for touch detection.")

func _physics_process(delta: float):
	# Only update position when touching
	if is_touching:
		# Convert screen coordinates to world space
		var result: Dictionary = _screen_to_world_space(current_touch_position)
		if result:
			# Move particles to touch position
			particles.global_position = result.position

func _on_touch_detected(touch_index: int, position: Vector2, size: Vector2):
	print("Touch detected: ", touch_index, " at (", position.x, ", ", position.y, ") size: ", size)
	is_touching = true
	particles.emitting = true
	current_touch_position = position

func _on_touch_moved(touch_index: int, position: Vector2, size: Vector2):
	if is_touching:
		current_touch_position = position

func _on_touch_ended(touch_index: int):
	print("Touch ended: ", touch_index)
	is_touching = false
	particles.emitting = false

func _screen_to_world_space(screen_pos: Vector2) -> Dictionary:
	# Convert screen coordinates to world space using raycasting
	query.from = cam.global_position
	query.to = query.from + _get_world_ray_from_screen(screen_pos)
	return space_state.intersect_ray(query)

func _get_world_ray_from_screen(screen_pos: Vector2) -> Vector3:
	# Convert screen position to world ray
	var viewport_size = get_viewport().size
	var normalized_pos = Vector2(screen_pos.x / viewport_size.x, screen_pos.y / viewport_size.y)
	return cam.project_ray_normal(normalized_pos) * INTERACT_RADIUS

