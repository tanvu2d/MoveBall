
using UnityEngine.SceneManagement;


public enum SceneName
{
    None,
    LoadScene,
    MainScene
}

public class SceneMN 
{
    public static void LoadScene(SceneName scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static string GetScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}
