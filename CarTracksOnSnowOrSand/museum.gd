extends Node3D

func _ready():
	Global.museum_ref = self
	setup_scene()
	
	Global.bone_added.connect(_on_bone_added)
	for bone_id in Global.bones:
		_show_bone(bone_id)
		
func setup_scene():
	$dogbone3.visible = false
	$dogbone4.visible = false
	$dogbone5.visible = false
	$Stegosaur/head2.visible = false
	$Stegosaur/front_legs.visible = false
	$Stegosaur/back_legs.visible = false
	$Stegosaur/spine.visible = false
	$Stegosaur/ribcage.visible = false
	$Stegosaur/tail.visible = false
	print("Museum scene reset")


func _on_bone_added(bone_id: String):
	print("Museum: bone added:", bone_id)
	_show_bone(bone_id)
	

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
		"front":
			$Stegosaur/front_legs.visible = true
		"back":
			$Stegosaur/back_legs.visible = true
		"spine":
			$Stegosaur/spine.visible = true
		"ribs":
			$Stegosaur/ribcage.visible = true
		"tail":
			$Stegosaur/tail.visible = true
		_:
			print("Unknown bone:", bone_id)
		
