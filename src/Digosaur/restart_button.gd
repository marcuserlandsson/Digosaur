extends CanvasLayer
@onready var audio = $"../ButtonClickSound"

func _ready():
	$Button.pressed.connect(_on_restart_pressed)

func _on_restart_pressed():
	print("Restarting game...")
	Global.reset_game()
	audio.play()
	get_tree().change_scene_to_file("res://Main.tscn")
