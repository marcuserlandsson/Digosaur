extends Node3D

@export var bone_id: String = ""

func _ready():
	$Area3D.input_event.connect(_on_input_event)

func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		print("Clicked:", bone_id)
		Global.bones.append("dogbone3")
		get_tree().change_scene_to_file("res://Museum.tscn")
