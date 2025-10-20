extends Node3D

@export var bone_id: String = "back"

func _ready():
	# Add to bone collectibles group for touch detection
	add_to_group("bone_collectibles")
	
	# Connect to all Area3D collision shapes for back legs
	var area_paths = [
		"back_legs/Plane_005/Area3D",
		"back_legs/Plane_006/Area3D",
		"back_legs/Plane_007/Area3D",
		"back_legs/back_leg_down/Area3D",
		"back_legs/back_leg_up/Area3D"
	]
	
	for path in area_paths:
		if has_node(path):
			var area = get_node(path)
			area.input_event.connect(_on_input_event)
			area.area_entered.connect(_on_area_entered)
			area.body_entered.connect(_on_body_entered)

func _on_input_event(camera, event, position, normal, shape_idx):
	# Handle direct input events (fallback)
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		collect_bone()

func _on_area_entered(area):
	# Handle area overlap (for touch detection)
	collect_bone()

func _on_body_entered(body):
	# Handle body overlap (for touch detection)
	collect_bone()

func collect_bone():
	print("Collected bone:", bone_id)
	Global.add_bone(bone_id)
	hide_bone()

func hide_bone():
	visible = false
	var area_paths = [
		"back_legs/Plane_005/Area3D",
		"back_legs/Plane_006/Area3D",
		"back_legs/Plane_007/Area3D",
		"back_legs/back_leg_down/Area3D",
		"back_legs/back_leg_up/Area3D"
	]
	
	for path in area_paths:
		if has_node(path):
			var area = get_node(path)
			area.monitoring = false
			area.monitorable = false
	print("Bone hidden:", bone_id)
