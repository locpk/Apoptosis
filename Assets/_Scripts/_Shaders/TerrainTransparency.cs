using UnityEngine;
using Random = System.Random;

//[ExecuteInEditMode]
public class TerrainTransparency : MonoBehaviour {
	private bool disableBasemap = true;
	private float alphaCutoff = .5f;
	public bool autoUpdateTransparencyMap = true;
	
	public Texture2D transparencyMap;

	Terrain terrain;
	TerrainData tData;
	Material tMaterial;
	void Update() {
		terrain = GetComponent<Terrain>();
		tData = terrain ? terrain.terrainData : null;
		tMaterial = terrain ? terrain.materialTemplate : null;
		if (!terrain || !tData || !tMaterial) return;
		
		if(disableBasemap && !Application.isPlaying && GetComponent<Terrain>().basemapDistance != 1000000)
			GetComponent<Terrain>().basemapDistance = 1000000;
		{
			var alphaCutoff_final = Application.isPlaying ? alphaCutoff + .00001f : alphaCutoff;
			tMaterial.SetFloat("_AlphaCutoff", alphaCutoff_final);
			tMaterial.SetFloat("_AlphaCutoff_2", alphaCutoff_final);
		}

		if (!transparencyMap && autoUpdateTransparencyMap) {
			UpdateTransparencyMap();
			ApplyTransparencyMap();
        } else {
			ApplyTransparencyMap();
        }
	}

	public void UpdateTransparencyMap() {
		var newTransparencyMapValues = new Color[tData.alphamapResolution, tData.alphamapResolution];
		for (var slotIndex = 0; slotIndex < tData.alphamapLayers; slotIndex++) {
			SplatPrototype slotTexture = tData.splatPrototypes[slotIndex];

			if (slotTexture.texture.name == "Transparent") {
				float[,,] slotApplicationMapValues = tData.GetAlphamaps(0, 0, tData.alphamapResolution, tData.alphamapResolution);
				for (var a = 0; a < tData.alphamapResolution; a++)
					for (var b = 0; b < tData.alphamapResolution; b++) {
						float textureStrength = slotApplicationMapValues[a, b, slotIndex];
						var newColor = new Color(0, 0, 0, textureStrength);
						newTransparencyMapValues[b, a] = newColor;
					}
				break;
			}
		}
		bool transparencyMapNeedsUpdating = !transparencyMap;
		if (!transparencyMapNeedsUpdating) {
			Color[] transparencyMap_colors = transparencyMap.GetPixels();
            if (transparencyMap.width != tData.alphamapResolution || transparencyMap.height != tData.alphamapResolution) {
				transparencyMapNeedsUpdating = true;
            }
            if (!transparencyMapNeedsUpdating) {
                for (var a = 0; a < tData.alphamapResolution; a++) {
                    for (var b = 0; b < tData.alphamapResolution; b++) {
                        if (transparencyMap_colors[(a * tData.alphamapResolution) + b] != newTransparencyMapValues[b, a]) {
							transparencyMapNeedsUpdating = true;
							break;
						}
                    }
                }
            }
		}

		if (transparencyMapNeedsUpdating) {
			if (transparencyMap) {
				DestroyImmediate(transparencyMap);
				transparencyMap = null;
			}
            if (!transparencyMap) {
				transparencyMap = new Texture2D(tData.alphamapResolution, tData.alphamapResolution);
            }

            for (var a = 0; a < tData.alphamapResolution; a++) {
                for (var b = 0; b < tData.alphamapResolution; b++) {
					transparencyMap.SetPixel(a, b, newTransparencyMapValues[a, b]);
                }
            }
			transparencyMap.Apply();
		}
	}
	public void ApplyTransparencyMap() {
		tMaterial.SetTexture("_TransparencyMap", transparencyMap);
	}
}