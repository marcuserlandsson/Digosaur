# Touch track script for Surface table
# Creates tracks based on touch input from the Surface table
extends Node3D
class_name TouchTrack

@onready var cam: Camera3D = $"../Camera3D"
@onready var particles1: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles1"
@onready var particles2: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles2"
@onready var particles3: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles3"
@onready var particles4: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles4"
@onready var particles5: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles5"
@onready var particles6: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles6"
@onready var particles7: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles7"
@onready var particles8: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles8"
@onready var particles9: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles9"
@onready var particles10: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles10"
@onready var particles11: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles11"
@onready var particles12: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles12"
@onready var particles13: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles13"
@onready var particles14: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles14"
@onready var particles15: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles15"
@onready var particles16: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles16"
@onready var particles17: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles17"
@onready var particles18: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles18"
@onready var particles19: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles19"
@onready var particles20: GPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles20"

@onready var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state
#@onready var tcp_client = $"../TCPClient"

const INTERACT_RADIUS: int = 15
const MERGE_DISTANCE = 10  # pixels - merge touches within this distance
const MAX_TOUCHES = 20     # prevent array bloat
const TRAIL_DURATION = 2.0  # seconds - how long trails last
var query := PhysicsRayQueryParameters3D.new()
var touch_position: Vector3
var is_touching: bool = false
var current_touches:= Array([])
var touch_trails: Array = []  # Store persistent touch trails
#var last_touch_positions:= Array([], TYPE_VECTOR4, "", null)  # Track previous touch positions

signal movePoint(point, newPosition)

func _ready():
	query.set_collide_with_areas(true)
	# Start with particles disabled
	particles1.emitting = false
	particles2.emitting = false
	particles3.emitting = false
	particles4.emitting = false
	particles5.emitting = false
	particles6.emitting = false
	particles7.emitting = false
	particles8.emitting = false
	particles9.emitting = false
	particles10.emitting = false
	particles11.emitting = false
	particles12.emitting = false
	particles13.emitting = false
	particles14.emitting = false
	particles15.emitting = false
	particles16.emitting = false
	particles17.emitting = false
	particles18.emitting = false
	particles19.emitting = false
	particles20.emitting = false


func _physics_process(delta: float):
	# Update existing trails (fade them out over time)
	update_touch_trails(delta)

	
	# Get touch data from TCP client
	if current_touches.size() > 0:
		#print("DEBUG: Processing ", current_touches.size(), " touches")
		
		# 1. Limit array size to prevent memory bloat
		if current_touches.size() > MAX_TOUCHES:
			current_touches = current_touches.slice(-MAX_TOUCHES)  # Keep only latest touches
		
		# 2. Merge nearby touches (hand spread simulation)
		merge_nearby_touches()
		
		# 3. Process all touches (multi-touch support)
		for i in range(current_touches.size()):
			var touch = current_touches[i]
			# Debug: #print touch coordinates
			#print("DEBUG: Touch coords - X:", touch[0], " Y:", touch[1], " Intensity:", touch[2], " Size:", touch[3])
			
			# Validate touch coordinates (should be within Surface table bounds)
			if touch[0] < 0 or touch[0] > 1920 or touch[1] < 0 or touch[1] > 1080:
				#print("DEBUG: Invalid touch coordinates, skipping")
				continue
			
			# Direct coordinate mapping (same resolution: 1920x1080)
			var x_to_z = float(2.67 - touch[0] / 1920 * 10.68)
			var y_to_x = float(-0.8 + touch[1] / 1080 * 6.1)
			var coords = Vector3(y_to_x, 0.05, x_to_z)
			
			#print("DEBUG: Mapped to world coords: ", coords)
			
			#print("touchPoint ", i, " movement: ", touch[4])
			if touch[4] == Vector2(-1, -1):
				print("New touchPoint ", i, " at: [", touch[0], ", ", touch[1], "]")
			else:
				print("Moved touchPoint ", i, " from ", "[", touch[0] - touch[4][0], ", ", touch[1] - touch[4][1], "] to [", touch[0], ", ", touch[1], "]")
			
			# Create persistent touch trail
			if i == 0:
				create_persistent_trail(coords, touch[3], particles1)  # touch[3] is intensity
			elif i == 1:
				create_persistent_trail(coords, touch[3], particles2)  # touch[3] is intensity
			elif i == 2:
				create_persistent_trail(coords, touch[3], particles3)  # touch[3] is intensity
			elif i == 3:
				create_persistent_trail(coords, touch[3], particles4)  # touch[3] is intensity
			elif i == 4:
				create_persistent_trail(coords, touch[3], particles5)  # touch[3] is intensity
			elif i == 5:
				create_persistent_trail(coords, touch[3], particles6)  # touch[3] is intensity
			elif i == 6:
				create_persistent_trail(coords, touch[3], particles7)  # touch[3] is intensity
			elif i == 7:
				create_persistent_trail(coords, touch[3], particles8)  # touch[3] is intensity
			elif i == 9:
				create_persistent_trail(coords, touch[3], particles9)  # touch[3] is intensity
			elif i == 9:
				create_persistent_trail(coords, touch[3], particles10)  # touch[3] is intensity
			elif i == 10:
				create_persistent_trail(coords, touch[3], particles11)  # touch[3] is intensity
			elif i == 11:
				create_persistent_trail(coords, touch[3], particles12)  # touch[3] is intensity
			elif i == 12:
				create_persistent_trail(coords, touch[3], particles13)  # touch[3] is intensity
			elif i == 13:
				create_persistent_trail(coords, touch[3], particles14)  # touch[3] is intensity
			elif i == 14:
				create_persistent_trail(coords, touch[3], particles15)  # touch[3] is intensity
			elif i == 15:
				create_persistent_trail(coords, touch[3], particles16)  # touch[3] is intensity
			elif i == 16:
				create_persistent_trail(coords, touch[3], particles17)  # touch[3] is intensity
			elif i == 17:
				create_persistent_trail(coords, touch[3], particles18)  # touch[3] is intensity
			elif i == 18:
				create_persistent_trail(coords, touch[3], particles19)  # touch[3] is intensity
			elif i == 19:
				create_persistent_trail(coords, touch[3], particles20)  # touch[3] is intensity
		
		# 4. Clear array after processing (prevents backlog but allows trail creation)
		current_touches.clear()
		#particles.emitting = true
	#else:
		# No current touches, but keep existing trails alive
		#particles.emitting = touch_trails.size() > 0

func _detect_from_cam_to_touch(touch_screen_pos: Vector2) -> Dictionary:
	var cam = get_viewport().get_camera_3d()
	if cam == null:
		return {}
	query.from = cam.global_position
	query.to = query.from + _get_world_touch_ray(cam, touch_screen_pos)
	return space_state.intersect_ray(query)

func _get_world_touch_ray(cam: Camera3D, touch_screen_pos: Vector2) -> Vector3:
	return cam.project_ray_normal(touch_screen_pos) * INTERACT_RADIUS


#func _on_tcp_client_touch_points(current_touches_input: Variant) -> void:
#	last_touch_positions = current_touches.duplicate(true)
#	current_touches = current_touches_input.duplicate(true)

func merge_nearby_touches():
	"""Merge touches that are close together (same finger/hand)"""
	if current_touches.size() <= 1:
		return
	
	var merged_touches = []
	var processed = []
	
	for i in range(current_touches.size()):
		if i in processed:
			continue
			
		var touch = current_touches[i]
		var merged_touch = touch
		var merge_count = 1
		
		# Find nearby touches to merge
		for j in range(i + 1, current_touches.size()):
			if j in processed:
				continue
				
			var other_touch = current_touches[j]
			var distance = sqrt(pow(touch[0] - other_touch[0], 2) + pow(touch[1] - other_touch[1], 2))
			
			if distance < MERGE_DISTANCE:
				# Merge touches: average position, sum intensity, max size
				merged_touch[0] = (merged_touch[0] * merge_count + other_touch[0]) / (merge_count + 1)
				merged_touch[1] = (merged_touch[1] * merge_count + other_touch[1]) / (merge_count + 1)
				merged_touch[2] = max(merged_touch[2], other_touch[2])  # Max intensity
				merged_touch[3] = max(merged_touch[3], other_touch[3])  # Max size
				merge_count += 1
				processed.append(j)
		
		merged_touches.append(merged_touch)
		processed.append(i)
	
	current_touches = merged_touches


func create_persistent_trail(position: Vector3, intensity: float, particleSystem: GPUParticles3D):
	"""Create a persistent trail that fades over time"""
	# Check if we should create a new trail or continue an existing one
	var should_create_new = true
	
	# If we have existing trails, check if this touch is close to any of them
	for trail in touch_trails:
		var distance = position.distance_to(trail.position)
		if distance < 0.5:  # Close to existing trail
			# Update existing trail instead of creating new one
			trail.position = position
			trail.intensity = intensity
			trail.time_remaining = TRAIL_DURATION  # Reset timer
			should_create_new = false
			break
	
	# Create new trail if needed
	if should_create_new:
		var new_trail = {
			"position": position,
			"intensity": intensity,
			"time_remaining": TRAIL_DURATION,
			"particle_system": null  # We'll use the main particle system for now
		}
		touch_trails.append(new_trail)
	
	# Update the main particle system to the latest position
	particleSystem.global_position = position
	particleSystem.emitting = true

func update_touch_trails(delta: float):
	"""Update all touch trails and remove expired ones"""
	var trails_to_remove = []
	
	for i in range(touch_trails.size()):
		var trail = touch_trails[i]
		trail.time_remaining -= delta
		
		if trail.time_remaining <= 0:
			trails_to_remove.append(i)
	
	# Remove expired trails (in reverse order to maintain indices)
	for i in range(trails_to_remove.size() - 1, -1, -1):
		touch_trails.remove_at(trails_to_remove[i])
	
	# If no trails left, stop emitting
	if touch_trails.size() == 0:
		particles1.emitting = false
		particles2.emitting = false
		particles3.emitting = false
		particles4.emitting = false
		particles5.emitting = false
		particles6.emitting = false
		particles7.emitting = false
		particles8.emitting = false
		particles9.emitting = false
		particles10.emitting = false
		particles11.emitting = false
		particles12.emitting = false
		particles13.emitting = false
		particles14.emitting = false
		particles15.emitting = false
		particles16.emitting = false
		particles17.emitting = false
		particles18.emitting = false
		particles19.emitting = false
		particles20.emitting = false


func _on_blob_tracking_track_data(tracking: Variant) -> void:
	current_touches = tracking.duplicate(true)
