#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WebGLBuild
{
    [MenuItem("Blastproof/WebGLBuildAndRun - Dev")]
    public static void BuildAndRunDevelopmentGame()
    {
        BuildAndRunGame(true);
    }

    [MenuItem("Blastproof/WebGLBuildAndRun - Production")]
    public static void BuildAndRunProductionGame()
    {
        BuildAndRunGame(false);
    }

    public static void BuildAndRunGame(bool Development)
    {
        // Get filename.
        //string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        
        string[] levels = new string[] { "Assets/Blastproof/Scenes/Main.unity" };

        //string path = EditorUtility.
        string buildPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.dataPath)), "Tools\\IntermediaryBuild");
        string frontendPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.dataPath)), "Frontend");
        //UnityEngine.Debug.Log(buildPath);

        //// Build player.
        BuildPipeline.BuildPlayer(levels, buildPath, BuildTarget.WebGL, Development ? BuildOptions.Development : BuildOptions.None);

        File.Copy(Path.Combine(buildPath, "Build\\IntermediaryBuild.data"), Path.Combine(frontendPath, "public\\unity\\JogaJoga.data"), true);
        File.Copy(Path.Combine(buildPath, "Build\\IntermediaryBuild.framework.js"), Path.Combine(frontendPath, "public\\unity\\JogaJoga.framework.js"), true);
        File.Copy(Path.Combine(buildPath, "Build\\IntermediaryBuild.loader.js"), Path.Combine(frontendPath, "public\\unity\\JogaJoga.loader.js"), true);
        File.Copy(Path.Combine(buildPath, "Build\\IntermediaryBuild.wasm"), Path.Combine(frontendPath, "public\\unity\\JogaJoga.wasm"), true);
        //UnityEngine.Debug.Log(PlayerPrefs.GetInt("BuildProcessId"));

        //if(PlayerPrefs.GetInt("BuildProcessId") != 0)
        //{
        //    var process = System.Diagnostics.Process.GetProcesses().FirstOrDefault(pr => pr.Id == PlayerPrefs.GetInt("BuildProcessId"));
        //    if(process != null)
        //    {
        //        process.Kill();
        //    }
        //}

        //Process cmd = System.Diagnostics.Process.Start("CMD.exe", "/C npm start --prefix " + toolPath);
        //PlayerPrefs.SetInt("BuildProcessId", cmd.Id);
    }
}
#endif