extends Node3D

## Runtime terrain brush that uses Terrain3DEditor, exactly like the editor does
## This script replicates the editor's "Raise" tool functionality in play mode

@export var terrain: Terrain3D = null
@export var brush_size: float = 2.0  # meters
@export var strength: float = 6.0   # percentage (1-100)
@export var raise_on_left_mouse: bool = true
@export var lower_with_ctrl: bool = true
@export var auto_add_regions: bool = true
@export var cleanup_on_exit: bool = true  # Delete new/modified region files on exit

var editor: Terrain3DEditor
var _camera: Camera3D
var _is_dragging: bool = false
var _brush_data: Dictionary = {}
var _original_region_files: Array[String] = []  # Track files that existed at start
var _data_directory: String = ""
var _touch_operations := {} # Maps finger_id to active (true)

const DEFAULT_BRUSH_PATH: String = "res://addons/terrain_3d/brushes/circle0.exr"
const BRUSH_PATH: String = "res://addons/terrain_3d/brushes"

func _ready() -> void:
	# Find terrain if not assigned
	if not terrain:
		var parent := get_parent()
		if parent is Terrain3D:
			terrain = parent
		else:
			var root := get_tree().get_current_scene()
			if root:
				var candidates := root.find_children("", "Terrain3D", true)
				if candidates.size() > 0:
					terrain = candidates[0]
	
	if not terrain:
		push_error("Terrain3DRuntimeBrush: No Terrain3D found!")
		set_process_input(false)
		return
	
	# Get camera
	_camera = get_viewport().get_camera_3d()
	if not _camera:
		push_error("Terrain3DRuntimeBrush: No Camera3D found in viewport!")
		set_process_input(false)
		return
	
	# Create Terrain3DEditor instance (same as editor plugin does)
	editor = Terrain3DEditor.new()
	if not editor:
		push_error("Terrain3DRuntimeBrush: Failed to create Terrain3DEditor!")
		set_process_input(false)
		return
	
	# Wait for terrain to be fully ready
	if not terrain.data:
		push_warning("Terrain3DRuntimeBrush: Terrain data not ready, waiting...")
		await get_tree().process_frame  # Wait a frame for terrain to initialize
	
	if not terrain.data:
		push_error("Terrain3DRuntimeBrush: Terrain data still not available!")
		set_process_input(false)
		return
	
	# Set up terrain/editor relationship (same as editor plugin)
	# Note: Order matters - set terrain on editor first, then set editor on terrain
	editor.set_terrain(terrain)
	terrain.set_editor(editor)
	
	# Give it a moment to initialize internally
	await get_tree().process_frame
	
	# Set tool and operation to SCULPT + ADD (Raise tool)
	editor.set_tool(Terrain3DEditor.SCULPT)
	editor.set_operation(Terrain3DEditor.ADD)
	
	# Load default brush
	_load_brush(DEFAULT_BRUSH_PATH)
	
	# Set up brush data
	_setup_brush_data()
	
	# Track original region files for cleanup on exit
	_track_original_regions()
	
	set_process_input(true)
	print("Terrain3DRuntimeBrush: Initialized with terrain and editor")


func _setup_brush_data() -> void:
	# Create brush data dictionary matching editor's format
	_brush_data = {
		"brush": _brush_data.get("brush", []),  # Will be set by _load_brush
		"size": brush_size,
		"strength": strength,
		"invert": false,
		"auto_regions": auto_add_regions,
		"align_to_view": true,
		"show_cursor_while_painting": false,  # Not needed in play mode
		"modifier_shift": false,
		"modifier_ctrl": false,
		"modifier_alt": false,
		"gamma": 1.0,
		"jitter": 50.0,
		"crosshair_threshold": 16.0,
		"slope": Vector2(0, 90),
		"enable_texture": false,
		"texture_filter": false,
		"margin": 0.0,
		"asset_id": 0,
		"mouse_pressure": 1.0
	}
	
	# Update brush data on editor
	if editor:
		editor.set_brush_data(_brush_data)


func _load_brush(path: String) -> void:
	var img: Image = Image.load_from_file(path)
	if not img:
		push_error("Terrain3DRuntimeBrush: Failed to load brush from: " + path)
		return
	
	# Convert to RF format if needed
	if img.get_format() != Image.FORMAT_RF:
		img.convert(Image.FORMAT_RF)
	
	# Resize to at least 1024x1024 (same as editor does)
	if img.get_width() < 1024 or img.get_height() < 1024:
		img = img.duplicate()
		img.resize(1024, 1024, Image.INTERPOLATE_CUBIC)
	
	var tex: ImageTexture = ImageTexture.create_from_image(img)
	
	# Set brush in brush_data (format: [Image, ImageTexture])
	_brush_data["brush"] = [img, tex]
	
	# Update editor with new brush
	if editor:
		editor.set_brush_data(_brush_data)

func paint_at_world_position(world_pos: Vector3, finger_id: int, phase: String, pressure := 1.0) -> void:
	if not terrain or not editor or not terrain.data:
		return
	terrain.set_camera(_camera)
	_brush_data["mouse_pressure"] = pressure
	editor.set_brush_data(_brush_data)
	if phase == "start":
		editor.start_operation(world_pos)
		_touch_operations[finger_id] = true
		editor.operate(world_pos, _camera.rotation.y)
	elif phase == "move":
		if _touch_operations.has(finger_id) and editor.is_operating():
			editor.operate(world_pos, _camera.rotation.y)
	elif phase == "end":
		if _touch_operations.has(finger_id) and editor.is_operating():
			if not editor.get_terrain():
				editor.set_terrain(terrain)
			if editor.get_terrain():
				editor.stop_operation()
			_touch_operations.erase(finger_id)

func _unhandled_input(event: InputEvent) -> void:
	if not terrain or not _camera or not editor:
		return
	
	# Ensure terrain is valid
	if not terrain.data:
		return
	
	# Handle mouse button
	if event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_LEFT:
		if event.pressed:
			if raise_on_left_mouse:
				_is_dragging = true
				# Get intersection and start operation (same as editor)
				var mouse_pos: Vector2 = event.position
				var origin: Vector3 = _camera.project_ray_origin(mouse_pos)
				var dir: Vector3 = _camera.project_ray_normal(mouse_pos)
				var hit: Vector3 = terrain.get_intersection(origin, dir, true)
				
				if hit.z <= 3.4e38 and not is_nan(hit.y):
					terrain.set_camera(_camera)
					
					# Check for Ctrl to invert (Lower)
					var invert := lower_with_ctrl and (Input.is_key_pressed(KEY_CTRL) or Input.is_key_pressed(KEY_META))
					if invert:
						editor.set_operation(Terrain3DEditor.ADD)
					else:
						editor.set_operation(Terrain3DEditor.SUBTRACT)
					
					# Update modifiers in brush_data
					_brush_data["modifier_ctrl"] = invert
					_brush_data["modifier_shift"] = Input.is_key_pressed(KEY_SHIFT)
					_brush_data["modifier_alt"] = Input.is_key_pressed(KEY_ALT) or Input.is_key_pressed(KEY_META)
					editor.set_brush_data(_brush_data)
					
					# Start operation and operate (exactly like editor does)
					if not editor.get_terrain():
						editor.set_terrain(terrain)
						if not editor.get_terrain():
							return
					editor.start_operation(hit)

					if editor.is_operating():
						editor.operate(hit, _camera.rotation.y)
		else:
			# Mouse released - stop operation (same as editor)
			if _is_dragging:
				if editor.is_operating():
					if editor.get_terrain():
						editor.set_terrain(terrain)
					else:
						editor.set_operation(Terrain3DEditor.OP_MAX)
				_is_dragging = false
		get_viewport().set_input_as_handled()
		return
	
	# Handle mouse motion while dragging (same as editor)
	if event is InputEventMouseMotion and _is_dragging:
		var mouse_pos: Vector2 = event.position
		var origin: Vector3 = _camera.project_ray_origin(mouse_pos)
		var dir: Vector3 = _camera.project_ray_normal(mouse_pos)
		var hit: Vector3 = terrain.get_intersection(origin, dir, true)
		
		if hit.z <= 3.4e38 and not is_nan(hit.y):
			terrain.set_camera(_camera)
			
			# Check for Ctrl to invert (Lower)
			var invert := lower_with_ctrl and (Input.is_key_pressed(KEY_CTRL) or Input.is_key_pressed(KEY_META))
			
			# Update modifiers
			_brush_data["modifier_ctrl"] = invert
			_brush_data["modifier_shift"] = Input.is_key_pressed(KEY_SHIFT)
			_brush_data["modifier_alt"] = Input.is_key_pressed(KEY_ALT) or Input.is_key_pressed(KEY_META)
			_brush_data["mouse_pressure"] = event.pressure if event.pressure > 0.0 else 1.0
			editor.set_brush_data(_brush_data)
			
			# Set operation based on invert
			if invert:
				editor.set_operation(Terrain3DEditor.ADD)
			else:
				editor.set_operation(Terrain3DEditor.SUBTRACT)
			
			# Operate (same as editor does continuously while dragging)
			if editor.is_operating():
				editor.operate(hit, _camera.rotation.y)
		get_viewport().set_input_as_handled()


func _track_original_regions() -> void:
	if not terrain or not terrain.data:
		return
	
	# Get the data directory
	_data_directory = terrain.data_directory
	if _data_directory.is_empty():
		_data_directory = "res://terrain_data"
	
	# Convert res:// path to project path if needed
	var dir_path: String = _data_directory
	if dir_path.begins_with("res://"):
		dir_path = ProjectSettings.globalize_path(dir_path)
		# On Windows, res:// might need special handling
		if not DirAccess.dir_exists_absolute(dir_path):
			# Try ProjectSettings.get_resource_path() approach
			var resource_path = ProjectSettings.globalize_path("res://")
			dir_path = resource_path.path_join(_data_directory.trim_prefix("res://"))
	
	# List all existing terrain region files
	if DirAccess.dir_exists_absolute(dir_path):
		var dir = DirAccess.open(dir_path)
		if dir:
			dir.list_dir_begin()
			var file_name = dir.get_next()
			while file_name != "":
				if file_name.begins_with("terrain3d") and file_name.ends_with(".res"):
					_original_region_files.append(file_name)
				file_name = dir.get_next()
	
	print("Terrain3DRuntimeBrush: Tracked ", _original_region_files.size(), " original region files")


func _exit_tree() -> void:
	if cleanup_on_exit:
		_cleanup_runtime_regions()
	
	if editor:
		editor.free()


func _cleanup_runtime_regions() -> void:
	if _data_directory.is_empty() or _original_region_files.is_empty():
		return
	
	var dir_path: String = _data_directory
	if dir_path.begins_with("res://"):
		dir_path = ProjectSettings.globalize_path(dir_path)
		if not DirAccess.dir_exists_absolute(dir_path):
			var resource_path = ProjectSettings.globalize_path("res://")
			dir_path = resource_path.path_join(_data_directory.trim_prefix("res://"))
	
	# Note: In Godot, we can't easily delete files at runtime if they're in res://
	# This is a limitation - files in res:// are read-only at runtime
	# However, we can at least log what would be cleaned up
	
	if not DirAccess.dir_exists_absolute(dir_path):
		print("Terrain3DRuntimeBrush: Cannot clean up - directory not accessible: ", dir_path)
		return
	
	var dir = DirAccess.open(dir_path)
	if not dir:
		print("Terrain3DRuntimeBrush: Cannot clean up - failed to open directory: ", dir_path)
		return
	
	var cleaned_count = 0
	dir.list_dir_begin()
	var file_name = dir.get_next()
	while file_name != "":
		if file_name.begins_with("terrain3d") and file_name.ends_with(".res"):
			# If this file wasn't in the original list, it's new
			if file_name not in _original_region_files:
				# Try to delete it (may fail if in res:// at runtime)
				var full_path = dir_path.path_join(file_name)
				var err = dir.remove(file_name)
				if err == OK:
					cleaned_count += 1
					print("Terrain3DRuntimeBrush: Cleaned up new region file: ", file_name)
				else:
					print("Terrain3DRuntimeBrush: Could not delete ", file_name, " (error: ", err, ") - file may be in res:// which is read-only at runtime")
		file_name = dir.get_next()
	
	if cleaned_count > 0:
		print("Terrain3DRuntimeBrush: Cleaned up ", cleaned_count, " new region file(s) on exit")
	else:
		print("Terrain3DRuntimeBrush: No new region files to clean up")


## Public functions to modify brush settings at runtime
func set_brush_size(new_size: float) -> void:
	brush_size = new_size
	_brush_data["size"] = brush_size
	if editor:
		editor.set_brush_data(_brush_data)


func set_strength(new_strength: float) -> void:
	strength = clamp(new_strength, 1.0, 100.0)
	_brush_data["strength"] = strength
	if editor:
		editor.set_brush_data(_brush_data)


func load_brush_from_path(path: String) -> void:
	_load_brush(path)
