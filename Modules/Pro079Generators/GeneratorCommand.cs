namespace Pro079Generators
{
    using CommandSystem;
    using Exiled.API.Features;
    using MEC;
    using Pro079.API.Interfaces;
    using System;
    using System.Collections.Generic;

    public class GeneratorCommand : ICommand079
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get((sender as CommandSender)?.SenderId);
            int blackoutCost = Cost + Pro079Generators.Singleton.Config.CostBlackout;
            if (arguments.Count < 2)
            {
                response = Pro079Generators.Singleton.Translations.Usage;
                return false;
            }

            if (!int.TryParse(arguments.At(1), out int stage) || stage > 6)
            {
                response = Pro079Generators.Singleton.Translations.Usage;
                return false;
            }

            response = Pro079.Pro079.Singleton.Translations.Success;
            switch (stage)
            {
                case 5:
                case 6:
                    if (!ply.IsBypassModeEnabled)
                    {
                        if (ply.Level < Pro079Generators.Singleton.Config.LevelBlackout)
                        {
                            response = Pro079.Logic.Methods.LevelString(Pro079Generators.Singleton.Config
                                .LevelBlackout);
                            return false;
                        }

                        if (ply.Energy < blackoutCost)
                        {
                            response = Pro079.Pro079.Singleton.Translations.LowEnergy;
                            return false;
                        }

                        ply.Energy -= Pro079Generators.Singleton.Config.CostBlackout;
                    }

                    Timing.RunCoroutine(stage == 5 ? Fake5Gens() : SixthGen());
                    Cooldown += Pro079Generators.Singleton.Config.BlackoutPenalty;
                    Timing.RunCoroutine(Pro079.Logic.Manager.SetOnCooldown(ply, this));
                    return true;
                default:
                    Exiled.API.Features.Cassie.Message($"Scp079Recon{stage}");
                    return true;
            }
        }

        public string Command => Pro079Generators.Singleton.Translations.Command;
        public string[] Aliases => Array.Empty<string>();
        public string Description => Pro079Generators.Singleton.Translations.Description;

        public string ExtraArguments => "[1-6]";
        public bool Cassie => true;
        public int Cooldown { get; private set; } = Pro079Generators.Singleton.Config.Cooldown;
        public int MinLevel => Pro079Generators.Singleton.Config.Level;
        public int Cost => Pro079Generators.Singleton.Config.Cost;
        public string CommandReady => Pro079Generators.Singleton.Translations.CommandReady;

        /// <summary>
        /// Fakes a suicide/suicides the given player (6th generator)
        /// </summary>
        private static IEnumerator<float> SixthGen(Player player = null)
        {
            Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP079RECON6", false, true);
            Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP 0 7 9 CONTAINEDSUCCESSFULLY", false, false);

            for (int j = 0; j < 350; j++)
            {
                yield return Timing.WaitForSeconds(0f);
            }

            Generator079.mainGenerator.ServerOvercharge(10f, true);
            foreach (Door door in Map.Doors)
            {
                Scp079Interactable component = door.GetComponent<Scp079Interactable>();
                if (component.currentZonesAndRooms[0].currentZone == "HeavyRooms" && door.isOpen && !door.locked &&
                    !door.destroyed)
                {
                    door.ChangeState(true);
                }
            }

            if (player != null) player.SetRole(RoleType.Spectator);
            Recontainer079.isLocked = true;
            for (int k = 0; k < 500; k++)
            {
                yield return Timing.WaitForSeconds(0f);
            }

            Recontainer079.isLocked = false;
        }

        /// <summary>
        /// Does the whole recontainment process the same way as main game does.
        /// </summary>
        private static IEnumerator<float> Fake5Gens()
        {
            // People complained about it being "easy to be told apart". Not anymore.
            NineTailedFoxAnnouncer annc = NineTailedFoxAnnouncer.singleton;
            while (annc.queue.Count > 0 || AlphaWarheadController.Host.inProgress)
            {
                yield return Timing.WaitForSeconds(0f);
            }

            Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP079RECON5", false, true);
            // This massive for loop jank is what the main game does. Go complain to them.
            for (int i = 0; i < 2750; i++)
            {
                yield return Timing.WaitForSeconds(0f);
            }

            while (annc.queue.Count > 0 || AlphaWarheadController.Host.inProgress)
            {
                yield return Timing.WaitForSeconds(0f);
            }

            Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP079RECON6", false, true);
            Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP 0 7 9 CONTAINEDSUCCESSFULLY", false, false);
            for (int j = 0; j < 350; j++)
            {
                yield return Timing.WaitForSeconds(0f);
            }

            Generator079.mainGenerator.ServerOvercharge(10f, true);
            foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
            {
                Scp079Interactable component = door.GetComponent<Scp079Interactable>();
                if (component.currentZonesAndRooms[0].currentZone == "HeavyRooms" && door.isOpen && !door.locked)
                {
                    door.ChangeState(true);
                }
            }

            Recontainer079.isLocked = true;
            for (int k = 0; k < 500; k++)
            {
                yield return Timing.WaitForSeconds(0f);
            }

            Recontainer079.isLocked = false;
        }
    }
}