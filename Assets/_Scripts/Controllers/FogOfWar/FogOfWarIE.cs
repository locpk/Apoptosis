using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FogOfWarIE : MonoBehaviour {

	public Shader shader;

	Camera mCam;
	FogOfWarController mFog;
	Matrix4x4 mInverseMVP;
	Material mMat;

	void OnEnable () {
		mCam = GetComponent<Camera>();
		mCam.depthTextureMode = DepthTextureMode.Depth;
		if (shader == null) shader = Shader.Find("Custom/FOW");
	}

	void OnDisable () { if (mMat) DestroyImmediate(mMat); }

	void Start () {
		if (!SystemInfo.supportsImageEffects || !shader || !shader.isSupported)
			enabled = false;
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if (mFog == null) {
			mFog = FogOfWarController.instance;
			if (mFog == null) mFog = FindObjectOfType(typeof(FogOfWarController)) as FogOfWarController;
		}

		if (mFog == null || !mFog.enabled) {
			enabled = false;
			return;
		}

		mInverseMVP = (mCam.projectionMatrix * mCam.worldToCameraMatrix).inverse;

		float invScale = 1f / mFog.worldSize;
		Transform t = mFog.transform;
		float x = t.position.x - mFog.worldSize * 0.5f;
		float z = t.position.z - mFog.worldSize * 0.5f;

		if (mMat == null) {
			mMat = new Material(shader);
			mMat.hideFlags = HideFlags.HideAndDontSave;
		}

		Vector4 camPos = mCam.transform.position;

		if (QualitySettings.antiAliasing > 0) {
			RuntimePlatform pl = Application.platform;

			if (pl == RuntimePlatform.WindowsEditor ||
				pl == RuntimePlatform.WindowsPlayer ||
				pl == RuntimePlatform.WindowsWebPlayer) {
				camPos.w = 1f;
			}
		}

		Vector4 p = new Vector4(-x * invScale, -z * invScale, invScale, mFog.blendFactor);
		mMat.SetColor("_Unexplored", mFog.unexploredColor);
		mMat.SetColor("_Explored", mFog.exploredColor);
		mMat.SetTexture("_FogTex0", mFog.texture0);
		mMat.SetTexture("_FogTex1", mFog.texture1);
		mMat.SetMatrix("_InverseMVP", mInverseMVP);
		mMat.SetVector("_CamPos", camPos);
		mMat.SetVector("_Params", p);

		Graphics.Blit(source, destination, mMat);
	}
}
