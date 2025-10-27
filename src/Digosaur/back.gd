extends Node3D

@export var bone_id: String = "back"

func _ready():
	add_to_group("bone_collectibles")
	# Connect all Area3D nodes for touch detection
	$back_model/back_legs/Plane_005/Area3D.input_event.connect(_on_input_event)
	$back_model/back_legs/Plane_006/Area3D.input_event.connect(_on_input_event)
	$back_model/back_legs/Plane_007/Area3D.input_event.connect(_on_input_event)
	$back_model/back_legs/back_leg_down/Area3D.input_event.connect(_on_input_event)
	$back_model/back_legs/back_leg_up/Area3D.input_event.connect(_on_input_event)

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
	$back_model/back_legs/Plane_005/Area3D.monitoring = false 
	$back_model/back_legs/Plane_006/Area3D.monitoring = false 
	$back_model/back_legs/Plane_007/Area3D.monitoring = false 
	$back_model/back_legs/back_leg_down/Area3D.monitoring = false 
	$back_model/back_legs/back_leg_up/Area3D.monitoring = false  
	print("Bone hidden:", bone_id)