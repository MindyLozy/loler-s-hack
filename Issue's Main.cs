using System;
using UnityEngine;
using MelonLoader;
using Il2Cpp;
using System.Reflection;
using Il2CppInterop.Runtime;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(L0LeRModMenu.Main), "Issue's Hack", "1.05", "MindyLozy")]
[assembly: MelonGame("Omega Mega Gigal Intel", "Granny 2 Enchanted")]

namespace L0LeRModMenu
{
    public class Main : MelonMod
    {
        private bool menuOpen = false;
        private Rect menuRect = new Rect(50, 50, 400, 550);
        private int currentTab = 0;
        private string[] tabNames = { "Baby AI", "Granny AI", "Grandpa AI" };

        // check
        private bool isInGame = false;

        // AI
        private bool babyCallGranny = false;
        private bool babyForceChase = false;
        private bool grannyIsChasing = false;
        private string grannyRunSpeed = "10.0";
        private bool grandpaIsAngry = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            // scenes
            if (sceneName != "Main" && sceneName != "Scene")
            {
                isInGame = true;
                MelonLogger.Msg("Scene Loaded");
            }
            else
            {
                isInGame = false;
                menuOpen = false;
                MelonLogger.Msg("check");
            }
        }

        public override void OnUpdate()
        {
            if (!isInGame) return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.Insert))
            {
                menuOpen = !menuOpen;

                if (menuOpen)
                {
                    Time.timeScale = 0.1f; // slowdown yes
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Time.timeScale = 1f;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            // cursor
            if (menuOpen)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public override void OnGUI()
        {
            if (!menuOpen || !isInGame) return;

            GUI.backgroundColor = new Color(0.05f, 0.05f, 0.05f, 0.95f);
            GUI.color = Color.white;
            
            menuRect = GUI.Window(0, menuRect, (GUI.WindowFunction)DrawMenu, "<color=yellow>Issue's Hack</color>");
        }
        
        private void DrawMenu(int windowID)
        {
            GUI.DragWindow(new Rect(0, 0, menuRect.width, 25));
            currentTab = GUILayout.Toolbar(currentTab, tabNames);
            
            GUILayout.BeginVertical();
            GUILayout.Space(10);

            switch (currentTab)
            {
                case 0: DrawBabyAI(); break;
                case 1: DrawGrannyAI(); break;
                case 2: DrawGrandpaAI(); break;
            }
            
            GUILayout.Space(10);
            if (GUILayout.Button("to close menu : insert")) menuOpen = false;
            GUILayout.EndVertical();
        }

        private void DrawBabyAI()
        {
            GUILayout.Label("<b>Baby AI</b>", GUI.skin.box);
            babyCallGranny = GUILayout.Toggle(babyCallGranny, "Call Granny");
            if (GUILayout.Button("Apply")) SetBabyBool("CalledGranny", babyCallGranny);
        }

        private void DrawGrannyAI()
        {
            GUILayout.Label("<b>Granny AI</b>", GUI.skin.box);
            if (GUILayout.Button("Kill Granny")) SetGrannyBool("IsDying", true);
            
            grannyIsChasing = GUILayout.Toggle(grannyIsChasing, "Force Chasing");
            if (GUILayout.Button("Apply Chasing")) SetGrannyBool("IsChasing", grannyIsChasing);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Speed:");
            grannyRunSpeed = GUILayout.TextField(grannyRunSpeed, 5);
            if (GUILayout.Button("Set")) SetGrannyFloat("Run_Speed", float.Parse(grannyRunSpeed));
            GUILayout.EndHorizontal();
        }

        private void DrawGrandpaAI()
        {
            GUILayout.Label("<b>Grandpa AI</b>", GUI.skin.box);
            grandpaIsAngry = GUILayout.Toggle(grandpaIsAngry, "Make Angry");
            if (GUILayout.Button("Apply")) SetGrandpaBool("IsAngry", grandpaIsAngry);
            if (GUILayout.Button("Kill Grandpa")) SetGrandpaBool("IsDying", true);
        }

        // no softlock plz

        private void SetBabyBool(string field, bool val) {
            var b = UnityEngine.Object.FindObjectOfType<AI_Baby>();
            if (b != null) typeof(AI_Baby).GetField(field, BindingFlags.Public | BindingFlags.Instance)?.SetValue(b, val);
        }

        private void SetGrannyBool(string field, bool val) {
            var g = UnityEngine.Object.FindObjectOfType<AI_Granny>();
            if (g != null) typeof(AI_Granny).GetField(field, BindingFlags.Public | BindingFlags.Instance)?.SetValue(g, val);
        }

        private void SetGrannyFloat(string field, float val) {
            var g = UnityEngine.Object.FindObjectOfType<AI_Granny>();
            if (g != null) typeof(AI_Granny).GetField(field, BindingFlags.Public | BindingFlags.Instance)?.SetValue(g, val);
        }

        private void SetGrandpaBool(string field, bool val) {
            var d = UnityEngine.Object.FindObjectOfType<AI_Grandpa>();
            if (d != null) typeof(AI_Grandpa).GetField(field, BindingFlags.Public | BindingFlags.Instance)?.SetValue(d, val);
        }
    }
}
