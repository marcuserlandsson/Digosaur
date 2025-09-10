# Surface table track script
# Creates tracks from Surface table touch input via UDP bridge
extends Node3D
class_name SurfaceTrack

@onready var cam: Camera3D = $"../Camera3D"
@onready var particles: CPUParticles3D = $"../SubViewport/RemoteParticles/TrackParticles"
@onready var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state

const INTERACT_RADIUS: int = 15
const UDP_PORT: int = 12345

var query := PhysicsRayQueryParameters3D.new()
var touch_position: Vector3
var is_touching: bool = false
var udp_server: UDPServer
var udp_packet_peer: PacketPeerUDP

func _ready():
	query.set_collide_with_areas(true)
	# Start with particles disabled
	particles.emitting = false
	
	# Setup UDP server to receive touch data from Surface bridge
	udp_server = UDPServer.new()
	udp_server.listen(UDP_PORT, "127.0.0.1")
	print("Surface track system ready. Listening for touch data on port ", UDP_PORT)

func _exit_tree():
	if udp_server:
		udp_server.stop()

func _physics_process(delta: float):
	# Check for incoming UDP packets
	if udp_server.is_connection_available():
		udp_packet_peer = udp_server.take_connection()
	
	if udp_packet_peer and udp_packet_peer.get_available_packet_count() > 0:
		var packet = udp_packet_peer.get_packet()
		var message = packet.get_string_from_utf8()
		_handle_touch_message(message)
	
	# Only update position when touching
	if is_touching:
		# Convert screen coordinates to world space
		var result: Dictionary = _screen_to_world_space(touch_position)
		if result:
			# Move particles to touch position
			particles.global_position = result.position

func _handle_touch_message(message: String):
	var parts = message.split(":")
	if parts.size() >= 4:
		var event_type = parts[0]
		var x = float(parts[1])
		var y = float(parts[2])
		var id = int(parts[3])
		
		print("Touch event: ", event_type, " at (", x, ", ", y, ") ID: ", id)
		
		match event_type:
			"DOWN":
				is_touching = true
				particles.emitting = true
				touch_position = Vector2(x, y)
			"MOVE":
				if is_touching:
					touch_position = Vector2(x, y)
			"UP":
				is_touching = false
				particles.emitting = false

func _screen_to_world_space(screen_pos: Vector2) -> Dictionary:
	# Convert screen coordinates to world space using raycasting
	# This is similar to the mouse raycasting but uses the touch position
	query.from = cam.global_position
	query.to = query.from + _get_world_ray_from_screen(screen_pos)
	return space_state.intersect_ray(query)

func _get_world_ray_from_screen(screen_pos: Vector2) -> Vector3:
	# Convert screen position to world ray
	# We need to project the screen position to a world ray
	var viewport_size = get_viewport().size
	var normalized_pos = Vector2(screen_pos.x / viewport_size.x, screen_pos.y / viewport_size.y)
	return cam.project_ray_normal(normalized_pos) * INTERACT_RADIUS

