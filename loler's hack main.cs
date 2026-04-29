using MelonLoader;
using UnityEngine;
using Il2Cpp;

[assembly: MelonInfo(typeof(L0LeRModMenu.ModMenu), "L0LeR Mod Menu", "1.0.0", "L0LeR / MindyLoozy")]
[assembly: MelonGame()]

namespace L0LeRModMenu
{
    public class ModMenu : MelonMod
    {
        private bool _showMenu = false;
        private int  _activeTab = 0;
        private readonly string[] _tabs = { "Baby_AI", "Granny_AI", "Grandpa_AI" };
        private Rect _windowRect = new Rect(30, 30, 520, 700);

        private Vector2 _scrollBaby    = Vector2.zero;
        private Vector2 _scrollGranny  = Vector2.zero;
        private Vector2 _scrollGrandpa = Vector2.zero;

        // ── Baby ──
        private bool _babyCallGranny = false;
        private bool _babyForceChase = false;

        // ── Granny ──
        private bool   _grannyAtDishes   = false;
        private bool   _grannyAtPiano    = false;
        private string _grannyDoorDist   = "3";
        private string _grannyKillDist   = "1.5";
        private string _grannyEyeR       = "1";
        private string _grannyEyeG       = "0";
        private string _grannyEyeB       = "0";
        private bool   _grannyForceChase = false;
        private string _grannyViewRange  = "15";
        private bool   _grannyAngry      = false;
        private bool   _grannyIsChasing  = false;
        private bool   _grannyIsDying    = false;
        private bool   _grannySearching  = false;
        private bool   _grannyPlaceTrap  = false;
        private string _grannyRunAnim    = "1";
        private string _grannyRunSpeed   = "5";
        private string _grannyWalkAnim   = "1";
        private string _grannyWalkSpeed  = "2";

        // ── Grandpa ──
        private bool   _dedAtCams    = false;
        private bool   _dedAtRadio   = false;
        private string _dedKillDist  = "1.5";
        private string _dedDoorDist  = "3";
        private bool   _dedSecRoom   = false;
        private bool   _dedAngry     = false;
        private bool   _dedChasing   = false;
        private bool   _dedDying     = false;
        private bool   _dedPlaceTrap = false;
        private string _dedRunAnim   = "1";
        private string _dedRunSpeed  = "5";
        private bool   _dedTrapPreg  = false;
        private string _dedWalkAnim  = "1";
        private string _dedWalkSpeed = "2";

        // ── Styles ──
        private GUIStyle _titleStyle;
        private GUIStyle _tabActiveStyle;
        private GUIStyle _tabStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _toggleStyle;
        private GUIStyle _buttonStyle;
        private GUIStyle _textFieldStyle;
        private GUIStyle _windowStyle;
        private bool     _stylesReady = false;

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
                _showMenu = !_showMenu;
        }

        public override void OnGUI()
        {
            if (!_showMenu) return;
            InitStyles();
            GUI.backgroundColor = new Color(0.07f, 0.07f, 0.07f, 0.97f);
            _windowRect = GUI.Window(31337, _windowRect, DrawWindow, "", _windowStyle);
        }

        private void DrawWindow(int id)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.Label("<color=#e8e8ff><b>L0LeR</b></color> <color=#ffffff>Mod Menu</color>", _titleStyle);
            GUILayout.Space(6);

            GUILayout.BeginHorizontal();
            for (int i = 0; i < _tabs.Length; i++)
            {
                GUIStyle s = (i == _activeTab) ? _tabActiveStyle : _tabStyle;
                if (GUILayout.Button(_tabs[i], s, GUILayout.Height(28)))
                    _activeTab = i;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(4);

            switch (_activeTab)
            {
                case 0: DrawBabyTab();    break;
                case 1: DrawGrannyTab();  break;
                case 2: DrawGrandpaTab(); break;
            }

            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0, 0, _windowRect.width, 30));
        }

        // ══════════════════════════════════════════════════════════
        //  TAB: Baby_AI
        // ══════════════════════════════════════════════════════════
        private void DrawBabyTab()
        {
            _scrollBaby = GUILayout.BeginScrollView(_scrollBaby, GUILayout.Height(600));
            SectionHeader("── Baby Slendrina AI ──");

            BoolApplyRow("Baby Call Granny", ref _babyCallGranny, () => {
                var b = FindObject<AI_Baby>();
                if (b != null) b.CalledGranny = _babyCallGranny;
            });

            BoolApplyRow("Force Baby Chase Player", ref _babyForceChase, () => {
                var b = FindObject<AI_Baby>();
                if (b != null) b.Chasing = _babyForceChase;
            });

            GUILayout.EndScrollView();
        }

        // ══════════════════════════════════════════════════════════
        //  TAB: Granny_AI
        // ══════════════════════════════════════════════════════════
        private void DrawGrannyTab()
        {
            _scrollGranny = GUILayout.BeginScrollView(_scrollGranny, GUILayout.Height(600));
            SectionHeader("── Granny AI ──");

            BoolApplyRow("Force Granny Be At Dishes", ref _grannyAtDishes, () => {
                var g = FindObject<AI_Granny>();
                if (g != null) g.AtDishes = _grannyAtDishes;
            });

            BoolApplyRow("Force Granny Be At Piano", ref _grannyAtPiano, () => {
                var g = FindObject<AI_Granny>();
                if (g != null) g.AtPiano = _grannyAtPiano;
            });

            SectionHeader("── Distances ──");

            FloatFieldApplyRow("Granny DoorDistance :", ref _grannyDoorDist, () => {
                var g = FindObject<AI_Granny>();
                if (g != null && float.TryParse(_grannyDoorDist, out float v)) g.DoorDistance = v;
            });

            FloatFieldApplyRow("Granny KillDistance :", ref _grannyKillDist, () => {
                var g = FindObject<AI_Granny>();
                if (g != null && float.TryParse(_grannyKillDist, out float v)) g.DistanceJumpscare = v;
            });

            SectionHeader("── Granny Eyes ──");

            GUILayout.Label("Granny Eyes Color  R / G / B :", _labelStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label("R", _labelStyle, GUILayout.Width(14));
            _grannyEyeR = GUILayout.TextField(_grannyEyeR, _textFieldStyle, GUILayout.Width(48));
            GUILayout.Label("G", _labelStyle, GUILayout.Width(14));
            _grannyEyeG = GUILayout.TextField(_grannyEyeG, _textFieldStyle, GUILayout.Width(48));
            GUILayout.Label("B", _labelStyle, GUILayout.Width(14));
            _grannyEyeB = GUILayout.TextField(_grannyEyeB, _textFieldStyle, GUILayout.Width(48));
            if (GUILayout.Button("Apply", _buttonStyle, GUILayout.Width(60)))
            {
                if (float.TryParse(_grannyEyeR, out float r) &&
                    float.TryParse(_grannyEyeG, out float g2) &&
                    float.TryParse(_grannyEyeB, out float b))
                {
                    var eyes = FindObject<Eyes_Granny>();
                    if (eyes != null)
                    {
                        var rend = eyes.GetComponent<Renderer>();
                        if (rend != null) rend.material.color = new Color(r, g2, b);
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);

            BoolApplyRow("Force Granny Chasing Player (Eyes)", ref _grannyForceChase, () => {
                var eyes = FindObject<Eyes_Granny>();
                if (eyes != null) eyes.PlayerSpotted = _grannyForceChase;
            });

            FloatFieldApplyRow("Granny ViewRange :", ref _grannyViewRange, () => {
                var eyes = FindObject<Eyes_Granny>();
                if (eyes != null && float.TryParse(_grannyViewRange, out float v)) eyes.ViewRange = v;
            });

            SectionHeader("── Granny State Flags ──");

            BoolApplyRow("Force Granny Be Angry", ref _grannyAngry, () => {
                var g = FindObject<AI_Granny>(); if (g != null) g.IsAngry = _grannyAngry;
            });

            BoolApplyRow("Force Granny Chasing Player (AI)", ref _grannyIsChasing, () => {
                var g = FindObject<AI_Granny>(); if (g != null) g.IsChasing = _grannyIsChasing;
            });

            BoolApplyRow("Kill Granny", ref _grannyIsDying, () => {
                var g = FindObject<AI_Granny>(); if (g != null) g.IsDying = _grannyIsDying;
            });

            BoolApplyRow("Force Granny Searching", ref _grannySearching, () => {
                var g = FindObject<AI_Granny>(); if (g != null) g.IsSearching = _grannySearching;
            });

            BoolApplyRow("Force Granny Drop BearTrap", ref _grannyPlaceTrap, () => {
                var g = FindObject<AI_Granny>(); if (g != null) g.PlacingTrap = _grannyPlaceTrap;
            });

            SectionHeader("── Granny Speed & Anim ──");

            FloatFieldApplyRow("Granny RunAnim speed :", ref _grannyRunAnim, () => {
                var g = FindObject<AI_Granny>();
                if (g != null && float.TryParse(_grannyRunAnim, out float v)) g.Run_Anim = v;
            });

            FloatFieldApplyRow("Granny RunSpeed :", ref _grannyRunSpeed, () => {
                var g = FindObject<AI_Granny>();
                if (g != null && float.TryParse(_grannyRunSpeed, out float v)) g.Run_Speed = v;
            });

            FloatFieldApplyRow("Granny WalkAnim :", ref _grannyWalkAnim, () => {
                var g = FindObject<AI_Granny>();
                if (g != null && float.TryParse(_grannyWalkAnim, out float v)) g.Walk_Anim = v;
            });

            FloatFieldApplyRow("Granny WalkSpeed :", ref _grannyWalkSpeed, () => {
                var g = FindObject<AI_Granny>();
                if (g != null && float.TryParse(_grannyWalkSpeed, out float v)) g.Walk_Speed = v;
            });

            GUILayout.EndScrollView();
        }

        // ══════════════════════════════════════════════════════════
        //  TAB: Grandpa_AI
        // ══════════════════════════════════════════════════════════
        private void DrawGrandpaTab()
        {
            _scrollGrandpa = GUILayout.BeginScrollView(_scrollGrandpa, GUILayout.Height(600));
            SectionHeader("── Grandpa AI ──");

            BoolApplyRow("Force Ded Be in Cams", ref _dedAtCams, () => {
                var gp = FindObject<AI_Grandpa>(); if (gp != null) gp.AtCams = _dedAtCams;
            });

            BoolApplyRow("Force Ded Dance in Radio", ref _dedAtRadio, () => {
                var gp = FindObject<AI_Grandpa>(); if (gp != null) gp.AtRadio = _dedAtRadio;
            });

            SectionHeader("── Distances ──");

            FloatFieldApplyRow("Ded DistanceJumpscare :", ref _dedKillDist, () => {
                var gp = FindObject<AI_Grandpa>();
                if (gp != null && float.TryParse(_dedKillDist, out float v)) gp.DistanceJumpscare = v;
            });

            FloatFieldApplyRow("Ded DoorDistance :", ref _dedDoorDist, () => {
                var gp = FindObject<AI_Grandpa>();
                if (gp != null && float.TryParse(_dedDoorDist, out float v)) gp.DoorDistance = v;
            });

            SectionHeader("── Grandpa State Flags ──");

            BoolApplyRow("Force Ded be in security", ref _dedSecRoom, () => {
                var gp = FindObject<AI_Grandpa>(); if (gp != null) gp.InSecurityRoom = _dedSecRoom;
            });

            BoolApplyRow("Force Ded be Angry at Player", ref _dedAngry, () => {
                var gp = FindObject<AI_Grandpa>(); if (gp != null) gp.IsAngry = _dedAngry;
            });

            BoolApplyRow("Force Ded Chase Player", ref _dedChasing, () => {
                var gp = FindObject<AI_Grandpa>(); if (gp != null) gp.IsChasing = _dedChasing;
            });

            BoolApplyRow("Kill Ded", ref _dedDying, () => {
                var gp = FindObject<AI_Grandpa>(); if (gp != null) gp.IsDying = _dedDying;
            });

            BoolApplyRow("Force Ded Drop Trap", ref _dedPlaceTrap, () => {
                var gp = FindObject<AI_Grandpa>(); if (gp != null) gp.PlacingTrap = _dedPlaceTrap;
            });

            BoolApplyRow("Force Ded Dropping Traps", ref _dedTrapPreg, () => {
                var gp = FindObject<AI_Grandpa>(); if (gp != null) gp.TrapPregnant = _dedTrapPreg;
            });

            SectionHeader("── Grandpa Speed & Anim ──");

            FloatFieldApplyRow("Ded RunAnim :", ref _dedRunAnim, () => {
                var gp = FindObject<AI_Grandpa>();
                if (gp != null && float.TryParse(_dedRunAnim, out float v)) gp.Run_Anim = v;
            });

            FloatFieldApplyRow("Ded RunSpeed :", ref _dedRunSpeed, () => {
                var gp = FindObject<AI_Grandpa>();
                if (gp != null && float.TryParse(_dedRunSpeed, out float v)) gp.Run_Speed = v;
            });

            FloatFieldApplyRow("Ded WalkAnim :", ref _dedWalkAnim, () => {
                var gp = FindObject<AI_Grandpa>();
                if (gp != null && float.TryParse(_dedWalkAnim, out float v)) gp.Walk_Anim = v;
            });

            FloatFieldApplyRow("Ded WalkSpeed :", ref _dedWalkSpeed, () => {
                var gp = FindObject<AI_Grandpa>();
                if (gp != null && float.TryParse(_dedWalkSpeed, out float v)) gp.Walk_Speed = v;
            });

            GUILayout.EndScrollView();
        }

        // ══════════════════════════════════════════════════════════
        //  Helpers
        // ══════════════════════════════════════════════════════════
        private void BoolApplyRow(string label, ref bool val, System.Action onApply)
        {
            GUILayout.BeginHorizontal();
            val = GUILayout.Toggle(val, "  " + label, _toggleStyle);
            if (GUILayout.Button("Apply", _buttonStyle, GUILayout.Width(60)))
                onApply?.Invoke();
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }

        private void FloatFieldApplyRow(string label, ref string strVal, System.Action onApply)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, _labelStyle, GUILayout.Width(200));
            strVal = GUILayout.TextField(strVal, _textFieldStyle, GUILayout.Width(70));
            if (GUILayout.Button("Apply", _buttonStyle, GUILayout.Width(60)))
                onApply?.Invoke();
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }

        private void SectionHeader(string text)
        {
            GUILayout.Space(4);
            GUILayout.Label("<color=#c8c8ff>" + text + "</color>", _labelStyle);
            GUILayout.Space(2);
        }

        private static T FindObject<T>() where T : UnityEngine.Object
            => UnityEngine.Object.FindObjectOfType<T>();

        // ══════════════════════════════════════════════════════════
        //  Styles init
        // ══════════════════════════════════════════════════════════
        private void InitStyles()
        {
            if (_stylesReady) return;
            _stylesReady = true;

            Texture2D darkTex   = MakeTex(1, 1, new Color(0.08f, 0.08f, 0.10f, 0.97f));
            Texture2D midTex    = MakeTex(1, 1, new Color(0.14f, 0.14f, 0.18f, 1f));
            Texture2D accentTex = MakeTex(1, 1, new Color(0.25f, 0.25f, 0.45f, 1f));
            Texture2D hoverTex  = MakeTex(1, 1, new Color(0.30f, 0.30f, 0.55f, 1f));

            _windowStyle = new GUIStyle(GUI.skin.window);
            _windowStyle.normal.background   = darkTex;
            _windowStyle.onNormal.background = darkTex;
            _windowStyle.border = new RectOffset(4, 4, 4, 4);

            _titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize  = 18,
                alignment = TextAnchor.MiddleCenter,
                richText  = true
            };
            _titleStyle.normal.textColor = Color.white;

            _tabStyle = new GUIStyle(GUI.skin.button) { fontSize = 13, alignment = TextAnchor.MiddleCenter };
            _tabStyle.normal.background = midTex;
            _tabStyle.normal.textColor  = new Color(0.75f, 0.75f, 0.85f);
            _tabStyle.hover.background  = hoverTex;
            _tabStyle.hover.textColor   = Color.white;

            _tabActiveStyle = new GUIStyle(_tabStyle);
            _tabActiveStyle.normal.background = accentTex;
            _tabActiveStyle.normal.textColor  = Color.white;
            _tabActiveStyle.fontStyle = FontStyle.Bold;

            _labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 12, richText = true };
            _labelStyle.normal.textColor = new Color(0.88f, 0.88f, 0.95f);

            _toggleStyle = new GUIStyle(GUI.skin.toggle) { fontSize = 12 };
            _toggleStyle.normal.textColor = new Color(0.88f, 0.88f, 0.95f);
            _toggleStyle.active.textColor = Color.white;

            _buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 11, alignment = TextAnchor.MiddleCenter };
            _buttonStyle.normal.background = accentTex;
            _buttonStyle.normal.textColor  = Color.white;
            _buttonStyle.hover.background  = hoverTex;
            _buttonStyle.hover.textColor   = Color.white;

            _textFieldStyle = new GUIStyle(GUI.skin.textField) { fontSize = 12 };
            _textFieldStyle.normal.background = midTex;
            _textFieldStyle.normal.textColor  = Color.white;
        }

        private static Texture2D MakeTex(int w, int h, Color col)
        {
            var tex = new Texture2D(w, h);
            var pix = new Color[w * h];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;
            tex.SetPixels(pix);
            tex.Apply();
            return tex;
        }
    }
}