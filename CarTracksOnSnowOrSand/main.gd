extends Node3D

func _ready():
	#added stuff from demo 2d_in_3d, might break stuff
	# Clear the viewport.
	var viewport = $SubViewport2
	$SubViewport2.set_clear_mode(SubViewport.CLEAR_MODE_ONCE)
	# Retrieve the texture and set it to the viewport quad.
	$SandHeight.mesh.material.set_shader_parameter("heightmap", viewport.get_texture())
	# think
