using UnityEngine;

namespace LeTai.Asset.TranslucentImage
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Tai Le Assets/Translucent Image Source")]
	public class TranslucentImageSource : MonoBehaviour
	{
		[Tooltip("Maximum number of times that the blur algorithm can run per second. Lower if you need more performance")]
		public float maxUpdateRate = float.PositiveInfinity;

		[Tooltip("Preview the effect on entire screen")]
		public bool preview;

		[SerializeField]
		private float size = 5f;

		[SerializeField]
		private int iteration = 4;

		[SerializeField]
		private int maxDepth = 4;

		[SerializeField]
		private int downsample;

		[SerializeField]
		private int lastDownsample;

		[SerializeField]
		private float strength;

		private float lastUpdate;

		private Camera camera;

		private Shader shader;

		private Material material;

		public RenderTexture BlurredScreen { get; private set; }

		public Camera Cam
		{
			get
			{
				return camera ?? (camera = GetComponent<Camera>());
			}
		}

		public float Strength
		{
			get
			{
				return strength = Size * Mathf.Pow(2f, Iteration + Downsample);
			}
			set
			{
				strength = Mathf.Max(0f, value);
				SetAdvancedFieldFromSimple();
			}
		}

		public float Size
		{
			get
			{
				return size;
			}
			set
			{
				size = Mathf.Max(0f, value);
			}
		}

		public int Iteration
		{
			get
			{
				return iteration;
			}
			set
			{
				iteration = Mathf.Max(0, value);
			}
		}

		public int Downsample
		{
			get
			{
				return downsample;
			}
			set
			{
				downsample = Mathf.Max(0, value);
			}
		}

		public int MaxDepth
		{
			get
			{
				return maxDepth;
			}
			set
			{
				maxDepth = Mathf.Max(1, value);
			}
		}

		private float ScreenSize
		{
			get
			{
				return (float)Mathf.Min(Cam.pixelWidth, Cam.pixelHeight) / 1080f;
			}
		}

		private float MinUpdateCycle
		{
			get
			{
				return (!(maxUpdateRate > 0f)) ? float.PositiveInfinity : (1f / maxUpdateRate);
			}
		}

		protected virtual void SetAdvancedFieldFromSimple()
		{
			Size = strength / Mathf.Pow(2f, Iteration + Downsample);
			if (Size < 1f)
			{
				if (Downsample > 0)
				{
					Downsample--;
					Size *= 2f;
				}
				else if (Iteration > 0)
				{
					Iteration--;
					Size *= 2f;
				}
			}
			while (Size > 8f)
			{
				Size /= 2f;
				Iteration++;
			}
		}

		protected virtual void Start()
		{
			camera = Cam;
			shader = Shader.Find("Hidden/EfficientBlur");
			if (!shader.isSupported)
			{
				base.enabled = false;
			}
			material = new Material(shader);
			BlurredScreen = new RenderTexture(Cam.pixelWidth >> Downsample, Cam.pixelHeight >> Downsample, 0)
			{
				filterMode = FilterMode.Bilinear
			};
			lastDownsample = Downsample;
		}

		protected virtual void ProgressiveResampling(RenderTexture source, int level, ref RenderTexture target)
		{
			level = Mathf.Min(level, MaxDepth);
			int width = source.width >> level + Downsample;
			int height = source.height >> level + Downsample;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(target, temporary, material, 0);
			RenderTexture.ReleaseTemporary(target);
			target = temporary;
		}

		protected virtual void ProgressiveBlur(RenderTexture source)
		{
			if (Downsample != lastDownsample)
			{
				BlurredScreen = new RenderTexture(Cam.pixelWidth >> Downsample, Cam.pixelHeight >> Downsample, 0);
				lastDownsample = Downsample;
			}
			if (BlurredScreen.IsCreated())
			{
				BlurredScreen.DiscardContents();
			}
			material.SetFloat("size", Size * ScreenSize);
			int num = ((iteration > 0) ? 1 : Downsample);
			int width = source.width >> num;
			int height = source.height >> num;
			RenderTexture target = RenderTexture.GetTemporary(width, height, 0, source.format);
			target.filterMode = FilterMode.Bilinear;
			source.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, target, material, 0);
			for (int i = 2; i < Iteration + 1; i++)
			{
				ProgressiveResampling(source, i, ref target);
			}
			for (int num2 = Iteration - 1; num2 > 0; num2--)
			{
				ProgressiveResampling(source, num2, ref target);
			}
			Graphics.Blit(target, BlurredScreen, material, 0);
			RenderTexture.ReleaseTemporary(target);
		}

		protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime - lastUpdate >= MinUpdateCycle)
			{
				ProgressiveBlur(source);
				lastUpdate = Time.unscaledTime;
			}
			if (preview)
			{
				Graphics.Blit(BlurredScreen, destination);
			}
			else
			{
				Graphics.Blit(source, destination);
			}
		}
	}
}
