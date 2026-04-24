using System;
using UnityEngine;
using MelonLoader;
using Il2Cpp;
using System.Reflection;
using Il2CppInterop.Runtime;

[assembly: MelonInfo(typeof(L0LeRModMenu.Main), "L0LeR's ModMenu", "1.0", "L0LeR")]
[assembly: MelonGame("Mod by L0LeR", "Granny Chapter Two Enchanted")]

namespace L0LeRModMenu
{
    public class Main : MelonMod
    {
        private bool menuOpen = false;
        private Rect menuRect = new Rect(20, 20, 600, 450);
        private int currentTab = 0;
        private string[] tabNames = { "Baby_AI", "Granny_AI", "Grandpa_AI" };

        private bool babyCallGranny = false;
        private bool babyForceChase = false;

        private bool grannyAtDishes = false;
        private bool grannyAtPiano = false;
        private string grannyDoorDistance = "5.0";
        private string grannyKillDistance = "2.0";
        private string grannyEyesColor = "#FF0000";
        private bool grannyPlayerSpotted = false;
        private string grannyViewRange = "30.0";
        private bool grannyIsAngry = false;
        private bool grannyIsChasing = false;
        private bool grannyIsDying = false;
        private bool grannyIsSearching = false;
        private bool grannyPlacingTrap = false;
        private string grannyRunAnim = "1.0";
        private string grannyRunSpeed = "10.0";
        private string grannyWalkAnim = "1.0";
        private string grannyWalkSpeed = "5.0";

        private bool grandpaAtCams = false;
        private bool grandpaAtRadio = false;
        private string grandpaDistanceJumpscare = "3.0";
        private string grandpaDoorDistance = "4.0";
        private bool grandpaInSecurityRoom = false;
        private bool grandpaIsAngry = false;
        private bool grandpaIsChasing = false;
        private bool grandpaIsDying = false;
        private bool grandpaPlacingTrap = false;
        private string grandpaRunAnim = "1.0";
        private string grandpaRunSpeed = "12.0";
        private bool grandpaTrapPregnant = false;
        private string grandpaWalkAnim = "1.0";
        private string grandpaWalkSpeed = "6.0";

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
                menuOpen = !menuOpen;
        }

public override void OnGUI()
        {
            if (!menuOpen) return;

            GUI.color = Color.white;
            GUI.backgroundColor = Color.black;
            
            // fix
            windowRect = GUI.Window(0, windowRect, (GUI.WindowFunction)DrawMenu, "<color=white>Issue's Hack</color>");
        }
        
        public override void OnGUI()
{
    if (!menuOpen) return;

    GUI.color = Color.white;
    GUI.backgroundColor = Color.black;
    
    // Используем menuRect, если windowRect выдает ошибку
    menuRect = GUI.Window(0, menuRect, (GUI.WindowFunction)DrawMenu, "<color=white>Issue's Hack</color>");
}
            switch (currentTab)
            {
                // next
                case 0: DrawBabyAI(); break;
                case 1: DrawGrannyAI(); break;
                case 2: DrawGrandpaAI(); break;
            }
            GUILayout.EndVertical();
        }

        private void DrawBabyAI()
        {
            GUILayout.Label("AI_Baby Controls", GUI.skin.box);

            babyCallGranny = GUILayout.Toggle(babyCallGranny, "Baby Call Granny");
            if (GUILayout.Button("Apply Baby Call Granny"))
            {
                var baby = GameObject.FindObjectOfType <AI_Baby>();
                if (baby != null)
                {
                    var field = typeof(AI_Baby).GetField("CalledGranny", BindingFlags.Public | BindingFlags.Instance);
                    if (field != null) field.SetValue(baby, babyCallGranny);
                }
            }

            babyForceChase = GUILayout.Toggle(babyForceChase, "Force Baby Chase Player");
            if (GUILayout.Button("Apply Force Chase"))
            {
                var baby = GameObject.FindObjectOfType <AI_Baby>();
                if (baby != null)
                {
                    var field = typeof(AI_Baby).GetField("Chasing", BindingFlags.Public | BindingFlags.Instance);
                    if (field != null) field.SetValue(baby, babyForceChase);
                    if (babyForceChase)
                    {
                        var player = GameObject.FindWithTag("Player")?.transform;
                        if (player != null) baby.transform.LookAt(player);
                    }
                }
            }
        }

        private void DrawGrannyAI()
        {
            GUILayout.Label("AI_Granny / Eyes_Granny Controls", GUI.skin.box);

            grannyAtDishes = GUILayout.Toggle(grannyAtDishes, "Force Granny Be At Dishes");
            if (GUILayout.Button("Apply At Dishes")) SetGrannyBool("AtDishes", grannyAtDishes);

            grannyAtPiano = GUILayout.Toggle(grannyAtPiano, "Force Granny Be At Piano");
            if (GUILayout.Button("Apply At Piano")) SetGrannyBool("AtPiano", grannyAtPiano);

            GUILayout.BeginHorizontal();
            GUILayout.Label("DoorDistance:");
            grannyDoorDistance = GUILayout.TextField(grannyDoorDistance, 10);
            if (GUILayout.Button("Apply DoorDist")) SetGrannyFloat("DoorDistance", float.Parse(grannyDoorDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("KillDistance:");
            grannyKillDistance = GUILayout.TextField(grannyKillDistance, 10);
            if (GUILayout.Button("Apply KillDist")) SetGrannyFloat("DistanceJumpscare", float.Parse(grannyKillDistance));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Eyes Color:");
            grannyEyesColor = GUILayout.TextField(grannyEyesColor, 10);
            if (GUILayout.Button("Apply Color"))
            {
                var eyes = GameObject.FindObjectOfType <Eyes_Granny>();
                if (eyes != null)
                {
                    Color color;
                    if (ColorUtility.TryParseHtmlString(grannyEyesColor, out color))
                    {
                        var prop = typeof(Eyes_Granny).GetProperty("color", BindingFlags.Public | BindingFlags.Instance);
                        if (prop != null) prop.SetValue(eyes, color);
                    }
                }
            }
            GUILayout.EndHorizontal();

            grannyPlayerSpotted = GUILayout.Toggle(grannyPlayerSpotted, "Force Granny Chasing Player");
            if (GUILayout.Button("Apply PlayerSpotted")) SetEyesGrannyBool("PlayerSpotted", grannyPlayerSpotted);

            GUILayout.BeginHorizontal();
            GUILayout.Label("ViewRange:");
            grannyViewRange = GUILayout.TextField(grannyViewRange, 10);
            if (GUILayout.Button("Apply ViewRange")) SetEyesGrannyFloat("ViewRange", float.Parse(grannyViewRange));
            GUILayout.EndHorizontal();

            grannyIsAngry = GUILayout.Toggle(grannyIsAngry, "Force Granny Be Angry");
            if (GUILayout.Button("Apply IsAngry")) SetGrannyBool("IsAngry", grannyIsAngry);

            grannyIsChasing = GUILayout.Toggle(grannyIsChasing, "Force Granny Chasing Player");
            if (GUILayout.Button("Apply IsChasing")) SetGrannyBool("IsChasing", grannyIsChasing);

            grannyIsDying = GUILayout.Toggle(grannyIsDying, "Kill Granny");
            if (GUILayout.Button("Apply Kill")) SetGrannyBool("IsDying", grannyIsDying);

            grannyIsSearching = GUILayout.Toggle(grannyIsSearching, "Force Granny Searching");
            if (GUILayout.Button("Apply IsSearching")) SetGrannyBool("IsSearching", grannyIsSearching);

            grannyPlacingTrap = GUILayout.Toggle(grannyPlacingTrap, "Force Granny Drop BearTrap");
            if (GUILayout.Button("Apply PlacingTrap")) SetGrannyBool("PlacingTrap", grannyPlacingTrap);

            GUILayout.BeginHorizontal();
            GUILayout.Label("RunAnim:");
            grannyRunAnim = GUILayout.TextField(grannyRunAnim, 10);
            if (GUILayout.Button("Apply RunAnim")) SetGrannyFloat("Run_Anim", float.Parse(grannyRunAnim));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("RunSpeed:");
            grannyRunSpeed = GUILayout.TextField(grannyRunSpeed, 10);
            if (GUILayout.Button("Apply RunSpeed")) SetGrannyFloat("Run_Speed", float.Parse(grannyRunSpeed));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("WalkAnim:");
            grannyWalkAnim = GUILayout.TextField(grannyWalkAnim, 10);
            if (GUILayout.Button("Apply WalkAnim")) SetGrannyFloat("Walk_Anim", float.Parse(grannyWalkAnim));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("WalkSpeed:");
            grannyWalkSpeed = GUILayout.TextField(grannyWalkSpeed, 10);
            if (GUILayout.Button("Apply WalkSpeed")) SetGrannyFloat("Walk_Speed", float.Parse(grannyWalkSpeed));
            GUILayout.EndHorizontal();
        }

        private void DrawGrandpaAI()
        {
            GUILayout.Label("AI_Grandpa Controls", GUI.skin.box);

            grandpaAtCams = GUILayout.Toggle(grandpaAtCams, "Force Ded Be in Cams");
            if (GUILayout.Button("Apply AtCams")) SetGrandpaBool("AtCams", grandpaAtCams);

            grandpaAtRadio = GUILayout.Toggle(grandpaAtRadio, "Force Ded Dance in Radio");
            if (GUILayout.Button("Apply AtRadio")) SetGrandpaBool("AtRadio", grandpaAtRadio);

            GUILayout.BeginHorizontal();
            GUILayout.Label("DistanceJumpscare:");
            grandpaDistanceJumpscare = GUILayout.TextField(grandpaDistanceJumpscare, 10);
            if (GUILayout.Button("Apply JumpscareDist")) SetGrandpaFloat("DistanceJumpscare", float.Parse(grandpaDistanceJumpscare));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("DoorDistance:");
            grandpaDoorDistance = GUILayout.TextField(grandpaDoorDistance, 10);
            if (GUILayout.Button("Apply DoorDist")) SetGrandpaFloat("DoorDistance", float.Parse(grandpaDoorDistance));
            GUILayout.EndHorizontal();

            grandpaInSecurityRoom = GUILayout.Toggle(grandpaInSecurityRoom, "Force Ded be in security");
            if (GUILayout.Button("Apply InSecurityRoom")) SetGrandpaBool("InSecurityRoom", grandpaInSecurityRoom);

            grandpaIsAngry = GUILayout.Toggle(grandpaIsAngry, "Force Ded be Angry at Player");
            if (GUILayout.Button("Apply IsAngry")) SetGrandpaBool("IsAngry", grandpaIsAngry);

            grandpaIsChasing = GUILayout.Toggle(grandpaIsChasing, "Force Ded Chase Player");
            if (GUILayout.Button("Apply IsChasing")) SetGrandpaBool("IsChasing", grandpaIsChasing);

            grandpaIsDying = GUILayout.Toggle(grandpaIsDying, "Kill Ded");
            if (GUILayout.Button("Apply Kill Ded")) SetGrandpaBool("IsDying", grandpaIsDying);

            grandpaPlacingTrap = GUILayout.Toggle(grandpaPlacingTrap, "Force Ded Drop Trap");
            if (GUILayout.Button("Apply PlacingTrap")) SetGrandpaBool("PlacingTrap", grandpaPlacingTrap);

            GUILayout.BeginHorizontal();
            GUILayout.Label("RunAnim:");
            grandpaRunAnim = GUILayout.TextField(grandpaRunAnim, 10);
            if (GUILayout.Button("Apply RunAnim")) SetGrandpaFloat("Run_Anim", float.Parse(grandpaRunAnim));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("RunSpeed:");
            grandpaRunSpeed = GUILayout.TextField(grandpaRunSpeed, 10);
            if (GUILayout.Button("Apply RunSpeed")) SetGrandpaFloat("Run_Speed", float.Parse(grandpaRunSpeed));
            GUILayout.EndHorizontal();

            grandpaTrapPregnant = GUILayout.Toggle(grandpaTrapPregnant, "Force Ded Dropping Trap's");
            if (GUILayout.Button("Apply TrapPregnant")) SetGrandpaBool("TrapPregnant", grandpaTrapPregnant);

            GUILayout.BeginHorizontal();
            GUILayout.Label("WalkAnim:");
            grandpaWalkAnim = GUILayout.TextField(grandpaWalkAnim, 10);
            if (GUILayout.Button("Apply WalkAnim")) SetGrandpaFloat("Walk_Anim", float.Parse(grandpaWalkAnim));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("WalkSpeed:");
            grandpaWalkSpeed = GUILayout.TextField(grandpaWalkSpeed, 10);
            if (GUILayout.Button("Apply WalkSpeed")) SetGrandpaFloat("Walk_Speed", float.Parse(grandpaWalkSpeed));
            GUILayout.EndHorizontal();
        }

        private void SetGrannyBool(string fieldName, bool value)
        {
            var granny = GameObject.FindObjectOfType <AI_Granny>();
            if (granny == null) return;
            var field = typeof(AI_Granny).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null) field.SetValue(granny, value);
        }

        private void SetGrannyFloat(string fieldName, float value)
        {
            var granny = GameObject.FindObjectOfType <AI_Granny>();
            if (granny == null) return;
            var field = typeof(AI_Granny).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null) field.SetValue(granny, value);
        }

        private void SetEyesGrannyBool(string fieldName, bool value)
        {
            var eyes = GameObject.FindObjectOfType <Eyes_Granny>();
            if (eyes == null) return;
            var field = typeof(Eyes_Granny).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null) field.SetValue(eyes, value);
        }

        private void SetEyesGrannyFloat(string fieldName, float value)
        {
            var eyes = GameObject.FindObjectOfType <Eyes_Granny>();
            if (eyes == null) return;
            var field = typeof(Eyes_Granny).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null) field.SetValue(eyes, value);
        }

        private void SetGrandpaBool(string fieldName, bool value)
        {
            var grandpa = GameObject.FindObjectOfType <AI_Grandpa>();
            if (grandpa == null) return;
            var field = typeof(AI_Grandpa).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null) field.SetValue(grandpa, value);
        }

        private void SetGrandpaFloat(string fieldName, float value)
        {
            var grandpa = GameObject.FindObjectOfType <AI_Grandpa>();
            if (grandpa == null) return;
            var field = typeof(AI_Grandpa).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null) field.SetValue(grandpa, value);
        }
    }
}
