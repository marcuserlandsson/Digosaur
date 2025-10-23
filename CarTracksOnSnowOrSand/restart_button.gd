extends CanvasLayer

func _ready():
	$Button.pressed.connect(_on_restart_pressed)

func _on_restart_pressed():
	print("Startar om spelet...")
	Global.reset_game()
	get_tree().change_scene_to_file("res://Main.tscn")
