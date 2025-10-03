extends Node2D

@export var color_w = Color(255, 255, 255, 1)
@export  var color_b = Color(0, 0, 0, 1)
@export var elemnt_type = ""
@export var texture: Texture2D:
	set(value):
		texture = value
		queue_redraw()

var radius = 100
var image

func _draw():
	if Input.is_action_pressed("click"):
		radius = 100
	if Input.is_action_just_released("click"):
		radius = 0.0

	draw_circle(Vector2.ZERO, radius, color_w)


func _process(delta):
	global_position = get_global_mouse_position()
	texture = load("res://Images/snow_height_copy.png")
	image = texture.get_image()
	# edit your image here
	image._draw()
	texture = ImageTexture.create_from_image(image)
	
