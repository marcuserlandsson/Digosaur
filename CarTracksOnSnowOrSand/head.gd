extends Node3D

@export var bone_id: String = "head"

func _ready():
	$head2/Cube_041/Area3D.input_event.connect(_on_input_event)
	$head2/head/Area3D.input_event.connect(_on_input_event)

func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		print("Clicked:", bone_id)
		Global.add_bone(bone_id)
		print("Bones array now:", Global.bones)
		#get_tree().change_scene_to_file("res://museum.tscn")
		hide_bone()
		
func hide_bone():
	visible = false
	$head2/Cube_041/Area3D.monitoring = false 
	$head2/head/Area3D.monitoring = false 
	print("Bone hidden:", bone_id)
