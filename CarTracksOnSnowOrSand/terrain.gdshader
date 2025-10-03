/*Shader code from https://godotshaders.com/shader/wandering-clipmap-stylized-terrain/
Heightmap and some changes (world-position --> UV) from https://godotshaders.com/shader/car-tracks-on-snow-or-sand-using-viewport-textures-and-particles/
Normalization code and some functions from https://docs.godotengine.org/en/3.0/tutorials/3d/vertex_displacement_with_shaders.html
changes done 3 oct 2025 by Kristin Eriksson

How to apply:
Add heightmap, change heightmap_scale & Slope_edge_noise_scale to same number (ex 1)
Heightmap_height_scale is max height of mountains/hills
Heightmap_normals_intensity makes more grainy(?)
*/

shader_type spatial;
render_mode diffuse_lambert, specular_toon;


group_uniforms heightmaps;
uniform sampler2D heightmap:hint_default_white;
//uniform sampler2D heightmap_normals;
uniform float heightmap_normals_intensity : hint_range(0.0, 1.0);
uniform sampler2D splatmap;
uniform float heightmap_scale;
uniform float heightmap_height_scale;

group_uniforms slope_definition;
//uniform float slope_shift : hint_range(-1.0, 1.0);
uniform float slope_edge_1 : hint_range(0, 1.0);
uniform float slope_edge_2 : hint_range(0, 1.0);
uniform float slope_edge_3 : hint_range(0, 1.0);
uniform sampler2D slope_edge_noise;
uniform float slope_edge_noise_scale;
uniform float slope_edge_noise_intensity : hint_range(0.0, 1.0);

group_uniforms colors;
uniform vec3 slope_color_remap_top : source_color = vec3(1, 1, 1);
uniform vec3 slope_color_remap_bottom : source_color;
uniform vec3 flat_color_remap_top : source_color = vec3(1, 1, 1);
uniform vec3 flat_color_remap_bottom : source_color;

group_uniforms textures;
uniform sampler2D flat_albedo : hint_default_white;
uniform sampler2D flat_normal : hint_normal;
uniform float flat_uv_scale = 1.0;
uniform float flat_normal_scale : hint_range(0.0, 1.0);
uniform sampler2D slope_albedo : hint_default_white;
uniform sampler2D slope_normal : hint_normal;
uniform float slope_uv_scale = 1.0;
uniform float slope_normal_scale : hint_range(0.0, 1.0);

group_uniforms properties;
uniform float flat_specular : hint_range(0.0, 1.0);
uniform float flat_roughness : hint_range(0.0, 1.0);
uniform float flat_metallic : hint_range(0.0, 1.0);
uniform float flat_normal_intensity : hint_range(0.0, 1.0);
uniform float slope_specular : hint_range(0.0, 1.0);
uniform float slope_roughness : hint_range(0.0, 1.0);
uniform float slope_metallic : hint_range(0.0, 1.0);
uniform float slope_normal_intensity : hint_range(0.0, 1.0);

//varying vec2 world_position;
varying vec3 vert_normal;


vec3 get_triplanarized_map(sampler2D sampler, vec4 projected_coords, vec3 normal_weights, float uv_scale) {
	vec3 texX = texture(sampler, projected_coords.zy * uv_scale).rgb;
	vec3 texY = texture(sampler, projected_coords.xz * uv_scale).rgb;
	vec3 texZ = texture(sampler, projected_coords.xy * uv_scale).rgb;
	return texX * normal_weights.x + texY * normal_weights.y + texZ * normal_weights.z;
}



void fragment() {
	vec4 projected_coords = INV_VIEW_MATRIX * vec4(VERTEX, 1.0);
	vec3 world_normal = abs(INV_VIEW_MATRIX * vec4(NORMAL, 0.0)).xyz;
	vec3 normal_weights = world_normal / (world_normal.x + world_normal.y + world_normal.z);

	vec3 triplanarized_slope_albedo = get_triplanarized_map(slope_albedo, projected_coords, normal_weights, slope_uv_scale * 0.01);
	vec3 triplanarized_slope_normal = get_triplanarized_map(slope_normal, projected_coords, normal_weights, slope_uv_scale * 0.01);
	vec3 triplanarized_flat_albedo = get_triplanarized_map(flat_albedo, projected_coords, normal_weights, flat_uv_scale * 0.01);
	vec3 triplanarized_flat_normal = get_triplanarized_map(flat_normal, projected_coords, normal_weights, flat_uv_scale * 0.01);

	float steepness = dot(world_normal, vec3(0.0, 1.0, 0.0));
	float inv_steepness = (steepness * -1.0) + 1.0;
	vec4 c = texture(slope_edge_noise, UV);
	c.r=1.0-c.r;
	c.g=1.0-c.g;
	c.b=1.0-c.b;
	//COLOR.rgba=c;

	float slope_edge_noise_remap = mix(1, texture(slope_edge_noise, UV/*world_position*/ * slope_edge_noise_scale).r, slope_edge_noise_intensity);
	float slope = texture(splatmap, UV/*world_position*/).r * slope_edge_noise_remap;
	
	
	float edge_1 = step(slope_edge_1, slope);
	float edge_2 = step(slope_edge_2, slope);
	float edge_3 = step(slope_edge_3, slope);
	float slope_mask = edge_1 * (2.0 / 5.0) + edge_2 * (2.0 / 5.0) + edge_3 * (1.0 / 5.0);
	float slope_mask_inverted = (slope_mask * -1.0) + 1.0;

	vec3 slope_color = mix(slope_color_remap_bottom, slope_color_remap_top, triplanarized_slope_albedo.r);
	vec3 flat_color = mix(flat_color_remap_bottom, flat_color_remap_top, triplanarized_flat_albedo.r);

	vec3 slope_normal_scaled = mix(vec3(0.5, 0.5, 1), triplanarized_slope_normal, slope_normal_scale);
	vec3 flat_normal_scaled = mix(vec3(0.5, 0.5, 1), triplanarized_flat_normal, flat_normal_scale);
	
	ALBEDO = mix(slope_color, flat_color, slope_mask_inverted);
	//ALBEDO = vec3(slope_mask);
	SPECULAR = mix(slope_specular, flat_specular, slope_mask_inverted);
	METALLIC = mix(slope_metallic, flat_metallic, slope_mask_inverted);
	ROUGHNESS = mix(slope_roughness, flat_roughness, slope_mask_inverted);
	NORMAL_MAP = mix(slope_normal_scaled, flat_normal_scaled, slope_mask_inverted);
	NORMAL_MAP_DEPTH = mix(slope_normal_intensity, flat_normal_intensity, slope_mask_inverted);
}
// from godot
float hash(vec2 p) {
  return fract(sin(dot(p * 17.17, vec2(14.91, 67.31))) * 4791.9511);
}
float noise(vec2 x) {
  vec2 p = floor(x);
  vec2 f = fract(x);
  f = f * f * (3.0 - 2.0 * f);
  vec2 a = vec2(1.0, 0.0);
  return mix(mix(hash(p + a.yy), hash(p + a.xy), f.x),
         mix(hash(p + a.yx), hash(p + a.xx), f.x), f.y);
}
float fbm(vec2 x) {
  float height = 0.0;
  float amplitude = 0.5;
  float frequency = 3.0;
  for (int i = 0; i < 6; i++){
    height += noise(x * frequency) * amplitude;
    amplitude *= 0.5;
    frequency *= 2.0;
  }
  return height;
}

void vertex() {
	float pixel_size = 1.0 / float(textureSize(heightmap, 0).x);
	vec2 half_pixel_offset = vec2(pixel_size, pixel_size) / 2.0;

	float heightmap_size = float(textureSize(heightmap, 0).x);
	
	//world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xz * 1.0 / (heightmap_size * heightmap_scale) + vec2(0.5) + half_pixel_offset; 
	VERTEX.y += texture(heightmap, UV/*world_position*/).r * heightmap_height_scale;
	
	vec2 e = vec2(0.01, 0.0);
	vec3 normal = normalize(vec3(fbm(VERTEX.xz - e) - fbm(VERTEX.xz + e), 2.0 * e.x, fbm(VERTEX.xz - e.yx) - fbm(VERTEX.xz + e.yx)));
	//vec3 total_normal = unpack_normalmap(texture(heightmap_normals, world_position));
	vert_normal = mix(vec3(0.5, 0.5, 1.0), normal, heightmap_normals_intensity);
	
	NORMAL = vert_normal;
}