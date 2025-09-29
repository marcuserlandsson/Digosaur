extends Node

var museum_window: Window

func _ready():
	#Main window: Digging scene

	#Museum window
	museum_window = Window.new()
	museum_window.title = "Museum"
	museum_window.mode = Window.MODE_WINDOWED

	var museum_screen_id := 1 
	var museum_pos = DisplayServer.screen_get_position(museum_screen_id)
	var museum_size = DisplayServer.screen_get_size(museum_screen_id)

	#museum_window.position = museum_pos
	#museum_window.size = museum_size
	
	museum_window.position = Vector2i(5, 300)
	museum_window.size = Vector2i(300, 400)

	# Add the museum scene
	var museum_scene = load("res://Museum.tscn").instantiate()
	museum_window.call_deferred("add_child", museum_scene)

	# Attach to root
	get_tree().root.call_deferred("add_child", museum_window)
	museum_window.visible = true

	print("Museum window placed at:", museum_pos, "size:", museum_size)
