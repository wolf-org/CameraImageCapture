using UnityEditor;
using UnityEngine;
using CIC.Core;

[CustomEditor(typeof(CameraImageCapture))]
[CanEditMultipleObjects]
public class CameraImageCaptureEditor : Editor
{
    private CameraImageCapture cic;

    private SerializedProperty camera;
    private SerializedProperty imageRes;
    private SerializedProperty writeType;
    private SerializedProperty imageFormat;

    private bool showFileSetting = true;
    private bool showComponents = true;

    private string foldPathPanel;

    public CameraImageCapture Cic
    {
        get
        {
            if (cic == null)
                cic = (CameraImageCapture)target;
            return cic;
        }
    }

    private void OnEnable()
    {
        Cic.fileInfors = CaptureInforManager.ReadLocalData();
        camera = serializedObject.FindProperty(nameof(Cic.targetCamera));
        imageRes = serializedObject.FindProperty(nameof(Cic.imageResolution));
        writeType = serializedObject.FindProperty(nameof(Cic.writeType));
        imageFormat = serializedObject.FindProperty(nameof(Cic.imageFormat));

        //fileName = serializedObject.FindProperty(nameof(Cic.fileName));
    }

    private void OnDestroy()
    {
        CaptureInforManager.WriteLocalData(Cic.fileInfors);
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        showComponents = EditorGUILayout.BeginFoldoutHeaderGroup(showComponents, "Components");
        if (showComponents)
        {
            EditorGUILayout.PropertyField(camera);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        showFileSetting = EditorGUILayout.BeginFoldoutHeaderGroup(showFileSetting, "Export setting");
        if (showFileSetting)
        {
            EditorGUILayout.LabelField("Export folder");
            if (Cic.saveFolderPath == null || Cic.saveFolderPath.Length == 0) Cic.saveFolderPath = Application.persistentDataPath;
            EditorStyles.textArea.wordWrap = true;
            Cic.saveFolderPath = EditorGUILayout.TextArea(Cic.saveFolderPath, GUILayout.Height(40));
            if (GUILayout.Button("Change folder"))
            {
                foldPathPanel = EditorUtility.OpenFolderPanel("Select export path", Cic.saveFolderPath, "");
                if (foldPathPanel != "") Cic.saveFolderPath = foldPathPanel;
            }
            EditorGUILayout.Space();
            Cic.fileName = EditorGUILayout.TextField("File name", Cic.fileName);
            EditorGUILayout.Space();
            Cic.isImageSerial = EditorGUILayout.Toggle("Image serialized", Cic.isImageSerial);
            Cic.isOverrideFile = EditorGUILayout.Toggle("Override file", Cic.isOverrideFile);
            EditorGUILayout.PropertyField(writeType);
            EditorGUILayout.PropertyField(imageFormat);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(imageRes);

        if (GUILayout.Button("Capture and save"))
        {
            Cic.CaptureAndSaveImage();
        }


        serializedObject.ApplyModifiedProperties();
    }

}