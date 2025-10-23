extends Node3D

var museum_window: Window
var original_bone_positions := {}

func _ready():
	Global.sand_ref = self
	_save_original_positions()
	_randomize_bone_positions()
	_test_vector()
	print("sandscene registered in global")

	var existing_window = get_tree().root.get_node_or_null("MuseumWindow")
	if existing_window:
		museum_window = existing_window
		print("Reusing existing Museum window.")
		_reset_museum_scene()
		return

	museum_window = Window.new()
	museum_window.name = "MuseumWindow"
	museum_window.title = "Museum"
	museum_window.mode = Window.MODE_WINDOWED
	museum_window.transient = false
	museum_window.position = Vector2i(5, 300)
	museum_window.size = Vector2i(800, 1000)

	var museum_scene = load("res://Museum.tscn").instantiate()
	museum_scene.name = "Museum"
	museum_window.add_child(museum_scene)

	get_tree().root.add_child(museum_window)
	museum_window.visible = true
	print("Created new Museum window.")
	
func _test_vector():
	var v = Vector3(1, 2, 3)
	print(v)

func _reset_museum_scene():
	if museum_window:
		var museum_scene = museum_window.get_node_or_null("Museum")
		if museum_scene and museum_scene.has_method("setup_scene"):
			museum_scene.setup_scene()
			print("Reset museum scene in same window.")

func _save_original_positions():
	var stego = $Stegosaur
	var bone_paths = {
		"head2": "head/head2",
		"front_legs": "Front/front_legs",
		"back_legs": "Back/back_legs",
		"spine": "Spine/spine",
		"ribcage": "Ribs/ribcage",
		"tail": "Tail/tail"
	}

	for bone_name in bone_paths.keys():
		var path = bone_paths[bone_name]
		if stego.has_node(path):
			var bone = stego.get_node(path)
			original_bone_positions[bone_name] = bone.position
	print("Saved main bone part positions.")


func _randomize_bone_positions():
	var stego = $Stegosaur
	var bone_paths = {
		"head2": "head/head2",
		"front_legs": "Front/front_legs",
		"back_legs": "Back/back_legs",
		"spine": "Spine/spine",
		"ribcage": "Ribs/ribcage",
		"tail": "Tail/tail"
	}

	var range = 4.0               # How far bones can move horizontally
	var sand_center = Vector3(0, 0, 0)
	var sand_bounds = 8.0

	var seed_val = Time.get_ticks_usec()
	seed(seed_val)
	print("Using seed:", seed_val)

	for bone_name in bone_paths.keys():
		var path = bone_paths[bone_name]
		if stego.has_node(path):
			var bone = stego.get_node(path)
			var base_pos = original_bone_positions.get(bone_name, bone.position)

			# Create new position that only changes X and Z
			var new_x = clamp(
				base_pos.x + randf_range(-range, range),
				sand_center.x - sand_bounds,
				sand_center.x + sand_bounds
			)
			var new_z = clamp(
				base_pos.z + randf_range(-range, range),
				sand_center.z - sand_bounds,
				sand_center.z + sand_bounds
			)

			bone.position = Vector3(new_x, base_pos.y, new_z)

			print("  🦴", bone_name, "->", bone.position)

	print("Bones randomized — heights preserved.")

func _get_all_bones(node: Node) -> Array:
	var bones := []
	for child in node.get_children():
		if child is Node3D and child.name != "CollisionShape3D":
			bones.append(child)
			bones += _get_all_bones(child)
	return bones
