//------------------------------
// Author: yangchengchao
// Data:   2020
//------------------------------

using UnityEditor;
using UnityEngine;

namespace MMYang
{
	public partial class ReaponProject : MonoBehaviour
	{
        [MenuItem("[杨成超]/示例/重启unity", false, 2)] 
        static void Reapon()
        {
            EditorApplication.OpenProject(Application.dataPath.Replace("Assets", string.Empty));
        }
    }
}

