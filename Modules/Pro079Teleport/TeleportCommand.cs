namespace Pro079Teleport
{
    using CommandSystem;
    using Exiled.API.Features;
    using Pro079.API.Interfaces;
    using Pro079.Logic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class TeleportCommand : ICommand079
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get((sender as CommandSender)?.SenderId);
            Player scpPlayer = Player.Get(Team.SCP).Where(scp => scp.Role != RoleType.Scp079).Shuffle().First();
            if (scpPlayer == null)
            {
                response = "No suitable SCPs found.";
                return false;
            }

            var camera = FindCameras(Map.FindParentRoom(scpPlayer.GameObject).gameObject).First();
            ply.ReferenceHub.scp079PlayerScript.CmdSwitchCamera(camera.cameraId, false);
            response = Pro079.Pro079.Singleton.Translations.Success;
            return true;
        }

        public string Command => Pro079Teleport.Singleton.Translations.Command;
        public string[] Aliases => Array.Empty<string>();
        public string Description => Pro079Teleport.Singleton.Translations.Description;

        public string ExtraArguments => string.Empty;
        public bool Cassie => false;
        public int Cooldown => Pro079Teleport.Singleton.Config.Cooldown;
        public int MinLevel => Pro079Teleport.Singleton.Config.Level;
        public int Cost => Pro079Teleport.Singleton.Config.Cost;
        public string CommandReady => Pro079Teleport.Singleton.Translations.CommandReady;

        private static List<Camera079> FindCameras(GameObject gameObject)
        {
            List<Camera079> cameraList = new List<Camera079>();
            foreach (Scp079Interactable scp079Interactable in Interface079.singleton.allInteractables)
            {
                foreach (Scp079Interactable.ZoneAndRoom zoneAndRoom in scp079Interactable.currentZonesAndRooms)
                {
                    if (zoneAndRoom.currentRoom != gameObject.name ||
                        zoneAndRoom.currentZone != gameObject.transform.parent.name)
                        continue;

                    if (scp079Interactable.type != Scp079Interactable.InteractableType.Camera)
                        continue;

                    Camera079 camera079 = scp079Interactable.GetComponent<Camera079>();
                    if (!cameraList.Contains(camera079))
                    {
                        cameraList.Add(camera079);
                    }
                }
            }

            return cameraList;
        }
    }
}