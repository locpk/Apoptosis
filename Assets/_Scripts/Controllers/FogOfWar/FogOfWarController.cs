using UnityEngine;
using System.Collections;
using System.Threading;

public class FogOfWarController : MonoBehaviour {

    public enum LOSChecks { None, OnlyOnce, EveryUpdate }

    public class Viewer {
		public bool isActive = false;
		public LOSChecks los = LOSChecks.None;
		public Vector3 pos = Vector3.zero;
		public float inner = 0f, outer = 0f;
		public bool[] cachedBuffer;
		public int cachedSize = 0, cachedX = 0, cachedY = 0;
	}

	public enum State { Blending, NeedUpdate, UpdateTexture0, UpdateTexture1 }

	static public FogOfWarController instance;

	static XList<Viewer> m_viewers = new XList<Viewer>();
	static XList<Viewer> m_added = new XList<Viewer>();
	static XList<Viewer> m_removed = new XList<Viewer>();

    protected int[,] m_heights;
	protected Transform m_trans;
	protected Vector3 m_origin = Vector3.zero;
	protected Vector3 m_size = Vector3.one;

    protected Color32[] m_buffer0;
	protected Color32[] m_buffer1;
	protected Color32[] m_buffer2;

	protected Texture2D m_texture0;
	protected Texture2D m_texture1;

	protected float m_blendFactor = 0f;
	protected float m_nextUpdate = 0f;
	protected int m_screenHeight = 0;
	protected State m_state = State.Blending;

    private Thread m_thread;
	private float m_elapsed = 0f;

	public Color unexploredColor = new Color(0.05f, 0.05f, 0.05f, 1f);
	public Color exploredColor = new Color(0.2f, 0.2f, 0.2f, 1f);
	public int worldSize = 256;
	public int textureSize = 128;
	public float updateFrequency = 0.25f;
	public float textureBlendTime = 1f;
	public int blurIterations = 2;
	public Vector2 heightRange = new Vector2(0f, 10f);
	public LayerMask raycastMask = -1;
	public float raycastRadius = 1f;
	public float margin = 0.4f;
	public bool debug = false;

	public Texture2D texture0 { get { return m_texture0; } }
	public Texture2D texture1 { get { return m_texture1; } }
	public float blendFactor { get { return m_blendFactor; } }

    static public Viewer CreateViewer() {
		Viewer viewer = new Viewer();
		viewer.isActive = false;
        lock (m_added) {
            m_added.Add(viewer);
        }
		return viewer;
	}

	static public void DeleteViewer(Viewer viewer) {
        lock (m_removed) {
            m_removed.Add(viewer); 
        }
    }

    void Awake() {
        instance = this;
    }

    void Start() {
        m_trans = transform;
		m_heights = new int[textureSize, textureSize];
		m_size = new Vector3(worldSize, heightRange.y - heightRange.x, worldSize);

		m_origin = m_trans.position;
		m_origin.x -= worldSize * 0.5f;
		m_origin.z -= worldSize * 0.5f;

		int size = textureSize * textureSize;
		m_buffer0 = new Color32[size];
		m_buffer1 = new Color32[size];
		m_buffer2 = new Color32[size];

		// Create the height grid
		CreateGrid();

		// Update the fog of war's visibility so that it's updated right away
		UpdateBuffer();
		UpdateTexture();
		m_nextUpdate = Time.time + updateFrequency;

		// Add a thread update function -- all visibility checks will be done on a separate thread
		m_thread = new Thread(ThreadUpdate);
		m_thread.Start();
    }

    void OnDestroy() {
		if (m_thread != null) {
			m_thread.Abort();
			while (m_thread.IsAlive) Thread.Sleep(1);
			m_thread = null;
		}
	}

	void Update() {
        if (textureBlendTime > 0.0f) {
            m_blendFactor = Mathf.Clamp01(m_blendFactor + Time.deltaTime / textureBlendTime);
        } else {
            m_blendFactor = 1.0f;
        }

		if (m_state == State.Blending) {
			float time = Time.time;
			if (m_nextUpdate < time) {
				m_nextUpdate = time + updateFrequency;
				m_state = State.NeedUpdate;
			}
		} else if (m_state != State.NeedUpdate) {
			UpdateTexture();
		}
	}

    void LateUpdate() {
		float invScale = 1f / worldSize;
		float x = m_trans.position.x - worldSize * 0.5f;
		float z = m_trans.position.z - worldSize * 0.5f;
		Vector4 p = new Vector4(-x * invScale, -z * invScale, invScale, m_blendFactor);

		Shader.SetGlobalColor("_FOWUnexplored", unexploredColor);
		Shader.SetGlobalColor("_FOWExplored", exploredColor);
		Shader.SetGlobalVector("_FOWParams", p);
		Shader.SetGlobalTexture("_FOWTex0", m_texture0);
		Shader.SetGlobalTexture("_FOWTex1", m_texture1);
	}

    void ThreadUpdate() {
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		for (;;) {
			if (m_state == State.NeedUpdate) {
				sw.Reset();
				sw.Start();
				UpdateBuffer();
				sw.Stop();
				if (debug) Debug.Log(sw.ElapsedMilliseconds);
				m_elapsed = 0.001f * (float)sw.ElapsedMilliseconds;
				m_state = State.UpdateTexture0;
			}
			Thread.Sleep(1);
		}
	}

    void OnDrawGizmosSelected() {
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(new Vector3(0f, (heightRange.x + heightRange.y) * 0.5f, 0f),
			new Vector3(worldSize, heightRange.y - heightRange.x, worldSize));
	}

    bool IsVisible (int sx, int sy, int fx, int fy, float outer, int sightHeight, int variance) {
		int dx = Mathf.Abs(fx - sx);
		int dy = Mathf.Abs(fy - sy);
		int ax = sx < fx ? 1 : -1;
		int ay = sy < fy ? 1 : -1;
		int dir = dx - dy;
		float sh = sightHeight;
		float fh = m_heights[fx, fy];
		float invDist = 1.0f / outer;
		float lerpFactor = 0.0f;
		for (;;) {
			if (sx == fx && sy == fy) return true;
			int xd = fx - sx;
			int yd = fy - sy;
			lerpFactor = invDist * Mathf.Sqrt(xd * xd + yd * yd);
			if (m_heights[sx, sy] > Mathf.Lerp(fh, sh, lerpFactor) + variance) return false;
			
			int dir2 = dir << 1;

			if (dir2 > -dy) {
				dir -= dy;
				sx += ax;
			}

			if (dir2 < dx) {
				dir += dx;
				sy += ay;
			}
		}
	}

    public int WorldToGridHeight (float height) {
		int val = Mathf.RoundToInt(height / m_size.y * 255.0f);
		return Mathf.Clamp(val, 0, 255);
	}

    protected virtual void CreateGrid() {
		Vector3 pos = m_origin;
		pos.y += m_size.y;
		float texToWorld = (float)worldSize / textureSize;
		bool useSphereCast = raycastRadius > 0f;

		for (int z = 0; z < textureSize; ++z) {
			pos.z = m_origin.z + z * texToWorld;

			for (int x = 0; x < textureSize; ++x) {
				pos.x = m_origin.x + x * texToWorld;

				RaycastHit hit;

				if (useSphereCast) {
					if (Physics.SphereCast(new Ray(pos, Vector3.down), raycastRadius, out hit, m_size.y, raycastMask)) {
						m_heights[x, z] = WorldToGridHeight(pos.y - hit.distance - raycastRadius);
						continue;
					}
				} else if (Physics.Raycast(new Ray(pos, Vector3.down), out hit, m_size.y, raycastMask)) {
					m_heights[x, z] = WorldToGridHeight(pos.y - hit.distance);
					continue;
				}
				m_heights[x, z] = 0;
			}
		}
	}

    void UpdateBuffer () {
		// Add all items scheduled to be added
		if (m_added.size > 0) {
			lock (m_added) {
				while (m_added.size > 0) {
					int index = m_added.size - 1;
					m_viewers.Add(m_added.buffer[index]);
					m_added.RemoveAt(index);
				}
			}
		}

		if (m_removed.size > 0) {
			lock (m_removed) {
				while (m_removed.size > 0) {
					int index = m_removed.size - 1;
					m_viewers.Remove(m_removed.buffer[index]);
					m_removed.RemoveAt(index);
				}
			}
		}

		float factor = (textureBlendTime > 0.0f) ? Mathf.Clamp01(m_blendFactor + m_elapsed / textureBlendTime) : 1.0f;

		for (int i = 0, imax = m_buffer0.Length; i < imax; ++i) {
			m_buffer0[i] = Color32.Lerp(m_buffer0[i], m_buffer1[i], factor);
			m_buffer1[i].r = 0;
		}

		float worldToTex = (float)textureSize / worldSize;

		for (int i = 0; i < m_viewers.size; ++i) {
			Viewer viewer = m_viewers[i];
			if (!viewer.isActive) continue;
			
			if (viewer.los == LOSChecks.None) {
				ViewUsingRadius(viewer, worldToTex);
			} else if (viewer.los == LOSChecks.OnlyOnce) {
				ViewUsingCache(viewer, worldToTex);
			} else {
				ViewUsingLOS(viewer, worldToTex);
			}
		}

		// Blur the final visibility data
		for (int i = 0; i < blurIterations; ++i) BlurVisibility();

		// View the map based on what's currently visible
		ViewMap();
	}

	void ViewUsingRadius (Viewer r, float worldToTex) {
		Vector3 pos = r.pos - m_origin;

		int xmin = Mathf.RoundToInt((pos.x - r.outer) * worldToTex);
		int ymin = Mathf.RoundToInt((pos.z - r.outer) * worldToTex);
		int xmax = Mathf.RoundToInt((pos.x + r.outer) * worldToTex);
		int ymax = Mathf.RoundToInt((pos.z + r.outer) * worldToTex);

		int cx = Mathf.RoundToInt(pos.x * worldToTex);
		int cy = Mathf.RoundToInt(pos.z * worldToTex);

		cx = Mathf.Clamp(cx, 0, textureSize - 1);
		cy = Mathf.Clamp(cy, 0, textureSize - 1);

		int radius = Mathf.RoundToInt(r.outer * r.outer * worldToTex * worldToTex);

		for (int y = ymin; y < ymax; ++y) {
			if (y > -1 && y < textureSize) {
				int yw = y * textureSize;

				for (int x = xmin; x < xmax; ++x) {
					if (x > -1 && x < textureSize) {
						int xd = x - cx;
						int yd = y - cy;
						int dist = xd * xd + yd * yd;

						// View this pixel
						if (dist < radius) m_buffer1[x + yw].r = 255;
					}
				}
			}
		}
	}

    void ViewUsingLOS (Viewer r, float worldToTex) {
		Vector3 pos = r.pos - m_origin;

		int xmin = Mathf.RoundToInt((pos.x - r.outer) * worldToTex);
		int ymin = Mathf.RoundToInt((pos.z - r.outer) * worldToTex);
		int xmax = Mathf.RoundToInt((pos.x + r.outer) * worldToTex);
		int ymax = Mathf.RoundToInt((pos.z + r.outer) * worldToTex);

		xmin = Mathf.Clamp(xmin, 0, textureSize - 1);
		xmax = Mathf.Clamp(xmax, 0, textureSize - 1);
		ymin = Mathf.Clamp(ymin, 0, textureSize - 1);
		ymax = Mathf.Clamp(ymax, 0, textureSize - 1);
		
		int cx = Mathf.RoundToInt(pos.x * worldToTex);
		int cy = Mathf.RoundToInt(pos.z * worldToTex);

		cx = Mathf.Clamp(cx, 0, textureSize - 1);
		cy = Mathf.Clamp(cy, 0, textureSize - 1);

		int minRange = Mathf.RoundToInt(r.inner * r.inner * worldToTex * worldToTex);
		int maxRange = Mathf.RoundToInt(r.outer * r.outer * worldToTex * worldToTex);
		int gh = WorldToGridHeight(r.pos.y);
		int variance = Mathf.RoundToInt(Mathf.Clamp01(margin / (heightRange.y - heightRange.x)) * 255);
		Color32 white = new Color32(255, 255, 255, 255);
		
		for (int y = ymin; y < ymax; ++y) {
			for (int x = xmin; x < xmax; ++x) {
				int xd = x - cx;
				int yd = y - cy;
				int dist = xd * xd + yd * yd;
				int index = x + y * textureSize;

				if (dist < minRange || (cx == x && cy == y)) {
					m_buffer1[index] = white;
				} else if (dist < maxRange) {
					Vector2 v = new Vector2(xd, yd);
					v.Normalize();
					v *= r.inner;

					int sx = cx + Mathf.RoundToInt(v.x);
					int sy = cy + Mathf.RoundToInt(v.y);

					if (sx > -1 && sx < textureSize &&
						sy > -1 && sy < textureSize &&
						IsVisible(sx, sy, x, y, Mathf.Sqrt(dist), gh, variance)) {
						m_buffer1[index] = white;
					}
				}
			}
		}
	}

    void ViewUsingCache (Viewer r, float worldToTex) {
		if (r.cachedBuffer == null) ViewIntoCache(r, worldToTex);

		Color32 white = new Color32(255, 255, 255, 255);

		for (int y = r.cachedY, ymax = r.cachedY + r.cachedSize; y < ymax; ++y) {
			if (y > -1 && y < textureSize) {
				int by = y * textureSize;
				int cy = (y - r.cachedY) * r.cachedSize;

				for (int x = r.cachedX, xmax = r.cachedX + r.cachedSize; x < xmax; ++x) {
					if (x > -1 && x < textureSize) {
						int cachedIndex = x - r.cachedX + cy;

						if (r.cachedBuffer[cachedIndex]) {
							m_buffer1[x + by] = white;
						}
					}
				}
			}
		}
	}

    void ViewIntoCache (Viewer r, float worldToTex) {
		Vector3 pos = r.pos - m_origin;

		int xmin = Mathf.RoundToInt((pos.x - r.outer) * worldToTex);
		int ymin = Mathf.RoundToInt((pos.z - r.outer) * worldToTex);
		int xmax = Mathf.RoundToInt((pos.x + r.outer) * worldToTex);
		int ymax = Mathf.RoundToInt((pos.z + r.outer) * worldToTex);

		int cx = Mathf.RoundToInt(pos.x * worldToTex);
		int cy = Mathf.RoundToInt(pos.z * worldToTex);

		cx = Mathf.Clamp(cx, 0, textureSize - 1);
		cy = Mathf.Clamp(cy, 0, textureSize - 1);

		int size = Mathf.RoundToInt(xmax - xmin);		
		r.cachedBuffer = new bool[size * size];
		r.cachedSize = size;
		r.cachedX = xmin;
		r.cachedY = ymin;

		for (int i = 0, imax = size * size; i < imax; ++i) r.cachedBuffer[i] = false;

		int minRange = Mathf.RoundToInt(r.inner * r.inner * worldToTex * worldToTex);
		int maxRange = Mathf.RoundToInt(r.outer * r.outer * worldToTex * worldToTex);
		int variance = Mathf.RoundToInt(Mathf.Clamp01(margin / (heightRange.y - heightRange.x)) * 255);
		int gh = WorldToGridHeight(r.pos.y);

		for (int y = ymin; y < ymax; ++y) {
			if (y > -1 && y < textureSize) {
				for (int x = xmin; x < xmax; ++x) {
					if (x > -1 && x < textureSize) {
						int xd = x - cx;
						int yd = y - cy;
						int dist = xd * xd + yd * yd;

						if (dist < minRange || (cx == x && cy == y)) {
							r.cachedBuffer[(x - xmin) + (y - ymin) * size] = true;
						} else if (dist < maxRange) {
							Vector2 v = new Vector2(xd, yd);
							v.Normalize();
							v *= r.inner;

							int sx = cx + Mathf.RoundToInt(v.x);
							int sy = cy + Mathf.RoundToInt(v.y);

							if (sx > -1 && sx < textureSize &&
								sy > -1 && sy < textureSize &&
								IsVisible(sx, sy, x, y, Mathf.Sqrt(dist), gh, variance) ) {
								r.cachedBuffer[(x - xmin) + (y - ymin) * size] = true;
							}
						}
					}
				}
			}
		}
	}

    void BlurVisibility () {
		Color32 c;

		for (int y = 0; y < textureSize; ++y) {
			int yw = y * textureSize;
			int yw0 = (y - 1);
			if (yw0 < 0) yw0 = 0;
			int yw1 = (y + 1);
			if (yw1 == textureSize) yw1 = y;

			yw0 *= textureSize;
			yw1 *= textureSize;

			for (int x = 0; x < textureSize; ++x) {
				int x0 = (x - 1);
				if (x0 < 0) x0 = 0;
				int x1 = (x + 1);
				if (x1 == textureSize) x1 = x;

				int index = x + yw;
				int val = m_buffer1[index].r;

				val += m_buffer1[x0 + yw].r;
				val += m_buffer1[x1 + yw].r;
				val += m_buffer1[x + yw0].r;
				val += m_buffer1[x + yw1].r;

				val += m_buffer1[x0 + yw0].r;
				val += m_buffer1[x1 + yw0].r;
				val += m_buffer1[x0 + yw1].r;
				val += m_buffer1[x1 + yw1].r;

				c = m_buffer2[index];
				c.r = (byte)(val / 9);
				m_buffer2[index] = c;
			}
		}

		Color32[] temp = m_buffer1;
		m_buffer1 = m_buffer2;
		m_buffer2 = temp;
	}

    void ViewMap () {
		for (int y = 0; y < textureSize; ++y) {
			int yw = y * textureSize;
			for (int x = 0; x < textureSize; ++x) {
				int index = x + yw;
				Color32 c = m_buffer1[index];
				if (c.g < c.r) {
					c.g = c.r;
					m_buffer1[index] = c;
				}
			}
		}
	}

    void UpdateTexture () {
		if (m_screenHeight != Screen.height || m_texture0 == null) {
			m_screenHeight = Screen.height;

			if (m_texture0 != null) Destroy(m_texture0);
			if (m_texture1 != null) Destroy(m_texture1);

			m_texture0 = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
			m_texture1 = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);

			m_texture0.wrapMode = TextureWrapMode.Clamp;
			m_texture1.wrapMode = TextureWrapMode.Clamp;

			m_texture0.SetPixels32(m_buffer0);
			m_texture0.Apply();
			m_texture1.SetPixels32(m_buffer1);
			m_texture1.Apply();
			m_state = State.Blending;
		} else if (m_state == State.UpdateTexture0) {
			m_texture0.SetPixels32(m_buffer0);
			m_texture0.Apply();
			m_state = State.UpdateTexture1;
			m_blendFactor = 0f;
		} else if (m_state == State.UpdateTexture1) {
			m_texture1.SetPixels32(m_buffer1);
			m_texture1.Apply();
			m_state = State.Blending;
		}
	}

    public bool IsVisible (Vector3 pos) {
		if (m_buffer0 == null) return false;
		pos -= m_origin;
		float worldToTex = (float)textureSize / worldSize;
		int cx = Mathf.RoundToInt(pos.x * worldToTex);
		int cy = Mathf.RoundToInt(pos.z * worldToTex);
		cx = Mathf.Clamp(cx, 0, textureSize - 1);
		cy = Mathf.Clamp(cy, 0, textureSize - 1);
		int index = cx + cy * textureSize;
		return m_buffer1[index].r > 0 || m_buffer0[index].r > 0;
	}

	public bool IsExplored (Vector3 pos) {
		if (m_buffer0 == null) return false;
		pos -= m_origin;

		float worldToTex = (float)textureSize / worldSize;
		int cx = Mathf.RoundToInt(pos.x * worldToTex);
		int cy = Mathf.RoundToInt(pos.z * worldToTex);

		cx = Mathf.Clamp(cx, 0, textureSize - 1);
		cy = Mathf.Clamp(cy, 0, textureSize - 1);
		return m_buffer0[cx + cy * textureSize].g > 0;
	}
}
