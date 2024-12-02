using UnityEngine;

public class SceneSave : MonoBehaviour
{
    public bool AutoSave = true;

    public float SaveInterval = 3f;

    public bool ClearEveryplay;

    private ItemManager itemManager;

    private float timeTemp;

    private void Start()
    {
        itemManager = (ItemManager)Object.FindObjectOfType(typeof(ItemManager));
        timeTemp = Time.time;
    }

    private void Update()
    {
        if (AutoSave && Time.time > timeTemp + SaveInterval)
        {
            SaveObjectPlacing();
            timeTemp = Time.time;
        }
    }

    public void LevelLoaded()
    {
        if (!ClearEveryplay)
        {
            LoadObjectPlacing();
        }
    }

    public void SaveObjectPlacing()
    {
        ObjectPlacing[] array = (ObjectPlacing[])Object.FindObjectsOfType(typeof(ObjectPlacing));
        string loadedLevelName = Application.loadedLevelName;
        string text = string.Empty;
        string text2 = string.Empty;
        string text3 = string.Empty;
        string text4 = string.Empty;

        for (int i = 0; i < array.Length; i++)
        {
            text += array[i].ItemID + ",";
            text2 += array[i].ItemUID + ",";
            text3 += array[i].transform.position.x + "," + array[i].transform.position.y + "," + array[i].transform.position.z + "|";
            text4 += array[i].transform.rotation.x + "," + array[i].transform.rotation.y + "," + array[i].transform.rotation.z + "," + array[i].transform.rotation.w + "|";
        }

        PlayerPrefs.SetString(loadedLevelName + "OBJID", text);
        PlayerPrefs.SetString(loadedLevelName + "OBJUID", text2);
        PlayerPrefs.SetString(loadedLevelName + "OBJPOS", text3);
        PlayerPrefs.SetString(loadedLevelName + "OBJROT", text4);
    }

    public void LoadObjectPlacing()
    {
        string loadedLevelName = Application.loadedLevelName;
        if (!itemManager)
        {
            return;
        }

        string objIdString = PlayerPrefs.GetString(loadedLevelName + "OBJID");
        string objUidString = PlayerPrefs.GetString(loadedLevelName + "OBJUID");
        string objPosString = PlayerPrefs.GetString(loadedLevelName + "OBJPOS");
        string objRotString = PlayerPrefs.GetString(loadedLevelName + "OBJROT");

        string[] array = objIdString.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] array2 = objUidString.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] array3 = objPosString.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] array4 = objRotString.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);

        Vector3[] positions = new Vector3[array.Length];
        Quaternion[] rotations = new Quaternion[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != string.Empty)
            {
                string[] positionParts = array3[i].Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (positionParts.Length > 2)
                {
                    Vector3 position = Vector3.zero;
                    float.TryParse(positionParts[0], out position.x);
                    float.TryParse(positionParts[1], out position.y);
                    float.TryParse(positionParts[2], out position.z);
                    positions[i] = position;
                }

                string[] rotationParts = array4[i].Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (rotationParts.Length > 3)
                {
                    Quaternion rotation = Quaternion.identity;
                    float.TryParse(rotationParts[0], out rotation.x);
                    float.TryParse(rotationParts[1], out rotation.y);
                    float.TryParse(rotationParts[2], out rotation.z);
                    float.TryParse(rotationParts[3], out rotation.w);
                    rotations[i] = rotation;
                }

                string itemuid = (i < array2.Length && array2.Length > 0) ? array2[i] : string.Empty;
                itemManager.DirectPlacingObject(array[i], itemuid, positions[i], rotations[i]);
            }
        }
    }

    public void ClearObjectPlacing()
    {
        string loadedLevelName = Application.loadedLevelName;
        PlayerPrefs.SetString(loadedLevelName + "OBJID", string.Empty);
        PlayerPrefs.SetString(loadedLevelName + "OBJPOS", string.Empty);
        PlayerPrefs.SetString(loadedLevelName + "OBJ ROT", string.Empty);
    }
}