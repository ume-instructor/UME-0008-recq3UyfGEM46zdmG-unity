using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UME
{   

    //[InitializeOnLoad]
    class ContextWindow : EditorWindow {

        GUIStyle headerStyle;
        GUIStyle errorStyle;
        GUIStyle okStyle;

        void OnEnable() {
            this.headerStyle = new GUIStyle();
            this.headerStyle.wordWrap = true;
            this.headerStyle.fontStyle = FontStyle.Italic;

            this.errorStyle = new GUIStyle();
            this.errorStyle.wordWrap = true;
            this.errorStyle.normal.textColor = Color.red;

            this.okStyle = new GUIStyle();
            this.okStyle.wordWrap = true;
            this.okStyle.normal.textColor = Color.blue;

            if(Context.Login == "ume-admin" && !EditorApplication.isCompiling && !EditorApplication.isPlayingOrWillChangePlaymode) {
                ShowWindow();
            }
        }
        // static ContextWindow(){
        //     if(Context.Login == "ume-admin")
        //         ShowWindow();
        // }

        [MenuItem("UME/Context")]
        public static void  ShowWindow () {
            if(Context.Login == "ume-admin" && !EditorApplication.isCompiling && !EditorApplication.isPlayingOrWillChangePlaymode) {
                EditorWindow win = EditorWindow.GetWindow(typeof(ContextWindow));
                win.titleContent.text = "Context";
            }
        }

        void OnGUI () {
            GUILayout.Space(10);
            GUILayout.Label("Select the lesson when opening Unity:", headerStyle);
            var newlesson = (LessonChoice)EditorGUILayout.EnumPopup("Lesson", Context.Lesson);
            
            //update DB if new lesson
            if((int) newlesson > 0 && Context.Lesson != newlesson){
                Context.Lesson = newlesson;
                DataLogging.UpdateChallengeInDatabase(ChallengeEventTypes.Lesson, newlesson.ToString());
            }

            if ((int) Context.Lesson < 1) GUILayout.Label("Please select a lesson", errorStyle);
            GUILayout.Space(20);

            GUILayout.Label("Select the challenge and click the appropriate button at the beginning and end:", headerStyle);
            UME.Context.Challenge = (ChallengeChoice)EditorGUILayout.EnumPopup("Challenge", Context.Challenge);

            // during challenge, add a button to stop the challenge and add buttons to start/stop group help
            if(Context.inChallenge){
                GUILayout.Label("Challenge Status: Started!", okStyle);
                
                //button to end challenge
                if( GUILayout.Button("End of challenge")){
                    Context.inChallenge = false;
                    Context.EndChallengeTime = DataLogging.CurrentTimeStamp();
                    DataLogging.UpdateChallengeInDatabase(ChallengeEventTypes.Challenge);

                    if(Context.inGroupHelp){
                        Context.inGroupHelp = false;
                        Context.EndGroupHelpTime = Context.EndChallengeTime;
                        GenericMenu mm = new GenericMenu();
                        foreach (KeyValuePair<string, string> item in Context.SkillSet)
                        {
                            mm.AddItem(new GUIContent(item.Value), false, GroupHelpPopUp, item.Key);
                        }
                        mm.ShowAsContext();
                    }
                }

                //Group help during challenges
                GUILayout.Space(10);
                GUILayout.Label("Group help mode during challenge", headerStyle);
                if(Context.inGroupHelp){
                    GUILayout.Label("In Help Mode!", okStyle);
                    if( GUILayout.Button("End group help")){
                        Context.inGroupHelp = false;
                        Context.EndGroupHelpTime = DataLogging.CurrentTimeStamp();

                        //pop up to select target skill
                        GenericMenu mm = new GenericMenu();
                        foreach (KeyValuePair<string, string> item in Context.SkillSet)
                        {
                            mm.AddItem(new GUIContent(item.Value), false, GroupHelpPopUp, item.Key);
                        }
                        mm.ShowAsContext();
                    }
                }
                else{
                    if( GUILayout.Button("Start group help")){
                        Context.inGroupHelp = true;
                        Context.StartGroupHelpTime = DataLogging.CurrentTimeStamp();
                    }
                }
            }
            //not in challenge, just add a button to start a challenge
            else{
                GUILayout.Label("Challenge Status: Stopped / Not started ", errorStyle);
                if (GUILayout.Button("Start challenge")){
                    if ((int) Context.Lesson > 0 && (int) Context.Challenge > 0){
                        Context.inChallenge = true;
                        Context.StartChallengeTime = DataLogging.CurrentTimeStamp();
                        //ComSocket.Broadcast("{\"target\":\"unity\",\"cmd\":\"start_challenge\",\"params\":[]}");
                    }
                }
            }
        }


        //Skill set pop up after help mode (register selected skill in DB)
        static void GroupHelpPopUp(object obj) {
            DataLogging.UpdateChallengeInDatabase(ChallengeEventTypes.GroupHelp, obj.ToString());
        }
    }
}