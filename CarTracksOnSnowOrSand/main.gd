extends Node3D

var museum_window: Window
var original_bone_positions := {}
var elapsed := 0.0
var timer_running := false

func _ready():
	Global.sand_ref = self
	_save_original_positions()
	_randomize_bone_positions()
	_start_timer()
	print("sandscene registered in global")
	Global.all_bones_found.connect(_on_all_bones_found)

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

	get_tree().root.call_deferred("add_child", museum_window)
	museum_window.visible = true
	print("Created new Museum window.")
	
	


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
			original_bone_positions[bone_name] = bone.global_position
	print("Saved original bone global positions.")


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

	var min_x = -1.6
	var max_x = 3.2
	var min_z = -4.2
	var max_z = 3.9

	var padding = 0.4
	min_x += padding
	max_x -= padding
	min_z += padding
	max_z -= padding

	var camera_center = Vector3(0.857, 0, -0.114)

	var seed_val = Time.get_ticks_usec()
	seed(seed_val)
	print("Using seed:", seed_val)

	for bone_name in bone_paths.keys():
		var path = bone_paths[bone_name]
		if stego.has_node(path):
			var bone = stego.get_node(path)
			var base_pos = original_bone_positions.get(bone_name, bone.global_position)

			var new_x = randf_range(min_x, max_x) + camera_center.x
			var new_z = randf_range(min_z, max_z) + camera_center.z

			bone.global_position = Vector3(new_x, base_pos.y, new_z)
			print("  🦴", bone_name, "->", bone.global_position)

	print("Bones randomized within adjusted camera bounds.")


func _get_all_bones(node: Node) -> Array:
	var bones := []
	for child in node.get_children():
		if child is Node3D and child.name != "CollisionShape3D":
			bones.append(child)
			bones += _get_all_bones(child)
	return bones


func _on_all_bones_found():
	_stop_timer()
	var minutes := int(elapsed) / 60
	var seconds := fmod(elapsed, 60.0)

	var label = Label3D.new()
	label.text = "Congratulations!!!\nYou found all the bones!\nTime: %02d:%05.2f" % [minutes, seconds]
	label.modulate = Color(1, 1, 0)
	label.scale = Vector3(1, 1, 1)
	label.billboard = BaseMaterial3D.BILLBOARD_ENABLED
	add_child(label)
	label.global_position = Vector3(0, 1, 0)

	


func _start_timer():
	elapsed = 0.0
	timer_running = true

func _stop_timer():
	timer_running = false

func _process(delta: float) -> void:
	if timer_running:
		elapsed += delta

	
