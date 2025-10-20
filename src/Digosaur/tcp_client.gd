extends Node

# TCP Client for connecting to the Surface table touch detection server
var tcp_client: StreamPeerTCP
var is_connected = false
var server_ip = "127.0.0.1"
var server_port = 666
var current_touches:= Array([], TYPE_VECTOR4, "", null)  # Store current touch data for other scripts to access
signal touch_points(current_touches)

func _ready():
	#print("TCP Client: Starting connection to Surface table server...")
	connect_to_server()

func connect_to_server():
	tcp_client = StreamPeerTCP.new()
	
	# Try to connect to the server
	var result = tcp_client.connect_to_host(server_ip, server_port)
	
	if result == OK:
		#print("TCP Client: Connection initiated to ", server_ip, ":", server_port)
		
		# Wait for connection to be fully established
		var attempts = 0
		while attempts < 50:  # Wait up to 5 seconds
			tcp_client.poll()
			var status = tcp_client.get_status()
			#print("TCP Client: Connection status: ", status)
			
			if status == StreamPeerTCP.STATUS_CONNECTED:
				#print("TCP Client: Connected to server!")
				is_connected = true
				break
			elif status == StreamPeerTCP.STATUS_ERROR:
				#print("TCP Client: Connection error!")
				return
			
			await get_tree().create_timer(0.1).timeout
			attempts += 1
		
		if is_connected:
			# Start requesting touch data
			request_touch_data()
		else:
			#print("TCP Client: Connection timeout!")
			return
	else:
		#print("TCP Client: Failed to connect to server. Error: ", result)
		# Retry connection after 2 seconds
		await get_tree().create_timer(2.0).timeout
		connect_to_server()

func request_touch_data():
	if not is_connected:
		return
	
	# Check connection status before sending
	var status = tcp_client.get_status()
	#print("TCP Client: Connection status before send: ", status)
	
	# Send request 4 (4-byte integer) to get touch data
	var request_data = PackedByteArray()
	request_data.resize(4)
	# Encode 4 as 32-bit little-endian integer
	request_data[0] = 4 & 0xFF
	request_data[1] = (4 >> 8) & 0xFF
	request_data[2] = (4 >> 16) & 0xFF
	request_data[3] = (4 >> 24) & 0xFF
	
	#print("TCP Client: Sending request 4")
	#print("TCP Client: Request data: ", request_data)
	
	# Try to send the 4-byte request using put_partial_data
	var send_result = tcp_client.put_partial_data(request_data)
	var error_code = send_result[0]  # Error code
	var bytes_sent = send_result[1]   # Bytes actually sent
	
	#print("TCP Client: Send result: ", send_result)
	#print("TCP Client: Error code: ", error_code)
	#print("TCP Client: Bytes sent: ", bytes_sent)
	
	if error_code != OK:
		#print("TCP Client: Failed to send request. Error: ", error_code)
		#print("TCP Client: Connection status after failed send: ", tcp_client.get_status())
		return
	
	if bytes_sent != 4:
		#print("TCP Client: Partial send. Expected 4 bytes, sent: ", bytes_sent)
		return
	
	#print("TCP Client: Successfully sent request 4")
	# Wait a bit then read the response
	await get_tree().create_timer(0.1).timeout
	read_touch_response()

func read_touch_response():
	if not is_connected:
		return
	
	# Read the data length (4 bytes)
	var length_data = tcp_client.get_data(4)
	var length_error = length_data[0]  # int
	var length_bytes = length_data[1]  # PackedByteArray
	
	# Check if we got the data successfully
	if length_error != OK:
		#print("TCP Client: Failed to read data length. Error: ", length_error)
		return
	
	if length_bytes.size() != 4:  # Check if we got 4 bytes
		#print("TCP Client: Failed to read data length. Got: ", length_bytes.size(), " bytes")
		return
	
	# Decode the length as little-endian 32-bit integer
	var data_length = length_bytes.decode_u32(0)
	#print("TCP Client: Data length: ", data_length)
	
	# Read the actual touch data
	var touch_data = tcp_client.get_data(data_length)
	var touch_error = touch_data[0]  # int
	var touch_bytes = touch_data[1]  # PackedByteArray
	
	# Check if we got the touch data successfully
	if touch_error != OK:
		#print("TCP Client: Failed to read touch data. Error: ", touch_error)
		return
	
	if touch_bytes.size() != data_length:
		#print("TCP Client: Failed to read touch data. Expected: ", data_length, " Got: ", touch_bytes.size())
		return
	
	# Convert bytes to string
	var touch_string = touch_bytes.get_string_from_utf8()
	#print("TCP Client: Received touch data: ", touch_string)
	
	# Parse the touch data
	parse_touch_data(touch_string)
	
	# Request more data after a short delay
	await get_tree().create_timer(0.016).timeout
	request_touch_data()

func parse_touch_data(touch_string: String):
	# Clear previous touches
	current_touches.clear()
	
	if touch_string == "release":
		#print("TCP Client: No touches detected")
		touch_points.emit(current_touches)
		return
	
	if touch_string.begins_with("multi_touch:"):
		var touch_data = touch_string.substr(12)  # Remove "multi_touch:" prefix
		var touches = touch_data.split("|")
		
		#print("TCP Client: Detected ", touches.size(), " touch(es):")
		for i in range(touches.size()):
			var touch_parts = touches[i].split(":")
			if touch_parts.size() >= 4:
				var x = int(touch_parts[0])
				var y = int(touch_parts[1])
				var intensity = int(touch_parts[2])
				var size = int(touch_parts[3])
				#print("  Touch ", i + 1, ": X=", x, " Y=", y, " Intensity=", intensity, " Size=", size)
				
				# Store touch data for other scripts to access
				var touch_point = Vector4(x, y, intensity, size)
				current_touches.append(touch_point)
		touch_points.emit(current_touches)
	else:
		return
		#print("TCP Client: Unknown touch data format: ", touch_string)

func _exit_tree():
	if tcp_client:
		tcp_client.disconnect_from_host()
		#print("TCP Client: Disconnected from server")
