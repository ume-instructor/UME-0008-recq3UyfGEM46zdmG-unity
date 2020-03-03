using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEditor;
using UnityEngine.Experimental.UIElements;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Linq;
using UnityEditor.Rendering;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;

namespace UME
{
    

[UnityEditor.InitializeOnLoad]
static class DataLogging
{  
    static string ActionTableColumns = "'ActionTable' (UserID, SessionID, LessonID, ChallengeID, Login, Timestamp, UnityMode, EditorTool, ActionType, ActionModalities, InterfaceArea, TargetComponent, ActionParams)";
    static string ActionTableDefinition = "ActionTable (UserID text, SessionID text, LessonID text, ChallengeID text, Login text, Timestamp text, UnityMode text, EditorTool text, ActionType text, ActionModalities text, InterfaceArea text, TargetComponent text, ActionParams text)";
    static string ChallengeTableColumns = "'ChallengeTable' (UserID, SessionID, LessonID, Event, ChallengeID, Login, StartTimestamp, EndTimestamp, Params)";
    static string ChallengeTableDefinition = "ChallengeTable (UserID text, SessionID text, LessonID text, Event string, ChallengeID text, Login text, StartTimestamp text, EndTimestamp text, Params text)";
    
    //metadata

    static string TelemetryDbPath
    {
        get
        {
            string envvar = System.Environment.GetEnvironmentVariable("UME_TELEMETRY_DB");
            if (envvar == null || true){
                envvar = Application.dataPath + "/../../logging/editor/default_unity.db";
                System.Environment.SetEnvironmentVariable("UME_TELEMETRY_DB", envvar);
            }
            return envvar;
        }

        set
        {
            System.Environment.SetEnvironmentVariable("UME_TELEMETRY_DB", value);
        }
    }    
    static string RecordingDbPath
    {
        get
        {
            string envvar = System.Environment.GetEnvironmentVariable("UME_RECORDING_DB");
            if (envvar == null || true){
                envvar =  Application.dataPath + "/../../logging/recording";
                System.Environment.SetEnvironmentVariable("UME_RECORDING_DB", envvar);
            }
            return envvar;
        }

        set
        {
            System.Environment.SetEnvironmentVariable("UME_RECORDING_DB", value);
        }
    }

    //manipulator
    public static string current_tool = "";
    static string last_selected_object = "";
    static string opened_audio_file = "";
    public static int nb_object_in_scene;
    public static int total_nb_components;
    public static long lastscroll = 0;
    public static long lastpan = 0;

    //teacher help
    public static bool isTeacherHelping = false;
    public static Process procaudio = null;

    //object property change
    public static int current_action_group = -1;
    public static string first_action_value_in_group = "";
    public static string last_action_value_in_group = "";
    public static string start_time_group = "";
    public static string property_group = "";
    public static string new_property_group = "";
    public static string target_group = "";
    public static string tool_group = "";
    public static string area_group = "";


    //DB
    static private IDbConnection dbConnection;
    static private IDbCommand dbCommand;
    static private SqliteConnection conn;
    //static private long lastDBwrite;


    public static bool debug = false;//true; //false


    static DataLogging()
    {
        InitDb();
        //OpenDb();
        InitSceneVar();
        
        //Callback for Scene
        UnityEditor.SceneManagement.EditorSceneManager.sceneSaved += OnSceneSaved;
        UnityEditor.SceneManagement.EditorSceneManager.sceneClosed += OnSceneClosed;
        UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnSceneOpened;
        UnityEditor.SceneManagement.EditorSceneManager.newSceneCreated += OnNewScene;

        //Callback for getting any selection on an object
        UnityEditor.Selection.selectionChanged += OnSelectionChanged;

        //Callback to get Keyboard and mouse events
        UnityEditor.SceneView.onSceneGUIDelegate = OnSceneGUI;

        //Callback for selection of any transform tool.
        //UnityEditor.EditorApplication.modifierKeysChanged += TransformToolChanged; //most likely not useful
        EditorApplication.update += onEditorUpdate;

        //Callback for getting a mouse right-click on an Inspector entity
        //UnityEditor.EditorApplication.contextualPropertyMenu += OnInspectorClick;

        //Callback for getting everything 
        UnityEditor.Undo.postprocessModifications += OnPropertyModification;

        //Callback for making changes to hierarchy window
        UnityEditor.EditorApplication.hierarchyWindowChanged += OnHierarchyChange;

        //Callback to get updates on changes on Play mode
        UnityEditor.EditorApplication.playModeStateChanged += onPlayStateChange;

        //Callback to get updates on changes on Play mode
        UnityEditor.EditorApplication.pauseStateChanged += onPauseChange;

        //Callback to get updates on changes on the project
        UnityEditor.EditorApplication.projectWindowChanged += OnProjectChanged;

    }

    //Init Action DB
    static void InitDb(){

        // create db if doesn't exist
        if (!File.Exists(TelemetryDbPath)){
            SqliteConnection.CreateFile(TelemetryDbPath);
            IDbConnection _dbCon = (IDbConnection) new SqliteConnection("URI=file:" + TelemetryDbPath);
            _dbCon.Open();

            //Action table
            IDbCommand _dbCmd = _dbCon.CreateCommand();
            string sql = "create table "+ ActionTableDefinition;
            _dbCmd.CommandText = sql;
            _dbCmd.ExecuteNonQuery();
            _dbCmd.Dispose();
            _dbCmd = null;


            //Challenge table
            _dbCmd = _dbCon.CreateCommand();
            sql = "create table "+ ChallengeTableDefinition;
            _dbCmd.CommandText = sql;
            _dbCmd.ExecuteNonQuery();
            _dbCmd.Dispose();
            _dbCmd = null;

            _dbCon.Close();
            _dbCon = null;
        }
    }

    //Open DB
    static void OpenDb(){
        DataLogging.conn = new SqliteConnection("URI=file:" + TelemetryDbPath);
        DataLogging.conn.Open();
    }

    //Close DB
    static void WriteDb(){
        DataLogging.conn.Close();
        DataLogging.conn.Open();
    }

    //Init scene variables
    static void InitSceneVar(){
        DataLogging.current_tool = TransformToolSelected();
        var allObj = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        DataLogging.nb_object_in_scene =
            allObj.Where(obj => (obj.hideFlags & HideFlags.HideInHierarchy) != HideFlags.HideInHierarchy).Count();

        var totalComps = 0;
        foreach(GameObject ob in allObj){
            if ((ob.hideFlags & HideFlags.HideInHierarchy) != HideFlags.HideInHierarchy){
                totalComps += ob.GetComponents(typeof(Component)).Length;
            }
        }
        DataLogging.total_nb_components = totalComps;
        //DataLogging.lastDBwrite = System.DateTime.Now.Ticks;
    }

    //Returns initial and final values of the current property being modified
    static UnityEditor.UndoPropertyModification[] OnPropertyModification(UnityEditor.UndoPropertyModification[] modifications)
    {
        var m_params = "";

        //if new group of property change, write previous group in DB
        if (DataLogging.current_action_group != Undo.GetCurrentGroup()){
            if (DataLogging.current_action_group != -1){
                if(DataLogging.new_property_group != "" && DataLogging.property_group != ""){
                    m_params = (DataLogging.current_action_group+"--"+DataLogging.new_property_group+"--"+DataLogging.last_action_value_in_group+"--"+DataLogging.property_group+"--"+DataLogging.first_action_value_in_group);
                    UpdateDatabase(ActionType: "ChangeProperty", Modality: (DataLogging.area_group == "Scene" ? "Mouse" : "Keyboard"), Time: DataLogging.start_time_group, Tool: DataLogging.tool_group, Area: DataLogging.area_group, Target: DataLogging.target_group, Params: m_params);
                }
            }
                    
            DataLogging.first_action_value_in_group = "";
            DataLogging.last_action_value_in_group = "";
            DataLogging.property_group = "";
            DataLogging.new_property_group = "";
            DataLogging.current_action_group = Undo.GetCurrentGroup();
            DataLogging.target_group = "";
            DataLogging.start_time_group = "";
            DataLogging.tool_group = "";

            //var transform_values = "";
            foreach (UnityEditor.UndoPropertyModification m in modifications)
            {
                if (m.currentValue.target.ToString().Contains("UnityEngine.Transform") ){
                    if(debug) UnityEngine.Debug.Log(Undo.GetCurrentGroup() + "UNDO transform" + m.currentValue.target + " " + m.currentValue.propertyPath );
         
                    /*if (transform_values == ""){
                        if(UnityEditor.Selection.objects != null){
                            foreach(GameObject ob in UnityEditor.Selection.objects){
                                var comp = ob.GetComponent(typeof(UnityEngine.Transform));
                                transform_values += ob.name+"[P"+comp.transform.localPosition+",R"+comp.transform.localRotation+",S"+comp.transform.localScale+"]";
                            }
                        }
                    } */
                }
                else{
                    if(debug) UnityEngine.Debug.Log("UNDO notransform" + m.currentValue.target + " " + m.currentValue.propertyPath + " " + Undo.GetCurrentGroupName() + " " + Undo.GetCurrentGroup());
                    //UpdateDatabase(ActionType: "ChangeProperty", Modality: "Inspector", Target: getCurrentSelectedObjectNames(), Params: m_params);
                }

                if (DataLogging.start_time_group == "") DataLogging.start_time_group = DataLogging.CurrentTimeStamp(); 
                if(DataLogging.target_group == "") DataLogging.target_group = DataLogging.getCurrentSelectedObjectNames();
                if(DataLogging.tool_group == "") DataLogging.tool_group = DataLogging.TransformToolSelected();
                DataLogging.first_action_value_in_group += "["+(m.previousValue.objectReference ? m.previousValue.objectReference.ToString() : m.previousValue.value.ToString())+"]";
                DataLogging.last_action_value_in_group += "["+(m.currentValue.objectReference ? m.currentValue.objectReference.ToString() : m.currentValue.value.ToString())+"]";
                DataLogging.property_group += "["+m.currentValue.target + "--" + m.currentValue.propertyPath+"]";
                DataLogging.area_group = (UnityEditor.EditorWindow.mouseOverWindow != null ? UnityEditor.EditorWindow.mouseOverWindow.titleContent.text : "");
                DataLogging.new_property_group = property_group;
            }
        }
        //during property change (relevant for transform drag and drop, or writing)
        else{
            DataLogging.last_action_value_in_group = "";
            DataLogging.new_property_group = "";
            foreach (UnityEditor.UndoPropertyModification m in modifications){
                DataLogging.last_action_value_in_group += "["+(m.currentValue.objectReference ? m.currentValue.objectReference.ToString() : m.currentValue.value.ToString())+"]";
                DataLogging.new_property_group += "["+m.currentValue.target + "--" + m.currentValue.propertyPath+"]";
                DataLogging.area_group = (UnityEditor.EditorWindow.mouseOverWindow != null ? UnityEditor.EditorWindow.mouseOverWindow.titleContent.text : "");
            }
        }
        return modifications;
    }

    //Draw visual indicator in teacher help mode
    private static void drawHelpFrame(){
    	foreach(UnityEditor.SceneView sceneView in UnityEditor.SceneView.sceneViews)
		{	
			if(sceneView != null && isTeacherHelping)
			{
                Material mat = new Material(Shader.Find("Sprites/Default"));
                float yoffset = .01f;
                float xoffset = .01f;
                GL.PushMatrix();
                mat.SetPass(0);
                GL.LoadOrtho();
                GL.Begin(GL.LINES);
                GL.Color(Color.red);
                float xMax = sceneView.camera.rect.xMax;
                float xMin = sceneView.camera.rect.xMin;
                float yMax = sceneView.camera.rect.yMax;
                float yMin = sceneView.camera.rect.yMin;
                GL.Vertex(new Vector3(sceneView.camera.rect.xMax-xoffset,sceneView.camera.rect.yMax-yoffset,0));
                GL.Vertex(new Vector3(sceneView.camera.rect.xMin+xoffset,sceneView.camera.rect.yMax-yoffset,0));
                GL.Vertex(new Vector3(sceneView.camera.rect.xMin+xoffset,sceneView.camera.rect.yMax-yoffset,0));
                GL.Vertex(new Vector3(sceneView.camera.rect.xMin+xoffset,sceneView.camera.rect.yMin+yoffset,0));
                GL.Vertex(new Vector3(sceneView.camera.rect.xMin+xoffset,sceneView.camera.rect.yMin+yoffset,0));
                GL.Vertex(new Vector3(sceneView.camera.rect.xMax-xoffset,sceneView.camera.rect.yMin+yoffset,0));
                GL.Vertex(new Vector3(sceneView.camera.rect.xMax-xoffset,sceneView.camera.rect.yMin+yoffset,0));
                GL.Vertex(new Vector3(sceneView.camera.rect.xMax-xoffset,sceneView.camera.rect.yMax-yoffset,0));
                GL.End();
                GL.PopMatrix();
            }
		}
    }

    //track events in the scene and teacher help mode
    static void OnSceneGUI(SceneView sceneview)
    {
        //the following line allows all mouseUp, mouseDown and mouseDrag events to be detected, but blocks object to be selected successfully
        //HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        
        drawHelpFrame();
        var e = Event.current;
        
        if (e.isMouse)
        {
            //Mouse
            Vector2 initPos = e.mousePosition;
            //right click drag
            if (e.type == EventType.MouseDrag && e.button == 1)
            {
                if( (System.DateTime.Now.Ticks - DataLogging.lastpan) > 5000000){
                    if(debug) UnityEngine.Debug.Log("Right Drag performed");
                    UpdateDatabase(ActionType: "Free-Pan", Modality: "Mouse", Params: e.delta.ToString());
                    DataLogging.lastpan = System.DateTime.Now.Ticks;
                }
            }
            
            if(debug){
                if (e.type == EventType.MouseUp)
                {   
                    //Left Mouse Button
                    if (e.button == 0) UnityEngine.Debug.Log("Left mouse click at coordinates: " + initPos.ToString());
                    //Right Mouse Button
                    else if (e.button == 1) UnityEngine.Debug.Log("Right Click");
                }
            }
        }

        else if (e.isScrollWheel)
        {
            //Mouse Scroll wheel 
            if( (System.DateTime.Now.Ticks - DataLogging.lastscroll) > 5000000){
                if(debug) UnityEngine.Debug.Log("Mouse scroll wheel used " + e.delta); 
                UpdateDatabase(ActionType: "Zoom", Modality: "Mouse", Target: DataLogging.getCurrentSelectedObjectNames(), Params: e.delta.y < 0 ? "Zoom-in" : "Zoom-out");
                DataLogging.lastscroll = System.DateTime.Now.Ticks;
            }
        }

        else
        {
            //Keyboard
            if (e != null && e.keyCode != KeyCode.None)
            {
                //Control based special commands
                //Undo
                if (e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.Z)
                {
                    if(debug) UnityEngine.Debug.Log("Ctrl Z performed");
                    UpdateDatabase(ActionType: "Cancel", Modality: "HotKey");
                }

                //Redo
                else if (e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.Y)
                {
                    if(debug) UnityEngine.Debug.Log("Ctrl Y performed");
                    UpdateDatabase(ActionType: "Redo", Modality: "HotKey");
                }

                //Select all
                else if (e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.A)
                {
                    if(debug) UnityEngine.Debug.Log("Ctrl A performed");
                    UpdateDatabase(ActionType: "SelectAll", Modality: "HotKey", Target: DataLogging.getCurrentSelectedObjectNames());
                }

                //Save
                else if (e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.S)
                {
                    if(debug) UnityEngine.Debug.Log("Ctrl S perfomed");
                    UpdateDatabase(ActionType: "Save as", Modality: "HotKey");
                }

                //Duplicate
                else if (e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.D)
                {
                    if(debug) UnityEngine.Debug.Log("Ctrl D perfomed");
                    UpdateDatabase(ActionType: "Duplicate", Modality: "HotKey", Target: DataLogging.getCurrentSelectedObjectNames());
                }

                //Copy
                else if (e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.C)
                {
                    if(debug) UnityEngine.Debug.Log("Ctrl C perfomed");
                    UpdateDatabase(ActionType: "Copy", Modality: "HotKey", Target: DataLogging.getCurrentSelectedObjectNames());
                }

                //Paste
                else if (e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.V)
                {
                    if(debug) UnityEngine.Debug.Log("Ctrl V perfomed");
                    UpdateDatabase(ActionType: "Paste", Modality: "HotKey", Target: DataLogging.getCurrentSelectedObjectNames());
                }

                //Teacher help
                else if (e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.Alpha1)
                {
                    if (isTeacherHelping) //exit teacher help mode
                    {
                        UnityEngine.Debug.Log("");
                        isTeacherHelping = false;
                        UpdateDatabase(ActionType: "TeacherHelp", Modality: "HotKey", Params: "EndHelp "+DataLogging.opened_audio_file);
                        //sceneview.RemoveNotification();
                        DataLogging.opened_audio_file = "";
                        if (DataLogging.procaudio != null && !DataLogging.procaudio.HasExited){
                            DataLogging.procaudio.Kill();
                        }

                        //display help menu
                        GenericMenu mm = new GenericMenu();                    
                        var m_scenename = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                        foreach (KeyValuePair<string, string> item in Context.SkillSet){
                            if(m_scenename == "challenge 1" && item.Value.Contains("Lesson1")){
                                mm.AddItem(new GUIContent(item.Value.Replace("Lesson1/","")), false, TeacherHelpPopUp, item.Key);
                            }
                            else if(m_scenename == "challenge 2" && item.Value.Contains("Lesson2")){
                                mm.AddItem(new GUIContent(item.Value.Replace("Lesson2/","")), false, TeacherHelpPopUp, item.Key);
                            }
                            else if(m_scenename == "challenge 3" && item.Value.Contains("Lesson3")){
                                mm.AddItem(new GUIContent(item.Value.Replace("Lesson3/","")), false, TeacherHelpPopUp, item.Key);
                            }
                        }

                        mm.AddSeparator("");
                        mm.AddSeparator("");
                        mm.AddSeparator("");
                        mm.AddSeparator("");
                        mm.AddSeparator("");

                        foreach (KeyValuePair<string, string> item in Context.SkillSet)
                        {
                            mm.AddItem(new GUIContent(item.Value), false, TeacherHelpPopUp, item.Key);
                        }

                        mm.ShowAsContext();
                    }
                    else{ //enter teacher help mode
                        UnityEngine.Debug.Log("Teacher help mode");
                        isTeacherHelping = true;
                        UpdateDatabase(ActionType: "TeacherHelp", Modality: "Keyboard", Params: "StartHelp");

                        //sceneview.ShowNotification(new GUIContent("Help"));
                        DataLogging.procaudio = new Process();
                        DataLogging.procaudio.StartInfo.FileName = "arecord";
                        DataLogging.procaudio.StartInfo.WorkingDirectory = DataLogging.RecordingDbPath+"/";
                        DataLogging.opened_audio_file = "-d 180 -f cd helpaudiorecord_"+Context.User+"_"+Context.Session+"_"+System.DateTime.Now.Ticks;
                        DataLogging.procaudio.StartInfo.Arguments = DataLogging.opened_audio_file;
                        DataLogging.procaudio.Start();
                    }
                }

                //Everything except control based special commands
                else if (e != null && e.keyCode != KeyCode.None && e.keyCode != KeyCode.LeftControl &&
                       !(e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.Z) &&
                       !(e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.Y) &&
                       !(e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.A) &&
                       !(e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.S) &&
                       !(e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.C) &&
                       !(e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.V) &&
                       !(e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.D) &&
                       !(e.control && e.type == EventType.KeyUp && e.keyCode == KeyCode.Alpha1))
                {
                    //Pan with arrows, maybe make it KeyUp (or maybe even let it be done by the next elseif block for KeyUp events
                    if (e.type == EventType.KeyDown)
                    {
                        if (e.keyCode == KeyCode.Delete)
                        {
                            UpdateDatabase(ActionType: "Delete", Modality: "HotKey", Target:DataLogging.last_selected_object);
                        }
                        else if (e.keyCode == KeyCode.F ){
                            if(debug) UnityEngine.Debug.Log("Focus performed with F");
                            UpdateDatabase(ActionType: "Focus", Modality: "HotKey", Target: getCurrentSelectedObjectNames());
                        }
                        /*else{
                            if(debug) UnityEngine.Debug.Log("Key pressed in editor: " + e.keyCode);
                            UpdateDatabase(ActionType: "keyPressed", Modality: "Keyboard", Target: getCurrentSelectedObjectNames(), Params: e.keyCode.ToString());
                        } */
                    }

                    //Any Key tracking on KeyUp.
                    else if (e.type == EventType.KeyUp)
                    {
                        //if(debug) UnityEngine.Debug.Log("Key Up in editor: " + e.keyCode);
                        if (e.keyCode == KeyCode.UpArrow)
                        {
                           UpdateDatabase(ActionType: "Free-pan", Modality: "Keyboard", Params: "Up");
                        }
                        else if (e.keyCode == KeyCode.DownArrow)
                        {
                            UpdateDatabase(ActionType: "Free-pan", Modality: "Keyboard", Params: "Down");
                        }
                        else if (e.keyCode == KeyCode.LeftArrow)
                        {
                            UpdateDatabase(ActionType: "Free-pan", Modality: "Keyboard", Params: "Left");
                        }
                        else if (e.keyCode == KeyCode.RightArrow)
                        {
                            UpdateDatabase(ActionType: "Free-pan", Modality: "Keyboard", Params: "Right");
                        }
                        else{
                            if(debug) UnityEngine.Debug.Log("Key up in editor: " + e.keyCode);
                            UpdateDatabase(ActionType: "KeyPressed", Modality: "Keyboard", Target: getCurrentSelectedObjectNames(), Params: e.keyCode.ToString());
                        } 
                        
                    }
                }
            }
        }
    }

    //Skill set pop up after help mode (register selected skill in DB)
    static void TeacherHelpPopUp(object obj) {
        if(debug) UnityEngine.Debug.Log(obj);
        UpdateDatabase(ActionType: "TeacherHelpSkill", Modality: "Keyboard", Params: obj.ToString());
    }

    //Used to track when current manipulator changes
    public static void onEditorUpdate(){
        if (Tools.current.ToString() != DataLogging.current_tool){
            if(debug) UnityEngine.Debug.Log("New tool = " + Tools.current.ToString());
            DataLogging.current_tool = Tools.current.ToString();
            UpdateDatabase(ActionType: "SelectTool", Modality: "Mouse", Params: Tools.current.ToString());
        }
    }

    //Callback when scene is saved
    static void OnSceneSaved(UnityEngine.SceneManagement.Scene scene)
    {
		if(debug) UnityEngine.Debug.Log("Callback save call");
        UpdateDatabase(ActionType: "Save scene", Modality: "", Target: scene.name);
        //DataLogging.WriteDb();
        OnPropertyModification(new UnityEditor.UndoPropertyModification[0]);
    }

    //Callback when scene is closed
    static void OnSceneClosed(UnityEngine.SceneManagement.Scene scene)
    {
		if(debug) UnityEngine.Debug.Log("Callback close call");
        UpdateDatabase(ActionType: "Close scene", Modality: "", Target: scene.name);
        //DataLogging.WriteDb();
        OnPropertyModification(new UnityEditor.UndoPropertyModification[0]);
    }

    //Callback when scene is opened
    static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
    {
		if(debug) UnityEngine.Debug.Log("Callback open call");
        UpdateDatabase(ActionType: "Open scene", Modality: "", Target: scene.name);
    }

    //Callback when new scene is created
    static void OnNewScene(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.NewSceneSetup setup, UnityEditor.SceneManagement.NewSceneMode mode)
    {
		if(debug) UnityEngine.Debug.Log("Callback new scene call");
        UpdateDatabase(ActionType: "Create new scene", Modality: "", Target: scene.name);
    }

    //callback when selection changes (either by selecting new objects or unselecting)
    static void OnSelectionChanged()
	{
        if (UnityEditor.Selection.activeGameObject == null)
        {
            UpdateDatabase(ActionType: "Unselect", Modality: "Mouse");
        }
        else
        {
            var objname = getCurrentSelectedObjectNames();
            if(debug) UnityEngine.Debug.Log(UnityEditor.Selection.objects.Length + " objects selected: " + objname);
            UpdateDatabase(ActionType: "Select", Modality: "Mouse", Target: objname, Params: ""+UnityEditor.Selection.objects.Length);
            DataLogging.last_selected_object = objname;
        }      
    }

    //Return currently selected manipulator
    static string TransformToolSelected()
    {
        //if(debug) UnityEngine.Debug.Log("Current selected transformation tool: " + UnityEditor.Tools.current);                             
        return UnityEditor.Tools.current.ToString();
    }


    /*static void OnInspectorClick(GenericMenu menu, SerializedProperty property)
    {
        if(debug) UnityEngine.Debug.Log("Inspector right-click logged at: " + UnityEditor.EditorApplication.contextualPropertyMenu.Target);
        if (property.serializedObject.targetObject.GetType() != typeof (Transform))
        {
            if(debug) UnityEngine.Debug.Log("Transform selected on");
        }
    } */

    //Callback when hierarchy changes, ie, when adding, duplicating, removing objects or moving them around in the hierarchy
    static void OnHierarchyChange()
    {
        var allObj = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        var objectset = "";
        var totalComps = 0;
        var m_NumberVisible = 0;
        
        if(allObj != null){
            m_NumberVisible = allObj.Where(obj => (obj.hideFlags & HideFlags.HideInHierarchy) != HideFlags.HideInHierarchy).Count();

            foreach(GameObject ob in allObj){
                if ((ob.hideFlags & HideFlags.HideInHierarchy) != HideFlags.HideInHierarchy){
                    //get parent
                    var parent = (ob.transform.parent != null ? ob.transform.parent.gameObject.name : "");
                    //get components
                    Component[] components = ob.GetComponents(typeof(Component));
                    var complist = "";

                    if(components != null && components.Length > 0){ //make sure there is at least one component
                        foreach(Component component in components) {
                            complist += component.GetType()+",";
                            totalComps += 1;
                        }
                        complist = complist.Remove(complist.Length - 1);
                    }
                    objectset += "["+ob.name+"{"+parent+"}/{"+complist+"}]";
                }
            }
        }
        
        if (m_NumberVisible > DataLogging.nb_object_in_scene){
            UpdateDatabase(ActionType: "Add or Duplicate", Modality: "", Target: getCurrentSelectedObjectNames(), Params: ""+m_NumberVisible+ "-"+DataLogging.nb_object_in_scene+"-"+objectset);
        }
        else if (m_NumberVisible < DataLogging.nb_object_in_scene){
            UpdateDatabase(ActionType: "Delete", Modality: "", Target: DataLogging.last_selected_object, Params: ""+m_NumberVisible+ "-"+DataLogging.nb_object_in_scene+"-"+objectset);
        }
        else{
            if(DataLogging.total_nb_components < totalComps){
                UpdateDatabase(ActionType: "AddComponent", Modality: "", Target: getCurrentSelectedObjectNames(), Params: ""+m_NumberVisible+"-"+objectset);
            }
            else if(DataLogging.total_nb_components > totalComps){
                UpdateDatabase(ActionType: "RemoveComponent", Modality: "", Target: getCurrentSelectedObjectNames(), Params: ""+m_NumberVisible+"-"+objectset);
            }
            else if(UnityEditor.EditorWindow.mouseOverWindow != null && UnityEditor.EditorWindow.mouseOverWindow.titleContent.text != "Project"){
                UpdateDatabase(ActionType: "ObjectMovedInHierarchy", Modality: "", Target: getCurrentSelectedObjectNames(), Params: ""+m_NumberVisible+"-"+objectset);
            }
        }

        DataLogging.total_nb_components = totalComps;
        DataLogging.nb_object_in_scene = m_NumberVisible;
        if(debug) UnityEngine.Debug.Log("Hierarchy window changed/updated " + m_NumberVisible);
    }

    //Called when changes in the Project window are made
    public static void OnProjectChanged(){
        if(debug) UnityEngine.Debug.Log("Project changed");
        UpdateDatabase(ActionType: "Project window changed", Modality: "Mouse", Target: getCurrentSelectedObjectNames());
    }

    //callback when play mode changes
    static void onPlayStateChange(PlayModeStateChange state)
    {
        if(debug) UnityEngine.Debug.Log("Play mode = " + state);

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            if(debug) UnityEngine.Debug.Log("Unity is in play mode."); 
            UpdateDatabase(ActionType: "Play", Modality: "Mouse");
        }
        else if (state == PlayModeStateChange.EnteredEditMode ){
            if(debug) UnityEngine.Debug.Log("Unity is in stop mode."); 
            UpdateDatabase(ActionType: "Stop", Modality: "Mouse");

            //reload session info
            if (EditorPrefs.HasKey("helpMode")) {
                DataLogging.isTeacherHelping = EditorPrefs.GetBool("helpMode");
                EditorPrefs.DeleteKey("helpMode");
            }
            if(Context.Login == "ume-admin") {
                if (EditorPrefs.HasKey("lesson")) {
                    Context.Lesson = (UME.LessonChoice) EditorPrefs.GetInt("lesson");
                }
                if (EditorPrefs.HasKey("challenge")) {
                    Context.Challenge = (UME.ChallengeChoice) EditorPrefs.GetInt("challenge");
                }
                if (EditorPrefs.HasKey("inChallenge")) {
                    Context.inChallenge = EditorPrefs.GetBool("inChallenge");
                    Context.StartChallengeTime = EditorPrefs.GetString("StartChallengeTime");
                    EditorPrefs.DeleteKey("inChallenge");
                }
                if (EditorPrefs.HasKey("inGroupHelp")) {
                    Context.inGroupHelp = EditorPrefs.GetBool("inGroupHelp");
                    Context.StartGroupHelpTime = EditorPrefs.GetString("StartGroupHelpTime");
                    EditorPrefs.DeleteKey("inGroupHelp");
                }
            }
        }
        else if (state == PlayModeStateChange.ExitingEditMode ){
            //save session info
            EditorPrefs.SetBool("helpMode", DataLogging.isTeacherHelping);
            OnPropertyModification(new UnityEditor.UndoPropertyModification[0]);
            
            if(Context.Login == "ume-admin") {
                EditorPrefs.SetInt("lesson", (int) Context.Lesson );
                EditorPrefs.SetInt("challenge", (int) Context.Challenge );
                if (Context.inChallenge){
                    EditorPrefs.SetBool("inChallenge", Context.inChallenge );
                    EditorPrefs.SetString("StartChallengeTime", Context.StartChallengeTime );
                }

                if (Context.inGroupHelp){
                    EditorPrefs.SetBool("inGroupHelp", Context.inGroupHelp );
                    EditorPrefs.SetString("StartGroupHelpTime", Context.StartGroupHelpTime );
                }
            }
        }
    }

    //Call when pausing / unpausing
    static void onPauseChange(PauseState state){
        if(debug) UnityEngine.Debug.Log("Pause mode = " + state);

        if(state.ToString() == "Paused"){
            UpdateDatabase(ActionType: "Pause", Modality: "Mouse");
        }
        else{
            UpdateDatabase(ActionType: "Unpause", Modality: "Mouse");
        }
    }

    //Return list of currently selected object names
    public static string getCurrentSelectedObjectNames(){
        var objname = "";
        if(UnityEditor.Selection.activeGameObject != null && UnityEditor.Selection.objects != null)
            if (UnityEditor.Selection.objects.Length > 1){
                foreach(GameObject ob in UnityEditor.Selection.objects){
                    objname += ob.name+",";
                }
                objname = objname.Remove(objname.Length - 1);
            }
            else if(UnityEditor.Selection.objects.Length == 1){
                objname = UnityEditor.Selection.activeGameObject.name;
            }

        return objname;
    }

    //Get system timestamp
    public static string CurrentTimeStamp()
    {
        return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
    }

    //Write in DB
    //static void UpdateDatabase(string Mode, string TargetName, string actionModality, string ActionType, string actionparams)
    public static void UpdateDatabase(string ActionType, string Modality, string Time = "", string Mode = "", string Tool = "", string Area = "", string Target = "", string Params = "")
    {
        var conn = new SqliteConnection("URI=file:" + TelemetryDbPath);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "INSERT INTO "+ActionTableColumns+" VALUES (@user_id, @session_id, @lesson_id, @challenge_id, @login, @time, @editormode, @curtool, @actionType , @keyOrMouse, @area, @target, @params)";

        if (Area == ""){
            Area = (UnityEditor.EditorWindow.mouseOverWindow ? UnityEditor.EditorWindow.mouseOverWindow.titleContent.text : ""); 
        }

        if (Mode == ""){
            Mode = (EditorApplication.isPlaying ? "Play" : "Edit");
        }
        if(Tool == ""){
            Tool = TransformToolSelected();
        }

        cmd.Parameters.Add(new SqliteParameter { ParameterName = "user_id", Value = Context.User });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "session_id", Value = Context.Session });        
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "lesson_id", Value = Context.Lesson });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "challenge_id", Value = (Context.inChallenge ? (int)Context.Challenge : -1) });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "login", Value = Context.Login });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "time", Value =  (Time == "" ? DataLogging.CurrentTimeStamp() : Time) });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "editormode", Value = Mode });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "curtool", Value = Tool });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "actionType", Value = ActionType });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "keyOrMouse", Value = Modality });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "area", Value = Area }); //logs interface area of scene, project and hierarchy ONLY when an object in any of them is selected. Doesn't detect mouseclicks in Inspector window.
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "target", Value = Target });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "params", Value = Params });

        cmd.ExecuteNonQuery();
        cmd.Dispose();
        cmd = null;

        /*if( (System.DateTime.Now.Ticks - DataLogging.lastDBwrite > 3000000000) ){
            DataLogging.WriteDb();
        }*/
        //DataLogging.lastDBwrite = System.DateTime.Now.Ticks;
        conn.Close();
    }

    //update challenge
    public static void UpdateChallengeInDatabase(UME.ChallengeEventTypes ev, string Params = "")
    {
        var conn = new SqliteConnection("URI=file:" + TelemetryDbPath);

        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "INSERT INTO "+ChallengeTableColumns+" VALUES (@user_id, @session_id, @lesson_id, @event, @challenge_id, @login, @start_time, @end_time, @params)";

        var evname = "";
        var starttime = "";
        var endtime = "";
        switch (ev){
            case ChallengeEventTypes.Challenge:
                evname = "Challenge";
                starttime = Context.StartChallengeTime;
                endtime = Context.EndChallengeTime;
                break;
            case ChallengeEventTypes.GroupHelp:
                evname = "GroupHelp";
                starttime = Context.StartGroupHelpTime;
                endtime = Context.EndGroupHelpTime;
                break;
            case ChallengeEventTypes.Lesson:
                evname = "Lesson";
                starttime = DataLogging.CurrentTimeStamp();
                break;
        }

        cmd.Parameters.Add(new SqliteParameter { ParameterName = "user_id", Value = Context.User });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "session_id", Value = Context.Session });        
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "lesson_id", Value = Context.Lesson });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "event", Value = evname });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "challenge_id", Value = (int)Context.Challenge });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "login", Value = Context.Login });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "start_time", Value =  starttime });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "end_time", Value = endtime });
        cmd.Parameters.Add(new SqliteParameter { ParameterName = "params", Value = Params });

        cmd.ExecuteNonQuery();
        cmd.Dispose();
        cmd = null;
        conn.Close();
    }
    
}
}
