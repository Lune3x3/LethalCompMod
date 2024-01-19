using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEditor;
using Unity;


namespace LethalCompMod
{
    class Mods
    {
        public static void staminaHack()
        {
            if (Loader.staminaHack)
            {
                GameNetworkManager.Instance.localPlayerController.sprintMeter = 1f;
            }
        }

        public static bool isOnScreen(Vector3 position, Camera camera)
        {
            if (position.x < 0f || position.x > camera.pixelWidth || position.y < 0f || position.y > camera.pixelHeight || position.z < 0f) return false;
            return true;
        }

        public static void ESP()
        {
            if (Loader.ESP)
            {
                var camera = GameNetworkManager.Instance.localPlayerController.gameplayCamera;
                var player = GameNetworkManager.Instance.localPlayerController;

                var ScreenScale = new Vector2((float)Screen.width / camera.pixelWidth, (float)Screen.height / camera.pixelHeight);
                var ScreenCenter = new Vector2((float)(Screen.width / 2), (float)(Screen.height - 1));

                var enemies = GameObject.FindObjectsOfType<EnemyAI>();

                if (enemies == null)
                {
                    return;
                }

                foreach (var enemy in enemies)
                {
                    if (enemy == null || enemy.isEnemyDead)
                    {
                        continue;
                    }

                    Vector3 pivotPos = enemy.transform.position;
                    Vector3 screenPos = camera.WorldToScreenPoint(pivotPos);

                    if (!isOnScreen(screenPos, camera))
                    {
                        continue;
                    }

                    var name = enemy.enemyType;
                    var distance = Vector3.Distance(player.transform.position, enemy.transform.position);

                    Vector2 vec2Pos = new Vector2(screenPos.x * ScreenScale.x, (float)Screen.height - (screenPos.y * ScreenScale.y));

                    Color color = Color.red;
                    string text = "" + name + ": " + distance;

                    Render.DrawString(new Vector2(vec2Pos.x, vec2Pos.y - 20f), text, true);
                    Render.DrawLine(ScreenCenter, vec2Pos, color, 2f);
                }
            }
        }

        public static void reachHack()
        {
            if (Loader.reachHack > 5f)
            {
                GameNetworkManager.Instance.localPlayerController.grabDistance = Loader.reachHack;
            }
        }

        public static void noWeight()
        {
            if (Loader.noWeight)
            {
                GameNetworkManager.Instance.localPlayerController.carryWeight = 1f;
            }
        }

        public static void magnet()
        {
            if (Loader.magnetHack)
            {
                var camera = GameNetworkManager.Instance.localPlayerController.gameplayCamera;
                var player = GameNetworkManager.Instance.localPlayerController;

                var items = GameObject.FindObjectsOfType<GrabbableObject>();

                var ScreenScale = new Vector2((float)Screen.width / camera.pixelWidth, (float)Screen.height / camera.pixelHeight);
                var ScreenCenter = new Vector2((float)(Screen.width / 2), (float)(Screen.height - 1));

                if (items == null || items.Length == 0)
                {
                    return;
                }

                foreach (var item in items)
                {
                    if (item == null || item.isHeld || !item.grabbable || item.isInShipRoom || item.isInElevator || !item.isInFactory)
                    {
                        continue;
                    }

                    Vector3 itemPos = item.transform.position;

                    Vector3 playerLook = camera.transform.forward;
                    Vector3 playerLookOrigin = camera.transform.position;

                    Vector3 itemToPlayerPos = itemPos - playerLookOrigin;
                    //change playerLook to look at item, then use reflection to call BeginGrabObject

                    RaycastHit hit;
                    if (Physics.Raycast(playerLookOrigin, itemToPlayerPos, out hit, player.grabDistance))
                    {
                        if (hit.transform.position == itemPos)
                        {
                            camera.transform.LookAt(itemPos);
                            // use reflection here plz

                            camera.transform.forward = playerLook;
                        }
                    }
                }
            }
        }
    }
}
