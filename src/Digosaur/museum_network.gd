extends Node

# Museum Network Communication - Sends bone collection events to museum machine
var museum_client: StreamPeerTCP
var is_connected_to_museum = false
var museum_ip = "192.168.1.11"  # Museum machine IP
var museum_port = 8888
var connection_attempts = 0
var max_connection_attempts = 10

signal museum_connected
signal museum_disconnected

func _ready():
	print("Museum Network: Starting museum connection...")
	connect_to_museum()
	
	# Connection test commented out - we know it works!
	# while true:
	#	await get_tree().create_timer(10.0).timeout
	#	if is_connected_to_museum:
	#		send_bone_collected("connection_test")

func connect_to_museum():
	if connection_attempts >= max_connection_attempts:
		print("Museum Network: Max connection attempts reached. Retrying in 30 seconds...")
		await get_tree().create_timer(30.0).timeout
		connection_attempts = 0
	
	museum_client = StreamPeerTCP.new()
	var result = museum_client.connect_to_host(museum_ip, museum_port)
	
	if result == OK:
		print("Museum Network: Connection initiated to museum at ", museum_ip, ":", museum_port)
		
		# Wait for connection to be established
		var attempts = 0
		while attempts < 50:  # Wait up to 5 seconds
			museum_client.poll()
			var status = museum_client.get_status()
			
			if status == StreamPeerTCP.STATUS_CONNECTED:
				print("Museum Network: Connected to museum!")
				is_connected_to_museum = true
				connection_attempts = 0
				museum_connected.emit()
				break
			elif status == StreamPeerTCP.STATUS_ERROR:
				print("Museum Network: Connection error!")
				break
			
			await get_tree().create_timer(0.1).timeout
			attempts += 1
		
		if not is_connected_to_museum:
			print("Museum Network: Connection timeout!")
			connection_attempts += 1
			# Retry connection after 5 seconds
			await get_tree().create_timer(5.0).timeout
			connect_to_museum()
	else:
		print("Museum Network: Failed to connect to museum. Error: ", result)
		connection_attempts += 1
		# Retry connection after 5 seconds
		await get_tree().create_timer(5.0).timeout
		connect_to_museum()

func send_bone_collected(bone_id: String):
	print("Museum Network: Attempting to send bone:", bone_id)
	print("Museum Network: Connection status:", is_connected_to_museum)
	
	if not is_connected_to_museum:
		print("Museum Network: Not connected to museum, cannot send bone:", bone_id)
		return
	
	# Create JSON message
	var message = {
		"type": "bone_collected",
		"bone_id": bone_id,
		"timestamp": Time.get_datetime_string_from_system(),
		"total_bones": Global.bones.size()
	}
	
	var json_string = JSON.stringify(message)
	var json_bytes = json_string.to_utf8_buffer()
	
	# Send message length first (4 bytes)
	var length_data = PackedByteArray()
	length_data.resize(4)
	length_data.encode_u32(0, json_bytes.size())
	
	var send_result = museum_client.put_partial_data(length_data)
	if send_result[0] != OK or send_result[1] != 4:
		print("Museum Network: Failed to send message length")
		handle_connection_error()
		return
	
	# Send JSON message
	send_result = museum_client.put_partial_data(json_bytes)
	if send_result[0] != OK or send_result[1] != json_bytes.size():
		print("Museum Network: Failed to send bone data")
		handle_connection_error()
		return
	
	print("Museum Network: Sent bone collection to museum:", bone_id)

func send_game_reset():
	if not is_connected_to_museum:
		print("Museum Network: Not connected to museum, cannot send reset")
		return
	
	# Create JSON message for game reset
	var message = {
		"type": "game_reset",
		"timestamp": Time.get_datetime_string_from_system()
	}
	
	var json_string = JSON.stringify(message)
	var json_bytes = json_string.to_utf8_buffer()
	
	# Send message length first (4 bytes)
	var length_data = PackedByteArray()
	length_data.resize(4)
	length_data.encode_u32(0, json_bytes.size())
	
	var send_result = museum_client.put_partial_data(length_data)
	if send_result[0] != OK or send_result[1] != 4:
		print("Museum Network: Failed to send reset message length")
		handle_connection_error()
		return
	
	# Send JSON message
	send_result = museum_client.put_partial_data(json_bytes)
	if send_result[0] != OK or send_result[1] != json_bytes.size():
		print("Museum Network: Failed to send reset data")
		handle_connection_error()
		return
	
	print("Museum Network: Sent game reset to museum")

func handle_connection_error():
	print("Museum Network: Connection error, attempting to reconnect...")
	is_connected_to_museum = false
	museum_disconnected.emit()
	await get_tree().create_timer(2.0).timeout
	connect_to_museum()

func _exit_tree():
	if museum_client:
		museum_client.disconnect_from_host()
		print("Museum Network: Disconnected from museum")
