class_name Chunk extends StaticBody3D
@export var mat: Material
@onready var collision_shape = $CollisionShape3D
@onready var mesh_instance: MeshInstance3D = $MeshInstance3D

var voxels:Dictionary[Vector3, Color] = {}

var surface_array: Array = []

var vertices = PackedVector3Array()
var normals= PackedVector3Array()
var colors = PackedColorArray()

var cube_vertices:Array[Vector3] = [
	Vector3(-0.5,-0.5,0.5),
	Vector3(0.5,-0.5,0.5),
	Vector3(0.5,-0.5,-0.5),
	Vector3(-0.5,-0.5,-0.5),
	Vector3(-0.5,0.5,0.5),
	Vector3(0.5,0.5,0.5),
	Vector3(0.5,0.5,-0.5),
	Vector3(-0.5,0.5,-0.5)
]

enum Face{BOTTOM, FRONT, RIGHT, TOP,LEFT,BACK}

var face_indices:Dictionary[Face, Array]={
	Face.FRONT:[[0,4,5],[0,5,1]],
	Face.BACK:[[2,7,3],[2,6,7]],
	Face.LEFT:[[3,7,4],[3,4,0]],
	Face.RIGHT:[[1,5,6],[1,6,2]],
	Face.BOTTOM:[[0,1,2],[0,2,3]],
	Face.TOP:[[4,7,6],[4,6,5]],
}

var face_normals:Dictionary[Face, Vector3]={
	Face.FRONT:Vector3(0,0,1),
	Face.BACK:Vector3(0,0,-1),
	Face.LEFT:Vector3(-1,0,0),
	Face.RIGHT:Vector3(1,0,0),
	Face.BOTTOM:Vector3(0,-1,0),
	Face.TOP:Vector3(0,1,0),
}

var face_colors:Dictionary[Face, Color]={
	Face.FRONT:Color.RED,
	Face.BACK:Color.ORANGE,
	Face.LEFT:Color.YELLOW,
	Face.RIGHT:Color.GREEN,
	Face.BOTTOM:Color.BLUE,
	Face.TOP:Color.PURPLE,
}

func _ready():
	surface_array.resize(Mesh.ARRAY_MAX);
	#generate_mesh()
	
func generate_data(chunk_size: int, max_height: int, noise: Noise, color_array: Array[Color]):
	for x in range(chunk_size):
		for z in range(chunk_size):
			var rand = ((noise.get_noise_2d(x,z)+0.5*noise.get_noise_2d(x*2,z*2)+0.25))
			var rand_p = pow(rand,2.1)
			var height = max_height*rand_p
			
			if height < position.y: continue
			var local_height = height - position.y
			for y in range(min(local_height, chunk_size)):
				voxels[Vector3(x,y,z)] = color_array[y%color_array.size()]
				 
	
func generate_mesh():
	for position in voxels:
		var color = voxels[position]
		if not has_neighbor(voxels, Face.FRONT, position):
			add_face(Face.FRONT, position, color)
		if not has_neighbor(voxels, Face.LEFT, position):
			add_face(Face.LEFT, position, color)
		if not has_neighbor(voxels, Face.RIGHT, position):
			add_face(Face.RIGHT, position, color)
		if not has_neighbor(voxels, Face.BOTTOM, position):
			add_face(Face.BOTTOM, position, color)
		if not has_neighbor(voxels, Face.BACK, position):
			add_face(Face.BACK, position, color)
		if not has_neighbor(voxels, Face.TOP, position):
			add_face(Face.TOP, position, color)
	commit_mesh()
	
func has_neighbor(data: Dictionary[Vector3, Color], face: Face, position:Vector3)->bool:
	var neightbor_position = position + face_normals[face]
	if data.has(neightbor_position):return true
	return false
	
func add_face(face: Face, position: Vector3, color:Color):
	var indices = face_indices[face]
	for triangle in indices:
		for index in triangle:
			vertices.append(cube_vertices[index]+position)
			normals.append(face_normals[face])
			colors.append(color)

func commit_mesh():
	surface_array[Mesh.ARRAY_VERTEX] = vertices
	surface_array[Mesh.ARRAY_NORMAL] = normals
	surface_array[Mesh.ARRAY_COLOR] = colors
	
	mesh_instance.mesh.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES, surface_array)
	mesh_instance.mesh.surface_set_material(0,mat)
	collision_shape.shape = mesh_instance.mesh.create_trimesh_shape()
