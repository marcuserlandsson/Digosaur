extends Node3D

@export var bone_id: String = "ribs"

func _ready():
	$ribcage/Cylinder_004/Area3D.input_event.connect(_on_input_event)
	$ribcage/Cylinder_005/Area3D.input_event.connect(_on_input_event)
	$ribcage/Cylinder_006/Area3D.input_event.connect(_on_input_event)
	$ribcage/Cylinder_007/Area3D.input_event.connect(_on_input_event)
	$ribcage/Cube_039/Area3D.input_event.connect(_on_input_event)
	$ribcage/Cube_040/Area3D.input_event.connect(_on_input_event)

func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		print("Clicked:", bone_id)
		Global.add_bone(bone_id)
		print("Bones array now:", Global.bones)
		#get_tree().change_scene_to_file("res://museum.tscn")
		hide_bone()
		
func hide_bone():
	visible = false
	$ribcage/Cylinder_004/Area3D.monitoring = false 
	$ribcage/Cylinder_005/Area3D.monitoring = false 
	$ribcage/Cylinder_006/Area3D.monitoring = false 
	$ribcage/Cylinder_007/Area3D.monitoring = false 
	$ribcage/Cube_039/Area3D.monitoring = false 
	$ribcage/Cube_040/Area3D.monitoring = false 
	print("Bone hidden:", bone_id)
