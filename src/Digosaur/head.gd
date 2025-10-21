extends Node3D

@export var bone_id: String = "head"

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
	print("Bone hidden:", bone_id)
