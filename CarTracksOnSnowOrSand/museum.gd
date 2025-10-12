extends Node3D

func _ready():
	$dogbone3.visible = false
	$dogbone4.visible = false
	$dogbone5.visible = false
	
	Global.bone_added.connect(_on_bone_added)
	for bone_id in Global.bones:
		_show_bone(bone_id)


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
		_:
			print("Unknown bone:", bone_id)
		
