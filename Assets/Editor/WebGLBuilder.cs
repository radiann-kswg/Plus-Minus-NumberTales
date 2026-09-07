using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// unityroom 向け WebGL ビルド。Tools > PluMi > Build WebGL（出力: Builds/WebGL、.gitignore 済み）。
/// 圧縮は Gzip・Decompression Fallback オフ（unityroom の要件）。ProjectSettings 側の値をそのまま使う。
public static class WebGLBuilder
{
    public const string OutputDir = "Builds/WebGL";

    [MenuItem("Tools/PluMi/Build WebGL")]
    public static void Build()
    {
        var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = OutputDir,
            target = BuildTarget.WebGL,
            options = BuildOptions.None,
        });
        var s = report.summary;
        Debug.Log($"[WebGLBuilder] {s.result} size={s.totalSize / 1024 / 1024}MB errors={s.totalErrors} warnings={s.totalWarnings} time={s.totalTime}");
        if (s.result == BuildResult.Succeeded)
            foreach (var f in Directory.GetFiles(Path.Combine(OutputDir, "Build")))
                Debug.Log($"[WebGLBuilder] {Path.GetFileName(f)} {new FileInfo(f).Length / 1024}KB");
    }
}
