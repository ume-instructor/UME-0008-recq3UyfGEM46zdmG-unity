using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UME{
    public enum LessonChoice{
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11,
        Twelve = 12,
        Thirteen = 13,
        Fourteen = 14
    }
    public enum ChallengeChoice{
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }

    public enum ChallengeEventTypes{
        Challenge,
        GroupHelp,
        Lesson
    }

    [InitializeOnLoad]
    public static class Context{
        public static string Login {get; set;}
        public static LessonChoice Lesson {get; set;}
        public static ChallengeChoice Challenge {get; set;}
        public static string Session {get; set;}
        public static string User {get; set;}
        public static string StartChallengeTime {get; set;}
        public static string EndChallengeTime {get; set;}
        public static bool inChallenge {get; set;}
        public static bool inGroupHelp {get; set;}
        public static string StartGroupHelpTime {get; set;}
        public static string EndGroupHelpTime {get; set;}

        public static Dictionary<string, string> SkillSet = new Dictionary<string, string>()
        {
            {"ToolMouse", "Lesson1/Manipulator"},
            {"StartStopGame", "Lesson1/Start and Stop game"},
            {"SelectScene", "Lesson1/Object Selection"},
            {"Translate", "Lesson1/Object Translation"},
            {"Rotate", "Lesson1/Object Rotation"},
            {"Scale", "Lesson1/Object Scale"},
            {"AddObject", "Lesson1/Object Addition"}, 
            {"Delete", "Lesson1/Object Deletion"},
            {"Duplicate", "Lesson1/Object Duplication"},
            {"Undo", "Lesson1/Undo"},
            {"Save", "Lesson1/Save File"},

            {"Rigidbody2D", "Lesson2/Rigidbody2D"},
            {"Collider2D", "Lesson2/Collider2D"},
            {"HingeJoint2D", "Lesson2/HingeJoint2D"},
            {"Physical_bouncy", "Lesson2/Physical Materials (bouncy)"},
            {"SpriteColor", "Lesson2/Sprite (color)"},

            {"AudioTrigger", "Lesson3/Audio Trigger"},
            {"Audioclips", "Lesson3/Audio clips"},
            {"LibraryTab", "Lesson3/Library Tab (saving prefabs)"},
            {"MoveHierarchy", "Lesson3/Move in hierarchy (add child)"},

            {"TriggerTag", "Lesson4/Trigger Tag"},
            {"AddComponents", "Lesson4/Add components"},
            {"Physics2D", "Lesson4/Physics 2D"},

            {"Spawner", "Lesson5/Spawner"},
            {"LifeSpan", "Lesson5/Life Span"},
            {"SpawnDelay", "Lesson5/Spawn delay"},
            {"Vertical", "Lesson5/Vertical"},
            {"Oscillation", "Lesson5/Oscillation"},

            {"Player", "Lesson6/Player-Character"},
            {"Keyforce", "Lesson6/Key Force"},

            {"CognitiveLvl", "Cognitive/Cognitive level (all)"}
        };


        static Context(){
            Login = Db.getLogin();
            User = Db.getCurrentUser();
            Session = Db.getCurrentSession();
            inChallenge = false;
            inGroupHelp = false;
        }
    }

}