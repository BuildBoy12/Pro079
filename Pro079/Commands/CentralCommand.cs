namespace Pro079.Commands
{
    using API.Interfaces;
    using CommandSystem;
    using Exiled.API.Features;
    using Logic;
    using MEC;
    using System;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class CentralCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get((sender as CommandSender)?.SenderId);
            if (!ply.IsScp079())
            {
                response = Pro079.Singleton.Translations.NotScp079;
                return false;
            }

            response = Methods.GetHelp();
            if (arguments.Count == 0)
                return true;

            ICommand079 command = Methods.GetCommand(arguments.At(0));
            if (command != null)
            {
                if (Pro079.Singleton.Config.EnableCassieCooldown && ply.OnCassieCooldown())
                {
                    response = Pro079.Singleton.Translations.CassieOnCooldown;
                    return false;
                }

                if (ply.OnCommandCooldown(command))
                {
                    response = Pro079.Singleton.Translations.Cooldown;
                    return false;
                }

                if (!ply.IsBypassModeEnabled)
                {
                    if (command.Cost > ply.Energy)
                    {
                        response = Pro079.Singleton.Translations.LowEnergy;
                        return false;
                    }

                    if (command.MinLevel > ply.Level)
                    {
                        response = Pro079.Singleton.Translations.LowLevel;
                        return false;
                    }
                }

                bool success = command.Execute(arguments, sender, out response);
                if (!success)
                    return false;

                Manager.CoroutineHandles.Add(Timing.RunCoroutine(Manager.SetOnCooldown(ply, command)));
                if (Pro079.Singleton.Config.EnableCassieCooldown)
                    Manager.CassieCooldowns[ply] =
                        DateTime.Now + TimeSpan.FromSeconds(Pro079.Singleton.Config.CassieCooldown);
                if (!ply.IsBypassModeEnabled)
                    ply.Energy -= command.Cost;

                return true;
            }

            if (arguments.At(0) == Pro079.Singleton.Translations.UltCmd)
            {
                if (arguments.Count < 2)
                {
                    response = Methods.FormatUltimates();
                    return true;
                }

                IUltimate079 ultimate = Methods.GetUltimate(arguments.At(1));
                if (ultimate == null)
                    return true;

                if (ply.OnUltimateCooldown())
                {
                    response = Pro079.Singleton.Translations.Cooldown;
                    return false;
                }

                if (!ply.IsBypassModeEnabled)
                {
                    if (ply.Energy < ultimate.Cost)
                    {
                        response = Pro079.Singleton.Translations.LowEnergy;
                        return false;
                    }

                    if (ply.Level < Pro079.Singleton.Config.UltimateLevel)
                    {
                        response = Pro079.Singleton.Translations.LowLevel;
                        return false;
                    }
                }

                bool success = ultimate.Execute(arguments, sender, out response);

                if (!success)
                    return false;

                Manager.UltimateCooldowns[ply] = DateTime.Now + TimeSpan.FromSeconds(ultimate.Cooldown);
                if (!ply.IsBypassModeEnabled)
                    ply.Energy -= ultimate.Cost;

                return true;
            }

            return true;
        }

        public string Command => "079";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Base command handler for Pro079";
    }
}