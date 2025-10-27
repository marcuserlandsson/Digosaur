extends Node

var bones: Array = []
const TOTAL_BONES := 6
const SKELETON_BONES = ["head", "front", "back", "spine", "ribs", "tail"]
signal bone_added(bone_id: String)
var museum_ref: Node = null
var sand_ref: Node = null
signal all_bones_found

func add_bone(bone_id: String):
	if bone_id not in bones:
		bones.append(bone_id)
		print("🦴 BONE COLLECTED: ", bone_id)
		print("📊 Total bones found: ", bones.size(), "/", TOTAL_BONES)
		print("🗂️ Complete collection: ", bones)
		emit_signal("bone_added", bone_id)
		
		if bones.size() == TOTAL_BONES:
			print("🎉 All bones found!")
			emit_signal("all_bones_found")
		
		# Send bone collection to museum if network is available
		if has_node("/root/MuseumNetwork"):
			get_node("/root/MuseumNetwork").send_bone_collected(bone_id)
	else:
		print("⚠️ Bone ", bone_id, " already collected!")

func reset_game():
	bones.clear()
	
	# Send reset signal to museum scene via network
	if has_node("/root/MuseumNetwork"):
		get_node("/root/MuseumNetwork").send_game_reset()
	
	if sand_ref and sand_ref.has_method("_start_timer"):
		sand_ref._start_timer()
	
	if sand_ref:
		print("Restarting: scheduling bone randomization...")
		await get_tree().process_frame
		if sand_ref.has_method("_randomize_bone_positions"):
			print("Randomizing bones now!")
			sand_ref._randomize_bone_positions()
	else:
		print("Sand scene reference not found.")

	if museum_ref and museum_ref.has_method("setup_scene"):
		museum_ref.setup_scene()

	print("🔄 Game restarted! All bones are back in the sand.")