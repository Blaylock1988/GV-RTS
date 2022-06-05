using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.IO;
using System.Xml.Serialization;
using VRage.Collections;
using VRage.Game;
using VRage.Utils;

namespace RelativeTopSpeedGV
{
    [ProtoContract]
    public class Settings
    {
        public static Settings Instance;

        public const string Filename = "RelativeTopSpeed.cfg";

        public static readonly Settings Default = new Settings() {
            EnableBoosting = true,
            IgnoreGridsWithoutThrust = true,
            ParachuteDeployHeight = 400,
            SpeedLimit = 160,
            RemoteControlSpeedLimit = 150,
			LargeGrid_MinCruise = 1,
			LargeGrid_MaxCruise = 100,
			LargeGrid_MaxBoostSpeed = 130,
			LargeGrid_ResistanceMultiplier = 1.5f,
			LargeGrid_MinMass = 200000,
			LargeGrid_MaxMass = 8000000,
			SmallGrid_MinCruise = 1,
			SmallGrid_MaxCruise = 150,
			SmallGrid_MaxBoostSpeed = 160,
			SmallGrid_ResistanceMultiplyer = 1f,
			SmallGrid_MinMass = 10000,
			SmallGrid_MaxMass = 400000,
			// Speed = a*Log(mass+c,b)+d
			SmallGrid_Cruise_a = -12.5f,
			SmallGrid_Cruise_b = 5f,
			SmallGrid_Cruise_c = 0,
			SmallGrid_Cruise_d = 204,
			SmallGrid_Boost_a = -16.3f,
			SmallGrid_Boost_b = 6,
			SmallGrid_Boost_c = 0,
			SmallGrid_Boost_d = 233,
			LargeGrid_Cruise_a = -4.3f,
			LargeGrid_Cruise_b = 1.4f,
			LargeGrid_Cruise_c = 614000,
			LargeGrid_Cruise_d = 270,
			LargeGrid_Boost_a = -34f,
			LargeGrid_Boost_b = 17f,
			LargeGrid_Boost_c = 65000,
			LargeGrid_Boost_d = 263,

		};

        [ProtoMember(1)]
        public bool EnableBoosting { get; set; }

        [ProtoMember(2)]
        public bool IgnoreGridsWithoutThrust { get; set; }

        [ProtoMember(3)]
        public float ParachuteDeployHeight { get; set; }

		[ProtoMember(4)]
        public float SpeedLimit { get; set; }

        [ProtoMember(5)]
        public float RemoteControlSpeedLimit { get; set; }

        [ProtoMember(6)]
        public float LargeGrid_MinCruise { get; set; }

        [ProtoMember(7)]
        public float LargeGrid_MaxCruise { get; set; }

        [ProtoMember(8)]
        public float LargeGrid_MaxMass { get; set; }

        [ProtoMember(9)]
        public float LargeGrid_MinMass { get; set; }

        [ProtoMember(10)]
        public float LargeGrid_MaxBoostSpeed { get; set; }

        [ProtoMember(11)]
        public float LargeGrid_ResistanceMultiplier { get; set; }

        [ProtoMember(12)]
        public float SmallGrid_MinCruise { get; set; }

        [ProtoMember(13)]
        public float SmallGrid_MaxCruise { get; set; }

        [ProtoMember(14)]
        public float SmallGrid_MaxMass { get; set; }

        [ProtoMember(15)]
        public float SmallGrid_MinMass { get; set; }

        [ProtoMember(16)]
        public float SmallGrid_MaxBoostSpeed { get; set; }

        [ProtoMember(17)]
        public float SmallGrid_ResistanceMultiplyer { get; set; }
		
        [ProtoMember(18)]
        public float SmallGrid_Cruise_a { get; set; }
		
        [ProtoMember(19)]
        public float SmallGrid_Cruise_b { get; set; }
		
        [ProtoMember(20)]
        public float SmallGrid_Cruise_c { get; set; }
		
        [ProtoMember(21)]
        public float SmallGrid_Cruise_d { get; set; }
		
        [ProtoMember(22)]
        public float SmallGrid_Boost_a { get; set; }
		
        [ProtoMember(23)]
        public float SmallGrid_Boost_b { get; set; }
		
        [ProtoMember(24)]
        public float SmallGrid_Boost_c { get; set; }
		
        [ProtoMember(25)]
        public float SmallGrid_Boost_d { get; set; }
		
        [ProtoMember(26)]
        public float LargeGrid_Cruise_a { get; set; }
		
        [ProtoMember(27)]
        public float LargeGrid_Cruise_b { get; set; }
		
        [ProtoMember(28)]
        public float LargeGrid_Cruise_c { get; set; }
		
        [ProtoMember(29)]
        public float LargeGrid_Cruise_d { get; set; }
		
        [ProtoMember(30)]
        public float LargeGrid_Boost_a { get; set; }
		
        [ProtoMember(31)]
        public float LargeGrid_Boost_b { get; set; }
		
        [ProtoMember(32)]
        public float LargeGrid_Boost_c { get; set; }
		
        [ProtoMember(33)]
        public float LargeGrid_Boost_d { get; set; }

        public void CalculateCurve()
        {

            MyDefinitionManager.Static.EnvironmentDefinition.LargeShipMaxSpeed = SpeedLimit;
            MyDefinitionManager.Static.EnvironmentDefinition.SmallShipMaxSpeed = SpeedLimit;


            // parachute deploy hight code is taken directly from midspaces configurable speed mod. All credit goes to them. 
            DictionaryReader<string, MyDropContainerDefinition> dropContainers = MyDefinitionManager.Static.GetDropContainerDefinitions();
            foreach (var kvp in dropContainers)
            {
                foreach (MyObjectBuilder_CubeGrid grid in kvp.Value.Prefab.CubeGrids)
                {
                    foreach (MyObjectBuilder_CubeBlock block in grid.CubeBlocks)
                    {
                        MyObjectBuilder_Parachute chute = block as MyObjectBuilder_Parachute;
                        if (chute != null)
                        {
                            chute.DeployHeight = ParachuteDeployHeight;
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return MyAPIGateway.Utilities.SerializeToXML(this);
        }

        public static void Validate(ref Settings s)
        {
            if (s.SpeedLimit <= 0)
            {
                s.SpeedLimit = 100;
            }

            if (s.RemoteControlSpeedLimit <= 0)
            {
                s.RemoteControlSpeedLimit = 100;
            }
            else if (s.RemoteControlSpeedLimit > s.SpeedLimit)
            {
                s.RemoteControlSpeedLimit = s.SpeedLimit;
            }

            if (s.ParachuteDeployHeight < 0)
            {
                s.ParachuteDeployHeight = 0;
            }

            #region Large Grid Validation

            if (s.LargeGrid_MinCruise < 0.01f)
            {
                s.LargeGrid_MinCruise = 0.01f;
            }
            else if (s.LargeGrid_MinCruise > s.SpeedLimit)
            {
                s.LargeGrid_MinCruise = s.SpeedLimit;
            }

            if (s.LargeGrid_MaxCruise < s.LargeGrid_MinCruise)
            {
                s.LargeGrid_MaxCruise = s.LargeGrid_MinCruise;
            }
            else if (s.LargeGrid_MaxCruise > s.SpeedLimit)
            {
                s.LargeGrid_MaxCruise = s.SpeedLimit;
            }

            if (s.LargeGrid_MaxBoostSpeed < s.LargeGrid_MaxCruise)
            {
                s.LargeGrid_MaxBoostSpeed = s.LargeGrid_MaxCruise;
            }
            else if (s.LargeGrid_MaxBoostSpeed > s.SpeedLimit)
            {
                s.LargeGrid_MaxBoostSpeed = s.SpeedLimit;
            }

            if (s.LargeGrid_ResistanceMultiplier <= 0)
            {
                s.LargeGrid_ResistanceMultiplier = 1f;
            }

            if (s.LargeGrid_MinMass < 0)
            {
                s.LargeGrid_MinMass = 0;
            }

            if (s.LargeGrid_MaxMass < s.LargeGrid_MinMass)
            {
                s.LargeGrid_MaxMass = s.LargeGrid_MinMass;
            }
            #endregion

            #region Small Grid Validation

            if (s.SmallGrid_MinCruise < 0.01)
            {
                s.SmallGrid_MinCruise = 0.01f;
            }
            else if (s.SmallGrid_MinCruise > s.SpeedLimit)
            {
                s.SmallGrid_MinCruise = s.SpeedLimit;
            }

            if (s.SmallGrid_MaxCruise < s.SmallGrid_MinCruise)
            {
                s.SmallGrid_MaxCruise = s.SmallGrid_MinCruise;
            }
            else if (s.SmallGrid_MaxCruise > s.SpeedLimit)
            {
                s.SmallGrid_MaxCruise = s.SpeedLimit;
            }

            if (s.SmallGrid_MaxBoostSpeed < s.SmallGrid_MaxCruise)
            {
                s.SmallGrid_MaxBoostSpeed = s.SmallGrid_MaxCruise;
            }
            else if (s.SmallGrid_MaxBoostSpeed > s.SpeedLimit)
            {
                s.SmallGrid_MaxBoostSpeed = s.SpeedLimit;
            }

            if (s.SmallGrid_ResistanceMultiplyer <= 0)
            {
                s.SmallGrid_ResistanceMultiplyer = 1f;
            }

            if (s.SmallGrid_MinMass < 0)
            {
                s.SmallGrid_MinMass = 0;
            }

            if (s.LargeGrid_MaxMass < s.SmallGrid_MinMass)
            {
                s.LargeGrid_MaxMass = s.SmallGrid_MinMass;
            }
            #endregion
        }

        public static Settings Load()
        {
			if (!MyAPIGateway.Multiplayer.IsServer)
			{
				return Default;
			}

            Settings s = null;
            try
            {
                if (MyAPIGateway.Utilities.FileExistsInWorldStorage(Filename, typeof(Settings)))
                {
                    MyLog.Default.Info("[RelativeTopSpeed] Loading settings from world storage");
                    TextReader reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(Filename, typeof(Settings));
                    string text = reader.ReadToEnd();
                    reader.Close();

                    s = MyAPIGateway.Utilities.SerializeFromXML<Settings>(text);
                    Validate(ref s);
                    Save(s);
                }
                else
                {
                    MyLog.Default.Info("[RelativeTopSpeed] Config file not found. Loading from local storage");
					if (MyAPIGateway.Utilities.FileExistsInLocalStorage(Filename, typeof(Settings)))
					{
						MyLog.Default.Info("[RelativeTopSpeed] Loading settings from local storage");
						TextReader reader = MyAPIGateway.Utilities.ReadFileInLocalStorage(Filename, typeof(Settings));
						string text = reader.ReadToEnd();
						reader.Close();

						s = MyAPIGateway.Utilities.SerializeFromXML<Settings>(text);
						Validate(ref s);
						Save(s);
					}
					else
					{
						MyLog.Default.Info("[RelativeTopSpeed] Config file not found. Loading defaults");
						s = Default;
						Save(s);
					}
                }
            }
            catch (Exception e)
            {
                MyLog.Default.Warning($"[RelativeTopSpeed] Failed to load saved configuration. Loading defaults\n {e.ToString()}");
                s = Default;
                Save(s);
            }

            MyDefinitionManager.Static.EnvironmentDefinition.LargeShipMaxSpeed = s.SpeedLimit;
            MyDefinitionManager.Static.EnvironmentDefinition.SmallShipMaxSpeed = s.SpeedLimit;
            s.CalculateCurve();
            return s;
        }

        public static void Save(Settings settings)
        {
            if (MyAPIGateway.Session.IsServer)
            {
                try
                {
					MyLog.Default.Info("[RelativeTopSpeed] Saving Settings");
					TextWriter writer;
					if (!MyAPIGateway.Utilities.FileExistsInLocalStorage(Filename, typeof(Settings)))
					{
						writer = MyAPIGateway.Utilities.WriteFileInLocalStorage(Filename, typeof(Settings));
						writer.Write(MyAPIGateway.Utilities.SerializeToXML(settings));
						writer.Close();
					}

					writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(Filename, typeof(Settings));
					writer.Write(MyAPIGateway.Utilities.SerializeToXML(settings));
					writer.Close();

                }
                catch (Exception e)
                {
                    MyLog.Default.Error($"[RelativeTopSpeed] Failed to save settings\n{e.ToString()}");
                }
            }
        }
    }
}
