extends Node3D

func _ready():
	# Hide all bones first
	$dogbone3.visible = false

	for bone in Global.bones:
		if bone == "dogbone3":
			show_bone_with_delay($dogbone3, 1.5)  # delay in seconds


func show_bone_with_delay(bone_node: Node3D, delay: float) -> void:
	await get_tree().create_timer(delay).timeout
	bone_node.visible = true
	print("Bone appeared:", bone_node.name)
