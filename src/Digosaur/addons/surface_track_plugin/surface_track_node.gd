extends Node
class_name SurfaceTrackNode

# DLL function signatures
var dll_handle: int = -1
var init_func: int = -1
var shutdown_func: int = -1
var get_touch_count_func: int = -1
var get_touch_x_func: int = -1
var get_touch_y_func: int = -1
var get_touch_size_x_func: int = -1
var get_touch_size_y_func: int = -1
var is_touch_active_func: int = -1

# Touch data
var touch_count: int = 0
var touch_positions: Array[Vector2] = []
var touch_sizes: Array[Vector2] = []
var touch_active: Array[bool] = []

# Signals
signal touch_detected(touch_index: int, position: Vector2, size: Vector2)
signal touch_moved(touch_index: int, position: Vector2, size: Vector2)
signal touch_ended(touch_index: int)

func _ready():
	load_dll()

func _exit_tree():
	unload_dll()

func load_dll():
	# Load the SurfaceTrackLibrary DLL
	var dll_path = "res://addons/surface_track_plugin/SurfaceTrackLibrary.dll"
	
	if not FileAccess.file_exists(dll_path):
		push_error("SurfaceTrackLibrary.dll not found at: " + dll_path)
		return false
	
	dll_handle = OS.load_dynamic_library(dll_path, OS.DYNAMIC_LIBRARY_IMPORT)
	if dll_handle == -1:
		push_error("Failed to load SurfaceTrackLibrary.dll")
		return false
	
	# Get function addresses
	init_func = OS.get_dynamic_library_symbol_handle(dll_handle, "Initialize")
	shutdown_func = OS.get_dynamic_library_symbol_handle(dll_handle, "Shutdown")
	get_touch_count_func = OS.get_dynamic_library_symbol_handle(dll_handle, "GetTouchCount")
	get_touch_x_func = OS.get_dynamic_library_symbol_handle(dll_handle, "GetTouchX")
	get_touch_y_func = OS.get_dynamic_library_symbol_handle(dll_handle, "GetTouchY")
	get_touch_size_x_func = OS.get_dynamic_library_symbol_handle(dll_handle, "GetTouchSizeX")
	get_touch_size_y_func = OS.get_dynamic_library_symbol_handle(dll_handle, "GetTouchSizeY")
	is_touch_active_func = OS.get_dynamic_library_symbol_handle(dll_handle, "IsTouchActive")
	
	if init_func == -1 or shutdown_func == -1 or get_touch_count_func == -1:
		push_error("Failed to get DLL function addresses")
		unload_dll()
		return false
	
	print("SurfaceTrackLibrary DLL loaded successfully")
	return true

func unload_dll():
	if dll_handle != -1:
		# Call shutdown before unloading
		if shutdown_func != -1:
			call_dll_function(shutdown_func, [])
		
		OS.unload_dynamic_library(dll_handle)
		dll_handle = -1
		print("SurfaceTrackLibrary DLL unloaded")

func initialize_surface(hwnd: int) -> bool:
	if init_func == -1:
		push_error("DLL not loaded")
		return false
	
	var result = call_dll_function(init_func, [hwnd])
	return result == 1

func call_dll_function(func_handle: int, args: Array) -> int:
	if func_handle == -1:
		return 0
	
	# This is a simplified version - in practice you'd need proper marshalling
	# For now, we'll use a workaround with GDScript's call functionality
	return 0

func _process(delta):
	if dll_handle == -1:
		return
	
	# Get current touch count
	var new_touch_count = get_touch_count()
	
	# Handle touch changes
	if new_touch_count != touch_count:
		handle_touch_count_change(new_touch_count)
	
	# Update existing touches
	update_touch_data()

func get_touch_count() -> int:
	if get_touch_count_func == -1:
		return 0
	
	# This would call the DLL function in a real implementation
	# For now, return 0 as placeholder
	return 0

func handle_touch_count_change(new_count: int):
	var old_count = touch_count
	touch_count = new_count
	
	# Resize arrays
	touch_positions.resize(touch_count)
	touch_sizes.resize(touch_count)
	touch_active.resize(touch_count)
	
	# Handle new touches
	for i in range(old_count, touch_count):
		touch_positions[i] = get_touch_position(i)
		touch_sizes[i] = get_touch_size(i)
		touch_active[i] = true
		touch_detected.emit(i, touch_positions[i], touch_sizes[i])
	
	# Handle ended touches
	for i in range(touch_count, old_count):
		touch_ended.emit(i)

func update_touch_data():
	for i in range(touch_count):
		var new_pos = get_touch_position(i)
		var new_size = get_touch_size(i)
		var new_active = is_touch_active(i)
		
		if new_pos != touch_positions[i] or new_size != touch_sizes[i]:
			touch_positions[i] = new_pos
			touch_sizes[i] = new_size
			touch_moved.emit(i, new_pos, new_size)
		
		touch_active[i] = new_active

func get_touch_position(index: int) -> Vector2:
	if get_touch_x_func == -1 or get_touch_y_func == -1:
		return Vector2.ZERO
	
	# This would call the DLL functions in a real implementation
	# For now, return zero as placeholder
	return Vector2.ZERO

func get_touch_size(index: int) -> Vector2:
	if get_touch_size_x_func == -1 or get_touch_size_y_func == -1:
		return Vector2.ZERO
	
	# This would call the DLL functions in a real implementation
	# For now, return zero as placeholder
	return Vector2.ZERO

func is_touch_active(index: int) -> bool:
	if is_touch_active_func == -1:
		return false
	
	# This would call the DLL function in a real implementation
	# For now, return false as placeholder
	return false
