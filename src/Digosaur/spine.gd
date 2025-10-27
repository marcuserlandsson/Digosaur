extends Node3D

@export var bone_id: String = "spine"

func _ready():
	add_to_group("bone_collectibles")
	# Connect all Area3D nodes for touch detection
	$spine_model/spine/Cube_001/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_003/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_004/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_005/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_006/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_007/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_008/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_009/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_010/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_011/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_012/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_013/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_002/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_035/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_036/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_037/Area3D.input_event.connect(_on_input_event)
	$spine_model/spine/Cube_038/Area3D.input_event.connect(_on_input_event)

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
	$spine_model/spine/Cube_001/Area3D.monitoring = false 
	$spine_model/spine/Cube_003/Area3D.monitoring = false
	$spine_model/spine/Cube_004/Area3D.monitoring = false
	$spine_model/spine/Cube_005/Area3D.monitoring = false
	$spine_model/spine/Cube_006/Area3D.monitoring = false
	$spine_model/spine/Cube_007/Area3D.monitoring = false
	$spine_model/spine/Cube_008/Area3D.monitoring = false
	$spine_model/spine/Cube_009/Area3D.monitoring = false
	$spine_model/spine/Cube_010/Area3D.monitoring = false
	$spine_model/spine/Cube_011/Area3D.monitoring = false
	$spine_model/spine/Cube_012/Area3D.monitoring = false
	$spine_model/spine/Cube_013/Area3D.monitoring = false
	$spine_model/spine/Cube/Area3D.monitoring = false
	$spine_model/spine/Cube_002/Area3D.monitoring = false
	$spine_model/spine/Cube_035/Area3D.monitoring = false
	$spine_model/spine/Cube_036/Area3D.monitoring = false
	$spine_model/spine/Cube_037/Area3D.monitoring = false
	$spine_model/spine/Cube_038/Area3D.monitoring = false
	print("Bone hidden:", bone_id)