extends Node3D

@export var bone_id: String = "tail"

func _ready():
	add_to_group("bone_collectibles")
	# Connect all Area3D nodes for touch detection
	$tail_model/tail/Cube_014/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Cube_015/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Cube_016/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Cube_017/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Cube_018/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Cube_019/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Cube_020/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Cube_033/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Cube_034/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Plane/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Plane_001/Area3D.input_event.connect(_on_input_event)
	$tail_model/tail/Plane_008/Area3D.input_event.connect(_on_input_event)

func _on_input_event(camera, event, position, normal, shape_idx):
	# Check if this is a touch event (not mouse)
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		collect_bone()

func collect_bone():
	print("Collected bone:", bone_id)
	Global.add_bone(bone_id)
	hide_bone()
	
func hide_bone():
	visible = false
	# Disable all Area3D monitoring
	$tail_model/tail/Cube_014/Area3D.monitoring = false
	$tail_model/tail/Cube_015/Area3D.monitoring = false
	$tail_model/tail/Cube_016/Area3D.monitoring = false
	$tail_model/tail/Cube_017/Area3D.monitoring = false
	$tail_model/tail/Cube_018/Area3D.monitoring = false
	$tail_model/tail/Cube_019/Area3D.monitoring = false
	$tail_model/tail/Cube_020/Area3D.monitoring = false
	$tail_model/tail/Cube_033/Area3D.monitoring = false
	$tail_model/tail/Cube_034/Area3D.monitoring = false
	$tail_model/tail/Plane/Area3D.monitoring = false
	$tail_model/tail/Plane_001/Area3D.monitoring = false
	$tail_model/tail/Plane_008/Area3D.monitoring = false
	print("Bone hidden:", bone_id)
