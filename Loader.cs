using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MelonLoader;
using UnityEngine;
using UnityEditor;
using Unity;

namespace LethalCompMod
{
    public class Loader : MelonMod
    {
        public static bool staminaHack = false;
        public static bool ESP = false;
        public static float reachHack = 0.0f;
        public static bool noWeight = false;
        public static bool magnetHack = false;

        public override void OnUpdate()
        {
            Mods.staminaHack();
            Mods.reachHack();
            Mods.noWeight();
        }

        public override void OnGUI()
        {
            /*
            GUI.Box(new Rect(25, 25, 200, 200), "Test Box");

            if (GUI.Button(new Rect(25, 30, 195, 40), "stamina hack"))
            {
                staminaHack = !staminaHack;
            }

            if (GUI.Button(new Rect(27, 80, 195, 40), "ESP"))
            {
                ESP = !ESP;
            }
            */

            GUI.Window(0, new Rect(25, 25, 400, 400), menuFunction, "Mod Window");

            Mods.ESP();
        }

        public void menuFunction(int WindowID)
        {
            var player = GameNetworkManager.Instance.localPlayerController;

            staminaHack = GUI.Toggle(new Rect(27, 30, 195, 40), staminaHack, "Infinite stamina");
            ESP = GUI.Toggle(new Rect(27, 80, 195, 40), ESP, "Wall Hacks");
            GUI.Label(new Rect(27, 130, 195, 20), "Increase Reach");
            reachHack = GUI.HorizontalSlider(new Rect(27, 150, 195, 20), reachHack, 5f, 50f);
            noWeight = GUI.Toggle(new Rect(27, 180, 195, 40), noWeight, "No Weight");

            if (player != null)
            {
                // Y is up and down for position
                Vector3 playerPos = player.transform.position;
                GUI.Label(new Rect(27, 230, 195, 40), "X: " + playerPos.x.ToString() + " Y: " + playerPos.y.ToString() + " Z: " + playerPos.z.ToString());

                // Y is left and right for looking
                Vector3 playerLook = player.transform.eulerAngles;
                GUI.Label(new Rect(27, 280, 195, 40), "x: " + playerLook.x.ToString() + " y: " + playerLook.y.ToString() + " z: " + playerLook.z.ToString());
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }
    }
}
