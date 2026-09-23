@tool
extends EditorPlugin


func _enable_plugin() -> void:
    add_autoload_singleton("DeDraw", "res://addons/de_draw/autoload/dedraw_global.gd")


func _disable_plugin() -> void:
    remove_autoload_singleton("DeDraw")


func _enter_tree() -> void:
    # Initialization of the plugin goes here.
    pass


func _exit_tree() -> void:
    # Clean-up of the plugin goes here.
    pass
