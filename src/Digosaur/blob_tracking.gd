extends Node

var current_touches:= Array([], TYPE_VECTOR4, "", null)
var previous_touches:= Array([], TYPE_VECTOR4, "", null)
var tracking:= Array([])
var threshold := float(100.0)
var distances_c_to_p = {}
var distances_p_to_c = {}


signal track_data(tracking)

func _on_tcp_client_touch_points(current_touches_input: Variant) -> void:
	previous_touches = current_touches.duplicate(true)
	current_touches = current_touches_input.duplicate(true)
	tracking_function(previous_touches, current_touches)
	track_data.emit(tracking)
	
func tracking_function(previous: Array, current: Array) -> void:
	distances_c_to_p.clear()
	distances_p_to_c.clear()
	tracking.clear()
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
	for i in distances_c_to_p.keys():
		var shortest = INF
		var best = Array()
		#print("New touchPoint: ", i)
		for j in distances_p_to_c.keys():
			#print("Previous touchPoint: ", j)
			#print(distances_c_to_p[i][j], " < ", shortest, " = ", distances_c_to_p[i][j] < shortest)
			#print(distances_c_to_p[i][j], " <= ", threshold, " = ", distances_c_to_p[i][j] <= threshold)
			#print(distances_c_to_p[i][j], " < ", distances_p_to_c[j].min(), " = ", distances_c_to_p[i][j] <= distances_p_to_c[j].min())
			#print("All tests: ", distances_c_to_p[i][j] < shortest and distances_c_to_p[i][j] <= threshold and distances_c_to_p[i][j] <= distances_p_to_c[j].min())
			if distances_c_to_p[i][j] < shortest and distances_c_to_p[i][j] <= threshold and distances_c_to_p[i][j] <= distances_p_to_c[j].min():
				best = Array([current[i][0], current[i][1], current[i][2], current[i][3], Vector2(current[i][0] - previous[j][0], current[i][1] - previous[j][1])])
				shortest = distances_c_to_p[i][j]
				#print("Succeeded, adding: ", Array([current[i][0], current[i][1], current[i][2], current[i][3], Vector2(current[i][0] - previous[j][0], current[i][1] - previous[j][1])]))
			#else:
				#tracking.append(Array([current[i][0], current[i][1], current[i][2], current[i][3], Vector2(-1, -1)]))
				#print("Failed, adding: ", Array([current[i][0], current[i][1], current[i][2], current[i][3], Vector2(-1, -1)]))
		if shortest == INF:
			tracking.append(Array([current[i][0], current[i][1], current[i][2], current[i][3], Vector2(-1, -1)]))
		else:
			tracking.append(best)
