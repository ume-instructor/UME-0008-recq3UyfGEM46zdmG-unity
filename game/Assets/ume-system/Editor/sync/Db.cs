using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;


namespace UME{

    public static class Db{

        private static string LoadJson(string path)
        {
            string json = "{}";
            using (System.IO.StreamReader r = new System.IO.StreamReader(path))
            {
                json = r.ReadToEnd();
            }
            return json;
        }
        
        private static string getData(string type, int id){
            string _data = "{}";
            switch(type){
                case "session":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("SESSION_DATA"));
                    break;
                case "lesson":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("LESSON_DATA"));
                    break;
                case "challenge":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("CHALLENGE_DATA"));
                    break;
                case "user":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("USER_DATA"));
                    break;
            }
            return _data;
        }
        private static string getData(string type, string id){
            string _data = "{}";
            switch(type){
                case "session":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("UME_SESSION_TABLE"));
                    break;
                case "lesson":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("UME_LESSON_TABLE"));
                    break;
                case "challenge":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("UME_CHALLENGE_TABLE"));
                    break;
                case "registrant":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("UME_REGISTRANT_TABLE"));
                    break;
                case "instructor":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("UME_INSTRUCTOR_TABLE"));
                    break;
                case "student":
                    _data = LoadJson(System.Environment.GetEnvironmentVariable("UME_STUDENT_DATA"));
                    
                    break;
            }
            //Dictionary<string, string> _table = JsonUtility.FromJson<Dictionary<string, string>>(_data);
            
            return _data;
        }
        //convert to Objects
        public static string getLogin(){
            return System.Environment.GetEnvironmentVariable("USER");
         }
        public static Dictionary<string, string>  getEntity(string type, int id){
            return JsonUtility.FromJson<Dictionary<string, string>>(getData(type,id));
        }
        public static string getCurrentUser(){
            return System.Environment.GetEnvironmentVariable("UME_USER_ID");
        }
        public static string getCurrentSession(){
            return System.Environment.GetEnvironmentVariable("UME_SESSION_ID");
        }
        // public static Student getStudent(string id){
        //     return JsonUtility.FromJson<Student>(getData("student",id));
        // }
        // public static Instructor getInstructor(string id){
        //     return JsonUtility.FromJson<Instructor>(getData("instructor",id));
        // }
        // public static Registrant getUser(string id){
        //     return JsonUtility.FromJson<Registrant>(getData("user",id));
        // }
        // public static Session getSession(string id){
        //     return JsonUtility.FromJson<Session>(getData("session",id));
        // }
        // public static Lesson getLesson(int id){
        //     return JsonUtility.FromJson<Lesson>(getData("lesson",id));
        // }
        // public static Challenge getChallenge(int id){
        //     return JsonUtility.FromJson<Challenge>(getData("challenge",id));
        // }
        //straight to dictionary


    }
}