#if UNITY_IOS || UNITY_OSX
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Aquiris.SQLite.Editor
{
    public static class SQLitePostBuildProcess
    {
        private const string usrLibSqlite3 = "usr/lib/libsqlite3.tbd";
        private const string frameworkSqlite3 = "Frameworks/libsqlite3.tbd";
        
        [PostProcessBuild(int.MaxValue)]
        private static void PostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            PBXProject project = new PBXProject();
            string pbxProjectFilePath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            project.ReadFromFile(pbxProjectFilePath);

            string targetGuid = project.GetUnityMainTargetGuid();
            
            project.AddFileToBuild(targetGuid, project.AddFile(usrLibSqlite3, frameworkSqlite3, PBXSourceTree.Sdk));
            project.AddFileToBuild(targetGuid, project.AddFile(usrLibSqlite3, frameworkSqlite3, PBXSourceTree.Build));
            
            project.WriteToFile(pbxProjectFilePath);
        }
    }
}
#endif
