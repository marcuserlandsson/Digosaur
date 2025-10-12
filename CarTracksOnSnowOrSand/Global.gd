extends Node

var bones: Array = []

signal bone_added(bone_id: String)

func add_bone(bone_id: String):
	if bone_id not in bones:
		bones.append(bone_id)
		print("Added bone:", bone_id)
		print("Bones now:", bones)
		emit_signal("bone_added", bone_id)
	else:
		print("Bone already collected:", bone_id)
