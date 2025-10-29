extends Node3D

# Museum scene - tracks and displays collected bones
var collected_bones: Array = []

func _ready():
	Global.museum_ref = self
	setup_scene()
	
	# Connect to museum server for network bone events
	if has_node("/root/MuseumServer"):
		get_node("/root/MuseumServer").bone_received.connect(_on_network_bone_received)
		get_node("/root/MuseumServer").game_reset.connect(_on_network_game_reset)
	
	# Also connect to Global for local testing
	Global.bone_added.connect(_on_bone_added)
	for bone_id in Global.bones:
		_show_bone(bone_id)

func setup_scene():
	# Hide all bones initially
	$dogbone3.visible = false
	$dogbone4.visible = false
	$dogbone5.visible = false
	$Stegosaur/head2.visible = false
	$Stegosaur/front_legs.visible = false
	$Stegosaur/back_legs.visible = false
	$Stegosaur/spine.visible = false
	$Stegosaur/ribcage.visible = false
	$Stegosaur/tail.visible = false
	
	$Stegosaur/head2_shadow.visible = true
	$Stegosaur/front_legs_shadow.visible = true
	$Stegosaur/back_legs_shadow.visible = true
	$Stegosaur/spine_shadow.visible = true
	$Stegosaur/ribcage_shadow.visible = true
	$Stegosaur/tail_shadow.visible = true
	
	# Clear collected bones tracking
	collected_bones.clear()
	print("Museum scene reset - all bones hidden")

func _on_bone_added(bone_id: String):
	print("Museum: Local bone added:", bone_id)
	_add_bone_to_collection(bone_id)

func _on_network_bone_received(bone_id: String):
	print("Museum: Network bone received:", bone_id)
	_add_bone_to_collection(bone_id)

func _on_network_game_reset():
	print("Museum: Network game reset received!")
	setup_scene()

func _add_bone_to_collection(bone_id: String):
	# Add to our local tracking if not already collected
	if bone_id not in collected_bones:
		collected_bones.append(bone_id)
		print("Museum: Added", bone_id, "to collection. Total:", collected_bones.size())
		_show_bone(bone_id)
	else:
		print("Museum: Bone", bone_id, "already collected")

func _show_bone(bone_id: String):
	match bone_id:
		"dogbone3":
			$dogbone3.visible = true
		"dogbone4":
			$dogbone4.visible = true
		"dogbone5":
			$dogbone5.visible = true
		"head":
			$Stegosaur/head2.visible = true
			$Stegosaur/head2_shadow.visible = false
		"front":
			$Stegosaur/front_legs.visible = true
			$Stegosaur/front_legs_shadow.visible = false
		"back":
			$Stegosaur/back_legs.visible = true
			$Stegosaur/back_legs_shadow.visible = false
		"spine":
			$Stegosaur/spine.visible = true
			$Stegosaur/spine_shadow.visible = false
		"ribs":
			$Stegosaur/ribcage.visible = true
			$Stegosaur/ribcage_shadow.visible = false
		"tail":
			$Stegosaur/tail.visible = true
			$Stegosaur/tail_shadow.visible = false
		_:
			print("Unknown bone:", bone_id)
		
