extends Node

var bones: Array = []
const TOTAL_BONES := 6
const SKELETON_BONES = ["head", "front", "back", "spine", "ribs", "tail"]
signal bone_added(bone_id: String)

func add_bone(bone_id: String):
	if bone_id not in bones:
		bones.append(bone_id)
		print("🦴 BONE COLLECTED: ", bone_id)
		print("📊 Total bones found: ", bones.size(), "/", TOTAL_BONES)
		print("🗂️ Complete collection: ", bones)
		emit_signal("bone_added", bone_id)
		
		# Send bone collection to museum if network is available
		if has_node("/root/MuseumNetwork"):
			get_node("/root/MuseumNetwork").send_bone_collected(bone_id)
	else:
		print("⚠️ Bone ", bone_id, " already collected!")
