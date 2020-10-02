using UnityEngine;
using UnityEditor;

namespace EditorFrameword
{
  using UnityEngine;
  using UnityEditor;

  public class ModulePlatformEditor : EditorWindow
  {

    [MenuItem("EditorFrameword/ModulePlatformEditor")]
    public static void Open()
    {
      var window = GetWindow<ModulePlatformEditor>();

      window.position = new Rect(
        Screen.width / 2,
        Screen.height * 2 / 3,
        600,
        500
      );

      window.Show();
    }
  }
}