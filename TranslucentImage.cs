using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.Asset.TranslucentImage
{
	public class TranslucentImage : Image, IMeshModifier
	{
		public TranslucentImageSource source;

		[Tooltip("(De)Saturate them image, 1 is normal, 0 is black and white, below zero make the image negative")]
		[Range(-1f, 3f)]
		public float vibrancy = 1f;

		[Tooltip("Brighten/darken them image")]
		[Range(-1f, 1f)]
		public float brightness;

		[Tooltip("Flatten the color behind to help keep contrast on varying background")]
		[Range(0f, 1f)]
		public float flatten = 0.1f;

		private Shader correctShader;

		private int vibrancyPropId;

		private int brightnessPropId;

		private int flattenPropId;

		private float oldVibrancy;

		private float oldBrightness;

		private float oldFlatten;

		[Tooltip("Blend between the sprite and background blur")]
		[Range(0f, 1f)]
		public float spriteBlending = 0.65f;

		protected override void Start()
		{
			base.Start();
			PrepShader();
			oldVibrancy = vibrancy;
			oldBrightness = brightness;
			oldFlatten = flatten;
			source = ((!source) ? Object.FindObjectOfType<TranslucentImageSource>() : source);
			material.SetTexture("_BlurTex", source.BlurredScreen);
			base.canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
		}

		private void PrepShader()
		{
			correctShader = Shader.Find("UI/TranslucentImage");
			vibrancyPropId = Shader.PropertyToID("_Vibrancy");
			brightnessPropId = Shader.PropertyToID("_Brightness");
			flattenPropId = Shader.PropertyToID("_Flatten");
		}

		protected void LateUpdate()
		{
			if (!source)
			{
				Debug.LogError("Source missing. Add TranslucentImageSource component to your main camera, then drag the camera to Source slot");
			}
			else if (IsActive() && (bool)source.BlurredScreen)
			{
				if (!material || material.shader != correctShader)
				{
					Debug.LogError("Material using \"UI/TranslucentImage\" is required");
				}
				materialForRendering.SetTexture("_BlurTex", source.BlurredScreen);
			}
		}

		private void Update()
		{
			if (source == null)
			{
				source = Object.FindObjectOfType<TranslucentImageSource>();
			}
			if (vibrancyPropId != 0 && brightnessPropId != 0 && flattenPropId != 0)
			{
				SyncMaterialProperty(vibrancyPropId, ref vibrancy, ref oldVibrancy);
				SyncMaterialProperty(brightnessPropId, ref brightness, ref oldBrightness);
				SyncMaterialProperty(flattenPropId, ref flatten, ref oldFlatten);
			}
		}

		private void SyncMaterialProperty(int propId, ref float value, ref float oldValue)
		{
			float @float = materialForRendering.GetFloat(propId);
			if (!Mathf.Approximately(@float, value))
			{
				if (!Mathf.Approximately(value, oldValue))
				{
					material.SetFloat(propId, value);
					materialForRendering.SetFloat(propId, value);
					SetMaterialDirty();
				}
				else
				{
					value = @float;
				}
			}
			oldValue = value;
		}

		public virtual void ModifyMesh(VertexHelper vh)
		{
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			for (int i = 0; i < list.Count; i++)
			{
				UIVertex value = list[i];
				value.uv1 = new Vector2(spriteBlending, 0f);
				list[i] = value;
			}
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			SetVerticesDirty();
		}

		protected override void OnDisable()
		{
			SetVerticesDirty();
			base.OnDisable();
		}

		protected override void OnDidApplyAnimationProperties()
		{
			SetVerticesDirty();
			base.OnDidApplyAnimationProperties();
		}

		public virtual void ModifyMesh(Mesh mesh)
		{
			using (VertexHelper vertexHelper = new VertexHelper(mesh))
			{
				ModifyMesh(vertexHelper);
				vertexHelper.FillMesh(mesh);
			}
		}
	}
}
