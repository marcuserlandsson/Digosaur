extends Node3D
#@onready var mesh_instance: MeshInstance3D = $MeshInstance
#@export var colors:Array[Color]
#@onready var img:Image  = Image.load_from_file("res://Images/snow_height.png")

@onready var chunk: Chunk = $Chunk

#var data: Dictionary[Vector3, Color] = {}

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	#var random_generator = FastNoiseLite.new()
	#random_generator.noise_type = FastNoiseLite.TYPE_SIMPLEX
	#random_generator.frequency = 0.001
	
	#for x in range(96):
		#for z in range(64):
			#for y in range(32):
				#var random = random_generator.get_noise_3d(x,y,z)
				#var height = (img.get_pixel(x,y).v )
				#if y > 24:
					#if random > -0.1:
					#if random.y < 1:
						#random.y = 1
						#data[Vector3(x,y,z)] = colors[int(y) % colors.size()]
				#else: 
					#if y == 0:
					#data[Vector3(x,y,z)] = colors[3]
	#chunk.generate_data(96,16,random_generator,colors)
	#chunk.generate_mesh()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
