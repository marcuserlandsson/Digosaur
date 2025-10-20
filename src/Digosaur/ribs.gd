extends Node3D

@export var bone_id: String = "ribs"

func _ready():
	# Add to bone collectibles group for touch detection
	add_to_group("bone_collectibles")
	
	# Connect to all Area3D collision shapes for ribs
	var area_paths = [
		"ribcage/Plane_008/Area3D",
		"ribcage/Plane_009/Area3D",
		"ribcage/Plane_010/Area3D",
		"ribcage/Plane_011/Area3D",
		"ribcage/Plane_012/Area3D",
		"ribcage/Plane_013/Area3D",
		"ribcage/Plane_014/Area3D",
		"ribcage/Plane_015/Area3D",
		"ribcage/Plane_016/Area3D",
		"ribcage/Plane_017/Area3D",
		"ribcage/Plane_018/Area3D",
		"ribcage/Plane_019/Area3D",
		"ribcage/Plane_020/Area3D",
		"ribcage/Plane_021/Area3D",
		"ribcage/Plane_022/Area3D",
		"ribcage/Plane_023/Area3D",
		"ribcage/Plane_024/Area3D",
		"ribcage/Plane_025/Area3D",
		"ribcage/Plane_026/Area3D",
		"ribcage/Plane_027/Area3D",
		"ribcage/Plane_028/Area3D",
		"ribcage/Plane_029/Area3D",
		"ribcage/Plane_030/Area3D",
		"ribcage/Plane_031/Area3D",
		"ribcage/Plane_032/Area3D",
		"ribcage/Plane_033/Area3D",
		"ribcage/Plane_034/Area3D"
	]
	
	for path in area_paths:
		if has_node(path):
			var area = get_node(path)
			area.input_event.connect(_on_input_event)
			area.area_entered.connect(_on_area_entered)
			area.body_entered.connect(_on_body_entered)

func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		collect_bone()

func _on_area_entered(area):
	collect_bone()

func _on_body_entered(body):
	collect_bone()

func collect_bone():
	print("Collected bone:", bone_id)
	Global.add_bone(bone_id)
	hide_bone()

func hide_bone():
	visible = false
	var area_paths = [
		"ribcage/Plane_008/Area3D",
		"ribcage/Plane_009/Area3D",
		"ribcage/Plane_010/Area3D",
		"ribcage/Plane_011/Area3D",
		"ribcage/Plane_012/Area3D",
		"ribcage/Plane_013/Area3D",
		"ribcage/Plane_014/Area3D",
		"ribcage/Plane_015/Area3D",
		"ribcage/Plane_016/Area3D",
		"ribcage/Plane_017/Area3D",
		"ribcage/Plane_018/Area3D",
		"ribcage/Plane_019/Area3D",
		"ribcage/Plane_020/Area3D",
		"ribcage/Plane_021/Area3D",
		"ribcage/Plane_022/Area3D",
		"ribcage/Plane_023/Area3D",
		"ribcage/Plane_024/Area3D",
		"ribcage/Plane_025/Area3D",
		"ribcage/Plane_026/Area3D",
		"ribcage/Plane_027/Area3D",
		"ribcage/Plane_028/Area3D",
		"ribcage/Plane_029/Area3D",
		"ribcage/Plane_030/Area3D",
		"ribcage/Plane_031/Area3D",
		"ribcage/Plane_032/Area3D",
		"ribcage/Plane_033/Area3D",
		"ribcage/Plane_034/Area3D"
	]
	
	for path in area_paths:
		if has_node(path):
			var area = get_node(path)
			area.monitoring = false
			area.monitorable = false
	print("Bone hidden:", bone_id)
