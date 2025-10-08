extends Sprite2D

@export var color_w = Color(255, 255, 255, 1)
@export  var color_b = Color(0, 0, 0, 1)
@export var elemnt_type = ""


var radius = 100
var img : Image

func _ready():
	img = Image.create_empty(2000,2000, false, Image.FORMAT_RGBA8)
	img.fill(color_w)
	#tex = ImageTexture.create_from_image(img)
	#texture = load("res://Images/snow_height_copy.png")
	#image = texture.get_image()
	

func paint(position):
	img.draw_circle(position, radius, color_b)


func _input(event):
	print("here")
	if event is InputEvent:
		if event.pressed and event.is_echo() == false:
			if event.button_index == MOUSE_BUTTON_LEFT:	
				print("all in")
				global_position = get_global_mouse_position()
				# edit your image here
				paint(global_position)
				texture = ImageTexture.create_from_image(img)
	
