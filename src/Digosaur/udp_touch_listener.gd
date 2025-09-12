extends Node

# Simple UDP listener for Surface table touch data
var udp_server: UDPServer
var current_touches: Array[Dictionary] = []

# Signals
signal touch_detected(touch_index: int, position: Vector2, size: Vector2)
signal touch_moved(touch_index: int, position: Vector2, size: Vector2)
signal touch_ended(touch_index: int)

func _ready():
	# Start UDP server to receive touch data from C# bridge
	udp_server = UDPServer.new()
	if udp_server.listen(12345) != OK:
		push_error("Failed to start UDP server on port 12345")
		return
	
	print("UDP server started on port 12345 - waiting for Surface bridge...")

func _exit_tree():
	if udp_server != null:
		udp_server.stop()
		print("UDP server stopped")

func _process(delta):
	if udp_server == null:
		return
	
	# Check for UDP messages
	udp_server.poll()
	if udp_server.is_connection_available():
		var peer = udp_server.take_connection()
		var message = peer.get_var()
		handle_udp_message(message)

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
		print("Touch detected: ", current_touches.size() - 1, " at ", touch.position)
		
	elif action == "UP" and parts.size() >= 2:
		# Touch up
		var id = int(parts[1])
		for i in range(current_touches.size()):
			if current_touches[i]["id"] == id:
				current_touches[i]["active"] = false
				touch_ended.emit(i)
				print("Touch ended: ", i)
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
