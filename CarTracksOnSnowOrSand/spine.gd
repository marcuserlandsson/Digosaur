extends Node3D

@export var bone_id: String = "spine"

func _ready():
	$spine/Cube_001/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_003/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_004/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_005/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_006/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_007/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_008/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_009/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_010/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_011/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_012/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_013/Area3D.input_event.connect(_on_input_event)
	$spine/Cube/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_002/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_035/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_036/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_037/Area3D.input_event.connect(_on_input_event)
	$spine/Cube_038/Area3D.input_event.connect(_on_input_event)
	

func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		print("Clicked:", bone_id)
		Global.add_bone(bone_id)
		print("Bones array now:", Global.bones)
		#get_tree().change_scene_to_file("res://museum.tscn")
		hide_bone()
		
func hide_bone():
	visible = false
	$spine/Cube_001/Area3D.monitoring = false 
	$spine/Cube_003/Area3D.monitoring = false
	$spine/Cube_004/Area3D.monitoring = false
	$spine/Cube_005/Area3D.monitoring = false
	$spine/Cube_006/Area3D.monitoring = false
	$spine/Cube_007/Area3D.monitoring = false
	$spine/Cube_008/Area3D.monitoring = false
	$spine/Cube_009/Area3D.monitoring = false
	$spine/Cube_010/Area3D.monitoring = false
	$spine/Cube_011/Area3D.monitoring = false
	$spine/Cube_012/Area3D.monitoring = false
	$spine/Cube_013/Area3D.monitoring = false
	$spine/Cube/Area3D.monitoring = false
	$spine/Cube_002/Area3D.monitoring = false
	$spine/Cube_035/Area3D.monitoring = false
	$spine/Cube_036/Area3D.monitoring = false
	$spine/Cube_037/Area3D.monitoring = false
	$spine/Cube_038/Area3D.monitoring = false
	 
	print("Bone hidden:", bone_id)
