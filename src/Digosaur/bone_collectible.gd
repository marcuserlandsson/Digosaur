extends Node3D

@export var bone_id: String = "dogbone3"

func _ready():
	# Add to bone collectibles group for touch detection
	add_to_group("bone_collectibles")
	
	# Connect to the area's input event for touch detection
	if has_node("Area3D"):
		$Area3D.input_event.connect(_on_input_event)
		$Area3D.area_entered.connect(_on_area_entered)
		$Area3D.body_entered.connect(_on_body_entered)

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
	if has_node("Area3D"):
		$Area3D.monitoring = false
		$Area3D.monitorable = false
	print("Bone hidden:", bone_id)
