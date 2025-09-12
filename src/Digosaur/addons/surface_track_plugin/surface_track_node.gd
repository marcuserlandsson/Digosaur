extends Node
class_name SurfaceTrackNode

# UDP communication
var udp_server: UDPServer
var current_touches: Array[Dictionary] = []

# Touch data
var touch_count: int = 0
var touch_positions: Array[Vector2] = []
var touch_sizes: Array[Vector2] = []
var touch_active: Array[bool] = []

# Signals
signal touch_detected(touch_index: int, position: Vector2, size: Vector2)
signal touch_moved(touch_index: int, position: Vector2, size: Vector2)
signal touch_ended(touch_index: int)

func _ready():
	load_dll()

func _exit_tree():
	unload_dll()

func load_dll():
	# Use UDP communication instead of DLL loading
	# This is much simpler and more reliable than GDExtension
	
	# Start UDP server to receive touch data from C# bridge
	udp_server = UDPServer.new()
	if udp_server.listen(12345) != OK:
		push_error("Failed to start UDP server on port 12345")
		return false
	
	print("UDP server started on port 12345 - waiting for Surface bridge...")
	return true

func unload_dll():
	if udp_server != null:
		udp_server.stop()
		udp_server = null
		print("UDP server stopped")

func initialize_surface(hwnd: int) -> bool:
	if init_func == null:
		push_error("DLL not loaded")
		return false
	
	# Simulate successful initialization
	print("Surface initialized with hwnd: " + str(hwnd))
	return true


func _process(delta):
	if udp_server == null:
		return
	
	# Check for UDP messages
	udp_server.poll()
	if udp_server.is_connection_available():
		var peer = udp_server.take_connection()
		var message = peer.get_var()
		handle_udp_message(message)
	
	# Update touch data
	update_touch_data()

func handle_udp_message(message: String):
	# Parse UDP message from C# bridge
	# Format: "DOWN:X:Y:ID" or "UP:ID" or "MOVE:X:Y:ID"
	var parts = message.split(":")
	if parts.size() < 2:
		return
	
	var action = parts[0]
	
	if action == "DOWN" and parts.size() >= 4:
		# Touch down
		var x = float(parts[1])
		var y = float(parts[2])
		var id = int(parts[3])
		
		var touch = {
			"id": id,
			"position": Vector2(x, y),
			"size": Vector2(10, 10),  # Default size
			"active": true
		}
		current_touches.append(touch)
		touch_detected.emit(current_touches.size() - 1, touch.position, touch.size)
		
	elif action == "UP" and parts.size() >= 2:
		# Touch up
		var id = int(parts[1])
		for i in range(current_touches.size()):
			if current_touches[i]["id"] == id:
				current_touches[i]["active"] = false
				touch_ended.emit(i)
				break
				
	elif action == "MOVE" and parts.size() >= 4:
		# Touch move
		var x = float(parts[1])
		var y = float(parts[2])
		var id = int(parts[3])
		
		for i in range(current_touches.size()):
			if current_touches[i]["id"] == id:
				current_touches[i]["position"] = Vector2(x, y)
				touch_moved.emit(i, Vector2(x, y), current_touches[i]["size"])
				break

func get_touch_count() -> int:
	return current_touches.size()

func handle_touch_count_change(new_count: int):
	var old_count = touch_count
	touch_count = new_count
	
	# Resize arrays
	touch_positions.resize(touch_count)
	touch_sizes.resize(touch_count)
	touch_active.resize(touch_count)
	
	# Handle new touches
	for i in range(old_count, touch_count):
		touch_positions[i] = get_touch_position(i)
		touch_sizes[i] = get_touch_size(i)
		touch_active[i] = true
		touch_detected.emit(i, touch_positions[i], touch_sizes[i])
	
	# Handle ended touches
	for i in range(touch_count, old_count):
		touch_ended.emit(i)

func update_touch_data():
	for i in range(touch_count):
		var new_pos = get_touch_position(i)
		var new_size = get_touch_size(i)
		var new_active = is_touch_active(i)
		
		if new_pos != touch_positions[i] or new_size != touch_sizes[i]:
			touch_positions[i] = new_pos
			touch_sizes[i] = new_size
			touch_moved.emit(i, new_pos, new_size)
		
		touch_active[i] = new_active

func get_touch_position(index: int) -> Vector2:
	if index >= current_touches.size():
		return Vector2.ZERO
	
	return current_touches[index]["position"]

func get_touch_size(index: int) -> Vector2:
	if index >= current_touches.size():
		return Vector2.ZERO
	
	return current_touches[index]["size"]

func is_touch_active(index: int) -> bool:
	if index >= current_touches.size():
		return false
	
	return current_touches[index]["active"]
