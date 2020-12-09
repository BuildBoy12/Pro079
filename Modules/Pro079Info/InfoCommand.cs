namespace Pro079Info
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using LightContainmentZoneDecontamination;
    using Pro079.API.Interfaces;
    using System;
    using System.Linq;

    public class InfoCommand : ICommand079
    {
        public InfoCommand()
        {
            _decontaminationEnabled = GameCore.ConfigFile.ServerConfig.GetBool("disable_decontamination");
            _maximumRespawnTime = GameCore.ConfigFile.ServerConfig.GetInt("maximum_MTF_time_to_spawn");
            _minimumRespawnTime = GameCore.ConfigFile.ServerConfig.GetInt("minimum_MTF_time_to_spawn");
        }

        private const float DecontaminationTime = 11.47f;
        private readonly bool _decontaminationEnabled;
        private readonly int _maximumRespawnTime;
        private readonly int _minimumRespawnTime;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get((sender as CommandSender)?.SenderId);
            int level = ply.IsBypassModeEnabled ? 5 : ply.Level;
            string humansAlive;
            string decontaminationTime;
            string escapedClassD;
            string escapedScientists;
            string aliveClassD;
            string aliveScientists;
            string aliveMtf;
            string aliveChaos;
            string estimatedMtfTime;

            #region Decontamination

            if (level < Pro079Info.Singleton.Config.DecontLevel)
            {
                decontaminationTime = $"[{Pro079.Logic.Methods.LevelString(Pro079Info.Singleton.Config.DecontLevel)}]";
            }
            else
            {
                if (!_decontaminationEnabled)
                {
                    decontaminationTime = Pro079Info.Singleton.Translations.DecontDisabled;
                }
                else if (Map.IsLCZDecontaminated)
                {
                    decontaminationTime = Pro079Info.Singleton.Translations.DecontHappened;
                }
                else
                {
                    TimeSpan auxTime = TimeSpan.FromMinutes(DecontaminationTime) -
                                       TimeSpan.FromSeconds(DecontaminationController.GetServerTime);
                    decontaminationTime = auxTime > TimeSpan.FromSeconds(0)
                        ? Stylize(auxTime.ToString())
                        : Pro079Info.Singleton.Translations.DecontBug;
                }
            }

            #endregion

            #region Escapes

            if (level < Pro079Info.Singleton.Config.EscapedLevel)
            {
                escapedClassD = escapedScientists =
                    $"[{Pro079.Logic.Methods.LevelString(Pro079Info.Singleton.Config.EscapedLevel)}]";
            }
            else
            {
                escapedClassD = Stylize(RoundSummary.escaped_ds.ToString("00"));
                escapedScientists = Stylize(RoundSummary.escaped_scientists.ToString("00"));
            }

            #endregion

            #region AliveRoles

            if (level < Pro079Info.Singleton.Config.AliveLevel)
            {
                humansAlive = $"[{Pro079.Logic.Methods.LevelString(Pro079Info.Singleton.Config.AliveLevel)}]";
            }
            else
            {
                humansAlive = (Player.Get(Team.CDP).Count() + Player.Get(Team.RSC).Count() +
                               Player.Get(Team.CHI).Count() + Player.Get(Team.MTF).Count()).ToString();
            }

            if (level < Pro079Info.Singleton.Config.RscCdpLevel)
            {
                aliveClassD = aliveScientists =
                    $"[{Pro079.Logic.Methods.LevelString(Pro079Info.Singleton.Config.RscCdpLevel)}]";
            }
            else
            {
                aliveClassD = Stylize(Player.Get(Team.CDP).Count().ToString("00"));
                aliveScientists = Stylize(Player.Get(Team.RSC).Count().ToString("00"));
            }

            if (level < Pro079Info.Singleton.Config.MtfChiLevel)
            {
                aliveChaos = aliveMtf =
                    $"[{Pro079.Logic.Methods.LevelString(Pro079Info.Singleton.Config.MtfChiLevel)}]";
            }
            else
            {
                aliveChaos = Stylize(Player.Get(Team.CHI).Count().ToString("00"));
                aliveMtf = Stylize(Player.Get(Team.MTF).Count().ToString("00"));
            }

            #endregion

            #region EstimatedMtfTime

            if (level > Pro079Info.Singleton.Config.MtfEstLevel)
            {
                if (Pro079Info.Singleton.Config.MtfOp)
                {
                    var respawnManager = PlayerManager.localPlayer.GetComponent<Respawning.RespawnManager>();
                    if (respawnManager._timeForNextSequence > 0)
                    {
                        estimatedMtfTime = Pro079Info.Singleton.Translations.MtfRespawn.Replace("$time",
                            Pro079Info.Singleton.Config.LongTime
                                ? SecondsToTime(respawnManager._timeForNextSequence)
                                : Stylize(respawnManager._timeForNextSequence.ToString("0")));
                    }
                    else
                    {
                        estimatedMtfTime = Pro079Info.Singleton.Translations.MtfEst2;
                    }
                }
                else
                {
                    if (Round.ElapsedTime.Seconds - _lastMtfSpawn < _minimumRespawnTime)
                    {
                        if (Pro079Info.Singleton.Config.LongTime)
                        {
                            estimatedMtfTime = Pro079Info.Singleton.Translations.MtfEst0
                                .Replace("$(min)",
                                    SecondsToTime(_minimumRespawnTime - Round.ElapsedTime.Seconds + _lastMtfSpawn))
                                .Replace("$(max)",
                                    SecondsToTime(_maximumRespawnTime - Round.ElapsedTime.Seconds + _lastMtfSpawn));
                        }
                        else
                        {
                            estimatedMtfTime = Pro079Info.Singleton.Translations.MtfEst0
                                .Replace("$(min)",
                                    Stylize(_minimumRespawnTime - Round.ElapsedTime.Seconds +
                                            _lastMtfSpawn.ToString("0")))
                                .Replace("$(max)",
                                    (_maximumRespawnTime - Round.ElapsedTime.Seconds + _lastMtfSpawn).ToString("0"));
                        }
                    }
                    else if (Round.ElapsedTime.Seconds - _lastMtfSpawn < _maximumRespawnTime)
                    {
                        estimatedMtfTime = Pro079Info.Singleton.Translations.MtfEst1.Replace("$(max)",
                            Pro079Info.Singleton.Config.LongTime
                                ? SecondsToTime(_maximumRespawnTime - Round.ElapsedTime.Seconds + _lastMtfSpawn)
                                : Stylize(_maximumRespawnTime - Round.ElapsedTime.Seconds +
                                          _lastMtfSpawn.ToString("0")));
                    }
                    else
                    {
                        estimatedMtfTime = Pro079Info.Singleton.Translations.MtfEst2;
                    }
                }
            }
            else
            {
                estimatedMtfTime = $"[{Pro079.Logic.Methods.LevelString(Pro079Info.Singleton.Config.MtfEstLevel)}]";
            }

            #endregion

            string infoMsg = Pro079.Logic.Extensions.ReplaceAfterToken(Pro079Info.Singleton.Translations.InfoMsg, '$',
                new[]
                {
                    new Tuple<string, object>("scpalive", humansAlive),
                    new Tuple<string, object>("humans", humansAlive),
                    new Tuple<string, object>("estMTF", estimatedMtfTime),
                    new Tuple<string, object>("decont", decontaminationTime),
                    new Tuple<string, object>("cdesc", escapedClassD),
                    new Tuple<string, object>("sciesc", escapedScientists),
                    new Tuple<string, object>("cdalive", aliveClassD),
                    new Tuple<string, object>("cialive", aliveChaos),
                    new Tuple<string, object>("scialive", aliveScientists),
                    new Tuple<string, object>("mtfalive", aliveMtf),
                });

            ply.SendConsoleMessage(infoMsg.Replace("\\n", Environment.NewLine), "white");

            #region Generators

            if (level < Pro079Info.Singleton.Config.GenLevel)
            {
                response =
                    $"<color=red>[{Pro079Info.Singleton.Translations.LockedUntil.Replace("$lvl", Stylize(Pro079Info.Singleton.Translations.Generators))}]</color>";
            }
            else
            {
                response = Pro079Info.Singleton.Translations.Generators;
                foreach (Generator079 generator079 in Generator079.Generators)
                {
                    response += Environment.NewLine +
                                Pro079Info.Singleton.Translations.HasTablet.Replace("$room", generator079.CurRoom) +
                                ' ';
                    if (generator079.NetworkremainingPowerup == 0)
                        response += Pro079Info.Singleton.Translations.GeneratorActivated + Environment.NewLine;
                    else
                        response += (generator079.isTabletConnected
                            ? Pro079Info.Singleton.Translations.HasTablet
                            : Pro079Info.Singleton.Translations.NoTablet + ' ' +
                              Pro079Info.Singleton.Translations.TimeLeft.Replace("$sec",
                                  Stylize((int) generator079.remainingPowerup)));
                }
            }

            #endregion

            return true;
        }

        public string Command => Pro079Info.Singleton.Translations.Command;
        public string[] Aliases => Array.Empty<string>();
        public string Description => Pro079Info.Singleton.Translations.Description;

        public string ExtraArguments => string.Empty;
        public bool Cassie => false;
        public int Cooldown => Pro079Info.Singleton.Config.Cooldown;
        public int MinLevel => 1;
        public int Cost => Pro079Info.Singleton.Config.Cost;
        public string CommandReady => Pro079Info.Singleton.Translations.CommandReady;

        private string Stylize(object obj)
            => $"<b><color={Pro079Info.Singleton.Config.Color}>{obj}</color></b>";

        private string SecondsToTime(float sec)
        {
            int seconds = (int) sec % 60;
            int minutes = ((int) sec - seconds) / 60;
            return (minutes > 0
                       ? Stylize(minutes.ToString()) +
                         $" {Pro079Info.Singleton.Translations.Minutes.Replace("$", (minutes != 1 ? Pro079Info.Singleton.Translations.PluralSuffix : string.Empty))}"
                       : string.Empty)
                   + ((seconds > 0 && minutes > 0) ? $" {Pro079Info.Singleton.Translations.And} " : string.Empty) +
                   (seconds != 0
                       ? $"{Stylize(seconds)} <color={Pro079Info.Singleton.Config.Color}>{Pro079Info.Singleton.Translations.Seconds.Replace("$", (seconds != 1 ? Pro079Info.Singleton.Translations.PluralSuffix : string.Empty))}</color>"
                       : string.Empty);
        }

        private int _lastMtfSpawn;
        public void OnRespawningTeam(RespawningTeamEventArgs _) => _lastMtfSpawn = Round.ElapsedTime.Seconds;
    }
}