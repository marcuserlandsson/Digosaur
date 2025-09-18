extends Node

# TCP Client for connecting to the Surface table touch detection server
var tcp_client: StreamPeerTCP
var is_connected = false
var server_ip = "127.0.0.1"
var server_port = 666

func _ready():
	print("TCP Client: Starting connection to Surface table server...")
	connect_to_server()

func connect_to_server():
	tcp_client = StreamPeerTCP.new()
	
	# Try to connect to the server
	var result = tcp_client.connect_to_host(server_ip, server_port)
	
	if result == OK:
		print("TCP Client: Connected to server at ", server_ip, ":", server_port)
		is_connected = true
		
		# Start requesting touch data
		request_touch_data()
	else:
		print("TCP Client: Failed to connect to server. Error: ", result)
		# Retry connection after 2 seconds
		await get_tree().create_timer(2.0).timeout
		connect_to_server()

func request_touch_data():
	if not is_connected:
		return
	
	# Send request 4 (4-byte integer) to get touch data
	var request_data = PackedByteArray()
	request_data.resize(4)
	request_data.encode_u32(0, 4)  # Encode 4 as 32-bit integer
	
	var bytes_sent = tcp_client.put_data(request_data)
	if bytes_sent == 4:
		print("TCP Client: Sent request 4 for touch data")
		# Wait a bit then read the response
		await get_tree().create_timer(0.1).timeout
		read_touch_response()
	else:
		print("TCP Client: Failed to send request. Bytes sent: ", bytes_sent)

func read_touch_response():
	if not is_connected:
		return
	
	# First read the data length (4 bytes)
	var length_data = tcp_client.get_data(4)
	if length_data[1] != 4:  # Check if we got 4 bytes
		print("TCP Client: Failed to read data length")
		return
	
	var data_length = length_data[0].decode_u32(0)
	print("TCP Client: Data length: ", data_length)
	
	# Read the actual touch data
	var touch_data = tcp_client.get_data(data_length)
	if touch_data[1] != data_length:
		print("TCP Client: Failed to read touch data")
		return
	
	# Convert bytes to string
	var touch_string = touch_data[0].get_string_from_utf8()
	print("TCP Client: Received touch data: ", touch_string)
	
	# Parse the touch data
	parse_touch_data(touch_string)
	
	# Request more data after a short delay
	await get_tree().create_timer(0.1).timeout
	request_touch_data()

func parse_touch_data(touch_string: String):
	if touch_string == "release":
		print("TCP Client: No touches detected")
		return
	
	if touch_string.begins_with("multi_touch:"):
		var touch_data = touch_string.substr(12)  # Remove "multi_touch:" prefix
		var touches = touch_data.split("|")
		
		print("TCP Client: Detected ", touches.size(), " touch(es):")
		for i in range(touches.size()):
			var touch_parts = touches[i].split(":")
			if touch_parts.size() >= 4:
				var x = int(touch_parts[0])
				var y = int(touch_parts[1])
				var intensity = int(touch_parts[2])
				var size = int(touch_parts[3])
				print("  Touch ", i + 1, ": X=", x, " Y=", y, " Intensity=", intensity, " Size=", size)
	else:
		print("TCP Client: Unknown touch data format: ", touch_string)

func _exit_tree():
	if tcp_client:
		tcp_client.disconnect_from_host()
		print("TCP Client: Disconnected from server")
