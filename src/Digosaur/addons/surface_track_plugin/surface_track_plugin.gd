@tool
extends EditorPlugin

const SurfaceTrackPlugin = preload("res://addons/surface_track_plugin/surface_track_node.gd")

func _enter_tree():
	add_custom_type("SurfaceTrackNode", "Node", SurfaceTrackPlugin, preload("res://addons/surface_track_plugin/icon.png"))

func _exit_tree():
	remove_custom_type("SurfaceTrackNode")
