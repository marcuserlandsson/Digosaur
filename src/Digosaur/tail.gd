extends Node3D

@export var bone_id: String = "tail"

func _ready():
	# Add to bone collectibles group for touch detection
	add_to_group("bone_collectibles")
	
	# Touch detection is handled by touch_track.gd via the bone_collectibles group
	# No need for individual input event handling here

func collect_bone():
	print("Collected bone:", bone_id)
	Global.add_bone(bone_id)
	hide_bone()

func hide_bone():
	visible = false
	if has_node("tail_mesh/Area3D"):
		$tail_mesh/Area3D.monitoring = false
		$tail_mesh/Area3D.monitorable = false
	print("Bone hidden:", bone_id)
