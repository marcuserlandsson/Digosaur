extends Node

var bones: Array = []
const TOTAL_BONES := 6
const SKELETON_BONES = ["head", "front", "back", "spine", "ribs", "tail"]
signal bone_added(bone_id: String)

func add_bone(bone_id: String):
	if bone_id not in bones:
		bones.append(bone_id)
		print("Added bone:", bone_id)
		print("Bones now:", bones)
		emit_signal("bone_added", bone_id)
