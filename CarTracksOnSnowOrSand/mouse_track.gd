# Simplified mouse track script
# Creates tracks only when mouse button is pressed and held
extends Node3D
class_name MouseTrack

@onready var cam: Camera3D = $"../Camera3D"
@onready var particles: CPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles"
@onready var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state

const INTERACT_RADIUS: int = 15
var query := PhysicsRayQueryParameters3D.new()
var mouse_position: Vector3
var is_mouse_pressed: bool = false

func _ready():
	query.set_collide_with_areas(true)
	# Start with particles disabled
	particles.emitting = false

func _input(event):
	# Check for mouse button press/release
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			is_mouse_pressed = event.pressed
			particles.emitting = is_mouse_pressed

func _physics_process(delta: float):
	# Only update position when mouse is pressed
	if is_mouse_pressed:
		# Get mouse position in world space
		var result: Dictionary = _detect_from_cam_to_mouse()
		if result:
			mouse_position = result.position
			# Move particles to mouse position
			particles.global_position = mouse_position

func _detect_from_cam_to_mouse() -> Dictionary:
	query.from = cam.global_position
	query.to = query.from + _get_world_mouse_ray()
	return space_state.intersect_ray(query)

func _get_world_mouse_ray() -> Vector3:
	var mouse_pos: Vector2 = get_viewport().get_mouse_position()
	return cam.project_ray_normal(mouse_pos) * INTERACT_RADIUS
