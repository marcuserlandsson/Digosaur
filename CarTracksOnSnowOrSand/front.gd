extends Node3D

@export var bone_id: String = "front"

func _ready():
	$front_legs/Plane_002/Area3D.input_event.connect(_on_input_event)
	$front_legs/Plane_003/Area3D.input_event.connect(_on_input_event)
	$front_legs/Plane_004/Area3D.input_event.connect(_on_input_event)
	$front_legs/front_leg_down/Area3D.input_event.connect(_on_input_event)
	$front_legs/front_leg_up/Area3D.input_event.connect(_on_input_event)

func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		print("Clicked:", bone_id)
		Global.add_bone(bone_id)
		print("Bones array now:", Global.bones)
		#get_tree().change_scene_to_file("res://museum.tscn")
		hide_bone()
		
func hide_bone():
	visible = false
	$front_legs/Plane_002/Area3D.monitoring = false 
	$front_legs/Plane_003/Area3D.monitoring = false 
	$front_legs/Plane_004/Area3D.monitoring = false 
	$front_legs/front_leg_down/Area3D.monitoring = false 
	$front_legs/front_leg_up/Area3D.monitoring = false  
	print("Bone hidden:", bone_id)
