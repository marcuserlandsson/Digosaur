extends Node3D

func _ready():
	$dogbone3.visible = false
	$dogbone4.visible = false
	$dogbone5.visible = false

func _process(delta):
	print("Bones array museum:", Global.bones)
	for bone in Global.bones:
		print("Museum shows:", bone)
		if bone == "dogbone3":
			$dogbone3.visible = true
		if bone == "dogbone4":
			$dogbone4.visible = true
		if bone == "dogbone5":
			$dogbone5.visible = true
		
