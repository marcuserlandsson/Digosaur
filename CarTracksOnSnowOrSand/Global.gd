extends Node

var bones: Array = []
const TOTAL_BONES := 6
signal bone_added(bone_id: String)
var museum_ref: Node = null
var sand_ref: Node = null
signal all_bones_found

func add_bone(bone_id: String):
	if bone_id not in bones:
		bones.append(bone_id)
		print("Added bone:", bone_id)
		emit_signal("bone_added", bone_id)
		
		if bones.size() == TOTAL_BONES:
			print("All bones found!")
			emit_signal("all_bones_found")

func reset_game():
	bones.clear()

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

	print("Spelet har startats om. Alla ben är borttagna.")
