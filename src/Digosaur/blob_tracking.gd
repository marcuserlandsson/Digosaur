extends Node

var current_touches:= Array([], TYPE_VECTOR4, "", null)
var previous_touches:= Array([], TYPE_VECTOR4, "", null)


func _on_tcp_client_touch_points(current_touches_input: Variant) -> void:
	previous_touches = current_touches.duplicate(true)
	current_touches = current_touches_input.duplicate(true)
	
