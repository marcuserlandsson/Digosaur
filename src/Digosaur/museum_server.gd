extends Node

# Museum Server - Receives bone collection events from sand scene
var museum_server: TCPServer
var museum_port = 8888
var is_server_running = false

signal bone_received(bone_id: String)

func _ready():
	print("Museum Server: Starting museum server on port ", museum_port)
	start_museum_server()

func start_museum_server():
	museum_server = TCPServer.new()
	var result = museum_server.listen(museum_port, "0.0.0.0")
	
	if result == OK:
		print("Museum Server: Server started successfully on port ", museum_port)
		is_server_running = true
	else:
		print("Museum Server: Failed to start server. Error: ", result)
		# Retry after 5 seconds
		await get_tree().create_timer(5.0).timeout
		start_museum_server()

func _process(delta):
	if not is_server_running:
		return
	
	# Check for new connections
	if museum_server.is_connection_available():
		var client = museum_server.take_connection()
		if client:
			print("Museum Server: New connection from sand scene!")
			handle_client_connection(client)

func handle_client_connection(client: StreamPeerTCP):
	print("Museum Server: Handling client connection...")
	
	# Wait for data
	var attempts = 0
	while attempts < 100:  # Wait up to 10 seconds for data
		client.poll()
		var status = client.get_status()
		
		if status == StreamPeerTCP.STATUS_CONNECTED:
			# Try to read message length (4 bytes)
			var length_data = client.get_data(4)
			if length_data[0] == OK and length_data[1].size() == 4:
				var message_length = length_data[1].decode_u32(0)
				print("Museum Server: Message length: ", message_length)
				
				# Read the actual message
				var message_data = client.get_data(message_length)
				if message_data[0] == OK and message_data[1].size() == message_length:
					var json_string = message_data[1].get_string_from_utf8()
					print("Museum Server: Received message: ", json_string)
					
					# Parse JSON message
					var json = JSON.new()
					var parse_result = json.parse(json_string)
					
					if parse_result == OK:
						var message = json.data
						if message.has("type") and message["type"] == "bone_collected":
							var bone_id = message.get("bone_id", "")
							if bone_id == "connection_test":
								print("🧪 CONNECTION TEST RECEIVED!")
							else:
								print("Museum Server: Bone collected: ", bone_id)
								bone_received.emit(bone_id)
					else:
						print("Museum Server: Failed to parse JSON message")
				else:
					print("Museum Server: Failed to read message data")
			else:
				print("Museum Server: Failed to read message length")
				break
		elif status == StreamPeerTCP.STATUS_ERROR or status == StreamPeerTCP.STATUS_NONE:
			print("Museum Server: Client disconnected")
			break
		
		await get_tree().create_timer(0.1).timeout
		attempts += 1
	
	client.disconnect_from_host()
	print("Museum Server: Client connection closed")

func _exit_tree():
	if museum_server:
		museum_server.stop()
		print("Museum Server: Server stopped")
