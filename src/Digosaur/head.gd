extends Node3D

@export var bone_id: String = "head"

func _ready():
	add_to_group("bone_collectibles")
	# Connect all Area3D nodes for touch detection
	$head_model/head2/Cube_041/Area3D.input_event.connect(_on_input_event)
	$head_model/head2/head/Area3D.input_event.connect(_on_input_event)

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
	$head_model/head2/Cube_041/Area3D.monitoring = false 
	$head_model/head2/head/Area3D.monitoring = false 
	print("Bone hidden:", bone_id)