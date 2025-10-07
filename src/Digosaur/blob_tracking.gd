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
		for j in previous:
			if !distances_p_to_c[j]:
				distances_p_to_c[j] = Array([])
			var previous_point = Vector2(j[0], j[1])
			var distance = (current_point - previous_point).length()
			distances_c_to_p[i].append(distance)
			distances_p_to_c[j].append(distance)
	for i in distances_c_to_p.keys():
		var shortest = INF
		for j in range(len(distances_c_to_p[i])):
			if distances_c_to_p[i][j] < shortest and distances_c_to_p[i][j] <= threshold and distances_c_to_p[i][j] < min(distances_p_to_c[j]):
				tracking.append(Array([current[i][0], current[i][1], current[i][2], current[i][3], Vector2(current[i][0] - previous[j][0], current[i][1] - previous[j][1])]))
			else:
				tracking.append(Array([current[i][0], current[i][1], current[i][2], current[i][3], "new"]))
			
