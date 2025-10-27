extends Node3D

var original_bone_positions := {}
var elapsed := 0.0
var timer_running := false

func _ready():
	Global.sand_ref = self
	_save_original_positions()
	_randomize_bone_positions()
	_start_timer()
	print("Sand scene registered in global")
	Global.all_bones_found.connect(_on_all_bones_found)

func _save_original_positions():
	var stego = $Stegosaur
	var bone_paths = {
		"head": "head",
		"front": "Front", 
		"back": "Back",
		"spine": "spine",
		"ribs": "ribs",
		"tail": "tail"
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
		"head": "head",
		"front": "Front",
		"back": "Back", 
		"spine": "spine",
		"ribs": "ribs",
		"tail": "tail"
	}

	# Bone size estimates (approximate bounding box radius for each bone)
	var bone_sizes = {
		"head": 0.8,
		"front": 1.2,
		"back": 1.2,
		"spine": 1.5,
		"ribs": 1.0,
		"tail": 1.3
	}

	# Adjusted bounds for our 16x9 sand area with extra padding for bone sizes
	var min_x = -8.0
	var max_x = 8.0
	var min_z = -4.5
	var max_z = 4.5

	var boundary_padding = 0.8  # Extra padding from edges
	min_x += boundary_padding
	max_x -= boundary_padding
	min_z += boundary_padding
	max_z -= boundary_padding

	var seed_val = Time.get_ticks_usec()
	seed(seed_val)
	print("Using seed:", seed_val)

	var placed_bones = []  # Array to track placed bone positions and sizes

	for bone_name in bone_paths.keys():
		var path = bone_paths[bone_name]
		if stego.has_node(path):
			var bone = stego.get_node(path)
			var base_pos = original_bone_positions.get(bone_name, bone.global_position)
			var bone_size = bone_sizes.get(bone_name, 1.0)
			
			var max_attempts = 50
			var placed = false
			
			for attempt in range(max_attempts):
				var new_x = randf_range(min_x, max_x)
				var new_z = randf_range(min_z, max_z)
				var new_pos = Vector3(new_x, base_pos.y, new_z)
				
				# Check if this position collides with any already placed bone
				var collision = false
				for placed_bone in placed_bones:
					var distance = new_pos.distance_to(placed_bone.position)
					var min_distance = bone_size + placed_bone.size
					
					if distance < min_distance:
						collision = true
						break
				
				# Check if bone would stick out of bounds
				var out_of_bounds = false
				if new_x - bone_size < min_x or new_x + bone_size > max_x:
					out_of_bounds = true
				if new_z - bone_size < min_z or new_z + bone_size > max_z:
					out_of_bounds = true
				
				if not collision and not out_of_bounds:
					# Position is valid!
					bone.global_position = new_pos
					placed_bones.append({"position": new_pos, "size": bone_size})
					print("  🦴", bone_name, "->", new_pos, "(attempt", attempt + 1, ")")
					placed = true
					break
			
			if not placed:
				print("  ⚠️", bone_name, "failed to place after", max_attempts, "attempts - using fallback position")
				# Fallback: place at a safe distance from other bones
				var fallback_x = randf_range(min_x + bone_size, max_x - bone_size)
				var fallback_z = randf_range(min_z + bone_size, max_z - bone_size)
				bone.global_position = Vector3(fallback_x, base_pos.y, fallback_z)
				placed_bones.append({"position": bone.global_position, "size": bone_size})

	print("Bones randomized with collision detection!")

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
