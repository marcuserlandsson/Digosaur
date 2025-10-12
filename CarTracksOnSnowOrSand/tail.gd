extends Node3D

@export var bone_id: String = "tail"

func _ready():
	$tail/Cube_014/Area3D.input_event.connect(_on_input_event)
	$tail/Cube_015/Area3D.input_event.connect(_on_input_event)
	$tail/Cube_016/Area3D.input_event.connect(_on_input_event)
	$tail/Cube_017/Area3D.input_event.connect(_on_input_event)
	$tail/Cube_018/Area3D.input_event.connect(_on_input_event)
	$tail/Cube_019/Area3D.input_event.connect(_on_input_event)
	$tail/Cube_020/Area3D.input_event.connect(_on_input_event)
	$tail/Cube_033/Area3D.input_event.connect(_on_input_event)
	$tail/Cube_034/Area3D.input_event.connect(_on_input_event)
	$tail/Plane/Area3D.input_event.connect(_on_input_event)
	$tail/Plane_001/Area3D.input_event.connect(_on_input_event)
	$tail/Plane_008/Area3D.input_event.connect(_on_input_event)

func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		print("Clicked:", bone_id)
		Global.add_bone(bone_id)
		print("Bones array now:", Global.bones)
		#get_tree().change_scene_to_file("res://museum.tscn")
		hide_bone()
		
func hide_bone():
	visible = false
	$tail/Cube_014/Area3D.monitoring = false
	$tail/Cube_015/Area3D.monitoring = false
	$tail/Cube_016/Area3D.monitoring = false
	$tail/Cube_017/Area3D.monitoring = false
	$tail/Cube_018/Area3D.monitoring = false
	$tail/Cube_019/Area3D.monitoring = false
	$tail/Cube_020/Area3D.monitoring = false
	$tail/Cube_033/Area3D.monitoring = false
	$tail/Cube_034/Area3D.monitoring = false
	$tail/Plane/Area3D.monitoring = false
	$tail/Plane_001/Area3D.monitoring = false
	$tail/Plane_008/Area3D.monitoring = false
	print("Bone hidden:", bone_id)
