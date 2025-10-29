class_name ChunkManager extends Node

@export var colors:Array[Color]
var dimensions: Vector3 = Vector3(96,16,64)
var chunk_size: int = 32
var noise_seed: int = 0
var random_generator = FastNoiseLite.new()

var number_of_chunks: Vector3

var chunk_class = preload("res://chunk.tscn")

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	random_generator.noise_type = FastNoiseLite.TYPE_SIMPLEX
	random_generator.frequency = 0.001
	
	number_of_chunks = dimensions / chunk_size
	generate_chunks()
	
func generate_chunks():
	for x in range(number_of_chunks.x):
		for z in range(number_of_chunks.z):
			for y in range(number_of_chunks.y):
				var new_chunk = chunk_class.instantiate()
				new_chunk.position = Vector3(x,y,z) * chunk_size
				add_child(new_chunk)
				new_chunk.generate_data(chunk_size, dimensions.y, random_generator, colors)
				new_chunk.generate_mesh()
