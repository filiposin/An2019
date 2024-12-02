using UnityEngine;

[DefaultExecutionOrder(-200)]
public class DungeonManager : MonoBehaviour
{
	public int m_Width = 10;

	public int m_Height = 10;

	public float m_Spacing = 4f;

	public GameObject[] m_Tiles = new GameObject[16];

	private void Awake()
	{
		Random.InitState(23431);
		int[] array = new int[m_Width * m_Height];
		for (int i = 0; i < m_Height; i++)
		{
			for (int j = 0; j < m_Width; j++)
			{
				bool flag = false;
				bool flag2 = false;
				if (j > 0)
				{
					flag = (array[j - 1 + i * m_Width] & 1) != 0;
				}
				if (i > 0)
				{
					flag2 = (array[j + (i - 1) * m_Width] & 2) != 0;
				}
				int num = 0;
				if (flag)
				{
					num |= 4;
				}
				if (flag2)
				{
					num |= 8;
				}
				if (j + 1 < m_Width && Random.value > 0.5f)
				{
					num |= 1;
				}
				if (i + 1 < m_Height && Random.value > 0.5f)
				{
					num |= 2;
				}
				array[j + i * m_Width] = num;
			}
		}
		for (int k = 0; k < m_Height; k++)
		{
			for (int l = 0; l < m_Width; l++)
			{
				Vector3 position = new Vector3((float)l * m_Spacing, 0f, (float)k * m_Spacing);
				if (m_Tiles[array[l + k * m_Width]] != null)
				{
					Object.Instantiate(m_Tiles[array[l + k * m_Width]], position, Quaternion.identity);
				}
			}
		}
	}
}
