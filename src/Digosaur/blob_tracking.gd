extends Node

var current_touches:= Array([], TYPE_VECTOR4, "", null)
var previous_touches:= Array([], TYPE_VECTOR4, "", null)
var tracking:= Array([]) # Holds data on current touchPoints, including tracking data 
var threshold := float(100.0) # Furthest allowed distance for tracking a touchPoint
# Two dictionaries that contain the distances form previous to new points
# c_to_p has current points' ids as keys and an array or distances to each previous point as value
# p_to_c has previous points' ids as keys and an array or distances to each current point as value 
var distances_c_to_p = {}
var distances_p_to_c = {}


signal track_data(tracking)

func _on_tcp_client_touch_points(current_touches_input: Variant) -> void:
	previous_touches = current_touches.duplicate(true)
	current_touches = current_touches_input.duplicate(true)
	tracking_function(previous_touches, current_touches)
	track_data.emit(tracking)
	
func tracking_function(previous: Array, current: Array) -> void:
	# Clear data from last frame
	distances_c_to_p.clear()
	distances_p_to_c.clear()
	tracking.clear()
	# Fill out the distance dictionaries for the current frame
	for i in range(len(current)):
		var current_point = Vector2(current[i][0], current[i][1])
		distances_c_to_p[i] = Array([])
		for j in range(len(previous)):
			if !distances_p_to_c.has(j):
				distances_p_to_c[j] = Array([])
			var previous_point = Vector2(previous[j][0], previous[j][1])
			var distance = (current_point - previous_point).length()
			distances_c_to_p[i].append(distance)
			distances_p_to_c[j].append(distance)
	# Calculate which current touchPoints are new and which ones are previous ones that have moved
	for i in distances_c_to_p.keys():
		# Will contain the shortest elligable distance between the current touchPoint and a previous one
		var shortest = INF 
		# Will contain current touchPoint data with calculated previous touchPoint location
		var current_point_tracking = Array()
		for j in distances_p_to_c.keys():
			# Check if the distance between the points is:
			# 1. Shorter than the currently known shortest elligable distance for the current touchPoint
			# 2. Within the distance threshold 
			# 3. The shortest distance from the previous touchPoint to any current one
			if distances_c_to_p[i][j] < shortest and distances_c_to_p[i][j] <= threshold and distances_c_to_p[i][j] <= distances_p_to_c[j].min():
				current_point_tracking = Array([current[i][0], current[i][1], current[i][2], current[i][3], Vector2(previous[j][0], previous[j][1])])
				shortest = distances_c_to_p[i][j]
		# If shortest is is still INF, the current touchPoint had no previous location and is therefore new
		if shortest == INF:
			# Set the previous location of new points to [-1, -1], touch_track.gd will recognize this as
			# the mark of a new point. Since the minimum value in both the x-axis and y-axis is 0, it
			# shouldn't be possible for the previous touchPoint to have this location otherwise 
			tracking.append(Array([current[i][0], current[i][1], current[i][2], current[i][3], Vector2(-1, -1)]))
		else:
			tracking.append(current_point_tracking)
