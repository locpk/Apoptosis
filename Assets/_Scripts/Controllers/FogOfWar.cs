using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogOfWar : MonoBehaviour {
	Texture2D fow_mask;
	public Transform[] viewers;
	public int maskResolution = 512;
    int playerViewRange = 14;

    struct ViewerPos {
        public int x, y;
    };

    ViewerPos[] old_pos;

	// Use this for initialization
	void Start () {
		fow_mask = new Texture2D (maskResolution, maskResolution, TextureFormat.ARGB32, true);
		for (int x=0; x<maskResolution; x++) {
			for (int y=0; y<maskResolution; y++) {
				fow_mask.SetPixel (x, y, Color.black);
			}
		}
		GetComponent<MeshRenderer> ().material.mainTexture = fow_mask;

        old_pos = new ViewerPos[viewers.Length];

        for (int i=0; i<viewers.Length; i++) {
            old_pos[i].x = (int)viewers[i].position.x;
            old_pos[i].y = (int)viewers[i].position.z;
            DrawCircleOnMask(old_pos[i].x, old_pos[i].y, playerViewRange, 0.0f);
        }
        fow_mask.Apply ();
    }
    
	// Update is called once per frame
	void Update () {
        bool updateMask = false;
        List<Rect> dirtyRegions = new List<Rect>();

        for (int i=0; i<viewers.Length; i++) {
            int player_x = (int)viewers[i].position.x,
                player_y = (int)viewers[i].position.z;

            if (player_x != old_pos[i].x || player_y != old_pos[i].y) {
                DrawCircleOnMask(old_pos[i].x, old_pos[i].y, playerViewRange, 0.5f);

                // Mark rectangle as dirty
                Rect dirty = new Rect(player_x - playerViewRange, player_y - playerViewRange,
                                      playerViewRange * 2, playerViewRange * 2);
                dirtyRegions.Add(dirty);

                updateMask = true;
            }
        }

        for (int i=0; i<viewers.Length; i++) {
            int player_x = (int)viewers[i].position.x,
                player_y = (int)viewers[i].position.z;

            bool hasMoved = (player_x != old_pos[i].x || player_y != old_pos[i].y);
            bool isRegionDirty = false;

            if (!hasMoved) {
                Rect playerRect = new Rect(player_x - playerViewRange, player_y - playerViewRange,
                                           playerViewRange * 2, playerViewRange * 2);
                foreach (Rect region in dirtyRegions) {
                    if (playerRect.Overlaps(region)) {
                        isRegionDirty = true;
                        break;
                    }
                }
            }
            
            if (hasMoved || isRegionDirty) {
                DrawCircleOnMask(player_x, player_y, playerViewRange, 0.0f);
                
                old_pos[i].x = player_x;
                old_pos[i].y = player_y;

                updateMask = true;
            }
        }

        if (updateMask) {
            fow_mask.Apply();
        }
	}

	void DrawCircleOnMask (int _x, int _y, int _r, float alpha) {
		//fow_mask.SetPixel(_x, _y, Color.red);

		for (int x = _x - _r; x<= _x + _r; x++) {
			for (int y = _y - _r; y <= _y + _r; y++) {
				float dist_sqr = (x - _x) * (x - _x) + (y - _y) * (y - _y);
				float r_sqr = _r * _r;

				if (dist_sqr <= r_sqr) {
					fow_mask.SetPixel (x, y, new Color (0.0f, 0.0f, 0.0f, alpha));
				}
			}
		}
		//fow_mask.Apply ();
	}

    //void OnGUI() {
    //    GUI.DrawTexture(new Rect(10, 10, 210, 210), fow_mask, ScaleMode.ScaleToFit, true);
    //}

	public Texture2D GetMaskTexture() {
		return fow_mask;
	}
}
