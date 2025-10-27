extends Node3D

@export var bone_id: String = "front"

func _ready():
	add_to_group("bone_collectibles")
	# Connect all Area3D nodes for touch detection
	$front_model/front_legs/Plane_002/Area3D.input_event.connect(_on_input_event)
	$front_model/front_legs/Plane_003/Area3D.input_event.connect(_on_input_event)
	$front_model/front_legs/Plane_004/Area3D.input_event.connect(_on_input_event)
	$front_model/front_legs/front_leg_down/Area3D.input_event.connect(_on_input_event)
	$front_model/front_legs/front_leg_up/Area3D.input_event.connect(_on_input_event)

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
	$front_model/front_legs/Plane_002/Area3D.monitoring = false 
	$front_model/front_legs/Plane_003/Area3D.monitoring = false 
	$front_model/front_legs/Plane_004/Area3D.monitoring = false 
	$front_model/front_legs/front_leg_down/Area3D.monitoring = false 
	$front_model/front_legs/front_leg_up/Area3D.monitoring = false  
	print("Bone hidden:", bone_id)