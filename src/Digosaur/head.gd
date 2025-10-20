extends Node3D

@export var bone_id: String = "head"

func _ready():
	# Add to bone collectibles group for touch detection
	add_to_group("bone_collectibles")
	
	# Connect to all Area3D collision shapes for this bone part
	if has_node("head2/Cube_041/Area3D"):
		$head2/Cube_041/Area3D.input_event.connect(_on_input_event)
		$head2/Cube_041/Area3D.area_entered.connect(_on_area_entered)
		$head2/Cube_041/Area3D.body_entered.connect(_on_body_entered)
	if has_node("head2/head/Area3D"):
		$head2/head/Area3D.input_event.connect(_on_input_event)
		$head2/head/Area3D.area_entered.connect(_on_area_entered)
		$head2/head/Area3D.body_entered.connect(_on_body_entered)

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
	print("Collected bone:", bone_id)3
	Global.add_bone(bone_id)
	hide_bone()

func hide_bone():
	visible = false
	if has_node("head2/Cube_041/Area3D"):
		$head2/Cube_041/Area3D.monitoring = false
		$head2/Cube_041/Area3D.monitorable = false
	if has_node("head2/head/Area3D"):
		$head2/head/Area3D.monitoring = false
		$head2/head/Area3D.monitorable = false
	print("Bone hidden:", bone_id)
